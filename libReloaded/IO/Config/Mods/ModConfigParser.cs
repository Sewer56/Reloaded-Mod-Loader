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

using IniParser;
using IniParser.Model;

namespace Reloaded.IO.Config.Mods
{
    /// <summary>
    /// Simple parser for the loader mod configuration file.
    /// </summary>
    public class ModConfigParser
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
        /// Initiates the mod config parser.
        /// </summary>
        public ModConfigParser()
        {
            _iniParser = new FileIniDataParser();
            _iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="modDirectory">The absolute directory of the individual mod in question.</param>
        public ModConfig ParseConfig(string modDirectory)
        {
            // Read the mod loader configuration.
            _iniData = _iniParser.ReadFile(modDirectory + "\\Config.ini");

            // Instantiate a new configuration struct.
            ModConfig modConfig = new ModConfig
            {
                ModLocation = modDirectory + "\\Config.ini",
                ModName = _iniData["Mod Configuration"]["Mod_Name"],
                ModDescription = _iniData["Mod Configuration"]["Mod_Description"],
                ModVersion = _iniData["Mod Configuration"]["Mod_Version"],
                ModAuthor = _iniData["Mod Configuration"]["Mod_Author"],
                ThemeSite = _iniData["Mod Configuration"]["Mod_Site"],
                ThemeGithub = _iniData["Mod Configuration"]["Mod_Github"],
                ModConfigExe = _iniData["Mod Configuration"]["Mod_Config"]

            };
            
            // Return the config file.
            return modConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="modConfig">The mod configuration structure defining the details of the individual mod.</param>
        public void WriteConfig(ModConfig modConfig)
        {
            // Change the values of the current fields.
            _iniData["Mod Configuration"]["Mod_Name"] = modConfig.ModName;
            _iniData["Mod Configuration"]["Mod_Description"] = modConfig.ModDescription;
            _iniData["Mod Configuration"]["Mod_Version"] = modConfig.ModVersion;
            _iniData["Mod Configuration"]["Mod_Author"] = modConfig.ModAuthor;
            _iniData["Mod Configuration"]["Mod_Site"] = modConfig.ThemeSite;
            _iniData["Mod Configuration"]["Mod_Github"] = modConfig.ThemeGithub;
            _iniData["Mod Configuration"]["Mod_Config"] = modConfig.ModConfigExe;

            // Write the file out to disk.
            _iniParser.WriteFile(modConfig.ModLocation, _iniData);
        }

        /// <summary>
        /// Defines a general struct for the loader mod configuration file.
        /// </summary>
        public class ModConfig
        {
            /// <summary>
            /// The name of the mod as it appears in the mod loader configuration tool.
            /// </summary>
            public string ModName { get; set; }

            /// <summary>
            /// The description of the mod.
            /// </summary>
            public string ModDescription { get; set; }

            /// <summary>
            /// The version of the mod. (Recommended Format: 1.XX)
            /// </summary>
            public string ModVersion { get; set; }

            /// <summary>
            /// The author of the specific mod.
            /// </summary>
            public string ModAuthor { get; set; }

            /// <summary>
            /// The site shown in the hyperlink on the loader for the mod.
            /// </summary>
            public string ThemeSite { get; set; }

            /// <summary>
            /// Used for self-updates from source code.
            /// </summary>
            public string ThemeGithub { get; set; }

            /// <summary>
            /// Specifies an executable or file in the same directory to be ran for configuration purposes.
            /// </summary>
            public string ModConfigExe { get; set; }

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the mod configuration for re-save purposes.
            /// </summary>
            public string ModLocation { get; set; }
        }
    }
}
