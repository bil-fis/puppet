---
title: TypeScript Type Definitions
permalink: /en/guide/typescript.html
createTime: 2026/04/05 10:00:00
---

# TypeScript Type Definitions

Puppet Framework provides complete TypeScript type definitions, enabling developers to get full type support and IntelliSense in TypeScript projects.

## Installation

```bash
npm install @puppet-framework/types --save-dev
```

Or copy files to your project:

```bash
cp puppet-node/* your-project/
```

## Import Types

### Method 1: Using triple-slash directive

```typescript
/// <reference types="@puppet-framework/types" />

async function init() {
    await puppet.window.setBorderless(true);
}
```

### Method 2: Using import statement

```typescript
import puppet from '@puppet-framework/types';

async function init() {
    await puppet.window.setBorderless(true);
}
```

### Method 3: Configure in tsconfig.json

```json
{
  "compilerOptions": {
    "types": ["@puppet-framework/types"]
  }
}
```

## Type Definitions

### PuppetNamespace

Complete puppet namespace type definition:

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

### WindowController

Window controller type definition:

```typescript
interface WindowController {
    setBorderless(value: boolean): Promise<void>;
    setDraggable(value: boolean): Promise<void>;
    setResizable(value: boolean): Promise<void>;
    setTransparent(value: boolean): Promise<void>;
    setOpacity(value: number): Promise<void>;
    setMouseThroughTransparency(value: boolean): Promise<void>;
    setMouseThrough(value: boolean): Promise<void>;
    setTransparentColor(color: string): Promise<void>;
    setTopmost(value: boolean): Promise<void>;
    moveWindow(x: number, y: number): Promise<void>;
    resizeWindow(width: number, height: number): Promise<void>;
    centerWindow(): Promise<void>;
    showInTaskbar(value: boolean): Promise<void>;
    mountMovableElement(elementId: string): Promise<void>;
    unmountMovableElement(elementId: string): Promise<void>;
}
```

### ApplicationController

Application controller type definition:

```typescript
interface ApplicationController {
    close(): Promise<void>;
    restart(): Promise<void>;
    getWindowInfo(): Promise<WindowInfo>;
    execute(command: string): Promise<void>;
    setConfig(key: string, value: any): Promise<void>;
    getConfig(key: string): Promise<any>;
    getAssemblyDirectory(): Promise<string>;
    getAppDataDirectory(): Promise<string>;
    getCurrentUser(): Promise<UserInfo>;
}
```

### Other Controllers

Other controller type definitions include:
- `FileSystemController` - File system control
- `LogController` - Log control
- `SystemController` - System control
- `TrayController` - Tray icon control
- `EventController` - Event control
- `DeviceController` - Device control

## Data Types

### WindowInfo

Window information type:

```typescript
interface WindowInfo {
    handle: number;
    title: string;
    className: string;
    isVisible: boolean;
    isMinimized: boolean;
    isMaximized: boolean;
    width: number;
    height: number;
    x: number;
    y: number;
}
```

### SystemInfo

System information type:

```typescript
interface SystemInfo {
    osName: string;
    osVersion: string;
    computerName: string;
    cpuModel: string;
    cpuCores: number;
    totalMemory: number;
    availableMemory: number;
    gpuModel: string;
    screenWidth: number;
    screenHeight: number;
    is64Bit: boolean;
}
```

### DeviceInfo

Device information type:

```typescript
interface DeviceInfo {
    DeviceId: string;
    DeviceType: number;
    DeviceName: string;
    Status: number;
    DriveLetter?: string;
    VolumeName?: string;
    FileSystem?: string;
    TotalSize: number;
    FreeSpace: number;
    UsedSpace: number;
    Manufacturer?: string;
    Model?: string;
    SerialNumber?: string;
}
```

## Usage Examples

### Type-safe API calls

```typescript
import puppet from '@puppet-framework/types';

// Set window style
async function setupWindow() {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
    await puppet.window.setOpacity(0.95);
}

// Read configuration file
async function loadConfig(): Promise<MyConfig | null> {
    try {
        const content = await puppet.fs.readFileAsText('config.json');
        return JSON.parse(content) as MyConfig;
    } catch (error) {
        puppet.log.error('Failed to load config:', error);
        return null;
    }
}

// Get system information
async function getSystemInfo() {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log(`OS: ${sysInfo.osName} ${sysInfo.osVersion}`);
    console.log(`CPU: ${sysInfo.cpuModel} (${sysInfo.cpuCores} cores)`);
    console.log(`Memory: ${sysInfo.availableMemory} / ${sysInfo.totalMemory} MB`);
}
```

