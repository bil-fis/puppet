export const sidebar = {"/":{"/guide/":{"items":"auto","prefix":"/guide/"},"/api/":{"items":[{"text":"概述","link":"/api/index","icon":"info"},{"text":"窗口控制","link":"/api/window","icon":"window"},{"text":"应用控制","link":"/api/application","icon":"apps"},{"text":"文件系统","link":"/api/fs","icon":"folder"},{"text":"日志","link":"/api/log","icon":"note"},{"text":"系统","link":"/api/system","icon":"setting"},{"text":"托盘图标","link":"/api/tray","icon":"menu"},{"text":"事件系统","link":"/api/events","icon":"bolt"},{"text":"设备系统","link":"/api/device","icon":"devices"}],"prefix":"/api/"},"/changelog/":{"items":"auto","prefix":"/changelog/"}},"/en/":{},"__auto__":{"/guide/":[{"text":"架构设计","link":"/guide/architecture.html"},{"text":"最佳实践","link":"/guide/best-practices.html"},{"text":"命令行参数","link":"/guide/cli-parameters.html"},{"text":"快速开始","link":"/guide/getting-started.html"},{"text":"框架介绍","link":"/guide/introduction.html"},{"text":"项目结构","link":"/guide/project-structure.html"},{"text":"PUP 文件格式","link":"/guide/pup-format.html"},{"text":"安全机制","link":"/guide/security.html"}],"/changelog/":[]},"__home__":{"/guide/":"/guide/","/changelog/":"/changelog/"}}

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
