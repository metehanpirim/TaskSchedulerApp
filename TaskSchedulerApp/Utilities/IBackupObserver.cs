namespace TaskSchedulerApp.Utilities
{
    public interface IBackupObserver
    {
        void Notify(string message);
    }
}
