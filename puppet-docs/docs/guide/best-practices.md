---
title: 最佳实践
permalink: /guide/best-practices.html
createTime: 2026/03/28 15:01:04
---

# 最佳实践

本章节汇集了开发高质量 Puppet 应用的最佳实践、设计模式和性能优化技巧，帮助您构建稳定、高效的应用程序。

## 代码组织

### 1. 模块化设计

将应用拆分为独立的模块，每个模块负责单一功能。

```javascript
// utils/api.js - API 封装
class ApiClient {
    async readFile(path) {
        return await puppet.fs.readFileAsText(path);
    }
    
    async writeFile(path, content) {
        return await puppet.fs.writeTextToFile(path, content);
    }
}

export const api = new ApiClient();

// app.js - 主应用
import { api } from './utils/api.js';

async function loadData() {
    const data = await api.readFile('data.json');
    return JSON.parse(data);
}
```

### 2. 配置管理

将配置与代码分离，便于管理和维护。

```javascript
// config.js - 配置文件
export const config = {
    appName: 'My App',
    version: '1.0.0',
    settings: {
        theme: 'light',
        language: 'zh-CN',
        autoUpdate: true
    },
    paths: {
        data: 'data/',
        cache: 'cache/',
        logs: 'logs/'
    }
};

// app.js - 使用配置
import { config } from './config.js';

async function initializeApp() {
    console.log('启动应用:', config.appName);
    await applySettings(config.settings);
}
```

### 3. 错误处理

实施全面的错误处理策略。

```javascript
// utils/errorHandler.js
class ErrorHandler {
    static handle(error, context = '') {
        // 记录错误
        puppet.log.error(`${context}: ${error.message}`);
        
        // 显示用户友好的消息
        showErrorMessage('操作失败，请重试');
        
        // 上报错误（可选）
        reportError(error);
    }
}

// 使用错误处理
try {
    await performOperation();
} catch (error) {
    ErrorHandler.handle(error, '加载文件');
}
```

## 窗口透明效果最佳实践 ⚠️

### 重要：优先使用 CSS 实现透明效果

在 Puppet 应用中实现透明效果时，**强烈推荐使用 CSS 而不是 JavaScript 方法**。

#### 推荐的实现方式

```html
<!DOCTYPE html>
<html>
<head>
    <style>
        /* ✅ 推荐：使用 CSS 设置透明背景 */
        :root {
            background: transparent;
        }

        /* ✅ 推荐：在 body 上设置透明 */
        body {
            background: transparent;
            margin: 0;
            padding: 0;
        }

        /* ✅ 推荐：使用 backdrop-filter 实现毛玻璃效果 */
        .glass-container {
            background: rgba(255, 255, 255, 0.1);
            backdrop-filter: blur(20px);
            border-radius: 16px;
            padding: 24px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
            border: 1px solid rgba(255, 255, 255, 0.18);
        }

        /* ✅ 推荐：使用 CSS 过渡实现平滑效果 */
        .smooth-transition {
            transition: all 0.3s ease;
        }

        .smooth-transition:hover {
            background: rgba(255, 255, 255, 0.2);
            transform: translateY(-2px);
        }
    </style>
</head>
<body>
    <div class="glass-container smooth-transition">
        <h1>我的应用</h1>
        <p>使用 CSS 实现的毛玻璃效果</p>
    </div>

    <script>
        window.addEventListener('DOMContentLoaded', async () => {
            // JavaScript 只用于窗口级别控制
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            
            // 可选：设置窗口整体透明度（不是背景）
            await puppet.window.setOpacity(0.95);
        });
    </script>
</body>
</html>
```

#### 不推荐的方式

```javascript
// ❌ 不推荐：使用 JavaScript 设置背景透明
await puppet.window.setTransparent(true);

// ❌ 不推荐：使用 JavaScript 设置透明度来实现背景效果
await puppet.window.setOpacity(0.5);
```

#### 为什么必须使用 CSS？

| 对比项 | CSS 方式 | JavaScript 方式 |
|--------|----------|-----------------|
| **性能** | ✅ GPU 加速，性能优秀 | ❌ 频繁调用，性能较差 |
| **精确控制** | ✅ 可以控制每个元素 | ❌ 只能控制整个窗口 |
| **动画效果** | ✅ 原生 CSS 过渡和动画 | ❌ 需要额外代码实现 |
| **维护性** | ✅ CSS 文件集中管理 | ❌ JavaScript 代码分散 |
| **标准性** | ✅ 标准 Web 技术 | ❌ 平台特定实现 |
| **灵活性** | ✅ 支持 CSS 变量和主题 | ❌ 硬编码，难以调整 |

