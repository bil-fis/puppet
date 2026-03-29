---
title: 托盘图标 API
permalink: /api/tray.html
createTime: 2026/03/28 15:09:46
---

# 托盘图标 API

托盘图标 API 提供了系统托盘图标管理功能，包括图标创建、菜单设置、通知等。

## 概述

`puppet.tray` 命名空间提供以下功能：

- 创建和更新托盘图标
- 设置托盘菜单
- 显示气泡通知
- 单击和双击事件处理
- 显示和隐藏托盘图标
- 自定义托盘图标

## 方法

### setTray()

创建或更新托盘图标。

```javascript
await puppet.tray.setTray(name: string): Promise<void>
```

**参数**：

- `name` (string) - 托盘图标名称（工具提示文本）

**示例**：

```javascript
// 创建托盘图标
await puppet.tray.setTray('我的应用');

// 更新托盘图标名称
await puppet.tray.setTray('我的应用 - 运行中');
```

### setMenu()

设置托盘菜单。

```javascript
await puppet.tray.setMenu(items: MenuItem[]): Promise<void>
```

**参数**：

```typescript
interface MenuItem {
    Text: string;      // 菜单项文本
    Command: string;   // 命令标识
}
```

**示例**：

```javascript
// 设置托盘菜单
await puppet.tray.setMenu([
    { Text: '打开', Command: 'open' },
    { Text: '设置', Command: 'settings' },
    { Text: '-', Command: 'separator' },
    { Text: '退出', Command: 'exit' }
]);
```

**说明**：

- `-` 作为命令标识表示分隔线
- 菜单项点击后会触发相应的命令

### showBalloon()

显示气泡通知。

```javascript
await puppet.tray.showBalloon(title: string, content: string, timeout: number, icon: string): Promise<void>
```

**参数**：

- `title` (string) - 通知标题
- `content` (string) - 通知内容
- `timeout` (number) - 显示时长（毫秒）
- `icon` (string) - 图标类型（'Info', 'Warning', 'Error', 'None'）

**示例**：

```javascript
// 显示信息通知
await puppet.tray.showBalloon(
    '通知',
    '这是一条消息',
    30000,
    'Info'
);

// 显示警告通知
await puppet.tray.showBalloon(
    '警告',
    '内存使用率过高',
    30000,
    'Warning'
);

// 显示错误通知
await puppet.tray.showBalloon(
    '错误',
    '操作失败',
    30000,
    'Error'
);
```

### onClick()

设置单击事件回调。

```javascript
await puppet.tray.onClick(callback: (command: string) => void): Promise<void>
```

**参数**：

- `callback` (function) - 单击事件回调函数

**示例**：

```javascript
// 设置单击事件
await puppet.tray.onClick((command) => {
    console.log('托盘被点击，命令:', command);
    
    switch (command) {
        case 'open':
            showMainWindow();
            break;
        case 'settings':
            openSettings();
            break;
        case 'exit':
            puppet.application.close();
            break;
    }
});
```

### onDoubleClick()

设置双击事件回调。

```javascript
await puppet.tray.onDoubleClick(callback: () => void): Promise<void>
```

**参数**：

- `callback` (function) - 双击事件回调函数

**示例**：

```javascript
// 设置双击事件
await puppet.tray.onDoubleClick(() => {
    console.log('托盘被双击');
    showMainWindow();
});
```

### hide()

隐藏托盘图标。

```javascript
await puppet.tray.hide(): Promise<void>
```

**示例**：

```javascript
// 隐藏托盘图标
await puppet.tray.hide();
```

### show()

显示托盘图标。

```javascript
await puppet.tray.show(): Promise<void>
```

**示例**：

```javascript
// 显示托盘图标
await puppet.tray.show();
```

### setIcon()

设置自定义托盘图标。

```javascript
await puppet.tray.setIcon(iconPath: string): Promise<void>
```

**参数**：

- `iconPath` (string) - 图标文件路径

**示例**：

```javascript
// 设置自定义图标
await puppet.tray.setIcon('C:\\MyApp\\icon.ico');

// 使用相对路径
await puppet.tray.setIcon('icon.ico');
```

**支持的图标格式**：

- ICO 格式（推荐）
- PNG 格式
- JPG 格式

