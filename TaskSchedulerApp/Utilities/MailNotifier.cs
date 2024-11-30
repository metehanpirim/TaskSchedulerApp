using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Observer that sends notifications via email.
    /// </summary>
    public class MailNotifier : IObserver
    {
        private readonly MailService _mailService;
        private readonly string _email;

        public MailNotifier(MailService mailService, string email)
        {
            _mailService = mailService;
            _email = email;
        }

        public void Notify(string message)
        {
            _mailService.SendMail(_email, "Task Notification", message);
        }
    }
}
