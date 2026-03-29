<template><div><p>本页面记录 Puppet 框架的所有版本更新和重要变更。</p>
<h2 id="版本规范" tabindex="-1"><a class="header-anchor" href="#版本规范"><span>版本规范</span></a></h2>
<p>Puppet 遵循语义化版本规范（Semantic Versioning）：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>主版本号.次版本号.修订号 (MAJOR.MINOR.PATCH)</span></span>
<span class="line"><span></span></span>
<span class="line"><span>示例：1.0.0</span></span>
<span class="line"><span>- 1：主版本号（不兼容的 API 修改）</span></span>
<span class="line"><span>- 0：次版本号（向下兼容的功能新增）</span></span>
<span class="line"><span>- 0：修订号（向下兼容的问题修正）</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="版本历史" tabindex="-1"><a class="header-anchor" href="#版本历史"><span>版本历史</span></a></h2>
<h3 id="v1-0-11-2026-03-29" tabindex="-1"><a class="header-anchor" href="#v1-0-11-2026-03-29"><span>v1.0.11 (2026-03-29)</span></a></h3>
<h4 id="新增功能" tabindex="-1"><a class="header-anchor" href="#新增功能"><span>新增功能</span></a></h4>
<ul>
<li>
<p><strong>PUP V1.2 格式</strong></p>
<ul>
<li>支持数字签名和证书验证</li>
<li>集成自签名 X.509 证书</li>
<li>数据库签名和验证功能</li>
<li>证书指纹验证</li>
<li>私钥加密存储（AES-256-GCM）</li>
<li>PBKDF2 密钥派生（100,000次迭代）</li>
</ul>
</li>
<li>
<p><strong>签名工具</strong></p>
<ul>
<li>新增 puppet-sign 独立签名工具</li>
<li>支持生成签名密钥对</li>
<li>支持数据库签名和验证</li>
<li>交互式和命令行两种模式</li>
</ul>
</li>
<li>
<p><strong>安全增强</strong></p>
<ul>
<li>SHA256withRSA 签名算法</li>
<li>RSA 2048/4096 位密钥支持</li>
<li>数据库完整性保护</li>
<li>签名验证失败警告机制</li>
</ul>
</li>
<li>
<p><strong>核心框架</strong></p>
<ul>
<li>基于 WebView2 的桌面应用框架</li>
<li>支持无边框窗口和透明效果</li>
<li>窗口拖动和缩放功能</li>
<li>点击穿透功能</li>
</ul>
</li>
<li>
<p><strong>文件系统 API</strong></p>
<ul>
<li>文件选择对话框</li>
<li>文件夹选择对话框</li>
<li>文件读写（文本、二进制、JSON）</li>
<li>文件追加操作</li>
<li>文件和文件夹删除</li>
<li>路径存在性检查</li>
</ul>
</li>
<li>
<p><strong>窗口控制 API</strong></p>
<ul>
<li>无边框窗口设置</li>
<li>窗口拖动和缩放</li>
<li>透明度控制</li>
<li>点击穿透</li>
<li>窗口位置和大小控制</li>
<li>窗口置顶</li>
<li>任务栏显示控制</li>
</ul>
</li>
<li>
<p><strong>应用控制 API</strong></p>
<ul>
<li>应用关闭和重启</li>
<li>窗口信息查询</li>
<li>外部程序执行</li>
<li>配置文件管理</li>
<li>系统路径获取</li>
</ul>
</li>
<li>
<p><strong>系统 API</strong></p>
<ul>
<li>系统信息获取（CPU、内存、GPU、操作系统）</li>
<li>屏幕截图</li>
<li>桌面壁纸获取</li>
<li>按键模拟</li>
<li>鼠标点击模拟</li>
<li>鼠标位置获取</li>
</ul>
</li>
<li>
<p><strong>托盘图标 API</strong></p>
<ul>
<li>托盘图标创建和管理</li>
<li>托盘菜单设置</li>
<li>气泡通知</li>
<li>单击和双击事件</li>
<li>自定义图标</li>
</ul>
</li>
<li>
<p><strong>事件系统 API</strong></p>
<ul>
<li>USB 设备插拔事件</li>
<li>磁盘挂载/卸载事件</li>
<li>窗口事件（焦点、最大化、移动、缩放）</li>
<li>电源状态变化事件</li>
</ul>
</li>
<li>
<p><strong>设备系统 API</strong></p>
<ul>
<li>设备查询（多种设备类型）</li>
<li>设备信息获取</li>
<li>磁盘空间分析</li>
</ul>
</li>
<li>
<p><strong>PUP 文件格式</strong></p>
<ul>
<li>自定义打包格式</li>
<li>AES-256 加密支持</li>
<li>单文件分发</li>
<li><strong>新增 PUP V1.1 格式</strong>
<ul>
<li>支持启动脚本功能</li>
<li>在 PUP 加载后自动执行预设脚本</li>
<li>支持窗口启动位置设置</li>
<li>支持无边框模式设置</li>
<li>支持窗口大小设置</li>
<li>命令行参数：<code v-pre>-v</code> 指定版本，<code v-pre>--script</code> 指定脚本文件</li>
</ul>
</li>
</ul>
</li>
<li>
<p><strong>命令行工具</strong></p>
<ul>
<li>GUI 模式</li>
<li>创建 PUP 文件（支持 V1.0 和 V1.1 格式）</li>
<li>加载 PUP 文件（自动识别版本）</li>
<li>裸文件夹模式</li>
<li><strong>新增版本参数 <code v-pre>-v</code></strong>
<ul>
<li><code v-pre>-v 1.0</code>：创建 V1.0 格式的 PUP 文件（默认）</li>
<li><code v-pre>-v 1.1</code>：创建 V1.1 格式的 PUP 文件，需配合 <code v-pre>--script</code> 参数</li>
<li><code v-pre>-v 1.2</code>：创建 V1.2 格式的 PUP 文件，需配合签名参数</li>
</ul>
</li>
<li><strong>新增脚本参数 <code v-pre>--script</code></strong>
<ul>
<li>指定启动脚本文件路径（V1.1/V1.2 格式支持）</li>
</ul>
</li>
<li><strong>新增签名参数 <code v-pre>--certificate</code></strong>
<ul>
<li>指定证书文件路径（仅 V1.2 格式需要）</li>
</ul>
</li>
<li><strong>新增私钥参数 <code v-pre>--private-key</code></strong>
<ul>
<li>指定私钥文件路径（仅 V1.2 格式需要）</li>
</ul>
</li>
<li><strong>新增私钥密码参数 <code v-pre>--private-key-password</code></strong>
<ul>
<li>指定私钥加密密码（仅 V1.2 格式需要）</li>
</ul>
</li>
</ul>
</li>
</ul>
<h4 id="改进-changed" tabindex="-1"><a class="header-anchor" href="#改进-changed"><span>改进 (Changed)</span></a></h4>
<ul>
<li>PUP 文件格式更新至 V1.2</li>
<li>新增数据库签名功能（Storage API）</li>
<li>新增自动签名验证功能</li>
<li>新增证书指纹验证</li>
<li>项目文件结构重组</li>
<li>提升安全性和数据完整性保护</li>
</ul>
<h4 id="安全特性" tabindex="-1"><a class="header-anchor" href="#安全特性"><span>安全特性</span></a></h4>
<ul>
<li>通信密钥验证</li>
<li>文件系统路径保护</li>
<li>危险操作权限确认</li>
<li>PUP 文件加密</li>
<li><strong>新增数据签名和验证（V1.2）</strong></li>
<li><strong>新增证书指纹验证（V1.2）</strong></li>
<li><strong>新增私钥加密存储（V1.2）</strong></li>
</ul>
<h4 id="技术栈" tabindex="-1"><a class="header-anchor" href="#技术栈"><span>技术栈</span></a></h4>
<ul>
<li>.NET 9.0</li>
<li>Windows Forms</li>
<li>Microsoft WebView2</li>
<li>COM Interop</li>
<li>WMI（设备监控）</li>
<li>SharpZipLib（ZIP 文件处理）</li>
<li>System.Security.Cryptography（加密和签名）</li>
<li>System.Security.Cryptography.X509Certificates（证书处理）</li>
<li>AES-256-GCM（对称加密）</li>
<li>PBKDF2（密钥派生）</li>
<li>RSA（非对称加密和签名）</li>
</ul>
<h2 id="变更类型说明" tabindex="-1"><a class="header-anchor" href="#变更类型说明"><span>变更类型说明</span></a></h2>
<h3 id="新增功能-added" tabindex="-1"><a class="header-anchor" href="#新增功能-added"><span>新增功能 (Added)</span></a></h3>
<p>新增的功能和特性。</p>
<h3 id="改进-changed-1" tabindex="-1"><a class="header-anchor" href="#改进-changed-1"><span>改进 (Changed)</span></a></h3>
<p>对现有功能的改进和优化。</p>
<h3 id="废弃-deprecated" tabindex="-1"><a class="header-anchor" href="#废弃-deprecated"><span>废弃 (Deprecated)</span></a></h3>
<p>即将移除的功能，建议迁移到替代方案。</p>
<h3 id="移除-removed" tabindex="-1"><a class="header-anchor" href="#移除-removed"><span>移除 (Removed)</span></a></h3>
<p>已移除的功能。</p>
<h3 id="修复-fixed" tabindex="-1"><a class="header-anchor" href="#修复-fixed"><span>修复 (Fixed)</span></a></h3>
<p>修复的问题和错误。</p>
<h3 id="安全-security" tabindex="-1"><a class="header-anchor" href="#安全-security"><span>安全 (Security)</span></a></h3>
<p>安全相关的修复和改进。</p>
<h2 id="如何使用更新日志" tabindex="-1"><a class="header-anchor" href="#如何使用更新日志"><span>如何使用更新日志</span></a></h2>
<h3 id="开发者" tabindex="-1"><a class="header-anchor" href="#开发者"><span>开发者</span></a></h3>
<ul>
<li>查看新增功能了解最新特性</li>
<li>注意废弃和移除的 API</li>
<li>及时更新代码以适应变更</li>
</ul>
<h3 id="用户" tabindex="-1"><a class="header-anchor" href="#用户"><span>用户</span></a></h3>
<ul>
<li>了解新版本带来的改进</li>
<li>查看修复的问题</li>
<li>注意安全更新</li>
</ul>
<h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><a href="#">GitHub Releases</a>：查看所有版本发布</li>
<li><VPLink href="../api/">API 文档</VPLink>：了解 API 变更</li>
<li><VPLink href="./migration/">迁移指南</VPLink>：版本迁移指南（如有）</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<ul>
<li>查看 <VPLink href="../api/">API 文档</VPLink> 了解详细用法</li>
<li>阅读 <VPLink href="../guide/getting-started.md">快速开始</VPLink> 开始使用</li>
<li>参考 <VPLink href="../guide/best-practices.md">最佳实践</VPLink> 提升开发质量</li>
</ul>
</div></template>


