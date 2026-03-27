using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace puppet.Controllers
{
    /// <summary>
    /// 设备状态枚举
    /// 基于 Microsoft Learn Win32_LogicalDisk 和 Win32_PnPEntity 文档
    /// </summary>
    public enum DeviceStatus
    {
        // CIM_LogicalDevice Status 属性
        Unknown = 0,
        OK = 1,
        Error = 2,
        Degraded = 3,
        PredFail = 4,
        Starting = 5,
        Stopping = 6,
        Service = 7,
        Stressed = 8,
        NonRecover = 9,
        NoContact = 10,
        LostComm = 11,
        
        // ConfigManagerErrorCode 映射
        NotConfigured = 20,
        Disabled = 22,
        NotPresent = 24,
        StillSettingUp = 25,
        DriversNotInstalled = 28,
        
        // Win32_LogicalDisk 特定状态
        Ready = 100,
        NotReady = 101,
        Pending = 102,
        Ejected = 103,
        Stalled = 104
    }

    /// <summary>
    /// 设备类型枚举
    /// 基于 Microsoft Learn Win32_LogicalDisk DriveType 和 Win32_PnPEntity PNPClass
    /// </summary>
    public enum DeviceType
    {
        Unknown = 0,
        
        // 磁盘驱动器类型 (Win32_LogicalDisk DriveType)
        RemovableDisk = 2,        // 移动磁盘 (USB闪存驱动器等)
        LocalDisk = 3,           // 本地磁盘 (硬盘)
        NetworkDrive = 4,        // 网络驱动器
        CompactDisc = 5,         // 光盘 (CD/DVD/Blu-ray)
        RAMDisk = 6,             // RAM磁盘
        
        // USB 设备类型 (基于 PNPClass)
        USBDevice = 100,         // USB 设备
        USBDisk = 101,           // USB 磁盘
        USBHub = 102,            // USB 集线器
        USBPrinter = 103,        // USB 打印机
        USBCamera = 104,         // USB 摄像头
        USBStorage = 105,        // USB 存储设备
        
        // 其他常见设备类型
        Keyboard = 200,
        Mouse = 201,
        Monitor = 202,
        Printer = 203,
        Scanner = 204,
        NetworkAdapter = 205,
        Audio = 206,
        Video = 207,
        Bluetooth = 208
    }

    /// <summary>
    /// 电源状态枚举
    /// 基于 Microsoft Learn Win32_Battery BatteryStatus
    /// </summary>
    public enum PowerStatus
    {
        Unknown = 0,
        Discharging = 1,         // 放电中
        ACConnected = 2,         // 连接交流电源
        FullyCharged = 3,        // 已充满
        Low = 4,                 // 电量低
        Critical = 5,            // 电量危急
        Charging = 6,            // 充电中
        ChargingHigh = 7,        // 充电中且电量高
        ChargingLow = 8,         // 充电中且电量低
        ChargingCritical = 9,    // 充电中且电量危急
        Undefined = 10,          // 未定义
        PartiallyCharged = 11    // 部分充电
    }

    /// <summary>
    /// 设备类
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class Device
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceStatus Status { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// PNP设备ID
        /// </summary>
        public string PNPDeviceID { get; set; }

        /// <summary>
        /// 盘符 (仅可移动存储设备)
        /// </summary>
        public string DriveLetter { get; set; }

        /// <summary>
        /// 卷标 (仅磁盘设备)
        /// </summary>
        public string VolumeName { get; set; }

        /// <summary>
        /// 总容量 (字节) (仅存储设备)
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        /// 可用空间 (字节) (仅存储设备)
        /// </summary>
        public ulong FreeSpace { get; set; }

        /// <summary>
        /// 文件系统 (仅磁盘设备)
        /// </summary>
        public string FileSystem { get; set; }

        /// <summary>
        /// 配置管理器错误代码
        /// </summary>
        public uint ConfigManagerErrorCode { get; set; }

        /// <summary>
        /// 是否在线/存在
        /// </summary>
        public bool Present { get; set; }

        /// <summary>
        /// 将设备转换为JSON字符串
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// 事件控制器
    /// 管理 JavaScript 事件监听器和回调
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class EventController
    {
        private CoreWebView2 _webview;
        private Form _form;
        
        // 事件监听器存储
        private ConcurrentDictionary<string, List<ulong>> _eventListeners = 
            new ConcurrentDictionary<string, List<ulong>>();
        
        // 回调函数存储
        private ConcurrentDictionary<ulong, string> _callbacks = 
            new ConcurrentDictionary<ulong, string>();
        
        // 回调ID计数器
        private ulong _nextCallbackId = 1;
        
        // 事件监听状态
        private ConcurrentDictionary<string, bool> _eventMonitoringStatus = 
            new ConcurrentDictionary<string, bool>();
        
        // USB设备监控
        private ManagementEventWatcher _usbArrivalWatcher;
        private ManagementEventWatcher _usbRemovalWatcher;
        private HashSet<string> _existingUSBDevices = new HashSet<string>();
        
        // 磁盘监控
        private ManagementEventWatcher _diskArrivalWatcher;
        private ManagementEventWatcher _diskRemovalWatcher;
        private HashSet<string> _existingDisks = new HashSet<string>();
        
        // 电源监控
        private ManagementEventWatcher _powerStatusWatcher;
        
        // 窗口状态
        private bool _isWindowActive = false;
        private FormWindowState _lastWindowState = FormWindowState.Normal;

        public EventController(CoreWebView2 webview, Form form)
        {
            _webview = webview;
            _form = form;
            
            // 初始化现有设备列表
            InitializeExistingDevices();
        }

        /// <summary>
        /// 初始化现有设备列表
        /// </summary>
        private void InitializeExistingDevices()
        {
            try
            {
                // 获取现有USB设备
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE PNPClass LIKE 'USB%'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string deviceId = obj["DeviceID"]?.ToString();
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            _existingUSBDevices.Add(deviceId);
                        }
                    }
                }

                // 获取现有磁盘
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string deviceId = obj["DeviceID"]?.ToString();
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            _existingDisks.Add(deviceId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化现有设备失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加事件监听器
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="callbackName">回调函数名</param>
        /// <returns>回调ID</returns>
        public ulong AddEventListener(string eventName, string callbackName)
        {
            // 生成回调ID
            ulong callbackId = _nextCallbackId++;
            
            // 添加到回调字典
            _callbacks[callbackId] = callbackName;
            
            // 添加到事件监听器列表
            _eventListeners.AddOrUpdate(
                eventName,
                new List<ulong> { callbackId },
                (key, existing) =>
                {
                    lock (existing)
                    {
                        existing.Add(callbackId);
                    }
                    return existing;
                }
            );
            
            // 如果是第一个监听器，启动对应的监控
            if (!_eventMonitoringStatus.ContainsKey(eventName) || 
                !_eventMonitoringStatus[eventName])
            {
                StartEventMonitoring(eventName);
            }
            
            return callbackId;
        }

        /// <summary>
        /// 移除事件监听器
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="callbackId">回调ID</param>
        public void RemoveEventListener(string eventName, ulong callbackId)
        {
            // 从回调字典移除
            _callbacks.TryRemove(callbackId, out _);
            
            // 从事件监听器列表移除
            if (_eventListeners.TryGetValue(eventName, out var listeners))
            {
                lock (listeners)
                {
                    listeners.Remove(callbackId);
                }
                
                // 如果没有监听器了，停止监控
                if (listeners.Count == 0)
                {
                    _eventListeners.TryRemove(eventName, out _);
                    StopEventMonitoring(eventName);
                }
            }
        }

        /// <summary>
        /// 启动事件监控
        /// </summary>
        /// <param name="eventName">事件名称</param>
        private void StartEventMonitoring(string eventName)
        {
            _eventMonitoringStatus[eventName] = true;
            
            switch (eventName)
            {
                case "usb-plug-in":
                    StartUSBMonitoring();
                    break;
                case "usb-plug-out":
                    StartUSBMonitoring();
                    break;
                case "disk-mount":
                    StartDiskMonitoring();
                    break;
                case "disk-unmount":
                    StartDiskMonitoring();
                    break;
                case "power-change":
                    StartPowerMonitoring();
                    break;
                case "window-focus":
                    StartWindowMonitoring();
                    break;
                case "window-blur":
                    StartWindowMonitoring();
                    break;
                case "window-maximize":
                case "window-minimize":
                case "window-restore":
                    StartWindowStateMonitoring();
                    break;
                case "window-resize":
                    StartWindowResizeMonitoring();
                    break;
                case "window-move":
                    StartWindowMoveMonitoring();
                    break;
            }
        }

        /// <summary>
        /// 停止事件监控
        /// </summary>
        /// <param name="eventName">事件名称</param>
        private void StopEventMonitoring(string eventName)
        {
            _eventMonitoringStatus[eventName] = false;
            
            // 检查是否还有其他相关的监听器
            bool hasRelatedListeners = CheckRelatedListeners(eventName);
            
            if (!hasRelatedListeners)
            {
                switch (eventName)
                {
                    case "usb-plug-in":
                    case "usb-plug-out":
                        StopUSBMonitoring();
                        break;
                    case "disk-mount":
                    case "disk-unmount":
                        StopDiskMonitoring();
                        break;
                    case "power-change":
                        StopPowerMonitoring();
                        break;
                    case "window-focus":
                    case "window-blur":
                        StopWindowMonitoring();
                        break;
                    case "window-maximize":
                    case "window-minimize":
                    case "window-restore":
                        StopWindowStateMonitoring();
                        break;
                    case "window-resize":
                        StopWindowResizeMonitoring();
                        break;
                    case "window-move":
                        StopWindowMoveMonitoring();
                        break;
                }
            }
        }

        /// <summary>
        /// 检查是否有相关的监听器
        /// </summary>
        private bool CheckRelatedListeners(string eventName)
        {
            var relatedEvents = GetRelatedEvents(eventName);
            return relatedEvents.Any(e => 
                _eventListeners.ContainsKey(e) && _eventListeners[e].Count > 0);
        }

        /// <summary>
        /// 获取相关事件
        /// </summary>
        private List<string> GetRelatedEvents(string eventName)
        {
            return eventName switch
            {
                "usb-plug-in" => new List<string> { "usb-plug-in", "usb-plug-out" },
                "usb-plug-out" => new List<string> { "usb-plug-in", "usb-plug-out" },
                "disk-mount" => new List<string> { "disk-mount", "disk-unmount" },
                "disk-unmount" => new List<string> { "disk-mount", "disk-unmount" },
                "window-focus" => new List<string> { "window-focus", "window-blur" },
                "window-blur" => new List<string> { "window-focus", "window-blur" },
                "window-maximize" => new List<string> { "window-maximize", "window-minimize", "window-restore" },
                "window-minimize" => new List<string> { "window-maximize", "window-minimize", "window-restore" },
                "window-restore" => new List<string> { "window-maximize", "window-minimize", "window-restore" },
                _ => new List<string> { eventName }
            };
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventData">事件数据</param>
        private async void TriggerEvent(string eventName, object eventData)
        {
            if (!_eventListeners.TryGetValue(eventName, out var listeners) || 
                listeners.Count == 0)
            {
                return;
            }
            
            List<ulong> callbacksToExecute;
            lock (listeners)
            {
                callbacksToExecute = listeners.ToList();
            }
            
            // 按注册顺序执行回调
            foreach (var callbackId in callbacksToExecute)
            {
                if (_callbacks.TryGetValue(callbackId, out var callbackName))
                {
                    await ExecuteCallback(callbackName, eventData);
                }
            }
        }

        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <param name="callbackName">回调函数名</param>
        /// <param name="eventData">事件数据</param>
        private async System.Threading.Tasks.Task ExecuteCallback(string callbackName, object eventData)
        {
            try
            {
                string eventDataJson = JsonConvert.SerializeObject(eventData);
                string script = $@"
                    if (typeof {callbackName} === 'function') {{
                        try {{
                            {callbackName}({eventDataJson});
                        }} catch (e) {{
                            console.error('回调函数执行错误:', e);
                        }}
                    }}
                ";
                await _webview.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"执行回调失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 开始USB设备监控
        /// </summary>
        private void StartUSBMonitoring()
        {
            if (_usbArrivalWatcher != null) return;
            
            try
            {
                // 监控USB设备插入
                _usbArrivalWatcher = new ManagementEventWatcher(
                    "SELECT * FROM __InstanceCreationEvent WITHIN 2 " +
                    "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                    "AND TargetInstance.PNPClass LIKE 'USB%'");
                
                _usbArrivalWatcher.EventArrived += OnUSBArrival;
                _usbArrivalWatcher.Start();
                
                // 监控USB设备拔出
                _usbRemovalWatcher = new ManagementEventWatcher(
                    "SELECT * FROM __InstanceDeletionEvent WITHIN 2 " +
                    "WHERE TargetInstance ISA 'Win32_PnPEntity' " +
                    "AND TargetInstance.PNPClass LIKE 'USB%'");
                
                _usbRemovalWatcher.EventArrived += OnUSBRemoval;
                _usbRemovalWatcher.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"启动USB监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// USB设备插入事件
        /// </summary>
        private void OnUSBArrival(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string deviceId = obj["DeviceID"]?.ToString();
                
                if (!string.IsNullOrEmpty(deviceId) && !_existingUSBDevices.Contains(deviceId))
                {
                    _existingUSBDevices.Add(deviceId);
                    
                    Device device = CreateDeviceFromWMI(obj);
                    
                    TriggerEvent("usb-plug-in", new
                    {
                        deviceType = device.Type,
                        Device = device
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"USB插入事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// USB设备拔出事件
        /// </summary>
        private void OnUSBRemoval(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string deviceId = obj["DeviceID"]?.ToString();
                
                if (!string.IsNullOrEmpty(deviceId) && _existingUSBDevices.Contains(deviceId))
                {
                    _existingUSBDevices.Remove(deviceId);
                    
                    Device device = CreateDeviceFromWMI(obj);
                    
                    TriggerEvent("usb-plug-out", new
                    {
                        deviceType = device.Type,
                        Device = device
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"USB拔出事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 停止USB监控
        /// </summary>
        private void StopUSBMonitoring()
        {
            try
            {
                _usbArrivalWatcher?.Stop();
                _usbArrivalWatcher?.Dispose();
                _usbArrivalWatcher = null;
                
                _usbRemovalWatcher?.Stop();
                _usbRemovalWatcher?.Dispose();
                _usbRemovalWatcher = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"停止USB监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 开始磁盘监控
        /// </summary>
        private void StartDiskMonitoring()
        {
            if (_diskArrivalWatcher != null) return;
            
            try
            {
                // 监控磁盘挂载
                _diskArrivalWatcher = new ManagementEventWatcher(
                    "SELECT * FROM __InstanceCreationEvent WITHIN 2 " +
                    "WHERE TargetInstance ISA 'Win32_LogicalDisk'");
                
                _diskArrivalWatcher.EventArrived += OnDiskArrival;
                _diskArrivalWatcher.Start();
                
                // 监控磁盘卸载
                _diskRemovalWatcher = new ManagementEventWatcher(
                    "SELECT * FROM __InstanceDeletionEvent WITHIN 2 " +
                    "WHERE TargetInstance ISA 'Win32_LogicalDisk'");
                
                _diskRemovalWatcher.EventArrived += OnDiskRemoval;
                _diskRemovalWatcher.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"启动磁盘监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 磁盘挂载事件
        /// </summary>
        private void OnDiskArrival(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string deviceId = obj["DeviceID"]?.ToString();
                
                if (!string.IsNullOrEmpty(deviceId) && !_existingDisks.Contains(deviceId))
                {
                    _existingDisks.Add(deviceId);
                    
                    Device device = CreateDiskFromWMI(obj);
                    
                    TriggerEvent("disk-mount", new
                    {
                        deviceType = device.Type,
                        Device = device
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"磁盘挂载事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 磁盘卸载事件
        /// </summary>
        private void OnDiskRemoval(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string deviceId = obj["DeviceID"]?.ToString();
                
                if (!string.IsNullOrEmpty(deviceId) && _existingDisks.Contains(deviceId))
                {
                    _existingDisks.Remove(deviceId);
                    
                    Device device = CreateDiskFromWMI(obj);
                    
                    TriggerEvent("disk-unmount", new
                    {
                        deviceType = device.Type,
                        Device = device
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"磁盘卸载事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 停止磁盘监控
        /// </summary>
        private void StopDiskMonitoring()
        {
            try
            {
                _diskArrivalWatcher?.Stop();
                _diskArrivalWatcher?.Dispose();
                _diskArrivalWatcher = null;
                
                _diskRemovalWatcher?.Stop();
                _diskRemovalWatcher?.Dispose();
                _diskRemovalWatcher = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"停止磁盘监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 开始电源监控
        /// </summary>
        private void StartPowerMonitoring()
        {
            if (_powerStatusWatcher != null) return;
            
            try
            {
                _powerStatusWatcher = new ManagementEventWatcher(
                    "SELECT * FROM Win32_Battery");
                
                _powerStatusWatcher.EventArrived += OnPowerStatusChange;
                _powerStatusWatcher.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"启动电源监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 电源状态变化事件
        /// </summary>
        private void OnPowerStatusChange(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject obj = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                uint batteryStatus = Convert.ToUInt32(obj["BatteryStatus"]);
                
                PowerStatus powerStatus = (PowerStatus)batteryStatus;
                
                TriggerEvent("power-change", new
                {
                    powerStatus = powerStatus,
                    status = powerStatus.ToString()
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"电源状态变化事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 停止电源监控
        /// </summary>
        private void StopPowerMonitoring()
        {
            try
            {
                _powerStatusWatcher?.Stop();
                _powerStatusWatcher?.Dispose();
                _powerStatusWatcher = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"停止电源监控失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 开始窗口监控
        /// </summary>
        private void StartWindowMonitoring()
        {
            if (_form is Form1 form1)
            {
                form1.Activated += OnWindowActivated;
                form1.Deactivate += OnWindowDeactivate;
            }
        }

        /// <summary>
        /// 窗口激活事件
        /// </summary>
        private void OnWindowActivated(object sender, EventArgs e)
        {
            if (!_isWindowActive)
            {
                _isWindowActive = true;
                TriggerEvent("window-focus", new { focused = true });
            }
        }

        /// <summary>
        /// 窗口失焦事件
        /// </summary>
        private void OnWindowDeactivate(object sender, EventArgs e)
        {
            if (_isWindowActive)
            {
                _isWindowActive = false;
                TriggerEvent("window-blur", new { focused = false });
            }
        }

        /// <summary>
        /// 停止窗口监控
        /// </summary>
        private void StopWindowMonitoring()
        {
            if (_form is Form1 form1)
            {
                form1.Activated -= OnWindowActivated;
                form1.Deactivate -= OnWindowDeactivate;
            }
        }

        /// <summary>
        /// 开始窗口状态监控
        /// </summary>
        private void StartWindowStateMonitoring()
        {
            if (_form is Form1 form1)
            {
                _lastWindowState = form1.WindowState;
            }
        }

        /// <summary>
        /// 检查窗口状态变化
        /// </summary>
        public void CheckWindowStateChange()
        {
            if (_form is Form1 form1)
            {
                FormWindowState currentState = form1.WindowState;
                
                if (currentState != _lastWindowState)
                {
                    string eventName = currentState switch
                    {
                        FormWindowState.Maximized => "window-maximize",
                        FormWindowState.Minimized => "window-minimize",
                        FormWindowState.Normal => "window-restore",
                        _ => null
                    };
                    
                    if (eventName != null)
                    {
                        TriggerEvent(eventName, new
                        {
                            previousState = _lastWindowState.ToString(),
                            currentState = currentState.ToString()
                        });
                    }
                    
                    _lastWindowState = currentState;
                }
            }
        }

        /// <summary>
        /// 停止窗口状态监控
        /// </summary>
        private void StopWindowStateMonitoring()
        {
            // 不需要特殊处理，只是停止检查
        }

        /// <summary>
        /// 开始窗口大小监控
        /// </summary>
        private void StartWindowResizeMonitoring()
        {
            // 通过WndProc处理
        }

        /// <summary>
        /// 窗口大小变化事件
        /// </summary>
        public void OnWindowResize(int width, int height)
        {
            TriggerEvent("window-resize", new
            {
                width = width,
                height = height
            });
        }

        /// <summary>
        /// 停止窗口大小监控
        /// </summary>
        private void StopWindowResizeMonitoring()
        {
            // 不需要特殊处理
        }

        /// <summary>
        /// 开始窗口移动监控
        /// </summary>
        private void StartWindowMoveMonitoring()
        {
            // 通过WndProc处理
        }

        /// <summary>
        /// 窗口移动事件
        /// </summary>
        public void OnWindowMove(int x, int y)
        {
            TriggerEvent("window-move", new
            {
                x = x,
                y = y
            });
        }

        /// <summary>
        /// 停止窗口移动监控
        /// </summary>
        private void StopWindowMoveMonitoring()
        {
            // 不需要特殊处理
        }

        /// <summary>
        /// 从WMI对象创建设备
        /// </summary>
        private Device CreateDeviceFromWMI(ManagementBaseObject obj)
        {
            Device device = new Device
            {
                DeviceID = obj["DeviceID"]?.ToString(),
                Name = obj["Name"]?.ToString(),
                Description = obj["Description"]?.ToString(),
                Manufacturer = obj["Manufacturer"]?.ToString(),
                PNPDeviceID = obj["PNPDeviceID"]?.ToString(),
                ConfigManagerErrorCode = Convert.ToUInt32(obj["ConfigManagerErrorCode"] ?? 0),
                Present = Convert.ToBoolean(obj["Present"] ?? true)
            };
            
            // 解析状态
            string status = obj["Status"]?.ToString();
            device.Status = ParseStatus(status, device.ConfigManagerErrorCode);
            
            // 解析设备类型
            string pnpClass = obj["PNPClass"]?.ToString();
            device.Type = ParseDeviceTypeFromPNPClass(pnpClass);
            
            return device;
        }

        /// <summary>
        /// 从WMI对象创建磁盘设备
        /// </summary>
        private Device CreateDiskFromWMI(ManagementBaseObject obj)
        {
            Device device = new Device
            {
                DeviceID = obj["DeviceID"]?.ToString(),
                DriveLetter = obj["DeviceID"]?.ToString(),
                Name = obj["Name"]?.ToString(),
                VolumeName = obj["VolumeName"]?.ToString(),
                FileSystem = obj["FileSystem"]?.ToString(),
                ConfigManagerErrorCode = Convert.ToUInt32(obj["ConfigManagerErrorCode"] ?? 0)
            };
            
            // 解析大小
            if (obj["Size"] != null)
            {
                device.Size = Convert.ToUInt64(obj["Size"]);
            }
            
            if (obj["FreeSpace"] != null)
            {
                device.FreeSpace = Convert.ToUInt64(obj["FreeSpace"]);
            }
            
            // 解析状态
            string status = obj["Status"]?.ToString();
            device.Status = ParseStatus(status, device.ConfigManagerErrorCode);
            
            // 解析磁盘类型
            uint driveType = Convert.ToUInt32(obj["DriveType"] ?? 0);
            device.Type = (DeviceType)driveType;
            
            return device;
        }

        /// <summary>
        /// 解析状态
        /// </summary>
        private DeviceStatus ParseStatus(string status, uint errorCode)
        {
            // 优先使用ConfigManagerErrorCode
            if (errorCode != 0)
            {
                return errorCode switch
                {
                    1 => DeviceStatus.NotConfigured,
                    22 => DeviceStatus.Disabled,
                    24 => DeviceStatus.NotPresent,
                    25 => DeviceStatus.StillSettingUp,
                    28 => DeviceStatus.DriversNotInstalled,
                    _ => DeviceStatus.Error
                };
            }
            
            // 使用Status属性
            return status?.ToLower() switch
            {
                "ok" => DeviceStatus.OK,
                "error" => DeviceStatus.Error,
                "degraded" => DeviceStatus.Degraded,
                "unknown" => DeviceStatus.Unknown,
                "pred fail" => DeviceStatus.PredFail,
                "starting" => DeviceStatus.Starting,
                "stopping" => DeviceStatus.Stopping,
                "service" => DeviceStatus.Service,
                "stressed" => DeviceStatus.Stressed,
                "nonrecover" => DeviceStatus.NonRecover,
                "no contact" => DeviceStatus.NoContact,
                "lost comm" => DeviceStatus.LostComm,
                _ => DeviceStatus.Unknown
            };
        }

        /// <summary>
        /// 从PNPClass解析设备类型
        /// </summary>
        private DeviceType ParseDeviceTypeFromPNPClass(string pnpClass)
        {
            if (string.IsNullOrEmpty(pnpClass)) return DeviceType.Unknown;
            
            return pnpClass.ToLower() switch
            {
                "usb" => DeviceType.USBDevice,
                "diskdrive" => DeviceType.LocalDisk,
                "volume" => DeviceType.RemovableDisk,
                "cdrom" => DeviceType.CompactDisc,
                "keyboard" => DeviceType.Keyboard,
                "mouse" => DeviceType.Mouse,
                "monitor" => DeviceType.Monitor,
                "printer" => DeviceType.Printer,
                "scanner" => DeviceType.Scanner,
                "net" => DeviceType.NetworkAdapter,
                "audioendpoint" => DeviceType.Audio,
                "display" => DeviceType.Video,
                "bluetooth" => DeviceType.Bluetooth,
                _ => DeviceType.Unknown
            };
        }

        /// <summary>
        /// 获取设备详细信息
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <returns>设备信息</returns>
        public Device GetDevice(string deviceId)
        {
            try
            {
                // 尝试从Win32_PnPEntity获取
                using (var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_PnPEntity WHERE DeviceID = '{deviceId}'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return CreateDeviceFromWMI(obj);
                    }
                }
                
                // 尝试从Win32_LogicalDisk获取
                using (var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_LogicalDisk WHERE DeviceID = '{deviceId}'"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return CreateDiskFromWMI(obj);
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取设备失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取指定类型的所有设备
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <returns>设备列表</returns>
        public List<Device> GetDevices(DeviceType deviceType)
        {
            List<Device> devices = new List<Device>();
            
            try
            {
                // 根据设备类型选择查询
                string query = deviceType switch
                {
                    DeviceType.RemovableDisk or DeviceType.LocalDisk or DeviceType.CompactDisc or DeviceType.RAMDisk =>
                        $"SELECT * FROM Win32_LogicalDisk WHERE DriveType = {(int)deviceType}",
                    _ => "SELECT * FROM Win32_PnPEntity"
                };
                
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        Device device = CreateDeviceFromWMI(obj);
                        
                        // 过滤设备类型
                        if (device.Type == deviceType || deviceType == DeviceType.Unknown)
                        {
                            devices.Add(device);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取设备列表失败: {ex.Message}");
            }
            
            return devices;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            StopUSBMonitoring();
            StopDiskMonitoring();
            StopPowerMonitoring();
            StopWindowMonitoring();
            StopWindowStateMonitoring();
            StopWindowResizeMonitoring();
            StopWindowMoveMonitoring();
            
            _eventListeners.Clear();
            _callbacks.Clear();
            _eventMonitoringStatus.Clear();
        }
    }
}