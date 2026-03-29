import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/guide/best-practices.html.vue"
const data = JSON.parse("{\"path\":\"/guide/best-practices.html\",\"title\":\"最佳实践 | 指南\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"最佳实践\",\"permalink\":\"/guide/best-practices.html\",\"createTime\":\"2026/03/28 15:01:04\"},\"readingTime\":{\"minutes\":12.15,\"words\":3644},\"git\":{},\"filePathRelative\":\"guide/best-practices.md\",\"headers\":[]}")
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
