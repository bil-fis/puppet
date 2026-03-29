---
pageLayout: home
hero:
  name: Puppet Framework
  text: A Desktop Application Development Framework Based on WebView2
  tagline: Build Desktop Applications with Web Technologies
  image:
    src: /logo.svg
    alt: Puppet Framework
  actions:
    - theme: brand
      text: Get Started
      link: /en/guide/getting-started.html
    - theme: alt
      text: API Documentation
      link: /en/api/
    - theme: alt
      text: View Examples
      link: /en/guide/best-practices.html

features:
  - title: 🌐 Web Tech Stack
    details: Build desktop applications using HTML, CSS, and JavaScript. Lower the development barrier with familiar development experience.
  - title: ⚡ Deep Integration
    details: Full access to Windows system capabilities including file system, window management, and device monitoring.
  - title: 🎯 Event-Driven
    details: Monitor and respond to USB devices, disks, and window events in real-time.
  - title: 🔒 Secure & Reliable
    details: Built-in multi-layer security mechanisms including communication validation, path protection, permission confirmation, and encrypted storage.
  - title: 📦 Unique Packaging
    details: Supports AES-256 encrypted PUP packaging format. Single-file distribution, secure and convenient.
  - title: 🚀 Lightweight & Efficient
    details: Based on .NET and WebView2. Low resource usage, excellent performance, and fast startup.

---

## Use Cases

### 💾 Device Management Tools
USB device managers, disk management utilities, peripheral monitoring applications.

### 📊 System Monitoring
System resource monitors, process managers, network monitoring tools.

### 🔔 Tray Applications
System tray tools, background service monitors, quick access toolbars.

### 🪟 Transparent Windows
Desktop widgets, floating tools, transparent overlay layers.

### 💡 Quick Prototypes
Concept validation applications, MVP development, internal tools.

### 🎨 Modern Desktop
Desktop applications that require web technologies, cross-device applications.

---

## Quick Start

```bash
# Create your first application
mkdir MyApp
cd MyApp

# Create index.html file
echo '<h1>Hello Puppet!</h1>' > index.html

# Run application (bare folder mode)
puppet.exe --nake-load .

# Or create PUP file
puppet.exe --create-pup -i . -o app.pup
puppet.exe --load-pup app.pup
```

## Core APIs

### Window Control
- Borderless windows, transparency effects, click-through
- Window dragging, resizing, always on top

### File System
- File selection dialogs, file read/write, delete operations

### System Functions
- System information retrieval, screenshots, input simulation

### Event System
- USB device plug/unplug, disk mounting, window events

### Tray Icon
- Tray menu, bubble notifications, custom icons

### Security Mechanisms
- PUP file digital signature (V1.2)
- Database signature verification
- Certificate integrity checking

---

<div style="text-align: center; padding: 40px 20px 20px; border-top: 1px solid var(--c-border); margin-top: 60px; color: var(--c-text-light);">
  <p style="margin: 0 0 10px 0; font-size: 16px;">
    © 2024 Puppet Framework. All rights reserved.
  </p>
  <p style="margin: 0; font-size: 14px;">
    Built with <a href="https://vuepress.vuejs.org/" target="_blank" style="color: var(--c-brand);">VuePress</a> and
    <a href="https://theme-plume.vuejs.press/" target="_blank" style="color: var(--c-brand);">Plume Theme</a>
  </p>
</div>