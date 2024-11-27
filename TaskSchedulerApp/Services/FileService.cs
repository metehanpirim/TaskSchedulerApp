using System.IO;
using TaskSchedulerApp.Utilities; // Logger'ı kullanmak için bu satırı ekleyin

namespace TaskSchedulerApp.Services
{
    /// <summary>
    /// Dosya işlemleri için genel işlevleri içerir.
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Belirtilen klasördeki tüm dosyaların bir listesini döndürür.
        /// </summary>
        public List<FileInfo> GetAllFiles(string folderPath)
        {
            try
            {
                return Directory.GetFiles(folderPath)
                    .Select(f => new FileInfo(f))
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.Log($"'{folderPath}' klasöründeki dosyaları alırken hata: {ex.Message}");
                return new List<FileInfo>();
            }
        }

        /// <summary>
        /// Belirli bir uzantıya sahip dosyaları döndürür.
        /// </summary>
        public List<FileInfo> GetFilesByExtension(string folderPath, string extension)
        {
            try
            {
                return Directory.GetFiles(folderPath, $"*{extension}")
                    .Select(f => new FileInfo(f))
                    .ToList();
            }
            catch (Exception ex)
            {
                Logger.Log($"'{folderPath}' klasöründeki {extension} dosyalarını alırken hata: {ex.Message}");
                return new List<FileInfo>();
            }
        }

        /// <summary>
        /// Belirtilen klasördeki belirli bir süreden eski dosyaları siler.
        /// </summary>
        public void DeleteOldFiles(string folderPath, int minutesOld)
        {
            try
            {
                var files = GetAllFiles(folderPath);
                foreach (var file in files)
                {
                    if (file.LastWriteTime < DateTime.Now.AddMinutes(-minutesOld))
                    {
                        Logger.Log($"Siliniyor: {file.Name}");
                        file.Delete();
                    }
                }
                Console.WriteLine($"'{folderPath}' içindeki {minutesOld} dakikadan eski dosyalar silindi.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Dosyalar silinirken bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli bir dosyayı başka bir klasöre kopyalar.
        /// </summary>
        public void CopyFile(string sourceFilePath, string destinationFolderPath)
        {
            try
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);
                File.Copy(sourceFilePath, destinationFilePath, true);
                Logger.Log($"Dosya başarıyla kopyalandı: {sourceFilePath} -> {destinationFilePath}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Dosya kopyalanırken bir hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen klasörün var olup olmadığını kontrol eder ve yoksa oluşturur.
        /// </summary>
        public void EnsureFolderExists(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Logger.Log($"'{folderPath}' klasörü oluşturuldu.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Klasör oluşturulurken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
