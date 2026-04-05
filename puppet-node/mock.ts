/**
 * Puppet Framework Mock Implementation
 * 
 * This module provides comprehensive mock implementations for testing
 * and development in browser/Node.js environments.
 * 
 * @author Puppet Framework Team
 * @version 1.0.0
 */

import type {
    WindowController,
    ApplicationController,
    FileSystemController,
    LogController,
    SystemController,
    TrayController,
    EventController,
    DeviceController,
    PuppetNamespace,
    WindowInfo,
    UserInfo,
    SystemInfo,
    MousePosition,
    DeviceInfo,
    DeviceTypes,
    DeviceStatuses
} from './puppet';

// ============================================================================
// Mock State Management
// ============================================================================

interface MockState {
    window: {
        borderless: boolean;
        draggable: boolean;
        resizable: boolean;
        transparent: boolean;
        opacity: number;
        mouseThroughTransparency: boolean;
        mouseThrough: boolean;
        transparentColor: string;
        topmost: boolean;
        x: number;
        y: number;
        width: number;
        height: number;
        inTaskbar: boolean;
        movableElements: Set<string>;
    };
    application: {
        config: Map<string, any>;
        assemblyDirectory: string;
        appDataDirectory: string;
        user: UserInfo;
    };
    filesystem: {
        files: Map<string, string>;
        jsonFiles: Map<string, object>;
    };
    system: {
        systemInfo: SystemInfo;
        mousePosition: MousePosition;
    };
    tray: {
        name: string;
        icon: string;
        visible: boolean;
        menu: Array<{ Text: string; Command: string }>;
        onClickCallback?: (command: string) => void;
        onDoubleClickCallback?: () => void;
    };
    events: {
        listeners: Map<string, Map<string, (event: any) => void>>;
        listenerCounter: number;
    };
    device: {
        devices: Map<string, DeviceInfo>;
    };
}

const mockState: MockState = {
    window: {
        borderless: false,
        draggable: false,
        resizable: true,
        transparent: false,
        opacity: 1.0,
        mouseThroughTransparency: false,
        mouseThrough: false,
        transparentColor: '#000000',
        topmost: false,
        x: 100,
        y: 100,
        width: 800,
        height: 600,
        inTaskbar: true,
        movableElements: new Set()
    },
    application: {
        config: new Map([
            ['theme', 'light'],
            ['language', 'zh-CN'],
            ['fontSize', 14]
        ]),
        assemblyDirectory: '/mock/assembly',
        appDataDirectory: '/mock/appdata',
        user: {
            name: 'Mock User',
            domain: 'MockDomain',
            homeDirectory: '/mock/user'
        }
    },
    filesystem: {
        files: new Map([
            ['test.txt', 'Hello World'],
            ['config.json', JSON.stringify({ theme: 'light' }, null, 2)]
        ]),
        jsonFiles: new Map([
            ['config.json', { theme: 'light', language: 'zh-CN' }]
        ])
    },
    system: {
        systemInfo: {
            osName: 'Mock OS',
            osVersion: '1.0.0',
            computerName: 'MockComputer',
            cpuModel: 'Mock CPU',
            cpuCores: 4,
            totalMemory: 8192,
            availableMemory: 4096,
            gpuModel: 'Mock GPU',
            screenWidth: 1920,
            screenHeight: 1080,
            is64Bit: true
        },
        mousePosition: { x: 0, y: 0 }
    },
    tray: {
        name: 'Mock Tray',
        icon: 'mock-icon.png',
        visible: true,
        menu: [],
        onClickCallback: undefined,
        onDoubleClickCallback: undefined
    },
    events: {
        listeners: new Map(),
        listenerCounter: 0
    },
    device: {
        devices: new Map([
            ['C:', {
                DeviceId: 'C:',
                DeviceType: 3, // localDisk
                DeviceName: 'Local Disk (C:)',
                Status: 1, // ok
                DriveLetter: 'C:',
                VolumeName: 'System',
                FileSystem: 'NTFS',
                TotalSize: 107374182400, // 100 GB
                FreeSpace: 53687091200, // 50 GB
                UsedSpace: 53687091200 // 50 GB
            }]
        ])
    }
};

// ============================================================================
// Mock State Accessors
// ============================================================================

/**
 * Get the mock state
 * 
 * @returns The current mock state
 */
export function getMockState(): MockState {
    return mockState;
}

/**
 * Reset the mock state to initial values
 */
