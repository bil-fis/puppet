---
title: Framework Introduction
permalink: /en/guide/introduction.html
createTime: 2026/03/28 14:52:37
---

# Framework Introduction

Puppet is an innovative Windows desktop application development framework that perfectly combines the flexibility of web technologies with the powerful features of native Windows applications. This chapter will introduce the core features, design philosophy, and suitable use cases of Puppet Framework in detail.

## What is Puppet?

Puppet is a desktop application framework based on **Microsoft WebView2** that allows developers to build cross-platform desktop applications using web technologies like HTML, CSS, and JavaScript. Unlike traditional Electron or NW.js, Puppet is designed specifically for Windows, providing deeper system integration and a more lightweight solution.

### Design Philosophy

Puppet follows these core principles:

1. **Web First** - Leverage modern web technology stack, lower development barrier
2. **Native Integration** - Provide full access to Windows system capabilities
3. **Lightweight & Efficient** - Based on .NET and WebView2, low resource usage, excellent performance
4. **Secure & Reliable** - Built-in multi-layer security mechanisms to protect system and user data
5. **Simple & Easy** - Clean API design, quick to get started

## Key Features

### 1. Web Technology Stack

Puppet uses web technologies to build user interfaces, which means:

- **Familiar Development Experience** - Use HTML, CSS, JavaScript for development
- **Rich Ecosystem** - Direct use of npm packages, frontend frameworks (Vue, React, etc.)
- **Modern UI Design** - Support for CSS3, Flexbox, Grid and other modern layout technologies
- **Responsive Design** - Easy adaptation to different screen sizes

::: tip Tip
Puppet supports all modern browser features because it's based on Edge WebView2 rendering engine.
:::

### 2. Native System Integration

Puppet provides deep access to Windows system capabilities:

- **Window Management** - Borderless windows, transparency effects, window dragging, click-through
- **File System** - File read/write, folder selection, path operations
- **System Information** - Get CPU, memory, GPU, OS information, etc.
- **Input Simulation** - Simulate keyboard and mouse operations
- **Tray Icon** - System tray integration, bubble notifications, custom menus
- **External Programs** - Execute system commands, open files, launch applications

### 3. Event System

Puppet provides powerful event monitoring capabilities:

- **USB Device Events** - Monitor USB device insertion and removal
- **Disk Events** - Monitor disk mounting and unmounting
- **Window Events** - Monitor window state changes
- **Custom Events** - Support for custom event handling

### 4. Security Mechanisms

Puppet has built-in multi-layer security protection:

- **Communication Validation** - Validate all local requests with runtime-generated random keys
- **File System Protection** - Automatically block access to Windows system folders
- **Permission Confirmation** - User confirmation dialogs before dangerous operations
- **Path Normalization** - Prevent path traversal attacks
- **PUP Encryption** - Support password-encrypted PUP file format
- **Digital Signatures** (V1.2) - Support X.509 certificate-based digital signatures to ensure application integrity and source authenticity
- **Database Signatures** (V1.2) - Support database signature verification to prevent data tampering

### 5. Unique PUP Packaging Format

Puppet provides a unique `.pup` file format:

- **Custom Format** - Proprietary format combining ZIP and AES encryption
- **Password Protection** - Support AES-256 encryption to protect source code
- **Single File Distribution** - All resources packaged into one file for easy distribution
- **Fast Loading** - Optimized file structure and loading mechanism
- **Multi-Version Support**:
  - **V1.0** - Basic features, supports encryption
  - **V1.1** - Supports startup scripts
  - **V1.2** - Supports digital signatures and certificate verification

## Technical Architecture

### Overall Architecture

```
┌─────────────────────────────────────────┐
│         Web Application Layer (HTML/CSS/JS)        │
├─────────────────────────────────────────┤
│         JavaScript API (puppet.*)       │
├─────────────────────────────────────────┤
│         COM Bridge Layer                │
├─────────────────────────────────────────┤
│         C# Business Logic Layer         │
├─────────────────────────────────────────┤
│         .NET Runtime                    │
├─────────────────────────────────────────┤
│         Windows OS                      │
└─────────────────────────────────────────┘
```

### Component Architecture

```
Puppet Application
│
├─── WebView2 Control (Web Renderer)
│    └─── HTML/CSS/JavaScript UI
│
├─── HTTP Server (Local)
│    ├─── Puppet API Endpoints
│    └─── Static File Server
│
├─── Controllers (Business Logic)
│    ├─── Window Controller
│    ├─── Application Controller
│    ├─── File System Controller
│    ├─── System Controller
│    ├─── Device Controller
│    ├─── Events Controller
│    ├─── Tray Controller
│    └─── Storage Controller
│
├─── PUP Server (File Loader)
│    ├─── PUP Parser
│    ├─── Zip Extractor
│    └─── AES Decryptor
│
└─── Security Layer
     ├─── Communication Validation
     ├─── Path Protection
     └── Permission Management
```

## Comparison

### Puppet vs Electron

| Feature | Puppet | Electron |
|---------|--------|----------|
| Platform | Windows Only | Cross-platform |
| Bundle Size | ~10 MB | ~150 MB |
| Memory Usage | Low | High |
| Startup Speed | Fast | Slow |
| System Integration | Deep | Limited |
| Development Experience | Simple | Complex |
| Distribution Format | Single .pup file | Multiple files/folders |

### Puppet vs WinForms/WPF

| Feature | Puppet | WinForms/WPF |
|---------|--------|--------------|
| UI Technology | Web (HTML/CSS/JS) | Native Controls |
| Design Flexibility | High | Limited |
| Learning Curve | Low (Web developers) | High (Requires C#/XAML) |
| Cross-platform | Windows Only | Windows Only |
| Modern UI Support | Excellent | Requires Custom Controls |
| Hot Reload | Supported | Limited |

## Use Cases

### ✅ Recommended

Puppet is particularly suitable for:

- **Device Management Tools** - USB device managers, disk management utilities, peripheral monitoring applications
- **System Monitoring** - System resource monitors, process managers, network monitoring tools
- **Tray Applications** - System tray tools, background service monitors, quick access toolbars
- **Transparent Windows** - Desktop widgets, floating tools, transparent overlay layers
- **Quick Prototypes** - Concept validation applications, MVP development, internal tools
- **Modern Desktop** - Desktop applications requiring web technologies, cross-device applications

### ❌ Not Recommended

- **Cross-platform applications** - Puppet is Windows-only
- **3D graphics applications** - Better to use Unity/Unreal
- **Large-scale enterprise applications** - Better to use WPF
- **Games** - Better to use game engines

## Tech Stack

Puppet Framework is built on the following technologies:

- **.NET 9.0** - Core framework
- **Windows Forms** - Desktop UI framework
- **WebView2** - Edge-based web rendering engine
- **COM Interop** - Bridge between JavaScript and C#
- **WMI** - Windows Management Interface for device monitoring
- **SharpZipLib** - ZIP file operations (for PUP format)

## Quick Links

- [Getting Started](./getting-started.md) - Create your first Puppet application
- [API Documentation](../api/) - Complete API reference manual
- [Best Practices](./best-practices.md) - Development recommendations
- [Security](./security.md) - Security features and best practices

## Next Steps

Now that you understand the basics of Puppet Framework, we recommend:

1. Read [Getting Started](./getting-started.md) to create your first Puppet application
2. Learn [Architecture](./architecture.md) to understand internal principles
3. Reference [API Documentation](../api/) for actual development
4. Follow [Best Practices](./best-practices.md) to develop high-quality applications