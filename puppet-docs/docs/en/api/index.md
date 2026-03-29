---
title: API Overview
permalink: /en/api/index.html
createTime: 2026/03/28 15:01:52
---

# API Overview

Puppet Framework provides a complete set of JavaScript APIs that allow you to access Windows system features in your web applications. All APIs are accessed through the `puppet` global object.

## Namespaces

Puppet API is divided into the following namespaces by functionality:

```javascript
puppet.window     // Window control
puppet.application // Application control
puppet.fs         // File system
puppet.log        // Logging
puppet.system     // System functions
puppet.tray       // Tray icon
puppet.events     // Event system
puppet.device     // Device system
puppet.storage    // Persistent storage
```

## API Features

### Asynchronous Operations

Most API methods are asynchronous and return Promise objects.

```javascript
// Async call
await puppet.window.setBorderless(true);

// Using Promise
puppet.window.setBorderless(true)
    .then(() => console.log('Window set to borderless'))
    .catch(err => console.error('Operation failed:', err));
```

### Error Handling

All API methods may throw exceptions, it's recommended to use try-catch to catch them.

```javascript
try {
    await puppet.fs.readFileAsText('config.json');
} catch (error) {
    console.error('Failed to read file:', error.message);
    puppet.log.error('File read failed');
}
```

### Type Support

API methods support multiple data types:

- **String**: File paths, config values, etc.
- **Number**: Window position, size, opacity, etc.
- **Boolean**: Switch options, status flags, etc.
- **Object**: Config objects, event data, etc.
- **Array**: File lists, menu items, etc.

## Quick Start

### Window Control

```javascript
// Set borderless window
await puppet.window.setBorderless(true);

// Set window opacity
await puppet.window.setOpacity(0.9);

// Center window
await puppet.window.centerWindow();
```

### File Operations

```javascript
// Open file selection dialog
const files = await puppet.fs.openFileDialog(
    ['Text Files', '*.txt'],
    false
);

// Read file content
const content = await puppet.fs.readFileAsText(files[0]);

// Write file
await puppet.fs.writeTextToFile('output.txt', content);
```

### System Information

```javascript
// Get system information
const sysInfo = await puppet.system.getSystemInfo();
console.log('OS:', sysInfo.osName);
console.log('CPU:', sysInfo.cpuModel);
console.log('Memory:', sysInfo.totalMemory);

// Take screenshot
const screenshot = await puppet.system.takeScreenShot();
```

### Event Listening

```javascript
// Monitor USB device insertion
await puppet.events.addEventListener('usb-plug-in', (event) => {
    console.log('USB device inserted:', event.data);
    puppet.log.info('USB device inserted');
});

// Monitor window maximize
await puppet.events.addEventListener('window-maximize', () => {
    console.log('Window maximized');
});
```

## API Naming Conventions

### Method Naming

- **set**: Set properties or states
  - `setBorderless()`
  - `setOpacity()`
  - `setMenu()`

- **get**: Get properties or information
  - `getWindowInfo()`
  - `getSystemInfo()`
  - `getDevices()`

- **is**: Query status
  - `exists()`

- **open**: Open dialogs
  - `openFileDialog()`
  - `openFolderDialog()`

### Event Naming

Event names use kebab-case format:

- `usb-plug-in`: USB device inserted
- `usb-plug-out`: USB device removed
- `window-maximize`: Window maximized
- `disk-mount`: Disk mounted

## API Reference

### Window Control API

Provides window management functionality, including style control, position adjustment, transparency effects, etc.

- [Window Control API Documentation](./window.md)

### Application Control API

Provides application lifecycle management and external program execution functionality.

- [Application Control API Documentation](./application.md)

### File System API

Provides file and folder operations, including read, write, selection, delete, etc.

- [File System API Documentation](./fs.md)

### Logging API

Provides logging functionality for debugging and error tracking.

- [Logging API Documentation](./log.md)

### System API

Provides system information retrieval, input simulation, screenshot, etc.

- [System API Documentation](./system.md)

### Tray Icon API

Provides system tray icon management, including menus, notifications, etc.

- [Tray Icon API Documentation](./tray.md)

### Event System API

Provides event listening and handling, supports device and window events.

- [Event System API Documentation](./events.md)

### Device System API

Provides device query and monitoring functionality.

- [Device System API Documentation](./device.md)

### Persistent Storage API

Provides SQLite-based key-value storage for data persistence.

- [Persistent Storage API Documentation](./storage.md)

## Version Compatibility

Current API version: v1.2

### PUP Format Versions

Puppet Framework supports multiple PUP file formats:

- **V1.0**: Basic features, supports encryption
- **V1.1**: Supports startup scripts
- **V1.2**: Supports digital signatures and certificate verification

### V1.2 New Features

V1.2 adds the following security features on top of V1.1:

- **Digital Signatures**: X.509 certificate-based digital signatures
- **Signature Verification**: Application and database signature verification
- **Certificate Management**: Certificate fingerprint, issuer, validity period, etc.
- **Database Signatures**: Database content signature verification to prevent tampering

### Backward Compatibility

Puppet Framework commits to maintaining API backward compatibility while the major version number remains unchanged.

### Deprecated APIs

If an API is deprecated, it will be clearly marked in the documentation, and alternatives will be provided.

## Limitations and Constraints

### Permission Restrictions

- Cannot access Windows system directories
- Cannot execute certain system commands directly (requires user confirmation)

### Performance Limitations

- Reading and writing large files may affect performance
- Frequent event monitoring may consume resources

### Platform Limitations

- Only supports Windows 10 and above
- Some features require administrator permissions

## Security Considerations

### Input Validation

Always validate user input:

```javascript
// Validate file path
if (!path || typeof path !== 'string') {
    throw new Error('Invalid path');
}
```

### Error Handling

Always handle possible errors:

```javascript
try {
    await riskyOperation();
} catch (error) {
    puppet.log.error('Operation failed:', error.message);
    showError('Operation failed, please try again');
}
```

### Sensitive Data

Don't hardcode sensitive information in code:

```javascript
// Not recommended
const password = 'my-secret-password';

// Recommended
const password = await loadPasswordFromConfig();
```

## Related Resources

- [Microsoft WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/) - WebView2 official documentation
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/) - .NET framework documentation
- [JavaScript Asynchronous Programming](https://developer.mozilla.org/en-US/docs/Learn/JavaScript/Asynchronous) - Asynchronous programming guide

## Next Steps

We recommend learning the API in this order:

1. Understand [Window Control API](./window.md) to create custom windows
2. Learn [File System API](./fs.md) to perform file operations
3. Explore [Event System API](./events.md) to implement event handling
4. Use [System API](./system.md) to get system information
5. Reference [Tray Icon API](./tray.md) to create tray applications