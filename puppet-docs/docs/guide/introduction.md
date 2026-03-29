---
title: 框架介绍
permalink: /guide/introduction.html
createTime: 2026/03/28 14:52:37
---

# 框架介绍

Puppet 是一个创新的 Windows 桌面应用开发框架，它将 Web 技术的灵活性与原生 Windows 应用的强大功能完美结合。本章节将详细介绍 Puppet 框架的核心特性、设计理念以及适用场景。

## 什么是 Puppet？

Puppet 是一个基于 **Microsoft WebView2** 的桌面应用框架，允许开发者使用 HTML、CSS 和 JavaScript 等 Web 技术构建跨平台的桌面应用程序。与传统的 Electron 或 NW.js 不同，Puppet 专为 Windows 系统设计，提供了更深层次的系统集成和更轻量级的解决方案。

### 设计理念

Puppet 的设计遵循以下核心原则：

1. **Web 优先**：充分利用现代 Web 技术栈，降低开发门槛
2. **原生集成**：提供对 Windows 系统功能的完整访问能力
3. **轻量高效**：基于 .NET 和 WebView2，资源占用小，性能优秀
4. **安全可靠**：内置多层安全机制，保护系统和用户数据
5. **简单易用**：简洁的 API 设计，快速上手

## 核心特性

### 1. Web 技术栈

Puppet 使用 Web 技术构建用户界面，这意味着：

- **熟悉的开发体验**：使用 HTML、CSS、JavaScript 进行开发
- **丰富的生态系统**：可直接使用 npm 包、前端框架（Vue、React 等）
- **现代 UI 设计**：支持 CSS3、Flexbox、Grid 等现代布局技术
- **响应式设计**：轻松适配不同屏幕尺寸

::: tip 提示
Puppet 支持所有现代浏览器特性，因为其基于 Edge WebView2 渲染引擎。
:::

### 2. 原生系统集成

Puppet 提供了对 Windows 系统功能的深度访问：

- **窗口管理**：无边框窗口、透明效果、窗口拖动、点击穿透
- **文件系统**：文件读写、文件夹选择、路径操作
- **系统信息**：获取 CPU、内存、GPU、操作系统等信息
- **输入模拟**：模拟键盘和鼠标操作
- **托盘图标**：系统托盘集成、气泡通知、自定义菜单
- **外部程序**：执行系统命令、打开文件、启动应用

### 3. 事件驱动架构

Puppet 提供强大的事件系统，支持：

- **USB 设备事件**：USB 设备插拔监控
- **磁盘事件**：磁盘挂载和卸载监控
- **窗口事件**：窗口焦点、最大化、移动、缩放等
- **电源事件**：电源状态变化监控
- **自定义事件**：支持自定义事件监听和处理

::: info 技术细节
事件系统基于 Windows Management Instrumentation (WMI) 实现，提供了稳定可靠的设备监控能力。
:::

### 4. 安全机制

Puppet 内置多层安全保护：

- **通信验证**：使用运行时生成的随机密钥验证所有本地请求
- **文件系统保护**：自动阻止访问 Windows 系统文件夹
- **权限确认**：危险操作前弹出用户确认对话框
- **路径规范化**：防止路径遍历攻击
- **PUP 加密**：支持密码加密的 PUP 文件格式

### 5. 独特的 PUP 打包格式

Puppet 提供了独特的 `.pup` 文件格式：

- **自定义格式**：结合 ZIP 和 AES 加密的专属格式
- **密码保护**：支持 AES-256 加密保护源代码
- **单文件分发**：所有资源打包为一个文件，便于分发
- **快速加载**：优化的文件结构和加载机制

## 技术架构

### 整体架构

```
┌─────────────────────────────────────────┐
│         Web 应用层 (HTML/CSS/JS)        │
├─────────────────────────────────────────┤
│         JavaScript API (puppet.*)       │
├─────────────────────────────────────────┤
│         COM 桥接层                      │
├─────────────────────────────────────────┤
│         C# 控制器层                      │
│  ┌──────────────────────────────────┐  │
│  │  WindowController                │  │
│  │  FileSystemController            │  │
│  │  ApplicationController            │  │
│  │  SystemController                │  │
│  │  EventController                 │  │
│  │  LogController                   │  │
│  │  TrayController                  │  │
│  └──────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         Windows Forms + WebView2        │
├─────────────────────────────────────────┤
│         Windows API / 系统服务          │
└─────────────────────────────────────────┘
```

### 数据流

```
用户操作 → JavaScript 代码 → COM 桥接 → C# 控制器 → Windows API / 系统服务
```

### 核心组件

1. **WebView2 控件**：渲染 Web 内容，提供浏览器功能
2. **COM 桥接**：实现 JavaScript 与 C# 之间的双向通信
3. **HTTP 服务器**：提供本地文件服务和 API 端点
4. **控制器**：封装各种系统功能的 API
5. **PUP 服务器**：解析和提供 PUP 文件内容

