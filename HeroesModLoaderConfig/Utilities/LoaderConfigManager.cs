using SonicHeroes.Misc;
using SonicHeroes.Misc.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Utilities
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
        /// Stores the Mod Loader Configuration Parser.
        /// </summary>
        public GameConfigParser GameConfigParser { get; set; }

        /// <summary>
        /// Starts up all of the individual parsers.
        /// </summary>
        public LoaderConfigManager()
        {
            // Instantiate the Parsers
            LoaderConfigParser = new LoaderConfigParser();
            GameConfigParser = new GameConfigParser();
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
    }
}
