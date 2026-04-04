---
title: File System API
permalink: /en/api/fs.html
createTime: 2026/03/28 15:07:04
---

# File System API

The File System API provides complete file and folder operations, including file selection, reading and writing, deletion, path checking, etc.

## Overview

The `puppet.fs` namespace provides the following features:

- File selection dialog
- Folder selection dialog
- File reading and writing (text, binary, JSON)
- File append operations
- File and folder deletion
- Path existence checking

## Methods

### openFileDialog()

Opens file selection dialog.

```javascript
await puppet.fs.openFileDialog(filter: string[], multiSelect: boolean): Promise<string[]>
```

**Parameters**:

- `filter` (string[]) - File filter array, each element contains description and pattern
- `multiSelect` (boolean) - Whether to allow selecting multiple files

**Return Value**:

Array of selected file paths.

**Example**:

```javascript
// Select single text file
const files = await puppet.fs.openFileDialog(
    ['Text Files', '*.txt'],
    false
);
console.log('Selected file:', files[0]);

// Select multiple image files
const images = await puppet.fs.openFileDialog(
    ['Image Files', '*.png;*.jpg;*.jpeg;*.gif'],
    true
);
console.log('Selected images:', images);

// Select any file
const anyFiles = await puppet.fs.openFileDialog(
    ['All Files', '*.*'],
    false
);
```

**Filter Format**:

- Each filter contains two parts: description and pattern
- Multiple patterns are separated by semicolons
- Supports wildcards `*` and `?`

### openFolderDialog()

Opens folder selection dialog.

```javascript
await puppet.fs.openFolderDialog(): Promise<string>
```

**Return Value**:

Selected folder path.

**Example**:

```javascript
// Select folder
const folder = await puppet.fs.openFolderDialog();
console.log('Selected folder:', folder);

// Load files in folder
if (folder) {
    const files = await puppet.fs.listFiles(folder);
    console.log('Files in folder:', files);
}
```

### readFileAsByte()

Reads file as Base64 encoded binary data.

```javascript
await puppet.fs.readFileAsByte(path: string): Promise<string>
```

**Parameters**:

- `path` (string) - File path

**Return Value**:

Base64 encoded file content.

**Example**:

```javascript
// Read image file
const imageData = await puppet.fs.readFileAsByte('image.png');

// Display image
const img = document.createElement('img');
img.src = 'data:image/png;base64,' + imageData;
document.body.appendChild(img);

// Read arbitrary binary file
const binaryData = await puppet.fs.readFileAsByte('data.bin');
console.log('Data length:', binaryData.length);
```

### readFileAsJson()

Reads JSON file and parses it to object.

```javascript
await puppet.fs.readFileAsJson(path: string): Promise<object>
```

**Parameters**:

- `path` (string) - File path

**Return Value**:

Parsed JSON object.

**Example**:

```javascript
// Read configuration file
const config = await puppet.fs.readFileAsJson('config.json');
console.log('Configuration:', config);

// Read data file
const data = await puppet.fs.readFileAsJson('data.json');
console.log('Data:', data.items);
```

**Notes**:

- File must be in valid JSON format
- Throws exception if file format is incorrect
- Recommended to use try-catch to catch errors

### writeByteToFile()

Writes Base64 encoded binary data to file.

```javascript
await puppet.fs.writeByteToFile(path: string, data: string): Promise<void>
```

**Parameters**:

- `path` (string) - File path
- `data` (string) - Base64 encoded data

**Example**:

```javascript
// Save image
const imageData = canvas.toDataURL('image/png').split(',')[1];
await puppet.fs.writeByteToFile('output.png', imageData);

// Save binary data
const binaryData = base64Encode(rawData);
await puppet.fs.writeByteToFile('output.bin', binaryData);
```

### writeTextToFile()

Writes text content to file.

```javascript
await puppet.fs.writeTextToFile(path: string, text: string): Promise<void>
```

**Parameters**:

- `path` (string) - File path
- `text` (string) - Text content

**Example**:

