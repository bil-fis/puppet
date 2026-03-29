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

### 二进制结构

PUP 文件由三个部分组成：

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.0 标识头 (8 字节)                   │
├─────────────────────────────────────────────────────┤
│           AES 加密的 ZIP 密码 (32 字节)              │
├─────────────────────────────────────────────────────┤
│           ZIP 数据 (变长)                            │
└─────────────────────────────────────────────────────┘
```

### 详细说明

#### 1. 标识头（8 字节）

固定字符串 `"PUP V1.0"`，用于识别文件格式和版本。

```csharp
private static readonly byte[] PUP_HEADER = Encoding.UTF8.GetBytes("PUP V1.0");
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

### 命令行方式

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

## 加载 PUP 文件

### 命令行方式

```bash
puppet.exe --load-pup <文件.pup>
```

**示例**：

```bash
puppet.exe --load-pup "C:\MyApp.pup"
```

### 配置文件方式

编辑 `puppet.ini` 文件：

```ini
[file]
file=C:\MyApp.pup
```

然后直接运行 `puppet.exe`。

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

当前版本使用 `"PUP V1.0"` 作为标识头。

```csharp
private static readonly byte[] PUP_HEADER = Encoding.UTF8.GetBytes("PUP V1.0");
```

### 版本升级

如果需要升级 PUP 格式：

1. 更新标识头（如 `"PUP V2.0"`）
2. 修改文件结构
3. 保持向后兼容性

```csharp
// 示例：版本检测
string header = Encoding.UTF8.GetString(fileData, 0, 8);
switch (header)
{
    case "PUP V1.0":
        // 使用 V1.0 解析逻辑
        break;
    case "PUP V2.0":
        // 使用 V2.0 解析逻辑
        break;
    default:
        throw new NotSupportedException("不支持的 PUP 版本");
}
```

## 与裸文件夹模式的对比

| 特性 | PUP 文件 | 裸文件夹 |
|------|----------|----------|
| 分发方式 | 单文件 | 文件夹 |
| 加密保护 | 支持 | 不支持 |
| 开发便利性 | 较低 | 高 |
| 热重载 | 不支持 | 支持 |
| 文件大小 | 较小 | 较大 |
| 加载速度 | 稍慢 | 快 |
| 适用场景 | 发布分发 | 开发调试 |

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

### 5. 分发策略

- 使用 HTTPS 分发 PUP 文件
- 提供文件校验（如 MD5、SHA256）
- 包含详细的更新日志

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

## 下一步

了解 PUP 格式后，建议：

1. 尝试创建您的第一个 PUP 文件
2. 学习 [命令行参数](./cli-parameters.md) 了解更多选项
3. 参考 [最佳实践](./best-practices.md) 优化您的项目