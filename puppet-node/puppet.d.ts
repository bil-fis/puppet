/**
 * Puppet Framework API Type Definitions
 * 
 * Complete type definitions for all Puppet Framework APIs
 */

// ============================================================================
// Window API
// ============================================================================

interface WindowController {
    /**
     * Set window borderless mode
     * @param value - true for borderless, false for bordered
     */
    setBorderless(value: boolean): Promise<void>;

    /**
     * Set window draggable
     * @param value - true for draggable, false for fixed
     */
    setDraggable(value: boolean): Promise<void>;

    /**
     * Set window resizable
     * @param value - true for resizable, false for fixed size
     */
    setResizable(value: boolean): Promise<void>;

    /**
     * Set window transparency support
     * @param value - true to enable transparency
     */
    setTransparent(value: boolean): Promise<void>;

    /**
     * Set window opacity (0.0 - 1.0)
     * @param value - Opacity value from 0.0 (fully transparent) to 1.0 (fully opaque)
     */
    setOpacity(value: number): Promise<void>;

    /**
     * Set mouse click through for transparent areas
     * @param value - true to enable click through
     */
    setMouseThroughTransparency(value: boolean): Promise<void>;

    /**
     * Set mouse click through for entire window
     * @param value - true to enable click through
     */
    setMouseThrough(value: boolean): Promise<void>;

    /**
     * Set transparent color
     * @param color - Color in hex format #RRGGBB
     */
    setTransparentColor(color: string): Promise<void>;

    /**
     * Set window always on top
     * @param value - true to set window on top
     */
    setTopmost(value: boolean): Promise<void>;

    /**
     * Move window to specific position
     * @param x - X coordinate
     * @param y - Y coordinate
     */
    moveWindow(x: number, y: number): Promise<void>;

    /**
     * Resize window
     * @param width - Window width in pixels
     * @param height - Window height in pixels
     */
    resizeWindow(width: number, height: number): Promise<void>;

    /**
     * Center window on screen
     */
    centerWindow(): Promise<void>;

    /**
     * Show/hide window in taskbar
     * @param value - true to show, false to hide
     */
    showInTaskbar(value: boolean): Promise<void>;

    /**
     * Mount an HTML element as movable
     * @param elementId - ID of the element to mount
     */
    mountMovableElement(elementId: string): Promise<void>;

    /**
     * Unmount a movable element
     * @param elementId - ID of the element to unmount
     */
    unmountMovableElement(elementId: string): Promise<void>;
}

// ============================================================================
// Application API
// ============================================================================

interface ApplicationController {
    /**
     * Close the application
     */
    close(): Promise<void>;

    /**
     * Restart the application
     */
    restart(): Promise<void>;

    /**
     * Get window information
     */
    getWindowInfo(): Promise<WindowInfo>;

    /**
     * Execute external program or command
     * @param command - Command or file path to execute
     */
    execute(command: string): Promise<void>;

    /**
     * Set configuration value
     * @param key - Configuration key
     * @param value - Configuration value
     */
    setConfig(key: string, value: any): Promise<void>;

    /**
     * Get configuration value
     * @param key - Configuration key
     */
    getConfig(key: string): Promise<any>;

    /**
     * Get assembly directory (app installation directory)
     */
    getAssemblyDirectory(): Promise<string>;

    /**
     * Get app data directory
     */
    getAppDataDirectory(): Promise<string>;

    /**
     * Get current user information
     */
    getCurrentUser(): Promise<UserInfo>;
}

// ============================================================================
// File System API
// ============================================================================

interface FileSystemController {
    /**
     * Open file selection dialog
     * @param filter - File filter array [description, pattern]
     * @param multiSelect - Allow multiple file selection
     */
    openFileDialog(filter: string[], multiSelect: boolean): Promise<string[]>;

    /**
     * Open folder selection dialog
     */
    openFolderDialog(): Promise<string>;

    /**
     * Read file as Base64 binary data
     * @param path - File path
     */
    readFileAsByte(path: string): Promise<string>;

    /**
     * Read file as JSON object
     * @param path - File path
     */
    readFileAsJson(path: string): Promise<object>;

    /**
     * Write Base64 binary data to file
     * @param path - File path
     * @param data - Base64 encoded data
     */
    writeByteToFile(path: string, data: string): Promise<void>;

    /**
     * Write text content to file
     * @param path - File path
     * @param text - Text content
     */
    writeTextToFile(path: string, text: string): Promise<void>;

    /**
     * Append Base64 binary data to file
     * @param path - File path
     * @param data - Base64 encoded data
     */
    appendByteToFile(path: string, data: string): Promise<void>;

    /**
     * Append text content to file
     * @param path - File path
     * @param text - Text content
     */
    appendTextToFile(path: string, text: string): Promise<void>;

