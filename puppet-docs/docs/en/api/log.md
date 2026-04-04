---
title: Log API
permalink: /en/api/log.html
createTime: 2026/03/28 15:07:28
---

# Log API

The Log API provides log output functionality for debugging, error tracking, and application monitoring.

## Overview

The `puppet.log` namespace provides the following features:

- Information log output
- Warning log output
- Error log output

## Methods

### setFile()

Sets log file save path, supporting custom format strings.

```javascript
puppet.log.setFile(pathPattern: string): void
```

**Parameters**:

- `pathPattern` (string) - Log file path pattern, supporting date-time formatting

**Supported Format Placeholders**:

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `{yyyy}` | 4-digit year | 2026 |
| `{yy}` | 2-digit year | 26 |
| `{MM}` | Month (01-12) | 03 |
| `{dd}` | Day (01-31) | 29 |
| `{HH}` | Hour (00-23) | 14 |
| `{mm}` | Minute (00-59) | 30 |
| `{ss}` | Second (00-59) | 45 |
| `{fff}` | Millisecond (000-999) | 123 |
| `{%i}` | Incrementing sequence number | 839472 |

**Example**:

```javascript
// Basic usage: Set fixed log file
puppet.log.setFile('logs/app.log');

// Use date-time formatting
puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');

// Use full date-time
puppet.log.setFile('logs/app-{yyyy-MM-dd HH-mm-ss}.log');

// Use date and sequence number
puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');

// Custom format
puppet.log.setFile('Puppet_log-{yy-MM-dd HH:MM:SS - %i}.log');

// Include subdirectory (will be created automatically)
puppet.log.setFile('logs/2026/03/app.log');
```

**Notes**:

::: warning Security Note
- Writing to Windows system sensitive folders (such as Windows, Program Files) will show a security confirmation dialog
- User can cancel the operation to prevent log writing to system folders
- Non-existent subdirectories will be created automatically
:::

### info()

Outputs information level log.

```javascript
puppet.log.info(message: string): void
```

**Parameters**:

- `message` (string) - Log message

**Example**:

```javascript
// Output information log
puppet.log.info('Application started');
puppet.log.info('User logged in: john');
puppet.log.info('Operation completed');
```

### warn()

Outputs warning level log.

```javascript
puppet.log.warn(message: string): void
```

**Parameters**:

- `message` (string) - Log message

**Example**:

```javascript
// Output warning log
puppet.log.warn('Configuration file missing, using default configuration');
puppet.log.warn('Connection timeout, retrying');
puppet.log.warn('High memory usage: 85%');
```

### error()

Outputs error level log.

```javascript
puppet.log.error(message: string): void
```

**Parameters**:

- `message` (string) - Log message

**Example**:

```javascript
// Output error log
puppet.log.error('File read failed: permission denied');
puppet.log.error('Network connection failed');
puppet.log.error('Operation timeout');
```

## Usage Examples

### Log File Configuration

```javascript
// Configure log file when application starts
async function initLogging() {
    // Use date as log file name
    puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');
    
    puppet.log.info('Log system initialized');
    puppet.log.info('Application started');
}

// Use full date-time format
async function initDetailedLogging() {
    puppet.log.setFile('logs/app-{yyyy-MM-dd HH-mm-ss}.log');
    
    puppet.log.info('Detailed log system initialized');
}

// Use date and sequence number to prevent file name conflicts
async function initSequencedLogging() {
    puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');
    
    puppet.log.info('Sequenced log system initialized');
}

// Archive logs by month
async function initMonthlyLogging() {
    puppet.log.setFile('logs/{yyyy}/{MM}/app-{dd}.log');
    
    puppet.log.info('Monthly archived log system initialized');
}
```

### Basic Logging

```javascript
// Application start
puppet.log.info('=== Application Started ===');

// Load configuration
try {
    const config = await loadConfig();
    puppet.log.info('Configuration loaded successfully');
} catch (error) {
    puppet.log.error('Failed to load configuration: ' + error.message);
}

// User operation
puppet.log.info('User clicked save button');
```

### Structured Logging

```javascript
// Structured log object
function logEvent(level, event, data) {
    const timestamp = new Date().toISOString();
    const logEntry = {
        timestamp,
        level,
        event,
        data
    };
    
    const message = JSON.stringify(logEntry);
    
    switch (level) {
        case 'info':
            puppet.log.info(message);
            break;
        case 'warn':
            puppet.log.warn(message);
            break;
        case 'error':
            puppet.log.error(message);
            break;
    }
}

// Use structured logging
logEvent('info', 'user_login', { username: 'john' });
logEvent('warn', 'slow_query', { duration: 5000 });
logEvent('error', 'api_failure', { code: 500, message: 'Internal Server Error' });
```

