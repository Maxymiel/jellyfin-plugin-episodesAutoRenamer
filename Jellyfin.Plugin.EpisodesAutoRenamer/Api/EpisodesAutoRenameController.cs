using MediaBrowser.Controller.Library;
using MediaBrowser.Model.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Jellyfin.Plugin.EpisodesAutoRenamer.Api
{
    /// <summary>
    /// API Controller for managing episode auto-renaming operations.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("EpisodesAutoRename")]
    public class EpisodesAutoRenameController : ControllerBase
    {
        private readonly AutoRenameManager _autoRenameManager;
        private readonly ILogger<AutoRenameManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesAutoRenameController"/> class.
        /// </summary>
        /// <param name="libraryManager">The library manager for accessing media library data.</param>
        /// <param name="logger">The logger for logging operations.</param>
        /// <param name="fileSystem">The file system interface for file operations.</param>
        public EpisodesAutoRenameController(
            ILibraryManager libraryManager,
            ILogger<AutoRenameManager> logger,
            IFileSystem fileSystem
        )
        {
            _autoRenameManager = new AutoRenameManager(libraryManager, logger, fileSystem);
            _logger = logger;
        }

        /// <summary>
        /// Counts the number of files that need to be renamed.
        /// </summary>
        /// <returns>The count of files to rename.</returns>
        [HttpGet("CountFilesToRename")]
        public ActionResult<int> CountFilesToRenameRequest()
        {
            _logger.LogInformation("Starting file count for renaming...");

            int count = _autoRenameManager.CountFilesToRename();

            _logger.LogInformation("Completed");
            return count;
        }

        /// <summary>
        /// Renames the files that match the renaming criteria.
        /// </summary>
        /// <returns>The number of files successfully renamed.</returns>
        [HttpGet("RenameFiles")]
        public ActionResult<int> RenameFilesRequest()
        {
            _logger.LogInformation("Manual file renaming...");

            int count = _autoRenameManager.RenameFiles();

            _logger.LogInformation("Completed");
            return count;
        }

        /// <summary>
        /// Retrieves a list of files that are eligible for renaming.
        /// </summary>
        /// <returns>A list of original and new file names.</returns>
        [HttpGet("GetFilesToRename")]
        public ActionResult GetFilesToRenameRequest()
        {
            _logger.LogInformation("Enumerating files for renaming...");

            IEnumerable<(string originalFileName, string newFileName)> count = _autoRenameManager.FilesToRename();

            _logger.LogInformation("Completed");

            return Ok(new { list = count.Select(x => $"{x.originalFileName} > {x.newFileName}").ToList() });
        }
    }
}
