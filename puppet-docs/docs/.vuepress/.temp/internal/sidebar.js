export const sidebar = {"/":{"/guide/":{"items":[{"text":"概述","link":"/guide/","icon":"info"},{"text":"介绍","link":"/guide/introduction","icon":"book"},{"text":"快速开始","link":"/guide/getting-started","icon":"rocket"},{"text":"项目结构","link":"/guide/project-structure","icon":"folder-tree"},{"text":"命令行参数","link":"/guide/cli-parameters","icon":"terminal"},{"text":"PUP 文件格式","link":"/guide/pup-format","icon":"file-zip"},{"text":"架构设计","link":"/guide/architecture","icon":"architecture"},{"text":"最佳实践","link":"/guide/best-practices","icon":"star"},{"text":"安全机制","link":"/guide/security","icon":"shield"},{"text":"Puppet 签名工具","link":"/guide/puppet-sign","icon":"key"}],"prefix":"/guide/"},"/api/":{"items":[{"text":"概述","link":"/api/index","icon":"info"},{"text":"窗口控制","link":"/api/window","icon":"window"},{"text":"应用控制","link":"/api/application","icon":"apps"},{"text":"文件系统","link":"/api/fs","icon":"folder"},{"text":"日志","link":"/api/log","icon":"note"},{"text":"系统","link":"/api/system","icon":"setting"},{"text":"托盘图标","link":"/api/tray","icon":"menu"},{"text":"事件系统","link":"/api/events","icon":"bolt"},{"text":"设备系统","link":"/api/device","icon":"devices"},{"text":"持久化存储","link":"/api/storage","icon":"database"}],"prefix":"/api/"},"/changelog/":{"items":"auto","prefix":"/changelog/"}},"/en/":{},"__auto__":{"/changelog/":[]},"__home__":{"/changelog/":"/changelog/"}}

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updateSidebar) {
    __VUE_HMR_RUNTIME__.updateSidebar(sidebar)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ sidebar }) => {
    __VUE_HMR_RUNTIME__.updateSidebar(sidebar)
  })
}
