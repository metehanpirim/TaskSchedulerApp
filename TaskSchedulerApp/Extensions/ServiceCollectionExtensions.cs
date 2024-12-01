using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerApp.Commands;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all services into the service collection.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<TaskManager>();
        services.AddSingleton<MailService>(sp => new MailService("smtp.gmail.com", 587, "dptaskscheduler@gmail.com", "xdsiturnhvvviibk"));
        services.AddScoped<BackupService>();
        services.AddScoped<FileService>();
        services.AddScoped<ResourceMonitorService>();
        return services;
    }

    /// <summary>
    /// Registers all commands into the service collection.
    /// </summary>
    /// <param name="services">The service collection to register commands into.</param>
    public static IServiceCollection RegisterCommands(this IServiceCollection services)
    {
        services.AddScoped<BackupCommand>();
        services.AddScoped<DeleteFilesCommand>();
        services.AddScoped<ResourceMonitorCommand>();
        services.AddScoped<ReminderCommand>();
        return services;
    }
}