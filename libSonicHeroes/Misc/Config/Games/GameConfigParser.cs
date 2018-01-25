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
        /// Defines a general struct for the loader game configuration file.
        /// </summary>
        public struct GameConfig
        {
            /// <summary>
            /// Specifies the directory of the game.
            /// </summary>
            public string GameDirectory { get; set; }

            /// <summary>
            /// Specifies the executable directory.
            /// The executable directory is relative to the game directory.
            /// </summary>
            public string ExecutableDirectory { get; set; }

            /// <summary>
            /// Defines the game hooking method.
            /// Instant will start the application and immediately hook to it.
            /// Delayed will hook at a delay (potential compatibility reasons).
            /// Manual will perform file replacements, and inject on user demand after manually launching the game.
            /// </summary>
            public HookMethod HookMethod { get; set; }

            /// <summary>
            /// Specifies the name of the game, as displayed by 
            /// the mod loader configuration utility. 
            /// </summary>
            public string GameName { get; set; }

            /// <summary>
            /// Specifies the version of the game, as displayed
            /// by the mod loader configuration utility.
            /// </summary>
            public string GameVersion { get; set; }

            /// <summary>
            /// Specifies the directory where mods for this game are stored.
            /// The path is relative to Mod-Loader-Mods.
            /// </summary>
            public string ModDirectory { get; set; }

            /// <summary>
            /// Specifies a list of enabled mods, separated by a comma.
            /// </summary>
            public List<string> EnabledMods { get; set; }

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the game configuration for re-save purposes.
            /// </summary>
            public string ConfigDirectory { get; set; }
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
            iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="gameConfigDirectory">The directory containing the configuration file for the game. e.g. $LOADERPATH\\Mod-Loader-Mods\\Games\\Sonic-Heroes</param>
        /// <returns></returns>
        public GameConfig ParseConfig(string gameConfigDirectory)
        {
            // Instantiate a new configuration struct.
            GameConfig gameConfig = new GameConfig();

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(gameConfigDirectory + "/Config.ini");

            // Parse the mod loader configuration.
            gameConfig.GameName = iniData["Game Configuration"]["Game_Name"];
            gameConfig.GameVersion = iniData["Game Configuration"]["Game_Version"];
            gameConfig.GameDirectory = iniData["Game Configuration"]["Game_Directory"];
            gameConfig.ExecutableDirectory = iniData["Game Configuration"]["Executable_Directory"];
            gameConfig.ModDirectory = iniData["Game Configuration"]["Mod_Directory"];
            gameConfig.HookMethod = (HookMethod)Enum.Parse(typeof(HookMethod), iniData["Game Configuration"]["Hook_Method"]);
            gameConfig.ConfigDirectory = gameConfigDirectory;
            gameConfig.EnabledMods = GetEnabledMods(gameConfig.ConfigDirectory);

            // Return the config file.
            return gameConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="gameConfig">The game configuration to be written to disk.</param>
        public void WriteConfig(GameConfig gameConfig)
        {
            // Change the values of the current fields.
            iniData["Game Configuration"]["Game_Name"] = gameConfig.GameName;
            iniData["Game Configuration"]["Game_Version"] = gameConfig.GameVersion;
            iniData["Game Configuration"]["Game_Directory"] = gameConfig.GameDirectory;
            iniData["Game Configuration"]["Executable_Directory"] = gameConfig.ExecutableDirectory;
            iniData["Game Configuration"]["Mod_Directory"] = gameConfig.ModDirectory;
            iniData["Game Configuration"]["Hook_Method"] = Enum.GetName(typeof(HookMethod), gameConfig.HookMethod);

            // Writes the list of currently enabled mods.
            WriteEnabledMods(gameConfig, gameConfig.ConfigDirectory);

            // Write the file out to disk
            iniParser.WriteFile(gameConfig.ConfigDirectory + "/Config.ini", iniData);
        }

        /// <summary>
        /// Reads the list of mods from the Config.ini file.
        /// </summary>
        /// <param name="gameDirectory">Stores the directory of the game config.</param>
        private List<string> GetEnabledMods(string gameConfigDirectory)
        {
            // Stores the currently enabled/disabled mods.
            List<string> loadedMods = new List<string>();

            // Retrieve the config file bare contents.
            string[] configFile = File.ReadAllLines(gameConfigDirectory + "/Enabled_Mods.ini");

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
        /// <param name="gameDirectory">Stores the directory of the game config.</param>
        private void WriteEnabledMods(GameConfig gameConfig, string gameConfigDirectory)
        {
            // Insert header of the file.
            gameConfig.EnabledMods.Insert(0, "# This file lists the directory names of all currently enabled mods.");

            // Write file list and header.
            File.WriteAllLines(gameConfigDirectory + "/Enabled_Mods.ini", gameConfig.EnabledMods);
        }
    }
}
