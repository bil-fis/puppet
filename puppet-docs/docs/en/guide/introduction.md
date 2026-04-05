---
title: Framework Introduction
permalink: /en/guide/introduction.html
createTime: 2026/03/28 14:52:37
---

# Framework Introduction

Puppet is an innovative Windows desktop application development framework that perfectly combines the flexibility of web technologies with the powerful functionality of native Windows applications. This chapter will详细介绍 Puppet framework's core features, design philosophy, and applicable scenarios.

## What is Puppet?

Puppet is a desktop application framework based on **Microsoft WebView2** that allows developers to build cross-platform desktop applications using web technologies such as HTML, CSS, and JavaScript. Unlike traditional Electron or NW.js, Puppet is designed specifically for Windows systems, providing deeper system integration and a more lightweight solution.

### Design Philosophy

Puppet's design follows these core principles:

1. **Web First**: Fully leverage modern web technology stack to reduce development barriers
2. **Native Integration**: Provide complete access to Windows system functionality
3. **Lightweight and Efficient**: Based on .NET and WebView2, with small resource footprint and excellent performance
4. **Secure and Reliable**: Built-in multi-layer security mechanisms to protect system and user data
5. **Simple and Easy to Use**: Clean API design for quick onboarding

## Core Features

### 1. Web Technology Stack

Puppet uses web technologies to build user interfaces, which means:

- **Familiar Development Experience**: Develop using HTML, CSS, JavaScript
- **Rich Ecosystem**: Directly use npm packages, frontend frameworks (Vue, React, etc.)
- **Modern UI Design**: Support for CSS3, Flexbox, Grid, and other modern layout technologies
- **Responsive Design**: Easy adaptation to different screen sizes

::: tip Tip
Puppet supports all modern browser features because it's based on the Edge WebView2 rendering engine.
:::

### 2. Native System Integration

Puppet provides deep access to Windows system functionality:

- **Window Management**: Borderless windows, transparency effects, window dragging, click-through
- **File System**: File read/write, folder selection, path operations
- **System Information**: Get CPU, memory, GPU, operating system, and other information
- **Input Simulation**: Simulate keyboard and mouse operations
- **Tray Icons**: System tray integration, balloon notifications, custom menus
- **External Programs**: Execute system commands, open files, launch applications

### 3. Event-Driven Architecture

Puppet provides a powerful event system supporting:

- **USB Device Events**: USB device plug-in/unplug monitoring
- **Disk Events**: Disk mount and unmount monitoring
- **Window Events**: Window focus, maximize, move, resize, etc.
- **Power Events**: Power state change monitoring
- **Custom Events**: Support for custom event listening and handling

::: info Technical Details
The event system is implemented using Windows Management Instrumentation (WMI), providing stable and reliable device monitoring capabilities.
:::

### 4. Security Mechanisms

Puppet has built-in multi-layer security protection:

- **Communication Verification**: Verify all local requests using randomly generated runtime keys
- **File System Protection**: Automatically block access to Windows system folders
- **Permission Confirmation**: Show user confirmation dialog before dangerous operations
- **Path Normalization**: Prevent path traversal attacks
- **PUP Encryption**: Support password-encrypted PUP file format
- **Digital Signature** (V1.2): Support X.509 certificate-based digital signatures to ensure application integrity and source trustworthiness
- **Database Signature** (V1.2): Support database signature verification to prevent data tampering

### 5. Unique PUP Packaging Format

Puppet provides a unique `.pup` file format:

- **Custom Format**: Exclusive format combining ZIP and AES encryption
- **Password Protection**: Support AES-256 encrypted source code
- **Single File Distribution**: All resources packaged into one file for easy distribution
- **Fast Loading**: Optimized file structure and loading mechanism
- **Multi-Version Support**:
  - **V1.0**: Basic features, supports encryption
  - **V1.1**: Supports startup scripts
  - **V1.2**: Supports digital signatures and certificate verification

## Technical Architecture

### Overall Architecture

```
┌─────────────────────────────────────────┐
│         Web Application Layer (HTML/CSS/JS)        │
├─────────────────────────────────────────┤
│         JavaScript API (puppet.*)       │
├─────────────────────────────────────────┤
│         COM Bridge Layer                      │
├─────────────────────────────────────────┤
│         C# Controller Layer                      │
│  ┌──────────────────────────────────┐  │
│  │  WindowController                │  │
│  │  FileSystemController            │  │
│  │  ApplicationController            │  │
│  │  SystemController                │  │
│  │  EventController                 │  │
│  │  LogController                   │  │
│  │  TrayController                  │  │
│  └──────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         Windows Forms + WebView2        │
├─────────────────────────────────────────┤
│         Windows API / System Services          │
└─────────────────────────────────────────┘
```

### Data Flow

```
User Operation → JavaScript Code → COM Bridge → C# Controller → Windows API / System Services
```

### Core Components

