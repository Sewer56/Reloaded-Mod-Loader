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
using IniParser;
using IniParser.Model;

namespace Reloaded.IO.Config.Games
{
    /// <summary>
    /// Simple parser for the loader game configuration files.
    /// </summary>
    public class GameConfigParser
    {
        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private readonly FileIniDataParser _iniParser;

        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData _iniData;

        /// <summary>
        /// Initiates the game config parser.
        /// </summary>
        public GameConfigParser()
        {
            _iniParser = new FileIniDataParser();
            _iniData = new IniData();
            _iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="gameConfigDirectory">The directory containing the configuration file for the game. e.g. $LOADERPATH\\Reloaded-Mods\\Games\\Sonic-Heroes</param>
        /// <returns></returns>
        public GameConfig ParseConfig(string gameConfigDirectory)
        {
            // Instantiate a new configuration struct.
            GameConfig gameConfig = new GameConfig();

            // Read the mod loader configuration.
            _iniData = _iniParser.ReadFile(gameConfigDirectory + "/Config.ini");

            // Parse the mod loader configuration.
            gameConfig.GameName = _iniData["Game Configuration"]["Game_Name"];
            gameConfig.GameVersion = _iniData["Game Configuration"]["Game_Version"];
            gameConfig.GameDirectory = _iniData["Game Configuration"]["Game_Directory"];
            gameConfig.ExecutableLocation = _iniData["Game Configuration"]["Executable_Directory"];
            gameConfig.ModDirectory = _iniData["Game Configuration"]["Mod_Directory"];
            gameConfig.ConfigDirectory = gameConfigDirectory;
            gameConfig.EnabledMods = GetEnabledMods(gameConfig.ConfigDirectory);

            // Return the config file.
            return gameConfig;
        }

        /// <summary>
        /// Creates a new config file from scratch.
        /// </summary>
        public void CreateNewConfig(GameConfig gameConfig)
        {
            // Create category
            _iniData.Sections.Add(new SectionData("Game Configuration"));

            // Create fields
            _iniData.Sections["Game Configuration"].AddKey("Game_Name");
            _iniData.Sections["Game Configuration"].AddKey("Game_Version");
            _iniData.Sections["Game Configuration"].AddKey("Game_Directory");
            _iniData.Sections["Game Configuration"].AddKey("Executable_Directory");
            _iniData.Sections["Game Configuration"].AddKey("Mod_Directory");

            // Write config
            WriteConfig(gameConfig);
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="gameConfig">The game configuration to be written to disk.</param>
        public void WriteConfig(GameConfig gameConfig)
        {
            // Change the values of the current fields.
            _iniData["Game Configuration"]["Game_Name"] = gameConfig.GameName;
            _iniData["Game Configuration"]["Game_Version"] = gameConfig.GameVersion;
            _iniData["Game Configuration"]["Game_Directory"] = gameConfig.GameDirectory;
            _iniData["Game Configuration"]["Executable_Directory"] = gameConfig.ExecutableLocation;
            _iniData["Game Configuration"]["Mod_Directory"] = gameConfig.ModDirectory;

            // Writes the list of currently enabled mods.
            WriteEnabledMods(gameConfig);

            // Write the file out to disk
            _iniParser.WriteFile(gameConfig.ConfigDirectory + "/Config.ini", _iniData);
        }

        /// <summary>
        /// Reads the list of mods from the Config.ini file.
        /// </summary>
        /// <param name="gameConfigDirectory">Stores the directory of the game config.</param>
        private List<string> GetEnabledMods(string gameConfigDirectory)
        {
            // Stores the currently enabled/disabled mods.
            List<string> loadedMods = new List<string>();

            // Retrieve the config file bare contents.
            string[] configFile = File.ReadAllLines(gameConfigDirectory + "/Enabled_Mods.ini");

            // Iterate over bare config file.
            foreach (string modFolder in configFile)
            {
                if (! modFolder.StartsWith("#"))
                    loadedMods.Add(modFolder);
            }

            // Return the list of loaded mods.
            return loadedMods;
        }

        /// <summary>
        /// Writes the list of currently enabled mods onto a text file.
        /// </summary>
        /// <param name="gameConfig">The game configuration structure used by Reloaded.</param>
        private void WriteEnabledMods(GameConfig gameConfig)
        {
            // Insert header of the file.
            gameConfig.EnabledMods.Insert(0, "# This file lists the directory names of all currently enabled mods.");

            // Write file list and header.
            File.WriteAllLines(gameConfig.ConfigDirectory + "/Enabled_Mods.ini", gameConfig.EnabledMods);
        }

        /// <summary>
        /// Defines a general struct for the loader game configuration file.
        /// </summary>
        public class GameConfig
        {
            /// <summary>
            /// Specifies the directory of the game.
            /// </summary>
            public string GameDirectory { get; set; }

            /// <summary>
            /// Specifies the executable location.
            /// The executable location is relative to the game directory.
            /// </summary>
            public string ExecutableLocation { get; set; }

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
            /// The path is relative to Reloaded-Mods.
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
    }
}
