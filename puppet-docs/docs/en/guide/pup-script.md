---
title: PUP Startup Script
permalink: /en/guide/pup-script.html
createTime: 2026/03/29 19:45:00
---

# PUP Startup Script

PUP startup script is an optional feature that allows automatic configuration of window properties when the application starts. Supported from PUP V1.1 version, V1.2 version continues to be compatible and enhances signature verification functionality.

## Overview

Startup script is a text file containing a series of commands for configuring the application window. By using startup scripts, you can:

- **Preset window position**: Automatically move window to specified position on startup
- **Configure window style**: Enable or disable borderless window
- **Set window size**: Customize application window dimensions

## Basic Usage

### Creating Script File

Create a text file (typically named `script.txt` or `startup.txt`) and add configuration commands:

```txt
# This is a startup script example
set startup_position center
set borderless true
set window_size 800,600
```

### Including Script When Creating PUP File

Use `--script` parameter to specify the script file:

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "C:\script.txt"

# V1.2 version (with signature)
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --script "C:\script.txt" --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

## Script Syntax

### Basic Format

Startup script uses simple command format:

```txt
set <property> <value>
```

- One command per line
- Commands are case-insensitive
- Supports comments using `//` or `#`
- Empty lines are ignored

### Supported Commands

#### 1. startup_position

Set window startup position.

**Syntax**:

```txt
set startup_position <position>
```

**Supported Values**:

| Value | Description |
|-------|-------------|
| `left-top` | Top-left of screen |
| `left-bottom` | Bottom-left of screen |
| `right-top` | Top-right of screen |
| `right-bottom` | Bottom-right of screen |
| `center` | Screen center |
| `x,y` | Custom coordinates (pixels) |

**Examples**:

```txt
# Use predefined position
set startup_position center
set startup_position left-top

# Use custom coordinates
set startup_position 100,100
set startup_position 1920,0
```

**Notes**:

- Coordinates are relative to the top-left corner of the primary screen
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

- Borderless window removes title bar and borders
- Borderless window typically needs custom drag functionality
- Suitable for creating floating windows, desktop widgets, etc.

#### 3. window_size

Set window size.

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

### Example 5: Richly Commented Script

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

Commands in the startup script are executed from top to bottom. It's recommended to organize the script in the following order:

1. **Set window size** (`window_size`) - First determine window dimensions
2. **Set window style** (`borderless`) - Then set window style
3. **Set window position** (`startup_position`) - Finally position the window

**Recommended Order**:

```txt
set window_size 800,600
set borderless true
set startup_position center
```

## Error Handling

If errors occur during script execution:

- Error messages are output to the console
- Current command will be skipped
- Script continues to execute subsequent commands

**Common Errors**:

```
Error executing line 3: Invalid startup position value: invalid
  Command: set startup_position invalid
```

**Solutions**:

- Check if command syntax is correct
- Confirm parameter values are within supported range
- Check error messages output in console

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
| Execution Time | When application starts | After application loads |
| Purpose | Initial configuration | Dynamic control |
| Change Window Position | ✅ | ✅ |
| Change Window Style | ✅ | ✅ |
| Change Window Size | ✅ | ✅ |
| Respond to User Actions | ❌ | ✅ |
| Dynamic Adjustment | ❌ | ✅ |

**Usage Recommendation**:

- **Startup Script**: Used to set initial application state
- **JavaScript API**: Used to respond to user interactions and dynamic window adjustments

**Mixed Usage Example**:

```txt
# Startup script: Set initial state
set startup_position center
set borderless true
```

```javascript
// JavaScript: Respond to user actions
document.getElementById('toggle-border').addEventListener('click', async () => {
    await puppet.window.setBorderless(!isBorderless);
});
```

## Best Practices

### 1. Use Comments

Add clear comments to the script explaining the purpose of each configuration:

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

### 3. Test Script

Test the script using bare folder mode before release:

```bash
puppet.exe --nake-load "C:\MyProject"
```

### 4. Document Script

Explain the script's functionality and configuration items in project documentation to help other developers understand and maintain.

### 5. Version Control

Include script file in version control system to record configuration change history.

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

If you need to select different configurations based on conditions, you can implement this in build scripts:

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

### Problem: Script Not Taking Effect

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

### Problem: Window Position Incorrect

**Possible Causes**:

1. Coordinates exceed screen range
2. Coordinate calculation error in multi-monitor environment

**Solutions**:

```txt
# Use predefined position instead of absolute coordinates
set startup_position center

# Or use smaller coordinate values
set startup_position 50,50
```

### Problem: Borderless Window Cannot Be Moved

**Reason**: Borderless window doesn't support dragging by default

**Solution**: Implement drag functionality using JavaScript API:

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

- [PUP File Format](./pup-format.html) - Learn about PUP file structure
- [Command Line Parameters](./cli-parameters.html) - --script parameter description
- [Window Control API](../api/window.html) - JavaScript window control API
- [Puppet Signing Tool](./puppet-sign.html) - Signing PUP files

## Changelog

### V1.2 (2026-03-29)

- Fully compatible with V1.1 startup script functionality
- Supports digital signature verification

### V1.1 (2026-03-28)

- First introduced startup script functionality
- Supports startup_position, borderless, window_size commands