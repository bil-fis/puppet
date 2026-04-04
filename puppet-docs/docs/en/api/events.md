---
title: Event System API
permalink: /en/api/events.html
createTime: 2026/03/28 15:12:12
---

# Event System API

The Event System API provides monitoring and handling capabilities for device events and window events.

## Overview

The `puppet.events` namespace provides the following features:

- Event listener registration
- Event listener removal
- Support for multiple event types

## Supported Event Types

### USB Device Events

| Event Name | Description |
|------------|-------------|
| `usb-plug-in` | USB device plugged in |
| `usb-plug-out` | USB device unplugged |

### Disk Events

| Event Name | Description |
|------------|-------------|
| `disk-mount` | Disk mounted |
| `disk-unmount` | Disk unmounted |

### Window Events

| Event Name | Description |
|------------|-------------|
| `window-focus` | Window gained focus |
| `window-blur` | Window lost focus |
| `window-maximize` | Window maximized |
| `window-minimize` | Window minimized |
| `window-restore` | Window restored |
| `window-resize` | Window resized |
| `window-move` | Window moved |

### Power Events

| Event Name | Description |
|------------|-------------|
| `power-change` | Power status changed |

## Methods

### addEventListener()

Registers event listener.

```javascript
await puppet.events.addEventListener(eventName: string, callback: (event: Event) => void): Promise<string>
```

**Parameters**:

- `eventName` (string) - Event name
- `callback` (function) - Event callback function

**Return Value**:

Listener ID, used to remove the listener later.

**Example**:

```javascript
// Monitor USB device plug-in
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB device plugged in:', event.data);
    puppet.log.info('USB device plugged in');
});

// Monitor window maximize
await puppet.events.addEventListener('window-maximize', () => {
    console.log('Window maximized');
});

// Monitor power change
await puppet.events.addEventListener('power-change', (event) => {
    console.log('Power status changed:', event.data);
});
```

### removeEventListener()

Removes event listener.

```javascript
await puppet.events.removeEventListener(eventName: string, listenerId: string): Promise<void>
```

**Parameters**:

- `eventName` (string) - Event name
- `listenerId` (string) - Listener ID (returned by `addEventListener`)

**Example**:

```javascript
// Register listener
const listenerId = await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB device plugged in:', event.data);
});

// Remove listener
await puppet.events.removeEventListener('usb-plug-in', listenerId);
```

## Event Objects

### USB Device Event

```typescript
interface USBEvent {
    DeviceId: string;      // Device ID
    DeviceName: string;    // Device name
    DriveLetter: string;   // Drive letter (e.g., 'E:')
    VolumeName: string;    // Volume label name
    FileSystem: string;    // File system type
    TotalSize: number;     // Total size (bytes)
    FreeSpace: number;     // Free space (bytes)
}
```

### Disk Event

```typescript
interface DiskEvent {
    DriveLetter: string;   // Drive letter
    VolumeName: string;    // Volume label name
    FileSystem: string;    // File system type
    TotalSize: number;     // Total size (bytes)
    FreeSpace: number;     // Free space (bytes)
}
```

### Window Event

```typescript
interface WindowEvent {
    Width: number;         // Window width
    Height: number;        // Window height
    X: number;             // Window X coordinate
    Y: number;             // Window Y coordinate
}
```

### Power Event

```typescript
interface PowerEvent {
    PowerStatus: string;   // Power status ('AC', 'Battery')
    BatteryLevel: number;  // Battery level (percentage)
    BatteryLife: number;   // Remaining time (minutes)
}
```

## Usage Examples

### USB Device Monitoring

