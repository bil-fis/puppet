<template><div><p>本章节介绍 Puppet 项目的标准目录结构和各个文件的作用。理解项目结构有助于您更好地组织和管理 Puppet 应用。</p>
<h2 id="标准项目结构" tabindex="-1"><a class="header-anchor" href="#标准项目结构"><span>标准项目结构</span></a></h2>
<p>一个典型的 Puppet 项目具有以下结构：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>MyPuppetProject/</span></span>
<span class="line"><span>├── index.html              # 主页面（入口文件）</span></span>
<span class="line"><span>├── css/                    # 样式文件目录</span></span>
<span class="line"><span>│   └── style.css          # 主样式表</span></span>
<span class="line"><span>├── js/                     # JavaScript 文件目录</span></span>
<span class="line"><span>│   └── app.js             # 主应用脚本</span></span>
<span class="line"><span>├── assets/                 # 资源文件目录</span></span>
<span class="line"><span>│   ├── images/            # 图片资源</span></span>
<span class="line"><span>│   ├── fonts/             # 字体文件</span></span>
<span class="line"><span>│   └── icons/             # 图标文件</span></span>
<span class="line"><span>├── lib/                    # 第三方库目录</span></span>
<span class="line"><span>│   └── vendor.js          # 第三方库文件</span></span>
<span class="line"><span>└── puppet.json             # 应用配置文件（可选）</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="文件说明" tabindex="-1"><a class="header-anchor" href="#文件说明"><span>文件说明</span></a></h2>
<h3 id="必需文件" tabindex="-1"><a class="header-anchor" href="#必需文件"><span>必需文件</span></a></h3>
<h4 id="index-html" tabindex="-1"><a class="header-anchor" href="#index-html"><span>index.html</span></a></h4>
<p>应用的主入口文件，通常包含：</p>
<div class="language-html line-numbers-mode" data-highlighter="shiki" data-ext="html" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-html"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;!</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">DOCTYPE</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> html</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">html</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> lang</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">zh-CN</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">head</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">meta</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> charset</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">UTF-8</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">meta</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> name</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">viewport</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> content</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">width=device-width, initial-scale=1.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">title</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">我的 Puppet 应用</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">title</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">link</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> rel</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">stylesheet</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> href</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">css/style.css</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">head</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">body</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    &#x3C;!-- 应用内容 --></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">div</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> id</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">app</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">        &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">h1</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">欢迎</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">h1</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">div</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    &#x3C;!-- 引入 JavaScript --></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">script</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> src</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">js/app.js</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">>&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">script</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">body</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">html</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>最佳实践</strong>：</p>
<ul>
<li>使用语义化的 HTML 标签</li>
<li>添加适当的 meta 标签</li>
<li>在页面底部加载 JavaScript 以提高性能</li>
<li>使用相对路径引用资源</li>
</ul>
<h3 id="推荐目录" tabindex="-1"><a class="header-anchor" href="#推荐目录"><span>推荐目录</span></a></h3>
<h4 id="css" tabindex="-1"><a class="header-anchor" href="#css"><span>css/</span></a></h4>
<p>存放所有样式文件：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>css/</span></span>
<span class="line"><span>├── style.css              # 主样式表</span></span>
<span class="line"><span>├── components/            # 组件样式</span></span>
<span class="line"><span>│   ├── button.css        # 按钮样式</span></span>
<span class="line"><span>│   └── dialog.css        # 对话框样式</span></span>
<span class="line"><span>└── themes/               # 主题样式</span></span>
<span class="line"><span>    └── dark.css          # 暗色主题</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>命名建议</strong>：</p>
<ul>
<li>使用小写字母和连字符（kebab-case）</li>
<li>主样式表命名为 <code v-pre>style.css</code></li>
<li>组件样式按功能分类</li>
</ul>
<h4 id="js" tabindex="-1"><a class="header-anchor" href="#js"><span>js/</span></a></h4>
<p>存放所有 JavaScript 文件：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>js/</span></span>
<span class="line"><span>├── app.js                 # 主应用脚本</span></span>
<span class="line"><span>├── utils/                 # 工具函数</span></span>
<span class="line"><span>│   ├── helpers.js        # 辅助函数</span></span>
<span class="line"><span>│   └── validators.js     # 验证函数</span></span>
<span class="line"><span>├── components/            # 组件脚本</span></span>
<span class="line"><span>│   ├── Button.js         # 按钮组件</span></span>
<span class="line"><span>│   └── Dialog.js         # 对话框组件</span></span>
<span class="line"><span>└── services/              # 服务层</span></span>
<span class="line"><span>    └── api.js            # API 封装</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>命名建议</strong>：</p>
<ul>
<li>使用 PascalCase 命名类和组件</li>
<li>使用 camelCase 命名函数和变量</li>
<li>按功能模块组织代码</li>
</ul>
<h4 id="assets" tabindex="-1"><a class="header-anchor" href="#assets"><span>assets/</span></a></h4>
<p>存放静态资源文件：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>assets/</span></span>
<span class="line"><span>├── images/               # 图片资源</span></span>
<span class="line"><span>│   ├── logo.png         # Logo</span></span>
<span class="line"><span>│   ├── icons/           # 图标</span></span>
<span class="line"><span>│   └── backgrounds/     # 背景图</span></span>
<span class="line"><span>├── fonts/               # 字体文件</span></span>
<span class="line"><span>│   └── font.ttf         # 字体文件</span></span>
<span class="line"><span>└── data/                # 数据文件</span></span>
<span class="line"><span>    └── config.json      # 配置数据</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>最佳实践</strong>：</p>
<ul>
<li>压缩图片以减少文件大小</li>
<li>使用适当的图片格式（PNG、JPG、SVG）</li>
<li>为字体文件添加 Web 格式支持</li>
</ul>
<h4 id="lib" tabindex="-1"><a class="header-anchor" href="#lib"><span>lib/</span></a></h4>
<p>存放第三方库文件：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>lib/</span></span>
<span class="line"><span>├── vue.min.js           # Vue.js</span></span>
<span class="line"><span>├── react.min.js         # React</span></span>
<span class="line"><span>└── axios.min.js         # Axios</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>建议</strong>：</p>
<ul>
<li>使用 CDN 引入常见库以减少项目体积</li>
<li>或使用 npm 安装并打包</li>
<li>保留库的版本信息</li>
</ul>
<h3 id="可选文件" tabindex="-1"><a class="header-anchor" href="#可选文件"><span>可选文件</span></a></h3>
<h4 id="puppet-json" tabindex="-1"><a class="header-anchor" href="#puppet-json"><span>puppet.json</span></a></h4>
<p>应用的配置文件，用于存储运行时配置：</p>
<div class="language-json line-numbers-mode" data-highlighter="shiki" data-ext="json" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-json"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">  "</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">appName</span><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">My App</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">  "</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">version</span><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">1.0.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">  "</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">settings</span><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">    "</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">theme</span><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">dark</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">    "</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">language</span><span style="--shiki-light:#99841877;--shiki-dark:#B8A96577">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">zh-CN</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">  }</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p>使用方式：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 读取配置</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> config</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Application</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">getConfig</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">appName</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 写入配置</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Application</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setConfig</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">theme</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">light</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="favicon-ico" tabindex="-1"><a class="header-anchor" href="#favicon-ico"><span>favicon.ico</span></a></h4>
<p>应用图标，Puppet 会自动使用网站的 favicon 作为窗口图标。</p>
<p><strong>推荐尺寸</strong>：</p>
<ul>
<li>16x16（小图标）</li>
<li>32x32（标准图标）</li>
<li>48x48（大图标）</li>
<li>256x256（高分辨率）</li>
</ul>
<h2 id="框架项目结构" tabindex="-1"><a class="header-anchor" href="#框架项目结构"><span>框架项目结构</span></a></h2>
<p>Puppet 框架本身的项目结构：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>puppet/</span></span>
<span class="line"><span>├── Program.cs                 # 应用入口</span></span>
<span class="line"><span>├── Form1.cs                   # 主窗口</span></span>
<span class="line"><span>├── Form1.Designer.cs          # 窗口设计器生成</span></span>
<span class="line"><span>├── Form1.resx                 # 资源文件</span></span>
<span class="line"><span>├── PupServer.cs               # PUP 服务器</span></span>
<span class="line"><span>├── PupCreator.cs              # PUP 创建器</span></span>
<span class="line"><span>├── AesHelper.cs               # 加密工具</span></span>
<span class="line"><span>├── SecretKey.cs               # 密钥管理</span></span>
<span class="line"><span>├── IniReader.cs               # INI 读取器</span></span>
<span class="line"><span>├── PortSelector.cs            # 端口选择器</span></span>
<span class="line"><span>├── PermissionDialog.cs        # 权限对话框</span></span>
<span class="line"><span>├── puppet.ini                 # 框架配置</span></span>
<span class="line"><span>├── puppet.csproj              # 项目文件</span></span>
<span class="line"><span>├── Controllers/               # 控制器目录</span></span>
<span class="line"><span>│   ├── ApplicationController.cs</span></span>
<span class="line"><span>│   ├── FileSystemController.cs</span></span>
<span class="line"><span>│   ├── WindowController.cs</span></span>
<span class="line"><span>│   ├── EventController.cs</span></span>
<span class="line"><span>│   ├── LogController.cs</span></span>
<span class="line"><span>│   ├── SystemController.cs</span></span>
<span class="line"><span>│   └── TrayController.cs</span></span>
<span class="line"><span>└── test-htmls/                # 测试页面</span></span>
<span class="line"><span>    ├── index.html</span></span>
<span class="line"><span>    ├── event-test.html</span></span>
<span class="line"><span>    └── device-test.html</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="配置文件" tabindex="-1"><a class="header-anchor" href="#配置文件"><span>配置文件</span></a></h2>
<h3 id="puppet-ini" tabindex="-1"><a class="header-anchor" href="#puppet-ini"><span>puppet.ini</span></a></h3>
<p>框架的配置文件，通常位于 Puppet.exe 同级目录：</p>
<div class="language-ini line-numbers-mode" data-highlighter="shiki" data-ext="ini" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-ini"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 要加载的 PUP 文件路径</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">app.pup</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">server</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 服务器端口（默认自动选择）</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">port</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">7738</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">security</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 是否启用严格模式</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">strict</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">true</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>用途</strong>：</p>
<ul>
<li>指定默认加载的 PUP 文件</li>
<li>配置服务器端口</li>
<li>设置安全选项</li>
</ul>
<h3 id="puppet-csproj" tabindex="-1"><a class="header-anchor" href="#puppet-csproj"><span>puppet.csproj</span></a></h3>
<p>.NET 项目文件，定义项目配置：</p>
<div class="language-xml line-numbers-mode" data-highlighter="shiki" data-ext="xml" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-xml"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">Project</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Sdk</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">Microsoft.NET.Sdk</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">  &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">PropertyGroup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">OutputType</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">WinExe</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">OutputType</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">TargetFramework</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">net9.0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">TargetFramework</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">UseWindowsForms</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">UseWindowsForms</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">  &#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">PropertyGroup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">  &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">ItemGroup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">PackageReference</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Include</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">Microsoft.Web.WebView2</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Version</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">1.0.1264.42</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> /></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">PackageReference</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Include</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">Newtonsoft.Json</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Version</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">13.0.3</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> /></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    &#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">PackageReference</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Include</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">SharpZipLib</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Version</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">1.3.3</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> /></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">  &#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">ItemGroup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;/</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">Project</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="文件命名约定" tabindex="-1"><a class="header-anchor" href="#文件命名约定"><span>文件命名约定</span></a></h2>
<h3 id="html-文件" tabindex="-1"><a class="header-anchor" href="#html-文件"><span>HTML 文件</span></a></h3>
<ul>
<li>使用小写字母和连字符（kebab-case）</li>
<li>主页面命名为 <code v-pre>index.html</code></li>
<li>其他页面按功能命名</li>
</ul>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>index.html           # 主页面</span></span>
<span class="line"><span>about.html           # 关于页面</span></span>
<span class="line"><span>settings.html        # 设置页面</span></span>
<span class="line"><span>user-profile.html    # 用户资料页面</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="css-文件" tabindex="-1"><a class="header-anchor" href="#css-文件"><span>CSS 文件</span></a></h3>
<ul>
<li>使用小写字母和连字符（kebab-case）</li>
<li>主样式表命名为 <code v-pre>style.css</code></li>
<li>组件样式按组件命名</li>
</ul>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>style.css            # 主样式表</span></span>
<span class="line"><span>button.css           # 按钮样式</span></span>
<span class="line"><span>modal.css            # 模态框样式</span></span>
<span class="line"><span>sidebar.css          # 侧边栏样式</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="javascript-文件" tabindex="-1"><a class="header-anchor" href="#javascript-文件"><span>JavaScript 文件</span></a></h3>
<ul>
<li>使用小写字母和连字符（kebab-case）</li>
<li>主脚本命名为 <code v-pre>app.js</code></li>
<li>组件脚本按组件命名</li>
</ul>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>app.js               # 主应用脚本</span></span>
<span class="line"><span>button.js            # 按钮组件</span></span>
<span class="line"><span>modal.js             # 模态框组件</span></span>
<span class="line"><span>utils.js             # 工具函数</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="资源文件" tabindex="-1"><a class="header-anchor" href="#资源文件"><span>资源文件</span></a></h3>
<ul>
<li>图片：使用小写字母和连字符</li>
<li>字体：保持原始文件名</li>
<li>图标：描述性命名</li>
</ul>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>logo.png             # Logo</span></span>
<span class="line"><span>background-image.png # 背景图</span></span>
<span class="line"><span>icon-close.png       # 关闭图标</span></span>
<span class="line"><span>roboto-regular.ttf   # 字体文件</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="组织策略" tabindex="-1"><a class="header-anchor" href="#组织策略"><span>组织策略</span></a></h2>
<h3 id="按功能组织" tabindex="-1"><a class="header-anchor" href="#按功能组织"><span>按功能组织</span></a></h3>
<p>适合大型项目，每个功能模块独立：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>project/</span></span>
<span class="line"><span>├── index.html</span></span>
<span class="line"><span>├── modules/</span></span>
<span class="line"><span>│   ├── user/</span></span>
<span class="line"><span>│   │   ├── user.html</span></span>
<span class="line"><span>│   │   ├── user.css</span></span>
<span class="line"><span>│   │   └── user.js</span></span>
<span class="line"><span>│   └── settings/</span></span>
<span class="line"><span>│       ├── settings.html</span></span>
<span class="line"><span>│       ├── settings.css</span></span>
<span class="line"><span>│       └── settings.js</span></span>
<span class="line"><span>└── shared/</span></span>
<span class="line"><span>    ├── css/</span></span>
<span class="line"><span>    ├── js/</span></span>
<span class="line"><span>    └── assets/</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="按类型组织" tabindex="-1"><a class="header-anchor" href="#按类型组织"><span>按类型组织</span></a></h3>
<p>适合中小型项目，文件按类型分类：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>project/</span></span>
<span class="line"><span>├── index.html</span></span>
<span class="line"><span>├── pages/              # 所有页面</span></span>
<span class="line"><span>│   ├── about.html</span></span>
<span class="line"><span>│   └── settings.html</span></span>
<span class="line"><span>├── css/                # 所有样式</span></span>
<span class="line"><span>│   └── style.css</span></span>
<span class="line"><span>├── js/                 # 所有脚本</span></span>
<span class="line"><span>│   └── app.js</span></span>
<span class="line"><span>└── assets/             # 所有资源</span></span>
<span class="line"><span>    ├── images/</span></span>
<span class="line"><span>    └── fonts/</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="混合组织" tabindex="-1"><a class="header-anchor" href="#混合组织"><span>混合组织</span></a></h3>
<p>结合两种方式的优点：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>project/</span></span>
<span class="line"><span>├── index.html</span></span>
<span class="line"><span>├── core/               # 核心文件</span></span>
<span class="line"><span>│   ├── css/</span></span>
<span class="line"><span>│   ├── js/</span></span>
<span class="line"><span>│   └── assets/</span></span>
<span class="line"><span>├── features/           # 功能模块</span></span>
<span class="line"><span>│   ├── user/</span></span>
<span class="line"><span>│   └── admin/</span></span>
<span class="line"><span>└── shared/             # 共享资源</span></span>
<span class="line"><span>    ├── components/</span></span>
<span class="line"><span>    └── utils/</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="版本控制" tabindex="-1"><a class="header-anchor" href="#版本控制"><span>版本控制</span></a></h2>
<h3 id="gitignore" tabindex="-1"><a class="header-anchor" href="#gitignore"><span>.gitignore</span></a></h3>
<p>推荐使用以下 <code v-pre>.gitignore</code> 配置：</p>
<div class="language-gitignore line-numbers-mode" data-highlighter="shiki" data-ext="gitignore" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-gitignore"><span class="line"><span># 依赖</span></span>
<span class="line"><span>node_modules/</span></span>
<span class="line"><span>lib/vendor.js</span></span>
<span class="line"><span></span></span>
<span class="line"><span># 构建输出</span></span>
<span class="line"><span>dist/</span></span>
<span class="line"><span>build/</span></span>
<span class="line"><span>*.pup</span></span>
<span class="line"><span></span></span>
<span class="line"><span># IDE</span></span>
<span class="line"><span>.vs/</span></span>
<span class="line"><span>.idea/</span></span>
<span class="line"><span>*.suo</span></span>
<span class="line"><span>*.user</span></span>
<span class="line"><span></span></span>
<span class="line"><span># 系统文件</span></span>
<span class="line"><span>.DS_Store</span></span>
<span class="line"><span>Thumbs.db</span></span>
<span class="line"><span></span></span>
<span class="line"><span># 日志</span></span>
<span class="line"><span>*.log</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="文件提交策略" tabindex="-1"><a class="header-anchor" href="#文件提交策略"><span>文件提交策略</span></a></h3>
<ul>
<li>提交源代码（HTML、CSS、JS）</li>
<li>提交配置文件（JSON、INI）</li>
<li>不提交编译输出（.pup 文件）</li>
<li>不提交依赖（node_modules）</li>
</ul>
<h2 id="打包和分发" tabindex="-1"><a class="header-anchor" href="#打包和分发"><span>打包和分发</span></a></h2>
<h3 id="pup-文件生成" tabindex="-1"><a class="header-anchor" href="#pup-文件生成"><span>PUP 文件生成</span></a></h3>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="分发包结构" tabindex="-1"><a class="header-anchor" href="#分发包结构"><span>分发包结构</span></a></h3>
<p>推荐的分发包结构：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>MyApp/</span></span>
<span class="line"><span>├── MyApp.pup            # PUP 文件</span></span>
<span class="line"><span>├── puppet.exe           # Puppet 运行时（可选）</span></span>
<span class="line"><span>├── README.txt           # 使用说明</span></span>
<span class="line"><span>└── LICENSE.txt          # 许可证</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="最佳实践" tabindex="-1"><a class="header-anchor" href="#最佳实践"><span>最佳实践</span></a></h2>
<h3 id="_1-目录结构" tabindex="-1"><a class="header-anchor" href="#_1-目录结构"><span>1. 目录结构</span></a></h3>
<ul>
<li>保持结构清晰和一致性</li>
<li>使用有意义的文件夹名称</li>
<li>避免过深的目录层级</li>
</ul>
<h3 id="_2-文件命名" tabindex="-1"><a class="header-anchor" href="#_2-文件命名"><span>2. 文件命名</span></a></h3>
<ul>
<li>使用一致的命名约定</li>
<li>名称应描述文件内容</li>
<li>避免特殊字符和空格</li>
</ul>
<h3 id="_3-代码组织" tabindex="-1"><a class="header-anchor" href="#_3-代码组织"><span>3. 代码组织</span></a></h3>
<ul>
<li>将相关代码放在一起</li>
<li>使用注释说明复杂逻辑</li>
<li>保持文件大小合理（建议 &lt; 500 行）</li>
</ul>
<h3 id="_4-资源管理" tabindex="-1"><a class="header-anchor" href="#_4-资源管理"><span>4. 资源管理</span></a></h3>
<ul>
<li>压缩图片和媒体文件</li>
<li>使用 CDN 加载第三方库</li>
<li>缓存常用资源</li>
</ul>
<h3 id="_5-配置管理" tabindex="-1"><a class="header-anchor" href="#_5-配置管理"><span>5. 配置管理</span></a></h3>
<ul>
<li>将配置与代码分离</li>
<li>使用 JSON 格式存储配置</li>
<li>提供默认配置值</li>
</ul>
<h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><VPLink href="./pup-format.md">PUP 文件格式</VPLink> - 了解 PUP 打包格式</li>
<li><VPLink href="./cli-parameters.md">命令行参数</VPLink> - 命令行工具使用说明</li>
<li><VPLink href="./best-practices.md">最佳实践</VPLink> - 开发建议和技巧</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>理解项目结构后，建议：</p>
<ol>
<li>创建您的第一个项目</li>
<li>学习 <VPLink href="../api/">API 文档</VPLink> 开始开发</li>
<li>参考 <VPLink href="./best-practices.md">最佳实践</VPLink> 提升代码质量</li>
</ol>
</div></template>


