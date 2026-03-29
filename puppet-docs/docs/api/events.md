---
title: 事件系统 API
permalink: /api/events.html
createTime: 2026/03/28 15:12:12
---

# 事件系统 API

事件系统 API 提供了设备事件和窗口事件的监听和处理功能。

## 概述

`puppet.events` 命名空间提供以下功能：

- 事件监听器注册
- 事件监听器移除
- 支持多种事件类型

## 支持的事件类型

### USB 设备事件

| 事件名称 | 描述 |
|----------|------|
| `usb-plug-in` | USB 设备插入 |
| `usb-plug-out` | USB 设备拔出 |

### 磁盘事件

| 事件名称 | 描述 |
|----------|------|
| `disk-mount` | 磁盘挂载 |
| `disk-unmount` | 磁盘卸载 |

### 窗口事件

| 事件名称 | 描述 |
|----------|------|
| `window-focus` | 窗口获得焦点 |
| `window-blur` | 窗口失去焦点 |
| `window-maximize` | 窗口最大化 |
| `window-minimize` | 窗口最小化 |
| `window-restore` | 窗口还原 |
| `window-resize` | 窗口大小改变 |
| `window-move` | 窗口移动 |

### 电源事件

| 事件名称 | 描述 |
|----------|------|
| `power-change` | 电源状态变化 |

## 方法

### addEventListener()

注册事件监听器。

```javascript
await puppet.events.addEventListener(eventName: string, callback: (event: Event) => void): Promise<string>
```

**参数**：

- `eventName` (string) - 事件名称
- `callback` (function) - 事件回调函数

**返回值**：

监听器 ID，用于后续移除监听器。

**示例**：

```javascript
// 监听 USB 设备插入
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
    puppet.log.info('USB 设备已插入');
});

// 监听窗口最大化
await puppet.events.addEventListener('window-maximize', () => {
    console.log('窗口已最大化');
});

// 监听电源变化
await puppet.events.addEventListener('power-change', (event) => {
    console.log('电源状态变化:', event.data);
});
```

### removeEventListener()

移除事件监听器。

```javascript
await puppet.events.removeEventListener(eventName: string, listenerId: string): Promise<void>
```

**参数**：

- `eventName` (string) - 事件名称
- `listenerId` (string) - 监听器 ID（由 `addEventListener` 返回）

**示例**：

```javascript
// 注册监听器
const listenerId = await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
});

// 移除监听器
await puppet.events.removeEventListener('usb-plug-in', listenerId);
```

## 事件对象

### USB 设备事件

```typescript
interface USBEvent {
    DeviceId: string;      // 设备 ID
    DeviceName: string;    // 设备名称
    DriveLetter: string;   // 驱动器号（如 'E:'）
    VolumeName: string;    // 卷标名称
    FileSystem: string;    // 文件系统类型
    TotalSize: number;     // 总大小（字节）
    FreeSpace: number;     // 可用空间（字节）
}
```

### 磁盘事件

```typescript
interface DiskEvent {
    DriveLetter: string;   // 驱动器号
    VolumeName: string;    // 卷标名称
    FileSystem: string;    // 文件系统类型
    TotalSize: number;     // 总大小（字节）
    FreeSpace: number;     // 可用空间（字节）
}
```

### 窗口事件

```typescript
interface WindowEvent {
    Width: number;         // 窗口宽度
    Height: number;        // 窗口高度
    X: number;             // 窗口 X 坐标
    Y: number;             // 窗口 Y 坐标
}
```

### 电源事件

```typescript
interface PowerEvent {
    PowerStatus: string;   // 电源状态（'AC', 'Battery'）
    BatteryLevel: number;  // 电池电量（百分比）
    BatteryLife: number;   // 剩余时间（分钟）
}
```

## 使用示例

### USB 设备监控

```javascript
class USBMonitor {
    constructor() {
        this.listenerId = null;
    }
    
    async startMonitoring() {
        // 监听 USB 设备插入
        this.listenerId = await puppet.events.addEventListener('usb-plug-in', async (event) => {
            await this.handleUSBPlugIn(event.data);
        });
        
        console.log('USB 监控已启动');
    }
    
    async handleUSBPlugIn(device) {
        console.log('USB 设备插入:', device);
        
        // 显示通知
        await puppet.tray.showBalloon(
            'USB 设备插入',
            `${device.DeviceName} (${device.DriveLetter})`,
            10000,
            'Info'
        );
        
        // 记录日志
        puppet.log.info(`USB 设备插入: ${device.DeviceName} (${device.DriveLetter})`);
        
        // 执行自定义操作
        await this.processUSBDevice(device);
    }
    
    async processUSBDevice(device) {
        // 示例：复制文件
        const sourcePath = device.DriveLetter + '\\';
        const destPath = 'C:\\Backups\\';
        
        // 复制文件逻辑...
        console.log('处理 USB 设备:', sourcePath);
    }
    
    async stopMonitoring() {
        if (this.listenerId) {
            await puppet.events.removeEventListener('usb-plug-in', this.listenerId);
            this.listenerId = null;
            console.log('USB 监控已停止');
        }
    }
}

// 使用 USB 监控
const usbMonitor = new USBMonitor();
usbMonitor.startMonitoring();

// 停止监控
// usbMonitor.stopMonitoring();
```

