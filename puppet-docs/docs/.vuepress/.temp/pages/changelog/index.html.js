import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/changelog/index.html.vue"
const data = JSON.parse("{\"path\":\"/changelog/\",\"title\":\"更新日志 | 更新日志\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"更新日志\",\"permalink\":\"/changelog/\",\"createTime\":\"2026/03/28 15:13:53\"},\"readingTime\":{\"minutes\":4.2,\"words\":1261},\"git\":{},\"filePathRelative\":\"changelog/README.md\",\"headers\":[]}")
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
