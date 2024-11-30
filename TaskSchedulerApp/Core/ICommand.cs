namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Defines the basic structure for all commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();
    }
}
