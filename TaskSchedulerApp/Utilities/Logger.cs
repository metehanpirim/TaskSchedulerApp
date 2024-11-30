using System;

namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// A utility class for logging messages and errors to the console.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs a general message with a timestamp.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
        }

        /// <summary>
        /// Logs an error message with a timestamp.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public static void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}");
        }
    }
}
