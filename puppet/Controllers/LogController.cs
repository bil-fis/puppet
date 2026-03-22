using System.Text;

namespace puppet.Controllers
{
    public class LogController
    {
        private string _logFilePath;

        public LogController()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "puppet.log");
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

            // 写入日志文件
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine, Encoding.UTF8);
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