export function resetMockState(): void {
    mockState.window = {
        borderless: false,
        draggable: false,
        resizable: true,
        transparent: false,
        opacity: 1.0,
        mouseThroughTransparency: false,
        mouseThrough: false,
        transparentColor: '#000000',
        topmost: false,
        x: 100,
        y: 100,
        width: 800,
        height: 600,
        inTaskbar: true,
        movableElements: new Set()
    };
    mockState.application.config = new Map([
        ['theme', 'light'],
        ['language', 'zh-CN'],
        ['fontSize', 14]
    ]);
    mockState.filesystem.files = new Map([
        ['test.txt', 'Hello World'],
        ['config.json', JSON.stringify({ theme: 'light' }, null, 2)]
    ]);
    mockState.filesystem.jsonFiles = new Map([
        ['config.json', { theme: 'light', language: 'zh-CN' }]
    ]);
    mockState.system.mousePosition = { x: 0, y: 0 };
    mockState.events.listeners = new Map();
    mockState.events.listenerCounter = 0;
    mockState.device.devices = new Map([
        ['C:', {
            DeviceId: 'C:',
            DeviceType: 3,
            DeviceName: 'Local Disk (C:)',
            Status: 1,
            DriveLetter: 'C:',
            VolumeName: 'System',
            FileSystem: 'NTFS',
            TotalSize: 107374182400,
            FreeSpace: 53687091200,
            UsedSpace: 53687091200
        }]
    ]);
}

// ============================================================================
// Mock Implementations
// ============================================================================

/**
 * Create a mock WindowController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock WindowController
 */
export function createMockWindowController(enableLogging = true): WindowController {
    return {
        async setBorderless(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setBorderless:', value);
            mockState.window.borderless = value;
        },
        async setDraggable(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setDraggable:', value);
            mockState.window.draggable = value;
        },
        async setResizable(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setResizable:', value);
            mockState.window.resizable = value;
        },
        async setTransparent(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setTransparent:', value);
            mockState.window.transparent = value;
        },
        async setOpacity(value: number): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setOpacity:', value);
            mockState.window.opacity = value;
        },
        async setMouseThroughTransparency(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setMouseThroughTransparency:', value);
            mockState.window.mouseThroughTransparency = value;
        },
        async setMouseThrough(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setMouseThrough:', value);
            mockState.window.mouseThrough = value;
        },
        async setTransparentColor(color: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setTransparentColor:', color);
            mockState.window.transparentColor = color;
        },
        async setTopmost(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.setTopmost:', value);
            mockState.window.topmost = value;
        },
        async moveWindow(x: number, y: number): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.moveWindow:', x, y);
            mockState.window.x = x;
            mockState.window.y = y;
        },
        async resizeWindow(width: number, height: number): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.resizeWindow:', width, height);
            mockState.window.width = width;
            mockState.window.height = height;
        },
        async centerWindow(): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.centerWindow');
            const screenW = mockState.system.systemInfo.screenWidth;
            const screenH = mockState.system.systemInfo.screenHeight;
            mockState.window.x = Math.floor((screenW - mockState.window.width) / 2);
            mockState.window.y = Math.floor((screenH - mockState.window.height) / 2);
        },
        async showInTaskbar(value: boolean): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.showInTaskbar:', value);
            mockState.window.inTaskbar = value;
        },
        async mountMovableElement(elementId: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.mountMovableElement:', elementId);
            mockState.window.movableElements.add(elementId);
        },
        async unmountMovableElement(elementId: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Window.unmountMovableElement:', elementId);
            mockState.window.movableElements.delete(elementId);
        }
    };
}

/**
 * Create a mock ApplicationController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock ApplicationController
 */
export function createMockApplicationController(enableLogging = true): ApplicationController {
    return {
        async close(): Promise<void> {
            if (enableLogging) console.log('[MOCK] Application.close');
            // In mock environment, we don't actually close
        },
        async restart(): Promise<void> {
            if (enableLogging) console.log('[MOCK] Application.restart');
            // In mock environment, we don't actually restart
        },
        async getWindowInfo(): Promise<WindowInfo> {
            if (enableLogging) console.log('[MOCK] Application.getWindowInfo');
            return {
                handle: 123456,
                title: 'Mock Window',
                className: 'MockWindowClass',
                isVisible: true,
                isMinimized: false,
                isMaximized: false,
                width: mockState.window.width,
                height: mockState.window.height,
                x: mockState.window.x,
                y: mockState.window.y
            };
        },
        async execute(command: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Application.execute:', command);
            // In mock environment, we don't actually execute
        },
        async setConfig(key: string, value: any): Promise<void> {
            if (enableLogging) console.log('[MOCK] Application.setConfig:', key, value);
            mockState.application.config.set(key, value);
        },
        async getConfig(key: string): Promise<any> {
            if (enableLogging) console.log('[MOCK] Application.getConfig:', key);
            return mockState.application.config.get(key) ?? null;
        },
        async getAssemblyDirectory(): Promise<string> {
            if (enableLogging) console.log('[MOCK] Application.getAssemblyDirectory');
            return mockState.application.assemblyDirectory;
        },
        async getAppDataDirectory(): Promise<string> {
            if (enableLogging) console.log('[MOCK] Application.getAppDataDirectory');
            return mockState.application.appDataDirectory;
        },
        async getCurrentUser(): Promise<UserInfo> {
            if (enableLogging) console.log('[MOCK] Application.getCurrentUser');
            return mockState.application.user;
        }
    };
}

