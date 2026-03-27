using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace puppet
{
    /// <summary>
    /// PUP 服务器
    /// 支持两种模式：
    /// 1. PUP 文件模式（标识头 + ZIP）
    /// 2. 裸文件夹模式（直接返回文件夹中的文件）
    /// </summary>
    public class PupServer
    {
        private enum ServerMode
        {
            PupFile,
            Folder
        }

        private readonly ServerMode _mode;
        private readonly string _sourcePath;
        private readonly int _port;
        private HttpListener? _listener;
        private bool _isRunning;
        private ZipArchive? _zipArchive;
        private string? _zipPassword;

        // PUP 文件标识头
        private const string PUP_HEADER = "PUP V1.0";
        private const int HEADER_LENGTH = 8;

        /// <summary>
        /// 构造函数（PUP 文件模式）
        /// </summary>
        public PupServer(string pupFilePath, int port = 7738)
        {
            _mode = ServerMode.PupFile;
            _sourcePath = pupFilePath;
            _port = port;
        }

        /// <summary>
        /// 构造函数（裸文件夹模式）
        /// </summary>
        public PupServer(string folderPath, bool isFolderMode, int port = 7738)
        {
            _mode = ServerMode.Folder;
            _sourcePath = folderPath;
            _port = port;
        }

        /// <summary>
        /// 获取服务器端口号
        /// </summary>
        public int Port => _port;

        /// <summary>
        /// 启动服务器
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                // 根据模式加载资源
                if (_mode == ServerMode.PupFile)
                {
                    if (!LoadPupFile())
                    {
                        throw new Exception("Failed to load PUP file");
                    }
                }
                else
                {
                    if (!Directory.Exists(_sourcePath))
                    {
                        throw new Exception($"Folder not found: {_sourcePath}");
                    }
                }

                // 启动 HTTP 服务器
                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://localhost:{_port}/");
                _listener.Start();
                _isRunning = true;

                Console.WriteLine($"PUP Server started on port {_port}");
                Console.WriteLine($"Mode: {_mode}");
                Console.WriteLine($"Source: {_sourcePath}");

                // 处理请求
                while (_isRunning)
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequest(context));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting server: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
            _listener?.Close();
            _zipArchive?.Dispose();
            Console.WriteLine("PUP Server stopped");
        }

        /// <summary>
        /// 加载 PUP 文件
        /// </summary>
        private bool LoadPupFile()
        {
            try
            {
                if (!File.Exists(_sourcePath))
                {
                    Console.WriteLine($"PUP file not found: {_sourcePath}");
                    return false;
                }

                byte[] fileBytes = File.ReadAllBytes(_sourcePath);

                // 1. 验证标识头
                if (fileBytes.Length < HEADER_LENGTH)
                {
                    Console.WriteLine("Invalid PUP file: too short");
                    return false;
                }

                string header = Encoding.ASCII.GetString(fileBytes, 0, HEADER_LENGTH);
                if (header != PUP_HEADER)
                {
                    Console.WriteLine($"Invalid PUP header: {header}");
                    return false;
                }

                // 2. 提取 ZIP 数据
                byte[] zipData = new byte[fileBytes.Length - HEADER_LENGTH];
                Buffer.BlockCopy(fileBytes, HEADER_LENGTH, zipData, 0, zipData.Length);

                // 3. 解密 ZIP 密码
                using (var zipStream = new MemoryStream(zipData))
                {
                    _zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);
                }

                // 4. 尝试解密密码
                _zipPassword = AesHelper.DecryptPasswordFromZip(_zipArchive);

                Console.WriteLine($"PUP file loaded successfully");
                if (!string.IsNullOrEmpty(_zipPassword))
                {
                    Console.WriteLine($"ZIP password: {_zipPassword}");
                }
                else
                {
                    Console.WriteLine("ZIP is not password protected");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading PUP file: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 处理 HTTP 请求
        /// </summary>
        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // 设置响应头
                response.AddHeader("X-Server", "Mingyao-fumi");

                // 获取请求路径
                string path = request.Url?.AbsolutePath ?? "/";

                // 移除开头的 /
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                // 如果路径为空，返回根目录
                if (string.IsNullOrEmpty(path))
                {
                    await ServeIndex(response);
                    return;
                }

                // 根据模式处理请求
                if (_mode == ServerMode.PupFile)
                {
                    await ServeFromZip(response, path);
                }
                else
                {
                    await ServeFromFolder(response, path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling request: {ex.Message}");
                try
                {
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = 500;
                    response.ContentType = "text/plain; charset=utf-8";
                    string errorContent = $"Internal server error: {ex.Message}";
                    byte[] errorBytes = Encoding.UTF8.GetBytes(errorContent);
                    response.ContentLength64 = errorBytes.Length;
                    await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
                }
                catch
                {
                    // 忽略响应错误
                }
            }
            finally
            {
                context.Response.Close();
            }
        }

        /// <summary>
        /// 从 ZIP 返回文件
        /// </summary>
        private async Task ServeFromZip(HttpListenerResponse response, string path)
        {
            ZipArchiveEntry? entry = _zipArchive?.GetEntry(path);

            if (entry == null)
            {
                response.StatusCode = 404;
                response.ContentType = "text/plain; charset=utf-8";
                string errorContent = $"File not found: {path}";
                byte[] errorBytes = Encoding.UTF8.GetBytes(errorContent);
                response.ContentLength64 = errorBytes.Length;
                await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
                return;
            }

            // 读取文件内容
            using (var entryStream = entry.Open())
            using (var memoryStream = new MemoryStream())
            {
                await entryStream.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                response.StatusCode = 200;
                response.ContentLength64 = fileBytes.Length;
                response.ContentType = GetContentType(path);

                await response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
        }

        /// <summary>
        /// 从文件夹返回文件
        /// </summary>
        private async Task ServeFromFolder(HttpListenerResponse response, string path)
        {
            string filePath = Path.Combine(_sourcePath, path.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                response.ContentType = "text/plain; charset=utf-8";
                string errorContent = $"File not found: {path}";
                byte[] errorBytes = Encoding.UTF8.GetBytes(errorContent);
                response.ContentLength64 = errorBytes.Length;
                await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);

            response.StatusCode = 200;
            response.ContentLength64 = fileBytes.Length;
            response.ContentType = GetContentType(path);

            await response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
        }

        /// <summary>
        /// 返回根目录（文件列表）
        /// </summary>
        private async Task ServeIndex(HttpListenerResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html; charset=utf-8";

            StringBuilder html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='UTF-8'>");
            html.AppendLine("<title>PUP Server</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 40px; }");
            html.AppendLine("h1 { color: #333; }");
            html.AppendLine("ul { list-style-type: none; padding: 0; }");
            html.AppendLine("li { margin: 10px 0; }");
            html.AppendLine("a { color: #0066cc; text-decoration: none; }");
            html.AppendLine("a:hover { text-decoration: underline; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("<h1>PUP Server Files</h1>");
            html.AppendLine("<ul>");

            if (_mode == ServerMode.PupFile && _zipArchive != null)
            {
                foreach (ZipArchiveEntry entry in _zipArchive.Entries)
                {
                    html.AppendLine($"<li><a href=\"/{entry.FullName}\">{entry.FullName}</a> ({entry.Length} bytes)</li>");
                }
            }
            else if (_mode == ServerMode.Folder)
            {
                foreach (string file in Directory.GetFiles(_sourcePath, "*", SearchOption.AllDirectories))
                {
                    string relativePath = Path.GetRelativePath(_sourcePath, file);
                    long fileSize = new FileInfo(file).Length;
                    html.AppendLine($"<li><a href=\"/{relativePath.Replace(Path.DirectorySeparatorChar, '/')}\">{relativePath}</a> ({fileSize} bytes)</li>");
                }
            }

            html.AppendLine("</ul>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            byte[] htmlBytes = Encoding.UTF8.GetBytes(html.ToString());
            response.ContentLength64 = htmlBytes.Length;
            await response.OutputStream.WriteAsync(htmlBytes, 0, htmlBytes.Length);
        }

        /// <summary>
        /// 根据文件扩展名获取 Content-Type
        /// </summary>
        private string GetContentType(string path)
        {
            string extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".html" => "text/html; charset=utf-8",
                ".htm" => "text/html; charset=utf-8",
                ".css" => "text/css; charset=utf-8",
                ".js" => "application/javascript; charset=utf-8",
                ".json" => "application/json; charset=utf-8",
                ".xml" => "application/xml; charset=utf-8",
                ".txt" => "text/plain; charset=utf-8",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".svg" => "image/svg+xml",
                ".ico" => "image/x-icon",
                ".woff" => "font/woff",
                ".woff2" => "font/woff2",
                ".ttf" => "font/ttf",
                ".eot" => "application/vnd.ms-fontobject",
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".ogg" => "audio/ogg",
                _ => "application/octet-stream"
            };
        }
    }
}