## 与其他框架的对比

### vs Electron

| 特性 | Puppet | Electron |
|------|--------|----------|
| 基础架构 | .NET + WebView2 | Node.js + Chromium |
| 应用体积 | ~50MB | ~150MB+ |
| 内存占用 | 较低 | 较高 |
| Windows 集成 | 深度 | 中等 |
| 跨平台 | 仅 Windows | 跨平台 |
| 学习曲线 | 平缓 | 中等 |
| 性能 | 优秀 | 良好 |

### vs C# WinForms/WPF

| 特性 | Puppet | WinForms/WPF |
|------|--------|--------------|
| 开发语言 | JavaScript/C# | C# |
| UI 技术 | HTML/CSS | XAML/Designer |
| 开发速度 | 快 | 中等 |
| 灵活性 | 高 | 中等 |
| 性能 | 优秀 | 优秀 |
| Windows 集成 | 深度 | 深度 |

## 适用场景

Puppet 特别适合以下应用场景：

### 1. 设备管理工具

- USB 设备管理器
- 磁盘管理工具
- 外设监控应用

### 2. 系统监控工具

- 系统资源监控
- 进程管理器
- 网络监控工具

### 3. 托盘应用

- 系统托盘工具
- 后台服务监控
- 快捷工具栏

### 4. 透明窗口应用

- 桌面小部件
- 悬浮工具
- 透明覆盖层

### 5. 快速原型开发

- 概念验证应用
- MVP 开发
- 内部工具

### 6. 现代桌面应用

- 需要 Web 技术的桌面应用
- 需要系统功能的 Web 应用
- 跨设备应用

## 不适用场景

以下场景可能不适合使用 Puppet：

- 需要跨平台的应用（考虑 Electron）
- 纯计算密集型应用（考虑原生开发）
- 需要 GPU 加速的游戏（考虑 Unity/Unreal）
- 极度追求性能的系统工具（考虑 C++）

## 技术栈

Puppet 框架基于以下技术构建：

### 核心技术

- **.NET 9.0**：核心运行时框架
- **Windows Forms**：桌面 UI 框架
- **Microsoft WebView2**：Web 渲染引擎（基于 Edge）

### 第三方库

- **Newtonsoft.Json**：JSON 序列化/反序列化
- **SharpZipLib**：ZIP 文件操作（用于 PUP 格式）
- **System.Management**：WMI 设备监控

### 开发工具

- **Visual Studio**：推荐的开发 IDE
- **MSBuild**：构建工具
- **NuGet**：包管理器

## 限制和约束

### 平台限制

- **操作系统**：仅支持 Windows 10 及以上版本
- **架构**：支持 x64 和 x86 架构
- **WebView2**：需要安装 WebView2 运行时

### 功能限制

- **跨平台**：不支持 macOS 和 Linux
- **移动端**：不支持移动平台
- **GPU 加速**：有限的 GPU 加速支持
- **原生模块**：不支持 Node.js 原生模块

### 性能考虑

- **大量计算**：密集计算建议使用 C# 扩展
- **内存管理**：需要注意 JavaScript 内存泄漏
- **DOM 操作**：大量 DOM 操作可能影响性能

## 学习路径

推荐按照以下路径学习 Puppet：

1. **基础入门**
   - 阅读 [快速开始](./getting-started.md)
   - 创建第一个应用
   - 理解基本概念

2. **核心功能**
   - 学习 [窗口控制](../api/window.md)
   - 掌握 [文件系统](../api/fs.md)
   - 了解 [系统功能](../api/system.md)

3. **进阶功能**
   - 探索 [事件系统](../api/events.md)
   - 使用 [设备监控](../api/device.md)
   - 实现 [托盘图标](../api/tray.md)

4. **高级主题**
   - 理解 [架构设计](./architecture.md)
   - 学习 [安全机制](./security.md)
   - 遵循 [最佳实践](./best-practices.md)

## 相关资源

### 官方资源

- [GitHub 仓库](#)：源代码和问题追踪
- [API 文档](../api/)：完整的 API 参考
- [示例代码](#)：实际应用示例

### 外部资源

- [Microsoft WebView2 文档](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)：WebView2 官方文档
- [.NET 文档](https://learn.microsoft.com/en-us/dotnet/)：.NET 框架文档
- [Windows API 文档](https://learn.microsoft.com/en-us/windows/win32/api/)：Windows API 参考

## 下一步

现在您已经了解了 Puppet 框架的基本情况，建议：

1. 阅读 [快速开始](./getting-started.md) 创建您的第一个应用
2. 了解 [架构设计](./architecture.md) 深入理解框架原理
3. 查看 [API 文档](../api/) 开始实际开发
4. 参考 [最佳实践](./best-practices.md) 提升代码质量