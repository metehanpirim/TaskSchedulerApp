using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Factories
{
    public class BackupServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BackupServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BackupService Create()
        {
            var backupService = new BackupService("DefaultBackupFolder");

            var consoleNotifier = _serviceProvider.GetService<ConsoleNotifier>();
            if (consoleNotifier != null)
            {
                backupService.AddObserver(consoleNotifier);
            }

            var mailNotifier = _serviceProvider.GetService<MailNotifier>();
            if (mailNotifier != null)
            {
                backupService.AddObserver(mailNotifier);
            }

            return backupService;
        }
    }
}
