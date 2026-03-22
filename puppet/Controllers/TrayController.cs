using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.Core;
using System.Runtime.InteropServices;

namespace puppet.Controllers
{
    /// <summary>
    /// 托盘图标控制器
    /// 基于 Microsoft Learn 官方文档实现
    /// 参考：https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/notifyicon-component-overview-windows-forms
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class TrayController
    {
        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _contextMenu;
        private CoreWebView2 _webview;
        private Dictionary<string, string> _menuCommands = new Dictionary<string, string>();
        private string _onClickCallback;
        private string _onDoubleClickCallback;

        public TrayController(CoreWebView2 webview)
        {
            _webview = webview;
            InitializeNotifyIcon();
        }

        /// <summary>
        /// 初始化 NotifyIcon 组件
        /// 参考：https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/notifyicon-component-overview-windows-forms
        /// </summary>
        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Visible = false;

            // 设置默认图标（使用系统图标）
            _notifyIcon.Icon = SystemIcons.Application;

            // 设置事件处理
            _notifyIcon.Click += NotifyIcon_Click;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // 初始化右键菜单
            _contextMenu = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip = _contextMenu;
        }

        /// <summary>
        /// 创建或更新托盘图标
        /// </summary>
        /// <param name="name">托盘图标名称/文本</param>
        public void SetTray(string name)
        {
            if (_notifyIcon == null)
            {
                InitializeNotifyIcon();
            }

            // 设置托盘图标的文本（鼠标悬停时显示）
            _notifyIcon.Text = name;

            // 显示托盘图标
            _notifyIcon.Visible = true;
        }

        /// <summary>
        /// 设置托盘菜单
        /// 参考：https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-associate-a-shortcut-menu-with-a-windows-forms-notifyicon-component
        /// </summary>
        /// <param name="menuJson">菜单项的 JSON 数组，格式：[{Text:"菜单项", Command:"回调函数名"}, ...]</param>
        public void SetMenu(string menuJson)
        {
            try
            {
                // 清空现有菜单
                _contextMenu.Items.Clear();
                _menuCommands.Clear();

                // 解析 JSON
                var menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(menuJson);

                if (menuItems != null)
                {
                    foreach (var item in menuItems)
                    {
                        var menuItem = new ToolStripMenuItem(item.Text);

                        if (!string.IsNullOrEmpty(item.Command))
                        {
                            // 存储命令和菜单项的映射
                            _menuCommands[item.Text] = item.Command;

                            // 添加点击事件
                            menuItem.Click += async (sender, e) =>
                            {
                                await ExecuteCommand(item.Command);
                            };
                        }

                        _contextMenu.Items.Add(menuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Invalid menu JSON format: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示气泡提示
        /// 参考：https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon.showballoontip
        /// </summary>
        /// <param name="title">气泡标题</param>
        /// <param name="content">气泡内容</param>
        /// <param name="timeout">显示时长（毫秒），默认 30000</param>
        /// <param name="icon">图标类型：None, Info, Warning, Error，默认 Info</param>
        public void ShowBalloon(string title, string content, int timeout = 30000, string icon = "Info")
        {
            if (_notifyIcon == null || !_notifyIcon.Visible)
            {
                throw new InvalidOperationException("Tray icon is not initialized. Call setTray() first.");
            }

            // 解析图标类型
            ToolTipIcon toolTipIcon = ToolTipIcon.Info;
            if (!string.IsNullOrEmpty(icon))
            {
                if (Enum.TryParse(icon, true, out ToolTipIcon parsedIcon))
                {
                    toolTipIcon = parsedIcon;
                }
            }

            // 显示气泡提示
            _notifyIcon.ShowBalloonTip(timeout, title, content, toolTipIcon);
        }

        /// <summary>
        /// 设置点击事件回调
        /// </summary>
        /// <param name="callbackName">JavaScript 回调函数名</param>
        public void OnClick(string callbackName)
        {
            _onClickCallback = callbackName;
        }

        /// <summary>
        /// 设置双击事件回调
        /// </summary>
        /// <param name="callbackName">JavaScript 回调函数名</param>
        public void OnDoubleClick(string callbackName)
        {
            _onDoubleClickCallback = callbackName;
        }

        /// <summary>
        /// 隐藏托盘图标
        /// </summary>
        public void Hide()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }
        }

        /// <summary>
        /// 显示托盘图标
        /// </summary>
        public void Show()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// 设置托盘图标
        /// </summary>
        /// <param name="iconPath">图标文件路径</param>
        public void SetIcon(string iconPath)
        {
            if (_notifyIcon == null)
            {
                InitializeNotifyIcon();
            }

            try
            {
                _notifyIcon.Icon = new Icon(iconPath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to load icon from '{iconPath}': {ex.Message}");
            }
        }

        /// <summary>
        /// NotifyIcon 单击事件处理
        /// 参考：https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon.click
        /// </summary>
        private async void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_onClickCallback))
            {
                await ExecuteCommand(_onClickCallback);
            }
        }

        /// <summary>
        /// NotifyIcon 双击事件处理
        /// 参考：https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon.mousedoubleclick
        /// </summary>
        private async void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_onDoubleClickCallback))
            {
                await ExecuteCommand(_onDoubleClickCallback);
            }
        }

        /// <summary>
        /// 执行 JavaScript 命令
        /// 参考：https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.executescriptasync
        /// </summary>
        /// <param name="command">JavaScript 函数名</param>
        private async System.Threading.Tasks.Task ExecuteCommand(string command)
        {
            try
            {
                if (_webview != null)
                {
                    // 执行 JavaScript 函数
                    await _webview.ExecuteScriptAsync($"if (typeof {command} === 'function') {{ {command}(); }}");
                }
            }
            catch (Exception ex)
            {
                // 静默失败，避免影响用户体验
                System.Diagnostics.Debug.WriteLine($"Failed to execute command '{command}': {ex.Message}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }

            if (_contextMenu != null)
            {
                _contextMenu.Dispose();
                _contextMenu = null;
            }

            _menuCommands.Clear();
        }

        /// <summary>
        /// 菜单项类（用于 JSON 反序列化）
        /// </summary>
        private class MenuItem
        {
            public string Text { get; set; }
            public string Command { get; set; }
        }
    }
}