namespace TaskSchedulerApp.Utilities
{
    public static class FolderUtils
    {
        public static void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine($"'{folderPath}' klasörü oluşturuldu.");
            }
        }
    }
}
