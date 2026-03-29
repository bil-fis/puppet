---
title: API 概述
permalink: /api/index.html
createTime: 2026/03/28 15:01:52
---

# API 概述

Puppet 框架提供了一套完整的 JavaScript API，允许您在 Web 应用中访问 Windows 系统功能。所有 API 都通过 `puppet` 全局对象访问。

## 命名空间

Puppet API 按功能分为以下几个命名空间：

```javascript
puppet.window     // 窗口控制
puppet.application // 应用控制
puppet.fs         // 文件系统
puppet.log        // 日志
puppet.system     // 系统功能
puppet.tray       // 托盘图标
puppet.events     // 事件系统
puppet.device     // 设备系统
puppet.storage    // 持久化存储
```

## API 特性

### 异步操作

大多数 API 方法都是异步的，返回 Promise 对象。

```javascript
// 异步调用
await puppet.window.setBorderless(true);

// 使用 Promise
puppet.window.setBorderless(true)
    .then(() => console.log('窗口已设置为无边框'))
    .catch(err => console.error('操作失败:', err));
```

### 错误处理

所有 API 方法都可能抛出异常，建议使用 try-catch 捕获。

```javascript
try {
    await puppet.fs.readFileAsText('config.json');
} catch (error) {
    console.error('读取文件失败:', error.message);
    puppet.log.error('文件读取失败');
}
```

### 类型支持

API 方法支持多种数据类型：

- **字符串**：文件路径、配置值等
- **数字**：窗口位置、大小、透明度等
- **布尔值**：开关选项、状态标志等
- **对象**：配置对象、事件数据等
- **数组**：文件列表、菜单项等

## 快速开始

### 窗口控制

```javascript
// 设置无边框窗口
await puppet.window.setBorderless(true);

// 设置窗口透明度
await puppet.window.setOpacity(0.9);

// 居中窗口
await puppet.window.centerWindow();
```

### 文件操作

```javascript
// 打开文件选择对话框
const files = await puppet.fs.openFileDialog(
    ['文本文件', '*.txt'],
    false
);

// 读取文件内容
const content = await puppet.fs.readFileAsText(files[0]);

// 写入文件
await puppet.fs.writeTextToFile('output.txt', content);
```

### 系统信息

```javascript
// 获取系统信息
const sysInfo = await puppet.system.getSystemInfo();
console.log('操作系统:', sysInfo.osName);
console.log('CPU:', sysInfo.cpuModel);
console.log('内存:', sysInfo.totalMemory);

// 截图
const screenshot = await puppet.system.takeScreenShot();
```

### 事件监听

```javascript
// 监听 USB 设备插入
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
    puppet.log.info('USB 设备已插入');
});

// 监听窗口最大化
await puppet.events.addEventListener('window-maximize', () => {
    console.log('窗口已最大化');
});
```

## API 命名约定

### 方法命名

- **set**：设置属性或状态
  - `setBorderless()`
  - `setOpacity()`
  - `setMenu()`

- **get**：获取属性或信息
  - `getWindowInfo()`
  - `getSystemInfo()`
  - `getDevices()`

- **is**：查询状态
  - `exists()`

- **open**：打开对话框
  - `openFileDialog()`
  - `openFolderDialog()`

### 事件命名

事件名称使用 kebab-case 格式：

- `usb-plug-in`：USB 设备插入
- `usb-plug-out`：USB 设备拔出
- `window-maximize`：窗口最大化
- `disk-mount`：磁盘挂载

## API 参考

### 窗口控制 API

提供窗口管理功能，包括样式控制、位置调整、透明效果等。

- [窗口控制 API 详细文档](./window.md)

### 应用控制 API

提供应用生命周期管理和外部程序执行功能。

- [应用控制 API 详细文档](./application.md)

### 文件系统 API

提供文件和文件夹操作功能，包括读写、选择、删除等。

- [文件系统 API 详细文档](./fs.md)

### 日志 API

提供日志输出功能，用于调试和错误追踪。

- [日志 API 详细文档](./log.md)

### 系统 API

提供系统信息获取、输入模拟、截图等功能。

- [系统 API 详细文档](./system.md)

### 托盘图标 API

提供系统托盘图标管理功能，包括菜单、通知等。

- [托盘图标 API 详细文档](./tray.md)

### 事件系统 API

提供事件监听和处理功能，支持设备和窗口事件。

- [事件系统 API 详细文档](./events.md)

### 设备系统 API

提供设备查询和监控功能。

- [设备系统 API 详细文档](./device.md)

### 持久化存储 API

提供基于 SQLite 的键值对存储功能，用于数据持久化。

- [持久化存储 API 详细文档](./storage.md)

## 版本兼容性

当前 API 版本：v1.2

### PUP 格式版本

Puppet 框架支持多种 PUP 文件格式：

- **V1.0**：基础功能，支持加密
- **V1.1**：支持启动脚本
- **V1.2**：支持数字签名和证书验证

### V1.2 新增功能

V1.2 版本在 V1.1 基础上增加了以下安全功能：

- **数字签名**：基于 X.509 证书的数字签名
- **签名验证**：应用和数据库签名验证
- **证书管理**：证书指纹、颁发者、有效期等信息查询
- **数据库签名**：数据库内容签名验证，防止篡改

### 向后兼容性

Puppet 框架承诺在主版本号不变的情况下保持 API 向后兼容。

### 废弃的 API

如果某个 API 被废弃，会在文档中明确标注，并提供替代方案。

## 限制和约束

### 权限限制

- 无法访问 Windows 系统目录
- 无法直接执行某些系统命令（需要用户确认）

### 性能限制

- 大文件读写可能影响性能
- 频繁的事件监听可能占用资源

### 平台限制

- 仅支持 Windows 10 及以上版本
- 部分功能需要管理员权限

## 安全考虑

### 输入验证

所有用户输入都应该进行验证：

```javascript
// 验证文件路径
if (!path || typeof path !== 'string') {
    throw new Error('无效的路径');
}
```

### 错误处理

始终处理可能的错误：

```javascript
try {
    await riskyOperation();
} catch (error) {
    puppet.log.error('操作失败:', error.message);
    showError('操作失败，请重试');
}
```

### 敏感数据

不要在代码中硬编码敏感信息：

```javascript
// 不推荐
const password = 'my-secret-password';

// 推荐
const password = await loadPasswordFromConfig();
```

## 相关资源

- [Microsoft WebView2 文档](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)：WebView2 官方文档
- [.NET 文档](https://learn.microsoft.com/en-us/dotnet/)：.NET 框架文档
- [JavaScript 异步编程](https://developer.mozilla.org/en-US/docs/Learn/JavaScript/Asynchronous)：异步编程指南

## 下一步

建议按照以下顺序学习 API：

1. 了解 [窗口控制 API](./window.md) 创建自定义窗口
2. 学习 [文件系统 API](./fs.md) 进行文件操作
3. 探索 [事件系统 API](./events.md) 实现事件处理
4. 使用 [系统 API](./system.md) 获取系统信息
5. 参考 [托盘图标 API](./tray.md) 创建托盘应用