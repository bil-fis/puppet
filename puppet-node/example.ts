/**
 * Puppet Framework TypeScript 示例
 * 
 * 这个示例展示了如何使用 @puppet-framework/types 进行类型安全的 Puppet 应用开发
 */

import puppet from './puppet';

// ============================================================================
// 应用初始化
// ============================================================================

/**
 * 初始化应用
 */
async function initializeApp(): Promise<void> {
    console.log('初始化 Puppet 应用...');
    
    try {
        // 设置窗口样式
        await setupWindow();
        
        // 加载配置
        const config = await loadConfig();
        
        // 显示主界面
        await showMainWindow();
        
        puppet.log.info('应用初始化完成');
    } catch (error) {
        puppet.log.error('应用初始化失败:', error);
        throw error;
    }
}

// ============================================================================
// 窗口设置
// ============================================================================

/**
 * 设置窗口样式
 */
async function setupWindow(): Promise<void> {
    // 设置为无边框窗口
    await puppet.window.setBorderless(true);
    
    // 启用拖动
    await puppet.window.setDraggable(true);
    
    // 启用缩放
    await puppet.window.setResizable(true);
    
    // 设置透明度
    await puppet.window.setOpacity(0.95);
    
    // 居中窗口
    await puppet.window.centerWindow();
    
    // 设置窗口置顶（可选）
    await puppet.window.setTopmost(false);
}

// ============================================================================
// 配置管理
// ============================================================================

interface AppConfig {
    theme: 'light' | 'dark';
    fontSize: number;
    language: 'zh-CN' | 'en-US';
    autoUpdate: boolean;
}

/**
 * 加载配置
 */
async function loadConfig(): Promise<AppConfig> {
    try {
        const content = await puppet.fs.readFileAsJson('config.json') as AppConfig;
        
        // 验证配置
        if (!isValidConfig(content)) {
            return getDefaultConfig();
        }
        
        return content;
    } catch (error) {
        puppet.log.warn('加载配置失败，使用默认配置');
        return getDefaultConfig();
    }
}

/**
 * 保存配置
 */
async function saveConfig(config: AppConfig): Promise<void> {
    try {
        await puppet.fs.writeTextToFile(
            'config.json',
            JSON.stringify(config, null, 2)
        );
        puppet.log.info('配置已保存');
    } catch (error) {
        puppet.log.error('保存配置失败:', error);
        throw error;
    }
}

/**
 * 验证配置
 */
function isValidConfig(config: any): config is AppConfig {
    return (
        typeof config === 'object' &&
        (config.theme === 'light' || config.theme === 'dark') &&
        typeof config.fontSize === 'number' &&
        (config.language === 'zh-CN' || config.language === 'en-US') &&
        typeof config.autoUpdate === 'boolean'
    );
}

/**
 * 获取默认配置
 */
function getDefaultConfig(): AppConfig {
    return {
        theme: 'light',
        fontSize: 14,
        language: 'zh-CN',
        autoUpdate: true
    };
}

// ============================================================================
// 主界面
// ============================================================================

/**
 * 显示主窗口
 */
async function showMainWindow(): Promise<void> {
    // 获取窗口信息
    const windowInfo = await puppet.application.getWindowInfo();
    console.log('窗口信息:', windowInfo);
    
    // 显示窗口（如果需要）
    await puppet.window.showInTaskbar(true);
}

// ============================================================================
// 文件操作示例
// ============================================================================

/**
 * 打开文件
 */
async function openFile(): Promise<string | null> {
    try {
        const files = await puppet.fs.openFileDialog(
            ['文本文件', '*.txt'],
            false
        );
        
        if (files.length > 0) {
            const content = await puppet.fs.readFileAsText(files[0]);
            console.log('文件内容:', content);
            return content;
        }
        
        return null;
    } catch (error) {
        puppet.log.error('打开文件失败:', error);
        return null;
    }
}

/**
 * 保存文件
 */
async function saveFile(content: string): Promise<boolean> {
    try {
        const files = await puppet.fs.openFileDialog(
            ['文本文件', '*.txt'],
            false
        );
        
        if (files.length > 0) {
            await puppet.fs.writeTextToFile(files[0], content);
            puppet.log.info('文件已保存');
            return true;
        }
        
        return false;
    } catch (error) {
        puppet.log.error('保存文件失败:', error);
        return false;
    }
}

// ============================================================================
// 系统功能示例
// ============================================================================

/**
 * 获取系统信息
 */
async function getSystemInfo(): Promise<void> {
    try {
        const sysInfo = await puppet.system.getSystemInfo();
        
        console.log('系统信息:');
        console.log(`  操作系统: ${sysInfo.osName} ${sysInfo.osVersion}`);
        console.log(`  计算机名: ${sysInfo.computerName}`);
        console.log(`  CPU: ${sysInfo.cpuModel} (${sysInfo.cpuCores} 核)`);
        console.log(`  内存: ${sysInfo.availableMemory} / ${sysInfo.totalMemory} MB`);
        console.log(`  GPU: ${sysInfo.gpuModel}`);
        console.log(`  屏幕: ${sysInfo.screenWidth} x ${sysInfo.screenHeight}`);
        console.log(`  架构: ${sysInfo.is64Bit ? '64 位' : '32 位'}`);
    } catch (error) {
        puppet.log.error('获取系统信息失败:', error);
    }
}

/**
 * 截取屏幕
 */
