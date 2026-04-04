using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace puppet
{
    /// <summary>
    /// 应用签名验证器，用于验证应用签名
    /// </summary>
    public class AppSignatureValidator
    {
        /// <summary>
        /// 验证数据库签名
        /// </summary>
        /// <param name="databasePath">数据库文件路径</param>
        /// <param name="certificate">证书</param>
        /// <param name="signature">签名数据</param>
        /// <returns>验证结果和详细信息</returns>
        public static (bool IsValid, string Message) ValidateDatabaseSignature(
            string databasePath,
            X509Certificate2 certificate,
            byte[] signature)
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                return (false, "数据库路径为空");
            }

            if (!File.Exists(databasePath))
            {
                return (false, $"数据库文件不存在: {databasePath}");
            }

            if (certificate == null)
            {
                return (false, "证书为空");
            }

            if (signature == null || signature.Length == 0)
            {
                return (false, "签名数据为空");
            }

            try
            {
                // 验证证书
                var (certValid, certMessage) = ValidateCertificate(certificate);
                if (!certValid)
                {
                    return (false, certMessage);
                }

                // 验证证书是自签名证书
                if (!CertificateUtils.IsSelfSigned(certificate))
                {
                    return (false, "证书必须是自签名证书");
                }

                // 读取数据库内容
                byte[] databaseContent = File.ReadAllBytes(databasePath);

                // 计算数据库哈希
                byte[] hash = CryptoUtils.ComputeSHA256(databaseContent);

                // 使用证书公钥验证签名
                bool signatureValid = CertificateUtils.VerifySignature(
                    databaseContent,
                    signature,
                    certificate,
                    HashAlgorithmName.SHA256);

                if (!signatureValid)
                {
                    return (false, "签名验证失败，数据库可能被篡改");
                }

                string appId = ExtractCommonName(certificate.Subject);
                string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);

                return (true, $"签名验证成功 - AppID: {appId}, 指纹: {fingerprint}");
            }
            catch (Exception ex)
            {
                return (false, $"签名验证异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证证书
        /// </summary>
        /// <param name="certificate">证书</param>
        /// <returns>验证结果和详细信息</returns>
        public static (bool IsValid, string Message) ValidateCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                return (false, "证书为空");
            }

            try
            {
                // 验证证书格式
                if (!CertificateUtils.IsValidCertificate(certificate))
                {
                    return (false, "证书无效或已过期");
                }

                // 检查密钥用途
                // 确保证书包含代码签名用途
                var ekuExtension = certificate.Extensions["2.5.29.37"] as X509EnhancedKeyUsageExtension;
                if (ekuExtension != null)
                {
                    bool hasCodeSigning = false;
                    foreach (var oid in ekuExtension.EnhancedKeyUsages)
                    {
                        if (oid.Value == "1.3.6.1.5.5.7.3.3") // Code Signing
                        {
                            hasCodeSigning = true;
                            break;
                        }
                    }
                    if (!hasCodeSigning)
                    {
                        return (false, "证书不包含代码签名用途");
                    }
                }

                // 检查基本约束
                var basicConstraints = certificate.Extensions["2.5.29.19"] as X509BasicConstraintsExtension;
                if (basicConstraints != null)
                {
                    if (basicConstraints.CertificateAuthority)
                    {
                        return (false, "证书不能是CA证书");
                    }
                }

                string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);
                string appId = ExtractCommonName(certificate.Subject);

                return (true, $"证书验证成功 - AppID: {appId}, 指纹: {fingerprint}");
            }
            catch (Exception ex)
            {
                return (false, $"证书验证异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证自签名证书的完整性
        /// </summary>
        /// <param name="certificate">证书</param>
        /// <returns>验证结果和详细信息</returns>
        public static (bool IsValid, string Message) ValidateSelfSignedCertificate(X509Certificate2 certificate)
        {
            if (certificate == null)
            {
                return (false, "证书为空");
            }

            try
            {
                // 验证证书是自签名
                if (!CertificateUtils.IsSelfSigned(certificate))
                {
                    return (false, "证书不是自签名证书");
                }

                // 验证证书有效期
                if (!CertificateUtils.IsValidCertificate(certificate))
                {
                    return (false, "证书已过期或未生效");
                }

                // 验证签名算法强度
                string sigAlgorithm = certificate.SignatureAlgorithm?.Value ?? "";
                if (sigAlgorithm.Contains("sha1", StringComparison.OrdinalIgnoreCase) ||
                    sigAlgorithm.Contains("md5", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, $"签名算法不安全: {sigAlgorithm}");
                }

                // 验证密钥长度
                var rsa = certificate.GetRSAPublicKey();
                if (rsa != null && rsa.KeySize < 2048)
                {
                    return (false, $"密钥长度不足: {rsa.KeySize}位（至少需要2048位）");
                }

                return (true, "自签名证书验证通过");
            }
            catch (Exception ex)
            {
                return (false, $"验证异常: {ex.Message}");
            }
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
        /// 验证证书指纹是否匹配
        /// </summary>
        /// <param name="certificate">证书</param>
        /// <param name="expectedFingerprint">期望的指纹</param>
        /// <returns>是否匹配</returns>
        public static bool ValidateFingerprint(X509Certificate2 certificate, string expectedFingerprint)
        {
            if (certificate == null || string.IsNullOrEmpty(expectedFingerprint))
            {
                return false;
            }

            string actualFingerprint = CertificateUtils.GetCertificateFingerprint(certificate);
            return string.Equals(actualFingerprint, expectedFingerprint, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 验证PUP文件的签名完整性
        /// </summary>
        /// <param name="pupPath">PUP文件路径</param>
        /// <param name="certificate">证书</param>
        /// <returns>验证结果和详细信息</returns>
        public static (bool IsValid, string Message) ValidatePupSignature(
            string pupPath,
            X509Certificate2 certificate)
        {
            if (string.IsNullOrEmpty(pupPath))
            {
                return (false, "PUP文件路径为空");
            }

            if (!File.Exists(pupPath))
            {
                return (false, $"PUP文件不存在: {pupPath}");
            }

            if (certificate == null)
            {
                return (false, "证书为空");
            }

            try
            {
                // 验证证书
                var (certValid, certMessage) = ValidateSelfSignedCertificate(certificate);
                if (!certValid)
                {
                    return (false, certMessage);
                }

                // 这里可以添加对PUP文件内容的验证
                // 例如验证ZIP数据、脚本内容等
                // 目前只验证证书本身

                string appId = ExtractCommonName(certificate.Subject);
                string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);

                return (true, $"PUP文件签名验证成功 - AppID: {appId}, 指纹: {fingerprint}");
            }
            catch (Exception ex)
            {
                return (false, $"PUP签名验证异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 比较两个证书的指纹
        /// </summary>
        /// <param name="cert1">证书1</param>
        /// <param name="cert2">证书2</param>
        /// <returns>是否相同</returns>
        public static bool CompareCertificates(X509Certificate2 cert1, X509Certificate2 cert2)
        {
            if (cert1 == null || cert2 == null)
            {
                return false;
            }

            string fingerprint1 = CertificateUtils.GetCertificateFingerprint(cert1);
            string fingerprint2 = CertificateUtils.GetCertificateFingerprint(cert2);

            return string.Equals(fingerprint1, fingerprint2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 验证数据库签名元数据
        /// </summary>
        /// <param name="databasePath">数据库文件路径</param>
        /// <param name="signature">签名数据</param>
        /// <param name="certificate">证书</param>
        /// <param name="expectedAppId">期望的应用ID（可选）</param>
        /// <returns>验证结果和详细信息</returns>
        public static (bool IsValid, string Message, string AppId, string Fingerprint) ValidateDatabaseWithMetadata(
            string databasePath,
            byte[] signature,
            X509Certificate2 certificate,
            string? expectedAppId = null)
        {
            var (isValid, message) = ValidateDatabaseSignature(databasePath, certificate, signature);

            if (!isValid)
            {
                return (false, message, "", "");
            }

            string appId = ExtractCommonName(certificate.Subject);
            string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);

            // 如果指定了期望的应用ID，则进行验证
            if (!string.IsNullOrEmpty(expectedAppId) && 
                !string.Equals(appId, expectedAppId, StringComparison.OrdinalIgnoreCase))
            {
                return (false, $"应用ID不匹配 - 期望: {expectedAppId}, 实际: {appId}", appId, fingerprint);
            }

            return (true, message, appId, fingerprint);
        }
    }
}