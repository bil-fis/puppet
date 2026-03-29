<template><div><p>本文档深入探讨 Puppet 框架的内部架构、核心组件以及它们之间的交互方式。理解这些内容有助于您更好地使用框架，并在需要时进行扩展和优化。</p>
<h2 id="整体架构" tabindex="-1"><a class="header-anchor" href="#整体架构"><span>整体架构</span></a></h2>
<p>Puppet 框架采用分层架构设计，每一层都有明确的职责和边界。</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                      用户应用层                              │</span></span>
<span class="line"><span>│                  (HTML/CSS/JavaScript)                       │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span>
<span class="line"><span>                              ↓</span></span>
<span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                    JavaScript API 层                         │</span></span>
<span class="line"><span>│              (window.puppet.* 命名空间)                       │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span>
<span class="line"><span>                              ↓</span></span>
<span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                      通信桥接层                              │</span></span>
<span class="line"><span>│              (COM Interop + WebMessage)                      │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span>
<span class="line"><span>                              ↓</span></span>
<span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                      控制器层                                │</span></span>
<span class="line"><span>│  ┌──────────┬──────────┬──────────┬──────────┬──────────┐   │</span></span>
<span class="line"><span>│  │ Window   │ File     │ App      │ System   │ Event    │   │</span></span>
<span class="line"><span>│  │ Controller│ System  │ Controller│ Controller│ Controller │   │</span></span>
<span class="line"><span>│  └──────────┴──────────┴──────────┴──────────┴──────────┘   │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span>
<span class="line"><span>                              ↓</span></span>
<span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                     平台适配层                               │</span></span>
<span class="line"><span>│              (Windows Forms + WebView2)                      │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span>
<span class="line"><span>                              ↓</span></span>
<span class="line"><span>┌─────────────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│                     Windows 系统层                           │</span></span>
<span class="line"><span>│              (Win32 API / WMI / .NET Framework)              │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────────────┘</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="核心组件详解" tabindex="-1"><a class="header-anchor" href="#核心组件详解"><span>核心组件详解</span></a></h2>
<h3 id="_1-应用入口-program-cs" tabindex="-1"><a class="header-anchor" href="#_1-应用入口-program-cs"><span>1. 应用入口（Program.cs）</span></a></h3>
<p><code v-pre>Program.cs</code> 是整个应用的入口点，负责：</p>
<ul>
<li><strong>安全初始化</strong>：生成和初始化通信密钥</li>
<li><strong>命令行处理</strong>：解析三种运行模式的命令行参数</li>
<li><strong>服务管理</strong>：创建和管理 PupServer 实例</li>
<li><strong>应用程序启动</strong>：启动 Windows Forms 应用程序</li>
</ul>
<h4 id="命令行参数处理" tabindex="-1"><a class="header-anchor" href="#命令行参数处理"><span>命令行参数处理</span></a></h4>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 支持三种运行模式</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">exe</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">                        // GUI 模式</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">exe</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> --</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">create</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">-</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">pup</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> -</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">i</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">folder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> -</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">o</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">pup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#999999;--shiki-dark:#666666"> [</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">-</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">p</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">password</span><span style="--shiki-light:#999999;--shiki-dark:#666666">>]</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 创建 PUP</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">exe</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> --</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">load</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">-</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">pup</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">pup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 加载 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">exe</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> --</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">nake</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">-</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">load</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">folder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">   // 加载裸文件夹</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><div class="hint-container info">
<p class="hint-container-title">详见文档</p>
<p>详细的命令行参数说明请参考 <VPLink href="./cli-parameters.md">命令行参数</VPLink> 文档。</p>
</div>
<h3 id="_2-主窗口-form1-cs" tabindex="-1"><a class="header-anchor" href="#_2-主窗口-form1-cs"><span>2. 主窗口（Form1.cs）</span></a></h3>
<p><code v-pre>Form1.cs</code> 是应用的主窗口，承载 WebView2 控件并协调各个组件。</p>
<h4 id="主要职责" tabindex="-1"><a class="header-anchor" href="#主要职责"><span>主要职责</span></a></h4>
<ul>
<li><strong>WebView2 初始化</strong>：配置和初始化 WebView2 控件</li>
<li><strong>JavaScript 注入</strong>：将所有控制器注入到 JavaScript 环境</li>
<li><strong>消息处理</strong>：处理来自 Web 层的消息和请求</li>
<li><strong>安全验证</strong>：验证所有传入请求的密钥</li>
<li><strong>窗口管理</strong>：处理窗口拖动、透明效果等</li>
<li><strong>图标管理</strong>：自动获取并设置窗口图标</li>
</ul>
<h4 id="关键代码片段" tabindex="-1"><a class="header-anchor" href="#关键代码片段"><span>关键代码片段</span></a></h4>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// WebView2 初始化</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">EnsureCoreWebView2Async</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">null</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 注入 JavaScript API</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">AddScriptToExecuteOnDocumentCreatedAsync</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">@"</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">    window.puppet = {</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">        window: new WindowControllerProxy(),</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">        application: new ApplicationControllerProxy(),</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">        fs: new FileSystemControllerProxy(),</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">        // ... 其他控制器</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">    };</span></span>
<span class="line"><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 处理 WebMessage</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">WebMessageReceived</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> +=</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> OnWebMessageReceived</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_3-控制器层-controllers" tabindex="-1"><a class="header-anchor" href="#_3-控制器层-controllers"><span>3. 控制器层（Controllers）</span></a></h3>
<p>控制器层是 Puppet 框架的核心，每个控制器负责一组相关的功能。</p>
<h4 id="控制器架构" tabindex="-1"><a class="header-anchor" href="#控制器架构"><span>控制器架构</span></a></h4>
<p>每个控制器都遵循相同的设计模式：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>控制器基类 (可选)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>具体控制器 (如 WindowController)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>JavaScript 代理类 (如 WindowControllerProxy)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>Web API 路由</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="控制器列表" tabindex="-1"><a class="header-anchor" href="#控制器列表"><span>控制器列表</span></a></h4>
<table>
<thead>
<tr>
<th>控制器</th>
<th>功能</th>
<th>文件</th>
</tr>
</thead>
<tbody>
<tr>
<td><code v-pre>ApplicationController</code></td>
<td>应用生命周期管理、外部程序执行</td>
<td><code v-pre>Controllers/ApplicationController.cs</code></td>
</tr>
<tr>
<td><code v-pre>FileSystemController</code></td>
<td>文件系统操作</td>
<td><code v-pre>Controllers/FileSystemController.cs</code></td>
</tr>
<tr>
<td><code v-pre>WindowController</code></td>
<td>窗口管理</td>
<td><code v-pre>Controllers/WindowController.cs</code></td>
</tr>
<tr>
<td><code v-pre>EventController</code></td>
<td>事件系统和设备监控</td>
<td><code v-pre>Controllers/EventController.cs</code></td>
</tr>
<tr>
<td><code v-pre>LogController</code></td>
<td>日志输出</td>
<td><code v-pre>Controllers/LogController.cs</code></td>
</tr>
<tr>
<td><code v-pre>SystemController</code></td>
<td>系统信息、输入模拟</td>
<td><code v-pre>Controllers/SystemController.cs</code></td>
</tr>
<tr>
<td><code v-pre>TrayController</code></td>
<td>托盘图标管理</td>
<td><code v-pre>Controllers/TrayController.cs</code></td>
</tr>
</tbody>
</table>
<h3 id="_4-pup-服务器-pupserver-cs" tabindex="-1"><a class="header-anchor" href="#_4-pup-服务器-pupserver-cs"><span>4. PUP 服务器（PupServer.cs）</span></a></h3>
<p><code v-pre>PupServer.cs</code> 是一个轻量级的 HTTP 服务器，负责提供 Web 内容。</p>
<h4 id="两种工作模式" tabindex="-1"><a class="header-anchor" href="#两种工作模式"><span>两种工作模式</span></a></h4>
<h5 id="pup-文件模式" tabindex="-1"><a class="header-anchor" href="#pup-文件模式"><span>PUP 文件模式</span></a></h5>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// PUP 文件结构</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">PUP</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> V1</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">0 </span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">标识头</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> 8 </span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">字节</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> +</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> [</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">AES</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> 加密的</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> ZIP</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> 密码</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> 32 </span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">字节</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> +</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> [</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">ZIP</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> 数据</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><ul>
<li>解析自定义的 PUP 文件格式</li>
<li>解密 ZIP 密钥</li>
<li>从内存中读取文件内容</li>
</ul>
<h5 id="裸文件夹模式" tabindex="-1"><a class="header-anchor" href="#裸文件夹模式"><span>裸文件夹模式</span></a></h5>
<ul>
<li>直接从文件系统提供文件</li>
<li>支持热重载（开发时使用）</li>
<li>自动检测文件变化</li>
</ul>
<h4 id="http-路由" tabindex="-1"><a class="header-anchor" href="#http-路由"><span>HTTP 路由</span></a></h4>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>/                        → index.html</span></span>
<span class="line"><span>/*.html                  → 静态 HTML 文件</span></span>
<span class="line"><span>/*.css                   → 静态 CSS 文件</span></span>
<span class="line"><span>/*.js                    → 静态 JavaScript 文件</span></span>
<span class="line"><span>/api/*                   → API 请求（转发到控制器）</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_5-辅助工具类" tabindex="-1"><a class="header-anchor" href="#_5-辅助工具类"><span>5. 辅助工具类</span></a></h3>
<h4 id="加密工具-aeshelper-cs" tabindex="-1"><a class="header-anchor" href="#加密工具-aeshelper-cs"><span>加密工具（AesHelper.cs）</span></a></h4>
<p>负责 PUP 文件的加密和解密：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 固定密钥用于加密 ZIP 密钥</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> FixedKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">ILOVEPUPPET</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">public</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> Encrypt</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> plainText</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> key</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">public static string Decrypt</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> cipherText</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> key</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="密钥管理-secretkey-cs" tabindex="-1"><a class="header-anchor" href="#密钥管理-secretkey-cs"><span>密钥管理（SecretKey.cs）</span></a></h4>
<p>生成和管理运行时密钥：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 生成随机密钥</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">public</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> GenerateKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">()</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 初始化并存储密钥</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">public static void Initialize</span><span style="--shiki-light:#999999;--shiki-dark:#666666">()</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="权限对话框-permissiondialog-cs" tabindex="-1"><a class="header-anchor" href="#权限对话框-permissiondialog-cs"><span>权限对话框（PermissionDialog.cs）</span></a></h4>
<p>自定义权限确认对话框：</p>
<ul>
<li>三种操作：允许、拒绝、永久阻止</li>
<li>记住用户选择</li>
<li>支持自定义消息</li>
</ul>
<h4 id="端口选择器-portselector-cs" tabindex="-1"><a class="header-anchor" href="#端口选择器-portselector-cs"><span>端口选择器（PortSelector.cs）</span></a></h4>
<p>自动选择可用端口：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 从 7738 开始，递增直到找到可用端口</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">public</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> int</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> SelectAvailablePort</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">int</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> startPort</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 7738</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="数据流详解" tabindex="-1"><a class="header-anchor" href="#数据流详解"><span>数据流详解</span></a></h2>
<h3 id="_1-javascript-到-c-的调用" tabindex="-1"><a class="header-anchor" href="#_1-javascript-到-c-的调用"><span>1. JavaScript 到 C# 的调用</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>用户操作 (如按钮点击)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>JavaScript 代码调用 puppet.window.setBorderless(true)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>WebMessage 发送到 C# 层</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>Form1.cs 接收消息</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>验证密钥</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>路由到 WindowController</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>调用 Windows API 修改窗口样式</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>返回结果到 JavaScript</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="代码示例" tabindex="-1"><a class="header-anchor" href="#代码示例"><span>代码示例</span></a></h4>
<p><strong>JavaScript 层</strong>：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 用户代码</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">setBorderless</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 内部实现</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">class</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> WindowControllerProxy</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    async</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> setBorderless</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">value</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> window</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">chrome</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">webview</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">postMessage</span><span style="--shiki-light:#999999;--shiki-dark:#666666">({</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">            controller</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">window</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">            action</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">setBorderless</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">            params</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> [</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">value</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">        });</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    }</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>C# 层</strong>：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// Form1.cs</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> async</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> void</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> OnWebMessageReceived</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">object</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> sender</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> CoreWebView2WebMessageReceivedEventArgs</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> message</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> JsonConvert</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">DeserializeObject</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">WebMessage</span><span style="--shiki-light:#999999;--shiki-dark:#666666">>(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">WebMessageAsJson</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 验证密钥</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Secret</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> !=</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> SecretKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Key</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 路由到控制器</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> result</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> await</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> RouteToController</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 返回结果</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">PostWebMessageAsJson</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">result</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_2-事件从-c-到-javascript" tabindex="-1"><a class="header-anchor" href="#_2-事件从-c-到-javascript"><span>2. 事件从 C# 到 JavaScript</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>系统事件 (如 USB 插入)</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>WMI 监控器检测到事件</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>EventController 处理事件</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>构造事件对象</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>通过 WebMessage 发送到 JavaScript</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>调用注册的回调函数</span></span>
<span class="line"><span>    ↓</span></span>
<span class="line"><span>用户代码执行</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="代码示例-1" tabindex="-1"><a class="header-anchor" href="#代码示例-1"><span>代码示例</span></a></h4>
<p><strong>C# 层</strong>：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// EventController.cs</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> void</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> OnUSBArrival</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">object</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> sender</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> EventArrivedEventArgs</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> device</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> ExtractDeviceInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">NewEvent</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> message</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        type</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">event</span><span style="--shiki-light:#999999;--shiki-dark:#666666">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        event</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">usb-plug-in</span><span style="--shiki-light:#999999;--shiki-dark:#666666">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        data</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> device</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    };</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">PostWebMessageAsJson</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">JsonConvert</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">SerializeObject</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>JavaScript 层</strong>：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 事件监听</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">events</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">addEventListener</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">usb-plug-in</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> function</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    console</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">USB 设备插入:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Device</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">});</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="安全机制" tabindex="-1"><a class="header-anchor" href="#安全机制"><span>安全机制</span></a></h2>
<h3 id="_1-通信安全" tabindex="-1"><a class="header-anchor" href="#_1-通信安全"><span>1. 通信安全</span></a></h3>
<p>所有 JavaScript 和 C# 之间的通信都经过密钥验证：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 生成随机密钥</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> secret</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> SecretKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GenerateKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// JavaScript 注入时包含密钥</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">AddScriptToExecuteOnDocumentCreatedAsync</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">@"</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">    window.PUPPET_SECRET = '</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> +</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> secret</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> +</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">';</span></span>
<span class="line"><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 接收消息时验证密钥</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Secret</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> !=</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> SecretKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Key</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    return</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"> // 拒绝请求</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_2-文件系统保护" tabindex="-1"><a class="header-anchor" href="#_2-文件系统保护"><span>2. 文件系统保护</span></a></h3>
<p>自动阻止访问系统敏感目录：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// FileSystemController.cs</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> ProtectedPaths</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetFolderPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">SpecialFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Windows</span><span style="--shiki-light:#999999;--shiki-dark:#666666">),</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetFolderPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">SpecialFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">System</span><span style="--shiki-light:#999999;--shiki-dark:#666666">),</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetFolderPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Environment</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">SpecialFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">SystemX86</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">};</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> bool</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> IsProtectedPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> path</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    foreach</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> protectedPath</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> in</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> ProtectedPaths</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    {</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">path</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">StartsWith</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">protectedPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> StringComparison</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">OrdinalIgnoreCase</span><span style="--shiki-light:#999999;--shiki-dark:#666666">))</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">            return</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    }</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    return</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> false</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_3-权限确认" tabindex="-1"><a class="header-anchor" href="#_3-权限确认"><span>3. 权限确认</span></a></h3>
<p>危险操作前弹出确认对话框：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 执行系统目录程序前确认</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">IsSystemPath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">command</span><span style="--shiki-light:#999999;--shiki-dark:#666666">))</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> result</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> PermissionDialog</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Show</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">执行系统程序</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">确定要执行此程序吗？</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">result</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> !=</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> DialogResult</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Yes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="性能优化" tabindex="-1"><a class="header-anchor" href="#性能优化"><span>性能优化</span></a></h2>
<h3 id="_1-webview2-优化" tabindex="-1"><a class="header-anchor" href="#_1-webview2-优化"><span>1. WebView2 优化</span></a></h3>
<ul>
<li><strong>禁用不必要的功能</strong>：关闭不需要的浏览器功能</li>
<li><strong>缓存管理</strong>：合理配置缓存策略</li>
<li><strong>进程隔离</strong>：使用单进程模式减少内存占用</li>
</ul>
<h3 id="_2-内存管理" tabindex="-1"><a class="header-anchor" href="#_2-内存管理"><span>2. 内存管理</span></a></h3>
<ul>
<li><strong>及时释放资源</strong>：使用 <code v-pre>using</code> 语句管理资源</li>
<li><strong>避免内存泄漏</strong>：正确处理事件订阅</li>
<li><strong>垃圾回收优化</strong>：减少不必要的对象创建</li>
</ul>
<h3 id="_3-文件操作优化" tabindex="-1"><a class="header-anchor" href="#_3-文件操作优化"><span>3. 文件操作优化</span></a></h3>
<ul>
<li><strong>异步 I/O</strong>：使用异步方法避免阻塞 UI 线程</li>
<li><strong>批量操作</strong>：合并多个文件操作</li>
<li><strong>缓存策略</strong>：缓存频繁访问的文件</li>
</ul>
<h2 id="扩展性" tabindex="-1"><a class="header-anchor" href="#扩展性"><span>扩展性</span></a></h2>
<h3 id="_1-添加新控制器" tabindex="-1"><a class="header-anchor" href="#_1-添加新控制器"><span>1. 添加新控制器</span></a></h3>
<p>要添加新的功能模块，可以创建新的控制器：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">public</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> class</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> MyFeatureController</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    public</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> async</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Task</span><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x3C;</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">></span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> MyMethod</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> param</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    {</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">        // 实现功能</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">result</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    }</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p>然后在 <code v-pre>Form1.cs</code> 中注册：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 创建控制器实例</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> myController</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> MyFeatureController</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 注入到 JavaScript</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">await</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">AddScriptToExecuteOnDocumentCreatedAsync</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">@"</span></span>
<span class="line"><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">    window.puppet.myFeature = new MyFeatureControllerProxy();</span></span>
<span class="line"><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_2-自定义事件" tabindex="-1"><a class="header-anchor" href="#_2-自定义事件"><span>2. 自定义事件</span></a></h3>
<p>可以扩展事件系统支持更多事件类型：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 在 EventController 中添加新事件</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> void</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> StartMyEventMonitoring</span><span style="--shiki-light:#999999;--shiki-dark:#666666">()</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 实现监控逻辑</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 触发事件</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> void</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> OnMyEvent</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">object</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> sender</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> EventArgs</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> e</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> message</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        type</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">event</span><span style="--shiki-light:#999999;--shiki-dark:#666666">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        event</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> '</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">my-event</span><span style="--shiki-light:#999999;--shiki-dark:#666666">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">        data</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"> /* 事件数据 */</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> }</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    };</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    webView21</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">CoreWebView2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">PostWebMessageAsJson</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">JsonConvert</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">SerializeObject</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">message</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="调试和监控" tabindex="-1"><a class="header-anchor" href="#调试和监控"><span>调试和监控</span></a></h2>
<h3 id="_1-日志系统" tabindex="-1"><a class="header-anchor" href="#_1-日志系统"><span>1. 日志系统</span></a></h3>
<p>使用 <code v-pre>LogController</code> 输出调试信息：</p>
<div class="language-javascript line-numbers-mode" data-highlighter="shiki" data-ext="javascript" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-javascript"><span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">info</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">调试信息</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">warn</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">警告信息</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">log</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">error</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">错误信息</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_2-开发者工具" tabindex="-1"><a class="header-anchor" href="#_2-开发者工具"><span>2. 开发者工具</span></a></h3>
<p>在 Puppet 应用中右键选择&quot;检查&quot;可以打开浏览器开发者工具：</p>
<ul>
<li>控制台：查看日志和错误</li>
<li>网络：监控 HTTP 请求</li>
<li>元素：检查和调试 DOM</li>
<li>源码：调试 JavaScript 代码</li>
</ul>
<h3 id="_3-性能分析" tabindex="-1"><a class="header-anchor" href="#_3-性能分析"><span>3. 性能分析</span></a></h3>
<p>使用开发者工具的性能面板：</p>
<ul>
<li>记录和分析性能</li>
<li>识别性能瓶颈</li>
<li>优化代码执行</li>
</ul>
<h2 id="最佳实践" tabindex="-1"><a class="header-anchor" href="#最佳实践"><span>最佳实践</span></a></h2>
<h3 id="_1-控制器设计" tabindex="-1"><a class="header-anchor" href="#_1-控制器设计"><span>1. 控制器设计</span></a></h3>
<ul>
<li><strong>单一职责</strong>：每个控制器只负责一组相关功能</li>
<li><strong>异步优先</strong>：使用异步方法避免阻塞</li>
<li><strong>错误处理</strong>：完善的异常处理和错误返回</li>
</ul>
<h3 id="_2-api-设计" tabindex="-1"><a class="header-anchor" href="#_2-api-设计"><span>2. API 设计</span></a></h3>
<ul>
<li><strong>一致性</strong>：保持 API 命名和用法的一致性</li>
<li><strong>可预测性</strong>：方法名应清晰表达其功能</li>
<li><strong>文档完善</strong>：提供详细的 API 文档</li>
</ul>
<h3 id="_3-安全考虑" tabindex="-1"><a class="header-anchor" href="#_3-安全考虑"><span>3. 安全考虑</span></a></h3>
<ul>
<li><strong>输入验证</strong>：验证所有用户输入</li>
<li><strong>路径规范化</strong>：防止路径遍历攻击</li>
<li><strong>权限检查</strong>：敏感操作需要权限确认</li>
</ul>
<h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><a href="https://learn.microsoft.com/en-us/microsoft-edge/webview2/" target="_blank" rel="noopener noreferrer">Microsoft WebView2 文档</a>：WebView2 官方文档</li>
<li><a href="https://learn.microsoft.com/en-us/dotnet/" target="_blank" rel="noopener noreferrer">.NET 文档</a>：.NET 框架文档</li>
<li><a href="https://learn.microsoft.com/en-us/windows/win32/api/" target="_blank" rel="noopener noreferrer">Windows API 文档</a>：Windows API 参考</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>理解架构后，建议：</p>
<ol>
<li>查看 <VPLink href="../api/">API 文档</VPLink> 了解具体用法</li>
<li>阅读 <VPLink href="./security.md">安全机制</VPLink> 了解安全细节</li>
<li>参考 <VPLink href="./best-practices.md">最佳实践</VPLink> 提升开发质量</li>
</ol>
</div></template>


