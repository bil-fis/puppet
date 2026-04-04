---
title: Device System API
permalink: /en/api/device.html
createTime: 2026/03/28 15:13:21
---

# Device System API

The Device System API provides device query and monitoring capabilities, supporting multiple device types.

## Overview

The `puppet.device` namespace provides the following features:

- Get single device information
- Get device list
- Support multiple device types

## Device Types

| Device Type | Constant | Description |
|-------------|----------|-------------|
| Unknown | `puppet.device.type.unknown` | Unknown device type |
| USB Disk | `puppet.device.type.usbDisk` | USB disk |
| Local Disk | `puppet.device.type.localDisk` | Local disk |
| Removable Disk | `puppet.device.type.removableDisk` | Removable disk |
| Keyboard | `puppet.device.type.keyboard` | Keyboard |
| Mouse | `puppet.device.type.mouse` | Mouse |
| Network Adapter | `puppet.device.type.networkAdapter` | Network adapter |
| Monitor | `puppet.device.type.monitor` | Monitor |
| Audio Device | `puppet.device.type.audio` | Audio device |

## Device Status

| Device Status | Constant | Description |
|---------------|----------|-------------|
| Normal | `puppet.device.status.ok` | Device is normal |
| Error | `puppet.device.status.error` | Device error |
| Degraded | `puppet.device.status.degraded` | Device degraded |
| Ready | `puppet.device.status.ready` | Device ready |
| Unknown | `puppet.device.status.unknown` | Unknown status |

## Methods

### getDevice()

Gets single device information.

```javascript
await puppet.device.getDevice(deviceId: string): Promise<DeviceInfo>
```

**Parameters**:

- `deviceId` (string) - Device ID (such as drive letter 'C:')

**Return Value**:

```typescript
interface DeviceInfo {
    DeviceId: string;        // Device ID
    DeviceType: number;      // Device type
    DeviceName: string;      // Device name
    Status: number;          // Device status
    DriveLetter: string;     // Drive letter (disk device)
    VolumeName: string;      // Volume label name
    FileSystem: string;      // File system type
    TotalSize: number;       // Total size (bytes)
    FreeSpace: number;       // Free space (bytes)
    UsedSpace: number;       // Used space (bytes)
    Manufacturer: string;    // Manufacturer
    Model: string;           // Model
    SerialNumber: string;    // Serial number
}
```

**Example**:

```javascript
// Get C drive information
const device = await puppet.device.getDevice('C:');
console.log('Device name:', device.DeviceName);
console.log('Total size:', formatBytes(device.TotalSize));
console.log('Free space:', formatBytes(device.FreeSpace));

// Get USB drive information
const usbDevice = await puppet.device.getDevice('E:');
console.log('USB device:', usbDevice.DeviceName);
```

### getDevices()

Gets device list.

```javascript
await puppet.device.getDevices(deviceType: number): Promise<DeviceInfo[]>
```

**Parameters**:

- `deviceType` (number) - Device type

**Return Value**:

Array of device information.

**Example**:

```javascript
// Get all USB disks
const usbDevices = await puppet.device.getDevices(puppet.device.type.usbDisk);
console.log('Number of USB disks:', usbDevices.length);
usbDevices.forEach(device => {
    console.log('-', device.DriveLetter, device.VolumeName);
});

// Get all local disks
const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
console.log('Local disks:', localDisks.map(d => d.DriveLetter).join(', '));

// Get all removable disks
const removableDisks = await puppet.device.getDevices(puppet.device.type.removableDisk);
console.log('Removable disks:', removableDisks.map(d => d.DriveLetter).join(', '));
```

## Usage Examples

### Disk Manager

```javascript
class DiskManager {
    async getLocalDisks() {
        const disks = await puppet.device.getDevices(puppet.device.type.localDisk);
        return disks;
    }
    
    async getRemovableDisks() {
        const disks = await puppet.device.getDevices(puppet.device.type.removableDisk);
        return disks;
    }
    
    async getUSBDisks() {
        const disks = await puppet.device.getDevices(puppet.device.type.usbDisk);
        return disks;
    }
    
    formatSize(bytes) {
        const units = ['B', 'KB', 'MB', 'GB', 'TB'];
        let size = bytes;
        let unitIndex = 0;
        
        while (size >= 1024 && unitIndex < units.length - 1) {
            size /= 1024;
            unitIndex++;
        }
        
        return `${size.toFixed(2)} ${units[unitIndex]}`;
    }
    
    getUsagePercentage(device) {
        return ((device.UsedSpace / device.TotalSize) * 100).toFixed(1);
    }
    
    async displayDiskInfo() {
        const localDisks = await this.getLocalDisks();
        
        console.log('Local disk information:');
        for (const disk of localDisks) {
            console.log(`${disk.DriveLetter} - ${disk.VolumeName}`);
            console.log(`  Total size: ${this.formatSize(disk.TotalSize)}`);
            console.log(`  Free space: ${this.formatSize(disk.FreeSpace)}`);
            console.log(`  Usage: ${this.getUsagePercentage(disk)}%`);
            console.log(`  File system: ${disk.FileSystem}`);
        }
    }
}

// Use disk manager
const diskManager = new DiskManager();
diskManager.displayDiskInfo();
```

