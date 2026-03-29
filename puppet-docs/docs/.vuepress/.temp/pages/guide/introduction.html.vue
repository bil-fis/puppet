<template><div><p>Puppet 是一个创新的 Windows 桌面应用开发框架，它将 Web 技术的灵活性与原生 Windows 应用的强大功能完美结合。本章节将详细介绍 Puppet 框架的核心特性、设计理念以及适用场景。</p>
<h2 id="什么是-puppet" tabindex="-1"><a class="header-anchor" href="#什么是-puppet"><span>什么是 Puppet？</span></a></h2>
<p>Puppet 是一个基于 <strong>Microsoft WebView2</strong> 的桌面应用框架，允许开发者使用 HTML、CSS 和 JavaScript 等 Web 技术构建跨平台的桌面应用程序。与传统的 Electron 或 NW.js 不同，Puppet 专为 Windows 系统设计，提供了更深层次的系统集成和更轻量级的解决方案。</p>
<h3 id="设计理念" tabindex="-1"><a class="header-anchor" href="#设计理念"><span>设计理念</span></a></h3>
<p>Puppet 的设计遵循以下核心原则：</p>
<ol>
<li><strong>Web 优先</strong>：充分利用现代 Web 技术栈，降低开发门槛</li>
<li><strong>原生集成</strong>：提供对 Windows 系统功能的完整访问能力</li>
<li><strong>轻量高效</strong>：基于 .NET 和 WebView2，资源占用小，性能优秀</li>
<li><strong>安全可靠</strong>：内置多层安全机制，保护系统和用户数据</li>
<li><strong>简单易用</strong>：简洁的 API 设计，快速上手</li>
</ol>
<h2 id="核心特性" tabindex="-1"><a class="header-anchor" href="#核心特性"><span>核心特性</span></a></h2>
<h3 id="_1-web-技术栈" tabindex="-1"><a class="header-anchor" href="#_1-web-技术栈"><span>1. Web 技术栈</span></a></h3>
<p>Puppet 使用 Web 技术构建用户界面，这意味着：</p>
<ul>
<li><strong>熟悉的开发体验</strong>：使用 HTML、CSS、JavaScript 进行开发</li>
<li><strong>丰富的生态系统</strong>：可直接使用 npm 包、前端框架（Vue、React 等）</li>
<li><strong>现代 UI 设计</strong>：支持 CSS3、Flexbox、Grid 等现代布局技术</li>
<li><strong>响应式设计</strong>：轻松适配不同屏幕尺寸</li>
</ul>
<div class="hint-container tip">
<p class="hint-container-title">提示</p>
<p>Puppet 支持所有现代浏览器特性，因为其基于 Edge WebView2 渲染引擎。</p>
</div>
<h3 id="_2-原生系统集成" tabindex="-1"><a class="header-anchor" href="#_2-原生系统集成"><span>2. 原生系统集成</span></a></h3>
<p>Puppet 提供了对 Windows 系统功能的深度访问：</p>
<ul>
<li><strong>窗口管理</strong>：无边框窗口、透明效果、窗口拖动、点击穿透</li>
<li><strong>文件系统</strong>：文件读写、文件夹选择、路径操作</li>
<li><strong>系统信息</strong>：获取 CPU、内存、GPU、操作系统等信息</li>
<li><strong>输入模拟</strong>：模拟键盘和鼠标操作</li>
<li><strong>托盘图标</strong>：系统托盘集成、气泡通知、自定义菜单</li>
<li><strong>外部程序</strong>：执行系统命令、打开文件、启动应用</li>
</ul>
<h3 id="_3-事件驱动架构" tabindex="-1"><a class="header-anchor" href="#_3-事件驱动架构"><span>3. 事件驱动架构</span></a></h3>
<p>Puppet 提供强大的事件系统，支持：</p>
<ul>
<li><strong>USB 设备事件</strong>：USB 设备插拔监控</li>
<li><strong>磁盘事件</strong>：磁盘挂载和卸载监控</li>
<li><strong>窗口事件</strong>：窗口焦点、最大化、移动、缩放等</li>
<li><strong>电源事件</strong>：电源状态变化监控</li>
<li><strong>自定义事件</strong>：支持自定义事件监听和处理</li>
</ul>
<div class="hint-container info">
<p class="hint-container-title">技术细节</p>
<p>事件系统基于 Windows Management Instrumentation (WMI) 实现，提供了稳定可靠的设备监控能力。</p>
</div>
<h3 id="_4-安全机制" tabindex="-1"><a class="header-anchor" href="#_4-安全机制"><span>4. 安全机制</span></a></h3>
<p>Puppet 内置多层安全保护：</p>
<ul>
<li><strong>通信验证</strong>：使用运行时生成的随机密钥验证所有本地请求</li>
<li><strong>文件系统保护</strong>：自动阻止访问 Windows 系统文件夹</li>
<li><strong>权限确认</strong>：危险操作前弹出用户确认对话框</li>
<li><strong>路径规范化</strong>：防止路径遍历攻击</li>
<li><strong>PUP 加密</strong>：支持密码加密的 PUP 文件格式</li>
<li><strong>数字签名</strong>（V1.2）：支持基于 X.509 证书的数字签名，确保应用完整性和来源可信度</li>
<li><strong>数据库签名</strong>（V1.2）：支持数据库签名验证，防止数据被篡改</li>
</ul>
<h3 id="_5-独特的-pup-打包格式" tabindex="-1"><a class="header-anchor" href="#_5-独特的-pup-打包格式"><span>5. 独特的 PUP 打包格式</span></a></h3>
<p>Puppet 提供了独特的 <code v-pre>.pup</code> 文件格式：</p>
<ul>
<li><strong>自定义格式</strong>：结合 ZIP 和 AES 加密的专属格式</li>
<li><strong>密码保护</strong>：支持 AES-256 加密保护源代码</li>
<li><strong>单文件分发</strong>：所有资源打包为一个文件，便于分发</li>
<li><strong>快速加载</strong>：优化的文件结构和加载机制</li>
<li><strong>多版本支持</strong>：
<ul>
<li><strong>V1.0</strong>：基础功能，支持加密</li>
<li><strong>V1.1</strong>：支持启动脚本</li>
<li><strong>V1.2</strong>：支持数字签名和证书验证</li>
</ul>
</li>
</ul>
<h2 id="技术架构" tabindex="-1"><a class="header-anchor" href="#技术架构"><span>技术架构</span></a></h2>
<h3 id="整体架构" tabindex="-1"><a class="header-anchor" href="#整体架构"><span>整体架构</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>┌─────────────────────────────────────────┐</span></span>
<span class="line"><span>│         Web 应用层 (HTML/CSS/JS)        │</span></span>
<span class="line"><span>├─────────────────────────────────────────┤</span></span>
<span class="line"><span>│         JavaScript API (puppet.*)       │</span></span>
<span class="line"><span>├─────────────────────────────────────────┤</span></span>
<span class="line"><span>│         COM 桥接层                      │</span></span>
<span class="line"><span>├─────────────────────────────────────────┤</span></span>
<span class="line"><span>│         C# 控制器层                      │</span></span>
<span class="line"><span>│  ┌──────────────────────────────────┐  │</span></span>
<span class="line"><span>│  │  WindowController                │  │</span></span>
<span class="line"><span>│  │  FileSystemController            │  │</span></span>
<span class="line"><span>│  │  ApplicationController            │  │</span></span>
<span class="line"><span>│  │  SystemController                │  │</span></span>
<span class="line"><span>│  │  EventController                 │  │</span></span>
<span class="line"><span>│  │  LogController                   │  │</span></span>
<span class="line"><span>│  │  TrayController                  │  │</span></span>
<span class="line"><span>│  └──────────────────────────────────┘  │</span></span>
<span class="line"><span>├─────────────────────────────────────────┤</span></span>
<span class="line"><span>│         Windows Forms + WebView2        │</span></span>
<span class="line"><span>├─────────────────────────────────────────┤</span></span>
<span class="line"><span>│         Windows API / 系统服务          │</span></span>
<span class="line"><span>└─────────────────────────────────────────┘</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="数据流" tabindex="-1"><a class="header-anchor" href="#数据流"><span>数据流</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>用户操作 → JavaScript 代码 → COM 桥接 → C# 控制器 → Windows API / 系统服务</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><h3 id="核心组件" tabindex="-1"><a class="header-anchor" href="#核心组件"><span>核心组件</span></a></h3>
<ol>
<li><strong>WebView2 控件</strong>：渲染 Web 内容，提供浏览器功能</li>
<li><strong>COM 桥接</strong>：实现 JavaScript 与 C# 之间的双向通信</li>
<li><strong>HTTP 服务器</strong>：提供本地文件服务和 API 端点</li>
<li><strong>控制器</strong>：封装各种系统功能的 API</li>
<li><strong>PUP 服务器</strong>：解析和提供 PUP 文件内容</li>
</ol>
<h2 id="与其他框架的对比" tabindex="-1"><a class="header-anchor" href="#与其他框架的对比"><span>与其他框架的对比</span></a></h2>
<h3 id="vs-electron" tabindex="-1"><a class="header-anchor" href="#vs-electron"><span>vs Electron</span></a></h3>
<table>
<thead>
<tr>
<th>特性</th>
<th>Puppet</th>
<th>Electron</th>
</tr>
</thead>
<tbody>
<tr>
<td>基础架构</td>
<td>.NET + WebView2</td>
<td>Node.js + Chromium</td>
</tr>
<tr>
<td>应用体积</td>
<td>~50MB</td>
<td>~150MB+</td>
</tr>
<tr>
<td>内存占用</td>
<td>较低</td>
<td>较高</td>
</tr>
<tr>
<td>Windows 集成</td>
<td>深度</td>
<td>中等</td>
</tr>
<tr>
<td>跨平台</td>
<td>仅 Windows</td>
<td>跨平台</td>
</tr>
<tr>
<td>学习曲线</td>
<td>平缓</td>
<td>中等</td>
</tr>
<tr>
<td>性能</td>
<td>优秀</td>
<td>良好</td>
</tr>
</tbody>
</table>
<h3 id="vs-c-winforms-wpf" tabindex="-1"><a class="header-anchor" href="#vs-c-winforms-wpf"><span>vs C# WinForms/WPF</span></a></h3>
<table>
<thead>
<tr>
<th>特性</th>
<th>Puppet</th>
<th>WinForms/WPF</th>
</tr>
</thead>
<tbody>
<tr>
<td>开发语言</td>
<td>JavaScript/C#</td>
<td>C#</td>
</tr>
<tr>
<td>UI 技术</td>
<td>HTML/CSS</td>
<td>XAML/Designer</td>
</tr>
<tr>
<td>开发速度</td>
<td>快</td>
<td>中等</td>
</tr>
<tr>
<td>灵活性</td>
<td>高</td>
<td>中等</td>
</tr>
<tr>
<td>性能</td>
<td>优秀</td>
<td>优秀</td>
</tr>
<tr>
<td>Windows 集成</td>
<td>深度</td>
<td>深度</td>
</tr>
</tbody>
</table>
<h2 id="适用场景" tabindex="-1"><a class="header-anchor" href="#适用场景"><span>适用场景</span></a></h2>
<p>Puppet 特别适合以下应用场景：</p>
<h3 id="_1-设备管理工具" tabindex="-1"><a class="header-anchor" href="#_1-设备管理工具"><span>1. 设备管理工具</span></a></h3>
<ul>
<li>USB 设备管理器</li>
<li>磁盘管理工具</li>
<li>外设监控应用</li>
</ul>
<h3 id="_2-系统监控工具" tabindex="-1"><a class="header-anchor" href="#_2-系统监控工具"><span>2. 系统监控工具</span></a></h3>
<ul>
<li>系统资源监控</li>
<li>进程管理器</li>
<li>网络监控工具</li>
</ul>
<h3 id="_3-托盘应用" tabindex="-1"><a class="header-anchor" href="#_3-托盘应用"><span>3. 托盘应用</span></a></h3>
<ul>
<li>系统托盘工具</li>
<li>后台服务监控</li>
<li>快捷工具栏</li>
</ul>
<h3 id="_4-透明窗口应用" tabindex="-1"><a class="header-anchor" href="#_4-透明窗口应用"><span>4. 透明窗口应用</span></a></h3>
<ul>
<li>桌面小部件</li>
<li>悬浮工具</li>
<li>透明覆盖层</li>
</ul>
<h3 id="_5-快速原型开发" tabindex="-1"><a class="header-anchor" href="#_5-快速原型开发"><span>5. 快速原型开发</span></a></h3>
<ul>
<li>概念验证应用</li>
<li>MVP 开发</li>
<li>内部工具</li>
</ul>
<h3 id="_6-现代桌面应用" tabindex="-1"><a class="header-anchor" href="#_6-现代桌面应用"><span>6. 现代桌面应用</span></a></h3>
<ul>
<li>需要 Web 技术的桌面应用</li>
<li>需要系统功能的 Web 应用</li>
<li>跨设备应用</li>
</ul>
<h2 id="不适用场景" tabindex="-1"><a class="header-anchor" href="#不适用场景"><span>不适用场景</span></a></h2>
<p>以下场景可能不适合使用 Puppet：</p>
<ul>
<li>需要跨平台的应用（考虑 Electron）</li>
<li>纯计算密集型应用（考虑原生开发）</li>
<li>需要 GPU 加速的游戏（考虑 Unity/Unreal）</li>
<li>极度追求性能的系统工具（考虑 C++）</li>
</ul>
<h2 id="技术栈" tabindex="-1"><a class="header-anchor" href="#技术栈"><span>技术栈</span></a></h2>
<p>Puppet 框架基于以下技术构建：</p>
<h3 id="核心技术" tabindex="-1"><a class="header-anchor" href="#核心技术"><span>核心技术</span></a></h3>
<ul>
<li><strong>.NET 9.0</strong>：核心运行时框架</li>
<li><strong>Windows Forms</strong>：桌面 UI 框架</li>
<li><strong>Microsoft WebView2</strong>：Web 渲染引擎（基于 Edge）</li>
</ul>
<h3 id="第三方库" tabindex="-1"><a class="header-anchor" href="#第三方库"><span>第三方库</span></a></h3>
<ul>
<li><strong>Newtonsoft.Json</strong>：JSON 序列化/反序列化</li>
<li><strong>SharpZipLib</strong>：ZIP 文件操作（用于 PUP 格式）</li>
<li><strong>System.Management</strong>：WMI 设备监控</li>
</ul>
<h3 id="开发工具" tabindex="-1"><a class="header-anchor" href="#开发工具"><span>开发工具</span></a></h3>
<ul>
<li><strong>Visual Studio</strong>：推荐的开发 IDE</li>
<li><strong>MSBuild</strong>：构建工具</li>
<li><strong>NuGet</strong>：包管理器</li>
</ul>
<h2 id="限制和约束" tabindex="-1"><a class="header-anchor" href="#限制和约束"><span>限制和约束</span></a></h2>
<h3 id="平台限制" tabindex="-1"><a class="header-anchor" href="#平台限制"><span>平台限制</span></a></h3>
<ul>
<li><strong>操作系统</strong>：仅支持 Windows 10 及以上版本</li>
<li><strong>架构</strong>：支持 x64 和 x86 架构</li>
<li><strong>WebView2</strong>：需要安装 WebView2 运行时</li>
</ul>
<h3 id="功能限制" tabindex="-1"><a class="header-anchor" href="#功能限制"><span>功能限制</span></a></h3>
<ul>
<li><strong>跨平台</strong>：不支持 macOS 和 Linux</li>
<li><strong>移动端</strong>：不支持移动平台</li>
<li><strong>GPU 加速</strong>：有限的 GPU 加速支持</li>
<li><strong>原生模块</strong>：不支持 Node.js 原生模块</li>
</ul>
<h3 id="性能考虑" tabindex="-1"><a class="header-anchor" href="#性能考虑"><span>性能考虑</span></a></h3>
<ul>
<li><strong>大量计算</strong>：密集计算建议使用 C# 扩展</li>
<li><strong>内存管理</strong>：需要注意 JavaScript 内存泄漏</li>
<li><strong>DOM 操作</strong>：大量 DOM 操作可能影响性能</li>
</ul>
<h2 id="学习路径" tabindex="-1"><a class="header-anchor" href="#学习路径"><span>学习路径</span></a></h2>
<p>推荐按照以下路径学习 Puppet：</p>
<ol>
<li>
<p><strong>基础入门</strong></p>
<ul>
<li>阅读 <VPLink href="./getting-started.md">快速开始</VPLink></li>
<li>创建第一个应用</li>
<li>理解基本概念</li>
</ul>
</li>
<li>
<p><strong>核心功能</strong></p>
<ul>
<li>学习 <VPLink href="../api/window.md">窗口控制</VPLink></li>
<li>掌握 <VPLink href="../api/fs.md">文件系统</VPLink></li>
<li>了解 <VPLink href="../api/system.md">系统功能</VPLink></li>
</ul>
</li>
<li>
<p><strong>进阶功能</strong></p>
<ul>
<li>探索 <VPLink href="../api/events.md">事件系统</VPLink></li>
<li>使用 <VPLink href="../api/device.md">设备监控</VPLink></li>
<li>实现 <VPLink href="../api/tray.md">托盘图标</VPLink></li>
</ul>
</li>
<li>
<p><strong>高级主题</strong></p>
<ul>
<li>理解 <VPLink href="./architecture.md">架构设计</VPLink></li>
<li>学习 <VPLink href="./security.md">安全机制</VPLink></li>
<li>遵循 <VPLink href="./best-practices.md">最佳实践</VPLink></li>
</ul>
</li>
</ol>
<h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<h3 id="官方资源" tabindex="-1"><a class="header-anchor" href="#官方资源"><span>官方资源</span></a></h3>
<ul>
<li><a href="#">GitHub 仓库</a>：源代码和问题追踪</li>
<li><VPLink href="../api/">API 文档</VPLink>：完整的 API 参考</li>
<li><a href="#">示例代码</a>：实际应用示例</li>
</ul>
<h3 id="外部资源" tabindex="-1"><a class="header-anchor" href="#外部资源"><span>外部资源</span></a></h3>
<ul>
<li><a href="https://learn.microsoft.com/en-us/microsoft-edge/webview2/" target="_blank" rel="noopener noreferrer">Microsoft WebView2 文档</a>：WebView2 官方文档</li>
<li><a href="https://learn.microsoft.com/en-us/dotnet/" target="_blank" rel="noopener noreferrer">.NET 文档</a>：.NET 框架文档</li>
<li><a href="https://learn.microsoft.com/en-us/windows/win32/api/" target="_blank" rel="noopener noreferrer">Windows API 文档</a>：Windows API 参考</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>现在您已经了解了 Puppet 框架的基本情况，建议：</p>
<ol>
<li>阅读 <VPLink href="./getting-started.md">快速开始</VPLink> 创建您的第一个应用</li>
<li>了解 <VPLink href="./architecture.md">架构设计</VPLink> 深入理解框架原理</li>
<li>查看 <VPLink href="../api/">API 文档</VPLink> 开始实际开发</li>
<li>参考 <VPLink href="./best-practices.md">最佳实践</VPLink> 提升代码质量</li>
</ol>
</div></template>