    /**
     * Check if file or directory exists
     * @param path - Path to check
     */
    exists(path: string): Promise<boolean>;

    /**
     * Delete file or directory
     * @param path - Path to delete
     */
    delete(path: string): Promise<void>;
}

// ============================================================================
// Log API
// ============================================================================

interface LogController {
    /**
     * Log info message
     * @param message - Message to log
     */
    info(message: string): void;

    /**
     * Log warning message
     * @param message - Message to log
     */
    warn(message: string): void;

    /**
     * Log error message
     * @param message - Message to log
     */
    error(message: string): void;
}

// ============================================================================
// System API
// ============================================================================

interface SystemController {
    /**
     * Get system information
     */
    getSystemInfo(): Promise<SystemInfo>;

    /**
     * Take screenshot
     */
    takeScreenShot(): Promise<string>;

    /**
     * Get desktop wallpaper
     */
    getDesktopWallpaper(): Promise<string>;

    /**
     * Simulate key press
     * @param keys - Keys to press
     */
    sendKey(...keys: string[]): Promise<void>;

    /**
     * Simulate mouse click
     * @param x - X coordinate
     * @param y - Y coordinate
     * @param button - Mouse button ('left', 'right', 'middle')
     */
    sendMouseClick(x: number, y: number, button: string): Promise<void>;

    /**
     * Get current mouse position
     */
    getMousePosition(): Promise<MousePosition>;
}

// ============================================================================
// Tray API
// ============================================================================

interface TrayController {
    /**
     * Create or update tray icon
     * @param name - Tray icon name (tooltip)
     */
    setTray(name: string): Promise<void>;

    /**
     * Set tray menu
     * @param items - Menu items
     */
    setMenu(items: MenuItem[]): Promise<void>;

    /**
     * Show balloon notification
     * @param title - Notification title
     * @param content - Notification content
     * @param timeout - Display duration in milliseconds
     * @param icon - Icon type ('Info', 'Warning', 'Error', 'None')
     */
    showBalloon(title: string, content: string, timeout: number, icon: string): Promise<void>;

    /**
     * Set click event handler
     * @param callback - Click callback function
     */
    onClick(callback: (command: string) => void): Promise<void>;

    /**
     * Set double click event handler
     * @param callback - Double click callback function
     */
    onDoubleClick(callback: () => void): Promise<void>;

    /**
     * Hide tray icon
     */
    hide(): Promise<void>;

    /**
     * Show tray icon
     */
    show(): Promise<void>;

    /**
     * Set custom tray icon
     * @param iconPath - Path to icon file
     */
    setIcon(iconPath: string): Promise<void>;
}

// ============================================================================
// Events API
// ============================================================================

interface EventController {
    /**
     * Add event listener
     * @param eventName - Event name
     * @param callback - Event callback function
     */
    addEventListener(eventName: string, callback: (event: any) => void): Promise<string>;

    /**
     * Remove event listener
     * @param eventName - Event name
     * @param listenerId - Listener ID returned by addEventListener
     */
    removeEventListener(eventName: string, listenerId: string): Promise<void>;
}

// ============================================================================
// Device API
// ============================================================================

interface DeviceController {
    /**
     * Get device information
     * @param deviceId - Device ID (e.g., drive letter 'C:')
     */
    getDevice(deviceId: string): Promise<DeviceInfo>;

    /**
     * Get devices by type
     * @param deviceType - Device type
     */
    getDevices(deviceType: number): Promise<DeviceInfo[]>;
}

// ============================================================================
// Device Types
// ============================================================================

interface DeviceTypes {
    unknown: number;
    usbDisk: number;
    localDisk: number;
    removableDisk: number;
    keyboard: number;
    mouse: number;
    networkAdapter: number;
    monitor: number;
    audio: number;
}

interface DeviceStatuses {
    ok: number;
    error: number;
    degraded: number;
    ready: number;
    unknown: number;
}

// ============================================================================
// Data Types
// ============================================================================

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

interface UserInfo {
    name: string;
    domain: string;
    homeDirectory: string;
}

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

interface MousePosition {
    x: number;
    y: number;
}

interface MenuItem {
    Text: string;
    Command: string;
}

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

// ============================================================================
// Main Puppet Namespace
// ============================================================================

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

// ============================================================================
// Export
// ============================================================================

export type {
    WindowController,
    ApplicationController,
    FileSystemController,
    LogController,
    SystemController,
    TrayController,
    EventController,
    DeviceController,
    WindowInfo,
    UserInfo,
    SystemInfo,
    MousePosition,
    MenuItem,
    DeviceInfo,
    DeviceTypes,
    DeviceStatuses,
    PuppetNamespace
};

export { PuppetNamespace as default };