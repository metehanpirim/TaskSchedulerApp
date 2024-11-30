using System;
using System.Threading.Tasks;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Tasks
{
    /// <summary>
    /// A task for sending reminder emails at specified times or intervals.
    /// </summary>
    public class ReminderTask : TaskBase
    {
        private readonly MailService _mailService;
        private readonly string _email;
        private readonly string _subject;
        private readonly string _body;
        private readonly TimeSpan _reminderTime;

        /// <summary>
        /// Initializes a new reminder task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="mailService">The service responsible for sending emails.</param>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="reminderTime">The time of day to send the reminder.</param>
        public ReminderTask(string name, MailService mailService, string email, string subject, string body, TimeSpan reminderTime)
            : base(name)
        {
            _mailService = mailService;
            _email = email;
            _subject = subject;
            _body = body;
            _reminderTime = reminderTime;
        }

        /// <summary>
        /// Executes the reminder task.
        /// </summary>
        protected override async Task Execute()
        {
            while (IsRunning)
            {
                var now = DateTime.Now;
                var nextReminder = new DateTime(now.Year, now.Month, now.Day, _reminderTime.Hours, _reminderTime.Minutes, 0);

                if (now > nextReminder)
                    nextReminder = nextReminder.AddDays(1);

                var delay = nextReminder - now;
                await Task.Delay(delay);

                if (IsRunning) // Check again before sending
                {
                    _mailService.SendMail(_email, _subject, _body);
                    Logger.Log($"Reminder sent to {_email} with subject '{_subject}'");
                }
            }
        }

        public override string GetDetails()
        {
            return base.GetDetails() + $", Email: {_email}, Subject: {_subject}, Time: {_reminderTime}";
        }
    }
}
