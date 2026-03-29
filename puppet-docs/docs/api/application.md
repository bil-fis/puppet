---
title: 应用控制 API
permalink: /api/application.html
createTime: 2026/03/28 15:04:53
---

# 应用控制 API

应用控制 API 提供了应用生命周期管理、外部程序执行、配置管理等功能。

## 概述

`puppet.application` 命名空间提供以下功能：

- 应用关闭和重启
- 窗口信息查询
- 外部程序执行
- 配置文件管理
- 系统路径获取
- 用户信息获取

## 方法

### close()

关闭应用。

```javascript
await puppet.application.close(): Promise<void>
```

**示例**：

```javascript
// 关闭应用
await puppet.application.close();

// 在按钮点击时关闭
document.getElementById('close-btn').addEventListener('click', async () => {
    await puppet.application.close();
});
```

**注意事项**：

- 调用此方法后应用会立即退出
- 建议在关闭前保存数据
- 无法取消关闭操作

### restart()

重启应用。

```javascript
await puppet.application.restart(): Promise<void>
```

**示例**：

```javascript
// 重启应用
await puppet.application.restart();

// 在设置更改后重启
document.getElementById('restart-btn').addEventListener('click', async () => {
    await saveSettings();
    await puppet.application.restart();
});
```

**说明**：

- 重启会关闭当前应用实例
- 然后启动新的应用实例
- 配置和状态会保留

### getWindowInfo()

获取窗口信息。

```javascript
await puppet.application.getWindowInfo(): Promise<WindowInfo>
```

**返回值**：

```typescript
interface WindowInfo {
    handle: number;           // 窗口句柄
    title: string;            // 窗口标题
    className: string;        // 窗口类名
    isVisible: boolean;       // 是否可见
    isMinimized: boolean;     // 是否最小化
    isMaximized: boolean;     // 是否最大化
    width: number;            // 窗口宽度
    height: number;           // 窗口高度
    x: number;                // 窗口 X 坐标
    y: number;                // 窗口 Y 坐标
}
```

**示例**：

```javascript
// 获取窗口信息
const info = await puppet.application.getWindowInfo();
console.log('窗口标题:', info.title);
console.log('窗口大小:', info.width, 'x', info.height);
console.log('窗口位置:', info.x, info.y);
```

### execute()

执行外部程序或命令。

```javascript
await puppet.application.execute(command: string): Promise<void>
```

**参数**：

- `command` (string) - 要执行的命令或文件路径

**示例**：

```javascript
// 打开文件
await puppet.application.execute('C:\\Documents\\report.pdf');

// 打开网页
await puppet.application.execute('https://www.example.com');

// 执行系统命令
await puppet.application.execute('cmd /c dir');

// 打开计算器
await puppet.application.execute('calc.exe');
```

**自动检测**：

API 会自动检测命令类型：

- **URL**：使用默认浏览器打开
- **文件**：使用关联程序打开
- **可执行文件**：直接执行
- **命令**：通过 cmd 执行

**安全限制**：

- 执行系统目录下的程序会弹出权限确认
- 某些危险操作需要用户确认
- 命令执行有 30 秒超时限制

::: warning 安全提示
不要执行未经验证的命令，这可能导致安全风险。
:::

### setConfig()

设置配置值（修改 puppet.ini 文件）。

**方法 1：修改全局配置**

```javascript
await puppet.application.setConfig(key: string, value: string): Promise<void>
```

**参数**：

- `key` (string) - 配置键名
- `value` (string) - 配置值（仅支持字符串）

**方法 2：修改指定节的配置**

```javascript
await puppet.application.setConfig(section: string, key: string, value: string): Promise<void>
```

**参数**：

- `section` (string) - 节名（如 "file"、"window" 等）
- `key` (string) - 配置键名
- `value` (string) - 配置值（仅支持字符串）

**说明**：

- 修改 `puppet.ini` 配置文件
- 修改前会弹出确认对话框
- 每次修改都会创建备份文件（.backup）
- 仅支持字符串类型的值

**示例**：

```javascript
// 修改全局配置（等同于修改 [file] 节）
await puppet.application.setConfig('file', 'C:\\MyApp\\app.pup');

// 修改指定节的配置
await puppet.application.setConfig('file', 'file', 'C:\\MyApp\\app.pup');
await puppet.application.setConfig('window', 'startup_position', '100,100');
await puppet.application.setConfig('app', 'language', 'zh-CN');
```