async function takeScreenshot(): Promise<string | null> {
    try {
        const screenshot = await puppet.system.takeScreenShot();
        
        // 显示截图（示例）
        const img = document.createElement('img');
        img.src = 'data:image/png;base64,' + screenshot;
        document.body.appendChild(img);
        
        // 保存截图
        await puppet.fs.writeByteToFile('screenshot.png', screenshot);
        puppet.log.info('截图已保存');
        
        return screenshot;
    } catch (error) {
        puppet.log.error('截图失败:', error);
        return null;
    }
}

// ============================================================================
// 事件处理示例
// ============================================================================

/**
 * 设置事件监听
 */
async function setupEventListeners(): Promise<void> {
    // 监听 USB 设备插入
    const usbListenerId = await puppet.events.addEventListener('usb-plug-in', async (event) => {
        console.log('USB 设备插入:', event.data);
        
        // 显示通知
        await puppet.tray.showBalloon(
            'USB 设备插入',
            event.data.DeviceName,
            10000,
            'Info'
        );
        
        // 记录日志
        puppet.log.info(`USB 设备插入: ${event.data.DeviceName} (${event.data.DriveLetter})`);
    });
    
    // 监听窗口最大化
    const maximizeListenerId = await puppet.events.addEventListener('window-maximize', () => {
        console.log('窗口已最大化');
        puppet.log.info('窗口状态: 最大化');
    });
    
    // 监听电源变化
    const powerListenerId = await puppet.events.addEventListener('power-change', async (event) => {
        console.log('电源状态变化:', event.data);
        
        if (event.data.PowerStatus === 'Battery') {
            await puppet.tray.showBalloon(
                '电源切换',
                '已切换到电池供电',
                5000,
                'Info'
            );
        }
    });
    
    console.log('事件监听器已设置');
}

// ============================================================================
// 托盘图标示例
// ============================================================================

/**
 * 设置托盘图标
 */
async function setupTrayIcon(): Promise<void> {
    try {
        // 创建托盘图标
        await puppet.tray.setTray('Puppet 应用');
        
        // 设置菜单
        await puppet.tray.setMenu([
            { Text: '显示窗口', Command: 'show' },
            { Text: '隐藏窗口', Command: 'hide' },
            { Text: '-', Command: 'separator' },
            { Text: '设置', Command: 'settings' },
            { Text: '关于', Command: 'about' },
            { Text: '-', Command: 'separator' },
            { Text: '退出', Command: 'exit' }
        ]);
        
        // 设置单击事件
        await puppet.tray.onClick(async (command) => {
            console.log('托盘被点击，命令:', command);
            
            switch (command) {
                case 'show':
                    await showMainWindow();
                    break;
                case 'hide':
                    await puppet.window.showInTaskbar(false);
                    break;
                case 'settings':
                    openSettings();
                    break;
                case 'about':
                    showAboutDialog();
                    break;
                case 'exit':
                    await puppet.application.close();
                    break;
            }
        });
        
        // 设置双击事件
        await puppet.tray.onDoubleClick(async () => {
            console.log('托盘被双击');
            await showMainWindow();
        });
        
        puppet.log.info('托盘图标已设置');
    } catch (error) {
        puppet.log.error('设置托盘图标失败:', error);
    }
}

/**
 * 打开设置对话框
 */
function openSettings(): void {
    alert('打开设置对话框');
}

/**
 * 显示关于对话框
 */
function showAboutDialog(): void {
    alert('Puppet Framework v1.0.0\n基于 WebView2 的桌面应用开发框架');
}

// ============================================================================
// 设备监控示例
// ============================================================================

/**
 * 获取磁盘信息
 */
async function getDiskInfo(): Promise<void> {
    try {
        // 获取本地磁盘
        const localDisks = await puppet.device.getDevices(puppet.device.type.localDisk);
        
        console.log('本地磁盘:');
        for (const disk of localDisks) {
            console.log(`  ${disk.DriveLetter} - ${disk.VolumeName || '无标签'}`);
            console.log(`    总大小: ${formatBytes(disk.TotalSize)}`);
            console.log(`    可用空间: ${formatBytes(disk.FreeSpace)}`);
            console.log(`    使用率: ${((disk.UsedSpace / disk.TotalSize) * 100).toFixed(1)}%`);
        }
    } catch (error) {
        puppet.log.error('获取磁盘信息失败:', error);
    }
}

/**
 * 格式化字节数
 */
function formatBytes(bytes: number): string {
    const units = ['B', 'KB', 'MB', 'GB', 'TB'];
    let size = bytes;
    let unitIndex = 0;
    
    while (size >= 1024 && unitIndex < units.length - 1) {
        size /= 1024;
        unitIndex++;
    }
    
    return `${size.toFixed(2)} ${units[unitIndex]}`;
}

// ============================================================================
// 主函数
// ============================================================================

/**
 * 主函数
 */
async function main(): Promise<void> {
    console.log('=== Puppet 应用启动 ===');
    
    try {
        // 初始化应用
        await initializeApp();
        
        // 设置事件监听
        await setupEventListeners();
        
        // 设置托盘图标
        await setupTrayIcon();
        
        // 获取系统信息
        await getSystemInfo();
        
        // 获取磁盘信息
        await getDiskInfo();
        
        console.log('=== 应用启动完成 ===');
    } catch (error) {
        console.error('应用启动失败:', error);
        puppet.log.error('应用启动失败');
    }
}

// 启动应用
main().catch(error => {
    console.error('未捕获的错误:', error);
    puppet.log.error('未捕获的错误');
});

// ============================================================================
// 导出
// ============================================================================

export {
    initializeApp,
    setupWindow,
    loadConfig,
    saveConfig,
    openFile,
    saveFile,
    getSystemInfo,
    takeScreenshot,
    setupEventListeners,
    setupTrayIcon,
    getDiskInfo
};