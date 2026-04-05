---
title: 窗口控制 API
permalink: /api/window.html
createTime: 2026/03/28 15:02:53
---

# 窗口控制 API

窗口控制 API 提供了对应用窗口的完整控制能力，包括样式设置、位置调整、透明效果、点击穿透等功能。

## 概述

`puppet.window` 命名空间提供以下功能：

- 无边框窗口
- 窗口拖动和缩放
- 透明效果
- 点击穿透
- 窗口位置和大小控制
- 窗口置顶
- 任务栏显示控制
- 可拖动元素挂载

## 方法

### setBorderless()

设置窗口是否为无边框样式。

```javascript
await puppet.window.setBorderless(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示无边框，`false` 表示有边框

**示例**：

```javascript
// 设置为无边框窗口
await puppet.window.setBorderless(true);

// 恢复为有边框窗口
await puppet.window.setBorderless(false);
```

**注意事项**：

- 无边框窗口后需要使用 `setDraggable()` 启用拖动
- 无边框窗口会失去默认的窗口标题栏
- 建议在页面加载完成后立即设置

::: tip 推荐用法
```javascript
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
});
```
:::

### setDraggable()

设置窗口是否可拖动。

```javascript
await puppet.window.setDraggable(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示可拖动，`false` 表示不可拖动

**示例**：

```javascript
// 启用窗口拖动
await puppet.window.setDraggable(true);

// 禁用窗口拖动
await puppet.window.setDraggable(false);
```

**说明**：

- 拖动功能通过模拟点击窗口标题栏实现
- 无边框窗口必须启用此功能才能拖动
- 可以使用 `mountMovableElement()` 指定特定元素作为拖动区域

### setResizable()

设置窗口是否可缩放。

```javascript
await puppet.window.setResizable(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示可缩放，`false` 表示不可缩放

**示例**：

```javascript
// 启用窗口缩放
await puppet.window.setResizable(true);

// 禁用窗口缩放
await puppet.window.setResizable(false);
```

### setTransparent()

设置窗口是否支持透明效果。

```javascript
await puppet.window.setTransparent(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示支持透明，`false` 表示不支持

**示例**：

```javascript
// 启用透明效果
await puppet.window.setTransparent(true);

// 禁用透明效果
await puppet.window.setTransparent(false);
```

**说明**：

- 透明效果需要配合 `setOpacity()` 使用
- 启用后，窗口背景色为透明的区域会穿透点击
- 可以使用 `setTransparentColor()` 指定透明颜色

### setOpacity()

设置窗口透明度（0.0 - 1.0）。

```javascript
await puppet.window.setOpacity(value: number): Promise<void>
```

**参数**：

- `value` (number) - 透明度值，范围 0.0（完全透明）到 1.0（完全不透明）

**示例**：

```javascript
// 设置为 50% 透明
await puppet.window.setOpacity(0.5);

// 设置为 90% 透明
await puppet.window.setOpacity(0.9);

// 完全不透明
await puppet.window.setOpacity(1.0);
```

**注意事项**：

- 值必须在 0.0 到 1.0 之间
- 0.0 表示窗口完全不可见
- 建议不要低于 0.3，否则可能无法看到窗口

### setMouseThroughTransparency()

设置透明区域是否穿透点击。

```javascript
await puppet.window.setMouseThroughTransparency(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示穿透，`false` 表示不穿透

**示例**：

```javascript
// 启用透明区域点击穿透
await puppet.window.setMouseThroughTransparency(true);

// 禁用透明区域点击穿透
await puppet.window.setMouseThroughTransparency(false);
```

**说明**：

- 启用后，透明区域的鼠标点击会穿透到下层窗口
- 适用于桌面小部件、悬浮工具等场景
- 需要先调用 `setTransparent(true)`

### setMouseThrough()

设置整个窗口是否穿透点击。

```javascript
await puppet.window.setMouseThrough(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示穿透，`false` 表示不穿透

**示例**：

```javascript
// 使窗口完全穿透
await puppet.window.setMouseThrough(true);

// 恢复窗口交互
await puppet.window.setMouseThrough(false);
```

**注意事项**：

- 穿透后无法与窗口交互
- 可以配合 `setOpacity()` 创建纯视觉效果
- 用于创建覆盖层或装饰效果

### setTransparentColor()

设置窗口的透明色。

```javascript
await puppet.window.setTransparentColor(color: string): Promise<void>
```

**参数**：

- `color` (string) - 颜色值，格式为 `#RRGGBB`

**示例**：

```javascript
// 设置白色为透明色
await puppet.window.setTransparentColor('#FFFFFF');

// 设置黑色为透明色
await puppet.window.setTransparentColor('#000000');

// 设置自定义颜色
await puppet.window.setTransparentColor('#FF0000');
```

**说明**：

- 指定颜色的区域会被视为透明
- 需要先调用 `setTransparent(true)`
- 支持十六进制颜色格式

### setTopmost()

设置窗口是否置顶。

```javascript
await puppet.window.setTopmost(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示置顶，`false` 表示不置顶

**示例**：

```javascript
// 窗口置顶
await puppet.window.setTopmost(true);

// 取消置顶
await puppet.window.setTopmost(false);
```

**用途**：

- 创建始终在前的工具窗口
- 创建覆盖层或提示窗口
- 创建桌面小部件

### moveWindow()

移动窗口到指定位置。

```javascript
await puppet.window.moveWindow(x: number, y: number): Promise<void>
```

**参数**：

- `x` (number) - 窗口左上角的 X 坐标
- `y` (number) - 窗口左上角的 Y 坐标

**示例**：

```javascript
// 移动到屏幕左上角
await puppet.window.moveWindow(0, 0);

// 移动到指定位置
await puppet.window.moveWindow(100, 200);

// 使用变量
const x = screen.width / 2 - windowWidth / 2;
const y = screen.height / 2 - windowHeight / 2;
await puppet.window.moveWindow(x, y);
```

### resizeWindow()

调整窗口大小。

```javascript
await puppet.window.resizeWindow(width: number, height: number): Promise<void>
```

**参数**：

- `width` (number) - 窗口宽度（像素）
- `height` (number) - 窗口高度（像素）

**示例**：

```javascript
// 设置为固定大小
await puppet.window.resizeWindow(800, 600);

// 全屏大小
const width = screen.width;
const height = screen.height;
await puppet.window.resizeWindow(width, height);

// 根据内容自适应
const contentWidth = document.body.scrollWidth;
const contentHeight = document.body.scrollHeight;
await puppet.window.resizeWindow(contentWidth, contentHeight);
```

### centerWindow()

将窗口居中显示。

```javascript
await puppet.window.centerWindow(): Promise<void>
```

**示例**：

```javascript
// 居中窗口
await puppet.window.centerWindow();

// 在窗口加载时自动居中
window.addEventListener('DOMContentLoaded', async () => {
    await puppet.window.centerWindow();
});
```

### showInTaskbar()

设置窗口是否在任务栏显示。

```javascript
await puppet.window.showInTaskbar(value: boolean): Promise<void>
```

**参数**：

- `value` (boolean) - `true` 表示显示，`false` 表示隐藏

**示例**：

```javascript
// 隐藏任务栏图标
await puppet.window.showInTaskbar(false);

// 显示任务栏图标
await puppet.window.showInTaskbar(true);
```

**用途**：

- 创建后台运行的应用
- 创建托盘应用
- 减少任务栏干扰

### mountMovableElement()

挂载可拖动的 HTML 元素。

```javascript
await puppet.window.mountMovableElement(elementId: string): Promise<void>
```

**参数**：

- `elementId` (string) - HTML 元素的 ID

**示例**：

```html
<div id="drag-handle" style="cursor: move;">
    拖动我
</div>

<script>
    // 挂载拖动元素
    await puppet.window.mountMovableElement('drag-handle');
</script>
```

**说明**：

- 指定的元素将成为窗口的拖动区域
- 点击并拖动该元素可以移动窗口
- 可以挂载多个元素

### unmountMovableElement()

卸载可拖动的 HTML 元素。

```javascript
await puppet.window.unmountMovableElement(elementId: string): Promise<void>
```

**参数**：

- `elementId` (string) - HTML 元素的 ID

**示例**：

```javascript
// 卸载拖动元素
await puppet.window.unmountMovableElement('drag-handle');
```

## 使用示例

### 创建无边框透明窗口

```javascript
window.addEventListener('DOMContentLoaded', async () => {
    // 设置无边框
    await puppet.window.setBorderless(true);
    
    // 启用拖动
    await puppet.window.setDraggable(true);
    
    // 启用透明效果
    await puppet.window.setTransparent(true);
    
    // 设置透明度
    await puppet.window.setOpacity(0.9);
    
    // 窗口置顶
    await puppet.window.setTopmost(true);
    
    // 隐藏任务栏图标
    await puppet.window.showInTaskbar(false);
});
```

### 创建自定义拖动区域

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
        <div class="title" id="drag-handle">我的应用</div>
        <button class="close-btn" onclick="closeApp()">关闭</button>
    </div>
    
    <div class="content">
        <p>这是应用的内容区域</p>
    </div>

    <script>
        // 初始化窗口
        window.addEventListener('DOMContentLoaded', async () => {
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            await puppet.window.setTransparent(true);
            await puppet.window.setOpacity(0.95);
            
            // 挂载自定义拖动区域
            await puppet.window.mountMovableElement('drag-handle');
        });
        
        // 关闭应用
        async function closeApp() {
            await puppet.application.close();
        }
    </script>
</body>
</html>
```

### 创建桌面小部件

```javascript
async function createWidget() {
    // 设置窗口样式
    await puppet.window.setBorderless(true);
    await puppet.window.setTransparent(true);
    await puppet.window.setOpacity(0.8);
    await puppet.window.setMouseThroughTransparency(true);
    await puppet.window.setTopmost(true);
    await puppet.window.showInTaskbar(false);
    
    // 调整窗口大小
    await puppet.window.resizeWindow(300, 400);
    
    // 移动到桌面右下角
    const screenInfo = await puppet.system.getScreenInfo();
    const x = screenInfo.width - 320;
    const y = screenInfo.height - 420;
    await puppet.window.moveWindow(x, y);
}

// 创建小部件
createWidget();
```

## 最佳实践

### 0. 透明效果最佳实践 ⚠️

**重要提示**：实现透明效果时，推荐使用 CSS 而不是 JavaScript 方法。

#### 推荐方式：使用 CSS

```css
/* 在 CSS 中设置透明背景 */
:root {
    background: transparent;
}

/* 或者在 HTML 元素上设置 */
body {
    background: transparent;
}

/* 或者针对特定元素 */
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
        /* 推荐方式：使用 CSS 设置透明 */
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
        毛玻璃效果内容
    </div>
</body>
</html>
```

#### 不推荐方式：使用 JavaScript 透明方法

```javascript
// ⚠️ 不推荐：使用 setOpacity() 设置透明
await puppet.window.setOpacity(0.5);

// ⚠️ 不推荐：使用 setTransparent()
await puppet.window.setTransparent(true);
```

#### 为什么推荐 CSS 方式？

| 特性 | CSS 方式 | JavaScript 方式 |
|------|----------|-----------------|
| 性能 | ✅ 更好，由 GPU 加速 | ❌ 较差，频繁调用 |
| 灵活性 | ✅ 可以精确控制每个元素 | ❌ 只能控制整个窗口 |
| 可维护性 | ✅ CSS 文件集中管理 | ❌ JavaScript 代码分散 |
| 兼容性 | ✅ 标准 CSS 特性 | ❌ 平台依赖 |
| 动画效果 | ✅ 支持 CSS 过渡和动画 | ❌ 需要额外代码实现 |

#### 何时使用 JavaScript 透明方法？

只有在以下情况才考虑使用 JavaScript 方法：

1. **需要整个窗口的透明度控制**：
```javascript
// 整个窗口的透明度（不是背景色）
await puppet.window.setOpacity(0.9);
```

2. **需要窗口完全不可见**：
```javascript
// 窗口完全隐藏但保持运行
await puppet.window.setOpacity(0.0);
```

3. **需要透明区域点击穿透**：
```javascript
// 透明区域可以点击穿透
await puppet.window.setTransparent(true);
await puppet.window.setMouseThroughTransparency(true);
```

#### 混合使用示例

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        /* CSS 处理背景透明和视觉效果 */
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
        <h1>我的应用</h1>
        <p>毛玻璃效果内容</p>
    </div>

    <script>
        // JavaScript 只处理窗口级别设置
        window.addEventListener('DOMContentLoaded', async () => {
            // 设置窗口基本属性
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            
            // 窗口整体透明度（可选，根据需要）
            await puppet.window.setOpacity(0.95);
        });
    </script>
</body>
</html>
```

::: tip 最佳实践总结
- **背景透明**：使用 CSS `background: transparent` 挂载到 `:root` 或 `body`
- **视觉效果**：使用 CSS `backdrop-filter` 实现毛玻璃效果
- **窗口控制**：使用 JavaScript 的 `setOpacity()` 只控制整个窗口的透明度
- **避免混用**：不要同时使用 CSS 背景和 JavaScript 窗口透明，会导致混乱
:::

### 1. 窗口初始化

在页面加载完成后立即初始化窗口：

```javascript
window.addEventListener('DOMContentLoaded', async () => {
    // 初始化窗口设置
    await puppet.window.setBorderless(true);
    await puppet.window.setDraggable(true);
    await puppet.window.centerWindow();
});
```

### 2. 性能优化

避免频繁调用窗口控制方法：

```javascript
// 不推荐：频繁调整透明度
setInterval(async () => {
    await puppet.window.setOpacity(Math.random());
}, 100);

// 推荐：使用动画和过渡效果
.element {
    transition: opacity 0.3s ease;
}
```

### 3. 用户体验

为用户提供窗口控制选项：

```javascript
// 添加设置菜单
const settingsMenu = [
    { text: '透明度 100%', action: () => puppet.window.setOpacity(1.0) },
    { text: '透明度 80%', action: () => puppet.window.setOpacity(0.8) },
    { text: '透明度 60%', action: () => puppet.window.setOpacity(0.6) },
    { text: '置顶', action: () => puppet.window.setTopmost(true) },
    { text: '取消置顶', action: () => puppet.window.setTopmost(false) }
];
```

### 4. 错误处理

捕获和处理可能的错误：

```javascript
try {
    await puppet.window.setBorderless(true);
} catch (error) {
    console.error('设置窗口样式失败:', error);
    puppet.log.error('窗口设置失败');
}
```

## 相关资源

- [Windows 窗口样式](https://learn.microsoft.com/en-us/windows/win32/winmsg/window-styles)：Windows API 窗口样式
- [WebView2 文档](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)：WebView2 官方文档
- [CSS Backdrop Filter](https://developer.mozilla.org/en-US/docs/Web/CSS/backdrop-filter)：CSS 背景滤镜

## 常见问题

### Q: 为什么窗口无法拖动？

A: 请确保：
1. 调用了 `setBorderless(true)`
2. 调用了 `setDraggable(true)`
3. 窗口没有完全透明

### Q: 如何创建圆形窗口？

A: 使用 CSS `border-radius` 和透明效果：

```css
body {
    border-radius: 50%;
    background: transparent;
}
```

### Q: 如何让窗口始终在鼠标下方？

A: 使用 `setMouseThrough(true)`：

```javascript
await puppet.window.setMouseThrough(true);
await puppet.window.setOpacity(0.5);
```