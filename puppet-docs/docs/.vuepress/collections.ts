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
  linkPrefix: '/guide',
  title: '指南',
  sidebar: 'auto'
})

// API 文档 - doc 类型，该类型带有侧边栏
const zhApiDoc = defineCollection({
  type: 'doc',
  dir: 'api',
  linkPrefix: '/api',
  title: 'API 文档',
  sidebar: [
    {
      text: '概述',
      link: '/api/index',
      icon: 'info'
    },
    {
      text: '窗口控制',
      link: '/api/window',
      icon: 'window'
    },
    {
      text: '应用控制',
      link: '/api/application',
      icon: 'apps'
    },
    {
      text: '文件系统',
      link: '/api/fs',
      icon: 'folder'
    },
    {
      text: '日志',
      link: '/api/log',
      icon: 'note'
    },
    {
      text: '系统',
      link: '/api/system',
      icon: 'setting'
    },
    {
      text: '托盘图标',
      link: '/api/tray',
      icon: 'menu'
    },
    {
      text: '事件系统',
      link: '/api/events',
      icon: 'bolt'
    },
    {
      text: '设备系统',
      link: '/api/device',
      icon: 'devices'
    }
  ]
})

// 更新日志 - doc 类型，该类型带有侧边栏
const zhChangelogDoc = defineCollection({
  type: 'doc',
  dir: 'changelog',
  linkPrefix: '/changelog',
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

// 英文配置预留（如需要可添加）
export const enCollections = defineCollections([])