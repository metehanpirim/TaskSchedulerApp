using System;
using System.Net;
using System.Net.Mail;
using TaskSchedulerApp.Utilities;

namespace TaskSchedulerApp.Services
{
    /// <summary>
    /// A service for sending emails using the SMTP protocol.
    /// </summary>
    public class MailService
    {
        private readonly string _smtpServer; // SMTP server address
        private readonly int _smtpPort;     // SMTP port number
        private readonly string _username; // SMTP username
        private readonly string _password; // SMTP password

        /// <summary>
        /// Initializes the MailService with the provided SMTP configuration.
        /// </summary>
        /// <param name="smtpServer">The address of the SMTP server.</param>
        /// <param name="smtpPort">The port number of the SMTP server.</param>
        /// <param name="username">The username for SMTP authentication.</param>
        /// <param name="password">The password for SMTP authentication.</param>
        public MailService(string smtpServer, int smtpPort, string username, string password)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        public void SendMail(string to, string subject, string body)
        {
            try
            {
                // Create a mail message
                var mail = new MailMessage(_username, to, subject, body);

                // Configure the SMTP client
                var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true // Enables SSL for secure email transmission
                };

                // Send the email
                client.Send(mail);

                // Log success
                Logger.Log($"Email successfully sent to: {to}");
            }
            catch (Exception ex)
            {
                // Log any errors during the email sending process
                Logger.LogError($"Error while sending email to {to}: {ex.Message}");
            }
        }
    }
}
