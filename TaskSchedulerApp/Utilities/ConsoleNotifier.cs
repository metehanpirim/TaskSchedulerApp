using System;
using TaskSchedulerApp.Core;

namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Observer that writes notifications to the console.
    /// </summary>
    public class ConsoleNotifier : IObserver
    {
        public void Notify(string message)
        {
            Console.WriteLine($"[NOTIFICATION] {message}");
        }
    }
}
