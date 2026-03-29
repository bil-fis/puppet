---
title: Changelog
permalink: /en/changelog/
createTime: 2026/03/28 14:54:00
---

# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- English documentation support
- GitHub Actions CI/CD workflow for automated releases
- Multi-language documentation structure

---

## [1.2.0] - 2026-03-29

### Added
- **PUP V1.2 Format Support**
  - Digital signature support based on X.509 certificates
  - Certificate and encrypted private key storage in PUP files
  - Database signature verification mechanism
  - Certificate fingerprint verification
  - Support for RSA 2048/4096 bit keys
  - Self-signed certificate support

- **puppet-sign Tool**
  - Standalone signing tool for generating signing key pairs
  - Database signing functionality
  - Signature verification capabilities
  - Interactive certificate generation
  - Support for custom certificate parameters

- **Security Features**
  - Application signature verification via `getAppInfo()` API
  - Database signature verification via `verifyDatabaseSignature()` API
  - Database signing via `signDatabase()` API
  - Certificate information query (fingerprint, issuer, validity period)
  - Automatic signature validation when accessing signed databases

- **New APIs**
  - `puppet.application.getAppInfo()` - Get application information including signature status
  - `puppet.storage.verifyDatabaseSignature()` - Verify database signature
  - `puppet.storage.signDatabase()` - Sign database

- **Documentation**
  - Comprehensive security documentation
  - PUP V1.2 format specification
  - puppet-sign tool documentation
  - PUP startup script documentation
  - Updated API documentation with signature features

### Changed
- Updated PUP file format to support V1.2
- Enhanced storage controller with signature verification
- Improved error handling for signature validation failures
- Updated documentation structure and navigation

### Security
- Implemented SHA256withRSA signature algorithm
- Added AES-256-GCM encryption for private key storage
- Implemented PBKDF2 key derivation (100,000 iterations)
- Added certificate validity period checking
- Prevented access to databases with invalid signatures

### Dependencies
- Updated to .NET 9.0
- Added System.Security.Cryptography API usage

---

## [1.1.0] - 2026-03-28

### Added
- **PUP V1.1 Format Support**
  - Startup script functionality
  - Automatic script execution on application launch
  - Window configuration via startup scripts

- **Startup Script Commands**
  - `set startup_position` - Set window startup position
  - `set borderless` - Enable/disable borderless mode
  - `set window_size` - Set window dimensions

- **Command Line Parameters**
  - `--script` parameter for specifying startup script file
  - `-v` or `--version` parameter for PUP version selection

- **Window Position Support**
  - Predefined positions: left-top, left-bottom, right-top, right-bottom, center
  - Custom coordinate support
  - Automatic screen area calculation (excluding taskbar)

### Changed
- Updated PUP file format to support V1.1
- Enhanced PupServer to parse and execute startup scripts
- Improved window controller with new positioning options
- Updated documentation with startup script usage

### Documentation
- Added startup script documentation
- Updated command line parameter documentation
- Enhanced best practices guide

---

## [1.0.0] - 2026-03-27

### Added
- **Initial Release**
  - Puppet Framework core functionality
  - WebView2 integration
  - Complete API suite

- **Window Control API**
  - Borderless window support
  - Window transparency effects
  - Window dragging and resizing
  - Click-through functionality
  - Window positioning (center, maximize, minimize, close)

- **File System API**
  - File and folder selection dialogs
  - File read/write operations
  - File deletion
  - Path operations

- **System API**
  - System information retrieval
  - Screenshot capture
  - Keyboard and mouse input simulation
  - Clipboard operations

- **Event System API**
  - USB device plug/unplug monitoring
  - Disk mount/unmount monitoring
  - Window state change events
  - Custom event support

- **Tray Icon API**
  - System tray icon management
  - Custom tray menus
  - Bubble notifications
  - Custom icon support

- **Device API**
  - USB device enumeration
  - Device information retrieval
  - Device status monitoring

- **Application API**
  - Application lifecycle management (close, restart)
  - Window information query
  - External program execution
  - Configuration file management
  - System path access
  - User information retrieval

- **Storage API**
  - SQLite-based key-value storage
  - Multi-database support
  - Database management
  - Batch operations
  - Query optimization

- **Logging API**
  - Multi-level logging (info, warn, error)
  - Console output
  - Log persistence

- **PUP File Format**
  - V1.0 format with AES-256 encryption
  - Single file distribution
  - Password protection
  - Optimized loading mechanism

- **Command Line Tools**
  - `--create-pup` - Create PUP files
  - `--load-pup` - Load PUP files
  - `--nake-load` - Load bare folders
  - `-p/--password` - Set encryption password

- **Security Features**
  - Communication validation with random keys
  - File system protection
  - Permission confirmation dialogs
  - Path normalization

- **Documentation**
  - Complete API reference
  - Getting started guide
  - Best practices
  - Architecture documentation
  - Project structure guide

### Technical Stack
- .NET 9.0
- Windows Forms
- WebView2
- COM Interop
- WMI
- SharpZipLib
- System.Data.SQLite

### Platform
- Windows 10 or higher
- .NET 9.0 Runtime
- WebView2 Runtime

---

## [Unreleased] - Future Plans

### Planned Features
- Cross-platform support (macOS, Linux)
- Plugin system
- Custom themes
- Advanced animation support
- Performance profiling tools
- Remote debugging support

### Improvements
- Better hot reload performance
- Enhanced error messages
- More comprehensive examples
- Improved documentation
- Better TypeScript definitions

---

## Version History

| Version | Date | Description |
|---------|------|-------------|
| 1.2.0 | 2026-03-29 | Digital signatures, puppet-sign tool, V1.2 format |
| 1.1.0 | 2026-03-28 | Startup scripts, V1.1 format |
| 1.0.0 | 2026-03-27 | Initial release |

---

## Upgrade Guide

### Upgrading from 1.1.0 to 1.2.0

1. **Update Dependencies**: Ensure .NET 9.0 is installed
2. **Review Breaking Changes**: No breaking changes in this release
3. **Update PUP Files**: Optional - can use existing V1.1 files or create new V1.2 files
4. **Learn New Features**: Review digital signature documentation
5. **Update Application Code**: Add signature verification if needed

### Upgrading from 1.0.0 to 1.1.0

1. **Update Dependencies**: Ensure .NET 9.0 is installed
2. **Review Breaking Changes**: No breaking changes in this release
3. **Use Startup Scripts**: Optional - can use existing V1.0 files or add startup scripts
4. **Update Application Code**: No code changes required
5. **Learn New Features**: Review startup script documentation

---

## Security Notes

### Vulnerability Reporting
If you discover a security vulnerability, please report it responsibly:
1. Do not create public issues
2. Send a private message with details
3. Allow time to fix before public disclosure

### Security Best Practices
- Always use signed PUP files in production
- Keep signing keys secure
- Regularly update to the latest version
- Follow security guidelines in documentation

---

## Contributors

- 林晚晚ss - Core development
- All contributors - Documentation and testing

---

## Links

- [GitHub Repository](https://github.com/your-username/puppet)
- [Documentation](https://puppet.ifiss.eu.org)
- [Issue Tracker](https://github.com/your-username/puppet/issues)
- [Security Policy](https://github.com/your-username/puppet/security)