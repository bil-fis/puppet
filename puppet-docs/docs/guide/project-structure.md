---
title: 项目结构
permalink: /guide/project-structure.html
createTime: 2026/03/28 14:54:32
---

# 项目结构

本章节介绍 Puppet 项目的标准目录结构和各个文件的作用。理解项目结构有助于您更好地组织和管理 Puppet 应用。

## 标准项目结构

一个典型的 Puppet 项目具有以下结构：

```
MyPuppetProject/
├── index.html              # 主页面（入口文件）
├── css/                    # 样式文件目录
│   └── style.css          # 主样式表
├── js/                     # JavaScript 文件目录
│   └── app.js             # 主应用脚本
├── assets/                 # 资源文件目录
│   ├── images/            # 图片资源
│   ├── fonts/             # 字体文件
│   └── icons/             # 图标文件
├── lib/                    # 第三方库目录
│   └── vendor.js          # 第三方库文件
└── puppet.json             # 应用配置文件（可选）
```

## 文件说明

### 必需文件

#### index.html

应用的主入口文件，通常包含：

```html
<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>我的 Puppet 应用</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <!-- 应用内容 -->
    <div id="app">
        <h1>欢迎</h1>
    </div>
    
    <!-- 引入 JavaScript -->
    <script src="js/app.js"></script>
</body>
</html>
```

**最佳实践**：

- 使用语义化的 HTML 标签
- 添加适当的 meta 标签
- 在页面底部加载 JavaScript 以提高性能
- 使用相对路径引用资源

### 推荐目录

#### css/

存放所有样式文件：

```
css/
├── style.css              # 主样式表
├── components/            # 组件样式
│   ├── button.css        # 按钮样式
│   └── dialog.css        # 对话框样式
└── themes/               # 主题样式
    └── dark.css          # 暗色主题
```

**命名建议**：

- 使用小写字母和连字符（kebab-case）
- 主样式表命名为 `style.css`
- 组件样式按功能分类

#### js/

存放所有 JavaScript 文件：

```
js/
├── app.js                 # 主应用脚本
├── utils/                 # 工具函数
│   ├── helpers.js        # 辅助函数
│   └── validators.js     # 验证函数
├── components/            # 组件脚本
│   ├── Button.js         # 按钮组件
│   └── Dialog.js         # 对话框组件
└── services/              # 服务层
    └── api.js            # API 封装
```

**命名建议**：

- 使用 PascalCase 命名类和组件
- 使用 camelCase 命名函数和变量
- 按功能模块组织代码

#### assets/

存放静态资源文件：

```
assets/
├── images/               # 图片资源
│   ├── logo.png         # Logo
│   ├── icons/           # 图标
│   └── backgrounds/     # 背景图
├── fonts/               # 字体文件
│   └── font.ttf         # 字体文件
└── data/                # 数据文件
    └── config.json      # 配置数据
```

**最佳实践**：

- 压缩图片以减少文件大小
- 使用适当的图片格式（PNG、JPG、SVG）
- 为字体文件添加 Web 格式支持

#### lib/

存放第三方库文件：

```
lib/
├── vue.min.js           # Vue.js
├── react.min.js         # React
└── axios.min.js         # Axios
```

**建议**：

- 使用 CDN 引入常见库以减少项目体积
- 或使用 npm 安装并打包
- 保留库的版本信息

### 可选文件

#### puppet.json

应用的配置文件，用于存储运行时配置：

```json
{
  "appName": "My App",
  "version": "1.0.0",
  "settings": {
    "theme": "dark",
    "language": "zh-CN"
  }
}
```

使用方式：

```javascript
// 读取配置
const config = await puppet.Application.getConfig('appName');

// 写入配置
await puppet.Application.setConfig('theme', 'light');
```

#### favicon.ico

应用图标，Puppet 会自动使用网站的 favicon 作为窗口图标。

**推荐尺寸**：

- 16x16（小图标）
- 32x32（标准图标）
- 48x48（大图标）
- 256x256（高分辨率）

## 框架项目结构

Puppet 框架本身的项目结构：

```
puppet/
├── Program.cs                 # 应用入口
├── Form1.cs                   # 主窗口
├── Form1.Designer.cs          # 窗口设计器生成
├── Form1.resx                 # 资源文件
├── PupServer.cs               # PUP 服务器
├── PupCreator.cs              # PUP 创建器
├── AesHelper.cs               # 加密工具
├── SecretKey.cs               # 密钥管理
├── IniReader.cs               # INI 读取器
├── PortSelector.cs            # 端口选择器
├── PermissionDialog.cs        # 权限对话框
├── puppet.ini                 # 框架配置
├── puppet.csproj              # 项目文件
├── Controllers/               # 控制器目录
│   ├── ApplicationController.cs
│   ├── FileSystemController.cs
│   ├── WindowController.cs
│   ├── EventController.cs
│   ├── LogController.cs
│   ├── SystemController.cs
│   └── TrayController.cs
└── test-htmls/                # 测试页面
    ├── index.html
    ├── event-test.html
    └── device-test.html
```

