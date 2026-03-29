---
title: PUP 文件格式
permalink: /guide/pup-format.html
createTime: 2026/03/28 14:55:17
---

# PUP 文件格式

PUP（Puppet Package）是 Puppet 框架专用的应用打包格式。它将整个 Web 应用打包为一个独立的文件，支持加密保护，便于分发和部署。

## 概述

PUP 文件是一种自定义的打包格式，结合了 ZIP 压缩和 AES 加密技术，具有以下特点：

- **单文件分发**：所有资源打包为一个文件
- **密码保护**：支持 AES-256 加密
- **快速加载**：优化的文件结构和加载机制
- **跨版本兼容**：版本标识确保兼容性

## 文件结构

### 版本概述

PUP 文件格式支持多个版本：

- **V1.0**：基础版本，支持 ZIP 打包和加密
- **V1.1**：增强版本，支持启动脚本功能
- **V1.2**：签名版本，支持证书和私钥，用于数据库签名验证

### V1.0 二进制结构

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.0 标识头 (8 字节)                   │
├─────────────────────────────────────────────────────┤
│           AES 加密的 ZIP 密码 (32 字节)              │
├─────────────────────────────────────────────────────┤
│           ZIP 数据 (变长)                            │
└─────────────────────────────────────────────────────┘
```

### V1.1 二进制结构

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.1 标识头 (8 字节)                   │
├─────────────────────────────────────────────────────┤
│           脚本长度 (4 字节，int32)                   │
├─────────────────────────────────────────────────────┤
│           启动脚本内容 (变长)                        │
├─────────────────────────────────────────────────────┤
│           AES 加密的 ZIP 密码 (32 字节)              │
├─────────────────────────────────────────────────────┤
│           ZIP 数据 (变长)                            │
└─────────────────────────────────────────────────────┘
```

### V1.2 二进制结构（带签名支持）

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.2 标识头 (8 字节)                   │
├─────────────────────────────────────────────────────┤
│           脚本长度 (4 字节，int32)                   │
├─────────────────────────────────────────────────────┤
│           启动脚本内容 (变长)                        │
├─────────────────────────────────────────────────────┤
│           证书长度 (4 字节，int32)                   │
├─────────────────────────────────────────────────────┤
│           证书数据 (变长，DER 格式)                  │
├─────────────────────────────────────────────────────┤
│           加密私钥长度 (4 字节，int32)               │
├─────────────────────────────────────────────────────┤
│           加密的私钥数据 (变长)                      │
├─────────────────────────────────────────────────────┤
│           AES 加密的 ZIP 密码 (32 字节)              │
├─────────────────────────────────────────────────────┤
│           ZIP 数据 (变长)                            │
└─────────────────────────────────────────────────────┘
```

**V1.2 安全特性**：

- **证书保护**：使用自签名 X.509 证书进行签名验证
- **私钥加密**：私钥使用 AES-256-GCM 加密，密钥通过 PBKDF2 派生
- **数据库签名**：支持对 SQLite 数据库进行签名和验证
- **指纹验证**：通过证书指纹确保证书未被替换

### 详细说明

#### 1. 标识头（8 字节）

固定字符串，用于识别文件格式和版本。

- V1.0: `"PUP V1.0"`
- V1.1: `"PUP V1.1"`

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
```

#### 2. 加密的 ZIP 密码（32 字节）

ZIP 文件的解压密码，使用固定密钥 `"ILOVEPUPPET"` 进行 AES 加密。

```csharp
// 固定加密密钥
private static readonly byte[] FixedKey = Encoding.UTF8.GetBytes("ILOVEPUPPET");

// 加密 ZIP 密码
string encryptedPassword = AesHelper.Encrypt(zipPassword, new string(FixedKey));
```

#### 3. ZIP 数据

包含整个应用文件的 ZIP 压缩数据。

## 创建 PUP 文件

### V1.0 格式