**推荐尺寸**：

- 16x16（小图标）
- 32x32（标准图标）
- 48x48（大图标）

## 使用示例

### 基础托盘应用

```javascript
// 初始化托盘
async function initTray() {
    // 创建托盘图标
    await puppet.tray.setTray('我的应用');
    
    // 设置菜单
    await puppet.tray.setMenu([
        { Text: '显示窗口', Command: 'show' },
        { Text: '隐藏窗口', Command: 'hide' },
        { Text: '-', Command: 'separator' },
        { Text: '关于', Command: 'about' },
        { Text: '退出', Command: 'exit' }
    ]);
    
    // 设置单击事件
    await puppet.tray.onClick((command) => {
        handleMenuClick(command);
    });
    
    // 设置双击事件
    await puppet.tray.onDoubleClick(() => {
        showMainWindow();
    });
    
    // 显示通知
    await puppet.tray.showBalloon(
        '欢迎',
        '应用已在后台运行',
        5000,
        'Info'
    );
}

// 处理菜单点击
function handleMenuClick(command) {
    switch (command) {
        case 'show':
            showMainWindow();
            break;
        case 'hide':
            hideMainWindow();
            break;
        case 'about':
            showAboutDialog();
            break;
        case 'exit':
            puppet.application.close();
            break;
    }
}

// 显示主窗口
function showMainWindow() {
    // 显示窗口逻辑
    document.body.style.display = 'block';
}

// 隐藏主窗口
function hideMainWindow() {
    // 隐藏窗口逻辑
    document.body.style.display = 'none';
}

// 显示关于对话框
function showAboutDialog() {
    alert('我的应用 v1.0');
}

// 初始化
window.addEventListener('DOMContentLoaded', initTray);
```

### 通知系统

```javascript
class NotificationSystem {
    constructor() {
        this.notifications = [];
    }
    
    async init() {
        await puppet.tray.setTray('通知系统');
        await this.setupMenu();
        await this.setupEvents();
    }
    
    async setupMenu() {
        await puppet.tray.setMenu([
            { Text: '查看通知', Command: 'view' },
            { Text: '清空通知', Command: 'clear' },
            { Text: '-', Command: 'separator' },
            { Text: '退出', Command: 'exit' }
        ]);
    }
    
    async setupEvents() {
        await puppet.tray.onClick((command) => {
            this.handleCommand(command);
        });
    }
    
    async notify(title, message, type = 'Info') {
        // 显示气泡通知
        await puppet.tray.showBalloon(title, message, 10000, type);
        
        // 保存通知
        this.notifications.push({
            title,
            message,
            type,
            timestamp: new Date()
        });
    }
    
    handleCommand(command) {
        switch (command) {
            case 'view':
                this.viewNotifications();
                break;
            case 'clear':
                this.clearNotifications();
                break;
            case 'exit':
                puppet.application.close();
                break;
        }
    }
    
    viewNotifications() {
        if (this.notifications.length === 0) {
            alert('没有通知');
            return;
        }
        
        const message = this.notifications
            .map(n => `[${n.timestamp.toLocaleTimeString()}] ${n.title}: ${n.message}`)
            .join('\n');
        
        alert(message);
    }
    
    clearNotifications() {
        this.notifications = [];
        alert('通知已清空');
    }
}

// 使用通知系统
const notificationSystem = new NotificationSystem();
notificationSystem.init();

// 发送通知
notificationSystem.notify('新消息', '您有 3 条新消息');
notificationSystem.notify('更新可用', '新版本已发布', 'Info');
notificationSystem.notify('警告', '磁盘空间不足', 'Warning');
```

### 状态监控

