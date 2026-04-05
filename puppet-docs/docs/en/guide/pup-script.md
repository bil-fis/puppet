---
title: PUP Startup Script
permalink: /en/guide/pup-script.html
createTime: 2026/03/29 19:45:00
---

# PUP Startup Script

The PUP startup script is an optional feature that allows automatic configuration of window properties when the application starts. Supported starting from PUP V1.1 version, V1.2 version continues compatibility and enhances signature verification functionality.

## Overview

The startup script is a text file containing a series of commands for configuring the application window. By using startup scripts, you can:

- **Preset Window Position**: Automatically move the window to a specified position on startup
- **Configure Window Style**: Enable or disable borderless windows
- **Set Window Size**: Customize application window dimensions

## Basic Usage

### Creating a Script File

Create a text file (usually named `script.txt` or `startup.txt`) and add configuration commands:

```txt
# This is a startup script example
set startup_position center
set borderless true
set window_size 800,600
```

### Including Script When Creating PUP File

Use the `--script` parameter to specify the script file:

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "C:\script.txt"

# V1.2 version (with signature)
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --script "C:\script.txt" --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

## Script Syntax

### Basic Format

Startup scripts use a simple command format:

```txt
set <property> <value>
```

- One command per line
- Commands are case-insensitive
- Supports adding comments with `//` or `#`
- Empty lines are ignored

### Supported Commands

#### 1. startup_position

Set the window startup position.

**Syntax**:

```txt
set startup_position <position>
```

**Supported Values**:

| Value | Description |
|-------|-------------|
| `left-top` | Top-left corner of screen |
| `left-bottom` | Bottom-left corner of screen |
| `right-top` | Top-right corner of screen |
| `right-bottom` | Bottom-right corner of screen |
| `center` | Center of screen |
| `x,y` | Custom coordinates (pixels) |

**Examples**:

```txt
# Using predefined positions
set startup_position center
set startup_position left-top

# Using custom coordinates
set startup_position 100,100
set startup_position 1920,0
```

**Notes**:

- Coordinates are relative to the top-left corner of the main screen
- Coordinate unit is pixels
- Window may be clipped by screen boundaries

#### 2. borderless

Set whether to enable borderless window.

**Syntax**:

```txt
set borderless <value>
```

**Supported Values**:

| Value | Description |
|-------|-------------|
| `true` | Enable borderless |
| `false` | Disable borderless |
| `1` | Enable borderless |
| `0` | Disable borderless |
| `yes` | Enable borderless |
| `no` | Disable borderless |

**Examples**:

```txt
set borderless true
set borderless 1
set borderless yes
```

**Notes**:

- Borderless windows remove title bar and borders
- Borderless windows typically need custom drag functionality
- Suitable for creating floating windows, desktop widgets, etc.

#### 3. window_size

Set the window size.

**Syntax**:

```txt
set window_size <width>,<height>
```

**Parameters**:

- `width` - Window width (pixels, must be positive integer)
- `height` - Window height (pixels, must be positive integer)

**Examples**:

```txt
set window_size 800,600
set window_size 1920,1080
set window_size 640,480
```

**Notes**:

- Width and height must be positive integers
- Use comma to separate width and height
- Window size cannot exceed screen dimensions

## Complete Examples

### Example 1: Centered Window

```txt
# Centered window, borderless, default size
set startup_position center
set borderless true
```

### Example 2: Fixed Position and Size

```txt
# Fixed position and size
set startup_position 100,100
set window_size 640,480
```

### Example 3: Bottom-Right Floating Window

```txt
# Bottom-right floating window
set startup_position right-bottom
set borderless true
set window_size 300,200
```

### Example 4: Fullscreen Application

```txt
# Fullscreen application (1920x1080)
set startup_position left-top
set borderless true
set window_size 1920,1080
```

### Example 5: Well-Commented Script

```txt
# ===================================
# MyApp Startup Script
# Version: 1.0
# Author: MyCompany
# ===================================

// Set window position to screen center
set startup_position center

// Enable borderless mode for more modern interface
set borderless true

// Set window size to 1280x720 (720p)
set window_size 1280,720
```

## Execution Order

Commands in the startup script are executed from top to bottom. It's recommended to organize scripts in the following order:

1. **Set Window Size** (`window_size`) - First determine window dimensions
2. **Set Window Style** (`borderless`) - Then set window style
3. **Set Window Position** (`startup_position`) - Finally position the window

**Recommended Order**:

```txt
set window_size 800,600
set borderless true
set startup_position center
```

## Error Handling

If errors occur during script execution:

- Error messages are output to the console
- Current command is skipped
- Script continues executing subsequent commands

**Common Errors**:

```
Error executing line 3: Invalid startup position value: invalid
  Command: set startup_position invalid
```

**Solutions**:

- Check if command syntax is correct
- Confirm parameter values are within supported range
- View error messages output to console

## Version Compatibility

