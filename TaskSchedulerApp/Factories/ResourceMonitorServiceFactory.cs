using Microsoft.Extensions.DependencyInjection;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Factories
{
    public class ResourceMonitorServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ResourceMonitorServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ResourceMonitorService Create()
        {
            var mailService = _serviceProvider.GetRequiredService<MailService>();
            return new ResourceMonitorService(mailService);
        }
    }
}
