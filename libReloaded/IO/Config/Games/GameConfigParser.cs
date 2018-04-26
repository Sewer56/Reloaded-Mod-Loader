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

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reloaded.Utilities;

namespace Reloaded.IO.Config.Games
{
    /// <summary>
    /// Simple parser for the loader game configuration files.
    /// </summary>
    public static class GameConfigParser
    {
        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="gameConfigDirectory">The directory containing the configuration file for the game. e.g. $LOADERPATH\\Reloaded-Mods\\Games\\Sonic-Heroes</param>
        /// <returns></returns>
        public static GameConfig ParseConfig(string gameConfigDirectory)
        {
            // Sets the configuration file location for the game.
            string configFile = gameConfigDirectory + $"\\{Strings.Parsers.ConfigFile}";
            
            // Try parsing the config file, else backup to default one.
            GameConfig config;

            try
            {
                config = File.Exists(configFile)
                         ? JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(configFile))
                         : new GameConfig();
            }
            catch { config = new GameConfig(); }
                

            // Set the directory of this config.
            config.ConfigLocation = configFile;

            // Override names if default config.
            if (gameConfigDirectory == LoaderPaths.GetGlobalGameConfigDirectory())
                config = GameConfig.SetGlobalConfigProperties(config);

            // Return the config file.
            return config;
        }

        /// <summary>
        /// Writes out the config file to disk.
        /// </summary>
        /// <param name="gameConfig">The game configuration to be written to disk.</param>
        public static void WriteConfig(GameConfig gameConfig)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(gameConfig, Strings.Parsers.SerializerSettings);

            // Write to disk
            File.WriteAllText(gameConfig.ConfigLocation, json);
        }

        /// <summary>
        /// Defines a general struct for the loader game configuration file.
        /// </summary>
        public class GameConfig
        {
            /// <summary>
            /// Specifies the directory of the game.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string GameDirectory { get; set; } = "Undefined";

            /// <summary>
            /// Specifies the executable location.
            /// The executable location is relative to the game directory.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ExecutableLocation { get; set; } = "Undefined";

            /// <summary>
            /// Specifies the name of the game, as displayed by 
            /// the mod loader configuration utility. 
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string GameName { get; set; } = "Undefined";

            /// <summary>
            /// Specifies the version of the game, as displayed
            /// by the mod loader configuration utility.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string GameVersion { get; set; } = "Undefined";

            /// <summary>
            /// Specifies the directory where mods for this game are stored.
            /// The path is relative to Reloaded-Mods.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModDirectory { get; set; } = "Undefined";

            /// <summary>
            /// Specifies a list of enabled mods, separated by a comma.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public List<string> EnabledMods { get; set; } = new List<string>();

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the game configuration for re-save purposes.
            /// </summary>
            [JsonIgnore]
            public string ConfigLocation { get; set; }

            /// <summary>
            /// Retrieves the properties of the special "global" configuration, a master above 
            /// all configuration which is used to load mods for all games.
            /// </summary>
            /// <returns>The global configuration properties</returns>
            public static GameConfig GetGlobalConfigProperties()
            {
                // Creates the global mod directory if it does not exist.
                // This is just to ensure safe usage of the global config.
                LoaderPaths.GetGlobalModDirectory();

                return new GameConfig()
                {
                    ExecutableLocation = "All Executables",
                    ModDirectory = Strings.Common.GlobalModFolder,
                    ConfigLocation = LoaderPaths.GetGlobalGameConfigDirectory() + $"\\{Strings.Parsers.ConfigFile}",
                    GameDirectory = "Between Time and Space",
                    GameName = Strings.Common.GlobalModName,
                    GameVersion = "Reloaded"
                };  
            }

            /// <summary>
            /// Applies a set of global configuration properties to the supplied configuration.
            /// all configuration which is used to load mods for all games.
            /// </summary>
            /// <returns>The global configuration properties</returns>
            public static GameConfig SetGlobalConfigProperties(GameConfig gameConfig)
            {
                gameConfig.ExecutableLocation = "All Executables";
                gameConfig.ModDirectory = Strings.Common.GlobalModFolder;
                gameConfig.ConfigLocation = LoaderPaths.GetGlobalGameConfigDirectory() + $"\\{Strings.Parsers.ConfigFile}";
                gameConfig.GameName = Strings.Common.GlobalModName;
                gameConfig.GameVersion = "Reloaded";
                gameConfig.GameDirectory = "Between Time and Space";

                return gameConfig;
            }

            /// <summary>
            /// Retrieves the path to the banner image for the current game, as seen
            /// in the main menu's first tab in Reloaded.
            /// </summary>
            /// <param name="config">The game configuration to retrieve the banner location for.</param>
            /// <returns>The location of the banner image for the current game configuraion.</returns>
            public static string GetBannerPath(GameConfig config)
            {
                return Path.GetDirectoryName(config.ConfigLocation) + $"\\{Strings.Launcher.BannerName}";
            }
        }
    }
}
