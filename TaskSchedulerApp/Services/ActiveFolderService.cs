namespace TaskSchedulerApp.Services
{
    using TaskSchedulerApp.Utilities;

    /// <summary>
    /// Active klasöründeki dosyaların yönetimini sağlar.
    /// Eski dosyaları silme işlemini ve gözlemcileri bilgilendirme işlevini içerir.
    /// </summary>
    public class ActiveFolderService
    {
        private readonly string _activeFolderPath; // Active klasörünün yolu
        private readonly List<IBackupObserver> _observers = new(); // Gözlemciler listesi

        public ActiveFolderService(string activeFolderPath)
        {
            _activeFolderPath = activeFolderPath;
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
        /// Belirtilen süreden daha eski dosyaları siler.
        /// </summary>
        /// <param name="minutesOld">Kaç dakikadan eski dosyaların silineceği.</param>
        public void DeleteFilesOlderThan(int minutesOld)
        {
            Console.WriteLine($"'{_activeFolderPath}' içindeki {minutesOld} dakikadan eski dosyalar siliniyor...");
            try
            {
                var files = Directory.GetFiles(_activeFolderPath);
                int deletedFilesCount = 0;

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < DateTime.Now.AddMinutes(-minutesOld))
                    {
                        Console.WriteLine($"Siliniyor: {fileInfo.Name}");
                        fileInfo.Delete();
                        deletedFilesCount++;
                    }
                }

                if (deletedFilesCount > 0)
                {
                    NotifyObservers($"{deletedFilesCount} dosya başarıyla silindi.");
                }
                else
                {
                    NotifyObservers("Silinecek uygun dosya bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                NotifyObservers($"Dosyalar silinirken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