#### 命令行方式

```bash
puppet.exe --create-pup -i <源文件夹> -o <输出文件.pup> [-p <密码>]
```

**参数说明**：

- `-i` 或 `--input`：源文件夹路径
- `-o` 或 `--output`：输出 PUP 文件路径
- `-p` 或 `--password`：（可选）ZIP 密码，用于加密

**示例**：

```bash
# 无密码创建
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"

# 使用密码创建
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword"
```

#### 代码方式（C#）

使用 `PupCreator` 类创建 PUP 文件：

```csharp
using Puppet;

// 创建 PUP 文件
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword"  // 可选
);
```

### V1.1 格式（带启动脚本）

#### 命令行方式

```bash
puppet.exe --create-pup -i <源文件夹> -o <输出文件.pup> [-p <密码>] -v 1.1 --script <脚本文件>
```

**参数说明**：

- `-i` 或 `--input`：源文件夹路径
- `-o` 或 `--output`：输出 PUP 文件路径
- `-p` 或 `--password`：（可选）ZIP 密码，用于加密
- `-v` 或 `--version`：PUP 版本，V1.1 需要指定
- `--script`：启动脚本文件路径（V1.1 必需）

**示例**：

```bash
# 创建 V1.1 格式的 PUP 文件
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.1 --script "C:\script.txt"

# 创建带密码的 V1.1 格式
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword" -v 1.1 --script "C:\script.txt"
```

#### 代码方式（C#）

使用 `PupCreator` 类创建 V1.1 格式的 PUP 文件：

```csharp
using Puppet;

// 创建 V1.1 格式的 PUP 文件
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword",  // 可选
    version: "1.1",                // 指定版本
    scriptFile: @"C:\script.txt"   // 启动脚本文件
);
```

## 启动脚本（V1.1）

### 概述

V1.1 格式支持在 PUP 加载后自动执行预设脚本，实现快速初始化窗口状态。

### 脚本语法

启动脚本使用简单的命令语法，每行一个命令：

```
set <属性> <值>
```

### 支持的命令

#### 1. 设置启动位置

**语法**：
```
set startup_position <x>,<y>
set startup_position <POSITION>
```

**参数**：
- `<x>,<y>`：指定坐标，例如 `100,200`
- `<POSITION>`：预定义位置，支持以下值：
  - `left-top`：左上角
  - `left-bottom`：左下角（排除任务栏）
  - `right-top`：右上角
  - `right-bottom`：右下角（排除任务栏）
  - `center`：屏幕中心

**示例**：
```
set startup_position 100,200
set startup_position center
set startup_position right-bottom
```

#### 2. 设置无边框模式

**语法**：
```
set borderless <true|false>
```

**参数**：
- `true`：启用无边框模式
- `false`：禁用无边框模式

**示例**：
```
set borderless true
set borderless false
```

#### 3. 设置窗口大小

**语法**：
```
set window_size <width>,<height>
```

**参数**：
- `<width>`：窗口宽度（像素）
- `<height>`：窗口高度（像素）

**示例**：
```
set window_size 800,600
set window_size 1024,768
```

### 脚本示例

**示例 1：右下角无边框窗口**
```
set startup_position right-bottom
set borderless true
set window_size 400,300
```

**示例 2：屏幕中心有边框窗口**
```
set startup_position center
set borderless false
set window_size 1024,768
```

**示例 3：指定位置和大小**
```
set startup_position 100,100
set borderless true
set window_size 800,600
```

### 脚本文件示例

创建一个名为 `startup.txt` 的脚本文件：

```
# Puppet 启动脚本
# 设置窗口为右下角无边框窗口
set startup_position right-bottom
set borderless true
set window_size 500,400
```

### 脚本执行时机

- 脚本在 PUP 文件加载后执行
- 在 WebView2 初始化完成后执行
- 在页面导航到应用 URL 之前执行
- 脚本执行错误不会阻止应用启动

