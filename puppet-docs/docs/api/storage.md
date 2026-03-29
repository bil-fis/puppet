---
title: 持久化存储 API
permalink: /api/storage.html
createTime: 2026/03/29 00:00:00
---

# 持久化存储 API

持久化存储 API 提供了基于 SQLite 的键值对存储功能，用于在 Puppet 应用中保存和检索数据。

## 概述

`puppet.storage` 命名空间提供以下功能：

- 键值对存储（类似 localStorage）
- 多数据库支持
- 数据持久化（跨应用启动保留）
- 线程安全操作
- 自动事务管理
- **数据签名验证（防止数据篡改）**

## 签名机制

Puppet Storage API 提供了基于自签名证书的数据签名验证机制，用于防止数据库被篡改。此机制参考 Android APK 签名设计，使用 RSA 2048 位密钥和 SHA256 签名算法。

### 签名原理

1. **自签名证书**：应用使用 RSA 密钥对生成自签名 X.509 证书
2. **数据库签名**：创建数据库时使用私钥对数据库内容进行签名
3. **签名验证**：打开数据库时使用证书公钥验证签名有效性

### 证书信息

每个签名包含以下信息：

- **应用ID (AppID)**：证书的 Common Name (CN)，用于标识应用
- **证书指纹**：SHA256 哈希值，用于唯一标识证书
- **签名算法**：SHA256withRSA
- **签名时间**：数据库签名的日期和时间

### 签名验证流程

```
┌─────────────────────────────────────────────┐
│  1. 打开数据库                                │
│  2. 读取数据库中的签名信息                     │
│  3. 从PUP文件提取应用证书                     │
│  4. 验证证书的有效性和自签名状态                │
│  5. 使用证书公钥验证数据库签名                  │
│  6. 验证通过 → 允许访问                       │
│  7. 验证失败 → 警告但仍允许访问（向后兼容）     │
└─────────────────────────────────────────────┘
```

### PUP 文件中的证书和私钥

当创建 PUP 文件时，可以将证书和私钥嵌入到文件中：

```bash
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

PUP 文件结构（V1.1）：

```
┌──────────────────────────────────────────────┐
│  PUP V1.1                                     │
├──────────────────────────────────────────────┤
│  脚本长度（4字节）                            │
├──────────────────────────────────────────────┤
│  脚本内容（变长）                              │
├──────────────────────────────────────────────┤
│  证书长度（4字节）                            │
├──────────────────────────────────────────────┤
│  证书数据（变长，PEM格式）                   │
├──────────────────────────────────────────────┤
│  私钥长度（4字节）                            │
├──────────────────────────────────────────────┤
│  私钥数据（变长，加密存储）                    │
├──────────────────────────────────────────────┤
│  AES加密密码（32字节）                         │
├──────────────────────────────────────────────┤
│  ZIP数据（变长）                              │
└──────────────────────────────────────────────┘
```

### 生成签名密钥

使用命令行工具生成签名密钥对：

```bash
# 交互式生成
puppet.exe --generate-signing-key --interactive

# 指定输出文件
puppet.exe --generate-signing-key --out-cert app.crt --out-key app.key
```

交互式生成会提示输入以下信息：

```
请输入证书信息:

应用标识符 [MyApp]: MyApp
组织名称 [MyCompany]: MyCompany
组织单位 [Development]: Development
国家 [CN]: CN
省份 []: Beijing
城市 []: Beijing
邮箱 []: admin@example.com
有效期限（年）[25]: 25
密钥长度 [2048]: 2048
```

### 签名数据库

使用命令行工具对现有数据库进行签名：

```bash
puppet.exe --sign-database default.db \
  --certificate app.crt \
  --private-key app.key
