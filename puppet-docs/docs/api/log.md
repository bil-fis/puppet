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

### 1. 日志级别使用

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

A: 日志会输出到控制台和应用日志文件中。

### Q: 如何清空日志？

A: 日志会持续追加，需要手动管理日志文件。

### Q: 可以自定义日志格式吗？

A: 可以使用结构化日志自定义格式。

### Q: 日志会影响性能吗？

A: 日志操作对性能影响很小，但应避免在循环中频繁输出日志。