```javascript
class SystemMonitor {
    constructor() {
        this.isMonitoring = false;
    }
    
    async init() {
        await puppet.tray.setTray('系统监控');
        await this.setupMenu();
        await this.setupEvents();
        this.startMonitoring();
    }
    
    async setupMenu() {
        await puppet.tray.setMenu([
            { Text: '显示状态', Command: 'status' },
            { Text: '暂停监控', Command: 'pause' },
            { Text: '恢复监控', Command: 'resume' },
            { Text: '-', Command: 'separator' },
            { Text: '退出', Command: 'exit' }
        ]);
    }
    
    async setupEvents() {
        await puppet.tray.onClick((command) => {
            this.handleCommand(command);
        });
    }
    
    async handleCommand(command) {
        switch (command) {
            case 'status':
                await this.showStatus();
                break;
            case 'pause':
                this.isMonitoring = false;
                await puppet.tray.setTray('系统监控 (已暂停)');
                break;
            case 'resume':
                this.isMonitoring = true;
                await puppet.tray.setTray('系统监控');
                break;
            case 'exit':
                puppet.application.close();
                break;
        }
    }
    
    startMonitoring() {
        this.isMonitoring = true;
        this.checkInterval = setInterval(async () => {
            if (this.isMonitoring) {
                await this.checkStatus();
            }
        }, 60000); // 每分钟检查一次
    }
    
    async checkStatus() {
        const sysInfo = await puppet.system.getSystemInfo();
        const memoryUsage = ((sysInfo.totalMemory - sysInfo.availableMemory) / sysInfo.totalMemory * 100).toFixed(1);
        
        // 警告阈值
        if (parseFloat(memoryUsage) > 80) {
            await puppet.tray.showBalloon(
                '内存警告',
                `内存使用率: ${memoryUsage}%`,
                10000,
                'Warning'
            );
        }
    }
    
    async showStatus() {
        const sysInfo = await puppet.system.getSystemInfo();
        const memoryUsage = ((sysInfo.totalMemory - sysInfo.availableMemory) / sysInfo.totalMemory * 100).toFixed(1);
        
        alert(`
系统状态：
CPU: ${sysInfo.cpuModel}
内存使用率: ${memoryUsage}%
屏幕: ${sysInfo.screenWidth}x${sysInfo.screenHeight}
        `.trim());
    }
}

// 使用系统监控
const monitor = new SystemMonitor();
monitor.init();
```

## 最佳实践

### 1. 图标设计

使用合适的托盘图标：

```javascript
// 推荐使用专业的图标设计
await puppet.tray.setIcon('icon.ico');

// 确保图标在深色和浅色背景下都清晰可见
// 使用透明背景
// 提供多个尺寸版本
```

### 2. 通知管理

合理使用通知功能：

```javascript
// 不要频繁发送通知
// 使用重要级别区分通知类型
// 提供清晰的通知内容
await puppet.tray.showBalloon(
    '重要通知',
    '您的操作已完成',
    5000,
    'Info'
);
```

### 3. 菜单组织

组织清晰的菜单结构：

```javascript
// 分组相关菜单项
await puppet.tray.setMenu([
    // 主要功能
    { Text: '打开', Command: 'open' },
    { Text: '设置', Command: 'settings' },
    { Text: '-', Command: 'separator' },
    // 辅助功能
    { Text: '帮助', Command: 'help' },
    { Text: '关于', Command: 'about' },
    { Text: '-', Command: 'separator' },
    // 系统功能
    { Text: '退出', Command: 'exit' }
]);
```

### 4. 状态更新

及时更新托盘状态：

```javascript
// 根据应用状态更新托盘
async function updateTrayStatus(status) {
    await puppet.tray.setTray(`我的应用 - ${status}`);
}

// 使用示例
updateTrayStatus('运行中');
updateTrayStatus('同步中...');
updateTrayStatus('已暂停');
```

## 相关资源

- [Windows 托盘图标](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/notifyicon-component-overview-windows-forms)：NotifyIcon 组件文档
- [图标设计指南](https://docs.microsoft.com/en-us/windows/apps/design/style/iconography/iconography-basics)：Windows 图标设计指南

## 常见问题

### Q: 托盘图标不显示怎么办？

A: 请确保：
1. 已调用 `setTray()` 方法
2. 图标文件路径正确
3. 图标文件格式正确

### Q: 如何隐藏主窗口但保持托盘图标？

A: 使用窗口控制 API 隐藏窗口：

```javascript
await puppet.window.showInTaskbar(false);
document.body.style.display = 'none';
```

### Q: 气泡通知显示时间可以调整吗？

A: 可以，通过 `showBalloon()` 的 `timeout` 参数调整。

### Q: 可以在菜单中使用图标吗？

A: 当前版本不支持菜单图标，只支持文本菜单。