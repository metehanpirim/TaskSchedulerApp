using System;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Tasks;

namespace TaskSchedulerApp.Commands
{
    /// <summary>
    /// Command to create and start a backup task.
    /// </summary>
    public class BackupCommand : ICommand
    {
        private readonly TaskManager _taskManager;
        private readonly BackupService _backupService;

        public BackupCommand(TaskManager taskManager, BackupService backupService)
        {
            _taskManager = taskManager;
            _backupService = backupService;
        }

        public void Execute()
        {
            Console.Write("Enter the source folder path to back up: ");
            string? sourceFolderPath = Console.ReadLine();

            Console.Write("Enter backup interval in minutes: ");
            if (!int.TryParse(Console.ReadLine(), out int intervalInMinutes))
            {
                Console.WriteLine("Invalid input. Backup task creation aborted.");
                return;
            }

            var task = new BackupTask("Backup Task", _backupService, sourceFolderPath!, intervalInMinutes);
            _taskManager.AddTask(task);

            // Run StartTask in a background thread
            Task.Run(() => task.StartTask());
        }
    }
}
