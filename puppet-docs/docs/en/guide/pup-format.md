---
title: PUP File Format
permalink: /en/guide/pup-format.html
createTime: 2026/03/28 14:55:17
---

# PUP File Format

PUP (Puppet Package) is a dedicated application packaging format for the Puppet framework. It packages an entire web application into a single file, supports encryption protection, and facilitates distribution and deployment.

## Overview

PUP file is a custom packaging format that combines ZIP compression and AES encryption technology, with the following features:

- **Single file distribution**: All resources packaged into one file
- **Password protection**: Supports AES-256 encryption
- **Fast loading**: Optimized file structure and loading mechanism
- **Cross-version compatibility**: Version identifiers ensure compatibility

## File Structure

### Version Overview

PUP file format supports multiple versions:

- **V1.0**: Basic version, supports ZIP packaging and encryption
- **V1.1**: Enhanced version, supports startup script functionality
- **V1.2**: Signature version, supports certificates and private keys for database signature verification

### V1.0 Binary Structure

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.0 Header (8 bytes)                  │
├─────────────────────────────────────────────────────┤
│           AES Encrypted ZIP Password (32 bytes)      │
├─────────────────────────────────────────────────────┤
│           ZIP Data (variable length)                 │
└─────────────────────────────────────────────────────┘
```

### V1.1 Binary Structure

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.1 Header (8 bytes)                  │
├─────────────────────────────────────────────────────┤
│           Script Length (4 bytes, int32)             │
├─────────────────────────────────────────────────────┤
│           Startup Script Content (variable length)   │
├─────────────────────────────────────────────────────┤
│           AES Encrypted ZIP Password (32 bytes)      │
├─────────────────────────────────────────────────────┤
│           ZIP Data (variable length)                 │
└─────────────────────────────────────────────────────┘
```

### V1.2 Binary Structure (with signature support)

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.2 Header (8 bytes)                  │
├─────────────────────────────────────────────────────┤
│           Script Length (4 bytes, int32)             │
├─────────────────────────────────────────────────────┤
│           Startup Script Content (variable length)   │
├─────────────────────────────────────────────────────┤
│           Certificate Length (4 bytes, int32)        │
├─────────────────────────────────────────────────────┤
│           Certificate Data (variable length, DER)    │
├─────────────────────────────────────────────────────┤
│           Encrypted Private Key Length (4 bytes)    │
├─────────────────────────────────────────────────────┤
│           Encrypted Private Key Data (variable)      │
├─────────────────────────────────────────────────────┤
│           AES Encrypted ZIP Password (32 bytes)      │
├─────────────────────────────────────────────────────┤
│           ZIP Data (variable length)                 │
└─────────────────────────────────────────────────────┘
```

**V1.2 Security Features**:

- **Certificate Protection**: Uses self-signed X.509 certificate for signature verification
- **Private Key Encryption**: Private key encrypted with AES-256-GCM, key derived via PBKDF2
- **Database Signature**: Supports signing and verifying SQLite databases
- **Fingerprint Verification**: Verifies certificate has not been replaced via certificate fingerprint

### Detailed Description

#### 1. Header (8 bytes)

Fixed string used to identify file format and version.

- V1.0: `"PUP V1.0"`
- V1.1: `"PUP V1.1"`
- V1.2: `"PUP V1.2"`

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
private static readonly byte[] PUP_HEADER_V1_2 = Encoding.UTF8.GetBytes("PUP V1.2");
```

#### 2. Encrypted ZIP Password (32 bytes)

The decompression password for the ZIP file, encrypted with a fixed key `"ILOVEPUPPET"` using AES.

```csharp
// Fixed encryption key
private static readonly byte[] FixedKey = Encoding.UTF8.GetBytes("ILOVEPUPPET");

// Encrypt ZIP password
string encryptedPassword = AesHelper.Encrypt(zipPassword, new string(FixedKey));
```

#### 3. ZIP Data

ZIP compressed data containing the entire application files.

## Creating PUP Files

### V1.0 Format

#### Command Line Method

```bash
puppet.exe --create-pup -i <source_folder> -o <output_file.pup> [-p <password>]
```

**Parameter Description**:

- `-i` or `--input`: Source folder path
- `-o` or `--output`: Output PUP file path
- `-p` or `--password`: (Optional) ZIP password for encryption

**Examples**:

```bash
# Create without password
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"

# Create with password
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword"
```