#### 正确的透明效果实现层次

```
┌─────────────────────────────────────────┐
│  第 1 层：CSS 背景（推荐）              │
│  background: transparent               │
│  backdrop-filter: blur(10px)           │
├─────────────────────────────────────────┤
│  第 2 层：CSS 元素样式（推荐）          │
│  background: rgba(255, 255, 255, 0.1)  │
│  border-radius, box-shadow             │
├─────────────────────────────────────────┤
│  第 3 层：JavaScript 窗口控制（慎用）    │
│  puppet.window.setOpacity(0.95)        │
│  仅用于窗口整体透明度，而非背景        │
└─────────────────────────────────────────┘
```

#### 使用场景指南

**✅ 使用 CSS 的情况（推荐）**：

```css
/* 1. 背景透明 */
body {
    background: transparent;
}

/* 2. 毛玻璃效果 */
.glass {
    background: rgba(255, 255, 255, 0.1);
    backdrop-filter: blur(20px);
}

/* 3. 元素透明度 */
.element {
    opacity: 0.8;
}

/* 4. 过渡动画 */
.element {
    transition: opacity 0.3s ease;
}
```

**⚠️ 使用 JavaScript 的情况（特殊需求）**：

```javascript
// 1. 整个窗口的透明度控制（不是背景）
await puppet.window.setOpacity(0.95);

// 2. 窗口完全隐藏但保持运行
await puppet.window.setOpacity(0.0);

// 3. 透明区域点击穿透（配合 CSS 使用）
await puppet.window.setTransparent(true);
await puppet.window.setMouseThroughTransparency(true);
```

#### 完整示例

```html
<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <style>
        /* CSS 设置透明背景 */
        :root {
            background: transparent;
        }

        body {
            background: transparent;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 20px;
        }

        /* 主容器 - 毛玻璃效果 */
        .app-container {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(20px);
            border-radius: 16px;
            padding: 24px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
            border: 1px solid rgba(255, 255, 255, 0.18);
            min-height: 400px;
        }

        /* 标题栏 */
        .title-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            padding-bottom: 15px;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        }

        .title {
            font-size: 18px;
            font-weight: 600;
            color: rgba(255, 255, 255, 0.9);
            cursor: move;
        }

        /* 内容区域 */
        .content {
            color: rgba(255, 255, 255, 0.85);
        }

        /* 按钮样式 */
        .button {
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            color: white;
            padding: 8px 16px;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .button:hover {
            background: rgba(255, 255, 255, 0.3);
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
    </style>
</head>
<body>
    <div class="app-container">
        <div class="title-bar">
            <div class="title" id="drag-handle">🎭 Puppet 应用</div>
            <button class="button" onclick="closeApp()">关闭</button>
        </div>
        
        <div class="content">
            <h2>欢迎使用 Puppet</h2>
            <p>这是一个使用 CSS 实现毛玻璃效果的示例。</p>
            <p>背景透明度和视觉效果完全由 CSS 控制，性能更优，维护更方便。</p>
            
            <div style="margin-top: 20px;">
                <button class="button" onclick="showInfo()">查看信息</button>
                <button class="button" onclick="changeTheme()">切换主题</button>
            </div>
        </div>
    </div>

    <script>
        // JavaScript 只处理窗口级别控制
        window.addEventListener('DOMContentLoaded', async () => {
            await puppet.window.setBorderless(true);
            await puppet.window.setDraggable(true);
            await puppet.window.mountMovableElement('drag-handle');
            
            // 可选：窗口整体透明度
            await puppet.window.setOpacity(0.95);
            
            // 窗口置顶
            await puppet.window.setTopmost(true);
        });

        async function closeApp() {
            await puppet.application.close();
        }

        function showInfo() {
            alert('此应用使用 CSS 实现透明效果！');
        }

        function changeTheme() {
            document.body.style.filter = 
                document.body.style.filter ? '' : 'hue-rotate(180deg)';
        }
    </script>
</body>
</html>
```

