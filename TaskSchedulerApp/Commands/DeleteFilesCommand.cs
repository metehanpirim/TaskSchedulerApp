namespace TaskSchedulerApp.Commands
{
    using TaskSchedulerApp.Services;

    public class DeleteFilesCommand
    {
        private readonly ActiveFolderService _activeFolderService;

        public DeleteFilesCommand(ActiveFolderService activeFolderService)
        {
            _activeFolderService = activeFolderService;
        }

        public void Execute(int minutesOld)
        {
            _activeFolderService.DeleteFilesOlderThan(minutesOld);
        }
    }
}