#### Code Method (C#)

Use `PupCreator` class to create PUP file:

```csharp
using Puppet;

// Create PUP file
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword"  // Optional
);
```

### V1.1 Format (with startup script)

#### Command Line Method

```bash
puppet.exe --create-pup -i <source_folder> -o <output_file.pup> [-p <password>] -v 1.1 --script <script_file>
```

**Parameter Description**:

- `-i` or `--input`: Source folder path
- `-o` or `--output`: Output PUP file path
- `-p` or `--password`: (Optional) ZIP password for encryption
- `-v` or `--version`: PUP version, V1.1 requires specification
- `--script`: Startup script file path (required for V1.1)

**Examples**:

```bash
# Create V1.1 format PUP file
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.1 --script "C:\script.txt"

# Create V1.1 format with password
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword" -v 1.1 --script "C:\script.txt"
```

#### Code Method (C#)

Use `PupCreator` class to create V1.1 format PUP file:

```csharp
using Puppet;

// Create V1.1 format PUP file
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword",  // Optional
    version: "1.1",                // Specify version
    scriptFile: @"C:\script.txt"   // Startup script file
);
```

### Startup Script (V1.1)

::: tip Detailed Documentation
For complete usage instructions, syntax reference, and best practices for startup scripts, please refer to [PUP Startup Script](./pup-script.html) documentation.
:::

### Overview

V1.1 format supports automatically executing preset scripts after PUP loading to quickly initialize window state.

### Script Syntax

Startup script uses simple command syntax, one command per line:

```
set <property> <value>
```

### Supported Commands

#### 1. Set Startup Position

**Syntax**:
```
set startup_position <x>,<y>
set startup_position <POSITION>
```

**Parameters**:
- `<x>,<y>`: Specify coordinates, e.g., `100,200`
- `<POSITION>`: Predefined position, supports the following values:
  - `left-top`: Top-left corner
  - `left-bottom`: Bottom-left corner (excluding taskbar)
  - `right-top`: Top-right corner
  - `right-bottom`: Bottom-right corner (excluding taskbar)
  - `center`: Screen center

**Examples**:
```
set startup_position 100,200
set startup_position center
set startup_position right-bottom
```

#### 2. Set Borderless Mode

**Syntax**:
```
set borderless <true|false>
```

**Parameters**:
- `true`: Enable borderless mode
- `false`: Disable borderless mode

**Examples**:
```
set borderless true
set borderless false
```

#### 3. Set Window Size

**Syntax**:
```
set window_size <width>,<height>
```

**Parameters**:
- `<width>`: Window width (pixels)
- `<height>`: Window height (pixels)

**Examples**:
```
set window_size 800,600
set window_size 1024,768
```

### Script Examples

**Example 1: Borderless Window in Bottom Right**
```
set startup_position right-bottom
set borderless true
set window_size 400,300
```

**Example 2: Centered Window with Border**
```
set startup_position center
set borderless false
set window_size 1024,768
```

**Example 3: Specific Position and Size**
```
set startup_position 100,100
set borderless true
set window_size 800,600
```

### Script File Example

Create a script file named `startup.txt`:

```
# Puppet startup script
# Set window as borderless in bottom-right corner
set startup_position right-bottom
set borderless true
set window_size 500,400
```

### Script Execution Timing

- Script executes after PUP file is loaded
- Executes after WebView2 initialization is complete
- Executes before page navigation to application URL
- Script execution errors will not prevent application startup

### Script Limitations

- Only one command per line
- Commands are case-insensitive
- Supports comments starting with `//` or `#`
- Empty lines are ignored

### V1.2 Format (with signature support)

#### Command Line Method

```bash
puppet.exe --create-pup -i <source_folder> -o <output_file.pup> [-p <password>] -v 1.2 --certificate <certificate_file> --private-key <private_key_file> --private-key-password <private_key_password>
```

**Parameter Description**:

- `-i` or `--input`: Source folder path
- `-o` or `--output`: Output PUP file path
- `-p` or `--password`: (Optional) ZIP password for encryption
- `-v` or `--version`: PUP version, V1.2 requires specification
- `--certificate`: Certificate file path (required for V1.2)
- `--private-key`: Private key file path (required for V1.2)
- `--private-key-password`: Private key encryption password (required for V1.2)

**Examples**:

