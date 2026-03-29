using System.Runtime.InteropServices;
using System.Text;

namespace puppet.Controllers
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class LogController
    {
        private string _logFilePath;
        private readonly object _lockObject = new object();

        public LogController()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.log");
        }

        /// <summary>
        /// 检查路径是否在敏感文件夹中
        /// </summary>
        private bool IsSensitiveFolder(string path)
        {
            try
            {
                string fullPath = Path.GetFullPath(path);
                string directoryPath = Path.GetDirectoryName(fullPath) ?? fullPath;

                // Windows系统文件夹
                string windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                string programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                string programFilesX86Folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                string systemFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
                string systemX86Folder = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);

                // 检查是否在敏感文件夹中
                if (directoryPath.StartsWith(windowsFolder, StringComparison.OrdinalIgnoreCase) ||
                    directoryPath.StartsWith(programFilesFolder, StringComparison.OrdinalIgnoreCase) ||
                    directoryPath.StartsWith(programFilesX86Folder, StringComparison.OrdinalIgnoreCase) ||
                    directoryPath.StartsWith(systemFolder, StringComparison.OrdinalIgnoreCase) ||
                    directoryPath.StartsWith(systemX86Folder, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置日志文件路径
        /// 支持格式化字符串，例如：
        /// - "Puppet_log-{YY-MM-DD HH:MM:SS - %i}.log"
        /// - "logs/app-{yyyy-MM-dd}.log"
        /// 支持的格式占位符：
        /// - {yyyy}: 4位年份
        /// - {yy}: 2位年份
        /// - {MM}: 月份（01-12）
        /// - {dd}: 日期（01-31）
        /// - {HH}: 小时（00-23）
        /// - {mm}: 分钟（00-59）
        /// - {ss}: 秒（00-59）
        /// - {fff}: 毫秒（000-999）
        /// - {%i}: 自增序号（从1开始）
        /// </summary>
        public void SetFile(string pathPattern)
        {
            if (string.IsNullOrWhiteSpace(pathPattern))
            {
                throw new ArgumentException("日志文件路径不能为空");
            }

            // 格式化文件路径
            string formattedPath = FormatLogPath(pathPattern);
            string fullPath = Path.GetFullPath(formattedPath);

            // 检查是否在敏感文件夹中
            if (IsSensitiveFolder(fullPath))
            {
                // 弹窗确认
                DialogResult result = PermissionDialog.ShowPermissionDialog(
                    "安全警告",
                    "您正在尝试将日志文件写入系统敏感文件夹，这可能影响系统安全。",
                    fullPath
                );

                if (result == DialogResult.Abort)
                {
                    // 永久阻止
                    return;
                }
                else if (result == DialogResult.Cancel)
                {
                    // 拒绝
                    return;
                }
                // DialogResult.OK - 允许，继续执行
            }

            // 检查目录是否存在，不存在则创建
            string? directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new Exception($"无法创建日志目录: {directory}", ex);
                }
            }

            // 更新日志文件路径
            _logFilePath = fullPath;
        }

        /// <summary>
        /// 格式化日志文件路径
        /// </summary>
        private string FormatLogPath(string pattern)
        {
            string result = pattern;
            DateTime now = DateTime.Now;

            // 替换日期时间占位符
            result = result.Replace("{yyyy}", now.ToString("yyyy"));
            result = result.Replace("{yy}", now.ToString("yy"));
            result = result.Replace("{MM}", now.ToString("MM"));
            result = result.Replace("{dd}", now.ToString("dd"));
            result = result.Replace("{HH}", now.ToString("HH"));
            result = result.Replace("{mm}", now.ToString("mm"));
            result = result.Replace("{ss}", now.ToString("ss"));
            result = result.Replace("{fff}", now.ToString("fff"));

            // 替换序号占位符（使用简单的自增序号）
            result = result.Replace("{%i}", DateTime.Now.Ticks.ToString().Substring(0, 6));

            return result;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logEntry = $"[{timestamp}] [{level}] {message}";

            // 输出到控制台（调试时可见）
            Console.WriteLine(logEntry);

            // 写入日志文件（线程安全）
            try
            {
                lock (_lockObject)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // 忽略日志写入错误
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        public void Info(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        public void Warn(string message)
        {
            WriteLog("WARN", message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public void Error(string message)
        {
            WriteLog("ERROR", message);
        }
    }
}