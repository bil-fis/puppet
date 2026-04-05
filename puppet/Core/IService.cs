using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace puppet.Core
{
    /// <summary>
    /// 服务器接口
    /// 定义 PUP 服务器的核心功能，实现解耦和可测试性
    /// 参考 Microsoft Learn 设计原则：https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// 获取服务器端口号
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 获取 PUP 版本
        /// </summary>
        string PupVersion { get; }

        /// <summary>
        /// 获取启动脚本内容（V1.1和V1.2版本）
        /// </summary>
        string? StartupScript { get; }

        /// <summary>
        /// 获取证书（V1.2版本）
        /// </summary>
        X509Certificate2? Certificate { get; }

        /// <summary>
        /// 获取是否包含签名（V1.2版本）
        /// </summary>
        bool HasSignature { get; }

        /// <summary>
        /// 启动服务器
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// 停止服务器
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// 获取服务器是否正在运行
        /// </summary>
        bool IsRunning { get; }
    }

    /// <summary>
    /// 窗口接口
    /// 定义窗口的抽象操作，实现解耦和可测试性
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// 异步执行操作在 UI 线程上
        /// </summary>
        void InvokeOnUIThread(Action action);

        /// <summary>
        /// 异步执行操作在 UI 线程上（带返回值）
        /// </summary>
        T InvokeOnUIThread<T>(Func<T> action);

        /// <summary>
        /// 检查是否需要调用 Invoke
        /// </summary>
        bool InvokeRequired { get; }
    }

    /// <summary>
    /// 服务生命周期接口
    /// 定义服务启动和停止的标准接口
    /// </summary>
    public interface IServiceLifecycle
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// 停止服务
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// 获取服务是否正在运行
        /// </summary>
        bool IsRunning { get; }
    }
}