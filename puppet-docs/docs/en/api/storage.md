---
title: Persistent Storage API
permalink: /en/api/storage.html
createTime: 2026/03/29 00:00:00
---

# Persistent Storage API

The Persistent Storage API provides SQLite-based key-value pair storage functionality for saving and retrieving data in Puppet applications.

## Overview

The `puppet.storage` namespace provides the following features:

- Key-value pair storage (similar to localStorage)
- Multi-database support
- Data persistence (retained across application restarts)
- Thread-safe operations
- Automatic transaction management
- **Data signature verification (prevents data tampering)**

## Signature Mechanism

Puppet Storage API provides a self-signed certificate-based data signature verification mechanism to prevent database tampering. This mechanism references the Android APK signature design and uses RSA 2048-bit keys and SHA256 signature algorithm.

### Signature Principle

1. **Self-signed Certificate**: Application uses RSA key pair to generate self-signed X.509 certificate
2. **Database Signature**: Uses private key to sign database content when creating database
3. **Signature Verification**: Uses certificate public key to verify signature validity when opening database

### Certificate Information

Each signature contains the following information:

- **Application ID (AppID)**: Certificate's Common Name (CN), used to identify application
- **Certificate Fingerprint**: SHA256 hash value, used to uniquely identify certificate
- **Signature Algorithm**: SHA256withRSA
- **Signature Time**: Date and time when database was signed

### Signature Verification Flow

```
┌─────────────────────────────────────────────┐
│  1. Open database                            │
│  2. Read signature information in database   │
│  3. Extract application certificate from PUP │
│  4. Verify certificate validity and self-sign│
│  5. Use certificate public key to verify     │
│     database signature                       │
│  6. Verification passed → Allow access       │
│  7. Verification failed → Warning but allow  │
│     access (backward compatibility)          │
└─────────────────────────────────────────────┘
```

### Certificate and Private Key in PUP File

When creating a PUP file, you can embed the certificate and private key into the file:

```bash
puppet.exe --create-pup -i myapp -o myapp.pup \
  --certificate app.crt \
  --private-key app.key
```

PUP file structure (V1.1):

```
┌──────────────────────────────────────────────┐
│  PUP V1.1                                     │
├──────────────────────────────────────────────┤
│  Script length (4 bytes)                     │
├──────────────────────────────────────────────┤
│  Script content (variable length)            │
├──────────────────────────────────────────────┤
│  Certificate length (4 bytes)                │
├──────────────────────────────────────────────┤
│  Certificate data (variable length, PEM)     │
├──────────────────────────────────────────────┤
│  Private key length (4 bytes)                │
├──────────────────────────────────────────────┤
│  Private key data (variable length, encrypted)│
├──────────────────────────────────────────────┤
│  AES encryption password (32 bytes)          │
├──────────────────────────────────────────────┤
│  ZIP data (variable length)                  │
└──────────────────────────────────────────────┘
```

### Generating Signature Keys

Use command line tool to generate signing key pair:

```bash
# Interactive generation
puppet.exe --generate-signing-key --interactive

# Specify output files
puppet.exe --generate-signing-key --out-cert app.crt --out-key app.key
```

Interactive generation will prompt for the following information:

```
Please enter certificate information:

Application ID [MyApp]: MyApp
Organization Name [MyCompany]: MyCompany
Organizational Unit [Development]: Development
Country [CN]: CN
Province []: Beijing
City []: Beijing
Email []: admin@example.com
Validity period (years) [25]: 25
Key length [2048]: 2048
```

### Signing Database

Use command line tool to sign existing database:

```bash
puppet.exe --sign-database default.db \
  --certificate app.crt \
  --private-key app.key
```

This will create a `.sig` signature file next to the database.

### Verifying Signature

Use command line tool to verify database signature:

```bash
puppet.exe --verify-database default.db --certificate app.crt
```

### Automatic Signing and Verification

