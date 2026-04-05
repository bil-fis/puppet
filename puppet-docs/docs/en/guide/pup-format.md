---
title: PUP File Format
permalink: /en/guide/pup-format.html
createTime: 2026/03/28 14:55:17
---

# PUP File Format

PUP (Puppet Package) is a proprietary application packaging format for the Puppet Framework. It packages an entire web application into a single file with encryption support for easy distribution and deployment.

## Overview

PUP file is a custom packaging format combining ZIP compression and AES encryption with the following features:

- **Single File Distribution**: All resources packaged into one file
- **Password Protection**: Supports AES-256 encryption
- **Fast Loading**: Optimized file structure and loading mechanism
- **Cross-Version Compatibility**: Version identifiers ensure compatibility

## File Structure

### Version Overview

PUP file format supports multiple versions:

- **V1.0**: Basic version, supports ZIP packaging and encryption
- **V1.1**: Enhanced version, supports startup script functionality
- **V1.2**: Signature version, supports certificates and private keys for database signature verification

### V1.0 Binary Structure

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.0 Identifier Header (8 bytes)       │
├─────────────────────────────────────────────────────┤
│           AES Encrypted ZIP Password (32 bytes)      │
├─────────────────────────────────────────────────────┤
│           ZIP Data (variable length)                 │
└─────────────────────────────────────────────────────┘
```

### V1.1 Binary Structure

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.1 Identifier Header (8 bytes)       │
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

### V1.2 Binary Structure (with Signature Support)

```
┌─────────────────────────────────────────────────────┐
│           PUP V1.2 Identifier Header (8 bytes)       │
├─────────────────────────────────────────────────────┤
│           Script Length (4 bytes, int32)             │
├─────────────────────────────────────────────────────┤
│           Startup Script Content (variable length)   │
├─────────────────────────────────────────────────────┤
│           Certificate Length (4 bytes, int32)        │
├─────────────────────────────────────────────────────┤
│           Certificate Data (variable length, DER)    │
├─────────────────────────────────────────────────────┤
│           Encrypted Private Key Length (4 bytes)     │
├─────────────────────────────────────────────────────┤
│           Encrypted Private Key Data (variable)      │
├─────────────────────────────────────────────────────┤
│           AES Encrypted ZIP Password (32 bytes)      │
├─────────────────────────────────────────────────────┤
│           ZIP Data (variable length)                 │
└─────────────────────────────────────────────────────┘
```

**V1.2 Security Features**:

- **Certificate Protection**: Uses self-signed X.509 certificates for signature verification
- **Private Key Encryption**: Private key encrypted using AES-256-GCM, key derived via PBKDF2
- **Database Signature**: Supports signing and verification of SQLite databases
- **Fingerprint Verification**: Ensures certificate hasn't been replaced via certificate fingerprint

### Detailed Description

#### 1. Identifier Header (8 bytes)

Fixed string for identifying file format and version.

- V1.0: `"PUP V1.0"`
- V1.1: `"PUP V1.1"`

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
```

#### 2. Encrypted ZIP Password (32 bytes)

ZIP file decompression password, encrypted using the fixed key `"ILOVEPUPPET"` with AES.

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

Use the `PupCreator` class to create PUP files:

```csharp
using Puppet;

// Create PUP file
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword"  // Optional
);
```

### V1.1 Format (with Startup Script)

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

Use the `PupCreator` class to create V1.1 format PUP files:

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

## Startup Scripts (V1.1)

::: tip Detailed Documentation
For complete usage instructions, syntax reference, and best practices for startup scripts, please refer to the [PUP Startup Script](./pup-script.md) documentation.
:::

### Overview

V1.1 format supports automatic execution of preset scripts when PUP loads, enabling quick initialization of window states.

### Script Syntax

Startup scripts use a simple command syntax, one command per line:

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
- `<x>,<y>`: Specified coordinates, e.g., `100,200`
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

**Example 1: Bottom-right borderless window**
```
set startup_position right-bottom
set borderless true
set window_size 400,300
```

**Example 2: Centered bordered window**
```
set startup_position center
set borderless false
set window_size 1024,768
```

**Example 3: Specified position and size**
```
set startup_position 100,100
set borderless true
set window_size 800,600
```

