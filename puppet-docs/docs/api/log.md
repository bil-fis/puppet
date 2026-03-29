---
title: 日志 API
permalink: /api/log.html
createTime: 2026/03/28 15:07:28
---

# 日志 API

日志 API 提供了日志输出功能，用于调试、错误追踪和应用监控。

## 概述

`puppet.log` 命名空间提供以下功能：

- 信息日志输出
- 警告日志输出
- 错误日志输出

## 方法

### setFile()

设置日志文件保存路径，支持自定义格式化字符串。

```javascript
puppet.log.setFile(pathPattern: string): void
```

**参数**：

- `pathPattern` (string) - 日志文件路径模式，支持日期时间格式化

**支持的格式占位符**：

| 占位符 | 说明 | 示例 |
|--------|------|------|
| `{yyyy}` | 4位年份 | 2026 |
| `{yy}` | 2位年份 | 26 |
| `{MM}` | 月份（01-12） | 03 |
| `{dd}` | 日期（01-31） | 29 |
| `{HH}` | 小时（00-23） | 14 |
| `{mm}` | 分钟（00-59） | 30 |
| `{ss}` | 秒（00-59） | 45 |
| `{fff}` | 毫秒（000-999） | 123 |
| `{%i}` | 自增序号 | 839472 |

**示例**：

```javascript
// 基础用法：设置固定日志文件
puppet.log.setFile('logs/app.log');

// 使用日期时间格式化
puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');

// 使用完整日期时间
puppet.log.setFile('logs/app-{yyyy-MM-dd HH-mm-ss}.log');

// 使用日期和序号
puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');

// 自定义格式
puppet.log.setFile('Puppet_log-{yy-MM-dd HH:MM:SS - %i}.log');

// 包含子目录（会自动创建）
puppet.log.setFile('logs/2026/03/app.log');
```

**注意事项**：

::: warning 安全提示
- 写入 Windows 系统敏感文件夹（如 Windows、Program Files）会弹出安全确认对话框
- 用户可以取消操作，防止日志写入系统文件夹
- 不存在子目录时会自动创建
:::

### info()

输出信息级别日志。

```javascript
puppet.log.info(message: string): void
```

**参数**：

- `message` (string) - 日志消息

**示例**：

```javascript
// 输出信息日志
puppet.log.info('应用启动');
puppet.log.info('用户登录: john');
puppet.log.info('操作完成');
```

### warn()

输出警告级别日志。

```javascript
puppet.log.warn(message: string): void
```

**参数**：

- `message` (string) - 日志消息

**示例**：

```javascript
// 输出警告日志
puppet.log.warn('配置文件缺失，使用默认配置');
puppet.log.warn('连接超时，正在重试');
puppet.log.warn('内存使用率过高: 85%');
```

### error()

输出错误级别日志。

```javascript
puppet.log.error(message: string): void
```

**参数**：

- `message` (string) - 日志消息

**示例**：

```javascript
// 输出错误日志
puppet.log.error('文件读取失败: permission denied');
puppet.log.error('网络连接失败');
puppet.log.error('操作超时');
```

## 使用示例

### 日志文件配置

```javascript
// 应用启动时配置日志文件
async function initLogging() {
    // 使用日期作为日志文件名
    puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');
    
    puppet.log.info('日志系统初始化完成');
    puppet.log.info('应用启动');
}

// 使用完整的日期时间格式
async function initDetailedLogging() {
    puppet.log.setFile('logs/app-{yyyy-MM-dd HH-mm-ss}.log');
    
    puppet.log.info('详细日志系统初始化完成');
}

// 使用日期和序号，防止文件名冲突
async function initSequencedLogging() {
    puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');
    
    puppet.log.info('序列化日志系统初始化完成');
}

// 按月归档日志
async function initMonthlyLogging() {
    puppet.log.setFile('logs/{yyyy}/{MM}/app-{dd}.log');
    
    puppet.log.info('按月归档的日志系统初始化完成');
}
```

### 基础日志

```javascript
// 应用启动
puppet.log.info('=== 应用启动 ===');

// 加载配置
try {
    const config = await loadConfig();
    puppet.log.info('配置加载成功');
} catch (error) {
    puppet.log.error('配置加载失败: ' + error.message);
}

// 用户操作
puppet.log.info('用户点击了保存按钮');
```

### 结构化日志

```javascript
// 结构化日志对象
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

// 使用结构化日志
logEvent('info', 'user_login', { username: 'john' });
logEvent('warn', 'slow_query', { duration: 5000 });
logEvent('error', 'api_failure', { code: 500, message: 'Internal Server Error' });
```