### 脚本限制

- 每行只能包含一个命令
- 命令不区分大小写
- 支持 `//` 和 `#` 开头的注释
- 空行会被忽略

### 代码方式（C#）

使用 `PupCreator` 类创建 PUP 文件：

```csharp
using Puppet;

// 创建 PUP 文件
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword"  // 可选
);
```

### V1.2 格式（带签名支持）

#### 命令行方式

```bash
puppet.exe --create-pup -i <源文件夹> -o <输出文件.pup> [-p <密码>] -v 1.2 --certificate <证书文件> --private-key <私钥文件> --private-key-password <私钥密码>
```

**参数说明**：

- `-i` 或 `--input`：源文件夹路径
- `-o` 或 `--output`：输出 PUP 文件路径
- `-p` 或 `--password`：（可选）ZIP 密码，用于加密
- `-v` 或 `--version`：PUP 版本，V1.2 需要指定
- `--certificate`：证书文件路径（V1.2 必需）
- `--private-key`：私钥文件路径（V1.2 必需）
- `--private-key-password`：私钥加密密码（V1.2 必需）

**示例**：

```bash
# 创建 V1.2 格式的 PUP 文件
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"

# 创建带密码和签名的 V1.2 格式
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"
```

#### 代码方式（C#）

使用 `PupCreator` 类创建 V1.2 格式的 PUP 文件：

```csharp
using Puppet;

// 创建 V1.2 格式的 PUP 文件
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword",  // 可选
    version: "1.2",                // 指定版本
    certificate: @"C:\app.crt",    // 证书文件
    privateKey: @"C:\app.key",     // 私钥文件
    privateKeyPassword: "MyKeyPassword"  // 私钥加密密码
);
```

#### 证书和私钥生成

使用 `puppet-sign` 工具生成签名密钥对：

```bash
# 生成签名密钥对
puppet-sign.exe --generate-signing-key --alias MyApp --organization MyOrg --country CN --validity 3650
```

生成的文件：
- `app.crt` - 自签名证书（包含公钥）
- `app.key` - RSA 私钥（PKCS#8 格式）

#### 数据库签名

使用 V1.2 格式的 PUP 文件时，数据库会自动支持签名功能：

```csharp
// 在代码中对数据库进行签名
StorageController storage = new StorageController(form);
storage.SignDatabase("default");
```

签名后的数据库会在 `puppet_metadata` 表中存储：
- `app_id` - 应用标识符（来自证书的 CN）
- `certificate_fingerprint` - 证书指纹（SHA256）
- `signature_data` - 签名数据（使用私钥签名）
- `created_at` - 签名时间戳

#### 签名验证

当加载 V1.2 格式的 PUP 文件时，系统会自动验证数据库签名：

```csharp
// PupServer 会自动验证签名
var server = new PupServer("myapp.pup", 7738);

// 首次访问数据库时会验证签名
storage.SetItem("default", "key", "value");
```

验证失败会输出警告信息：
```
✗ Database signature verification failed: default
  WARNING: Database may have been tampered with
```

## 加载 PUP 文件

### 命令行方式

```bash
puppet.exe --load-pup <文件.pup>
```

**示例**：

```bash
# 加载 V1.0 格式
puppet.exe --load-pup "C:\MyApp.pup"

# 加载 V1.1 格式（会自动执行启动脚本）
puppet.exe --load-pup "C:\MyAppV1_1.pup"

# 加载 V1.2 格式（会自动加载证书和私钥，支持数据库签名）
puppet.exe --load-pup "C:\MyAppV1_2.pup"
```

**版本自动识别**：

PupServer 会自动识别 PUP 文件的版本：

- V1.0：解析标识头 `"PUP V1.0"`
- V1.1：解析标识头 `"PUP V1.1"` 并执行启动脚本
- V1.2：解析标识头 `"PUP V1.2"`，加载证书和私钥，支持数据库签名