**puppet.ini 文件格式**：

```ini
[file]
file=C:\MyApp\app.pup

[window]
startup_position=100,100
borderless=true
window_size=800,600

[app]
language=zh-CN
theme=dark
```

**安全提示**：



- 修改配置前会弹出确认对话框

- 用户可以取消修改操作

- 错误操作会显示错误信息



::: warning 警告

配置文件 `puppet.ini` 是 Puppet 框架的全局配置文件，修改可能影响框架的启动行为。请谨慎操作。

:::



::: tip 推荐用法

**重要区别**：`puppet.application.setConfig()` 用于框架配置，`puppet.storage` 用于应用数据。



- **setConfig** - 框架配置（如默认 PUP 文件路径）

  - 修改 `puppet.ini` 文件

  - 需要用户确认

  - 影响框架启动行为

  

- **storage** - 应用数据（如用户设置、应用状态）

  - 使用 SQLite 数据库

  - 直接存储，无需确认

  - 应用间可隔离或共享



**示例对比**：



```javascript

// ❌ 错误：使用 setConfig 存储应用数据

await puppet.application.setConfig('myapp', 'username', 'john');



// ✅ 正确：使用 storage 存储应用数据

await puppet.storage.setItem('default', 'username', 'john');



// ✅ 正确：使用 setConfig 修改框架配置

await puppet.application.setConfig('file', 'file', 'C:\\MyApp\\app.pup');

```



:::

### getAssemblyDirectory()

获取程序集目录（应用安装目录）。

```javascript
await puppet.application.getAssemblyDirectory(): Promise<string>
```

**返回值**：

程序集目录的完整路径。

**示例**：

```javascript
// 获取程序集目录
const appDir = await puppet.application.getAssemblyDirectory();
console.log('应用目录:', appDir);

// 构建资源路径
const configPath = appDir + '\\config.json';
const iconPath = appDir + '\\icons\\app.ico';
```

### getAppDataDirectory()

获取应用数据目录。

```javascript
await puppet.application.getAppDataDirectory(): Promise<string>
```

**返回值**：

应用数据目录的完整路径（通常是 `%APPDATA%\YourApp`）。

**示例**：

```javascript
// 获取应用数据目录
const dataDir = await puppet.application.getAppDataDirectory();
console.log('数据目录:', dataDir);

// 保存用户数据
const userDataPath = dataDir + '\\user.json';
await puppet.fs.writeTextToFile(userDataPath, JSON.stringify(userData));
```

### getCurrentUser()

获取当前用户信息。

```javascript
await puppet.application.getCurrentUser(): Promise<UserInfo>
```

**返回值**：

```typescript
interface UserInfo {
    name: string;        // 用户名
    domain: string;     // 域名
    homeDirectory: string;  // 主目录
}
```

**示例**：

```javascript
// 获取用户信息
const user = await puppet.application.getCurrentUser();
console.log('用户名:', user.name);
console.log('域名:', user.domain);
console.log('主目录:', user.homeDirectory);
```

### getAppInfo()

获取应用信息，包括签名状态（V1.2 格式）。

```javascript
await puppet.application.getAppInfo(): Promise<AppInfo>
```

**返回值**：

```typescript
interface AppInfo {
    name: string;                    // 应用名称
    version: string;                 // PUP 文件版本
    hasSignature: boolean;           // 是否已签名（V1.2）
    certificateThumbprint?: string;  // 证书指纹（V1.2）
    certificateIssuer?: string;      // 证书颁发者（V1.2）
    certificateSubject?: string;     // 证书主题（V1.2）
    certificateValidFrom?: Date;     // 证书生效时间（V1.2）
    certificateValidTo?: Date;       // 证书过期时间（V1.2）
}
```

**示例**：

```javascript
// 获取应用信息
const appInfo = await puppet.application.getAppInfo();
console.log('应用名称:', appInfo.name);
console.log('PUP 版本:', appInfo.version);

// 检查签名状态（V1.2 格式）
if (appInfo.hasSignature) {
    console.log('✓ 应用已签名');
    console.log('证书指纹:', appInfo.certificateThumbprint);
    console.log('证书颁发者:', appInfo.certificateIssuer);
    console.log('证书主题:', appInfo.certificateSubject);
    console.log('有效期:', appInfo.certificateValidFrom, '至', appInfo.certificateValidTo);
} else {
    console.warn('✗ 应用未签名');
}
```

