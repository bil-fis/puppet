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
                        // 检查是否在系统目录下
                        string fullPath = Path.GetFullPath(command);
                        bool isSystemPath = false;

                        // 检查所有驱动器的 Windows 文件夹
                        var drives = DriveInfo.GetDrives();
                        foreach (var drive in drives)
                        {
                            if (drive.DriveType == DriveType.Fixed)
                            {
                                string windowsPath = Path.Combine(drive.RootDirectory.FullName, "Windows");
                                string system32Path = Path.Combine(windowsPath, "System32");
                                string sysWOW64Path = Path.Combine(windowsPath, "SysWOW64");

                                if (fullPath.StartsWith(windowsPath, StringComparison.OrdinalIgnoreCase) ||
                                    fullPath.StartsWith(system32Path, StringComparison.OrdinalIgnoreCase) ||
                                    fullPath.StartsWith(sysWOW64Path, StringComparison.OrdinalIgnoreCase))
                                {
                                    isSystemPath = true;
                                    break;
                                }
                            }
                        }

                        // 如果是系统目录下的可执行文件，弹窗确认
                        if (isSystemPath)
                        {
                            DialogResult result = PermissionDialog.ShowPermissionDialog(
                                "安全警告",
                                "您正在尝试执行系统目录下的可执行文件，这可能影响系统安全。",
                                fullPath
                            );

                            if (result == DialogResult.Abort)
                            {
                                // 永久阻止
                                return $"Error: 用户已永久阻止执行此文件";
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                // 拒绝
                                return $"Error: 用户取消了操作";
                            }
                            // DialogResult.OK - 允许，继续执行
                        }

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
        /// 修改配置文件 puppet.ini（全局配置）
        /// </summary>
        public void SetConfig(string key, string value)
        {
            SetConfig("file", key, value);
        }

        /// <summary>
        /// 修改配置文件 puppet.ini（指定节）
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void SetConfig(string section, string key, string value)
        {
            // 检查是否在UI线程
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() => SetConfig(section, key, value)));
                return;
            }

            // 显示确认对话框
            DialogResult result = MessageBox.Show(
                _form,
                $"您确定要修改配置文件吗？\n\n节: {section}\n键: {key}\n值: {value}\n\n此操作将修改 puppet.ini 文件，可能影响框架的启动行为。",
                "确认修改配置",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (result == DialogResult.Cancel)
            {
                Console.WriteLine($"用户取消了配置修改: [{section}] {key} = {value}");
                return;
            }

            try
            {
                string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.ini");
                var iniReader = new IniReader(iniPath);

                // 读取现有配置（如果需要）
                string oldValue = iniReader.GetValue(section, key, "");

                // 更新配置
                iniReader.SetValue(section, key, value);

                // 保存配置
                iniReader.Save();

                Console.WriteLine($"配置已更新: [{section}] {key} = {value} (原值: {oldValue})");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _form,
                    $"修改配置文件失败: {ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine($"修改配置失败: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 读取配置文件 puppet.ini（全局配置）
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>配置值，如果不存在返回空字符串</returns>
        public string GetConfig(string key)
        {
            return GetConfig("file", key);
        }

        /// <summary>
        /// 读取配置文件 puppet.ini（指定节）
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <returns>配置值，如果不存在返回空字符串</returns>
        public string GetConfig(string section, string key)
        {
            try
            {
                string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.ini");
                var iniReader = new IniReader(iniPath);

                string value = iniReader.GetValue(section, key, "");
                Console.WriteLine($"配置已读取: [{section}] {key} = {value}");
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取配置失败: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return "";
            }
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