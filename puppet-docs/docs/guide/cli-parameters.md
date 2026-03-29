---
title: 命令行参数
permalink: /guide/cli-parameters.html
createTime: 2026/03/28 14:57:15
---

# 命令行参数

Puppet 框架提供灵活的命令行接口，支持多种运行模式和工作方式。本章节详细介绍所有可用的命令行参数及其用法。

## 概述

Puppet 支持三种主要运行模式：

1. **GUI 模式**：启动图形界面应用
2. **创建 PUP 模式**：创建 PUP 打包文件
3. **加载 PUP 模式**：加载并运行 PUP 文件
4. **裸文件夹模式**：从文件夹加载应用（开发时使用）

## 基本语法

```bash
puppet.exe [选项] [参数]
```

## 运行模式

### 1. GUI 模式

启动 Puppet 的图形界面应用，通常用于调试或手动操作。

```bash
puppet.exe
```

**特点**：

- 显示主窗口
- 可以通过界面选择加载 PUP 文件或文件夹
- 适用于快速测试和调试

**配置**：

GUI 模式会读取 `puppet.ini` 配置文件中的默认设置：

```ini
[file]
file=app.pup
```

### 2. 创建 PUP 模式

将 Web 应用打包为 PUP 文件。

```bash
puppet.exe --create-pup -i <输入文件夹> -o <输出文件.pup> [-p <密码>]
```

**参数说明**：

| 参数 | 必需 | 说明 |
|------|------|------|
| `--create-pup` | 是 | 指定创建 PUP 文件模式 |
| `-i` 或 `--input` | 是 | 源文件夹路径 |
| `-o` 或 `--output` | 是 | 输出 PUP 文件路径 |
| `-p` 或 `--password` | 否 | ZIP 密码（可选） |

**示例**：

```bash
# 基本用法
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"

# 使用密码
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MySecretPassword"

# 使用长参数名
puppet.exe --create-pup --input "C:\MyApp" --output "C:\MyApp.pup" --password "MySecretPassword"
```

::: tip 提示
如果未指定密码，系统会自动生成随机密码。
:::

### 3. 加载 PUP 模式

加载并运行 PUP 文件。

```bash
puppet.exe --load-pup <文件.pup>
```

**参数说明**：

| 参数 | 必需 | 说明 |
|------|------|------|
| `--load-pup` | 是 | 指定加载 PUP 文件模式 |
| `<文件.pup>` | 是 | PUP 文件路径 |

**示例**：

```bash
# 基本用法
puppet.exe --load-pup "C:\MyApp.pup"

# 使用相对路径
puppet.exe --load-pup "app.pup"

# 使用环境变量
puppet.exe --load-pup "%APPDATA%\MyApp\app.pup"
```

### 4. 裸文件夹模式

直接从文件夹加载应用（不打包为 PUP 文件）。

```bash
puppet.exe --nake-load <文件夹路径>
```

**参数说明**：

| 参数 | 必需 | 说明 |
|------|------|------|
| `--nake-load` | 是 | 指定裸文件夹模式 |
| `<文件夹路径>` | 是 | Web 应用文件夹路径 |

**示例**：

```bash
# 基本用法
puppet.exe --nake-load "C:\MyApp"

# 使用相对路径
puppet.exe --nake-load ".\dist"

# 开发时使用（支持热重载）
puppet.exe --nake-load "C:\MyProject"
```

::: tip 开发建议
裸文件夹模式支持热重载，修改文件后刷新即可看到变化，非常适合开发调试。
:::

## 参数详解

### 输入参数（-i, --input）

指定源文件夹路径。

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"
```

**注意事项**：

- 路径必须存在
- 路径可以包含空格（使用引号）
- 支持绝对路径和相对路径
- 支持环境变量

### 输出参数（-o, --output）

指定输出文件路径。

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"
```

**注意事项**：

