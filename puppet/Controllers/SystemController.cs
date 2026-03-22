using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Management;
using Newtonsoft.Json;

namespace puppet.Controllers
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class SystemController
    {
        #region Win32 API Declarations

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int SPI_GETDESKWALLPAPER = 115;

        #endregion

        /// <summary>
        /// 获取系统信息
        /// </summary>
        public string GetSystemInfo()
        {
            var info = new
            {
                OperatingSystem = new
                {
                    OSName = Environment.OSVersion.Platform.ToString(),
                    Version = Environment.OSVersion.VersionString,
                    MachineName = Environment.MachineName,
                    UserName = Environment.UserName
                },
                Hardware = new
                {
                    ProcessorCount = Environment.ProcessorCount,
                    ProcessorArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
                    SystemPageSize = Environment.SystemPageSize
                },
                Memory = new
                {
                    TotalPhysicalMemory = GetTotalPhysicalMemory(),
                    AvailablePhysicalMemory = GetAvailablePhysicalMemory()
                },
                Display = new
                {
                    PrimaryScreen = new
                    {
                        Width = Screen.PrimaryScreen.Bounds.Width,
                        Height = Screen.PrimaryScreen.Bounds.Height,
                        BitsPerPixel = Screen.PrimaryScreen.BitsPerPixel
                    }
                },
                GPU = GetGPUInfo()
            };

            return JsonConvert.SerializeObject(info, Formatting.Indented);
        }

        /// <summary>
        /// 获取总物理内存
        /// </summary>
        private long GetTotalPhysicalMemory()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToInt64(obj["TotalPhysicalMemory"]);
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取可用物理内存（单位：KB）
        /// 参考：Microsoft Learn - WMI Tasks: Computer Hardware
        /// </summary>
        private long GetAvailablePhysicalMemory()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToInt64(obj["FreePhysicalMemory"]);
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取 GPU 信息
        /// </summary>
        private string GetGPUInfo()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_VideoController"))
                {
                    var gpus = new List<string>();
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        gpus.Add(obj["Name"]?.ToString() ?? "Unknown");
                    }
                    return string.Join(", ", gpus);
                }
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// 截图
        /// </summary>
        public string TakeScreenShot()
        {
            try
            {
                // 获取主屏幕尺寸
                Rectangle bounds = Screen.PrimaryScreen.Bounds;

                // 创建位图
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    // 绘制屏幕到位图
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    }

                    // 转换为 Base64
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        return Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Screenshot failed: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取系统背景图
        /// </summary>
        public string GetDesktopWallpaper()
        {
            try
            {
                string wallpaperPath = new string('\0', 260);
                SystemParametersInfo(SPI_GETDESKWALLPAPER, 260, wallpaperPath, 0);
                wallpaperPath = wallpaperPath.TrimEnd('\0');

                if (File.Exists(wallpaperPath))
                {
                    byte[] imageBytes = File.ReadAllBytes(wallpaperPath);
                    return Convert.ToBase64String(imageBytes);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 模拟按键
        /// </summary>
        public void SendKey(params string[] keys)
        {
            foreach (var key in keys)
            {
                byte keyCode = GetKeyCode(key);
                if (keyCode != 0)
                {
                    // 按下键
                    keybd_event(keyCode, 0, 0, 0);
                    // 释放键
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0);
                }
            }
        }

        /// <summary>
        /// 获取按键代码
        /// </summary>
        private byte GetKeyCode(string key)
        {
            // 虚拟键码映射
            var keyMap = new Dictionary<string, byte>(StringComparer.OrdinalIgnoreCase)
            {
                { "A", 0x41 }, { "B", 0x42 }, { "C", 0x43 }, { "D", 0x44 },
                { "E", 0x45 }, { "F", 0x46 }, { "G", 0x47 }, { "H", 0x48 },
                { "I", 0x49 }, { "J", 0x4A }, { "K", 0x4B }, { "L", 0x4C },
                { "M", 0x4D }, { "N", 0x4E }, { "O", 0x4F }, { "P", 0x50 },
                { "Q", 0x51 }, { "R", 0x52 }, { "S", 0x53 }, { "T", 0x54 },
                { "U", 0x55 }, { "V", 0x56 }, { "W", 0x57 }, { "X", 0x58 },
                { "Y", 0x59 }, { "Z", 0x5A },
                { "0", 0x30 }, { "1", 0x31 }, { "2", 0x32 }, { "3", 0x33 },
                { "4", 0x34 }, { "5", 0x35 }, { "6", 0x36 }, { "7", 0x37 },
                { "8", 0x38 }, { "9", 0x39 },
                { "ENTER", 0x0D }, { "RETURN", 0x0D }, { "TAB", 0x09 },
                { "SPACE", 0x20 }, { "BACKSPACE", 0x08 }, { "DELETE", 0x2E },
                { "ESC", 0x1B }, { "ESCAPE", 0x1B }, { "F1", 0x70 },
                { "F2", 0x71 }, { "F3", 0x72 }, { "F4", 0x73 }, { "F5", 0x74 },
                { "F6", 0x75 }, { "F7", 0x76 }, { "F8", 0x77 }, { "F9", 0x78 },
                { "F10", 0x79 }, { "F11", 0x7A }, { "F12", 0x7B },
                { "LEFT", 0x25 }, { "UP", 0x26 }, { "RIGHT", 0x27 }, { "DOWN", 0x28 }
            };

            if (keyMap.TryGetValue(key, out byte code))
            {
                return code;
            }

            return 0;
        }

        /// <summary>
        /// 模拟鼠标点击
        /// </summary>
        public void SendMouseClick(int x, int y, string button = "left")
        {
            uint flags = 0;

            switch (button.ToLower())
            {
                case "left":
                    flags = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
                    break;
                case "right":
                    flags = MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP;
                    break;
                case "middle":
                    flags = MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP;
                    break;
                default:
                    flags = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
                    break;
            }

            mouse_event(flags, (uint)x, (uint)y, 0, 0);
        }

        /// <summary>
        /// 获取鼠标坐标
        /// </summary>
        public string GetMousePosition()
        {
            POINT point;
            GetCursorPos(out point);

            var position = new
            {
                X = point.X,
                Y = point.Y
            };

            return JsonConvert.SerializeObject(position);
        }
    }
}