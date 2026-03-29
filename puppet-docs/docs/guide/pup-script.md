---
title: PUP 启动脚本
permalink: /guide/pup-script.html
createTime: 2026/03/29 19:45:00
---

# PUP 启动脚本

PUP 启动脚本是一个可选的功能，允许在应用启动时自动配置窗口属性。从 PUP V1.1 版本开始支持，V1.2 版本继续兼容并增强了签名验证功能。

## 概述

启动脚本是一个文本文件，包含一系列用于配置应用窗口的命令。通过使用启动脚本，您可以：

- **预设窗口位置**：在启动时自动将窗口移动到指定位置
- **配置窗口样式**：启用或禁用无边框窗口
- **设置窗口大小**：自定义应用窗口的尺寸

## 基本用法

### 创建脚本文件

创建一个文本文件（通常命名为 `script.txt` 或 `startup.txt`），添加配置命令：

```txt
# 这是一个启动脚本示例
set startup_position center
set borderless true
set window_size 800,600
```

### 创建 PUP 文件时包含脚本

使用 `--script` 参数指定脚本文件：

```bash
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "C:\script.txt"

# V1.2 版本（带签名）
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.2 --script "C:\script.txt" --certificate "app.crt" --private-key "app.key" --private-key-password "MyPassword"
```

## 脚本语法

### 基本格式

启动脚本使用简单的命令格式：

```txt
set <property> <value>
```

- 每行一个命令
- 命令不区分大小写
- 支持使用 `//` 或 `#` 添加注释
- 空行会被忽略

### 支持的命令

#### 1. startup_position

设置窗口启动位置。

**语法**：

```txt
set startup_position <position>
```

**支持的值**：

| 值 | 说明 |
|------|------|
| `left-top` | 屏幕左上角 |
| `left-bottom` | 屏幕左下角 |
| `right-top` | 屏幕右上角 |
| `right-bottom` | 屏幕右下角 |
| `center` | 屏幕中心 |
| `x,y` | 自定义坐标（像素） |

**示例**：

```txt
# 使用预定义位置
set startup_position center
set startup_position left-top

# 使用自定义坐标
set startup_position 100,100
set startup_position 1920,0
```

**注意事项**：

- 坐标是相对于主屏幕的左上角
- 坐标单位为像素
- 窗口可能被屏幕边界裁剪

#### 2. borderless

设置是否启用无边框窗口。

**语法**：

```txt
set borderless <value>
```

**支持的值**：

| 值 | 说明 |
|------|------|
| `true` | 启用无边框 |
| `false` | 禁用无边框 |
| `1` | 启用无边框 |
| `0` | 禁用无边框 |
| `yes` | 启用无边框 |
| `no` | 禁用无边框 |

**示例**：

```txt
set borderless true
set borderless 1
set borderless yes
```

**注意事项**：

- 无边框窗口会移除标题栏和边框
- 无边框窗口通常需要自己实现拖动功能
- 适合制作悬浮窗、桌面小部件等

#### 3. window_size

设置窗口大小。

**语法**：

```txt
set window_size <width>,<height>
```

**参数**：

- `width` - 窗口宽度（像素，必须为正整数）
- `height` - 窗口高度（像素，必须为正整数）

**示例**：

```txt
set window_size 800,600
set window_size 1920,1080
set window_size 640,480
```

**注意事项**：

- 宽度和高度必须为正整数
- 使用逗号分隔宽度与高度
- 窗口大小不能超过屏幕尺寸

## 完整示例

### 示例 1：居中窗口

```txt
# 居中窗口，无边框，默认大小
set startup_position center
set borderless true
```

### 示例 2：固定位置和大小

```txt
# 固定位置和大小
set startup_position 100,100
set window_size 640,480
```

### 示例 3：右下角悬浮窗

```txt
# 右下角悬浮窗
set startup_position right-bottom
set borderless true
set window_size 300,200
```

### 示例 4：全屏应用

```txt
# 全屏应用（1920x1080）
set startup_position left-top
set borderless true
set window_size 1920,1080
```

### 示例 5：注释丰富的脚本

```txt
# ===================================
# MyApp 启动脚本
# 版本: 1.0
# 作者: MyCompany
# ===================================

// 设置窗口位置为屏幕中心
set startup_position center

// 启用无边框模式，制作更现代的界面
set borderless true

// 设置窗口大小为 1280x720（720p）
set window_size 1280,720
```

## 执行顺序

启动脚本中的命令按照从上到下的顺序执行。建议按照以下顺序组织脚本：

1. **设置窗口大小**（`window_size`）- 先确定窗口尺寸
2. **设置窗口样式**（`borderless`）- 然后设置窗口样式
3. **设置窗口位置**（`startup_position`）- 最后定位窗口

**推荐顺序**：

```txt
set window_size 800,600
set borderless true
set startup_position center
```

## 错误处理

如果脚本执行过程中出现错误：

- 错误信息会输出到控制台
- 当前命令会被跳过
- 脚本会继续执行后续命令

**常见错误**：

```
Error executing line 3: Invalid startup position value: invalid
  Command: set startup_position invalid
```

**解决方案**：

- 检查命令语法是否正确
- 确认参数值在支持的范围内
- 查看控制台输出的错误信息

## 版本兼容性

