export const collections = {"/":[{"type":"doc","dir":"guide","linkPrefix":"/guide","title":"指南"},{"type":"doc","dir":"api","linkPrefix":"/api","title":"API 文档"},{"type":"doc","dir":"changelog","linkPrefix":"/changelog","title":"更新日志"}]}

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updateCollections) {
    __VUE_HMR_RUNTIME__.updateCollections(collections)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ collections }) => {
    __VUE_HMR_RUNTIME__.updateCollections(collections)
  })
}
