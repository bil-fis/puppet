---
title: Project Structure
permalink: /en/guide/project-structure.html
createTime: 2026/03/28 14:54:32
---

# Project Structure

This chapter introduces the standard directory structure of a Puppet project and the purpose of each file. Understanding the project structure helps you better organize and manage Puppet applications.

## Standard Project Structure

A typical Puppet project has the following structure:

```
MyPuppetProject/
├── index.html              # Main page (entry file)
├── css/                    # Styles directory
│   └── style.css          # Main stylesheet
├── js/                     # JavaScript files directory
│   └── app.js             # Main application script
├── assets/                 # Assets directory
│   ├── images/            # Image resources
│   ├── fonts/             # Font files
│   └── icons/             # Icon files
├── lib/                    # Third-party libraries directory
│   └── vendor.js          # Third-party library files
└── puppet.json             # Application configuration file (optional)
```

## File Descriptions

### Required Files

#### index.html

The main entry file of the application, typically containing:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My Puppet App</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <!-- Application content -->
    <div id="app">
        <h1>Welcome</h1>
    </div>

    <!-- Include JavaScript -->
    <script src="js/app.js"></script>
</body>
</html>
```

**Best Practices**:

- Use semantic HTML tags
- Add appropriate meta tags
- Load JavaScript at the bottom of the page for better performance
- Use relative paths to reference resources

### Recommended Directories

#### css/

Store all style files:

```
css/
├── style.css              # Main stylesheet
├── components/            # Component styles
│   ├── button.css        # Button styles
│   └── dialog.css        # Dialog styles
└── themes/               # Theme styles
    └── dark.css          # Dark theme
```

**Naming Recommendations**:

- Use lowercase letters and hyphens (kebab-case)
- Name the main stylesheet `style.css`
- Classify component styles by function

#### js/

Store all JavaScript files:

```
js/
├── app.js                 # Main application script
├── utils/                 # Utility functions
│   ├── helpers.js        # Helper functions
│   └── validators.js     # Validation functions
├── components/            # Component scripts
│   ├── Button.js         # Button component
│   └── Dialog.js         # Dialog component
└── services/              # Service layer
    └── api.js            # API wrappers
```

**Naming Recommendations**:

- Use PascalCase for classes and components
- Use camelCase for functions and variables
- Organize code by functional modules

#### assets/

Store static resource files:

```
assets/
├── images/               # Image resources
│   ├── logo.png         # Logo
│   ├── icons/           # Icons
│   └── backgrounds/     # Background images
├── fonts/               # Font files
│   └── font.ttf         # Font file
└── data/                # Data files
    └── config.json      # Configuration data
```

**Best Practices**:

- Compress images to reduce file size
- Use appropriate image formats (PNG, JPG, SVG)
- Add Web format support for font files

#### lib/

Store third-party library files:

```
lib/
├── vue.min.js           # Vue.js
├── react.min.js         # React
└── axios.min.js         # Axios
```

**Recommendations**:

- Use CDN to include common libraries to reduce project size
- Or use npm install and bundle
- Keep library version information

### Optional Files

#### puppet.json

Application configuration file for storing runtime configuration:

```json
{
  "appName": "My App",
  "version": "1.0.0",
  "settings": {
    "theme": "dark",
    "language": "en-US"
  }
}
```

Usage:

```javascript
// Read configuration
const config = await puppet.Application.getConfig('appName');

// Write configuration
await puppet.Application.setConfig('theme', 'light');
```

#### favicon.ico

Application icon, Puppet will automatically use the website's favicon as the window icon.

**Recommended Sizes**:

- 16x16 (small icon)
- 32x32 (standard icon)
- 48x48 (large icon)
- 256x256 (high resolution)

## Framework Project Structure

The project structure of the Puppet Framework itself:

```
puppet/
├── Program.cs                 # Application entry point
├── Form1.cs                   # Main window
├── Form1.Designer.cs          # Window designer generated
├── Form1.resx                 # Resource file
├── PupServer.cs               # PUP server
├── PupCreator.cs              # PUP creator
├── AesHelper.cs               # Encryption utility (moved to Core/)
├── SecretKey.cs               # Key management (moved to Core/)
├── IniReader.cs               # INI reader (moved to Core/)
├── PortSelector.cs            # Port selector (moved to Core/)
├── PermissionDialog.cs        # Permission dialog (moved to UI/)
├── PupCreator.cs              # PUP file creator (moved to PUP/)
├── PupServer.cs               # PUP server (moved to PUP/)
├── StorageController.cs        # Persistent storage controller
├── puppet.ini                 # Framework configuration
├── puppet.csproj              # Project file
├── Controllers/               # Controllers directory
│   ├── ApplicationController.cs
│   ├── FileSystemController.cs
│   ├── WindowController.cs
│   ├── EventController.cs
│   ├── LogController.cs
│   ├── SystemController.cs
│   └── TrayController.cs
├── Core/                     # Core utility classes
│   ├── AesHelper.cs
│   ├── IniReader.cs
│   ├── PortSelector.cs
│   ├── SecretKey.cs
│   └── Security/            # Signing and security utility classes
│       ├── AppSignatureValidator.cs
│       ├── CertificateUtils.cs
│       ├── CryptoUtils.cs
│       └── SecurityException.cs
├── PUP/                      # PUP file processing
│   ├── PupCreator.cs
│   ├── PupServer.cs
│   └── PupScriptExecutor.cs
├── UI/                       # User interface components
│   └── PermissionDialog.cs
└── test-htmls/                # Test pages
    ├── index.html
    ├── event-test.html
    └── device-test.html