| 版本 | 启动脚本支持 | 签名支持 |
|------|------------|---------|
| V1.0 | ❌ | ❌ |
| V1.1 | ✅ | ❌ |
| V1.2 | ✅ | ✅ |

### V1.1

- 支持基本的启动脚本功能
- 支持 startup_position、borderless、window_size 命令

### V1.2

- 完全兼容 V1.1 的启动脚本功能
- 支持数字签名验证
- 脚本和 PUP 文件内容可以被签名保护

## 与 JavaScript API 的对比

启动脚本和 JavaScript API 都可以控制窗口，但用途不同：

| 特性 | 启动脚本 | JavaScript API |
|------|---------|---------------|
| 执行时机 | 应用启动时 | 应用加载后 |
| 用途 | 初始配置 | 动态控制 |
| 修改窗口位置 | ✅ | ✅ |
| 修改窗口样式 | ✅ | ✅ |
| 修改窗口大小 | ✅ | ✅ |
| 响应用户操作 | ❌ | ✅ |
| 动态调整 | ❌ | ✅ |

**使用建议**：

- **启动脚本**：用于设置应用的初始状态
- **JavaScript API**：用于响应用户交互和动态调整窗口

**混合使用示例**：

```txt
# 启动脚本：设置初始状态
set startup_position center
set borderless true
```

```javascript
// JavaScript：响应用户操作
document.getElementById('toggle-border').addEventListener('click', async () => {
    await puppet.window.setBorderless(!isBorderless);
});
```

## 最佳实践

### 1. 使用注释

为脚本添加清晰的注释，说明每个配置的作用：

```txt
# 设置窗口为无边框模式，提供更现代的界面体验
set borderless true
```

### 2. 提供默认值

如果用户需要自定义配置，可以在脚本中提供合理的默认值：

```txt
# 默认窗口大小（可被用户配置覆盖）
set window_size 1024,768
```

### 3. 测试脚本

在发布前，使用裸文件夹模式测试脚本：

```bash
puppet.exe --nake-load "C:\MyProject"
```

### 4. 文档化脚本

在项目文档中说明脚本的功能和配置项，方便其他开发者理解和维护。

### 5. 版本控制

将脚本文件纳入版本控制系统，记录配置变更历史。

## 高级用法

### 动态脚本生成

如果需要根据不同环境生成不同的脚本，可以使用脚本生成工具：

```bash
# 开发环境
echo "set startup_position 0,0" > dev-script.txt
echo "set borderless false" >> dev-script.txt

# 生产环境
echo "set startup_position center" > prod-script.txt
echo "set borderless true" >> prod-script.txt
echo "set window_size 800,600" >> prod-script.txt

# 创建 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp-dev.pup" -v 1.1 --script "dev-script.txt"
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp-prod.pup" -v 1.1 --script "prod-script.txt"
```

### 条件配置

如果需要根据条件选择不同的配置，可以在构建脚本中实现：

```powershell
# PowerShell 构建脚本
$scriptContent = ""
if ($env:BUILD_ENV -eq "production") {
    $scriptContent = "set borderless true`nset window_size 800,600"
} else {
    $scriptContent = "set borderless false`nset window_size 1024,768"
}

Set-Content -Path "script.txt" -Value $scriptContent
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "script.txt"
```

## 故障排除

### 问题：脚本没有生效

**可能原因**：

1. 脚本文件路径不正确
2. PUP 版本低于 V1.1
3. 脚本语法错误

**解决方案**：

```bash
# 检查 PUP 版本
puppet.exe --version

# 检查脚本文件是否存在
type script.txt

# 使用绝对路径创建 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "MyApp.pup" -v 1.1 --script "C:\full\path\to\script.txt"
```

### 问题：窗口位置不正确

**可能原因**：

1. 坐标超出屏幕范围
2. 多显示器环境下的坐标计算错误

**解决方案**：

```txt
# 使用预定义位置代替绝对坐标
set startup_position center

# 或使用较小的坐标值
set startup_position 50,50
```

### 问题：无边框窗口无法移动

**原因**：无边框窗口默认不支持拖动

**解决方案**：

使用 JavaScript API 实现拖动功能：

```javascript
let isDragging = false;
let startX, startY;

document.addEventListener('mousedown', (e) => {
    isDragging = true;
    startX = e.clientX;
    startY = e.clientY;
});

document.addEventListener('mousemove', async (e) => {
    if (isDragging) {
        const dx = e.clientX - startX;
        const dy = e.clientY - startY;
        const info = await puppet.window.getWindowInfo();
        await puppet.window.moveWindow(info.x + dx, info.y + dy);
        startX = e.clientX;
        startY = e.clientY;
    }
});

document.addEventListener('mouseup', () => {
    isDragging = false;
});
```

## 相关资源

- [PUP 文件格式](./pup-format.md) - 了解 PUP 文件的结构
- [命令行参数](./cli-parameters.md) - --script 参数说明
- [窗口控制 API](../api/window.md) - JavaScript 窗口控制 API
- [Puppet 签名工具](./puppet-sign.md) - 签名 PUP 文件

## 更新日志

### V1.2 (2026-03-29)

- 完全兼容 V1.1 的启动脚本功能
- 支持数字签名验证

### V1.1 (2026-03-28)

- 首次引入启动脚本功能
- 支持 startup_position、borderless、window_size 命令