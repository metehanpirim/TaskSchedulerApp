using System;
using TaskSchedulerApp.Commands;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

class Program
{
    static void Main(string[] args)
    {
        // Initialize MailService with Gmail SMTP configuration
        var mailService = new MailService("smtp.gmail.com", 587, "dptaskscheduler@gmail.com", "xdsiturnhvvviibk");

        // Initialize services
        var backupService = new BackupService("Backups");
        var fileService = new FileService();
        var resourceMonitorService = new ResourceMonitorService(mailService);

        // Attach observers to services
        var consoleNotifier = new ConsoleNotifier();
        var mailNotifier = new MailNotifier(mailService, "recipient@example.com");

        backupService.AddObserver(consoleNotifier);
        backupService.AddObserver(mailNotifier);

        fileService.AddObserver(consoleNotifier);

        // Initialize TaskManager
        var taskManager = new TaskManager();

        // Initialize commands
        var backupCommand = new BackupCommand(taskManager, backupService);
        var deleteFilesCommand = new DeleteFilesCommand(taskManager, fileService);
        var resourceMonitorCommand = new ResourceMonitorCommand(taskManager, resourceMonitorService);
        var reminderCommand = new ReminderCommand(taskManager, mailService);

        // Main menu loop
        while (true)
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

            string? choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n--- Create a Backup Task ---");
                    backupCommand.Execute();
                    break;

                case "2":
                    Console.WriteLine("\n--- Create a Delete Files Task ---");
                    deleteFilesCommand.Execute();
                    break;

                case "3":
                    Console.WriteLine("\n--- Create a Resource Monitoring Task ---");
                    resourceMonitorCommand.Execute();
                    break;

                case "4":
                    Console.WriteLine("\n--- Create a Reminder Task ---");
                    reminderCommand.Execute();
                    break;

                case "5":
                    Console.WriteLine("\n--- List of Tasks ---");
                    taskManager.ListTasks();
                    break;

                case "6":
                    Console.WriteLine("\n--- Remove a Task ---");
                    taskManager.ListTasks();
                    Console.Write("Enter the task number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int taskNumber))
                    {
                        taskManager.RemoveTask(taskNumber - 1);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                    break;

                case "7":
                    Console.WriteLine("Exiting the application...");
                    return;

                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }
}
