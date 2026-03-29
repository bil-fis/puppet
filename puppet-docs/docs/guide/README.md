---
title: 指南
permalink: /guide/
createTime: 2026/03/28 14:50:37
---

# 指南

欢迎使用 Puppet 框架文档。本指南将帮助您了解 Puppet 框架的核心概念、架构设计以及如何使用它构建桌面应用程序。

## 文档目录

- [快速开始](./getting-started.md) - 5 分钟快速上手 Puppet 框架
- [框架介绍](./introduction.md) - 了解 Puppet 框架的功能和特性
- [架构设计](./architecture.md) - 深入了解框架的内部架构和工作原理
- [项目结构](./project-structure.md) - Puppet 项目的标准目录结构
- [PUP 文件格式](./pup-format.md) - 了解 Puppet 独特的打包格式
- [命令行参数](./cli-parameters.md) - 命令行工具使用说明
- [安全机制](./security.md) - 框架的安全特性和最佳实践
- [最佳实践](./best-practices.md) - 开发高质量 Puppet 应用的建议
- [Puppet 签名工具](./puppet-sign.md) - 使用 puppet-sign 进行签名和验证

## 相关工具

- **puppet-sign** - 独立的签名工具，用于生成签名密钥对、签名数据库和验证签名
  - 支持生成自签名 X.509 证书
  - 支持 RSA 2048/4096 位密钥
  - 支持数据库签名和验证
  - 详见 [Puppet 签名工具](./puppet-sign.md)

## 什么是 Puppet？

Puppet 是一个基于 **WebView2** 的 Windows 桌面应用开发框架，允许开发者使用熟悉的 Web 技术（HTML、CSS、JavaScript）构建功能强大的桌面应用程序。

### 核心特性

- **Web 技术栈**：使用 HTML、CSS、JavaScript 构建界面
- **原生系统集成**：提供对 Windows 系统功能的完整访问
- **事件驱动**：支持 USB 设备、磁盘、窗口等事件监控
- **安全机制**：内置权限控制和安全验证
- **独特打包**：支持加密的 PUP 打包格式

### 适用场景

- 需要系统级功能的桌面应用
- 设备管理工具
- 系统监控工具
- 托盘应用
- 透明窗口应用
- 快速原型开发

## 技术栈

Puppet 框架基于以下技术构建：

- **.NET 9.0** - 核心框架
- **Windows Forms** - 桌面 UI 框架
- **WebView2** - 基于 Edge 的 Web 渲染引擎
- **COM Interop** - JavaScript 与 C# 的桥接
- **WMI** - Windows 管理接口，用于设备监控

## 快速链接

- [API 文档](../api/) - 完整的 API 参考手册
- [GitHub 仓库](#) - 源代码
- [问题反馈](#) - 提交问题和建议

## 获取帮助

如果您在使用过程中遇到问题：

1. 查阅本文档的相关章节
2. 查看 [API 文档](../api/) 了解具体用法
3. 搜索 [已知问题](#)
4. 在 [GitHub Issues](#) 提交问题

## 下一步

建议按照以下顺序阅读文档：

1. 阅读 [快速开始](./getting-started.md) 创建您的第一个 Puppet 应用
2. 了解 [框架介绍](./introduction.md) 掌握核心概念
3. 学习 [架构设计](./architecture.md) 理解工作原理
4. 参考 [API 文档](../api/) 进行实际开发
5. 遵循 [最佳实践](./best-practices.md) 开发高质量应用