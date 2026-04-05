# @puppet-framework/types

Puppet Framework 的 TypeScript 类型定义和运行时支持库。

## 功能特性

- ✅ 完整的 TypeScript 类型定义和 IntelliSense 支持
- ✅ 浏览器环境开发支持（使用模拟实现）
- ✅ Node.js 环境开发支持（使用模拟实现）
- ✅ 自动环境检测
- ✅ 类型安全的 API 使用
- ✅ 在框架环境下使用真实的 puppet API

## 安装

```bash
npm install @puppet-framework/types --save-dev
```

或者复制文件到您的项目中：

```bash
# 复制所有文件到您的项目目录
cp puppet-node/* your-project/
```

## 使用方法

### TypeScript 项目

1. 在您的 TypeScript 文件顶部导入类型：

```typescript
/// <reference types="@puppet-framework/types" />

// 或者
import puppet from '@puppet-framework/types';
```

2. 使用 puppet API：

```typescript
// 设置窗口为无边框
await puppet.window.setBorderless(true);

// 设置窗口透明度
await puppet.window.setOpacity(0.9);

// 读取文件
const content = await puppet.fs.readFileAsText('config.json');

// 获取系统信息
const sysInfo = await puppet.system.getSystemInfo();
console.log('操作系统:', sysInfo.osName);
```

### JavaScript 项目

1. 在 HTML 中引入 JavaScript 文件：

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>My Puppet App</title>
</head>
<body>
    <script src="puppet.js"></script>
    <script>
        // 使用 puppet API
        async function init() {
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
        }
        
        init();
    </script>
</body>
</html>
```

### Node.js 项目

```javascript
// 导入 puppet
const puppet = require('@puppet-framework/types');

// 使用 puppet API
async function main() {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log('系统信息:', sysInfo);
}

main();
```

## 环境检测

库会自动检测运行环境：

- **Puppet Framework 环境**：使用真实的 puppet API（由 C# 应用注入）
- **浏览器环境**：使用模拟实现，用于开发和调试
- **Node.js 环境**：使用模拟实现，用于测试和开发

## API 文档

### Window API

```typescript
// 窗口控制
await puppet.window.setBorderless(true);
await puppet.window.setDraggable(true);
await puppet.window.setResizable(true);
await puppet.window.setTransparent(true);
await puppet.window.setOpacity(0.9);
await puppet.window.setTopmost(true);
await puppet.window.centerWindow();
await puppet.window.moveWindow(100, 200);
await puppet.window.resizeWindow(800, 600);
```

### Application API

```typescript
// 应用控制
await puppet.application.close();
await puppet.application.restart();
const windowInfo = await puppet.application.getWindowInfo();
await puppet.application.execute('calc.exe');
```

### File System API

```typescript
// 文件系统
const files = await puppet.fs.openFileDialog(['文本文件', '*.txt'], false);
const content = await puppet.fs.readFileAsText(files[0]);
await puppet.fs.writeTextToFile('output.txt', 'Hello World');
const exists = await puppet.fs.exists('file.txt');
await puppet.fs.delete('file.txt');
```

### System API

```typescript
// 系统功能
const sysInfo = await puppet.system.getSystemInfo();
const screenshot = await puppet.system.takeScreenShot();
await puppet.system.sendKey('A', 'B', 'ENTER');
await puppet.system.sendMouseClick(100, 200, 'left');
const mousePos = await puppet.system.getMousePosition();
```

### Tray API

```typescript
// 托盘图标
await puppet.tray.setTray('My App');
await puppet.tray.setMenu([
    { Text: '打开', Command: 'open' },
    { Text: '退出', Command: 'exit' }
]);
await puppet.tray.showBalloon('通知', '消息内容', 30000, 'Info');
```

### Events API

```typescript
// 事件系统
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
});
```

### Device API

```typescript
// 设备系统
const device = await puppet.device.getDevice('C:');
const devices = await puppet.device.getDevices(puppet.device.type.usbDisk);
```

## 类型定义

所有 API 都有完整的 TypeScript 类型定义：

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

## 开发模式

在浏览器中开发时，所有 puppet API 调用都会输出到控制台：

```javascript
// 浏览器控制台输出
[MOCK] setBorderless: true
[MOCK] setOpacity: 0.9
[MOCK] getSystemInfo
```

这允许您在不运行 Puppet Framework 的情况下开发和测试代码。

## 生产环境

在生产环境（Puppet Framework 应用）中，模拟实现会被自动替换为真实的 puppet API。

## 最佳实践

### 1. 使用 TypeScript

推荐使用 TypeScript 以获得完整的类型检查和 IntelliSense：

```typescript
async function initWindow() {
    // TypeScript 会检查参数类型
    await puppet.window.setBorderless(true);
    await puppet.window.setOpacity(0.9);
    await puppet.window.centerWindow();
}
```

### 2. 错误处理

始终处理可能的错误：

```typescript
try {
    const content = await puppet.fs.readFileAsText('config.json');
    const config = JSON.parse(content);
    return config;
} catch (error) {
    puppet.log.error('读取配置失败:', error.message);
    return null;
}
```

### 3. 环境兼容

代码应该在不同环境下都能工作：

```typescript
async function saveData(data: any) {
    try {
        await puppet.fs.writeTextToFile('data.json', JSON.stringify(data));
        puppet.log.info('数据已保存');
    } catch (error) {
        // 在浏览器环境中可能会失败，但不会影响功能
        console.error('保存数据失败（开发环境）:', error);
    }
}
```

### 4. 类型检查

利用 TypeScript 的类型系统：

```typescript
interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
}

