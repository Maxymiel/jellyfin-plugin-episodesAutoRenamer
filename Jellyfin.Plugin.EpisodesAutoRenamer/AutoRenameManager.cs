using MediaBrowser.Controller.Library;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.EpisodesAutoRenamer
{
    /// <summary>
    /// Manages the auto-renaming of episodes based on configured rules.
    /// </summary>
    public class AutoRenameManager
    {
        private readonly ILogger<AutoRenameManager> _logger;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoRenameManager"/> class.
        /// </summary>
        /// <param name="libraryManager">The library manager for accessing media library data.</param>
        /// <param name="logger">The logger for logging operations.</param>
        /// <param name="fileSystem">The file system interface for file operations.</param>
        public AutoRenameManager(
            ILibraryManager libraryManager,
            ILogger<AutoRenameManager> logger,
            IFileSystem fileSystem
        )
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Counts the number of files that need to be renamed.
        /// </summary>
        /// <returns>The count of files to rename.</returns>
        public int CountFilesToRename()
        {
            return FilesToRename().Count();
        }

        /// <summary>
        /// Renames the files that match the renaming criteria.
        /// </summary>
        /// <param name="progress">An optional progress reporter for tracking the renaming progress.</param>
        /// <returns>The number of files successfully renamed.</returns>
        public int RenameFiles(IProgress<double> progress = null!)
        {
            var filesToRename = FilesToRename().ToList();
            int renamedFiles = 0;

            foreach (var (originalFileName, newFileName) in filesToRename)
            {
                try
                {
                    File.Move(originalFileName, newFileName);

                    var percent = renamedFiles / (double)filesToRename.Count * 100;
                    progress?.Report((int)percent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to rename file {OriginalFileName} to {NewFileName}", originalFileName, newFileName);
                }
                finally
                {
                    renamedFiles++;
                    _logger.LogInformation("Renamed file {OriginalFileName} to {NewFileName}", originalFileName, newFileName);
                }
            }

            progress?.Report(100);

            return renamedFiles;
        }

        /// <summary>
        /// Retrieves a list of files that are eligible for renaming.
        /// </summary>
        public IEnumerable<(string originalFileName, string newFileName)> FilesToRename()
        {
            if (Plugin.Instance.PluginConfiguration.IncludedLocations.Length == 0
                || Plugin.Instance.PluginConfiguration.IncludedFileExtensions.Length == 0
                || Plugin.Instance.PluginConfiguration.RegularExpressions.Length == 0)
            {
                yield break;
            }

            foreach (var location in Plugin.Instance.PluginConfiguration.IncludedLocations)
            {
                foreach (var originalFile in _fileSystem.GetFiles(location, true))
                {
                    var fileName = Plugin.Instance.PluginConfiguration.ReadFullPath ? originalFile.FullName : Path.GetFileName(originalFile.FullName);
                    var extension = Path.GetExtension(originalFile.FullName);

                    if (!Plugin.Instance.PluginConfiguration.IncludedFileExtensions
                        .Any(x => x.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    if (TryDetectEpisode(
                        fileName,
                        Plugin.Instance.PluginConfiguration.RegularExpressions,
                        out string episode))
                    {
                        var newFileName = $"{Path.Combine(Path.GetDirectoryName(originalFile.FullName)!, episode)}{extension}";

                        if (originalFile.FullName != newFileName)
                        {
                            yield return (originalFile.FullName, newFileName);
                        }
                    }
                }
            }
        }

        private bool TryDetectEpisode(string originalFileName, string[] patterns, out string episode)
        {
            int episodeNumber = -1;
            int seasonNumber = 0;

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(originalFileName, pattern);
                if (match.Success && int.TryParse(match.Groups["ep"].Value, out episodeNumber))
                {
                    _ = int.TryParse(match.Groups["sn"].Value, out seasonNumber);
                    break;
                }
            }

            if (episodeNumber == -1)
            {
                episode = null!;
                return false;
            }

            episode = FormatEpisodeName(seasonNumber, episodeNumber, Plugin.Instance.PluginConfiguration.NewFileMask);

            return true;
        }

        private string FormatEpisodeName(int season, int episode, string pattern)
        {
            pattern = Regex.Replace(pattern, @"<sn:(\d+)>", match =>
            {
                int digits = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                return $"{season.ToString($"D{digits}", CultureInfo.InvariantCulture)}";
            });

            pattern = Regex.Replace(pattern, @"<ep:(\d+)>", match =>
            {
                int digits = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                return $"{episode.ToString($"D{digits}", CultureInfo.InvariantCulture)}";
            });

            return pattern;
        }
    }
}
