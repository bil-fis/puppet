---
pageLayout: home
hero:
  name: Puppet Framework
  text: 基于 WebView2 的桌面应用开发框架
  tagline: 用 Web 技术构建桌面应用
  image:
    src: /logo.svg
    alt: Puppet Framework
  actions:
    - theme: brand
      text: 快速上手
      link: /guide/getting-started.html
    - theme: alt
      text: API 文档
      link: /api/
    - theme: alt
      text: 查看示例
      link: /guide/best-practices.html

features:
  - title: 🌐 Web 技术栈
    details: 使用 HTML、CSS、JavaScript 构建桌面应用，降低开发门槛，熟悉的开发体验
  - title: ⚡ 深度集成
    details: 提供对 Windows 系统功能的完整访问能力，包括文件系统、窗口管理、设备监控等
  - title: 🎯 事件驱动
    details: 支持 USB 设备、磁盘、窗口等事件监控，实现实时响应和自动化操作
  - title: 🔒 安全可靠
    details: 内置多层安全机制，通信验证、路径保护、权限确认、加密存储
  - title: 📦 独特打包
    details: 支持 AES-256 加密的 PUP 打包格式，单文件分发，安全便捷
  - title: 🚀 轻量高效
    details: 基于 .NET 和 WebView2，资源占用小，性能优秀，启动快速

---

## 适用场景

### 💾 设备管理工具
USB 设备管理器、磁盘管理工具、外设监控应用

### 📊 系统监控
系统资源监控、进程管理器、网络监控工具

### 🔔 托盘应用
系统托盘工具、后台服务监控、快捷工具栏

### 🪟 透明窗口
桌面小部件、悬浮工具、透明覆盖层

### 💡 快速原型
概念验证应用、MVP 开发、内部工具

### 🎨 现代桌面
需要 Web 技术的桌面应用、跨设备应用

---

## 快速开始

\`\`\`bash
# 创建第一个应用
mkdir MyApp
cd MyApp

# 创建 index.html 文件
echo '<h1>Hello Puppet!</h1>' > index.html

# 运行应用（裸文件夹模式）
puppet.exe --nake-load .

# 或创建 PUP 文件
puppet.exe --create-pup -i . -o app.pup
puppet.exe --load-pup app.pup
\`\`\`

## 核心 API

### 窗口控制
- 无边框窗口、透明效果、点击穿透
- 窗口拖动、缩放、置顶

### 文件系统
- 文件选择对话框、文件读写、删除操作

### 系统功能
- 系统信息获取、截图、输入模拟

### 事件系统
- USB 设备插拔、磁盘挂载、窗口事件

### 托盘图标
- 托盘菜单、气泡通知、自定义图标

---

<div style="text-align: center; padding: 40px 20px 20px; border-top: 1px solid var(--c-border); margin-top: 60px; color: var(--c-text-light);">
  <p style="margin: 0 0 10px 0; font-size: 16px;">
    © 2024 Puppet Framework. All rights reserved.
  </p>
  <p style="margin: 0; font-size: 14px;">
    使用 <a href="https://vuepress.vuejs.org/" target="_blank" style="color: var(--c-brand);">VuePress</a> 和 
    <a href="https://theme-plume.vuejs.press/" target="_blank" style="color: var(--c-brand);">Plume 主题</a> 构建
  </p>
</div>