---
title: TypeScript 类型定义
permalink: /guide/typescript.html
createTime: 2026/04/05 10:00:00
---

# TypeScript 类型定义

Puppet Framework 提供了完整的 TypeScript 类型定义，让开发者可以在 TypeScript 项目中获得完整的类型支持和 IntelliSense。

## 安装

```bash
npm install @puppet-framework/types --save-dev
```

或者复制文件到您的项目中：

```bash
cp puppet-node/* your-project/
```

## 导入类型

### 方式一：使用 triple-slash 指令

```typescript
/// <reference types="@puppet-framework/types" />

async function init() {
    await puppet.window.setBorderless(true);
}
```

### 方式二：使用 import 语句

```typescript
import puppet from '@puppet-framework/types';

async function init() {
    await puppet.window.setBorderless(true);
}
```

### 方式三：在 tsconfig.json 中配置

```json
{
  "compilerOptions": {
    "types": ["@puppet-framework/types"]
  }
}
```

## 类型定义

### PuppetNamespace

完整的 puppet 命名空间类型定义：

```typescript
interface PuppetNamespace {
    window: WindowController;
    application: ApplicationController;
    fs: FileSystemController;
    log: LogController;
    system: SystemController;
    tray: TrayController;
    events: EventController;
    device: DeviceController;
    type: DeviceTypes;
    status: DeviceStatuses;
}
```

### WindowController

窗口控制器类型定义：

```typescript
interface WindowController {
    setBorderless(value: boolean): Promise<void>;
    setDraggable(value: boolean): Promise<void>;
    setResizable(value: boolean): Promise<void>;
    setTransparent(value: boolean): Promise<void>;
    setOpacity(value: number): Promise<void>;
    setMouseThroughTransparency(value: boolean): Promise<void>;
    setMouseThrough(value: boolean): Promise<void>;
    setTransparentColor(color: string): Promise<void>;
    setTopmost(value: boolean): Promise<void>;
    moveWindow(x: number, y: number): Promise<void>;
    resizeWindow(width: number, height: number): Promise<void>;
    centerWindow(): Promise<void>;
    showInTaskbar(value: boolean): Promise<void>;
    mountMovableElement(elementId: string): Promise<void>;
    unmountMovableElement(elementId: string): Promise<void>;
}
```

### ApplicationController

应用控制器类型定义：

```typescript
interface ApplicationController {
    close(): Promise<void>;
    restart(): Promise<void>;
    getWindowInfo(): Promise<WindowInfo>;
    execute(command: string): Promise<void>;
    setConfig(key: string, value: any): Promise<void>;
    getConfig(key: string): Promise<any>;
    getAssemblyDirectory(): Promise<string>;
    getAppDataDirectory(): Promise<string>;
    getCurrentUser(): Promise<UserInfo>;
}
```

### 其他控制器

其他控制器的类型定义包括：
- `FileSystemController` - 文件系统控制
- `LogController` - 日志控制
- `SystemController` - 系统控制
- `TrayController` - 托盘图标控制
- `EventController` - 事件控制
- `DeviceController` - 设备控制

## 数据类型

### WindowInfo

窗口信息类型：

```typescript
interface WindowInfo {
    handle: number;
    title: string;
    className: string;
    isVisible: boolean;
    isMinimized: boolean;
    isMaximized: boolean;
    width: number;
    height: number;
    x: number;
    y: number;
}
```

### SystemInfo

系统信息类型：

```typescript
interface SystemInfo {
    osName: string;
    osVersion: string;
    computerName: string;
    cpuModel: string;
    cpuCores: number;
    totalMemory: number;
    availableMemory: number;
    gpuModel: string;
    screenWidth: number;
    screenHeight: number;
    is64Bit: boolean;
}
```

### DeviceInfo

设备信息类型：

```typescript
interface DeviceInfo {
    DeviceId: string;
    DeviceType: number;
    DeviceName: string;
    Status: number;
    DriveLetter?: string;
    VolumeName?: string;
    FileSystem?: string;
    TotalSize: number;
    FreeSpace: number;
    UsedSpace: number;
    Manufacturer?: string;
    Model?: string;
    SerialNumber?: string;
}
```

## 使用示例

### 类型安全的 API 调用

```typescript
import puppet from '@puppet-framework/types';

// 设置窗口样式
async function setupWindow() {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
    await puppet.window.setOpacity(0.95);
}

// 读取配置文件
async function loadConfig(): Promise<MyConfig | null> {
    try {
        const content = await puppet.fs.readFileAsText('config.json');
        return JSON.parse(content) as MyConfig;
    } catch (error) {
        puppet.log.error('读取配置失败:', error);
        return null;
    }
}

// 获取系统信息
async function getSystemInfo() {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log(`操作系统: ${sysInfo.osName} ${sysInfo.osVersion}`);
    console.log(`CPU: ${sysInfo.cpuModel} (${sysInfo.cpuCores} 核)`);
    console.log(`内存: ${sysInfo.availableMemory} / ${sysInfo.totalMemory} MB`);
}
```

