---
title: Command Line Parameters
permalink: /en/guide/cli-parameters.html
createTime: 2026/03/28 14:57:15
---

# Command Line Parameters

The Puppet Framework provides a flexible command-line interface supporting multiple running modes and working methods. This chapter details all available command-line parameters and their usage.

## Overview

Puppet supports three main running modes:

1. **GUI Mode**: Launch the graphical interface application
2. **Create PUP Mode**: Create PUP package files
3. **Load PUP Mode**: Load and run PUP files
4. **Bare Folder Mode**: Load applications from folders (for development)

## Basic Syntax

```bash
puppet.exe [options] [parameters]
```

## Running Modes

### 1. GUI Mode

Launch Puppet's GUI application, usually used for debugging or manual operations.

```bash
puppet.exe
```

**Features**:

- Display main window
- Can select to load PUP files or folders through the interface
- Suitable for quick testing and debugging

**Configuration**:

GUI mode reads default settings from the `puppet.ini` configuration file:

```ini
[file]
file=app.pup
```

### 2. Create PUP Mode

Package a web application into a PUP file.

```bash
puppet.exe --create-pup -i <input_folder> -o <output_file.pup> [-p <password>]
```

**Parameter Description**:

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--create-pup` | Yes | Specify create PUP file mode |
| `-i` or `--input` | Yes | Source folder path |
| `-o` or `--output` | Yes | Output PUP file path |
| `-p` or `--password` | No | ZIP password (optional) |
| `-v` or `--version` | No | PUP version (1.0, 1.1, or 1.2, default 1.0) |
| `--script` | No | Startup script file (V1.1/V1.2 version support) |
| `--certificate` | No | Certificate file path (V1.2 version required) |
| `--private-key` | No | Private key file path (V1.2 version required) |
| `--private-key-password` | No | Private key encryption password (V1.2 version required) |

**Examples**:

```bash
# Basic usage (V1.0)
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"

# With password (V1.0)
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword"

# V1.1 format (with startup script)
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.1 --script "C:\script.txt"

# V1.2 format (with signature support)
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"

# Using long parameter names
puppet.exe --create-pup --input "C:\MyApp" --output "C:\MyApp.pup" --password "MySecretPassword" --version 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"
```

::: tip Tip
- If no password is specified, the system will automatically generate a random password
- V1.1 and V1.2 versions require specifying the `-v` parameter
- V1.2 version must provide certificate, private key, and private key password parameters
- Use the puppet-sign tool to generate signing key pairs: `puppet-sign.exe --generate-signing-key --alias MyApp`
:::

### 3. Load PUP Mode

Load and run a PUP file.

```bash
puppet.exe --load-pup <file.pup>
```

**Parameter Description**:

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--load-pup` | Yes | Specify load PUP file mode |
| `<file.pup>` | Yes | PUP file path |

**Examples**:

```bash
# Basic usage
puppet.exe --load-pup "C:\MyApp.pup"

# Using relative path
puppet.exe --load-pup "app.pup"

# Using environment variable
puppet.exe --load-pup "%APPDATA%\MyApp\app.pup"
```

### 4. Bare Folder Mode

Load applications directly from folders (without packaging as PUP files).

```bash
puppet.exe --nake-load <folder_path>
```

**Parameter Description**:

| Parameter | Required | Description |
|-----------|----------|-------------|
| `--nake-load` | Yes | Specify bare folder mode |
| `<folder_path>` | Yes | Web application folder path |

**Examples**:

```bash
# Basic usage
puppet.exe --nake-load "C:\MyApp"

# Using relative path
puppet.exe --nake-load ".\dist"

# For development (supports hot reload)
puppet.exe --nake-load "C:\MyProject"
```

::: tip Development Suggestion
Bare folder mode supports hot reload. After modifying files, simply refresh to see changes, making it very suitable for development and debugging.
:::

## Parameter Details

### Input Parameter (-i, --input)

