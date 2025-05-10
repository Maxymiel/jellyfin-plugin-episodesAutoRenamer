# Jellyfin Episodes AutoRenamer Plugin

<p align="center">
<img alt="Episodes AutoRenamer" src="./docs/logo.png"/>
<br/>
</p>

Plugin for automatically renaming TV show episode files in your Jellyfin media library.

# Install Process
## From repository (recommended)

1. Go to `Dashboard -> Plugins` and select the `Repositories` tab

2. Add a new repository with the following details:
   * Repository name: `Episodes AutoRenamer`
   * Repository URL: `https://github.com/Maxymiel/jellyfin-plugin-episodesAutoRenamer/raw/master/manifest.json`
3. Go to the `Catalog` tab
4. Find `Episodes AutoRenamer` in the `General` section and install it
5. Restart Jellyfin to apply the changes

## From zip file

1. Download the latest available release from the repository releases
2. Extract the included files into the `plugins/EpisodesAutoRenamer` directory within your Jellyfin installation (you may need to create these folders manually).
3. Restart Jellyfin

## From sources

1. Clone or download this repository
2. Ensure you have .NET Core SDK setup and installed
3. Build the plugin with the following command:

```sh
dotnet publish -c Release -o output
```

4. Place the `Jellyfin.Plugin.EpisodesAutoRenamer.dll` file in a folder called `plugins/EpisodesAutoRenamer` within your Jellyfin installation (you may need to create these folders manually).

# Configuring the Plugin

Since the plugin works by moving files in your file system, it will not run immediately after installation. You must configure it first and verify that the configuration behaves as expected.

## Choose Libraries

Select the libraries in which the plugin will search for files to rename.

## Set Regular Expression

Define regular expressions to extract season and episode numbers from your filenames.

You can choose to use the full file path or a partial one by toggling the checkbox `Read full file path when determining episode name.`
For example, if your filenames include the season number, you can uncheck this option and set expressions that account for it.

To create and test your regular expressions, you can use [regex101](https://regex101.com/) — enter your expression and test strings to verify that season and episode numbers are correctly detected.

> [!IMPORTANT]
> Regular expressions are applied top to bottom, matching the first valid result. Therefore, order them from most specific to most general.

> [!IMPORTANT]
> The plugin detects the season number using the `<sn>` group, and the episode number using the `<ep>` group.

> [!TIP]
> Default settings target a pattern where episodes are stored in a folder named "Season 00", which works in most cases.

## Set Target File Extensions

Specify which file extensions the plugin should consider, separated by semicolons.
By default: `.mp4`, `.mkv`, and `.avi`.

## Set Filename Mask

Set the mask for how the new filename should look.

## Checking that everything works as expected

To make sure everything is working as expected, click the `Get files to rename` button.
You’ll see a text field showing how the plugin plans to rename each file.

If the output looks correct, you can click the `Rename all files` button to start the renaming process.

## Scheduler

In the **Scheduler**, you can set how often the plugin should check file names automatically (disabled by default).
