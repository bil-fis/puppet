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

设置配置值。

```javascript
await puppet.application.setConfig(key: string, value: any): Promise<void>
```

**参数**：

- `key` (string) - 配置键名
- `value` (any) - 配置值（支持字符串、数字、布尔值、对象、数组）

**示例**：

```javascript
// 设置字符串配置
await puppet.application.setConfig('username', 'john');

// 设置数字配置
await puppet.application.setConfig('fontSize', 14);

// 设置布尔配置
await puppet.application.setConfig('darkMode', true);

// 设置对象配置
await puppet.application.setConfig('preferences', {
    theme: 'dark',
    language: 'zh-CN',
    autoUpdate: true
});

// 设置数组配置
await puppet.application.setConfig('recentFiles', [
    'file1.txt',
    'file2.txt',
    'file3.txt'
]);
```

### getConfig()

获取配置值。

```javascript
await puppet.application.getConfig(key: string): Promise<any>
```

**参数**：

- `key` (string) - 配置键名

**返回值**：

配置值，如果不存在则返回 `null`。

**示例**：

```javascript
// 获取配置
const username = await puppet.application.getConfig('username');
const fontSize = await puppet.application.getConfig('fontSize');
const preferences = await puppet.application.getConfig('preferences');

console.log('用户名:', username);
console.log('字体大小:', fontSize);
console.log('偏好设置:', preferences);
```

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
// 配置管理类
class ConfigManager {
    constructor() {
        this.defaults = {
            theme: 'light',
            language: 'zh-CN',
            fontSize: 14,
            autoUpdate: true
        };
    }
    
    async load() {
        const config = { ...this.defaults };
        
        // 加载保存的配置
        for (const key of Object.keys(this.defaults)) {
            const value = await puppet.application.getConfig(key);
            if (value !== null) {
                config[key] = value;
            }
        }
        
        return config;
    }
    
    async save(config) {
        for (const [key, value] of Object.entries(config)) {
            await puppet.application.setConfig(key, value);
        }
    }
    
    async reset() {
        for (const key of Object.keys(this.defaults)) {
            await puppet.application.setConfig(key, this.defaults[key]);
        }
    }
}

// 使用配置管理器
const configManager = new ConfigManager();

// 加载配置
const config = await configManager.load();
console.log('当前配置:', config);

// 保存配置
await configManager.save({
    theme: 'dark',
    fontSize: 16
});

// 重置配置
await configManager.reset();
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
    const currentVersion = await puppet.application.getConfig('version');
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
        const data = await puppet.application.getConfig('appConfig');
        return data || {};
    }
    
    async save() {
        await puppet.application.setConfig('appConfig', this.config);
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

A: 配置数据保存在应用目录下的 `puppet.json` 文件中。

### Q: 如何在应用重启后保留状态？

A: 使用 `setConfig()` 和 `getConfig()` 保存和恢复状态：

```javascript
// 保存状态
await puppet.application.setConfig('lastPage', currentPage);

// 恢复状态
const lastPage = await puppet.application.getConfig('lastPage');
if (lastPage) {
    navigateTo(lastPage);
}
```