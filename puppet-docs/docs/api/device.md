---
title: 设备系统 API
permalink: /api/device.html
createTime: 2026/03/28 15:13:21
---

# 设备系统 API

设备系统 API 提供了设备查询和监控功能，支持多种设备类型。

## 概述

`puppet.device` 命名空间提供以下功能：

- 获取单个设备信息
- 获取设备列表
- 支持多种设备类型

## 设备类型

| 设备类型 | 常量 | 说明 |
|----------|------|------|
| 未知 | `puppet.device.type.unknown` | 未知设备类型 |
| USB 磁盘 | `puppet.device.type.usbDisk` | USB 磁盘 |
| 本地磁盘 | `puppet.device.type.localDisk` | 本地磁盘 |
| 可移动磁盘 | `puppet.device.type.removableDisk` | 可移动磁盘 |
| 键盘 | `puppet.device.type.keyboard` | 键盘 |
| 鼠标 | `puppet.device.type.mouse` | 鼠标 |
| 网络适配器 | `puppet.device.type.networkAdapter` | 网络适配器 |
| 显示器 | `puppet.device.type.monitor` | 显示器 |
| 音频设备 | `puppet.device.type.audio` | 音频设备 |

## 设备状态

| 设备状态 | 常量 | 说明 |
|----------|------|------|
| 正常 | `puppet.device.status.ok` | 设备正常 |
| 错误 | `puppet.device.status.error` | 设备错误 |
| 降级 | `puppet.device.status.degraded` | 设备降级 |
| 就绪 | `puppet.device.status.ready` | 设备就绪 |
| 未知 | `puppet.device.status.unknown` | 未知状态 |

## 方法

### getDevice()

获取单个设备信息。

```javascript
await puppet.device.getDevice(deviceId: string): Promise<DeviceInfo>
```

**参数**：

- `deviceId` (string) - 设备 ID（如驱动器号 'C:'）

**返回值**：

```typescript
interface DeviceInfo {
    DeviceId: string;        // 设备 ID
    DeviceType: number;      // 设备类型
    DeviceName: string;      // 设备名称
    Status: number;          // 设备状态
    DriveLetter: string;     // 驱动器号（磁盘设备）
    VolumeName: string;      // 卷标名称
    FileSystem: string;      // 文件系统类型
    TotalSize: number;       // 总大小（字节）
    FreeSpace: number;       // 可用空间（字节）
    UsedSpace: number;       // 已用空间（字节）
    Manufacturer: string;    // 制造商
    Model: string;           // 型号
    SerialNumber: string;    // 序列号
}
```

**示例**：

```javascript
// 获取 C 盘信息
const device = await puppet.device.getDevice('C:');
console.log('设备名称:', device.DeviceName);
console.log('总大小:', formatBytes(device.TotalSize));
console.log('可用空间:', formatBytes(device.FreeSpace));

// 获取 U 盘信息
const usbDevice = await puppet.device.getDevice('E:');
console.log('USB 设备:', usbDevice.DeviceName);
```

### getDevices()

获取设备列表。

```javascript
await puppet.device.getDevices(deviceType: number): Promise<DeviceInfo[]>
```

**参数**：

- `deviceType` (number) - 设备类型

**返回值**：

设备信息数组。

**示例**：

```javascript
// 获取所有 USB 磁盘
const usbDevices = await puppet.device.getDevices(puppet.device.type.usbDisk);
console.log('USB 磁盘数量:', usbDevices.length);
usbDevices.forEach(device => {
    console.log('-', device.DriveLetter, device.VolumeName);
});

// 获取所有本地磁盘
const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
console.log('本地磁盘:', localDisks.map(d => d.DriveLetter).join(', '));

// 获取所有可移动磁盘
const removableDisks = await puppet.device.getDevices(puppet.device.type.removableDisk);
console.log('可移动磁盘:', removableDisks.map(d => d.DriveLetter).join(', '));
```

## 使用示例

### 磁盘管理器

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
        
        console.log('本地磁盘信息:');
        for (const disk of localDisks) {
            console.log(`${disk.DriveLetter} - ${disk.VolumeName}`);
            console.log(`  总大小: ${this.formatSize(disk.TotalSize)}`);
            console.log(`  可用空间: ${this.formatSize(disk.FreeSpace)}`);
            console.log(`  使用率: ${this.getUsagePercentage(disk)}%`);
            console.log(`  文件系统: ${disk.FileSystem}`);
        }
    }
}

// 使用磁盘管理器
const diskManager = new DiskManager();
diskManager.displayDiskInfo();
```

### USB 设备检测

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
            
            // 检测新设备
            if (!this.previousDevices.has(device.DeviceId)) {
                await this.onDevicePluggedIn(device);
            }
        }
        
        // 检测移除的设备
        for (const [deviceId, device] of this.previousDevices) {
            if (!currentDevices.has(deviceId)) {
                await this.onDevicePluggedOut(device);
            }
        }
        
        this.previousDevices = currentDevices;
    }
    
    async onDevicePluggedIn(device) {
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
        
        // 执行自动操作
        await this.autoProcess(device);
    }
    
    async onDevicePluggedOut(device) {
        console.log('USB 设备移除:', device);
        
        // 显示通知
        await puppet.tray.showBalloon(
            'USB 设备移除',
            `${device.DeviceName}`,
            5000,
            'Info'
        );
        
        // 记录日志
        puppet.log.info(`USB 设备移除: ${device.DeviceName}`);
    }
    
    async autoProcess(device) {
        // 示例：自动备份
        console.log('自动处理 USB 设备:', device.DriveLetter);
        
        // 备份逻辑...
    }
    
    startMonitoring(interval = 5000) {
        this.monitoringInterval = setInterval(async () => {
            await this.scanDevices();
        }, interval);
        
        console.log('USB 设备监控已启动');
    }
    
    stopMonitoring() {
        if (this.monitoringInterval) {
            clearInterval(this.monitoringInterval);
            this.monitoringInterval = null;
            console.log('USB 设备监控已停止');
        }
    }
}

// 使用 USB 设备检测器
const usbDetector = new USBDeviceDetector();
usbDetector.startMonitoring();

// 停止监控
// usbDetector.stopMonitoring();
```

