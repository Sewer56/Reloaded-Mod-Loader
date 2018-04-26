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

using Newtonsoft.Json;
using Reloaded.Utilities;

namespace Reloaded
{
    /// <summary>
    /// The strings class contains various common string names and global settings related to strings,
    /// present for consistency and to ensure that no wrong changes occur when refactoring.
    /// For file paths and where things are stored, see <see cref="LoaderPaths"/>.
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Contains the strings belonging to the Mod Loader's Loader.
        /// </summary>
        public static class Loader
        {
            /// <summary>
            /// Contains the name of the 32bit mod DLL file for a Reloaded Mod Loader mod.
            /// </summary>
            public static string Mod32BitDllFile = "main32.dll";

            /// <summary>
            /// Contains the name of the 32bit mod DLL file for a Reloaded Mod Loader mod.
            /// </summary>
            public static string Mod64BitDllFile = "main64.dll";
        }

        /// <summary>
        /// Contains the strings belonging to the Mod Loader's Launcher.
        /// </summary>
        public static class Launcher
        {
            /// <summary>
            /// Contains the name of the menus used in the Reloaded Mod Loader launcher.
            /// </summary>
            public static class Menus
            {
                /// <summary>
                /// Contains the name of the Main Menu used in Reloaded Launcher.
                /// </summary>
                public static string MainMenuName = "Main Menu";

                /// <summary>
                /// Contains the name of the Mods Menu used in Reloaded Launcher.
                /// </summary>
                public static string ModsMenuName = "Mods Menu";

                /// <summary>
                /// Contains the name of the Input Menu used in Reloaded Launcher.
                /// </summary>
                public static string InputMenuName = "Reloaded Input Stack";

                /// <summary>
                /// Contains the name of the Theme Menu used in Reloaded Launcher.
                /// </summary>
                public static string ThemeMenuName = "Theme Menu";

                /// <summary>
                /// Contains the name of the Manage Menu used in Reloaded Launcher.
                /// </summary>
                public static string ManageMenuName = "Game Management Menu";

                /// <summary>
                /// Contains the name of the About Menu used in Reloaded Launcher.
                /// </summary>
                public static string AboutMenuName = "About [Reloaded]";
            }

            /// <summary>
            /// Contains the name of the preview banners used for mods of Reloaded Mod Loader.
            /// </summary>
            public static string BannerName = "Banner.png";

            /// <summary>
            /// Contains the name of the default log file used in Reloaded.
            /// </summary>
            public static string DefaultLogFileName = "Reloaded-Log.txt";
        }

        /// <summary>
        /// Contains the strings shared between all of the Reloaded Projects using libReloaded.
        /// </summary>
        public static class Common
        {
            /// <summary>
            /// The attach setting used in the Mod Loader Loader.
            /// </summary>
            public static string LoaderSettingAttach { get; } = "--attach";

            /// <summary>
            /// The config setting used in the Mod Loader Loader.
            /// </summary>
            public static string LoaderSettingConfig { get; } = "--config";

            /// <summary>
            /// The log setting used in the Mod Loader Loader.
            /// </summary>
            public static string LoaderSettingLog { get; } = "--log";

            /// <summary>
            /// The name of the mod list that is used globally, regardless of the game.
            /// </summary>
            public static string GlobalModName { get; } = "!-- Global Mods --!";

            /// <summary>
            /// The name of the folder used for the global mod list, both in Reloaded-Config\Games
            /// and Reloaded-Mods\
            /// </summary>
            public static string GlobalModFolder { get; } = "!Global";
        }

        /// <summary>
        /// Contains the strings shared between all of the Reloaded parsers, such as file locations,
        /// comment characters and the such.
        /// </summary>
        public static class Parsers
        {
            /// <summary>
            /// Defines the default JSON serialization settings for the project.
            /// </summary>
            public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };

            /// <summary>
            /// Contains the character used in ini file comments and comments in other similar files.
            /// </summary>
            public static string CommentCharacter { get; } = "#";

            /// <summary>
            /// Contains the name of the file used for reading and writing of enabled mods.
            /// </summary>
            public static string EnabledModsFile { get; } = "Enabled_Mods.ini";

            /// <summary>
            /// Contains the name of the file used for reading and writing of general mod loader configs.
            /// </summary>
            public static string ConfigFile { get; } = "Config.ini";

            /// <summary>
            /// Contains the name of the file used for reading and writing of theme properties and styles.
            /// </summary>
            public static string ThemeFile { get; } = "Theme.json";

            /// <summary>
            /// Contains the name of the file used for reading and writing of general mod loader configs.
            /// </summary>
            public static string ConfigFileNew { get; } = "Config.json";
        }
    }
}
