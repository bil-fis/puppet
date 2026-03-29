import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/index.html.vue"
const data = JSON.parse("{\"path\":\"/\",\"title\":\"创建第一个应用\",\"lang\":\"zh-CN\",\"frontmatter\":{\"pageLayout\":\"home\",\"hero\":{\"name\":\"Puppet Framework\",\"text\":\"基于 WebView2 的桌面应用开发框架\",\"tagline\":\"用 Web 技术构建桌面应用\",\"image\":{\"src\":\"/logo.svg\",\"alt\":\"Puppet Framework\"},\"actions\":[{\"theme\":\"brand\",\"text\":\"快速上手\",\"link\":\"/guide/getting-started.html\"},{\"theme\":\"alt\",\"text\":\"API 文档\",\"link\":\"/api/\"},{\"theme\":\"alt\",\"text\":\"查看示例\",\"link\":\"/guide/best-practices.html\"}]},\"features\":[{\"title\":\"🌐 Web 技术栈\",\"details\":\"使用 HTML、CSS、JavaScript 构建桌面应用，降低开发门槛，熟悉的开发体验\"},{\"title\":\"⚡ 深度集成\",\"details\":\"提供对 Windows 系统功能的完整访问能力，包括文件系统、窗口管理、设备监控等\"},{\"title\":\"🎯 事件驱动\",\"details\":\"支持 USB 设备、磁盘、窗口等事件监控，实现实时响应和自动化操作\"},{\"title\":\"🔒 安全可靠\",\"details\":\"内置多层安全机制，通信验证、路径保护、权限确认、加密存储\"},{\"title\":\"📦 独特打包\",\"details\":\"支持 AES-256 加密的 PUP 打包格式，单文件分发，安全便捷\"},{\"title\":\"🚀 轻量高效\",\"details\":\"基于 .NET 和 WebView2，资源占用小，性能优秀，启动快速\"}]},\"readingTime\":{\"minutes\":2.03,\"words\":608},\"git\":{},\"filePathRelative\":\"README.md\",\"headers\":[]}")
export { comp, data }

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updatePageData) {
    __VUE_HMR_RUNTIME__.updatePageData(data)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ data }) => {
    __VUE_HMR_RUNTIME__.updatePageData(data)
  })
}
