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
    /// Provides a quick and easy parser for the .ini files used to define theme configurations.
    /// </summary>
    class ThemeConfigParser
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
        /// Defines a general struct for the loader theme configuration file.
        /// </summary>
        public struct ThemeConfig
        {
            /// <summary>
            /// The name of the theme as it appears in the mod loader configuration tool.
            /// </summary>
            public string Theme_Name;

            /// <summary>
            /// The description of the mod.
            /// </summary>
            public string Theme_Description;

            /// <summary>
            /// The version of the theme. (Recommended Format: 1.XX)
            /// </summary>
            public string Theme_Version;

            /// <summary>
            /// The author of the specific theme.
            /// </summary>
            public string Theme_Author;

            /// <summary>
            /// The site shown in the hyperlink on the loader for your theme.
            /// </summary>
            public string Theme_Site;

            /// <summary>
            /// Use if you want to provide self-updates from source code..
            /// </summary>
            public string Theme_Github;
        }

        /// <summary>
        /// Initiates the Theme Config Parser.
        /// </summary>
        public ThemeConfigParser()
        {
            iniParser = new FileIniDataParser();
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual theme to Mod-Loader-Config/Themes. e.g. Default</param>
        public ThemeConfig ParseConfig(string themeDirectory)
        {
            // Instantiate a new configuration struct.
            ThemeConfig themeConfig = new ThemeConfig();

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(LoaderPaths.GetModLoaderConfigDirectory() + "/Themes/" + themeDirectory + "/Config.ini");

            // Parse the mod loader configuration.
            themeConfig.Theme_Name = iniData["Theme Configuration"]["Theme_Name"];
            themeConfig.Theme_Description = iniData["Theme Configuration"]["Theme_Description"];
            themeConfig.Theme_Version = iniData["Theme Configuration"]["Theme_Version"];
            themeConfig.Theme_Author = iniData["Theme Configuration"]["Theme_Author"];
            themeConfig.Theme_Site = iniData["Theme Configuration"]["Theme_Site"];
            themeConfig.Theme_Github = iniData["Theme Configuration"]["Theme_Github"];

            // Return the config file.
            return themeConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual mod to Mod-Loader-Mods. e.g. Sonic-Heroes/Vanilla-Tweakbox-II</param>
        public void WriteConfig(ThemeConfig modConfig, string themeDirectory)
        {
            // Change the values of the current fields.
            iniData["Theme Configuration"]["Theme_Name"] = modConfig.Theme_Name;
            iniData["Theme Configuration"]["Theme_Description"] = modConfig.Theme_Description;
            iniData["Theme Configuration"]["Theme_Version"] = modConfig.Theme_Version;
            iniData["Theme Configuration"]["Theme_Author"] = modConfig.Theme_Author;
            iniData["Theme Configuration"]["Theme_Site"] = modConfig.Theme_Site;
            iniData["Theme Configuration"]["Theme_Github"] = modConfig.Theme_Github;

            // Write the file out to disk.
            iniParser.WriteFile(LoaderPaths.GetModLoaderConfigDirectory() + "/Themes/" + themeDirectory + "/Config.ini", iniData);
        }
    }
}