### 配置文件方式

编辑 `puppet.ini` 文件：

```ini
[file]
file=C:\MyApp.pup
```

然后直接运行 `puppet.exe`。

### 加载流程

#### V1.0 加载流程

```
1. 读取文件前 8 字节，验证标识头 "PUP V1.0"
        ↓
2. 读取接下来的 32 字节（加密的 ZIP 密码）
        ↓
3. 使用固定密钥解密 ZIP 密码
        ↓
4. 读取剩余数据（ZIP 数据）
        ↓
5. 使用解密的 ZIP 密码解压 ZIP 数据
        ↓
6. 提取文件到内存或临时目录
```

#### V1.1 加载流程

```
1. 读取文件前 8 字节，验证标识头 "PUP V1.1"
        ↓
2. 读取接下来的 4 字节（脚本长度）
        ↓
3. 读取启动脚本内容（变长）
        ↓
4. 读取接下来的 32 字节（加密的 ZIP 密码）
        ↓
5. 使用固定密钥解密 ZIP 密码
        ↓
6. 读取剩余数据（ZIP 数据）
        ↓
7. 使用解密的 ZIP 密码解压 ZIP 数据
        ↓
8. 提取文件到内存或临时目录
        ↓
9. 执行启动脚本（在 WebView2 初始化完成后）
```

#### V1.2 加载流程（带签名支持）

```
1. 读取文件前 8 字节，验证标识头 "PUP V1.2"
        ↓
2. 读取接下来的 4 字节（脚本长度）
        ↓
3. 读取启动脚本内容（变长）
        ↓
4. 读取接下来的 4 字节（证书长度）
        ↓
5. 读取证书数据（变长，DER 格式）
        ↓
6. 解析证书并提取公钥和证书指纹
        ↓
7. 读取接下来的 4 字节（加密私钥长度）
        ↓
8. 读取加密的私钥数据（变长）
        ↓
9. 使用 PBKDF2 派生密钥并解密私钥（AES-256-GCM）
        ↓
10. 读取接下来的 32 字节（加密的 ZIP 密码）
        ↓
11. 使用固定密钥解密 ZIP 密码
        ↓
12. 读取剩余数据（ZIP 数据）
        ↓
13. 使用解密的 ZIP 密码解压 ZIP 数据
        ↓
14. 提取文件到内存或临时目录
        ↓
15. 执行启动脚本（如果有）
        ↓
16. 存储证书和私钥参数，用于数据库签名
```

## 加密机制

### 加密流程

```
1. 生成随机 ZIP 密码
        ↓
2. 使用固定密钥 "ILOVEPUPPET" 加密 ZIP 密码
        ↓
3. 创建 ZIP 文件（使用 ZIP 密码加密）
        ↓
4. 拼接：标识头 + 加密的 ZIP 密码 + ZIP 数据
        ↓
5. 写入 PUP 文件
```

### 解密流程

```
1. 读取文件前 8 字节，验证标识头
        ↓
2. 读取接下来的 32 字节（加密的 ZIP 密码）
        ↓
3. 使用固定密钥解密 ZIP 密码
        ↓
4. 读取剩余数据（ZIP 数据）
        ↓
5. 使用解密的 ZIP 密码解压 ZIP 数据
        ↓
6. 提取文件到内存或临时目录
```

### 加密算法

PUP 使用以下加密算法：

- **加密算法**：AES (Advanced Encryption Standard)
- **密钥长度**：256 位
- **模式**：CBC (Cipher Block Chaining)
- **填充**：PKCS7

::: tip 安全说明
加密的 ZIP 密码使用固定的密钥 `"ILOVEPUPPET"` 加密，这是一种轻量级的保护方式。如果需要更强的安全性，建议使用文件系统加密（如 BitLocker）或分发时使用 HTTPS。
:::

## ZIP 密码生成

如果未指定密码，系统会自动生成随机密码：

