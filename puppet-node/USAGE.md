# Puppet Framework TypeScript 类型定义使用指南

## 快速开始

### 1. 安装

```bash
# 方法1: 使用 npm
npm install @puppet-framework/types --save-dev

# 方法2: 直接复制文件
cp puppet-node/* your-project/
```

### 2. TypeScript 项目

在 `tsconfig.json` 中添加：

```json
{
  "compilerOptions": {
    "types": ["@puppet-framework/types"]
  }
}
```

在代码中使用：

```typescript
import puppet from '@puppet-framework/types';

async function init() {
    await puppet.window.setBorderless(true);
    await puppet.window.setOpacity(0.9);
}
```

### 3. JavaScript 项目

在 HTML 中引入：

```html
<script src="puppet.js"></script>
<script>
    async function init() {
        await puppet.window.setBorderless(true);
        await puppet.window.setOpacity(0.9);
    }
    init();
</script>
```

### 4. Node.js 项目

```javascript
const puppet = require('@puppet-framework/types');

async function main() {
    const sysInfo = await puppet.system.getSystemInfo();
    console.log(sysInfo);
}
main();
```

## 环境说明

### Puppet Framework 环境
- 真实的 puppet API 由 C# 应用注入
- 所有功能正常工作

### 浏览器环境
- 使用模拟实现
- API 调用输出到控制台
- 用于开发和调试

### Node.js 环境
- 使用模拟实现
- API 调用输出到控制台
- 用于测试和开发

## 文件说明

- `index.d.ts` - 主类型定义文件
- `puppet.d.ts` - 完整的 API 类型定义
- `puppet.js` - 运行时 JavaScript 文件（环境检测 + 模拟实现）
- `example.ts` - TypeScript 示例
- `example.html` - HTML 示例
- `package.json` - npm 包配置
- `tsconfig.json` - TypeScript 配置

## 常见问题

### Q: 如何在 TypeScript 中获得类型提示？
A: 确保已正确导入类型定义，TypeScript 会自动提供 IntelliSense。

### Q: 为什么浏览器中调用 API 没有实际效果？
A: 浏览器环境使用模拟实现，只输出日志。在 Puppet Framework 环境中会有实际效果。

### Q: 如何判断当前运行环境？
A: 检查 `window.chrome.webview` 是否存在。

## 更多信息

- 完整文档: README.md
- 示例代码: example.ts, example.html