import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/guide/getting-started.html.vue"
const data = JSON.parse("{\"path\":\"/guide/getting-started.html\",\"title\":\"快速开始 | 指南\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"快速开始\",\"permalink\":\"/guide/getting-started.html\",\"createTime\":\"2026/03/28 14:51:54\"},\"readingTime\":{\"minutes\":3.95,\"words\":1186},\"git\":{},\"filePathRelative\":\"guide/getting-started.md\",\"headers\":[]}")
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
