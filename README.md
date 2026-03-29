<div align="center">

# 🎭 Puppet Framework

**基于 WebView2 的 Windows 桌面应用开发框架**

![Puppet Logo](puppet.png)

用熟悉的 Web 技术构建功能强大的桌面应用程序

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Platform](https://img.shields.io/badge/platform-Windows-blue.svg)](https://www.microsoft.com/windows)
[![Documentation](https://img.shields.io/badge/docs-latest-green.svg)](https://puppet.ifiss.eu.org)

[English](#english) | [中文](#中文)

</div>

---

## 中文

### ✨ 特性

- **🌐 Web 技术栈** - 使用 HTML、CSS、JavaScript 构建界面，降低开发门槛
- **⚡ 深度集成** - 提供对 Windows 系统功能的完整访问能力
- **🎯 事件驱动** - 支持 USB 设备、磁盘、窗口等事件监控
- **🔒 安全可靠** - 内置多层安全机制，支持数字签名验证
- **📦 独特打包** - 支持 AES-256 加密的 PUP 打包格式
- **🚀 轻量高效** - 基于 .NET 和 WebView2，资源占用小，性能优秀

### 🎯 适用场景

- 💾 **设备管理工具** - USB 设备管理器、磁盘管理工具
- 📊 **系统监控** - 系统资源监控、进程管理器
- 🔔 **托盘应用** - 系统托盘工具、后台服务监控
- 🪟 **透明窗口** - 桌面小部件、悬浮工具
- 💡 **快速原型** - 概念验证应用、MVP 开发

### 🚀 快速开始

#### 安装

1. **下载最新版本**

   访问 [Releases](https://github.com/your-username/puppet/releases) 下载适合的版本：
   - **Framework-dependent**（需要 .NET 9.0）：体积小，适合开发者
   - **Self-contained**（包含运行时）：开箱即用，适合普通用户

2. **解压并运行**

   ```powershell
   # 解压到任意目录
   Expand-Archive puppet-windows-self-contained.zip -DestinationPath C:\Puppet

   # 运行
   C:\Puppet\puppet.exe
   ```

#### 创建第一个应用

```bash
# 1. 创建项目目录
mkdir MyFirstPuppetApp
cd MyFirstPuppetApp

# 2. 创建 index.html
echo '<h1>Hello Puppet!</h1>' > index.html

# 3. 运行应用（裸文件夹模式）
puppet.exe --nake-load .

# 4. 打包为 PUP 文件
puppet.exe --create-pup -i . -o app.pup
```

#### 使用签名功能（V1.2）

```bash
# 1. 生成签名密钥对
puppet-sign.exe --generate-signing-key --interactive

# 2. 创建带签名的 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

### 📚 文档

完整文档请访问：**[puppet.ifiss.eu.org](https://puppet.ifiss.eu.org)**

- [快速开始](https://puppet.ifiss.eu.org/guide/getting-started.html)
- [API 文档](https://puppet.ifiss.eu.org/api/)
- [PUP 文件格式](https://puppet.ifiss.eu.org/guide/pup-format.html)
- [安全机制](https://puppet.ifiss.eu.org/guide/security.html)
- [最佳实践](https://puppet.ifiss.eu.org/guide/best-practices.html)

### 🌟 核心功能

#### 窗口控制
```javascript
// 无边框窗口
await puppet.window.setBorderless(true);

// 设置透明度
await puppet.window.setOpacity(0.9);

// 窗口居中
await puppet.window.centerWindow();
```

#### 文件操作
```javascript
// 打开文件选择对话框
const files = await puppet.fs.openFileDialog(['文本文件', '*.txt']);

// 读取文件
const content = await puppet.fs.readFileAsText(files[0]);

// 写入文件
await puppet.fs.writeTextToFile('output.txt', content);
```

#### 系统功能
```javascript
// 获取系统信息
const sysInfo = await puppet.system.getSystemInfo();
console.log('操作系统:', sysInfo.osName);

// 截图
const screenshot = await puppet.system.takeScreenShot();
```

#### 事件监听
```javascript
// 监听 USB 设备插入
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
});

// 监听窗口最大化
await puppet.events.addEventListener('window-maximize', () => {
    console.log('窗口已最大化');
});
```

### 🛠️ 开发指南

#### 系统要求

- **操作系统**: Windows 10 或更高版本
- **.NET 运行时**: .NET 9.0 或更高版本（Framework-dependent 版本需要）
- **WebView2**: 通常已随 Edge 浏览器安装

#### 构建项目

```bash
# 克隆仓库
git clone https://github.com/your-username/puppet.git
cd puppet

# 恢复依赖
dotnet restore puppet.sln

# 构建 Release 版本
dotnet build puppet.sln --configuration Release

# 发布
dotnet publish puppet/puppet.csproj --configuration Release --output publish
```

### 📦 PUP 文件格式

Puppet 使用独特的 `.pup` 文件格式：

- **V1.0** - 基础功能，支持加密
- **V1.1** - 支持启动脚本
- **V1.2** - 支持数字签名和证书验证

### 🔐 安全机制

- 通信验证 - 使用随机密钥验证所有本地请求
- 文件系统保护 - 自动阻止访问系统文件夹
- 权限确认 - 危险操作前弹出用户确认
- 数字签名 - 基于 X.509 证书的签名验证（V1.2）
- 数据库签名 - 防止数据库被篡改（V1.2）

### 🤝 贡献

欢迎贡献代码、报告问题或提出建议！

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

### 📝 更新日志

查看 [CHANGELOG](puppet-docs/docs/changelog/README.md) 了解版本更新历史。

### 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

### 🙏 致谢

- [Microsoft WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) - Web 渲染引擎
- [.NET](https://dotnet.microsoft.com/) - 核心框架
- [VuePress](https://vuepress.vuejs.org/) - 文档生成工具

### 📧 联系方式

- 项目主页: [https://github.com/your-username/puppet](https://github.com/your-username/puppet)
- 文档: [https://puppet.ifiss.eu.org](https://puppet.ifiss.eu.org)
- 问题反馈: [GitHub Issues](https://github.com/your-username/puppet/issues)

---

## English

### ✨ Features

- **🌐 Web Tech Stack** - Build interfaces with HTML, CSS, JavaScript
- **⚡ Deep Integration** - Full access to Windows system capabilities
- **🎯 Event-Driven** - Monitor USB devices, disks, windows, and more
- **🔒 Secure & Reliable** - Multi-layer security with digital signature support
- **📦 Unique Packaging** - AES-256 encrypted PUP file format
- **🚀 Lightweight & Efficient** - Built on .NET and WebView2

### 🎯 Use Cases

- 💾 **Device Management Tools** - USB managers, disk utilities
- 📊 **System Monitoring** - Resource monitors, process managers
- 🔔 **Tray Applications** - System tray tools, background services
- 🪟 **Transparent Windows** - Desktop widgets, floating tools
- 💡 **Quick Prototypes** - Concept validation, MVP development

### 🚀 Quick Start

#### Installation

1. **Download Latest Release**

   Visit [Releases](https://github.com/your-username/puppet/releases) and download:
   - **Framework-dependent** (requires .NET 9.0): Smaller size, for developers
   - **Self-contained** (includes runtime): Ready to use, for end users

2. **Extract and Run**

   ```powershell
   # Extract to any directory
   Expand-Archive puppet-windows-self-contained.zip -DestinationPath C:\Puppet

   # Run
   C:\Puppet\puppet.exe
   ```

#### Create Your First App

```bash
# 1. Create project directory
mkdir MyFirstPuppetApp
cd MyFirstPuppetApp

# 2. Create index.html
echo '<h1>Hello Puppet!</h1>' > index.html

# 3. Run app (bare folder mode)
puppet.exe --nake-load .

# 4. Package as PUP file
puppet.exe --create-pup -i . -o app.pup
```

#### Use Signing Features (V1.2)

```bash
# 1. Generate signing key pair
puppet-sign.exe --generate-signing-key --interactive

# 2. Create signed PUP file
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

### 📚 Documentation

Full documentation available at: **[puppet.ifiss.eu.org](https://puppet.ifiss.eu.org)**

- [Getting Started](https://puppet.ifiss.eu.org/guide/getting-started.html)
- [API Documentation](https://puppet.ifiss.eu.org/api/)
- [PUP File Format](https://puppet.ifiss.eu.org/guide/pup-format.html)
- [Security](https://puppet.ifiss.eu.org/guide/security.html)
- [Best Practices](https://puppet.ifiss.eu.org/guide/best-practices.html)

### 🌟 Core Features

#### Window Control
```javascript
// Borderless window
await puppet.window.setBorderless(true);

// Set opacity
await puppet.window.setOpacity(0.9);

// Center window
await puppet.window.centerWindow();
```

#### File Operations
```javascript
// Open file dialog
const files = await puppet.fs.openFileDialog(['Text Files', '*.txt']);

// Read file
const content = await puppet.fs.readFileAsText(files[0]);

// Write file
await puppet.fs.writeTextToFile('output.txt', content);
```

#### System Functions
```javascript
// Get system info
const sysInfo = await puppet.system.getSystemInfo();
console.log('OS:', sysInfo.osName);

// Take screenshot
const screenshot = await puppet.system.takeScreenShot();
```

#### Event Listening
```javascript
// Monitor USB device insertion
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB device inserted:', event.data);
});

// Monitor window maximize
await puppet.events.addEventListener('window-maximize', () => {
    console.log('Window maximized');
});
```

### 🛠️ Development Guide

#### System Requirements

- **OS**: Windows 10 or higher
- **.NET Runtime**: .NET 9.0 or higher (required for Framework-dependent version)
- **WebView2**: Usually installed with Edge browser

#### Build Project

```bash
# Clone repository
git clone https://github.com/your-username/puppet.git
cd puppet

# Restore dependencies
dotnet restore puppet.sln

# Build Release version
dotnet build puppet.sln --configuration Release

# Publish
dotnet publish puppet/puppet.csproj --configuration Release --output publish
```

### 📦 PUP File Format

Puppet uses a unique `.pup` file format:

- **V1.0** - Basic features with encryption support
- **V1.1** - Startup script support
- **V1.2** - Digital signature and certificate verification

### 🔐 Security Features

- Communication verification with random keys
- File system protection
- Permission confirmation dialogs
- Digital signature based on X.509 certificates (V1.2)
- Database signature verification (V1.2)

### 🤝 Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### 📝 Changelog

See [CHANGELOG](puppet-docs/docs/changelog/README.md) for version history.

### 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### 🙏 Acknowledgments

- [Microsoft WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) - Web rendering engine
- [.NET](https://dotnet.microsoft.com/) - Core framework
- [VuePress](https://vuepress.vuejs.org/) - Documentation generator

### 📧 Contact

- Project Home: [https://github.com/your-username/puppet](https://github.com/your-username/puppet)
- Documentation: [https://puppet.ifiss.eu.org](https://puppet.ifiss.eu.org)
- Issues: [GitHub Issues](https://github.com/your-username/puppet/issues)

---

<div align="center">

**Made with ❤️ by 林晚晚ss**

[⬆ Back to Top](#-puppet-framework)

</div>