/**
 * Create a mock FileSystemController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock FileSystemController
 */
export function createMockFileSystemController(enableLogging = true): FileSystemController {
    return {
        async openFileDialog(filter: string[], multiSelect: boolean): Promise<string[]> {
            if (enableLogging) console.log('[MOCK] FileSystem.openFileDialog:', filter, multiSelect);
            // Return empty array in mock environment
            return [];
        },
        async openFolderDialog(): Promise<string> {
            if (enableLogging) console.log('[MOCK] FileSystem.openFolderDialog');
            return '';
        },
        async readFileAsByte(path: string): Promise<string> {
            if (enableLogging) console.log('[MOCK] FileSystem.readFileAsByte:', path);
            return mockState.filesystem.files.get(path) ?? '';
        },
        async readFileAsJson(path: string): Promise<object> {
            if (enableLogging) console.log('[MOCK] FileSystem.readFileAsJson:', path);
            return mockState.filesystem.jsonFiles.get(path) ?? {};
        },
        async writeByteToFile(path: string, data: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] FileSystem.writeByteToFile:', path);
            mockState.filesystem.files.set(path, data);
        },
        async writeTextToFile(path: string, text: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] FileSystem.writeTextToFile:', path);
            mockState.filesystem.files.set(path, text);
            // Try to parse as JSON
            try {
                const json = JSON.parse(text);
                mockState.filesystem.jsonFiles.set(path, json);
            } catch {
                // Not JSON, ignore
            }
        },
        async appendByteToFile(path: string, data: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] FileSystem.appendByteToFile:', path);
            const existing = mockState.filesystem.files.get(path) ?? '';
            mockState.filesystem.files.set(path, existing + data);
        },
        async appendTextToFile(path: string, text: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] FileSystem.appendTextToFile:', path);
            const existing = mockState.filesystem.files.get(path) ?? '';
            const newText = existing + text;
            mockState.filesystem.files.set(path, newText);
            // Try to parse as JSON
            try {
                const json = JSON.parse(newText);
                mockState.filesystem.jsonFiles.set(path, json);
            } catch {
                // Not JSON, ignore
            }
        },
        async exists(path: string): Promise<boolean> {
            if (enableLogging) console.log('[MOCK] FileSystem.exists:', path);
            return mockState.filesystem.files.has(path);
        },
        async delete(path: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] FileSystem.delete:', path);
            mockState.filesystem.files.delete(path);
            mockState.filesystem.jsonFiles.delete(path);
        }
    };
}

/**
 * Create a mock LogController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock LogController
 */
export function createMockLogController(enableLogging = true): LogController {
    return {
        info(message: string): void {
            if (enableLogging) console.log('[INFO]', message);
        },
        warn(message: string): void {
            if (enableLogging) console.warn('[WARN]', message);
        },
        error(message: string): void {
            if (enableLogging) console.error('[ERROR]', message);
        }
    };
}

/**
 * Create a mock SystemController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock SystemController
 */
export function createMockSystemController(enableLogging = true): SystemController {
    return {
        async getSystemInfo(): Promise<SystemInfo> {
            if (enableLogging) console.log('[MOCK] System.getSystemInfo');
            return { ...mockState.system.systemInfo };
        },
        async takeScreenShot(): Promise<string> {
            if (enableLogging) console.log('[MOCK] System.takeScreenShot');
            // Return empty base64 string
            return '';
        },
        async getDesktopWallpaper(): Promise<string> {
            if (enableLogging) console.log('[MOCK] System.getDesktopWallpaper');
            return '';
        },
        async sendKey(...keys: string[]): Promise<void> {
            if (enableLogging) console.log('[MOCK] System.sendKey:', ...keys);
        },
        async sendMouseClick(x: number, y: number, button: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] System.sendMouseClick:', x, y, button);
        },
        async getMousePosition(): Promise<MousePosition> {
            if (enableLogging) console.log('[MOCK] System.getMousePosition');
            return { ...mockState.system.mousePosition };
        }
    };
}

/**
 * Create a mock TrayController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock TrayController
 */
