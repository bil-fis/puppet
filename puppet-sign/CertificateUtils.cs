using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace puppet
{
    /// <summary>
    /// 证书工具类，提供证书操作相关功能
    /// </summary>
    public static class CertificateUtils
    {
        /// <summary>
        /// 计算证书指纹（SHA256）
        /// </summary>
        /// <param name="certificate">X.509证书</param>
        /// <returns>证书指纹（十六进制字符串，大写，带SHA256:前缀）</returns>
        public static string GetCertificateFingerprint(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            byte[] thumbprint = certificate.GetCertHash();
            string hexThumbprint = BitConverter.ToString(thumbprint).Replace("-", "").ToUpper();
            return $"SHA256:{hexThumbprint}";
        }

        /// <summary>
        /// 检查证书是否为自签名证书
        /// </summary>
        /// <param name="certificate">X.509证书</param>
        /// <returns>如果是自签名证书返回true，否则返回false</returns>
        public static bool IsSelfSigned(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            // 自签名证书的Subject和Issuer相同
            string subject = certificate.Subject;
            string issuer = certificate.Issuer;

            return string.Equals(subject, issuer, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 验证证书的有效性
        /// </summary>
        /// <param name="certificate">X.509证书</param>
        /// <returns>如果证书有效返回true，否则返回false</returns>
        public static bool IsValidCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                return false;
            }

            try
            {
                // 检查证书是否在有效期内
                DateTime now = DateTime.Now;
                if (now < certificate.NotBefore || now > certificate.NotAfter)
                {
                    return false;
                }

                // 检查私钥是否存在（如果需要）
                if (!certificate.HasPrivateKey)
                {
                    // 对于仅验证公钥的场景，私钥不是必需的
                    // 这里可以根据具体需求调整
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从证书中提取信息
        /// </summary>
        /// <param name="certificate">X.509证书</param>
        /// <returns>证书信息（JSON格式字符串）</returns>
        public static string ExtractCertificateInfo(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            var info = new
            {
                subject = certificate.Subject,
                issuer = certificate.Issuer,
                notBefore = certificate.NotBefore.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                notAfter = certificate.NotAfter.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                fingerprint = GetCertificateFingerprint(certificate),
                serialNumber = BitConverter.ToString(certificate.GetSerialNumber()).Replace("-", ""),
                version = certificate.Version.ToString(),
                hasPrivateKey = certificate.HasPrivateKey,
                signatureAlgorithm = certificate.SignatureAlgorithm?.FriendlyName ?? "Unknown"
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(info, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// 将证书导出为PEM格式
        /// </summary>
        /// <param name="certificate">X.509证书</param>
        /// <returns>PEM格式证书字符串</returns>
        public static string ExportToPem(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }

            byte[] certBytes = certificate.Export(X509ContentType.Cert);
            string base64Cert = Convert.ToBase64String(certBytes);
            StringBuilder pemBuilder = new StringBuilder();
            pemBuilder.AppendLine("-----BEGIN CERTIFICATE-----");
            pemBuilder.AppendLine(base64Cert);
            pemBuilder.Append("-----END CERTIFICATE-----");

            return pemBuilder.ToString();
        }

        /// <summary>
        /// 从PEM格式导入证书
        /// </summary>
        /// <param name="pemContent">PEM格式证书字符串</param>
        /// <returns>X.509证书</returns>
        public static X509Certificate2 ImportFromPem(string pemContent)
        {
            if (string.IsNullOrEmpty(pemContent))
            {
                throw new ArgumentNullException(nameof(pemContent));
            }

            // 移除PEM头尾
            string cleaned = pemContent
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            byte[] certBytes = Convert.FromBase64String(cleaned);
            return System.Security.Cryptography.X509Certificates.X509CertificateLoader.LoadCertificate(certBytes);
        }

        /// <summary>
        /// 将RSA私钥导出为PEM格式
        /// </summary>
        /// <param name="privateKey">RSA私钥</param>
        /// <param name="exportEncrypted">是否导出加密的私钥（PKCS#8格式）</param>
        /// <returns>PEM格式私钥字符串</returns>
        public static string ExportPrivateKeyToPem(RSAParameters privateKey, bool exportEncrypted = false)
        {
            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(privateKey);
                    
                    // 导出为 PKCS#8 格式
                    var pkcs8 = rsa.ExportPkcs8PrivateKey();
                    return System.Convert.ToBase64String(pkcs8);
                }
            }
            catch (Exception ex)
            {
                throw new SecurityException("Failed to export private key to PEM format", ex);
            }
        }

        /// <summary>
        /// 从PEM格式导入私钥
        /// </summary>
        /// <param name="pemContent">PEM格式私钥字符串</param>
        /// <returns>RSA私钥参数</returns>
        public static System.Security.Cryptography.RSAParameters ImportPrivateKeyFromPem(string pemContent)
        {
            try
            {
                using (var rsa = RSA.Create())
                {
                    byte[] privateKeyBytes = System.Convert.FromBase64String(pemContent);
                    int bytesRead;
                    rsa.ImportPkcs8PrivateKey(privateKeyBytes, out bytesRead);
                    return rsa.ExportParameters(true);
                }
            }
            catch (Exception ex)
            {
                throw new SecurityException("Failed to import private key from PEM format", ex);
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="signature">签名数据</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>如果签名有效返回true，否则返回false</returns>
        public static bool VerifySignature(
            byte[] data,
            byte[] signature,
            RSAParameters publicKey,
            HashAlgorithmName hashAlgorithm = default)
        {
            if (data == null || signature == null)
            {
                return false;
            }

            if (hashAlgorithm == default)
            {
                hashAlgorithm = HashAlgorithmName.SHA256;
            }

            try
            {
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(publicKey);
                    return rsa.VerifyData(data, signature, hashAlgorithm, RSASignaturePadding.Pkcs1);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证签名（使用证书）
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="signature">签名数据</param>
        /// <param name="certificate">包含公钥的证书</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>如果签名有效返回true，否则返回false</returns>
        public static bool VerifySignature(
            byte[] data,
            byte[] signature,
            X509Certificate2 certificate,
            HashAlgorithmName hashAlgorithm = default)
        {
            if (certificate == null)
            {
                return false;
            }

            RSAParameters publicKey;
            try
            {
                var rsa = certificate.GetRSAPublicKey();
                if (rsa == null)
                {
                    return false;
                }
                publicKey = rsa.ExportParameters(false);
            }
            catch
            {
                return false;
            }

            return VerifySignature(data, signature, publicKey, hashAlgorithm);
        }

        /// <summary>
        /// 计算数据的哈希值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="hashAlgorithm">哈希算法（默认SHA256）</param>
        /// <returns>哈希值</returns>
        public static byte[] ComputeHash(byte[] data, HashAlgorithmName hashAlgorithm = default)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (hashAlgorithm == default)
            {
                hashAlgorithm = HashAlgorithmName.SHA256;
            }

            return hashAlgorithm.Name switch
            {
                "SHA256" => System.Security.Cryptography.SHA256.HashData(data),
                "SHA384" => System.Security.Cryptography.SHA384.HashData(data),
                "SHA512" => System.Security.Cryptography.SHA512.HashData(data),
                "SHA1" => System.Security.Cryptography.SHA1.HashData(data),
                "MD5" => System.Security.Cryptography.MD5.HashData(data),
                _ => throw new NotSupportedException($"不支持的哈希算法: {hashAlgorithm.Name}")
            };
        }
    }
}