```csharp
private static string GenerateRandomPassword()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, 16)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}
```

## 文件验证

PUP 服务器在加载时会验证文件格式：

```csharp
public bool LoadPupFile()
{
    // 1. 读取文件
    byte[] fileData = File.ReadAllBytes(_pupFilePath);
    
    // 2. 验证标识头
    if (fileData.Length < 40)  // 8 (header) + 32 (encrypted password)
        return false;
    
    string header = Encoding.UTF8.GetString(fileData, 0, 8);
    if (header != "PUP V1.0")
        return false;
    
    // 3. 提取加密的 ZIP 密码
    string encryptedPassword = Encoding.UTF8.GetString(fileData, 8, 32);
    
    // 4. 解密 ZIP 密码
    string zipPassword = AesHelper.Decrypt(encryptedPassword, new string(FixedKey));
    
    // 5. 提取 ZIP 数据
    byte[] zipData = new byte[fileData.Length - 40];
    Array.Copy(fileData, 40, zipData, 0, zipData.Length);
    
    // 6. 打开 ZIP 文件
    _zipFile = new ZipInputStream(new MemoryStream(zipData));
    _zipFile.Password = zipPassword;
    
    return true;
}
```

## 性能考虑

### 文件大小

PUP 文件大小取决于：

- 源文件总大小
- ZIP 压缩率（通常 30-70%）
- 加密开销（约 40 字节）

**优化建议**：

- 压缩图片和媒体文件
- 移除未使用的资源
- 使用 CDN 加载第三方库

### 加载速度

PUP 文件加载速度取决于：

- 文件大小
- 磁盘读取速度
- 解密和解压速度

**优化建议**：

- 保持文件大小合理（建议 < 50MB）
- 使用 SSD 提升读取速度
- 预加载常用资源

## 版本兼容性

### 标识头

当前支持的版本：

- V1.0: `"PUP V1.0"` - 基础版本
- V1.1: `"PUP V1.1"` - 增强版本（支持启动脚本）
- V1.2: `"PUP V1.2"` - 签名版本（支持证书和数据库签名）

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
private static readonly byte[] PUP_HEADER_V1_2 = Encoding.UTF8.GetBytes("PUP V1.2");
```

### 版本检测

PupServer 会自动检测 PUP 文件版本并使用相应的解析逻辑：

```csharp
// 示例：版本检测
string header = Encoding.UTF8.GetString(fileData, 0, 8);
switch (header)
{
    case "PUP V1.0":
        // 使用 V1.0 解析逻辑
        LoadPupV1_0(fileBytes);
        break;
    case "PUP V1.1":
        // 使用 V1.1 解析逻辑
        LoadPupV1_1(fileBytes);
        break;
    case "PUP V1.2":
        // 使用 V1.2 解析逻辑（支持签名）
        LoadPupV1_2(fileBytes);
        break;
    default:
        throw new NotSupportedException("不支持的 PUP 版本");
}
```

### 向后兼容性

- V1.1 格式完全兼容 V1.0 的所有功能
- V1.2 格式完全兼容 V1.1 的所有功能
- V1.1 文件包含额外的启动脚本数据
- V1.2 文件包含证书和加密私钥，支持数据库签名
- PupServer 可以加载和解析所有版本的 PUP 文件
- 建议新项目使用 V1.2 格式以获得企业级安全保护

### 版本选择建议

- **V1.0**：适用于简单的应用，不需要启动时配置
- **V1.1**：适用于需要预设窗口状态的应用
- **V1.2**：适用于需要数据库签名和完整性的应用，推荐用于生产环境

### 版本升级

如果需要将 V1.0 升级到 V1.1：

1. 创建启动脚本文件
2. 使用 `-v 1.1` 和 `--script` 参数重新创建 PUP 文件
3. 测试新格式的 PUP 文件

```bash
# 升级到 V1.1
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyAppV1_1.pup" -v 1.1 --script "C:\startup.txt"

