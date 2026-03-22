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

        // 重写 CreateParams 以使用 WS_EX_NOREDIRECTIONBITMAP
        // 根据 Microsoft 文档，这样可以避免不透明的重定向表面
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // 使用 WS_EX_NOREDIRECTIONBITMAP 创建无重定向表面的窗口
                // 这允许 DirectComposition 直接控制窗口渲染
                cp.ExStyle |= 0x00200000; // WS_EX_NOREDIRECTIONBITMAP
                return cp;
            }
        }

        public Form1()
        {
            InitializeComponent();

            // 设置窗口标题栏和图标
            this.BackColor = SystemColors.Control;
            this.FormBorderStyle = FormBorderStyle.Sizable; // 有边框和标题栏
            this.Text = "Puppet"; // 设置窗口标题
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); // 设置窗口图标

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

            // 设置窗口为分层窗口
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
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

                // 加载 HTML 内容（包含初始化脚本）
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

        // 重写 WndProc 处理智能点击穿透
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;
            const int HTCLIENT = 1;

            base.WndProc(ref m);

            // 如果启用了透明模式且不是完全穿透模式，实现智能点击穿透
            if (m.Msg == WM_NCHITTEST && _isTransparent && !_mouseThrough)
            {
                // 获取鼠标位置
                Point cursorPos = new Point(m.LParam.ToInt32() & 0xFFFF, (m.LParam.ToInt32() >> 16) & 0xFFFF);
                Point clientPos = this.PointToClient(cursorPos);

                // 检查点击是否在 WebView2 控件区域内
                if (!IsPointInWebView(clientPos))
                {
                    // 点击不在 WebView2 区域内，穿透到下层窗口
                    m.Result = (IntPtr)HTTRANSPARENT;
                }
            }
        }

        // 检查点是否在 WebView2 控件内
        private bool IsPointInWebView(Point point)
        {
            return webView2Bounds.Contains(point);
        }

        // WebView2 控件的边界
        private Rectangle webView2Bounds
        {
            get
            {
                return new Rectangle(webView21.Location, webView21.Size);
            }
        }

        private string GetInitScript()
        {
            return @"
                // 创建 puppet 命名空间，使用包装函数确保方法名不区分大小写
                window.puppet = {
                    window: chrome.webview.hostObjects.sync.window,
                    Application: chrome.webview.hostObjects.sync.application,
                    fs: chrome.webview.hostObjects.sync.fs,
                    log: chrome.webview.hostObjects.sync.log,
                    System: chrome.webview.hostObjects.sync.system
                };

                // 设置选项
                chrome.webview.hostObjects.options.defaultSyncProxy = true;
                chrome.webview.hostObjects.options.shouldSerializeDates = true;

                // 创建全局 puppet 对象，方法名不区分大小写
                window.puppet.window = {
                    setBorderless: async function(v) { return await chrome.webview.hostObjects.sync.window.SetBorderless(!!v); },
                    setDraggable: async function(v) { return await chrome.webview.hostObjects.sync.window.SetDraggable(!!v); },
                    setResizable: async function(v) { return await chrome.webview.hostObjects.sync.window.SetResizable(!!v); },
                    setTransparent: async function(v) { return await chrome.webview.hostObjects.sync.window.SetTransparent(!!v); },
                    setOpacity: async function(v) { return await chrome.webview.hostObjects.sync.window.SetOpacity(Number(v)); },
                    setMouseThroughTransparency: async function(v) { return await chrome.webview.hostObjects.sync.window.SetMouseThroughTransparency(!!v); },
                    setMouseThrough: async function(v) { return await chrome.webview.hostObjects.sync.window.SetMouseThrough(!!v); },
                    setTransparentColor: async function(c) { return await chrome.webview.hostObjects.sync.window.SetTransparentColor(String(c)); },
                    setTopmost: async function(v) { return await chrome.webview.hostObjects.sync.window.SetTopmost(!!v); },
                    moveWindow: async function(x, y) { return await chrome.webview.hostObjects.sync.window.MoveWindow(Number(x), Number(y)); },
                    resizeWindow: async function(w, h) { return await chrome.webview.hostObjects.sync.window.ResizeWindow(Number(w), Number(h)); },
                    centerWindow: async function() { return await chrome.webview.hostObjects.sync.window.CenterWindow(); }
                };

                window.puppet.Application = {
                    close: async function() { return await chrome.webview.hostObjects.sync.application.Close(); },
                    restart: async function() { return await chrome.webview.hostObjects.sync.application.Restart(); },
                    getWindowInfo: async function() { return await chrome.webview.hostObjects.sync.application.GetWindowInfo(); },
                    execute: async function(cmd) { return await chrome.webview.hostObjects.sync.application.Execute(String(cmd)); },
                    setConfig: async function(k, v) { return await chrome.webview.hostObjects.sync.application.SetConfig(String(k), String(v)); },
                    getAssemblyDirectory: async function() { return await chrome.webview.hostObjects.sync.application.GetAssemblyDirectory(); },
                    getAppDataDirectory: async function() { return await chrome.webview.hostObjects.sync.application.GetAppDataDirectory(); },
                    getCurrentUser: async function() { return await chrome.webview.hostObjects.sync.application.GetCurrentUser(); }
                };

                window.puppet.fs = {
                    openFileDialog: async function(f, m) { 
                        var filter = typeof f === 'string' ? f : JSON.stringify(f);
                        return await chrome.webview.hostObjects.sync.fs.OpenFileDialog(filter, !!m); 
                    },
                    openFolderDialog: async function() { return await chrome.webview.hostObjects.sync.fs.OpenFolderDialog(); },
                    readFileAsByte: async function(p) { return await chrome.webview.hostObjects.sync.fs.ReadFileAsByte(String(p)); },
                    readFileAsJson: async function(p) { return await chrome.webview.hostObjects.sync.fs.ReadFileAsJson(String(p)); },
                    writeByteToFile: async function(p, d) { return await chrome.webview.hostObjects.sync.fs.WriteByteToFile(String(p), d); },
                    writeTextToFile: async function(p, t) { return await chrome.webview.hostObjects.sync.fs.WriteTextToFile(String(p), String(t)); },
                    appendByteToFile: async function(p, d) { return await chrome.webview.hostObjects.sync.fs.AppendByteToFile(String(p), d); },
                    appendTextToFile: async function(p, t) { return await chrome.webview.hostObjects.sync.fs.AppendTextToFile(String(p), String(t)); },
                    exists: async function(p) { return await chrome.webview.hostObjects.sync.fs.Exists(String(p)); },
                    delete: async function(p) { return await chrome.webview.hostObjects.sync.fs.Delete(String(p)); }
                };

                window.puppet.log = {
                    info: async function(m) { return await chrome.webview.hostObjects.sync.log.Info('[info]: ' + String(m)); },
                    warn: async function(m) { return await chrome.webview.hostObjects.sync.log.Warn('[warn]: ' + String(m)); },
                    error: async function(m) { return await chrome.webview.hostObjects.sync.log.Error('[error]: ' + String(m)); }
                };

                window.puppet.System = {
                    getSystemInfo: async function() { return await chrome.webview.hostObjects.sync.system.GetSystemInfo(); },
                    takeScreenShot: async function() { return await chrome.webview.hostObjects.sync.system.TakeScreenShot(); },
                    getDesktopWallpaper: async function() { return await chrome.webview.hostObjects.sync.system.GetDesktopWallpaper(); },
                    sendKey: async function() { 
                        const args = Array.from(arguments).map(a => String(a));
                        return await chrome.webview.hostObjects.sync.system.SendKey.apply(null, args);
                    },
                    sendMouseClick: async function(x, y, b) { return await chrome.webview.hostObjects.sync.system.SendMouseClick(Number(x), Number(y), String(b || 'left')); },
                    getMousePosition: async function() { return await chrome.webview.hostObjects.sync.system.GetMousePosition(); }
                };
            ";
        }

        private string GetHtmlContent()
        {
            string initScript = GetInitScript();
            return @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <script>" + initScript + @"</script>
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
                        <button onclick='testTransparent()'>启用透明</button>
                        <button onclick='testOpacity()'>透明度 0.7</button>
                        <button onclick='testOpacityFull()'>透明度 1.0</button>
                        <button onclick='testTopmost()'>置顶切换</button>
                        <button onclick='testCenter()'>居中窗口</button>
                        <button onclick='testFullThrough()'>完全穿透</button>
                        <button onclick='testNormalMode()'>正常模式</button>
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

                    async function testFullThrough() {
                        try {
                            await puppet.window.setMouseThrough(true);
                            log('完全穿透模式已启用：整个窗口都会穿透到下层窗口');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testNormalMode() {
                        try {
                            await puppet.window.setMouseThrough(false);
                            log('正常模式已启用：窗口正常响应所有鼠标点击');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testTransparent() {
                        try {
                            await puppet.window.setTransparent(true);
                            log('透明模式已启用：窗口整体变为透明，透明区域可穿透');
                        } catch(e) {
                            log('错误: ' + e.message);
                        }
                    }

                    async function testOpacityFull() {
                        try {
                            await puppet.window.setOpacity(1.0);
                            log('透明度已设置为 1.0（完全不透明）');
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

            if (_mouseThrough)
            {
                // 完全穿透模式：整个窗口都穿透
                NativeMethods.SetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE,
                    NativeMethods.GetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE) | (int)WindowExStyle.Transparent);
            }
            else
            {
                // 正常模式：不穿透
                int currentStyle = NativeMethods.GetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE);
                NativeMethods.SetWindowLong(this.Handle, (int)GetWindowLongIndex.GWL_EXSTYLE,
                    currentStyle & ~(int)WindowExStyle.Transparent);
            }

            // 强制刷新窗口
            this.Invalidate();
            this.Update();
        }

        public void SetTransparent(bool transparent)
        {
            _isTransparent = transparent;

            if (transparent)
            {
                // 使用 Form.Opacity 实现透明度
                // 结合 WS_EX_NOREDIRECTIONBITMAP，可以实现真正的透明窗口
                // 且不会导致鼠标穿透
                this.Opacity = 0.9; // 默认透明度
                
                // 设置背景为黑色，配合透明度
                this.BackColor = Color.Black;
                
                // 确保 WebView2 背景是透明的
                if (webView21.IsHandleCreated)
                {
                    webView21.DefaultBackgroundColor = Color.Transparent;
                }
            }
            else
            {
                // 恢复正常
                this.BackColor = SystemColors.Control;
                this.Opacity = 1.0;
                
                // 强制刷新
                this.Invalidate();
                this.Update();
                webView21.Invalidate();
                webView21.Update();
            }
        }

        public void SetOpacity(double opacity)
        {
            _windowOpacity = Math.Max(0.1, Math.Min(1.0, opacity));
            
            // 直接使用 Form.Opacity 属性
            // 配合 WS_EX_NOREDIRECTIONBITMAP，可以实现真正的透明效果
            this.Opacity = _windowOpacity;
        }

        private bool _isTransparent = false;
        private double _windowOpacity = 1.0;

        // 移除透明穿透功能，因为 WebView2 的技术限制使得准确检测透明区域非常困难
        // 保留此方法以保持 API 兼容性，但不实现任何功能
        public void SetMouseThroughTransparency(bool enabled)
        {
            // 由于 WebView2 的渲染机制，无法可靠地实现透明区域检测
            // 建议用户只使用 setMouseThrough 来切换完全穿透和正常模式
            _logController.Warn("透明穿透功能不可用，请使用 setMouseThrough 切换完全穿透和正常模式");
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

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);
    }

    internal enum GetWindowLongIndex
    {
        GWL_EXSTYLE = -20
    }

    internal enum WindowExStyle : uint
    {
        Transparent = 0x00000020,
        Layered = 0x00080000
    }

    internal enum LayeredWindowAttributes
    {
        LWA_COLORKEY = 0x00000001,
        LWA_ALPHA = 0x00000002
    }

    #endregion
}
 