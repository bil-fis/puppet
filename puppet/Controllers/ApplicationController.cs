using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace puppet.Controllers
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ApplicationController
    {
        private Form _form;
        private string _configFilePath;

        public ApplicationController(Form form)
        {
            _form = form;
            _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.json");
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        public void Close()
        {
            _form.Close();
        }

        /// <summary>
        /// 重启程序
        /// </summary>
        public void Restart()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath,
                UseShellExecute = true
            };
            Process.Start(startInfo);
            _form.Close();
        }

        /// <summary>
        /// 获取当前窗口句柄或调试信息
        /// </summary>
        public string GetWindowInfo()
        {
            var info = new
            {
                Handle = _form.Handle.ToString(),
                Text = _form.Text,
                Location = new { X = _form.Location.X, Y = _form.Location.Y },
                Size = new { Width = _form.Width, Height = _form.Height },
                Opacity = _form.Opacity,
                TopMost = _form.TopMost,
                WindowState = _form.WindowState.ToString()
            };
            return JsonConvert.SerializeObject(info, Formatting.Indented);
        }

        /// <summary>
        /// 执行外部程序、打开网址或 URI
        /// 根据 Microsoft Learn 官方文档实现
        /// 参考：https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/start-internet-browser
        /// </summary>
        public string Execute(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return "Error: Command cannot be empty";
            }

            try
            {
                command = command.Trim();

                // 检查是否是 URL（http://, https://）
                if (command.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    command.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    // 使用默认浏览器打开网址
                    var urlProcess = Process.Start(new ProcessStartInfo
                    {
                        FileName = command,
                        UseShellExecute = true
                    });
                    if (urlProcess == null)
                    {
                        return $"Error: Failed to open URL {command}";
                    }
                    return $"Success: Opened URL {command}";
                }

                // 检查是否是 URI（mailto:, tel:, ftp://, file:// 等）
                if (command.Contains("://") && 
                    (command.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
                     command.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) ||
                     command.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ||
                     command.StartsWith("file://", StringComparison.OrdinalIgnoreCase)))
                {
                    // 使用系统关联程序打开 URI
                    var uriProcess = Process.Start(new ProcessStartInfo
                    {
                        FileName = command,
                        UseShellExecute = true
                    });
                    if (uriProcess == null)
                    {
                        return $"Error: Failed to open URI {command}";
                    }
                    return $"Success: Opened URI {command}";
                }

                // 检查是否是本地文件路径
                if (File.Exists(command))
                {
                    // 检查是否是可执行文件
                    string extension = Path.GetExtension(command).ToLowerInvariant();
                    if (extension == ".exe" || extension == ".bat" || extension == ".cmd" || extension == ".msi")
                    {
                        // 启动可执行文件
                        var exeProcess = Process.Start(new ProcessStartInfo
                        {
                            FileName = command,
                            UseShellExecute = true
                        });
                        if (exeProcess == null)
                        {
                            return $"Error: Failed to start executable {command}";
                        }
                        return $"Success: Started executable {command}";
                    }
                    else
                    {
                        // 使用默认程序打开文件
                        var fileProcess = Process.Start(new ProcessStartInfo
                        {
                            FileName = command,
                            UseShellExecute = true
                        });
                        if (fileProcess == null)
                        {
                            return $"Error: Failed to open file {command}";
                        }
                        return $"Success: Opened file {command}";
                    }
                }

                // 检查是否是文件夹
                if (Directory.Exists(command))
                {
                    // 打开文件夹
                    var folderProcess = Process.Start(new ProcessStartInfo
                    {
                        FileName = command,
                        UseShellExecute = true
                    });
                    if (folderProcess == null)
                    {
                        return $"Error: Failed to open folder {command}";
                    }
                    return $"Success: Opened folder {command}";
                }

                // 如果都不是，尝试作为命令执行（通过 cmd.exe）
                // 这样可以支持命令行指令，如 "dir"、"ping" 等
                
                // 特殊处理交互式命令（如 cmd、powershell），添加退出命令
                string cmdArguments = $"/c {command}";
                string lowerCommand = command.ToLowerInvariant();
                if (lowerCommand == "cmd" || lowerCommand == "cmd.exe")
                {
                    cmdArguments = "/c cmd /c exit";  // 立即退出
                }
                else if (lowerCommand == "powershell" || lowerCommand == "pwsh")
                {
                    cmdArguments = "/c powershell -Command exit";  // 立即退出
                }
                else if (lowerCommand == "bash")
                {
                    cmdArguments = "/c bash -c exit";  // 立即退出
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = cmdArguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    // 使用控制台默认编码，避免 UTF8 在 Windows 上导致乱码
                    StandardOutputEncoding = Console.OutputEncoding,
                    StandardErrorEncoding = Console.OutputEncoding
                };

                Process? process = null;
                try
                {
                    process = Process.Start(startInfo);
                }
                catch (Win32Exception)
                {
                    // 如果 cmd.exe 执行失败，尝试直接执行命令
                    return TryExecuteDirect(command);
                }
                
                if (process == null)
                {
                    return TryExecuteDirect(command);
                }

                // 使用异步读取避免死锁
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();
                
                // 等待进程完成（最多等待 30 秒）
                if (process.WaitForExit(30000))
                {
                    string output = outputTask.Result;
                    string error = errorTask.Result;

                    if (!string.IsNullOrEmpty(error))
                    {
                        return $"Error: {error}";
                    }
                    return output ?? string.Empty;
                }
                else
                {
                    // 超时，强制结束进程
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                    return $"Error: Command timeout (30s): {command}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        /// <summary>
        /// 尝试直接执行命令（不通过 cmd.exe）
        /// </summary>
        private string TryExecuteDirect(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    UseShellExecute = true
                };

                var process = Process.Start(startInfo);
                if (process == null)
                {
                    return $"Error: Failed to execute: {command}";
                }
                return $"Success: Executed {command}";
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }

        /// <summary>
        /// 修改配置文件 puppet.json
        /// </summary>
        public void SetConfig(string key, string value)
        {
            Dictionary<string, object>? config = new Dictionary<string, object>();

            // 读取现有配置
            if (File.Exists(_configFilePath))
            {
                string json = File.ReadAllText(_configFilePath);
                config = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                config ??= new Dictionary<string, object>();
            }

            // 更新配置
            config[key] = value;

            // 写入配置文件
            string updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(_configFilePath, updatedJson);
        }

        /// <summary>
        /// 获取程序自身目录
        /// </summary>
        public string GetAssemblyDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 获取Appdata目录
        /// </summary>
        public string GetAppDataDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        public string GetCurrentUser()
        {
            return Environment.UserName;
        }
    }
}