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
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Helper class that makes working with and obtaining collections of configurations easier for the user.
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Retrieves all of the game individual game configurations, including the global
        /// game configuration containing global mods.
        /// </summary>
        public static List<GameConfig> GetAllGameConfigs()
        {
            // Retrieves the name of all directories in the 'Games' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetModLoaderGamesDirectory());

            // Retrieve the game configurations
            List<GameConfig> gameConfigurations = new List<GameConfig>(directories.Length);

            // Read each game configuration
            foreach (string directory in directories)
            {
                try { gameConfigurations.Add(GameConfig.ParseConfig(directory)); }
                catch (Exception ex) { MessageBox.Show("One of your game configurations is missing or corrupt: " + ex.Message); }
            }

            // Return.
            return gameConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual game configurations.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public static void WriteAllGameConfigs(List<GameConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (GameConfig gameConfiguration in gameConfigurations) GameConfig.WriteConfig(gameConfiguration);
        }

        /// <summary>
        /// Retrieves all of the game individual mod configurations for the currently selected game.
        /// </summary>
        /// <remarks>The returned mod configurations do have the optional ParentGame field complete.</remarks>
        public static List<ModConfig> GetAllModsForGame(GameConfig gameConfiguration)
        {
            // Retrieves the name of all directories in the 'Mods' folder for the game.
            string[] modDirectories = Directory.GetDirectories(LoaderPaths.GetModLoaderModDirectory() + "\\" + gameConfiguration.ModDirectory);

            // Retrieve the game configurations
            List<ModConfig> modConfigurations = new List<ModConfig>(modDirectories.Length);

            // Read each game configuration
            foreach (string directory in modDirectories)
                try { modConfigurations.Add(ModConfig.ParseConfig(directory, gameConfiguration)); }
                catch (Exception ex) { MessageBox.Show("One of your mod configurations is missing or corrupt: " + ex.Message); }

            // Return.
            return modConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual mod configurations for the currently selected game.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public static void WriteAllMods(List<GameConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (GameConfig gameConfiguration in gameConfigurations) GameConfig.WriteConfig(gameConfiguration);
        }

        /// <summary>
        /// Retrieves all of the individual theme configurations.
        /// </summary>
        public static List<ThemeConfig> GetAllThemeConfigs()
        {
            // Retrieves the name of all directories in the 'Themes' folder.
            string[] directories = Directory.GetDirectories(LoaderPaths.GetModLoaderThemeDirectory());

            // Retrieve the game configurations
            List<ThemeConfig> themeConfigurations = new List<ThemeConfig>(directories.Length);

            // Read each game configuration
            foreach (string directory in directories)
                try { themeConfigurations.Add(ThemeConfig.ParseConfig(directory)); }
                catch (Exception ex) { MessageBox.Show("One of your theme configurations is missing or corrupt: " + ex.Message); }

            // Return.
            return themeConfigurations;
        }

        /// <summary>
        /// Writes all of the game individual game configurations.
        /// </summary>
        /// <param name="gameConfigurations">List of game configurations to be written back.</param>
        public static void WriteAllThemeConfigs(List<ThemeConfig> gameConfigurations)
        {
            // Read each game configuration
            foreach (ThemeConfig themeConfiguration in gameConfigurations)
                ThemeConfig.WriteConfig(themeConfiguration);
        }
    }
}
