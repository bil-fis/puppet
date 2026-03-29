---
title: 快速开始
permalink: /guide/getting-started.html
createTime: 2026/03/28 14:51:54
---

# 快速开始

本指南将在 5 分钟内帮助您创建第一个 Puppet 应用程序。

## 环境要求

在开始之前，请确保您的系统满足以下要求：

- **操作系统**：Windows 10 或更高版本
- **.NET 运行时**：.NET 9.0 或更高版本
- **WebView2 运行时**：通常已随 Edge 浏览器安装，如未安装可从 [Microsoft 官网](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) 下载

::: tip 提示
Puppet 框架会自动检查并提示安装所需的运行时环境。
:::

## 创建第一个应用

### 1. 准备项目文件

创建一个新文件夹作为您的项目目录：

```bash
mkdir MyFirstPuppetApp
cd MyFirstPuppetApp
```

### 2. 创建主页面

创建 `index.html` 文件：

```html
<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>我的第一个 Puppet 应用</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            color: white;
        }
        
        .container {
            text-align: center;
            padding: 40px;
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
        }
        
        h1 {
            font-size: 2.5em;
            margin-bottom: 20px;
        }
        
        p {
            font-size: 1.2em;
            margin-bottom: 30px;
        }
        
        button {
            padding: 12px 24px;
            font-size: 16px;
            border: none;
            border-radius: 8px;
            background: white;
            color: #667eea;
            cursor: pointer;
            transition: transform 0.2s, box-shadow 0.2s;
            margin: 0 10px;
        }
        
        button:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
        
        button:active {
            transform: translateY(0);
        }
        
        .info {
            margin-top: 20px;
            font-size: 14px;
            opacity: 0.8;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>🎭 Puppet Framework</h1>
        <p>欢迎来到 Puppet 框架！</p>
        <button onclick="testWindow()">窗口控制</button>
        <button onclick="testSystem()">系统信息</button>
        <button onclick="testLog()">测试日志</button>
        <div class="info" id="info">点击按钮测试功能</div>
    </div>

    <script>
        // 窗口控制测试
        async function testWindow() {
            try {
                // 设置透明度
                await puppet.window.setOpacity(0.9);
                
                // 显示信息
                showInfo('窗口透明度已设置为 90%');
                
                // 2秒后恢复
                setTimeout(async () => {
                    await puppet.window.setOpacity(1.0);
                }, 2000);
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        // 系统信息测试
        async function testSystem() {
            try {
                const sysInfo = await puppet.system.getSystemInfo();
                showInfo('操作系统: ' + sysInfo.osName);
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        // 日志测试
        function testLog() {
            puppet.log.info('这是一条信息日志');
            puppet.log.warn('这是一条警告日志');
            puppet.log.error('这是一条错误日志');
            showInfo('日志已输出，请查看控制台');
        }
        
        // 显示信息
        function showInfo(message) {
            document.getElementById('info').textContent = message;
        }
    </script>
</body>
</html>
```

### 3. 运行应用

有两种方式运行 Puppet 应用：

#### 方式一：裸文件夹模式（开发时使用）

这是开发过程中推荐的方式，可以实时看到代码修改的效果：

```bash
# 假设 puppet.exe 在 E:\puppet\puppet\bin\Debug\ 目录下
E:\puppet\puppet\bin\Debug\puppet.exe --nake-load "C:\MyFirstPuppetApp"
```

#### 方式二：PUP 文件模式（发布时使用）

首先创建 PUP 文件：

```bash
E:\puppet\puppet\bin\Debug\puppet.exe --create-pup -i "C:\MyFirstPuppetApp" -o "C:\MyFirstPuppetApp.pup"
```

然后运行 PUP 文件：

```bash
E:\puppet\puppet\bin\Debug\puppet.exe --load-pup "C:\MyFirstPuppetApp.pup"
```

### 4. 测试功能

点击应用中的按钮，测试以下功能：

- **窗口控制**：窗口透明度会短暂变化
- **系统信息**：显示当前操作系统名称
- **测试日志**：在控制台输出不同级别的日志

## 下一步

恭喜！您已经成功创建了第一个 Puppet 应用。接下来可以：

1. **深入学习 API**：查看 [API 文档](../api/) 了解所有可用的功能
2. **窗口控制**：学习如何创建无边框、透明窗口
3. **文件操作**：了解如何读写本地文件
4. **事件系统**：实现设备插拔、窗口事件等监控
5. **系统功能**：获取系统信息、模拟按键等

## 常见问题

### Q: 如何创建无边框窗口？

A: 在您的 HTML 中添加以下代码：

```javascript
// 页面加载后立即设置
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
});
```

详细说明请参考 [窗口控制 API](../api/window.md)。

### Q: 如何调试应用？

A: 您可以：

1. 使用浏览器开发者工具（在 Puppet 应用中右键 -> 检查）
2. 使用 `puppet.log.info()` 等方法输出日志
3. 在裸文件夹模式下，直接修改 HTML 文件即可实时看到变化

### Q: PUP 文件可以加密吗？

A: 可以。创建 PUP 文件时指定密码：

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -p "mypassword"
```

详细说明请参考 [PUP 文件格式](./pup-format.md)。

### Q: 如何创建带签名的 PUP 文件？

A: 使用 V1.2 格式和 puppet-sign 工具：

```bash
# 1. 生成签名密钥对
puppet-sign.exe --generate-signing-key --alias MyApp --key-size 2048

# 2. 创建带签名的 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "mypassword"
```

带签名的 PUP 文件可以确保应用的完整性和来源可信度。详细说明请参考 [安全机制](./security.md) 和 [PUP 文件格式](./pup-format.md)。

### Q: 如何让应用开机自启动？

A: 使用 Application API 的 `execute` 方法创建快捷方式：

```javascript
await puppet.Application.execute(
    'cmd /c mkshortcut /target:"C:\\MyApp\\puppet.exe" /shortcut:"%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"'
);
```

## 相关资源

- [框架介绍](./introduction.md) - 了解 Puppet 的核心特性
- [架构设计](./architecture.md) - 深入了解框架内部原理
- [命令行参数](./cli-parameters.md) - 所有命令行选项说明
- [API 文档](../api/) - 完整的 API 参考手册

## 获取帮助

如果您在开发过程中遇到问题：

1. 查阅本文档的相关章节
2. 查看 [API 文档](../api/) 了解具体用法
3. 在 [GitHub Issues](#) 提交问题