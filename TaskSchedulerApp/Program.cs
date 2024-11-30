using System;
using TaskSchedulerApp.Initializers;

class Program
{
    static void Main(string[] args)
    {
        // Initialize the application
        var appInitializer = new AppInitializer();

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
                    appInitializer.BackupCommand.Execute();
                    break;

                case "2":
                    Console.WriteLine("\n--- Create a Delete Files Task ---");
                    appInitializer.DeleteFilesCommand.Execute();
                    break;

                case "3":
                    Console.WriteLine("\n--- Create a Resource Monitoring Task ---");
                    appInitializer.ResourceMonitorCommand.Execute();
                    break;

                case "4":
                    Console.WriteLine("\n--- Create a Reminder Task ---");
                    appInitializer.ReminderCommand.Execute();
                    break;

                case "5":
                    Console.WriteLine("\n--- List of Tasks ---");
                    appInitializer.TaskManager.ListTasks();
                    break;

                case "6":
                    Console.WriteLine("\n--- Remove a Task ---");
                    appInitializer.TaskManager.ListTasks();
                    Console.Write("Enter the task number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int taskNumber))
                    {
                        appInitializer.TaskManager.RemoveTask(taskNumber - 1);
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
