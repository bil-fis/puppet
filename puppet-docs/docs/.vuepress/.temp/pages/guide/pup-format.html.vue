<template><div><p>PUP（Puppet Package）是 Puppet 框架专用的应用打包格式。它将整个 Web 应用打包为一个独立的文件，支持加密保护，便于分发和部署。</p>
<h2 id="概述" tabindex="-1"><a class="header-anchor" href="#概述"><span>概述</span></a></h2>
<p>PUP 文件是一种自定义的打包格式，结合了 ZIP 压缩和 AES 加密技术，具有以下特点：</p>
<ul>
<li><strong>单文件分发</strong>：所有资源打包为一个文件</li>
<li><strong>密码保护</strong>：支持 AES-256 加密</li>
<li><strong>快速加载</strong>：优化的文件结构和加载机制</li>
<li><strong>跨版本兼容</strong>：版本标识确保兼容性</li>
</ul>
<h2 id="文件结构" tabindex="-1"><a class="header-anchor" href="#文件结构"><span>文件结构</span></a></h2>
<h3 id="版本概述" tabindex="-1"><a class="header-anchor" href="#版本概述"><span>版本概述</span></a></h3>
<p>PUP 文件格式支持多个版本：</p>
<ul>
<li><strong>V1.0</strong>：基础版本，支持 ZIP 打包和加密</li>
<li><strong>V1.1</strong>：增强版本，支持启动脚本功能</li>
<li><strong>V1.2</strong>：签名版本，支持证书和私钥，用于数据库签名验证</li>
</ul>
<h3 id="v1-0-二进制结构" tabindex="-1"><a class="header-anchor" href="#v1-0-二进制结构"><span>V1.0 二进制结构</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>┌─────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│           PUP V1.0 标识头 (8 字节)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           AES 加密的 ZIP 密码 (32 字节)              │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           ZIP 数据 (变长)                            │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────┘</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="v1-1-二进制结构" tabindex="-1"><a class="header-anchor" href="#v1-1-二进制结构"><span>V1.1 二进制结构</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>┌─────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│           PUP V1.1 标识头 (8 字节)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           脚本长度 (4 字节，int32)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           启动脚本内容 (变长)                        │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           AES 加密的 ZIP 密码 (32 字节)              │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           ZIP 数据 (变长)                            │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────┘</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="v1-2-二进制结构-带签名支持" tabindex="-1"><a class="header-anchor" href="#v1-2-二进制结构-带签名支持"><span>V1.2 二进制结构（带签名支持）</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>┌─────────────────────────────────────────────────────┐</span></span>
<span class="line"><span>│           PUP V1.2 标识头 (8 字节)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           脚本长度 (4 字节，int32)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           启动脚本内容 (变长)                        │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           证书长度 (4 字节，int32)                   │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           证书数据 (变长，DER 格式)                  │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           加密私钥长度 (4 字节，int32)               │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           加密的私钥数据 (变长)                      │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           AES 加密的 ZIP 密码 (32 字节)              │</span></span>
<span class="line"><span>├─────────────────────────────────────────────────────┤</span></span>
<span class="line"><span>│           ZIP 数据 (变长)                            │</span></span>
<span class="line"><span>└─────────────────────────────────────────────────────┘</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>V1.2 安全特性</strong>：</p>
<ul>
<li><strong>证书保护</strong>：使用自签名 X.509 证书进行签名验证</li>
<li><strong>私钥加密</strong>：私钥使用 AES-256-GCM 加密，密钥通过 PBKDF2 派生</li>
<li><strong>数据库签名</strong>：支持对 SQLite 数据库进行签名和验证</li>
<li><strong>指纹验证</strong>：通过证书指纹确保证书未被替换</li>
</ul>
<h3 id="详细说明" tabindex="-1"><a class="header-anchor" href="#详细说明"><span>详细说明</span></a></h3>
<h4 id="_1-标识头-8-字节" tabindex="-1"><a class="header-anchor" href="#_1-标识头-8-字节"><span>1. 标识头（8 字节）</span></a></h4>
<p>固定字符串，用于识别文件格式和版本。</p>
<ul>
<li>V1.0: <code v-pre>&quot;PUP V1.0&quot;</code></li>
<li>V1.1: <code v-pre>&quot;PUP V1.1&quot;</code></li>
</ul>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> PUP_HEADER_V1_0</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> PUP_HEADER_V1_1</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.1</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="_2-加密的-zip-密码-32-字节" tabindex="-1"><a class="header-anchor" href="#_2-加密的-zip-密码-32-字节"><span>2. 加密的 ZIP 密码（32 字节）</span></a></h4>
<p>ZIP 文件的解压密码，使用固定密钥 <code v-pre>&quot;ILOVEPUPPET&quot;</code> 进行 AES 加密。</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 固定加密密钥</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> FixedKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">ILOVEPUPPET</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 加密 ZIP 密码</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> encryptedPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> AesHelper</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Encrypt</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">zipPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">FixedKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="_3-zip-数据" tabindex="-1"><a class="header-anchor" href="#_3-zip-数据"><span>3. ZIP 数据</span></a></h4>
<p>包含整个应用文件的 ZIP 压缩数据。</p>
<h2 id="创建-pup-文件" tabindex="-1"><a class="header-anchor" href="#创建-pup-文件"><span>创建 PUP 文件</span></a></h2>
<h3 id="v1-0-格式" tabindex="-1"><a class="header-anchor" href="#v1-0-格式"><span>V1.0 格式</span></a></h3>
<h4 id="命令行方式" tabindex="-1"><a class="header-anchor" href="#命令行方式"><span>命令行方式</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">源文件</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">夹</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">输出文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> [-p </span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">&#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">密</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">码</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">]</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<ul>
<li><code v-pre>-i</code> 或 <code v-pre>--input</code>：源文件夹路径</li>
<li><code v-pre>-o</code> 或 <code v-pre>--output</code>：输出 PUP 文件路径</li>
<li><code v-pre>-p</code> 或 <code v-pre>--password</code>：（可选）ZIP 密码，用于加密</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 无密码创建</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用密码创建</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="代码方式-c" tabindex="-1"><a class="header-anchor" href="#代码方式-c"><span>代码方式（C#）</span></a></h4>
<p>使用 <code v-pre>PupCreator</code> 类创建 PUP 文件：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">using</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 创建 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">PupCreator</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">CreatePup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    sourceFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    outputPupFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    password</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 可选</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="v1-1-格式-带启动脚本" tabindex="-1"><a class="header-anchor" href="#v1-1-格式-带启动脚本"><span>V1.1 格式（带启动脚本）</span></a></h3>
<h4 id="命令行方式-1" tabindex="-1"><a class="header-anchor" href="#命令行方式-1"><span>命令行方式</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">源文件</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">夹</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">输出文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> [-p </span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">&#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">密</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">码</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">]</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.1</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --script</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">脚本文</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">件</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<ul>
<li><code v-pre>-i</code> 或 <code v-pre>--input</code>：源文件夹路径</li>
<li><code v-pre>-o</code> 或 <code v-pre>--output</code>：输出 PUP 文件路径</li>
<li><code v-pre>-p</code> 或 <code v-pre>--password</code>：（可选）ZIP 密码，用于加密</li>
<li><code v-pre>-v</code> 或 <code v-pre>--version</code>：PUP 版本，V1.1 需要指定</li>
<li><code v-pre>--script</code>：启动脚本文件路径（V1.1 必需）</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建 V1.1 格式的 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.1</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --script</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\script.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建带密码的 V1.1 格式</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.1</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --script</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\script.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="代码方式-c-1" tabindex="-1"><a class="header-anchor" href="#代码方式-c-1"><span>代码方式（C#）</span></a></h4>
<p>使用 <code v-pre>PupCreator</code> 类创建 V1.1 格式的 PUP 文件：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">using</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 创建 V1.1 格式的 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">PupCreator</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">CreatePup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    sourceFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    outputPupFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    password</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 可选</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    version</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">1.1</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">                // 指定版本</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    scriptFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\script.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">   // 启动脚本文件</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="启动脚本-v1-1" tabindex="-1"><a class="header-anchor" href="#启动脚本-v1-1"><span>启动脚本（V1.1）</span></a></h2>
<div class="hint-container tip">
<p class="hint-container-title">详细文档</p>
<p>关于启动脚本的完整使用说明、语法参考和最佳实践，请参阅 <VPLink href="./pup-script.md">PUP 启动脚本</VPLink> 文档。</p>
</div>
<h3 id="概述-1" tabindex="-1"><a class="header-anchor" href="#概述-1"><span>概述</span></a></h3>
<p>V1.1 格式支持在 PUP 加载后自动执行预设脚本，实现快速初始化窗口状态。</p>
<h3 id="脚本语法" tabindex="-1"><a class="header-anchor" href="#脚本语法"><span>脚本语法</span></a></h3>
<p>启动脚本使用简单的命令语法，每行一个命令：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set &#x3C;属性> &#x3C;值></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><h3 id="支持的命令" tabindex="-1"><a class="header-anchor" href="#支持的命令"><span>支持的命令</span></a></h3>
<h4 id="_1-设置启动位置" tabindex="-1"><a class="header-anchor" href="#_1-设置启动位置"><span>1. 设置启动位置</span></a></h4>
<p><strong>语法</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set startup_position &#x3C;x>,&#x3C;y></span></span>
<span class="line"><span>set startup_position &#x3C;POSITION></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>参数</strong>：</p>
<ul>
<li><code v-pre>&lt;x&gt;,&lt;y&gt;</code>：指定坐标，例如 <code v-pre>100,200</code></li>
<li><code v-pre>&lt;POSITION&gt;</code>：预定义位置，支持以下值：
<ul>
<li><code v-pre>left-top</code>：左上角</li>
<li><code v-pre>left-bottom</code>：左下角（排除任务栏）</li>
<li><code v-pre>right-top</code>：右上角</li>
<li><code v-pre>right-bottom</code>：右下角（排除任务栏）</li>
<li><code v-pre>center</code>：屏幕中心</li>
</ul>
</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set startup_position 100,200</span></span>
<span class="line"><span>set startup_position center</span></span>
<span class="line"><span>set startup_position right-bottom</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="_2-设置无边框模式" tabindex="-1"><a class="header-anchor" href="#_2-设置无边框模式"><span>2. 设置无边框模式</span></a></h4>
<p><strong>语法</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set borderless &#x3C;true|false></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数</strong>：</p>
<ul>
<li><code v-pre>true</code>：启用无边框模式</li>
<li><code v-pre>false</code>：禁用无边框模式</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set borderless true</span></span>
<span class="line"><span>set borderless false</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="_3-设置窗口大小" tabindex="-1"><a class="header-anchor" href="#_3-设置窗口大小"><span>3. 设置窗口大小</span></a></h4>
<p><strong>语法</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set window_size &#x3C;width>,&#x3C;height></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数</strong>：</p>
<ul>
<li><code v-pre>&lt;width&gt;</code>：窗口宽度（像素）</li>
<li><code v-pre>&lt;height&gt;</code>：窗口高度（像素）</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set window_size 800,600</span></span>
<span class="line"><span>set window_size 1024,768</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="脚本示例" tabindex="-1"><a class="header-anchor" href="#脚本示例"><span>脚本示例</span></a></h3>
<p><strong>示例 1：右下角无边框窗口</strong></p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set startup_position right-bottom</span></span>
<span class="line"><span>set borderless true</span></span>
<span class="line"><span>set window_size 400,300</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>示例 2：屏幕中心有边框窗口</strong></p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set startup_position center</span></span>
<span class="line"><span>set borderless false</span></span>
<span class="line"><span>set window_size 1024,768</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>示例 3：指定位置和大小</strong></p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>set startup_position 100,100</span></span>
<span class="line"><span>set borderless true</span></span>
<span class="line"><span>set window_size 800,600</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="脚本文件示例" tabindex="-1"><a class="header-anchor" href="#脚本文件示例"><span>脚本文件示例</span></a></h3>
<p>创建一个名为 <code v-pre>startup.txt</code> 的脚本文件：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span># Puppet 启动脚本</span></span>
<span class="line"><span># 设置窗口为右下角无边框窗口</span></span>
<span class="line"><span>set startup_position right-bottom</span></span>
<span class="line"><span>set borderless true</span></span>
<span class="line"><span>set window_size 500,400</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="脚本执行时机" tabindex="-1"><a class="header-anchor" href="#脚本执行时机"><span>脚本执行时机</span></a></h3>
<ul>
<li>脚本在 PUP 文件加载后执行</li>
<li>在 WebView2 初始化完成后执行</li>
<li>在页面导航到应用 URL 之前执行</li>
<li>脚本执行错误不会阻止应用启动</li>
</ul>
<h3 id="脚本限制" tabindex="-1"><a class="header-anchor" href="#脚本限制"><span>脚本限制</span></a></h3>
<ul>
<li>每行只能包含一个命令</li>
<li>命令不区分大小写</li>
<li>支持 <code v-pre>//</code> 和 <code v-pre>#</code> 开头的注释</li>
<li>空行会被忽略</li>
</ul>
<h3 id="代码方式-c-2" tabindex="-1"><a class="header-anchor" href="#代码方式-c-2"><span>代码方式（C#）</span></a></h3>
<p>使用 <code v-pre>PupCreator</code> 类创建 PUP 文件：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">using</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 创建 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">PupCreator</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">CreatePup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    sourceFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    outputPupFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    password</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 可选</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="v1-2-格式-带签名支持" tabindex="-1"><a class="header-anchor" href="#v1-2-格式-带签名支持"><span>V1.2 格式（带签名支持）</span></a></h3>
<h4 id="命令行方式-2" tabindex="-1"><a class="header-anchor" href="#命令行方式-2"><span>命令行方式</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">源文件</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">夹</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">输出文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> [-p </span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">&#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">密</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">码</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">]</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.2</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --certificate</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">证书文</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">件</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">私钥文</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">件</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key-password</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">私钥密</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">码</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<ul>
<li><code v-pre>-i</code> 或 <code v-pre>--input</code>：源文件夹路径</li>
<li><code v-pre>-o</code> 或 <code v-pre>--output</code>：输出 PUP 文件路径</li>
<li><code v-pre>-p</code> 或 <code v-pre>--password</code>：（可选）ZIP 密码，用于加密</li>
<li><code v-pre>-v</code> 或 <code v-pre>--version</code>：PUP 版本，V1.2 需要指定</li>
<li><code v-pre>--certificate</code>：证书文件路径（V1.2 必需）</li>
<li><code v-pre>--private-key</code>：私钥文件路径（V1.2 必需）</li>
<li><code v-pre>--private-key-password</code>：私钥加密密码（V1.2 必需）</li>
</ul>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建 V1.2 格式的 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.2</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --certificate</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.crt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key-password</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MyKeyPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建带密码和签名的 V1.2 格式</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.2</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --certificate</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.crt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key-password</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MyKeyPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="代码方式-c-3" tabindex="-1"><a class="header-anchor" href="#代码方式-c-3"><span>代码方式（C#）</span></a></h4>
<p>使用 <code v-pre>PupCreator</code> 类创建 V1.2 格式的 PUP 文件：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">using</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Puppet</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 创建 V1.2 格式的 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">PupCreator</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">CreatePup</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    sourceFolder</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    outputPupFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    password</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 可选</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    version</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">1.2</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">                // 指定版本</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    certificate</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.crt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 证书文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    privateKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> @"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">     // 私钥文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    privateKeyPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MyKeyPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 私钥加密密码</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="证书和私钥生成" tabindex="-1"><a class="header-anchor" href="#证书和私钥生成"><span>证书和私钥生成</span></a></h4>
<p>使用 <code v-pre>puppet-sign</code> 工具生成签名密钥对：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 生成签名密钥对</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet-sign.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --generate-signing-key</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --alias</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> MyApp</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --organization</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> MyOrg</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --country</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> CN</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --validity</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 3650</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><p>生成的文件：</p>
<ul>
<li><code v-pre>app.crt</code> - 自签名证书（包含公钥）</li>
<li><code v-pre>app.key</code> - RSA 私钥（PKCS#8 格式）</li>
</ul>
<h4 id="数据库签名" tabindex="-1"><a class="header-anchor" href="#数据库签名"><span>数据库签名</span></a></h4>
<p>使用 V1.2 格式的 PUP 文件时，数据库会自动支持签名功能：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 在代码中对数据库进行签名</span></span>
<span class="line"><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994">StorageController</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> storage</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> StorageController</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">form</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">storage</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">SignDatabase</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">default</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p>签名后的数据库会在 <code v-pre>puppet_metadata</code> 表中存储：</p>
<ul>
<li><code v-pre>app_id</code> - 应用标识符（来自证书的 CN）</li>
<li><code v-pre>certificate_fingerprint</code> - 证书指纹（SHA256）</li>
<li><code v-pre>signature_data</code> - 签名数据（使用私钥签名）</li>
<li><code v-pre>created_at</code> - 签名时间戳</li>
</ul>
<h4 id="签名验证" tabindex="-1"><a class="header-anchor" href="#签名验证"><span>签名验证</span></a></h4>
<p>当加载 V1.2 格式的 PUP 文件时，系统会自动验证数据库签名：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// PupServer 会自动验证签名</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> server</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> PupServer</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">myapp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 7738</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 首次访问数据库时会验证签名</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">storage</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">SetItem</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">default</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">value</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p>验证失败会输出警告信息：</p>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>✗ Database signature verification failed: default</span></span>
<span class="line"><span>  WARNING: Database may have been tampered with</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="加载-pup-文件" tabindex="-1"><a class="header-anchor" href="#加载-pup-文件"><span>加载 PUP 文件</span></a></h2>
<h3 id="命令行方式-3" tabindex="-1"><a class="header-anchor" href="#命令行方式-3"><span>命令行方式</span></a></h3>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 加载 V1.0 格式</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 加载 V1.1 格式（会自动执行启动脚本）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyAppV1_1.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 加载 V1.2 格式（会自动加载证书和私钥，支持数据库签名）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyAppV1_2.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>版本自动识别</strong>：</p>
<p>PupServer 会自动识别 PUP 文件的版本：</p>
<ul>
<li>V1.0：解析标识头 <code v-pre>&quot;PUP V1.0&quot;</code></li>
<li>V1.1：解析标识头 <code v-pre>&quot;PUP V1.1&quot;</code> 并执行启动脚本</li>
<li>V1.2：解析标识头 <code v-pre>&quot;PUP V1.2&quot;</code>，加载证书和私钥，支持数据库签名</li>
</ul>
<h3 id="配置文件方式" tabindex="-1"><a class="header-anchor" href="#配置文件方式"><span>配置文件方式</span></a></h3>
<p>编辑 <code v-pre>puppet.ini</code> 文件：</p>
<div class="language-ini line-numbers-mode" data-highlighter="shiki" data-ext="ini" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-ini"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">C:\MyApp.pup</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><p>然后直接运行 <code v-pre>puppet.exe</code>。</p>
<h3 id="加载流程" tabindex="-1"><a class="header-anchor" href="#加载流程"><span>加载流程</span></a></h3>
<h4 id="v1-0-加载流程" tabindex="-1"><a class="header-anchor" href="#v1-0-加载流程"><span>V1.0 加载流程</span></a></h4>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>1. 读取文件前 8 字节，验证标识头 "PUP V1.0"</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>2. 读取接下来的 32 字节（加密的 ZIP 密码）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>3. 使用固定密钥解密 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>4. 读取剩余数据（ZIP 数据）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>5. 使用解密的 ZIP 密码解压 ZIP 数据</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>6. 提取文件到内存或临时目录</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="v1-1-加载流程" tabindex="-1"><a class="header-anchor" href="#v1-1-加载流程"><span>V1.1 加载流程</span></a></h4>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>1. 读取文件前 8 字节，验证标识头 "PUP V1.1"</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>2. 读取接下来的 4 字节（脚本长度）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>3. 读取启动脚本内容（变长）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>4. 读取接下来的 32 字节（加密的 ZIP 密码）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>5. 使用固定密钥解密 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>6. 读取剩余数据（ZIP 数据）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>7. 使用解密的 ZIP 密码解压 ZIP 数据</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>8. 提取文件到内存或临时目录</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>9. 执行启动脚本（在 WebView2 初始化完成后）</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="v1-2-加载流程-带签名支持" tabindex="-1"><a class="header-anchor" href="#v1-2-加载流程-带签名支持"><span>V1.2 加载流程（带签名支持）</span></a></h4>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>1. 读取文件前 8 字节，验证标识头 "PUP V1.2"</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>2. 读取接下来的 4 字节（脚本长度）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>3. 读取启动脚本内容（变长）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>4. 读取接下来的 4 字节（证书长度）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>5. 读取证书数据（变长，DER 格式）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>6. 解析证书并提取公钥和证书指纹</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>7. 读取接下来的 4 字节（加密私钥长度）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>8. 读取加密的私钥数据（变长）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>9. 使用 PBKDF2 派生密钥并解密私钥（AES-256-GCM）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>10. 读取接下来的 32 字节（加密的 ZIP 密码）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>11. 使用固定密钥解密 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>12. 读取剩余数据（ZIP 数据）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>13. 使用解密的 ZIP 密码解压 ZIP 数据</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>14. 提取文件到内存或临时目录</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>15. 执行启动脚本（如果有）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>16. 存储证书和私钥参数，用于数据库签名</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="加密机制" tabindex="-1"><a class="header-anchor" href="#加密机制"><span>加密机制</span></a></h2>
<h3 id="加密流程" tabindex="-1"><a class="header-anchor" href="#加密流程"><span>加密流程</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>1. 生成随机 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>2. 使用固定密钥 "ILOVEPUPPET" 加密 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>3. 创建 ZIP 文件（使用 ZIP 密码加密）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>4. 拼接：标识头 + 加密的 ZIP 密码 + ZIP 数据</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>5. 写入 PUP 文件</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="解密流程" tabindex="-1"><a class="header-anchor" href="#解密流程"><span>解密流程</span></a></h3>
<div class="language- line-numbers-mode" data-highlighter="shiki" data-ext="" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-"><span class="line"><span>1. 读取文件前 8 字节，验证标识头</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>2. 读取接下来的 32 字节（加密的 ZIP 密码）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>3. 使用固定密钥解密 ZIP 密码</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>4. 读取剩余数据（ZIP 数据）</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>5. 使用解密的 ZIP 密码解压 ZIP 数据</span></span>
<span class="line"><span>        ↓</span></span>
<span class="line"><span>6. 提取文件到内存或临时目录</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="加密算法" tabindex="-1"><a class="header-anchor" href="#加密算法"><span>加密算法</span></a></h3>
<p>PUP 使用以下加密算法：</p>
<ul>
<li><strong>加密算法</strong>：AES (Advanced Encryption Standard)</li>
<li><strong>密钥长度</strong>：256 位</li>
<li><strong>模式</strong>：CBC (Cipher Block Chaining)</li>
<li><strong>填充</strong>：PKCS7</li>
</ul>
<div class="hint-container tip">
<p class="hint-container-title">安全说明</p>
<p>加密的 ZIP 密码使用固定的密钥 <code v-pre>&quot;ILOVEPUPPET&quot;</code> 加密，这是一种轻量级的保护方式。如果需要更强的安全性，建议使用文件系统加密（如 BitLocker）或分发时使用 HTTPS。</p>
</div>
<h2 id="zip-密码生成" tabindex="-1"><a class="header-anchor" href="#zip-密码生成"><span>ZIP 密码生成</span></a></h2>
<p>如果未指定密码，系统会自动生成随机密码：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> GenerateRandomPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666">()</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    const</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> chars</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">    var</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> random</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> Random</span><span style="--shiki-light:#999999;--shiki-dark:#666666">();</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    return</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Enumerable</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Repeat</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">chars</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 16</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">        .</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Select</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">s</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> =></span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> s</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">random</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Next</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">s</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Length</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)]).</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">ToArray</span><span style="--shiki-light:#999999;--shiki-dark:#666666">());</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="文件验证" tabindex="-1"><a class="header-anchor" href="#文件验证"><span>文件验证</span></a></h2>
<p>PUP 服务器在加载时会验证文件格式：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">public</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> bool</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> LoadPupFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">()</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 1. 读取文件</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> File</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">ReadAllBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">_pupFilePath</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 2. 验证标识头</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Length</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> &#x3C;</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 40</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">  // 8 (header) + 32 (encrypted password)</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> false</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> header</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetString</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">header</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> !=</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        return</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> false</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 3. 提取加密的 ZIP 密码</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> encryptedPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetString</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 32</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 4. 解密 ZIP 密码</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> zipPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> AesHelper</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Decrypt</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">encryptedPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">FixedKey</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 5. 提取 ZIP 数据</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> zipData</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Length</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> -</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 40</span><span style="--shiki-light:#999999;--shiki-dark:#666666">];</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    Array</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Copy</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 40</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> zipData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> zipData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Length</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    // 6. 打开 ZIP 文件</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    _zipFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> ZipInputStream</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> MemoryStream</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">zipData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">));</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">    _zipFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Password</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> zipPassword</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    return</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> true</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="性能考虑" tabindex="-1"><a class="header-anchor" href="#性能考虑"><span>性能考虑</span></a></h2>
<h3 id="文件大小" tabindex="-1"><a class="header-anchor" href="#文件大小"><span>文件大小</span></a></h3>
<p>PUP 文件大小取决于：</p>
<ul>
<li>源文件总大小</li>
<li>ZIP 压缩率（通常 30-70%）</li>
<li>加密开销（约 40 字节）</li>
</ul>
<p><strong>优化建议</strong>：</p>
<ul>
<li>压缩图片和媒体文件</li>
<li>移除未使用的资源</li>
<li>使用 CDN 加载第三方库</li>
</ul>
<h3 id="加载速度" tabindex="-1"><a class="header-anchor" href="#加载速度"><span>加载速度</span></a></h3>
<p>PUP 文件加载速度取决于：</p>
<ul>
<li>文件大小</li>
<li>磁盘读取速度</li>
<li>解密和解压速度</li>
</ul>
<p><strong>优化建议</strong>：</p>
<ul>
<li>保持文件大小合理（建议 &lt; 50MB）</li>
<li>使用 SSD 提升读取速度</li>
<li>预加载常用资源</li>
</ul>
<h2 id="版本兼容性" tabindex="-1"><a class="header-anchor" href="#版本兼容性"><span>版本兼容性</span></a></h2>
<h3 id="标识头" tabindex="-1"><a class="header-anchor" href="#标识头"><span>标识头</span></a></h3>
<p>当前支持的版本：</p>
<ul>
<li>V1.0: <code v-pre>&quot;PUP V1.0&quot;</code> - 基础版本</li>
<li>V1.1: <code v-pre>&quot;PUP V1.1&quot;</code> - 增强版本（支持启动脚本）</li>
<li>V1.2: <code v-pre>&quot;PUP V1.2&quot;</code> - 签名版本（支持证书和数据库签名）</li>
</ul>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> PUP_HEADER_V1_0</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> PUP_HEADER_V1_1</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.1</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">private</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> static</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> readonly</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> byte</span><span style="--shiki-light:#999999;--shiki-dark:#666666">[]</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> PUP_HEADER_V1_2</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.2</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="版本检测" tabindex="-1"><a class="header-anchor" href="#版本检测"><span>版本检测</span></a></h3>
<p>PupServer 会自动检测 PUP 文件版本并使用相应的解析逻辑：</p>
<div class="language-csharp line-numbers-mode" data-highlighter="shiki" data-ext="csharp" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-csharp"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">// 示例：版本检测</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">string</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> header</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> Encoding</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">UTF8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">.</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">GetString</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileData</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">,</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 8</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">switch</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">header</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">{</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    case</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.0</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">        // 使用 V1.0 解析逻辑</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">        LoadPupV1_0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        break</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    case</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.1</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">        // 使用 V1.1 解析逻辑</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">        LoadPupV1_1</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        break</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    case</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUP V1.2</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">        // 使用 V1.2 解析逻辑（支持签名）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">        LoadPupV1_2</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileBytes</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        break</span><span style="--shiki-light:#999999;--shiki-dark:#666666">;</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    default</span><span style="--shiki-light:#999999;--shiki-dark:#666666">:</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">        throw</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> new</span><span style="--shiki-light:#2E8F82;--shiki-dark:#5DA994"> NotSupportedException</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">不支持的 PUP 版本</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">);</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="向后兼容性" tabindex="-1"><a class="header-anchor" href="#向后兼容性"><span>向后兼容性</span></a></h3>
<ul>
<li>V1.1 格式完全兼容 V1.0 的所有功能</li>
<li>V1.2 格式完全兼容 V1.1 的所有功能</li>
<li>V1.1 文件包含额外的启动脚本数据</li>
<li>V1.2 文件包含证书和加密私钥，支持数据库签名</li>
<li>PupServer 可以加载和解析所有版本的 PUP 文件</li>
<li>建议新项目使用 V1.2 格式以获得企业级安全保护</li>
</ul>
<h3 id="版本选择建议" tabindex="-1"><a class="header-anchor" href="#版本选择建议"><span>版本选择建议</span></a></h3>
<ul>
<li><strong>V1.0</strong>：适用于简单的应用，不需要启动时配置</li>
<li><strong>V1.1</strong>：适用于需要预设窗口状态的应用</li>
<li><strong>V1.2</strong>：适用于需要数据库签名和完整性的应用，推荐用于生产环境</li>
</ul>
<h3 id="版本升级" tabindex="-1"><a class="header-anchor" href="#版本升级"><span>版本升级</span></a></h3>
<p>如果需要将 V1.0 升级到 V1.1：</p>
<ol>
<li>创建启动脚本文件</li>
<li>使用 <code v-pre>-v 1.1</code> 和 <code v-pre>--script</code> 参数重新创建 PUP 文件</li>
<li>测试新格式的 PUP 文件</li>
</ol>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 升级到 V1.1</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyAppV1_1.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.1</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --script</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\startup.txt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 升级到 V1.2</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyAppV1_2.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -v</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1.2</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --certificate</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.crt</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\app.key</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --private-key-password</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MyKeyPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="与裸文件夹模式的对比" tabindex="-1"><a class="header-anchor" href="#与裸文件夹模式的对比"><span>与裸文件夹模式的对比</span></a></h2>
<table>
<thead>
<tr>
<th>特性</th>
<th>PUP V1.0</th>
<th>PUP V1.1</th>
<th>PUP V1.2</th>
<th>裸文件夹</th>
</tr>
</thead>
<tbody>
<tr>
<td>分发方式</td>
<td>单文件</td>
<td>单文件</td>
<td>单文件</td>
<td>文件夹</td>
</tr>
<tr>
<td>加密保护</td>
<td>支持</td>
<td>支持</td>
<td>支持</td>
<td>不支持</td>
</tr>
<tr>
<td>启动脚本</td>
<td>不支持</td>
<td>支持</td>
<td>支持</td>
<td>不支持</td>
</tr>
<tr>
<td>数据库签名</td>
<td>不支持</td>
<td>不支持</td>
<td>支持</td>
<td>不支持</td>
</tr>
<tr>
<td>证书验证</td>
<td>不支持</td>
<td>不支持</td>
<td>支持</td>
<td>不支持</td>
</tr>
<tr>
<td>窗口预设</td>
<td>代码控制</td>
<td>脚本控制</td>
<td>脚本控制</td>
<td>代码控制</td>
</tr>
<tr>
<td>开发便利性</td>
<td>较低</td>
<td>较低</td>
<td>中等</td>
<td>高</td>
</tr>
<tr>
<td>热重载</td>
<td>不支持</td>
<td>不支持</td>
<td>不支持</td>
<td>支持</td>
</tr>
<tr>
<td>文件大小</td>
<td>较小</td>
<td>稍大</td>
<td>较大</td>
<td>较大</td>
</tr>
<tr>
<td>加载速度</td>
<td>稍慢</td>
<td>稍慢</td>
<td>稍慢</td>
<td>快</td>
</tr>
<tr>
<td>安全级别</td>
<td>中等</td>
<td>中等</td>
<td>高</td>
<td>低</td>
</tr>
<tr>
<td>适用场景</td>
<td>简单应用发布</td>
<td>复杂应用发布</td>
<td>生产环境发布</td>
<td>开发调试</td>
</tr>
</tbody>
</table>
<h2 id="最佳实践" tabindex="-1"><a class="header-anchor" href="#最佳实践"><span>最佳实践</span></a></h2>
<h3 id="_1-创建-pup-文件" tabindex="-1"><a class="header-anchor" href="#_1-创建-pup-文件"><span>1. 创建 PUP 文件</span></a></h3>
<ul>
<li>在发布前创建 PUP 文件</li>
<li>使用有意义的密码</li>
<li>测试 PUP 文件可以正常加载</li>
</ul>
<h3 id="_2-密码管理" tabindex="-1"><a class="header-anchor" href="#_2-密码管理"><span>2. 密码管理</span></a></h3>
<ul>
<li>不要在代码中硬编码密码</li>
<li>使用环境变量或配置文件存储密码</li>
<li>定期更换密码</li>
</ul>
<h3 id="_3-文件优化" tabindex="-1"><a class="header-anchor" href="#_3-文件优化"><span>3. 文件优化</span></a></h3>
<ul>
<li>压缩图片和媒体文件</li>
<li>移除调试代码和注释</li>
<li>使用生产环境的构建配置</li>
</ul>
<h3 id="_4-版本控制" tabindex="-1"><a class="header-anchor" href="#_4-版本控制"><span>4. 版本控制</span></a></h3>
<ul>
<li>在文件名中包含版本号</li>
<li>保留历史版本的 PUP 文件</li>
<li>记录每个版本的变更</li>
</ul>
<h3 id="_5-签名管理-v1-2" tabindex="-1"><a class="header-anchor" href="#_5-签名管理-v1-2"><span>5. 签名管理（V1.2）</span></a></h3>
<ul>
<li>使用 puppet-sign 工具生成签名密钥对</li>
<li>使用强密码保护私钥（至少 16 位）</li>
<li>定期备份证书和私钥文件</li>
<li>妥善保管私钥密码，不要提交到版本控制</li>
<li>定期检查证书有效期，提前续期</li>
<li>对数据库进行签名前先备份数据</li>
</ul>
<h3 id="_6-分发策略" tabindex="-1"><a class="header-anchor" href="#_6-分发策略"><span>6. 分发策略</span></a></h3>
<ul>
<li>使用 HTTPS 分发 PUP 文件</li>
<li>提供文件校验（如 MD5、SHA256）</li>
<li>包含详细的更新日志</li>
<li>对于 V1.2 格式，可以提供证书指纹供验证</li>
</ul>
<h2 id="故障排除" tabindex="-1"><a class="header-anchor" href="#故障排除"><span>故障排除</span></a></h2>
<h3 id="常见问题" tabindex="-1"><a class="header-anchor" href="#常见问题"><span>常见问题</span></a></h3>
<h4 id="_1-无效的-pup-文件" tabindex="-1"><a class="header-anchor" href="#_1-无效的-pup-文件"><span>1. &quot;无效的 PUP 文件&quot;</span></a></h4>
<p><strong>原因</strong>：文件格式不正确或已损坏</p>
<p><strong>解决方案</strong>：</p>
<ul>
<li>重新创建 PUP 文件</li>
<li>检查文件是否完整</li>
<li>验证文件头是否为 &quot;PUP V1.0&quot;</li>
</ul>
<h4 id="_2-解密失败" tabindex="-1"><a class="header-anchor" href="#_2-解密失败"><span>2. &quot;解密失败&quot;</span></a></h4>
<p><strong>原因</strong>：加密密码不正确</p>
<p><strong>解决方案</strong>：</p>
<ul>
<li>确认创建时使用的密码</li>
<li>检查密码是否包含特殊字符</li>
<li>重新创建 PUP 文件</li>
</ul>
<h4 id="_3-文件过大" tabindex="-1"><a class="header-anchor" href="#_3-文件过大"><span>3. &quot;文件过大&quot;</span></a></h4>
<p><strong>原因</strong>：包含大量资源文件</p>
<p><strong>解决方案</strong>：</p>
<ul>
<li>压缩图片和媒体文件</li>
<li>移除未使用的资源</li>
<li>使用 CDN 加载第三方库</li>
</ul>
<h4 id="_4-加载缓慢" tabindex="-1"><a class="header-anchor" href="#_4-加载缓慢"><span>4. &quot;加载缓慢&quot;</span></a></h4>
<p><strong>原因</strong>：文件较大或磁盘读取速度慢</p>
<p><strong>解决方案</strong>：</p>
<ul>
<li>优化文件大小</li>
<li>使用 SSD</li>
<li>预加载常用资源</li>
</ul>
<h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><VPLink href="./cli-parameters.md">命令行参数</VPLink> - 完整的命令行选项</li>
<li><VPLink href="./project-structure.md">项目结构</VPLink> - 项目目录组织</li>
<li><VPLink href="./best-practices.md">最佳实践</VPLink> - 开发建议</li>
<li><VPLink href="./security.md">安全机制</VPLink> - 签名验证和安全特性（V1.2）</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>了解 PUP 格式后，建议：</p>
<ol>
<li>尝试创建您的第一个 PUP 文件</li>
<li>学习 <VPLink href="./cli-parameters.md">命令行参数</VPLink> 了解更多选项</li>
<li>参考 <VPLink href="./best-practices.md">最佳实践</VPLink> 优化您的项目</li>
<li>了解 <VPLink href="./security.md">安全机制</VPLink> 实现数据签名和完整性保护</li>
</ol>
</div></template>