**注意事项**：

- `hasSignature`、`certificateThumbprint` 等字段仅 V1.2 格式可用
- V1.0 和 V1.1 格式这些字段将为 `undefined`
- 用于应用安全验证和完整性检查
- 建议在生产环境中使用带签名的 PUP 文件

## 使用示例

### 应用初始化

```javascript
async function initializeApp() {
    // 获取应用信息
    const appDir = await puppet.application.getAssemblyDirectory();
    const dataDir = await puppet.application.getAppDataDirectory();
    const user = await puppet.application.getCurrentUser();
    
    console.log('应用目录:', appDir);
    console.log('数据目录:', dataDir);
    console.log('当前用户:', user.name);
    
    // 加载配置
    const config = await loadConfig();
    await applyConfig(config);
}

// 初始化应用
window.addEventListener('DOMContentLoaded', initializeApp);
```

### 配置管理

```javascript
// 配置管理类（基于 puppet.ini）
class ConfigManager {
    constructor() {
        this.defaults = {
            'file': '',
            'app.language': 'zh-CN',
            'app.theme': 'light',
            'window.startup_position': 'center',
            'window.borderless': 'false',
            'window.window_size': '800,600'
        };
    }
    
    async save(section, key, value) {
        // 设置配置（会弹出确认对话框）
        await puppet.application.setConfig(section, key, value);
    }
    
    async resetToDefaults() {
        // 重置为默认值（每个都会弹出确认）
        for (const [configKey, defaultValue] of Object.entries(this.defaults)) {
            const parts = configKey.split('.');
            if (parts.length === 2) {
                await this.save(parts[0], parts[1], defaultValue);
            } else {
                await this.save('file', parts[0], defaultValue);
            }
        }
    }
}

// 使用配置管理器
const configManager = new ConfigManager();

// 修改 PUP 文件路径
await configManager.save('file', 'file', 'C:\\MyApp\\app.pup');

// 修改应用语言
await configManager.save('app', 'language', 'en-US');

// 修改窗口启动位置
await configManager.save('window', 'startup_position', '100,100');
```

### 文件关联打开

```javascript
async function openFile(filePath) {
    // 检查文件是否存在
    if (!await puppet.fs.exists(filePath)) {
        throw new Error('文件不存在');
    }
    
    // 获取文件扩展名
    const extension = filePath.split('.').pop().toLowerCase();
    
    // 根据文件类型处理
    switch (extension) {
        case 'txt':
        case 'json':
        case 'md':
            // 在应用内打开
            const content = await puppet.fs.readFileAsText(filePath);
            displayContent(content);
            break;
            
        case 'pdf':
        case 'doc':
        case 'docx':
            // 使用外部程序打开
            await puppet.application.execute(filePath);
            break;
            
        case 'http':
        case 'https':
            // URL
            await puppet.application.execute(filePath);
            break;
            
        default:
            throw new Error('不支持的文件类型');
    }
}

// 使用示例
await openFile('document.pdf');
await openFile('data.json');
```

### 应用更新

```javascript
async function checkForUpdates() {
    const currentVersionJson = await puppet.storage.getItem('default', 'version');
    const currentVersion = currentVersionJson ? JSON.parse(currentVersionJson) : '1.0.0';
    const latestVersion = await fetchLatestVersion();
    
    if (compareVersions(latestVersion, currentVersion) > 0) {
        // 发现新版本
        showUpdateNotification(latestVersion);
    }
}

async function performUpdate() {
    // 下载更新
    const updateFile = await downloadUpdate();
    
    // 执行更新程序
    await puppet.application.execute(updateFile);
    
    // 关闭当前应用
    await puppet.application.close();
}

// 检查更新
await checkForUpdates();
```

### 快捷方式创建

