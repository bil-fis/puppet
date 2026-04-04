---
title: System API
permalink: /en/api/system.html
createTime: 2026/03/28 15:08:10
---

# System API

The System API provides features such as system information retrieval, input simulation, and screenshots.

## Overview

The `puppet.system` namespace provides the following features:

- Get system information (CPU, memory, GPU, operating system)
- Screen capture
- Get desktop wallpaper
- Simulate key presses
- Simulate mouse clicks
- Get mouse position

## Methods

### getSystemInfo()

Gets system information.

```javascript
await puppet.system.getSystemInfo(): Promise<SystemInfo>
```

**Return Value**:

```typescript
interface SystemInfo {
    osName: string;           // Operating system name
    osVersion: string;        // Operating system version
    computerName: string;     // Computer name
    cpuModel: string;         // CPU model
    cpuCores: number;         // CPU cores
    totalMemory: number;      // Total memory (MB)
    availableMemory: number;  // Available memory (MB)
    gpuModel: string;         // GPU model
    screenWidth: number;      // Screen width
    screenHeight: number;     // Screen height
    is64Bit: boolean;         // Whether it's a 64-bit system
}
```

**Example**:

```javascript
// Get system information
const sysInfo = await puppet.system.getSystemInfo();
console.log('Operating system:', sysInfo.osName);
console.log('CPU:', sysInfo.cpuModel, `(${sysInfo.cpuCores} cores)`);
console.log('Memory:', sysInfo.totalMemory, 'MB');
console.log('Screen:', `${sysInfo.screenWidth}x${sysInfo.screenHeight}`);
```

### takeScreenShot()

Takes screen capture.

```javascript
await puppet.system.takeScreenShot(): Promise<string>
```

**Return Value**:

Base64 encoded PNG image data.

**Example**:

```javascript
// Take screenshot
const screenshot = await puppet.system.takeScreenShot();

// Display screenshot
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + screenshot;
document.body.appendChild(img);

// Save screenshot
await puppet.fs.writeByteToFile('screenshot.png', screenshot);
```

### getDesktopWallpaper()

Gets desktop wallpaper.

```javascript
await puppet.system.getDesktopWallpaper(): Promise<string>
```

**Return Value**:

Base64 encoded image data.

**Example**:

```javascript
// Get desktop wallpaper
const wallpaper = await puppet.system.getDesktopWallpaper();

// Display wallpaper
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + wallpaper;
document.body.appendChild(img);

// Save wallpaper
await puppet.fs.writeByteToFile('wallpaper.png', wallpaper);
```

### sendKey()

Simulates key presses.

```javascript
await puppet.system.sendKey(...keys: string[]): Promise<void>
```

**Parameters**:

- `keys` (string[]) - List of keys to simulate

**Supported Keys**:

- Letters: A-Z
- Numbers: 0-9
- Function keys: ENTER, TAB, SPACE, ESC
- Control keys: CTRL, ALT, SHIFT
- Arrow keys: UP, DOWN, LEFT, RIGHT

**Example**:

```javascript
// Simulate single key
await puppet.system.sendKey('ENTER');
await puppet.system.sendKey('A');
await puppet.system.sendKey('SPACE');

// Simulate combination keys
await puppet.system.sendKey('CTRL', 'C');      // Copy
await puppet.system.sendKey('CTRL', 'V');      // Paste
await puppet.system.sendKey('CTRL', 'A');      // Select all
await puppet.system.sendKey('ALT', 'TAB');     // Switch window

// Simulate shortcuts
await puppet.system.sendKey('CTRL', 'S');      // Save
await puppet.system.sendKey('CTRL', 'Z');      // Undo
await puppet.system.sendKey('CTRL', 'SHIFT', 'ESC'); // Task Manager
```

### sendMouseClick()

Simulates mouse click.

```javascript
await puppet.system.sendMouseClick(x: number, y: number, button: string): Promise<void>
```

**Parameters**:

- `x` (number) - Mouse X coordinate
- `y` (number) - Mouse Y coordinate
- `button` (string) - Mouse button ('left', 'right', 'middle')

**Example**:

```javascript
// Left click
await puppet.system.sendMouseClick(100, 200, 'left');

// Right click
await puppet.system.sendMouseClick(100, 200, 'right');

// Middle click
await puppet.system.sendMouseClick(100, 200, 'middle');

// Click screen center
const sysInfo = await puppet.system.getSystemInfo();
const centerX = sysInfo.screenWidth / 2;
const centerY = sysInfo.screenHeight / 2;
await puppet.system.sendMouseClick(centerX, centerY, 'left');
```

### getMousePosition()

Gets current mouse position.

```javascript
await puppet.system.getMousePosition(): Promise<MousePosition>
```

**Return Value**:

```typescript
interface MousePosition {
    x: number;  // Mouse X coordinate
    y: number;  // Mouse Y coordinate
}
```

**Example**:

```javascript
// Get mouse position
const pos = await puppet.system.getMousePosition();
console.log('Mouse position:', `(${pos.x}, ${pos.y})`);

// Track mouse position in real time
setInterval(async () => {
    const pos = await puppet.system.getMousePosition();
    document.getElementById('mouse-pos').textContent = `X: ${pos.x}, Y: ${pos.y}`;
}, 100);
```

## Usage Examples

### System Information Display

