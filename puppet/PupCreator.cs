using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace puppet
{
    /// <summary>
    /// PUP 文件创建器
    /// 用于创建 PUP V1.0 格式的文件
    /// </summary>
    public class PupCreator
    {
        // PUP 文件标识头
        private const string PUP_HEADER = "PUP V1.0";

        /// <summary>
        /// 创建 PUP 文件
        /// </summary>
        /// <param name="sourceFolder">源文件夹</param>
        /// <param name="outputPupFile">输出 PUP 文件路径</param>
        /// <param name="password">ZIP 密码（可选）</param>
        public static void CreatePup(string sourceFolder, string outputPupFile, string? password = null)
        {
            if (!Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException($"Source folder not found: {sourceFolder}");
            }

            Console.WriteLine($"Creating PUP file...");
            Console.WriteLine($"Source: {sourceFolder}");
            Console.WriteLine($"Output: {outputPupFile}");
            Console.WriteLine($"Password: {(string.IsNullOrEmpty(password) ? "None" : "Protected")}");

            // 1. 创建临时 ZIP 文件
            string tempZipFile = Path.GetTempFileName();
            try
            {
                Console.WriteLine($"Creating temporary ZIP file...");

                if (string.IsNullOrEmpty(password))
                {
                    // 创建无密码 ZIP
                    CreateZipWithoutPassword(sourceFolder, tempZipFile);
                }
                else
                {
                    // 创建带密码的 ZIP（密码已加密）
                    AesHelper.CreateZipWithEncryptedPassword(sourceFolder, tempZipFile, password);
                }

                // 2. 读取 ZIP 数据
                byte[] zipData = File.ReadAllBytes(tempZipFile);
                Console.WriteLine($"ZIP size: {zipData.Length} bytes");

                // 3. 添加 PUP 标识头
                byte[] pupHeader = Encoding.ASCII.GetBytes(PUP_HEADER);

                // 4. 合并数据
                byte[] pupData = new byte[pupHeader.Length + zipData.Length];
                Buffer.BlockCopy(pupHeader, 0, pupData, 0, pupHeader.Length);
                Buffer.BlockCopy(zipData, 0, pupData, pupHeader.Length, zipData.Length);

                // 5. 保存 PUP 文件
                string outputDir = Path.GetDirectoryName(outputPupFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                File.WriteAllBytes(outputPupFile, pupData);
                Console.WriteLine($"PUP file created successfully: {outputPupFile}");
                Console.WriteLine($"Total size: {pupData.Length} bytes");
            }
            finally
            {
                // 删除临时 ZIP 文件
                if (File.Exists(tempZipFile))
                {
                    File.Delete(tempZipFile);
                }
            }
        }

        /// <summary>
        /// 创建无密码的 ZIP 文件
        /// </summary>
        private static void CreateZipWithoutPassword(string sourceFolder, string zipPath)
        {
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (string file in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
                {
                    string entryName = Path.GetRelativePath(sourceFolder, file);
                    zip.CreateEntryFromFile(file, entryName);
                    Console.WriteLine($"  Added: {entryName}");
                }
            }
        }
    }
}