```javascript
async function createShortcut() {
    const appDir = await puppet.application.getAssemblyDirectory();
    const exePath = appDir + '\\puppet.exe';
    const pupPath = appDir + '\\app.pup';
    
    const desktop = Environment.GetFolderPath(
        Environment.SpecialFolder.Desktop
    );
    
    const shortcutPath = desktop + '\\MyApp.lnk';
    
    // 创建快捷方式
    await puppet.application.execute(
        `powershell -Command "$s=(New-Object -COM WScript.Shell).CreateShortcut('${shortcutPath}');$s.TargetPath='${exePath}';$s.Arguments='--load-pup ${pupPath}';$s.Save()"`
    );
}
```

## 最佳实践

### 1. 配置管理

使用结构化的配置管理：

```javascript
class AppConfig {
    constructor() {
        this.config = {};
    }
    
    async init() {
        this.config = await this.load();
    }
    
    async load() {
        const dataJson = await puppet.storage.getItem('default', 'appConfig');
        return dataJson ? JSON.parse(dataJson) : {};
    }
    
    async save() {
        await puppet.storage.setItem('default', 'appConfig', JSON.stringify(this.config));
    }
    
    get(key, defaultValue) {
        return this.config[key] ?? defaultValue;
    }
    
    set(key, value) {
        this.config[key] = value;
        this.save();
    }
}
```

### 2. 错误处理

完善的错误处理机制：

```javascript
async function safeExecute(command) {
    try {
        await puppet.application.execute(command);
        return true;
    } catch (error) {
        puppet.log.error('执行命令失败:', error.message);
        showError('操作失败: ' + error.message);
        return false;
    }
}
```

### 3. 路径处理

使用规范的路径处理：

```javascript
async function getAppPath(filename) {
    const appDir = await puppet.application.getAssemblyDirectory();
    return path.join(appDir, filename);
}

async function getDataPath(filename) {
    const dataDir = await puppet.application.getAppDataDirectory();
    return path.join(dataDir, filename);
}
```

### 4. 用户数据存储

将用户数据存储在应用数据目录：

```javascript
async function saveUserData(data) {
    const dataDir = await puppet.application.getAppDataDirectory();
    const userDataFile = dataDir + '\\userdata.json';
    
    await puppet.fs.writeTextToFile(
        userDataFile,
        JSON.stringify(data, null, 2)
    );
}

async function loadUserData() {
    const dataDir = await puppet.application.getAppDataDirectory();
    const userDataFile = dataDir + '\\userdata.json';
    
    if (await puppet.fs.exists(userDataFile)) {
        const content = await puppet.fs.readFileAsText(userDataFile);
        return JSON.parse(content);
    }
    
    return {};
}
```

## 相关资源

- [Windows 程序执行](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process)：Process 类文档
- [应用数据存储](https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid)：已知文件夹 ID
- [JSON 配置](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON)：JSON 使用指南

## 常见问题

### Q: 为什么执行命令会弹出确认对话框？

A: 为了安全考虑，执行系统目录下的程序或危险操作会弹出权限确认。

### Q: 如何永久记住用户的权限选择？

A: 在权限对话框中选择"永久阻止"，该选择会被记住。

### Q: 配置数据保存在哪里？

A: 配置数据保存在应用目录下的 `puppet.ini` 文件中，这是 Puppet 框架的全局配置文件。

### Q: 修改配置时为什么会弹出确认对话框？

A: 为了防止误操作，修改 `puppet.ini` 文件时会弹出确认对话框，用户可以取消修改。

### Q: setConfig 支持哪些数据类型？

A: 目前仅支持字符串类型。如果需要存储复杂数据，请先转换为 JSON 字符串：

```javascript
// ✅ 推荐：使用 storage 存储对象
const obj = { theme: 'dark', fontSize: 16 };
await puppet.storage.setItem('default', 'preferences', JSON.stringify(obj));

// 读取对象
const content = await puppet.storage.getItem('default', 'preferences');
const preferences = JSON.parse(content);
```

### Q: 如何在应用重启后保留状态？

A: 使用 `puppet.storage` 保存状态：

```javascript
// 保存状态
await puppet.storage.setItem('default', 'lastPage', currentPage);

// 恢复状态
const lastPage = await puppet.storage.getItem('default', 'lastPage');
if (lastPage) {
    navigateTo(lastPage);
}
```

::: tip 说明
`puppet.storage` 专为应用数据持久化设计，比 `setConfig()` 更方便、更安全。
:::