- 输出目录必须存在
- 文件扩展名必须是 `.pup`
- 如果文件已存在，会被覆盖
- 路径可以包含空格（使用引号）

### 密码参数（-p, --password）

指定 ZIP 文件的加密密码。

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup" -p "MyPassword"
```

**注意事项**：

- 密码区分大小写
- 支持特殊字符
- 如果未指定，系统会自动生成随机密码
- 加载 PUP 文件时不需要提供密码（密码已加密存储在文件中）

## 配置文件

除了命令行参数，Puppet 还支持通过配置文件设置默认值。

### puppet.ini

位于 Puppet.exe 同级目录的配置文件：

```ini
[file]
; 默认加载的 PUP 文件路径
file=app.pup

[server]
; 服务器端口（默认自动选择）
port=7738

[security]
; 是否启用严格模式
strict=true
```

**配置项说明**：

| 配置项 | 说明 | 默认值 |
|--------|------|--------|
| `file` | 默认加载的 PUP 文件 | 无 |
| `port` | HTTP 服务器端口 | 7738（自动选择） |
| `strict` | 严格模式 | true |

**使用方式**：

配置文件中的设置会在 GUI 模式下使用：

```bash
# 使用配置文件中的设置
puppet.exe
```

## 命令行组合

### 常见组合

#### 开发工作流

```bash
# 1. 开发时使用裸文件夹模式
puppet.exe --nake-load "C:\MyProject"

# 2. 测试打包
puppet.exe --create-pup -i "C:\MyProject\dist" -o "C:\MyProject\app.pup"

# 3. 测试 PUP 文件
puppet.exe --load-pup "C:\MyProject\app.pup"
```

#### 批量打包

```bash
# Windows 批处理脚本
@echo off
set SOURCE=C:\Projects
set OUTPUT=C:\Releases

puppet.exe --create-pup -i "%SOURCE%\App1" -o "%OUTPUT%\App1.pup"
puppet.exe --create-pup -i "%SOURCE%\App2" -o "%OUTPUT%\App2.pup"
puppet.exe --create-pup -i "%SOURCE%\App3" -o "%OUTPUT%\App3.pup"

echo 打包完成！
pause
```

#### 自动化构建

```bash
# PowerShell 脚本
$source = "C:\MyProject\dist"
$output = "C:\Releases\MyApp_$((Get-Date).ToString('yyyyMMdd')).pup"

& "puppet.exe" --create-pup -i $source -o $output -p "MySecretPassword"

if ($LASTEXITCODE -eq 0) {
    Write-Host "打包成功：$output"
} else {
    Write-Host "打包失败"
    exit 1
}
```

## 错误处理

### 常见错误

#### 1. "输入文件夹不存在"

```bash
puppet.exe --create-pup -i "C:\NonExistent" -o "output.pup"
```

**原因**：指定的输入文件夹不存在

**解决方案**：检查路径是否正确

#### 2. "输出目录不存在"

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\NonExistent\output.pup"
```

**原因**：输出目录不存在

**解决方案**：先创建输出目录

```bash
mkdir C:\NonExistent
puppet.exe --create-pup -i "C:\MyApp" -o "C:\NonExistent\output.pup"
```

#### 3. "无效的 PUP 文件"

```bash
puppet.exe --load-pup "invalid.pup"
```

**原因**：文件格式不正确或已损坏

**解决方案**：重新创建 PUP 文件

#### 4. "端口已被占用"

**原因**：指定或自动选择的端口已被其他程序占用

**解决方案**：系统会自动尝试下一个端口，或手动指定其他端口

## 环境变量

Puppet 支持使用环境变量指定路径：

```bash
# 使用 APPDATA 环境变量
puppet.exe --load-pup "%APPDATA%\MyApp\app.pup"

# 使用自定义环境变量
set MY_APP_PATH=C:\MyApp
puppet.exe --load-pup "%MY_APP_PATH%\app.pup"
```

## 路径规范

