namespace TaskSchedulerApp.Commands
{
    using TaskSchedulerApp.Services;

    public class BackupCommand
    {
        private readonly BackupService _backupService;

        public BackupCommand(BackupService backupService)
        {
            _backupService = backupService;
        }

        public void Execute()
        {
            if (!_backupService.HasActiveFolderChanged())
            {
                Console.WriteLine("Son backup'tan beri Active klasöründe bir değişiklik yok. Backup işlemi atlandı.");
                return;
            }

            _backupService.ExecuteBackup();
        }
    }
}
