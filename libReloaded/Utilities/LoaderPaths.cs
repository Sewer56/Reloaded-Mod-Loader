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
using System.IO;
using System.Reflection;
using Reloaded.IO.Config.Loader;

namespace Reloaded.Utilities
{
    /// <summary>
    /// Class which helps with the retrieval of a certain folder locations for DLLs such as mods
    /// that are currently injected into a target process.
    /// </summary>
    public static class LoaderPaths
    {
        /// <summary>
        /// Specifies the location of the file which informs injected DLLs of the 
        /// current location of the mod loader in question. 
        /// </summary>
        private static readonly string ModLoaderData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Reloaded-Mod-Loader";

        /// <summary>
        /// Specifies the location of the configuration folder for Reloaded.
        /// </summary>
        private static readonly string ConfigLocation = GetModLoaderDirectory() + "\\Reloaded-Config";

        /// <summary>
        /// Specifies the location of the mods folder for Reloaded.
        /// </summary>
        private static readonly string ModsLocation = GetModLoaderDirectory() + "\\Reloaded-Mods";

        /// <summary>
        /// Specifies the location of the libraries folder for Reloaded.
        /// </summary>
        private static readonly string LibrariesLocation = GetModLoaderDirectory() + "\\Reloaded-Libraries";

        /// <summary>
        /// Specifies the location of the themes folder for Reloaded.
        /// </summary>
        private static readonly string ThemesLocation = GetModLoaderConfigDirectory() + "\\Themes";

        /// <summary>
        /// Specifies the location of the games folder for Reloaded.
        /// </summary>
        private static readonly string GamesLocation = GetModLoaderConfigDirectory() + "\\Games";

        /// <summary>
        /// Specifies the location of the configuration file for Reloaded.
        /// </summary>
        private static readonly string ConfigFileLocation = GetModLoaderConfigDirectory() + $"/{Strings.Parsers.ConfigFile}";

        /// <summary>
        /// Specifies the location of the global mods folder for Reloaded, mods which
        /// are always executed regardless of the game played in question.
        /// </summary>
        private static readonly string GlobalModsLocation = GetModLoaderModDirectory() + $"\\{Strings.Common.GlobalModFolder}";

        /// <summary>
        /// Specifies the location of the global config folder for Reloaded, a dummy config
        /// allowing for support of mods which are always loaded regardless of the game.
        /// </summary>
        private static readonly string GlobalConfigLocation = GetModLoaderGamesDirectory() + $"\\{Strings.Common.GlobalModFolder}";

        /// <summary>
        /// Retrieves the directory of the current process where the DLL resides in. i.e. the game directory.
        /// </summary>
        public static string GetProcessDirectory()
        {
            return Environment.CurrentDirectory;
        }

        /// <summary>
        /// Retrieves the directory where the mod DLL (main.dll) is stored.
        /// This function is intended to be ran from inside mods and retrieves the location of the current mod.
        /// </summary>
        public static string GetModDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Retrieves the directory of the mod loader user data, useful for reading configuration
        /// files by modifications, mod loader libraries and other programs.
        /// </summary>
        public static string GetModLoaderDirectory()
        {
            // Create Reloaded Directory if it doesn't exist.
            if (! Directory.Exists(ModLoaderData)) { Directory.CreateDirectory(ModLoaderData); }

            return ModLoaderData;
        }

        /// <summary>
        /// Retrieves the location of the mod loader's main configuration file.
        /// </summary>
        public static string GetModLoaderConfig()
        {
            // Create loader config if it does not exist.
            if (!File.Exists(ConfigFileLocation))
            {
                LoaderConfigParser localParser = new LoaderConfigParser();
                localParser.CreateConfig(ConfigFileLocation);
            }

            return ConfigFileLocation;
        }

        /// <summary>
        /// Returns true if the mod loader configuration file exists, else false.
        /// </summary>
        public static bool CheckModLoaderConfig()
        {
            return File.Exists(ConfigFileLocation);
        }

        /// <summary>
        /// Retrieves the mod loader's main config directory.
        /// </summary>
        public static string GetModLoaderConfigDirectory()
        {
            // Create folder if does not exist.
            if (!Directory.Exists(ConfigLocation))
            { Directory.CreateDirectory(ConfigLocation); }

            return ConfigLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's main library directory.
        /// </summary>
        public static string GetModLoaderLibraryDirectory()
        {
            if (! Directory.Exists(LibrariesLocation)) { Directory.CreateDirectory(LibrariesLocation); }
            return LibrariesLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's game configuration directory.
        /// </summary>
        public static string GetModLoaderGamesDirectory()
        {
            if (!Directory.Exists(GamesLocation)) { Directory.CreateDirectory(GamesLocation); }
            return GamesLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's theme configuration directory.
        /// </summary>
        public static string GetModLoaderThemeDirectory()
        {
            if (!Directory.Exists(ThemesLocation)) { Directory.CreateDirectory(ThemesLocation); }
            return ThemesLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's main mod directory.
        /// </summary>
        public static string GetModLoaderModDirectory()
        {
            if (!Directory.Exists(ModsLocation)) { Directory.CreateDirectory(ModsLocation); }
            return ModsLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's global mod directory, containing mods that will always
        /// be loaded, first regardless of the game.
        /// </summary>
        public static string GetGlobalModDirectory()
        {
            if (!Directory.Exists(GlobalModsLocation)) { Directory.CreateDirectory(GlobalModsLocation); }
            return GlobalModsLocation;
        }

        /// <summary>
        /// Retrieves the mod loader's global mod config directory, containing a dummy configuration to
        /// provide support for mods that will always be loaded regardless of the game.
        /// </summary>
        public static string GetGlobalConfigDirectory()
        {
            if (!Directory.Exists(GlobalConfigLocation)) { Directory.CreateDirectory(GlobalConfigLocation); }
            return GlobalConfigLocation;
        }

        /// <summary>
        /// Returns a path relative to the mod loader directory.
        /// e.g. D:/Stuff/ModLoader/Reloaded-Mods => Reloaded-Mods
        /// </summary>
        /// <param name="path">The path inside the mod loader configuration.</param>
        public static string GetModLoaderRelativePath(string path)
        {
            // Note: Last character will be a backslash, do not include.
            return path.Substring(GetModLoaderDirectory().Length + 1);
        }
    }
}