### Windows 路径

```bash
# 绝对路径
puppet.exe --load-pup "C:\MyApp\app.pup"

# 相对路径
puppet.exe --load-pup ".\app.pup"
puppet.exe --load-pup "app.pup"

# 包含空格的路径（使用引号）
puppet.exe --load-pup "C:\My Folder\app.pup"
```

### 网络路径

```bash
# UNC 路径
puppet.exe --load-pup "\\server\share\app.pup"

# 映射驱动器
puppet.exe --load-pup "Z:\app.pup"
```

### 特殊字符

```bash
# 路径包含特殊字符（使用引号）
puppet.exe --create-pup -i "C:\My App (2024)" -o "C:\Output\app.pup"
```

## 返回码

Puppet 命令行工具返回以下退出码：

| 返回码 | 说明 |
|--------|------|
| 0 | 成功 |
| 1 | 一般错误 |
| 2 | 参数错误 |
| 3 | 文件不存在 |
| 4 | 权限错误 |
| 5 | 格式错误 |

**示例**：

```bash
puppet.exe --create-pup -i "C:\MyApp" -o "C:\MyApp.pup"

if %ERRORLEVEL% EQU 0 (
    echo 打包成功
) else (
    echo 打包失败，错误码：%ERRORLEVEL%
)
```

## 最佳实践

### 1. 路径处理

- 使用引号包裹包含空格的路径
- 优先使用绝对路径
- 验证路径存在性后再使用

### 2. 密码管理

- 不要在命令历史中暴露密码
- 使用环境变量或配置文件存储密码
- 定期更换密码

### 3. 自动化脚本

- 检查返回码以确定操作是否成功
- 添加适当的错误处理
- 记录操作日志

### 4. 开发流程

- 开发时使用裸文件夹模式
- 测试时创建 PUP 文件
- 发布时验证 PUP 文件

## 示例脚本

### 完整的构建脚本（PowerShell）

```powershell
# build.ps1
param(
    [string]$Source = ".\dist",
    [string]$Output = ".\releases",
    [string]$Password = $env:PUPPET_PASSWORD
)

# 创建输出目录
if (-not (Test-Path $Output)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# 生成文件名
$version = (Get-Date).ToString("yyyyMMddHHmmss")
$outputFile = Join-Path $Output "app_$version.pup"

# 构建 PUP 文件
Write-Host "正在构建 PUP 文件..." -ForegroundColor Cyan
& "puppet.exe" --create-pup -i $Source -o $outputFile -p $Password

if ($LASTEXITCODE -eq 0) {
    Write-Host "构建成功：$outputFile" -ForegroundColor Green
    
    # 显示文件信息
    $fileInfo = Get-Item $outputFile
    Write-Host "文件大小：$($fileInfo.Length / 1MB) MB"
} else {
    Write-Host "构建失败" -ForegroundColor Red
    exit 1
}
```

### 快速启动脚本（批处理）

```batch
@echo off
:: start.bat

:: 设置路径
set PUPPET_EXE=C:\Puppet\puppet.exe
set APP_PATH=C:\MyApp

:: 检查文件是否存在
if not exist "%PUPPET_EXE%" (
    echo Puppet.exe 未找到
    pause
    exit /b 1
)

if not exist "%APP_PATH%" (
    echo 应用路径不存在
    pause
    exit /b 1
)

:: 启动应用
echo 正在启动 Puppet 应用...
"%PUPPET_EXE%" --nake-load "%APP_PATH%"

pause
```

## 相关资源

- [PUP 文件格式](./pup-format.md) - PUP 文件结构说明
- [项目结构](./project-structure.md) - 项目目录组织
- [快速开始](./getting-started.md) - 快速上手指南

## 下一步

了解命令行参数后，建议：

1. 尝试不同的运行模式
2. 创建自动化构建脚本
3. 参考 [最佳实践](./best-practices.md) 优化工作流