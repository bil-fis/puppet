using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using puppet.Core.Security;

namespace puppet.PUP
{
    /// <summary>
    /// PUP 文件创建器
    /// 用于创建 PUP V1.0、V1.1 和 V1.2 格式的文件
    /// </summary>
    public class PupCreator
    {
        // PUP 文件标识头
        private const string PUP_HEADER_V1_0 = "PUP V1.0";
        private const string PUP_HEADER_V1_1 = "PUP V1.1";
        private const string PUP_HEADER_V1_2 = "PUP V1.2";
    
        /// <summary>
        /// 创建 PUP 文件
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="outputPupFile">输出 PUP 文件路径</param>
        /// <param name="password">ZIP 密码（可选）</param>
        /// <param name="version">PUP 版本（1.0、1.1 或 1.2）</param>
        /// <param name="scriptFile">启动脚本文件（V1.1和V1.2版本可选）</param>
        /// <param name="certificate">证书文件（V1.2版本必需）</param>
        /// <param name="privateKey">私钥文件（V1.2版本必需）</param>
        /// <param name="privateKeyPassword">私钥加密密码（V1.2版本必需）</param>
        public static void CreatePup(
            string sourceFolder, 
            string outputPupFile, 
            string? password = null, 
            string version = "1.0", 
            string? scriptFile = null,
            string? certificate = null,
            string? privateKey = null,
            string? privateKeyPassword = null)
        {
            try
            {
                Console.WriteLine("=== PUP 文件创建器 ===");
                Console.WriteLine($"源文件夹: {sourceFolder}");
                Console.WriteLine($"输出文件: {outputPupFile}");
                Console.WriteLine($"PUP版本: {version}");
                Console.WriteLine($"密码保护: {(string.IsNullOrEmpty(password) ? "否" : "是")}");
                
                // 验证版本
                if (version != "1.0" && version != "1.1" && version != "1.2")
                {
                    throw new ArgumentException($"不支持的 PUP 版本: {version}，支持的版本为 1.0、1.1 和 1.2");
                }
                
                // V1.2 版本需要证书和私钥
                if (version == "1.2")
                {
                    if (string.IsNullOrEmpty(certificate))
                    {
                        throw new ArgumentException("V1.2 版本需要指定证书文件");
                    }
                    if (string.IsNullOrEmpty(privateKey))
                    {
                        throw new ArgumentException("V1.2 版本需要指定私钥文件");
                    }
                    if (string.IsNullOrEmpty(privateKeyPassword))
                    {
                        throw new ArgumentException("V1.2 版本需要指定私钥加密密码");
                    }
                    Console.WriteLine($"证书文件: {certificate}");
                    Console.WriteLine($"私钥文件: {privateKey}");
                }
                
                if ((version == "1.1" || version == "1.2") && !string.IsNullOrEmpty(scriptFile))
                {
                    Console.WriteLine($"启动脚本: {scriptFile}");
                }
                Console.WriteLine();

                // 1. 验证源文件夹
                Console.WriteLine("步骤 1/7: 验证源文件夹...");
                if (!Directory.Exists(sourceFolder))
                {
                    throw new DirectoryNotFoundException($"源文件夹不存在: {sourceFolder}");
                }
                Console.WriteLine($"  ✓ 源文件夹存在");
                
                // 显示源文件夹信息
                DirectoryInfo dirInfo = new DirectoryInfo(sourceFolder);
                Console.WriteLine($"  完整路径: {dirInfo.FullName}");
                Console.WriteLine($"  文件夹大小: {FormatBytes(dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(f => f.Length))}");
                
                // 列出源文件夹中的文件
                Console.WriteLine($"  包含文件:");
                var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    Console.WriteLine("    警告: 源文件夹为空！");
                }
                else
                {
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        string relativePath = Path.GetRelativePath(sourceFolder, file);
                        Console.WriteLine($"    - {relativePath} ({FormatBytes(fileInfo.Length)})");
                    }
                }
                Console.WriteLine();

                // 2. 验证输出目录
                Console.WriteLine("步骤 2/7: 验证输出目录...");
                string outputDir = Path.GetDirectoryName(outputPupFile) ?? Directory.GetCurrentDirectory();
                Console.WriteLine($"  输出目录: {outputDir}");
                
