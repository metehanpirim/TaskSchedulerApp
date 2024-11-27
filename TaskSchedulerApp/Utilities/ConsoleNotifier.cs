namespace TaskSchedulerApp.Utilities
{
    public class ConsoleNotifier : IBackupObserver
    {
        public void Notify(string message)
        {
            Console.WriteLine($"[NOTIFICATION] {message}");
        }
    }
}