```javascript
async function displaySystemInfo() {
    const sysInfo = await puppet.system.getSystemInfo();
    
    const infoHTML = `
        <div class="system-info">
            <h2>System Information</h2>
            <div class="info-item">
                <label>Operating System:</label>
                <span>${sysInfo.osName} ${sysInfo.osVersion}</span>
            </div>
            <div class="info-item">
                <label>Computer Name:</label>
                <span>${sysInfo.computerName}</span>
            </div>
            <div class="info-item">
                <label>CPU:</label>
                <span>${sysInfo.cpuModel} (${sysInfo.cpuCores} cores)</span>
            </div>
            <div class="info-item">
                <label>Memory:</label>
                <span>${sysInfo.availableMemory} / ${sysInfo.totalMemory} MB</span>
            </div>
            <div class="info-item">
                <label>GPU:</label>
                <span>${sysInfo.gpuModel}</span>
            </div>
            <div class="info-item">
                <label>Screen:</label>
                <span>${sysInfo.screenWidth} x ${sysInfo.screenHeight}</span>
            </div>
            <div class="info-item">
                <label>Architecture:</label>
                <span>${sysInfo.is64Bit ? '64-bit' : '32-bit'}</span>
            </div>
        </div>
    `;
    
    document.getElementById('info-container').innerHTML = infoHTML;
}

// Display system information
displaySystemInfo();
```

### Screenshot Tool

```javascript
class ScreenshotTool {
    async capture() {
        // Take screenshot
        const screenshot = await puppet.system.takeScreenShot();
        
        // Display screenshot
        this.displayScreenshot(screenshot);
        
        // Return screenshot data
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
        // Save screenshot
        await puppet.fs.writeByteToFile(filename, data);
        console.log('Screenshot saved:', filename);
    }
    
    async captureAndSave() {
        const data = await this.capture();
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const filename = `screenshot_${timestamp}.png`;
        await this.save(data, filename);
    }
}

// Use screenshot tool
const screenshotTool = new ScreenshotTool();

// Capture screenshot
document.getElementById('capture-btn').addEventListener('click', async () => {
    await screenshotTool.capture();
});

// Capture and save
document.getElementById('save-btn').addEventListener('click', async () => {
    await screenshotTool.captureAndSave();
});
```

### Automation Operations

```javascript
async function performAutomatedAction() {
    // Wait 2 seconds
    await new Promise(resolve => setTimeout(resolve, 2000));
    
    // Get system information
    const sysInfo = await puppet.system.getSystemInfo();
    console.log('Screen size:', `${sysInfo.screenWidth}x${sysInfo.screenHeight}`);
    
    // Click at specified position
    await puppet.system.sendMouseClick(100, 100, 'left');
    
    // Wait 1 second
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    // Simulate key presses
    await puppet.system.sendKey('A');
    await puppet.system.sendKey('B');
    await puppet.system.sendKey('ENTER');
    
    console.log('Automated operation completed');
}

// Execute automated operation
performAutomatedAction();
```

### Mouse Tracker

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
        
        // Update display
        document.getElementById('mouse-pos').textContent = 
            `X: ${pos.x}, Y: ${pos.y} (${this.positions.length} points)`;
        
        // Continue tracking
        requestAnimationFrame(() => this.track());
    }
    
    stopTracking() {
        this.isTracking = false;
        console.log('Tracking ended, recorded', this.positions.length, 'points');
    }
    
    getPositions() {
        return this.positions;
    }
}

// Use mouse tracker
const tracker = new MouseTracker();

document.getElementById('start-tracking').addEventListener('click', () => {
    tracker.startTracking();
});

document.getElementById('stop-tracking').addEventListener('click', () => {
    tracker.stopTracking();
});
```

## Best Practices

### 1. Error Handling

Catch errors that may occur during system operations:

```javascript
try {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log('System information:', sysInfo);
} catch (error) {
    puppet.log.error('Failed to get system information:', error.message);
    showError('Cannot get system information');
}
```

### 2. Performance Considerations

Avoid frequent system operation calls:

```javascript
// Not recommended: Frequently get mouse position
setInterval(async () => {
    const pos = await puppet.system.getMousePosition();
    // ...
}, 10);

// Recommended: Use requestAnimationFrame
function trackMouse() {
    requestAnimationFrame(async () => {
        const pos = await puppet.system.getMousePosition();
        // ...
        trackMouse();
    });
}
```

### 3. User Confirmation

Get user confirmation before automated operations:

```javascript
async function safeClick(x, y) {
    const confirmed = confirm(`Are you sure you want to click at (${x}, ${y})?`);
    if (confirmed) {
        await puppet.system.sendMouseClick(x, y, 'left');
    }
}
```

### 4. Permission Prompt

Prompt user about possible required permissions:

```javascript
async function performAutomation() {
    console.log('About to perform automated operation...');
    console.log('Please ensure the application has sufficient permissions');
    
    await performAutomatedAction();
}
```

## Related Resources

- [Windows API](https://learn.mozilla.org/en-US/docs/Web/API): Windows API documentation
- [Keyboard Events](https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent): Keyboard event API
- [Mouse Events](https://developer.mozilla.org/en-US/docs/Web/API/MouseEvent): Mouse event API

## Common Questions

### Q: Why doesn't simulating key presses work?

A: Please ensure:
1. Application has sufficient permissions
2. Target window is in active state
3. Key names are correct

### Q: Can drag operations be simulated?

A: Drag operations can be achieved by combining mouse clicks and movements.

### Q: How is screenshot performance?

A: Screenshot performance depends on screen resolution, typically between 100-500ms.

### Q: Does getting mouse position affect performance?

A: Getting mouse position itself has very low performance overhead, but frequent calls may affect performance.