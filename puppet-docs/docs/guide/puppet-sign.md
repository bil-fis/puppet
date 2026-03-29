---
title: Puppet 签名工具
permalink: /guide/puppet-sign.html
createTime: 2026/03/29 19:28:00
---

# Puppet 签名工具

`puppet-sign` 是一个独立的命令行工具，用于生成签名密钥对、对数据库进行签名以及验证签名。该工具主要用于 V1.2 格式的 PUP 文件，为应用提供数字签名和完整性验证功能。

## 功能概述

`puppet-sign` 提供以下功能：

- **生成签名密钥对**：生成自签名 X.509 证书和 RSA 私钥
- **签名数据库**：对 SQLite 数据库文件进行数字签名
- **验证签名**：验证数据库文件的签名有效性

## 安装

`puppet-sign` 随 Puppet 框架一起发布，位于 Puppet 安装目录下：

```bash
E:\puppet\puppet-sign\bin\Debug\net9.0\puppet-sign.exe
```

## 快速开始

### 1. 生成签名密钥对

使用 `--generate-signing-key` 命令生成签名密钥对：

```bash
# 交互式生成
puppet-sign.exe --generate-signing-key --interactive

# 使用命令行参数生成
puppet-sign.exe --generate-signing-key --alias MyApp --validity 3650
```

这将生成两个文件：
- `app.crt` - 自签名 X.509 证书
- `app.key` - RSA 私钥

### 2. 创建带签名的 PUP 文件

使用生成的密钥对创建带签名的 PUP 文件：

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

### 3. 签名数据库（可选）

对独立的数据库文件进行签名：

```bash
puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key
```

## 命令详解

### --generate-signing-key

生成签名密钥对和自签名证书。

**用法**：

```bash
puppet-sign.exe --generate-signing-key [选项]
```

**选项**：

| 选项 | 说明 | 默认值 |
|------|------|--------|
| `--interactive` | 交互式输入证书信息 | - |
| `--alias <name>` | 应用标识符（CN） | - |
| `--organization <org>` | 组织名称（O） | - |
| `--ou <unit>` | 组织单位（OU） | - |
| `--country <code>` | 国家代码（C） | CN |
| `--validity <days>` | 有效期（天） | 9125（25年） |
| `--key-size <2048\|4096>` | 密钥长度（位） | 2048 |
| `--out-cert <file>` | 输出证书文件路径 | app.crt |
| `--out-key <file>` | 输出私钥文件路径 | app.key |

**示例**：

```bash
# 交互式生成（推荐首次使用）
puppet-sign.exe --generate-signing-key --interactive

# 使用默认参数生成
puppet-sign.exe --generate-signing-key --alias MyApp

# 自定义参数生成
puppet-sign.exe --generate-signing-key \
  --alias MyApp \
  --organization MyCompany \
  --ou Development \
  --country CN \
  --validity 3650 \
  --key-size 4096 \
  --out-cert myapp.crt \
  --out-key myapp.key
```

**输出示例**：

```
=== 生成签名密钥对 ===

✓ 证书已保存: app.crt
✓ 私钥已保存: app.key

证书信息:
  应用标识 (CN): MyApp
  组织 (O): MyCompany
  组织单位 (OU): Development
  国家 (C): CN
  有效期: 3650 天
  密钥长度: 4096 位
  证书指纹: 3F5A8C1D9E2B4F7A6C8D1E3F5A7B9C2D

生成成功!
```

### --sign-database

对数据库文件进行数字签名。

**用法**：

```bash
puppet-sign.exe --sign-database <database.db> [选项]
```

**选项**：

| 选项 | 说明 |
|------|------|
| `--certificate <file>` | 证书文件路径（必需） |
| `--private-key <file>` | 私钥文件路径（必需） |

**示例**：

```bash
# 签名数据库
puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key

# 签名指定路径的数据库
puppet-sign.exe --sign-database "C:\MyApp\data.db" --certificate "C:\certs\app.crt" --private-key "C:\keys\app.key"
```

**输出示例**：

```
=== 签名数据库 ===

数据库大小: 8192 bytes
✓ 数据库已签名
  签名大小: 256 bytes
  签名算法: SHA256withRSA

签名成功!
```

**注意事项**：

- 签名会修改数据库文件，添加签名元数据
- 签名后对数据库的任何修改都会导致签名验证失败
- 建议在数据库内容确定后再进行签名
- 证书和私钥必须是一对密钥

### --verify-database

验证数据库文件的签名有效性。

**用法**：

```bash
puppet-sign.exe --verify-database <database.db> [选项]
```

**选项**：

| 选项 | 说明 |
|------|------|
| `--certificate <file>` | 证书文件路径（必需） |

**示例**：

```bash
# 验证数据库签名
puppet-sign.exe --verify-database default.db --certificate app.crt

# 验证指定路径的数据库
puppet-sign.exe --verify-database "C:\MyApp\data.db" --certificate "C:\certs\app.crt"
```

**输出示例**：

```
=== 验证数据库签名 ===

数据库大小: 8192 bytes

验证结果:
  证书指纹: 3F5A8C1D9E2B4F7A6C8D1E3F5A7B9C2D
  应用ID: MyApp

✓ 验证成功!
```

## 证书信息

### 证书格式

`puppet-sign` 生成的证书是标准的 X.509 自签名证书，包含以下信息：

- **Common Name (CN)**：应用标识符
- **Organization (O)**：组织名称
- **Organizational Unit (OU)**：组织单位
- **Country (C)**：国家代码
- **有效期**：证书有效期
- **公钥**：RSA 公钥
- **指纹**：SHA-256 证书指纹

### 密钥对

- **私钥格式**：PKCS#8 PEM 格式
- **公钥算法**：RSA
- **支持的密钥长度**：2048 位或 4096 位
- **签名算法**：SHA256withRSA