```

这会在数据库旁边创建一个 `.sig` 签名文件。

### 验证签名

使用命令行工具验证数据库签名：

```bash
puppet.exe --verify-database default.db --certificate app.crt
```

### 自动签名和验证

当使用包含证书和私钥的 PUP 文件运行 Puppet 时：

1. **创建新数据库**：自动使用嵌入的证书和私钥对数据库进行签名
2. **打开已有数据库**：自动验证签名有效性，失败时记录警告

### 签名元数据表

签名信息存储在数据库的 `__puppet_metadata__` 表中：

```sql
CREATE TABLE __puppet_metadata__ (
    id INTEGER PRIMARY KEY,
    app_id TEXT NOT NULL,           -- 应用标识（证书的CN）
    app_fingerprint TEXT NOT NULL,  -- 证书指纹（SHA256）
    signature BLOB NOT NULL,        -- 数字签名（二进制）
    signature_algorithm TEXT,      -- 签名算法（SHA256withRSA）
    created_at INTEGER NOT NULL,    -- 创建时间
    cert_info TEXT,                 -- 证书信息（JSON）
    version TEXT NOT NULL           -- 版本号
);
```

### 向后兼容性

签名机制是向后兼容的：

- **没有签名的数据库**：可以正常访问，但会记录警告
- **验证失败**：仍然允许访问，但会记录警告信息
- **可选功能**：签名是可选的，不是强制性的

### 安全性

签名机制提供以下安全保障：

1. **数据完整性**：防止数据库被篡改
2. **身份验证**：验证数据库的创建者
3. **防篡改**：任何对数据库的修改都会导致签名验证失败

### 注意事项

1. **私钥保护**：私钥在 PUP 文件中使用 AES-256-GCM 加密存储
2. **证书有效期**：建议设置较长的有效期（如 25 年）
3. **密钥备份**：妥善保管证书和私钥文件，丢失后无法恢复
4. **签名不可逆**：一旦签名，不能修改签名而不破坏验证

## 为什么使用 Storage API？

### vs WebView2 localStorage

| 特性 | WebView2 localStorage | puppet.storage |
|------|---------------------|----------------|
| 数据隔离 | 需要不同的 UserDataFolder | 天然隔离 |
| 资源消耗 | 创建独立浏览器进程 | 轻量级 SQLite |
| 数据格式 | 仅支持字符串 | 支持字符串（推荐 JSON） |
| 多应用隔离 | 需要多个 UDF | 自动隔离 |
| 跨进程访问 | 不支持 | 支持（通过文件） |

### vs 修改 puppet.ini

| 特性 | puppet.ini | puppet.storage |
|------|-----------|----------------|
| 用途 | 框架配置 | 应用数据 |
| 格式 | INI 文本 | SQLite 数据库 |
| 修改方式 | 需要弹窗确认 | 直接修改 |
| 数据结构 | 扁平键值对 | 多数据库支持 |
| 事务支持 | 无 | 支持 |
| 查询能力 | 无 | 支持 |

## 数据库概念

### 数据库（Database）

Storage API 支持多个独立的数据库，每个数据库对应一个 SQLite 文件：

- **默认数据库**：名为 `default`，用于通用存储
- **自定义数据库**：可以创建任意数量的数据库
- **数据库隔离**：不同数据库之间的数据完全隔离

### 存储位置

数据库文件存储在用户的应用数据目录：

```
%APPDATA%\puppet\storage\
├── default.db      # 默认数据库
├── app1.db         # 应用1的数据库
├── app2.db         # 应用2的数据库
└── ...
```

## 方法

### setItem()

设置键值对。

```javascript
await puppet.storage.setItem(database: string, key: string, value: string): Promise<void>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）
- `key` (string) - 键名
- `value` (string) - 值（建议使用 JSON 字符串）

**示例**：

```javascript
// 存储简单字符串
await puppet.storage.setItem('default', 'username', 'john');

// 存储对象（使用 JSON）
const user = { name: 'john', age: 30, email: 'john@example.com' };
await puppet.storage.setItem('default', 'user', JSON.stringify(user));

// 存储数组
const recentFiles = ['file1.txt', 'file2.txt', 'file3.txt'];
await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(recentFiles));

// 存储到自定义数据库
await puppet.storage.setItem('app1', 'settings', JSON.stringify({ theme: 'dark' }));
```