async function loadConfig(): Promise<AppConfig | null> {
    try {
        const content = await puppet.fs.readFileAsText('config.json');
        return JSON.parse(content) as AppConfig;
    } catch (error) {
        return null;
    }
}
```

### 5. 透明窗口背景

如果要使用透明背景窗口，需要在 HTML 或 body 元素上设置透明背景：

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Transparent Window</title>
    <style>
        /* 设置 html 和 body 背景为透明 */
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
        async function init() {
            // 启用透明背景
            await puppet.window.setTransparent(true);
            
            // 设置窗口为无边框
            await puppet.window.setBorderless(true);
            
            // 设置透明度（可选）
            await puppet.window.setOpacity(0.95);
            
            // 设置透明颜色
            await puppet.window.setTransparentColor('#000000');
            
            // 启用鼠标穿透（可选）
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

## 常见问题

### Q: 为什么在浏览器中调用 puppet API 没有实际效果？

A: 在浏览器环境中，puppet API 使用模拟实现，只输出日志而不执行实际操作。这是为了允许开发和调试。要测试实际功能，需要在 Puppet Framework 环境中运行。

### Q: 如何判断当前运行环境？

A: 检查 `window.chrome` 和 `window.chrome.webview` 是否存在：

```typescript
function isPuppetEnvironment(): boolean {
    return typeof window !== 'undefined' && 
           typeof (window as any).chrome !== 'undefined' && 
           typeof (window as any).chrome.webview !== 'undefined';
}
```

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

## 测试

### 运行测试

```bash
# 运行所有测试
npm test

# 运行测试并监听文件变化
npm run test:watch

# 运行测试并生成覆盖率报告
npm run test:coverage

# 手动运行测试（需要先构建）
npm run build
npm run test:manual
```

### 编写测试

使用提供的测试工具编写测试：

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
        // 设置测试环境
        puppet = setupTestEnvironment({ enableLogging: false, resetState: true });
    });

    afterEach(() => {
        // 清理测试环境
        cleanupTestEnvironment();
    });

    it('should do something', async () => {
        // 测试代码
        await puppet.window.setBorderless(true);
        
        // 验证结果
        const state = puppet.__getMockState?.();
        assertEqual(state?.window.borderless, true);
    });
});
```

### Mock 功能

库提供了完整的 mock 实现，用于测试：

```typescript
import { createMockPuppetNamespace, getMockState } from '@puppet-framework/types/mock';

// 创建 mock puppet
const mockPuppet = createMockPuppetNamespace(false);

// 获取 mock 状态
const state = getMockState();

// 访问内部状态
console.log(state.window.borderless);
console.log(state.application.config);
```

### 测试工具

可用的测试工具函数：

- `setupTestEnvironment()` - 设置测试环境
- `cleanupTestEnvironment()` - 清理测试环境
- `getMockState()` - 获取 mock 状态
- `resetMockState()` - 重置 mock 状态
- `simulateEvent()` - 模拟事件
- `assertEqual()` - 断言相等
- `assertTruthy()` - 断言为真
- `assertFalsy()` - 断言为假
- `wait()` - 等待指定时间
- `createSpy()` - 创建 spy 函数

### 测试覆盖率

测试目标覆盖率：

- 分支覆盖率：80%
- 函数覆盖率：80%
- 行覆盖率：80%
- 语句覆盖率：80%

## 许可证

Copyright © 2024-2026 林晚晚ss.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.