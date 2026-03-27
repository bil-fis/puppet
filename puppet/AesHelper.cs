using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace puppet
{
    /// <summary>
    /// AES 加密/解密工具类
    /// 使用固定密钥 ILOVEPUPPET
    /// 参考 Microsoft Learn 文档：https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes
    /// </summary>
    public static class AesHelper
    {
        // 固定密钥
        private const string FIXED_KEY = "ILOVEPUPPET";

        /// <summary>
        /// 获取 AES 密钥（从固定密钥生成）
        /// </summary>
        private static byte[] GetKey()
        {
            // 使用 SHA256 生成 32 字节密钥
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(FIXED_KEY));
            }
        }

        /// <summary>
        /// 获取 AES IV（初始化向量）
        /// </summary>
        private static byte[] GetIv()
        {
            // 使用固定 IV（在实际应用中应该从加密数据中读取）
            return Encoding.UTF8.GetBytes("PUPPETV1.0".PadRight(16).Substring(0, 16));
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns>Base64 编码的密文</returns>
        public static string Encrypt(string plainText)
        {
            byte[] key = GetKey();
            byte[] iv = GetIv();

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">Base64 编码的密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string cipherText)
        {
            byte[] key = GetKey();
            byte[] iv = GetIv();
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var msDecrypt = new MemoryStream(cipherBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="plainBytes">明文字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] EncryptBytes(byte[] plainBytes)
        {
            byte[] key = GetKey();
            byte[] iv = GetIv();

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="cipherBytes">加密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes)
        {
            byte[] key = GetKey();
            byte[] iv = GetIv();

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var msDecrypt = new MemoryStream(cipherBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var msPlain = new MemoryStream())
                {
                    csDecrypt.CopyTo(msPlain);
                    return msPlain.ToArray();
                }
            }
        }

        /// <summary>
        /// 创建带密码的 ZIP 文件（密码已加密）
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="zipPath">ZIP 文件输出路径</param>
        /// <param name="password">ZIP 密码（将被加密）</param>
        public static void CreateZipWithEncryptedPassword(string sourceFolder, string zipPath, string password)
        {
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                // 添加文件夹中的所有文件
                foreach (string file in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
                {
                    string entryName = Path.GetRelativePath(sourceFolder, file);
                    zip.CreateEntryFromFile(file, entryName);
                }

                // 加密密码并存储在 ZIP 中
                if (!string.IsNullOrEmpty(password))
                {
                    string encryptedPassword = Encrypt(password);
                    var passwordEntry = zip.CreateEntry("__password__.enc");
                    using (var writer = new StreamWriter(passwordEntry.Open()))
                    {
                        writer.Write(encryptedPassword);
                    }
                }
            }
        }

        /// <summary>
        /// 从 ZIP 文件中解密密码
        /// </summary>
        /// <param name="zipArchive">ZIP 归档对象</param>
        /// <returns>解密后的密码，如果不存在返回 null</returns>
        public static string? DecryptPasswordFromZip(ZipArchive zipArchive)
        {
            ZipArchiveEntry? passwordEntry = zipArchive.GetEntry("__password__.enc");
            if (passwordEntry == null)
            {
                return null;
            }

            using (var passwordStream = passwordEntry.Open())
            using (var reader = new StreamReader(passwordStream))
            {
                string encryptedPassword = reader.ReadToEnd();
                return Decrypt(encryptedPassword);
            }
        }
    }
}