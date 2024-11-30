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
        private string _backupFolderPath;

        /// <summary>
        /// Initializes the BackupService with an initial backup folder path.
        /// </summary>
        /// <param name="initialBackupFolderPath">The default backup folder path.</param>
        public BackupService(string initialBackupFolderPath)
        {
            _backupFolderPath = initialBackupFolderPath;
            FolderUtils.EnsureFolderExists(_backupFolderPath);
        }

        /// <summary>
        /// Sets a new backup folder path and ensures the folder exists.
        /// </summary>
        /// <param name="backupFolderPath">The new backup folder path.</param>
        public void SetBackupFolderPath(string backupFolderPath)
        {
            _backupFolderPath = backupFolderPath;
            FolderUtils.EnsureFolderExists(_backupFolderPath);
        }

        /// <summary>
        /// Executes a backup operation for the specified source folder.
        /// </summary>
        /// <param name="sourceFolderPath">The folder to back up.</param>
        public void ExecuteBackup(string sourceFolderPath)
        {
            Console.WriteLine($"Starting backup for folder: {sourceFolderPath}");

            try
            {
                if (!Directory.Exists(sourceFolderPath))
                {
                    NotifyObservers($"Source folder does not exist: {sourceFolderPath}");
                    Logger.LogError($"Backup failed. Source folder does not exist: {sourceFolderPath}");
                    return;
                }

                // Prepare backup file name and path
                string backupFileName = $"Backup_{DateTime.Now:yyyyMMddHHmmss}.zip";
                string backupFilePath = Path.Combine(_backupFolderPath, backupFileName);

                // Temporary folder for creating the backup
                string tempFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
                FolderUtils.EnsureFolderExists(tempFolderPath);

                // Copy files to temporary folder
                CopyFilesToTemp(sourceFolderPath, tempFolderPath);

                // Compress files into a ZIP archive
                System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, backupFilePath);

                // Clean up the temporary folder
                Directory.Delete(tempFolderPath, true);

                NotifyObservers($"Backup completed successfully: {backupFilePath}");
                Logger.Log($"Backup completed successfully. Source: {sourceFolderPath}, Target: {backupFilePath}");
            }
            catch (Exception ex)
            {
                NotifyObservers($"Error during backup: {ex.Message}");
                Logger.LogError($"Backup failed. Source: {sourceFolderPath}, Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes older backups exceeding the specified retention limit.
        /// </summary>
        /// <param name="maxBackupFiles">The maximum number of backup files to retain.</param>
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
                    Logger.Log($"Deleted old backup file: {file.FullName}");
                }
            }
            catch (Exception ex)
            {
                NotifyObservers($"Error managing old backups: {ex.Message}");
                Logger.LogError($"Error managing old backups: {ex.Message}");
            }
        }

        /// <summary>
        /// Copies files from the source folder to a temporary folder.
        /// </summary>
        /// <param name="sourceFolderPath">The source folder path.</param>
        /// <param name="tempFolderPath">The temporary folder path.</param>
        private void CopyFilesToTemp(string sourceFolderPath, string tempFolderPath)
        {
            foreach (var file in Directory.GetFiles(sourceFolderPath))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(tempFolderPath, fileName), true);
            }
        }
    }
}
