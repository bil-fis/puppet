import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/guide/index.html.vue"
const data = JSON.parse("{\"path\":\"/guide/\",\"title\":\"指南 | 指南\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"指南\",\"permalink\":\"/guide/\",\"createTime\":\"2026/03/28 14:50:37\"},\"readingTime\":{\"minutes\":2.04,\"words\":611},\"git\":{},\"filePathRelative\":\"guide/README.md\",\"headers\":[]}")
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
