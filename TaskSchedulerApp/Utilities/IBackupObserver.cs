namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Defines a contract for objects that observe and respond to notifications.
    /// </summary>
    public interface IBackupObserver
    {
        /// <summary>
        /// Method to handle notifications.
        /// </summary>
        /// <param name="message">The notification message.</param>
        void Notify(string message);
    }
}
