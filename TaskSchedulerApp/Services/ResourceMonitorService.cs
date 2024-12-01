using System;
using System.Diagnostics;
using System.Management;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Services
{
    /// <summary>
    /// Service responsible for monitoring system resources like CPU and RAM usage.
    /// </summary>
    public class ResourceMonitorService
    {
        private readonly MailService _mailService;

        /// <summary>
        /// Initializes the ResourceMonitorService with a MailService for notifications.
        /// </summary>
        /// <param name="mailService">The mail service used for sending notifications.</param>
        public ResourceMonitorService(MailService mailService)
        {
            _mailService = mailService;
        }

        /// <summary>
        /// Monitors the CPU and RAM usage and sends an alert if usage exceeds the threshold.
        /// </summary>
        /// <param name="cpuThreshold">CPU usage threshold in percentage.</param>
        /// <param name="ramThreshold">RAM usage threshold in percentage.</param>
        /// <param name="email">The email address to send notifications.</param>
        public void MonitorResources(double cpuThreshold, double ramThreshold, string email)
        {
            var cpuUsage = GetCpuUsage();
            var ramUsage = GetRamUsage();
            var machineName = Environment.MachineName;

            if (cpuUsage > cpuThreshold)
            {
                Logger.Log($"CPU usage exceeded: {cpuUsage}%");
                _mailService.SendMail(email, "CPU Usage Alert", $"CPU usage is at {cpuUsage}% on {machineName}");
            }

            if (ramUsage > ramThreshold)
            {
                Logger.Log($"RAM usage exceeded: {ramUsage}%");
                _mailService.SendMail(email, "RAM Usage Alert", $"RAM usage is at {ramUsage}% on {machineName}");
            }
        }

        private double GetCpuUsage()
        {
            using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue(); // İlk değeri al ve atla
            System.Threading.Thread.Sleep(500); // 500 ms bekle
            return Math.Round(cpuCounter.NextValue(), 2); // Gerçek CPU kullanımını ölç
        }

        private double GetRamUsage()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return Math.Round((1 - ramCounter.NextValue() / GetTotalRam()) * 100, 2);
        }

        private double GetTotalRam()
        {
            double totalRam = 0;
            var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
            foreach (var obj in searcher.Get())
            {
                totalRam = Convert.ToDouble(obj["TotalVisibleMemorySize"]) / 1024;
            }
            return totalRam;
        }
    }
}
