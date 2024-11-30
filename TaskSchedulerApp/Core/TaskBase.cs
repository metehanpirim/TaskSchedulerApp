using System;
using System.Threading.Tasks;

namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Base class for all scheduled tasks.
    /// </summary>
    public abstract class TaskBase
    {
        /// <summary>
        /// Unique identifier for the task.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Descriptive name of the task.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates whether the task is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Constructor to initialize the base properties of the task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        protected TaskBase(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsRunning = false;
        }

        /// <summary>
        /// Starts the execution of the task.
        /// </summary>
        public async Task StartTask()
        {
            if (IsRunning)
            {
                Console.WriteLine($"Task '{Name}' is already running.");
                return;
            }

            Console.WriteLine($"Starting task: {Name}");
            IsRunning = true;
            await Execute();
        }

        /// <summary>
        /// Stops the execution of the task.
        /// </summary>
        public void StopTask()
        {
            if (!IsRunning)
            {
                Console.WriteLine($"Task '{Name}' is not running.");
                return;
            }

            Console.WriteLine($"Stopping task: {Name}");
            IsRunning = false;
        }

        /// <summary>
        /// Abstract method to define the execution logic for the task.
        /// </summary>
        protected abstract Task Execute();

        /// <summary>
        /// Provides details of the task for listing purposes.
        /// </summary>
        /// <returns>String containing task details.</returns>
        public virtual string GetDetails()
        {
            return $"Task ID: {Id}, Name: {Name}, IsRunning: {IsRunning}";
        }
    }
}