### 自定义类型定义

```typescript
// 定义应用配置类型
interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
    language: 'zh-CN' | 'en-US';
    autoUpdate: boolean;
}

// 定义用户设置类型
interface UserSettings {
    name: string;
    email: string;
    notifications: boolean;
}

// 使用类型
async function saveConfig(config: AppConfig) {
    await puppet.fs.writeTextToFile(
        'config.json',
        JSON.stringify(config, null, 2)
    );
}

async function loadSettings(): Promise<UserSettings | null> {
    try {
        const content = await puppet.fs.readFileAsText('settings.json');
        return JSON.parse(content) as UserSettings;
    } catch (error) {
        return null;
    }
}
```

### 事件处理类型

```typescript
// 定义事件数据类型
interface USBEvent {
    DeviceId: string;
    DeviceName: string;
    DriveLetter: string;
    DeviceType: number;
}

interface WindowEvent {
    eventType: string;
    timestamp: number;
}

// 类型安全的事件监听
async function setupEventListeners() {
    // USB 设备插入事件
    await puppet.events.addEventListener('usb-plug-in', (event: { data: USBEvent }) => {
        console.log(`USB 设备插入: ${event.data.DeviceName} (${event.data.DriveLetter})`);
        puppet.log.info(`检测到 USB 设备: ${event.data.DeviceName}`);
    });

    // 窗口最大化事件
    await puppet.events.addEventListener('window-maximize', (event: WindowEvent) => {
        console.log(`窗口在 ${new Date(event.timestamp).toLocaleTimeString()} 最大化`);
    });
}
```

## 类型守卫

使用类型守卫进行运行时类型检查：

```typescript
import { isWindowInfo, isSystemInfo, isDeviceInfo } from '@puppet-framework/types';

// 检查窗口信息
function handleWindowInfo(info: any) {
    if (isWindowInfo(info)) {
        // TypeScript 现在知道 info 是 WindowInfo 类型
        console.log(`窗口: ${info.title} (${info.width}x${info.height})`);
    } else {
        console.error('无效的窗口信息');
    }
}

// 检查系统信息
function handleSystemInfo(info: any) {
    if (isSystemInfo(info)) {
        // TypeScript 现在知道 info 是 SystemInfo 类型
        console.log(`系统: ${info.osName} ${info.osVersion}`);
    } else {
        console.error('无效的系统信息');
    }
}

// 检查设备信息
function handleDeviceInfo(device: any) {
    if (isDeviceInfo(device)) {
        // TypeScript 现在知道 device 是 DeviceInfo 类型
        console.log(`设备: ${device.DeviceName} (${device.DeviceType})`);
    } else {
        console.error('无效的设备信息');
    }
}
```

## 环境检测

检测当前运行环境：

```typescript
import { 
    isPuppetEnvironment, 
    isBrowserEnvironment, 
    isNodeEnvironment 
} from '@puppet-framework/types';

function checkEnvironment() {
    if (isPuppetEnvironment()) {
        console.log('运行在 Puppet Framework 环境');
        // 使用真实的 puppet API
    } else if (isBrowserEnvironment()) {
        console.log('运行在浏览器环境');
        // 使用 mock 实现
    } else if (isNodeEnvironment()) {
        console.log('运行在 Node.js 环境');
        // 使用 mock 实现
    }
}
```

## 透明窗口

