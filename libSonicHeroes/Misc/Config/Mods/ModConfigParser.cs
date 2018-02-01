using IniParser;
using IniParser.Model;

namespace SonicHeroes.Misc.Config
{
    /// <summary>
    /// Simple parser for the loader mod configuration file.
    /// </summary>
    public class ModConfigParser
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
            public string ModConfigEXE { get; set; }

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the mod configuration for re-save purposes.
            /// </summary>
            public string ModLocation { get; set; }
        }

        /// <summary>
        /// Initiates the mod config parser.
        /// </summary>
        public ModConfigParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="modDirectory">The absolute directory of the individual mod in question.</param>
        public ModConfig ParseConfig(string modDirectory)
        {
            // Instantiate a new configuration struct.
            ModConfig modConfig = new ModConfig();

            // Set the mod directory.
            modConfig.ModLocation = modDirectory + "\\Config.ini";

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(modConfig.ModLocation);

            // Parse the mod loader configuration.
            modConfig.ModName = iniData["Mod Configuration"]["Mod_Name"];
            modConfig.ModDescription = iniData["Mod Configuration"]["Mod_Description"];
            modConfig.ModVersion = iniData["Mod Configuration"]["Mod_Version"];
            modConfig.ModAuthor = iniData["Mod Configuration"]["Mod_Author"];
            modConfig.ThemeSite = iniData["Mod Configuration"]["Mod_Site"];
            modConfig.ThemeGithub = iniData["Mod Configuration"]["Mod_Github"];
            modConfig.ModConfigEXE = iniData["Mod Configuration"]["Mod_Config"];

            // Return the config file.
            return modConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="modDirectory">The relative directory of the individual mod to Mod-Loader-Mods. e.g. Sonic-Heroes/Vanilla-Tweakbox-II</param>
        public void WriteConfig(ModConfig modConfig)
        {
            // Change the values of the current fields.
            iniData["Mod Configuration"]["Mod_Name"] = modConfig.ModName;
            iniData["Mod Configuration"]["Mod_Description"] = modConfig.ModDescription;
            iniData["Mod Configuration"]["Mod_Version"] = modConfig.ModVersion;
            iniData["Mod Configuration"]["Mod_Author"] = modConfig.ModAuthor;
            iniData["Mod Configuration"]["Mod_Site"] = modConfig.ThemeSite;
            iniData["Mod Configuration"]["Mod_Github"] = modConfig.ThemeGithub;
            iniData["Mod Configuration"]["Mod_Config"] = modConfig.ModConfigEXE;

            // Write the file out to disk.
            iniParser.WriteFile(modConfig.ModLocation, iniData);
        }
    }
}
