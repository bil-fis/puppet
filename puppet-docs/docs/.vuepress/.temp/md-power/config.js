import { defineClientConfig } from 'vuepress/client'
import VPCopyButton from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/components/VPCopyButton.vue'
import Tabs from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/components/Tabs.vue'
import CodeTabs from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/components/CodeTabs.vue'
import Plot from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/components/Plot.vue'
import FileTreeNode from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/components/FileTreeNode.vue'
import { setupMarkHighlight } from 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/composables/mark.js'

import 'E:/puppet/puppet-docs/node_modules/vuepress-theme-plume/node_modules/vuepress-plugin-md-power/lib/client/styles/index.css'

export default defineClientConfig({
  enhance({ router, app }) {
    app.component('VPCopyButton', VPCopyButton)
    app.component('Tabs', Tabs)
    app.component('CodeTabs', CodeTabs)
    app.component('Plot', Plot)
    app.component('FileTreeNode', FileTreeNode)
  },
  setup() {
        setupMarkHighlight("eager")

  }
})
