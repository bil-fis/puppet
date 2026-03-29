---
title: 架构设计
permalink: /guide/architecture.html
createTime: 2026/03/28 14:53:30
---

# 架构设计

本文档深入探讨 Puppet 框架的内部架构、核心组件以及它们之间的交互方式。理解这些内容有助于您更好地使用框架，并在需要时进行扩展和优化。

## 整体架构

Puppet 框架采用分层架构设计，每一层都有明确的职责和边界。

```
┌─────────────────────────────────────────────────────────────┐
│                      用户应用层                              │
│                  (HTML/CSS/JavaScript)                       │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    JavaScript API 层                         │
│              (window.puppet.* 命名空间)                       │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                      通信桥接层                              │
│              (COM Interop + WebMessage)                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                      控制器层                                │
│  ┌──────────┬──────────┬──────────┬──────────┬──────────┐   │
│  │ Window   │ File     │ App      │ System   │ Event    │   │
│  │ Controller│ System  │ Controller│ Controller│ Controller │   │
│  └──────────┴──────────┴──────────┴──────────┴──────────┘   │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     平台适配层                               │
│              (Windows Forms + WebView2)                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     Windows 系统层                           │
│              (Win32 API / WMI / .NET Framework)              │
└─────────────────────────────────────────────────────────────┘
```

## 核心组件详解

### 1. 应用入口（Program.cs）

`Program.cs` 是整个应用的入口点，负责：

- **安全初始化**：生成和初始化通信密钥
- **命令行处理**：解析三种运行模式的命令行参数
- **服务管理**：创建和管理 PupServer 实例
- **应用程序启动**：启动 Windows Forms 应用程序

#### 命令行参数处理

```csharp
// 支持三种运行模式
puppet.exe                        // GUI 模式
puppet.exe --create-pup -i <folder> -o <file.pup> [-p <password>]  // 创建 PUP
puppet.exe --load-pup <file.pup>  // 加载 PUP 文件
puppet.exe --nake-load <folder>   // 加载裸文件夹
```

::: info 详见文档
详细的命令行参数说明请参考 [命令行参数](./cli-parameters.md) 文档。
:::

### 2. 主窗口（Form1.cs）

`Form1.cs` 是应用的主窗口，承载 WebView2 控件并协调各个组件。

#### 主要职责

- **WebView2 初始化**：配置和初始化 WebView2 控件
- **JavaScript 注入**：将所有控制器注入到 JavaScript 环境
- **消息处理**：处理来自 Web 层的消息和请求
- **安全验证**：验证所有传入请求的密钥
- **窗口管理**：处理窗口拖动、透明效果等
- **图标管理**：自动获取并设置窗口图标

#### 关键代码片段

```csharp
// WebView2 初始化
await webView21.CoreWebView2.EnsureCoreWebView2Async(null);

// 注入 JavaScript API
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.puppet = {
        window: new WindowControllerProxy(),
        application: new ApplicationControllerProxy(),
        fs: new FileSystemControllerProxy(),
        // ... 其他控制器
    };
");

// 处理 WebMessage
webView21.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
```

### 3. 控制器层（Controllers）

控制器层是 Puppet 框架的核心，每个控制器负责一组相关的功能。

#### 控制器架构

每个控制器都遵循相同的设计模式：

```
控制器基类 (可选)
    ↓
具体控制器 (如 WindowController)
    ↓
JavaScript 代理类 (如 WindowControllerProxy)
    ↓
Web API 路由
```

#### 控制器列表

| 控制器 | 功能 | 文件 |
|--------|------|------|
| `ApplicationController` | 应用生命周期管理、外部程序执行 | `Controllers/ApplicationController.cs` |
| `FileSystemController` | 文件系统操作 | `Controllers/FileSystemController.cs` |
| `WindowController` | 窗口管理 | `Controllers/WindowController.cs` |
| `EventController` | 事件系统和设备监控 | `Controllers/EventController.cs` |
| `LogController` | 日志输出 | `Controllers/LogController.cs` |
| `SystemController` | 系统信息、输入模拟 | `Controllers/SystemController.cs` |
| `TrayController` | 托盘图标管理 | `Controllers/TrayController.cs` |

