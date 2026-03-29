---
title: Puppet Signing Tool
permalink: /en/guide/puppet-sign.html
createTime: 2026/03/29 19:28:00
---

# Puppet Signing Tool

`puppet-sign` is a standalone command-line tool for generating signing key pairs, signing databases, and verifying signatures. This tool is primarily used for V1.2 format PUP files, providing digital signature and integrity verification functionality for applications.

## Feature Overview

`puppet-sign` provides the following features:

- **Generate Signing Key Pairs**: Generate self-signed X.509 certificates and RSA private keys
- **Sign Databases**: Digitally sign SQLite database files
- **Verify Signatures**: Verify database file signature validity

## Installation

`puppet-sign` is distributed with the Puppet framework, located in the Puppet installation directory:

```bash
E:\puppet\puppet-sign\bin\Debug\net9.0\puppet-sign.exe
```

## Quick Start

### 1. Generate Signing Key Pair

Use `--generate-signing-key` command to generate signing key pair:

```bash
# Interactive generation
puppet-sign.exe --generate-signing-key --interactive

# Generate using command-line parameters
puppet-sign.exe --generate-signing-key --alias MyApp --validity 3650
```

This generates two files:
- `app.crt` - Self-signed X.509 certificate
- `app.key` - RSA private key

### 2. Create Signed PUP File

Use generated key pair to create signed PUP file:

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

### 3. Sign Database (Optional)

Sign standalone database file:

```bash
puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key
```

## Command Details

### --generate-signing-key

Generate signing key pair and self-signed certificate.

**Usage**:

```bash
puppet-sign.exe --generate-signing-key [options]
```

**Options**:

| Option | Description | Default |
|--------|-------------|---------|
| `--interactive` | Interactive input of certificate information | - |
| `--alias <name>` | Application identifier (CN) | - |
| `--organization <org>` | Organization name (O) | - |
| `--ou <unit>` | Organizational unit (OU) | - |
| `--country <code>` | Country code (C) | CN |
| `--validity <days>` | Validity period (days) | 9125 (25 years) |
| `--key-size <2048\|4096>` | Key length (bits) | 2048 |
| `--out-cert <file>` | Output certificate file path | app.crt |
| `--out-key <file>` | Output private key file path | app.key |

**Examples**:

```bash
# Interactive generation (recommended for first use)
puppet-sign.exe --generate-signing-key --interactive

# Generate with default parameters
puppet-sign.exe --generate-signing-key --alias MyApp

# Generate with custom parameters
puppet-sign.exe --generate-signing-key \
  --alias MyApp \
  --organization MyCompany \
  --ou Development \
  --country CN \
  --validity 3650 \
  --key-size 4096 \
  --out-cert myapp.crt \
  --out-key myapp.key
```

**Output Example**:

```
=== Generate Signing Key Pair ===

✓ Certificate saved: app.crt
✓ Private key saved: app.key

Certificate Information:
  Application ID (CN): MyApp
  Organization (O): MyCompany
  Organizational Unit (OU): Development
  Country (C): CN
  Validity: 3650 days
  Key Length: 4096 bits
  Certificate Fingerprint: 3F5A8C1D9E2B4F7A6C8D1E3F5A7B9C2D

Generation successful!
```

### --sign-database

Digitally sign database file.

**Usage**:

```bash
puppet-sign.exe --sign-database <database.db> [options]
```

**Options**:

| Option | Description |
|--------|-------------|
| `--certificate <file>` | Certificate file path (required) |
| `--private-key <file>` | Private key file path (required) |

**Examples**:

```bash
# Sign database
puppet-sign.exe --sign-database default.db --certificate app.crt --private-key app.key

# Sign database at specified path
puppet-sign.exe --sign-database "C:\MyApp\data.db" --certificate "C:\certs\app.crt" --private-key "C:\keys\app.key"
```

**Output Example**:

```
=== Sign Database ===

Database size: 8192 bytes
✓ Database signed
  Signature size: 256 bytes
  Signature algorithm: SHA256withRSA

Signing successful!
```

**Notes**:

- Signing modifies the database file by adding signature metadata
- Any modification to the database after signing will cause signature verification failure
- Recommended to sign after database content is finalized
- Certificate and private key must be a matching key pair

### --verify-database

Verify database file signature validity.

**Usage**:

```bash
puppet-sign.exe --verify-database <database.db> [options]
```

**Options**:

| Option | Description |
|--------|-------------|
| `--certificate <file>` | Certificate file path (required) |

**Examples**:

```bash
# Verify database signature
puppet-sign.exe --verify-database default.db --certificate app.crt

# Verify database at specified path
puppet-sign.exe --verify-database "C:\MyApp\data.db" --certificate "C:\certs\app.crt"
```

**Output Example**:

```
=== Verify Database Signature ===

Database size: 8192 bytes

Verification Result:
  Certificate Fingerprint: 3F5A8C1D9E2B4F7A6C8D1E3F5A7B9C2D
  Application ID: MyApp

✓ Verification successful!
```

## Certificate Information

### Certificate Format

Certificates generated by `puppet-sign` are standard X.509 self-signed certificates containing the following information:

- **Common Name (CN)**: Application identifier
- **Organization (O)**: Organization name
- **Organizational Unit (OU)**: Organizational unit
- **Country (C)**: Country code
- **Validity Period**: Certificate validity period
- **Public Key**: RSA public key
- **Fingerprint**: SHA-256 certificate fingerprint

### Key Pair

