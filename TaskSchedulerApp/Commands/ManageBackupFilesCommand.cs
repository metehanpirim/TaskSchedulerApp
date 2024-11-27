namespace TaskSchedulerApp.Commands
{
    using TaskSchedulerApp.Services;

    public class ManageBackupFilesCommand
    {
        private readonly BackupService _backupService;

        public ManageBackupFilesCommand(BackupService backupService)
        {
            _backupService = backupService;
        }

        public void Execute()
        {
            Console.WriteLine("Eski backup dosyaları yönetiliyor...");
            _backupService.ManageOldBackups();
        }
    }
}