Specify the source folder path.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"
```

**Notes**:

- Path must exist
- Path can contain spaces (use quotes)
- Supports absolute and relative paths
- Supports environment variables

### Output Parameter (-o, --output)

Specify the output file path.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"
```

**Notes**:

- Output directory must exist
- File extension must be `.pup`
- If file exists, it will be overwritten
- Path can contain spaces (use quotes)

### Password Parameter (-p, --password)

Specify the encryption password for the ZIP file.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MyPassword"
```

**Notes**:

- Password is case-sensitive
- Supports special characters
- If not specified, system will automatically generate a random password
- Password not required when loading PUP file (password is encrypted and stored in file)

### Version Parameter (-v, --version)

Specify the PUP file format version.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2
```

**Notes**:

- Supported versions: 1.0, 1.1, 1.2
- Default version: 1.0
- Feature support differences by version:
  - V1.0: Basic features, supports encryption
  - V1.1: Supports startup scripts
  - V1.2: Supports digital signatures and certificate verification

### Startup Script Parameter (--script)

Specify the startup script file for V1.1/V1.2 versions.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.1 --script "C:\script.txt"
```

**Notes**:

- Only V1.1 and V1.2 versions support this
- Script file must exist
- Script will be automatically executed when application starts
- Supports JavaScript and SQL statements

### Certificate Parameter (--certificate)

Specify the digital certificate file path for V1.2 version.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt"
```

**Notes**:

- Only V1.2 version supports this
- Must be used with `--private-key` and `--private-key-password`
- Certificate format: X.509 self-signed certificate
- Recommended to use puppet-sign tool to generate key pairs
- Certificate is used to verify PUP file integrity and source

### Private Key Parameter (--private-key)

Specify the private key file path for V1.2 version.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --private-key "C:\app.key"
```

**Notes**:

- Only V1.2 version supports this
- Must be used with `--certificate` and `--private-key-password`
- Private key format: PKCS#8 format, encrypted using AES-256-GCM
- Private key is used to generate digital signatures
- Private key will be securely stored in the PUP file

### Private Key Password Parameter (--private-key-password)

Specify the decryption password for the V1.2 version private key.

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --private-key-password "MyKeyPassword"
```

**Notes**:

- Only V1.2 version supports this
- Used to decrypt the private key file
- Password is case-sensitive
- Supports special characters
- Private key password is not stored in the PUP file

## Configuration File

In addition to command-line parameters, Puppet supports setting defaults through configuration files.

### puppet.ini

Configuration file located in the same directory as Puppet.exe:

```ini
[file]
; Default PUP file path to load
file=app.pup

[server]
; Server port (default auto-select)
port=7738

[security]
; Whether to enable strict mode
strict=true
```

**Configuration Item Description**:

| Configuration Item | Description | Default Value |
|-------------------|-------------|---------------|
| `file` | Default PUP file to load | None |
| `port` | HTTP server port | 7738 (auto-select) |
| `strict` | Strict mode | true |

**Usage**:

Settings in the configuration file are used in GUI mode:

```bash
# Use settings from configuration file
puppet.exe
```

## Command Line Combinations

### Common Combinations

#### Development Workflow

```bash
# 1. Use bare folder mode during development
puppet.exe --nake-load "C:\MyProject"

# 2. Test packaging (V1.0)
puppet.exe --create-pup -i "C:\MyProject\dist" -o "C:\MyProject\app.pup"

# 3. Test packaging (V1.1 - with startup script)
puppet.exe --create-pup -i "C:\MyProject\dist" -o "C:\MyProject\app.pup" -v 1.1 --script "C:\script.txt"

# 4. Test packaging (V1.2 - with signature)
puppet.exe --create-pup -i "C:\MyProject\dist" -o "C:\MyProject\app.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyKeyPassword"

# 5. Test PUP file
puppet.exe --load-pup "C:\MyProject\app.pup"
```

#### Using puppet-sign to Generate Key Pairs