When running Puppet with a PUP file containing certificate and private key:

1. **Create new database**: Automatically uses embedded certificate and private key to sign database
2. **Open existing database**: Automatically verifies signature validity, logs warning if failed

### Signature Metadata Table

Signature information is stored in the `__puppet_metadata__` table in the database:

```sql
CREATE TABLE __puppet_metadata__ (
    id INTEGER PRIMARY KEY,
    app_id TEXT NOT NULL,           -- Application ID (certificate CN)
    app_fingerprint TEXT NOT NULL,  -- Certificate fingerprint (SHA256)
    signature BLOB NOT NULL,        -- Digital signature (binary)
    signature_algorithm TEXT,      -- Signature algorithm (SHA256withRSA)
    created_at INTEGER NOT NULL,    -- Creation time
    cert_info TEXT,                 -- Certificate information (JSON)
    version TEXT NOT NULL           -- Version number
);
```

### Backward Compatibility

The signature mechanism is backward compatible:

- **Unsigned databases**: Can be accessed normally, but will log a warning
- **Verification failure**: Still allows access, but will log warning message
- **Optional feature**: Signature is optional, not mandatory

### Security

The signature mechanism provides the following security guarantees:

1. **Data integrity**: Prevents database from being tampered
2. **Identity verification**: Verifies the creator of the database
3. **Tamper-proof**: Any modification to the database will cause signature verification to fail

### Notes

1. **Private key protection**: Private key is encrypted with AES-256-GCM in PUP file
2. **Certificate validity**: Recommended to set a long validity period (e.g., 25 years)
3. **Key backup**: Keep certificate and private key files safe, cannot be recovered once lost
4. **Irreversible signature**: Once signed, cannot modify signature without breaking verification

## Why Use Storage API?

### vs WebView2 localStorage

| Feature | WebView2 localStorage | puppet.storage |
|---------|---------------------|----------------|
| Data isolation | Needs different UserDataFolder | Naturally isolated |
| Resource consumption | Creates separate browser process | Lightweight SQLite |
| Data format | Only supports strings | Supports strings (JSON recommended) |
| Multi-app isolation | Needs multiple UDF | Automatically isolated |
| Cross-process access | Not supported | Supported (via file) |

### vs Modifying puppet.ini

| Feature | puppet.ini | puppet.storage |
|---------|-----------|----------------|
| Purpose | Framework configuration | Application data |
| Format | INI text | SQLite database |
| Modification method | Requires popup confirmation | Direct modification |
| Data structure | Flat key-value pairs | Multi-database support |
| Transaction support | No | Supported |
| Query capability | No | Supported |

## Database Concepts

### Database

Storage API supports multiple independent databases, each corresponding to a SQLite file:

- **Default database**: Named `default`, used for general storage
- **Custom databases**: Can create any number of databases
- **Database isolation**: Data between different databases is completely isolated

### Storage Location

Database files are stored in user's application data directory:

```
%APPDATA%\puppet\storage\
├── default.db      # Default database
├── app1.db         # Application 1's database
├── app2.db         # Application 2's database
└── ...
```

## Methods

### setItem()

Sets key-value pair.

```javascript
await puppet.storage.setItem(database: string, key: string, value: string): Promise<void>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)
- `key` (string) - Key name
- `value` (string) - Value (JSON string recommended)

**Example**:

```javascript
// Store simple string
await puppet.storage.setItem('default', 'username', 'john');

// Store object (using JSON)
const user = { name: 'john', age: 30, email: 'john@example.com' };
await puppet.storage.setItem('default', 'user', JSON.stringify(user));

// Store array
const recentFiles = ['file1.txt', 'file2.txt', 'file3.txt'];
await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(recentFiles));

// Store to custom database
await puppet.storage.setItem('app1', 'settings', JSON.stringify({ theme: 'dark' }));
```

### getItem()

Gets key-value pair.

```javascript
await puppet.storage.getItem(database: string, key: string): Promise<string>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)
- `key` (string) - Key name