## 配置文件

### puppet.ini

框架的配置文件，通常位于 Puppet.exe 同级目录：

```ini
[file]
; 要加载的 PUP 文件路径
file=app.pup

[server]
; 服务器端口（默认自动选择）
port=7738

[security]
; 是否启用严格模式
strict=true
```

**用途**：

- 指定默认加载的 PUP 文件
- 配置服务器端口
- 设置安全选项

### puppet.csproj

.NET 项目文件，定义项目配置：

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

## 文件命名约定

### HTML 文件

- 使用小写字母和连字符（kebab-case）
- 主页面命名为 `index.html`
- 其他页面按功能命名

```
index.html           # 主页面
about.html           # 关于页面
settings.html        # 设置页面
user-profile.html    # 用户资料页面
```

### CSS 文件

- 使用小写字母和连字符（kebab-case）
- 主样式表命名为 `style.css`
- 组件样式按组件命名

```
style.css            # 主样式表
button.css           # 按钮样式
modal.css            # 模态框样式
sidebar.css          # 侧边栏样式
```

### JavaScript 文件

- 使用小写字母和连字符（kebab-case）
- 主脚本命名为 `app.js`
- 组件脚本按组件命名

```
app.js               # 主应用脚本
button.js            # 按钮组件
modal.js             # 模态框组件
utils.js             # 工具函数
```

### 资源文件

- 图片：使用小写字母和连字符
- 字体：保持原始文件名
- 图标：描述性命名

```
logo.png             # Logo
background-image.png # 背景图
icon-close.png       # 关闭图标
roboto-regular.ttf   # 字体文件
```

## 组织策略

### 按功能组织

适合大型项目，每个功能模块独立：

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

### 按类型组织

适合中小型项目，文件按类型分类：

```
project/
├── index.html
├── pages/              # 所有页面
│   ├── about.html
│   └── settings.html
├── css/                # 所有样式
│   └── style.css
├── js/                 # 所有脚本
│   └── app.js
└── assets/             # 所有资源
    ├── images/
    └── fonts/
```

### 混合组织

结合两种方式的优点：

```
project/
├── index.html
├── core/               # 核心文件
│   ├── css/
│   ├── js/
│   └── assets/
├── features/           # 功能模块
│   ├── user/
│   └── admin/
└── shared/             # 共享资源
    ├── components/
    └── utils/
```

## 版本控制

### .gitignore

推荐使用以下 `.gitignore` 配置：

```gitignore
# 依赖
node_modules/
lib/vendor.js

# 构建输出
dist/
build/
*.pup

# IDE
.vs/
.idea/
*.suo
*.user

# 系统文件
.DS_Store
Thumbs.db

# 日志
*.log
```

### 文件提交策略

- 提交源代码（HTML、CSS、JS）
- 提交配置文件（JSON、INI）
- 不提交编译输出（.pup 文件）
- 不提交依赖（node_modules）

## 打包和分发

### PUP 文件生成

```bash
# 创建 PUP 文件
puppet.exe --create-pup -i "C:\MyProject" -o "C:\MyProject.pup"
```

### 分发包结构

推荐的分发包结构：

```
MyApp/
├── MyApp.pup            # PUP 文件
├── puppet.exe           # Puppet 运行时（可选）
├── README.txt           # 使用说明
└── LICENSE.txt          # 许可证
```

## 最佳实践

### 1. 目录结构

- 保持结构清晰和一致性
- 使用有意义的文件夹名称
- 避免过深的目录层级

### 2. 文件命名

- 使用一致的命名约定
- 名称应描述文件内容
- 避免特殊字符和空格

### 3. 代码组织

- 将相关代码放在一起
- 使用注释说明复杂逻辑
- 保持文件大小合理（建议 < 500 行）

### 4. 资源管理

- 压缩图片和媒体文件
- 使用 CDN 加载第三方库
- 缓存常用资源

### 5. 配置管理

- 将配置与代码分离
- 使用 JSON 格式存储配置
- 提供默认配置值

## 相关资源

- [PUP 文件格式](./pup-format.md) - 了解 PUP 打包格式
- [命令行参数](./cli-parameters.md) - 命令行工具使用说明
- [最佳实践](./best-practices.md) - 开发建议和技巧

## 下一步

理解项目结构后，建议：

1. 创建您的第一个项目
2. 学习 [API 文档](../api/) 开始开发
3. 参考 [最佳实践](./best-practices.md) 提升代码质量