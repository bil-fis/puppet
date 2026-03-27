using System;
using System.Net;
using System.Net.Sockets;

namespace puppet
{
    /// <summary>
    /// 端口选择工具类
    /// 用于自动选择可用的端口
    /// 参考 Microsoft Learn 文档：https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener
    /// </summary>
    public static class PortSelector
    {
        /// <summary>
        /// 检查端口是否可用
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>是否可用</returns>
        public static bool IsPortAvailable(int port)
        {
            try
            {
                using (var listener = new TcpListener(IPAddress.Loopback, port))
                {
                    listener.Start();
                    listener.Stop();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取一个可用的端口
        /// </summary>
        /// <param name="startPort">起始端口</param>
        /// <returns>可用的端口号</returns>
        public static int GetAvailablePort(int startPort = 7738)
        {
            int port = startPort;
            int maxAttempts = 100;

            for (int i = 0; i < maxAttempts; i++)
            {
                if (IsPortAvailable(port))
                {
                    return port;
                }
                port++;
            }

            throw new Exception($"No available port found starting from {startPort}");
        }

        /// <summary>
        /// 获取下一个可用的端口
        /// </summary>
        /// <param name="startPort">起始端口</param>
        ///param name="currentPort">当前端口</param>
        /// <returns>下一个可用的端口号</returns>
        public static int GetNextAvailablePort(int startPort, int currentPort)
        {
            int port = Math.Max(startPort, currentPort + 1);
            return GetAvailablePort(port);
        }
    }
}