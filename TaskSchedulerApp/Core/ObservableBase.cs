using System.Collections.Generic;

namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Base class for observable objects that notify observers of changes.
    /// </summary>
    public abstract class ObservableBase
    {
        private readonly List<IObserver> _observers = new();

        /// <summary>
        /// Adds an observer to the list.
        /// </summary>
        /// <param name="observer">The observer to add.</param>
        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        /// <summary>
        /// Removes an observer from the list.
        /// </summary>
        /// <param name="observer">The observer to remove.</param>
        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Notifies all observers with a message.
        /// </summary>
        /// <param name="message">The notification message.</param>
        protected void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Notify(message);
            }
        }
    }
}