### getItem()

获取键值对。

```javascript
await puppet.storage.getItem(database: string, key: string): Promise<string>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）
- `key` (string) - 键名

**返回值**：

值字符串，如果键不存在返回空字符串。

**示例**：

```javascript
// 获取简单字符串
const username = await puppet.storage.getItem('default', 'username');
console.log(username); // "john"

// 获取对象
const userJson = await puppet.storage.getItem('default', 'user');
const user = JSON.parse(userJson);
console.log(user.name); // "john"
console.log(user.age);  // 30

// 获取数组
const recentFilesJson = await puppet.storage.getItem('default', 'recentFiles');
const recentFiles = JSON.parse(recentFilesJson);
console.log(recentFiles); // ["file1.txt", "file2.txt", "file3.txt"]
```

### removeItem()

删除键值对。

```javascript
await puppet.storage.removeItem(database: string, key: string): Promise<void>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）
- `key` (string) - 键名

**示例**：

```javascript
// 删除单个键
await puppet.storage.removeItem('default', 'username');

// 删除自定义数据库中的键
await puppet.storage.removeItem('app1', 'settings');
```

### clear()

清空指定数据库的所有数据。

```javascript
await puppet.storage.clear(database: string): Promise<void>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）

**示例**：

```javascript
// 清空默认数据库
await puppet.storage.clear('default');

// 清空自定义数据库
await puppet.storage.clear('app1');
```

::: danger 警告
清空数据库会删除所有数据，操作不可恢复！
:::

### getKeys()

获取指定数据库中的所有键。

```javascript
await puppet.storage.getKeys(database: string): Promise<string[]>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）

**返回值**：

键名数组。

**示例**：

```javascript
// 获取所有键
const keys = await puppet.storage.getKeys('default');
console.log(keys); // ["username", "user", "recentFiles"]

// 遍历所有数据
for (const key of keys) {
    const value = await puppet.storage.getItem('default', key);
    console.log(`${key}: ${value}`);
}
```

### hasItem()

检查键是否存在。

```javascript
await puppet.storage.hasItem(database: string, key: string): Promise<boolean>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）
- `key` (string) - 键名

**返回值**：

是否存在。

**示例**：

```javascript
// 检查键是否存在
const hasUsername = await puppet.storage.hasItem('default', 'username');
if (hasUsername) {
    console.log('用户名已存在');
} else {
    console.log('用户名不存在');
}

// 使用示例
async function ensureUserSetup() {
    if (!await puppet.storage.hasItem('default', 'user')) {
        // 首次运行，初始化用户数据
        await puppet.storage.setItem('default', 'user', JSON.stringify({
            name: 'guest',
            language: 'zh-CN'
        }));
    }
}
```

### getSize()

获取数据库大小（字节数）。

```javascript
await puppet.storage.getSize(database: string): Promise<number>
```

**参数**：

- `database` (string) - 数据库名称（默认为 `'default'`）

**返回值**：

数据库文件大小（字节数）。

**示例**：

```javascript
// 获取数据库大小
const size = await puppet.storage.getSize('default');
console.log(`数据库大小: ${size} bytes`);

// 格式化显示
function formatBytes(bytes) {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(2) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
}

console.log(`数据库大小: ${formatBytes(size)}`);
```

### deleteDatabase()

删除整个数据库。

```javascript
await puppet.storage.deleteDatabase(database: string): Promise<void>
```

**参数**：

- `database` (string) - 数据库名称

**示例**：

```javascript
// 删除自定义数据库
await puppet.storage.deleteDatabase('app1');

// 删除默认数据库（谨慎操作）
await puppet.storage.deleteDatabase('default');
```

::: danger 警告
删除数据库会删除所有数据，操作不可恢复！
:::

### getDatabases()

获取所有数据库列表。

```javascript
await puppet.storage.getDatabases(): Promise<string[]>
```

**返回值**：

数据库名称数组。

**示例**：

```javascript
// 获取所有数据库
const databases = await puppet.storage.getDatabases();
console.log(databases); // ["default", "app1", "app2"]

