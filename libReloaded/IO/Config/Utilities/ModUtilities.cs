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
using System.Linq;
using Reloaded.IO.Config.Games;
using Reloaded.IO.Config.Mods;
using Reloaded.Utilities;

namespace Reloaded.IO.Config.Utilities
{
    /// <summary>
    /// Contains various different utilities related to collecting and managing mod and game information.
    /// Primarily intended to be used from Reloaded Mod Loader mods (DLL Injected Assemblies)
    /// </summary>
    public static class ModUtilities
    {
        /// <summary>
        /// Obtains the game configuration based off of the supplied executable path.
        /// Returns the matching game configurations, if any.
        /// </summary>
        /// <param name="executablePath">The executable location to find the game configuration for.</param>
        /// <returns>The game configuration with a matching absolute executable path to the given path.</returns>
        public static GameConfigParser.GameConfig GetGameConfigFromExecutablePath(string executablePath)
        {
            // Retrieve all game configurations.
            List<GameConfigParser.GameConfig> gameConfigs = ConfigManager.GetAllGameConfigs();

            // Returns the game configuration with a matching executable path.
            return gameConfigs.First(x => Path.Combine(x.GameDirectory, x.ExecutableLocation) == executablePath);
        }

        /// <summary>
        /// Retrieves all currently enabled mods for the current specified game configuration,
        /// including the global modifications not covered by <see cref="ConfigManager"/>
        /// </summary>
        /// <param name="gameConfiguration">The game configuration to obtain the enabled mods for, including globals.</param>
        /// <returns>A list of all currently enabled mods for the game, including globals.</returns>
        public static List<ModConfigParser.ModConfig> GetAllEnabledMods(GameConfigParser.GameConfig gameConfiguration)
        {
            // Get mods enabled for this game.
            List<ModConfigParser.ModConfig> modConfigurations = ConfigManager.GetAllMods(gameConfiguration);

            // Filter out the enabled mods.
            modConfigurations = modConfigurations.Where(x =>
                    // Get folder name containing the mod = Path.GetFileName(Path.GetDirectoryName(x.ModLocation))
                    // Check if it's contained in the enabled mods list
                    gameConfiguration.EnabledMods.Contains(Path.GetFileName(Path.GetDirectoryName(x.ModLocation)))).ToList();

            // Append global mods and return.
            modConfigurations.AddRange(GetEnabledGlobalMods());

            return modConfigurations;
        }

        /// <summary>
        /// Retrieves all currently enabled global modifications, executed regardless of 
        /// the individual mod configuration.
        /// </summary>
        /// <returns>A list of currently globally enabled mods.</returns>
        public static List<ModConfigParser.ModConfig> GetEnabledGlobalMods()
        {
            // Get global mod configuration.
            GameConfigParser.GameConfig globalModConfig = GameConfigParser.ParseConfig(LoaderPaths.GetGlobalGameConfigDirectory());

            // Get all mods for the global configuration.
            List<ModConfigParser.ModConfig> modConfigurations = ConfigManager.GetAllMods(globalModConfig);

            // Filter out the enabled mods.
            modConfigurations = modConfigurations.Where(x =>
                // Get folder name containing the mod = Path.GetFileName(Path.GetDirectoryName(x.ModLocation))
                // Check if it's contained in the enabled mods list
                globalModConfig.EnabledMods.Contains(Path.GetFileName(Path.GetDirectoryName(x.ModLocation)))).ToList();

            return modConfigurations;
        }
    }
}
