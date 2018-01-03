using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Misc.Config
{
    /// <summary>
    /// Simple parser for the loader game configuration files.
    /// </summary>
    public class GameConfigParser
    {
        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData iniData;

        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private FileIniDataParser iniParser;

        /// <summary>
        /// Stores the location of the configuration file for the game.
        /// </summary>
        private string configDirectory;

        /// <summary>
        /// Defines a general struct for the loader game configuration file.
        /// </summary>
        public struct GameConfig
        {
            /// <summary>
            /// Specifies the directory of the game.
            /// </summary>
            public string Game_Directory;

            /// <summary>
            /// Specifies the executable directory.
            /// The executable directory is relative to the game directory.
            /// </summary>
            public string Executable_Directory;

            /// <summary>
            /// Defines the game hooking method.
            /// Instant will start the application and immediately hook to it.
            /// Delayed will hook at a delay (potential compatibility reasons).
            /// Manual will perform file replacements, and inject on user demand after manually launching the game.
            /// </summary>
            public HookMethod Hook_Method;

            /// <summary>
            /// Specifies a list of enabled mods, separated by a comma.
            /// </summary>
            public List<string> Enabled_Mods;
        }

        /// <summary>
        /// Defines the individual game hooking methods.
        /// Instant will start the application and immediately hook to it.
        /// Delayed will hook at a delay (potential compatibility reasons)
        /// Manual will perform file replacements, and inject on user demand after manually launching the game.
        /// </summary>
        public enum HookMethod
        {
            /// <summary>
            /// Instant will start the application and immediately hook to it.
            /// </summary>
            Instant,
            /// <summary>
            /// Delayed will hook at a delay (potential compatibility reasons),
            /// </summary>
            Delayed,
            /// <summary>
            /// Manual will perform file replacements, and inject on user demand after manually launching the game.
            /// </summary>
            Manual
        }

        /// <summary>
        /// Initiates the game config parser.
        /// </summary>
        public GameConfigParser()
        {
            iniParser = new FileIniDataParser();
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="gameName">The name of the game folder containing the configuration file for the game. e.g. Sonic-Heroes</param>
        /// <returns></returns>
        public GameConfig ParseConfig(string gameName)
        {
            // Instantiate a new configuration struct.
            GameConfig gameConfig = new GameConfig();

            // Configuration directory
            configDirectory = LoaderPaths.GetModLoaderConfigDirectory() + "/Games/" + gameName + "/Config.ini";

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(configDirectory);

            // Parse the mod loader configuration.
            gameConfig.Game_Directory = iniData["Game Configuration"]["Game_Directory"];
            gameConfig.Executable_Directory = iniData["Game Configuration"]["Executable_Directory"];
            gameConfig.Hook_Method = (HookMethod)Enum.Parse(typeof(HookMethod), iniData["Game Configuration"]["Hook_Method"]);
            gameConfig.Enabled_Mods = GetEnabledMods();

            // Return the config file.
            return gameConfig;
        }



        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="gameConfig"></param>
        public void WriteConfig(GameConfig gameConfig, string gameName)
        {
            // Change the values of the current fields.
            iniData["Game Configuration"]["Game_Directory"] = gameConfig.Game_Directory;
            iniData["Game Configuration"]["Executable_Directory"] = gameConfig.Executable_Directory;
            iniData["Game Configuration"]["Hook_Method"] = Enum.GetName(typeof(HookMethod), gameConfig.Hook_Method);

            // Writes the list of currently enabled mods.
            WriteEnabledMods(gameConfig);

            // Write the file out to disk
            iniParser.WriteFile(configDirectory, iniData);
        }

        /// <summary>
        /// Reads the list of mods from the Config.ini file.
        /// </summary>
        private List<string> GetEnabledMods()
        {
            // Stores the currently enabled/disabled mods.
            List<string> loadedMods = new List<string>();

            // Retrieve the config file bare contents.
            string[] configFile = File.ReadAllLines(configDirectory);

            // Iterate over bare config file.
            for (int z = 0; z < configFile.Length; z++)
            {
                // Search for enabled mods header.
                if (configFile[z].StartsWith("#")) { continue; }
                else { loadedMods.Add(configFile[z]); }
            }

            // Return the list of loaded mods.
            return loadedMods;
        }

        /// <summary>
        /// Writes the list of currently enabled mods onto a text file.
        /// </summary>
        private void WriteEnabledMods(GameConfig gameConfig)
        {
            // Insert header of the file.
            gameConfig.Enabled_Mods.Insert(0, "# This file lists the directory names of all currently enabled mods.");

            // Write file list and header.
            File.WriteAllLines(configDirectory, gameConfig.Enabled_Mods);
        }
    }
}
