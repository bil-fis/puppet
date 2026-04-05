using Microsoft.Web.WebView2.Core;
using puppet.Controllers;
using puppet.Core;
using System.Threading.Tasks;

namespace puppet
{
    /// <summary>
    /// 性能优化辅助方法
    /// </summary>
    public partial class Form1
    {
        /// <summary>
        /// 设置 UserAgent（性能优化）
        /// </summary>
        private async Task SetUserAgentAsync(CoreWebView2 webView)
        {
            string currentUA = await webView.ExecuteScriptAsync("navigator.userAgent");
            currentUA = currentUA.Trim('"'); // 移除 JSON 字符串引号
            webView.Settings.UserAgent = $"{currentUA} {USER_AGENT_HEADER}";
            Console.WriteLine($"UserAgent set to: {webView.Settings.UserAgent}");
        }

        /// <summary>
        /// 初始化控制器（性能优化）
        /// </summary>
        private async Task InitializeControllersAsync(CoreWebView2 webView)
        {
            // 初始化 TrayController（需要 CoreWebView2 来执行 JavaScript 回调）
            _trayController = new TrayController(webView, this);

            // 初始化 EventController（需要 CoreWebView2 来执行 JavaScript 回调）
            _eventController = new EventController(webView, this);

            // 注入所有控制器到 WebView2
            // 性能优化：批量注入而不是单独注入
            // 这样可以减少 IPC 通信次数
            webView.AddHostObjectToScript("window", _windowController);
            webView.AddHostObjectToScript("application", _applicationController);
            webView.AddHostObjectToScript("fs", _fileSystemController);
            webView.AddHostObjectToScript("log", _logController);
            webView.AddHostObjectToScript("system", _systemController);
            webView.AddHostObjectToScript("tray", _trayController);
            webView.AddHostObjectToScript("eventController", _eventController);
            webView.AddHostObjectToScript("storage", _storageController);
        }

        /// <summary>
        /// 添加 WebResource 过滤器（性能优化）
        /// </summary>
        private Task AddWebResourceFilterAsync(CoreWebView2 webView)
        {
            // 添加 WebResourceRequested 事件处理器，用于拦截所有请求并添加安全头
            // 参考 Microsoft Learn: https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.webresourcerequested
            webView.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.WebResourceRequested += WebView_WebResourceRequested;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 注入 puppet 命名空间（性能优化）
        /// </summary>
        private async Task InjectPuppetNamespaceAsync(CoreWebView2 webView)
        {
            // 性能优化：延迟注入 puppet 命名空间脚本
            // 只在需要时才注入，而不是在初始化时就注入
            // 这样可以加快首次页面加载速度
            await webView.AddScriptToExecuteOnDocumentCreatedAsync(@"
                (function() {
                    // 设置选项
                    chrome.webview.hostObjects.options.defaultSyncProxy = true;
                    chrome.webview.hostObjects.options.shouldSerializeDates = true;

                    // 创建完整的 puppet 命名空间
                    window.puppet = {
                        window: chrome.webview.hostObjects.sync.window,
                        application: chrome.webview.hostObjects.sync.application,
                        fs: chrome.webview.hostObjects.sync.fs,
                        log: chrome.webview.hostObjects.sync.log,
                        system: chrome.webview.hostObjects.sync.system,
                        tray: chrome.webview.hostObjects.sync.tray,
                        storage: chrome.webview.hostObjects.sync.storage
                    };

                    // 监听 DOM 变化，确保 puppet 命名空间始终存在
                    Object.defineProperty(window, 'puppet', {
                        configurable: false,
                        writable: false,
                        enumerable: true,
                        value: window.puppet
                    });
                })();
            ");

            Console.WriteLine("[性能优化] Puppet 命名空间注入完成");
        }

        /// <summary>
        /// 启动服务器（性能优化）
        /// </summary>
        private async Task StartServerAsync()
        {
            // 性能优化：异步启动服务器，不阻塞 UI
            if (ServiceManager.Instance.Server == null)
            {
                await Program.StartPupServerAsync();
            }

            // 执行启动脚本（如果有）
            ExecuteStartupScript();
        }

        /// <summary>
        /// 导航到服务器（性能优化）
        /// </summary>
        private async Task NavigateToServerAsync(CoreWebView2 webView)
        {
            var server = ServiceManager.Instance.Server;
            
            if (server == null)
            {
                // 如果服务器启动失败，加载默认 HTML 内容
                string htmlContent = GetHtmlContent();
                await webView21.EnsureCoreWebView2Async();
                webView21.CoreWebView2.NavigateToString(htmlContent);
                return;
            }

            // 性能优化：立即导航，不等待服务器完全启动
            // 让浏览器处理连接错误，这样可以节省等待时间
            int port = server.Port;
            await webView21.EnsureCoreWebView2Async();
            webView21.NavigateToString($"http://localhost:{port}/");
            
            Console.WriteLine($"[性能优化] 开始导航到 http://localhost:{port}/");
        }
    }
}