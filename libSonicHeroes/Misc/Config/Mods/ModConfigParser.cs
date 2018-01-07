using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public struct ModConfig
        {
            /// <summary>
            /// The name of the mod as it appears in the mod loader configuration tool.
            /// </summary>
            public string ModName;

            /// <summary>
            /// The description of the mod.
            /// </summary>
            public string ModDescription;

            /// <summary>
            /// The version of the mod. (Recommended Format: 1.XX)
            /// </summary>
            public string ModVersion;

            /// <summary>
            /// The author of the specific mod.
            /// </summary>
            public string ModAuthor;

            /// <summary>
            /// The site shown in the hyperlink on the loader for the mod.
            /// </summary>
            public string ModSite;

            /// <summary>
            /// Used for self-updates from source code.
            /// </summary>
            public string ModGithub;

            /// <summary>
            /// Specifies an executable or file in the same directory to be ran for configuration purposes.
            /// </summary>
            public string ModConfigEXE;
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
        /// <param name="modDirectory">The relative directory of the individual mod to Mod-Loader-Mods. e.g. Sonic-Heroes/Vanilla-Tweakbox-II</param>
        public ModConfig ParseConfig(string modDirectory)
        {
            // Instantiate a new configuration struct.
            ModConfig modConfig = new ModConfig();

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(LoaderPaths.GetModLoaderConfigDirectory() + "/" + modDirectory + "/Config.ini");

            // Parse the mod loader configuration.
            modConfig.ModName = iniData["Mod Configuration"]["Mod_Name"];
            modConfig.ModDescription = iniData["Mod Configuration"]["Mod_Description"];
            modConfig.ModVersion = iniData["Mod Configuration"]["Mod_Version"];
            modConfig.ModAuthor = iniData["Mod Configuration"]["Mod_Author"];
            modConfig.ModSite = iniData["Mod Configuration"]["Mod_Site"];
            modConfig.ModGithub = iniData["Mod Configuration"]["Mod_Gi  thub"];
            modConfig.ModConfigEXE = iniData["Mod Configuration"]["Mod_Config"];

            // Return the config file.
            return modConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="modDirectory">The relative directory of the individual mod to Mod-Loader-Mods. e.g. Sonic-Heroes/Vanilla-Tweakbox-II</param>
        public void WriteConfig(ModConfig modConfig, string modDirectory)
        {
            // Change the values of the current fields.
            iniData["Mod Configuration"]["Mod_Name"] = modConfig.ModName;
            iniData["Mod Configuration"]["Mod_Description"] = modConfig.ModDescription;
            iniData["Mod Configuration"]["Mod_Version"] = modConfig.ModVersion;
            iniData["Mod Configuration"]["Mod_Author"] = modConfig.ModAuthor;
            iniData["Mod Configuration"]["Mod_Site"] = modConfig.ModSite;
            iniData["Mod Configuration"]["Mod_Github"] = modConfig.ModGithub;
            iniData["Mod Configuration"]["Mod_Config"] = modConfig.ModConfigEXE;

            // Write the file out to disk.
            iniParser.WriteFile(LoaderPaths.GetModLoaderConfigDirectory() + "/" + modDirectory + "/Config.ini", iniData);
        }
    }
}
