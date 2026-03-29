---
title: 文件系统 API
permalink: /api/fs.html
createTime: 2026/03/28 15:07:04
---

# 文件系统 API

文件系统 API 提供了完整的文件和文件夹操作功能，包括文件选择、读写、删除、路径检查等。

## 概述

`puppet.fs` 命名空间提供以下功能：

- 文件选择对话框
- 文件夹选择对话框
- 文件读写（文本、二进制、JSON）
- 文件追加操作
- 文件和文件夹删除
- 路径存在性检查

## 方法

### openFileDialog()

打开文件选择对话框。

```javascript
await puppet.fs.openFileDialog(filter: string[], multiSelect: boolean): Promise<string[]>
```

**参数**：

- `filter` (string[]) - 文件过滤器数组，每个元素包含描述和模式
- `multiSelect` (boolean) - 是否允许选择多个文件

**返回值**：

选中的文件路径数组。

**示例**：

```javascript
// 选择单个文本文件
const files = await puppet.fs.openFileDialog(
    ['文本文件', '*.txt'],
    false
);
console.log('选择的文件:', files[0]);

// 选择多个图片文件
const images = await puppet.fs.openFileDialog(
    ['图片文件', '*.png;*.jpg;*.jpeg;*.gif'],
    true
);
console.log('选择的图片:', images);

// 选择任意文件
const anyFiles = await puppet.fs.openFileDialog(
    ['所有文件', '*.*'],
    false
);
```

**过滤器格式**：

- 每个过滤器包含两个部分：描述和模式
- 多个模式用分号分隔
- 支持通配符 `*` 和 `?`

### openFolderDialog()

打开文件夹选择对话框。

```javascript
await puppet.fs.openFolderDialog(): Promise<string>
```

**返回值**：

选中的文件夹路径。

**示例**：

```javascript
// 选择文件夹
const folder = await puppet.fs.openFolderDialog();
console.log('选择的文件夹:', folder);

// 加载文件夹中的文件
if (folder) {
    const files = await puppet.fs.listFiles(folder);
    console.log('文件夹中的文件:', files);
}
```

### readFileAsByte()

读取文件为 Base64 编码的二进制数据。

```javascript
await puppet.fs.readFileAsByte(path: string): Promise<string>
```

**参数**：

- `path` (string) - 文件路径

**返回值**：

Base64 编码的文件内容。

**示例**：

```javascript
// 读取图片文件
const imageData = await puppet.fs.readFileAsByte('image.png');

// 显示图片
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + imageData;
document.body.appendChild(img);

// 读取任意二进制文件
const binaryData = await puppet.fs.readFileAsByte('data.bin');
console.log('数据长度:', binaryData.length);
```

### readFileAsJson()

读取 JSON 文件并解析为对象。

```javascript
await puppet.fs.readFileAsJson(path: string): Promise<object>
```

**参数**：

- `path` (string) - 文件路径

**返回值**：

解析后的 JSON 对象。

**示例**：

```javascript
// 读取配置文件
const config = await puppet.fs.readFileAsJson('config.json');
console.log('配置:', config);

// 读取数据文件
const data = await puppet.fs.readFileAsJson('data.json');
console.log('数据:', data.items);
```

**注意事项**：

- 文件必须是有效的 JSON 格式
- 如果文件格式错误会抛出异常
- 建议使用 try-catch 捕获错误

### writeByteToFile()

将 Base64 编码的二进制数据写入文件。

```javascript
await puppet.fs.writeByteToFile(path: string, data: string): Promise<void>
```

**参数**：

- `path` (string) - 文件路径
- `data` (string) - Base64 编码的数据

**示例**：

```javascript
// 保存图片
const imageData = canvas.toDataURL('image/png').split(',')[1];
await puppet.fs.writeByteToFile('output.png', imageData);

// 保存二进制数据
const binaryData = base64Encode(rawData);
await puppet.fs.writeByteToFile('output.bin', binaryData);
```

### writeTextToFile()

将文本内容写入文件。

```javascript
await puppet.fs.writeTextToFile(path: string, text: string): Promise<void>
```

**参数**：

- `path` (string) - 文件路径
- `text` (string) - 文本内容

**示例**：

```javascript
// 写入文本文件
await puppet.fs.writeTextToFile('output.txt', 'Hello, World!');

// 写入多行文本
const content = 'Line 1\nLine 2\nLine 3';
await puppet.fs.writeTextToFile('multiline.txt', content);

// 写入 JSON
const config = { theme: 'dark', fontSize: 14 };
await puppet.fs.writeTextToFile('config.json', JSON.stringify(config, null, 2));
```

### appendByteToFile()

将 Base64 编码的二进制数据追加到文件末尾。

```javascript
await puppet.fs.appendByteToFile(path: string, data: string): Promise<void>
```

**参数**：

- `path` (string) - 文件路径
- `data` (string) - Base64 编码的数据

**示例**：

