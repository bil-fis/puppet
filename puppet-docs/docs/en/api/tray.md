---
title: Tray Icon API
permalink: /en/api/tray.html
createTime: 2026/03/28 15:09:46
---

# Tray Icon API

The Tray Icon API provides system tray icon management functionality, including icon creation, menu setup, notifications, etc.

## Overview

The `puppet.tray` namespace provides the following features:

- Create and update tray icons
- Set tray menus
- Show balloon notifications
- Click and double-click event handling
- Show and hide tray icons
- Custom tray icons

## Methods

### setTray()

Creates or updates tray icon.

```javascript
await puppet.tray.setTray(name: string): Promise<void>
```

**Parameters**:

- `name` (string) - Tray icon name (tooltip text)

**Example**:

```javascript
// Create tray icon
await puppet.tray.setTray('My App');

// Update tray icon name
await puppet.tray.setTray('My App - Running');
```

### setMenu()

Sets tray menu.

```javascript
await puppet.tray.setMenu(items: MenuItem[]): Promise<void>
```

**Parameters**:

```typescript
interface MenuItem {
    Text: string;      // Menu item text
    Command: string;   // Command identifier
}
```

**Example**:

```javascript
// Set tray menu
await puppet.tray.setMenu([
    { Text: 'Open', Command: 'open' },
    { Text: 'Settings', Command: 'settings' },
    { Text: '-', Command: 'separator' },
    { Text: 'Exit', Command: 'exit' }
]);
```

**Description**:

- `-` as command identifier indicates separator
- Menu item click will trigger corresponding command

### showBalloon()

Shows balloon notification.

```javascript
await puppet.tray.showBalloon(title: string, content: string, timeout: number, icon: string): Promise<void>
```

**Parameters**:

- `title` (string) - Notification title
- `content` (string) - Notification content
- `timeout` (number) - Display duration (milliseconds)
- `icon` (string) - Icon type ('Info', 'Warning', 'Error', 'None')

**Example**:

```javascript
// Show info notification
await puppet.tray.showBalloon(
    'Notification',
    'This is a message',
    30000,
    'Info'
);

// Show warning notification
await puppet.tray.showBalloon(
    'Warning',
    'High memory usage',
    30000,
    'Warning'
);

// Show error notification
await puppet.tray.showBalloon(
    'Error',
    'Operation failed',
    30000,
    'Error'
);
```

### onClick()

Sets click event callback.

```javascript
await puppet.tray.onClick(callback: (command: string) => void): Promise<void>
```

**Parameters**:

- `callback` (function) - Click event callback function

**Example**:

```javascript
// Set click event
await puppet.tray.onClick((command) => {
    console.log('Tray clicked, command:', command);
    
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

Sets double-click event callback.

```javascript
await puppet.tray.onDoubleClick(callback: () => void): Promise<void>
```

**Parameters**:

- `callback` (function) - Double-click event callback function

**Example**:

```javascript
// Set double-click event
await puppet.tray.onDoubleClick(() => {
    console.log('Tray double-clicked');
    showMainWindow();
});
```

### hide()

Hides tray icon.

```javascript
await puppet.tray.hide(): Promise<void>
```

**Example**:

```javascript
// Hide tray icon
await puppet.tray.hide();
```

### show()

Shows tray icon.

```javascript
await puppet.tray.show(): Promise<void>
```

**Example**:

```javascript
// Show tray icon
await puppet.tray.show();
```

### setIcon()

Sets custom tray icon.

```javascript
await puppet.tray.setIcon(iconPath: string): Promise<void>
```

**Parameters**:

- `iconPath` (string) - Icon file path

**Example**:

```javascript
// Set custom icon
await puppet.tray.setIcon('C:\\MyApp\\icon.ico');

// Use relative path
await puppet.tray.setIcon('icon.ico');
```

**Supported Icon Formats**:

- ICO format (recommended)
- PNG format
- JPG format

**Recommended Sizes**:

- 16x16 (small icon)
- 32x32 (standard icon)
- 48x48 (large icon)

## Usage Examples

### Basic Tray Application

```javascript
// Initialize tray
async function initTray() {
    // Create tray icon
    await puppet.tray.setTray('My App');
    
    // Set menu
    await puppet.tray.setMenu([
        { Text: 'Show Window', Command: 'show' },
        { Text: 'Hide Window', Command: 'hide' },
        { Text: '-', Command: 'separator' },
        { Text: 'About', Command: 'about' },
        { Text: 'Exit', Command: 'exit' }
    ]);
    
    // Set click event
    await puppet.tray.onClick((command) => {
        handleMenuClick(command);
    });
    
    // Set double-click event
    await puppet.tray.onDoubleClick(() => {
        showMainWindow();
    });
    
    // Show notification
    await puppet.tray.showBalloon(
        'Welcome',
        'App is running in background',
        5000,
        'Info'
    );
}

// Handle menu click
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

// Show main window
function showMainWindow() {
    // Show window logic
    document.body.style.display = 'block';
}

// Hide main window
function hideMainWindow() {
    // Hide window logic
    document.body.style.display = 'none';
}

// Show about dialog
function showAboutDialog() {
    alert('My App v1.0');
}

// Initialize
window.addEventListener('DOMContentLoaded', initTray);
```

### Notification System

```javascript
class NotificationSystem {
    constructor() {
        this.notifications = [];
    }
    
    async init() {
        await puppet.tray.setTray('Notification System');
        await this.setupMenu();
        await this.setupEvents();
    }
    