### Error Tracking

```javascript
class ErrorTracker {
    static track(error, context = {}) {
        const errorInfo = {
            message: error.message,
            stack: error.stack,
            timestamp: new Date().toISOString(),
            context
        };
        
        // Output to Puppet log
        puppet.log.error(JSON.stringify(errorInfo));
        
        // Show user-friendly message
        showError('Operation failed, please try again');
        
        // Optional: Report to error tracking service
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

// Use error tracking
try {
    await riskyOperation();
} catch (error) {
    ErrorTracker.track(error, { action: 'loadData' });
}
```

### Performance Monitoring

```javascript
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
        
        if (duration > 1000) {
            puppet.log.warn(`[Performance] ${label}: ${duration.toFixed(2)}ms`);
        } else {
            puppet.log.info(`[Performance] ${label}: ${duration.toFixed(2)}ms`);
        }
        
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

// Use performance monitoring
const monitor = new PerformanceMonitor();

await monitor.measureAsync('loadData', async () => {
    return await loadData();
});
```

## Best Practices

### 1. Log File Management

Manage log files reasonably to avoid log files becoming too large:

```javascript
// Split logs by date
puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');

// Archive by month
puppet.log.setFile('logs/{yyyy}/{MM}/app.log');

// Use sequence number to prevent file name conflicts
puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');

// Periodically clean old logs (recommended to implement externally)
async function cleanupOldLogs(daysToKeep = 30) {
    const cutoffDate = new Date();
    cutoffDate.setDate(cutoffDate.getDate() - daysToKeep);
    
    // Traverse log directory and delete old logs
    // This needs to be implemented at the application layer
}
```

### 2. Log Level Usage

Choose appropriate log level based on message severity:

```javascript
// INFO: Normal operations
puppet.log.info('Application started');
puppet.log.info('User logged in');
puppet.log.info('Data saved successfully');

// WARN: Potential issues
puppet.log.warn('Configuration missing, using default value');
puppet.log.warn('Long response time');
puppet.log.warn('Cache miss');

// ERROR: Errors and exceptions
puppet.log.error('File read failed');
puppet.log.error('Database connection failed');
puppet.log.error('API request failed');
```

### 3. Structured Messages

Use structured log messages for easy parsing:

```javascript
// Recommended: Structured message
puppet.log.info(JSON.stringify({
    event: 'user_action',
    action: 'click',
    target: 'save_button',
    timestamp: Date.now()
}));

// Not recommended: Non-structured message
puppet.log.info('User clicked save button');
```

### 4. Context Information

Include sufficient context information:

```javascript
// Include context
puppet.log.error(JSON.stringify({
    error: 'File read failed',
    path: '/path/to/file.txt',
    reason: 'Permission denied',
    user: 'john'
}));

// No context
puppet.log.error('File read failed');
```

### 5. Avoid Sensitive Information

Do not include sensitive information in logs:

```javascript
// Not recommended: Contains sensitive information
puppet.log.info('User logged in: john password: secret123');

// Recommended: No sensitive information
puppet.log.info('User logged in: john');
```

## Related Resources

- [JavaScript Console API](https://developer.mozilla.org/en-US/docs/Web/API/console): Browser console API
- [Logging Best Practices](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Using_promises#Error_handling): Error handling best practices

## Common Questions

### Q: Where are logs saved?

A: By default, logs are saved in the `puppet.log` file in the application directory. You can use the `puppet.log.setFile()` method to customize the log file path and format.

### Q: How to clear logs?

A: Logs are continuously appended to the file. If you need to clear logs, you can:
- Delete or clear the log file
- Use a new log file name (through date-time formatting)

### Q: Can I customize log format?

A: Yes! Using the `puppet.log.setFile()` method, you can customize the log file path and format, supporting date-time and sequence number placeholders.

### Q: Will logging affect performance?

A: Log operations use thread-safe file writing, which has minimal impact on performance. However, avoid outputting logs frequently in loops.

### Q: Why does a confirmation dialog appear when writing to system folders?

A: For security reasons, writing to Windows system sensitive folders (such as Windows, Program Files) will show a security confirmation dialog to prevent accidental operations from affecting system security.