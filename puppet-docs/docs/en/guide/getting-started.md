---
title: Getting Started
permalink: /en/guide/getting-started.html
createTime: 2026/03/28 14:51:54
---

# Getting Started

This guide will help you create your first Puppet application in 5 minutes.

## Requirements

Before you begin, make sure your system meets the following requirements:

- **Operating System**: Windows 10 or higher
- **.NET Runtime**: .NET 9.0 or higher
- **WebView2 Runtime**: Usually installed with Edge browser, if not installed you can download from [Microsoft official website](https://developer.microsoft.com/en-us/microsoft-edge/webview2/)

::: tip Tip
Puppet Framework will automatically check and prompt you to install required runtime environments.
:::

## Creating Your First Application

### 1. Prepare Project Files

Create a new folder as your project directory:

```bash
mkdir MyFirstPuppetApp
cd MyFirstPuppetApp
```

### 2. Create Main Page

Create `index.html` file:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My First Puppet App</title>
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
        <p>Welcome to Puppet Framework!</p>
        <button onclick="testWindow()">Window Control</button>
        <button onclick="testSystem()">System Info</button>
        <button onclick="testLog()">Test Log</button>
        <div class="info" id="info">Click buttons to test features</div>
    </div>

    <script>
        // Window control test
        async function testWindow() {
            try {
                // Set opacity
                await puppet.window.setOpacity(0.9);

                // Show info
                showInfo('Window opacity set to 90%');

                // Restore after 2 seconds
                setTimeout(async () => {
                    await puppet.window.setOpacity(1.0);
                }, 2000);
            } catch (error) {
                showInfo('Error: ' + error.message);
            }
        }

        // System info test
        async function testSystem() {
            try {
                const sysInfo = await puppet.system.getSystemInfo();
                showInfo('OS: ' + sysInfo.osName);
            } catch (error) {
                showInfo('Error: ' + error.message);
            }
        }

        // Log test
        function testLog() {
            puppet.log.info('This is an info log');
            puppet.log.warn('This is a warning log');
            puppet.log.error('This is an error log');
            showInfo('Logs have been output, check console');
        }

        // Show info
        function showInfo(message) {
            document.getElementById('info').textContent = message;
        }
    </script>
</body>
</html>
```

### 3. Run Application

There are two ways to run a Puppet application:

#### Method 1: Bare Folder Mode (Recommended for Development)

This is the recommended method during development, allowing you to see code changes in real-time:

```bash
# Assuming puppet.exe is in E:\puppet\puppet\bin\Debug\ directory
E:\puppet\puppet\bin\Debug\puppet.exe --nake-load "C:\MyFirstPuppetApp"
```

#### Method 2: PUP File Mode (For Release)

First create a PUP file:

```bash
E:\puppet\puppet\bin\Debug\puppet.exe --create-pup -i "C:\MyFirstPuppetApp" -o "C:\MyFirstPuppetApp.pup"
```

Then run the PUP file:

```bash
E:\puppet\puppet\bin\Debug\puppet.exe --load-pup "C:\MyFirstPuppetApp.pup"
```

### 4. Test Features

Click the buttons in the application to test the following features:

- **Window Control**: Window opacity will briefly change
- **System Info**: Display current operating system name
- **Test Log**: Output different levels of logs to console

## Next Steps

Congratulations! You have successfully created your first Puppet application. Next you can:

1. **Learn API**: View [API Documentation](../api/) to learn about all available features
2. **Window Control**: Learn how to create borderless, transparent windows
3. **File Operations**: Learn how to read and write local files
4. **Event System**: Implement device plug/unplug, window event monitoring
5. **System Functions**: Get system information, simulate keystrokes

## Common Questions

### Q: How to create a borderless window?

A: Add the following code to your HTML:

```javascript
// Set immediately after page loads
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
});
```

For details, see [Window Control API](../api/window.md).

### Q: How to debug applications?

A: You can:

1. Use browser developer tools (right-click in Puppet app -> Inspect)
2. Use `puppet.log.info()` and other methods to output logs
3. In bare folder mode, you can directly modify HTML files to see changes in real-time

### Q: Can PUP files be encrypted?

A: Yes. Specify a password when creating a PUP file:

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -p "mypassword"
```

For details, see [PUP File Format](./pup-format.md).

### Q: How to make the application start automatically on boot?

A: Use the `execute` method in Application API to create a shortcut:

```javascript
await puppet.Application.execute(
    'cmd /c mkshortcut /target:"C:\\MyApp\\puppet.exe" /shortcut:"%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\Startup"'
);
```

## Related Resources

- [Framework Introduction](./introduction.md) - Learn about Puppet's core features
- [Architecture Design](./architecture.md) - Deep dive into framework internals
- [Command Line Parameters](./cli-parameters.md) - All command line options
- [API Documentation](../api/) - Complete API reference manual

## Getting Help

If you encounter issues during development:

1. Consult relevant sections of this documentation
2. Check [API Documentation](../api/) for specific usage
3. Submit issues in [GitHub Issues](#)