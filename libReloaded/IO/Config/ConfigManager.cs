/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
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
using System.Windows.Forms;
using Reloaded.IO.Config.Games;
using Reloaded.IO.Config.Loader;
using Reloaded.IO.Config.Mods;
using Reloaded.IO.Config.Themes;
using Reloaded.Utilities;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Stores all of the currently loaded configurator parsers for the mod loader configuration manager.
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// Stores the Mod Loader Configuration Parser.
        /// </summary>
        public LoaderConfigParser LoaderConfigParser { get; set; } = new LoaderConfigParser();

        /// <summary>
        /// Stores the Mod Loader Game Configuration Parser.
        /// </summary>
        public GameConfigParser GameConfigParser { get; set; } = new GameConfigParser();

        /// <summary>
        /// Stores the Mod Loader Mod Configuration Parser.
        /// </summary>
        public ModConfigParser ModConfigParser { get; set; } = new ModConfigParser();

        /// <summary>
        /// Stores the theme configuration parser.
        /// </summary>
        public ThemeConfigParser ThemeConfigParser { get; set; } = new ThemeConfigParser();

        /// <summary>
        /// Retrieves all of the game individual game configurations, including the global
        /// game configuration containing global mods.
        /// </summary>
        public List<GameConfigParser.GameConfig> GetAllGameConfigs()
        {
            // Retrieves the name of all directories in the 'Games' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetModLoaderGamesDirectory());

            // Retrieve the game configurations
            List<GameConfigParser.GameConfig> gameConfigurations = new List<GameConfigParser.GameConfig>(directories.Length);

            // Read each game configuration
            foreach (string directory in directories)
            {
                try { gameConfigurations.Add(GameConfigParser.ParseConfig(directory)); }
                catch (Exception ex) { MessageBox.Show("One of your game configurations is missing or corrupt: " + ex.Message); }
            }

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
            foreach (GameConfigParser.GameConfig gameConfiguration in gameConfigurations) GameConfigParser.WriteConfig(gameConfiguration);
        }

        /// <summary>
        /// Retrieves all of the game individual mod configurations for the currently selected game.
        /// </summary>
        public List<ModConfigParser.ModConfig> GetAllMods(GameConfigParser.GameConfig gameConfiguration)
        {
            // Retrieves the name of all directories in the 'Mods' folder for the game.
            string[] modDirectories = Directory.GetDirectories(LoaderPaths.GetModLoaderModDirectory() + "\\" + gameConfiguration.ModDirectory);

            // Retrieve the game configurations
            List<ModConfigParser.ModConfig> modConfigurations = new List<ModConfigParser.ModConfig>(modDirectories.Length);

            // Read each game configuration
            foreach (string directory in modDirectories)
                try { modConfigurations.Add(ModConfigParser.ParseConfig(directory)); }
                catch (Exception ex) { MessageBox.Show("One of your mod configurations is missing or corrupt: " + ex.Message); }

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
            foreach (GameConfigParser.GameConfig gameConfiguration in gameConfigurations) GameConfigParser.WriteConfig(gameConfiguration);
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
            foreach (string directory in directories)
                try { themeConfigurations.Add(ThemeConfigParser.ParseConfig(directory)); }
                catch (Exception ex) { MessageBox.Show("One of your theme configurations is missing or corrupt: " + ex.Message); }

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
            foreach (ThemeConfigParser.ThemeConfig themeConfiguration in gameConfigurations)
                ThemeConfigParser.WriteConfig(themeConfiguration);
        }
    }
}
