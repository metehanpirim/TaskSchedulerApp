using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerApp.Commands;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Factories;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Initializers
{
    public class AppInitializer
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public AppInitializer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<TaskManager>();

            // Register factories
            RegisterFactories(serviceCollection);

            // Register observers
            RegisterObservers(serviceCollection);

            // Register commands
            RegisterCommands(serviceCollection);

            // Build service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void RegisterFactories(IServiceCollection services)
        {
            services.AddScoped<BackupServiceFactory>();
            services.AddScoped<FileServiceFactory>();
            services.AddScoped<ResourceMonitorServiceFactory>();

            services.AddSingleton<MailService>(sp =>
                new MailService("smtp.gmail.com", 587, "dptaskscheduler@gmail.com", "xdsiturnhvvviibk"));
        }

        private void RegisterObservers(IServiceCollection services)
        {
            services.AddSingleton<ConsoleNotifier>();

            Console.Write("Would you like to enable email notifications? (yes/no): ");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            if (choice == "yes")
            {
                Console.Write("Enter the email address to receive notifications: ");
                string? email = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    services.AddSingleton<MailNotifier>(sp =>
                    {
                        var mailService = sp.GetRequiredService<MailService>();
                        return new MailNotifier(mailService, email);

                    });
                }
            }
            
        }

        private void RegisterCommands(IServiceCollection services)
        {
            services.AddSingleton<BackupCommand>();
            services.AddSingleton<DeleteFilesCommand>();
            services.AddSingleton<ResourceMonitorCommand>();
            services.AddSingleton<ReminderCommand>();
        }

        public T GetCommand<T>() where T : class
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        public BackupService CreateBackupService()
        {
            return ServiceProvider.GetRequiredService<BackupServiceFactory>().Create();
        }

        public FileService CreateFileService()
        {
            return ServiceProvider.GetRequiredService<FileServiceFactory>().Create();
        }

        public ResourceMonitorService CreateResourceMonitorService()
        {
            return ServiceProvider.GetRequiredService<ResourceMonitorServiceFactory>().Create();
        }
    }
}
