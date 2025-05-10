using Jellyfin.Plugin.EpisodesAutoRenamer.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;

namespace Jellyfin.Plugin.EpisodesAutoRenamer
{
    /// <summary>
    /// Represents the Episodes AutoRenamer plugin for Jellyfin.
    /// </summary>
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="appPaths"></param>
        /// <param name="xmlSerializer"></param>
        public Plugin(IServerApplicationPaths appPaths, IXmlSerializer xmlSerializer) : base(appPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public override string Name => "Episodes AutoRenamer";

        /// <summary>
        /// Gets the instance of the plugin.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public override string Description => "Regex-based episode auto-renaming";

        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        public PluginConfiguration PluginConfiguration => Configuration;

        /// <summary>
        /// Gets the unique identifier for the plugin.
        /// </summary>
        public override Guid Id => Guid.Parse("167d6b6c-1f03-44c1-8f71-86a57ca6c9c8");

        /// <summary>
        /// Gets the plugin info.
        /// </summary>
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return
            [
                new PluginPageInfo
                {
                    Name = "Episodes AutoRenamer",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.configPage.html"
                }
            ];
        }
    }
}
