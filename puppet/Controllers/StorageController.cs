using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using puppet.PUP;
using puppet.Core.Security;
using puppet.Core;

namespace puppet.Controllers
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class StorageController
    {
        private readonly Form _form;
        private readonly string _storagePath;
        private readonly Dictionary<string, SQLiteConnection> _connections;

        public StorageController(Form form)
        {
            _form = form;
            _storagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "puppet",
                "storage"
            );
            // 内存优化：预分配容量，减少动态扩容
            _connections = new Dictionary<string, SQLiteConnection>(capacity: 8);

            // 确保存储目录存在
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        /// <summary>
        /// 获取服务器实例（从 ServiceManager）
        /// </summary>
        private IServer? Server => ServiceManager.Instance.Server;

        /// <summary>
        /// 设置键值对
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <param name="key">键名</param>
        /// <param name="value">值（JSON字符串）</param>
        public void SetItem(string database, string key, string value)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() => SetItem(database, key, value)));
                return;
            }

            try
            {
                var connection = GetConnection(database);

                // 创建表（如果不存在）
                using (var command = new SQLiteCommand(
                    @"CREATE TABLE IF NOT EXISTS storage (
                        key TEXT PRIMARY KEY,
                        value TEXT NOT NULL,
                        created_at INTEGER NOT NULL,
                        updated_at INTEGER NOT NULL
                    )",
                    connection))
                {
                    command.ExecuteNonQuery();
                }

                // 插入或更新数据
                long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                using (var command = new SQLiteCommand(
                    @"INSERT OR REPLACE INTO storage (key, value, created_at, updated_at)
                      VALUES (
                        @key,
                        @value,
                        COALESCE((SELECT created_at FROM storage WHERE key = @key), @now),
                        @now
                      )",
                    connection))
                {
                    command.Parameters.AddWithValue("@key", key);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@now", now);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"Storage: Set [{database}] {key}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error setting item - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取键值对
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <param name="key">键名</param>
        /// <returns>值（JSON字符串），如果不存在返回空字符串</returns>
        public string GetItem(string database, string key)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                return (string)_form.Invoke(new Func<string>(() => GetItem(database, key)));
            }

            try
            {
                var connection = GetConnection(database);

                using (var command = new SQLiteCommand(
                    "SELECT value FROM storage WHERE key = @key",
                    connection))
                {
                    command.Parameters.AddWithValue("@key", key);

                    var result = command.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error getting item - {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// 删除键值对
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <param name="key">键名</param>
        public void RemoveItem(string database, string key)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() => RemoveItem(database, key)));
                return;
            }

            try
            {
                var connection = GetConnection(database);

                using (var command = new SQLiteCommand(
                    "DELETE FROM storage WHERE key = @key",
                    connection))
                {
                    command.Parameters.AddWithValue("@key", key);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"Storage: Removed [{database}] {key}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error removing item - {ex.Message}");
            }
        }

        /// <summary>
        /// 清空指定数据库的所有数据
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        public void Clear(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() => Clear(database)));
                return;
            }

            try
            {
                var connection = GetConnection(database);

                using (var command = new SQLiteCommand("DELETE FROM storage", connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"Storage: Cleared [{database}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error clearing database - {ex.Message}");
            }
        }

        /// <summary>
        /// 获取指定数据库中的所有键
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <returns>键列表（JSON数组字符串）</returns>
        public string GetKeys(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                return (string)_form.Invoke(new Func<string>(() => GetKeys(database)));
            }

            try
            {
                var connection = GetConnection(database);
                var keys = new List<string>();

                using (var command = new SQLiteCommand("SELECT key FROM storage", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        keys.Add(reader.GetString(0));
                    }
                }

                return Newtonsoft.Json.JsonConvert.SerializeObject(keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error getting keys - {ex.Message}");
                return "[]";
            }
        }

        /// <summary>
        /// 检查键是否存在
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <param name="key">键名</param>
        /// <returns>是否存在</returns>
        public bool HasItem(string database, string key)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                return (bool)_form.Invoke(new Func<bool>(() => HasItem(database, key)));
            }

            try
            {
                var connection = GetConnection(database);

                using (var command = new SQLiteCommand(
                    "SELECT COUNT(*) FROM storage WHERE key = @key",
                    connection))
                {
                    command.Parameters.AddWithValue("@key", key);
                    var result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error checking item - {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取数据库大小（字节数）
        /// </summary>
        /// <param name="database">数据库名称（默认为 'default'）</param>
        /// <returns>数据库大小</returns>
        public long GetSize(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                return (long)_form.Invoke(new Func<long>(() => GetSize(database)));
            }

            try
            {
                string dbPath = Path.Combine(_storagePath, $"{database}.db");
                if (File.Exists(dbPath))
                {
                    return new FileInfo(dbPath).Length;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error getting size - {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="database">数据库名称</param>
        public void DeleteDatabase(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                return;
            }

            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() => DeleteDatabase(database)));
                return;
            }

            try
            {
                // 关闭连接
                if (_connections.ContainsKey(database))
                {
                    _connections[database]?.Close();
                    _connections.Remove(database);
                }

                // 删除数据库文件
                string dbPath = Path.Combine(_storagePath, $"{database}.db");
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }

                Console.WriteLine($"Storage: Deleted database [{database}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error deleting database - {ex.Message}");
            }
        }

        /// <summary>
        /// 获取所有数据库列表
        /// </summary>
        /// <returns>数据库名称列表（JSON数组字符串）</returns>
        public string GetDatabases()
        {
            // 确保在UI线程
            if (_form.InvokeRequired)
            {
                return (string)_form.Invoke(new Func<string>(() => GetDatabases()));
            }

            try
            {
                var databases = new List<string>();
                foreach (var file in Directory.GetFiles(_storagePath, "*.db"))
                {
                    databases.Add(Path.GetFileNameWithoutExtension(file));
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(databases);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Storage: Error getting databases - {ex.Message}");
                return "[]";
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        private SQLiteConnection GetConnection(string database)
        {
            if (!_connections.ContainsKey(database))
            {
                string dbPath = Path.Combine(_storagePath, $"{database}.db");
                string connectionString = $"Data Source={dbPath};Version=3;";
                
                var connection = new SQLiteConnection(connectionString);
                connection.Open();
                _connections[database] = connection;

                // 如果 PUP 文件包含签名，则验证数据库签名
                if (Server != null && Server.HasSignature)
                {
                    VerifyDatabaseSignature(database, dbPath);
                }
            }

            return _connections[database];
        }

        /// <summary>
        /// 验证数据库签名
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="dbPath">数据库文件路径</param>
        private void VerifyDatabaseSignature(string database, string dbPath)
        {
            try
            {
                if (Server?.Certificate == null)
                {
                    Console.WriteLine($"No certificate available for signature verification of database: {database}");
                    return;
                }

                // 检查数据库是否存在
                if (!File.Exists(dbPath))
                {
                    Console.WriteLine($"Database file does not exist, skipping signature verification: {database}");
                    return;
                }

                // 检查数据库元数据表是否存在
                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();
                    
                    // 检查元数据表是否存在
                    using (var command = new SQLiteCommand(
                        "SELECT name FROM sqlite_master WHERE type='table' AND name='puppet_metadata'", 
                        connection))
                    {
                        var result = command.ExecuteScalar();
                        
                        if (result == null)
                        {
                            Console.WriteLine($"No signature metadata found in database: {database} (database not signed yet)");
                            return;
                        }
                    }

                    // 读取签名数据
                    using (var command = new SQLiteCommand(
                        "SELECT app_id, certificate_fingerprint, signature_data FROM puppet_metadata", 
                        connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string appId = reader.GetString(0);
                                string certFingerprint = reader.GetString(1);
                                byte[] signatureData = (byte[])reader["signature_data"];

                                Console.WriteLine($"Found signature metadata in database: {database}");
                                Console.WriteLine($"  AppID: {appId}");
                                Console.WriteLine($"  Certificate Fingerprint: {certFingerprint}");

                                // 验证证书指纹是否匹配
                                string expectedFingerprint = CertificateUtils.GetCertificateFingerprint(Server.Certificate);
                                if (certFingerprint != expectedFingerprint)
                                {
                                    Console.WriteLine($"WARNING: Certificate fingerprint mismatch in database: {database}");
                                    Console.WriteLine($"  Expected: {expectedFingerprint}");
                                    Console.WriteLine($"  Found: {certFingerprint}");
                                    return;
                                }

                                // 验证签名
                                var (isValid, message) = AppSignatureValidator.ValidateDatabaseSignature(
                                    dbPath,
                                    Server.Certificate,
                                    signatureData);

                                if (isValid)
                                {
                                    Console.WriteLine($"✓ Database signature verified successfully: {database}");
                                    Console.WriteLine($"  {message}");
                                }
                                else
                                {
                                    Console.WriteLine($"✗ Database signature verification failed: {database}");
                                    Console.WriteLine($"  {message}");
                                    Console.WriteLine($"  WARNING: Database may have been tampered with");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying database signature for {database}: {ex.Message}");
                Console.WriteLine($"  Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 对数据库进行签名（仅V1.2格式支持）
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <returns>是否成功</returns>
        public bool SignDatabase(string database)
        {
            if (string.IsNullOrEmpty(database))
            {
                database = "default";
            }

            try
            {
                if (Server == null || !Server.HasSignature)
                {
                    Console.WriteLine("Cannot sign database: No certificate available (V1.2 format required)");
                    return false;
                }

                if (Server.Certificate == null)
                {
                    Console.WriteLine("Cannot sign database: Certificate is null");
                    return false;
                }

                // 获取私钥参数（需要转换为 PupServer 才能访问此方法）
                var privateKeyParams = (Server as PupServer)?.GetPrivateKeyParameters();
                if (privateKeyParams == null)
                {
                    Console.WriteLine("Cannot sign database: Failed to get private key parameters");
                    return false;
                }

                string dbPath = Path.Combine(_storagePath, $"{database}.db");

                if (!File.Exists(dbPath))
                {
                    Console.WriteLine($"Database file does not exist: {database}");
                    return false;
                }

                // 读取数据库内容
                byte[] databaseContent = File.ReadAllBytes(dbPath);

                // 使用私钥签名
                byte[] signature;
                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(privateKeyParams.Value);
                    signature = rsa.SignData(databaseContent, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }

                // 创建或更新元数据表
                using (var connection = GetConnection(database))
                {
                    // 创建元数据表
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS puppet_metadata (
                                id INTEGER PRIMARY KEY AUTOINCREMENT,
                                app_id TEXT NOT NULL,
                                certificate_fingerprint TEXT NOT NULL,
                                signature_data BLOB NOT NULL,
                                created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                            )";
                        command.ExecuteNonQuery();
                    }

                    // 删除现有的元数据
                    using (var command = new SQLiteCommand("DELETE FROM puppet_metadata", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // 插入新的签名元数据
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"
                            INSERT INTO puppet_metadata (app_id, certificate_fingerprint, signature_data)
                            VALUES (@appId, @certFingerprint, @signatureData)";
                        
                        // 提取 AppID
                        string appId = Server.Certificate.Subject.Split(',')[0].Replace("CN=", "").Trim();
                        string certFingerprint = CertificateUtils.GetCertificateFingerprint(Server.Certificate);

                        command.Parameters.AddWithValue("@appId", appId);
                        command.Parameters.AddWithValue("@certFingerprint", certFingerprint);
                        command.Parameters.AddWithValue("@signatureData", signature);

                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"Database signed successfully: {database}");
                Console.WriteLine($"  AppID: {Server.Certificate.Subject.Split(',')[0].Replace("CN=", "").Trim()}");
                Console.WriteLine($"  Certificate Fingerprint: {CertificateUtils.GetCertificateFingerprint(Server.Certificate)}");
                Console.WriteLine($"  Signature Size: {signature.Length} bytes");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing database {database}: {ex.Message}");
                Console.WriteLine($"  Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// 关闭所有连接
        /// </summary>
        public void Dispose()
        {
            foreach (var connection in _connections.Values)
            {
                connection?.Close();
                connection?.Dispose();
            }
            _connections.Clear();
        }
    }
}