using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace puppet
{
    /// <summary>
    /// 加密工具类，提供密码学和加密相关功能
    /// </summary>
    public static class CryptoUtils
    {
        private const int DefaultKeySize = 256; // 256位 = 32字节
        private const int DefaultIterationCount = 100000; // PBKDF2迭代次数
        private const int DefaultSaltSize = 16; // 盐值长度

        /// <summary>
        /// 使用PBKDF2派生密钥
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐值（如果为null，则生成随机盐值）</param>
        /// <param name="keySize">密钥大小（位）</param>
        /// <param name="iterationCount">迭代次数</param>
        /// <returns>派生密钥和盐值的元组</returns>
        public static (byte[] Key, byte[] Salt) DeriveKey(
            string password,
            byte[] salt = null,
            int keySize = DefaultKeySize,
            int iterationCount = DefaultIterationCount)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // 生成随机盐值（如果未提供）
            if (salt == null || salt.Length != DefaultSaltSize)
            {
                salt = new byte[DefaultSaltSize];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            // 使用PBKDF2派生密钥
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterationCount, HashAlgorithmName.SHA256))
            {
                byte[] key = pbkdf2.GetBytes(keySize / 8);
                return (key, salt);
            }
        }

        /// <summary>
        /// 使用AES-256-GCM加密数据
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="key">密钥（32字节）</param>
        /// <param name="nonce">Nonce（12字节，如果为null则生成随机nonce）</param>
        /// <returns>加密数据（包含nonce和认证标签）</returns>
        public static byte[] EncryptAesGcm(byte[] plaintext, byte[] key, byte[] nonce = null)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (key == null || key.Length != 32)
            {
                throw new ArgumentException("密钥必须是32字节（256位）", nameof(key));
            }

            // 生成随机nonce（如果未提供）
            if (nonce == null || nonce.Length != 12)
            {
                nonce = new byte[12];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(nonce);
                }
            }

            // 使用AES-GCM加密
            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[16];

            using (var aes = new AesGcm(key, tag.Length))
            {
                aes.Encrypt(nonce, plaintext, ciphertext, tag);
            }

            // 组合nonce + tag + ciphertext
            byte[] result = new byte[nonce.Length + tag.Length + ciphertext.Length];
            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
            Buffer.BlockCopy(ciphertext, 0, result, nonce.Length + tag.Length, ciphertext.Length);

            return result;
        }

        /// <summary>
        /// 使用AES-256-GCM解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据（包含nonce、tag和ciphertext）</param>
        /// <param name="key">密钥（32字节）</param>
        /// <returns>明文</returns>
        public static byte[] DecryptAesGcm(byte[] encryptedData, byte[] key)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException(nameof(encryptedData));
            }

            if (key == null || key.Length != 32)
            {
                throw new ArgumentException("密钥必须是32字节（256位）", nameof(key));
            }

            if (encryptedData.Length < 12 + 16)
            {
                throw new ArgumentException("加密数据太短", nameof(encryptedData));
            }

            // 提取nonce、tag和ciphertext
            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            byte[] ciphertext = new byte[encryptedData.Length - 12 - 16];

            Buffer.BlockCopy(encryptedData, 0, nonce, 0, 12);
            Buffer.BlockCopy(encryptedData, 12, tag, 0, 16);
            Buffer.BlockCopy(encryptedData, 28, ciphertext, 0, ciphertext.Length);

            // 使用AES-GCM解密
            byte[] plaintext = new byte[ciphertext.Length];

            try
            {
                using (var aes = new AesGcm(key, tag.Length))
                {
                    aes.Decrypt(nonce, ciphertext, tag, plaintext);
                }
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("解密失败：可能是密钥错误或数据被篡改", ex);
            }

            return plaintext;
        }

        /// <summary>
        /// 使用密码加密数据（AES-256-GCM + PBKDF2）
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <param name="password">密码</param>
        /// <returns>加密数据（包含salt、nonce、tag和ciphertext）</returns>
        public static byte[] EncryptWithPassword(byte[] plaintext, string password)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // 使用PBKDF2派生密钥
            var (key, salt) = DeriveKey(password);

            // 使用AES-GCM加密
            byte[] encrypted = EncryptAesGcm(plaintext, key);

            // 组合salt + encrypted
            byte[] result = new byte[salt.Length + encrypted.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(encrypted, 0, result, salt.Length, encrypted.Length);

            return result;
        }

        /// <summary>
        /// 使用密码解密数据（AES-256-GCM + PBKDF2）
        /// </summary>
        /// <param name="encryptedData">加密数据（包含salt、nonce、tag和ciphertext）</param>
        /// <param name="password">密码</param>
        /// <returns>明文</returns>
        public static byte[] DecryptWithPassword(byte[] encryptedData, string password)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException(nameof(encryptedData));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (encryptedData.Length < DefaultSaltSize)
            {
                throw new ArgumentException("加密数据太短", nameof(encryptedData));
            }

            // 提取salt和加密数据
            byte[] salt = new byte[DefaultSaltSize];
            byte[] encrypted = new byte[encryptedData.Length - DefaultSaltSize];

            Buffer.BlockCopy(encryptedData, 0, salt, 0, DefaultSaltSize);
            Buffer.BlockCopy(encryptedData, DefaultSaltSize, encrypted, 0, encrypted.Length);

            // 使用PBKDF2派生密钥
            var (key, _) = DeriveKey(password, salt);

            // 使用AES-GCM解密
            return DecryptAesGcm(encrypted, key);
        }

        /// <summary>
        /// 生成随机字节数组
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>随机字节数组</returns>
        public static byte[] GenerateRandomBytes(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("长度必须大于0", nameof(length));
            }

            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 计算数据的SHA256哈希值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>哈希值（32字节）</returns>
        public static byte[] ComputeSHA256(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data);
            }
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>十六进制字符串（小写）</returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 将十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] FromHexString(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentNullException(nameof(hex));
            }

            // 移除可能存在的空格和分隔符
            hex = hex.Replace(" ", "").Replace("-", "").Trim();

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("十六进制字符串长度必须是偶数", nameof(hex));
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 将字符串转换为字节数组（UTF-8编码）
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] StringToBytes(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }

            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 将字节数组转换为字符串（UTF-8编码）
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>字符串</returns>
        public static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}