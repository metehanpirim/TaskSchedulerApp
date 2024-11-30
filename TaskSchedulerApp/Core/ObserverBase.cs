namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Interface for observers that react to notifications.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Notifies the observer with a message.
        /// </summary>
        /// <param name="message">The notification message.</param>
        void Notify(string message);
    }
}
