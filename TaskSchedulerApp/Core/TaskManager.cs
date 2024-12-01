using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TaskSchedulerApp.Services;
using TaskSchedulerApp.Tasks;

namespace TaskSchedulerApp.Core
{
    /// <summary>
    /// Manages all scheduled tasks in the system with persistent storage.
    /// </summary>
    public class TaskManager
    {
        private readonly List<TaskBase> _tasks;
        private const string StorageFile = "tasks.json";
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor to initialize the TaskManager and load tasks from storage.
        /// </summary>
        public TaskManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _tasks = LoadTasksFromStorage();
        }

        /// <summary>
        /// Adds a new task to the manager and saves it to storage.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        public void AddTask(TaskBase task)
        {
            _tasks.Add(task);
            SaveTasksToStorage();
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
            SaveTasksToStorage();
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

        /// <summary>
        /// Saves all tasks to a JSON file for persistent storage.
        /// </summary>
        private void SaveTasksToStorage()
        {
            var serializedTasks = JsonConvert.SerializeObject(_tasks, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(StorageFile, serializedTasks);
        }

        /// <summary>
        /// Loads tasks from the JSON storage file.
        /// </summary>
        /// <returns>A list of tasks loaded from storage.</returns>
        private List<TaskBase> LoadTasksFromStorage()
        {
            if (!File.Exists(StorageFile)) return new List<TaskBase>();

            try
            {
                var serializedTasks = File.ReadAllText(StorageFile);
                var tasks = JsonConvert.DeserializeObject<List<TaskBase>>(serializedTasks, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }) ?? new List<TaskBase>();

                // Rebind services to tasks
                foreach (var task in tasks)
                {
                    RebindTaskDependencies(task);
                    Task.Run(() => task.StartTask());
                }

                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks from storage: {ex.Message}");
                return new List<TaskBase>();
            }
        }

        /// <summary>
        /// Rebinds service dependencies to a task after deserialization.
        /// </summary>
        /// <param name="task">The task to rebind dependencies for.</param>
        private void RebindTaskDependencies(TaskBase task)
        {
            switch (task)
            {
                case BackupTask backupTask:
                    var backupServiceFactory = _serviceProvider.GetService<Factories.BackupServiceFactory>();
                    if (backupServiceFactory != null)
                    {
                        backupTask._backupService = backupServiceFactory.Create();
                    }
                    break;

                case DeleteFilesTask deleteFilesTask:
                    var fileServiceFactory = _serviceProvider.GetService<Factories.FileServiceFactory>();
                    if (fileServiceFactory != null)
                    {
                        deleteFilesTask._fileService = fileServiceFactory.Create();
                    }
                    break;

                case ResourceMonitorTask resourceMonitorTask:
                    var resourceMonitorServiceFactory = _serviceProvider.GetService<Factories.ResourceMonitorServiceFactory>();
                    if (resourceMonitorServiceFactory != null)
                    {
                        resourceMonitorTask._monitorService = resourceMonitorServiceFactory.Create();
                    }
                    break;

                case ReminderTask reminderTask:
                    var mailService = _serviceProvider.GetService<MailService>();
                    if (mailService != null)
                    {
                        reminderTask._mailService = mailService;
                    }
                    break;
            }
        }
    }
}
