using Microsoft.Web.WebView2.Core;
using puppet.Controllers;
using System.Runtime.InteropServices;

namespace puppet
{
    public partial class Form1 : Form
    {
        // 控制器实例
        private WindowController _windowController;
        private ApplicationController _applicationController;
        private FileSystemController _fileSystemController;
        private LogController _logController;
        private SystemController _systemController;

        // 窗口拖动相关
        private bool _isDraggable = false;
        private bool _isMouseDown = false;
        private Point _mouseOffset;

        // 鼠标穿透相关
        private bool _mouseThrough = false;
        private bool _mouseThroughTransparency = false;

        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "0x00000000");

            // 初始化控制器
            _windowController = new WindowController(this);
            _applicationController = new ApplicationController(this);
            _fileSystemController = new FileSystemController();
            _logController = new LogController();
            _systemController = new SystemController();

            // 初始化 WebView2
            webView21.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            webView21.NavigationStarting += WebView_NavigationStarting;
            webView21.Source = new Uri("about:blank");

            // 绑定鼠标事件用于拖动
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
        }

        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // 设置背景完全透明
                webView21.DefaultBackgroundColor = Color.Transparent;

                // 注入所有控制器到 WebView2
                webView21.CoreWebView2.AddHostObjectToScript("window", _windowController);
                webView21.CoreWebView2.AddHostObjectToScript("application", _applicationController);
                webView21.CoreWebView2.AddHostObjectToScript("fs", _fileSystemController);
                webView21.CoreWebView2.AddHostObjectToScript("log", _logController);
                webView21.CoreWebView2.AddHostObjectToScript("system", _systemController);

                // 注入 JavaScript 辅助函数，创建 puppet 命名空间
                string initScript = GetInitScript();
                await webView21.CoreWebView2.ExecuteScriptAsync(initScript);

                // 加载 HTML 内容
                string htmlContent = GetHtmlContent();
                await webView21.EnsureCoreWebView2Async();
                webView21.CoreWebView2.NavigateToString(htmlContent);
            }
            else
            {
                MessageBox.Show("WebView2 初始化失败: " + e.ToString());
            }
        }

        private void WebView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            // 在导航开始前，确保新页面的背景也是透明的
        }

        private string GetInitScript()
        {
            return @"
                // 创建 puppet 命名空间
                window.puppet = {
                    window: chrome.webview.hostObjects.window,
                    Application: chrome.webview.hostObjects.application,
                    fs: chrome.webview.hostObjects.fs,
                    log: chrome.webview.hostObjects.log,
                    System: chrome.webview.hostObjects.system
                };

                // 添加错误处理包装器
                window.puppet.wrapFunction = async function(obj, methodName, ...args) {
                    try {
                        const result = await obj[methodName](...args);
                        return result;
                    } catch (error) {
                        console.error('Error calling', methodName, ':', error);
                        throw error;
                    }
                };

                // 包装所有方法
                ['setBorderless', 'setDraggable', 'setResizable', 'setTransparent', 
                 'setOpacity', 'setMouseThroughTransparency', 'setMouseThrough', 
                 'setTransparentColor', 'setTopmost', 'moveWindow', 'resizeWindow', 'centerWindow'].forEach(method => {
                    window.puppet.window[method] = async function(...args) {
                        return await window.puppet.wrapFunction(window.puppet.window, method, ...args);
                    };
                });

                ['close', 'restart', 'getWindowInfo', 'execute', 'setConfig', 
                 'getAssemblyDirectory', 'getAppDataDirectory', 'getCurrentUser'].forEach(method => {
                    window.puppet.Application[method] = async function(...args) {
                        return await window.puppet.wrapFunction(window.puppet.Application, method, ...args);
                    };
                });

                ['openFileDialog', 'openFolderDialog', 'readFileAsByte', 'readFileAsJson', 
                 'writeByteToFile', 'writeTextToFile', 'appendByteToFile', 'appendTextToFile', 
                 'exists', 'delete'].forEach(method => {
                    window.puppet.fs[method] = async function(...args) {
                        return await window.puppet.wrapFunction(window.puppet.fs, method, ...args);
                    };
                });

                ['info', 'warn', 'error'].forEach(method => {
                    window.puppet.log[method] = async function(...args) {
                        return await window.puppet.wrapFunction(window.puppet.log, method, ...args);
                    };
                });

                ['getSystemInfo', 'takeScreenShot', 'getDesktopWallpaper', 
                 'sendKey', 'sendMouseClick', 'getMousePosition'].forEach(method => {
                    window.puppet.System[method] = async function(...args) {
                        return await window.puppet.wrapFunction(window.puppet.System, method, ...args);
                    };
                });
            ";
        }

        private string GetHtmlContent()
        {
            return @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <style>
                    html, body {
                        margin: 0;
                        padding: 0;
                        width: 100%;
                        height: 100%;
                        background-color: transparent;
                        font-family: Arial, sans-serif;
                        color: white;
                    }
                    .container {
                        padding: 20px;
                    }
                    .div1 {
                        width: 100%;
                        height: 150px;
                        background-color: rgba(255, 0, 0, 0.9);
                        color: white;
                        text-align: center;
                        line-height: 150px;
                        font-size: 24px;
                        border-radius: 10px;
                        margin-bottom: 20px;
                    }
                    .div2 {
                        width: 100%;
                        height: 150px;
                        background-color: rgba(0, 0, 255, 0.9);
                        color: white;
                        text-align: center;
                        line-height: 150px;
                        font-size: 24px;
                        border-radius: 10px;
                        margin-bottom: 20px;
                    }
                    .div1, .div2 {
                        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
                        cursor: pointer;
                        transition: transform 0.2s;
                    }
                    .div1:hover, .div2:hover {
                        transform: scale(1.02);
                    }
                    .info {
                        text-align: center;
                        margin-top: 20px;
                        color: white;
                        font-size: 14px;
                    }
                    .console {
                        background: rgba(0,0,0,0.8);
                        color: #0f0;
                        padding: 10px;
                        border-radius: 5px;
                        margin-top: 20px;
                        font-family: 'Courier New', monospace;
                        font-size: 12px;
                        max-height: 200px;
                        overflow-y: auto;
                    }
                    button {
                        background: rgba(255,255,255,0.2);
                        border: 1px solid rgba(255,255,255,0.4);
                        color: white;
                        padding: 10px 20px;
                        margin: 5px;
                        border-radius: 5px;
                        cursor: pointer;
                        font-size: 14px;
                    }
                    button:hover {
                        background: rgba(255,255,255,0.3);
                    }
                    .buttons {
                        display: flex;
                        flex-wrap: wrap;
                        justify-content: center;
                        gap: 10px;
                        margin-top: 20px;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='div1' onclick='testWindowAPI()'>测试窗口 API (点击)</div>
                    <div class='div2' onclick='testSystemAPI()'>测试系统 API (点击)</div>
                    
                    <div class='buttons'>
                        <button onclick='testBorderless()'>无边框切换</button>
                        <button onclick='testOpacity()'>透明度 0.7</button>
                        <button onclick='testTopmost()'>置顶切换</button>
                        <button onclick='testCenter()'>居中窗口</button>
                        <button onclick='testLog()'>测试日志</button>
                        <button onclick='testGetWindowInfo()'>获取窗口信息</button>
                        <button onclick='testScreenshot()'>截图</button>
                    </div>

                    <div class='info'>Puppet WebView2 API 测试</div>
                    <div class='console' id='console'>等待操作...</div>
                </div>

                <script>
                    function log(message) {
                        const consoleDiv = document.getElementById('console');
                        consoleDiv.innerHTML += '<div>' + message + '</div>';
                        consoleDiv.scrollTop = consoleDiv.scrollHeight;
                    }

                    async function testBorderless() {
                        try {
                            await puppet.window.setBorderless(true);
                            log('无边框模式已启用');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testOpacity() {
                        try {
                            await puppet.window.setOpacity(0.7);
                            log('透明度已设置为 0.7');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testTopmost() {
                        try {
                            await puppet.window.setTopmost(true);
                            log('窗口已置顶');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testCenter() {
                        try {
                            await puppet.window.centerWindow();
                            log('窗口已居中');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testLog() {
                        try {
                            await puppet.log.info('这是一条信息日志');
                            await puppet.log.warn('这是一条警告日志');
                            await puppet.log.error('这是一条错误日志');
                            log('日志测试完成');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testGetWindowInfo() {
                        try {
                            const info = await puppet.Application.getWindowInfo();
                            log('窗口信息: ' + info);
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testWindowAPI() {
                        try {
                            await puppet.window.moveWindow(100, 100);
                            await puppet.window.resizeWindow(600, 400);
                            log('窗口 API 测试成功');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testSystemAPI() {
                        try {
                            const systemInfo = await puppet.System.getSystemInfo();
                            log('系统信息已获取');
                            const mousePos = await puppet.System.getMousePosition();
                            log('鼠标位置: ' + mousePos);
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testScreenshot() {
                        try {
                            const screenshot = await puppet.System.takeScreenShot();
                            log('截图成功，长度: ' + screenshot.length);
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }
                </script>
            </body>
            </html>";
        }

        // 窗口拖动相关方法
        public void SetDraggable(bool draggable)
        {
            _isDraggable = draggable;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isDraggable && e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;
                _mouseOffset = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown && _isDraggable)
            {
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-_mouseOffset.X, -_mouseOffset.Y);
                this.Location = newLocation;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

        // 鼠标穿透相关方法
        public void SetMouseThrough(bool enabled)
        {
            _mouseThrough = enabled;
            if (enabled)
            {
                this.WindowExStyle |= WindowExStyle.Transparent;
            }
            else
            {
                this.WindowExStyle &= ~WindowExStyle.Transparent;
            }
        }

        public void SetMouseThroughTransparency(bool enabled)
        {
            _mouseThroughTransparency = enabled;
        }

        // 窗口扩展样式
        private WindowExStyle WindowExStyle
        {
            get
            {
                return (WindowExStyle)NativeMethods.GetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE);
            }
            set
            {
                NativeMethods.SetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE, (int)value);
            }
        }
    }

    #region Win32 API 和枚举

    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }

    internal enum GetWindowLongIndex
    {
        GWL_EXSTYLE = -20
    }

    [Flags]
    internal enum WindowExStyle : uint
    {
        Transparent = 0x00000020
    }

    #endregion
}
 