// 遍历所有数据库
for (const db of databases) {
    const size = await puppet.storage.getSize(db);
    const keys = await puppet.storage.getKeys(db);
    console.log(`数据库: ${db}, 大小: ${size} bytes, 键数: ${keys.length}`);
}
```

### verifyDatabaseSignature()

验证数据库签名（V1.2 格式）。

```javascript
await puppet.storage.verifyDatabaseSignature(database: string): Promise<SignatureResult>
```

**参数**：

- `database` (string) - 数据库名称

**返回值**：

```typescript
interface SignatureResult {
    isValid: boolean;           // 签名是否有效
    message: string;            // 验证结果消息
    certificateThumbprint?: string;  // 证书指纹
    signedAt?: Date;            // 签名时间
}
```

**示例**：

```javascript
// 验证数据库签名
const result = await puppet.storage.verifyDatabaseSignature('default');

if (result.isValid) {
    console.log('✓ 数据库签名验证通过');
    console.log('证书指纹:', result.certificateThumbprint);
    console.log('签名时间:', result.signedAt);
} else {
    console.error('✗ 数据库签名验证失败:', result.message);
    // 可以选择拒绝访问或采取其他安全措施
}
```

**注意事项**：

- 仅 V1.2 格式的数据库支持签名验证
- 未签名的数据库会返回 `isValid: false` 和相应消息
- 签名验证失败时数据库仍可访问，但建议记录警告

### signDatabase()

对数据库进行签名（V1.2 格式）。

```javascript
await puppet.storage.signDatabase(database: string): Promise<boolean>
```

**参数**：

- `database` (string) - 数据库名称

**返回值**：

- `boolean` - 签名是否成功

**示例**：

```javascript
// 对数据库进行签名
const success = await puppet.storage.signDatabase('default');

if (success) {
    console.log('✓ 数据库签名成功');
} else {
    console.error('✗ 数据库签名失败');
}
```

**注意事项**：

- 仅 V1.2 格式支持此功能
- 需要 PUP 文件包含有效的证书和私钥
- 数据库只能签名一次，重复签名会覆盖之前的签名
- 签名后对数据库的任何修改都会导致签名验证失败

## 使用示例

### 基本使用

```javascript
// 存储用户设置
async function saveSettings(settings) {
    await puppet.storage.setItem('default', 'settings', JSON.stringify(settings));
}

// 加载用户设置
async function loadSettings() {
    const settingsJson = await puppet.storage.getItem('default', 'settings');
    if (settingsJson) {
        return JSON.parse(settingsJson);
    }
    
    // 返回默认设置
    return {
        theme: 'light',
        language: 'zh-CN',
        fontSize: 14
    };
}

// 使用示例
const settings = await loadSettings();
settings.theme = 'dark';
await saveSettings(settings);
```

### 多应用隔离

```javascript
// 应用1的存储
await puppet.storage.setItem('app1', 'data', JSON.stringify({ value: 'app1 data' }));

// 应用2的存储
await puppet.storage.setItem('app2', 'data', JSON.stringify({ value: 'app2 data' }));

// 互不干扰
const app1Data = JSON.parse(await puppet.storage.getItem('app1', 'data'));
const app2Data = JSON.parse(await puppet.storage.getItem('app2', 'data'));

console.log(app1Data.value); // "app1 data"
console.log(app2Data.value); // "app2 data"
```

### 最近文件列表

```javascript
// 添加最近文件
async function addRecentFile(filePath) {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    const recentFiles = recentJson ? JSON.parse(recentJson) : [];
    
    // 添加到开头
    recentFiles.unshift(filePath);
    
    // 限制数量
    if (recentFiles.length > 10) {
        recentFiles.pop();
    }
    
    // 去重
    const uniqueFiles = [...new Set(recentFiles)];
    
    await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(uniqueFiles));
}

// 获取最近文件
async function getRecentFiles() {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    return recentJson ? JSON.parse(recentJson) : [];
}