```bash
# 1. Generate signing key pairs
puppet-sign.exe --generate-signing-key --alias MyApp --key-size 2048

# 2. This will generate app.crt and app.key files in current directory

# 3. Create signed PUP file using generated key pairs
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

#### Batch Packaging

```bash
# Windows batch script
@echo off
set SOURCE=C:\Projects
set OUTPUT=C:\Releases

puppet.exe --create-pup -i "%SOURCE%\App1" -o "%OUTPUT%\App1.pup"
puppet.exe --create-pup -i "%SOURCE%\App2" -o "%OUTPUT%\App2.pup"
puppet.exe --create-pup -i "%SOURCE%\App3" -o "%OUTPUT%\App3.pup"

echo Packaging complete!
pause
```

#### Automated Build

```bash
# PowerShell script
$source = "C:\MyProject\dist"
$output = "C:\Releases\MyApp_$((Get-Date).ToString('yyyyMMdd')).pup"

& "puppet.exe" --create-pup -i $source -o $output -p "MySecretPassword"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful: $output"
} else {
    Write-Host "Build failed"
    exit 1
}
```

## Error Handling

### Common Errors

#### 1. "Input folder does not exist"

```bash
puppet.exe --create-pup -i "C:\NonExistent" -o "output.pup"
```

**Cause**: Specified input folder does not exist

**Solution**: Check if path is correct

#### 2. "Output directory does not exist"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\NonExistent\output.pup"
```

**Cause**: Output directory does not exist

**Solution**: Create output directory first

```bash
mkdir C:\NonExistent
puppet.exe --create-pup -i "C:\MyApp" -o "C:\NonExistent\output.pup"
```

#### 3. "Invalid PUP file"

```bash
puppet.exe --load-pup "invalid.pup"
```

**Cause**: File format is incorrect or corrupted

**Solution**: Recreate PUP file

#### 4. "Port already in use"

**Cause**: Specified or auto-selected port is already occupied by another program

**Solution**: System will automatically try next port, or manually specify another port

#### 5. "Certificate file does not exist"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\missing.crt"
```

**Cause**: Specified certificate file does not exist

**Solution**:
- Check if certificate file path is correct
- Use puppet-sign to generate key pairs: `puppet-sign.exe --generate-signing-key --alias MyApp`

#### 6. "Private key file does not exist"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --private-key "C:\missing.key"
```

**Cause**: Specified private key file does not exist

**Solution**:
- Check if private key file path is correct
- Ensure certificate and private key are the same key pair

#### 7. "Incorrect private key password"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "WrongPassword"
```

**Cause**: Provided private key password is incorrect

**Solution**: Use the correct password set when generating key pairs

#### 8. "Invalid version parameter"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.3
```

**Cause**: Specified version is not supported

**Solution**: Use supported versions (1.0, 1.1, or 1.2)

#### 9. "V1.2 version missing required parameters"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2
```

**Cause**: V1.2 version must provide certificate, private key, and private key password parameters

**Solution**:
```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "C:\app.crt" --private-key "C:\app.key" --private-key-password "MyPassword"
```

## Environment Variables

Puppet supports using environment variables to specify paths:

```bash
# Use APPDATA environment variable
puppet.exe --load-pup "%APPDATA%\MyApp\app.pup"

# Use custom environment variable
set MY_APP_PATH=C:\MyApp
puppet.exe --load-pup "%MY_APP_PATH%\app.pup"
```

## Path Specifications

### Windows Paths

```bash
# Absolute path
puppet.exe --load-pup "C:\MyApp\app.pup"

# Relative path
puppet.exe --load-pup ".\app.pup"
puppet.exe --load-pup "app.pup"

# Paths with spaces (use quotes)
puppet.exe --load-pup "C:\My Folder\app.pup"
```

### Network Paths

```bash
# UNC path
puppet.exe --load-pup "\\server\share\app.pup"

