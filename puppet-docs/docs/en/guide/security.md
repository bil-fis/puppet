---
title: Security
permalink: /en/guide/security.html
createTime: 2026/03/28 14:59:03
---

# Security Mechanisms

The Puppet Framework has built-in multi-layer security mechanisms to protect system and user data security. This chapter details Puppet's security features, potential risks, and security best practices.

## Security Architecture

Puppet adopts a defense-in-depth security strategy, providing protection at multiple levels:

```
┌─────────────────────────────────────────┐
│         Application Layer Security               │
│  ┌───────────────────────────────────┐  │
│  │  Permission Confirmation Dialog    │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         Communication Layer Security       │
│  ┌───────────────────────────────────┐  │
│  │  Key Verification                 │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         File System Security               │
│  ┌───────────────────────────────────┐  │
│  │  Path Protection                  │  │
│  └───────────────────────────────────┘  │
├─────────────────────────────────────────┤
│         Data Layer Security               │
│  ┌───────────────────────────────────┐  │
│  │  PUP Encryption                   │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

## Core Security Features

### 1. Communication Security

#### Key Verification Mechanism

All communication between JavaScript and C# is verified with keys to prevent unauthorized access.

```csharp
// Generate random key
string secret = SecretKey.GenerateKey();

// Store key
SecretKey.Key = secret;