### USB Device Detection

```javascript
class USBDeviceDetector {
    constructor() {
        this.previousDevices = new Map();
    }
    
    async scanDevices() {
        const devices = await puppet.device.getDevices(puppet.device.type.usbDisk);
        const currentDevices = new Map();
        
        for (const device of devices) {
            currentDevices.set(device.DeviceId, device);
            
            // Detect new device
            if (!this.previousDevices.has(device.DeviceId)) {
                await this.onDevicePluggedIn(device);
            }
        }
        
        // Detect removed device
        for (const [deviceId, device] of this.previousDevices) {
            if (!currentDevices.has(deviceId)) {
                await this.onDevicePluggedOut(device);
            }
        }
        
        this.previousDevices = currentDevices;
    }
    
    async onDevicePluggedIn(device) {
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
        
        // Execute automatic operation
        await this.autoProcess(device);
    }
    
    async onDevicePluggedOut(device) {
        console.log('USB device removed:', device);
        
        // Show notification
        await puppet.tray.showBalloon(
            'USB Device Removed',
            `${device.DeviceName}`,
            5000,
            'Info'
        );
        
        // Log
        puppet.log.info(`USB device removed: ${device.DeviceName}`);
    }
    
    async autoProcess(device) {
        // Example: Automatic backup
        console.log('Auto processing USB device:', device.DriveLetter);
        
        // Backup logic...
    }
    
    startMonitoring(interval = 5000) {
        this.monitoringInterval = setInterval(async () => {
            await this.scanDevices();
        }, interval);
        
        console.log('USB device monitoring started');
    }
    
    stopMonitoring() {
        if (this.monitoringInterval) {
            clearInterval(this.monitoringInterval);
            this.monitoringInterval = null;
            console.log('USB device monitoring stopped');
        }
    }
}

// Use USB device detector
const usbDetector = new USBDeviceDetector();
usbDetector.startMonitoring();

// Stop monitoring
// usbDetector.stopMonitoring();
```

### Device Information Viewer

```javascript
class DeviceViewer {
    constructor() {
        this.displays = [];
    }
    
    async loadDevices() {
        this.displays = [];
        
        // Local disks
        const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
        this.displays.push(...localDisks.map(d => ({
            type: 'Local Disk',
            ...d
        })));
        
        // Removable disks
        const removableDisks = await puppet.device.getDevices(puppet.device.type.removableDisk);
        this.displays.push(...removableDisks.map(d => ({
            type: 'Removable Disk',
            ...d
        })));
        
        // USB disks
        const usbDisks = await puppet.device.getDevices(puppet.device.type.usbDisk);
        this.displays.push(...usbDisks.map(d => ({
            type: 'USB Disk',
            ...d
        })));
    }
    
    getDeviceType(deviceType) {
        const types = {
            [puppet.device.type.unknown]: 'Unknown',
            [puppet.device.type.usbDisk]: 'USB Disk',
            [puppet.device.type.localDisk]: 'Local Disk',
            [puppet.device.type.removableDisk]: 'Removable Disk',
            [puppet.device.type.keyboard]: 'Keyboard',
            [puppet.device.type.mouse]: 'Mouse',
            [puppet.device.type.networkAdapter]: 'Network Adapter',
            [puppet.device.type.monitor]: 'Monitor',
            [puppet.device.type.audio]: 'Audio Device'
        };
        return types[deviceType] || 'Unknown';
    }
    
    getDeviceStatus(status) {
        const statuses = {
            [puppet.device.status.ok]: 'Normal',
            [puppet.device.status.error]: 'Error',
            [puppet.device.status.degraded]: 'Degraded',
            [puppet.device.status.ready]: 'Ready',
            [puppet.device.status.unknown]: 'Unknown'
        };
        return statuses[status] || 'Unknown';
    }
    
    formatSize(bytes) {
        const units = ['B', 'KB', 'MB', 'GB', 'TB'];
        let size = bytes;
        let unitIndex = 0;
        
        while (size >= 1024 && unitIndex < units.length - 1) {
            size /= 1024;
            unitIndex++;
        }
        
        return `${size.toFixed(2)} ${units[unitIndex]}`;
    }
    
    render() {
        const html = `
            <div class="device-viewer">
                <h2>Device List</h2>
                <table class="device-table">
                    <thead>
                        <tr>
                            <th>Type</th>
                            <th>Name</th>
                            <th>Drive</th>
                            <th>Status</th>
                            <th>Total Size</th>
                            <th>Free Space</th>
                            <th>Usage</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${this.displays.map(device => `
                            <tr>
                                <td>${this.getDeviceType(device.DeviceType)}</td>
                                <td>${device.DeviceName || '-'}</td>
                                <td>${device.DriveLetter || '-'}</td>
                                <td>${this.getDeviceStatus(device.Status)}</td>
                                <td>${this.formatSize(device.TotalSize)}</td>
                                <td>${this.formatSize(device.FreeSpace)}</td>
                                <td>${((device.UsedSpace / device.TotalSize) * 100).toFixed(1)}%</td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            </div>
        `;
        
        document.getElementById('device-container').innerHTML = html;
    }
    
    async refresh() {
        await this.loadDevices();
        this.render();
    }
}

