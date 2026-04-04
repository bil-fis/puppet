---
title: Application Control API
permalink: /en/api/application.html
createTime: 2026/03/28 15:04:53
---

# Application Control API

The Application Control API provides application lifecycle management, external program execution, configuration management, and other features.

## Overview

The `puppet.application` namespace provides the following features:

- Application close and restart
- Window information query
- External program execution
- Configuration file management
- System path retrieval
- User information retrieval

## Methods

### close()

Closes the application.

```javascript
await puppet.application.close(): Promise<void>
```

**Example**:

```javascript
// Close the application
await puppet.application.close();

// Close on button click
document.getElementById('close-btn').addEventListener('click', async () => {
    await puppet.application.close();
});
```

**Notes**:

- The application will exit immediately after calling this method
- It is recommended to save data before closing
- The close operation cannot be cancelled

### restart()

Restarts the application.

```javascript
await puppet.application.restart(): Promise<void>
```

**Example**:

```javascript
// Restart the application
await puppet.application.restart();

// Restart after settings change
document.getElementById('restart-btn').addEventListener('click', async () => {
    await saveSettings();
    await puppet.application.restart();
});
```

**Description**:

- Restart closes the current application instance
- Then starts a new application instance
- Configuration and state will be preserved

### getWindowInfo()

Gets window information.

```javascript
await puppet.application.getWindowInfo(): Promise<WindowInfo>
```

**Return Value**:

```typescript
interface WindowInfo {
    handle: number;           // Window handle
    title: string;            // Window title
    className: string;        // Window class name
    isVisible: boolean;       // Whether visible
    isMinimized: boolean;     // Whether minimized
    isMaximized: boolean;     // Whether maximized
    width: number;            // Window width
    height: number;           // Window height
    x: number;                // Window X coordinate
    y: number;                // Window Y coordinate
}
```

**Example**:

```javascript
// Get window information
const info = await puppet.application.getWindowInfo();
console.log('Window title:', info.title);
console.log('Window size:', info.width, 'x', info.height);
console.log('Window position:', info.x, info.y);
```

### execute()

Executes external program or command.

```javascript
await puppet.application.execute(command: string): Promise<void>
```

**Parameters**:

- `command` (string) - Command or file path to execute

**Example**:

```javascript
// Open file
await puppet.application.execute('C:\\Documents\\report.pdf');

// Open webpage
await puppet.application.execute('https://www.example.com');

// Execute system command
await puppet.application.execute('cmd /c dir');

// Open calculator
await puppet.application.execute('calc.exe');
```

**Auto Detection**:

The API automatically detects the command type:

- **URL**: Opens with default browser
- **File**: Opens with associated program
- **Executable**: Directly executes
- **Command**: Executes through cmd

**Security Restrictions**:

- Executing programs in system directories will show a permission confirmation dialog
- Some dangerous operations require user confirmation
- Command execution has a 30-second timeout limit

::: warning Security Warning
Do not execute unverified commands, as this may cause security risks.
:::

### setConfig()

Sets configuration value (modifies puppet.ini file).

**Method 1: Modify global configuration**

```javascript
await puppet.application.setConfig(key: string, value: string): Promise<void>
```

**Parameters**:

- `key` (string) - Configuration key
- `value` (string) - Configuration value (string only)

**Method 2: Modify configuration for a specific section**

```javascript
await puppet.application.setConfig(section: string, key: string, value: string): Promise<void>
```

**Parameters**:

- `section` (string) - Section name (such as "file", "window", etc.)
- `key` (string) - Configuration key
- `value` (string) - Configuration value (string only)

**Description**:

- Modifies the `puppet.ini` configuration file
- Shows a confirmation dialog before modification
- Creates a backup file (.backup) for each modification
- Only supports string type values

**Example**:

```javascript
// Modify global configuration (equivalent to modifying [file] section)
await puppet.application.setConfig('file', 'C:\\MyApp\\app.pup');

// Modify configuration for a specific section
await puppet.application.setConfig('file', 'file', 'C:\\MyApp\\app.pup');
await puppet.application.setConfig('window', 'startup_position', '100,100');
await puppet.application.setConfig('app', 'language', 'en-US');
```

