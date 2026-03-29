import comp from "E:/puppet/puppet-docs/docs/.vuepress/.temp/pages/api/index.html.vue"
const data = JSON.parse("{\"path\":\"/api/index.html\",\"title\":\"API 概述 | API 文档\",\"lang\":\"zh-CN\",\"frontmatter\":{\"title\":\"API 概述\",\"permalink\":\"/api/index.html\",\"createTime\":\"2026/03/28 15:01:52\"},\"readingTime\":{\"minutes\":4.63,\"words\":1389},\"git\":{},\"filePathRelative\":\"api/index.md\",\"headers\":[]}")
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
