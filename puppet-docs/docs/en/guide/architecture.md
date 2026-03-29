---
title: Architecture Design
permalink: /en/guide/architecture.html
createTime: 2026/03/28 14:53:30
---

# Architecture Design

This document delves into the internal architecture, core components, and their interactions within the Puppet framework. Understanding these contents will help you better use the framework and perform extensions and optimizations when needed.

## Overall Architecture

The Puppet framework adopts a layered architecture design, with each layer having clear responsibilities and boundaries.

```
┌─────────────────────────────────────────────────────────────┐
│                      User Application Layer                   │
│                  (HTML/CSS/JavaScript)                       │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    JavaScript API Layer                     │
│              (window.puppet.* namespace)                    │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                      Communication Bridge Layer              │
│              (COM Interop + WebMessage)                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                      Controller Layer                        │
│  ┌──────────┬──────────┬──────────┬──────────┬──────────┐   │
│  │ Window   │ File     │ App      │ System   │ Event    │   │
│  │ Controller│ System  │ Controller│ Controller│ Controller │   │
│  └──────────┴──────────┴──────────┴──────────┴──────────┘   │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     Platform Adaptation Layer                │
│              (Windows Forms + WebView2)                      │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     Windows System Layer                     │
│              (Win32 API / WMI / .NET Framework)              │
└─────────────────────────────────────────────────────────────┘
```

## Core Components Detailed

### 1. Application Entry (Program.cs)

`Program.cs` is the entry point of the entire application, responsible for:

- **Security Initialization**: Generate and initialize communication keys
- **Command Line Processing**: Parse command-line parameters for three running modes
- **Service Management**: Create and manage PupServer instances
- **Application Launch**: Start Windows Forms application

#### Command Line Parameter Processing

```csharp
// Supports three running modes
puppet.exe                        // GUI mode
puppet.exe --create-pup -i <folder> -o <file.pup> [-p <password>]  // Create PUP
puppet.exe --load-pup <file.pup>  // Load PUP file
puppet.exe --nake-load <folder>   // Load bare folder
```

::: info See Documentation
For detailed command-line parameter explanations, please refer to [Command Line Parameters](./cli-parameters.html) documentation.
:::

### 2. Main Window (Form1.cs)

`Form1.cs` is the application's main window, hosting the WebView2 control and coordinating various components.

#### Main Responsibilities

- **WebView2 Initialization**: Configure and initialize WebView2 control
- **JavaScript Injection**: Inject all controllers into JavaScript environment
- **Message Handling**: Handle messages and requests from Web layer
- **Security Verification**: Verify keys for all incoming requests
- **Window Management**: Handle window dragging, transparency effects, etc.
- **Icon Management**: Automatically retrieve and set window icon

#### Key Code Snippets

```csharp
// WebView2 initialization
await webView21.CoreWebView2.EnsureCoreWebView2Async(null);

// Inject JavaScript API
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.puppet = {
        window: new WindowControllerProxy(),
        application: new ApplicationControllerProxy(),
        fs: new FileSystemControllerProxy(),
        // ... other controllers
    };
");

// Handle WebMessage
webView21.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
```

### 3. Controller Layer (Controllers)

The controller layer is the core of the Puppet framework, with each controller responsible for a group of related functions.

#### Controller Architecture

Each controller follows the same design pattern:

```
Controller Base Class (optional)
    ↓
Specific Controller (e.g., WindowController)
    ↓
JavaScript Proxy Class (e.g., WindowControllerProxy)
    ↓
Web API Routing
```

#### Controller List

| Controller | Function | File |
|------------|----------|------|
| `ApplicationController` | Application lifecycle management, external program execution | `Controllers/ApplicationController.cs` |
| `FileSystemController` | File system operations | `Controllers/FileSystemController.cs` |
| `WindowController` | Window management | `Controllers/WindowController.cs` |
| `EventController` | Event system and device monitoring | `Controllers/EventController.cs` |
| `LogController` | Log output | `Controllers/LogController.cs` |
| `SystemController` | System information, input simulation | `Controllers/SystemController.cs` |
| `TrayController` | Tray icon management | `Controllers/TrayController.cs` |
| `StorageController` | Persistent storage (supports signature verification) | `StorageController.cs` |

### 4. PUP Server (PupServer.cs)

`PupServer.cs` is a lightweight HTTP server responsible for providing web content.

#### Two Working Modes

##### PUP File Mode