                if (string.IsNullOrEmpty(outputDir))
                {
                    outputDir = Directory.GetCurrentDirectory();
                    Console.WriteLine($"  使用当前目录: {outputDir}");
                }
                
                if (!Directory.Exists(outputDir))
                {
                    Console.WriteLine($"  创建输出目录...");
                    Directory.CreateDirectory(outputDir);
                    Console.WriteLine($"  ✓ 输出目录已创建");
                }
                else
                {
                    Console.WriteLine($"  ✓ 输出目录已存在");
                }
                
                // 检查是否覆盖现有文件
                if (File.Exists(outputPupFile))
                {
                    Console.WriteLine($"  警告: 文件已存在，将被覆盖: {outputPupFile}");
                    var existingInfo = new FileInfo(outputPupFile);
                    Console.WriteLine($"  现有文件大小: {FormatBytes(existingInfo.Length)}");
                }
                Console.WriteLine();

                // 3. 创建临时 ZIP 文件
                Console.WriteLine("步骤 3/7: 创建临时 ZIP 文件...");
                string tempZipFile = Path.GetTempFileName();
                Console.WriteLine($"  临时文件: {tempZipFile}");
                
                // 删除临时文件，因为 Path.GetTempFileName() 会预先创建一个空文件
                if (File.Exists(tempZipFile))
                {
                    File.Delete(tempZipFile);
                    Console.WriteLine($"  ✓ 已删除临时空文件");
                }
                
                try
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        Console.WriteLine($"  创建无密码 ZIP...");
                        CreateZipWithoutPassword(sourceFolder, tempZipFile);
                    }
                    else
                    {
                        Console.WriteLine($"  创建加密 ZIP...");
                        CreateZipWithPassword(sourceFolder, tempZipFile, password);
                    }
                    
                    // 验证ZIP文件是否创建成功
                    if (!File.Exists(tempZipFile))
                    {
                        throw new InvalidOperationException("临时ZIP文件创建失败！");
                    }
                    
