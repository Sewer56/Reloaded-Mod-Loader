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
using Reloaded.Input.Modules;
using Reloaded.Utilities;

namespace Reloaded.IO.Config.Loader
{
    /// <summary>
    /// Simple parser for the main mod loader configuration file.
    /// </summary>
    public static class LoaderConfigParser
    {
        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <returns>Parses a Mod Loader configuration file.</returns>
        public static Config ParseConfig()
        {
            // Try parsing the config file, else return default one.
            try
            {
                return File.Exists(LoaderPaths.GetModLoaderConfig())
                    ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(LoaderPaths.GetModLoaderConfig()))
                    : new Config();
            }
            catch  { return new Config(); }
        }

        /// <summary>
        /// Writes out the config file to the config file location/
        /// </summary>
        /// <param name="config">The config file to write to disk.</param>
        public static void WriteConfig(Config config)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(config, Strings.Parsers.SerializerSettings);

            // Write to disk
            File.WriteAllText(LoaderPaths.GetModLoaderConfig(), json);
        }

        /// <summary>
        /// Defines a general struct for the Mod Loader Configuration tile.
        /// </summary>
        [JsonObject(ItemRequired = Required.Always)]
        public class Config
        {
            /// <summary>
            /// Specifies the preferred configuration for DirectInput devices.
            /// Specifies the current configuration type used for DirectInput devices.
            /// Controllers can be differentiated by product (identical controllers will carry identical config) or...
            /// Controllers can be differentiated by instance (each controller is unique but also tied to USB port).
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public Remapper.DirectInputConfigType DirectInputConfigType { get; set; } = Remapper.DirectInputConfigType.ProductGUID;

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
            public bool EnableAutomaticUpdates { get; set; } = true;

            /// <summary>
            /// Set to true to enable silent updates in Reloaded Mod Loader.
            /// Shows no prompt to the user.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public bool SilentUpdates { get; set; }
        }
    }
}