```csharp
// PUP file structure
[PUP V1.0 Header 8 bytes] + [AES Encrypted ZIP Password 32 bytes] + [ZIP Data]
// PUP V1.1/V1.2 structure:
[PUP V1.1/V1.2 Header 8 bytes] + [Script Length 4 bytes] + [Script Content] + [Certificate Length 4 bytes] + [Certificate Data] + [Encrypted Private Key Length 4 bytes] + [Encrypted Private Key Data] + [Encrypted Password 32 bytes] + [ZIP Data]
```

- Parse custom PUP file format (supports V1.0, V1.1, V1.2)
- V1.2 format supports loading certificates and encrypted private keys
- Decrypt ZIP key
- Read file content from memory

##### Bare Folder Mode

- Directly provide files from file system
- Support hot reload (used during development)
- Automatically detect file changes

#### HTTP Routing

```
/                        → index.html
/*.html                  → Static HTML files
/*.css                   → Static CSS files
/*.js                    → Static JavaScript files
/api/*                   → API requests (forward to controllers)
```

### 5. Utility Classes

#### Encryption Utility (AesHelper.cs)

Responsible for PUP file encryption and decryption:

```csharp
// Fixed key for encrypting ZIP key
private static readonly byte[] FixedKey = Encoding.UTF8.GetBytes("ILOVEPUPPET");

public static string Encrypt(string plainText, string key)
public static string Decrypt(string cipherText, string key)
```

#### Key Management (SecretKey.cs)

Generate and manage runtime keys:

```csharp
// Generate random key
public static string GenerateKey()

// Initialize and store key
public static void Initialize()
```

#### Permission Dialog (PermissionDialog.cs)

Custom permission confirmation dialog:

- Three operations: Allow, Deny, Permanently Block
- Remember user choice
- Support custom messages

#### Port Selector (PortSelector.cs)

Automatically select available port:

```csharp
// Start from 7738, increment until available port is found
public static int SelectAvailablePort(int startPort = 7738)
```

#### Signing Utility Classes (Core/Security/)

Located in `Core/Security/` directory, providing complete signing and verification functionality:

- **AppSignatureValidator.cs** - Database signature verifier
- **CertificateUtils.cs** - Certificate utility class (certificate import, export, verification)
- **CryptoUtils.cs** - Encryption utility class (AES-256-GCM, PBKDF2 key derivation)
- **SecurityException.cs** - Security exception class

**Signing Process**:
```
Database Content → SHA256 Hash → RSA Private Key Signature → Store in puppet_metadata Table
```

**Verification Process**:
```
Database Content → SHA256 Hash → Certificate Public Key Verification → Compare Signature Data
```

## Data Flow Detailed

### 1. JavaScript to C# Call

```
User Action (e.g., button click)
    ↓
JavaScript code calls puppet.window.setBorderless(true)
    ↓
WebMessage sent to C# layer
    ↓
Form1.cs receives message
    ↓
Verify key
    ↓
Route to WindowController
    ↓
Call Windows API to modify window style
    ↓
Return result to JavaScript
```

#### Code Example

**JavaScript Layer**:
```javascript
// User code
await puppet.window.setBorderless(true);

// Internal implementation
class WindowControllerProxy {
    async setBorderless(value) {
        return window.chrome.webview.postMessage({
            controller: 'window',
            action: 'setBorderless',
            params: [value]
        });
    }
}
```

**C# Layer**:
```csharp
// Form1.cs
private async void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    var message = JsonConvert.DeserializeObject<WebMessage>(e.WebMessageAsJson);
    
    // Verify key
    if (message.Secret != SecretKey.Key)
        return;
    
    // Route to controller
    var result = await RouteToController(message);
    
    // Return result
    webView21.CoreWebView2.PostWebMessageAsJson(result);
}
```

### 2. Events from C# to JavaScript

```
System Event (e.g., USB insertion)
    ↓
WMI monitor detects event
    ↓
EventController handles event
    ↓
Construct event object
    ↓
Send to JavaScript via WebMessage
    ↓
Call registered callback function
    ↓
User code executes
```

#### Code Example

**C# Layer**:
```csharp
// EventController.cs
private void OnUSBArrival(object sender, EventArrivedEventArgs e)
{
    var device = ExtractDeviceInfo(e.NewEvent);
    var message = new {
        type = 'event',
        event = 'usb-plug-in',
        data = device
    };
    
    webView21.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(message));
}
```

**JavaScript Layer**:
```javascript
// Event listener
puppet.events.addEventListener('usb-plug-in', function(e) {
    console.log('USB device inserted:', e.Device);
});
```