**puppet.ini file format**:

```ini
[file]
file=C:\MyApp\app.pup

[window]
startup_position=100,100
borderless=true
window_size=800,600

[app]
language=en-US
theme=dark
```

**Security Note**:

- Shows confirmation dialog before modifying configuration
- User can cancel the modification
- Error operations will display error messages

::: warning Warning
The `puppet.ini` configuration file is the global configuration file for the Puppet framework. Modifications may affect framework startup behavior. Please operate with caution.
:::

::: tip Recommended Usage
**Important Difference**: `puppet.application.setConfig()` is for framework configuration, `puppet.storage` is for application data.

- **setConfig** - Framework configuration (such as default PUP file path)
  - Modifies `puppet.ini` file
  - Requires user confirmation
  - Affects framework startup behavior

- **storage** - Application data (such as user settings, application state)
  - Uses SQLite database
  - Stores directly without confirmation
  - Can be isolated or shared between applications

**Example Comparison**:

```javascript
// ❌ Wrong: Using setConfig to store application data
await puppet.application.setConfig('myapp', 'username', 'john');

// ✅ Correct: Using storage to store application data
await puppet.storage.setItem('default', 'username', 'john');

// ✅ Correct: Using setConfig to modify framework configuration
await puppet.application.setConfig('file', 'file', 'C:\\MyApp\\app.pup');
```

:::

### getAssemblyDirectory()

Gets the assembly directory (application installation directory).

```javascript
await puppet.application.getAssemblyDirectory(): Promise<string>
```

**Return Value**:

Full path to the assembly directory.

**Example**:

```javascript
// Get assembly directory
const appDir = await puppet.application.getAssemblyDirectory();
console.log('Application directory:', appDir);

// Build resource paths
const configPath = appDir + '\\config.json';
const iconPath = appDir + '\\icons\\app.ico';
```

### getAppDataDirectory()

Gets the application data directory.

```javascript
await puppet.application.getAppDataDirectory(): Promise<string>
```

**Return Value**:

Full path to the application data directory (typically `%APPDATA%\YourApp`).

**Example**:

```javascript
// Get application data directory
const dataDir = await puppet.application.getAppDataDirectory();
console.log('Data directory:', dataDir);

// Save user data
const userDataPath = dataDir + '\\user.json';
await puppet.fs.writeTextToFile(userDataPath, JSON.stringify(userData));
```

### getCurrentUser()

Gets current user information.

```javascript
await puppet.application.getCurrentUser(): Promise<UserInfo>
```

**Return Value**:

```typescript
interface UserInfo {
    name: string;        // Username
    domain: string;     // Domain
    homeDirectory: string;  // Home directory
}
```

**Example**:

```javascript
// Get user information
const user = await puppet.application.getCurrentUser();
console.log('Username:', user.name);
console.log('Domain:', user.domain);
console.log('Home directory:', user.homeDirectory);
```

### getAppInfo()

Gets application information, including signature status (V1.2 format).

```javascript
await puppet.application.getAppInfo(): Promise<AppInfo>
```

**Return Value**:

```typescript
interface AppInfo {
    name: string;                    // Application name
    version: string;                 // PUP file version
    hasSignature: boolean;           // Whether signed (V1.2)
    certificateThumbprint?: string;  // Certificate fingerprint (V1.2)
    certificateIssuer?: string;      // Certificate issuer (V1.2)
    certificateSubject?: string;     // Certificate subject (V1.2)
    certificateValidFrom?: Date;     // Certificate valid from date (V1.2)
    certificateValidTo?: Date;       // Certificate valid to date (V1.2)
}
```

**Example**:

```javascript
// Get application information
const appInfo = await puppet.application.getAppInfo();
console.log('Application name:', appInfo.name);
console.log('PUP version:', appInfo.version);

// Check signature status (V1.2 format)
if (appInfo.hasSignature) {
    console.log('✓ Application is signed');
    console.log('Certificate fingerprint:', appInfo.certificateThumbprint);
    console.log('Certificate issuer:', appInfo.certificateIssuer);
    console.log('Certificate subject:', appInfo.certificateSubject);
    console.log('Valid period:', appInfo.certificateValidFrom, 'to', appInfo.certificateValidTo);
} else {
    console.warn('✗ Application is not signed');
}
```