**Return Value**:

Value string, returns empty string if key does not exist.

**Example**:

```javascript
// Get simple string
const username = await puppet.storage.getItem('default', 'username');
console.log(username); // "john"

// Get object
const userJson = await puppet.storage.getItem('default', 'user');
const user = JSON.parse(userJson);
console.log(user.name); // "john"
console.log(user.age);  // 30

// Get array
const recentFilesJson = await puppet.storage.getItem('default', 'recentFiles');
const recentFiles = JSON.parse(recentFilesJson);
console.log(recentFiles); // ["file1.txt", "file2.txt", "file3.txt"]
```

### removeItem()

Deletes key-value pair.

```javascript
await puppet.storage.removeItem(database: string, key: string): Promise<void>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)
- `key` (string) - Key name

**Example**:

```javascript
// Delete single key
await puppet.storage.removeItem('default', 'username');

// Delete key in custom database
await puppet.storage.removeItem('app1', 'settings');
```

### clear()

Clears all data in specified database.

```javascript
await puppet.storage.clear(database: string): Promise<void>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)

**Example**:

```javascript
// Clear default database
await puppet.storage.clear('default');

// Clear custom database
await puppet.storage.clear('app1');
```

::: danger Warning
Clearing database will delete all data, operation cannot be recovered!
:::

### getKeys()

Gets all keys in specified database.

```javascript
await puppet.storage.getKeys(database: string): Promise<string[]>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)

**Return Value**:

Array of key names.

**Example**:

```javascript
// Get all keys
const keys = await puppet.storage.getKeys('default');
console.log(keys); // ["username", "user", "recentFiles"]

// Traverse all data
for (const key of keys) {
    const value = await puppet.storage.getItem('default', key);
    console.log(`${key}: ${value}`);
}
```

### hasItem()

Checks if key exists.

```javascript
await puppet.storage.hasItem(database: string, key: string): Promise<boolean>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)
- `key` (string) - Key name

**Return Value**:

Whether it exists.

**Example**:

```javascript
// Check if key exists
const hasUsername = await puppet.storage.hasItem('default', 'username');
if (hasUsername) {
    console.log('Username already exists');
} else {
    console.log('Username does not exist');
}

// Usage example
async function ensureUserSetup() {
    if (!await puppet.storage.hasItem('default', 'user')) {
        // First run, initialize user data
        await puppet.storage.setItem('default', 'user', JSON.stringify({
            name: 'guest',
            language: 'en-US'
        }));
    }
}
```

### getSize()

Gets database size (in bytes).

```javascript
await puppet.storage.getSize(database: string): Promise<number>
```

**Parameters**:

- `database` (string) - Database name (default is `'default'`)

**Return Value**:

Database file size (in bytes).

**Example**:

```javascript
// Get database size
const size = await puppet.storage.getSize('default');
console.log(`Database size: ${size} bytes`);

// Format for display
function formatBytes(bytes) {
    if (bytes < 1024) return bytes + ' B';
    if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(2) + ' KB';
    return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
}

console.log(`Database size: ${formatBytes(size)}`);
```

### deleteDatabase()

Deletes entire database.

```javascript
await puppet.storage.deleteDatabase(database: string): Promise<void>
```

**Parameters**:

- `database` (string) - Database name

**Example**:

```javascript
// Delete custom database
await puppet.storage.deleteDatabase('app1');

// Delete default database (caution)
await puppet.storage.deleteDatabase('default');
```

::: danger Warning
Deleting database will delete all data, operation cannot be recovered!
:::

### getDatabases()

Gets list of all databases.

```javascript
await puppet.storage.getDatabases(): Promise<string[]>
```

**Return Value**:

Array of database names.

**Example**:

