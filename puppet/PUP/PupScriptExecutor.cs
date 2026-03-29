using System;
using System.Drawing;
using puppet.Controllers;

namespace puppet
{
    /// <summary>
    /// PUP 启动脚本执行器
    /// 用于解析和执行 PUP V1.1 的启动脚本
    /// </summary>
    public class PupScriptExecutor
    {
        private readonly Form1 _form;
        private readonly WindowController _windowController;

        public PupScriptExecutor(Form1 form, WindowController windowController)
        {
            _form = form;
            _windowController = windowController;
        }

        /// <summary>
        /// 执行启动脚本
        /// </summary>
        /// <param name="scriptContent">脚本内容</param>
        public void ExecuteScript(string scriptContent)
        {
            if (string.IsNullOrWhiteSpace(scriptContent))
            {
                Console.WriteLine("No startup script to execute");
                return;
            }

            Console.WriteLine("=== Executing PUP Startup Script ===");
            Console.WriteLine($"Script content:\n{scriptContent}");
            Console.WriteLine();

            string[] lines = scriptContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = 0;

            foreach (string line in lines)
            {
                lineNum++;
                string trimmedLine = line.Trim();

                // 跳过注释和空行
                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("//") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                try
                {
                    ExecuteCommand(trimmedLine, lineNum);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing line {lineNum}: {ex.Message}");
                    Console.WriteLine($"  Command: {trimmedLine}");
                }
            }

            Console.WriteLine("=== Script Execution Completed ===");
            Console.WriteLine();
        }

        /// <summary>
        /// 执行单个命令
        /// </summary>
        private void ExecuteCommand(string command, int lineNum)
        {
            Console.WriteLine($"[Line {lineNum}] Executing: {command}");

            // 解析命令
            string[] parts = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return;
            }

            // 检查是否是 set 命令
            if (parts[0].ToLower() != "set")
            {
                throw new Exception($"Unknown command: {parts[0]}. Expected 'set'");
            }

            if (parts.Length < 2)
            {
                throw new Exception("Missing property name after 'set'");
            }

            string property = parts[1].ToLower();
            string value = string.Join(" ", parts, 2, parts.Length - 2);

            switch (property)
            {
                case "startup_position":
                    SetStartupPosition(value);
                    break;

                case "borderless":
                    SetBorderless(value);
                    break;

                case "window_size":
                    SetWindowSize(value);
                    break;

                default:
                    throw new Exception($"Unknown property: {property}. Supported properties: startup_position, borderless, window_size");
            }
        }

        /// <summary>
        /// 设置启动位置
        /// 用法: set startup_position x,y 或 set startup_position POSITION
        /// POSITION 可以是: left-top, left-bottom, right-top, right-bottom, center
        /// </summary>
        private void SetStartupPosition(string value)
        {
            string trimmedValue = value.Trim();

            // 检查是否是预定义位置
            if (trimmedValue.Equals("left-top", StringComparison.OrdinalIgnoreCase))
            {
                _windowController.MoveWindow(0, 0);
                Console.WriteLine("  ✓ Startup position set to: left-top");
                return;
            }
            else if (trimmedValue.Equals("left-bottom", StringComparison.OrdinalIgnoreCase))
            {
                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                _windowController.MoveWindow(0, screen.Height - _form.Height);
                Console.WriteLine("  ✓ Startup position set to: left-bottom");
                return;
            }
            else if (trimmedValue.Equals("right-top", StringComparison.OrdinalIgnoreCase))
            {
                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                _windowController.MoveWindow(screen.Width - _form.Width, 0);
                Console.WriteLine("  ✓ Startup position set to: right-top");
                return;
            }
            else if (trimmedValue.Equals("right-bottom", StringComparison.OrdinalIgnoreCase))
            {
                Rectangle screen = Screen.PrimaryScreen.WorkingArea;
                _windowController.MoveWindow(screen.Width - _form.Width, screen.Height - _form.Height);
                Console.WriteLine("  ✓ Startup position set to: right-bottom");
                return;
            }
            else if (trimmedValue.Equals("center", StringComparison.OrdinalIgnoreCase))
            {
                _windowController.CenterWindow();
                Console.WriteLine("  ✓ Startup position set to: center");
                return;
            }

            // 尝试解析坐标
            string[] coords = trimmedValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (coords.Length == 2)
            {
                if (int.TryParse(coords[0].Trim(), out int x) && int.TryParse(coords[1].Trim(), out int y))
                {
                    _windowController.MoveWindow(x, y);
                    Console.WriteLine($"  ✓ Startup position set to: ({x}, {y})");
                    return;
                }
            }

            throw new Exception($"Invalid startup position value: {value}. Expected: x,y or one of [left-top, left-bottom, right-top, right-bottom, center]");
        }

        /// <summary>
        /// 设置是否无边框
        /// 用法: set borderless true 或 set borderless false
        /// </summary>
        private void SetBorderless(string value)
        {
            string trimmedValue = value.Trim().ToLower();

            bool borderless;
            if (trimmedValue == "true" || trimmedValue == "1" || trimmedValue == "yes")
            {
                borderless = true;
            }
            else if (trimmedValue == "false" || trimmedValue == "0" || trimmedValue == "no")
            {
                borderless = false;
            }
            else
            {
                throw new Exception($"Invalid borderless value: {value}. Expected: true or false");
            }

            _windowController.SetBorderless(borderless);
            Console.WriteLine($"  ✓ Borderless set to: {borderless}");
        }

        /// <summary>
        /// 设置窗口大小
        /// 用法: set window_size width,height
        /// </summary>
        private void SetWindowSize(string value)
        {
            string trimmedValue = value.Trim();
            string[] coords = trimmedValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (coords.Length != 2)
            {
                throw new Exception($"Invalid window_size value: {value}. Expected: width,height");
            }

            if (int.TryParse(coords[0].Trim(), out int width) && int.TryParse(coords[1].Trim(), out int height))
            {
                if (width <= 0 || height <= 0)
                {
                    throw new Exception($"Window size must be positive. Got: width={width}, height={height}");
                }

                _windowController.ResizeWindow(width, height);
                Console.WriteLine($"  ✓ Window size set to: {width}x{height}");
            }
            else
            {
                throw new Exception($"Invalid window_size value: {value}. Expected: two integers separated by comma");
            }
        }
    }
}