- **Private Key Format**: PKCS#8 PEM format
- **Public Key Algorithm**: RSA
- **Supported Key Lengths**: 2048 or 4096 bits
- **Signature Algorithm**: SHA256withRSA

## Security Best Practices

### 1. Protect Private Key

Private key files (`.key`) contain sensitive information and should:

- Be protected with strong passwords (private keys in PUP files will be encrypted)
- Not be committed to version control systems
- Be stored securely with restricted access permissions
- Be rotated regularly

### 2. Certificate Management

- Use different certificates for different applications
- Record certificate fingerprints and application identifiers
- Set reasonable certificate validity periods
- Update certificates before expiration

### 3. Signing Workflow

- Sign PUP files before release
- Verify signatures when application starts
- Refuse to run unsigned or signature-verification-failed applications
- Sign sensitive databases

### 4. Key Rotation

```bash
# 1. Generate new key pair
puppet-sign.exe --generate-signing-key --alias MyApp-v2 --out-cert app-v2.crt --out-key app-v2.key

# 2. Use new key pair to create signed PUP file
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -v 1.2 --certificate "app-v2.crt" --private-key "app-v2.key" --private-key-password "NewPassword"

# 3. Securely backup or delete old keys
```

## Workflow Examples

### Complete Signing Workflow

```bash
# 1. Develop application
cd C:\MyProject
# ... develop code ...

# 2. Generate signing key pair
cd E:\puppet\puppet-sign\bin\Debug\net9.0
puppet-sign.exe --generate-signing-key --interactive

# 3. Create signed PUP file
puppet.exe --create-pup \
  -i "C:\MyProject\dist" \
  -o "C:\Releases\MyApp-v1.0.0.pup" \
  -v 1.2 \
  --certificate "app.crt" \
  --private-key "app.key" \
  --private-key-password "MySecurePassword"

# 4. Verify PUP file (optional)
# In application, check signature status
# See Application API's getAppInfo() method
```

### Database Signing Workflow

```bash
# 1. Create and initialize database
# ... create database and table structure ...

# 2. Populate initial data
# ... insert initial data ...

# 3. Sign database
puppet-sign.exe --sign-database data.db --certificate app.crt --private-key app.key

# 4. Verify signature (optional)
puppet-sign.exe --verify-database data.db --certificate app.crt

# 5. Package signed database into PUP file
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

## Common Questions

### Q: Why use puppet-sign?

A: `puppet-sign` provides the following benefits:

- **Application Integrity**: Ensures application files have not been tampered with
- **Source Verification**: Verifies application publisher
- **Data Security**: Protects databases from malicious modification
- **User Trust**: Increases user trust in the application

### Q: Can I use third-party certificates?

A: Yes. `puppet-sign` supports using standard X.509 certificates. If you have a CA-signed certificate, you can use it directly:

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --certificate "my-cert.crt" --private-key "my-key.key" --private-key-password "MyPassword"
```

### Q: Does signing affect performance?

A: Signature verification has minimal performance impact:

- Signature verification when application starts adds approximately 10-50ms
- Signature verification when accessing database has almost no performance impact
- Signature verification is only performed once, not repeatedly

### Q: How do I check signature status in my application?

A: Use Application API:

```javascript
// Check application signature status
const appInfo = await puppet.application.getAppInfo();

if (appInfo.hasSignature) {
    console.log('Application is signed');
    console.log('Certificate fingerprint:', appInfo.certificateThumbprint);
} else {
    console.warn('Application is not signed');
}
```

### Q: Can I modify a signed PUP file?

A: No. Once signed, the PUP file cannot be modified or signature verification will fail. If modification is needed, you must:

1. Rebuild using original source code
2. Recreate PUP file
3. Re-sign

### Q: What should I do when certificate expires?

A: When certificate expires:

1. Generate new key pair
2. Use new key to recreate and sign PUP file
3. Release updated version
4. Users upgrade to new version

### Q: How do I verify if a signature is valid?

A: Use Storage API in your application:

```javascript
// Verify database signature
const result = await puppet.storage.verifyDatabaseSignature('default');

if (result.isValid) {
    console.log('Database signature verification passed');
    console.log('Certificate fingerprint:', result.certificateThumbprint);
} else {
    console.error('Database signature verification failed:', result.message);
}
```

## Technical Details

### Signature Algorithm

- **Hash Algorithm**: SHA-256
- **Signature Algorithm**: SHA256withRSA
- **Key Length**: 2048 or 4096 bits
- **Certificate Format**: X.509 self-signed certificate

### Database Signing Process

1. Read all bytes of database file
2. Calculate SHA-256 hash of database content
3. Sign hash value with RSA private key
4. Store signature information in database's `__puppet_metadata__` table

### Signature Verification Process

1. Read signature information from database's `__puppet_metadata__` table
2. Read all bytes of database file
3. Calculate SHA-256 hash of database content
4. Verify signature using certificate public key
5. Check certificate validity and self-signed status

## Related Resources

- [PUP File Format](./pup-format.html) - Learn about PUP file signing mechanism
- [Security Mechanisms](./security.html) - Puppet framework security features
- [Application API](../api/application.html) - getAppInfo() method
- [Storage API](../api/storage.html) - verifyDatabaseSignature() method
- [Command Line Parameters](./cli-parameters.html) - Parameters for creating signed PUP files

## Get Help

If you encounter issues while using `puppet-sign`:

```bash
# View help information
puppet-sign.exe --help
```

Or refer to relevant sections of this documentation.