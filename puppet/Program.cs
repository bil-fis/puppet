using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puppet
{
    internal static class Program
    {
        // 全局服务器实例（用于在 Form1 中访问）
        public static PupServer? Server { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {
            // 处理命令行参数
            if (args.Length > 0)
            {
                HandleCommandLineArguments(args);
                return;
            }

            // 正常启动 GUI 应用
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        /// <summary>
        /// 处理命令行参数
        /// 参考 Microsoft Learn 文档：https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/program-structure/main-command-line
        /// </summary>
        private static void HandleCommandLineArguments(string[] args)
        {
            // 检查是否是 --create-pup 命令
            if (args[0] == "--create-pup")
            {
                HandleCreatePupCommand(args);
            }
            // 检查是否是 --nake-load 命令
            else if (args[0] == "--nake-load")
            {
                HandleNakeLoadCommand(args);
            }
            else
            {
                Console.WriteLine($"Unknown command: {args[0]}");
                Console.WriteLine("Available commands:");
                Console.WriteLine("  --create-pup -i <input-folder> -o <output.pup> -p <password>");
                Console.WriteLine("  --nake-load <folder>");
            }
        }

        /// <summary>
        /// 处理 --create-pup 命令
        /// </summary>
        private static void HandleCreatePupCommand(string[] args)
        {
            string? inputFolder = null;
            string? outputFile = null;
            string? password = null;

            // 解析参数
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-i":
                        if (i + 1 < args.Length)
                        {
                            inputFolder = args[++i];
                        }
                        break;
                    case "-o":
                        if (i + 1 < args.Length)
                        {
                            outputFile = args[++i];
                        }
                        break;
                    case "-p":
                        if (i + 1 < args.Length)
                        {
                            password = args[++i];
                        }
                        break;
                }
            }

            // 验证参数
            if (string.IsNullOrEmpty(inputFolder))
            {
                Console.WriteLine("Error: Input folder is required. Use -i <input-folder>");
                return;
            }

            if (string.IsNullOrEmpty(outputFile))
            {
                Console.WriteLine("Error: Output file is required. Use -o <output.pup>");
                return;
            }

            // 创建 PUP 文件
            try
            {
                PupCreator.CreatePup(inputFolder, outputFile, password);
                Console.WriteLine("PUP file created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating PUP file: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理 --nake-load 命令
        /// </summary>
        private static void HandleNakeLoadCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Folder path is required.");
                Console.WriteLine("Usage: --nake-load <folder>");
                return;
            }

            string folder = args[1];

            if (!Directory.Exists(folder))
            {
                Console.WriteLine($"Error: Folder not found: {folder}");
                return;
            }

            try
            {
                // 选择可用端口
                int port = PortSelector.GetAvailablePort(7738);
                Console.WriteLine($"Using port: {port}");

                // 创建并启动服务器（裸文件夹模式）
                Server = new PupServer(folder, true, port);
                Console.WriteLine($"Starting PUP Server in nake-load mode...");
                Console.WriteLine($"Serving folder: {folder}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                Console.WriteLine("Press Enter to stop the server...");
                Console.WriteLine();

                // 启动服务器
                Server.StartAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting server: {ex.Message}");
            }
        }

        /// <summary>
        /// 启动 PUP 服务器（从 Form1 调用）
        /// </summary>
        public static async Task<int> StartPupServerAsync()
        {
            try
            {
                // 读取 puppet.ini 配置
                string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.ini");
                IniReader iniReader = new IniReader(iniPath);
                
                string pupFile = iniReader.GetValue("file", "");
                
                if (string.IsNullOrEmpty(pupFile) || !File.Exists(pupFile))
                {
                    Console.WriteLine("No valid PUP file configured in puppet.ini");
                    return 0;
                }

                // 选择可用端口
                int port = PortSelector.GetAvailablePort(7738);
                Console.WriteLine($"Using port: {port}");

                // 创建并启动服务器（PUP 文件模式）
                Server = new PupServer(pupFile, port);
                Console.WriteLine($"Starting PUP Server...");
                Console.WriteLine($"Serving PUP file: {pupFile}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                
                // 在后台启动服务器
                _ = Server.StartAsync();

                return port;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting PUP server: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 启动裸文件夹模式的服务器
        /// </summary>
        public static async Task<int> StartNakeLoadServerAsync(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Console.WriteLine($"Folder not found: {folder}");
                    return 0;
                }

                // 选择可用端口
                int port = PortSelector.GetAvailablePort(7738);
                Console.WriteLine($"Using port: {port}");

                // 创建并启动服务器（裸文件夹模式）
                Server = new PupServer(folder, true, port);
                Console.WriteLine($"Starting PUP Server in nake-load mode...");
                Console.WriteLine($"Serving folder: {folder}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                
                // 在后台启动服务器
                _ = Server.StartAsync();

                return port;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting nake-load server: {ex.Message}");
                return 0;
            }
        }
    }
}