### 错误追踪

```javascript
class ErrorTracker {
    static track(error, context = {}) {
        const errorInfo = {
            message: error.message,
            stack: error.stack,
            timestamp: new Date().toISOString(),
            context
        };
        
        // 输出到 Puppet 日志
        puppet.log.error(JSON.stringify(errorInfo));
        
        // 显示用户友好的消息
        showError('操作失败，请重试');
        
        // 可选：上报到错误追踪服务
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

// 使用错误追踪
try {
    await riskyOperation();
} catch (error) {
    ErrorTracker.track(error, { action: 'loadData' });
}
```

### 性能监控

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
            puppet.log.warn(`[性能] ${label}: ${duration.toFixed(2)}ms`);
        } else {
            puppet.log.info(`[性能] ${label}: ${duration.toFixed(2)}ms`);
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

// 使用性能监控
const monitor = new PerformanceMonitor();

await monitor.measureAsync('loadData', async () => {
    return await loadData();
});
```

## 最佳实践

### 1. 日志文件管理

合理管理日志文件，避免日志文件过大：

```javascript
// 按日期分割日志
puppet.log.setFile('logs/app-{yyyy-MM-dd}.log');

// 按月归档
puppet.log.setFile('logs/{yyyy}/{MM}/app.log');

// 使用序号防止文件名冲突
puppet.log.setFile('logs/app-{yyyy-MM-dd}_{%i}.log');

// 定期清理旧日志（建议在外部实现）
async function cleanupOldLogs(daysToKeep = 30) {
    const cutoffDate = new Date();
    cutoffDate.setDate(cutoffDate.getDate() - daysToKeep);
    
    // 遍历日志目录，删除旧日志
    // 这需要在应用层实现
}
```

### 2. 日志级别使用

根据消息严重性选择合适的日志级别：

```javascript
// INFO：正常操作
puppet.log.info('应用启动');
puppet.log.info('用户登录');
puppet.log.info('数据保存成功');

// WARN：潜在问题
puppet.log.warn('配置缺失，使用默认值');
puppet.log.warn('响应时间过长');
puppet.log.warn('缓存未命中');

// ERROR：错误和异常
puppet.log.error('文件读取失败');
puppet.log.error('数据库连接失败');
puppet.log.error('API 请求失败');
```

### 2. 结构化消息

使用结构化的日志消息便于解析：

```javascript
// 推荐：结构化消息
puppet.log.info(JSON.stringify({
    event: 'user_action',
    action: 'click',
    target: 'save_button',
    timestamp: Date.now()
}));

// 不推荐：非结构化消息
puppet.log.info('用户点击了保存按钮');
```

### 3. 上下文信息

包含足够的上下文信息：

```javascript
// 包含上下文
puppet.log.error(JSON.stringify({
    error: 'File read failed',
    path: '/path/to/file.txt',
    reason: 'Permission denied',
    user: 'john'
}));

// 不包含上下文
puppet.log.error('文件读取失败');
```

### 4. 避免敏感信息

不要在日志中包含敏感信息：

```javascript
// 不推荐：包含敏感信息
puppet.log.info('用户登录: john password: secret123');

// 推荐：不包含敏感信息
puppet.log.info('用户登录: john');
```

## 相关资源

- [JavaScript 控制台 API](https://developer.mozilla.org/en-US/docs/Web/API/console)：浏览器控制台 API
- [日志最佳实践](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Using_promises#Error_handling)：错误处理最佳实践

## 常见问题

### Q: 日志保存在哪里？

A: 默认日志保存在应用目录下的 `puppet.log` 文件中。您可以使用 `puppet.log.setFile()` 方法自定义日志文件路径和格式。

### Q: 如何清空日志？

A: 日志会持续追加到文件中。如果需要清空日志，可以：
- 删除或清空日志文件
- 使用新的日志文件名（通过日期时间格式化）

### Q: 可以自定义日志格式吗？

A: 是的！使用 `puppet.log.setFile()` 方法可以自定义日志文件路径和格式，支持日期时间和序号占位符。

### Q: 日志会影响性能吗？

A: 日志操作使用线程安全的文件写入，对性能影响很小。但应避免在循环中频繁输出日志。

### Q: 为什么写入系统文件夹会弹出确认对话框？

A: 为了安全起见，写入 Windows 系统敏感文件夹（如 Windows、Program Files）会弹出安全确认对话框，防止误操作影响系统安全。