### 4. PUP 服务器（PupServer.cs）

`PupServer.cs` 是一个轻量级的 HTTP 服务器，负责提供 Web 内容。

#### 两种工作模式

##### PUP 文件模式

```csharp
// PUP 文件结构
[PUP V1.0 标识头 8 字节] + [AES 加密的 ZIP 密码 32 字节] + [ZIP 数据]
```

- 解析自定义的 PUP 文件格式
- 解密 ZIP 密钥
- 从内存中读取文件内容

##### 裸文件夹模式

- 直接从文件系统提供文件
- 支持热重载（开发时使用）
- 自动检测文件变化

#### HTTP 路由

```
/                        → index.html
/*.html                  → 静态 HTML 文件
/*.css                   → 静态 CSS 文件
/*.js                    → 静态 JavaScript 文件
/api/*                   → API 请求（转发到控制器）
```

### 5. 辅助工具类

#### 加密工具（AesHelper.cs）

负责 PUP 文件的加密和解密：

```csharp
// 固定密钥用于加密 ZIP 密钥
private static readonly byte[] FixedKey = Encoding.UTF8.GetBytes("ILOVEPUPPET");

public static string Encrypt(string plainText, string key)
public static string Decrypt(string cipherText, string key)
```

#### 密钥管理（SecretKey.cs）

生成和管理运行时密钥：

```csharp
// 生成随机密钥
public static string GenerateKey()

// 初始化并存储密钥
public static void Initialize()
```

#### 权限对话框（PermissionDialog.cs）

自定义权限确认对话框：

- 三种操作：允许、拒绝、永久阻止
- 记住用户选择
- 支持自定义消息

#### 端口选择器（PortSelector.cs）

自动选择可用端口：

```csharp
// 从 7738 开始，递增直到找到可用端口
public static int SelectAvailablePort(int startPort = 7738)
```

## 数据流详解

### 1. JavaScript 到 C# 的调用

```
用户操作 (如按钮点击)
    ↓
JavaScript 代码调用 puppet.window.setBorderless(true)
    ↓
WebMessage 发送到 C# 层
    ↓
Form1.cs 接收消息
    ↓
验证密钥
    ↓
路由到 WindowController
    ↓
调用 Windows API 修改窗口样式
    ↓
返回结果到 JavaScript
```

#### 代码示例

**JavaScript 层**：
```javascript
// 用户代码
await puppet.window.setBorderless(true);

// 内部实现
class WindowControllerProxy {
    async setBorderless(value) {
        return window.chrome.webview.postMessage({
            controller: 'window',
            action: 'setBorderless',
            params: [value]
        });
    }
}
```

**C# 层**：
```csharp
// Form1.cs
private async void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    var message = JsonConvert.DeserializeObject<WebMessage>(e.WebMessageAsJson);
    
    // 验证密钥
    if (message.Secret != SecretKey.Key)
        return;
    
    // 路由到控制器
    var result = await RouteToController(message);
    
    // 返回结果
    webView21.CoreWebView2.PostWebMessageAsJson(result);
}
```

### 2. 事件从 C# 到 JavaScript

```
系统事件 (如 USB 插入)
    ↓
WMI 监控器检测到事件
    ↓
EventController 处理事件
    ↓
构造事件对象
    ↓
通过 WebMessage 发送到 JavaScript
    ↓
调用注册的回调函数
    ↓
用户代码执行
```

#### 代码示例

**C# 层**：
```csharp
// EventController.cs
private void OnUSBArrival(object sender, EventArrivedEventArgs e)
{
    var device = ExtractDeviceInfo(e.NewEvent);
    var message = new {
        type = 'event',
        event = 'usb-plug-in',
        data = device
    };
    
    webView21.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(message));
}
```

**JavaScript 层**：
```javascript
// 事件监听
puppet.events.addEventListener('usb-plug-in', function(e) {
    console.log('USB 设备插入:', e.Device);
});
```

## 安全机制

### 1. 通信安全

所有 JavaScript 和 C# 之间的通信都经过密钥验证：

```csharp
// 生成随机密钥
string secret = SecretKey.GenerateKey();

// JavaScript 注入时包含密钥
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.PUPPET_SECRET = '" + secret + @"';
");

// 接收消息时验证密钥
if (message.Secret != SecretKey.Key)
    return; // 拒绝请求
```

