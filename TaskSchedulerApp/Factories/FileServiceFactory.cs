using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Factories
{
    public class FileServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public FileServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public FileService Create()
        {
            var fileService = new FileService();

            var consoleNotifier = _serviceProvider.GetService<ConsoleNotifier>();
            if (consoleNotifier != null)
            {
                fileService.AddObserver(consoleNotifier);
            }

            var mailNotifier = _serviceProvider.GetService<MailNotifier>();
            if (mailNotifier != null)
            {
                fileService.AddObserver(mailNotifier);
            }

            return fileService;
        }
    }
}
