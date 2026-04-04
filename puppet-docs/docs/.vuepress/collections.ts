/**
 * @see https://theme-plume.vuejs.press/guide/collection/ 查看文档了解配置详情。
 *
 * Collections 配置文件，它在 `.vuepress/plume.config.ts` 中被导入。
 *
 * 请注意，你应该先在这里配置好 Collections，然后再启动 vuepress，主题会在启动 vuepress 时，
 * 读取这里配置的 Collections，然后在与 Collection 相关的 Markdown 文件中，自动生成 permalink。
 *
 * collection 的 type 为 `post` 时，表示为 文档列表类型（即没有侧边导航栏，有文档列表页）
 * 可用于实现如 博客、专栏 等以文章列表聚合形式的文档集合 （内容相对碎片化的）
 *
 * collection 的 type 为 `doc` 时，表示为文档类型（即有侧边导航栏）
 * 可用于实现如 笔记、知识库、文档等以侧边导航栏形式的文档集合 （内容强关联、成体系的）
 * 如果发现 侧边栏没有显示，那么请检查你的配置是否正确，以及 Markdown 文件中的 permalink
 * 是否是以对应的 Collection 配置的 link 的前缀开头。 是否展示侧边栏是根据 页面链接 的前缀 与 `collection.link`
 * 的前缀是否匹配来决定。
 */

/**
 * 在受支持的 IDE 中会智能提示配置项。
 *
 * - `defineCollections` 是用于定义 collection 集合的帮助函数
 * - `defineCollection` 是用于定义单个 collection 配置的帮助函数
 *
 * 通过 `defineCollection` 定义的 collection 配置，应该填入 `defineCollections` 中
 */
import { defineCollection, defineCollections } from 'vuepress-theme-plume'

/* =================== locale: zh-CN ======================= */

// 指南文档 - doc 类型，该类型带有侧边栏
const zhGuideDoc = defineCollection({
  type: 'doc',
  dir: 'guide',
  link: '/guide/',
  title: '指南',
  sidebar: [
    {
      text: '概述',
      link: '/guide/',
      icon: 'info'
    },
    {
      text: '介绍',
      link: '/guide/introduction.html',
      icon: 'book'
    },
    {
      text: '快速开始',
      link: '/guide/getting-started.html',
      icon: 'rocket'
    },
    {
      text: '项目结构',
      link: '/guide/project-structure.html',
      icon: 'folder-tree'
    },
    {
      text: '命令行参数',
      link: '/guide/cli-parameters.html',
      icon: 'terminal'
    },
    {
      text: 'PUP 文件格式',
      link: '/guide/pup-format.html',
      icon: 'file-zip'
    },
    {
      text: 'PUP 启动脚本',
      link: '/guide/pup-script.html',
      icon: 'code'
    },
    {
      text: '架构设计',
      link: '/guide/architecture.html',
      icon: 'architecture'
    },
    {
      text: '最佳实践',
      link: '/guide/best-practices.html',
      icon: 'star'
    },
    {
      text: '安全机制',
      link: '/guide/security.html',
      icon: 'shield'
    },
    {
      text: 'Puppet 签名工具',
      link: '/guide/puppet-sign.html',
      icon: 'key'
    }
  ]
})

// API 文档 - doc 类型，该类型带有侧边栏
const zhApiDoc = defineCollection({
  type: 'doc',
  dir: 'api',
  link: '/api/',
  title: 'API 文档',
  sidebar: [
    {
      text: '概述',
      link: '/api/index.html',
      icon: 'info'
    },
    {
      text: '窗口控制',
      link: '/api/window.html',
      icon: 'window'
    },
    {
      text: '应用控制',
      link: '/api/application.html',
      icon: 'apps'
    },
    {
      text: '文件系统',
      link: '/api/fs.html',
      icon: 'folder'
    },
    {
      text: '日志',
      link: '/api/log.html',
      icon: 'note'
    },
    {
      text: '系统',
      link: '/api/system.html',
      icon: 'setting'
    },
    {
      text: '托盘图标',
      link: '/api/tray.html',
      icon: 'menu'
    },
    {
      text: '事件系统',
      link: '/api/events.html',
      icon: 'bolt'
    },
    {
      text: '设备系统',
      link: '/api/device.html',
      icon: 'devices'
    },
    {
      text: '持久化存储',
      link: '/api/storage.html',
      icon: 'database'
    }
  ]
})

