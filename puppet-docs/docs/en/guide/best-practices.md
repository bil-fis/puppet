---
title: Best Practices
permalink: /en/guide/best-practices.html
createTime: 2026/03/28 15:01:04
---

# Best Practices

This chapter compiles best practices, design patterns, and performance optimization techniques for developing high-quality Puppet applications, helping you build stable and efficient applications.

## Code Organization

### 1. Modular Design

Break your application into independent modules, each responsible for a single function.

```javascript
// utils/api.js - API wrapper
class ApiClient {
    async readFile(path) {
        return await puppet.fs.readFileAsText(path);
    }
    
    async writeFile(path, content) {
        return await puppet.fs.writeTextToFile(path, content);
    }
}

export const api = new ApiClient();

// app.js - Main application
import { api } from './utils/api.js';

async function loadData() {
    const data = await api.readFile('data.json');
    return JSON.parse(data);
}
```

### 2. Configuration Management

Separate configuration from code for easier management and maintenance.

```javascript
// config.js - Configuration file
export const config = {
    appName: 'My App',
    version: '1.0.0',
    settings: {
        theme: 'light',
        language: 'en-US',
        autoUpdate: true
    },
    paths: {
        data: 'data/',
        cache: 'cache/',
        logs: 'logs/'
    }
};

// app.js - Use configuration
import { config } from './config.js';

async function initializeApp() {
    console.log('Starting application:', config.appName);
    await applySettings(config.settings);
}
```

### 3. Error Handling

Implement comprehensive error handling strategy.

```javascript
// utils/errorHandler.js
class ErrorHandler {
    static handle(error, context = '') {
        // Log error
        puppet.log.error(`${context}: ${error.message}`);
        
        // Show user-friendly message
        showErrorMessage('Operation failed, please try again');
        
        // Report error (optional)
        reportError(error);
    }
}

// Use error handling
try {
    await performOperation();
} catch (error) {
    ErrorHandler.handle(error, 'Loading file');
}
```

## Window Transparency Best Practices ⚠️

### Important: Use CSS for Transparency Effects

When implementing transparency effects in Puppet applications, **strongly recommend using CSS instead of JavaScript methods**.

#### Recommended Approach: Use CSS

```css
/* ✅ Recommended: Use CSS to set transparent background */
:root {
    background: transparent;
}

/* ✅ Recommended: Set transparency on body */
body {
    background: transparent;
    margin: 0;
    padding: 0;
}

/* ✅ Recommended: Use backdrop-filter for glass effect */
.glass-container {
    background: rgba(255, 255, 255, 0.1);
    backdrop-filter: blur(20px);
    border-radius: 16px;
    padding: 24px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
    border: 1px solid rgba(255, 255, 255, 0.18);
}

/* ✅ Recommended: Use CSS transitions for smooth effects */
.smooth-transition {
    transition: all 0.3s ease;
}

.smooth-transition:hover {
    background: rgba(255, 255, 255, 0.2);
    transform: translateY(-2px);
}
```

#### Not Recommended Approach: Use JavaScript

```javascript
// ❌ Not recommended: Use setOpacity to set transparency
await puppet.window.setOpacity(0.5);

// ❌ Not recommended: Use setTransparent
await puppet.window.setTransparent(true);
```

#### Why Recommend CSS Approach?

| Feature | CSS Approach | JavaScript Approach |
|---------|--------------|---------------------|
| **Performance** | ✅ Better, GPU accelerated | ❌ Worse, frequent calls |
| **Precise Control** | ✅ Can control each element | ❌ Only controls entire window |
| **Animation Effects** | ✅ Native CSS transitions and animations | ❌ Requires extra code to implement |
| **Maintainability** | ✅ CSS files centralized management | ❌ JavaScript code scattered |
| **Standard Compliance** | ✅ Standard CSS features | ❌ Platform dependent |
| **Flexibility** | ✅ Supports CSS variables and themes | ❌ Hardcoded, difficult to adjust |

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

## Performance Optimization

### 1. Lazy Loading