::: tip 记住
- **背景透明**：使用 `background: transparent` 挂载到 `:root` 或 `body`
- **视觉效果**：使用 CSS `backdrop-filter` 和 `rgba` 颜色
- **动画效果**：使用 CSS `transition` 和 `animation`
- **窗口控制**：JavaScript 的 `setOpacity()` 只用于窗口整体透明度
:::

## 性能优化

### 1. 延迟加载

按需加载资源，减少初始加载时间。

```javascript
// 懒加载 JavaScript 模块
async function loadFeature(featureName) {
    const module = await import(`./features/${featureName}.js`);
    return module.default;
}

// 按需加载功能
button.addEventListener('click', async () => {
    const feature = await loadFeature('advanced');
    feature.initialize();
});
```

### 2. 资源优化

优化图片和媒体文件大小。

```html
<!-- 使用合适的图片格式 -->
<picture>
    <source srcset="image.webp" type="image/webp">
    <source srcset="image.jpg" type="image/jpeg">
    <img src="image.jpg" alt="图片" loading="lazy">
</picture>

<!-- 使用压缩的字体 -->
<link rel="preload" href="fonts/Roboto-Regular.woff2" as="font" type="font/woff2" crossorigin>
```

### 3. 缓存策略

合理使用缓存减少重复加载。

```javascript
// 缓存文件内容
const cache = new Map();

async function getCachedFile(path) {
    if (cache.has(path)) {
        return cache.get(path);
    }
    
    const content = await puppet.fs.readFileAsText(path);
    cache.set(path, content);
    return content;
}

// 清除缓存
function clearCache() {
    cache.clear();
}
```

### 4. 批量操作

合并多个操作，减少通信开销。

```javascript
// 不推荐：多次调用
for (const file of files) {
    await puppet.fs.readFileAsText(file);
}

// 推荐：批量处理
async function batchReadFiles(files) {
    const results = await Promise.all(
        files.map(file => puppet.fs.readFileAsText(file))
    );
    return results;
}
```

## 用户体验

### 1. 响应式设计

确保应用在不同尺寸窗口中都能正常显示。

```css
/* 响应式布局 */
.container {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 20px;
}

@media (max-width: 768px) {
    .container {
        grid-template-columns: 1fr;
    }
}
```

### 2. 加载状态

为耗时操作提供加载反馈。

```javascript
async function loadData() {
    showLoading('正在加载数据...');
    
    try {
        const data = await fetchRemoteData();
        renderData(data);
    } finally {
        hideLoading();
    }
}

function showLoading(message) {
    const loader = document.createElement('div');
    loader.className = 'loading-spinner';
    loader.textContent = message;
    document.body.appendChild(loader);
}

function hideLoading() {
    const loader = document.querySelector('.loading-spinner');
    if (loader) loader.remove();
}
```

### 3. 错误提示

提供清晰、友好的错误提示。

```javascript
function showError(message, details = '') {
    const toast = document.createElement('div');
    toast.className = 'toast error';
    toast.innerHTML = `
        <div class="toast-message">${message}</div>
        ${details ? `<div class="toast-details">${details}</div>` : ''}
    `;
    
    document.body.appendChild(toast);
    
    // 3秒后自动消失
    setTimeout(() => toast.remove(), 3000);
}

// 使用示例
try {
    await riskyOperation();
} catch (error) {
    showError('操作失败', error.message);
}
```

### 4. 键盘快捷键

实现键盘快捷键提升效率。

```javascript
// 快捷键映射
const shortcuts = {
    'Ctrl+S': saveFile,
    'Ctrl+O': openFile,
    'Ctrl+Q': quitApp,
    'F5': refresh
};

// 监听键盘事件
document.addEventListener('keydown', (e) => {
    const key = getShortcutKey(e);
    
    if (shortcuts[key]) {
        e.preventDefault();
        shortcuts[key]();
    }
});

function getShortcutKey(e) {
    const modifiers = [];
    if (e.ctrlKey) modifiers.push('Ctrl');
    if (e.altKey) modifiers.push('Alt');
    if (e.shiftKey) modifiers.push('Shift');
    
    modifiers.push(e.key.toUpperCase());
    return modifiers.join('+');
}
```

## 安全实践

### 1. 输入验证

始终验证用户输入。