如果要使用透明背景窗口，必须在 HTML 和 body 元素上设置透明背景：

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Transparent Window</title>
    <style>
        /* 必须设置 html 和 body 背景为透明 */
        html {
            background: transparent;
        }
        
        body {
            background: transparent;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
        
        /* 可选：添加渐变背景到内容区域 */
        .content {
            background: linear-gradient(135deg, rgba(255,255,255,0.1), rgba(255,255,255,0.05));
            backdrop-filter: blur(10px);
            border-radius: 20px;
            padding: 20px;
        }
    </style>
</head>
<body>
    <div class="content">
        <h1>Transparent Window</h1>
        <p>This window has a transparent background.</p>
    </div>
    
    <script src="puppet.js"></script>
    <script>
        import puppet from '@puppet-framework/types';
        
        async function init() {
            // 启用透明背景
            await puppet.window.setTransparent(true);
            
            // 设置窗口为无边框
            await puppet.window.setBorderless(true);
            
            // 设置透明度
            await puppet.window.setOpacity(0.95);
            
            // 设置透明颜色
            await puppet.window.setTransparentColor('#000000');
            
            // 启用鼠标穿透
            await puppet.window.setMouseThroughTransparency(true);
        }
        
        init();
    </script>
</body>
</html>
```

**重要提示：**
- 必须同时在 `html` 和 `body` 元素上设置 `background: transparent`
- 使用 `setTransparentColor()` 设置需要透明的颜色
- 使用 `setMouseThroughTransparency()` 让鼠标点击穿透透明区域
- 使用 `setOpacity()` 调整整体透明度（0.0 - 1.0）

## 开发模式

在浏览器或 Node.js 环境中开发时，所有 puppet API 调用都会输出到控制台：

```javascript
// 浏览器控制台输出
[MOCK] setBorderless: true
[MOCK] setOpacity: 0.9
[MOCK] getSystemInfo
```

这允许您在不运行 Puppet Framework 的情况下开发和测试代码。

## 测试

### 设置测试环境

```typescript
import { 
    setupTestEnvironment, 
    cleanupTestEnvironment,
    assertEqual,
    assertTruthy
} from '@puppet-framework/types/test-utils';

describe('My Feature', () => {
    let puppet: any;

    beforeEach(() => {
        puppet = setupTestEnvironment({ 
            enableLogging: false, 
            resetState: true 
        });
    });

    afterEach(() => {
        cleanupTestEnvironment();
    });

    it('should set window to borderless', async () => {
        await puppet.window.setBorderless(true);
        
        const state = puppet.__getMockState?.();
        assertEqual(state?.window.borderless, true);
    });
});
```

### 使用 Mock

```typescript
import { 
    createMockPuppetNamespace, 
    getMockState,
    simulateEvent 
} from '@puppet-framework/types/mock';

// 创建 mock puppet
const mockPuppet = createMockPuppetNamespace(false);

// 获取 mock 状态
const state = getMockState();

// 访问内部状态
console.log(state.window.borderless);
console.log(state.application.config);

// 模拟事件
simulateEvent('usb-plug-in', {
    DeviceId: 'E:',
    DeviceName: 'USB Drive',
    DeviceType: 2
});
```

## 最佳实践

### 1. 使用类型接口

为应用数据定义类型接口：

```typescript
interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
    language: 'zh-CN' | 'en-US';
}

interface UserPreferences {
    autoSave: boolean;
    notifications: boolean;
    checkUpdates: boolean;
}
```

### 2. 错误处理

使用 try-catch 处理可能的错误：

```typescript
async function loadConfig(): Promise<AppConfig | null> {
    try {
        const content = await puppet.fs.readFileAsText('config.json');
        const config = JSON.parse(content) as AppConfig;
        
        // 验证配置
        if (isValidConfig(config)) {
            return config;
        }
        
        return getDefaultConfig();
    } catch (error) {
        puppet.log.error('加载配置失败:', error);
        return getDefaultConfig();
    }
}
```

### 3. 环境兼容

编写兼容不同环境的代码：

```typescript
async function saveData(data: any) {
    try {
        await puppet.fs.writeTextToFile('data.json', JSON.stringify(data));
        puppet.log.info('数据已保存');
    } catch (error) {
        // 在开发环境中可能会失败
        if (isPuppetEnvironment()) {
            throw error;
        } else {
            console.warn('保存数据失败（开发环境）:', error);
        }
    }
}
```

### 4. 类型断言

谨慎使用类型断言，优先使用类型守卫：

```typescript
// 好的做法：使用类型守卫
if (isWindowInfo(info)) {
    console.log(info.title);
}

// 避免：过度使用类型断言
const info = someValue as WindowInfo; // 可能不安全
```

## 常见问题

### Q: TypeScript 报错 "Cannot find name 'puppet'"

A: 确保已正确导入类型定义：

```typescript
/// <reference types="@puppet-framework/types" />
```

或者在 `tsconfig.json` 中添加：

```json
{
  "compilerOptions": {
    "types": ["@puppet-framework/types"]
  }
}
```

### Q: 为什么在浏览器中调用 API 没有实际效果？

A: 在浏览器环境中，puppet API 使用模拟实现，只输出日志。在 Puppet Framework 环境中会有实际效果。

### Q: 如何判断当前运行环境？

A: 使用提供的工具函数：

```typescript
import { isPuppetEnvironment } from '@puppet-framework/types';

if (isPuppetEnvironment()) {
    console.log('运行在 Puppet Framework 环境');
}
```

### Q: 透明窗口背景不生效怎么办？

A: 确保同时设置了 `html` 和 `body` 的背景为透明：

```css
html {
    background: transparent;
}

body {
    background: transparent;
}
```

然后调用：

```typescript
await puppet.window.setTransparent(true);
await puppet.window.setTransparentColor('#000000');
```

## 相关链接

- [完整 API 文档](/api/)
- [快速开始](/guide/getting-started.html)
- [最佳实践](/guide/best-practices.html)
- [测试文档](https://github.com/bil-fis/puppet/blob/main/puppet-node/README.md#测试)