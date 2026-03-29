<template><div><p>Puppet 框架提供灵活的命令行接口，支持多种运行模式和工作方式。本章节详细介绍所有可用的命令行参数及其用法。</p>
<h2 id="概述" tabindex="-1"><a class="header-anchor" href="#概述"><span>概述</span></a></h2>
<p>Puppet 支持三种主要运行模式：</p>
<ol>
<li><strong>GUI 模式</strong>：启动图形界面应用</li>
<li><strong>创建 PUP 模式</strong>：创建 PUP 打包文件</li>
<li><strong>加载 PUP 模式</strong>：加载并运行 PUP 文件</li>
<li><strong>裸文件夹模式</strong>：从文件夹加载应用（开发时使用）</li>
</ol>
<h2 id="基本语法" tabindex="-1"><a class="header-anchor" href="#基本语法"><span>基本语法</span></a></h2>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> [选项] </span><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">参数</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><h2 id="运行模式" tabindex="-1"><a class="header-anchor" href="#运行模式"><span>运行模式</span></a></h2>
<h3 id="_1-gui-模式" tabindex="-1"><a class="header-anchor" href="#_1-gui-模式"><span>1. GUI 模式</span></a></h3>
<p>启动 Puppet 的图形界面应用，通常用于调试或手动操作。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>特点</strong>：</p>
<ul>
<li>显示主窗口</li>
<li>可以通过界面选择加载 PUP 文件或文件夹</li>
<li>适用于快速测试和调试</li>
</ul>
<p><strong>配置</strong>：</p>
<p>GUI 模式会读取 <code v-pre>puppet.ini</code> 配置文件中的默认设置：</p>
<div class="language-ini line-numbers-mode" data-highlighter="shiki" data-ext="ini" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-ini"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">app.pup</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_2-创建-pup-模式" tabindex="-1"><a class="header-anchor" href="#_2-创建-pup-模式"><span>2. 创建 PUP 模式</span></a></h3>
<p>将 Web 应用打包为 PUP 文件。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">输入文件</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">夹</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">输出文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> [-p </span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">&#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">密</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">码</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">]</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<table>
<thead>
<tr>
<th>参数</th>
<th>必需</th>
<th>说明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code v-pre>--create-pup</code></td>
<td>是</td>
<td>指定创建 PUP 文件模式</td>
</tr>
<tr>
<td><code v-pre>-i</code> 或 <code v-pre>--input</code></td>
<td>是</td>
<td>源文件夹路径</td>
</tr>
<tr>
<td><code v-pre>-o</code> 或 <code v-pre>--output</code></td>
<td>是</td>
<td>输出 PUP 文件路径</td>
</tr>
<tr>
<td><code v-pre>-p</code> 或 <code v-pre>--password</code></td>
<td>否</td>
<td>ZIP 密码（可选）</td>
</tr>
</tbody>
</table>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 基本用法</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用密码</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用长参数名</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --input</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --output</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --password</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><div class="hint-container tip">
<p class="hint-container-title">提示</p>
<p>如果未指定密码，系统会自动生成随机密码。</p>
</div>
<h3 id="_3-加载-pup-模式" tabindex="-1"><a class="header-anchor" href="#_3-加载-pup-模式"><span>3. 加载 PUP 模式</span></a></h3>
<p>加载并运行 PUP 文件。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文件.pu</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<table>
<thead>
<tr>
<th>参数</th>
<th>必需</th>
<th>说明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code v-pre>--load-pup</code></td>
<td>是</td>
<td>指定加载 PUP 文件模式</td>
</tr>
<tr>
<td><code v-pre>&lt;文件.pup&gt;</code></td>
<td>是</td>
<td>PUP 文件路径</td>
</tr>
</tbody>
</table>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 基本用法</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用相对路径</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用环境变量</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%APPDATA%\MyApp\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="_4-裸文件夹模式" tabindex="-1"><a class="header-anchor" href="#_4-裸文件夹模式"><span>4. 裸文件夹模式</span></a></h3>
<p>直接从文件夹加载应用（不打包为 PUP 文件）。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --nake-load</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> &#x3C;</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文件夹路</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">径</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">></span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>参数说明</strong>：</p>
<table>
<thead>
<tr>
<th>参数</th>
<th>必需</th>
<th>说明</th>
</tr>
</thead>
<tbody>
<tr>
<td><code v-pre>--nake-load</code></td>
<td>是</td>
<td>指定裸文件夹模式</td>
</tr>
<tr>
<td><code v-pre>&lt;文件夹路径&gt;</code></td>
<td>是</td>
<td>Web 应用文件夹路径</td>
</tr>
</tbody>
</table>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 基本用法</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --nake-load</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用相对路径</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --nake-load</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.\dist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 开发时使用（支持热重载）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --nake-load</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><div class="hint-container tip">
<p class="hint-container-title">开发建议</p>
<p>裸文件夹模式支持热重载，修改文件后刷新即可看到变化，非常适合开发调试。</p>
</div>
<h2 id="参数详解" tabindex="-1"><a class="header-anchor" href="#参数详解"><span>参数详解</span></a></h2>
<h3 id="输入参数-i-input" tabindex="-1"><a class="header-anchor" href="#输入参数-i-input"><span>输入参数（-i, --input）</span></a></h3>
<p>指定源文件夹路径。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>注意事项</strong>：</p>
<ul>
<li>路径必须存在</li>
<li>路径可以包含空格（使用引号）</li>
<li>支持绝对路径和相对路径</li>
<li>支持环境变量</li>
</ul>
<h3 id="输出参数-o-output" tabindex="-1"><a class="header-anchor" href="#输出参数-o-output"><span>输出参数（-o, --output）</span></a></h3>
<p>指定输出文件路径。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>注意事项</strong>：</p>
<ul>
<li>输出目录必须存在</li>
<li>文件扩展名必须是 <code v-pre>.pup</code></li>
<li>如果文件已存在，会被覆盖</li>
<li>路径可以包含空格（使用引号）</li>
</ul>
<h3 id="密码参数-p-password" tabindex="-1"><a class="header-anchor" href="#密码参数-p-password"><span>密码参数（-p, --password）</span></a></h3>
<p>指定 ZIP 文件的加密密码。</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MyPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>注意事项</strong>：</p>
<ul>
<li>密码区分大小写</li>
<li>支持特殊字符</li>
<li>如果未指定，系统会自动生成随机密码</li>
<li>加载 PUP 文件时不需要提供密码（密码已加密存储在文件中）</li>
</ul>
<h2 id="配置文件" tabindex="-1"><a class="header-anchor" href="#配置文件"><span>配置文件</span></a></h2>
<p>除了命令行参数，Puppet 还支持通过配置文件设置默认值。</p>
<h3 id="puppet-ini" tabindex="-1"><a class="header-anchor" href="#puppet-ini"><span>puppet.ini</span></a></h3>
<p>位于 Puppet.exe 同级目录的配置文件：</p>
<div class="language-ini line-numbers-mode" data-highlighter="shiki" data-ext="ini" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-ini"><span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 默认加载的 PUP 文件路径</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">file</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">app.pup</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">server</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 服务器端口（默认自动选择）</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">port</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">7738</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">[</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">security</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">; 是否启用严格模式</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">strict</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">true</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><p><strong>配置项说明</strong>：</p>
<table>
<thead>
<tr>
<th>配置项</th>
<th>说明</th>
<th>默认值</th>
</tr>
</thead>
<tbody>
<tr>
<td><code v-pre>file</code></td>
<td>默认加载的 PUP 文件</td>
<td>无</td>
</tr>
<tr>
<td><code v-pre>port</code></td>
<td>HTTP 服务器端口</td>
<td>7738（自动选择）</td>
</tr>
<tr>
<td><code v-pre>strict</code></td>
<td>严格模式</td>
<td>true</td>
</tr>
</tbody>
</table>
<p><strong>使用方式</strong>：</p>
<p>配置文件中的设置会在 GUI 模式下使用：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用配置文件中的设置</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="命令行组合" tabindex="-1"><a class="header-anchor" href="#命令行组合"><span>命令行组合</span></a></h2>
<h3 id="常见组合" tabindex="-1"><a class="header-anchor" href="#常见组合"><span>常见组合</span></a></h3>
<h4 id="开发工作流" tabindex="-1"><a class="header-anchor" href="#开发工作流"><span>开发工作流</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 1. 开发时使用裸文件夹模式</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --nake-load</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 2. 测试打包</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject\dist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 3. 测试 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="批量打包" tabindex="-1"><a class="header-anchor" href="#批量打包"><span>批量打包</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># Windows 批处理脚本</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">@echo</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> off</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">set</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> SOURCE=C:</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076">\P</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">rojects</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">set</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> OUTPUT=C:</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076">\R</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">eleases</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%SOURCE%\App1</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%OUTPUT%\App1.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%SOURCE%\App2</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%OUTPUT%\App2.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%SOURCE%\App3</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%OUTPUT%\App3.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">echo</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> 打包完成！</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">pause</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="自动化构建" tabindex="-1"><a class="header-anchor" href="#自动化构建"><span>自动化构建</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># PowerShell 脚本</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">$source</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> = </span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyProject\dist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">$output</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> = </span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\Releases\MyApp_</span><span style="--shiki-light:#999999;--shiki-dark:#666666">$((</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">Get-Date</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.ToString</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665">'yyyyMMdd'</span><span style="--shiki-light:#999999;--shiki-dark:#666666">))</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">&#x26;</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> "puppet.exe"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> $source</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> $output</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -p</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">MySecretPassword</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">$LASTEXITCODE</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> -eq 0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">打包成功：$output</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> else</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">    Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">打包失败</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    exit</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="错误处理" tabindex="-1"><a class="header-anchor" href="#错误处理"><span>错误处理</span></a></h2>
<h3 id="常见错误" tabindex="-1"><a class="header-anchor" href="#常见错误"><span>常见错误</span></a></h3>
<h4 id="_1-输入文件夹不存在" tabindex="-1"><a class="header-anchor" href="#_1-输入文件夹不存在"><span>1. &quot;输入文件夹不存在&quot;</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\NonExistent</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">output.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>原因</strong>：指定的输入文件夹不存在</p>
<p><strong>解决方案</strong>：检查路径是否正确</p>
<h4 id="_2-输出目录不存在" tabindex="-1"><a class="header-anchor" href="#_2-输出目录不存在"><span>2. &quot;输出目录不存在&quot;</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\NonExistent\output.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>原因</strong>：输出目录不存在</p>
<p><strong>解决方案</strong>：先创建输出目录</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">mkdir</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> C:</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076">\N</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">onExistent</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\NonExistent\output.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h4 id="_3-无效的-pup-文件" tabindex="-1"><a class="header-anchor" href="#_3-无效的-pup-文件"><span>3. &quot;无效的 PUP 文件&quot;</span></a></h4>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">invalid.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div></div></div><p><strong>原因</strong>：文件格式不正确或已损坏</p>
<p><strong>解决方案</strong>：重新创建 PUP 文件</p>
<h4 id="_4-端口已被占用" tabindex="-1"><a class="header-anchor" href="#_4-端口已被占用"><span>4. &quot;端口已被占用&quot;</span></a></h4>
<p><strong>原因</strong>：指定或自动选择的端口已被其他程序占用</p>
<p><strong>解决方案</strong>：系统会自动尝试下一个端口，或手动指定其他端口</p>
<h2 id="环境变量" tabindex="-1"><a class="header-anchor" href="#环境变量"><span>环境变量</span></a></h2>
<p>Puppet 支持使用环境变量指定路径：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用 APPDATA 环境变量</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%APPDATA%\MyApp\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 使用自定义环境变量</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">set</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> MY_APP_PATH=C:</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076">\M</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">yApp</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">%MY_APP_PATH%\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="路径规范" tabindex="-1"><a class="header-anchor" href="#路径规范"><span>路径规范</span></a></h2>
<h3 id="windows-路径" tabindex="-1"><a class="header-anchor" href="#windows-路径"><span>Windows 路径</span></a></h3>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 绝对路径</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 相对路径</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 包含空格的路径（使用引号）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\My Folder\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="网络路径" tabindex="-1"><a class="header-anchor" href="#网络路径"><span>网络路径</span></a></h3>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># UNC 路径</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076">\\</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">server\share\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 映射驱动器</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --load-pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">Z:\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="特殊字符" tabindex="-1"><a class="header-anchor" href="#特殊字符"><span>特殊字符</span></a></h3>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 路径包含特殊字符（使用引号）</span></span>
<span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\My App (2024)</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\Output\app.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="返回码" tabindex="-1"><a class="header-anchor" href="#返回码"><span>返回码</span></a></h2>
<p>Puppet 命令行工具返回以下退出码：</p>
<table>
<thead>
<tr>
<th>返回码</th>
<th>说明</th>
</tr>
</thead>
<tbody>
<tr>
<td>0</td>
<td>成功</td>
</tr>
<tr>
<td>1</td>
<td>一般错误</td>
</tr>
<tr>
<td>2</td>
<td>参数错误</td>
</tr>
<tr>
<td>3</td>
<td>文件不存在</td>
</tr>
<tr>
<td>4</td>
<td>权限错误</td>
</tr>
<tr>
<td>5</td>
<td>格式错误</td>
</tr>
</tbody>
</table>
<p><strong>示例</strong>：</p>
<div class="language-bash line-numbers-mode" data-highlighter="shiki" data-ext="bash" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-bash"><span class="line"><span style="--shiki-light:#59873A;--shiki-dark:#80A665">puppet.exe</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> --create-pup</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -i</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#A65E2B;--shiki-dark:#C99076"> -o</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">C:\MyApp.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#59873A;--shiki-dark:#80A665"> %ERRORLEVEL%</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> EQU</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 0</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> (</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    echo</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> 打包成功</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">) else (</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    echo</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> 打包失败，错误码：%ERRORLEVEL%</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">)</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="最佳实践" tabindex="-1"><a class="header-anchor" href="#最佳实践"><span>最佳实践</span></a></h2>
<h3 id="_1-路径处理" tabindex="-1"><a class="header-anchor" href="#_1-路径处理"><span>1. 路径处理</span></a></h3>
<ul>
<li>使用引号包裹包含空格的路径</li>
<li>优先使用绝对路径</li>
<li>验证路径存在性后再使用</li>
</ul>
<h3 id="_2-密码管理" tabindex="-1"><a class="header-anchor" href="#_2-密码管理"><span>2. 密码管理</span></a></h3>
<ul>
<li>不要在命令历史中暴露密码</li>
<li>使用环境变量或配置文件存储密码</li>
<li>定期更换密码</li>
</ul>
<h3 id="_3-自动化脚本" tabindex="-1"><a class="header-anchor" href="#_3-自动化脚本"><span>3. 自动化脚本</span></a></h3>
<ul>
<li>检查返回码以确定操作是否成功</li>
<li>添加适当的错误处理</li>
<li>记录操作日志</li>
</ul>
<h3 id="_4-开发流程" tabindex="-1"><a class="header-anchor" href="#_4-开发流程"><span>4. 开发流程</span></a></h3>
<ul>
<li>开发时使用裸文件夹模式</li>
<li>测试时创建 PUP 文件</li>
<li>发布时验证 PUP 文件</li>
</ul>
<h2 id="示例脚本" tabindex="-1"><a class="header-anchor" href="#示例脚本"><span>示例脚本</span></a></h2>
<h3 id="完整的构建脚本-powershell" tabindex="-1"><a class="header-anchor" href="#完整的构建脚本-powershell"><span>完整的构建脚本（PowerShell）</span></a></h3>
<div class="language-powershell line-numbers-mode" data-highlighter="shiki" data-ext="powershell" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-powershell"><span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># build.ps1</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">param</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    [</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Source</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.\dist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">,</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    [</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Output</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">.\releases</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">,</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    [</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">string</span><span style="--shiki-light:#999999;--shiki-dark:#666666">]$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Password</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> $</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">env:</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">PUPPET_PASSWORD</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 创建输出目录</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">-not</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">Test-Path</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> $</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Output</span><span style="--shiki-light:#999999;--shiki-dark:#666666">))</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    New-Item</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">ItemType Directory </span><span style="--shiki-light:#999999;--shiki-dark:#666666">-</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">Path </span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Output</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> |</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965"> Out-Null</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 生成文件名</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">version</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">Get-Date</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">.ToString</span><span style="--shiki-light:#999999;--shiki-dark:#666666">(</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">yyyyMMddHHmmss</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">outputFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965"> Join-Path</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> $</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Output</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">app_</span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">version.pup</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD"># 构建 PUP 文件</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">正在构建 PUP 文件...</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">ForegroundColor Cyan</span></span>
<span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">&#x26;</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">puppet.exe</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> --</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">create</span><span style="--shiki-light:#999999;--shiki-dark:#666666">-</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">pup </span><span style="--shiki-light:#999999;--shiki-dark:#666666">-</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">i </span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Source</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">o </span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">outputFile</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">p </span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">Password</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> ($</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965">LASTEXITCODE</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> -eq</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 0</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">构建成功：</span><span style="--shiki-light:#999999;--shiki-dark:#666666">$</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">outputFile</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">ForegroundColor Green</span></span>
<span class="line"><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">    </span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">    # 显示文件信息</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">    $</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">fileInfo</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> =</span><span style="--shiki-light:#998418;--shiki-dark:#B8A965"> Get-Item</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> $</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A">outputFile</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">文件大小：</span><span style="--shiki-light:#999999;--shiki-dark:#666666">$($</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">fileInfo.Length </span><span style="--shiki-light:#999999;--shiki-dark:#666666">/</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">MB</span><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D"> MB</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> else</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> {</span></span>
<span class="line"><span style="--shiki-light:#998418;--shiki-dark:#B8A965">    Write-Host</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">构建失败</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> -</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">ForegroundColor Red</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    exit</span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91"> 1</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">}</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="快速启动脚本-批处理" tabindex="-1"><a class="header-anchor" href="#快速启动脚本-批处理"><span>快速启动脚本（批处理）</span></a></h3>
<div class="language-batch line-numbers-mode" data-highlighter="shiki" data-ext="batch" style="--shiki-light:#393a34;--shiki-dark:#dbd7caee;--shiki-light-bg:#ffffff;--shiki-dark-bg:#121212"><pre class="shiki shiki-themes vitesse-light vitesse-dark vp-code" v-pre=""><code class="language-batch"><span class="line"><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676">@</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">echo</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> off</span></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">:: start.bat</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">:: 设置路径</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">set</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> PUPPET_EXE</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">C:\Puppet\puppet.exe</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">set</span><span style="--shiki-light:#B07D48;--shiki-dark:#BD976A"> APP_PATH</span><span style="--shiki-light:#999999;--shiki-dark:#666666">=</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE">C:\MyApp</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">:: 检查文件是否存在</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> not</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> exist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUPPET_EXE</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    echo</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> Puppet.exe 未找到</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    pause</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    exit</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> /b </span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91">1</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">if</span><span style="--shiki-light:#AB5959;--shiki-dark:#CB7676"> not</span><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375"> exist</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77"> "</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">APP_PATH</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666"> (</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    echo</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> 应用路径不存在</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    pause</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">    exit</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> /b </span><span style="--shiki-light:#2F798A;--shiki-dark:#4C9A91">1</span></span>
<span class="line"><span style="--shiki-light:#999999;--shiki-dark:#666666">)</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#A0ADA0;--shiki-dark:#758575DD">:: 启动应用</span></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">echo</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> 正在启动 Puppet 应用...</span></span>
<span class="line"><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">PUPPET_EXE</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#393A34;--shiki-dark:#DBD7CAEE"> --nake-load </span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B56959;--shiki-dark:#C98A7D">APP_PATH</span><span style="--shiki-light:#999999;--shiki-dark:#666666">%</span><span style="--shiki-light:#B5695977;--shiki-dark:#C98A7D77">"</span></span>
<span class="line"></span>
<span class="line"><span style="--shiki-light:#1E754F;--shiki-dark:#4D9375">pause</span></span></code></pre>
<div class="line-numbers" aria-hidden="true" style="counter-reset:line-number 0"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h2 id="相关资源" tabindex="-1"><a class="header-anchor" href="#相关资源"><span>相关资源</span></a></h2>
<ul>
<li><VPLink href="./pup-format.md">PUP 文件格式</VPLink> - PUP 文件结构说明</li>
<li><VPLink href="./project-structure.md">项目结构</VPLink> - 项目目录组织</li>
<li><VPLink href="./getting-started.md">快速开始</VPLink> - 快速上手指南</li>
</ul>
<h2 id="下一步" tabindex="-1"><a class="header-anchor" href="#下一步"><span>下一步</span></a></h2>
<p>了解命令行参数后，建议：</p>
<ol>
<li>尝试不同的运行模式</li>
<li>创建自动化构建脚本</li>
<li>参考 <VPLink href="./best-practices.md">最佳实践</VPLink> 优化工作流</li>
</ol>
</div></template>


