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

namespace Reloaded.Misc
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
        private static readonly string MOD_LOADER_LINK_FILE = Path.GetTempPath() + "\\Reloaded-Link.txt";

        /// <summary>
        /// Specifies the relative location of the main configuration file for the loader.
        /// </summary>
        private static readonly string RELATIVELOCATION_CONFIGFILE = "\\Reloaded-Config\\Config.ini";

        /// <summary>
        /// Specifies the relative location of the configuration directory for the loader.
        /// </summary>
        private static readonly string RELATIVELOCATION_CONFIG = "\\Reloaded-Config";

        /// <summary>
        /// Specifies the relative location of the configuration directory for the loader.
        /// </summary>
        private static readonly string RELATIVELOCATION_THEMES = "\\Reloaded-Config\\Themes";

        /// <summary>
        /// Specifies the relative location of the individual game backup files relative to the mod loader.
        /// </summary>
        private static string RELATIVELOCATION_BACKUP = "\\Reloaded-Backup";

        /// <summary>
        /// Specifies the relative location of the mod loader's mod directory.
        /// </summary>
        private static readonly string RELATIVELOCATION_MODS = "\\Reloaded-Mods";

        /// <summary>
        /// Specifies the relative location of the mod loader libraries relative to the mod loader.
        /// </summary>
        private static readonly string RELATIVELOCATION_LIBRARIES = "\\Reloaded-Libraries";

        /// <summary>
        /// Specifies the relative location of the mod loader libraries relative to the mod loader.
        /// </summary>
        private static readonly string RELATIVELOCATION_GAMES = "\\Reloaded-Config\\Games";

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
        /// Retrieves the directory of the mod loader itself, useful for reading configuration
        /// files by modifications, mod loader libraries and other programs.
        /// </summary>
        public static string GetModLoaderDirectory()
        {
            return File.ReadAllText(MOD_LOADER_LINK_FILE);
        }

        /// <summary>
        /// Retrieves the location of the text file where the link file to be written which defines the loader location is stored.
        /// </summary>
        public static string GetModLoaderLinkLocation()
        {
            return MOD_LOADER_LINK_FILE;
        }

        /// <summary>
        /// Retrieves the location of the mod loader's main configuration file.
        /// </summary>
        public static string GetModLoaderConfig()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_CONFIGFILE;
        }

        /// <summary>
        /// Retrieves the mod loader's main config directory.
        /// </summary>
        public static string GetModLoaderConfigDirectory()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_CONFIG;
        }

        /// <summary>
        /// Retrieves the mod loader's main library directory.
        /// </summary>
        public static string GetModLoaderLibraryDirectory()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_LIBRARIES;
        }

        /// <summary>
        /// Retrieves the mod loader's game configuration directory.
        /// </summary>
        public static string GetModLoaderGameDirectory()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_GAMES;
        }

        /// <summary>
        /// Retrieves the mod loader's theme configuration directory.
        /// </summary>
        public static string GetModLoaderThemeDirectory()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_THEMES;
        }

        /// <summary>
        /// Retrieves the mod loader's main mod directory.
        /// </summary>
        public static string GetModLoaderModDirectory()
        {
            return GetModLoaderDirectory() + RELATIVELOCATION_MODS;
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