Load resources on demand to reduce initial load time.

```javascript
// Lazy load JavaScript modules
async function loadFeature(featureName) {
    const module = await import(`./features/${featureName}.js`);
    return module.default;
}

// Load features on demand
button.addEventListener('click', async () => {
    const feature = await loadFeature('advanced');
    feature.initialize();
});
```

### 2. Resource Optimization

Optimize image and media file sizes.

```html
<!-- Use appropriate image formats -->
<picture>
    <source srcset="image.webp" type="image/webp">
    <source srcset="image.jpg" type="image/jpeg">
    <img src="image.jpg" alt="Image" loading="lazy">
</picture>

<!-- Use compressed fonts -->
<link rel="preload" href="fonts/Roboto-Regular.woff2" as="font" type="font/woff2" crossorigin>
```

### 3. Caching Strategy

Reasonably use caching to reduce repeated loading.

```javascript
// Cache file content
const cache = new Map();

async function getCachedFile(path) {
    if (cache.has(path)) {
        return cache.get(path);
    }
    
    const content = await puppet.fs.readFileAsText(path);
    cache.set(path, content);
    return content;
}

// Clear cache
function clearCache() {
    cache.clear();
}
```

### 4. Batch Operations

Combine multiple operations to reduce communication overhead.

```javascript
// Not recommended: Multiple calls
for (const file of files) {
    await puppet.fs.readFileAsText(file);
}

// Recommended: Batch processing
async function batchReadFiles(files) {
    const results = await Promise.all(
        files.map(file => puppet.fs.readFileAsText(file))
    );
    return results;
}
```

## User Experience

### 1. Responsive Design

Ensure application displays correctly on different window sizes.