# Mapped drive
puppet.exe --load-pup "Z:\app.pup"
```

### Special Characters

```bash
# Path with special characters (use quotes)
puppet.exe --create-pup -i "C:\My App (2024)" -o "C:\Output\app.pup"
```

## Return Codes

The Puppet command-line tool returns the following exit codes:

| Return Code | Description |
|-------------|-------------|
| 0 | Success |
| 1 | General error |
| 2 | Parameter error |
| 3 | File not found |
| 4 | Permission error |
| 5 | Format error |

**Return Code Description**:

Puppet's `Main` method returns `void`, so the program itself does not directly return an exit code. When the program is executed in Windows, the return value (if any) is stored in environment variables:

- **Batch files**: Can use `%ERRORLEVEL%` to check the exit code of the previous command
- **PowerShell**: Can use `$LastExitCode` to check the exit code of the previous command

**Note**: For Windows Forms GUI applications, `void` is usually returned because they mainly interact through the user interface. If you need to check operation status in batch scripts, it's recommended to use the puppet-sign tool, which is a console application that can correctly return exit codes.

**Example**:

```bash
# Use puppet-sign tool (console application)
puppet-sign.exe --generate-signing-key --alias MyApp

if %ERRORLEVEL% EQU 0 (
    echo Key generation successful
) else (
    echo Key generation failed, error code: %ERRORLEVEL%
)
```

**PowerShell Example**:

```powershell
# Use puppet-sign tool
puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key

if ($LastExitCode -eq 0) {
    Write-Host "Signing successful"
} else {
    Write-Host "Signing failed, error code: $LastExitCode"
}
```

## Best Practices

### 1. Path Handling

- Use quotes to wrap paths containing spaces
- Prefer absolute paths
- Verify path existence before use

### 2. Password Management

- Do not expose passwords in command history
- Use environment variables or configuration files to store passwords
- Change passwords regularly

### 3. Automation Scripts

- Check return codes to determine if operations were successful
- Add appropriate error handling
- Log operations

### 4. Development Process

- Use bare folder mode during development
- Create PUP files for testing
- Verify PUP files before release

## Example Scripts

### Complete Build Script (PowerShell)

```powershell
# build.ps1
param(
    [string]$Source = ".\dist",
    [string]$Output = ".\releases",
    [string]$Password = $env:PUPPET_PASSWORD
)

# Create output directory
if (-not (Test-Path $Output)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate filename
$version = (Get-Date).ToString("yyyyMMddHHmmss")
$outputFile = Join-Path $Output "app_$version.pup"

# Build PUP file
Write-Host "Building PUP file..." -ForegroundColor Cyan
& "puppet.exe" --create-pup -i $Source -o $outputFile -p $Password

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful: $outputFile" -ForegroundColor Green
    
    # Display file information
    $fileInfo = Get-Item $outputFile
    Write-Host "File size: $($fileInfo.Length / 1MB) MB"
} else {
    Write-Host "Build failed" -ForegroundColor Red
    exit 1
}
```

### Quick Launch Script (Batch)

```batch
@echo off
:: start.bat

:: Set paths
set PUPPET_EXE=C:\Puppet\puppet.exe
set APP_PATH=C:\MyApp

:: Check if files exist
if not exist "%PUPPET_EXE%" (
    echo Puppet.exe not found
    pause
    exit /b 1
)

if not exist "%APP_PATH%" (
    echo Application path does not exist
    pause
    exit /b 1
)

:: Launch application
echo Starting Puppet application...
"%PUPPET_EXE%" --nake-load "%APP_PATH%"

pause
```

## Related Resources

- [PUP File Format](./pup-format.md) - PUP file structure description
- [Project Structure](./project-structure.md) - Project directory organization
- [Getting Started](./getting-started.md) - Quick start guide

## Next Steps

After understanding command-line parameters, it is recommended to:

1. Try different running modes
2. Create automated build scripts
3. Reference [Best Practices](./best-practices.md) to optimize your workflow