// 更新日志 - doc 类型，该类型带有侧边栏
const zhChangelogDoc = defineCollection({
  type: 'doc',
  dir: 'changelog',
  link: '/changelog/',
  title: '更新日志',
  sidebar: 'auto'
})

/**
 * 导出所有的 collections
 */
export const zhCollections = defineCollections([
  zhGuideDoc,
  zhApiDoc,
  zhChangelogDoc,
])

/* =================== locale: en-US ======================= */

// Guide documentation - doc type with sidebar
const enGuideDoc = defineCollection({
  type: 'doc',
  dir: 'guide',
  link: '/en/guide/',
  title: 'Guide',
  sidebar: [
    {
      text: 'Overview',
      link: '/en/guide/',
      icon: 'info',
      target: '_self'
    },
    {
      text: 'Introduction',
      link: '/en/guide/introduction.html',
      icon: 'book',
      target: '_self'
    },
    {
      text: 'Getting Started',
      link: '/en/guide/getting-started.html',
      icon: 'rocket',
      target: '_self'
    },
    {
      text: 'Project Structure',
      link: '/en/guide/project-structure.html',
      icon: 'folder-tree',
      target: '_self'
    },
    {
      text: 'Command Line Parameters',
      link: '/en/guide/cli-parameters.html',
      icon: 'terminal',
      target: '_self'
    },
    {
      text: 'PUP File Format',
      link: '/en/guide/pup-format.html',
      icon: 'file-zip',
      target: '_self'
    },
    {
      text: 'PUP Startup Script',
      link: '/en/guide/pup-script.html',
      icon: 'code',
      target: '_self'
    },
    {
      text: 'Architecture',
      link: '/en/guide/architecture.html',
      icon: 'architecture',
      target: '_self'
    },
    {
      text: 'Best Practices',
      link: '/en/guide/best-practices.html',
      icon: 'star',
      target: '_self'
    },
    {
      text: 'Security',
      link: '/en/guide/security.html',
      icon: 'shield',
      target: '_self'
    },
    {
      text: 'Puppet Signing Tool',
      link: '/en/guide/puppet-sign.html',
      icon: 'key',
      target: '_self'
    }
  ]
})

// API documentation - doc type with sidebar
const enApiDoc = defineCollection({
  type: 'doc',
  dir: 'api',
  link: '/en/api/',
  title: 'API Documentation',
  sidebar: [
    {
      text: 'Overview',
      link: '/en/api/index.html',
      icon: 'info',
      target: '_self'
    },
    {
      text: 'Window Control',
      link: '/en/api/window.html',
      icon: 'window',
      target: '_self'
    },
    {
      text: 'Application Control',
      link: '/en/api/application.html',
      icon: 'apps',
      target: '_self'
    },
    {
      text: 'File System',
      link: '/en/api/fs.html',
      icon: 'folder',
      target: '_self'
    },
    {
      text: 'Log',
      link: '/en/api/log.html',
      icon: 'note',
      target: '_self'
    },
    {
      text: 'System',
      link: '/en/api/system.html',
      icon: 'setting',
      target: '_self'
    },
    {
      text: 'Tray Icon',
      link: '/en/api/tray.html',
      icon: 'menu',
      target: '_self'
    },
    {
      text: 'Event System',
      link: '/en/api/events.html',
      icon: 'bolt',
      target: '_self'
    },
    {
      text: 'Device System',
      link: '/en/api/device.html',
      icon: 'devices',
      target: '_self'
    },
    {
      text: 'Persistent Storage',
      link: '/en/api/storage.html',
      icon: 'database',
      target: '_self'
    }
  ]
})

// Changelog - doc type with sidebar
const enChangelogDoc = defineCollection({
  type: 'doc',
  dir: 'changelog',
  link: '/en/changelog/',
  title: 'Changelog',
  sidebar: 'auto'
})

/**
 * Export all collections
 */
export const enCollections = defineCollections([
  enGuideDoc,
  enApiDoc,
  enChangelogDoc,
])