```javascript
class USBMonitor {
    constructor() {
        this.listenerId = null;
    }
    
    async startMonitoring() {
        // Monitor USB device plug-in
        this.listenerId = await puppet.events.addEventListener('usb-plug-in', async (event) => {
            await this.handleUSBPlugIn(event.data);
        });
        
        console.log('USB monitoring started');
    }
    
    async handleUSBPlugIn(device) {
        console.log('USB device plugged in:', device);
        
        // Show notification
        await puppet.tray.showBalloon(
            'USB Device Plugged In',
            `${device.DeviceName} (${device.DriveLetter})`,
            10000,
            'Info'
        );
        
        // Log
        puppet.log.info(`USB device plugged in: ${device.DeviceName} (${device.DriveLetter})`);
        
        // Execute custom operation
        await this.processUSBDevice(device);
    }
    
    async processUSBDevice(device) {
        // Example: Copy files
        const sourcePath = device.DriveLetter + '\\';
        const destPath = 'C:\\Backups\\';
        
        // Copy file logic...
        console.log('Processing USB device:', sourcePath);
    }
    
    async stopMonitoring() {
        if (this.listenerId) {
            await puppet.events.removeEventListener('usb-plug-in', this.listenerId);
            this.listenerId = null;
            console.log('USB monitoring stopped');
        }
    }
}

// Use USB monitor
const usbMonitor = new USBMonitor();
usbMonitor.startMonitoring();

// Stop monitoring
// usbMonitor.stopMonitoring();
```

### Window State Tracking

```javascript
class WindowTracker {
    constructor() {
        this.listeners = new Map();
    }
    
    async startTracking() {
        // Monitor window events
        this.listeners.set('focus', await puppet.events.addEventListener('window-focus', () => {
            console.log('Window gained focus');
            this.onFocus();
        }));
        
        this.listeners.set('blur', await puppet.events.addEventListener('window-blur', () => {
            console.log('Window lost focus');
            this.onBlur();
        }));
        
        this.listeners.set('maximize', await puppet.events.addEventListener('window-maximize', (event) => {
            console.log('Window maximized:', event.data);
            this.onMaximize(event.data);
        }));
        
        this.listeners.set('resize', await puppet.events.addEventListener('window-resize', (event) => {
            console.log('Window resized:', event.data);
            this.onResize(event.data);
        }));
        
        console.log('Window tracking started');
    }
    
    onFocus() {
        // Handle when window gains focus
        document.body.classList.add('focused');
    }
    
    onBlur() {
        // Handle when window loses focus
        document.body.classList.remove('focused');
    }
    
    onMaximize(data) {
        // Handle when window is maximized
        console.log('Window size:', `${data.Width}x${data.Height}`);
    }
    
    onResize(data) {
        // Handle when window size changes
        console.log('New size:', `${data.Width}x${data.Height}`);
        
        // Adaptive layout
        this.adjustLayout(data.Width, data.Height);
    }
    
    adjustLayout(width, height) {
        // Adjust layout based on window size
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
        console.log('Window tracking stopped');
    }
}

// Use window tracker
const windowTracker = new WindowTracker();
windowTracker.startTracking();
```

### Power Monitoring

```javascript
class PowerMonitor {
    constructor() {
        this.listenerId = null;
    }
    
    async startMonitoring() {
        // Monitor power change
        this.listenerId = await puppet.events.addEventListener('power-change', async (event) => {
            await this.handlePowerChange(event.data);
        });
        
        console.log('Power monitoring started');
    }
    
    async handlePowerChange(powerData) {
        console.log('Power status changed:', powerData);
        
        // Show notification
        if (powerData.PowerStatus === 'Battery') {
            await puppet.tray.showBalloon(
                'Power Switch',
                'Switched to battery power',
                5000,
                'Info'
            );
        } else {
            await puppet.tray.showBalloon(
                'Power Switch',
                'Power adapter connected',
                5000,
                'Info'
            );
        }
        
        // Low battery warning
        if (powerData.BatteryLevel < 20) {
            await puppet.tray.showBalloon(
                'Low Battery Warning',
                `Battery level: ${powerData.BatteryLevel}%`,
                10000,
                'Warning'
            );
        }
        
        // Log
        puppet.log.info(`Power status: ${powerData.PowerStatus}, Battery: ${powerData.BatteryLevel}%`);
    }
    
    async stopMonitoring() {
        if (this.listenerId) {
            await puppet.events.removeEventListener('power-change', this.listenerId);
            this.listenerId = null;
            console.log('Power monitoring stopped');
        }
    }
}

// Use power monitor
const powerMonitor = new PowerMonitor();
powerMonitor.startMonitoring();
```

### Comprehensive Event Handling

