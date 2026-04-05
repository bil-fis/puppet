---
title: Window Control
permalink: /en/api/window.html
createTime: 2026/03/28 15:02:11
---

# Window Control API

Window Control API provides complete control over application windows, including style settings, position adjustment, transparency effects, click-through, and more.

## Overview

The `puppet.window` namespace provides the following features:

- Borderless windows
- Window dragging and resizing
- Transparency effects
- Click-through
- Window position and size control
- Window always on top
- Taskbar display control
- Movable element mounting

## Methods

### setBorderless()

Set whether the window is borderless.

```javascript
await puppet.window.setBorderless(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for borderless, `false` for bordered

**Examples**:

```javascript
// Set as borderless window
await puppet.window.setBorderless(true);

// Restore to bordered window
await puppet.window.setBorderless(false);
```

**Notes**:

- After borderless window, need to use `setDraggable()` to enable dragging
- Borderless window loses default window title bar
- Recommended to set immediately after page loads

::: tip Recommended Usage
```javascript
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
});
```
:::

### setDraggable()

Set whether the window is draggable.

```javascript
await puppet.window.setDraggable(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for draggable, `false` for not draggable

**Examples**:

```javascript
// Enable window dragging
await puppet.window.setDraggable(true);

// Disable window dragging
await puppet.window.setDraggable(false);
```

**Description**:

- Dragging functionality is implemented by simulating clicking the window title bar
- Borderless windows must enable this feature to be draggable
- Can use `mountMovableElement()` to specify specific elements as drag areas

### setResizable()

Set whether the window is resizable.

```javascript
await puppet.window.setResizable(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for resizable, `false` for not resizable

**Examples**:

```javascript
// Enable window resizing
await puppet.window.setResizable(true);

// Disable window resizing
await puppet.window.setResizable(false);
```

### setTransparent()

Set whether the window supports transparency effects.

```javascript
await puppet.window.setTransparent(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for transparency support, `false` for no transparency

**Examples**:

```javascript
// Enable transparency effects
await puppet.window.setTransparent(true);

// Disable transparency effects
await puppet.window.setTransparent(false);
```

**Description**:

- Transparency effects need to be used with `setOpacity()`
- After enabling, transparent background areas will allow click-through
- Can use `setTransparentColor()` to specify transparent color

### setOpacity()

Set window transparency (0.0 - 1.0).

```javascript
await puppet.window.setOpacity(value: number): Promise<void>
```

**Parameters**:

- `value` (number) - Transparency value, range 0.0 (completely transparent) to 1.0 (completely opaque)

**Examples**:

```javascript
// Set to 50% transparent
await puppet.window.setOpacity(0.5);

// Set to 90% transparent
await puppet.window.setOpacity(0.9);

// Completely opaque
await puppet.window.setOpacity(1.0);
```

**Notes**:

- Value must be between 0.0 and 1.0
- 0.0 means window completely invisible
- Recommended not to go below 0.3, otherwise window may not be visible

### setMouseThroughTransparency()

Set whether transparent areas allow click-through.

```javascript
await puppet.window.setMouseThroughTransparency(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for click-through, `false` for no click-through

**Examples**:

```javascript
// Enable transparent area click-through
await puppet.window.setMouseThroughTransparency(true);

// Disable transparent area click-through
await puppet.window.setMouseThroughTransparency(false);
```

**Description**:

- When enabled, mouse clicks on transparent areas will pass through to the underlying window
- Suitable for desktop widgets, floating tools, etc.
- Must call `setTransparent(true)` first

### setMouseThrough()

Set whether the entire window allows click-through.

```javascript
await puppet.window.setMouseThrough(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for click-through, `false` for no click-through

**Examples**:

```javascript
// Make window completely click-through
await puppet.window.setMouseThrough(true);

// Restore window interaction
await puppet.window.setMouseThrough(false);
```

**Notes**:

- After click-through, cannot interact with window
- Can be combined with `setOpacity()` to create pure visual effects
- Used for creating overlays or decorative effects

### setTransparentColor()

Set window transparent color.

```javascript
await puppet.window.setTransparentColor(color: string): Promise<void>
```

**Parameters**:

- `color` (string) - Color value, format `#RRGGBB`

**Examples**:

```javascript
// Set white as transparent color
await puppet.window.setTransparentColor('#FFFFFF');

// Set black as transparent color
await puppet.window.setTransparentColor('#000000');

// Set custom color
await puppet.window.setTransparentColor('#FF0000');
```

**Description**:

- Areas with specified color will be treated as transparent
- Must call `setTransparent(true)` first
- Supports hexadecimal color format

### setTopmost()

Set whether window is always on top.

```javascript
await puppet.window.setTopmost(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for always on top, `false` for not always on top

**Examples**:

```javascript
// Set window always on top
await puppet.window.setTopmost(true);

// Cancel always on top
await puppet.window.setTopmost(false);
```

**Usage**:

- Create tool windows that are always on top
- Create overlay or hint windows
- Create desktop widgets

### moveWindow()

Move window to specified position.

```javascript
await puppet.window.moveWindow(x: number, y: number): Promise<void>
```

**Parameters**:

- `x` (number) - X coordinate of window's top-left corner
- `y` (number) - Y coordinate of window's top-left corner

**Examples**:

```javascript
// Move to top-left corner of screen
await puppet.window.moveWindow(0, 0);

// Move to specified position
await puppet.window.moveWindow(100, 200);

// Use variables
const x = screen.width / 2 - windowWidth / 2;
const y = screen.height / 2 - windowHeight / 2;
await puppet.window.moveWindow(x, y);
```

### resizeWindow()

Adjust window size.

```javascript
await puppet.window.resizeWindow(width: number, height: number): Promise<void>
```

**Parameters**:

- `width` (number) - Window width (pixels)
- `height` (number) - Window height (pixels)

**Examples**:

```javascript
// Set to fixed size
await puppet.window.resizeWindow(800, 600);

// Fullscreen size
const width = screen.width;
const height = screen.height;
await puppet.window.resizeWindow(width, height);

// Adaptive based on content
const contentWidth = document.body.scrollWidth;
const contentHeight = document.body.scrollHeight;
await puppet.window.resizeWindow(contentWidth, contentHeight);
```

### centerWindow()

Center the window on screen.

```javascript
await puppet.window.centerWindow(): Promise<void>
```

**Examples**:

```javascript
// Center window
await puppet.window.centerWindow();

// Auto-center when window loads
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.centerWindow();
});
```

### showInTaskbar()

Set whether window shows in taskbar.

```javascript
await puppet.window.showInTaskbar(value: boolean): Promise<void>
```

**Parameters**:

- `value` (boolean) - `true` for show, `false` for hide

**Examples**:

```javascript
// Hide taskbar icon
await puppet.window.showInTaskbar(false);

// Show taskbar icon
await puppet.window.showInTaskbar(true);
```

**Usage**:

- Create applications that run in background
- Create tray applications
- Reduce taskbar interference

### mountMovableElement()

Mount draggable HTML element.

```javascript
await puppet.window.mountMovableElement(elementId: string): Promise<void>
```

**Parameters**:

- `elementId` (string) - HTML element ID

**Examples**:

```html
<div id="drag-handle" style="cursor: move;">
    Drag me
</div>

<script>
    // Mount drag element
    await puppet.window.mountMovableElement('drag-handle');
</script>
```

**Description**:

- Specified element becomes window's drag area
- Clicking and dragging this element can move window
- Can mount multiple elements

### unmountMovableElement()

Unmount draggable HTML element.

```javascript
await puppet.window.unmountMovableElement(elementId: string): Promise<void>
```

**Parameters**:

- `elementId` (string) - HTML element ID

**Examples**:

```javascript
// Unmount drag element
await puppet.window.unmountMovableElement('drag-handle');
```

## Usage Examples

### Create Borderless Transparent Window

```javascript
window.addEventListener('DOMContentLoaded', async () => {
    // Set borderless
    await puppet.window.setBorderless(true);
    
    // Enable dragging
    await puppet.window.setDraggable(true);
    
    // Enable transparency effects
    await puppet.window.setTransparent(true);
    
    // Set transparency
    await puppet.window.setOpacity(0.9);
    
    // Window always on top
    await puppet.window.setTopmost(true);
    
    // Hide taskbar icon
    await puppet.window.showInTaskbar(false);
});
```

### Create Custom Drag Area

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        * {
            margin: 0;
            padding: 0;
        }
        
        body {
            background: rgba(255, 255, 255, 0.8);
            backdrop-filter: blur(10px);
            border-radius: 12px;
            padding: 20px;
        }
        
        .title-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-bottom: 15px;
            border-bottom: 1px solid rgba(0, 0, 0, 0.1);
            margin-bottom: 15px;
        }
        
        .title {
            font-weight: bold;
            cursor: move;
        }
        
        .close-btn {
            cursor: pointer;
            padding: 5px 10px;
            background: #ff4444;
            color: white;
            border: none;
            border-radius: 4px;
        }
        
        .content {
            min-height: 200px;
        }
    </style>
</head>
<body>
    <div class="title-bar">
        <div class="title" id="drag-handle">My Application</div>
        <button class="close-btn" onclick="closeApp()">Close</button>
    </div>
    
    <div class="content">
        <p>This is the application content area</p>
    </div>

    <script>
        // Initialize window
        window.addEventListener('DOMContentLoaded', async () => {
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            await puppet.window.setTransparent(true);
            await puppet.window.setOpacity(0.95);
            
            // Mount custom drag area
            await puppet.window.mountMovableElement('drag-handle');
        });
        
        // Close application
        async function closeApp() {
            await puppet.application.close();
        }
    </script>
</body>
</html>
```

### Create Desktop Widget

```javascript
async function createWidget() {
    // Set window style
    await puppet.window.setBorderless(true);
    await puppet.window.setTransparent(true);
    await puppet.window.setOpacity(0.8);
    await puppet.window.setMouseThroughTransparency(true);
    await puppet.window.setTopmost(true);
    await puppet.window.showInTaskbar(false);
    
    // Adjust window size
    await puppet.window.resizeWindow(300, 400);
    
    // Move to bottom-right of desktop
    const screenInfo = await puppet.system.getScreenInfo();
    const x = screenInfo.width - 320;
    const y = screenInfo.height - 420;
    await puppet.window.moveWindow(x, y);
}

// Create widget
createWidget();
```

## Best Practices

### 0. Transparency Effects Best Practices ⚠️

**Important Note**: When implementing transparency effects, **strongly recommend using CSS instead of JavaScript methods**.

#### Recommended Approach: Use CSS

```css
/* In CSS set transparent background */
:root {
    background: transparent;
}

/* Or set on HTML element */
body {
    background: transparent;
}

/* Or for specific element */
.container {
    background: transparent;
    backdrop-filter: blur(10px);
}
```

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        /* Recommended: Use CSS to set transparency */
        body {
            background: transparent;
            backdrop-filter: blur(10px);
            border-radius: 12px;
        }
        
        .glass-effect {
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(20px);
            border: 1px solid rgba(255, 255, 255, 0.2);
        }
    </style>
</head>
<body>
    <div class="glass-effect">
        Glass effect content
    </div>
</body>
</html>
```

#### Not Recommended Approach: Use JavaScript Transparency Methods

```javascript
// ⚠️ Not recommended: Use setOpacity to set transparency
await puppet.window.setOpacity(0.5);

// ⚠️ Not recommended: Use setTransparent
await puppet.window.setTransparent(true);
```

#### Why Recommend CSS Approach?

| Feature | CSS Approach | JavaScript Approach |
|---------|--------------|---------------------|
| Performance | ✅ Better, GPU accelerated | ❌ Worse, frequent calls |
| Precise Control | ✅ Can control each element | ❌ Only controls entire window |
| Maintainability | ✅ CSS files centralized management | ❌ JavaScript code scattered |
| Compatibility | ✅ Standard CSS features | ❌ Platform dependent |
| Animation Effects | ✅ Supports CSS transitions and animations | ❌ Requires extra code to implement |

#### When to Use JavaScript Transparency Methods?

Only consider using JavaScript methods in these situations:

1. **Need entire window transparency control**:
```javascript
// Entire window transparency (not background color)
await puppet.window.setOpacity(0.9);
```

2. **Need window completely invisible**:
```javascript
// Window completely hidden but running
await puppet.window.setOpacity(0.0);
```

3. **Need transparent area click-through**:
```javascript
// Transparent areas can be clicked through
await puppet.window.setTransparent(true);
await puppet.window.setMouseThroughTransparency(true);
```

#### Mixed Usage Example

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        /* CSS handles background transparency and visual effects */
        body {
            background: transparent;
            margin: 0;
            padding: 0;
        }
        
        .container {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(20px);
            border-radius: 16px;
            padding: 24px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
            border: 1px solid rgba(255, 255, 255, 0.18);
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>My Application</h1>
        <p>Glass effect content</p>
    </div>

    <script>
        // JavaScript only handles window-level settings
        window.addEventListener('DOMContentLoaded', async () => {
            // Set window basic properties
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            
            // Window overall transparency (optional, as needed)
            await puppet.window.setOpacity(0.95);
        });
    </script>
</body>
</html>
```

::: tip Best Practice Summary
- **Background transparency**: Use CSS `background: transparent` on `:root` or `body`
- **Visual effects**: Use CSS `backdrop-filter` for glass effects
- **Window control**: Use JavaScript `setOpacity()` only for overall window transparency
- **Avoid mixing**: Don't use CSS background and JavaScript window transparency together, it causes confusion
:::

### 1. Window Initialization

Initialize window settings immediately after page loads:

```javascript
window.addEventListener('DOMContentLoaded', async () => {
    // Initialize window settings
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
    await puppet.window.centerWindow();
});
```

### 2. Performance Optimization

Avoid frequent window control method calls:

```javascript
// Not recommended: Frequently adjust transparency
setInterval(async () => {
    await puppet.window.setOpacity(Math.random());
}, 100);

// Recommended: Use animation and transition effects
.element {
    transition: opacity 0.3s ease;
}
```

### 3. User Experience

Provide window control options for users:

```javascript
// Add settings menu
const settingsMenu = [
    { text: 'Opacity 100%', action: () => puppet.window.setOpacity(1.0) },
    { text: 'Opacity 80%', action: () => puppet.window.setOpacity(0.8) },
    { text: 'Opacity 60%', action: () => puppet.window.setOpacity(0.6) },
    { text: 'Always on Top', action: () => puppet.window.setTopmost(true) },
    { text: 'Cancel Always on Top', action: () => puppet.window.setTopmost(false) }
];
```

### 4. Error Handling

Catch and handle possible errors:

```javascript
try {
    await puppet.window.setBorderless(true);
} catch (error) {
    console.error('Failed to set window style:', error);
    puppet.log.error('Window setting failed');
}
```

## Related Resources

- [Windows Window Styles](https://learn.microsoft.com/en-us/windows/win32/winmsg/window-styles): Windows API window styles
- [WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/): WebView2 official documentation
- [CSS Backdrop Filter](https://developer.mozilla.org/en-US/docs/Web/CSS/backdrop-filter): CSS background filter

## Common Questions

### Q: Why can't the window be dragged?

A: Please ensure:
1. Called `setBorderless(true)`
2. Called `setDraggable(true)`
3. Window is not completely transparent

### Q: How do I create a round window?

A: Use CSS `border-radius` and transparency effects:

```css
body {
    border-radius: 50%;
    background: transparent;
}
```

### Q: How do I make the window always below the mouse?

A: Use `setMouseThrough(true)`:

```javascript
await puppet.window.setMouseThrough(true);
await puppet.window.setOpacity(0.5);
```