                    var zipInfo = new FileInfo(tempZipFile);
                    Console.WriteLine($"  ✓ ZIP文件已创建 ({FormatBytes(zipInfo.Length)})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ ZIP文件创建失败: {ex.Message}");
                    Console.WriteLine($"  堆栈跟踪: {ex.StackTrace}");
                    throw;
                }
                Console.WriteLine();

                // 4. 读取 ZIP 数据
                Console.WriteLine("步骤 4/7: 读取 ZIP 数据...");
                byte[] zipData = File.ReadAllBytes(tempZipFile);
                Console.WriteLine($"  ZIP 数据大小: {FormatBytes(zipData.Length)} ({zipData.Length} bytes)");
                Console.WriteLine();

                // 5. 处理脚本数据（V1.1）
                byte[] scriptData = Array.Empty<byte>();
                if (version == "1.1" && !string.IsNullOrEmpty(scriptFile))
                {
                    Console.WriteLine("步骤 5/7: 读取启动脚本...");
                    if (!File.Exists(scriptFile))
                    {
                        throw new FileNotFoundException($"启动脚本文件不存在: {scriptFile}");
                    }
                    
                    scriptData = File.ReadAllBytes(scriptFile);
                    Console.WriteLine($"  ✓ 脚本文件已读取 ({FormatBytes(scriptData.Length)} bytes)");
                    Console.WriteLine($"  脚本内容预览:");
                    string scriptContent = Encoding.UTF8.GetString(scriptData);
                    string[] lines = scriptContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int previewLines = Math.Min(lines.Length, 5);
                    for (int i = 0; i < previewLines; i++)
                    {
                        Console.WriteLine($"    {i + 1}. {lines[i]}");
                    }
                    if (lines.Length > 5)
                    {
                        Console.WriteLine($"    ... (共 {lines.Length} 行)");
                    }
                    Console.WriteLine();
                }

                // 6. 创建 PUP 文件
                Console.WriteLine("步骤 6/7: 创建 PUP 文件...");
                
                // 确定头部
                string pupHeaderStr = version switch
                {
                    "1.1" => PUP_HEADER_V1_1,
                    "1.2" => PUP_HEADER_V1_2,
                    _ => PUP_HEADER_V1_0
                };
                byte[] pupHeader = Encoding.ASCII.GetBytes(pupHeaderStr);
                Console.WriteLine($"  添加标识头: {pupHeaderStr} ({pupHeader.Length} bytes)");

                byte[] encryptedPassword = Array.Empty<byte>();

                // 如果有密码，对密码进行AES加密
                if (!string.IsNullOrEmpty(password))
                {
                    Console.WriteLine($"  对ZIP密码进行AES加密...");
                    // 将密码转换为字节，填充到32字节（2个16字节块，适合ECB模式）
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] paddedPassword = new byte[32];
                    Array.Copy(passwordBytes, 0, paddedPassword, 0, Math.Min(passwordBytes.Length, 32));
                    // ECB 模式加密 32 字节 = 32 字节输出
                    encryptedPassword = AesHelper.EncryptBytes(paddedPassword);
                    Console.WriteLine($"  加密后密码长度: {encryptedPassword.Length} bytes");
                }
                else
                {
                    Console.WriteLine($"  无密码保护");
                }

                // 准备证书和私钥（V1.2版本）
                byte[] certificateData = Array.Empty<byte>();
                byte[] encryptedPrivateKey = Array.Empty<byte>();
                
                if (version == "1.2")
                {
                    Console.WriteLine($"  加载证书和私钥...");
                    
                    // 加载证书
                    string certPem = File.ReadAllText(certificate!);
                    X509Certificate2 cert = CertificateUtils.ImportFromPem(certPem);
                    certificateData = cert.Export(X509ContentType.Cert);
                    Console.WriteLine($"  证书数据长度: {certificateData.Length} bytes");
                    
                    // 加载私钥
                    string keyPem = File.ReadAllText(privateKey!);
                    RSAParameters privateKeyParams = CertificateUtils.ImportPrivateKeyFromPem(keyPem);
                    
                    // 将私钥转换为字节数组
                    using (var rsa = RSA.Create())
                    {
                        rsa.ImportParameters(privateKeyParams);
                        byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();
                        
                        // 使用密码加密私钥（AES-256-GCM + PBKDF2）
                        encryptedPrivateKey = CryptoUtils.EncryptWithPassword(privateKeyBytes, privateKeyPassword!);
                        Console.WriteLine($"  加密私钥数据长度: {encryptedPrivateKey.Length} bytes");
                    }
                }

                // 根据版本拼接数据
                byte[] pupData;
                if (version == "1.2")
                {
                    // V1.2格式：头部 + 4字节脚本长度 + 脚本内容 + 4字节证书长度 + 证书数据 + 4字节加密私钥长度 + 加密私钥数据 + 加密密码 + ZIP数据
                    byte[] scriptLengthBytes = BitConverter.GetBytes(scriptData.Length);
                    byte[] certLengthBytes = BitConverter.GetBytes(certificateData.Length);
                    byte[] encryptedKeyLengthBytes = BitConverter.GetBytes(encryptedPrivateKey.Length);

                    int totalSize = pupHeader.Length + 4 + scriptData.Length + 4 + certificateData.Length + 
                                    4 + encryptedPrivateKey.Length + encryptedPassword.Length + zipData.Length;
                    pupData = new byte[totalSize];
                    int offset = 0;

                    // 头部
                    Buffer.BlockCopy(pupHeader, 0, pupData, offset, pupHeader.Length);
                    offset += pupHeader.Length;

                    // 脚本
                    Buffer.BlockCopy(scriptLengthBytes, 0, pupData, offset, 4);
                    offset += 4;
                    Buffer.BlockCopy(scriptData, 0, pupData, offset, scriptData.Length);
                    offset += scriptData.Length;

                    // 证书
                    Buffer.BlockCopy(certLengthBytes, 0, pupData, offset, 4);
                    offset += 4;
                    Buffer.BlockCopy(certificateData, 0, pupData, offset, certificateData.Length);
                    offset += certificateData.Length;

                    // 加密私钥
                    Buffer.BlockCopy(encryptedKeyLengthBytes, 0, pupData, offset, 4);
                    offset += 4;
                    Buffer.BlockCopy(encryptedPrivateKey, 0, pupData, offset, encryptedPrivateKey.Length);
                    offset += encryptedPrivateKey.Length;

                    // 加密密码
                    Buffer.BlockCopy(encryptedPassword, 0, pupData, offset, encryptedPassword.Length);
                    offset += encryptedPassword.Length;

                    // ZIP数据
                    Buffer.BlockCopy(zipData, 0, pupData, offset, zipData.Length);

                    Console.WriteLine($"  合并数据大小: {FormatBytes(pupData.Length)} ({pupData.Length} bytes)");
                    Console.WriteLine($"    - 头部: {pupHeader.Length} bytes");
                    Console.WriteLine($"    - 脚本长度: 4 bytes");
                    Console.WriteLine($"    - 脚本内容: {scriptData.Length} bytes");
                    Console.WriteLine($"    - 证书长度: 4 bytes");
                    Console.WriteLine($"    - 证书数据: {certificateData.Length} bytes");
                    Console.WriteLine($"    - 加密私钥长度: 4 bytes");
                    Console.WriteLine($"    - 加密私钥数据: {encryptedPrivateKey.Length} bytes");
                    Console.WriteLine($"    - 加密密码: {encryptedPassword.Length} bytes");
                    Console.WriteLine($"    - ZIP数据: {zipData.Length} bytes");
                }
                else if (version == "1.1")
                {
                    // V1.1格式：头部 + 4字节脚本长度 + 脚本内容 + 加密密码 + ZIP数据
                    byte[] scriptLengthBytes = BitConverter.GetBytes(scriptData.Length);

                    pupData = new byte[pupHeader.Length + 4 + scriptData.Length + encryptedPassword.Length + zipData.Length];
                    int offset = 0;

                    Buffer.BlockCopy(pupHeader, 0, pupData, offset, pupHeader.Length);
                    offset += pupHeader.Length;

                    Buffer.BlockCopy(scriptLengthBytes, 0, pupData, offset, 4);
                    offset += 4;

                    Buffer.BlockCopy(scriptData, 0, pupData, offset, scriptData.Length);
                    offset += scriptData.Length;

                    Buffer.BlockCopy(encryptedPassword, 0, pupData, offset, encryptedPassword.Length);
                    offset += encryptedPassword.Length;

                    Buffer.BlockCopy(zipData, 0, pupData, offset, zipData.Length);

                    Console.WriteLine($"  合并数据大小: {FormatBytes(pupData.Length)} ({pupData.Length} bytes)");
                    Console.WriteLine($"    - 头部: {pupHeader.Length} bytes");
                    Console.WriteLine($"    - 脚本长度: 4 bytes");
                    Console.WriteLine($"    - 脚本内容: {scriptData.Length} bytes");
                    Console.WriteLine($"    - 加密密码: {encryptedPassword.Length} bytes");
                    Console.WriteLine($"    - ZIP数据: {zipData.Length} bytes");
                }
                else
                {
                    // V1.0格式：头部 + 加密密码 + ZIP数据
                    pupData = new byte[pupHeader.Length + encryptedPassword.Length + zipData.Length];
                    Buffer.BlockCopy(pupHeader, 0, pupData, 0, pupHeader.Length);
                    Buffer.BlockCopy(encryptedPassword, 0, pupData, pupHeader.Length, encryptedPassword.Length);
                    Buffer.BlockCopy(zipData, 0, pupData, pupHeader.Length + encryptedPassword.Length, zipData.Length);
                    
                    Console.WriteLine($"  合并数据大小: {FormatBytes(pupData.Length)} ({pupData.Length} bytes)");
                    Console.WriteLine($"    - 头部: {pupHeader.Length} bytes");
                    Console.WriteLine($"    - 加密密码: {encryptedPassword.Length} bytes");
                    Console.WriteLine($"    - ZIP数据: {zipData.Length} bytes");
                }
                
                Console.WriteLine($"  写入文件: {outputPupFile}");

                try
                {
                    File.WriteAllBytes(outputPupFile, pupData);
                    Console.WriteLine($"  ✓ PUP文件已写入");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ 文件写入失败: {ex.Message}");
                    Console.WriteLine($"  堆栈跟踪: {ex.StackTrace}");
                    throw;
                }
                Console.WriteLine();

                // 7. 验证文件
                Console.WriteLine("步骤 7/7: 验证 PUP 文件...");
                if (!File.Exists(outputPupFile))
                {
                    throw new InvalidOperationException("PUP文件创建后验证失败！");
                }
                
                var pupInfo = new FileInfo(outputPupFile);
                Console.WriteLine($"  ✓ PUP文件已验证存在");
                Console.WriteLine($"  文件大小: {FormatBytes(pupInfo.Length)}");
                Console.WriteLine($"  文件路径: {pupInfo.FullName}");
                Console.WriteLine($"  创建时间: {pupInfo.CreationTime}");
                Console.WriteLine();

                // 清理临时文件
                Console.WriteLine("清理临时文件...");
                if (File.Exists(tempZipFile))
                {
                    File.Delete(tempZipFile);
                    Console.WriteLine($"  ✓ 临时ZIP文件已删除");
                }
                Console.WriteLine();

                // 最终总结
                Console.WriteLine("=== 创建成功 ===");
                Console.WriteLine($"PUP文件: {outputPupFile}");
                Console.WriteLine($"PUP版本: {version}");
                Console.WriteLine($"总大小: {FormatBytes(pupInfo.Length)}");
                Console.WriteLine($"包含文件: {files.Length} 个");
                Console.WriteLine($"密码保护: {(string.IsNullOrEmpty(password) ? "否" : $"是 ({password.Length} 字符)")}");
                if (version == "1.1")
                {
                    Console.WriteLine($"启动脚本: {(string.IsNullOrEmpty(scriptFile) ? "无" : $"{FormatBytes(scriptData.Length)}")}");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("=== 创建失败 ===");
                Console.WriteLine($"错误: {ex.GetType().Name}");
                Console.WriteLine($"消息: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("详细信息:");
                Console.WriteLine($"  {ex.StackTrace}");
                Console.WriteLine();
                
                // 如果是已知异常类型，提供帮助信息
                if (ex is DirectoryNotFoundException)
                {
                    Console.WriteLine("提示: 请检查源文件夹路径是否正确");
                }
                else if (ex is UnauthorizedAccessException)
                {
                    Console.WriteLine("提示: 请检查是否有足够的权限访问源文件夹或写入输出目录");
                }
                else if (ex is IOException)
                {
                    Console.WriteLine("提示: 请检查文件路径是否有效，或是否有其他程序占用文件");
                }
                
                throw;
            }
        }

        /// <summary>
        /// 创建无密码的 ZIP 文件
        /// </summary>
        private static void CreateZipWithoutPassword(string sourceFolder, string zipPath)
        {
            var fastZip = new FastZip();
            fastZip.CreateEmptyDirectories = true;

            var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
            int fileCount = 0;

            Console.WriteLine($"  添加文件到ZIP:");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                string relativePath = Path.GetRelativePath(sourceFolder, file);
                fileCount++;
                Console.WriteLine($"    [{fileCount}] {relativePath} ({FormatBytes(fileInfo.Length)})");
            }

            fastZip.CreateZip(zipPath, sourceFolder, true, "");
            Console.WriteLine($"  总计: {fileCount} 个文件");
        }

        /// <summary>
        /// 创建带密码的 ZIP 文件
        /// </summary>
        private static void CreateZipWithPassword(string sourceFolder, string zipPath, string password)
        {
            var fastZip = new FastZip();
            fastZip.CreateEmptyDirectories = true;
            fastZip.Password = password;

            var files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
            int fileCount = 0;

            Console.WriteLine($"  添加文件到ZIP (密码保护):");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                string relativePath = Path.GetRelativePath(sourceFolder, file);
                fileCount++;
                Console.WriteLine($"    [{fileCount}] {relativePath} ({FormatBytes(fileInfo.Length)})");
            }

            fastZip.CreateZip(zipPath, sourceFolder, true, "");
            Console.WriteLine($"  总计: {fileCount} 个文件 (已加密)");
        }

        /// <summary>
        /// 格式化字节大小
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// 从主题字符串中提取CN（Common Name）
        /// </summary>
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
    }
    }
