---
title: 系统 API
permalink: /api/system.html
createTime: 2026/03/28 15:08:10
---

# 系统 API

系统 API 提供了系统信息获取、输入模拟、截图等功能。

## 概述

`puppet.system` 命名空间提供以下功能：

- 获取系统信息（CPU、内存、GPU、操作系统）
- 屏幕截图
- 获取桌面壁纸
- 模拟按键
- 模拟鼠标点击
- 获取鼠标位置

## 方法

### getSystemInfo()

获取系统信息。

```javascript
await puppet.system.getSystemInfo(): Promise<SystemInfo>
```

**返回值**：

```typescript
interface SystemInfo {
    osName: string;           // 操作系统名称
    osVersion: string;        // 操作系统版本
    computerName: string;     // 计算机名称
    cpuModel: string;         // CPU 型号
    cpuCores: number;         // CPU 核心数
    totalMemory: number;      // 总内存（MB）
    availableMemory: number;  // 可用内存（MB）
    gpuModel: string;         // GPU 型号
    screenWidth: number;      // 屏幕宽度
    screenHeight: number;     // 屏幕高度
    is64Bit: boolean;         // 是否为 64 位系统
}
```

**示例**：

```javascript
// 获取系统信息
const sysInfo = await puppet.system.getSystemInfo();
console.log('操作系统:', sysInfo.osName);
console.log('CPU:', sysInfo.cpuModel, `(${sysInfo.cpuCores} 核)`);
console.log('内存:', sysInfo.totalMemory, 'MB');
console.log('屏幕:', `${sysInfo.screenWidth}x${sysInfo.screenHeight}`);
```

### takeScreenShot()

截取屏幕截图。

```javascript
await puppet.system.takeScreenShot(): Promise<string>
```

**返回值**：

Base64 编码的 PNG 图片数据。

**示例**：

```javascript
// 截取屏幕
const screenshot = await puppet.system.takeScreenShot();

// 显示截图
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + screenshot;
document.body.appendChild(img);

// 保存截图
await puppet.fs.writeByteToFile('screenshot.png', screenshot);
```

### getDesktopWallpaper()

获取桌面壁纸。

```javascript
await puppet.system.getDesktopWallpaper(): Promise<string>
```

**返回值**：

Base64 编码的图片数据。

**示例**：

```javascript
// 获取桌面壁纸
const wallpaper = await puppet.system.getDesktopWallpaper();

// 显示壁纸
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + wallpaper;
document.body.appendChild(img);

// 保存壁纸
await puppet.fs.writeByteToFile('wallpaper.png', wallpaper);
```

### sendKey()

模拟按键。

```javascript
await puppet.system.sendKey(...keys: string[]): Promise<void>
```

**参数**：

- `keys` (string[]) - 要模拟的按键列表

**支持的按键**：

- 字母：A-Z
- 数字：0-9
- 功能键：ENTER, TAB, SPACE, ESC
- 控制键：CTRL, ALT, SHIFT
- 方向键：UP, DOWN, LEFT, RIGHT

**示例**：

```javascript
// 模拟单个按键
await puppet.system.sendKey('ENTER');
await puppet.system.sendKey('A');
await puppet.system.sendKey('SPACE');

// 模拟组合键
await puppet.system.sendKey('CTRL', 'C');      // 复制
await puppet.system.sendKey('CTRL', 'V');      // 粘贴
await puppet.system.sendKey('CTRL', 'A');      // 全选
await puppet.system.sendKey('ALT', 'TAB');     // 切换窗口

// 模拟快捷键
await puppet.system.sendKey('CTRL', 'S');      // 保存
await puppet.system.sendKey('CTRL', 'Z');      // 撤销
await puppet.system.sendKey('CTRL', 'SHIFT', 'ESC'); // 任务管理器
```

### sendMouseClick()

模拟鼠标点击。

```javascript
await puppet.system.sendMouseClick(x: number, y: number, button: string): Promise<void>
```

**参数**：

