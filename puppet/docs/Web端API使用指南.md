# Web端API使用指南

本文档介绍如何在网页中使用Puppet框架提供的JavaScript API。

## 目录

- [快速开始](#快速开始)
- [窗口控制 API (puppet.window)](#窗口控制-api-puppetwindow)
- [应用控制 API (puppet.Application)](#应用控制-api-puppetapplication)
- [文件系统 API (puppet.fs)](#文件系统-api-puppetfs)
- [日志 API (puppet.log)](#日志-api-puppetlog)
- [系统 API (puppet.system)](#系统-api-puppetsystem)
- [托盘图标 API (puppet.tray)](#托盘图标-api-puppettray)
- [透明区域 API](#透明区域-api)
- [消息通信](#消息通信)
- [完整示例](#完整示例)

## 快速开始

### 访问puppet命名空间

在Puppet框架中，所有API都通过全局的`puppet`对象暴露。在任何页面中都可以直接访问：

```javascript
// 检查puppet是否可用
if (window.puppet) {
    console.log('Puppet API 已加载');
} else {
    console.log('Puppet API 未加载');
}
```

### 异步调用

所有API方法都是异步的，使用`async/await`语法：

```javascript
async function example() {
    try {
        const result = await puppet.window.setOpacity(0.5);
        console.log('操作成功:', result);
    } catch (error) {
        console.error('操作失败:', error);
    }
}

example();
```

### 错误处理

始终使用try-catch处理可能的错误：

```javascript
try {
    await puppet.window.setOpacity(1.2); // 无效值
} catch (error) {
    console.error('错误:', error.message);
}
```

## 窗口控制 API (puppet.window)

窗口控制API用于操作应用程序窗口的外观和行为。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| setBorderless | (boolean) | void | 设置无边框模式 |
| setDraggable | (boolean) | void | 设置窗口可拖动 |
| setResizable | (boolean) | void | 设置窗口可缩放 |
| setOpacity | (number) | void | 设置窗口透明度 (0.0-1.0) |
| setMouseThroughTransparency | (boolean) | void | 设置透明区域鼠标穿透 |
| setMouseThrough | (boolean) | void | 设置整个窗口鼠标穿透 |
| setTransparentColor | (string) | void | 设置透明颜色 |
| setTopmost | (boolean) | void | 设置窗口置顶 |
| showInTaskbar | (boolean) | void | 设置是否在任务栏显示 |
| moveWindow | (number, number) | void | 移动窗口到指定位置 |
| resizeWindow | (number, number) | void | 调整窗口大小 |
| centerWindow | () | void | 居中窗口 |

### 详细说明

#### setBorderless(isBorderless)

设置窗口为无边框模式。

```javascript
// 启用无边框
await puppet.window.setBorderless(true);

// 禁用无边框
await puppet.window.setBorderless(false);
```

**参数**：
- `isBorderless` (boolean): true=无边框，false=有边框

#### setOpacity(opacity)

设置窗口透明度。

```javascript
// 设置为半透明
await puppet.window.setOpacity(0.5);

// 设置为不透明
await puppet.window.setOpacity(1.0);

// 设置为非常透明
await puppet.window.setOpacity(0.1);
```

**参数**：
- `opacity` (number): 透明度值，范围0.0-1.0

**注意事项**：
- 0.0 = 完全透明
- 1.0 = 完全不透明
- 建议值：0.3-0.8

#### moveWindow(x, y)

移动窗口到屏幕指定位置。

```javascript
// 移动到屏幕左上角
await puppet.window.moveWindow(0, 0);

// 移动到指定位置
await puppet.window.moveWindow(100, 200);
```

**参数**：
- `x` (number): 屏幕X坐标
- `y` (number): 屏幕Y坐标

#### resizeWindow(width, height)

调整窗口大小。

```javascript
// 调整为800x600
await puppet.window.resizeWindow(800, 600);

// 调整为全屏大小
await puppet.window.resizeWindow(1920, 1080);
```

**参数**：
- `width` (number): 窗口宽度（像素）
- `height` (number): 窗口高度（像素）

#### centerWindow()

将窗口居中显示在屏幕上。

```javascript
await puppet.window.centerWindow();
```

#### setTopmost(isTopmost)

设置窗口是否始终置顶。

```javascript
// 置顶窗口
await puppet.window.setTopmost(true);

// 取消置顶
await puppet.window.setTopmost(false);
```

**参数**：
- `isTopmost` (boolean): true=置顶，false=不置顶

#### setMouseThrough(isEnabled)

设置整个窗口的鼠标穿透。

```javascript
// 启用鼠标穿透（点击穿透到下层窗口）
await puppet.window.setMouseThrough(true);

// 禁用鼠标穿透
await puppet.window.setMouseThrough(false);
```

**使用场景**：
- 创建桌面装饰工具
- 实现覆盖层效果

#### setTransparentColor(color)

设置透明颜色，匹配该颜色的区域将变得透明。

```javascript
// 设置绿色为透明色
await puppet.window.setTransparentColor('#00FF00');

// 设置红色为透明色
await puppet.window.setTransparentColor('rgb(255, 0, 0)');
```

**参数**：
- `color` (string): 颜色值，支持HEX或RGB格式

**注意事项**：
- 需配合`setMouseThroughTransparency`使用
- 颜色格式：`#RRGGBB` 或 `rgb(r, g, b)`

### 使用示例

```javascript
// 创建一个无边框、半透明、置顶的窗口
async function createOverlayWindow() {
    try {
        await puppet.window.setBorderless(true);
        await puppet.window.setOpacity(0.8);
        await puppet.window.setTopmost(true);
        await puppet.window.centerWindow();
        console.log('窗口配置完成');
    } catch (error) {
        console.error('配置失败:', error);
    }
}

// 创建一个全屏覆盖窗口
async function createFullscreenOverlay() {
    const screenWidth = window.screen.width;
    const screenHeight = window.screen.height;
    
    await puppet.window.setBorderless(true);
    await puppet.window.setOpacity(0.3);
    await puppet.window.moveWindow(0, 0);
    await puppet.window.resizeWindow(screenWidth, screenHeight);
    await puppet.window.setTopmost(true);
}
```

## 应用控制 API (puppet.Application)

应用控制API用于管理应用程序的生命周期和执行系统命令。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| close | () | void | 关闭应用程序 |
| restart | () | void | 重启应用程序 |
| getWindowInfo | () | string | 获取窗口信息 |
| execute | (string) | string | 执行命令 |
| setConfig | (string, string) | void | 设置配置 |
| getAssemblyDirectory | () | string | 获取程序目录 |
| getAppDataDirectory | () | string | 获取AppData目录 |
| getCurrentUser | () | string | 获取当前用户 |

### 详细说明

#### close()

关闭应用程序。

```javascript
await puppet.Application.close();
```

**注意事项**：
- 此操作不可逆
- 建议在关闭前保存数据

#### restart()

重启应用程序。

```javascript
await puppet.Application.restart();
```

**使用场景**：
- 应用更新后需要重启
- 重置应用状态

#### getWindowInfo()

获取当前窗口的详细信息。

```javascript
const info = await puppet.Application.getWindowInfo();
console.log(info);

// 输出示例：
// {
//   "Handle": "123456",
//   "Text": "Puppet",
//   "Location": { "X": 100, "Y": 100 },
//   "Size": { "Width": 800, "Height": 600 },
//   "Opacity": 1.0,
//   "TopMost": false,
//   "WindowState": "Normal"
// }
```

**返回值**：JSON字符串，包含窗口详细信息

#### execute(command)

执行系统命令、打开URL或文件。

```javascript
// 打开网址
await puppet.Application.execute('https://www.example.com');

// 打开文件
await puppet.Application.execute('C:\\Documents\\file.txt');

// 执行命令
const result = await puppet.Application.execute('dir');
console.log(result);

// 打开文件夹
await puppet.Application.execute('C:\\Windows');

// 发送邮件
await puppet.Application.execute('mailto:test@example.com');
```

**参数**：
- `command` (string): 要执行的命令

**支持的类型**：
- URL（http://, https://）
- 文件路径
- 文件夹路径
- 系统命令
- URI（mailto:, tel:, ftp://, file://）

**返回值**：命令执行结果或错误信息

**注意事项**：
- 执行超时：30秒
- 交互式命令（如cmd、powershell）会自动退出

#### setConfig(key, value)

设置配置项。

```javascript
await puppet.Application.setConfig('theme', 'dark');
await puppet.Application.setConfig('language', 'zh-CN');
```

**参数**：
- `key` (string): 配置键名
- `value` (string): 配置值

**存储位置**：程序目录下的`puppet.json`文件

#### getAssemblyDirectory()

获取应用程序的安装目录。

```javascript
const appDir = await puppet.Application.getAssemblyDirectory();
console.log('应用目录:', appDir);
```

#### getAppDataDirectory()

获取用户AppData目录。

```javascript
const appDataDir = await puppet.Application.getAppDataDirectory();
console.log('AppData目录:', appDataDir);
```

#### getCurrentUser()

获取当前登录用户名。

```javascript
const username = await puppet.Application.getCurrentUser();
console.log('当前用户:', username);
```

### 使用示例

```javascript
// 应用设置管理
async function saveUserSettings(settings) {
    for (const [key, value] of Object.entries(settings)) {
        await puppet.Application.setConfig(key, value);
    }
    console.log('设置已保存');
}

// 应用信息面板
async function showAppInfo() {
    const info = await puppet.Application.getWindowInfo();
    const user = await puppet.Application.getCurrentUser();
    const appDir = await puppet.Application.getAssemblyDirectory();
    
    alert(`
        应用信息：
        用户：${user}
        目录：${appDir}
        窗口信息：${info}
    `);
}

// 安全关闭应用
async function safeClose() {
    if (confirm('确定要关闭应用吗？')) {
        // 保存数据
        await puppet.Application.setConfig('lastClosed', new Date().toISOString());
        // 关闭应用
        await puppet.Application.close();
    }
}
```

## 文件系统 API (puppet.fs)

文件系统API用于操作本地文件和文件夹。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| openFileDialog | (filter, multiSelect) | string/array | 打开文件选择框 |
| openFolderDialog | () | string | 打开文件夹选择框 |
| readFileAsByte | (path) | string | 读取文件为Base64 |
| readFileAsJson | (path) | string | 读取JSON文件 |
| writeByteToFile | (path, data) | void | 写入Base64数据到文件 |
| writeTextToFile | (path, text) | void | 写入文本到文件 |
| appendByteToFile | (path, data) | void | 追加Base64数据到文件 |
| appendTextToFile | (path, text) | void | 追加文本到文件 |
| exists | (path) | boolean | 检查路径是否存在 |
| delete | (path) | void | 删除文件或文件夹 |

### 详细说明

#### openFileDialog(filter, multiSelect)

弹出系统文件选择对话框。

```javascript
// 单选文件
const filePath = await puppet.fs.openFileDialog('所有文件|*.*', false);
console.log('选择的文件:', filePath);

// 多选文件
const filePaths = await puppet.fs.openFileDialog('图片文件|*.png;*.jpg;*.jpeg', true);
console.log('选择的文件:', filePaths);

// 使用JSON格式的过滤器
const filter = JSON.stringify(['文本文件', '*.txt']);
const filePath = await puppet.fs.openFileDialog(filter, false);
```

**参数**：
- `filter` (string): 文件过滤器，格式："名称|扩展名" 或 JSON数组
- `multiSelect` (boolean): 是否允许多选

**返回值**：
- 单选：文件路径字符串
- 多选：文件路径数组
- 取消：null

**过滤器示例**：
```javascript
// 文本格式
'所有文件|*.*'
'文本文件|*.txt'
'图片文件|*.png;*.jpg;*.jpeg'
'所有支持的文件|*.txt;*.doc;*.docx;*.pdf'

// JSON格式
JSON.stringify(['文本文件', '*.txt'])
JSON.stringify(['图片文件', '*.png;*.jpg'])
```

#### openFolderDialog()

弹出系统文件夹选择对话框。

```javascript
const folderPath = await puppet.fs.openFolderDialog();
if (folderPath) {
    console.log('选择的文件夹:', folderPath);
} else {
    console.log('用户取消了选择');
}
```

**返回值**：
- 文件夹路径字符串
- 取消时返回null

#### readFileAsByte(path)

读取文件并返回Base64编码的数据。

```javascript
const base64Data = await puppet.fs.readFileAsByte('C:\\test.png');
console.log('文件数据（Base64）:', base64Data);

// 显示图片
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + base64Data;
document.body.appendChild(img);
```

**参数**：
- `path` (string): 文件路径

**返回值**：Base64编码的字符串

#### readFileAsJson(path)

读取JSON文件并返回JSON字符串。

```javascript
const jsonData = await puppet.fs.readFileAsJson('C:\\config.json');
const config = JSON.parse(jsonData);
console.log('配置:', config);
```

**参数**：
- `path` (string): JSON文件路径

**返回值**：JSON字符串

**注意事项**：
- 文件必须包含有效的JSON
- 如果文件不是JSON格式，会抛出异常

#### writeByteToFile(path, base64Data)

将Base64数据写入文件。

```javascript
const base64Data = 'SGVsbG8gV29ybGQ='; // "Hello World"的Base64
await puppet.fs.writeByteToFile('C:\\output.bin', base64Data);
```

**参数**：
- `path` (string): 文件路径
- `base64Data` (string): Base64编码的数据

#### writeTextToFile(path, text)

将文本写入文件。

```javascript
await puppet.fs.writeTextToFile('C:\\output.txt', 'Hello, World!');
await puppet.fs.writeTextToFile('C:\\config.json', JSON.stringify({key: 'value'}));
```

**参数**：
- `path` (string): 文件路径
- `text` (string): 文本内容

**编码**：UTF-8

#### appendByteToFile(path, base64Data)

将Base64数据追加到文件末尾。

```javascript
await puppet.fs.appendByteToFile('C:\\log.bin', base64Data);
```

**参数**：
- `path` (string): 文件路径
- `base64Data` (string): Base64编码的数据

#### appendTextToFile(path, text)

将文本追加到文件末尾。

```javascript
await puppet.fs.appendTextToFile('C:\\log.txt', 'Log entry\n');
```

**参数**：
- `path` (string): 文件路径
- `text` (string): 文本内容

#### exists(path)

检查文件或文件夹是否存在。

```javascript
const exists = await puppet.fs.exists('C:\\test.txt');
if (exists) {
    console.log('文件存在');
} else {
    console.log('文件不存在');
}
```

**参数**：
- `path` (string): 文件或文件夹路径

**返回值**：boolean

#### delete(path)

删除文件或文件夹。

```javascript
await puppet.fs.delete('C:\\test.txt');
console.log('文件已删除');

// 删除文件夹（包括所有内容）
await puppet.fs.delete('C:\\myFolder');
```

**参数**：
- `path` (string): 文件或文件夹路径

**注意事项**：
- 删除文件夹会递归删除所有内容
- 此操作不可逆

### 安全限制

出于安全考虑，以下路径无法访问：
- `C:\Windows\` 及其子目录
- `C:\Windows\System32\`
- `C:\Windows\SysWOW64\`

尝试访问这些路径会抛出`UnauthorizedAccessException`。

### 使用示例

```javascript
// 图片上传和处理
async function uploadImage() {
    const filePath = await puppet.fs.openFileDialog('图片文件|*.png;*.jpg;*.jpeg', false);
    if (!filePath) return;
    
    const base64Data = await puppet.fs.readFileAsByte(filePath);
    
    // 显示图片
    const img = document.createElement('img');
    img.src = 'data:image/png;base64,' + base64Data;
    document.body.appendChild(img);
}

// 保存配置文件
async function saveConfig(config) {
    const jsonData = JSON.stringify(config, null, 2);
    const filePath = await puppet.fs.openFileDialog('JSON文件|*.json', false);
    if (filePath) {
        await puppet.fs.writeTextToFile(filePath, jsonData);
        console.log('配置已保存');
    }
}

// 加载配置文件
async function loadConfig() {
    const filePath = await puppet.fs.openFileDialog('JSON文件|*.json', false);
    if (!filePath) return null;
    
    try {
        const jsonData = await puppet.fs.readFileAsJson(filePath);
        return JSON.parse(jsonData);
    } catch (error) {
        console.error('加载配置失败:', error);
        return null;
    }
}

// 日志记录
async function logToFile(message) {
    const timestamp = new Date().toISOString();
    const logEntry = `[${timestamp}] ${message}\n`;
    await puppet.fs.appendTextToFile('C:\\logs\\app.log', logEntry);
}
```

## 日志 API (puppet.log)

日志API用于记录应用程序的运行日志。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| info | (message) | void | 记录信息日志 |
| warn | (message) | void | 记录警告日志 |
| error | (message) | void | 记录错误日志 |

### 详细说明

#### info(message)

记录信息级别的日志。

```javascript
await puppet.log.info('应用程序启动');
await puppet.log.info('用户登录成功');
await puppet.log.info('操作完成');
```

#### warn(message)

记录警告级别的日志。

```javascript
await puppet.log.warn('配置文件未找到，使用默认配置');
await puppet.log.warn('API响应超时，使用缓存数据');
```

#### error(message)

记录错误级别的日志。

```javascript
await puppet.log.error('数据库连接失败');
await puppet.log.error('文件读取错误: ' + error.message);
```

### 日志格式

所有日志都遵循以下格式：

```
[时间戳] [级别] 消息
```

**示例**：
```
[2026-03-27 10:30:45.123] [INFO] 应用程序启动
[2026-03-27 10:30:46.234] [WARN] 配置文件未找到，使用默认配置
[2026-03-27 10:30:47.345] [ERROR] 数据库连接失败
```

### 日志存储

- **位置**：程序目录下的`puppet.log`文件
- **编码**：UTF-8
- **格式**：每行一条日志

### 使用示例

```javascript
// 日志包装函数
async function log(type, message) {
    const timestamp = new Date().toISOString();
    const logMessage = `[${timestamp}] ${message}`;
    
    switch(type) {
        case 'info':
            await puppet.log.info(logMessage);
            break;
        case 'warn':
            await puppet.log.warn(logMessage);
            break;
        case 'error':
            await puppet.log.error(logMessage);
            break;
    }
    
    // 同时输出到控制台
    console.log(`[${type.toUpperCase()}] ${logMessage}`);
}

// 使用示例
log('info', '用户点击了按钮');
log('warn', '输入值超出范围');
log('error', '操作失败: ' + error.message);

// 异步操作日志
async function performAsyncOperation() {
    try {
        await puppet.log.info('开始异步操作');
        // 执行操作
        await puppet.log.info('操作完成');
    } catch (error) {
        await puppet.log.error('操作失败: ' + error.message);
        throw error;
    }
}

// 性能监控
async function monitorPerformance() {
    const startTime = Date.now();
    await puppet.log.info('性能监控开始');
    
    // 执行任务
    await doSomething();
    
    const duration = Date.now() - startTime;
    await puppet.log.info(`任务完成，耗时: ${duration}ms`);
}
```

## 系统 API (puppet.system)

系统API用于获取系统信息和执行系统操作。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| getSystemInfo | () | string | 获取系统信息 |
| takeScreenShot | () | string | 截取屏幕 |
| getDesktopWallpaper | () | string | 获取桌面壁纸 |
| sendKey | (...keys) | void | 模拟按键 |
| sendMouseClick | (x, y, button) | void | 模拟鼠标点击 |
| getMousePosition | () | string | 获取鼠标位置 |

### 详细说明

#### getSystemInfo()

获取详细的系统信息。

```javascript
const systemInfo = await puppet.system.getSystemInfo();
const info = JSON.parse(systemInfo);

console.log('操作系统:', info.OperatingSystem.OSName);
console.log('版本:', info.OperatingSystem.Version);
console.log('处理器数:', info.Hardware.ProcessorCount);
console.log('总内存:', info.Memory.TotalPhysicalMemory);
console.log('GPU:', info.GPU);
```

**返回值**：JSON字符串，包含以下信息：

```json
{
  "OperatingSystem": {
    "OSName": "Win32NT",
    "Version": "10.0.22000.0",
    "MachineName": "DESKTOP-ABC123",
    "UserName": "User"
  },
  "Hardware": {
    "ProcessorCount": 8,
    "ProcessorArchitecture": "X64",
    "SystemPageSize": 4096
  },
  "Memory": {
    "TotalPhysicalMemory": 17179869184,
    "AvailablePhysicalMemory": 8589934592
  },
  "Display": {
    "PrimaryScreen": {
      "Width": 1920,
      "Height": 1080,
      "BitsPerPixel": 32
    }
  },
  "GPU": "NVIDIA GeForce RTX 3080"
}
```

#### takeScreenShot()

截取主屏幕并返回Base64编码的图片。

```javascript
const screenshot = await puppet.system.takeScreenShot();

// 显示截图
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + screenshot;
document.body.appendChild(img);

// 保存截图
await puppet.fs.writeByteToFile('C:\\screenshot.png', screenshot);
```

**返回值**：Base64编码的PNG图片

**注意事项**：
- 只截取主屏幕
- 图片格式：PNG

#### getDesktopWallpaper()

获取当前桌面壁纸。

```javascript
const wallpaper = await puppet.system.getDesktopWallpaper();

if (wallpaper) {
    // 显示壁纸
    const img = document.createElement('img');
    img.src = 'data:image/png;base64,' + wallpaper;
    document.body.appendChild(img);
} else {
    console.log('无法获取壁纸');
}
```

**返回值**：
- 成功：Base64编码的图片
- 失败：null

#### sendKey(...keys)

模拟键盘按键。

```javascript
// 单个按键
await puppet.system.sendKey('A');
await puppet.system.sendKey('ENTER');

// 多个按键
await puppet.system.sendKey('A', 'B', 'C', 'ENTER');

// 组合键（依次按下）
await puppet.system.sendKey('CTRL', 'C');
```

**支持的按键**：
- 字母：A-Z
- 数字：0-9
- 功能键：ENTER, TAB, SPACE, BACKSPACE, DELETE, ESC, F1-F12
- 方向键：LEFT, UP, RIGHT, DOWN

**注意事项**：
- 每个按键会依次按下并释放
- 不支持组合键（如Ctrl+C需要分别发送）

#### sendMouseClick(x, y, button)

模拟鼠标点击。

```javascript
// 左键点击
await puppet.system.sendMouseClick(100, 200, 'left');

// 右键点击
await puppet.system.sendMouseClick(100, 200, 'right');

// 中键点击
await puppet.system.sendMouseClick(100, 200, 'middle');

// 默认左键点击
await puppet.system.sendMouseClick(100, 200);
```

**参数**：
- `x` (number): 屏幕X坐标
- `y` (number): 屏幕Y坐标
- `button` (string): 按钮类型，默认'left'

**按钮类型**：
- `left` - 左键
- `right` - 右键
- `middle` - 中键

#### getMousePosition()

获取当前鼠标位置。

```javascript
const position = await puppet.system.getMousePosition();
const pos = JSON.parse(position);

console.log('X坐标:', pos.X);
console.log('Y坐标:', pos.Y);
```

**返回值**：JSON字符串

```json
{
  "X": 100,
  "Y": 200
}
```

### 使用示例

```javascript
// 系统信息面板
async function showSystemInfo() {
    const systemInfo = await puppet.system.getSystemInfo();
    const info = JSON.parse(systemInfo);
    
    alert(`
        系统信息：
        操作系统：${info.OperatingSystem.OSName} ${info.OperatingSystem.Version}
        计算机名：${info.OperatingSystem.MachineName}
        用户：${info.OperatingSystem.UserName}
        处理器：${info.Hardware.ProcessorCount} 核心
        总内存：${(info.Memory.TotalPhysicalMemory / 1024 / 1024 / 1024).toFixed(2)} GB
        可用内存：${(info.Memory.AvailablePhysicalMemory / 1024 / 1024 / 1024).toFixed(2)} GB
        屏幕分辨率：${info.Display.PrimaryScreen.Width}x${info.Display.PrimaryScreen.Height}
        显卡：${info.GPU}
    `);
}

// 截图并保存
async function captureAndSaveScreenshot() {
    try {
        const screenshot = await puppet.system.takeScreenShot();
        
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const filename = `screenshot-${timestamp}.png`;
        
        await puppet.fs.writeByteToFile(`C:\\screenshots\\${filename}`, screenshot);
        console.log('截图已保存:', filename);
    } catch (error) {
        console.error('截图失败:', error);
    }
}

// 自动点击器
async function autoClick(x, y, interval, count) {
    for (let i = 0; i < count; i++) {
        await puppet.system.sendMouseClick(x, y, 'left');
        console.log(`点击 ${i + 1}/${count}`);
        
        if (i < count - 1) {
            await new Promise(resolve => setTimeout(resolve, interval));
        }
    }
}

// 键盘自动化
async function typeText(text) {
    const keyMap = {
        ' ': 'SPACE',
        '\n': 'ENTER',
        '\t': 'TAB'
    };
    
    for (const char of text) {
        const key = keyMap[char] || char.toUpperCase();
        await puppet.system.sendKey(key);
        await new Promise(resolve => setTimeout(resolve, 50)); // 打字速度
    }
}

// 鼠标追踪
async function trackMouse() {
    let lastPosition = null;
    
    setInterval(async () => {
        const position = await puppet.system.getMousePosition();
        const pos = JSON.parse(position);
        
        if (lastPosition) {
            const dx = pos.X - lastPosition.X;
            const dy = pos.Y - lastPosition.Y;
            console.log(`鼠标移动: dx=${dx}, dy=${dy}`);
        }
        
        lastPosition = pos;
    }, 100);
}
```

## 托盘图标 API (puppet.tray)

托盘图标API用于管理系统托盘图标。

### 方法列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| setTray | (name) | void | 创建或更新托盘图标 |
| setMenu | (menu) | void | 设置托盘菜单 |
| showBalloon | (title, content, timeout, icon) | void | 显示气泡提示 |
| onClick | (callback) | void | 设置点击事件 |
| onDoubleClick | (callback) | void | 设置双击事件 |
| hide | () | void | 隐藏托盘图标 |
| show | () | void | 显示托盘图标 |
| setIcon | (iconPath) | void | 设置托盘图标 |

### 详细说明

#### setTray(name)

创建或更新托盘图标。

```javascript
// 创建托盘图标
await puppet.tray.setTray('我的应用');
```

**参数**：
- `name` (string): 托盘图标名称（鼠标悬停时显示）

**注意事项**：
- 首次调用会创建托盘图标
- 后续调用会更新托盘图标名称
- 默认使用应用图标

#### setMenu(menu)

设置托盘右键菜单。

```javascript
const menu = [
    { Text: '打开主窗口', Command: 'showMainWindow' },
    { Text: '设置', Command: 'openSettings' },
    { Text: '-' },
    { Text: '退出', Command: 'quitApp' }
];

await puppet.tray.setMenu(menu);

// 定义回调函数
window.showMainWindow = function() {
    console.log('显示主窗口');
};

window.openSettings = function() {
    console.log('打开设置');
};

window.quitApp = function() {
    puppet.Application.close();
};
```

**参数**：
- `menu` (array): 菜单项数组

**菜单项格式**：
```javascript
{
    Text: string,      // 菜单项文本
    Command: string    // JavaScript函数名
}
```

**分隔线**：
```javascript
{ Text: '-' }
```

#### showBalloon(title, content, timeout, icon)

显示气泡提示。

```javascript
// 信息提示
await puppet.tray.showBalloon(
    '通知',
    '这是一条信息提示',
    5000,
    'Info'
);

// 警告提示
await puppet.tray.showBalloon(
    '警告',
    '这是一条警告提示',
    5000,
    'Warning'
);

// 错误提示
await puppet.tray.showBalloon(
    '错误',
    '这是一条错误提示',
    10000,
    'Error'
);
```

**参数**：
- `title` (string): 气泡标题
- `content` (string): 气泡内容
- `timeout` (number): 显示时长（毫秒），默认30000
- `icon` (string): 图标类型，默认'Info'

**图标类型**：
- `None` - 无图标
- `Info` - 信息图标
- `Warning` - 警告图标
- `Error` - 错误图标

#### onClick(callback)

设置托盘图标单击事件。

```javascript
// 设置点击事件
await puppet.tray.onClick('onTrayClick');

// 定义回调函数
window.onTrayClick = function() {
    console.log('托盘图标被点击');
    // 显示主窗口
    puppet.window.show();
};
```

**参数**：
- `callback` (string): JavaScript函数名

#### onDoubleClick(callback)

设置托盘图标双击事件。

```javascript
// 设置双击事件
await puppet.tray.onDoubleClick('onTrayDoubleClick');

// 定义回调函数
window.onTrayDoubleClick = function() {
    console.log('托盘图标被双击');
    // 显示或隐藏主窗口
    if (puppet.window.isVisible()) {
        puppet.window.hide();
    } else {
        puppet.window.show();
    }
};
```

#### hide()

隐藏托盘图标。

```javascript
await puppet.tray.hide();
```

#### show()

显示托盘图标。

```javascript
await puppet.tray.show();
```

#### setIcon(iconPath)

设置托盘图标。

```javascript
await puppet.tray.setIcon('C:\\icons\\app.ico');
```

**参数**：
- `iconPath` (string): 图标文件路径

**支持格式**：.ico, .png, .jpg

### 使用示例

```javascript
// 完整的托盘图标设置
async function setupTrayIcon() {
    // 创建托盘图标
    await puppet.tray.setTray('我的应用 v1.0');
    
    // 设置菜单
    await puppet.tray.setMenu([
        { Text: '显示主窗口', Command: 'showMain' },
        { Text: '暂停监控', Command: 'pauseMonitoring' },
        { Text: '恢复监控', Command: 'resumeMonitoring' },
        { Text: '-' },
        { Text: '关于', Command: 'showAbout' },
        { Text: '退出', Command: 'quit' }
    ]);
    
    // 设置点击事件
    await puppet.tray.onClick('onTrayClick');
    
    // 设置双击事件
    await puppet.tray.onDoubleClick('onTrayDoubleClick');
    
    // 设置自定义图标
    await puppet.tray.setIcon('C:\\icons\\myapp.ico');
}

// 定义所有回调函数
window.showMain = function() {
    puppet.window.show();
    puppet.window.setTopmost(true);
};

window.pauseMonitoring = function() {
    console.log('监控已暂停');
    // 暂停逻辑
};

window.resumeMonitoring = function() {
    console.log('监控已恢复');
    // 恢复逻辑
};

window.showAbout = function() {
    alert('我的应用 v1.0\n\n由 Puppet 框架构建');
};

window.quit = function() {
    puppet.Application.close();
};

window.onTrayClick = function() {
    // 单击切换窗口可见性
    console.log('托盘被点击');
};

window.onTrayDoubleClick = function() {
    // 双击显示主窗口
    showMain();
};

// 通知系统
async function sendNotification(title, message) {
    await puppet.tray.showBalloon(
        title,
        message,
        5000,
        'Info'
    );
}

// 使用示例
setupTrayIcon();

// 发送通知
setTimeout(() => {
    sendNotification('欢迎', '欢迎使用我的应用！');
}, 1000);
```

## 透明区域 API

透明区域API用于实现智能点击穿透功能。

### API列表

| 方法 | 参数 | 返回值 | 说明 |
|------|------|--------|------|
| enableTransparentMode | () | void | 启用透明模式 |
| disableTransparentMode | () | void | 禁用透明模式 |
| sendTransparentRegions | () | void | 发送透明区域数据 |
| startTransparentRegionMonitoring | () | void | 开始监听DOM变化 |
| stopTransparentRegionMonitoring | () | void | 停止监听DOM变化 |

### 详细说明

#### enableTransparentMode()

启用透明模式，自动检测透明区域并实现智能点击穿透。

```javascript
// 启用透明模式
puppet.enableTransparentMode();
```

**工作原理**：
1. 遍历DOM树，检测透明元素
2. 收集透明区域的坐标
3. 将区域数据发送到C#端
4. 实现智能点击穿透

**透明元素判定**：
- `background: transparent`
- `rgba(0, 0, 0, 0)` 或 alpha <= 0.05
- `opacity` <= 0.05
- `visibility: hidden` 或 `display: none`

#### disableTransparentMode()

禁用透明模式。

```javascript
// 禁用透明模式
puppet.disableTransparentMode();
```

#### sendTransparentRegions()

手动发送透明区域数据。

```javascript
// 手动发送透明区域
puppet.sendTransparentRegions();
```

**使用场景**：
- 动态更新透明区域
- 手动触发区域更新

#### startTransparentRegionMonitoring()

开始监听DOM变化，自动更新透明区域。

```javascript
// 开始监听
puppet.startTransparentRegionMonitoring();
```

**监听的事件**：
- DOM子元素变化
- 样式属性变化（style, class, hidden）
- 鼠标移动事件

#### stopTransparentRegionMonitoring()

停止监听DOM变化。

```javascript
// 停止监听
puppet.stopTransparentRegionMonitoring();
```

### 使用示例

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        body {
            margin: 0;
            padding: 0;
            background-color: transparent;
        }
        
        .transparent-region {
            background-color: transparent;
            width: 200px;
            height: 200px;
            border: 1px dashed rgba(255, 255, 255, 0.3);
        }
        
        .opaque-region {
            background-color: rgba(255, 0, 0, 0.5);
            width: 200px;
            height: 200px;
            color: white;
            text-align: center;
            line-height: 200px;
        }
    </style>
</head>
<body>
    <!-- 透明区域，点击会穿透 -->
    <div class="transparent-region">
        透明区域（点击穿透）
    </div>
    
    <!-- 不透明区域，点击会被捕获 -->
    <div class="opaque-region">
        不透明区域
    </div>
    
    <script>
        // 启用透明模式
        puppet.enableTransparentMode();
        
        // 动态添加透明元素
        function addTransparentElement() {
            const div = document.createElement('div');
            div.style.backgroundColor = 'transparent';
            div.style.width = '100px';
            div.style.height = '100px';
            div.style.border = '1px dashed rgba(255,255,255,0.3)';
            document.body.appendChild(div);
            
            // 透明区域会自动更新
        }
        
        // 切换元素透明度
        function toggleOpacity(element) {
            if (element.style.opacity === '0') {
                element.style.opacity = '1';
            } else {
                element.style.opacity = '0';
            }
            // 透明区域会自动更新
        }
    </script>
</body>
</html>
```

```javascript
// 动态透明效果
async function createDynamicOverlay() {
    // 启用透明模式
    puppet.enableTransparentMode();
    
    // 设置窗口透明
    await puppet.window.setOpacity(0.3);
    await puppet.window.setBorderless(true);
    await puppet.window.setTopmost(true);
    
    // 创建覆盖层
    const overlay = document.createElement('div');
    overlay.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        pointer-events: none;
        background-color: transparent;
    `;
    document.body.appendChild(overlay);
    
    // 添加交互区域
    const interactiveArea = document.createElement('div');
    interactiveArea.style.cssText = `
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        width: 300px;
        height: 200px;
        background-color: rgba(0, 0, 255, 0.7);
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        pointer-events: auto;
        border-radius: 10px;
    `;
    interactiveArea.textContent = '可点击区域';
    document.body.appendChild(interactiveArea);
}

// 透明度动画
function animateOpacity() {
    let opacity = 0;
    let direction = 1;
    
    setInterval(() => {
        opacity += direction * 0.05;
        
        if (opacity >= 1) {
            opacity = 1;
            direction = -1;
        } else if (opacity <= 0) {
            opacity = 0;
            direction = 1;
        }
        
        // 创建透明元素
        const element = document.createElement('div');
        element.style.backgroundColor = `rgba(255, 0, 0, ${opacity})`;
        element.style.width = '50px';
        element.style.height = '50px';
        element.style.position = 'absolute';
        element.style.top = Math.random() * 500 + 'px';
        element.style.left = Math.random() * 500 + 'px';
        document.body.appendChild(element);
        
        // 自动更新透明区域
    }, 100);
}
```

## 消息通信

通过`chrome.webview.postMessage`可以向C#端发送消息。

### 发送消息

```javascript
// 发送消息到C#
chrome.webview.postMessage({
    type: 'messageType',
    data: {
        key: 'value'
    }
});
```

### 消息格式

```javascript
{
    type: string,      // 消息类型
    data: any         // 消息数据
}
```

### 使用示例

```javascript
// 发送透明区域数据
function updateTransparentRegions(regions) {
    chrome.webview.postMessage({
        type: 'transparentRegions',
        data: regions
    });
}

// 发送用户操作日志
function logUserAction(action) {
    chrome.webview.postMessage({
        type: 'userAction',
        data: {
            action: action,
            timestamp: new Date().toISOString(),
            userId: 'user123'
        }
    });
}

// 发送文件上传请求
function requestFileUpload(file) {
    chrome.webview.postMessage({
        type: 'fileUpload',
        data: {
            fileName: file.name,
            fileSize: file.size,
            fileType: file.type
        }
    });
}
```

## 完整示例

### 示例1：简单的控制面板

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #f0f0f0;
        }
        
        .control-panel {
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .button-group {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 20px;
        }
        
        button {
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            background-color: #007bff;
            color: white;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        
        button:hover {
            background-color: #0056b3;
        }
        
        .info {
            margin-top: 20px;
            padding: 10px;
            background-color: #d4edda;
            border-radius: 5px;
            color: #155724;
        }
    </style>
</head>
<body>
    <div class="control-panel">
        <h1>控制面板</h1>
        
        <div class="button-group">
            <button onclick="setBorderless()">无边框</button>
            <button onclick="setOpacity(0.5)">半透明</button>
            <button onclick="setOpacity(1.0)">不透明</button>
            <button onclick="setTopmost()">置顶</button>
            <button onclick="centerWindow()">居中</button>
            <button onclick="getInfo()">获取信息</button>
        </div>
        
        <div class="info" id="info">
            等待操作...
        </div>
    </div>
    
    <script>
        function showInfo(message) {
            document.getElementById('info').textContent = message;
        }
        
        async function setBorderless() {
            try {
                await puppet.window.setBorderless(true);
                showInfo('无边框模式已启用');
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function setOpacity(value) {
            try {
                await puppet.window.setOpacity(value);
                showInfo('透明度已设置为 ' + value);
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function setTopmost() {
            try {
                await puppet.window.setTopmost(true);
                showInfo('窗口已置顶');
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function centerWindow() {
            try {
                await puppet.window.centerWindow();
                showInfo('窗口已居中');
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function getInfo() {
            try {
                const info = await puppet.Application.getWindowInfo();
                showInfo('窗口信息: ' + info);
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
    </script>
</body>
</html>
```

### 示例2：文件管理器

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
        }
        
        .toolbar {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }
        
        button {
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            background-color: #28a745;
            color: white;
            cursor: pointer;
        }
        
        button:hover {
            background-color: #218838;
        }
        
        .file-list {
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 10px;
            min-height: 200px;
        }
        
        .file-item {
            padding: 10px;
            border-bottom: 1px solid #eee;
            cursor: pointer;
        }
        
        .file-item:hover {
            background-color: #f5f5f5;
        }
    </style>
</head>
<body>
    <h1>文件管理器</h1>
    
    <div class="toolbar">
        <button onclick="openFile()">打开文件</button>
        <button onclick="openFolder()">打开文件夹</button>
        <button onclick="saveFile()">保存文件</button>
    </div>
    
    <div class="file-list" id="fileList">
        <p>暂无文件</p>
    </div>
    
    <script>
        let currentFiles = [];
        
        async function openFile() {
            try {
                const filePath = await puppet.fs.openFileDialog('所有文件|*.*', false);
                if (filePath) {
                    addFileToList(filePath);
                    showInfo('已打开文件: ' + filePath);
                }
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function openFolder() {
            try {
                const folderPath = await puppet.fs.openFolderDialog();
                if (folderPath) {
                    showInfo('已打开文件夹: ' + folderPath);
                }
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        async function saveFile() {
            try {
                const filePath = await puppet.fs.openFileDialog('文本文件|*.txt', false);
                if (filePath) {
                    const content = prompt('请输入文件内容:');
                    if (content) {
                        await puppet.fs.writeTextToFile(filePath, content);
                        showInfo('文件已保存: ' + filePath);
                    }
                }
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        function addFileToList(filePath) {
            currentFiles.push(filePath);
            renderFileList();
        }
        
        function renderFileList() {
            const listDiv = document.getElementById('fileList');
            
            if (currentFiles.length === 0) {
                listDiv.innerHTML = '<p>暂无文件</p>';
                return;
            }
            
            listDiv.innerHTML = currentFiles.map(file => 
                `<div class="file-item" onclick="selectFile('${file}')">${file}</div>`
            ).join('');
        }
        
        async function selectFile(filePath) {
            try {
                const content = await puppet.fs.readFileAsJson(filePath);
                alert('文件内容:\n' + content);
            } catch (error) {
                showInfo('错误: ' + error.message);
            }
        }
        
        function showInfo(message) {
            alert(message);
        }
    </script>
</body>
</html>
```

### 示例3：系统监控面板

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #1a1a1a;
            color: white;
        }
        
        .dashboard {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 20px;
        }
        
        .card {
            background-color: #2d2d2d;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.3);
        }
        
        .card h2 {
            margin-top: 0;
            color: #4fc3f7;
        }
        
        .info-item {
            margin: 10px 0;
            padding: 10px;
            background-color: #3d3d3d;
            border-radius: 5px;
        }
        
        .button-group {
            display: flex;
            gap: 10px;
            margin-top: 20px;
        }
        
        button {
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            background-color: #4fc3f7;
            color: white;
            cursor: pointer;
        }
        
        button:hover {
            background-color: #29b6f6;
        }
        
        #screenshot {
            max-width: 100%;
            margin-top: 10px;
            border-radius: 5px;
        }
    </style>
</head>
<body>
    <h1>系统监控面板</h1>
    
    <div class="dashboard">
        <div class="card">
            <h2>系统信息</h2>
            <div id="systemInfo">
                <p>加载中...</p>
            </div>
            <div class="button-group">
                <button onclick="loadSystemInfo()">刷新</button>
            </div>
        </div>
        
        <div class="card">
            <h2>鼠标位置</h2>
            <div id="mousePosition">
                <p>等待数据...</p>
            </div>
            <div class="button-group">
                <button onclick="startMouseTracking()">开始追踪</button>
                <button onclick="stopMouseTracking()">停止追踪</button>
            </div>
        </div>
        
        <div class="card">
            <h2>屏幕截图</h2>
            <div id="screenshotArea">
                <p>点击按钮截图</p>
            </div>
            <div class="button-group">
                <button onclick="takeScreenshot()">截图</button>
                <button onclick="saveScreenshot()">保存</button>
            </div>
        </div>
    </div>
    
    <script>
        let mouseTrackingInterval = null;
        let currentScreenshot = null;
        
        async function loadSystemInfo() {
            try {
                const info = await puppet.system.getSystemInfo();
                const data = JSON.parse(info);
                
                document.getElementById('systemInfo').innerHTML = `
                    <div class="info-item">操作系统: ${data.OperatingSystem.OSName}</div>
                    <div class="info-item">版本: ${data.OperatingSystem.Version}</div>
                    <div class="info-item">用户: ${data.OperatingSystem.UserName}</div>
                    <div class="info-item">处理器: ${data.Hardware.ProcessorCount} 核心</div>
                    <div class="info-item">总内存: ${(data.Memory.TotalPhysicalMemory / 1024 / 1024 / 1024).toFixed(2)} GB</div>
                    <div class="info-item">屏幕: ${data.Display.PrimaryScreen.Width}x${data.Display.PrimaryScreen.Height}</div>
                    <div class="info-item">显卡: ${data.GPU}</div>
                `;
            } catch (error) {
                console.error('加载系统信息失败:', error);
            }
        }
        
        function startMouseTracking() {
            if (mouseTrackingInterval) return;
            
            mouseTrackingInterval = setInterval(async () => {
                try {
                    const position = await puppet.system.getMousePosition();
                    const pos = JSON.parse(position);
                    
                    document.getElementById('mousePosition').innerHTML = `
                        <div class="info-item">X: ${pos.X}</div>
                        <div class="info-item">Y: ${pos.Y}</div>
                    `;
                } catch (error) {
                    console.error('获取鼠标位置失败:', error);
                }
            }, 100);
        }
        
        function stopMouseTracking() {
            if (mouseTrackingInterval) {
                clearInterval(mouseTrackingInterval);
                mouseTrackingInterval = null;
            }
        }
        
        async function takeScreenshot() {
            try {
                const screenshot = await puppet.system.takeScreenShot();
                currentScreenshot = screenshot;
                
                const img = document.createElement('img');
                img.id = 'screenshot';
                img.src = 'data:image/png;base64,' + screenshot;
                
                document.getElementById('screenshotArea').innerHTML = '';
                document.getElementById('screenshotArea').appendChild(img);
            } catch (error) {
                console.error('截图失败:', error);
            }
        }
        
        async function saveScreenshot() {
            if (!currentScreenshot) {
                alert('请先截图');
                return;
            }
            
            try {
                const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
                const filename = `screenshot-${timestamp}.png`;
                
                await puppet.fs.writeByteToFile(`C:\\screenshots\\${filename}`, currentScreenshot);
                alert('截图已保存: ' + filename);
            } catch (error) {
                alert('保存失败: ' + error.message);
            }
        }
        
        // 页面加载时自动加载系统信息
        loadSystemInfo();
    </script>
</body>
</html>
```

## 注意事项

1. **异步调用**：所有API都是异步的，必须使用`async/await`
2. **错误处理**：始终使用try-catch处理可能的错误
3. **权限限制**：某些系统路径无法访问
4. **性能考虑**：频繁调用可能会影响性能
5. **安全性**：不要信任用户输入，始终进行验证

## 总结

Puppet框架提供了丰富的JavaScript API，让开发者可以轻松创建功能强大的桌面应用。通过本指南，你应该能够：

- 使用窗口控制API操作应用程序窗口
- 使用应用控制API管理应用生命周期
- 使用文件系统API读写文件
- 使用日志API记录应用日志
- 使用系统API获取系统信息
- 使用托盘图标API管理系统托盘
- 使用透明区域API实现智能点击穿透

如有问题，请参考代码示例或查看技术文档。