using Microsoft.Web.WebView2.Core;

namespace puppet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "0x00000000");

            // 初始化 WebView2
            webView21.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            webView21.NavigationStarting += WebView_NavigationStarting; // 可选：防止跳转时白屏
            webView21.Source = new Uri("about:blank"); // 先加载空白页，确保初始化
        }

        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // 设置背景完全透明
                webView21.DefaultBackgroundColor = Color.Transparent; // 等效于 Color.FromArgb(0,0,0,0)

                // 加载包含两个 div 的 HTML 内容
                string htmlContent = GetHtmlContent();
                // 使用 NavigateToString 直接加载 HTML
                await webView21.EnsureCoreWebView2Async(); // 确保 CoreWebView2 已就绪
                webView21.CoreWebView2.NavigateToString(htmlContent);
            }
            else
            {
                MessageBox.Show("WebView2 初始化失败: " + e.ToString());
            }
        }

        private void WebView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            // 可选：在导航开始前，确保新页面的背景也是透明的
            // 但这里我们只用 NavigateToString，所以无需额外操作
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
                        background-color: transparent; /* 透明背景关键 */
                    }
                    .div1 {
                        width: 100%;
                        height: 150px;
                        background-color: rgba(255, 0, 0, 0.9);
                        color: white;
                        text-align: center;
                        line-height: 150px;
                        font-size: 24px;
                        font-family: Arial, sans-serif;
                    }
                    .div2 {
                        width: 100%;
                        height: 150px;
                        background-color: rgba(0, 0, 255, 0.9);
                        color: white;
                        text-align: center;
                        line-height: 150px;
                        font-size: 24px;
                        font-family: Arial, sans-serif;
                        margin-top: 50px; /* 这 50px 的间隙将是透明的 */
                    }
                    /* 可选：添加一些阴影效果，增强层次感 */
                    .div1, .div2 {
                        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
                    }
                </style>
            </head>
            <body>
                <div class='div1'>上面的 Div</div>
                <div class='div2'>下面的 Div</div>
                <div style='text-align:center; margin-top:20px; color:white; font-family:Arial;'>
                    (中间区域透明，可见下方窗口内容)
                </div>
            </body>
            </html>";
        }
    }
}
 