| Version | Startup Script Support | Signature Support |
|---------|------------------------|-------------------|
| V1.0 | ❌ | ❌ |
| V1.1 | ✅ | ❌ |
| V1.2 | ✅ | ✅ |

### V1.1

- Supports basic startup script functionality
- Supports startup_position, borderless, window_size commands

### V1.2

- Fully compatible with V1.1 startup script functionality
- Supports digital signature verification
- Scripts and PUP file contents can be signature protected

## Comparison with JavaScript API

Startup scripts and JavaScript API can both control windows, but serve different purposes:

| Feature | Startup Script | JavaScript API |
|---------|----------------|----------------|
| Execution Timing | On application startup | After application loads |
| Purpose | Initial configuration | Dynamic control |
| Modify Window Position | ✅ | ✅ |
| Modify Window Style | ✅ | ✅ |
| Modify Window Size | ✅ | ✅ |
| Respond to User Operations | ❌ | ✅ |
| Dynamic Adjustment | ❌ | ✅ |

**Usage Recommendations**:

- **Startup Script**: For setting application initial state
- **JavaScript API**: For responding to user interaction and dynamic window adjustment

**Mixed Usage Example**:

```txt
# Startup script: Set initial state
set startup_position center
set borderless true
```

```javascript
// JavaScript: Respond to user operations
document.getElementById('toggle-border').addEventListener('click', async () => {
    await puppet.window.setBorderless(!isBorderless);
});
```

## Best Practices

### 1. Use Comments

Add clear comments to scripts explaining each configuration's purpose:

```txt
# Set window to borderless mode for more modern interface experience
set borderless true
```

### 2. Provide Default Values

If users need to customize configuration, provide reasonable default values in the script:

```txt
# Default window size (can be overridden by user configuration)
set window_size 1024,768
```

### 3. Test Scripts

Before release, test scripts using bare folder mode:

```bash
puppet.exe --nake-load "C:\MyProject"
```

### 4. Document Scripts

In project documentation, explain script functionality and configuration items for easier understanding and maintenance by other developers.

### 5. Version Control

Include script files in version control system to record configuration change history.

## Advanced Usage

### Dynamic Script Generation

If you need to generate different scripts for different environments, you can use script generation tools:

```bash
# Development environment
echo "set startup_position 0,0" > dev-script.txt
echo "set borderless false" >> dev-script.txt

# Production environment
echo "set startup_position center" > prod-script.txt
echo "set borderless true" >> prod-script.txt
echo "set window_size 800,600" >> prod-script.txt

# Create PUP file
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp-dev.pup" -v 1.1 --script "dev-script.txt"
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp-prod.pup" -v 1.1 --script "prod-script.txt"
```

### Conditional Configuration

If you need to select different configurations based on conditions, you can implement it in build scripts:

```powershell
# PowerShell build script
$scriptContent = ""
if ($env:BUILD_ENV -eq "production") {
    $scriptContent = "set borderless true`nset window_size 800,600"
} else {
    $scriptContent = "set borderless false`nset window_size 1024,768"
}

Set-Content -Path "script.txt" -Value $scriptContent
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "script.txt"
```

## Troubleshooting

### Issue: Script Not Taking Effect

**Possible Causes**:

1. Script file path is incorrect
2. PUP version is lower than V1.1
3. Script syntax error

**Solutions**:

```bash
# Check PUP version
puppet.exe --version

# Check if script file exists
type script.txt

# Create PUP file using absolute path
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "C:\full\path\to\script.txt"
```

### Issue: Window Position Incorrect

**Possible Causes**:

1. Coordinates exceed screen range
2. Coordinate calculation error in multi-monitor environment

**Solutions**:

```txt
# Use predefined positions instead of absolute coordinates
set startup_position center

# Or use smaller coordinate values
set startup_position 50,50
```

### Issue: Borderless Window Cannot Be Moved

**Cause**: Borderless windows don't support dragging by default

**Solution**:

Implement drag functionality using JavaScript API:

```javascript
let isDragging = false;
let startX, startY;

document.addEventListener('mousedown', (e) => {
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
});

document.addEventListener('mousemove', async (e) => {
    if (isDragging) {
        const dx = e.clientX - startX;
        const dy = e.clientY - startY;
        const info = await puppet.window.getWindowInfo();
        await puppet.window.moveWindow(info.x + dx, info.y + dy);
        startX = e.clientX;
        startY = e.clientY;
    }
});

document.addEventListener('mouseup', () => {
    isDragging = false;
});
```

## Related Resources

- [PUP File Format](./pup-format.md) - Understand PUP file structure
- [Command Line Parameters](./cli-parameters.md) - --script parameter description
- [Window Control API](../api/window.md) - JavaScript window control API
- [Puppet Signing Tool](./puppet-sign.md) - Sign PUP files

## Changelog

### V1.2 (2026-03-29)

- Fully compatible with V1.1 startup script functionality
- Supports digital signature verification

### V1.1 (2026-03-28)

- First introduction of startup script functionality
- Supports startup_position, borderless, window_size commands