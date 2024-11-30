using System;
using System.Threading.Tasks;
using TaskSchedulerApp.Core;
using TaskSchedulerApp.Services;

namespace TaskSchedulerApp.Tasks
{
    /// <summary>
    /// A task for managing file cleanup operations.
    /// </summary>
    public class DeleteFilesTask : TaskBase
    {
        private readonly FileService _fileService;
        private readonly string _folderPath;
        private readonly int _intervalInMinutes;
        private readonly int? _minutesOld;
        private readonly int? _keepRecentCount;

        /// <summary>
        /// Initializes a new delete files task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="fileService">The service responsible for file operations.</param>
        /// <param name="folderPath">The folder to clean up.</param>
        /// <param name="intervalInMinutes">The cleanup interval in minutes.</param>
        /// <param name="minutesOld">The threshold in minutes for deleting old files.</param>
        /// <param name="keepRecentCount">The number of recent files to keep.</param>
        public DeleteFilesTask(string name, FileService fileService, string folderPath, int intervalInMinutes, int? minutesOld = null, int? keepRecentCount = null)
            : base(name)
        {
            _fileService = fileService;
            _folderPath = folderPath;
            _intervalInMinutes = intervalInMinutes;
            _minutesOld = minutesOld;
            _keepRecentCount = keepRecentCount;
        }

        /// <summary>
        /// Executes the delete files task.
        /// </summary>
        protected override async Task Execute()
        {
            while (IsRunning)
            {
                if (_minutesOld.HasValue)
                {
                    _fileService.DeleteOldFiles(_folderPath, _minutesOld.Value);
                }
                else if (_keepRecentCount.HasValue)
                {
                    _fileService.DeleteFilesExceptRecent(_folderPath, _keepRecentCount.Value);
                }

                await Task.Delay(_intervalInMinutes * 60000); // Wait for the next interval
            }
        }

        public override string GetDetails()
        {
            string criteria = _minutesOld.HasValue ? $"Older than {_minutesOld} minutes" :
                             _keepRecentCount.HasValue ? $"Keep {_keepRecentCount} most recent files" : "No criteria specified";
            return base.GetDetails() + $", Folder: {_folderPath}, Interval: {_intervalInMinutes} minutes, Criteria: {criteria}";
        }
    }
}