1. **WebView2 Control**: Renders web content and provides browser functionality
2. **COM Bridge**: Implements bidirectional communication between JavaScript and C#
3. **HTTP Server**: Provides local file services and API endpoints
4. **Controllers**: Encapsulate various system function APIs
5. **PUP Server**: Parses and serves PUP file content

## Comparison with Other Frameworks

### vs Electron

| Feature | Puppet | Electron |
|---------|--------|----------|
| Base Architecture | .NET + WebView2 | Node.js + Chromium |
| Application Size | ~50MB | ~150MB+ |
| Memory Usage | Lower | Higher |
| Windows Integration | Deep | Medium |
| Cross-Platform | Windows Only | Cross-Platform |
| Learning Curve | Gentle | Medium |
| Performance | Excellent | Good |

### vs C# WinForms/WPF

| Feature | Puppet | WinForms/WPF |
|---------|--------|--------------|
| Development Language | JavaScript/C# | C# |
| UI Technology | HTML/CSS | XAML/Designer |
| Development Speed | Fast | Medium |
| Flexibility | High | Medium |
| Performance | Excellent | Excellent |
| Windows Integration | Deep | Deep |

## Applicable Scenarios

Puppet is particularly suitable for the following application scenarios:

### 1. Device Management Tools

- USB device manager
- Disk management tools
- Peripheral monitoring applications

### 2. System Monitoring Tools

- System resource monitoring
- Process manager
- Network monitoring tools

### 3. Tray Applications

- System tray tools
- Background service monitoring
- Quick toolbar

### 4. Transparent Window Applications

- Desktop widgets
- Floating tools
- Transparent overlays

### 5. Rapid Prototype Development

- Proof of concept applications
- MVP development
- Internal tools

### 6. Modern Desktop Applications

- Desktop applications requiring web technologies
- Web applications requiring system functionality
- Cross-device applications

## Inapplicable Scenarios

The following scenarios may not be suitable for using Puppet:

- Applications requiring cross-platform support (consider Electron)
- Purely compute-intensive applications (consider native development)
- Games requiring GPU acceleration (consider Unity/Unreal)
- System tools that demand extreme performance (consider C++)

## Technology Stack

The Puppet Framework is built on the following technologies:

### Core Technologies

- **.NET 10.0**: Core runtime framework
- **Windows Forms**: Desktop UI framework
- **Microsoft WebView2**: Web rendering engine (based on Edge)

### Third-Party Libraries

- **Newtonsoft.Json**: JSON serialization/deserialization
- **SharpZipLib**: ZIP file operations (for PUP format)
- **System.Management**: WMI device monitoring

### Development Tools

- **Visual Studio**: Recommended development IDE
- **MSBuild**: Build tool
- **NuGet**: Package manager

## Limitations and Constraints

### Platform Limitations

- **Operating System**: Only supports Windows 10 and above
- **Architecture**: Supports x64 and x86 architectures
- **WebView2**: Requires WebView2 runtime installation

### Functional Limitations

- **Cross-Platform**: Does not support macOS and Linux
- **Mobile**: Does not support mobile platforms
- **GPU Acceleration**: Limited GPU acceleration support
- **Native Modules**: Does not support Node.js native modules

### Performance Considerations

- **Heavy Computation**: Intensive computation is recommended to use C# extensions
- **Memory Management**: Be aware of JavaScript memory leaks
- **DOM Operations**: Large amounts of DOM operations may affect performance

## Learning Path

It is recommended to learn Puppet in the following order:

1. **Basic Getting Started**
   - Read [Getting Started](./getting-started.md)
   - Create your first application
   - Understand basic concepts

2. **Core Features**
   - Learn [Window Control](../api/window.md)
   - Master [File System](../api/fs.md)
   - Understand [System Functions](../api/system.md)

3. **Advanced Features**
   - Explore [Event System](../api/events.md)
   - Use [Device Monitoring](../api/device.md)
   - Implement [Tray Icons](../api/tray.md)

4. **Advanced Topics**
   - Understand [Architecture Design](./architecture.md)
   - Learn [Security Mechanisms](./security.md)
   - Follow [Best Practices](./best-practices.md)

## Related Resources

### Official Resources

- [GitHub Repository](#): Source code and issue tracking
- [API Documentation](../api/): Complete API reference
- [Example Code](#): Actual application examples

### External Resources

- [Microsoft WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/): WebView2 official documentation
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/): .NET framework documentation
- [Windows API Documentation](https://learn.microsoft.com/en-us/windows/win32/api/): Windows API reference

## Next Steps

Now that you have a basic understanding of the Puppet Framework, it is recommended to:

1. Read [Getting Started](./getting-started.md) to create your first application
2. Learn [Architecture Design](./architecture.md) to deeply understand framework principles
3. View [API Documentation](../api/) to start actual development
4. Reference [Best Practices](./best-practices.md) to improve code quality