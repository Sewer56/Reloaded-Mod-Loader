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

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Reloaded.IO.Config;
using Reloaded.IO.Config.Interfaces;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows;
using Reloaded_Plugin_System;
using Reloaded_Plugin_System.Config;
using Reloaded_Plugin_System.Utilities;

namespace ReloadedLauncher
{
    /// <summary>
    /// Provides access to global application objects, settings and other common and/or shared variables.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Stores the current theme of the application.
        /// </summary>
        public static Theme Theme { get; set; }

        /// <summary>
        /// Defines the name of the menu that the user is currently in.
        /// Used for correctly setting the titlebar title.
        /// </summary>
        public static string CurrentMenuName { get; set; }

        /// <summary>
        /// Stores the base form, which contains all of the child forms.
        /// </summary>
        public static Base BaseForm { get; set; }

        /// <summary>
        /// Stores the current configuration for the mod loader.
        /// </summary>
        public static LoaderConfig LoaderConfiguration { get; set; }

        /// <summary>
        /// Stores the currently loaded/highlighted game configuration.
        /// Changed when the user changes the game in the main menu.
        /// </summary>
        public static GameConfig CurrentGameConfig { get; set; }

        /// <summary>
        /// Stores the currently loaded/highlighted modification/plugin configuration.
        /// Changed when the user changes the modification/plugin in the mod/plugin menu.
        /// </summary>
        public static ModConfig CurrentModConfig { get; set; }

        /// <summary>
        /// Stores the current loaded/highlighted theme configuration.
        /// Changes when the user changes the theme in the themes menu.
        /// </summary>
        public static ThemeConfig CurrentThemeConfig { get; set; }

        /// <summary>
        /// Stores the currently loaded/highlighted plugin from the list of plugins.
        /// </summary>
        public static PluginConfig CurrentPlugin { get; set; }

        /// <summary>
        /// Stores the list of all current Plugin configurations.
        /// </summary>
        public static List<PluginConfig> PluginConfigurations { get; set; }

        /// <summary>
        /// Stores the individual game configurations for loaded games.
        /// </summary>
        public static List<GameConfig> GameConfigurations
        {
            get => _gameConfigurations;
            set => _gameConfigurations = ResolveGameConfigurations(value);
        }
        private static List<GameConfig> _gameConfigurations;

        /// <summary>
        /// Stores the individual mod configuration for the currently selected game.
        /// </summary>
        public static List<ModConfig> ModConfigurations
        {
            get => _modConfigurations;
            set => _modConfigurations = ResolveModConfigurations(value);
        }
        private static List<ModConfig> _modConfigurations;


        /// <summary>
        /// Stores the individual theme configurations.
        /// </summary>
        public static List<ThemeConfig> ThemeConfigurations { get; set; }

        /*
            --------------------
            Plugin Handling Code
            --------------------
        */

        /// <summary>
        /// Allows individual plugins to intercept mod list loading.
        /// </summary>
        private static List<ModConfig> ResolveModConfigurations(List<ModConfig> newModConfigurations)
        {
            // Gets the configs, lets each mod tamper with them and restores configs.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            List<IModConfigV1> modConfigV1s = newModConfigurations.ToList<IModConfigV1>();
            foreach (var launcherEventPlugin in PluginLoader.LauncherEventPlugins)
                modConfigV1s = launcherEventPlugin.ResolveModConfigurations(modConfigV1s);

            return modConfigV1s.Cast<ModConfig>().ToList();
        }

        /// <summary>
        /// Allows individual plugins to intercept mod list loading.
        /// </summary>
        private static List<GameConfig> ResolveGameConfigurations(List<GameConfig> newGameConfigurations)
        {
            // Gets the configs, lets each mod tamper with them and restores configs.
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LauncherHelper.JsonIgnoreAttributeIgnorerContractResolver();

            List<IGameConfigV1> gameConfigsV1s = newGameConfigurations.Cast<IGameConfigV1>().ToList();
            foreach (var launcherEventPlugin in PluginLoader.LauncherEventPlugins)
                gameConfigsV1s = launcherEventPlugin.ResolveGameConfigurations(gameConfigsV1s);

            return gameConfigsV1s.Cast<GameConfig>().ToList();
        }
    }
}
