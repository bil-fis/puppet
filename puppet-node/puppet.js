/**
 * Puppet Framework Runtime
 * 
 * This module provides environment detection and mock implementations
 * for development in browser/Node.js environments.
 */

(function (global) {
    'use strict';

    // ============================================================================
    // Environment Detection
    // ============================================================================

    const isBrowser = typeof window !== 'undefined' && typeof window.document !== 'undefined';
    const isNodeJS = typeof process !== 'undefined' && process.versions && process.versions.node;
    const isPuppetFramework = isBrowser && typeof window.chrome !== 'undefined' && typeof window.chrome.webview !== 'undefined';

    // ============================================================================
    // Mock Implementations
    // ============================================================================

    function createMockWindowController() {
        return {
            async setBorderless(value) {
                console.log('[MOCK] setBorderless:', value);
                return Promise.resolve();
            },
            async setDraggable(value) {
                console.log('[MOCK] setDraggable:', value);
                return Promise.resolve();
            },
            async setResizable(value) {
                console.log('[MOCK] setResizable:', value);
                return Promise.resolve();
            },
            async setTransparent(value) {
                console.log('[MOCK] setTransparent:', value);
                return Promise.resolve();
            },
            async setOpacity(value) {
                console.log('[MOCK] setOpacity:', value);
                return Promise.resolve();
            },
            async setMouseThroughTransparency(value) {
                console.log('[MOCK] setMouseThroughTransparency:', value);
                return Promise.resolve();
            },
            async setMouseThrough(value) {
                console.log('[MOCK] setMouseThrough:', value);
                return Promise.resolve();
            },
            async setTransparentColor(color) {
                console.log('[MOCK] setTransparentColor:', color);
                return Promise.resolve();
            },
            async setTopmost(value) {
                console.log('[MOCK] setTopmost:', value);
                return Promise.resolve();
            },
            async moveWindow(x, y) {
                console.log('[MOCK] moveWindow:', x, y);
                return Promise.resolve();
            },
            async resizeWindow(width, height) {
                console.log('[MOCK] resizeWindow:', width, height);
                return Promise.resolve();
            },
            async centerWindow() {
                console.log('[MOCK] centerWindow');
                return Promise.resolve();
            },
            async showInTaskbar(value) {
                console.log('[MOCK] showInTaskbar:', value);
                return Promise.resolve();
            },
            async mountMovableElement(elementId) {
                console.log('[MOCK] mountMovableElement:', elementId);
                return Promise.resolve();
            },
            async unmountMovableElement(elementId) {
                console.log('[MOCK] unmountMovableElement:', elementId);
                return Promise.resolve();
            }
        };
    }

    function createMockApplicationController() {
        return {
            async close() {
                console.log('[MOCK] close');
                return Promise.resolve();
            },
            async restart() {
                console.log('[MOCK] restart');
                return Promise.resolve();
            },
            async getWindowInfo() {
                console.log('[MOCK] getWindowInfo');
                return Promise.resolve({
                    handle: 0,
                    title: 'Mock Window',
                    className: 'MockWindow',
                    isVisible: true,
                    isMinimized: false,
                    isMaximized: false,
                    width: 800,
                    height: 600,
                    x: 100,
                    y: 100
                });
            },
            async execute(command) {
                console.log('[MOCK] execute:', command);
                return Promise.resolve();
            },
            async setConfig(key, value) {
                console.log('[MOCK] setConfig:', key, value);
                return Promise.resolve();
            },
            async getConfig(key) {
                console.log('[MOCK] getConfig:', key);
                return Promise.resolve(null);
            },
            async getAssemblyDirectory() {
                console.log('[MOCK] getAssemblyDirectory');
                return Promise.resolve('.');
            },
            async getAppDataDirectory() {
                console.log('[MOCK] getAppDataDirectory');
                return Promise.resolve('.');
            },
            async getCurrentUser() {
                console.log('[MOCK] getCurrentUser');
                return Promise.resolve({
                    name: 'Mock User',
                    domain: 'MockDomain',
                    homeDirectory: '/mock/user'
                });
            }
        };
    }

    function createMockFileSystemController() {
        return {
            async openFileDialog(filter, multiSelect) {
                console.log('[MOCK] openFileDialog:', filter, multiSelect);
                return Promise.resolve([]);
            },
            async openFolderDialog() {
                console.log('[MOCK] openFolderDialog');
                return Promise.resolve('');
            },
            async readFileAsByte(path) {
                console.log('[MOCK] readFileAsByte:', path);
                return Promise.resolve('');
            },
            async readFileAsJson(path) {
                console.log('[MOCK] readFileAsJson:', path);
                return Promise.resolve({});
            },
            async writeByteToFile(path, data) {
                console.log('[MOCK] writeByteToFile:', path);
                return Promise.resolve();
            },
            async writeTextToFile(path, text) {
                console.log('[MOCK] writeTextToFile:', path);
                return Promise.resolve();
            },
            async appendByteToFile(path, data) {
                console.log('[MOCK] appendByteToFile:', path);
                return Promise.resolve();
            },
            async appendTextToFile(path, text) {
                console.log('[MOCK] appendTextToFile:', path);
                return Promise.resolve();
            },
            async exists(path) {
                console.log('[MOCK] exists:', path);
                return Promise.resolve(false);
            },
            async delete(path) {
                console.log('[MOCK] delete:', path);
                return Promise.resolve();
            }
        };
    }

    function createMockLogController() {
        return {
            info(message) {
                console.log('[INFO]', message);
            },
            warn(message) {
                console.warn('[WARN]', message);
            },
            error(message) {
                console.error('[ERROR]', message);
            }
        };
    }

    function createMockSystemController() {
        return {
            async getSystemInfo() {
                console.log('[MOCK] getSystemInfo');
                return Promise.resolve({
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
                });
            },
            async takeScreenShot() {
                console.log('[MOCK] takeScreenShot');
                return Promise.resolve('');
            },
            async getDesktopWallpaper() {
                console.log('[MOCK] getDesktopWallpaper');
                return Promise.resolve('');
            },
            async sendKey(...keys) {
                console.log('[MOCK] sendKey:', ...keys);
                return Promise.resolve();
            },
            async sendMouseClick(x, y, button) {
                console.log('[MOCK] sendMouseClick:', x, y, button);
                return Promise.resolve();
            },
            async getMousePosition() {
                console.log('[MOCK] getMousePosition');
                return Promise.resolve({ x: 0, y: 0 });
            }
        };
    }

    function createMockTrayController() {
        return {
            async setTray(name) {
                console.log('[MOCK] setTray:', name);
                return Promise.resolve();
            },
            async setMenu(items) {
                console.log('[MOCK] setMenu:', items);
                return Promise.resolve();
            },
            async showBalloon(title, content, timeout, icon) {
                console.log('[MOCK] showBalloon:', title, content, timeout, icon);
                return Promise.resolve();
            },
            async onClick(callback) {
                console.log('[MOCK] onClick registered');
                return Promise.resolve();
            },
            async onDoubleClick(callback) {
                console.log('[MOCK] onDoubleClick registered');
                return Promise.resolve();
            },
            async hide() {
                console.log('[MOCK] hide');
                return Promise.resolve();
            },
            async show() {
                console.log('[MOCK] show');
                return Promise.resolve();
            },
            async setIcon(iconPath) {
                console.log('[MOCK] setIcon:', iconPath);
                return Promise.resolve();
            }
        };
    }

    function createMockEventController() {
        return {
            async addEventListener(eventName, callback) {
                console.log('[MOCK] addEventListener:', eventName);
                return Promise.resolve('mock-listener-id');
            },
            async removeEventListener(eventName, listenerId) {
                console.log('[MOCK] removeEventListener:', eventName, listenerId);
                return Promise.resolve();
            }
        };
    }

    function createMockDeviceController() {
        return {
            async getDevice(deviceId) {
                console.log('[MOCK] getDevice:', deviceId);
                return Promise.resolve({
                    DeviceId: deviceId,
                    DeviceType: 0,
                    DeviceName: 'Mock Device',
                    Status: 1,
                    TotalSize: 0,
                    FreeSpace: 0,
                    UsedSpace: 0
                });
            },
            async getDevices(deviceType) {
                console.log('[MOCK] getDevices:', deviceType);
                return Promise.resolve([]);
            }
        };
    }

    // ============================================================================
    // Create Mock Namespace
    // ============================================================================

    function createMockPuppetNamespace() {
        return {
            window: createMockWindowController(),
            application: createMockApplicationController(),
            fs: createMockFileSystemController(),
            log: createMockLogController(),
            system: createMockSystemController(),
            tray: createMockTrayController(),
            events: createMockEventController(),
            device: createMockDeviceController(),
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
            },
            status: {
                ok: 1,
                error: 2,
                degraded: 3,
                ready: 100,
                unknown: 0
            }
        };
    }

    // ============================================================================
    // Initialize Puppet Namespace
    // ============================================================================

    function initializePuppet() {
        if (isPuppetFramework) {
            // In Puppet Framework environment, use the real puppet namespace
            // injected by the C# application
            console.log('[Puppet] Running in Puppet Framework environment');
            return global.puppet;
        } else {
            // In browser or Node.js environment, use mock implementations
            console.log('[Puppet] Running in', isBrowser ? 'browser' : 'Node.js', 'environment with mock implementation');
            const mockPuppet = createMockPuppetNamespace();
            
            // Make puppet available globally
            if (isBrowser) {
                global.puppet = mockPuppet;
            }
            
            return mockPuppet;
        }
    }

    // ============================================================================
    // Export
    // ============================================================================

    const puppet = initializePuppet();

    // Export for Node.js
    if (isNodeJS) {
        module.exports = puppet;
    }

    // Export for ES modules
    if (typeof module !== 'undefined' && module.exports) {
        global.puppet = module.exports;
    }

})(typeof window !== 'undefined' ? window : global);