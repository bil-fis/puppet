import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/api/log.html.vue"
const data = JSON.parse("{\"path\":\"/api/log.html\",\"title\":\"日志 API | API 文档\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"日志 API\",\"permalink\":\"/api/log.html\",\"createTime\":\"2026/03/28 15:07:28\"},\"readingTime\":{\"minutes\":3.29,\"words\":987},\"git\":{},\"filePathRelative\":\"api/log.md\",\"headers\":[]}")
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