### 窗口状态跟踪

```javascript
class WindowTracker {
    constructor() {
        this.listeners = new Map();
    }
    
    async startTracking() {
        // 监听窗口事件
        this.listeners.set('focus', await puppet.events.addEventListener('window-focus', () => {
            console.log('窗口获得焦点');
            this.onFocus();
        }));
        
        this.listeners.set('blur', await puppet.events.addEventListener('window-blur', () => {
            console.log('窗口失去焦点');
            this.onBlur();
        }));
        
        this.listeners.set('maximize', await puppet.events.addEventListener('window-maximize', (event) => {
            console.log('窗口最大化:', event.data);
            this.onMaximize(event.data);
        }));
        
        this.listeners.set('resize', await puppet.events.addEventListener('window-resize', (event) => {
            console.log('窗口大小改变:', event.data);
            this.onResize(event.data);
        }));
        
        console.log('窗口跟踪已启动');
    }
    
    onFocus() {
        // 窗口获得焦点时的处理
        document.body.classList.add('focused');
    }
    
    onBlur() {
        // 窗口失去焦点时的处理
        document.body.classList.remove('focused');
    }
    
    onMaximize(data) {
        // 窗口最大化时的处理
        console.log('窗口大小:', `${data.Width}x${data.Height}`);
    }
    
    onResize(data) {
        // 窗口大小改变时的处理
        console.log('新尺寸:', `${data.Width}x${data.Height}`);
        
        // 自适应布局
        this.adjustLayout(data.Width, data.Height);
    }
    
    adjustLayout(width, height) {
        // 根据窗口大小调整布局
        if (width < 800) {
            document.body.classList.add('small-screen');
        } else {
            document.body.classList.remove('small-screen');
        }
    }
    
    async stopTracking() {
        for (const [event, listenerId] of this.listeners) {
            await puppet.events.removeEventListener(event, listenerId);
        }
        this.listeners.clear();
        console.log('窗口跟踪已停止');
    }
}

// 使用窗口跟踪
const windowTracker = new WindowTracker();
windowTracker.startTracking();
```

### 电源监控

```javascript
class PowerMonitor {
    constructor() {
        this.listenerId = null;
    }
    
    async startMonitoring() {
        // 监听电源变化
        this.listenerId = await puppet.events.addEventListener('power-change', async (event) => {
            await this.handlePowerChange(event.data);
        });
        
        console.log('电源监控已启动');
    }
    
    async handlePowerChange(powerData) {
        console.log('电源状态变化:', powerData);
        
        // 显示通知
        if (powerData.PowerStatus === 'Battery') {
            await puppet.tray.showBalloon(
                '电源切换',
                '已切换到电池供电',
                5000,
                'Info'
            );
        } else {
            await puppet.tray.showBalloon(
                '电源切换',
                '已连接电源适配器',
                5000,
                'Info'
            );
        }
        
        // 低电量警告
        if (powerData.BatteryLevel < 20) {
            await puppet.tray.showBalloon(
                '低电量警告',
                `电池电量: ${powerData.BatteryLevel}%`,
                10000,
                'Warning'
            );
        }
        
        // 记录日志
        puppet.log.info(`电源状态: ${powerData.PowerStatus}, 电量: ${powerData.BatteryLevel}%`);
    }
    
    async stopMonitoring() {
        if (this.listenerId) {
            await puppet.events.removeEventListener('power-change', this.listenerId);
            this.listenerId = null;
            console.log('电源监控已停止');
        }
    }
}

// 使用电源监控
const powerMonitor = new PowerMonitor();
powerMonitor.startMonitoring();
```

### 综合事件处理