```javascript
// Write text file
await puppet.fs.writeTextToFile('output.txt', 'Hello, World!');

// Write multi-line text
const content = 'Line 1\nLine 2\nLine 3';
await puppet.fs.writeTextToFile('multiline.txt', content);

// Write JSON
const config = { theme: 'dark', fontSize: 14 };
await puppet.fs.writeTextToFile('config.json', JSON.stringify(config, null, 2));
```

### appendByteToFile()

Appends Base64 encoded binary data to the end of file.

```javascript
await puppet.fs.appendByteToFile(path: string, data: string): Promise<void>
```

**Parameters**:

- `path` (string) - File path
- `data` (string) - Base64 encoded data

**Example**:

```javascript
// Append image data
const imageData = canvas.toDataURL('image/png').split(',')[1];
await puppet.fs.appendByteToFile('images.dat', imageData);

// Append binary data
const binaryData = base64Encode(rawData);
await puppet.fs.appendByteToFile('data.bin', binaryData);
```

### appendTextToFile()

Appends text content to the end of file.

```javascript
await puppet.fs.appendTextToFile(path: string, text: string): Promise<void>
```

**Parameters**:

- `path` (string) - File path
- `text` (string) - Text content

**Example**:

```javascript
// Append log
await puppet.fs.appendTextToFile('log.txt', '[INFO] Operation completed\n');

// Append data
await puppet.fs.appendTextToFile('data.txt', 'New data\n');

// Append multiple lines
const lines = ['Line 4', 'Line 5', 'Line 6'];
for (const line of lines) {
    await puppet.fs.appendTextToFile('file.txt', line + '\n');
}
```

### exists()

Checks if file or folder exists.

```javascript
await puppet.fs.exists(path: string): Promise<boolean>
```

**Parameters**:

- `path` (string) - File or folder path

**Return Value**:

`true` means exists, `false` means does not exist.

**Example**:

```javascript
// Check if file exists
const fileExists = await puppet.fs.exists('config.json');
if (fileExists) {
    const config = await puppet.fs.readFileAsJson('config.json');
} else {
    console.log('Configuration file does not exist');
}

// Check if folder exists
const folderExists = await puppet.fs.exists('C:\\MyFolder');
if (!folderExists) {
    // Create folder (use system command)
    await puppet.application.execute('mkdir C:\\MyFolder');
}
```

### delete()

Deletes file or folder.

```javascript
await puppet.fs.delete(path: string): Promise<void>
```

**Parameters**:

- `path` (string) - File or folder path

**Example**:

```javascript
// Delete file
await puppet.fs.delete('temp.txt');

// Delete folder
await puppet.fs.delete('C:\\TempFolder');

// Check before deleting
if (await puppet.fs.exists('old-file.txt')) {
    await puppet.fs.delete('old-file.txt');
}
```

**Notes**:

- Deleting folder recursively deletes all files in it
- Deletion operation cannot be undone, use with caution
- Deleting system directory will show permission confirmation

## Usage Examples

### File Manager

```javascript
class FileManager {
    constructor() {
        this.currentPath = '';
    }
    
    async openFile() {
        const files = await puppet.fs.openFileDialog(
            ['All Files', '*.*'],
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
                    showMessage('Unsupported file type');
            }
        } catch (error) {
            showMessage('Failed to read file: ' + error.message);
        }
    }
    
    async saveFile(content) {
        const files = await puppet.fs.openFileDialog(
            ['Text Files', '*.txt'],
            false
        );
        
        if (files && files.length > 0) {
            await puppet.fs.writeTextToFile(files[0], content);
            showMessage('File saved');
        }
    }
}

// Use file manager
const fileManager = new FileManager();
fileManager.openFile();
```

### Log System

```javascript
class Logger {
    constructor(logFile) {
        this.logFile = logFile;
    }
    
    async log(level, message) {
        const timestamp = new Date().toISOString();
        const logEntry = `[${timestamp}] [${level}] ${message}\n`;
        
        // Append to log file
        await puppet.fs.appendTextToFile(this.logFile, logEntry);
        
        // Output to console
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

// Use log system
const logger = new Logger('app.log');
await logger.info('Application started');
await logger.warn('Configuration missing');
await logger.error('Operation failed');
```

### Configuration Management

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
                puppet.log.error('Failed to load configuration:', error.message);
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