```javascript
function validateFilePath(path) {
    // 检查路径格式
    if (!path || typeof path !== 'string') {
        throw new Error('无效的路径');
    }
    
    // 检查路径长度
    if (path.length > 260) {
        throw new Error('路径过长');
    }
    
    // 检查非法字符
    const invalidChars = /[<>:"|?*]/;
    if (invalidChars.test(path)) {
        throw new Error('路径包含非法字符');
    }
    
    return true;
}

// 使用验证
async function safeReadFile(path) {
    validateFilePath(path);
    return await puppet.fs.readFileAsText(path);
}
```

### 2. 权限控制

最小权限原则，只请求必要的权限。

```javascript
// 只在需要时请求权限
async function saveConfig(config) {
    const configPath = await getConfigPath();
    return await puppet.fs.writeTextToFile(configPath, JSON.stringify(config));
}

// 避免直接操作系统文件
// 不推荐：
// await puppet.fs.writeTextToFile('C:\\Windows\\config.ini', data);
```

### 3. 数据加密

保护敏感数据。

```javascript
// 加密函数
async function encryptData(data, key) {
    const encoder = new TextEncoder();
    const dataBytes = encoder.encode(data);
    
    const keyBytes = await crypto.subtle.importKey(
        'raw',
        encoder.encode(key),
        { name: 'AES-GCM' },
        false,
        ['encrypt']
    );
    
    const iv = crypto.getRandomValues(new Uint8Array(12));
    const encrypted = await crypto.subtle.encrypt(
        { name: 'AES-GCM', iv },
        keyBytes,
        dataBytes
    );
    
    return {
        iv: Array.from(iv),
        data: Array.from(new Uint8Array(encrypted))
    };
}
```

## 调试技巧

### 1. 日志记录

使用分级日志记录重要事件。

```javascript
// 日志级别
const LogLevel = {
    DEBUG: 0,
    INFO: 1,
    WARN: 2,
    ERROR: 3
};

// 日志函数
function log(level, message, data = null) {
    const timestamp = new Date().toISOString();
    const logEntry = {
        timestamp,
        level,
        message,
        data
    };
    
    // 输出到控制台
    console.log(`[${timestamp}] [${level}] ${message}`, data || '');
    
    // 输出到 Puppet 日志
    switch (level) {
        case LogLevel.INFO:
            puppet.log.info(message);
            break;
        case LogLevel.WARN:
            puppet.log.warn(message);
            break;
        case LogLevel.ERROR:
            puppet.log.error(message);
            break;
    }
    
    // 保存到文件
    saveLogToFile(logEntry);
}

// 使用示例
log(LogLevel.INFO, '应用启动', { version: '1.0.0' });
log(LogLevel.WARN, '配置缺失', { key: 'apiKey' });
log(LogLevel.ERROR, '操作失败', { error: error.message });
```

### 2. 性能监控

监控应用性能。

```javascript
// 性能监控
class PerformanceMonitor {
    constructor() {
        this.metrics = new Map();
    }
    
    start(label) {
        this.metrics.set(label, performance.now());
    }
    
    end(label) {
        const startTime = this.metrics.get(label);
        if (!startTime) return;
        
        const duration = performance.now() - startTime;
        console.log(`${label}: ${duration.toFixed(2)}ms`);
        
        this.metrics.delete(label);
        return duration;
    }
    
    measureAsync(label, asyncFn) {
        this.start(label);
        const result = await asyncFn();
        this.end(label);
        return result;
    }
}

// 使用示例
const monitor = new PerformanceMonitor();

await monitor.measureAsync('loadData', async () => {
    return await loadData();
});
```

### 3. 错误追踪

追踪错误来源。

```javascript
// 错误追踪
class ErrorTracker {
    static track(error, context = {}) {
        const errorInfo = {
            message: error.message,
            stack: error.stack,
            timestamp: new Date().toISOString(),
            context
        };
        
        // 记录错误
        puppet.log.error(JSON.stringify(errorInfo));
        
        // 显示错误
        showError(error.message);
        
        // 上报错误（可选）
        this.report(errorInfo);
    }
    
    static report(errorInfo) {
        // 发送到错误追踪服务
        // fetch('https://error-tracking.com/api', {
        //     method: 'POST',
        //     body: JSON.stringify(errorInfo)
        // });
    }
}

// 使用示例
try {
    await riskyOperation();
} catch (error) {
    ErrorTracker.track(error, { action: 'loadData' });
}
```

## 测试策略

### 1. 单元测试

测试独立的功能模块。