## Security Mechanisms

### 1. Communication Security

All communication between JavaScript and C# is verified with a key:

```csharp
// Generate random key
string secret = SecretKey.GenerateKey();

// JavaScript injection includes key
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.PUPPET_SECRET = '" + secret + @"';
");

// Verify key when receiving message
if (message.Secret != SecretKey.Key)
    return; // Reject request
```

### 2. File System Protection

Automatically block access to system sensitive directories:

```csharp
// FileSystemController.cs
private static readonly string[] ProtectedPaths = {
    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
    Environment.GetFolderPath(Environment.SpecialFolder.System),
    Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)
};

private bool IsProtectedPath(string path)
{
    foreach (var protectedPath in ProtectedPaths)
    {
        if (path.StartsWith(protectedPath, StringComparison.OrdinalIgnoreCase))
            return true;
    }
    return false;
}
```

### 3. Permission Confirmation

Show confirmation dialog for dangerous operations:

```csharp
// Confirm before executing system directory program
if (IsSystemPath(command))
{
    var result = PermissionDialog.Show("Execute System Program", "Are you sure you want to execute this program?");
    if (result != DialogResult.Yes)
        return;
}
```

## Performance Optimization

### 1. WebView2 Optimization

- **Disable unnecessary features**: Turn off browser features not needed
- **Cache management**: Configure cache strategy appropriately
- **Process isolation**: Use single process mode to reduce memory usage

### 2. Memory Management

- **Release resources promptly**: Use `using` statements to manage resources
- **Avoid memory leaks**: Handle event subscriptions correctly
- **Garbage collection optimization**: Reduce unnecessary object creation

### 3. File Operation Optimization

- **Asynchronous I/O**: Use async methods to avoid blocking UI thread
- **Batch operations**: Combine multiple file operations
- **Cache strategy**: Cache frequently accessed files

## Extensibility

### 1. Adding New Controllers

To add new functionality modules, create new controllers:

```csharp
public class MyFeatureController
{
    public async Task<string> MyMethod(string param)
    {
        // Implement functionality
        return "result";
    }
}
```

Then register in `Form1.cs`:

```csharp
// Create controller instance
var myController = new MyFeatureController();

// Inject into JavaScript
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.puppet.myFeature = new MyFeatureControllerProxy();
");
```

### 2. Custom Events

Extend event system to support more event types:

```csharp
// Add new event in EventController
private void StartMyEventMonitoring()
{
    // Implement monitoring logic
}

// Trigger event
private void OnMyEvent(object sender, EventArgs e)
{
    var message = new {
        type = 'event',
        event = 'my-event',
        data = new { /* event data */ }
    };
    
    webView21.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(message));
}
```

## Debugging and Monitoring

### 1. Logging System

Use `LogController` to output debug information:

```javascript
puppet.log.info('Debug information');
puppet.log.warn('Warning information');
puppet.log.error('Error information');
```

### 2. Developer Tools

Right-click in Puppet application and select "Inspect" to open browser developer tools:

- Console: View logs and errors
- Network: Monitor HTTP requests
- Elements: Inspect and debug DOM
- Sources: Debug JavaScript code

### 3. Performance Analysis

Use Performance panel in developer tools:

- Record and analyze performance
- Identify performance bottlenecks
- Optimize code execution

## Best Practices

### 1. Controller Design

- **Single Responsibility**: Each controller only handles a group of related functions
- **Async First**: Use async methods to avoid blocking
- **Error Handling**: Comprehensive exception handling and error return

### 2. API Design

- **Consistency**: Maintain consistent API naming and usage
- **Predictability**: Method names should clearly express their functionality
- **Complete Documentation**: Provide detailed API documentation

### 3. Security Considerations

- **Input Validation**: Validate all user inputs
- **Path Normalization**: Prevent path traversal attacks
- **Permission Checks**: Dangerous operations require permission confirmation

## Related Resources

- [Microsoft WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/): WebView2 official documentation
- [.NET Documentation](https://learn.microsoft.com/en-us/dotnet/): .NET framework documentation
- [Windows API Documentation](https://learn.microsoft.com/en-us/windows/win32/api/): Windows API reference

## Next Steps

After understanding the architecture, it is recommended to:

1. View [API Documentation](../api/) to learn specific usage
2. Read [Security Mechanisms](./security.html) to understand security details
3. Reference [Best Practices](./best-practices.html) to improve development quality