export function createMockTrayController(enableLogging = true): TrayController {
    return {
        async setTray(name: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.setTray:', name);
            mockState.tray.name = name;
        },
        async setMenu(items: Array<{ Text: string; Command: string }>): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.setMenu:', items);
            mockState.tray.menu = items;
        },
        async showBalloon(title: string, content: string, timeout: number, icon: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.showBalloon:', title, content, timeout, icon);
        },
        async onClick(callback: (command: string) => void): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.onClick registered');
            mockState.tray.onClickCallback = callback;
        },
        async onDoubleClick(callback: () => void): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.onDoubleClick registered');
            mockState.tray.onDoubleClickCallback = callback;
        },
        async hide(): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.hide');
            mockState.tray.visible = false;
        },
        async show(): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.show');
            mockState.tray.visible = true;
        },
        async setIcon(iconPath: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Tray.setIcon:', iconPath);
            mockState.tray.icon = iconPath;
        }
    };
}

/**
 * Create a mock EventController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock EventController
 */
export function createMockEventController(enableLogging = true): EventController {
    return {
        async addEventListener(eventName: string, callback: (event: any) => void): Promise<string> {
            if (enableLogging) console.log('[MOCK] Events.addEventListener:', eventName);
            const listenerId = `listener-${mockState.events.listenerCounter++}`;
            
            if (!mockState.events.listeners.has(eventName)) {
                mockState.events.listeners.set(eventName, new Map());
            }
            
            mockState.events.listeners.get(eventName)!.set(listenerId, callback);
            return listenerId;
        },
        async removeEventListener(eventName: string, listenerId: string): Promise<void> {
            if (enableLogging) console.log('[MOCK] Events.removeEventListener:', eventName, listenerId);
            const eventListeners = mockState.events.listeners.get(eventName);
            if (eventListeners) {
                eventListeners.delete(listenerId);
            }
        }
    };
}

/**
 * Create a mock DeviceController
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock DeviceController
 */
export function createMockDeviceController(enableLogging = true): DeviceController {
    return {
        async getDevice(deviceId: string): Promise<DeviceInfo> {
            if (enableLogging) console.log('[MOCK] Device.getDevice:', deviceId);
            return mockState.device.devices.get(deviceId) ?? {
                DeviceId: deviceId,
                DeviceType: 0,
                DeviceName: 'Unknown Device',
                Status: 0,
                TotalSize: 0,
                FreeSpace: 0,
                UsedSpace: 0
            };
        },
        async getDevices(deviceType: number): Promise<DeviceInfo[]> {
            if (enableLogging) console.log('[MOCK] Device.getDevices:', deviceType);
            return Array.from(mockState.device.devices.values()).filter(
                device => device.DeviceType === deviceType
            );
        }
    };
}

/**
 * Create a complete mock PuppetNamespace
 * 
 * @param enableLogging - Enable console logging for method calls
 * @returns Mock PuppetNamespace
 */
export function createMockPuppetNamespace(enableLogging = true): PuppetNamespace {
    return {
        window: createMockWindowController(enableLogging),
        application: createMockApplicationController(enableLogging),
        fs: createMockFileSystemController(enableLogging),
        log: createMockLogController(enableLogging),
        system: createMockSystemController(enableLogging),
        tray: createMockTrayController(enableLogging),
        events: createMockEventController(enableLogging),
        device: createMockDeviceController(enableLogging),
        type: {
            unknown: 0,
            usbDisk: 101,
            localDisk: 3,
            removableDisk: 2,
            keyboard: 200,
            mouse: 201,
            networkAdapter: 202,
            monitor: 203,
            audio: 204
        } as DeviceTypes,
        status: {
            ok: 1,
            error: 2,
            degraded: 3,
            ready: 100,
            unknown: 0
        } as DeviceStatuses
    };
}

// ============================================================================
// Event Simulation
// ============================================================================

/**
 * Simulate an event being triggered
 * 
 * @param eventName - The name of the event to simulate
 * @param eventData - The event data to pass to listeners
 */
export function simulateEvent(eventName: string, eventData: any): void {
    const listeners = mockState.events.listeners.get(eventName);
    if (listeners) {
        listeners.forEach((callback) => {
            try {
                callback(eventData);
            } catch (error) {
                console.error(`[MOCK] Error in event listener for ${eventName}:`, error);
            }
        });
    }
}

/**
 * Simulate a mouse click on the tray icon
 * 
 * @param command - The command to simulate
 */
export function simulateTrayClick(command: string): void {
    if (mockState.tray.onClickCallback) {
        mockState.tray.onClickCallback(command);
    }
}

/**
 * Simulate a double click on the tray icon
 */
export function simulateTrayDoubleClick(): void {
    if (mockState.tray.onDoubleClickCallback) {
        mockState.tray.onDoubleClickCallback();
    }
}

// ============================================================================
// Default Export
// ============================================================================

/**
 * Default export for easy import
 */
export default createMockPuppetNamespace;