using System;
using System.Threading.Tasks;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Tasks
{
    /// <summary>
    /// A task for managing backup operations.
    /// </summary>
    public class BackupTask : TaskBase
    {
        private readonly BackupService _backupService;
        private readonly string _sourceFolderPath;
        private readonly string _backupFolderPath;
        private readonly int _intervalInMinutes;

        /// <summary>
        /// Initializes a new backup task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="backupService">The service responsible for backups.</param>
        /// <param name="sourceFolderPath">The folder to back up.</param>
        /// <param name="intervalInMinutes">The backup interval in minutes.</param>
        public BackupTask(string name, BackupService backupService, string sourceFolderPath, string backupFolderPath, int intervalInMinutes)
            : base(name)
        {
            _backupService = backupService;
            _sourceFolderPath = sourceFolderPath;
            _intervalInMinutes = intervalInMinutes;
            _backupFolderPath = backupFolderPath;
        }

        /// <summary>
        /// Executes the backup task.
        /// </summary>
        protected override async Task Execute()
        {
            while (IsRunning)
            {
                _backupService.ExecuteBackup(_sourceFolderPath);
                await Task.Delay(_intervalInMinutes * 60000); // Wait for the next interval
            }
        }

        public override string GetDetails()
        {
            return base.GetDetails() + $", Source: {_sourceFolderPath}, Target: {_backupFolderPath}, Interval: {_intervalInMinutes} minutes";
        }
    }
}
