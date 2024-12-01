using System;
using TaskSchedulerApp.Initializers;
using TaskSchedulerApp.Utilities;
using TaskSchedulerApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TaskSchedulerApp.Utilities
{
    public class MenuHandler
    {
        private readonly AppInitializer _appInitializer;

        public MenuHandler(AppInitializer appInitializer)
        {
            _appInitializer = appInitializer;
        }

        /// <summary>
        /// Displays the main menu and handles user input.
        /// </summary>
        public void ShowMainMenu()
        {
            while (true)
            {
                PrintMenu();

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ExecuteBackupTask();
                        break;
                    case "2":
                        ExecuteDeleteFilesTask();
                        break;
                    case "3":
                        ExecuteResourceMonitorTask();
                        break;
                    case "4":
                        ExecuteReminderTask();
                        break;
                    case "5":
                        ListTasks();
                        break;
                    case "6":
                        RemoveTask();
                        break;
                    case "7":
                        ExitApplication();
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("\n--- Task Scheduler Menu ---");
            Console.WriteLine("1. Create a backup task");
            Console.WriteLine("2. Create a delete files task");
            Console.WriteLine("3. Create a resource monitoring task");
            Console.WriteLine("4. Create a reminder task");
            Console.WriteLine("5. List all tasks");
            Console.WriteLine("6. Remove a task");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice: ");
        }

        private void ExecuteBackupTask()
        {
            Console.WriteLine("\n--- Create a Backup Task ---");
            _appInitializer.GetCommand<TaskSchedulerApp.Commands.BackupCommand>().Execute();
        }

        private void ExecuteDeleteFilesTask()
        {
            Console.WriteLine("\n--- Create a Delete Files Task ---");
            _appInitializer.GetCommand<TaskSchedulerApp.Commands.DeleteFilesCommand>().Execute();
        }

        private void ExecuteResourceMonitorTask()
        {
            Console.WriteLine("\n--- Create a Resource Monitoring Task ---");
            _appInitializer.GetCommand<TaskSchedulerApp.Commands.ResourceMonitorCommand>().Execute();
        }

        private void ExecuteReminderTask()
        {
            Console.WriteLine("\n--- Create a Reminder Task ---");
            _appInitializer.GetCommand<TaskSchedulerApp.Commands.ReminderCommand>().Execute();
        }

        private void ListTasks()
        {
            Console.WriteLine("\n--- List of Tasks ---");
            _appInitializer.ServiceProvider.GetService<TaskSchedulerApp.Core.TaskManager>()?.ListTasks();
        }

        private void RemoveTask()
        {
            Console.WriteLine("\n--- Remove a Task ---");
            var taskManager = _appInitializer.ServiceProvider.GetService<TaskSchedulerApp.Core.TaskManager>();

            taskManager?.ListTasks();

            Console.Write("Enter the task number to remove: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                taskManager?.RemoveTask(taskNumber - 1);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        private void ExitApplication()
        {
            Console.WriteLine("Exiting the application...");
        }
    }
}
