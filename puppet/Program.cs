using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using puppet.PUP;
using puppet.Core;

namespace puppet
{
    internal static class Program
    {
        // 使用 ServiceManager 管理服务器实例（替代全局静态属性）
        // 参考 Microsoft Learn 依赖注入指南：https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines

        [STAThread]
        static void Main(string[] args)
        {
            // 初始化安全密钥（必须在处理任何其他逻辑之前）
            SecretKey.Initialize();

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
            // 检查是否是 --load-pup 命令
            else if (args[0] == "--load-pup")
            {
                HandleLoadPupCommand(args);
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
                Console.WriteLine("  --load-pup <pup-file>");
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
            string? version = "1.0";
            string? scriptFile = null;
            string? certificate = null;
            string? privateKey = null;
            string? privateKeyPassword = null;

            Console.WriteLine("=== PUP 文件创建工具 ===");
            Console.WriteLine();
            Console.WriteLine("用法: puppet.exe --create-pup -i <input-folder> -o <output.pup> [-p <password>] [-v <version>] [--script <script-file>] [--certificate <file>] [--private-key <file>] [--private-key-password <password>]");
            Console.WriteLine();

            if (args.Length < 2)
            {
                Console.WriteLine("错误: 缺少必需参数");
                Console.WriteLine();
                Console.WriteLine("参数说明:");
                Console.WriteLine("  -i <input-folder>        必需，源文件夹路径");
                Console.WriteLine("  -o <output.pup>           必需，输出PUP文件路径");
                Console.WriteLine("  -p <password>            可选，ZIP密码");
                Console.WriteLine("  -v <version>             可选，PUP版本（1.0、1.1 或 1.2），默认为 1.0");
                Console.WriteLine("  --script <file>          可选，V1.1/V1.2版本的启动脚本文件");
                Console.WriteLine("  --certificate <file>     可选，证书文件（V1.2版本必需）");
                Console.WriteLine("  --private-key <file>     可选，私钥文件（V1.2版本必需）");
                Console.WriteLine("  --private-key-password   可选，私钥加密密码（V1.2版本必需）");
                Console.WriteLine();
                Console.WriteLine("示例:");
                Console.WriteLine("  puppet.exe --create-pup -i \"C:\\MyProject\" -o \"C:\\Output\\myapp.pup\"");
                Console.WriteLine("  puppet.exe --create-pup -i \"C:\\MyProject\" -o \"C:\\Output\\myapp.pup\" -p \"mypassword\"");
                Console.WriteLine("  puppet.exe --create-pup -i \"C:\\MyProject\" -o \"C:\\Output\\myapp.pup\" -v 1.1 --script \"C:\\script.txt\"");
                Console.WriteLine("  puppet.exe --create-pup -i \"C:\\MyProject\" -o \"C:\\Output\\myapp.pup\" -v 1.2 --certificate \"C:\\app.crt\" --private-key \"C:\\app.key\" --private-key-password \"keypassword\"");
                Console.WriteLine();
                return;
            }

            // 解析参数
            try
            {
                for (int i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-i":
                            if (i + 1 < args.Length)
                            {
                                inputFolder = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: -i 参数缺少值");
                                return;
                            }
                            break;
                        case "-o":
                            if (i + 1 < args.Length)
                            {
                                outputFile = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: -o 参数缺少值");
                                return;
                            }
                            break;
                        case "-p":
                            if (i + 1 < args.Length)
                            {
                                password = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: -p 参数缺少值");
                                return;
                            }
                            break;
                        case "-v":
                        case "--version":
                            if (i + 1 < args.Length)
                            {
                                version = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: -v/--version 参数缺少值");
                                return;
                            }
                            break;
                        case "--script":
                            if (i + 1 < args.Length)
                            {
                                scriptFile = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: --script 参数缺少值");
                                return;
                            }
                            break;
                        case "--certificate":
                            if (i + 1 < args.Length)
                            {
                                certificate = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: --certificate 参数缺少值");
                                return;
                            }
                            break;
                        case "--private-key":
                            if (i + 1 < args.Length)
                            {
                                privateKey = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: --private-key 参数缺少值");
                                return;
                            }
                            break;
                        case "--private-key-password":
                            if (i + 1 < args.Length)
                            {
                                privateKeyPassword = args[++i];
                            }
                            else
                            {
                                Console.WriteLine("错误: --private-key-password 参数缺少值");
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine($"错误: 未知参数 {args[i]}");
                            Console.WriteLine("使用 --create-pup -h 查看帮助信息");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: 参数解析失败 - {ex.Message}");
                return;
            }

            // 验证参数
            Console.WriteLine("验证参数...");
            Console.WriteLine();
            
            if (string.IsNullOrEmpty(inputFolder))
            {
                Console.WriteLine("错误: 必需参数 -i <input-folder> 未提供");
                Console.WriteLine();
                Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup>");
                return;
            }

            if (string.IsNullOrEmpty(outputFile))
            {
                Console.WriteLine("错误: 必需参数 -o <output.pup> 未提供");
                Console.WriteLine();
                Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup>");
                return;
            }

            // 验证版本参数
            if (version != "1.0" && version != "1.1" && version != "1.2")
            {
                Console.WriteLine("错误: 无效的版本号。支持的版本: 1.0, 1.1, 1.2");
                return;
            }

            // 如果是V1.1版本，必须提供脚本文件
            if (version == "1.1" && string.IsNullOrEmpty(scriptFile))
            {
                Console.WriteLine("错误: V1.1版本需要提供 --script 参数");
                Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup> -v 1.1 --script <script-file>");
                return;
            }

            // 如果是V1.2版本，必须提供证书和私钥
            if (version == "1.2")
            {
                if (string.IsNullOrEmpty(certificate))
                {
                    Console.WriteLine("错误: V1.2版本需要提供 --certificate 参数");
                    Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup> -v 1.2 --certificate <cert> --private-key <key> --private-key-password <password>");
                    return;
                }
                if (string.IsNullOrEmpty(privateKey))
                {
                    Console.WriteLine("错误: V1.2版本需要提供 --private-key 参数");
                    Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup> -v 1.2 --certificate <cert> --private-key <key> --private-key-password <password>");
                    return;
                }
                if (string.IsNullOrEmpty(privateKeyPassword))
                {
                    Console.WriteLine("错误: V1.2版本需要提供 --private-key-password 参数");
                    Console.WriteLine("提示: 使用 puppet.exe --create-pup -i <input-folder> -o <output.pup> -v 1.2 --certificate <cert> --private-key <key> --private-key-password <password>");
                    return;
                }
            }

            // 验证脚本文件
            if (!string.IsNullOrEmpty(scriptFile))
            {
                if (!File.Exists(scriptFile))
                {
                    Console.WriteLine($"错误: 脚本文件不存在: {scriptFile}");
                    return;
                }
                Console.WriteLine($"  ✓ 脚本文件存在: {scriptFile}");
            }

            // 验证输入文件夹
            Console.WriteLine($"检查源文件夹: {inputFolder}");
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine($"错误: 源文件夹不存在: {inputFolder}");
                Console.WriteLine();
                Console.WriteLine("提示:");
                Console.WriteLine("  1. 检查路径是否正确");
                Console.WriteLine("  2. 确保路径使用正确的斜杠方向");
                Console.WriteLine("  3. 确保你有访问该文件夹的权限");
                return;
            }
            Console.WriteLine($"  ✓ 源文件夹存在");
            
            // 检查源文件夹是否为空
            var files = Directory.GetFiles(inputFolder, "*", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Console.WriteLine($"警告: 源文件夹为空: {inputFolder}");
                Console.WriteLine("  建议确认文件夹包含要打包的文件");
            }
            else
            {
                Console.WriteLine($"  包含 {files.Length} 个文件");
            }
            Console.WriteLine();

            // 验证输出文件扩展名
            if (!outputFile.EndsWith(".pup", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"警告: 输出文件扩展名不是 .pup: {outputFile}");
                Console.WriteLine("  建议使用 .pup 扩展名");
            }
            Console.WriteLine($"输出文件: {outputFile}");
            Console.WriteLine($"PUP版本: {version}");
            Console.WriteLine();

            // 检查密码
            if (!string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"ZIP密码: {password} ({password.Length} 字符)");
                Console.WriteLine("  注意: 密码将在ZIP中被加密");
            }
            Console.WriteLine();

            // 创建 PUP 文件
            try
            {
                Console.WriteLine("开始创建 PUP 文件...");
                Console.WriteLine();
                
                PupCreator.CreatePup(inputFolder, outputFile, password, version, scriptFile, certificate, privateKey, privateKeyPassword);
                
                Console.WriteLine();
                Console.WriteLine("✓ PUP 文件创建成功!");
                Console.WriteLine($"  文件位置: {outputFile}");
                Console.WriteLine($"  PUP版本: {version}");
                
                if (version == "1.2" && !string.IsNullOrEmpty(certificate))
                {
                    Console.WriteLine($"  包含证书: {certificate}");
                    if (!string.IsNullOrEmpty(privateKey))
                    {
                        Console.WriteLine($"  包含私钥: {privateKey}");
                    }
                }
                
                // 验证文件是否真的创建了
                if (File.Exists(outputFile))
                {
                    var fileInfo = new FileInfo(outputFile);
                    Console.WriteLine($"  文件大小: {FormatBytes(fileInfo.Length)}");
                }
                else
                {
                    Console.WriteLine("  ⚠ 警告: 文件验证失败，请检查文件是否在预期位置");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("✗ PUP 文件创建失败");
                Console.WriteLine();
                Console.WriteLine("错误详情:");
                Console.WriteLine($"  类型: {ex.GetType().Name}");
                Console.WriteLine($"  消息: {ex.Message}");
                Console.WriteLine();
                
                // 提供针对性的帮助
                if (ex is DirectoryNotFoundException)
                {
                    Console.WriteLine("常见原因:");
                    Console.WriteLine("   • 路径拼写错误");
                    Console.WriteLine("  • 文件夹被删除或移动");
                    Console.WriteLine("  • 网络路径访问失败");
                }
                else if (ex is UnauthorizedAccessException)
                {
                    Console.WriteLine("常见原因:");
                    Console.WriteLine("  • 没有读取源文件夹的权限");
                    Console.WriteLine("  • 没有写入输出目录的权限");
                    Console.WriteLine("  • U盘或网络驱动器访问限制");
                    Console.WriteLine();
                    Console.WriteLine("解决方法:");
                    Console.WriteLine("  • 尝试以管理员身份运行");
                    Console.WriteLine("  • 检查文件夹权限设置");
                    Console.WriteLine("  • 更换到本地磁盘操作");
                }
                else if (ex is IOException)
                {
                    Console.WriteLine("常见原因:");
                    Console.WriteLine("  • 路径格式不正确");
                    Console.WriteLine("  • 文件名包含非法字符");
                    Console.WriteLine("  • 磁盘空间不足");
                    Console.WriteLine("  • 文件正在被其他程序使用");
                }
                else if (ex is Exception)
                {
                    Console.WriteLine("如果问题持续存在，请:");
                    Console.WriteLine("   • 检查是否有其他程序占用文件");
                    Console.WriteLine("  • 确保有足够的磁盘空间");
                    Console.WriteLine("  • 尝试使用不同的输出文件名");
                }
            }
        }
        
        /// <summary>
        /// 格式化字节大小
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// 处理 --load-pup 命令
        /// </summary>
        private static void HandleLoadPupCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: PUP file path is required.");
                Console.WriteLine("Usage: --load-pup <pup-file>");
                return;
            }

            string pupFile = args[1];

            if (!File.Exists(pupFile))
            {
                Console.WriteLine($"Error: PUP file not found: {pupFile}");
                return;
            }

            try
            {
                // 选择可用端口
                int port = PortSelector.GetAvailablePort(7738);
                Console.WriteLine($"Using port: {port}");

                // 创建服务器（PUP 文件模式）
                var server = new PupServer(pupFile, port);
                
                // 使用 ServiceManager 注册服务器实例
                ServiceManager.Instance.Register<IServer>(server);
                
                Console.WriteLine($"Starting PUP Server...");
                Console.WriteLine($"Serving PUP file: {pupFile}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                Console.WriteLine();

                // 在后台启动服务器（不阻塞主线程）
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await server.StartAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Server error: {ex.Message}");
                    }
                });

