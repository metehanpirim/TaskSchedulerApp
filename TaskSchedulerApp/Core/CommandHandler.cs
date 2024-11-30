using System;

namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Handles user input and delegates commands to the appropriate tasks.
    /// </summary>
    public class CommandHandler
    {
        private readonly TaskManager _taskManager;

        /// <summary>
        /// Initializes the CommandHandler with a TaskManager instance.
        /// </summary>
        /// <param name="taskManager">The task manager to manage tasks.</param>
        public CommandHandler(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        /// <summary>
        /// Processes user commands and interacts with the TaskManager.
        /// </summary>
        public void ProcessCommand()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. List Tasks");
            Console.WriteLine("3. Remove Task");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter task name: ");
                    string? taskName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(taskName))
                    {
                        var newTask = new ExampleTask(taskName); // Example task creation
                        _taskManager.AddTask(newTask);
                    }
                    break;

                case "2":
                    _taskManager.ListTasks();
                    break;

                case "3":
                    _taskManager.ListTasks();
                    Console.Write("Enter the task number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int taskIndex))
                    {
                        _taskManager.RemoveTask(taskIndex - 1);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                    break;

                case "4":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Example task for demonstration purposes.
    /// </summary>
    public class ExampleTask : TaskBase
    {
        public ExampleTask(string name) : base(name) { }

        protected override async Task Execute()
        {
            Console.WriteLine($"Executing task '{Name}'...");
            await Task.Delay(1000); // Simulate task execution
            Console.WriteLine($"Task '{Name}' completed.");
        }
    }
}
