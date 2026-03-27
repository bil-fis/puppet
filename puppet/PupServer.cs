using System;
using System.IO;
using System.Net;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
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
        private ZipFile? _zipFile;
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
            _zipFile?.Close();
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

                // 2. 提取加密密码和 ZIP 数据
                // PUP文件格式：头部(8字节) + 加密密码(变长) + ZIP数据
                // 加密密码使用AES加密，输出长度是固定的32字节
                const int ENCRYPTED_PASSWORD_LENGTH = 32; // AES-256输出32字节

                if (fileBytes.Length < HEADER_LENGTH + ENCRYPTED_PASSWORD_LENGTH)
                {
                    Console.WriteLine("Invalid PUP file: too short to contain password");
                    return false;
                }

                // 提取加密密码
                byte[] encryptedPassword = new byte[ENCRYPTED_PASSWORD_LENGTH];
                Buffer.BlockCopy(fileBytes, HEADER_LENGTH, encryptedPassword, 0, ENCRYPTED_PASSWORD_LENGTH);

                // 提取 ZIP 数据
                int zipDataOffset = HEADER_LENGTH + ENCRYPTED_PASSWORD_LENGTH;
                byte[] zipData = new byte[fileBytes.Length - zipDataOffset];
                Buffer.BlockCopy(fileBytes, zipDataOffset, zipData, 0, zipData.Length);

                // 3. 解密 ZIP 密码
                if (!IsAllZeros(encryptedPassword))
                {
                    Console.WriteLine("PUP file is password protected, decrypting...");
                    try
                    {
                        byte[] passwordBytes = AesHelper.DecryptBytes(encryptedPassword);
                        // 去除空字符，因为加密时填充到了32字节
                        _zipPassword = Encoding.UTF8.GetString(passwordBytes).TrimEnd('\0');
                        Console.WriteLine($"Password decrypted successfully (length: {_zipPassword.Length})");
                        Console.WriteLine($"Password value: {_zipPassword}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to decrypt password: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        // 即使解密失败，也尝试读取ZIP（可能是无密码的）
                        _zipPassword = null;
                    }
                }
                else
                {
                    Console.WriteLine("PUP file is not password protected");
                    _zipPassword = null;
                }

                // 4. 加载 ZIP
                try
                {
                    // 保存 ZIP 数据到临时文件
                    string tempZipFile = Path.Combine(Path.GetTempPath(), $"pup_temp_{Guid.NewGuid()}.zip");
                    File.WriteAllBytes(tempZipFile, zipData);

                    try
                    {
                        // 使用 SharpZipLib 打开 ZIP 文件
                        _zipFile = new ZipFile(tempZipFile);
                        
                        // 设置密码（必须在访问任何条目之前设置）
                        if (!string.IsNullOrEmpty(_zipPassword))
                        {
                            _zipFile.Password = _zipPassword;
                            Console.WriteLine($"ZIP password set: {_zipPassword} (length: {_zipPassword.Length})");
                        }
                        
                        // 显示 ZIP 文件信息
                        Console.WriteLine($"ZIP file loaded successfully");
                        Console.WriteLine($"Total entries: {_zipFile.Count}");
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
                        Console.WriteLine($"Failed to load ZIP: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create temp file: {ex.Message}");
                    return false;
                }
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
                response.AddHeader("Server", "PuppetPussy");

                // 获取请求路径
                string path = request.Url?.AbsolutePath ?? "/";

                // 移除开头的 /
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                // 如果路径为空，检查是否存在index.html
                if (string.IsNullOrEmpty(path))
                {
                    await ServeRoot(response);
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
            try
            {
                ZipEntry? entry = _zipFile?.GetEntry(path);

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

                // 读取文件内容（在后台线程中执行）
                byte[] fileBytes = await Task.Run(() =>
                {
                    using (var entryStream = _zipFile.GetInputStream(entry))
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                });

                response.StatusCode = 200;
                response.ContentLength64 = fileBytes.Length;
                response.ContentType = GetContentType(path);

                await response.OutputStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ContentType = "text/plain; charset=utf-8";
                string errorContent = $"Error reading file: {ex.Message}";
                byte[] errorBytes = Encoding.UTF8.GetBytes(errorContent);
                response.ContentLength64 = errorBytes.Length;
                await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
                
                Console.WriteLine($"Error serving file {path}: {ex.Message}");
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
        /// 处理根路径请求（优先返回index.html，否则返回文件列表）
        /// </summary>
        private async Task ServeRoot(HttpListenerResponse response)
        {
            // 检查是否存在index.html
            bool hasIndexHtml = false;
            string indexPath = "";

            if (_mode == ServerMode.PupFile && _zipFile != null)
            {
                // 检查ZIP中是否有index.html
                var indexEntry = _zipFile.GetEntry("index.html");
                if (indexEntry != null)
                {
                    hasIndexHtml = true;
                }
            }
            else if (_mode == ServerMode.Folder)
            {
                // 检查文件夹中是否有index.html
                indexPath = Path.Combine(_sourcePath, "index.html");
                hasIndexHtml = File.Exists(indexPath);
            }
            
            // 如果存在index.html，返回index.html内容
            if (hasIndexHtml)
            {
                Console.WriteLine("Serving index.html");
                if (_mode == ServerMode.PupFile)
                {
                    await ServeFromZip(response, "index.html");
                }
                else
                {
                    await ServeFromFolder(response, "index.html");
                }
            }
            else
            {
                // 不存在index.html，返回文件列表
                Console.WriteLine("No index.html found, serving file list");
                await ServeIndex(response);
            }
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

            if (_mode == ServerMode.PupFile && _zipFile != null)
            {
                foreach (ZipEntry entry in _zipFile)
                {
                    html.AppendLine($"<li><a href=\"/{entry.Name}\">{entry.Name}</a> ({entry.CompressedSize} bytes)</li>");
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
        /// 检查字节数组是否全部为零
        /// </summary>
        private static bool IsAllZeros(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                if (b != 0)
                {
                    return false;
                }
            }
            return true;
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
