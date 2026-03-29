using System;
using System.Security.Cryptography;

namespace puppet
{
    /// <summary>
    /// 全局密钥管理类
    /// 用于在客户端和服务器之间共享安全密钥
    /// </summary>
    public static class SecretKey
    {
        // 请求头名称
        public const string HEADER_NAME = "Puppet-Secret";

        // 安全密钥（程序启动时生成）
        private static string? _secretKey;

        /// <summary>
        /// 获取当前的安全密钥
        /// </summary>
        public static string Key
        {
            get
            {
                if (_secretKey == null)
                {
                    throw new InvalidOperationException("SecretKey 尚未初始化，请先调用 Initialize()");
                }
                return _secretKey;
            }
        }

        /// <summary>
        /// 初始化安全密钥（在程序启动时调用）
        /// </summary>
        public static void Initialize()
        {
            if (_secretKey != null)
            {
                return; // 已经初始化过了
            }

            // 生成随机密钥（32字节的随机数据，转换为 Base64）
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            _secretKey = Convert.ToBase64String(randomBytes);

            Console.WriteLine($"SecretKey 已初始化: {_secretKey.Substring(0, 8)}... (长度: {_secretKey.Length})");
        }

        /// <summary>
        /// 验证密钥是否匹配
        /// </summary>
        public static bool Validate(string? key)
        {
            if (string.IsNullOrEmpty(key) || _secretKey == null)
            {
                return false;
            }
            return key == _secretKey;
        }
    }
}