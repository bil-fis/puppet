# Puppet JavaScript API 使用指南

本文档详细介绍了Puppet框架的JavaScript API，包括事件系统和设备系统的完整使用方法。

## 目录

- [快速开始](#快速开始)
- [事件系统](#事件系统)
  - [addEventListener](#addeventlistener)
  - [removeEventListener](#removeeventlistener)
  - [可用事件列表](#可用事件列表)
- [设备系统](#设备系统)
  - [getDevice](#getdevice)
  - [getDevices](#getdevices)
  - [设备类型](#设备类型)
  - [设备状态](#设备状态)
  - [电源状态](#电源状态)
- [完整示例](#完整示例)

---

## 快速开始

### 基本使用

Puppet框架提供了一个全局的`puppet`命名空间，所有功能都通过这个命名空间访问：

```javascript
// 事件系统
puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log('U盘插入:', e.Device.Name);
});

// 设备系统
const devices = await puppet.device.getDevices(puppet.device.type.usbDisk);
console.log('USB设备:', devices);
```

### 初始化

Puppet命名空间会在WebView2加载完成后自动初始化，无需手动初始化。

---

## 事件系统

事件系统允许你监听系统级事件，如USB设备插入、窗口变化、电源切换等。

### addEventListener

为指定的事件添加监听器。

#### 语法

```javascript
const listenerId = await puppet.events.addEventListener(eventName, callback);
```

#### 参数

| 参数 | 类型 | 说明 |
|------|------|------|
| `eventName` | `String` | 事件名称，参见[可用事件列表](#可用事件列表) |
| `callback` | `Function` 或 `String` | 回调函数（支持直接传函数或函数名） |

#### 返回值

返回一个数字（listenerId），用于稍后移除监听器。

#### 使用方式

**方式1：直接传函数（推荐）**
```javascript
const listenerId = await puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log('U盘插入:', e.Device.Name);
    console.log('盘符:', e.Device.DriveLetter);
    console.log('容量:', e.Device.Size);
});
```

**方式2：传递函数名**
```javascript
// 定义回调函数
window.onUSBPlugIn = function(e) {
    console.log('U盘插入:', e.Device.Name);
};

// 注册监听器
const listenerId = await puppet.events.addEventListener('usb-plug-in', 'onUSBPlugIn');
```

#### 回调函数参数

回调函数会接收一个事件对象`e`，包含以下属性：

```javascript
{
    deviceType: Number,    // 设备类型
    Device: Object          // 设备详细信息对象
}
```

#### 示例

```javascript
// 监听USB插入
const listenerId1 = await puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log('USB设备插入:');
    console.log('  设备类型:', e.deviceType);
    console.log('  设备名称:', e.Device.Name);
    console.log('  盘符:', e.Device.DriveLetter);
    console.log('  容量:', e.Device.Size);
    console.log('  可用空间:', e.Device.FreeSpace);
});

// 监听窗口焦点变化
const listenerId2 = await puppet.events.addEventListener('window-focus', (e) => {
    console.log('窗口获得焦点');
});

// 监听电源变化
const listenerId3 = await puppet.events.addEventListener('power-change', (e) => {
    console.log('电源状态变化');
    console.log('  电源状态:', e.status);
    console.log('  电池电量:', e.batteryLevel);
});
```

### removeEventListener

移除之前添加的事件监听器。

#### 语法

```javascript
await puppet.events.removeEventListener(eventName, listenerId);
```

#### 参数

| 参数 | 类型 | 说明 |
|------|------|------|
| `eventName` | `String` | 事件名称 |
| `listenerId` | `Number` | `addEventListener`返回的监听器ID |

#### 返回值

无返回值。

#### 示例

```javascript
// 添加监听器
const listenerId = await puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log('U盘插入:', e.Device.Name);
});

// 稍后移除监听器
await puppet.events.removeEventListener('usb-plug-in', listenerId);
```

#### 注意事项

- `listenerId`必须使用`addEventListener`返回的值
- 移除不存在的监听器不会抛出错误
- 一个事件可以有多个监听器，每个监听器有独立的ID

### 可用事件列表

#### USB设备事件

##### usb-plug-in

**触发时机**：当USB存储设备（U盘）插入时触发

**事件数据**：
```javascript
{
    deviceType: 101,  // puppet.device.type.usbDisk
    Device: {
        DeviceID: "H:",
        Type: 101,
        Status: 0,
        Name: "H:",
        Description: null,
        Manufacturer: null,
        PNPDeviceID: null,
        DriveLetter: "H:",
        VolumeName: "林晚晚",
        Size: 61530406912,
        FreeSpace: 59073396736,
        FileSystem: "NTFS",
        ConfigManagerErrorCode: 0,
        Present: false
    }
}
```

**示例**：
```javascript
await puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log(`U盘 ${e.Device.DriveLetter} 已插入`);
    console.log(`卷标: ${e.Device.VolumeName}`);
    console.log(`容量: ${formatBytes(e.Device.Size)}`);
    console.log(`可用空间: ${formatBytes(e.Device.FreeSpace)}`);
});
```

##### usb-plug-out

**触发时机**：当USB存储设备（U盘）拔出时触发

**事件数据**：与`usb-plug-in`相同

**示例**：
```javascript
await puppet.events.addEventListener('usb-plug-out', (e) => {
    console.log(`U盘 ${e.Device.DriveLetter} 已拔出`);
});
```

#### 磁盘事件

##### disk-mount

**触发时机**：当磁盘被挂载时触发

**事件数据**：与`usb-plug-in`相同

**示例**：
```javascript
await puppet.events.addEventListener('disk-mount', (e) => {
    console.log(`磁盘 ${e.Device.DriveLetter} 已挂载`);
});
```

##### disk-unmount

**触发时机**：当磁盘被卸载时触发

**事件数据**：与`usb-plug-in`相同

**示例**：
```javascript
await puppet.events.addEventListener('disk-unmount', (e) => {
    console.log(`磁盘 ${e.Device.DriveLetter} 已卸载`);
});
```

#### 窗口事件

##### window-focus

**触发时机**：当窗口获得焦点时触发

**事件数据**：
```javascript
{
    focused: true
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-focus', (e) => {
    console.log('窗口获得焦点');
});
```

##### window-blur

**触发时机**：当窗口失去焦点时触发

**事件数据**：
```javascript
{
    focused: false
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-blur', (e) => {
    console.log('窗口失去焦点');
});
```

##### window-maximize

**触发时机**：当窗口被最大化时触发

**事件数据**：
```javascript
{
    windowState: 'maximized'
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-maximize', (e) => {
    console.log('窗口已最大化');
});
```

##### window-minimize

**触发时机**：当窗口被最小化时触发

**事件数据**：
```javascript
{
    windowState: 'minimized'
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-minimize', (e) => {
    console.log('窗口已最小化');
});
```

##### window-restore

**触发时机**：当窗口从最大化或最小化恢复时触发

**事件数据**：
```javascript
{
    windowState: 'normal'
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-restore', (e) => {
    console.log('窗口已恢复正常');
});
```

##### window-resize

**触发时机**：当窗口大小改变时触发

**事件数据**：
```javascript
{
    width: Number,    // 新的宽度
    height: Number    // 新的高度
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-resize', (e) => {
    console.log(`窗口大小改变: ${e.width}x${e.height}`);
});
```

##### window-move

**触发时机**：当窗口被拖动时触发

**事件数据**：
```javascript
{
    x: Number,    // 新的X坐标
    y: Number     // 新的Y坐标
}
```

**示例**：
```javascript
await puppet.events.addEventListener('window-move', (e) => {
    console.log(`窗口移动到: (${e.x}, ${e.y})`);
});
```

#### 电源事件

##### power-change

**触发时机**：当电源状态改变时触发（插入/拔出电源适配器）

**事件数据**：
```javascript
{
    status: Number,       // 电源状态
    batteryLevel: Number  // 电池电量百分比（0-100）
}
```

**示例**：
```javascript
await puppet.events.addEventListener('power-change', (e) => {
    console.log('电源状态变化');
    console.log('  状态:', e.status);
    console.log('  电量:', e.batteryLevel + '%');
});
```

---

## 设备系统

设备系统提供查询和管理系统设备的功能。

### getDevice

获取单个设备的详细信息。

#### 语法

```javascript
const device = await puppet.device.getDevice(deviceId);
```

#### 参数

| 参数 | 类型 | 说明 |
|------|------|------|
| `deviceId` | `String` | 设备ID，例如"C:"、"H:"等 |

#### 返回值

返回一个设备对象，如果设备不存在则返回`null`。

#### 设备对象结构

```javascript
{
    DeviceID: String,              // 设备ID
    Type: Number,                 // 设备类型
    Status: Number,                // 设备状态
    Name: String,                  // 设备名称
    Description: String,           // 设备描述
    Manufacturer: String,          // 制造商
    PNPDeviceID: String,           // PNP设备ID
    DriveLetter: String,           // 盘符（仅可移动存储设备）
    VolumeName: String,            // 卷标
    Size: Number,                  // 总容量（字节）
    FreeSpace: Number,             // 可用空间（字节）
    FileSystem: String,            // 文件系统
    ConfigManagerErrorCode: Number // 配置管理器错误代码
}
```

#### 示例

```javascript
// 获取C盘信息
const cDrive = await puppet.device.getDevice('C:');
if (cDrive) {
    console.log('C盘信息:');
    console.log('  容量:', formatBytes(cDrive.Size));
    console.log('  可用空间:', formatBytes(cDrive.FreeSpace));
    console.log('  文件系统:', cDrive.FileSystem);
    console.log('  卷标:', cDrive.VolumeName);
} else {
    console.log('C盘不存在');
}

// 获取U盘信息
const usbDrive = await puppet.device.getDevice('H:');
if (usbDrive) {
    console.log('U盘信息:', usbDrive);
}
```

### getDevices

获取指定类型的所有设备列表。

#### 语法

```javascript
const devices = await puppet.device.getDevices(deviceType);
```

#### 参数

| 参数 | 类型 | 说明 |
|------|------|------|
| `deviceType` | `Number` | 设备类型，参见[设备类型](#设备类型) |

#### 返回值

返回一个设备对象数组。

#### 示例

```javascript
// 获取所有U盘
const usbDisks = await puppet.device.getDevices(puppet.device.type.usbDisk);
console.log(`找到 ${usbDisks.length} 个U盘:`);
usbDisks.forEach(disk => {
    console.log(`  ${disk.DriveLetter} - ${disk.VolumeName || '无卷标'}`);
    console.log(`    容量: ${formatBytes(disk.Size)}`);
    console.log(`    可用: ${formatBytes(disk.FreeSpace)}`);
});

// 获取所有本地磁盘
const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
console.log(`找到 ${localDisks.length} 个本地磁盘:`);
localDisks.forEach(disk => {
    console.log(`  ${disk.DriveLetter}`);
});

// 获取所有设备
const allDevices = await puppet.device.getDevices(puppet.device.type.unknown);
console.log(`系统共有 ${allDevices.length} 个设备`);
```

### 设备类型

设备类型定义在`puppet.device.type`对象中：

| 类型 | 值 | 说明 |
|------|---|------|
| `unknown` | 0 | 未知设备 |
| `removableDisk` | 2 | 可移动磁盘 |
| `localDisk` | 3 | 本地磁盘 |
| `networkDrive` | 4 | 网络驱动器 |
| `compactDisc` | 5 | 光盘 |
| `ramDisk` | 6 | RAM磁盘 |
| `usbDevice` | 100 | USB设备 |
| `usbDisk` | 101 | USB磁盘（U盘） |
| `usbHub` | 102 | USB集线器 |
| `usbPrinter` | 103 | USB打印机 |
| `usbCamera` | 104 | USB摄像头 |
| `usbStorage` | 105 | USB存储设备 |
| `keyboard` | 200 | 键盘 |
| `mouse` | 201 | 鼠标 |
| `monitor` | 202 | 显示器 |
| `printer` | 203 | 打印机 |
| `scanner` | 204 | 扫描仪 |
| `networkAdapter` | 205 | 网络适配器 |
| `audio` | 206 | 音频设备 |
| `video` | 207 | 视频设备 |
| `bluetooth` | 208 | 蓝牙设备 |

#### 示例

```javascript
// 获取U盘
const usbDisks = await puppet.device.getDevices(puppet.device.type.usbDisk);

// 获取所有可移动磁盘
const removableDisks = await puppet.device.getDevices(puppet.device.type.removableDisk);

// 获取本地磁盘
const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);

// 获取所有设备
const allDevices = await puppet.device.getDevices(puppet.device.type.unknown);
```

### 设备状态

设备状态定义在`puppet.device.status`对象中：

| 状态 | 值 | 说明 |
|------|---|------|
| `unknown` | 0 | 未知状态 |
| `ok` | 1 | 正常 |
| `error` | 2 | 错误 |
| `degraded` | 3 | 降级 |
| `predFail` | 4 | 预测失败 |
| `starting` | 5 | 启动中 |
| `stopping` | 6 | 停止中 |
| `service` | 7 | 服务 |
| `stressed` | 8 | 压力过大 |
| `nonRecover` | 9 | 不可恢复 |
| `noContact` | 10 | 无联系 |
| `lostComm` | 11 | 丢失通信 |
| `notConfigured` | 20 | 未配置 |
| `disabled` | 22 | 已禁用 |
| `notPresent` | 24 | 不存在 |
| `stillSettingUp` | 25 | 正在设置中 |
| `driversNotInstalled` | 28 | 驱动未安装 |
| `ready` | 100 | 就绪 |
| `notReady` | 101 | 未就绪 |
| `pending` | 102 | 等待中 |
| `ejected` | 103 | 已弹出 |
| `stalled` | 104 | 已停止 |

#### 示例

```javascript
const device = await puppet.device.getDevice('C:');
if (device) {
    console.log('设备状态:', device.Status);
    
    // 检查设备是否正常
    if (device.Status === puppet.device.status.ok) {
        console.log('设备状态正常');
    } else if (device.Status === puppet.device.status.error) {
        console.log('设备状态错误');
    }
}
```

### 电源状态

电源状态定义在`puppet.device.powerStatus`对象中：

| 状态 | 值 | 说明 |
|------|---|------|
| `unknown` | 0 | 未知 |
| `discharging` | 1 | 放电中 |
| `acConnected` | 2 | 已连接交流电源 |
| `fullyCharged` | 3 | 已充满 |
| `low` | 4 | 电量低 |
| `critical` | 5 | 电量严重不足 |
| `charging` | 6 | 充电中 |
| `chargingHigh` | 7 | 充电中（高电量） |
| `chargingLow` | 8 | 充电中（低电量） |
| `chargingCritical` | 9 | 充电中（严重不足） |
| `undefined` | 10 | 未定义 |
| `partiallyCharged` | 11 | 部分充电 |

#### 示例

```javascript
await puppet.events.addEventListener('power-change', (e) => {
    console.log('电源状态:', e.status);
    
    switch(e.status) {
        case puppet.device.powerStatus.charging:
            console.log('正在充电');
            break;
        case puppet.device.powerStatus.discharging:
            console.log('正在放电');
            break;
        case puppet.device.powerStatus.fullyCharged:
            console.log('已充满电');
            break;
        case puppet.device.powerStatus.acConnected:
            console.log('已连接电源');
            break;
    }
});
```

---

## 完整示例

### 示例1：USB设备管理器

```javascript
// 存储USB设备监听器ID
const usbListeners = [];

// 监听USB插入
const plugInListener = await puppet.events.addEventListener('usb-plug-in', (e) => {
    console.log('=== USB设备插入 ===');
    console.log('盘符:', e.Device.DriveLetter);
    console.log('卷标:', e.Device.VolumeName);
    console.log('容量:', formatBytes(e.Device.Size));
    console.log('可用空间:', formatBytes(e.Device.FreeSpace));
    console.log('文件系统:', e.Device.FileSystem);
    console.log('==================');
    
    // 显示设备信息
    displayDeviceInfo(e.Device);
});
usbListeners.push({ event: 'usb-plug-in', id: plugInListener });

// 监听USB拔出
const plugOutListener = await puppet.events.addEventListener('usb-plug-out', (e) => {
    console.log('=== USB设备拔出 ===');
    console.log('盘符:', e.Device.DriveLetter);
    console.log('卷标:', e.Device.VolumeName);
    console.log('==================');
});
usbListeners.push({ event: 'usb-plug-out', id: plugOutListener });

// 辅助函数：格式化字节大小
function formatBytes(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
}

// 辅助函数：显示设备信息
function displayDeviceInfo(device) {
    const infoDiv = document.getElementById('device-info');
    infoDiv.innerHTML = `
        <h3>设备信息</h3>
        <p><strong>盘符:</strong> ${device.DriveLetter}</p>
        <p><strong>卷标:</strong> ${device.VolumeName || '无卷标'}</p>
        <p><strong>容量:</strong> ${formatBytes(device.Size)}</p>
        <p><strong>可用空间:</strong> ${formatBytes(device.FreeSpace)}</p>
        <p><strong>文件系统:</strong> ${device.FileSystem}</p>
        <p><strong>使用率:</strong> ${((1 - device.FreeSpace / device.Size) * 100).toFixed(1)}%</p>
    `;
}

// 停止监听
function stopUSBListeners() {
    usbListeners.forEach(listener => {
        puppet.events.removeEventListener(listener.event, listener.id);
    });
    usbListeners.length = 0;
    console.log('USB监听器已停止');
}
```

### 示例2：磁盘监控器

```javascript
// 监听所有磁盘挂载和卸载事件
await puppet.events.addEventListener('disk-mount', (e) => {
    console.log(`磁盘 ${e.Device.DriveLetter} 已挂载`);
    updateDiskList();
});

await puppet.events.addEventListener('disk-unmount', (e) => {
    console.log(`磁盘 ${e.Device.DriveLetter} 已卸载`);
    updateDiskList();
});

// 更新磁盘列表
async function updateDiskList() {
    const allDisks = await puppet.device.getDevices(puppet.device.type.unknown);
    
    const diskListDiv = document.getElementById('disk-list');
    diskListDiv.innerHTML = '';
    
    allDisks.forEach(disk => {
        const diskItem = document.createElement('div');
        diskItem.className = 'disk-item';
        diskItem.innerHTML = `
            <span class="disk-icon">${getDiskIcon(disk.Type)}</span>
            <span class="disk-name">${disk.DriveLetter}</span>
            <span class="disk-info">${formatBytes(disk.Size)} (${formatBytes(disk.FreeSpace)} 可用)</span>
        `;
        diskListDiv.appendChild(diskItem);
    });
}

function getDiskIcon(type) {
    switch(type) {
        case 2: return '📱';  // 可移动磁盘
        case 3: return '💾';  // 本地磁盘
        case 4: return '🌐';  // 网络驱动器
        case 5: return '💿';  // 光盘
        case 6: return '🔴';  // RAM磁盘
        default: return '💽';
    }
}
```

### 示例3：窗口状态监控

```javascript
// 监听窗口状态变化
await puppet.events.addEventListener('window-maximize', () => {
    console.log('窗口已最大化');
});

await puppet.events.addEventListener('window-minimize', () => {
    console.log('窗口已最小化');
});

await puppet.events.addEventListener('window-restore', () => {
    console.log('窗口已恢复正常');
});

await puppet.events.addEventListener('window-resize', (e) => {
    console.log(`窗口大小改变: ${e.width}x${e.height}`);
    updateWindowInfo();
});

await puppet.events.addEventListener('window-move', (e) => {
    console.log(`窗口移动到: (${e.x}, ${e.y})`);
    updateWindowInfo();
});

// 更新窗口信息显示
async function updateWindowInfo() {
    const info = await puppet.Application.getWindowInfo();
    const infoDiv = document.getElementById('window-info');
    infoDiv.innerHTML = `
        <p><strong>位置:</strong> (${info.x}, ${info.y})</p>
        <p><strong>大小:</strong> ${info.width}x${info.height}</p>
        <p><strong>状态:</strong> ${info.state}</p>
    `;
}
```

### 示例4：电源监控

```javascript
// 监听电源状态变化
await puppet.events.addEventListener('power-change', (e) => {
    console.log('=== 电源状态变化 ===');
    console.log('状态:', getPowerStatusText(e.status));
    console.log('电量:', e.batteryLevel + '%');
    console.log('==================');
    
    updatePowerDisplay(e.status, e.batteryLevel);
});

function getPowerStatusText(status) {
    const statusMap = {
        1: '放电中',
        2: '已连接交流电源',
        3: '已充满电',
        4: '电量低',
        5: '电量严重不足',
        6: '充电中',
        7: '充电中（高电量）',
        8: '充电中（低电量）',
        9: '充电中（严重不足）',
        11: '部分充电'
    };
    return statusMap[status] || '未知';
}

function updatePowerDisplay(status, batteryLevel) {
    const powerDiv = document.getElementById('power-status');
    
    let icon = '🔋';
    if (status === 6 || status === 7 || status === 8 || status === 9) {
        icon = '⚡';
    } else if (status === 2) {
        icon = '🔌';
    }
    
    powerDiv.innerHTML = `
        <div class="power-icon">${icon}</div>
        <div class="power-info">
            <div class="power-status">${getPowerStatusText(status)}</div>
            <div class="battery-bar">
                <div class="battery-fill" style="width: ${batteryLevel}%"></div>
            </div>
            <div class="battery-text">${batteryLevel}%</div>
        </div>
    `;
}
```

### 示例5：完整的设备管理应用

```javascript
class DeviceManager {
    constructor() {
        this.listeners = [];
        this.currentDevices = [];
        this.init();
    }
    
    async init() {
        // 初始化设备列表
        await this.refreshDeviceList();
        
        // 注册事件监听器
        await this.registerEventListeners();
        
        // 定期刷新设备列表（每5秒）
        this.refreshInterval = setInterval(() => this.refreshDeviceList(), 5000);
    }
    
    async registerEventListeners() {
        // USB事件
        const usbPlugIn = await puppet.events.addEventListener('usb-plug-in', (e) => {
            this.onDeviceAdded(e.Device);
            this.refreshDeviceList();
        });
        this.listeners.push({ event: 'usb-plug-in', id: usbPlugIn });
        
        const usbPlugOut = await puppet.events.addEventListener('usb-plug-out', (e) => {
            this.onDeviceRemoved(e.Device);
            this.refreshDeviceList();
        });
        this.listeners.push({ event: 'usb-plug-out', id: usbPlugOut });
        
        // 磁盘事件
        const diskMount = await puppet.events.addEventListener('disk-mount', (e) => {
            this.onDeviceAdded(e.Device);
            this.refreshDeviceList();
        });
        this.listeners.push({ event: 'disk-mount', id: diskMount });
        
        const diskUnmount = await puppet.events.addEventListener('disk-unmount', (e) => {
            this.onDeviceRemoved(e.Device);
            this.refreshDeviceList();
        });
        this.listeners.push({ event: 'disk-unmount', id: diskUnmount });
    }
    
    async refreshDeviceList() {
        this.currentDevices = await puppet.device.getDevices(puppet.device.type.unknown);
        this.renderDeviceList();
    }
    
    renderDeviceList() {
        const container = document.getElementById('device-container');
        container.innerHTML = '';
        
        if (this.currentDevices.length === 0) {
            container.innerHTML = '<p>没有找到设备</p>';
            return;
        }
        
        this.currentDevices.forEach(device => {
            const deviceCard = this.createDeviceCard(device);
            container.appendChild(deviceCard);
        });
    }
    
    createDeviceCard(device) {
        const card = document.createElement('div');
        card.className = 'device-card';
        card.innerHTML = `
            <div class="device-icon">${this.getDeviceIcon(device.Type)}</div>
            <div class="device-info">
                <h3>${device.DriveLetter || device.Name}</h3>
                <p class="device-type">${this.getDeviceTypeName(device.Type)}</p>
                <p class="device-volume">${device.VolumeName || '无卷标'}</p>
                <p class="device-size">${this.formatBytes(device.Size)}</p>
                <p class="device-free">${this.formatBytes(device.FreeSpace)} 可用</p>
                <p class="device-status">${this.getStatusName(device.Status)}</p>
            </div>
        `;
        return card;
    }
    
    getDeviceIcon(type) {
        const iconMap = {
            2: '📱', 3: '💾', 4: '🌐', 5: '💿', 6: '🔴',
            100: '🔌', 101: '💾', 102: '🔀', 103: '🖨️',
            104: '📷', 105: '💿', 200: '⌨️', 201: '🖱️',
            202: '🖥️', 203: '🖨️', 204: '🖨️', 205: '🌐',
            206: '🔊', 207: '🎬', 208: '📶'
        };
        return iconMap[type] || '💽';
    }
    
    getDeviceTypeName(type) {
        const nameMap = {
            0: '未知设备', 2: '可移动磁盘', 3: '本地磁盘',
            4: '网络驱动器', 5: '光盘', 6: 'RAM磁盘',
            100: 'USB设备', 101: 'USB磁盘', 102: 'USB集线器',
            103: 'USB打印机', 104: 'USB摄像头', 105: 'USB存储设备',
            200: '键盘', 201: '鼠标', 202: '显示器',
            203: '打印机', 204: '扫描仪', 205: '网络适配器',
            206: '音频设备', 207: '视频设备', 208: '蓝牙设备'
        };
        return nameMap[type] || '未知';
    }
    
    getStatusName(status) {
        const nameMap = {
            0: '未知', 1: '正常', 2: '错误', 3: '降级',
            4: '预测失败', 5: '启动中', 6: '停止中',
            7: '服务', 8: '压力过大', 9: '不可恢复',
            10: '无联系', 11: '丢失通信', 20: '未配置',
            22: '已禁用', 24: '不存在', 25: '正在设置中',
            28: '驱动未安装', 100: '就绪', 101: '未就绪',
            102: '等待中', 103: '已弹出', 104: '已停止'
        };
        return nameMap[status] || '未知';
    }
    
    formatBytes(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
    }
    
    onDeviceAdded(device) {
        console.log(`设备已添加: ${device.DriveLetter || device.Name}`);
        this.showNotification('设备已连接', `${device.DriveLetter || device.Name} - ${this.getDeviceTypeName(device.Type)}`);
    }
    
    onDeviceRemoved(device) {
        console.log(`设备已移除: ${device.DriveLetter || device.Name}`);
        this.showNotification('设备已断开', `${device.DriveLetter || device.Name}`);
    }
    
    showNotification(title, message) {
        if ('Notification' in window && Notification.permission === 'granted') {
            new Notification(title, { body: message });
        }
    }
    
    destroy() {
        // 清理事件监听器
        this.listeners.forEach(listener => {
            puppet.events.removeEventListener(listener.event, listener.id);
        });
        this.listeners = [];
        
        // 清理定时器
        if (this.refreshInterval) {
            clearInterval(this.refreshInterval);
        }
    }
}

// 使用示例
const deviceManager = new DeviceManager();
```

---

## 注意事项

### 1. 异步操作

所有`puppet` API都是异步的，必须使用`await`：

```javascript
// ✅ 正确
const devices = await puppet.device.getDevices(puppet.device.type.usbDisk);

// ❌ 错误
const devices = puppet.device.getDevices(puppet.device.type.usbDisk);
```

### 2. 事件监听器管理

- 每次调用`addEventListener`都会创建一个新的监听器
- 记住保存返回的`listenerId`，以便后续移除
- 不要为同一个事件添加太多监听器，以免影响性能

### 3. 性能考虑

- 事件监听器是懒加载的，只有添加监听器后才开始后台监听
- 移除不需要的监听器以释放系统资源
- 定期刷新设备列表时注意刷新频率

### 4. 错误处理

```javascript
try {
    const devices = await puppet.device.getDevices(puppet.device.type.usbDisk);
    console.log('获取设备成功:', devices);
} catch (error) {
    console.error('获取设备失败:', error);
}
```

### 5. 兼容性

- 只在Windows系统上可用
- 需要管理员权限才能监听某些事件
- 某些功能可能需要特定的Windows版本支持

---

## 更多信息

- [WebView2 JS绑定指南](./WebView2-JS绑定指南.md)
- [Puppet框架文档](./README.md)
- [Microsoft Learn - Win32_LogicalDisk](https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldisk)
- [Microsoft Learn - WMI Events](https://learn.microsoft.com/en-us/windows/win32/wmisdk/receiving-a-wmi-event)