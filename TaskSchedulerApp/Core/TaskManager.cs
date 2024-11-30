using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Manages all scheduled tasks in the system.
    /// </summary>
    public class TaskManager
    {
        private readonly List<TaskBase> _tasks;

        /// <summary>
        /// Constructor to initialize the TaskManager.
        /// </summary>
        public TaskManager()
        {
            _tasks = new List<TaskBase>();
        }

        /// <summary>
        /// Adds a new task to the manager.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        public void AddTask(TaskBase task)
        {
            _tasks.Add(task);
            Console.WriteLine($"Task '{task.Name}' added successfully.");
        }

        /// <summary>
        /// Lists all tasks currently managed.
        /// </summary>
        public void ListTasks()
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.WriteLine("List of tasks:");
            for (int i = 0; i < _tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_tasks[i].GetDetails()}");
            }
        }

        /// <summary>
        /// Stops and removes a task by its index in the list.
        /// </summary>
        /// <param name="index">The index of the task to be removed.</param>
        public void RemoveTask(int index)
        {
            if (index < 0 || index >= _tasks.Count)
            {
                Console.WriteLine("Invalid task index.");
                return;
            }

            var task = _tasks[index];
            task.StopTask();
            _tasks.RemoveAt(index);
            Console.WriteLine($"Task '{task.Name}' removed successfully.");
        }

        /// <summary>
        /// Finds a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to find.</param>
        /// <returns>The matching task or null if not found.</returns>
        public TaskBase? FindTaskById(Guid taskId)
        {
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }
    }
}
