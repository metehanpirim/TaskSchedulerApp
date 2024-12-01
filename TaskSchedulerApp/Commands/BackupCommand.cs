using System;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Factories;
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
        private readonly BackupServiceFactory _factory;

        public BackupCommand(TaskManager taskManager, BackupServiceFactory factory)
        {
            _taskManager = taskManager;
            _factory = factory;
        }

        public void Execute()
        {
            // Get the source folder path from the user
            Console.Write("Enter the source folder path to back up: ");
            string? sourceFolderPath = Console.ReadLine()?.Trim();

            // Validate the source folder path
            if (string.IsNullOrWhiteSpace(sourceFolderPath) || !System.IO.Directory.Exists(sourceFolderPath))
            {
                Console.WriteLine("Invalid source folder path. Task creation aborted.");
                return;
            }

            // Get the backup folder path from the user
            Console.Write("Enter the backup folder path to store backups: ");
            string? backupFolderPath = Console.ReadLine()?.Trim();

            // Validate the backup folder path
            if (string.IsNullOrWhiteSpace(backupFolderPath))
            {
                Console.WriteLine("Invalid backup folder path. Task creation aborted.");
                return;
            }

            var backupService = _factory.Create();
            backupService.SetBackupFolderPath(backupFolderPath);

            // Get the backup interval in minutes
            Console.Write("Enter backup interval in minutes: ");
            if (!int.TryParse(Console.ReadLine(), out int intervalInMinutes) || intervalInMinutes <= 0)
            {
                Console.WriteLine("Invalid interval. Task creation aborted.");
                return;
            }

            // Create and start the backup task
            var task = new BackupTask("Backup Task", backupService, sourceFolderPath, backupFolderPath, intervalInMinutes);
            _taskManager.AddTask(task);

            // Start the task in the background
            Task.Run(() => task.StartTask());
        }
    }
}
