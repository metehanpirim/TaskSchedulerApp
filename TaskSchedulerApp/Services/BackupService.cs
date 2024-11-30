using System;
using System.IO;
using System.Linq;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Services
{
    /// <summary>
    /// Service responsible for managing backup operations with observer notifications.
    /// </summary>
    public class BackupService : ObservableBase
    {
        private readonly string _backupFolderPath;

        public BackupService(string backupFolderPath)
        {
            _backupFolderPath = backupFolderPath;
            FolderUtils.EnsureFolderExists(_backupFolderPath);
        }

        public void ExecuteBackup(string sourceFolderPath)
        {
            Console.WriteLine($"Starting backup for folder: {sourceFolderPath}");

            if (!Directory.Exists(sourceFolderPath))
            {
                NotifyObservers($"Source folder does not exist: {sourceFolderPath}");
                return;
            }

            try
            {
                string backupFileName = $"Backup_{DateTime.Now:yyyyMMddHHmmss}.zip";
                string backupFilePath = Path.Combine(_backupFolderPath, backupFileName);
                string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Temp");

                FolderUtils.EnsureFolderExists(tempFolderPath);
                CopyFilesToTemp(sourceFolderPath, tempFolderPath);
                System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, backupFilePath);
                Directory.Delete(tempFolderPath, true);

                NotifyObservers($"Backup completed successfully: {backupFilePath}");
            }
            catch (Exception ex)
            {
                NotifyObservers($"Error during backup: {ex.Message}");
            }
        }

        private void CopyFilesToTemp(string sourceFolderPath, string tempFolderPath)
        {
            foreach (var file in Directory.GetFiles(sourceFolderPath))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(tempFolderPath, fileName), true);
            }
        }

        public void ManageOldBackups(int maxBackupFiles)
        {
            try
            {
                var backupFiles = Directory.GetFiles(_backupFolderPath, "*.zip")
                                           .Select(file => new FileInfo(file))
                                           .OrderByDescending(file => file.CreationTime)
                                           .ToList();

                foreach (var file in backupFiles.Skip(maxBackupFiles))
                {
                    file.Delete();
                    NotifyObservers($"Deleted old backup file: {file.Name}");
                }
            }
            catch (Exception ex)
            {
                NotifyObservers($"Error managing old backups: {ex.Message}");
            }
        }
    }
}
