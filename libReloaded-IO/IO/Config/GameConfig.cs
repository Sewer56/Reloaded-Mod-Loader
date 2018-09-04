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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Reloaded.IO.Config.Interfaces;
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Defines a general struct for the loader game configuration file.
    /// </summary>
    public class GameConfig : IGameConfigV1
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
        /// Specifies command-line arguments to be passed to the application.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string CommandLineArgs { get; set; } = "";

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
            if (gameConfigDirectory == LoaderPaths.GetGlobalGameConfigDirectory()) // Function call gemerates !Global if it does not exist.
                config = GameConfig.SetGlobalConfigProperties(config);

            // Create mod directory if nonexistant.
            if (!Directory.Exists(LoaderPaths.GetModLoaderModDirectory() + $"\\{config.ModDirectory}"))
            { Directory.CreateDirectory(LoaderPaths.GetModLoaderModDirectory() + $"\\{config.ModDirectory}"); }

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
            string json = JsonConvert.SerializeObject(gameConfig, Formatting.Indented);

            // Write to disk
            File.WriteAllText(gameConfig.ConfigLocation, json);
        }

        /// <summary>
        /// Retrieves the properties of the special "global" configuration, a master above 
        /// all configuration which is used to load mods for all games.
        /// </summary>
        /// <returns>The hardcoded global configuration properties.</returns>
        public static GameConfig GetGlobalConfig()
        {
            return ParseConfig(LoaderPaths.GetGlobalGameConfigDirectory());
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

        /// <summary>
        /// Obtains the game configuration based off of the supplied executable path.
        /// Returns the matching game configurations, if any.
        /// </summary>
        /// <param name="executablePath">The executable location to find the game configuration for.</param>
        /// <returns>The game configuration with a matching absolute executable path to the given path.</returns>
        public static GameConfig GetGameConfigFromExecutablePath(string executablePath)
        {
            // Retrieve all game configurations.
            List<GameConfig> gameConfigs = ConfigManager.GetAllGameConfigs();

            // Returns the game configuration with a matching executable path.
            return gameConfigs.First(x => Path.Combine(x.GameDirectory, x.ExecutableLocation) == executablePath);
        }

        /// <summary>
        /// Retrieves all currently enabled mods for the current specified game configuration,
        /// including the global modifications not covered by <see cref="ConfigManager"/>.
        /// Note: This list is not topologically sorted and ignores the dependency order, consider passing the result to <see cref="TopologicallySortConfigurations"/>
        /// in order to obtain a list of mods where the dependencies of mods are loaded first.
        /// </summary>
        /// <param name="gameConfiguration">The game configuration to obtain the enabled mods for, including globals.</param>
        /// <returns>A list of all currently enabled mods for the game, including globals.</returns>
        public static List<ModConfig> GetAllEnabledMods(GameConfig gameConfiguration)
        {
            // Retrieve the game configurations
            List<ModConfig> modConfigurations = new List<ModConfig>(gameConfiguration.EnabledMods.Count);

            // Read each game configuration
            foreach (string directory in gameConfiguration.EnabledMods)
            {
                string configPath = Path.Combine(LoaderPaths.GetModLoaderModDirectory(), gameConfiguration.ModDirectory, directory);
                try { modConfigurations.Add(ModConfig.ParseConfig(configPath, gameConfiguration)); }
                catch (Exception ex) { MessageBox.Show("One of your mod configurations is missing or corrupt: " + ex.Message); }
            }


            // Append global mods and return.
            modConfigurations.AddRange(GetEnabledGlobalMods());

            return modConfigurations;
        }

        /// <summary>
        /// Retrieves all currently enabled global modifications, executed regardless of 
        /// the individual mod configuration.
        /// </summary>
        /// <returns>A list of currently globally enabled mods.</returns>
        public static List<ModConfig> GetEnabledGlobalMods()
        {
            // Get global mod configuration.
            GameConfig globalModConfig = GameConfig.ParseConfig(LoaderPaths.GetGlobalGameConfigDirectory());

            // Get all mods for the global configuration.
            List<ModConfig> modConfigurations = ConfigManager.GetAllModsForGame(globalModConfig);

            // Filter out the enabled mods.
            modConfigurations = modConfigurations.Where(x =>
                // Get folder name containing the mod = Path.GetFileName(Path.GetDirectoryName(x.ModLocation))
                // Check if it's contained in the enabled mods list
                globalModConfig.EnabledMods.Contains(Path.GetFileName(Path.GetDirectoryName(x.ModLocation)))).ToList();

            return modConfigurations;
        }

        #region Topological Sorting
        /// <summary>
        /// Defines an individual node in a tree or graph structure which can be used
        /// for depth first search such as topological sorting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Node<T>
        {
            internal Mark Visited { get; set; } = Mark.NotVisited;
            public T Element { get; set; }
            public List<Node<T>> Edges { get; set; } // Stores the individual list of dependencies of the current node. i.e. Dependent Mods.

            public Node(T element)
            { this.Element = element; }
        }

        internal enum Mark
        {
            NotVisited,
            Visiting,
            Visited
        }

        /// <summary>
        /// Topologically sorts a provided list of mod loader mods, based on the individual dependencies
        /// of the individual mods within an unsorted list.
        /// </summary>
        /// <param name="modConfigurations">The individual modification configurations to be sorted.</param>
        /// <returns>A sorted list of mods whereby mods depending on other mods are loaded first.</returns>
        public static List<ModConfig> TopologicallySortConfigurations(List<ModConfig> modConfigurations)
        {
            // List of sorted modifications.
            List<ModConfig> sortedMods = new List<ModConfig>(modConfigurations.Count);

            // Populate a list of all nodes (without dependencies).
            List<Node<ModConfig>> allNodes = new List<Node<ModConfig>>(modConfigurations.Count);
            foreach (var modConfigNode in modConfigurations)
                allNodes.Add(new Node<ModConfig>(modConfigNode));

            // Generate list of dependencies for our individual node list, using our existing node list.
            foreach (var node in allNodes)
                node.Edges = allNodes.Where(x => node.Element.Dependencies.Contains(x.Element.ModId)).ToList();

            // Perform a depth first search topologically.
            // While there are still unmarked nodes to be visited.
            while (allNodes.Count(x => x.Visited == Mark.NotVisited) > 0)
            {
                // Visit first unvisited node.
                VisitNode(allNodes.First(x => x.Visited == Mark.NotVisited), sortedMods);
            }

            return sortedMods;
        }

        /// <summary>
        /// Visits a node, recursively building a complete list of mod configurations to be loaded.
        /// </summary>
        /// <param name="node">The node to be visited.</param>
        /// <param name="sortedMods">Contains an (originally) empty list of sorted mods.</param>
        private static void VisitNode(Node<ModConfig> node, List<ModConfig> sortedMods)
        {
            // Already fully visited, no need to return.
            if (node.Visited == Mark.Visited)
                return;

            // Not a Directed Acyclic Graph (No cycle to loop back)
            if (node.Visited == Mark.Visiting)
                return;

            // Mark as currently visiting.
            node.Visited = Mark.Visiting;

            // For each dependency of the current node.
            foreach (var dependency in node.Edges)
            { VisitNode(dependency, sortedMods); }

            // Mark the node as visited.
            node.Visited = Mark.Visited;
            sortedMods.Add(node.Element);
        }
        #endregion

    }
}
