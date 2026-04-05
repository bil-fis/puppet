---
title: Guide
permalink: /en/guide/
createTime: 2026/03/28 14:50:37
---

# Guide

Welcome to the Puppet Framework documentation. This guide will help you understand the core concepts, architecture design, and how to use the Puppet Framework to build desktop applications.

## Table of Contents

- [Getting Started](./getting-started.md) - Get started with the Puppet Framework in 5 minutes
- [Introduction](./introduction.md) - Learn about the features and characteristics of the Puppet Framework
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
  - Supports RSA 2048/4096-bit keys
  - Supports database signing and verification
  - See [Puppet Signing Tool](./puppet-sign.md) for details

## What is Puppet?

Puppet is a Windows desktop application development framework based on **WebView2** that allows developers to build powerful desktop applications using familiar web technologies (HTML, CSS, JavaScript).

### Core Features

- **Web Technology Stack**: Build interfaces using HTML, CSS, JavaScript
- **Native System Integration**: Complete access to Windows system functionality
- **Event-Driven**: Supports USB device, disk, window, and other event monitoring
- **Security Mechanisms**: Built-in permission control and security verification
- **Unique Packaging**: Supports encrypted PUP packaging format

### Use Cases

- Desktop applications requiring system-level functionality
- Device management tools
- System monitoring tools
- Tray applications
- Transparent window applications
- Rapid prototype development

## Technology Stack

The Puppet Framework is built on the following technologies:

- **.NET 10.0** - Core framework
- **Windows Forms** - Desktop UI framework
- **WebView2** - Edge-based web rendering engine
- **COM Interop** - Bridge between JavaScript and C#
- **WMI** - Windows Management Interface for device monitoring

## Quick Links

- [API Documentation](../api/) - Complete API reference manual
- [GitHub Repository](#) - Source code
- [Issue Tracker](#) - Submit issues and suggestions

## Getting Help

If you encounter issues during usage:

1. Check the relevant sections of this documentation
2. View [API Documentation](../api/) for specific usage
3. Search [Known Issues](#)
4. Submit issues on [GitHub Issues](#)

## Next Steps

It is recommended to read the documentation in the following order:

1. Read [Getting Started](./getting-started.md) to create your first Puppet application
2. Learn about [Introduction](./introduction.md) to master core concepts
3. Study [Architecture](./architecture.md) to understand working principles
4. Reference [API Documentation](../api/) for actual development
5. Follow [Best Practices](./best-practices.md) to develop high-quality applications