### 2. 文件系统保护

自动阻止访问系统敏感目录：

```csharp
// FileSystemController.cs
private static readonly string[] ProtectedPaths = {
    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
    Environment.GetFolderPath(Environment.SpecialFolder.System),
    Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)
};

private bool IsProtectedPath(string path)
{
    foreach (var protectedPath in ProtectedPaths)
    {
        if (path.StartsWith(protectedPath, StringComparison.OrdinalIgnoreCase))
            return true;
    }
    return false;
}
```

### 3. 权限确认

危险操作前弹出确认对话框：

```csharp
// 执行系统目录程序前确认
if (IsSystemPath(command))
{
    var result = PermissionDialog.Show("执行系统程序", "确定要执行此程序吗？");
    if (result != DialogResult.Yes)
        return;
}
```

## 性能优化

### 1. WebView2 优化

- **禁用不必要的功能**：关闭不需要的浏览器功能
- **缓存管理**：合理配置缓存策略
- **进程隔离**：使用单进程模式减少内存占用

### 2. 内存管理

- **及时释放资源**：使用 `using` 语句管理资源
- **避免内存泄漏**：正确处理事件订阅
- **垃圾回收优化**：减少不必要的对象创建

### 3. 文件操作优化

- **异步 I/O**：使用异步方法避免阻塞 UI 线程
- **批量操作**：合并多个文件操作
- **缓存策略**：缓存频繁访问的文件

## 扩展性

### 1. 添加新控制器

要添加新的功能模块，可以创建新的控制器：

```csharp
public class MyFeatureController
{
    public async Task<string> MyMethod(string param)
    {
        // 实现功能
        return "result";
    }
}
```

然后在 `Form1.cs` 中注册：

```csharp
// 创建控制器实例
var myController = new MyFeatureController();

// 注入到 JavaScript
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.puppet.myFeature = new MyFeatureControllerProxy();
");
```

### 2. 自定义事件

可以扩展事件系统支持更多事件类型：

```csharp
// 在 EventController 中添加新事件
private void StartMyEventMonitoring()
{
    // 实现监控逻辑
}

// 触发事件
private void OnMyEvent(object sender, EventArgs e)
{
    var message = new {
        type = 'event',
        event = 'my-event',
        data = new { /* 事件数据 */ }
    };
    
    webView21.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(message));
}
```

## 调试和监控

### 1. 日志系统

使用 `LogController` 输出调试信息：

```javascript
puppet.log.info('调试信息');
puppet.log.warn('警告信息');
puppet.log.error('错误信息');
```

### 2. 开发者工具

在 Puppet 应用中右键选择"检查"可以打开浏览器开发者工具：

- 控制台：查看日志和错误
- 网络：监控 HTTP 请求
- 元素：检查和调试 DOM
- 源码：调试 JavaScript 代码

### 3. 性能分析

使用开发者工具的性能面板：

- 记录和分析性能
- 识别性能瓶颈
- 优化代码执行

## 最佳实践

### 1. 控制器设计

- **单一职责**：每个控制器只负责一组相关功能
- **异步优先**：使用异步方法避免阻塞
- **错误处理**：完善的异常处理和错误返回

### 2. API 设计

- **一致性**：保持 API 命名和用法的一致性
- **可预测性**：方法名应清晰表达其功能
- **文档完善**：提供详细的 API 文档

### 3. 安全考虑

- **输入验证**：验证所有用户输入
- **路径规范化**：防止路径遍历攻击
- **权限检查**：敏感操作需要权限确认

## 相关资源

- [Microsoft WebView2 文档](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)：WebView2 官方文档
- [.NET 文档](https://learn.microsoft.com/en-us/dotnet/)：.NET 框架文档
- [Windows API 文档](https://learn.microsoft.com/en-us/windows/win32/api/)：Windows API 参考

## 下一步

理解架构后，建议：

1. 查看 [API 文档](../api/) 了解具体用法
2. 阅读 [安全机制](./security.md) 了解安全细节
3. 参考 [最佳实践](./best-practices.md) 提升开发质量