### 设备信息查看器

```javascript
class DeviceViewer {
    constructor() {
        this.displays = [];
    }
    
    async loadDevices() {
        this.displays = [];
        
        // 本地磁盘
        const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
        this.displays.push(...localDisks.map(d => ({
            type: '本地磁盘',
            ...d
        })));
        
        // 可移动磁盘
        const removableDisks = await puppet.device.getDevices(puppet.device.type.removableDisk);
        this.displays.push(...removableDisks.map(d => ({
            type: '可移动磁盘',
            ...d
        })));
        
        // USB 磁盘
        const usbDisks = await puppet.device.getDevices(puppet.device.type.usbDisk);
        this.displays.push(...usbDisks.map(d => ({
            type: 'USB 磁盘',
            ...d
        })));
    }
    
    getDeviceType(deviceType) {
        const types = {
            [puppet.device.type.unknown]: '未知',
            [puppet.device.type.usbDisk]: 'USB 磁盘',
            [puppet.device.type.localDisk]: '本地磁盘',
            [puppet.device.type.removableDisk]: '可移动磁盘',
            [puppet.device.type.keyboard]: '键盘',
            [puppet.device.type.mouse]: '鼠标',
            [puppet.device.type.networkAdapter]: '网络适配器',
            [puppet.device.type.monitor]: '显示器',
            [puppet.device.type.audio]: '音频设备'
        };
        return types[deviceType] || '未知';
    }
    
    getDeviceStatus(status) {
        const statuses = {
            [puppet.device.status.ok]: '正常',
            [puppet.device.status.error]: '错误',
            [puppet.device.status.degraded]: '降级',
            [puppet.device.status.ready]: '就绪',
            [puppet.device.status.unknown]: '未知'
        };
        return statuses[status] || '未知';
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
                <h2>设备列表</h2>
                <table class="device-table">
                    <thead>
                        <tr>
                            <th>类型</th>
                            <th>名称</th>
                            <th>驱动器</th>
                            <th>状态</th>
                            <th>总大小</th>
                            <th>可用空间</th>
                            <th>使用率</th>
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

// 使用设备查看器
const deviceViewer = new DeviceViewer();

// 加载设备
await deviceViewer.loadDevices();
deviceViewer.render();

// 刷新设备
document.getElementById('refresh-btn').addEventListener('click', async () => {
    await deviceViewer.refresh();
});
```

### 存储空间分析

```javascript
class StorageAnalyzer {
    async analyzeStorage() {
        const disks = await puppet.device.getDevices(puppet.device.type.localDisk);
        const analysis = [];
        
        for (const disk of disks) {
            const usage = (disk.UsedSpace / disk.TotalSize) * 100;
            let status = '正常';
            
            if (usage > 90) {
                status = '严重';
            } else if (usage > 75) {
                status = '警告';
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

// 使用存储分析器
const storageAnalyzer = new StorageAnalyzer();

// 生成报告
const report = await storageAnalyzer.generateReport();
console.log('存储分析报告:', report);
```

## 最佳实践

### 1. 错误处理

捕获设备操作可能出现的错误：

```javascript
async function safeGetDevice(deviceId) {
    try {
        const device = await puppet.device.getDevice(deviceId);
        return device;
    } catch (error) {
        puppet.log.error('获取设备信息失败:', error.message);
        return null;
    }
}
```

### 2. 性能优化

避免频繁查询设备信息：

```javascript
class DeviceCache {
    constructor() {
        this.cache = new Map();
        this.cacheTimeout = 60000; // 1 分钟
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

### 3. 异步操作

正确处理异步操作：

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

### 4. 数据验证

验证设备数据：

```javascript
function validateDevice(device) {
    if (!device || !device.DeviceId) {
        throw new Error('无效的设备数据');
    }
    
    if (device.TotalSize < 0 || device.FreeSpace < 0) {
        throw new Error('无效的存储数据');
    }
    
    if (device.UsedSpace > device.TotalSize) {
        throw new Error('存储数据不一致');
    }
    
    return true;
}
```

## 相关资源

- [Win32_LogicalDisk](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk)：Win32 逻辑磁盘类
- [Windows 设备管理](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/computer-system-hardware-classes)：Windows 设备管理 API
- [WMI 设备查询](https://learn.microsoft.com/en-us/windows/win32/wmisdk/wmi-start-page)：WMI 文档

## 常见问题

### Q: 如何获取所有设备类型？

A: 可以遍历所有设备类型常量：

```javascript
const types = [
    puppet.device.type.unknown,
    puppet.device.type.usbDisk,
    puppet.device.type.localDisk,
    puppet.device.type.removableDisk
];

for (const type of types) {
    const devices = await puppet.device.getDevices(type);
    console.log(`类型 ${type}:`, devices.length, '个设备');
}
```

### Q: 设备 ID 是什么格式？

A: 对于磁盘设备，设备 ID 通常是驱动器号（如 'C:', 'E:'）。

### Q: 如何监听设备插拔？

A: 使用事件系统 API：

```javascript
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB 设备插入:', event.data);
});
```

### Q: 设备查询性能如何？

A: 设备查询通常很快（< 100ms），但频繁查询可能影响性能，建议使用缓存。