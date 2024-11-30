using System;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Tasks;

namespace TaskSchedulerApp.Commands
{
    /// <summary>
    /// Command to create and start a delete files task.
    /// </summary>
    public class DeleteFilesCommand : ICommand
    {
        private readonly TaskManager _taskManager;
        private readonly FileService _fileService;

        public DeleteFilesCommand(TaskManager taskManager, FileService fileService)
        {
            _taskManager = taskManager;
            _fileService = fileService;
        }

        public void Execute()
        {
            Console.Write("Enter the folder path to clean up: ");
            string? folderPath = Console.ReadLine();

            Console.WriteLine("Choose cleanup criteria:");
            Console.WriteLine("1. Delete files older than a certain number of minutes.");
            Console.WriteLine("2. Keep only the most recent N files.");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter the threshold in minutes: ");
                if (int.TryParse(Console.ReadLine(), out int minutesOld))
                {
                    Console.Write("Enter cleanup interval in minutes: ");
                    if (int.TryParse(Console.ReadLine(), out int intervalInMinutes))
                    {
                        var task = new DeleteFilesTask("Delete Old Files Task", _fileService, folderPath!, intervalInMinutes, minutesOld);
                        _taskManager.AddTask(task);

                        // Run StartTask in a background thread
                        Task.Run(() => task.StartTask());
                    }
                    else
                    {
                        Console.WriteLine("Invalid input for interval.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for minutes.");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter the number of most recent files to keep: ");
                if (int.TryParse(Console.ReadLine(), out int keepRecentCount))
                {
                    Console.Write("Enter cleanup interval in minutes: ");
                    if (int.TryParse(Console.ReadLine(), out int intervalInMinutes))
                    {
                        var task = new DeleteFilesTask("Delete Files Task", _fileService, folderPath!, intervalInMinutes, keepRecentCount: keepRecentCount);
                        _taskManager.AddTask(task);

                        // Run StartTask in a background thread
                        Task.Run(() => task.StartTask());
                    }
                    else
                    {
                        Console.WriteLine("Invalid input for interval.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input for file count.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}
