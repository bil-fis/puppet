using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace puppet.Core
{
    /// <summary>
    /// 服务管理器
    /// 提供统一的服务访问点，替代全局静态属性
    /// 参考 Microsoft Learn 依赖注入指南：https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/overview
    /// </summary>
    public class ServiceManager
    {
        private static readonly Lazy<ServiceManager> _instance = new Lazy<ServiceManager>(() => new ServiceManager());
        private readonly Dictionary<Type, object> _services;
        private readonly object _lock = new object();

        /// <summary>
        /// 获取 ServiceManager 的单例实例
        /// </summary>
        public static ServiceManager Instance => _instance.Value;

        /// <summary>
        /// 私有构造函数，确保单例模式
        /// </summary>
        private ServiceManager()
        {
            _services = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="service">服务实例</param>
        public void Register<TService>(TService service) where TService : class
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            lock (_lock)
            {
                _services[typeof(TService)] = service;
            }
        }

        /// <summary>
        /// 注册服务（使用接口类型）
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TService">服务实现类型</typeparam>
        /// <param name="service">服务实例</param>
        public void Register<TInterface, TService>(TService service) where TService : class, TInterface
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            lock (_lock)
            {
                _services[typeof(TInterface)] = service;
                _services[typeof(TService)] = service;
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <returns>服务实例，如果不存在则返回 null</returns>
        public TService? GetService<TService>() where TService : class
        {
            lock (_lock)
            {
                if (_services.TryGetValue(typeof(TService), out var service))
                {
                    return service as TService;
                }
                return null;
            }
        }

        /// <summary>
        /// 检查服务是否已注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <returns>如果服务已注册返回 true，否则返回 false</returns>
        public bool IsRegistered<TService>() where TService : class
        {
            lock (_lock)
            {
                return _services.ContainsKey(typeof(TService));
            }
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        public void Unregister<TService>() where TService : class
        {
            lock (_lock)
            {
                _services.Remove(typeof(TService));
            }
        }

        /// <summary>
        /// 清除所有服务
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _services.Clear();
            }
        }

        /// <summary>
        /// 获取服务器服务（便捷方法）
        /// </summary>
        public IServer? Server => GetService<IServer>();

        /// <summary>
        /// 获取窗口服务（便捷方法）
        /// </summary>
        public IWindow? Window => GetService<IWindow>();
    }
}