// Use device viewer
const deviceViewer = new DeviceViewer();

// Load devices
await deviceViewer.loadDevices();
deviceViewer.render();

// Refresh devices
document.getElementById('refresh-btn').addEventListener('click', async () => {
    await deviceViewer.refresh();
});
```

### Storage Space Analysis

```javascript
class StorageAnalyzer {
    async analyzeStorage() {
        const disks = await puppet.device.getDevices(puppet.device.type.localDisk);
        const analysis = [];
        
        for (const disk of disks) {
            const usage = (disk.UsedSpace / disk.TotalSize) * 100;
            let status = 'Normal';
            
            if (usage > 90) {
                status = 'Critical';
            } else if (usage > 75) {
                status = 'Warning';
            }
            
            analysis.push({
                disk: disk.DriveLetter,
                total: disk.TotalSize,
                used: disk.UsedSpace,
                free: disk.FreeSpace,
                usage: usage,
                status: status
            });
        }
        
        return analysis;
    }
    
    formatBytes(bytes) {
        const units = ['B', 'KB', 'MB', 'GB', 'TB'];
        let size = bytes;
        let unitIndex = 0;
        
        while (size >= 1024 && unitIndex < units.length - 1) {
            size /= 1024;
            unitIndex++;
        }
        
        return `${size.toFixed(2)} ${units[unitIndex]}`;
    }
    
    generateReport() {
        return this.analyzeStorage().then(analysis => {
            const report = {
                timestamp: new Date().toISOString(),
                disks: analysis,
                summary: {
                    total: analysis.reduce((sum, d) => sum + d.total, 0),
                    used: analysis.reduce((sum, d) => sum + d.used, 0),
                    free: analysis.reduce((sum, d) => sum + d.free, 0)
                }
            };
            
            return report;
        });
    }
}

// Use storage analyzer
const storageAnalyzer = new StorageAnalyzer();

// Generate report
const report = await storageAnalyzer.generateReport();
console.log('Storage analysis report:', report);
```

## Best Practices

### 1. Error Handling

Catch errors that may occur during device operations:

```javascript
async function safeGetDevice(deviceId) {
    try {
        const device = await puppet.device.getDevice(deviceId);
        return device;
    } catch (error) {
        puppet.log.error('Failed to get device information:', error.message);
        return null;
    }
}
```

### 2. Performance Optimization

Avoid frequent device queries:

```javascript
class DeviceCache {
    constructor() {
        this.cache = new Map();
        this.cacheTimeout = 60000; // 1 minute
    }
    
    async getDevice(deviceId) {
        const cached = this.cache.get(deviceId);
        
        if (cached && Date.now() - cached.timestamp < this.cacheTimeout) {
            return cached.data;
        }
        
        const device = await puppet.device.getDevice(deviceId);
        this.cache.set(deviceId, {
            timestamp: Date.now(),
            data: device
        });
        
        return device;
    }
    
    clear() {
        this.cache.clear();
    }
}
```

### 3. Async Operations

Handle async operations correctly:

```javascript
async function getAllDisks() {
    const results = await Promise.all([
        puppet.device.getDevices(puppet.device.type.localDisk),
        puppet.device.getDevices(puppet.device.type.removableDisk),
        puppet.device.getDevices(puppet.device.type.usbDisk)
    ]);
    
    return {
        local: results[0],
        removable: results[1],
        usb: results[2]
    };
}
```

### 4. Data Validation

Validate device data:

```javascript
function validateDevice(device) {
    if (!device || !device.DeviceId) {
        throw new Error('Invalid device data');
    }
    
    if (device.TotalSize < 0 || device.FreeSpace < 0) {
        throw new Error('Invalid storage data');
    }
    
    if (device.UsedSpace > device.TotalSize) {
        throw new Error('Inconsistent storage data');
    }
    
    return true;
}
```

## Related Resources

- [Win32_LogicalDisk](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk): Win32 logical disk class
- [Windows Device Management](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes): Windows device management API
- [WMI Device Query](https://learn.microsoft.com/en-us/windows/win32/wmisdk/wmi-start-page): WMI documentation

## Common Questions

### Q: How to get all device types?

A: You can iterate through all device type constants:

```javascript
const types = [
    puppet.device.type.unknown,
    puppet.device.type.usbDisk,
    puppet.device.type.localDisk,
    puppet.device.type.removableDisk
];

for (const type of types) {
    const devices = await puppet.device.getDevices(type);
    console.log(`Type ${type}:`, devices.length, 'devices');
}
```

### Q: What format is the device ID?

A: For disk devices, the device ID is usually the drive letter (such as 'C:', 'E:').

### Q: How to listen for device plug-in/plug-out?

A: Use the event system API:

```javascript
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB device plugged in:', event.data);
});
```

### Q: How is device query performance?

A: Device queries are usually fast (< 100ms), but frequent queries may affect performance, so caching is recommended.