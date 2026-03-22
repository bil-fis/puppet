using System.Diagnostics;
using System.Runtime.InteropServices;
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
        /// 执行外部程序
        /// </summary>
        public string Execute(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (var process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        return $"Error: {error}";
                    }
                    return output;
                }
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