```javascript
// Get all databases
const databases = await puppet.storage.getDatabases();
console.log(databases); // ["default", "app1", "app2"]

// Traverse all databases
for (const db of databases) {
    const size = await puppet.storage.getSize(db);
    const keys = await puppet.storage.getKeys(db);
    console.log(`Database: ${db}, Size: ${size} bytes, Keys: ${keys.length}`);
}
```

### verifyDatabaseSignature()

Verifies database signature (V1.2 format).

```javascript
await puppet.storage.verifyDatabaseSignature(database: string): Promise<SignatureResult>
```

**Parameters**:

- `database` (string) - Database name

**Return Value**:

```typescript
interface SignatureResult {
    isValid: boolean;           // Whether signature is valid
    message: string;            // Verification result message
    certificateThumbprint?: string;  // Certificate fingerprint
    signedAt?: Date;            // Signature time
}
```

**Example**:

```javascript
// Verify database signature
const result = await puppet.storage.verifyDatabaseSignature('default');

if (result.isValid) {
    console.log('✓ Database signature verification passed');
    console.log('Certificate fingerprint:', result.certificateThumbprint);
    console.log('Signature time:', result.signedAt);
} else {
    console.error('✗ Database signature verification failed:', result.message);
    // Can choose to deny access or take other security measures
}
```

**Notes**:

- Only V1.2 format databases support signature verification
- Unsigned databases will return `isValid: false` and corresponding message
- Database can still be accessed when signature verification fails, but it is recommended to log a warning

### signDatabase()

Signs database (V1.2 format).

```javascript
await puppet.storage.signDatabase(database: string): Promise<boolean>
```

**Parameters**:

- `database` (string) - Database name

**Return Value**:

- `boolean` - Whether signing was successful

**Example**:

```javascript
// Sign database
const success = await puppet.storage.signDatabase('default');

if (success) {
    console.log('✓ Database signature successful');
} else {
    console.error('✗ Database signature failed');
}
```

**Notes**:

- Only V1.2 format supports this feature
- Requires PUP file to contain valid certificate and private key
- Database can only be signed once, repeated signing will overwrite previous signature
- Any modification to the database after signing will cause signature verification to fail

## Usage Examples

### Basic Usage

```javascript
// Store user settings
async function saveSettings(settings) {
    await puppet.storage.setItem('default', 'settings', JSON.stringify(settings));
}

// Load user settings
async function loadSettings() {
    const settingsJson = await puppet.storage.getItem('default', 'settings');
    if (settingsJson) {
        return JSON.parse(settingsJson);
    }
    
    // Return default settings
    return {
        theme: 'light',
        language: 'en-US',
        fontSize: 14
    };
}

// Usage example
const settings = await loadSettings();
settings.theme = 'dark';
await saveSettings(settings);
```

### Multi-Application Isolation

```javascript
// Application 1's storage
await puppet.storage.setItem('app1', 'data', JSON.stringify({ value: 'app1 data' }));

// Application 2's storage
await puppet.storage.setItem('app2', 'data', JSON.stringify({ value: 'app2 data' }));

// No interference
const app1Data = JSON.parse(await puppet.storage.getItem('app1', 'data'));
const app2Data = JSON.parse(await puppet.storage.getItem('app2', 'data'));

console.log(app1Data.value); // "app1 data"
console.log(app2Data.value); // "app2 data"
```

### Recent Files List

```javascript
// Add recent file
async function addRecentFile(filePath) {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    const recentFiles = recentJson ? JSON.parse(recentJson) : [];
    
    // Add to beginning
    recentFiles.unshift(filePath);
    
    // Limit quantity
    if (recentFiles.length > 10) {
        recentFiles.pop();
    }
    
    // Remove duplicates
    const uniqueFiles = [...new Set(recentFiles)];
    
    await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(uniqueFiles));
}

// Get recent files
async function getRecentFiles() {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    return recentJson ? JSON.parse(recentJson) : [];
}

// Usage example
await addRecentFile('C:\\Documents\\file1.txt');
await addRecentFile('C:\\Documents\\file2.txt');

const recentFiles = await getRecentFiles();
console.log(recentFiles); // ["C:\\Documents\\file2.txt", "C:\\Documents\\file1.txt"]
```