### Script File Example

Create a script file named `startup.txt`:

```
# Puppet Startup Script
# Set window to bottom-right borderless window
set startup_position right-bottom
set borderless true
set window_size 500,400
```

### Script Execution Timing

- Script executes after PUP file loads
- Executes after WebView2 initialization completes
- Executes before page navigates to application URL
- Script execution errors won't prevent application startup

### Script Limitations

- Each line can only contain one command
- Commands are case-insensitive
- Supports comments starting with `//` or `#`
- Empty lines are ignored

### Code Method (C#)

Use the `PupCreator` class to create PUP files:

```csharp
using Puppet;

// Create PUP file
PupCreator.CreatePup(
    sourceFolder: @"C:\MyApp",
    outputPupFile: @"C:\MyApp.pup",
    password: "MySecretPassword"  // Optional
);
```

### V1.2 Format (with Signature Support)

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

Use the `PupCreator` class to create V1.2 format PUP files:

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

Use the `puppet-sign` tool to generate signing key pairs:

```bash
# Generate signing key pairs
puppet-sign.exe --generate-signing-key --alias MyApp --organization MyOrg --country CN --validity 3650
```

Generated files:
- `app.crt` - Self-signed certificate (contains public key)
- `app.key` - RSA private key (PKCS#8 format)

#### Database Signature

When using V1.2 format PUP files, the database automatically supports signature functionality:

```csharp
// Sign database in code
StorageController storage = new StorageController(form);
storage.SignDatabase("default");
```

After signing, the database stores the following in the `puppet_metadata` table:
- `app_id` - Application identifier (from certificate CN)
- `certificate_fingerprint` - Certificate fingerprint (SHA256)
- `signature_data` - Signature data (signed using private key)
- `created_at` - Signature timestamp

#### Signature Verification

When loading V1.2 format PUP files, the system automatically verifies database signatures:

```csharp
// PupServer automatically verifies signature
var server = new PupServer("myapp.pup", 7738);

// Signature is verified on first database access
storage.SetItem("default", "key", "value");
```

Verification failure outputs warning messages:
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

# Load V1.1 format (startup script executes automatically)
puppet.exe --load-pup "C:\MyAppV1_1.pup"

# Load V1.2 format (automatically loads certificate and private key, supports database signature)
puppet.exe --load-pup "C:\MyAppV1_2.pup"
```

**Automatic Version Recognition**:

PupServer automatically recognizes PUP file versions:

- V1.0: Parses identifier header `"PUP V1.0"`
- V1.1: Parses identifier header `"PUP V1.1"` and executes startup script
- V1.2: Parses identifier header `"PUP V1.2"`, loads certificate and private key, supports database signature

### Configuration File Method

Edit the `puppet.ini` file:

```ini
[file]
file=C:\MyApp.pup
```

Then simply run `puppet.exe`.

### Loading Process

#### V1.0 Loading Process

```
1. Read first 8 bytes of file, verify identifier header "PUP V1.0"
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
1. Read first 8 bytes of file, verify identifier header "PUP V1.1"
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

#### V1.2 Loading Process (with Signature Support)

```
1. Read first 8 bytes of file, verify identifier header "PUP V1.2"
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
9. Derive key using PBKDF2 and decrypt private key (AES-256-GCM)
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
3. Create ZIP file (encrypted using ZIP password)
        ↓
4. Concatenate: identifier header + encrypted ZIP password + ZIP data
        ↓
5. Write to PUP file
```

### Decryption Process

```
1. Read first 8 bytes of file, verify identifier header
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
The encrypted ZIP password uses the fixed key `"ILOVEPUPPET"`, which is a lightweight protection method. If you need stronger security, consider using file system encryption (like BitLocker) or distribution over HTTPS.
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

The PUP server verifies file format when loading:

```csharp
public bool LoadPupFile()
{
    // 1. Read file
    byte[] fileData = File.ReadAllBytes(_pupFilePath);
    
    // 2. Verify identifier header
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

- Total source file size
- ZIP compression rate (typically 30-70%)
- Encryption overhead (approximately 40 bytes)

**Optimization Suggestions**:

- Compress images and media files
- Remove unused resources
- Use CDN to load third-party libraries

### Loading Speed

PUP file loading speed depends on:

- File size
- Disk read speed
- Decryption and decompression speed

**Optimization Suggestions**:

- Keep file size reasonable (recommended < 50MB)
- Use SSD to improve read speed
- Preload frequently used resources

## Version Compatibility

### Identifier Headers

Currently supported versions:

- V1.0: `"PUP V1.0"` - Basic version
- V1.1: `"PUP V1.1"` - Enhanced version (supports startup scripts)
- V1.2: `"PUP V1.2"` - Signature version (supports certificates and database signatures)

```csharp
private static readonly byte[] PUP_HEADER_V1_0 = Encoding.UTF8.GetBytes("PUP V1.0");
private static readonly byte[] PUP_HEADER_V1_1 = Encoding.UTF8.GetBytes("PUP V1.1");
private static readonly byte[] PUP_HEADER_V1_2 = Encoding.UTF8.GetBytes("PUP V1.2");
```

### Version Detection

PupServer automatically detects PUP file versions and uses appropriate parsing logic:

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
        // Use V1.2 parsing logic (supports signatures)
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
- V1.2 files contain certificates and encrypted private keys, supporting database signatures
- PupServer can load and parse all versions of PUP files
- New projects are recommended to use V1.2 format for enterprise-level security protection

### Version Selection Recommendations

- **V1.0**: Suitable for simple applications not requiring startup configuration
- **V1.1**: Suitable for applications requiring preset window states
- **V1.2**: Suitable for applications requiring database signatures and integrity, recommended for production environments

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
| Encryption Protection | Supported | Supported | Supported | Not supported |
| Startup Script | Not supported | Supported | Supported | Not supported |
| Database Signature | Not supported | Not supported | Supported | Not supported |
| Certificate Verification | Not supported | Not supported | Supported | Not supported |
| Window Preset | Code control | Script control | Script control | Code control |
| Development Convenience | Lower | Lower | Medium | High |
| Hot Reload | Not supported | Not supported | Not supported | Supported |
| File Size | Smaller | Slightly larger | Larger | Larger |
| Loading Speed | Slightly slower | Slightly slower | Slightly slower | Fast |
| Security Level | Medium | Medium | High | Low |
| Use Cases | Simple app release | Complex app release | Production release | Development debugging |

## Best Practices

### 1. Creating PUP Files

- Create PUP files before release
- Use meaningful passwords
- Test that PUP files load correctly

### 2. Password Management

- Don't hardcode passwords in code
- Use environment variables or configuration files to store passwords
- Change passwords regularly

### 3. File Optimization

- Compress images and media files
- Remove debug code and comments
- Use production build configurations

### 4. Version Control

- Include version numbers in filenames
- Keep historical versions of PUP files
- Record changes for each version

### 5. Signature Management (V1.2)

- Use puppet-sign tool to generate signing key pairs
- Use strong passwords to protect private keys (at least 16 characters)
- Regularly backup certificate and private key files
- Securely store private key passwords, don't commit to version control
- Regularly check certificate validity, renew in advance
- Backup databases before signing

### 6. Distribution Strategy

- Use HTTPS to distribute PUP files
- Provide file verification (such as MD5, SHA256)
- Include detailed update logs
- For V1.2 format, provide certificate fingerprint for verification

## Troubleshooting

### Common Issues

#### 1. "Invalid PUP file"

**Cause**: Incorrect file format or corrupted file

**Solution**:
- Recreate PUP file
- Check if file is complete
- Verify file header is "PUP V1.0"

#### 2. "Decryption failed"

**Cause**: Incorrect encryption password

**Solution**:
- Confirm password used during creation
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

- [Command Line Parameters](./cli-parameters.md) - Complete command line options
- [Project Structure](./project-structure.md) - Project directory organization
- [Best Practices](./best-practices.md) - Development recommendations
- [Security Mechanisms](./security.md) - Signature verification and security features (V1.2)

## Next Steps

After understanding PUP format, it is recommended to:

1. Try creating your first PUP file
2. Learn [Command Line Parameters](./cli-parameters.md) for more options
3. Reference [Best Practices](./best-practices.md) to optimize your project
4. Learn [Security Mechanisms](./security.md) to implement data signing and integrity protection