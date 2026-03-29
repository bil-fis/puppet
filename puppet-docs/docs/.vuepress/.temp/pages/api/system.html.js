import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/api/system.html.vue"
const data = JSON.parse("{\"path\":\"/api/system.html\",\"title\":\"系统 API | API 文档\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"系统 API\",\"permalink\":\"/api/system.html\",\"createTime\":\"2026/03/28 15:08:10\"},\"readingTime\":{\"minutes\":5.06,\"words\":1518},\"git\":{},\"filePathRelative\":\"api/system.md\",\"headers\":[]}")
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
