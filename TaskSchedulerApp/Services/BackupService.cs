namespace TaskSchedulerApp.Services
{
    using TaskSchedulerApp.Utilities;

    /// <summary>
    /// Backup işlemleriyle ilgili tüm iş mantığını içerir.
    /// Yedekleme, eski yedeklerin silinmesi ve gözlemcileri bilgilendirme işlemlerini yönetir.
    /// </summary>
    public class BackupService
    {
        private readonly string _activeFolderPath; // Active klasörünün yolu
        private readonly string _backupFolderPath; // Backup klasörünün yolu
        private DateTime _lastBackupTime; // Son yedekleme zamanı

        private readonly List<IBackupObserver> _observers = new(); // Gözlemciler listesi

        public BackupService(string activeFolderPath, string backupFolderPath)
        {
            _activeFolderPath = activeFolderPath;
            _backupFolderPath = backupFolderPath;
            _lastBackupTime = DateTime.MinValue; // Varsayılan olarak eski bir tarih
        }

        /// <summary>
        /// Yeni bir gözlemci ekler.
        /// </summary>
        public void AddObserver(IBackupObserver observer)
        {
            _observers.Add(observer);
        }

        /// <summary>
        /// Gözlemcilere bildirim gönderir.
        /// </summary>
        private void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Notify(message);
            }
        }

        /// <summary>
        /// Active klasöründeki dosyalarda bir değişiklik olup olmadığını kontrol eder.
        /// </summary>
        public bool HasActiveFolderChanged()
        {
            var files = Directory.GetFiles(_activeFolderPath);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTime > _lastBackupTime)
                {
                    return true; // Dosyalarda değişiklik tespit edildi
                }
            }
            return false; // Değişiklik yok
        }

        /// <summary>
        /// Son yedekleme zamanını sıfırlar.
        /// </summary>
        public void ResetLastBackupTime()
        {
            _lastBackupTime = DateTime.MinValue;
        }

        /// <summary>
        /// Yedekleme işlemini gerçekleştirir.
        /// Active klasöründeki dosyaları ZIP olarak Backup klasörüne kaydeder.
        /// </summary>
        public void ExecuteBackup()
        {
            Console.WriteLine("Yedekleme işlemi başlatılıyor...");

            string[] allFiles = Directory.GetFiles(_activeFolderPath);
            if (allFiles.Length == 0)
            {
                NotifyObservers("Yedeklenecek dosya bulunamadı.");
                return;
            }

            string zipFileName = $"Backup_{DateTime.Now:yyyyMMddHHmmss}.zip";
            string zipFilePath = Path.Combine(_backupFolderPath, zipFileName);
            string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Temp");

            try
            {
                FolderUtils.EnsureFolderExists(tempFolderPath);

                foreach (var file in allFiles)
                {
                    string fileName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(tempFolderPath, fileName), true);
                }

                System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath);
                NotifyObservers($"Yedekleme tamamlandı: {zipFileName}");

                Directory.Delete(tempFolderPath, true);

                ManageOldBackups();
                _lastBackupTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                NotifyObservers($"Yedekleme işlemi sırasında bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Backup klasöründeki eski yedekleri temizler.
        /// Maksimum sayıda yedek dosya tutulur.
        /// </summary>
        public void ManageOldBackups()
        {
            try
            {
                var zipFiles = Directory.GetFiles(_backupFolderPath, "*.zip")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                if (zipFiles.Count > 3)
                {
                    for (int i = 3; i < zipFiles.Count; i++)
                    {
                        NotifyObservers($"Eski yedekleme dosyası siliniyor: {zipFiles[i].Name}");
                        zipFiles[i].Delete();
                    }
                }
                else
                {
                    NotifyObservers("Backup klasöründe yalnızca 3 veya daha az dosya var. Silme işlemi yapılmadı.");
                }
            }
            catch (Exception ex)
            {
                NotifyObservers($"Eski yedekleri yönetirken bir hata oluştu: {ex.Message}");
            }
        }
    }
}