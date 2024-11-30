using System;
using System.IO;
using System.Linq;
using TaskSchedulerApp.Core;

namespace TaskSchedulerApp.Services
{
    /// <summary>
    /// Service responsible for managing file operations in folders with observer notifications.
    /// </summary>
    public class FileService : ObservableBase
    {
        /// <summary>
        /// Deletes files older than the specified number of minutes in a folder.
        /// </summary>
        public void DeleteOldFiles(string folderPath, int minutesOld)
        {
            DeleteFiles(folderPath, file =>
                file.LastWriteTime < DateTime.Now.AddMinutes(-minutesOld),
                $"Deleting files older than {minutesOld} minutes");
        }

        /// <summary>
        /// Deletes all files except the most recent N files.
        /// </summary>
        public void DeleteFilesExceptRecent(string folderPath, int keepRecentCount)
        {
            var files = Directory.GetFiles(folderPath)
                                 .Select(file => new FileInfo(file))
                                 .OrderByDescending(file => file.LastWriteTime)
                                 .ToList();

            foreach (var file in files.Skip(keepRecentCount))
            {
                file.Delete();
                NotifyObservers($"Deleted file: {file.Name}");
            }
        }

        /// <summary>
        /// Common logic for deleting files based on a condition.
        /// </summary>
        private void DeleteFiles(string folderPath, Func<FileInfo, bool> condition, string actionDescription)
        {
            Console.WriteLine(actionDescription);

            try
            {
                var files = Directory.GetFiles(folderPath)
                                     .Select(file => new FileInfo(file))
                                     .Where(condition)
                                     .ToList();

                foreach (var file in files)
                {
                    file.Delete();
                    NotifyObservers($"Deleted file: {file.Name}");
                }
            }
            catch (Exception ex)
            {
                NotifyObservers($"Error deleting files: {ex.Message}");
            }
        }
    }
}
