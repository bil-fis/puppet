using System;
using System.Drawing;
using System.Windows.Forms;

namespace puppet
{
    /// <summary>
    /// 权限确认对话框
    /// 用于在执行危险操作前确认用户意图
    /// </summary>
    public class PermissionDialog : Form
    {
        private Label _messageLabel;
        private Label _pathLabel;
        private Button _allowButton;
        private Button _denyButton;
        private Button _blockButton;
        private CheckBox _rememberCheckBox;
        private bool _allowAction = false;
        private bool _blockAction = false;

        public PermissionDialog(string title, string message, string? path = null)
        {
            InitializeComponents(title, message, path);
        }

        private void InitializeComponents(string title, string message, string? path)
        {
            this.Text = title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(450, 180);
            this.BackColor = Color.White;

            // 消息标签
            _messageLabel = new Label
            {
                Text = message,
                Location = new Point(20, 20),
                Size = new Size(410, 40),
                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 50, 50)
            };
            this.Controls.Add(_messageLabel);

            // 路径标签
            if (!string.IsNullOrEmpty(path))
            {
                _pathLabel = new Label
                {
                    Text = $"路径: {path}",
                    Location = new Point(20, 65),
                    Size = new Size(410, 20),
                    Font = new Font("Consolas", 9F),
                    ForeColor = Color.FromArgb(80, 80, 80)
                };
                this.Controls.Add(_pathLabel);
            }

            // 记住选择复选框
            _rememberCheckBox = new CheckBox
            {
                Text = "记住此选择（仅本次会话）",
                Location = new Point(20, 95),
                Size = new Size(200, 20),
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            this.Controls.Add(_rememberCheckBox);

            // 允许按钮
            _allowButton = new Button
            {
                Text = "允许",
                Location = new Point(20, 125),
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            _allowButton.Click += (s, e) =>
            {
                _allowAction = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            this.Controls.Add(_allowButton);

            // 拒绝按钮
            _denyButton = new Button
            {
                Text = "拒绝",
                Location = new Point(130, 125),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Cancel,
                BackColor = Color.FromArgb(255, 255, 255),
                ForeColor = Color.FromArgb(80, 80, 80),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            _denyButton.Click += (s, e) =>
            {
                _allowAction = false;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            this.Controls.Add(_denyButton);

            // 永久阻止按钮
            _blockButton = new Button
            {
                Text = "永久阻止",
                Location = new Point(240, 125),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Abort,
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            _blockButton.Click += (s, e) =>
            {
                _allowAction = false;
                _blockAction = true;
                this.DialogResult = DialogResult.Abort;
                this.Close();
            };
            this.Controls.Add(_blockButton);
        }

        /// <summary>
        /// 显示权限确认对话框
        /// </summary>
        /// <returns>
        /// DialogResult.OK - 允许
        /// DialogResult.Cancel - 拒绝
        /// DialogResult.Abort - 永久阻止
        /// </returns>
        public static DialogResult ShowPermissionDialog(string title, string message, string? path = null)
        {
            using (var dialog = new PermissionDialog(title, message, path))
            {
                return dialog.ShowDialog();
            }
        }

        /// <summary>
        /// 是否允许操作
        /// </summary>
        public bool AllowAction => _allowAction;

        /// <summary>
        /// 是否永久阻止
        /// </summary>
        public bool BlockAction => _blockAction;

        /// <summary>
        /// 是否记住选择
        /// </summary>
        public bool RememberChoice => _rememberCheckBox.Checked;
    }
}