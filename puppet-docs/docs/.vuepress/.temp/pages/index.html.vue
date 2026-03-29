<template><div><h2 id="适用场景" tabindex="-1"><a class="header-anchor" href="#适用场景"><span>适用场景</span></a></h2>
<h3 id="💾-设备管理工具" tabindex="-1"><a class="header-anchor" href="#💾-设备管理工具"><span>💾 设备管理工具</span></a></h3>
<p>USB 设备管理器、磁盘管理工具、外设监控应用</p>
<h3 id="📊-系统监控" tabindex="-1"><a class="header-anchor" href="#📊-系统监控"><span>📊 系统监控</span></a></h3>
<p>系统资源监控、进程管理器、网络监控工具</p>
<h3 id="🔔-托盘应用" tabindex="-1"><a class="header-anchor" href="#🔔-托盘应用"><span>🔔 托盘应用</span></a></h3>
<p>系统托盘工具、后台服务监控、快捷工具栏</p>
<h3 id="🪟-透明窗口" tabindex="-1"><a class="header-anchor" href="#🪟-透明窗口"><span>🪟 透明窗口</span></a></h3>
<p>桌面小部件、悬浮工具、透明覆盖层</p>
<h3 id="💡-快速原型" tabindex="-1"><a class="header-anchor" href="#💡-快速原型"><span>💡 快速原型</span></a></h3>
<p>概念验证应用、MVP 开发、内部工具</p>
<h3 id="🎨-现代桌面" tabindex="-1"><a class="header-anchor" href="#🎨-现代桌面"><span>🎨 现代桌面</span></a></h3>
<p>需要 Web 技术的桌面应用、跨设备应用</p>
<hr>
<h2 id="快速开始" tabindex="-1"><a class="header-anchor" href="#快速开始"><span>快速开始</span></a></h2>
<p>```bash</p>
<h1 id="创建第一个应用" tabindex="-1"><a class="header-anchor" href="#创建第一个应用"><span>创建第一个应用</span></a></h1>
<p>mkdir MyApp
cd MyApp</p>
<h1 id="创建-index-html-文件" tabindex="-1"><a class="header-anchor" href="#创建-index-html-文件"><span>创建 index.html 文件</span></a></h1>
<p>echo '<h1>Hello Puppet!</h1>' &gt; index.html</p>
<h1 id="运行应用-裸文件夹模式" tabindex="-1"><a class="header-anchor" href="#运行应用-裸文件夹模式"><span>运行应用（裸文件夹模式）</span></a></h1>
<p>puppet.exe --nake-load .</p>
<h1 id="或创建-pup-文件" tabindex="-1"><a class="header-anchor" href="#或创建-pup-文件"><span>或创建 PUP 文件</span></a></h1>
<p>puppet.exe --create-pup -i . -o app.pup
puppet.exe --load-pup app.pup
```</p>
<h2 id="核心-api" tabindex="-1"><a class="header-anchor" href="#核心-api"><span>核心 API</span></a></h2>
<h3 id="窗口控制" tabindex="-1"><a class="header-anchor" href="#窗口控制"><span>窗口控制</span></a></h3>
<ul>
<li>无边框窗口、透明效果、点击穿透</li>
<li>窗口拖动、缩放、置顶</li>
</ul>
<h3 id="文件系统" tabindex="-1"><a class="header-anchor" href="#文件系统"><span>文件系统</span></a></h3>
<ul>
<li>文件选择对话框、文件读写、删除操作</li>
</ul>
<h3 id="系统功能" tabindex="-1"><a class="header-anchor" href="#系统功能"><span>系统功能</span></a></h3>
<ul>
<li>系统信息获取、截图、输入模拟</li>
</ul>
<h3 id="事件系统" tabindex="-1"><a class="header-anchor" href="#事件系统"><span>事件系统</span></a></h3>
<ul>
<li>USB 设备插拔、磁盘挂载、窗口事件</li>
</ul>
<h3 id="托盘图标" tabindex="-1"><a class="header-anchor" href="#托盘图标"><span>托盘图标</span></a></h3>
<ul>
<li>托盘菜单、气泡通知、自定义图标</li>
</ul>
<hr>
<div style="text-align: center; padding: 40px 20px 20px; border-top: 1px solid var(--c-border); margin-top: 60px; color: var(--c-text-light);">
  <p style="margin: 0 0 10px 0; font-size: 16px;">
    © 2024 Puppet Framework. All rights reserved.
  </p>
  <p style="margin: 0; font-size: 14px;">
    使用 <a href="https://vuepress.vuejs.org/" target="_blank" style="color: var(--c-brand);">VuePress</a> 和 
    <a href="https://theme-plume.vuejs.press/" target="_blank" style="color: var(--c-brand);">Plume 主题</a> 构建
  </p>
</div></div></template>