**Notes**:

- `hasSignature`, `certificateThumbprint` and other fields are only available in V1.2 format
- V1.0 and V1.1 formats will have these fields as `undefined`
- Used for application security verification and integrity checking
- Recommended to use signed PUP files in production environments

## Usage Examples

### Application Initialization

```javascript
async function initializeApp() {
    // Get application information
    const appDir = await puppet.application.getAssemblyDirectory();
    const dataDir = await puppet.application.getAppDataDirectory();
    const user = await puppet.application.getCurrentUser();
    
    console.log('Application directory:', appDir);
    console.log('Data directory:', dataDir);
    console.log('Current user:', user.name);
    
    // Load configuration
    const config = await loadConfig();
    await applyConfig(config);
}

// Initialize application
window.addEventListener('DOMContentLoaded', initializeApp);
```

### Configuration Management

```javascript
// Configuration manager (based on puppet.ini)
class ConfigManager {
    constructor() {
        this.defaults = {
            'file': '',
            'app.language': 'en-US',
            'app.theme': 'light',
            'window.startup_position': 'center',
            'window.borderless': 'false',
            'window.window_size': '800,600'
        };
    }
    
    async save(section, key, value) {
        // Set configuration (will show confirmation dialog)
        await puppet.application.setConfig(section, key, value);
    }
    
    async resetToDefaults() {
        // Reset to defaults (each will show confirmation)
        for (const [configKey, defaultValue] of Object.entries(this.defaults)) {
            const parts = configKey.split('.');
            if (parts.length === 2) {
                await this.save(parts[0], parts[1], defaultValue);
            } else {
                await this.save('file', parts[0], defaultValue);
            }
        }
    }
}

// Use configuration manager
const configManager = new ConfigManager();

// Modify PUP file path
await configManager.save('file', 'file', 'C:\\MyApp\\app.pup');

// Modify application language
await configManager.save('app', 'language', 'en-US');

// Modify window startup position
await configManager.save('window', 'startup_position', '100,100');
```

### File Association Open

```javascript
async function openFile(filePath) {
    // Check if file exists
    if (!await puppet.fs.exists(filePath)) {
        throw new Error('File does not exist');
    }
    
    // Get file extension
    const extension = filePath.split('.').pop().toLowerCase();
    
    // Process based on file type
    switch (extension) {
        case 'txt':
        case 'json':
        case 'md':
            // Open within application
            const content = await puppet.fs.readFileAsText(filePath);
            displayContent(content);
            break;
            
        case 'pdf':
        case 'doc':
        case 'docx':
            // Open with external program
            await puppet.application.execute(filePath);
            break;
            
        case 'http':
        case 'https':
            // URL
            await puppet.application.execute(filePath);
            break;
            
        default:
            throw new Error('Unsupported file type');
    }
}

// Usage example
await openFile('document.pdf');
await openFile('data.json');
```

### Application Update

```javascript
async function checkForUpdates() {
    const currentVersionJson = await puppet.storage.getItem('default', 'version');
    const currentVersion = currentVersionJson ? JSON.parse(currentVersionJson) : '1.0.0';
    const latestVersion = await fetchLatestVersion();
    
    if (compareVersions(latestVersion, currentVersion) > 0) {
        // New version found
        showUpdateNotification(latestVersion);
    }
}

async function performUpdate() {
    // Download update
    const updateFile = await downloadUpdate();
    
    // Execute update program
    await puppet.application.execute(updateFile);
    
    // Close current application
    await puppet.application.close();
}

// Check for updates
await checkForUpdates();
```

### Shortcut Creation

