<template><div><p>Puppet 框架提供了一套完整的 JavaScript API，允许您在 Web 应用中访问 Windows 系统功能。所有 API 都通过 <code v-pre>puppet</code> 全局对象访问。</p>
<h2 id="命名空间" tabindex="-1"><a class="header-anchor" href="#命名空间"><span>命名空间</span></a></h2>
<p>Puppet API 按功能分为以下几个命名空间：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">     // 窗口控制</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">application</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"> // 应用控制</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fs</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">         // 文件系统</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">        // 日志</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">system</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">     // 系统功能</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">tray</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">       // 托盘图标</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">events</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">     // 事件系统</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">device</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">     // 设备系统</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="api-特性" tabindex="-1"><a class="header-anchor" href="#api-特性"><span>API 特性</span></a></h2>
<h3 id="异步操作" tabindex="-1"><a class="header-anchor" href="#异步操作"><span>异步操作</span></a></h3>
<p>大多数 API 方法都是异步的，返回 Promise 对象。</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 异步调用</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setBorderless</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 使用 Promise</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setBorderless</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    .</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">then</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(()</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =></span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">窗口已设置为无边框</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">))</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    .</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">catch</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">err</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =></span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">操作失败:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> err</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="错误处理" tabindex="-1"><a class="header-anchor" href="#错误处理"><span>错误处理</span></a></h3>
<p>所有 API 方法都可能抛出异常，建议使用 try-catch 捕获。</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">try</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fs</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">readFileAsText</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">config.json</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> catch</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">读取文件失败:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文件读取失败</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="类型支持" tabindex="-1"><a class="header-anchor" href="#类型支持"><span>类型支持</span></a></h3>
<p>API 方法支持多种数据类型：</p>
<ul>
<li><strong>字符串</strong>：文件路径、配置值等</li>
<li><strong>数字</strong>：窗口位置、大小、透明度等</li>
<li><strong>布尔值</strong>：开关选项、状态标志等</li>
<li><strong>对象</strong>：配置对象、事件数据等</li>
<li><strong>数组</strong>：文件列表、菜单项等</li>
</ul>
<h2 id="快速开始" tabindex="-1"><a class="header-anchor" href="#快速开始"><span>快速开始</span></a></h2>
<h3 id="窗口控制" tabindex="-1"><a class="header-anchor" href="#窗口控制"><span>窗口控制</span></a></h3>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 设置无边框窗口</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setBorderless</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 设置窗口透明度</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setOpacity</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91">0.9</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 居中窗口</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">centerWindow</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="文件操作" tabindex="-1"><a class="header-anchor" href="#文件操作"><span>文件操作</span></a></h3>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 打开文件选择对话框</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> files</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fs</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">openFileDialog</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    [</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文本文件</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">*.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">],</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    false</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 读取文件内容</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> content</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fs</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">readFileAsText</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">files</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91">0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 写入文件</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fs</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">writeTextToFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">output.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> content</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="系统信息" tabindex="-1"><a class="header-anchor" href="#系统信息"><span>系统信息</span></a></h3>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 获取系统信息</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> sysInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">system</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">getSystemInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">操作系统:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> sysInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">osName</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">CPU:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> sysInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">cpuModel</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">内存:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> sysInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">totalMemory</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 截图</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> screenshot</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">system</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">takeScreenShot</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="事件监听" tabindex="-1"><a class="header-anchor" href="#事件监听"><span>事件监听</span></a></h3>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 监听 USB 设备插入</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">events</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">addEventListener</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">usb-plug-in</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">event</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =></span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">USB 设备插入:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> event</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">data</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">info</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">USB 设备已插入</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">});</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 监听窗口最大化</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">events</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">addEventListener</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">window-maximize</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> ()</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =></span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">窗口已最大化</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">});</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="api-命名约定" tabindex="-1"><a class="header-anchor" href="#api-命名约定"><span>API 命名约定</span></a></h2>
<h3 id="方法命名" tabindex="-1"><a class="header-anchor" href="#方法命名"><span>方法命名</span></a></h3>
<ul>
<li>
<p><strong>set</strong>：设置属性或状态</p>
<ul>
<li><code v-pre>setBorderless()</code></li>
<li><code v-pre>setOpacity()</code></li>
<li><code v-pre>setMenu()</code></li>
</ul>
</li>
<li>
<p><strong>get</strong>：获取属性或信息</p>
<ul>
<li><code v-pre>getWindowInfo()</code></li>
<li><code v-pre>getSystemInfo()</code></li>
<li><code v-pre>getDevices()</code></li>
</ul>
</li>
<li>
<p><strong>is</strong>：查询状态</p>
<ul>
<li><code v-pre>exists()</code></li>
</ul>
</li>
<li>
<p><strong>open</strong>：打开对话框</p>
<ul>
<li><code v-pre>openFileDialog()</code></li>
<li><code v-pre>openFolderDialog()</code></li>
</ul>
</li>
</ul>
<h3 id="事件命名" tabindex="-1"><a class="header-anchor" href="#事件命名"><span>事件命名</span></a></h3>
<p>事件名称使用 kebab-case 格式：</p>
<ul>
<li><code v-pre>usb-plug-in</code>：USB 设备插入</li>
<li><code v-pre>usb-plug-out</code>：USB 设备拔出</li>
<li><code v-pre>window-maximize</code>：窗口最大化</li>
<li><code v-pre>disk-mount</code>：磁盘挂载</li>
</ul>
<h2 id="api-参考" tabindex="-1"><a class="header-anchor" href="#api-参考"><span>API 参考</span></a></h2>
<h3 id="窗口控制-api" tabindex="-1"><a class="header-anchor" href="#窗口控制-api"><span>窗口控制 API</span></a></h3>
<p>提供窗口管理功能，包括样式控制、位置调整、透明效果等。</p>
<ul>
<li><VPLink href="./window.md">窗口控制 API 详细文档</VPLink></li>
</ul>
<h3 id="应用控制-api" tabindex="-1"><a class="header-anchor" href="#应用控制-api"><span>应用控制 API</span></a></h3>
<p>提供应用生命周期管理和外部程序执行功能。</p>
<ul>
<li><VPLink href="./application.md">应用控制 API 详细文档</VPLink></li>
</ul>
<h3 id="文件系统-api" tabindex="-1"><a class="header-anchor" href="#文件系统-api"><span>文件系统 API</span></a></h3>
<p>提供文件和文件夹操作功能，包括读写、选择、删除等。</p>
<ul>
<li><VPLink href="./fs.md">文件系统 API 详细文档</VPLink></li>
</ul>
<h3 id="日志-api" tabindex="-1"><a class="header-anchor" href="#日志-api"><span>日志 API</span></a></h3>
<p>提供日志输出功能，用于调试和错误追踪。</p>
<ul>
<li><VPLink href="./log.md">日志 API 详细文档</VPLink></li>
</ul>
<h3 id="系统-api" tabindex="-1"><a class="header-anchor" href="#系统-api"><span>系统 API</span></a></h3>
<p>提供系统信息获取、输入模拟、截图等功能。</p>
<ul>
<li><VPLink href="./system.md">系统 API 详细文档</VPLink></li>
</ul>
<h3 id="托盘图标-api" tabindex="-1"><a class="header-anchor" href="#托盘图标-api"><span>托盘图标 API</span></a></h3>
<p>提供系统托盘图标管理功能，包括菜单、通知等。</p>
<ul>
<li><VPLink href="./tray.md">托盘图标 API 详细文档</VPLink></li>
</ul>
<h3 id="事件系统-api" tabindex="-1"><a class="header-anchor" href="#事件系统-api"><span>事件系统 API</span></a></h3>
<p>提供事件监听和处理功能，支持设备和窗口事件。</p>
<ul>
<li><VPLink href="./events.md">事件系统 API 详细文档</VPLink></li>
</ul>
<h3 id="设备系统-api" tabindex="-1"><a class="header-anchor" href="#设备系统-api"><span>设备系统 API</span></a></h3>
<p>提供设备查询和监控功能。</p>
<ul>
<li><VPLink href="./device.md">设备系统 API 详细文档</VPLink></li>
</ul>
<h2 id="版本兼容性" tabindex="-1"><a class="header-anchor" href="#版本兼容性"><span>版本兼容性</span></a></h2>
<p>当前 API 版本：v1.0</p>
<h3 id="向后兼容性" tabindex="-1"><a class="header-anchor" href="#向后兼容性"><span>向后兼容性</span></a></h3>
<p>Puppet 框架承诺在主版本号不变的情况下保持 API 向后兼容。</p>
<h3 id="废弃的-api" tabindex="-1"><a class="header-anchor" href="#废弃的-api"><span>废弃的 API</span></a></h3>
<p>如果某个 API 被废弃，会在文档中明确标注，并提供替代方案。</p>
<h2 id="限制和约束" tabindex="-1"><a class="header-anchor" href="#限制和约束"><span>限制和约束</span></a></h2>
<h3 id="权限限制" tabindex="-1"><a class="header-anchor" href="#权限限制"><span>权限限制</span></a></h3>
<ul>
<li>无法访问 Windows 系统目录</li>
<li>无法直接执行某些系统命令（需要用户确认）</li>
</ul>
<h3 id="性能限制" tabindex="-1"><a class="header-anchor" href="#性能限制"><span>性能限制</span></a></h3>
<ul>
<li>大文件读写可能影响性能</li>
<li>频繁的事件监听可能占用资源</li>
</ul>
<h3 id="平台限制" tabindex="-1"><a class="header-anchor" href="#平台限制"><span>平台限制</span></a></h3>
<ul>
<li>仅支持 Windows 10 及以上版本</li>
<li>部分功能需要管理员权限</li>
</ul>
<h2 id="安全考虑" tabindex="-1"><a class="header-anchor" href="#安全考虑"><span>安全考虑</span></a></h2>
<h3 id="输入验证" tabindex="-1"><a class="header-anchor" href="#输入验证"><span>输入验证</span></a></h3>
<p>所有用户输入都应该进行验证：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 验证文件路径</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">!</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">path</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> ||</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> typeof</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> path</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> !==</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">string</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    throw</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> Error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">无效的路径</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="错误处理-1" tabindex="-1"><a class="header-anchor" href="#错误处理-1"><span>错误处理</span></a></h3>
<p>始终处理可能的错误：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">try</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    await</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> riskyOperation</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> catch</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">操作失败:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    showError</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">操作失败，请重试</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="敏感数据" tabindex="-1"><a class="header-anchor" href="#敏感数据"><span>敏感数据</span></a></h3>
<p>不要在代码中硬编码敏感信息：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 不推荐</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> password</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">my-secret-password</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 推荐</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">const</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> password</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> await</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> loadPasswordFromConfig</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><a href="https://learn.microsoft.com/en-us/microsoft-edge/webview2/" target="_blank" rel="noopener noreferrer">Microsoft WebView2 文档</a>：WebView2 官方文档</li>
<li><a href="https://learn.microsoft.com/en-us/dotnet/" target="_blank" rel="noopener noreferrer">.NET 文档</a>：.NET 框架文档</li>
<li><a href="https://developer.mozilla.org/en-US/docs/Learn/JavaScript/Asynchronous" target="_blank" rel="noopener noreferrer">JavaScript 异步编程</a>：异步编程指南</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>建议按照以下顺序学习 API：</p>
<ol>
<li>了解 <VPLink href="./window.md">窗口控制 API</VPLink> 创建自定义窗口</li>
<li>学习 <VPLink href="./fs.md">文件系统 API</VPLink> 进行文件操作</li>
<li>探索 <VPLink href="./events.md">事件系统 API</VPLink> 实现事件处理</li>
<li>使用 <VPLink href="./system.md">系统 API</VPLink> 获取系统信息</li>
<li>参考 <VPLink href="./tray.md">托盘图标 API</VPLink> 创建托盘应用</li>
</ol>
</div></template>


