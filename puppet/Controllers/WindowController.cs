using System.Drawing;

namespace puppet.Controllers
{
    public class WindowController
    {
        private Form _form;

        public WindowController(Form form)
        {
            _form = form;
        }

        /// <summary>
        /// 是否启用无边框
        /// </summary>
        public void SetBorderless(bool borderless)
        {
            if (borderless)
            {
                _form.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                _form.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        /// <summary>
        /// 窗口是否可拖动
        /// </summary>
        public void SetDraggable(bool draggable)
        {
            // 在 WinForms 中，无边框窗口需要手动实现拖动功能
            // 这里我们标记一个状态，实际拖动逻辑在 Form 的 MouseDown 事件中处理
            if (_form is Form1 form1)
            {
                form1.SetDraggable(draggable);
            }
        }

        /// <summary>
        /// 窗口是否可缩放
        /// </summary>
        public void SetResizable(bool resizable)
        {
            if (resizable)
            {
                if (_form.FormBorderStyle == FormBorderStyle.None)
                {
                    _form.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                }
                else
                {
                    _form.FormBorderStyle = FormBorderStyle.Sizable;
                }
            }
            else
            {
                if (_form.FormBorderStyle == FormBorderStyle.None)
                {
                    // 保持无边框但不可缩放
                }
                else
                {
                    _form.FormBorderStyle = FormBorderStyle.FixedSingle;
                }
            }
        }

        /// <summary>
        /// 开启或关闭全局透明
        /// </summary>
        public void SetTransparent(bool transparent)
        {
            if (transparent)
            {
                _form.BackColor = Color.LimeGreen;
                _form.TransparencyKey = Color.LimeGreen;
            }
            else
            {
                _form.BackColor = SystemColors.Control;
                _form.TransparencyKey = Color.Empty;
            }
        }

        /// <summary>
        /// 设定窗口透明度 0-1
        /// </summary>
        public void SetOpacity(double opacity)
        {
            _form.Opacity = Math.Max(0.1, Math.Min(1.0, opacity));
        }

        /// <summary>
        /// 开启或关闭透明区域鼠标点击穿透
        /// </summary>
        public void SetMouseThroughTransparency(bool enabled)
        {
            // 需要结合透明色使用，当点击透明色区域时穿透
            if (_form is Form1 form1)
            {
                form1.SetMouseThroughTransparency(enabled);
            }
        }

        /// <summary>
        /// 开启或关闭整个窗口鼠标点击穿透
        /// </summary>
        public void SetMouseThrough(bool enabled)
        {
            if (_form is Form1 form1)
            {
                form1.SetMouseThrough(enabled);
            }
        }

        /// <summary>
        /// 设定某种颜色为透明色
        /// </summary>
        public void SetTransparentColor(string color)
        {
            try
            {
                Color transparentColor = ColorTranslator.FromHtml(color);
                _form.TransparencyKey = transparentColor;
            }
            catch
            {
                throw new ArgumentException("Invalid color format. Use hex format like '#RRGGBB' or '#AARRGGBB'");
            }
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        public void SetTopmost(bool topmost)
        {
            _form.TopMost = topmost;
        }

        /// <summary>
        /// 移动窗口到坐标位置
        /// </summary>
        public void MoveWindow(int x, int y)
        {
            _form.Location = new Point(x, y);
        }

        /// <summary>
        /// 设置窗口宽高
        /// </summary>
        public void ResizeWindow(int width, int height)
        {
            _form.Size = new Size(width, height);
        }

        /// <summary>
        /// 居中窗口
        /// </summary>
        public void CenterWindow()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            _form.Location = new Point(
                (screen.Width - _form.Width) / 2,
                (screen.Height - _form.Height) / 2
            );
        }
    }
}