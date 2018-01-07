using System;
using System.IO;
using System.Reflection;

namespace SonicHeroes.Misc
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
        private static string MOD_LOADER_LINK_FILE = Path.GetTempPath() + "\\Mod-Loader-Link.txt";

        /// <summary>
        /// Specifies the relative location of the main configuration file for the loader.
        /// </summary>
        private static string RELATIVELOCATION_CONFIGFILE = "\\Mod-Loader-Config\\Config.ini";

        /// <summary>
        /// Specifies the relative location of the configuration directory for the loader.
        /// </summary>
        private static string RELATIVELOCATION_CONFIG = "\\Mod-Loader-Config\\";

        /// <summary>
        /// Specifies the relative location of the individual game backup files relative to the mod loader.
        /// </summary>
        private static string RELATIVELOCATION_BACKUP = "\\Mod-Loader-Backup\\";

        /// <summary>
        /// Specifies the relative location of the mod loader libraries relative to the mod loader.
        /// </summary>
        private static string RELATIVELOCATION_LIBRARIES = "\\Mod-Loader-Libraries\\";

        /// <summary>
        /// Retrieves the directory of the current process where the DLL resides in. i.e. the game directory.
        /// </summary>
        public static string GetProcessDirectory()
        {
            return Environment.CurrentDirectory;
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
        /// Retrieves the mod loader's main config as an array of strings.
        /// </summary>
        public static string GetModLoaderConfig()
        {
            return File.ReadAllText(GetModLoaderDirectory() + RELATIVELOCATION_CONFIGFILE);
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
        /// Retrieves the directory where the mod DLL (main.dll) is stored.
        /// </summary>
        public static string GetModDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

    }
}