```javascript
// 测试示例
describe('ApiClient', () => {
    it('应该成功读取文件', async () => {
        const client = new ApiClient();
        const content = await client.readFile('test.txt');
        expect(content).toBe('test content');
    });
    
    it('应该处理文件不存在错误', async () => {
        const client = new ApiClient();
        await expect(client.readFile('nonexistent.txt'))
            .rejects.toThrow('文件不存在');
    });
});
```

### 2. 集成测试

测试模块间的交互。

```javascript
describe('应用集成测试', () => {
    it('应该完整加载应用', async () => {
        await initializeApp();
        const isLoaded = checkAppLoaded();
        expect(isLoaded).toBe(true);
    });
});
```

### 3. 手动测试

测试关键功能和用户流程。

**测试清单**：

- [ ] 应用启动正常
- [ ] 所有功能按钮可用
- [ ] 文件读写正常
- [ ] 错误处理正确
- [ ] 性能可接受
- [ ] 窗口样式正确

## 部署建议

### 1. 版本管理

使用语义化版本号。

```
主版本号.次版本号.修订号 (MAJOR.MINOR.PATCH)

示例：1.2.3
- 1：主版本号（不兼容的 API 修改）
- 2：次版本号（向下兼容的功能新增）
- 3：修订号（向下兼容的问题修正）
```

### 2. 发布流程

标准发布流程：

1. **开发阶段**
   - 使用裸文件夹模式开发
   - 实施代码审查
   - 编写测试用例

2. **测试阶段**
   - 创建 PUP 文件
   - 进行功能测试
   - 性能测试

3. **发布阶段**
   - 更新版本号
   - 编写更新日志
   - 分发 PUP 文件

4. **维护阶段**
   - 监控错误日志
   - 收集用户反馈
   - 修复问题

### 3. 更新机制

实现自动更新功能。

```javascript
async function checkForUpdates() {
    const currentVersion = await getCurrentVersion();
    const latestVersion = await fetchLatestVersion();
    
    if (compareVersions(latestVersion, currentVersion) > 0) {
        showUpdateNotification(latestVersion);
    }
}

function compareVersions(v1, v2) {
    const parts1 = v1.split('.').map(Number);
    const parts2 = v2.split('.').map(Number);
    
    for (let i = 0; i < parts1.length; i++) {
        if (parts1[i] > parts2[i]) return 1;
        if (parts1[i] < parts2[i]) return -1;
    }
    
    return 0;
}
```

## 文档和注释

### 1. 代码注释

为复杂逻辑添加注释。

```javascript
/**
 * 读取并解析配置文件
 * @param {string} path - 配置文件路径
 * @returns {Promise<Object>} 配置对象
 * @throws {Error} 文件不存在或格式错误时抛出异常
 */
async function loadConfig(path) {
    try {
        // 读取文件内容
        const content = await puppet.fs.readFileAsText(path);
        
        // 解析 JSON
        const config = JSON.parse(content);
        
        // 验证配置
        validateConfig(config);
        
        return config;
    } catch (error) {
        throw new Error(`加载配置失败: ${error.message}`);
    }
}
```

### 2. API 文档

为公共 API 编写文档。

```javascript
/**
 * @module ApiClient
 * @description 提供 Puppet API 的封装
 */

/**
 * 读取文件内容
 * @param {string} path - 文件路径
 * @returns {Promise<string>} 文件内容
 * @example
 * const content = await api.readFile('data.txt');
 */
async function readFile(path) {
    return await puppet.fs.readFileAsText(path);
}
```

### 3. README

为项目编写详细的 README。

```markdown
# My Puppet App

## 简介

这是一个基于 Puppet 框架的桌面应用。

## 功能

- 功能 1
- 功能 2
- 功能 3

## 安装

1. 下载 PUP 文件
2. 运行 puppet.exe --load-pup app.pup

## 使用

详细使用说明请参考 [文档](./docs/)

## 开发

```bash
# 开发模式
puppet.exe --nake-load .\dist

# 构建 PUP 文件
puppet.exe --create-pup -i .\dist -o app.pup
```

## 许可证

MIT
```

## 相关资源

- [API 文档](../api/) - 完整的 API 参考
- [安全机制](./security.md) - 安全最佳实践
- [快速开始](./getting-started.md) - 快速上手指南

## 下一步

遵循最佳实践后，建议：

1. 定期审查和优化代码
2. 持续学习新的技术和模式
3. 收集用户反馈并改进
4. 参考开源项目获取灵感