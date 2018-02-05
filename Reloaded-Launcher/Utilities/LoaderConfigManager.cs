using Reloaded.Misc;
using Reloaded.Misc.Config;
using System.Collections.Generic;
using System.IO;

namespace ReloadedLauncher.Utilities
{
    /// <summary>
    /// Stores all of the currently loaded configurator parsers for the mod loader configuration manager.
    /// </summary>
    public class LoaderConfigManager
    {
        /// <summary>
        /// Stores the Mod Loader Configuration Parser.
        /// </summary>
        public LoaderConfigParser LoaderConfigParser { get; set; }

        /// <summary>
        /// Stores the Mod Loader Game Configuration Parser.
        /// </summary>
        public GameConfigParser GameConfigParser { get; set; }

        /// <summary>
        /// Stores the Mod Loader Mod Configuration Parser.
        /// </summary>
        public ModConfigParser ModConfigParser { get; set; }

        /// <summary>
        /// Stores the theme configuration parser.
        /// </summary>
        public ThemeConfigParser ThemeConfigParser { get; set; }

        /// <summary>
        /// Starts up all of the individual parsers.
        /// </summary>
        public LoaderConfigManager()
        {
            // Instantiate the Parsers
            LoaderConfigParser = new LoaderConfigParser();
            GameConfigParser = new GameConfigParser();
            ModConfigParser = new ModConfigParser();
            ThemeConfigParser = new ThemeConfigParser();
        }

        /// <summary>
        /// Retrieves all of the game individual game configurations.
        /// </summary>
        public List<GameConfigParser.GameConfig> GetAllGameConfigs()
        {
            // Retrieves the name of all directories in the 'Games' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetModLoaderGameDirectory());

            // Retrieve the game configurations
            List<GameConfigParser.GameConfig> gameConfigurations = new List<GameConfigParser.GameConfig>(directories.Length);

            // Read each game configuration
            foreach (string directory in directories) { gameConfigurations.Add(GameConfigParser.ParseConfig(directory)); }

            // Return.
            return gameConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual game configurations.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public void WriteAllGameConfigs(List<GameConfigParser.GameConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (GameConfigParser.GameConfig gameConfiguration in gameConfigurations) { GameConfigParser.WriteConfig(gameConfiguration); }
        }

        /// <summary>
        /// Retrieves all of the game individual mod configurations for the currently selected game.
        /// </summary>
        /// <param name="gameDirectory">The game configuration which holds the path to the mods.</param>
        public List<ModConfigParser.ModConfig> GetAllMods(GameConfigParser.GameConfig gameConfiguration)
        {
            // Retrieves the name of all directories in the 'Mods' folder for the game.
            string[] modDirectories = Directory.GetDirectories(LoaderPaths.GetModLoaderModDirectory() + "\\" + gameConfiguration.ModDirectory);

            // Retrieve the game configurations
            List<ModConfigParser.ModConfig> modConfigurations = new List<ModConfigParser.ModConfig>(modDirectories.Length);

            // Read each game configuration
            foreach (string directory in modDirectories) { modConfigurations.Add(ModConfigParser.ParseConfig(directory)); }

            // Return.
            return modConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual mod configurations for the currently selected game.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public void WriteAllMods(List<GameConfigParser.GameConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (GameConfigParser.GameConfig gameConfiguration in gameConfigurations) { GameConfigParser.WriteConfig(gameConfiguration); }
        }

        /// <summary>
        /// Retrieves all of the individual theme configurations.
        /// </summary>
        public List<ThemeConfigParser.ThemeConfig> GetAllThemeConfigs()
        {
            // Retrieves the name of all directories in the 'Themes' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetModLoaderThemeDirectory());

            // Retrieve the game configurations
            List<ThemeConfigParser.ThemeConfig> themeConfigurations = new List<ThemeConfigParser.ThemeConfig>(directories.Length);

            // Read each game configuration
            foreach (string directory in directories) { themeConfigurations.Add(ThemeConfigParser.ParseConfig(directory)); }

            // Return.
            return themeConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual game configurations.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public void WriteAllThemeConfigs(List<ThemeConfigParser.ThemeConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (ThemeConfigParser.ThemeConfig themeConfiguration in gameConfigurations) { ThemeConfigParser.WriteConfig(themeConfiguration); }
        }
    }
}
