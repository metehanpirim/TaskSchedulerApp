using TaskSchedulerApp.Commands;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Initializers
{
    public class AppInitializer
    {
        public MailService MailService { get; private set; }
        public BackupService BackupService { get; private set; }
        public FileService FileService { get; private set; }
        public ResourceMonitorService ResourceMonitorService { get; private set; }
        public TaskManager TaskManager { get; private set; }

        public ICommand BackupCommand { get; private set; }
        public ICommand DeleteFilesCommand { get; private set; }
        public ICommand ResourceMonitorCommand { get; private set; }
        public ICommand ReminderCommand { get; private set; }

        public AppInitializer()
        {
            // Initialize MailService with Gmail SMTP configuration
            MailService = new MailService("smtp.gmail.com", 587, "dptaskscheduler@gmail.com", "xdsiturnhvvviibk");

            // Initialize services
            BackupService = new BackupService("DefaultBackupFolder");
            FileService = new FileService();
            ResourceMonitorService = new ResourceMonitorService(MailService);

            // Attach observers
            SetupNotifiers(BackupService, FileService, MailService);

            // Initialize TaskManager
            TaskManager = TaskManager.Instance;

            // Initialize commands
            BackupCommand = new BackupCommand(TaskManager, BackupService);
            DeleteFilesCommand = new DeleteFilesCommand(TaskManager, FileService);
            ResourceMonitorCommand = new ResourceMonitorCommand(TaskManager, ResourceMonitorService);
            ReminderCommand = new ReminderCommand(TaskManager, MailService);
        }

        /// <summary>
        /// Sets up notifiers for services based on user preferences.
        /// </summary>
        private void SetupNotifiers(BackupService backupService, FileService fileService, MailService mailService)
        {
            var consoleNotifier = new ConsoleNotifier();
            backupService.AddObserver(consoleNotifier);
            fileService.AddObserver(consoleNotifier);

            Console.Write("Would you like to receive email notifications? (yes/no): ");
            string? emailNotificationChoice = Console.ReadLine()?.Trim().ToLower();

            if (emailNotificationChoice == "yes")
            {
                Console.Write("Enter the email address to receive notifications: ");
                string? recipientEmail = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(recipientEmail))
                {
                    Console.WriteLine("Invalid email address. Skipping email notifications.");
                }
                else
                {
                    var mailNotifier = new MailNotifier(mailService, recipientEmail);
                    backupService.AddObserver(mailNotifier);
                    fileService.AddObserver(mailNotifier);
                    Console.WriteLine($"Email notifications will be sent to: {recipientEmail}");
                }
            }
            else
            {
                Console.WriteLine("Email notifications are disabled.");
            }
        }
    }
}
