/**
 * @see https://theme-plume.vuejs.press/config/navigation/ 查看文档了解配置详情
 *
 * Navbar 配置文件，它在 `.vuepress/plume.config.ts` 中被导入。
 */

import { defineNavbarConfig } from 'vuepress-theme-plume'

export const zhNavbar = defineNavbarConfig([
  { text: '首页', link: '/' },
  { text: '指南', link: '/guide/' },
  { text: 'API 文档', link: '/api/' },
  { text: '更新日志', link: '/changelog/' },
])

export const enNavbar = defineNavbarConfig([
  { text: 'Home', link: '/en/' },
  { text: 'Guide', link: '/en/guide/' },
  { text: 'API', link: '/en/api/' },
  { text: 'Changelog', link: '/en/changelog/' },
])