# GitHub Actions CI/CD

本目录包含 Puppet 项目的 GitHub Actions 工作流配置。

## 工作流文件

### release.yml

用于自动化构建和发布 Release 版本的工作流。

## 使用方法

### 创建 Release

1. 确保所有更改已提交并推送到 GitHub
2. 创建一个新的 tag（版本号格式：`v*.*.*`）

```bash
# 创建并推送 tag
git tag v1.2.0
git push origin v1.2.0
```

3. GitHub Actions 会自动：
   - 构建 puppet 和 puppet-sign 项目
   - 生成两种发布版本：
     - **Framework-dependent**：需要 .NET 9.0 运行时（体积小）
     - **Self-contained**：包含 .NET 运行时（体积大，无需额外安装）
   - 创建 GitHub Release
   - 上传 zip 文件到 Release

### 发布文件说明

#### Framework-dependent 版本

- `puppet-windows-framework-dependent.zip` - Puppet 主程序（需要 .NET 9.0）
- `puppet-sign-windows-framework-dependent.zip` - 签名工具（需要 .NET 9.0）

#### Self-contained 版本

- `puppet-windows-self-contained.zip` - Puppet 主程序（包含 .NET 运行时）
- `puppet-sign-windows-self-contained.zip` - 签名工具（包含 .NET 运行时）

## 版本号规范

- **正式版本**：`v1.2.0`
- **预发布版本**：`v1.2.0-beta.1`, `v1.2.0-rc.1`

预发布版本会自动标记为 prerelease。

## 系统要求

### 构建环境

- Windows Server 2019 或更高版本
- .NET 9.0 SDK

### 运行环境

- Windows 10 或更高版本
- WebView2 运行时
- Framework-dependent 版本还需要 .NET 9.0 运行时

## 本地测试

在推送 tag 之前，建议在本地测试构建：

```bash
# 恢复依赖
dotnet restore puppet.sln

# 构建 Release 版本
dotnet build puppet.sln --configuration Release

# 发布 Framework-dependent 版本
dotnet publish puppet/puppet.csproj --configuration Release --output puppet-test/framework-dependent --self-contained false
dotnet publish puppet-sign/puppet-sign.csproj --configuration Release --output puppet-sign-test/framework-dependent --self-contained false

# 发布 Self-contained 版本
dotnet publish puppet/puppet.csproj --configuration Release --output puppet-test/self-contained --self-contained true --runtime win-x64
dotnet publish puppet-sign/puppet-sign.csproj --configuration Release --output puppet-sign-test/self-contained --self-contained true --runtime win-x64
```

## 故障排除

### 构建失败

1. 检查 GitHub Actions 日志
2. 确认 .NET 版本配置正确
3. 检查项目文件是否有语法错误

### Release 创建失败

1. 确认 tag 格式正确（`v*.*.*`）
2. 检查 GitHub Token 权限
3. 确认没有重复的 tag

### 文件下载问题

1. 检查 zip 文件是否完整
2. 确认系统满足运行时要求
3. 尝试使用 Self-contained 版本

## 相关链接

- [GitHub Actions 文档](https://docs.github.com/en/actions)
- [.NET 9.0 文档](https://learn.microsoft.com/en-us/dotnet/core/)
- [softprops/action-gh-release](https://github.com/softprops/action-gh-release)