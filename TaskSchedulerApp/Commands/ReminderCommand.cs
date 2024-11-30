using System;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Tasks;

namespace TaskSchedulerApp.Commands
{
    /// <summary>
    /// Command to create and start a reminder task.
    /// </summary>
    public class ReminderCommand : ICommand
    {
        private readonly TaskManager _taskManager;
        private readonly MailService _mailService;

        public ReminderCommand(TaskManager taskManager, MailService mailService)
        {
            _taskManager = taskManager;
            _mailService = mailService;
        }

        public void Execute()
        {
            Console.Write("Enter recipient email: ");
            string? email = Console.ReadLine();

            Console.Write("Enter email subject: ");
            string? subject = Console.ReadLine();

            Console.Write("Enter email body: ");
            string? body = Console.ReadLine();

            Console.Write("Enter reminder time (HH:mm): ");
            if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan reminderTime))
            {
                var task = new ReminderTask("Reminder Task", _mailService, email!, subject!, body!, reminderTime);
                _taskManager.AddTask(task);

                // Run StartTask in a background thread
                Task.Run(() => task.StartTask());
            }
            else
            {
                Console.WriteLine("Invalid time format.");
            }
        }
    }
}
