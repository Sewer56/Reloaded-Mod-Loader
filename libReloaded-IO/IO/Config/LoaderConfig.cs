/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.IO;
using Newtonsoft.Json;
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Defines a general struct for the Mod Loader Configuration tile.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class LoaderConfig
    {
        /// <summary>
        /// Specifies the subdirectory containing the current mod loader theme.
        /// The subdirectory is relative to Reloaded-Config/Themes/
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string CurrentTheme { get; set; } = "!Reloaded";

        /// <summary>
        /// Specifies whether or not to automatically exit the launcher after launching a game.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool ExitAfterLaunch { get; set; } = true;

        /// <summary>
        /// Allows for the user to receive pre-release builds, marked as
        /// pre-release on Github.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool AllowBetaBuilds { get; set; }

        /// <summary>
        /// Automatically updates without requesting the user, Google Chrome style.
        /// No alerts, nothing.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool EnableAutomaticUpdates { get; set; }

        /// <summary>
        /// Set to true to enable silent updates in Reloaded Mod Loader.
        /// Shows no prompt to the user.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool SilentUpdates { get; set; }

        /// <summary>
        /// If set to false, displays the first launch prompt when booting Reloaded.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public bool FirstLaunch { get; set; } = true;

        /// <summary>
        /// Stores the physical directory location of the last loaded game configuration for automatic re-selection in launcher.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string LastGameFolder { get; set; }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <returns>Parses a Mod Loader configuration file.</returns>
        public static LoaderConfig ParseConfig()
        {
            // Try parsing the Config file, else return default one.
            try
            {
                return File.Exists(LoaderPaths.GetModLoaderConfig())
                    ? JsonConvert.DeserializeObject<LoaderConfig>(File.ReadAllText(LoaderPaths.GetModLoaderConfig()))
                    : new LoaderConfig();
            }
            catch { return new LoaderConfig(); }
        }

        /// <summary>
        /// Writes out the Config file to the Config file location.
        /// </summary>
        /// <param name="config">The Config file to write to disk.</param>
        public static void WriteConfig(LoaderConfig config)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);

            // Write to disk
            File.WriteAllText(LoaderPaths.GetModLoaderConfig(), json);
        }
    }
}