# 升级到 V1.2
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyAppV1_2.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"
```

## 与裸文件夹模式的对比

| 特性 | PUP V1.0 | PUP V1.1 | PUP V1.2 | 裸文件夹 |
|------|----------|----------|----------|----------|
| 分发方式 | 单文件 | 单文件 | 单文件 | 文件夹 |
| 加密保护 | 支持 | 支持 | 支持 | 不支持 |
| 启动脚本 | 不支持 | 支持 | 支持 | 不支持 |
| 数据库签名 | 不支持 | 不支持 | 支持 | 不支持 |
| 证书验证 | 不支持 | 不支持 | 支持 | 不支持 |
| 窗口预设 | 代码控制 | 脚本控制 | 脚本控制 | 代码控制 |
| 开发便利性 | 较低 | 较低 | 中等 | 高 |
| 热重载 | 不支持 | 不支持 | 不支持 | 支持 |
| 文件大小 | 较小 | 稍大 | 较大 | 较大 |
| 加载速度 | 稍慢 | 稍慢 | 稍慢 | 快 |
| 安全级别 | 中等 | 中等 | 高 | 低 |
| 适用场景 | 简单应用发布 | 复杂应用发布 | 生产环境发布 | 开发调试 |

## 最佳实践

### 1. 创建 PUP 文件

- 在发布前创建 PUP 文件
- 使用有意义的密码
- 测试 PUP 文件可以正常加载

### 2. 密码管理

- 不要在代码中硬编码密码
- 使用环境变量或配置文件存储密码
- 定期更换密码

### 3. 文件优化

- 压缩图片和媒体文件
- 移除调试代码和注释
- 使用生产环境的构建配置

### 4. 版本控制

- 在文件名中包含版本号
- 保留历史版本的 PUP 文件
- 记录每个版本的变更

### 5. 签名管理（V1.2）

- 使用 puppet-sign 工具生成签名密钥对
- 使用强密码保护私钥（至少 16 位）
- 定期备份证书和私钥文件
- 妥善保管私钥密码，不要提交到版本控制
- 定期检查证书有效期，提前续期
- 对数据库进行签名前先备份数据

### 6. 分发策略

- 使用 HTTPS 分发 PUP 文件
- 提供文件校验（如 MD5、SHA256）
- 包含详细的更新日志
- 对于 V1.2 格式，可以提供证书指纹供验证

## 故障排除

### 常见问题

#### 1. "无效的 PUP 文件"

**原因**：文件格式不正确或已损坏

**解决方案**：
- 重新创建 PUP 文件
- 检查文件是否完整
- 验证文件头是否为 "PUP V1.0"

#### 2. "解密失败"

**原因**：加密密码不正确

**解决方案**：
- 确认创建时使用的密码
- 检查密码是否包含特殊字符
- 重新创建 PUP 文件

#### 3. "文件过大"

**原因**：包含大量资源文件

**解决方案**：
- 压缩图片和媒体文件
- 移除未使用的资源
- 使用 CDN 加载第三方库

#### 4. "加载缓慢"

**原因**：文件较大或磁盘读取速度慢

**解决方案**：
- 优化文件大小
- 使用 SSD
- 预加载常用资源

## 相关资源

- [命令行参数](./cli-parameters.md) - 完整的命令行选项
- [项目结构](./project-structure.md) - 项目目录组织
- [最佳实践](./best-practices.md) - 开发建议
- [安全机制](./security.md) - 签名验证和安全特性（V1.2）

## 下一步

了解 PUP 格式后，建议：

1. 尝试创建您的第一个 PUP 文件
2. 学习 [命令行参数](./cli-parameters.md) 了解更多选项
3. 参考 [最佳实践](./best-practices.md) 优化您的项目
4. 了解 [安全机制](./security.md) 实现数据签名和完整性保护