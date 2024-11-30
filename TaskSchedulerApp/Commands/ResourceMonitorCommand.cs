using System;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Tasks;

namespace TaskSchedulerApp.Commands
{
    /// <summary>
    /// Command to create and start a resource monitoring task.
    /// </summary>
    public class ResourceMonitorCommand : ICommand
    {
        private readonly TaskManager _taskManager;
        private readonly ResourceMonitorService _monitorService;

        public ResourceMonitorCommand(TaskManager taskManager, ResourceMonitorService monitorService)
        {
            _taskManager = taskManager;
            _monitorService = monitorService;
        }

        public void Execute()
        {
            Console.WriteLine("Choose the resource to monitor:");
            Console.WriteLine("1. CPU");
            Console.WriteLine("2. RAM");
            Console.WriteLine("3. Both");
            string? choice = Console.ReadLine();

            double cpuThreshold = 0, ramThreshold = 0;

            if (choice == "1" || choice == "3")
            {
                Console.Write("Enter CPU usage threshold (in %): ");
                double.TryParse(Console.ReadLine(), out cpuThreshold);
            }

            if (choice == "2" || choice == "3")
            {
                Console.Write("Enter RAM usage threshold (in %): ");
                double.TryParse(Console.ReadLine(), out ramThreshold);
            }

            Console.Write("Enter the email address for notifications: ");
            string? email = Console.ReadLine();

            Console.Write("Enter monitoring interval in minutes: ");
            if (int.TryParse(Console.ReadLine(), out int intervalInMinutes))
            {
                var task = new ResourceMonitorTask("Resource Monitor Task", _monitorService, cpuThreshold, ramThreshold, email!, intervalInMinutes);
                _taskManager.AddTask(task);

                // Run StartTask in a background thread
                Task.Run(() => task.StartTask());
            }
            else
            {
                Console.WriteLine("Invalid input for interval.");
            }
        }
    }
}
