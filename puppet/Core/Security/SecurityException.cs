using System;

namespace puppet.Core.Security
{
    /// <summary>
    /// 安全相关异常
    /// </summary>
    public class SecurityException : Exception
    {
        /// <summary>
        /// 初始化 SecurityException 的新实例
        /// </summary>
        public SecurityException()
        {
        }

        /// <summary>
        /// 初始化 SecurityException 的新实例，使用指定的错误消息
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public SecurityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化 SecurityException 的新实例，使用指定的错误消息和内部异常
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        /// <param name="innerException">导致当前异常的异常</param>
        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}