```bash
# Create V1.2 format PUP file
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"

# Create V1.2 format with password and signature
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"
```

#### Code Method (C#)

Use `PupCreator` class to create V1.2 format PUP file:

```csharp
using Puppet;

// Create V1.2 format PUP file
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword",  // Optional
    version: "1.2",                // Specify version
    certificate: @"C:\app.crt",    // Certificate file
    privateKey: @"C:\app.key",     // Private key file
    privateKeyPassword: "MyKeyPassword"  // Private key encryption password
);
```

#### Certificate and Private Key Generation

Use `puppet-sign` tool to generate signing key pair:

```bash
# Generate signing key pair
puppet-sign.exe --generate-signing-key --alias MyApp --organization MyOrg --country CN --validity 3650
```

Generated files:
- `app.crt` - Self-signed certificate (contains public key)
- `app.key` - RSA private key (PKCS#8 format)

#### Database Signature

When using V1.2 format PUP file, database automatically supports signature functionality:

```csharp
// Sign database in code
StorageController storage = new StorageController(form);
storage.SignDatabase("default");
```

Signed database will store in `puppet_metadata` table:
- `app_id` - Application identifier (from certificate CN)
- `certificate_fingerprint` - Certificate fingerprint (SHA256)
- `signature_data` - Signature data (signed with private key)
- `created_at` - Signature timestamp

#### Signature Verification

When loading V1.2 format PUP file, system automatically verifies database signature:

```csharp
// PupServer automatically verifies signature
var server = new PupServer("myapp.pup", 7738);

// Signature is verified on first database access
storage.SetItem("default", "key", "value");
```

Verification failure outputs warning message:
```
✗ Database signature verification failed: default
  WARNING: Database may have been tampered with
```

## Loading PUP Files

### Command Line Method

```bash
puppet.exe --load-pup <file.pup>
```

**Examples**:

```bash
# Load V1.0 format
puppet.exe --load-pup "C:\MyApp.pup"

# Load V1.1 format (automatically executes startup script)
puppet.exe --load-pup "C:\MyAppV1_1.pup"

# Load V1.2 format (automatically loads certificate and private key, supports database signature)
puppet.exe --load-pup "C:\MyAppV1_2.pup"
```

**Automatic Version Recognition**:

PupServer automatically recognizes PUP file version:

- V1.0: Parses header `"PUP V1.0"`
- V1.1: Parses header `"PUP V1.1"` and executes startup script
- V1.2: Parses header `"PUP V1.2"`, loads certificate and private key, supports database signature

### Configuration File Method

Edit `puppet.ini` file:

```ini
[file]
file=C:\MyApp.pup
```

Then run `puppet.exe` directly.

### Loading Process

#### V1.0 Loading Process

```
1. Read first 8 bytes of file, verify header "PUP V1.0"
        ↓
2. Read next 32 bytes (encrypted ZIP password)
        ↓
3. Decrypt ZIP password using fixed key
        ↓
4. Read remaining data (ZIP data)
        ↓
5. Decompress ZIP data using decrypted ZIP password
        ↓
6. Extract files to memory or temporary directory
```

#### V1.1 Loading Process

```
1. Read first 8 bytes of file, verify header "PUP V1.1"
        ↓
2. Read next 4 bytes (script length)
        ↓
3. Read startup script content (variable length)
        ↓
4. Read next 32 bytes (encrypted ZIP password)
        ↓
5. Decrypt ZIP password using fixed key
        ↓
6. Read remaining data (ZIP data)
        ↓
7. Decompress ZIP data using decrypted ZIP password
        ↓
8. Extract files to memory or temporary directory
        ↓
9. Execute startup script (after WebView2 initialization completes)
```

#### V1.2 Loading Process (with signature support)

```
1. Read first 8 bytes of file, verify header "PUP V1.2"
        ↓
2. Read next 4 bytes (script length)
        ↓
3. Read startup script content (variable length)
        ↓
4. Read next 4 bytes (certificate length)
        ↓
5. Read certificate data (variable length, DER format)
        ↓
6. Parse certificate and extract public key and certificate fingerprint
        ↓
7. Read next 4 bytes (encrypted private key length)
        ↓
8. Read encrypted private key data (variable length)
        ↓
9. Use PBKDF2 to derive key and decrypt private key (AES-256-GCM)
        ↓
10. Read next 32 bytes (encrypted ZIP password)
        ↓
11. Decrypt ZIP password using fixed key
        ↓
12. Read remaining data (ZIP data)
        ↓
13. Decompress ZIP data using decrypted ZIP password
        ↓
14. Extract files to memory or temporary directory
        ↓
15. Execute startup script (if present)
        ↓
16. Store certificate and private key parameters for database signing
```

## Encryption Mechanism

### Encryption Process

```
1. Generate random ZIP password
        ↓
2. Encrypt ZIP password using fixed key "ILOVEPUPPET"
        ↓
3. Create ZIP file (using ZIP password for encryption)
        ↓
4. Concatenate: header + encrypted ZIP password + ZIP data
        ↓
5. Write to PUP file
```

### Decryption Process

```
1. Read first 8 bytes of file, verify header
        ↓
2. Read next 32 bytes (encrypted ZIP password)
        ↓
3. Decrypt ZIP password using fixed key
        ↓
4. Read remaining data (ZIP data)
        ↓
5. Decompress ZIP data using decrypted ZIP password
        ↓
6. Extract files to memory or temporary directory
```

### Encryption Algorithm

PUP uses the following encryption algorithms:

- **Encryption Algorithm**: AES (Advanced Encryption Standard)
- **Key Length**: 256 bits
- **Mode**: CBC (Cipher Block Chaining)
- **Padding**: PKCS7

::: tip Security Note
The encrypted ZIP password uses a fixed key `"ILOVEPUPPET"` for encryption, which is a lightweight protection method. If stronger security is needed, it's recommended to use file system encryption (such as BitLocker) or use HTTPS for distribution.
:::

## ZIP Password Generation

If no password is specified, the system automatically generates a random password:

```csharp
private static string GenerateRandomPassword()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, 16)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}
```

## File Verification

PUP server verifies file format when loading:

```csharp
public bool LoadPupFile()
{
    // 1. Read file
    byte[] fileData = File.ReadAllBytes(_pupFilePath);
    
    // 2. Verify header
    if (fileData.Length < 40)  // 8 (header) + 32 (encrypted password)
        return false;
    
    string header = Encoding.UTF8.GetString(fileData, 0, 8);
    if (header != "PUP V1.0")
        return false;
    
    // 3. Extract encrypted ZIP password
    string encryptedPassword = Encoding.UTF8.GetString(fileData, 8, 32);
    
    // 4. Decrypt ZIP password
    string zipPassword = AesHelper.Decrypt(encryptedPassword, new string(FixedKey));
    
    // 5. Extract ZIP data
    byte[] zipData = new byte[fileData.Length - 40];
    Array.Copy(fileData, 40, zipData, 0, zipData.Length);
    
    // 6. Open ZIP file
    _zipFile = new ZipInputStream(new MemoryStream(zipData));
    _zipFile.Password = zipPassword;
    
    return true;
}
```

## Performance Considerations

### File Size

PUP file size depends on:

- Total size of source files
- ZIP compression rate (typically 30-70%)
- Encryption overhead (about 40 bytes)

**Optimization Recommendations**:

- Compress images and media files
- Remove unused resources
- Use CDN to load third-party libraries

### Loading Speed

PUP file loading speed depends on:

- File size
- Disk read speed
- Decryption and decompression speed

**Optimization Recommendations**:

- Keep file size reasonable (recommended < 50MB)
- Use SSD to improve read speed
- Preload frequently used resources

## Version Compatibility

### Header

Currently supported versions:

- V1.0: `"PUP V1.0"` - Basic version
- V1.1: `"PUP V1.1"` - Enhanced version (supports startup script)
- V1.2: `"PUP V1.2"` - Signature version (supports certificate and database signature)

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
private static readonly byte[] PUP_HEADER_V1_2 = Encoding.UTF8.GetBytes("PUP V1.2");
```

### Version Detection

PupServer automatically detects PUP file version and uses appropriate parsing logic:

```csharp
// Example: Version detection
string header = Encoding.UTF8.GetString(fileData, 0, 8);
switch (header)
{
    case "PUP V1.0":
        // Use V1.0 parsing logic
        LoadPupV1_0(fileBytes);
        break;
    case "PUP V1.1":
        // Use V1.1 parsing logic
        LoadPupV1_1(fileBytes);
        break;
    case "PUP V1.2":
        // Use V1.2 parsing logic (with signature support)
        LoadPupV1_2(fileBytes);
        break;
    default:
        throw new NotSupportedException("Unsupported PUP version");
}
```

### Backward Compatibility

- V1.1 format is fully compatible with all V1.0 features
- V1.2 format is fully compatible with all V1.1 features
- V1.1 files contain additional startup script data
- V1.2 files contain certificate and encrypted private key, supporting database signature
- PupServer can load and parse all versions of PUP files
- New projects are recommended to use V1.2 format for enterprise-level security protection

### Version Selection Recommendations

- **V1.0**: Suitable for simple applications that don't need startup configuration
- **V1.1**: Suitable for applications that need preset window state
- **V1.2**: Suitable for applications that need database signature and integrity, recommended for production environments

### Version Upgrade

If you need to upgrade from V1.0 to V1.1:

1. Create a startup script file
2. Use `-v 1.1` and `--script` parameters to recreate PUP file
3. Test the new format PUP file

```bash
# Upgrade to V1.1
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyAppV1_1.pup" -v 1.1 --script "C:\startup.txt"

# Upgrade to V1.2
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyAppV1_2.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"
```

## Comparison with Bare Folder Mode

| Feature | PUP V1.0 | PUP V1.1 | PUP V1.2 | Bare Folder |
|---------|----------|----------|----------|-------------|
| Distribution | Single file | Single file | Single file | Folder |
| Encryption | Supported | Supported | Supported | Not supported |
| Startup Script | Not supported | Supported | Supported | Not supported |
| Database Signature | Not supported | Not supported | Supported | Not supported |
| Certificate Verification | Not supported | Not supported | Supported | Not supported |
| Window Preset | Code control | Script control | Script control | Code control |
| Development Convenience | Low | Low | Medium | High |
| Hot Reload | Not supported | Not supported | Not supported | Supported |
| File Size | Smaller | Slightly larger | Larger | Larger |
| Loading Speed | Slightly slower | Slightly slower | Slightly slower | Fast |
| Security Level | Medium | Medium | High | Low |
| Use Case | Simple app release | Complex app release | Production release | Development debugging |

## Best Practices

### 1. Creating PUP Files

- Create PUP files before release
- Use meaningful passwords
- Test that PUP files can be loaded normally

### 2. Password Management

- Don't hardcode passwords in code
- Use environment variables or configuration files to store passwords
- Change passwords regularly

### 3. File Optimization

- Compress images and media files
- Remove debug code and comments
- Use production build configuration

### 4. Version Control

- Include version number in filename
- Keep historical versions of PUP files
- Record changes for each version

### 5. Signature Management (V1.2)

- Use puppet-sign tool to generate signing key pair
- Use strong password to protect private key (at least 16 characters)
- Regularly backup certificate and private key files
- Securely store private key password, don't commit to version control
- Regularly check certificate validity, renew in advance
- Backup database before signing

### 6. Distribution Strategy

- Use HTTPS to distribute PUP files
- Provide file verification (such as MD5, SHA256)
- Include detailed changelog
- For V1.2 format, can provide certificate fingerprint for verification

## Troubleshooting

### Common Problems

#### 1. "Invalid PUP file"

**Cause**: File format is incorrect or corrupted

**Solution**:
- Recreate PUP file
- Check if file is complete
- Verify file header is "PUP V1.0"

#### 2. "Decryption failed"

**Cause**: Encryption password is incorrect

**Solution**:
- Confirm password used when creating
- Check if password contains special characters
- Recreate PUP file

#### 3. "File too large"

**Cause**: Contains many resource files

**Solution**:
- Compress images and media files
- Remove unused resources
- Use CDN to load third-party libraries

#### 4. "Slow loading"

**Cause**: Large file size or slow disk read speed

**Solution**:
- Optimize file size
- Use SSD
- Preload frequently used resources

## Related Resources

- [Command Line Parameters](./cli-parameters.html) - Complete command-line options
- [Project Structure](./project-structure.html) - Project directory organization
- [Best Practices](./best-practices.html) - Development recommendations
- [Security Mechanisms](./security.html) - Signature verification and security features (V1.2)

## Next Steps

After understanding PUP format, it is recommended to:

1. Try creating your first PUP file
2. Learn [Command Line Parameters](./cli-parameters.html) for more options
3. Reference [Best Practices](./best-practices.html) to optimize your project
4. Learn [Security Mechanisms](./security.html) to implement data signing and integrity protection