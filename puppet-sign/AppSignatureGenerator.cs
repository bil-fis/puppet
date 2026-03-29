using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace puppet
{
    /// <summary>
    /// 应用签名生成器，用于生成应用签名密钥对和签名
    /// </summary>
    public class AppSignatureGenerator
    {
        /// <summary>
        /// 证书信息类
        /// </summary>
        public class CertificateInfo
        {
            public string CommonName { get; set; } = "MyApp";
            public string Organization { get; set; } = "MyCompany";
            public string OrganizationalUnit { get; set; } = "Development";
            public string Country { get; set; } = "CN";
            public string State { get; set; } = "";
            public string Locality { get; set; } = "";
            public string Email { get; set; } = "";
            public int ValidityYears { get; set; } = 25;
        }

        /// <summary>
        /// 生成RSA密钥对
        /// </summary>
        /// <param name="keySize">密钥大小（2048或4096位）</param>
        /// <returns>RSA密钥对</returns>
        public static (RSAParameters PublicKey, RSAParameters PrivateKey) GenerateRSAKeyPair(int keySize = 2048)
        {
            if (keySize != 2048 && keySize != 4096)
            {
                throw new ArgumentException("密钥大小必须是2048或4096位", nameof(keySize));
            }

            using (var rsa = RSA.Create(keySize))
            {
                RSAParameters publicKey = rsa.ExportParameters(false);
                RSAParameters privateKey = rsa.ExportParameters(true);
                return (publicKey, privateKey);
            }
        }

        /// <summary>
        /// 创建自签名证书
        /// </summary>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="certInfo">证书信息</param>
        /// <returns>自签名证书</returns>
        public static X509Certificate2 CreateSelfSignedCertificate(RSAParameters privateKey, CertificateInfo certInfo)
        {
            if (certInfo == null)
            {
                throw new ArgumentNullException(nameof(certInfo));
            }

            // 创建证书请求
            var distinguishedName = new X500DistinguishedName(
                $"CN={certInfo.CommonName}, " +
                $"O={certInfo.Organization}, " +
                $"OU={certInfo.OrganizationalUnit}, " +
                $"C={certInfo.Country}");

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);

                var request = new CertificateRequest(
                    distinguishedName,
                    rsa,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                // 添加基本约束（非CA）
                request.CertificateExtensions.Add(
                    new X509BasicConstraintsExtension(
                        certificateAuthority: false,
                        hasPathLengthConstraint: false,
                        pathLengthConstraint: 0,
                        critical: true));

                // 添加密钥用途
                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(
                        X509KeyUsageFlags.DigitalSignature |
                        X509KeyUsageFlags.KeyEncipherment,
                        critical: true));

                // 添加扩展密钥用途
                var eku = new OidCollection {
                    new Oid("1.3.6.1.5.5.7.3.3"), // Code Signing
                    new Oid("1.3.6.1.5.5.7.3.2")  // Client Authentication
                };
                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(eku, critical: true));

                // 添加主题备用名称（如果提供了邮箱）
                if (!string.IsNullOrEmpty(certInfo.Email))
                {
                    var sanBuilder = new SubjectAlternativeNameBuilder();
                    sanBuilder.AddEmailAddress(certInfo.Email);
                    request.CertificateExtensions.Add(sanBuilder.Build(critical: false));
                }

                // 创建自签名证书
                DateTimeOffset now = DateTimeOffset.Now;
                DateTimeOffset notAfter = now.AddYears(certInfo.ValidityYears);

                var certificate = request.CreateSelfSigned(
                    now.AddMinutes(-5), // 稍微提前，避免时钟同步问题
                    notAfter);

                return certificate;
            }
        }

        /// <summary>
        /// 使用私钥对数据进行签名
        /// </summary>
        /// <param name="data">要签名的数据</param>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>签名数据</returns>
        public static byte[] SignData(byte[] data, RSAParameters privateKey, HashAlgorithmName hashAlgorithm = default)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (hashAlgorithm == default || hashAlgorithm == null)
            {
                hashAlgorithm = HashAlgorithmName.SHA256;
            }

            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(data, hashAlgorithm, RSASignaturePadding.Pkcs1);
            }
        }

        /// <summary>
        /// 使用证书对数据进行签名
        /// </summary>
        /// <param name="data">要签名的数据</param>
        /// <param name="certificate">包含私钥的证书</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>签名数据</returns>
        public static byte[] SignData(byte[] data, X509Certificate2 certificate, HashAlgorithmName hashAlgorithm = default)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            if (!certificate.HasPrivateKey)
            {
                throw new ArgumentException("证书必须包含私钥才能签名", nameof(certificate));
            }

            if (hashAlgorithm == default || hashAlgorithm == null)
            {
                hashAlgorithm = HashAlgorithmName.SHA256;
            }

            using (var rsa = certificate.GetRSAPrivateKey())
            {
                return rsa.SignData(data, hashAlgorithm, RSASignaturePadding.Pkcs1);
            }
        }

        /// <summary>
        /// 对文件进行签名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>签名数据</returns>
        public static byte[] SignFile(string filePath, RSAParameters privateKey, HashAlgorithmName hashAlgorithm = default)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件不存在", filePath);
            }

            byte[] fileData = File.ReadAllBytes(filePath);
            return SignData(fileData, privateKey, hashAlgorithm);
        }

        /// <summary>
        /// 对数据库文件进行签名（用于存储API）
        /// </summary>
        /// <param name="databasePath">数据库文件路径</param>
        /// <param name="certificate">包含私钥的证书</param>
        /// <param name="appId">应用ID（证书的CN）</param>
        /// <returns>签名数据和元数据信息</returns>
        public static (byte[] Signature, string AppId, string Fingerprint, string CertInfo) SignDatabase(
            string databasePath,
            X509Certificate2 certificate)
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentNullException(nameof(databasePath));
            }

            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException("数据库文件不存在", databasePath);
            }

            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            // 读取数据库文件内容
            byte[] databaseContent = File.ReadAllBytes(databasePath);

            // 对数据库进行签名
            byte[] signature = SignData(databaseContent, certificate);

            // 获取应用ID（证书的CN）
            string appId = ExtractCommonName(certificate.Subject);

            // 获取证书指纹
            string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);

            // 获取证书信息
            string certInfo = CertificateUtils.ExtractCertificateInfo(certificate);

            return (signature, appId, fingerprint, certInfo);
        }

        /// <summary>
        /// 从主题字符串中提取CN（Common Name）
        /// </summary>
        /// <param name="subject">主题字符串</param>
        /// <returns>Common Name</returns>
        private static string ExtractCommonName(string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                return "";
            }

            // 解析X500名称，提取CN
            string[] parts = subject.Split(',');
            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Substring(3);
                }
            }

            return "";
        }

        /// <summary>
        /// 导出私钥为PEM格式（PKCS#8）
        /// </summary>
        /// <param name="privateKey">RSA私钥</param>
        /// <returns>PEM格式私钥字符串</returns>
        public static string ExportPrivateKeyToPem(RSAParameters privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);

                // 导出PKCS#8格式的私钥
                byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();
                string base64 = Convert.ToBase64String(privateKeyBytes);

                StringBuilder pem = new StringBuilder();
                pem.AppendLine("-----BEGIN PRIVATE KEY-----");
                pem.AppendLine(base64);
                pem.Append("-----END PRIVATE KEY-----");

                return pem.ToString();
            }
        }

        /// <summary>
        /// 从PEM格式导入私钥
        /// </summary>
        /// <param name="pemContent">PEM格式私钥字符串</param>
        /// <returns>RSA私钥</returns>
        public static RSAParameters ImportPrivateKeyFromPem(string pemContent)
        {
            if (string.IsNullOrEmpty(pemContent))
            {
                throw new ArgumentNullException(nameof(pemContent));
            }

            // 移除PEM头尾
            string cleaned = pemContent
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            byte[] privateKeyBytes = Convert.FromBase64String(cleaned);

            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                return rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// 生成完整的签名密钥对（证书和私钥）
        /// </summary>
        /// <param name="certInfo">证书信息</param>
        /// <param name="keySize">密钥大小（2048或4096位）</param>
        /// <returns>证书和私钥</returns>
        public static (X509Certificate2 Certificate, RSAParameters PrivateKey) GenerateSigningKeyPair(
            CertificateInfo certInfo,
            int keySize = 2048)
        {
            // 生成RSA密钥对
            var (_, privateKey) = GenerateRSAKeyPair(keySize);

            // 创建自签名证书
            X509Certificate2 certificate = CreateSelfSignedCertificate(privateKey, certInfo);

            return (certificate, privateKey);
        }

        /// <summary>
        /// 使用密码加密私钥（用于存储在PUP文件中）
        /// </summary>
        /// <param name="privateKey">RSA私钥参数</param>
        /// <param name="password">密码</param>
        /// <returns>加密后的私钥数据</returns>
        public static byte[] EncryptPrivateKey(RSAParameters privateKey, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // 将私钥参数转换为字节数组（这里简化处理，实际可能需要更复杂的序列化）
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();

                // 使用AES-256-GCM加密
                return CryptoUtils.EncryptWithPassword(privateKeyBytes, password);
            }
        }

        /// <summary>
        /// 使用密码解密私钥
        /// </summary>
        /// <param name="encryptedPrivateKey">加密的私钥数据</param>
        /// <param name="password">密码</param>
        /// <returns>RSA私钥参数</returns>
        public static RSAParameters DecryptPrivateKey(byte[] encryptedPrivateKey, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // 使用AES-256-GCM解密
            byte[] privateKeyBytes = CryptoUtils.DecryptWithPassword(encryptedPrivateKey, password);

            // 导入私钥
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                return rsa.ExportParameters(true);
            }
        }
    }
}