namespace TaskSchedulerApp.Utilities
{
    using Newtonsoft.Json;

    /// <summary>
    /// JSON tabanlı konfigürasyon dosyasını okuyan ve ayarları sağlayan sınıf.
    /// </summary>
    public class Configuration
    {
        private readonly dynamic? _settings; // Ayarların JSON'dan yüklenen dinamik objesi

        public Configuration(string configFilePath = "appsettings.json")
        {
            try
            {
                var json = File.ReadAllText(configFilePath);
                _settings = JsonConvert.DeserializeObject<dynamic>(json);
            }
            catch (Exception ex)
            {
                Logger.Log($"Ayarlar yüklenirken bir hata oluştu: {ex.Message}");
                _settings = null; // Varsayılan olarak null atanır
            }
        }

        /// <summary>
        /// JSON'daki yollarla ilgili ayarları döndürür.
        /// </summary>
        /// <param name="key">Ayarın anahtarı (örn. ActiveFolder).</param>
        /// <returns>Anahtara karşılık gelen yol değeri.</returns>
        public string GetPath(string key)
        {
            try
            {
                return _settings?.Paths?[key]?.ToString() ?? string.Empty;
            }
            catch
            {
                Logger.Log($"'{key}' için ayar bulunamadı.");
                return string.Empty;
            }
        }

        /// <summary>
        /// Zamanlayıcı ile ilgili ayarları döndürür.
        /// </summary>
        /// <param name="key">Ayarın anahtarı (örn. BackupIntervalInMinutes).</param>
        /// <returns>Zamanlayıcı ayar değeri (dakika cinsinden).</returns>
        public int GetTimer(string key)
        {
            try
            {
                return (int)(_settings?.Timers?[key] ?? 0);
            }
            catch
            {
                Logger.Log($"'{key}' için zamanlayıcı ayarı bulunamadı.");
                return 0;
            }
        }
    }
}
