using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace puppet
{
    /// <summary>
    /// INI 文件读取器
    /// 用于读取 puppet.ini 配置文件
    /// </summary>
    public class IniReader
    {
        private readonly string _filePath;
        private readonly Dictionary<string, Dictionary<string, string>> _data;

        public IniReader(string filePath)
        {
            _filePath = filePath;
            _data = new Dictionary<string, Dictionary<string, string>>();
            Load();
        }

        /// <summary>
        /// 加载 INI 文件
        /// </summary>
        private void Load()
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            string currentSection = "";

            foreach (string line in File.ReadAllLines(_filePath, Encoding.UTF8))
            {
                string trimmedLine = line.Trim();

                // 跳过空行和注释
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                // 解析节
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                    if (!_data.ContainsKey(currentSection))
                    {
                        _data[currentSection] = new Dictionary<string, string>();
                    }
                    continue;
                }

                // 解析键值对
                int equalIndex = trimmedLine.IndexOf('=');
                if (equalIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalIndex).Trim();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();

                    if (!string.IsNullOrEmpty(currentSection))
                    {
                        _data[currentSection][key] = value;
                    }
                }
            }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="section">节名（留空表示全局配置）</param>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(section))
            {
                // 查找所有节中的键
                foreach (var sectionData in _data.Values)
                {
                    if (sectionData.ContainsKey(key))
                    {
                        return sectionData[key];
                    }
                }
                return defaultValue;
            }

            if (_data.ContainsKey(section) && _data[section].ContainsKey(key))
            {
                return _data[section][key];
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取全局配置（不带节的配置）
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public string GetValue(string key, string defaultValue = "")
        {
            return GetValue("", key, defaultValue);
        }

        /// <summary>
        /// 检查节是否存在
        /// </summary>
        /// <param name="section">节名</param>
        /// <returns>是否存在</returns>
        public bool SectionExists(string section)
        {
            return _data.ContainsKey(section);
        }

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>是否存在</returns>
        public bool KeyExists(string key)
        {
            foreach (var sectionData in _data.Values)
            {
                if (sectionData.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设置配置值
        /// </summary>
        /// <param name="section">节名</param>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void SetValue(string section, string key, string value)
        {
            // 如果节不存在，创建新节
            if (!_data.ContainsKey(section))
            {
                _data[section] = new Dictionary<string, string>();
            }

            // 设置值
            _data[section][key] = value;
        }

        /// <summary>
        /// 保存 INI 文件
        /// </summary>
        public void Save()
        {
            try
            {
                // 备份原文件
                if (File.Exists(_filePath))
                {
                    string backupPath = _filePath + ".backup";
                    File.Copy(_filePath, backupPath, true);
                }

                // 写入新内容
                using (var writer = new StreamWriter(_filePath, false, Encoding.UTF8))
                {
                    foreach (var section in _data)
                    {
                        // 写入节头
                        writer.WriteLine($"[{section.Key}]");

                        // 写入键值对
                        foreach (var kvp in section.Value)
                        {
                            writer.WriteLine($"{kvp.Key}={kvp.Value}");
                        }

                        // 节之间空一行
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save INI file: {ex.Message}", ex);
            }
        }
    }
}