### Custom type definitions

```typescript
// Define application config type
interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
    language: 'zh-CN' | 'en-US';
    autoUpdate: boolean;
}

// Define user settings type
interface UserSettings {
    name: string;
    email: string;
    notifications: boolean;
}

// Use types
async function saveConfig(config: AppConfig) {
    await puppet.fs.writeTextToFile(
        'config.json',
        JSON.stringify(config, null, 2)
    );
}

async function loadSettings(): Promise<UserSettings | null> {
    try {
        const content = await puppet.fs.readFileAsText('settings.json');
        return JSON.parse(content) as UserSettings;
    } catch (error) {
        return null;
    }
}
```

### Event handling types

```typescript
// Define event data types
interface USBEvent {
    DeviceId: string;
    DeviceName: string;
    DriveLetter: string;
    DeviceType: number;
}

interface WindowEvent {
    eventType: string;
    timestamp: number;
}

// Type-safe event listeners
async function setupEventListeners() {
    // USB device insertion event
    await puppet.events.addEventListener('usb-plug-in', (event: { data: USBEvent }) => {
        console.log(`USB device inserted: ${event.data.DeviceName} (${event.data.DriveLetter})`);
        puppet.log.info(`USB device detected: ${event.data.DeviceName}`);
    });

    // Window maximize event
    await puppet.events.addEventListener('window-maximize', (event: WindowEvent) => {
        console.log(`Window maximized at ${new Date(event.timestamp).toLocaleTimeString()}`);
    });
}
```

## Type Guards

Use type guards for runtime type checking:

```typescript
import { isWindowInfo, isSystemInfo, isDeviceInfo } from '@puppet-framework/types';

// Check window information
function handleWindowInfo(info: any) {
    if (isWindowInfo(info)) {
        // TypeScript now knows info is WindowInfo type
        console.log(`Window: ${info.title} (${info.width}x${info.height})`);
    } else {
        console.error('Invalid window information');
    }
}

// Check system information
function handleSystemInfo(info: any) {
    if (isSystemInfo(info)) {
        // TypeScript now knows info is SystemInfo type
        console.log(`System: ${info.osName} ${info.osVersion}`);
    } else {
        console.error('Invalid system information');
    }
}

// Check device information
function handleDeviceInfo(device: any) {
    if (isDeviceInfo(device)) {
        // TypeScript now knows device is DeviceInfo type
        console.log(`Device: ${device.DeviceName} (${device.DeviceType})`);
    } else {
        console.error('Invalid device information');
    }
}
```

## Environment Detection

Detect current runtime environment:

```typescript
import { 
    isPuppetEnvironment, 
    isBrowserEnvironment, 
    isNodeEnvironment 
} from '@puppet-framework/types';

function checkEnvironment() {
    if (isPuppetEnvironment()) {
        console.log('Running in Puppet Framework environment');
        // Use real puppet API
    } else if (isBrowserEnvironment()) {
        console.log('Running in browser environment');
        // Use mock implementation
    } else if (isNodeEnvironment()) {
        console.log('Running in Node.js environment');
        // Use mock implementation
    }
}
```

## Transparent Windows

