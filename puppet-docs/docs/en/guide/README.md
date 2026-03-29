---
title: Guide
permalink: /en/guide/
createTime: 2026/03/28 14:50:37
---

# Guide

Welcome to the Puppet Framework documentation. This guide will help you understand the core concepts, architecture, and how to use Puppet Framework to build desktop applications.

## Table of Contents

- [Getting Started](./getting-started.md) - Get started with Puppet Framework in 5 minutes
- [Introduction](./introduction.md) - Learn about Puppet Framework's features and capabilities
- [Architecture](./architecture.md) - Deep dive into the framework's internal architecture and working principles
- [Project Structure](./project-structure.md) - Standard directory structure for Puppet projects
- [PUP File Format](./pup-format.md) - Understand Puppet's unique packaging format
- [PUP Startup Script](./pup-script.md) - Use startup scripts to configure application windows
- [Command Line Parameters](./cli-parameters.md) - Command line tool usage instructions
- [Security Mechanisms](./security.md) - Framework security features and best practices
- [Best Practices](./best-practices.md) - Recommendations for developing high-quality Puppet applications
- [Puppet Signing Tool](./puppet-sign.md) - Use puppet-sign for signing and verification

## Related Tools

- **puppet-sign** - Standalone signing tool for generating signing key pairs, signing databases, and verifying signatures
  - Supports generating self-signed X.509 certificates
  - Supports RSA 2048/4096 bit keys
  - Supports database signing and verification
  - See [Puppet Signing Tool](./puppet-sign.md)

## What is Puppet?

Puppet is a **Microsoft WebView2** based desktop application framework that allows developers to build powerful desktop applications using familiar web technologies (HTML, CSS, JavaScript). Unlike traditional Electron or NW.js, Puppet is designed specifically for Windows, providing deeper system integration and a more lightweight solution.

### Core Principles

Puppet is designed around the following core principles:

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

Puppet is particularly suitable for the following application scenarios:

### ✅ Recommended

- Applications requiring system-level functionality
- Device management tools
- System monitoring tools
- Tray applications
- Transparent window applications
- Quick prototype development

### ❌ Not Recommended

- Cross-platform applications (Puppet is Windows-only)
- Applications requiring 3D graphics (better to use Unity/Unreal)
- Large-scale enterprise applications (better to use WPF)
- Games (better to use game engines)

## Tech Stack

Puppet Framework is built on the following technologies:

- **.NET 9.0** - Core framework
- **Windows Forms** - Desktop UI framework
- **WebView2** - Edge-based web rendering engine
- **COM Interop** - Bridge between JavaScript and C#
- **WMI** - Windows Management Interface for device monitoring

## Quick Links

- [API Documentation](../api/) - Complete API reference manual
- [GitHub Repository](#) - Source code
- [Issue Tracker](#) - Submit issues and suggestions

## Getting Help

If you encounter issues during development:

1. Consult relevant sections of this documentation
2. Check [API Documentation](../api/) for specific usage
3. Search [Known Issues](#)
4. Submit issues in [GitHub Issues](#)

## Next Steps

We recommend following the documentation in this order:

1. Read [Getting Started](./getting-started.md) to create your first Puppet application
2. Learn [Introduction](./introduction.md) to understand core concepts
3. Study [Architecture](./architecture.md) to understand working principles
4. Reference [API Documentation](../api/) for actual development
5. Follow [Best Practices](./best-practices.md) to develop high-quality applications