```javascript
class EventHandler {
    constructor() {
        this.listeners = new Map();
    }
    
    async initialize() {
        // USB device events
        this.listeners.set('usb-in', await puppet.events.addEventListener('usb-plug-in', (event) => {
            this.onUSBPlugIn(event.data);
        }));
        
        this.listeners.set('usb-out', await puppet.events.addEventListener('usb-plug-out', (event) => {
            this.onUSBPlugOut(event.data);
        }));
        
        // Window events
        this.listeners.set('focus', await puppet.events.addEventListener('window-focus', () => {
            this.onWindowFocus();
        }));
        
        this.listeners.set('blur', await puppet.events.addEventListener('window-blur', () => {
            this.onWindowBlur();
        }));
        
        // Power events
        this.listeners.set('power', await puppet.events.addEventListener('power-change', (event) => {
            this.onPowerChange(event.data);
        }));
        
        console.log('Event handler initialized');
    }
    
    onUSBPlugIn(device) {
        console.log('USB device plugged in:', device);
        this.showNotification('USB Device Plugged In', device.DeviceName);
    }
    
    onUSBPlugOut(device) {
        console.log('USB device unplugged:', device);
        this.showNotification('USB Device Unplugged', device.DeviceName);
    }
    
    onWindowFocus() {
        console.log('Window gained focus');
        this.updateStatus('Active');
    }
    
    onWindowBlur() {
        console.log('Window lost focus');
        this.updateStatus('Inactive');
    }
    
    onPowerChange(powerData) {
        console.log('Power status changed:', powerData);
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
        console.log('Event handler cleaned up');
    }
}

// Use event handler
const eventHandler = new EventHandler();
eventHandler.initialize();

// Cleanup
// eventHandler.cleanup();
```

## Best Practices

### 1. Clean Up Listeners Timely

Remove unnecessary listeners to avoid memory leaks:

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

// Use
const eventManager = new EventManager();
await eventManager.addListener('usb-plug-in', (event) => { /* ... */ });
await eventManager.addListener('window-focus', () => { /* ... */ });

// Cleanup
await eventManager.removeAllListeners();
```

### 2. Error Handling

Handle possible errors in callbacks:

```javascript
await puppet.events.addEventListener('usb-plug-in', async (event) => {
    try {
        await handleUSBDevice(event.data);
    } catch (error) {
        puppet.log.error('Failed to handle USB device:', error.message);
    }
});
```

### 3. Debounce Handling

Debounce frequently triggered events:

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

// Use
const debouncedHandler = new DebouncedHandler();

await puppet.events.addEventListener('window-resize', (event) => {
    debouncedHandler.handle(() => {
        adjustLayout(event.data.Width, event.data.Height);
    });
});
```

### 4. Event Filtering

Filter unwanted events in callbacks:

```javascript
await puppet.events.addEventListener('usb-plug-in', (event) => {
    // Only handle specific types of devices
    if (event.data.DeviceName.includes('SanDisk')) {
        handleSanDiskDevice(event.data);
    }
});
```

## Related Resources

- [WMI Events](https://learn.microsoft.com/en-us/windows/win32/wmisdk/receiving-a-wmi-event): WMI event documentation
- [JavaScript Events](https://developer.mozilla.org/en-US/docs/Learn/JavaScript/Building_blocks/Events): JavaScript event guide
- [Windows Device Management](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk): Windows device management API

## Common Questions

### Q: Why are some events not triggering?

A: Please ensure:
1. Event name is correct
2. Device or system supports the event
3. Listener is properly registered

### Q: How to listen to multiple events at the same time?

A: You can call `addEventListener()` multiple times:

```javascript
await puppet.events.addEventListener('usb-plug-in', callback1);
await puppet.events.addEventListener('window-focus', callback2);
```

### Q: Can event callback remove itself?

A: Yes, use the returned listenerId in the callback:

```javascript
const listenerId = await puppet.events.addEventListener('usb-plug-in', async (event) => {
    // Handle event
    // Remove itself
    await puppet.events.removeEventListener('usb-plug-in', listenerId);
});
```

### Q: How frequent are event triggers?

A: Event trigger frequency depends on system and device type, most events trigger immediately when state changes.