```css
/* Responsive layout */
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

### 2. Loading States

Provide loading feedback for time-consuming operations.

```javascript
async function loadData() {
    showLoading('Loading data...');
    
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

### 3. Error Prompts

Provide clear, friendly error messages.

```javascript
function showError(message, details = '') {
    const toast = document.createElement('div');
    toast.className = 'toast error';
    toast.innerHTML = `
        <div class="toast-message">${message}</div>
        ${details ? `<div class="toast-details">${details}</div>` : ''}
    `;
    
    document.body.appendChild(toast);
    
    // Auto disappear after 3 seconds
    setTimeout(() => toast.remove(), 3000);
}

// Usage example
try {
    await riskyOperation();
} catch (error) {
    showError('Operation failed', error.message);
}
```

### 4. Keyboard Shortcuts

Implement keyboard shortcuts to improve efficiency.

```javascript
// Shortcut mapping
const shortcuts = {
    'Ctrl+S': saveFile,
    'Ctrl+O': openFile,
    'Ctrl+Q': quitApp,
    'F5': refresh
};

// Listen for keyboard events
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

## Security Practices

### 1. Input Validation

Always validate user input.

```javascript
function validateFilePath(path) {
    // Check path format
    if (!path || typeof path !== 'string') {
        throw new Error('Invalid path');
    }
    
    // Check path length
    if (path.length > 260) {
        throw new Error('Path too long');
    }
    
    // Check illegal characters
    const invalidChars = /[<>:"|?*]/;
    if (invalidChars.test(path)) {
        throw new Error('Path contains illegal characters');
    }
    
    return true;
}

// Use validation
async function safeReadFile(path) {
    validateFilePath(path);
    return await puppet.fs.readFileAsText(path);
}
```

### 2. Permission Control

Principle of least privilege, only request necessary permissions.

```javascript
// Only request permissions when needed
async function saveConfig(config) {
    const configPath = await getConfigPath();
    return await puppet.fs.writeTextToFile(configPath, JSON.stringify(config));
}

// Avoid direct system file operations
// Not recommended:
// await puppet.fs.writeTextToFile('C:\\Windows\\config.ini', data);
```

### 3. Data Encryption

Protect sensitive data.

```javascript
// Encryption function
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

### 4. PUP File Signature

Use digital signature functionality of V1.2 format to ensure application integrity and source credibility.

```javascript
// Check if PUP file is signed
async function checkPupSignature() {
    try {
        const appInfo = await puppet.application.getAppInfo();
        
        if (appInfo.hasSignature) {
            console.log('PUP file is signed');
            console.log('Certificate fingerprint:', appInfo.certificateThumbprint);
            console.log('Certificate issuer:', appInfo.certificateIssuer);
            return true;
        } else {
            console.warn('PUP file is not signed, may have security risks');
            return false;
        }
    } catch (error) {
        console.error('Signature check failed:', error.message);
        return false;
    }
}

// Verify signature when application starts
window.addEventListener('DOMContentLoaded', async () => {
    const isSigned = await checkPupSignature();
    
    if (!isSigned) {
        // Can choose to refuse running unsigned applications
        // document.body.innerHTML = '<h1>Security Warning: Application Not Signed</h1>';
    }
});
```

**Create Signed PUP File**:

```bash
# 1. Use puppet-sign to generate signing key pair
puppet-sign.exe --generate-signing-key --alias MyApp --key-size 2048

# 2. Create signed PUP file
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"

# 3. Sign database (optional)
puppet-sign.exe --sign-database --database "C:\MyProject\data.db" --private-key "app.key" --private-key-password "MyPassword"
```

**Security Recommendations**:

- Always create digital signatures for production environment PUP files
- Use strong passwords to protect private key files
- Regularly rotate signing key pairs
- Verify signature status when application starts
- Refuse to run PUP files from untrusted sources

### 5. Database Signature Verification

Ensure database has not been tampered with.

```javascript
// Check database signature status
async function checkDatabaseSignature(databaseName) {
    try {
        const result = await puppet.storage.verifyDatabaseSignature(databaseName);
        
        if (result.isValid) {
            console.log(`Database ${databaseName} signature verification passed`);
            return true;
        } else {
            console.error(`Database ${databaseName} signature verification failed: ${result.message}`);
            return false;
        }
    } catch (error) {
        console.error('Database signature check failed:', error.message);
        return false;
    }
}

// Use signed database
async function useSecureDatabase() {
    // First check signature
    const isSecure = await checkDatabaseSignature('main');
    
    if (!isSecure) {
        throw new Error('Database signature verification failed, access denied');
    }
    
    // Normal use after verification passes
    const result = await puppet.storage.executeSQL('main', 'SELECT * FROM users');
    return result;
}
```

## Debugging Tips

### 1. Logging

Use hierarchical logging to record important events.

```javascript
// Log levels
const LogLevel = {
    DEBUG: 0,
    INFO: 1,
    WARN: 2,
    ERROR: 3
};

// Log function
function log(level, message, data = null) {
    const timestamp = new Date().toISOString();
    const logEntry = {
        timestamp,
        level,
        message,
        data
    };
    
    // Output to console
    console.log(`[${timestamp}] [${level}] ${message}`, data || '');
    
    // Output to Puppet log
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
    
    // Save to file
    saveLogToFile(logEntry);
}

// Usage example
log(LogLevel.INFO, 'Application started', { version: '1.0.0' });
log(LogLevel.WARN, 'Configuration missing', { key: 'apiKey' });
log(LogLevel.ERROR, 'Operation failed', { error: error.message });
}
```

### 2. Performance Monitoring

Monitor application performance.

```javascript
// Performance monitoring
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

// Usage example
const monitor = new PerformanceMonitor();

await monitor.measureAsync('loadData', async () => {
    return await loadData();
});
```

### 3. Error Tracking

Track error sources.

```javascript
// Error tracking
class ErrorTracker {
    static track(error, context = {}) {
        const errorInfo = {
            message: error.message,
            stack: error.stack,
            timestamp: new Date().toISOString(),
            context
        };
        
        // Log error
        puppet.log.error(JSON.stringify(errorInfo));
        
        // Display error
        showError(error.message);
        
        // Report error (optional)
        this.report(errorInfo);
    }
    
    static report(errorInfo) {
        // Send to error tracking service
        // fetch('https://error-tracking.com/api', {
        //     method: 'POST',
        //     body: JSON.stringify(errorInfo)
        // });
    }
}

// Usage example
try {
    await riskyOperation();
} catch (error) {
    ErrorTracker.track(error, { action: 'loadData' });
}
```

## Testing Strategy

### 1. Unit Testing

Test independent functional modules.

```javascript
// Test example
describe('ApiClient', () => {
    it('should successfully read file', async () => {
        const client = new ApiClient();
        const content = await client.readFile('test.txt');
        expect(content).toBe('test content');
    });
    
    it('should handle file not found error', async () => {
        const client = new ApiClient();
        await expect(client.readFile('nonexistent.txt'))
            .rejects.toThrow('File not found');
    });
});
```

### 2. Integration Testing

Test interactions between modules.

```javascript
describe('Application Integration Test', () => {
    it('should completely load application', async () => {
        await initializeApp();
        const isLoaded = checkAppLoaded();
        expect(isLoaded).toBe(true);
    });
});
```

### 3. Manual Testing

Test key features and user workflows.

**Test Checklist**:

- [ ] Application starts normally
- [ ] All function buttons are available
- [ ] File read/write works normally
- [ ] Error handling is correct
- [ ] Performance is acceptable
- [ ] Window style is correct

## Deployment Recommendations

### 1. Version Management

Use semantic versioning.

```
MAJOR.MINOR.PATCH

Example: 1.2.3
- 1: Major version (incompatible API changes)
- 2: Minor version (backward compatible feature additions)
- 3: Patch version (backward compatible bug fixes)
```

### 2. Release Process

Standard release process:

1. **Development Phase**
   - Use bare folder mode for development
   - Implement code review
   - Write test cases

2. **Testing Phase**
   - Create PUP file
   - Perform functional testing
   - Performance testing

3. **Release Phase**
   - Update version number
   - Write changelog
   - Distribute PUP file

4. **Maintenance Phase**
   - Monitor error logs
   - Collect user feedback
   - Fix issues

### 3. Update Mechanism

Implement automatic update functionality.

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

## Documentation and Comments

### 1. Code Comments

Add comments for complex logic.

```javascript
/**
 * Read and parse configuration file
 * @param {string} path - Configuration file path
 * @returns {Promise<Object>} Configuration object
 * @throws {Error} Throws exception when file doesn't exist or format is incorrect
 */
async function loadConfig(path) {
    try {
        // Read file content
        const content = await puppet.fs.readFileAsText(path);
        
        // Parse JSON
        const config = JSON.parse(content);
        
        // Validate configuration
        validateConfig(config);
        
        return config;
    } catch (error) {
        throw new Error(`Failed to load configuration: ${error.message}`);
    }
}
```

### 2. API Documentation

Write documentation for public APIs.

```javascript
/**
 * @module ApiClient
 * @description Provides Puppet API wrapper
 */

/**
 * Read file content
 * @param {string} path - File path
 * @returns {Promise<string>} File content
 * @example
 * const content = await api.readFile('data.txt');
 */
async function readFile(path) {
    return await puppet.fs.readFileAsText(path);
}
```

### 3. README

Write detailed README for your project.

```markdown
# My Puppet App

## Introduction

This is a desktop application based on the Puppet framework.

## Features

- Feature 1
- Feature 2
- Feature 3

## Installation

1. Download PUP file
2. Run puppet.exe --load-pup app.pup

## Usage

For detailed usage instructions, please refer to [Documentation](./docs/)

## Development

```bash
# Development mode
puppet.exe --nake-load .\dist

# Build PUP file
puppet.exe --create-pup -i .\dist -o app.pup
```

## License

MIT
```

## Related Resources

- [API Documentation](../api/) - Complete API reference
- [Security Mechanisms](./security.html) - Security best practices
- [Quick Start](./getting-started.html) - Quick start guide

## Next Steps

After following best practices, it is recommended to:

1. Regularly review and optimize code
2. Continuously learn new technologies and patterns
3. Collect user feedback and improve
4. Get inspiration from open source projects