// 使用示例
await addRecentFile('C:\\Documents\\file1.txt');
await addRecentFile('C:\\Documents\\file2.txt');

const recentFiles = await getRecentFiles();
console.log(recentFiles); // ["C:\\Documents\\file2.txt", "C:\\Documents\\file1.txt"]
```

### 用户偏好管理

```javascript
class Preferences {
    constructor() {
        this.database = 'default';
        this.key = 'preferences';
        this.defaults = {
            theme: 'light',
            language: 'zh-CN',
            fontSize: 14,
            autoSave: true,
            notifications: true
        };
    }
    
    async load() {
        const prefsJson = await puppet.storage.getItem(this.database, this.key);
        if (prefsJson) {
            return { ...this.defaults, ...JSON.parse(prefsJson) };
        }
        return { ...this.defaults };
    }
    
    async save(preferences) {
        await puppet.storage.setItem(this.database, this.key, JSON.stringify(preferences));
    }
    
    async reset() {
        await this.save(this.defaults);
    }
}

// 使用示例
const prefs = new Preferences();

// 加载偏好
const preferences = await prefs.load();
console.log('当前偏好:', preferences);

// 修改偏好
preferences.theme = 'dark';
preferences.fontSize = 16;
await prefs.save(preferences);

// 重置偏好
await prefs.reset();
```

### 数据库管理

```javascript
// 查看数据库信息
async function showDatabaseInfo() {
    const databases = await puppet.storage.getDatabases();
    
    console.log('=== 数据库信息 ===');
    for (const db of databases) {
        const size = await puppet.storage.getSize(db);
        const keys = await puppet.storage.getKeys(db);
        const sizeMB = (size / (1024 * 1024)).toFixed(2);
        
        console.log(`数据库: ${db}`);
        console.log(`  大小: ${sizeMB} MB`);
        console.log(`  键数: ${keys.length}`);
        console.log(`  键列表: ${keys.join(', ')}`);
        console.log();
    }
}

// 清理大数据库
async function cleanupLargeDatabases() {
    const databases = await puppet.storage.getDatabases();
    const maxSize = 10 * 1024 * 1024; // 10 MB
    
    for (const db of databases) {
        const size = await puppet.storage.getSize(db);
        if (size > maxSize) {
            console.log(`数据库 ${db} 过大 (${size} bytes)，建议清理`);
        }
    }
}

// 使用示例
await showDatabaseInfo();
await cleanupLargeDatabases();
```

## 最佳实践

### 1. 使用 JSON 格式

始终使用 JSON 格式存储复杂对象：

```javascript
// 好的做法
const user = { name: 'john', age: 30 };
await puppet.storage.setItem('default', 'user', JSON.stringify(user));

// 读取时解析
const userJson = await puppet.storage.getItem('default', 'user');
const user = JSON.parse(userJson);

// 避免
await puppet.storage.setItem('default', 'user_name', 'john');
await puppet.storage.setItem('default', 'user_age', '30');
```

### 2. 使用有意义的键名

使用清晰、有意义的键名：

```javascript
// 好的做法
await puppet.storage.setItem('default', 'user_settings', JSON.stringify(settings));
await puppet.storage.setItem('default', 'app_state', JSON.stringify(state));

// 避免
await puppet.storage.setItem('default', 'data1', ...);
await puppet.storage.setItem('default', 'temp', ...);
```

### 3. 错误处理

始终进行错误处理：

```javascript
async function safeGetItem(key) {
    try {
        const value = await puppet.storage.getItem('default', key);
        return value;
    } catch (error) {
        console.error('获取数据失败:', error);
        return null;
    }
}