- `x` (number) - 鼠标 X 坐标
- `y` (number) - 鼠标 Y 坐标
- `button` (string) - 鼠标按钮（'left', 'right', 'middle'）

**示例**：

```javascript
// 左键点击
await puppet.system.sendMouseClick(100, 200, 'left');

// 右键点击
await puppet.system.sendMouseClick(100, 200, 'right');

// 中键点击
await puppet.system.sendMouseClick(100, 200, 'middle');

// 点击屏幕中心
const sysInfo = await puppet.system.getSystemInfo();
const centerX = sysInfo.screenWidth / 2;
const centerY = sysInfo.screenHeight / 2;
await puppet.system.sendMouseClick(centerX, centerY, 'left');
```

### getMousePosition()

获取鼠标当前位置。

```javascript
await puppet.system.getMousePosition(): Promise<MousePosition>
```

**返回值**：

```typescript
interface MousePosition {
    x: number;  // 鼠标 X 坐标
    y: number;  // 鼠标 Y 坐标
}
```

**示例**：

```javascript
// 获取鼠标位置
const pos = await puppet.system.getMousePosition();
console.log('鼠标位置:', `(${pos.x}, ${pos.y})`);

// 实时追踪鼠标位置
setInterval(async () => {
    const pos = await puppet.system.getMousePosition();
    document.getElementById('mouse-pos').textContent = `X: ${pos.x}, Y: ${pos.y}`;
}, 100);
```

## 使用示例

### 系统信息显示

```javascript
async function displaySystemInfo() {
    const sysInfo = await puppet.system.getSystemInfo();
    
    const infoHTML = `
        <div class="system-info">
            <h2>系统信息</h2>
            <div class="info-item">
                <label>操作系统:</label>
                <span>${sysInfo.osName} ${sysInfo.osVersion}</span>
            </div>
            <div class="info-item">
                <label>计算机名:</label>
                <span>${sysInfo.computerName}</span>
            </div>
            <div class="info-item">
                <label>CPU:</label>
                <span>${sysInfo.cpuModel} (${sysInfo.cpuCores} 核)</span>
            </div>
            <div class="info-item">
                <label>内存:</label>
                <span>${sysInfo.availableMemory} / ${sysInfo.totalMemory} MB</span>
            </div>
            <div class="info-item">
                <label>GPU:</label>
                <span>${sysInfo.gpuModel}</span>
            </div>
            <div class="info-item">
                <label>屏幕:</label>
                <span>${sysInfo.screenWidth} x ${sysInfo.screenHeight}</span>
            </div>
            <div class="info-item">
                <label>架构:</label>
                <span>${sysInfo.is64Bit ? '64 位' : '32 位'}</span>
            </div>
        </div>
    `;
    
    document.getElementById('info-container').innerHTML = infoHTML;
}

// 显示系统信息
displaySystemInfo();
```

### 截图工具

```javascript
class ScreenshotTool {
    async capture() {
        // 截取屏幕
        const screenshot = await puppet.system.takeScreenShot();
        
        // 显示截图
        this.displayScreenshot(screenshot);
        
        // 返回截图数据
        return screenshot;
    }
    
    displayScreenshot(data) {
        const img = document.createElement('img');
        img.src = 'data:image/png;base64,' + data;
        img.className = 'screenshot';
        
        const container = document.getElementById('screenshot-container');
        container.innerHTML = '';
        container.appendChild(img);
    }
    
    async save(data, filename) {
        // 保存截图
        await puppet.fs.writeByteToFile(filename, data);
        console.log('截图已保存:', filename);
    }
    
    async captureAndSave() {
        const data = await this.capture();
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const filename = `screenshot_${timestamp}.png`;
        await this.save(data, filename);
    }
}

// 使用截图工具
const screenshotTool = new ScreenshotTool();

// 捕获截图
document.getElementById('capture-btn').addEventListener('click', async () => {
    await screenshotTool.capture();
});

// 捕获并保存
document.getElementById('save-btn').addEventListener('click', async () => {
    await screenshotTool.captureAndSave();
});
```

