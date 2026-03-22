using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace puppet.Controllers
{
    public class FileSystemController
    {
        /// <summary>
        /// 检查路径是否安全（不指向 Windows 系统文件夹）
        /// </summary>
        private bool IsPathSafe(string path)
        {
            try
            {
                // 获取规范化的完整路径
                string fullPath = Path.GetFullPath(path);

                // 获取所有驱动器的根目录
                var drives = DriveInfo.GetDrives();

                // 检查每个驱动器的 Windows 文件夹
                foreach (var drive in drives)
                {
                    if (drive.DriveType == DriveType.Fixed)
                    {
                        string windowsPath = Path.Combine(drive.RootDirectory.FullName, "Windows");
                        string system32Path = Path.Combine(windowsPath, "System32");
                        string sysWOW64Path = Path.Combine(windowsPath, "SysWOW64");

                        // 检查路径是否以这些系统路径开头
                        if (fullPath.StartsWith(windowsPath, StringComparison.OrdinalIgnoreCase) ||
                            fullPath.StartsWith(system32Path, StringComparison.OrdinalIgnoreCase) ||
                            fullPath.StartsWith(sysWOW64Path, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 弹出系统原生文件选择框
        /// </summary>
        public object OpenFileDialog(string? filterJson, bool multiSelect)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = multiSelect;

                // 解析过滤器
                if (!string.IsNullOrEmpty(filterJson))
                {
                    try
                    {
                        var filters = JsonConvert.DeserializeObject<List<string>>(filterJson);
                        if (filters != null && filters.Count >= 2)
                        {
                            string filterName = filters[0];
                            string filterPattern = filters[1];
                            openFileDialog.Filter = $"{filterName}|{filterPattern}";
                        }
                    }
                    catch
                    {
                        openFileDialog.Filter = "All Files (*.*)|*.*";
                    }
                }
                else
                {
                    openFileDialog.Filter = "All Files (*.*)|*.*";
                }

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (multiSelect)
                    {
                        return openFileDialog.FileNames;
                    }
                    else
                    {
                        return openFileDialog.FileName;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 弹出系统原生文件夹选择框
        /// </summary>
        public string? OpenFolderDialog()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    return folderDialog.SelectedPath;
                }
                return null;
            }
        }

        /// <summary>
        /// 读取文件字节数据
        /// </summary>
        public string ReadFileAsByte(string path)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 读取文件以json格式
        /// </summary>
        public string ReadFileAsJson(string path)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            string json = File.ReadAllText(path);
            // 验证是否为有效的 JSON
            try
            {
                JToken.Parse(json);
                return json;
            }
            catch (JsonException)
            {
                throw new InvalidDataException("File does not contain valid JSON");
            }
        }

        /// <summary>
        /// 向文件写入字节
        /// </summary>
        public void WriteByteToFile(string path, string base64Data)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            byte[] bytes = Convert.FromBase64String(base64Data);
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 向文件写入文本
        /// </summary>
        public void WriteTextToFile(string path, string text)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            File.WriteAllText(path, text, Encoding.UTF8);
        }

        /// <summary>
        /// 向文件追加字节
        /// </summary>
        public void AppendByteToFile(string path, string base64Data)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", path);
            }

            byte[] bytes = Convert.FromBase64String(base64Data);
            File.AppendAllText(path, Convert.ToBase64String(bytes));
        }

        /// <summary>
        /// 向文件追加文本
        /// </summary>
        public void AppendTextToFile(string path, string text)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            File.AppendAllText(path, text, Encoding.UTF8);
        }

        /// <summary>
        /// 路径，文件是否存在
        /// </summary>
        public bool Exists(string path)
        {
            if (!IsPathSafe(path))
            {
                return false;
            }

            return File.Exists(path) || Directory.Exists(path);
        }

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        public void Delete(string path)
        {
            if (!IsPathSafe(path))
            {
                throw new UnauthorizedAccessException("Access to Windows system folders is not allowed");
            }

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                throw new FileNotFoundException("Path not found", path);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Directory.Delete(path, true);
            }
        }
    }
}