using MediaBrowser.Model.Plugins;
using System;

namespace Jellyfin.Plugin.EpisodesAutoRenamer.Configuration
{
    /// <summary>
    /// Represents the configuration for the Episodes AutoRenamer plugin.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether the configuration is filled.
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Gets or sets the file extensions to include for renaming.
        /// </summary>
        public string[] IncludedFileExtensions { get; set; }

        /// <summary>
        /// Gets or sets the locations to include for renaming operations.
        /// </summary>
        public string[] IncludedLocations { get; set; }

        /// <summary>
        /// Gets or sets the regular expressions used for matching episodes.
        /// </summary>
        public string[] RegularExpressions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the full file path should be read for matching episodes.
        /// </summary>
        public bool ReadFullPath { get; set; }

        /// <summary>
        /// Gets or sets the mask used for generating new file names.
        /// </summary>
        public string NewFileMask { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfiguration"/> class with default values.
        /// </summary>
        public PluginConfiguration()
        {
            IsFilled = false;
            IncludedFileExtensions = Array.Empty<string>();
            IncludedLocations = Array.Empty<string>();
            RegularExpressions = Array.Empty<string>();
            ReadFullPath = false;
            NewFileMask = string.Empty;
        }
    }
}