```

**Directory Organization Description**:

- **Controllers/** - Controller classes that handle API requests
- **Core/** - Core utility classes, including encryption, configuration, security, etc.
- **PUP/** - PUP file format processing, supports V1.0, V1.1, V1.2 formats
- **UI/** - User interface components, such as permission dialogs

## Configuration Files

### puppet.ini

Framework configuration file, typically located in the same directory as Puppet.exe:

```ini
[file]
; Path to the PUP file to load
file=app.pup

[server]
; Server port (defaults to auto-select)
port=7738

[security]
; Whether to enable strict mode
strict=true
```

**Usage**:

- Specify the default PUP file to load
- Configure server port
- Set security options

### puppet.csproj

.NET project file defining project configuration:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Web.WebView2" Version="1.0.1264.42" />
    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
    <PackageReference Include="SharpZipLib" Version="1.3.3" />
  </ItemGroup>
</Project>
```

## File Naming Conventions

### HTML Files

- Use lowercase letters and hyphens (kebab-case)
- Name the main page `index.html`
- Name other pages by function

```
index.html           # Main page
about.html           # About page
settings.html        # Settings page
user-profile.html    # User profile page
```

### CSS Files

- Use lowercase letters and hyphens (kebab-case)
- Name the main stylesheet `style.css`
- Name component styles by component

```
style.css            # Main stylesheet
button.css           # Button styles
modal.css            # Modal styles
sidebar.css          # Sidebar styles
```

### JavaScript Files

- Use lowercase letters and hyphens (kebab-case)
- Name the main script `app.js`
- Name component scripts by component

```
app.js               # Main application script
button.js            # Button component
modal.js             # Modal component
utils.js             # Utility functions
```

### Resource Files

- Images: Use lowercase letters and hyphens
- Fonts: Keep original filenames
- Icons: Descriptive naming

```
logo.png             # Logo
background-image.png # Background image
icon-close.png       # Close icon
roboto-regular.ttf   # Font file
```

## Organization Strategies

### Organize by Function

Suitable for large projects, each functional module is independent:

```
project/
├── index.html
├── modules/
│   ├── user/
│   │   ├── user.html
│   │   ├── user.css
│   │   └── user.js
│   └── settings/
│       ├── settings.html
│       ├── settings.css
│       └── settings.js
└── shared/
    ├── css/
    ├── js/
    └── assets/
```

### Organize by Type

Suitable for small to medium projects, files are classified by type:

```
project/
├── index.html
├── pages/              # All pages
│   ├── about.html
│   └── settings.html
├── css/                # All styles
│   └── style.css
├── js/                 # All scripts
│   └── app.js
└── assets/             # All resources
    ├── images/
    └── fonts/
```

### Hybrid Organization

Combine the advantages of both approaches:

```
project/
├── index.html
├── core/               # Core files
│   ├── css/
│   ├── js/
│   └── assets/
├── features/           # Feature modules
│   ├── user/
│   └── admin/
└── shared/             # Shared resources
    ├── components/
    └── utils/
```

## Version Control

### .gitignore

Recommended `.gitignore` configuration:

```gitignore
# Dependencies
node_modules/
lib/vendor.js

# Build output
dist/
build/
*.pup

# IDE
.vs/
.idea/
*.suo
*.user

# System files
.DS_Store
Thumbs.db

# Logs
*.log
```

### File Commit Strategy

- Commit source code (HTML, CSS, JS)
- Commit configuration files (JSON, INI)
- Do not commit build output (.pup files)
- Do not commit dependencies (node_modules)

## Packaging and Distribution

### PUP File Generation

```bash
# Create PUP file
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup"
```

### Distribution Package Structure

Recommended distribution package structure:

```
MyApp/
├── MyApp.pup            # PUP file
├── puppet.exe           # Puppet runtime (optional)
├── README.txt           # Usage instructions
└── LICENSE.txt          # License
```

## Best Practices

### 1. Directory Structure

- Keep structure clear and consistent
- Use meaningful folder names
- Avoid too deep directory levels

### 2. File Naming

- Use consistent naming conventions
- Names should describe file content
- Avoid special characters and spaces

### 3. Code Organization

- Keep related code together
- Use comments to explain complex logic
- Keep file sizes reasonable (recommended < 500 lines)

### 4. Resource Management

- Compress images and media files
- Use CDN to load third-party libraries
- Cache commonly used resources

### 5. Configuration Management

- Separate configuration from code
- Use JSON format to store configuration
- Provide default configuration values

## Related Resources

- [PUP File Format](./pup-format.md) - Learn about PUP packaging format
- [Command Line Parameters](./cli-parameters.md) - Command line tool usage
- [Best Practices](./best-practices.md) - Development recommendations and tips

## Next Steps

After understanding the project structure, we recommend:

1. Create your first project
2. Learn [API Documentation](../api/) to start development
3. Reference [Best Practices](./best-practices.md) to improve code quality