## 安全最佳实践

### 1. 保护私钥

私钥文件（`.key`）包含敏感信息，应该：

- 使用强密码保护（PUP 文件中的私钥会被加密）
- 不要将私钥文件提交到版本控制系统
- 安全存储私钥文件，限制访问权限
- 定期轮换密钥对

### 2. 证书管理

- 为不同的应用使用不同的证书
- 记录证书的指纹和应用标识
- 设置合理的证书有效期
- 证书过期前及时更新

### 3. 签名流程

- 在发布前对 PUP 文件进行签名
- 在应用启动时验证签名
- 拒绝运行未签名或签名验证失败的应用
- 对敏感数据库进行签名

### 4. 密钥轮换

```bash
# 1. 生成新的密钥对
puppet-sign.exe --generate-signing-key --alias MyApp-v2 --out-cert app-v2.crt --out-key app-v2.key

# 2. 使用新密钥创建 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -v 1.2 --certificate "app-v2.crt" --private-key "app-v2.key" --private-key-password "NewPassword"

# 3. 安全备份或删除旧密钥
```

## 工作流程示例

### 完整的签名工作流

```bash
# 1. 开发应用
cd C:\MyProject
# ... 开发代码 ...

# 2. 生成签名密钥对
cd E:\puppet\puppet-sign\bin\Debug\net9.0
puppet-sign.exe --generate-signing-key --interactive

# 3. 创建带签名的 PUP 文件
puppet.exe --create-pup \
  -i "C:\MyProject\dist" \
  -o "C:\Releases\MyApp-v1.0.0.pup" \
  -v 1.2 \
  --certificate "app.crt" \
  --private-key "app.key" \
  --private-key-password "MySecurePassword"

# 4. 验证 PUP 文件（可选）
# 在应用中检查签名状态
# 参见 Application API 的 getAppInfo() 方法
```

### 数据库签名工作流

```bash
# 1. 创建并初始化数据库
# ... 创建数据库和表结构 ...

# 2. 填充初始数据
# ... 插入初始数据 ...

# 3. 对数据库进行签名
puppet-sign.exe --sign-database data.db --certificate app.crt --private-key app.key

# 4. 验证签名（可选）
puppet-sign.exe --verify-database data.db --certificate app.crt

# 5. 将签名后的数据库打包到 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

## 常见问题

### Q: 为什么要使用 puppet-sign？

A: `puppet-sign` 提供了以下好处：

- **应用完整性**：确保应用文件未被篡改
- **来源验证**：验证应用的发布者
- **数据安全**：保护数据库不被恶意修改
- **用户信任**：提高用户对应用的信任度

### Q: 可以使用第三方证书吗？

A: 可以。`puppet-sign` 支持使用标准的 X.509 证书。如果您已有 CA 签发的证书，可以直接使用：

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "my-cert.crt" --private-key "my-key.key" --private-key-password "MyPassword"
```

### Q: 签名会影响性能吗？

A: 签名验证的性能影响很小：

- 应用启动时验证签名，大约增加 10-50ms
- 数据库访问时验证签名，几乎无性能影响
- 签名验证只进行一次，不会重复验证

### Q: 如何在应用中检查签名状态？

A: 使用 Application API：

```javascript
// 检查应用签名状态
const appInfo = await puppet.application.getAppInfo();

if (appInfo.hasSignature) {
    console.log('应用已签名');
    console.log('证书指纹:', appInfo.certificateThumbprint);
} else {
    console.warn('应用未签名');
}
```

### Q: 签名后可以修改 PUP 文件吗？

A: 不可以。签名后的 PUP 文件不能被修改，否则签名验证将失败。如果需要修改，需要：

1. 使用原始源代码重新构建
2. 重新创建 PUP 文件
3. 重新签名

### Q: 证书过期后怎么办？

A: 证书过期后：

1. 生成新的密钥对
2. 使用新密钥重新创建和签名 PUP 文件
3. 发布更新版本
4. 用户升级到新版本

### Q: 如何验证签名是否有效？

A: 在应用中使用 Storage API：

```javascript
// 验证数据库签名
const result = await puppet.storage.verifyDatabaseSignature('default');

if (result.isValid) {
    console.log('数据库签名验证通过');
    console.log('证书指纹:', result.certificateThumbprint);
} else {
    console.error('数据库签名验证失败:', result.message);
}
```

## 技术细节

### 签名算法

- **哈希算法**：SHA-256
- **签名算法**：SHA256withRSA
- **密钥长度**：2048 或 4096 位
- **证书格式**：X.509 自签名证书

### 数据库签名流程

1. 读取数据库文件的所有字节
2. 使用 SHA-256 计算数据库内容的哈希值
3. 使用私钥对哈希值进行 RSA 签名
4. 将签名信息存储在数据库的 `__puppet_metadata__` 表中

### 签名验证流程

1. 从数据库的 `__puppet_metadata__` 表读取签名信息
2. 读取数据库文件的所有字节
3. 使用 SHA-256 计算数据库内容的哈希值
4. 使用证书公钥验证签名
5. 检查证书的有效性和自签名状态

## 相关资源

- [PUP 文件格式](./pup-format.md) - 了解 PUP 文件的签名机制
- [安全机制](./security.md) - Puppet 框架的安全特性
- [Application API](../api/application.md) - getAppInfo() 方法
- [Storage API](../api/storage.md) - verifyDatabaseSignature() 方法
- [命令行参数](./cli-parameters.md) - 创建带签名 PUP 文件的参数

## 获取帮助

如果在使用 `puppet-sign` 时遇到问题：

```bash
# 查看帮助信息
puppet-sign.exe --help
```

或查阅本文档的相关章节。