async function safeSetItem(key, value) {
    try {
        await puppet.storage.setItem('default', key, JSON.stringify(value));
        return true;
    } catch (error) {
        console.error('保存数据失败:', error);
        return false;
    }
}
```

### 4. 数据验证

读取数据后进行验证：

```javascript
async function loadUserData() {
    const userJson = await puppet.storage.getItem('default', 'user');
    
    if (!userJson) {
        return null;
    }
    
    try {
        const user = JSON.parse(userJson);
        
        // 验证数据结构
        if (!user.name || !user.email) {
            console.warn('用户数据格式不正确');
            return null;
        }
        
        return user;
    } catch (error) {
        console.error('解析用户数据失败:', error);
        return null;
    }
}
```

### 5. 定期清理

定期清理不需要的数据：

```javascript
async function cleanupOldFiles() {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    const recentFiles = recentJson ? JSON.parse(recentJson) : [];
    
    // 只保留存在的文件
    const validFiles = [];
    for (const file of recentFiles) {
        if (await puppet.fs.exists(file)) {
            validFiles.push(file);
        }
    }
    
    await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(validFiles));
}
```

### 6. 使用命名空间

使用前缀或命名空间组织数据：

```javascript
// 好的做法 - 使用前缀
await puppet.storage.setItem('default', 'user_profile', JSON.stringify(profile));
await puppet.storage.setItem('default', 'user_preferences', JSON.stringify(prefs));
await puppet.storage.setItem('default', 'user_history', JSON.stringify(history));

// 或者使用对象
const userData = {
    profile: { name: 'john' },
    preferences: { theme: 'dark' },
    history: ['file1', 'file2']
};
await puppet.storage.setItem('default', 'user_data', JSON.stringify(userData));
```

## 性能考虑

### 数据库大小

- SQLite 数据库文件会随数据增长
- 建议定期清理不需要的数据
- 大型数据考虑使用文件系统

### 批量操作

批量操作时考虑事务：

```javascript
// 目前 Storage API 不直接支持事务
// 但可以通过组合操作实现类似效果
async function batchUpdate(updates) {
    const oldData = await puppet.storage.getItem('default', 'data');
    const data = oldData ? JSON.parse(oldData) : {};
    
    // 批量更新
    Object.assign(data, updates);
    
    // 一次性保存
    await puppet.storage.setItem('default', 'data', JSON.stringify(data));
}
```

### 查询优化

对于大量数据，考虑分页或索引：

```javascript
// 分页加载
async function loadPaginatedData(page, pageSize) {
    const allDataJson = await puppet.storage.getItem('default', 'items');
    const allItems = allDataJson ? JSON.parse(allDataJson) : [];
    
    const start = page * pageSize;
    const end = start + pageSize;
    
    return allItems.slice(start, end);
}
```

## 常见问题

### Q: Storage API 和 localStorage 有什么区别？

A: Storage API 使用 SQLite 数据库，支持多数据库隔离和跨应用数据共享，而 localStorage 是浏览器内置的，在同一网站的所有 WebView2 实例间共享数据，会导致数据串。

### Q: 为什么不直接修改 puppet.ini？

A: puppet.ini 是框架配置文件，修改需要弹窗确认。Storage API 专门用于应用数据存储，操作更直接、更高效。

### Q: 数据存储在哪里？

A: 数据存储在 `%APPDATA%\puppet\storage\` 目录下，每个数据库对应一个 `.db` 文件。

### Q: 如何备份数据？

A: 复制 `%APPDATA%\puppet\storage\` 目录即可备份所有数据。

### Q: 存储的数据大小有限制吗？

A: 理论上 SQLite 支持非常大的数据库（可达 140TB），但建议单个数据库不超过 100MB 以保证性能。

### Q: 如何删除所有数据？

A: 使用 `deleteDatabase()` 方法删除数据库，或直接删除 `%APPDATA%\puppet\storage\` 目录。

### Q: 可以在多个应用间共享数据吗？

A: 可以！多个应用可以使用相同的数据库名称来共享数据，或者使用不同的数据库名称来隔离数据。

## 相关资源

- [SQLite 官方文档](https://www.sqlite.org/docs.html)
- [System.Data.SQLite](https://system.data.sqlite.org/)
- [应用控制 API](application.html) - 其他应用相关 API
- [文件系统 API](fs.html) - 文件操作 API