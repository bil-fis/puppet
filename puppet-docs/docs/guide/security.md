---
title: 安全机制
permalink: /guide/security.html
createTime: 2026/03/28 14:59:03
---

# 安全机制

Puppet 框架内置多层安全机制，保护系统和用户数据的安全。本章节详细介绍 Puppet 的安全特性、潜在风险以及安全最佳实践。

## 安全架构

Puppet 采用纵深防御的安全策略，在多个层面提供保护：

```
┌─────────────────────────────────────────┐
│         应用层安全                       │
│  ┌───────────────────────────────────┐  │
│  │  权限确认对话框                    │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         通信层安全                       │
│  ┌───────────────────────────────────┐  │
│  │  密钥验证                          │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         文件系统安全                     │
│  ┌───────────────────────────────────┐  │
│  │  路径保护                          │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         数据层安全                       │
│  ┌───────────────────────────────────┐  │
│  │  PUP 加密                          │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

## 核心安全特性

### 1. 通信安全

#### 密钥验证机制

所有 JavaScript 和 C# 之间的通信都经过密钥验证，防止未授权的访问。

```csharp
// 生成随机密钥
string secret = SecretKey.GenerateKey();

// 存储密钥
SecretKey.Key = secret;

// 注入到 JavaScript
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.PUPPET_SECRET = '" + secret + @"';
");
```

**工作原理**：

1. 应用启动时生成随机密钥
2. 密钥注入到 JavaScript 环境
3. 所有请求必须包含正确的密钥
4. C# 层验证密钥后处理请求

**验证代码**：

```csharp
private async void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    var message = JsonConvert.DeserializeObject<WebMessage>(e.WebMessageAsJson);
    
    // 验证密钥
    if (message.Secret != SecretKey.Key)
    {
        puppet.log.warn("拒绝未授权的请求");
        return;
    }
    
    // 处理请求
    var result = await RouteToController(message);
    webView21.CoreWebView2.PostWebMessageAsJson(result);
}
```

::: tip 安全提示
密钥每次应用启动时都会重新生成，确保不同会话之间的隔离。
:::

### 2. 文件系统保护

#### 路径保护

自动阻止访问 Windows 系统敏感目录，防止恶意操作。

```csharp
private static readonly string[] ProtectedPaths = {
    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
    Environment.GetFolderPath(Environment.SpecialFolder.System),
    Environment.GetFolderPath(Environment.SpecialFolder.SystemX86),
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
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

**受保护的路径**：

- `C:\Windows` - Windows 系统目录
- `C:\Windows\System32` - 系统文件目录
- `C:\Windows\SysWOW64` - 64 位系统文件目录
- `C:\Program Files` - 程序文件目录
- `C:\Program Files (x86)` - 32 位程序文件目录

**使用示例**：

```csharp
public async Task<string> ReadFile(string path)
{
    // 检查路径是否受保护
    if (IsProtectedPath(path))
    {
        throw new UnauthorizedAccessException("无法访问系统目录");
    }
    
    // 读取文件
    return File.ReadAllText(path);
}
```

#### 路径规范化

防止路径遍历攻击，确保路径安全。

```csharp
private string NormalizePath(string path)
{
    // 获取完整路径
    string fullPath = Path.GetFullPath(path);
    
    // 规范化路径分隔符
    fullPath = fullPath.Replace('/', '\\');
    
    // 验证路径格式
    if (path.Contains("..") || path.Contains("~"))
    {
        throw new ArgumentException("非法的路径格式");
    }
    
    return fullPath;
}
```

**攻击示例**：

```javascript
// 尝试路径遍历攻击
puppet.fs.readFileAsText("../../Windows/System32/config.txt");
// 会被拦截，抛出异常
```

### 3. 权限确认

#### 危险操作确认

对于可能影响系统安全的操作，弹出用户确认对话框。

```csharp
public async Task ExecuteCommand(string command)
{
    // 检查是否为系统命令
    if (IsSystemCommand(command))
    {
        var result = PermissionDialog.Show(
            "执行系统命令",
            $"确定要执行以下命令吗？\n\n{command}",
            PermissionDialogType.Warning
        );
        
        if (result != DialogResult.Yes)
        {
            throw new OperationCanceledException("用户取消了操作");
        }
    }
    
    // 执行命令
    Process.Start(command);
}
```

**需要确认的操作**：

- 执行系统目录下的程序
- 写入系统目录
- 删除重要文件
- 执行系统命令
- 修改注册表

**权限对话框选项**：

- **允许**：允许本次操作
- **拒绝**：拒绝本次操作
- **永久阻止**：永久阻止此类操作（记住选择）

### 4. PUP 加密

#### 文件加密

PUP 文件使用 AES-256 加密保护源代码。

```csharp
// 加密 ZIP 密码
string encryptedPassword = AesHelper.Encrypt(zipPassword, "ILOVEPUPPET");

// 解密 ZIP 密码
string zipPassword = AesHelper.Decrypt(encryptedPassword, "ILOVEPUPPET");
```

**加密特性**：

- 算法：AES (Advanced Encryption Standard)
- 密钥长度：256 位
- 模式：CBC (Cipher Block Chaining)
- 填充：PKCS7

::: warning 安全限制
加密的 ZIP 密钥使用固定的密钥 `"ILOVEPUPPET"` 加密，这是一种轻量级的保护方式。如果需要更强的安全性，建议使用文件系统加密（如 BitLocker）。
:::

### 5. 数据签名验证

#### 签名机制

Puppet Storage API 提供了基于自签名证书的数据签名验证机制，防止数据库被篡改。此机制参考 Android APK 签名设计，提供企业级的数据完整性保护。

**签名架构**：

```
┌─────────────────────────────────────────────┐
│  应用层                                   │
│  ┌─────────────────────────────────────┐  │
│  │  RSA 2048/4096 位密钥对            │  │
│  │  自签名 X.509 证书                  │  │
│  │  SHA256withRSA 签名                │  │
│  └─────────────────────────────────────┘  │
├─────────────────────────────────────────────┤
│  验证层                                   │
│  ┌─────────────────────────────────────┐  │
│  │  证书有效性验证                      │  │
│  │  自签名状态检查                      │  │
│  │  签名完整性验证                      │  │
│  │  证书指纹比对                        │  │
│  └─────────────────────────────────────┘  │
├─────────────────────────────────────────────┤
│  数据层                                   │
│  ┌─────────────────────────────────────┐  │
│  │  数据库元数据表                      │  │
│  │  应用ID (AppID)                      │  │
│  │  证书指纹                           │  │
│  │  签名数据                           │  │
│  └─────────────────────────────────────┘  │
└─────────────────────────────────────────────┘
```

#### 安全特性

**1. 数据完整性保护**

- 使用 SHA256withRSA 算法对数据库内容进行签名
- 任何对数据库的修改都会导致签名验证失败
- 提供防篡改保证

**2. 身份验证**

- 通过证书的应用ID（CN）验证数据库创建者
- 通过证书指纹确保证书的唯一性
- 防止冒名顶替

**3. 密钥保护**

- 私钥在 PUP 文件中使用 AES-256-GCM 加密存储
- 密钥派生使用 PBKDF2（100,000次迭代）
- 防止私钥泄露

**4. 算法强度**

- RSA 密钥：2048 或 4096 位
- 签名算法：SHA256withRSA
- 加密算法：AES-256-GCM
- 密钥派生：PBKDF2 + SHA256

#### 签名流程

**1. 生成签名密钥对**

```bash
# 交互式生成
puppet.exe --generate-signing-key --interactive
```

生成的文件：
- `app.crt` - 自签名证书（包含公钥）
- `app.key` - RSA 私钥（加密存储）

**2. 创建带签名的 PUP 文件**

```bash
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

**3. 数据库自动签名**

当使用包含证书的 PUP 文件时：

- **创建新数据库**：自动使用私钥签名
- **打开已有数据库**：自动验证签名
- **验证失败**：记录警告，但允许访问（向后兼容）

#### 验证流程

**签名验证步骤**：

```
1. 读取数据库内容
2. 计算数据库的 SHA256 哈希
3. 从数据库元数据读取签名
4. 从 PUP 文件提取证书
5. 使用证书公钥验证签名
6. 检查证书有效性和自签名状态
7. 验证通过 → 允许访问
8. 验证失败 → 记录警告
```

**验证失败处理**：

```
┌─────────────────────────────────────────────┐
│  验证失败场景                               │
├─────────────────────────────────────────────┤
│  1. 数据库被篡改                            │
│     → 警告："数据库签名验证失败"             │
│     → 仍允许访问（向后兼容）                 │
├─────────────────────────────────────────────┤
│  2. 证书不匹配                             │
│     → 警告："证书指纹不匹配"                 │
│     → 仍允许访问（向后兼容）                 │
├─────────────────────────────────────────────┤
│  3. 证书已过期                             │
│     → 警告："证书已过期"                     │
│     → 仍允许访问（向后兼容）                 │
└─────────────────────────────────────────────┘
```

#### 安全注意事项

**1. 私钥保护**

```csharp
// 私钥在 PUP 文件中加密存储
byte[] encryptedPrivateKey = CryptoUtils.EncryptWithPassword(
    privateKeyBytes,
    password
);

// 解密时需要正确的密码
var privateKey = AppSignatureGenerator.DecryptPrivateKey(
    encryptedPrivateKey,
    password
);
```

**2. 证书有效期**

- 建议设置较长的有效期（如 25 年）
- 定期检查证书到期时间
- 过期前生成新证书并重新签名

**3. 密钥备份**

```bash
# 备份证书和私钥
copy app.crt app.crt.backup
copy app.key app.key.backup

# 存储在安全的位置
# 不要上传到公共仓库
```

**4. 签名不可逆**

- 一旦签名，不能修改签名而不破坏验证
- 修改数据库内容会导致签名验证失败
- 需要重新签名才能修改

#### 安全最佳实践

**1. 始终使用签名**

```bash
# 创建 PUP 文件时始终包含证书和私钥
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

**2. 定期验证签名**

```bash
# 定期验证数据库签名
puppet.exe --verify-database default.db --certificate app.crt
```

**3. 监控签名验证失败**

```javascript
// 在应用中监控签名验证失败
const signatureLogs = await puppet.log.getLogs();
const failures = signatureLogs.filter(log => 
  log.message.includes('签名验证失败')
);

if (failures.length > 0) {
  console.warn('发现签名验证失败:', failures);
}
```

**4. 保护密钥文件**

```bash
# 设置文件权限（Linux/macOS）
chmod 600 app.key

# 设置文件权限（Windows）
icacls app.key /inheritance:r
icacls app.key /grant:r "%USERNAME%:F"
```

**5. 使用强密钥**

```bash
# 使用 4096 位密钥（更高安全性）
puppet.exe --generate-signing-key --key-size 4096
```

#### 安全威胁分析

**1. 中间人攻击**

- **风险**：攻击者拦截 PUP 文件并替换证书
- **防护**：证书指纹验证，确保证书未被替换

**2. 重放攻击**

- **风险**：攻击者使用旧的签名数据库替换新数据库
- **防护**：签名包含时间戳，可以检测重放

**3. 密钥泄露**

- **风险**：私钥被泄露，攻击者可以伪造签名
- **防护**：私钥加密存储，定期轮换密钥

**4. 证书伪造**

- **风险**：攻击者创建假证书并签名恶意数据库
- **防护**：检查证书指纹和自签名状态

#### 安全审计

**签名检查清单**：

- [ ] 所有 PUP 文件都包含证书和私钥
- [ ] 私钥文件权限设置正确
- [ ] 定期验证数据库签名
- [ ] 监控签名验证失败事件
- [ ] 证书有效期设置合理
- [ ] 密钥备份在安全位置
- [ ] 使用足够强度的密钥（2048+位）
- [ ] 私钥使用强密码保护

**日志监控**：

```javascript
// 监控签名相关事件
async function monitorSignatureEvents() {
  const logs = await puppet.log.getLogs();
  
  const signatureEvents = logs.filter(log =>
    log.message.includes('签名') ||
    log.message.includes('证书') ||
    log.message.includes('fingerprint')
  );
  
  for (const event of signatureEvents) {
    console.log(`[${event.time}] ${event.level}: ${event.message}`);
  }
}
```

## 安全最佳实践

### 1. 输入验证

#### JavaScript 层

```javascript
// 验证文件路径
async function safeReadFile(path) {
    // 基本验证
    if (!path || typeof path !== 'string') {
        throw new Error('无效的路径');
    }
    
    // 检查路径长度
    if (path.length > 260) {
        throw new Error('路径过长');
    }
    
    // 检查非法字符
    const invalidChars = /[<>:"|?*]/;
    if (invalidChars.test(path)) {
        throw new Error('路径包含非法字符');
    }
    
    // 读取文件
    return await puppet.fs.readFileAsText(path);
}
```

#### C# 层

```csharp
public async Task<string> ReadFile(string path)
{
    // 验证路径
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new ArgumentNullException(nameof(path));
    }
    
    // 规范化路径
    path = NormalizePath(path);
    
    // 检查路径保护
    if (IsProtectedPath(path))
    {
        throw new UnauthorizedAccessException("无法访问系统目录");
    }
    
    // 读取文件
    return File.ReadAllText(path);
}
```

### 2. 错误处理

#### 安全的错误消息

```javascript
try {
    const result = await puppet.fs.readFileAsText(path);
    return result;
} catch (error) {
    // 不要暴露敏感信息
    console.error('读取文件失败');
    throw new Error('无法读取文件');
}
```

#### 日志记录

```javascript
// 记录安全事件
puppet.log.warn('尝试访问受保护的路径: ' + path);
puppet.log.error('权限被拒绝');
```

### 3. 权限最小化

#### 最小权限原则

只授予应用所需的最小权限：

```javascript
// 只请求必要的文件访问权限
const file = await puppet.fs.readFileAsText('config.json');

// 避免直接执行系统命令
// 不推荐：
// await puppet.application.execute('cmd /c del C:\\Windows\\file.txt');
```

#### 限制访问范围

```javascript
// 限制在应用目录内操作
const appPath = await puppet.application.getAssemblyDirectory();
const configPath = appPath + '\\config.json';
const config = await puppet.fs.readFileAsText(configPath);
```

### 4. 数据保护

#### 敏感数据加密

```javascript
// 加密敏感数据
function encryptData(data, key) {
    // 使用 Web Crypto API 加密
    // ...
}

// 存储加密后的数据
await puppet.fs.writeTextToFile('encrypted.dat', encryptedData);
```

#### 安全存储

```javascript
// 使用 puppet.json 存储配置（相对安全）
await puppet.application.setConfig('apiKey', encryptedKey);

// 不要在代码中硬编码敏感信息
// 不推荐：
// const apiKey = 'my-secret-key-12345';
```

## 安全审计

### 常见安全风险

#### 1. 路径遍历攻击

**风险**：通过特殊字符访问系统文件

**防护**：
- 路径规范化
- 路径保护列表
- 输入验证

#### 2. 命令注入

**风险**：通过命令执行恶意代码

**防护**：
- 权限确认对话框
- 命令白名单
- 参数验证

#### 3. 未授权访问

**风险**：未验证的请求访问系统资源

**防护**：
- 密钥验证机制
- 请求签名
- 会话管理

#### 4. 数据泄露

**风险**：敏感信息被泄露

**防护**：
- 数据加密
- 安全日志
- 错误处理

### 安全检查清单

在发布应用前，请检查以下项目：

- [ ] 所有文件路径都经过验证
- [ ] 敏感操作都有权限确认
- [ ] 不在代码中硬编码敏感信息
- [ ] 使用加密保护 PUP 文件
- [ ] 实施适当的错误处理
- [ ] 记录安全相关事件
- [ ] 限制应用的访问范围
- [ ] 定期更新依赖库
- [ ] 进行安全测试

## 安全工具

### 文件扫描

使用 Windows Defender 扫描 PUP 文件：

```bash
# 扫描单个文件
MpCmdRun.exe -Scan -ScanType 3 -File "app.pup"

# 扫描整个目录
MpCmdRun.exe -Scan -ScanType 3 -File "C:\MyApp"
```

### 代码审计

定期审计代码中的安全问题：

```javascript
// 检查是否有硬编码的密钥
const hasHardcodedKey = code.includes('password') || code.includes('secret');

// 检查是否有危险的 eval 调用
const hasEval = code.includes('eval(');

// 检查是否有直接的命令执行
const hasCommand = code.includes('Application.execute');
```

### 日志分析

监控应用日志，发现异常行为：

```javascript
// 记录所有文件访问
puppet.log.info('访问文件: ' + path);

// 记录所有命令执行
puppet.log.warn('执行命令: ' + command);

// 记录所有错误
puppet.log.error('操作失败: ' + error.message);
```

## 相关资源

### Microsoft Learn

- [Windows 安全最佳实践](https://learn.microsoft.com/en-us/windows/security/)：Windows 安全指南
- [.NET 安全编码](https://learn.microsoft.com/en-us/dotnet/standard/security/)：.NET 安全编程
- [WebView2 安全性](https://learn.microsoft.com/en-us/microsoft-edge/webview2/security/)：WebView2 安全指南

### Mozilla

- [Web 安全](https://developer.mozilla.org/en-US/docs/Web/Security)：Web 安全指南
- [HTTPS](https://developer.mozilla.org/en-US/docs/Web/Security/HTTPS)：HTTPS 最佳实践
- [CSP](https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP)：内容安全策略

## 下一步

了解安全机制后，建议：

1. 审查您的应用代码，查找安全漏洞
2. 实施安全检查清单中的项目
3. 参考 [最佳实践](./best-practices.md) 提升应用安全性
4. 定期进行安全测试和审计