// Use configuration manager
const configManager = new ConfigManager('config.json');
await configManager.load();

// Read configuration
const theme = configManager.get('theme', 'light');
const fontSize = configManager.get('fontSize', 14);

// Write configuration
configManager.set('theme', 'dark');
configManager.set('fontSize', 16);
```

### Data Backup

```javascript
async function backupData(sourceFile, backupDir) {
    // Create backup directory
    if (!await puppet.fs.exists(backupDir)) {
        await puppet.application.execute(`mkdir "${backupDir}"`);
    }
    
    // Generate backup file name
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    const fileName = sourceFile.split('\\').pop();
    const backupFile = `${backupDir}\\${timestamp}_${fileName}`;
    
    // Read source file
    const content = await puppet.fs.readFileAsByte(sourceFile);
    
    // Write backup file
    await puppet.fs.writeByteToFile(backupFile, content);
    
    console.log('Backup created:', backupFile);
}

// Usage example
await backupData('data.json', 'C:\\Backups');
```

## Security Considerations

### Path Validation

Always validate file paths:

```javascript
async function safeReadFile(path) {
    // Basic validation
    if (!path || typeof path !== 'string') {
        throw new Error('Invalid path');
    }
    
    // Check for illegal characters
    const invalidChars = /[<>:"|?*]/;
    if (invalidChars.test(path)) {
        throw new Error('Path contains illegal characters');
    }
    
    // Check path length
    if (path.length > 260) {
        throw new Error('Path too long');
    }
    
    // Read file
    return await puppet.fs.readFileAsText(path);
}
```

### Permission Check

Check file access permissions:

```javascript
async function checkAccess(path) {
    try {
        // Try to read file
        await puppet.fs.readFileAsText(path);
        return true;
    } catch (error) {
        console.error('Cannot access file:', error.message);
        return false;
    }
}
```

### Sensitive Operation Confirmation

Confirm before deleting or overwriting important files:

```javascript
async function safeDelete(path) {
    if (await puppet.fs.exists(path)) {
        const confirmed = confirm(`Are you sure you want to delete ${path}?`);
        if (confirmed) {
            await puppet.fs.delete(path);
        }
    }
}
```

## Best Practices

### 1. Error Handling

Always handle file operation errors:

```javascript
try {
    const content = await puppet.fs.readFileAsText('file.txt');
} catch (error) {
    puppet.log.error('Failed to read file:', error.message);
    showError('Cannot read file');
}
```

### 2. Resource Cleanup

Clean up temporary files promptly:

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

// Use
try {
    const tempFile = await createTempFile('temporary content');
    // Use temporary file
} finally {
    await cleanupTempFile(tempFile);
}
```

### 3. Batch Operations

Use Promise.all for batch file operations:

```javascript
async function batchReadFiles(files) {
    const results = await Promise.all(
        files.map(file => puppet.fs.readFileAsText(file))
    );
    return results;
}

// Use
const files = ['file1.txt', 'file2.txt', 'file3.txt'];
const contents = await batchReadFiles(files);
```

### 4. Path Normalization

Use standardized path format:

```javascript
function normalizePath(path) {
    // Convert to absolute path
    path = path.replace(/^\.\//, process.cwd() + '\\');
    
    // Use backslashes uniformly
    path = path.replace(/\//g, '\\');
    
    // Remove extra slashes
    path = path.replace(/\\+/g, '\\');
    
    return path;
}
```

## Related Resources

- [Windows File System](https://learn.microsoft.com/en-us/windows/win32/fileio/file-systems): Windows file system documentation
- [JSON Format](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON): JSON usage guide
- [Base64 Encoding](https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding): Base64 encoding explanation

## Common Questions

### Q: Why can't I access certain files?

A: Puppet automatically blocks access to Windows system directories, which is a security mechanism.

### Q: How to read large files?

A: For large files, it is recommended to read in chunks or use stream processing.

### Q: Can deletion operations be undone?

A: No, deletion operations are permanent, use with caution.

### Q: How to get file size?

A: You can use system commands to get it:

```javascript
const result = await puppet.application.execute('powershell -Command "(Get-Item file.txt).length"');
```