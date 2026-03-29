import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/guide/introduction.html.vue"
const data = JSON.parse("{\"path\":\"/guide/introduction.html\",\"title\":\"框架介绍 | 指南\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"框架介绍\",\"permalink\":\"/guide/introduction.html\",\"createTime\":\"2026/03/28 14:52:37\"},\"readingTime\":{\"minutes\":6.1,\"words\":1831},\"git\":{},\"filePathRelative\":\"guide/introduction.md\",\"headers\":[]}")
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
