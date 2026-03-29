using System;

namespace puppet
{
    /// <summary>
    /// 安全异常类，用于表示签名验证、证书验证等安全相关的错误
    /// </summary>
    public class SecurityException : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public enum ErrorCode
        {
            /// <summary>未知错误</summary>
            Unknown,
            /// <summary>证书无效</summary>
            InvalidCertificate,
            /// <summary>证书已过期</summary>
            ExpiredCertificate,
            /// <summary>证书不是自签名证书</summary>
            NotSelfSignedCertificate,
            /// <summary>签名验证失败</summary>
            SignatureVerificationFailed,
            /// <summary>数据库被篡改</summary>
            DatabaseTampered,
            /// <summary>密钥无效</summary>
            InvalidKey,
            /// <summary>权限不足</summary>
            AccessDenied,
            /// <summary>应用ID不匹配</summary>
            AppIdMismatch,
            /// <summary>指纹不匹配</summary>
            FingerprintMismatch,
            /// <summary>加密失败</summary>
            EncryptionFailed,
            /// <summary>解密失败</summary>
            DecryptionFailed,
            /// <summary>算法不支持</summary>
            AlgorithmNotSupported,
            /// <summary>数据格式错误</summary>
            InvalidDataFormat
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        public ErrorCode Code { get; }

        /// <summary>
        /// 初始化SecurityException的新实例
        /// </summary>
        public SecurityException()
            : base("安全错误")
        {
            Code = ErrorCode.Unknown;
        }

        /// <summary>
        /// 使用指定错误消息初始化SecurityException的新实例
        /// </summary>
        /// <param name="message">错误消息</param>
        public SecurityException(string message)
            : base(message)
        {
            Code = ErrorCode.Unknown;
        }

        /// <summary>
        /// 使用指定错误消息和错误代码初始化SecurityException的新实例
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        public SecurityException(string message, ErrorCode code)
            : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 使用指定错误消息和内部异常初始化SecurityException的新实例
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
            Code = ErrorCode.Unknown;
        }

        /// <summary>
        /// 使用指定错误消息、错误代码和内部异常初始化SecurityException的新实例
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="innerException">内部异常</param>
        public SecurityException(string message, ErrorCode code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        /// <summary>
        /// 创建证书无效异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException InvalidCertificateException(string message = "证书无效")
        {
            return new SecurityException(message, ErrorCode.InvalidCertificate);
        }

        /// <summary>
        /// 创建证书已过期异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException ExpiredCertificateException(string message = "证书已过期")
        {
            return new SecurityException(message, ErrorCode.ExpiredCertificate);
        }

        /// <summary>
        /// 创建签名验证失败异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException SignatureVerificationFailedException(string message = "签名验证失败")
        {
            return new SecurityException(message, ErrorCode.SignatureVerificationFailed);
        }

        /// <summary>
        /// 创建数据库被篡改异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException DatabaseTamperedException(string message = "数据库可能被篡改")
        {
            return new SecurityException(message, ErrorCode.DatabaseTampered);
        }

        /// <summary>
        /// 创建权限不足异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException AccessDeniedException(string message = "权限不足，访问被拒绝")
        {
            return new SecurityException(message, ErrorCode.AccessDenied);
        }

        /// <summary>
        /// 创建应用ID不匹配异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException AppIdMismatchException(string message = "应用ID不匹配")
        {
            return new SecurityException(message, ErrorCode.AppIdMismatch);
        }

        /// <summary>
        /// 创建指纹不匹配异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException FingerprintMismatchException(string message = "证书指纹不匹配")
        {
            return new SecurityException(message, ErrorCode.FingerprintMismatch);
        }

        /// <summary>
        /// 创建解密失败异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        /// <returns>SecurityException实例</returns>
        public static SecurityException DecryptionFailedException(string message = "解密失败", Exception innerException = null)
        {
            return new SecurityException(message, ErrorCode.DecryptionFailed, innerException);
        }
    }
}