```javascript
// 追加图片数据
const imageData = canvas.toDataURL('image/png').split(',')[1];
await puppet.fs.appendByteToFile('images.dat', imageData);

// 追加二进制数据
const binaryData = base64Encode(rawData);
await puppet.fs.appendByteToFile('data.bin', binaryData);
```

### appendTextToFile()

将文本内容追加到文件末尾。

```javascript
await puppet.fs.appendTextToFile(path: string, text: string): Promise<void>
```

**参数**：

- `path` (string) - 文件路径
- `text` (string) - 文本内容

**示例**：

```javascript
// 追加日志
await puppet.fs.appendTextToFile('log.txt', '[INFO] 操作完成\n');

// 追加数据
await puppet.fs.appendTextToFile('data.txt', 'New data\n');

// 追加多行
const lines = ['Line 4', 'Line 5', 'Line 6'];
for (const line of lines) {
    await puppet.fs.appendTextToFile('file.txt', line + '\n');
}
```

### exists()

检查文件或文件夹是否存在。

```javascript
await puppet.fs.exists(path: string): Promise<boolean>
```

**参数**：

- `path` (string) - 文件或文件夹路径

**返回值**：

`true` 表示存在，`false` 表示不存在。

**示例**：

```javascript
// 检查文件是否存在
const fileExists = await puppet.fs.exists('config.json');
if (fileExists) {
    const config = await puppet.fs.readFileAsJson('config.json');
} else {
    console.log('配置文件不存在');
}

// 检查文件夹是否存在
const folderExists = await puppet.fs.exists('C:\\MyFolder');
if (!folderExists) {
    // 创建文件夹（使用系统命令）
    await puppet.application.execute('mkdir C:\\MyFolder');
}
```

### delete()

删除文件或文件夹。

```javascript
await puppet.fs.delete(path: string): Promise<void>
```

**参数**：

- `path` (string) - 文件或文件夹路径

**示例**：

```javascript
// 删除文件
await puppet.fs.delete('temp.txt');

// 删除文件夹
await puppet.fs.delete('C:\\TempFolder');

// 删除前检查是否存在
if (await puppet.fs.exists('old-file.txt')) {
    await puppet.fs.delete('old-file.txt');
}
```

**注意事项**：

- 删除文件夹会递归删除其中的所有文件
- 删除操作不可撤销，请谨慎使用
- 删除系统目录会弹出权限确认

## 使用示例

### 文件管理器

```javascript
class FileManager {
    constructor() {
        this.currentPath = '';
    }
    
    async openFile() {
        const files = await puppet.fs.openFileDialog(
            ['所有文件', '*.*'],
            false
        );
        
        if (files && files.length > 0) {
            this.currentPath = files[0];
            await this.displayFile();
        }
    }
    
    async displayFile() {
        const extension = this.currentPath.split('.').pop().toLowerCase();
        
        try {
            switch (extension) {
                case 'json':
                    const jsonData = await puppet.fs.readFileAsJson(this.currentPath);
                    displayJson(jsonData);
                    break;
                    
                case 'txt':
                case 'md':
                    const textData = await puppet.fs.readFileAsText(this.currentPath);
                    displayText(textData);
                    break;
                    
                case 'png':
                case 'jpg':
                case 'jpeg':
                    const imageData = await puppet.fs.readFileAsByte(this.currentPath);
                    displayImage('data:image/' + extension + ';base64,' + imageData);
                    break;
                    
                default:
                    showMessage('不支持的文件类型');
            }
        } catch (error) {
            showMessage('读取文件失败: ' + error.message);
        }
    }
    
    async saveFile(content) {
        const files = await puppet.fs.openFileDialog(
            ['文本文件', '*.txt'],
            false
        );
        
        if (files && files.length > 0) {
            await puppet.fs.writeTextToFile(files[0], content);
            showMessage('文件已保存');
        }
    }
}

// 使用文件管理器
const fileManager = new FileManager();
fileManager.openFile();
```

### 日志系统

```javascript
class Logger {
    constructor(logFile) {
        this.logFile = logFile;
    }
    
    async log(level, message) {
        const timestamp = new Date().toISOString();
        const logEntry = `[${timestamp}] [${level}] ${message}\n`;
        
        // 追加到日志文件
        await puppet.fs.appendTextToFile(this.logFile, logEntry);
        
        // 输出到控制台
        console.log(logEntry.trim());
    }
    
    async info(message) {
        await this.log('INFO', message);
    }
    
    async warn(message) {
        await this.log('WARN', message);
    }
    
    async error(message) {
        await this.log('ERROR', message);
    }
}

// 使用日志系统
const logger = new Logger('app.log');
await logger.info('应用启动');
await logger.warn('配置缺失');
await logger.error('操作失败');
```

### 配置管理