```javascript
async function createShortcut() {
    const appDir = await puppet.application.getAssemblyDirectory();
    const exePath = appDir + '\\puppet.exe';
    const pupPath = appDir + '\\app.pup';
    
    const desktop = Environment.GetFolderPath(
        Environment.SpecialFolder.Desktop
    );
    
    const shortcutPath = desktop + '\\MyApp.lnk';
    
    // Create shortcut
    await puppet.application.execute(
        `powershell -Command "$s=(New-Object -COM WScript.Shell).CreateShortcut('${shortcutPath}');$s.TargetPath='${exePath}';$s.Arguments='--load-pup ${pupPath}';$s.Save()"`
    );
}
```

## Best Practices

### 1. Configuration Management

Use structured configuration management:

```javascript
class AppConfig {
    constructor() {
        this.config = {};
    }
    
    async init() {
        this.config = await this.load();
    }
    
    async load() {
        const dataJson = await puppet.storage.getItem('default', 'appConfig');
        return dataJson ? JSON.parse(dataJson) : {};
    }
    
    async save() {
        await puppet.storage.setItem('default', 'appConfig', JSON.stringify(this.config));
    }
    
    get(key, defaultValue) {
        return this.config[key] ?? defaultValue;
    }
    
    set(key, value) {
        this.config[key] = value;
        this.save();
    }
}
```

### 2. Error Handling

Comprehensive error handling mechanism:

```javascript
async function safeExecute(command) {
    try {
        await puppet.application.execute(command);
        return true;
    } catch (error) {
        puppet.log.error('Command execution failed:', error.message);
        showError('Operation failed: ' + error.message);
        return false;
    }
}
```

### 3. Path Handling

Use proper path handling:

```javascript
async function getAppPath(filename) {
    const appDir = await puppet.application.getAssemblyDirectory();
    return path.join(appDir, filename);
}

async function getDataPath(filename) {
    const dataDir = await puppet.application.getAppDataDirectory();
    return path.join(dataDir, filename);
}
```

### 4. User Data Storage

Store user data in application data directory:

```javascript
async function saveUserData(data) {
    const dataDir = await puppet.application.getAppDataDirectory();
    const userDataFile = dataDir + '\\userdata.json';
    
    await puppet.fs.writeTextToFile(
        userDataFile,
        JSON.stringify(data, null, 2)
    );
}

async function loadUserData() {
    const dataDir = await puppet.application.getAppDataDirectory();
    const userDataFile = dataDir + '\\userdata.json';
    
    if (await puppet.fs.exists(userDataFile)) {
        const content = await puppet.fs.readFileAsText(userDataFile);
        return JSON.parse(content);
    }
    
    return {};
}
```

## Related Resources

- [Windows Program Execution](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process): Process class documentation
- [Application Data Storage](https://learn.microsoft.com/en-us/windows/win32/shell/knownfolderid): Known Folder IDs
- [JSON Configuration](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON): JSON usage guide

## Common Questions

### Q: Why does executing commands show a confirmation dialog?

A: For security reasons, executing programs in system directories or dangerous operations will show a permission confirmation dialog.

### Q: How to permanently remember user's permission choice?

A: In the permission dialog, select "Permanently block", the choice will be remembered.

### Q: Where is configuration data stored?

A: Configuration data is stored in the `puppet.ini` file in the application directory, which is the global configuration file for the Puppet framework.

### Q: Why does a confirmation dialog appear when modifying configuration?

A: To prevent accidental operations, a confirmation dialog will appear when modifying the `puppet.ini` file, and the user can cancel the modification.

### Q: What data types does setConfig support?

A: Currently only string types are supported. If you need to store complex data, convert to JSON string first:

```javascript
// ✅ Recommended: Use storage to store objects
const obj = { theme: 'dark', fontSize: 16 };
await puppet.storage.setItem('default', 'preferences', JSON.stringify(obj));

// Read object
const content = await puppet.storage.getItem('default', 'preferences');
const preferences = JSON.parse(content);
```

### Q: How to preserve state after application restart?

A: Use `puppet.storage` to save state:

```javascript
// Save state
await puppet.storage.setItem('default', 'lastPage', currentPage);

// Restore state
const lastPage = await puppet.storage.getItem('default', 'lastPage');
if (lastPage) {
    navigateTo(lastPage);
}
```

::: tip Note
`puppet.storage` is specifically designed for application data persistence, more convenient and secure than `setConfig()`.
:::