### 自动化操作

```javascript
async function performAutomatedAction() {
    // 等待 2 秒
    await new Promise(resolve => setTimeout(resolve, 2000));
    
    // 获取系统信息
    const sysInfo = await puppet.system.getSystemInfo();
    console.log('屏幕尺寸:', `${sysInfo.screenWidth}x${sysInfo.screenHeight}`);
    
    // 点击指定位置
    await puppet.system.sendMouseClick(100, 100, 'left');
    
    // 等待 1 秒
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    // 模拟按键
    await puppet.system.sendKey('A');
    await puppet.system.sendKey('B');
    await puppet.system.sendKey('ENTER');
    
    console.log('自动化操作完成');
}

// 执行自动化操作
performAutomatedAction();
```

### 鼠标追踪器

```javascript
class MouseTracker {
    constructor() {
        this.isTracking = false;
        this.positions = [];
    }
    
    startTracking() {
        this.isTracking = true;
        this.positions = [];
        this.track();
    }
    
    async track() {
        if (!this.isTracking) return;
        
        const pos = await puppet.system.getMousePosition();
        this.positions.push(pos);
        
        // 更新显示
        document.getElementById('mouse-pos').textContent = 
            `X: ${pos.x}, Y: ${pos.y} (${this.positions.length} 点)`;
        
        // 继续追踪
        requestAnimationFrame(() => this.track());
    }
    
    stopTracking() {
        this.isTracking = false;
        console.log('追踪结束，共记录', this.positions.length, '个点');
    }
    
    getPositions() {
        return this.positions;
    }
}

// 使用鼠标追踪器
const tracker = new MouseTracker();

document.getElementById('start-tracking').addEventListener('click', () => {
    tracker.startTracking();
});

document.getElementById('stop-tracking').addEventListener('click', () => {
    tracker.stopTracking();
});
```

## 最佳实践

### 1. 错误处理

捕获系统操作可能出现的错误：

```javascript
try {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log('系统信息:', sysInfo);
} catch (error) {
    puppet.log.error('获取系统信息失败:', error.message);
    showError('无法获取系统信息');
}
```

### 2. 性能考虑

避免频繁调用系统操作：

```javascript
// 不推荐：频繁获取鼠标位置
setInterval(async () => {
    const pos = await puppet.system.getMousePosition();
    // ...
}, 10);

// 推荐：使用 requestAnimationFrame
function trackMouse() {
    requestAnimationFrame(async () => {
        const pos = await puppet.system.getMousePosition();
        // ...
        trackMouse();
    });
}
```

### 3. 用户确认

自动化操作前获取用户确认：

```javascript
async function safeClick(x, y) {
    const confirmed = confirm(`确定要在 (${x}, ${y}) 位置点击吗？`);
    if (confirmed) {
        await puppet.system.sendMouseClick(x, y, 'left');
    }
}
```

### 4. 权限提示

提示用户可能需要的权限：

```javascript
async function performAutomation() {
    console.log('即将执行自动化操作...');
    console.log('请确保应用有足够的权限');
    
    await performAutomatedAction();
}
```

## 相关资源

- [Windows API](https://learn.mozilla.org/en-US/docs/Web/API)：Windows API 文档
- [键盘事件](https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent)：键盘事件 API
- [鼠标事件](https://developer.mozilla.org/en-US/docs/Web/API/MouseEvent)：鼠标事件 API

## 常见问题

### Q: 模拟按键为什么不起作用？

A: 请确保：
1. 应用有足够的权限
2. 目标窗口处于激活状态
3. 按键名称正确

### Q: 可以模拟拖拽操作吗？

A: 可以通过组合鼠标点击和移动实现拖拽。

### Q: 截图性能如何？

A: 截图性能取决于屏幕分辨率，通常在 100-500ms 之间。

### Q: 获取鼠标位置会影响性能吗？

A: 获取鼠标位置本身性能开销很小，但频繁调用可能影响性能。