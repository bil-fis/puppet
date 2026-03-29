import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/api/application.html.vue"
const data = JSON.parse("{\"path\":\"/api/application.html\",\"title\":\"应用控制 API | API 文档\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"应用控制 API\",\"permalink\":\"/api/application.html\",\"createTime\":\"2026/03/28 15:04:53\"},\"readingTime\":{\"minutes\":8.32,\"words\":2495},\"git\":{},\"filePathRelative\":\"api/application.md\",\"headers\":[]}")
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
