# Release 指南

本文档说明如何使用 GitHub Actions 自动构建和发布 Puppet 框架的 Release 版本。

## 前置条件

- Git 已安装并配置
- GitHub 账户和仓库访问权限
- 本地已安装 .NET 9.0 SDK（用于本地测试）

## 创建 Release 的步骤

### 1. 更新版本号

在发布前，确保所有相关文件中的版本号已更新：

- `puppet/puppet.csproj` - 项目版本
- `puppet-sign/puppet-sign.csproj` - 签名工具版本
- `puppet-docs/docs/changelog/README.md` - 更新日志

### 2. 更新更新日志

在 `puppet-docs/docs/changelog/README.md` 中添加新版本的更新内容：

```markdown
## v1.2.0 (2026-03-29)

### 新增
- 支持 PUP V1.2 格式的数字签名功能
- 新增 puppet-sign 签名工具
- 支持数据库签名验证

### 改进
- 优化了文档结构
- 改进了错误处理

### 修复
- 修复了窗口位置计算问题
```

### 3. 提交更改

```bash
# 添加所有更改
git add .

# 提交更改
git commit -m "Release v1.2.0"

# 推送到远程仓库
git push origin main
```

### 4. 创建并推送 Tag

```bash
# 创建 tag
git tag v1.2.0

# 推送 tag
git push origin v1.2.0
```

### 5. 等待构建完成

- 访问 GitHub 仓库的 "Actions" 标签
- 查看 "Release Build" 工作流的执行状态
- 构建通常需要 3-5 分钟

### 6. 检查 Release

构建完成后：

1. 访问 GitHub 仓库的 "Releases" 标签
2. 确认新的 Release 已创建
3. 下载并测试 zip 文件

## 版本号规范

### 正式版本

格式：`v主版本号.次版本号.修订号`

- `v1.0.0` - 第一个正式版本
- `v1.1.0` - 新增功能的次要版本
- `v1.1.1` - 错误修复的补丁版本

### 预发布版本

格式：`v主版本号.次版本号.修订号-预发布标识.预发布编号`

- `v1.2.0-alpha.1` - Alpha 测试版本
- `v1.2.0-beta.1` - Beta 测试版本
- `v1.2.0-rc.1` - Release Candidate 版本

预发布版本会自动标记为 prerelease。

## 发布文件说明

### Framework-dependent 版本

**优点**：
- 文件体积小（约 5-10 MB）
- 下载速度快

**缺点**：
- 需要用户安装 .NET 9.0 运行时

**适用场景**：
- 企业内部部署（已安装 .NET 运行时）
- 有经验的开发者用户

### Self-contained 版本

**优点**：
- 无需安装 .NET 运行时
- 开箱即用

**缺点**：
- 文件体积大（约 80-120 MB）
- 下载时间较长

**适用场景**：
- 普通用户
- 快速部署
- 不确定用户是否安装 .NET 运行时

## 本地测试

在推送 tag 之前，建议进行本地测试：

```powershell
# 1. 恢复依赖
dotnet restore puppet.sln

# 2. 构建 Release 版本
dotnet build puppet.sln --configuration Release

# 3. 测试 Framework-dependent 版本
dotnet publish puppet/puppet.csproj --configuration Release --output test-framework --self-contained false
dotnet publish puppet-sign/puppet-sign.csproj --configuration Release --output test-sign-framework --self-contained false

# 4. 测试 Self-contained 版本
dotnet publish puppet/puppet.csproj --configuration Release --output test-self --self-contained true --runtime win-x64
dotnet publish puppet-sign/puppet-sign.csproj --configuration Release --output test-sign-self --self-contained true --runtime win-x64

# 5. 运行测试
.\test-framework\puppet.exe --help
.\test-sign-framework\puppet-sign.exe --help
```

## 验证 Release

### 1. 下载文件

从 GitHub Release 下载以下文件：

- `puppet-windows-framework-dependent.zip`
- `puppet-sign-windows-framework-dependent.zip`
- `puppet-windows-self-contained.zip`
- `puppet-sign-windows-self-contained.zip`

### 2. 解压并测试

```powershell
# 解压 Framework-dependent 版本
Expand-Archive puppet-windows-framework-dependent.zip -DestinationPath puppet-test
Expand-Archive puppet-sign-windows-framework-dependent.zip -DestinationPath puppet-sign-test

# 测试运行
.\puppet-test\puppet.exe --help
.\puppet-sign-test\puppet-sign.exe --help

# 测试创建 PUP 文件
.\puppet-test\puppet.exe --create-pup -i "C:\test-project" -o "test.pup"

# 测试签名功能
.\puppet-sign-test\puppet-sign.exe --generate-signing-key --interactive
```

### 3. 检查文档

确认文档已正确更新：

- [PUP 文件格式](../puppet-docs/docs/guide/pup-format.md)
- [更新日志](../puppet-docs/docs/changelog/README.md)
- [API 文档](../puppet-docs/docs/api/)

## 常见问题

### Q: 构建失败怎么办？

A: 检查以下内容：

1. GitHub Actions 日志中的错误信息
2. 项目文件是否有语法错误
3. 依赖包是否正确配置
4. .NET 版本是否正确

### Q: 如何删除错误的 Release？

A:

```bash
# 删除本地 tag
git tag -d v1.2.0

# 删除远程 tag
git push origin :refs/tags/v1.2.0

# 在 GitHub 上手动删除 Release
```

### Q: 如何修改已发布的 Release？

A:

1. 在 GitHub 上找到对应的 Release
2. 点击 "Edit release" 按钮
3. 修改 Release 说明和附件
4. 保存更改

### Q: 构建需要多长时间？

A: 通常需要 3-5 分钟，取决于：

- GitHub 的服务器负载
- 项目的复杂程度
- 网络速度

### Q: 如何回退到之前的版本？

A:

1. 在 GitHub 上找到之前的 Release
2. 下载对应的 zip 文件
3. 解压并运行

或者：

```bash
# 检出之前的版本
git checkout v1.1.0

# 重新构建
dotnet build puppet.sln --configuration Release
```

## 相关资源

- [GitHub Actions 工作流](.github/workflows/release.yml)
- [更新日志](puppet-docs/docs/changelog/README.md)
- [API 文档](puppet-docs/docs/api/)

## 支持

如果在发布过程中遇到问题：

1. 检查 GitHub Actions 日志
2. 提交 Issue 到 GitHub 仓库