// Inject into JavaScript
await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(@"
    window.PUPPET_SECRET = '" + secret + @"';
");
```

**Working Principle**:

1. Generate random key on application startup
2. Inject key into JavaScript environment
3. All requests must include correct key
4. C# layer verifies key before processing request

**Verification Code**:

```csharp
private async void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    var message = JsonConvert.DeserializeObject<WebMessage>(e.WebMessageAsJson);
    
    // Verify key
    if (message.Secret != SecretKey.Key)
    {
        puppet.log.warn("Unauthorized request rejected");
        return;
    }
    
    // Process request
    var result = await RouteToController(message);
    webView21.CoreWebView2.PostWebMessageAsJson(result);
}
```

::: tip Security Note
The key is regenerated each time the application starts, ensuring isolation between different sessions.
:::

### 2. File System Protection

#### Path Protection

Automatically blocks access to Windows system sensitive directories to prevent malicious operations.

```csharp
private static readonly string[] ProtectedPaths = {
    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
    Environment.GetFolderPath(Environment.SpecialFolder.System),
    Environment.GetFolderPath(Environment.SpecialFolder.SystemX86),
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
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

**Protected Paths**:

- `C:\Windows` - Windows system directory
- `C:\Windows\System32` - System files directory
- `C:\Windows\SysWOW64` - 64-bit system files directory
- `C:\Program Files` - Program files directory
- `C:\Program Files (x86)` - 32-bit program files directory

**Usage Example**:

```csharp
public async Task<string> ReadFile(string path)
{
    // Check if path is protected
    if (IsProtectedPath(path))
    {
        throw new UnauthorizedAccessException("Cannot access system directory");
    }
    
    // Read file
    return File.ReadAllText(path);
}
```

#### Path Normalization

Prevents path traversal attacks to ensure path security.

```csharp
private string NormalizePath(string path)
{
    // Get full path
    string fullPath = Path.GetFullPath(path);
    
    // Normalize path separators
    fullPath = fullPath.Replace('/', '\\');
    
    // Verify path format
    if (path.Contains("..") || path.Contains("~"))
    {
        throw new ArgumentException("Illegal path format");
    }
    
    return fullPath;
}
```

**Attack Example**:

```javascript
// Attempt path traversal attack
puppet.fs.readFileAsText("../../Windows/System32/config.txt");
// Will be intercepted, throws exception
```

### 3. Permission Confirmation

#### Dangerous Operation Confirmation

For operations that may affect system security, show user confirmation dialog.

```csharp
public async Task ExecuteCommand(string command)
{
    // Check if it's a system command
    if (IsSystemCommand(command))
    {
        var result = PermissionDialog.Show(
            "Execute System Command",
            $"Are you sure you want to execute the following command?\n\n{command}",
            PermissionDialogType.Warning
        );
        
        if (result != DialogResult.Yes)
        {
            throw new OperationCanceledException("User canceled operation");
        }
    }
    
    // Execute command
    Process.Start(command);
}
```

**Operations Requiring Confirmation**:

- Execute programs in system directory
- Write to system directory
- Delete important files
- Execute system commands
- Modify registry

**Permission Dialog Options**:

- **Allow**: Allow this operation
- **Deny**: Deny this operation
- **Block Permanently**: Permanently block this type of operation (remember choice)

### 4. PUP Encryption

#### File Encryption

PUP files use AES-256 encryption to protect source code.

```csharp
// Encrypt ZIP password
string encryptedPassword = AesHelper.Encrypt(zipPassword, "ILOVEPUPPET");

// Decrypt ZIP password
string zipPassword = AesHelper.Decrypt(encryptedPassword, "ILOVEPUPPET");
```

**Encryption Features**:

- Algorithm: AES (Advanced Encryption Standard)
- Key Length: 256 bits
- Mode: CBC (Cipher Block Chaining)
- Padding: PKCS7

::: warning Security Limitation
The encrypted ZIP key is encrypted using the fixed key `"ILOVEPUPPET"`, which is a lightweight protection method. If you need stronger security, consider using file system encryption (such as BitLocker).
:::

### 5. Data Signature Verification

#### Signature Mechanism

Puppet Storage API provides a self-signed certificate-based data signature verification mechanism to prevent database tampering. This mechanism references Android APK signature design, providing enterprise-level data integrity protection.

**Signature Architecture**:

```
┌─────────────────────────────────────────────┐
│  Application Layer                         │
│  ┌─────────────────────────────────────┐  │
│  │  RSA 2048/4096-bit Key Pairs       │  │
│  │  Self-signed X.509 Certificate      │  │
│  │  SHA256withRSA Signature            │  │
│  └─────────────────────────────────────┘  │
├─────────────────────────────────────────────┤
│  Verification Layer                       │
│  ┌─────────────────────────────────────┐  │
│  │  Certificate Validity Verification  │  │
│  │  Self-signed Status Check           │  │
│  │  Signature Integrity Verification   │  │
│  │  Certificate Fingerprint Comparison │  │
│  └─────────────────────────────────────┘  │
├─────────────────────────────────────────────┤
│  Data Layer                               │
│  ┌─────────────────────────────────────┐  │
│  │  Database Metadata Table            │  │
│  │  Application ID (AppID)             │  │
│  │  Certificate Fingerprint            │  │
│  │  Signature Data                     │  │
│  └─────────────────────────────────────┘  │
└─────────────────────────────────────────────┘
```

#### Security Features

**1. Data Integrity Protection**

- Uses SHA256withRSA algorithm to sign database content
- Any modifications to the database will cause signature verification failure
- Provides tamper-proof guarantee

**2. Identity Verification**

- Verifies database creator through certificate application ID (CN)
- Ensures certificate uniqueness through certificate fingerprint
- Prevents impersonation

**3. Key Protection**

- Private keys in PUP files are encrypted and stored using AES-256-GCM
- Key derivation uses PBKDF2 (100,000 iterations)
- Prevents private key leakage

**4. Algorithm Strength**

- RSA Keys: 2048 or 4096 bits
- Signature Algorithm: SHA256withRSA
- Encryption Algorithm: AES-256-GCM
- Key Derivation: PBKDF2 + SHA256

#### Signature Process

**1. Generate Signing Key Pairs**

```bash
# Interactive generation
puppet.exe --generate-signing-key --interactive
```

Generated files:
- `app.crt` - Self-signed certificate (contains public key)
- `app.key` - RSA private key (encrypted storage)

**2. Create Signed PUP File**

```bash
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

**3. Automatic Database Signing**

When using PUP files containing certificates:

- **Create New Database**: Automatically sign using private key
- **Open Existing Database**: Automatically verify signature
- **Verification Failure**: Log warning, but allow access (backward compatibility)

#### Verification Process

**Signature Verification Steps**:

```
1. Read database content
2. Calculate SHA256 hash of database content
3. Read signature from database metadata
4. Extract certificate from PUP file
5. Verify signature using certificate public key
6. Check certificate validity and self-signed status
7. Verification passes → Allow access
8. Verification fails → Log warning
```

**Verification Failure Handling**:

```
┌─────────────────────────────────────────────┐
│  Verification Failure Scenarios            │
├─────────────────────────────────────────────┤
│  1. Database tampered                      │
│     → Warning: "Database signature verification failed" │
│     → Still allow access (backward compatibility)       │
├─────────────────────────────────────────────┤
│  2. Certificate mismatch                  │
│     → Warning: "Certificate fingerprint mismatch"       │
│     → Still allow access (backward compatibility)       │
├─────────────────────────────────────────────┤
│  3. Certificate expired                    │
│     → Warning: "Certificate expired"       │
│     → Still allow access (backward compatibility)       │
└─────────────────────────────────────────────┘
```

#### Security Notes

**1. Private Key Protection**

```csharp
// Private key is encrypted and stored in PUP file
byte[] encryptedPrivateKey = CryptoUtils.EncryptWithPassword(
    privateKeyBytes,
    password
);

// Decryption requires correct password
var privateKey = AppSignatureGenerator.DecryptPrivateKey(
    encryptedPrivateKey,
    password
);
```

**2. Certificate Validity**

- Recommended to set long validity period (such as 25 years)
- Regularly check certificate expiration time
- Generate new certificate and re-sign before expiration

**3. Key Backup**

```bash
# Backup certificate and private key
copy app.crt app.crt.backup
copy app.key app.key.backup

# Store in secure location
# Do not upload to public repositories
```

**4. Signing is Irreversible**

- Once signed, cannot modify signature without breaking verification
- Modifying database content will cause signature verification failure
- Need to re-sign to modify

#### Security Best Practices

**1. Always Use Signing**

```bash
# Always include certificate and private key when creating PUP files
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

**2. Regularly Verify Signatures**

```bash
# Regularly verify database signatures
puppet.exe --verify-database default.db --certificate app.crt
```

**3. Monitor Signature Verification Failures**

```javascript
// Monitor signature verification failures in application
const signatureLogs = await puppet.log.getLogs();
const failures = signatureLogs.filter(log => 
  log.message.includes('signature verification failed')
);

if (failures.length > 0) {
  console.warn('Signature verification failures found:', failures);
}
```

**4. Protect Key Files**

```bash
# Set file permissions (Linux/macOS)
chmod 600 app.key

# Set file permissions (Windows)
icacls app.key /inheritance:r
icacls app.key /grant:r "%USERNAME%:F"
```

**5. Use Strong Keys**

```bash
# Use 4096-bit keys (higher security)
puppet.exe --generate-signing-key --key-size 4096
```

#### Security Threat Analysis

**1. Man-in-the-Middle Attack**

- **Risk**: Attacker intercepts PUP file and replaces certificate
- **Protection**: Certificate fingerprint verification ensures certificate hasn't been replaced

**2. Replay Attack**

- **Risk**: Attacker uses old signed database to replace new database
- **Protection**: Signature includes timestamp, can detect replay

**3. Key Leakage**

- **Risk**: Private key is leaked, attacker can forge signatures
- **Protection**: Private key encrypted storage, regular key rotation

**4. Certificate Forgery**

- **Risk**: Attacker creates fake certificate and signs malicious database
- **Protection**: Check certificate fingerprint and self-signed status

#### Security Audit

**Signature Checklist**:

- [ ] All PUP files contain certificate and private key
- [ ] Private key file permissions set correctly
- [ ] Regularly verify database signatures
- [ ] Monitor signature verification failure events
- [ ] Certificate validity period set reasonably
- [ ] Keys backed up in secure location
- [ ] Use sufficient key strength (2048+ bits)
- [ ] Private key protected with strong password

**Log Monitoring**:

```javascript
// Monitor signature-related events
async function monitorSignatureEvents() {
  const logs = await puppet.log.getLogs();
  
  const signatureEvents = logs.filter(log =>
    log.message.includes('signature') ||
    log.message.includes('certificate') ||
    log.message.includes('fingerprint')
  );
  
  for (const event of signatureEvents) {
    console.log(`[${event.time}] ${event.level}: ${event.message}`);
  }
}
```

## Security Best Practices

### 1. Input Validation

#### JavaScript Layer

```javascript
// Validate file path
async function safeReadFile(path) {
    // Basic validation
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
    
    // Read file
    return await puppet.fs.readFileAsText(path);
}
```

#### C# Layer

```csharp
public async Task<string> ReadFile(string path)
{
    // Validate path
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new ArgumentNullException(nameof(path));
    }
    
    // Normalize path
    path = NormalizePath(path);
    
    // Check path protection
    if (IsProtectedPath(path))
    {
        throw new UnauthorizedAccessException("Cannot access system directory");
    }
    
    // Read file
    return File.ReadAllText(path);
}
```

### 2. Error Handling

#### Secure Error Messages

```javascript
try {
    const result = await puppet.fs.readFileAsText(path);
    return result;
} catch (error) {
    // Don't expose sensitive information
    console.error('File read failed');
    throw new Error('Cannot read file');
}
```

#### Logging

```javascript
// Log security events
puppet.log.warn('Attempt to access protected path: ' + path);
puppet.log.error('Permission denied');
```

### 3. Minimum Permissions

#### Principle of Least Privilege

Grant applications only the minimum permissions needed:

```javascript
// Only request necessary file access permissions
const file = await puppet.fs.readFileAsText('config.json');

// Avoid direct system command execution
// Not recommended:
// await puppet.application.execute('cmd /c del C:\\Windows\\file.txt');
```

#### Limit Access Scope

```javascript
// Limit operations to application directory
const appPath = await puppet.application.getAssemblyDirectory();
const configPath = appPath + '\\config.json';
const config = await puppet.fs.readFileAsText(configPath);
```

### 4. Data Protection

#### Encrypt Sensitive Data

```javascript
// Encrypt sensitive data
function encryptData(data, key) {
    // Use Web Crypto API for encryption
    // ...
}

// Store encrypted data
await puppet.fs.writeTextToFile('encrypted.dat', encryptedData);
```

#### Secure Storage

```javascript
// Use puppet.json for configuration storage (relatively secure)
await puppet.application.setConfig('apiKey', encryptedKey);

// Don't hardcode sensitive information in code
// Not recommended:
// const apiKey = 'my-secret-key-12345';
```

## Security Audit

### Common Security Risks

#### 1. Path Traversal Attack

**Risk**: Access system files through special characters

**Protection**:
- Path normalization
- Path protection list
- Input validation

#### 2. Command Injection

**Risk**: Execute malicious code through command execution

**Protection**:
- Permission confirmation dialog
- Command whitelist
- Parameter validation

#### 3. Unauthorized Access

**Risk**: Unverified requests access system resources

**Protection**:
- Key verification mechanism
- Request signing
- Session management

#### 4. Data Leakage

**Risk**: Sensitive information is leaked

**Protection**:
- Data encryption
- Security logging
- Error handling

### Security Checklist

Before releasing your application, check the following items:

- [ ] All file paths are validated
- [ ] Dangerous operations have permission confirmation
- [ ] No hardcoded sensitive information in code
- [ ] Use encryption to protect PUP files
- [ ] Implement appropriate error handling
- [ ] Log security-related events
- [ ] Limit application access scope
- [ ] Regularly update dependencies
- [ ] Conduct security testing

## Security Tools

### File Scanning

Use Windows Defender to scan PUP files:

```bash
# Scan single file
MpCmdRun.exe -Scan -ScanType 3 -File "app.pup"

# Scan entire directory
MpCmdRun.exe -Scan -ScanType 3 -File "C:\MyApp"
```

### Code Audit

Regularly audit code for security issues:

```javascript
// Check for hardcoded keys
const hasHardcodedKey = code.includes('password') || code.includes('secret');

// Check for dangerous eval calls
const hasEval = code.includes('eval(');

// Check for direct command execution
const hasCommand = code.includes('Application.execute');
```

### Log Analysis

Monitor application logs to detect abnormal behavior:

```javascript
// Log all file accesses
puppet.log.info('Access file: ' + path);

// Log all command executions
puppet.log.warn('Execute command: ' + command);

// Log all errors
puppet.log.error('Operation failed: ' + error.message);
```

## Related Resources

### Microsoft Learn

- [Windows Security Best Practices](https://learn.microsoft.com/en-us/windows/security/): Windows security guide
- [.NET Security Coding](https://learn.microsoft.com/en-us/dotnet/standard/security/): .NET security programming
- [WebView2 Security](https://learn.microsoft.com/en-us/microsoft-edge/webview2/security/): WebView2 security guide

### Mozilla

- [Web Security](https://developer.mozilla.org/en-US/docs/Web/Security): Web security guide
- [HTTPS](https://developer.mozilla.org/en-US/docs/Web/Security/HTTPS): HTTPS best practices
- [CSP](https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP): Content Security Policy

## Next Steps

After understanding security mechanisms, it is recommended to:

1. Audit your application code for security vulnerabilities
2. Implement items from the security checklist
3. Reference [Best Practices](./best-practices.md) to improve application security
4. Regularly conduct security testing and audits