### User Preferences Management

```javascript
class Preferences {
    constructor() {
        this.database = 'default';
        this.key = 'preferences';
        this.defaults = {
            theme: 'light',
            language: 'en-US',
            fontSize: 14,
            autoSave: true,
            notifications: true
        };
    }
    
    async load() {
        const prefsJson = await puppet.storage.getItem(this.database, this.key);
        if (prefsJson) {
            return { ...this.defaults, ...JSON.parse(prefsJson) };
        }
        return { ...this.defaults };
    }
    
    async save(preferences) {
        await puppet.storage.setItem(this.database, this.key, JSON.stringify(preferences));
    }
    
    async reset() {
        await this.save(this.defaults);
    }
}

// Usage example
const prefs = new Preferences();

// Load preferences
const preferences = await prefs.load();
console.log('Current preferences:', preferences);

// Modify preferences
preferences.theme = 'dark';
preferences.fontSize = 16;
await prefs.save(preferences);

// Reset preferences
await prefs.reset();
```

### Database Management

```javascript
// View database information
async function showDatabaseInfo() {
    const databases = await puppet.storage.getDatabases();
    
    console.log('=== Database Information ===');
    for (const db of databases) {
        const size = await puppet.storage.getSize(db);
        const keys = await puppet.storage.getKeys(db);
        const sizeMB = (size / (1024 * 1024)).toFixed(2);
        
        console.log(`Database: ${db}`);
        console.log(`  Size: ${sizeMB} MB`);
        console.log(`  Keys: ${keys.length}`);
        console.log(`  Key list: ${keys.join(', ')}`);
        console.log();
    }
}

// Clean up large databases
async function cleanupLargeDatabases() {
    const databases = await puppet.storage.getDatabases();
    const maxSize = 10 * 1024 * 1024; // 10 MB
    
    for (const db of databases) {
        const size = await puppet.storage.getSize(db);
        if (size > maxSize) {
            console.log(`Database ${db} too large (${size} bytes), recommend cleanup`);
        }
    }
}

// Usage example
await showDatabaseInfo();
await cleanupLargeDatabases();
```

## Best Practices

### 1. Use JSON Format

Always use JSON format for complex objects:

```javascript
// Good practice
const user = { name: 'john', age: 30 };
await puppet.storage.setItem('default', 'user', JSON.stringify(user));

// Parse when reading
const userJson = await puppet.storage.getItem('default', 'user');
const user = JSON.parse(userJson);

// Avoid
await puppet.storage.setItem('default', 'user_name', 'john');
await puppet.storage.setItem('default', 'user_age', '30');
```

### 2. Use Meaningful Key Names

Use clear, meaningful key names:

```javascript
// Good practice
await puppet.storage.setItem('default', 'user_settings', JSON.stringify(settings));
await puppet.storage.setItem('default', 'app_state', JSON.stringify(state));

// Avoid
await puppet.storage.setItem('default', 'data1', ...);
await puppet.storage.setItem('default', 'temp', ...);
```

### 3. Error Handling

Always perform error handling:

```javascript
async function safeGetItem(key) {
    try {
        const value = await puppet.storage.getItem('default', key);
        return value;
    } catch (error) {
        console.error('Failed to get data:', error);
        return null;
    }
}

async function safeSetItem(key, value) {
    try {
        await puppet.storage.setItem('default', key, JSON.stringify(value));
        return true;
    } catch (error) {
        console.error('Failed to save data:', error);
        return false;
    }
}
```

### 4. Data Validation

Validate data after reading:

```javascript
async function loadUserData() {
    const userJson = await puppet.storage.getItem('default', 'user');
    
    if (!userJson) {
        return null;
    }
    
    try {
        const user = JSON.parse(userJson);
        
        // Validate data structure
        if (!user.name || !user.email) {
            console.warn('User data format incorrect');
            return null;
        }
        
        return user;
    } catch (error) {
        console.error('Failed to parse user data:', error);
        return null;
    }
}
```

### 5. Regular Cleanup

Regularly clean up unnecessary data:

```javascript
async function cleanupOldFiles() {
    const recentJson = await puppet.storage.getItem('default', 'recentFiles');
    const recentFiles = recentJson ? JSON.parse(recentJson) : [];
    
    // Only keep existing files
    const validFiles = [];
    for (const file of recentFiles) {
        if (await puppet.fs.exists(file)) {
            validFiles.push(file);
        }
    }
    
    await puppet.storage.setItem('default', 'recentFiles', JSON.stringify(validFiles));
}
```

### 6. Use Namespaces

Use prefixes or namespaces to organize data:

```javascript
// Good practice - use prefixes
await puppet.storage.setItem('default', 'user_profile', JSON.stringify(profile));
await puppet.storage.setItem('default', 'user_preferences', JSON.stringify(prefs));
await puppet.storage.setItem('default', 'user_history', JSON.stringify(history));

// Or use objects
const userData = {
    profile: { name: 'john' },
    preferences: { theme: 'dark' },
    history: ['file1', 'file2']
};
await puppet.storage.setItem('default', 'user_data', JSON.stringify(userData));
```

## Performance Considerations

### Database Size

- SQLite database files grow with data
- Recommended to regularly clean up unnecessary data
- For large data, consider using file system

### Batch Operations

Consider transactions for batch operations:

```javascript
// Currently Storage API does not directly support transactions
// But can achieve similar effect through combining operations
async function batchUpdate(updates) {
    const oldData = await puppet.storage.getItem('default', 'data');
    const data = oldData ? JSON.parse(oldData) : {};
    
    // Batch update
    Object.assign(data, updates);
    
    // Save once
    await puppet.storage.setItem('default', 'data', JSON.stringify(data));
}
```

### Query Optimization

For large amounts of data, consider pagination or indexing:

```javascript
// Paginated loading
async function loadPaginatedData(page, pageSize) {
    const allDataJson = await puppet.storage.getItem('default', 'items');
    const allItems = allDataJson ? JSON.parse(allDataJson) : [];
    
    const start = page * pageSize;
    const end = start + pageSize;
    
    return allItems.slice(start, end);
}
```

## Common Questions

### Q: What's the difference between Storage API and localStorage?

A: Storage API uses SQLite database, supports multi-database isolation and cross-application data sharing, while localStorage is browser built-in and shares data among all WebView2 instances on the same site, which can lead to data confusion.

### Q: Why not directly modify puppet.ini?

A: puppet.ini is framework configuration file, modification requires popup confirmation. Storage API is specifically designed for application data storage, operations are more direct and efficient.

### Q: Where is data stored?

A: Data is stored in `%APPDATA%\puppet\storage\` directory, each database corresponds to a `.db` file.

### Q: How to backup data?

A: Copy `%APPDATA%\puppet\storage\` directory to backup all data.

### Q: Is there a limit on stored data size?

A: Theoretically SQLite supports very large databases (up to 140TB), but it is recommended that a single database does not exceed 100MB to ensure performance.

### Q: How to delete all data?

A: Use `deleteDatabase()` method to delete database, or directly delete `%APPDATA%\puppet\storage\` directory.

### Q: Can data be shared between multiple applications?

A: Yes! Multiple applications can use the same database name to share data, or use different database names to isolate data.

## Related Resources

- [SQLite Official Documentation](https://www.sqlite.org/docs.html)
- [System.Data.SQLite](https://system.data.sqlite.org/)
- [Application Control API](application.html) - Other application-related APIs
- [File System API](fs.html) - File operation APIs