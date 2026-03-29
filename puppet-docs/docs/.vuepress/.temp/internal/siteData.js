export const siteData = JSON.parse("{\"base\":\"/\",\"lang\":\"zh-CN\",\"title\":\"\",\"description\":\"\",\"head\":[[\"link\",{\"rel\":\"icon\",\"type\":\"image/x-icon\",\"href\":\"/puppet.ico\"}],[\"link\",{\"rel\":\"icon\",\"type\":\"image/png\",\"sizes\":\"32x32\",\"href\":\"/puppet.png\"}]],\"locales\":{\"/\":{\"title\":\"Puppet Document\",\"lang\":\"zh-CN\",\"description\":\"Puppet Framework Document\"},\"/en/\":{\"title\":\"Puppet Document\",\"lang\":\"en-US\",\"description\":\"Puppet Framework Document\"}}}")

if (import.meta.webpackHot) {
  import.meta.webpackHot.accept()
  if (__VUE_HMR_RUNTIME__.updateSiteData) {
    __VUE_HMR_RUNTIME__.updateSiteData(siteData)
  }
}

if (import.meta.hot) {
  import.meta.hot.accept(({ siteData }) => {
    __VUE_HMR_RUNTIME__.updateSiteData(siteData)
  })
}
