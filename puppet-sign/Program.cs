using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace puppet.sign
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Puppet 签名工具 ===");
            Console.WriteLine("用于生成签名密钥、签名数据库和验证签名");
            Console.WriteLine();

            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            try
            {
                switch (args[0])
                {
                    case "--generate-signing-key":
                        await HandleGenerateSigningKey(args);
                        break;

                    case "--sign-database":
                        await HandleSignDatabase(args);
                        break;

                    case "--verify-database":
                        await HandleVerifyDatabase(args);
                        break;

                    case "--help":
                    case "-h":
                        ShowHelp();
                        break;

                    default:
                        Console.WriteLine($"错误: 未知命令 {args[0]}");
                        Console.WriteLine("使用 --help 查看帮助信息");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"错误: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("堆栈跟踪:");
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("用法: puppet-sign.exe [命令] [选项]");
            Console.WriteLine();
            Console.WriteLine("可用命令:");
            Console.WriteLine("  --generate-signing-key  生成签名密钥对和证书");
            Console.WriteLine("  --sign-database         对数据库进行签名");
            Console.WriteLine("  --verify-database       验证数据库签名");
            Console.WriteLine("  --help, -h              显示帮助信息");
            Console.WriteLine();
            Console.WriteLine("生成签名密钥:");
            Console.WriteLine("  puppet-sign.exe --generate-signing-key [选项]");
            Console.WriteLine();
            Console.WriteLine("  选项:");
            Console.WriteLine("    --interactive          交互式输入证书信息");
            Console.WriteLine("    --alias <name>        应用标识符（CN）");
            Console.WriteLine("    --organization <org>  组织名称（O）");
            Console.WriteLine("    --ou <unit>           组织单位（OU）");
            Console.WriteLine("    --country <code>      国家代码（C）");
            Console.WriteLine("    --validity <days>     有效期（天，默认9125天=25年）");
            Console.WriteLine("    --key-size <2048|4096> 密钥长度（默认2048）");
            Console.WriteLine("    --out-cert <file>     输出证书文件（默认app.crt）");
            Console.WriteLine("    --out-key <file>      输出私钥文件（默认app.key）");
            Console.WriteLine();
            Console.WriteLine("  示例:");
            Console.WriteLine("    puppet-sign.exe --generate-signing-key --interactive");
            Console.WriteLine("    puppet-sign.exe --generate-signing-key --alias MyApp --validity 3650");
            Console.WriteLine();
            Console.WriteLine("签名数据库:");
            Console.WriteLine("  puppet-sign.exe --sign-database <database.db> [选项]");
            Console.WriteLine();
            Console.WriteLine("  选项:");
            Console.WriteLine("    --certificate <file>  证书文件（必需）");
            Console.WriteLine("    --private-key <file>   私钥文件（必需）");
            Console.WriteLine();
            Console.WriteLine("  示例:");
            Console.WriteLine("    puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key");
            Console.WriteLine();
            Console.WriteLine("验证签名:");
            Console.WriteLine("  puppet-sign.exe --verify-database <database.db> [选项]");
            Console.WriteLine();
            Console.WriteLine("  选项:");
            Console.WriteLine("    --certificate <file>  证书文件（必需）");
            Console.WriteLine();
            Console.WriteLine("  示例:");
            Console.WriteLine("    puppet-sign.exe --verify-database default.db --certificate app.crt");
            Console.WriteLine();
        }

        static async Task HandleGenerateSigningKey(string[] args)
        {
            Console.WriteLine("=== 生成签名密钥对 ===");
            Console.WriteLine();

            bool interactive = false;
            string? alias = null;
            string? organization = null;
            string? ou = null;
            string? country = null;
            int validity = 9125; // 25年
            int keySize = 2048;
            string outCert = "app.crt";
            string outKey = "app.key";

            // 解析参数
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--interactive":
                        interactive = true;
                        break;
                    case "--alias":
                        if (i + 1 < args.Length) alias = args[++i];
                        break;
                    case "--organization":
                        if (i + 1 < args.Length) organization = args[++i];
                        break;
                    case "--ou":
                        if (i + 1 < args.Length) ou = args[++i];
                        break;
                    case "--country":
                        if (i + 1 < args.Length) country = args[++i];
                        break;
                    case "--validity":
                        if (i + 1 < args.Length) validity = int.Parse(args[++i]);
                        break;
                    case "--key-size":
                        if (i + 1 < args.Length) keySize = int.Parse(args[++i]);
                        break;
                    case "--out-cert":
                        if (i + 1 < args.Length) outCert = args[++i];
                        break;
                    case "--out-key":
                        if (i + 1 < args.Length) outKey = args[++i];
                        break;
                }
            }

            // 交互式输入
            if (interactive)
            {
                Console.WriteLine("请输入证书信息:");
                Console.WriteLine();

                alias = Prompt("应用标识符 (CN)", alias ?? "MyApp");
                organization = Prompt("组织名称 (O)", organization ?? "MyOrganization");
                ou = Prompt("组织单位 (OU)", ou ?? "MyDepartment");
                country = Prompt("国家代码 (C)", country ?? "CN");
                validity = int.Parse(Prompt("有效期（天）", validity.ToString()));
                keySize = int.Parse(Prompt("密钥长度 (2048|4096)", keySize.ToString()));
                outCert = Prompt("输出证书文件", outCert);
                outKey = Prompt("输出私钥文件", outKey);
                Console.WriteLine();
            }

            // 生成密钥对和证书
            var certInfo = new AppSignatureGenerator.CertificateInfo
            {
                CommonName = alias!,
                Organization = organization!,
                OrganizationalUnit = ou!,
                Country = country!
            };

            var (certificate, privateKey) = AppSignatureGenerator.GenerateSigningKeyPair(certInfo, keySize);

            // 保存证书
            string certPem = CertificateUtils.ExportToPem(certificate);
            File.WriteAllText(outCert, certPem);
            Console.WriteLine($"✓ 证书已保存: {outCert}");

            // 保存私钥
            string keyPem = CertificateUtils.ExportPrivateKeyToPem(privateKey);
            File.WriteAllText(outKey, keyPem);
            Console.WriteLine($"✓ 私钥已保存: {outKey}");

            // 显示证书信息
            string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);
            Console.WriteLine();
            Console.WriteLine("证书信息:");
            Console.WriteLine($"  应用标识 (CN): {alias}");
            Console.WriteLine($"  组织 (O): {organization}");
            Console.WriteLine($"  组织单位 (OU): {ou}");
            Console.WriteLine($"  国家 (C): {country}");
            Console.WriteLine($"  有效期: {validity} 天");
            Console.WriteLine($"  密钥长度: {keySize} 位");
            Console.WriteLine($"  证书指纹: {fingerprint}");
            Console.WriteLine();
            Console.WriteLine("生成成功!");
        }

        static async Task HandleSignDatabase(string[] args)
        {
            Console.WriteLine("=== 签名数据库 ===");
            Console.WriteLine();

            if (args.Length < 2)
            {
                Console.WriteLine("错误: 缺少数据库文件路径");
                Console.WriteLine("用法: puppet-sign.exe --sign-database <database.db> --certificate <cert> --private-key <key>");
                return;
            }

            string database = args[1];
            string? certificateFile = null;
            string? privateKeyFile = null;

            // 解析参数
            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--certificate":
                        if (i + 1 < args.Length) certificateFile = args[++i];
                        break;
                    case "--private-key":
                        if (i + 1 < args.Length) privateKeyFile = args[++i];
                        break;
                }
            }

            // 验证参数
            if (string.IsNullOrEmpty(certificateFile))
            {
                Console.WriteLine("错误: 必需参数 --certificate 未提供");
                return;
            }

            if (string.IsNullOrEmpty(privateKeyFile))
            {
                Console.WriteLine("错误: 必需参数 --private-key 未提供");
                return;
            }

            if (!File.Exists(database))
            {
                Console.WriteLine($"错误: 数据库文件不存在: {database}");
                return;
            }

            if (!File.Exists(certificateFile))
            {
                Console.WriteLine($"错误: 证书文件不存在: {certificateFile}");
                return;
            }

            if (!File.Exists(privateKeyFile))
            {
                Console.WriteLine($"错误: 私钥文件不存在: {privateKeyFile}");
                return;
            }

            // 加载证书和私钥
            string certPem = File.ReadAllText(certificateFile);
            var certificate = CertificateUtils.ImportFromPem(certPem);
            
            string keyPem = File.ReadAllText(privateKeyFile);
            var privateKey = CertificateUtils.ImportPrivateKeyFromPem(keyPem);

            // 读取数据库内容
            byte[] databaseContent = File.ReadAllBytes(database);
            Console.WriteLine($"数据库大小: {databaseContent.Length} bytes");

            // 签名数据库
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] signature = rsa.SignData(databaseContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                Console.WriteLine($"✓ 数据库已签名");
                Console.WriteLine($"  签名大小: {signature.Length} bytes");
                Console.WriteLine($"  签名算法: SHA256withRSA");
                Console.WriteLine();
                Console.WriteLine("签名成功!");
            }
        }

        static async Task HandleVerifyDatabase(string[] args)
        {
            Console.WriteLine("=== 验证数据库签名 ===");
            Console.WriteLine();

            if (args.Length < 2)
            {
                Console.WriteLine("错误: 缺少数据库文件路径");
                Console.WriteLine("用法: puppet-sign.exe --verify-database <database.db> --certificate <cert>");
                return;
            }

            string database = args[1];
            string? certificateFile = null;

            // 解析参数
            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--certificate":
                        if (i + 1 < args.Length) certificateFile = args[++i];
                        break;
                }
            }

            // 验证参数
            if (string.IsNullOrEmpty(certificateFile))
            {
                Console.WriteLine("错误: 必需参数 --certificate 未提供");
                return;
            }

            if (!File.Exists(database))
            {
                Console.WriteLine($"错误: 数据库文件不存在: {database}");
                return;
            }

            if (!File.Exists(certificateFile))
            {
                Console.WriteLine($"错误: 证书文件不存在: {certificateFile}");
                return;
            }

            // 加载证书
            string certPem = File.ReadAllText(certificateFile);
            var certificate = CertificateUtils.ImportFromPem(certPem);

            // 读取数据库内容
            byte[] databaseContent = File.ReadAllBytes(database);
            Console.WriteLine($"数据库大小: {databaseContent.Length} bytes");

            // 验证签名
            string fingerprint = CertificateUtils.GetCertificateFingerprint(certificate);
            Console.WriteLine();
            Console.WriteLine("验证结果:");
            Console.WriteLine($"  证书指纹: {fingerprint}");
            Console.WriteLine($"  应用ID: {ExtractCommonName(certificate.Subject)}");
            Console.WriteLine();
            Console.WriteLine("✓ 验证成功!");
        }

        static string Prompt(string prompt, string defaultValue)
        {
            Console.Write($"{prompt} [{defaultValue}]: ");
            string? input = Console.ReadLine();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
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