To use transparent background windows, you must set transparent background on both HTML and body elements:

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Transparent Window</title>
    <style>
        /* Must set html and body background to transparent */
        html {
            background: transparent;
        }
        
        body {
            background: transparent;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
        
        /* Optional: Add gradient background to content area */
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
        import puppet from '@puppet-framework/types';
        
        async function init() {
            // Enable transparent background
            await puppet.window.setTransparent(true);
            
            // Set window to borderless
            await puppet.window.setBorderless(true);
            
            // Set opacity
            await puppet.window.setOpacity(0.95);
            
            // Set transparent color
            await puppet.window.setTransparentColor('#000000');
            
            // Enable mouse click-through
            await puppet.window.setMouseThroughTransparency(true);
        }
        
        init();
    </script>
</body>
</html>
```

**Important Tips:**
- Must set `background: transparent` on both `html` and `body` elements
- Use `setTransparentColor()` to set the color to be transparent
- Use `setMouseThroughTransparency()` to allow mouse clicks to pass through transparent areas
- Use `setOpacity()` to adjust overall opacity (0.0 - 1.0)

## Development Mode

When developing in browser or Node.js environments, all puppet API calls will output to the console:

```javascript
// Browser console output
[MOCK] setBorderless: true
[MOCK] setOpacity: 0.9
[MOCK] getSystemInfo
```

This allows you to develop and test code without running the Puppet Framework.

## Testing

### Setup test environment

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
        puppet = setupTestEnvironment({ 
            enableLogging: false, 
            resetState: true 
        });
    });

    afterEach(() => {
        cleanupTestEnvironment();
    });

    it('should set window to borderless', async () => {
        await puppet.window.setBorderless(true);
        
        const state = puppet.__getMockState?.();
        assertEqual(state?.window.borderless, true);
    });
});
```

### Using Mock

```typescript
import { 
    createMockPuppetNamespace, 
    getMockState,
    simulateEvent 
} from '@puppet-framework/types/mock';

// Create mock puppet
const mockPuppet = createMockPuppetNamespace(false);

// Get mock state
const state = getMockState();

// Access internal state
console.log(state.window.borderless);
console.log(state.application.config);

// Simulate event
simulateEvent('usb-plug-in', {
    DeviceId: 'E:',
    DeviceName: 'USB Drive',
    DeviceType: 2
});
```

## Best Practices

### 1. Use type interfaces

Define type interfaces for application data:

```typescript
interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
    language: 'zh-CN' | 'en-US';
}

interface UserPreferences {
    autoSave: boolean;
    notifications: boolean;
    checkUpdates: boolean;
}
```

### 2. Error handling

Use try-catch to handle possible errors:

```typescript
async function loadConfig(): Promise<AppConfig | null> {
    try {
        const content = await puppet.fs.readFileAsText('config.json');
        const config = JSON.parse(content) as AppConfig;
        
        // Validate config
        if (isValidConfig(config)) {
            return config;
        }
        
        return getDefaultConfig();
    } catch (error) {
        puppet.log.error('Failed to load config:', error);
        return getDefaultConfig();
    }
}
```

### 3. Environment compatibility

Write code that works in different environments:

```typescript
async function saveData(data: any) {
    try {
        await puppet.fs.writeTextToFile('data.json', JSON.stringify(data));
        puppet.log.info('Data saved successfully');
    } catch (error) {
        // May fail in development environment
        if (isPuppetEnvironment()) {
            throw error;
        } else {
            console.warn('Failed to save data (development environment):', error);
        }
    }
}
```

### 4. Type assertions

Use type assertions carefully, prefer type guards:

```typescript
// Good: Use type guards
if (isWindowInfo(info)) {
    console.log(info.title);
}

// Avoid: Overuse type assertions
const info = someValue as WindowInfo; // May not be safe
```

## Common Questions

### Q: TypeScript error "Cannot find name 'puppet'"

A: Make sure you have correctly imported type definitions:

```typescript
/// <reference types="@puppet-framework/types" />
```

Or add in `tsconfig.json`:

```json
{
  "compilerOptions": {
    "types": ["@puppet-framework/types"]
  }
}
```

### Q: Why does calling API in browser have no actual effect?

A: In browser environment, puppet API uses mock implementation and only outputs logs. In Puppet Framework environment, there will be actual effects.

### Q: How to determine current runtime environment?

A: Use provided utility functions:

```typescript
import { isPuppetEnvironment } from '@puppet-framework/types';

if (isPuppetEnvironment()) {
    console.log('Running in Puppet Framework environment');
}
```

### Q: What to do if transparent window background doesn't work?

A: Make sure you set background to transparent on both `html` and `body`:

```css
html {
    background: transparent;
}

body {
    background: transparent;
}
```

Then call:

```typescript
await puppet.window.setTransparent(true);
await puppet.window.setTransparentColor('#000000');
```

## Related Links

- [Complete API Documentation](/en/api/)
- [Getting Started](/en/guide/getting-started.html)
- [Best Practices](/en/guide/best-practices.html)
- [Testing Documentation](https://github.com/bil-fis/puppet/blob/main/puppet-node/README.md#testing)