```javascript
class ConfigManager {
    constructor(configFile) {
        this.configFile = configFile;
        this.config = {};
    }
    
    async load() {
        if (await puppet.fs.exists(this.configFile)) {
            try {
                this.config = await puppet.fs.readFileAsJson(this.configFile);
            } catch (error) {
                puppet.log.error('加载配置失败:', error.message);
                this.config = {};
            }
        }
        
        return this.config;
    }
    
    async save() {
        await puppet.fs.writeTextToFile(
            this.configFile,
            JSON.stringify(this.config, null, 2)
        );
    }
    
    get(key, defaultValue) {
        return this.config[key] ?? defaultValue;
    }
    
    set(key, value) {
        this.config[key] = value;
        this.save();
    }
    
    reset() {
        this.config = {};
        this.save();
    }
}

// 使用配置管理器
const configManager = new ConfigManager('config.json');
await configManager.load();

// 读取配置
const theme = configManager.get('theme', 'light');
const fontSize = configManager.get('fontSize', 14);

// 写入配置
configManager.set('theme', 'dark');
configManager.set('fontSize', 16);
```

### 数据备份

```javascript
async function backupData(sourceFile, backupDir) {
    // 创建备份目录
    if (!await puppet.fs.exists(backupDir)) {
        await puppet.application.execute(`mkdir "${backupDir}"`);
    }
    
    // 生成备份文件名
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    const fileName = sourceFile.split('\\').pop();
    const backupFile = `${backupDir}\\${timestamp}_${fileName}`;
    
    // 读取源文件
    const content = await puppet.fs.readFileAsByte(sourceFile);
    
    // 写入备份文件
    await puppet.fs.writeByteToFile(backupFile, content);
    
    console.log('备份已创建:', backupFile);
}

// 使用示例
await backupData('data.json', 'C:\\Backups');
```

## 安全注意事项

### 路径验证

始终验证文件路径：

```javascript
async function safeReadFile(path) {
    // 基本验证
    if (!path || typeof path !== 'string') {
        throw new Error('无效的路径');
    }
    
    // 检查非法字符
    const invalidChars = /[<>:"|?*]/;
    if (invalidChars.test(path)) {
        throw new Error('路径包含非法字符');
    }
    
    // 检查路径长度
    if (path.length > 260) {
        throw new Error('路径过长');
    }
    
    // 读取文件
    return await puppet.fs.readFileAsText(path);
}
```

### 权限检查

检查文件访问权限：

```javascript
async function checkAccess(path) {
    try {
        // 尝试读取文件
        await puppet.fs.readFileAsText(path);
        return true;
    } catch (error) {
        console.error('无法访问文件:', error.message);
        return false;
    }
}
```

### 敏感操作确认

删除或覆盖重要文件前确认：

```javascript
async function safeDelete(path) {
    if (await puppet.fs.exists(path)) {
        const confirmed = confirm(`确定要删除 ${path} 吗？`);
        if (confirmed) {
            await puppet.fs.delete(path);
        }
    }
}
```

## 最佳实践

### 1. 错误处理

始终处理文件操作错误：

```javascript
try {
    const content = await puppet.fs.readFileAsText('file.txt');
} catch (error) {
    puppet.log.error('读取文件失败:', error.message);
    showError('无法读取文件');
}
```

### 2. 资源清理

及时清理临时文件：

```javascript
async function createTempFile(content) {
    const tempFile = 'temp_' + Date.now() + '.txt';
    await puppet.fs.writeTextToFile(tempFile, content);
    return tempFile;
}

async function cleanupTempFile(tempFile) {
    if (await puppet.fs.exists(tempFile)) {
        await puppet.fs.delete(tempFile);
    }
}

// 使用
try {
    const tempFile = await createTempFile('temporary content');
    // 使用临时文件
} finally {
    await cleanupTempFile(tempFile);
}
```

### 3. 批量操作

批量操作文件时使用 Promise.all：

```javascript
async function batchReadFiles(files) {
    const results = await Promise.all(
        files.map(file => puppet.fs.readFileAsText(file))
    );
    return results;
}

// 使用
const files = ['file1.txt', 'file2.txt', 'file3.txt'];
const contents = await batchReadFiles(files);
```

### 4. 路径规范化

使用规范的路径格式：

```javascript
function normalizePath(path) {
    // 转换为绝对路径
    path = path.replace(/^\.\//, process.cwd() + '\\');
    
    // 统一使用反斜杠
    path = path.replace(/\//g, '\\');
    
    // 移除多余的斜杠
    path = path.replace(/\\+/g, '\\');
    
    return path;
}
```

## 相关资源

- [Windows 文件系统](https://learn.microsoft.com/en-us/windows/win32/fileio/file-systems)：Windows 文件系统文档
- [JSON 格式](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON)：JSON 使用指南
- [Base64 编码](https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding)：Base64 编码说明

## 常见问题

### Q: 为什么无法访问某些文件？

A: Puppet 会自动阻止访问 Windows 系统目录，这是安全机制。

### Q: 如何读取大文件？

A: 对于大文件，建议分块读取或使用流式处理。

### Q: 删除操作可以撤销吗？

A: 不可以，删除操作是永久的，请谨慎使用。

### Q: 如何获取文件大小？

A: 可以使用系统命令获取：

```javascript
const result = await puppet.application.execute('powershell -Command "(Get-Item file.txt).length"');
```