    async setupMenu() {
        await puppet.tray.setMenu([
            { Text: 'View Notifications', Command: 'view' },
            { Text: 'Clear Notifications', Command: 'clear' },
            { Text: '-', Command: 'separator' },
            { Text: 'Exit', Command: 'exit' }
        ]);
    }
    
    async setupEvents() {
        await puppet.tray.onClick((command) => {
            this.handleCommand(command);
        });
    }
    
    async notify(title, message, type = 'Info') {
        // Show balloon notification
        await puppet.tray.showBalloon(title, message, 10000, type);
        
        // Save notification
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
            alert('No notifications');
            return;
        }
        
        const message = this.notifications
            .map(n => `[${n.timestamp.toLocaleTimeString()}] ${n.title}: ${n.message}`)
            .join('\n');
        
        alert(message);
    }
    
    clearNotifications() {
        this.notifications = [];
        alert('Notifications cleared');
    }
}

// Use notification system
const notificationSystem = new NotificationSystem();
notificationSystem.init();

// Send notification
notificationSystem.notify('New Message', 'You have 3 new messages');
notificationSystem.notify('Update Available', 'New version released', 'Info');
notificationSystem.notify('Warning', 'Low disk space', 'Warning');
```

### Status Monitoring

```javascript
class SystemMonitor {
    constructor() {
        this.isMonitoring = false;
    }
    
    async init() {
        await puppet.tray.setTray('System Monitor');
        await this.setupMenu();
        await this.setupEvents();
        this.startMonitoring();
    }
    
    async setupMenu() {
        await puppet.tray.setMenu([
            { Text: 'Show Status', Command: 'status' },
            { Text: 'Pause Monitoring', Command: 'pause' },
            { Text: 'Resume Monitoring', Command: 'resume' },
            { Text: '-', Command: 'separator' },
            { Text: 'Exit', Command: 'exit' }
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
                await puppet.tray.setTray('System Monitor (Paused)');
                break;
            case 'resume':
                this.isMonitoring = true;
                await puppet.tray.setTray('System Monitor');
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
        }, 60000); // Check every minute
    }
    
    async checkStatus() {
        const sysInfo = await puppet.system.getSystemInfo();
        const memoryUsage = ((sysInfo.totalMemory - sysInfo.availableMemory) / sysInfo.totalMemory * 100).toFixed(1);
        
        // Warning threshold
        if (parseFloat(memoryUsage) > 80) {
            await puppet.tray.showBalloon(
                'Memory Warning',
                `Memory usage: ${memoryUsage}%`,
                10000,
                'Warning'
            );
        }
    }
    
    async showStatus() {
        const sysInfo = await puppet.system.getSystemInfo();
        const memoryUsage = ((sysInfo.totalMemory - sysInfo.availableMemory) / sysInfo.totalMemory * 100).toFixed(1);
        
        alert(`
System Status:
CPU: ${sysInfo.cpuModel}
Memory Usage: ${memoryUsage}%
Screen: ${sysInfo.screenWidth}x${sysInfo.screenHeight}
        `.trim());
    }
}

// Use system monitor
const monitor = new SystemMonitor();
monitor.init();
```

## Best Practices

### 1. Icon Design

Use appropriate tray icons:

```javascript
// Recommended to use professional icon design
await puppet.tray.setIcon('icon.ico');

// Ensure icon is clearly visible in both dark and light backgrounds
// Use transparent background
// Provide multiple size versions
```

### 2. Notification Management

Use notification functionality appropriately:

```javascript
// Don't send notifications frequently
// Use importance levels to distinguish notification types
// Provide clear notification content
await puppet.tray.showBalloon(
    'Important Notification',
    'Your operation has completed',
    5000,
    'Info'
);
```

### 3. Menu Organization

Organize clear menu structure:

```javascript
// Group related menu items
await puppet.tray.setMenu([
    // Main functions
    { Text: 'Open', Command: 'open' },
    { Text: 'Settings', Command: 'settings' },
    { Text: '-', Command: 'separator' },
    // Auxiliary functions
    { Text: 'Help', Command: 'help' },
    { Text: 'About', Command: 'about' },
    { Text: '-', Command: 'separator' },
    // System functions
    { Text: 'Exit', Command: 'exit' }
]);
```

### 4. Status Update

Update tray status promptly:

```javascript
// Update tray based on app status
async function updateTrayStatus(status) {
    await puppet.tray.setTray(`My App - ${status}`);
}

// Usage example
updateTrayStatus('Running');
updateTrayStatus('Syncing...');
updateTrayStatus('Paused');
```

## Related Resources

- [Windows Tray Icon](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/notifyicon-component-overview-windows-forms): NotifyIcon component documentation
- [Icon Design Guide](https://docs.microsoft.com/en-us/windows/apps/design/style/iconography/iconography-basics): Windows icon design guide

## Common Questions

### Q: What to do if tray icon doesn't show?

A: Please ensure:
1. `setTray()` method has been called
2. Icon file path is correct
3. Icon file format is correct

### Q: How to hide main window but keep tray icon?

A: Use window control API to hide window:

```javascript
await puppet.window.showInTaskbar(false);
document.body.style.display = 'none';
```

### Q: Can balloon notification display time be adjusted?

A: Yes, adjust through `showBalloon()`'s `timeout` parameter.

### Q: Can icons be used in menus?

A: Current version doesn't support menu icons, only text menus.