```javascript
class EventHandler {
    constructor() {
        this.listeners = new Map();
    }
    
    async initialize() {
        // USB 设备事件
        this.listeners.set('usb-in', await puppet.events.addEventListener('usb-plug-in', (event) => {
            this.onUSBPlugIn(event.data);
        }));
        
        this.listeners.set('usb-out', await puppet.events.addEventListener('usb-plug-out', (event) => {
            this.onUSBPlugOut(event.data);
        }));
        
        // 窗口事件
        this.listeners.set('focus', await puppet.events.addEventListener('window-focus', () => {
            this.onWindowFocus();
        }));
        
        this.listeners.set('blur', await puppet.events.addEventListener('window-blur', () => {
            this.onWindowBlur();
        }));
        
        // 电源事件
        this.listeners.set('power', await puppet.events.addEventListener('power-change', (event) => {
            this.onPowerChange(event.data);
        }));
        
        console.log('事件处理器已初始化');
    }
    
    onUSBPlugIn(device) {
        console.log('USB 设备插入:', device);
        this.showNotification('USB 设备插入', device.DeviceName);
    }
    
    onUSBPlugOut(device) {
        console.log('USB 设备拔出:', device);
        this.showNotification('USB 设备拔出', device.DeviceName);
    }
    
    onWindowFocus() {
        console.log('窗口获得焦点');
        this.updateStatus('激活');
    }
    
    onWindowBlur() {
        console.log('窗口失去焦点');
        this.updateStatus('非激活');
    }
    
    onPowerChange(powerData) {
        console.log('电源状态变化:', powerData);
        this.updateBatteryStatus(powerData.BatteryLevel);
    }
    
    showNotification(title, message) {
        puppet.tray.showBalloon(title, message, 5000, 'Info');
    }
    
    updateStatus(status) {
        document.getElementById('status').textContent = status;
    }
    
    updateBatteryStatus(level) {
        document.getElementById('battery').textContent = `${level}%`;
    }
    
    async cleanup() {
        for (const [name, listenerId] of this.listeners) {
            await puppet.events.removeEventListener(name, listenerId);
        }
        this.listeners.clear();
        console.log('事件处理器已清理');
    }
}

// 使用事件处理器
const eventHandler = new EventHandler();
eventHandler.initialize();

// 清理
// eventHandler.cleanup();
```

## 最佳实践

### 1. 及时清理监听器

移除不需要的监听器以避免内存泄漏：

```javascript
class EventManager {
    constructor() {
        this.listeners = [];
    }
    
    async addListener(eventName, callback) {
        const listenerId = await puppet.events.addEventListener(eventName, callback);
        this.listeners.push({ eventName, listenerId });
        return listenerId;
    }
    
    async removeAllListeners() {
        for (const { eventName, listenerId } of this.listeners) {
            await puppet.events.removeEventListener(eventName, listenerId);
        }
        this.listeners = [];
    }
}

// 使用
const eventManager = new EventManager();
await eventManager.addListener('usb-plug-in', (event) => { /* ... */ });
await eventManager.addListener('window-focus', () => { /* ... */ });

// 清理
await eventManager.removeAllListeners();
```

### 2. 错误处理

在回调中处理可能的错误：

```javascript
await puppet.events.addEventListener('usb-plug-in', async (event) => {
    try {
        await handleUSBDevice(event.data);
    } catch (error) {
        puppet.log.error('处理 USB 设备失败:', error.message);
    }
});
```

### 3. 防抖处理

对频繁触发的事件进行防抖处理：

```javascript
class DebouncedHandler {
    constructor() {
        this.timeout = null;
    }
    
    handle(callback, delay = 300) {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }
        
        this.timeout = setTimeout(() => {
            callback();
            this.timeout = null;
        }, delay);
    }
}

// 使用
const debouncedHandler = new DebouncedHandler();

await puppet.events.addEventListener('window-resize', (event) => {
    debouncedHandler.handle(() => {
        adjustLayout(event.data.Width, event.data.Height);
    });
});
```

### 4. 事件过滤

在回调中过滤不需要的事件：

```javascript
await puppet.events.addEventListener('usb-plug-in', (event) => {
    // 只处理特定类型的设备
    if (event.data.DeviceName.includes('SanDisk')) {
        handleSanDiskDevice(event.data);
    }
});
```

## 相关资源

- [WMI 事件](https://learn.microsoft.com/en-us/windows/win32/wmisdk/receiving-a-wmi-event)：WMI 事件文档
- [JavaScript 事件](https://developer.mozilla.org/en-US/docs/Learn/JavaScript/Building_blocks/Events)：JavaScript 事件指南
- [Windows 设备管理](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk)：Windows 设备管理 API

## 常见问题

### Q: 为什么某些事件没有触发？

A: 请确保：
1. 事件名称正确
2. 设备或系统支持该事件
3. 监听器已正确注册

### Q: 如何同时监听多个事件？

A: 可以多次调用 `addEventListener()`：

```javascript
await puppet.events.addEventListener('usb-plug-in', callback1);
await puppet.events.addEventListener('window-focus', callback2);
```

### Q: 事件回调可以移除自身吗？

A: 可以，在回调中使用返回的 listenerId：

```javascript
const listenerId = await puppet.events.addEventListener('usb-plug-in', async (event) => {
    // 处理事件
    // 移除自身
    await puppet.events.removeEventListener('usb-plug-in', listenerId);
});
```

### Q: 事件触发频率如何？

A: 事件触发频率取决于系统和设备类型，大多数事件在状态变化时立即触发。