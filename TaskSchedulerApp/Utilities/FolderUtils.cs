using System;
using System.IO;

namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Utility class for folder-related operations.
    /// </summary>
    public static class FolderUtils
    {
        /// <summary>
        /// Ensures that the specified folder exists. If it doesn't, it will be created.
        /// </summary>
        /// <param name="folderPath">The path of the folder to check or create.</param>
        public static void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Logger.Log($"Folder created: {folderPath}");
            }
        }
    }
}