                // 启动 GUI 应用（与服务器并行运行）
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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

                // 创建服务器（裸文件夹模式）
                var server = new PupServer(folder, true, port);
                
                // 使用 ServiceManager 注册服务器实例
                ServiceManager.Instance.Register<IServer>(server);
                
                Console.WriteLine($"Starting PUP Server in nake-load mode...");
                Console.WriteLine($"Serving folder: {folder}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                Console.WriteLine();

                // 在后台启动服务器（不阻塞主线程）
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await server.StartAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Server error: {ex.Message}");
                    }
                });

                // 启动 GUI 应用（与服务器并行运行）
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
                var server = new PupServer(pupFile, port);
                
                // 使用 ServiceManager 注册服务器实例
                ServiceManager.Instance.Register<IServer>(server);
                
                Console.WriteLine($"Starting PUP Server...");
                Console.WriteLine($"Serving PUP file: {pupFile}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                
                // 在后台启动服务器
                _ = server.StartAsync();

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
                var server = new PupServer(folder, true, port);
                
                // 使用 ServiceManager 注册服务器实例
                ServiceManager.Instance.Register<IServer>(server);
                
                Console.WriteLine($"Starting PUP Server in nake-load mode...");
                Console.WriteLine($"Serving folder: {folder}");
                Console.WriteLine($"Server URL: http://localhost:{port}/");
                
                // 在后台启动服务器
                _ = server.StartAsync();

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
