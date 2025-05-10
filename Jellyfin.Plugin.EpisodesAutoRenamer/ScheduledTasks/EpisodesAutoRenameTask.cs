using MediaBrowser.Controller.Library;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.EpisodesAutoRenamer.ScheduledTasks
{
    /// <summary>
    /// Represents a scheduled task for automatically renaming episode files.
    /// </summary>
    public class EpisodesAutoRenameTask : IScheduledTask
    {
        private readonly ILogger _logger;
        private readonly AutoRenameManager _autoRenameManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesAutoRenameTask"/> class.
        /// </summary>
        /// <param name="libraryManager">The library manager for accessing media library data.</param>
        /// <param name="logger">The logger for logging operations.</param>
        /// <param name="fileSystem">The file system interface for file operations.</param>
        public EpisodesAutoRenameTask(
            ILibraryManager libraryManager,
            ILogger<AutoRenameManager> logger,
            IFileSystem fileSystem
        )
        {
            _logger = logger;
            _autoRenameManager = new AutoRenameManager(libraryManager, logger, fileSystem);
        }

        /// <summary>
        /// Executes the task to rename episode files.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="progress">A progress reporter for tracking the renaming progress.</param>
        public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            _logger.LogInformation("Rename files...");

            int renamed = _autoRenameManager.RenameFiles(progress);

            _logger.LogInformation("{Renamed} files renamed", renamed);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the default triggers for the scheduled task.
        /// </summary>
        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return new List<TaskTriggerInfo>();
        }

        /// <summary>
        /// Executes the task asynchronously to rename episode files.
        /// </summary>
        /// <param name="progress">A progress reporter for tracking the renaming progress.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            return Execute(cancellationToken, progress);
        }

        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        public string Name => "Episodes Auto Rename";

        /// <summary>
        /// Gets the unique key for the task.
        /// </summary>
        public string Key => "EpisodesAutoRenameTask";

        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        public string Description => "Scans selected libraries and renames files using regular expressions";

        /// <summary>
        /// Gets the category of the task.
        /// </summary>
        public string Category => "Episodes AutoRenamer";
    }
}
