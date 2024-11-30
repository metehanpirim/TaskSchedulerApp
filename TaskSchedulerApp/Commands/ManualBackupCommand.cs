//namespace TaskSchedulerApp.Commands
//{
//    using TaskSchedulerApp.Services;

//    public class ManualBackupCommand
//    {
//        private readonly BackupService _backupService;

//        public ManualBackupCommand(BackupService backupService)
//        {
//            _backupService = backupService;
//        }

//        public void Execute()
//        {
//            Console.WriteLine("Manuel backup işlemi başlatılıyor...");
//            _backupService.ResetLastBackupTime();
//            _backupService.ExecuteBackup();
//        }
//    }
//}
