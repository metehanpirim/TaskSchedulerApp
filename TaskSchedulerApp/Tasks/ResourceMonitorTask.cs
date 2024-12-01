using System;
using System.Threading.Tasks;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Tasks
{
    /// <summary>
    /// A task for monitoring system resources and sending notifications if thresholds are exceeded.
    /// </summary>
    public class ResourceMonitorTask : TaskBase
    {
        private readonly ResourceMonitorService _monitorService;
        private readonly double _cpuThreshold;
        private readonly double _ramThreshold;
        private readonly string _email;
        private readonly int _intervalInSeconds;

        /// <summary>
        /// Initializes a new resource monitor task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="monitorService">The service responsible for resource monitoring.</param>
        /// <param name="cpuThreshold">The CPU usage threshold in percentage.</param>
        /// <param name="ramThreshold">The RAM usage threshold in percentage.</param>
        /// <param name="email">The email address for notifications.</param>
        /// <param name="intervalInMinutes">The monitoring interval in minutes.</param>
        public ResourceMonitorTask(string name, ResourceMonitorService monitorService, double cpuThreshold, double ramThreshold, string email, int intervalInSeconds)
            : base(name)
        {
            _monitorService = monitorService;
            _cpuThreshold = cpuThreshold;
            _ramThreshold = ramThreshold;
            _email = email;
            _intervalInSeconds = intervalInSeconds;
        }

        /// <summary>
        /// Executes the resource monitoring task.
        /// </summary>
        protected override async Task Execute()
        {
            while (IsRunning)
            {
                _monitorService.MonitorResources(_cpuThreshold, _ramThreshold, _email);
                await Task.Delay(_intervalInSeconds * 1000); // Wait for the next interval
            }
        }

        public override string GetDetails()
        {
            string details = base.GetDetails();

            // CPU threshold check
            if (_cpuThreshold <= 100)
                details += $", CPU Threshold: {_cpuThreshold}%";

            // RAM threshold check
            if (_ramThreshold <= 100)
                details += $", RAM Threshold: {_ramThreshold}%";
            
            details += $", Email: {_email}, Interval: {_intervalInSeconds} seconds";

            return details;
        }

    }
}
