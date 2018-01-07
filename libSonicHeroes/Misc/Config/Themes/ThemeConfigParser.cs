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
            public string ThemeName;

            /// <summary>
            /// The description of the mod.
            /// </summary>
            public string ThemeDescription;

            /// <summary>
            /// The version of the theme. (Recommended Format: 1.XX)
            /// </summary>
            public string ThemeVersion;

            /// <summary>
            /// The author of the specific theme.
            /// </summary>
            public string ThemeAuthor;

            /// <summary>
            /// The site shown in the hyperlink on the loader for your theme.
            /// </summary>
            public string ThemeSite;

            /// <summary>
            /// Use if you want to provide self-updates from source code..
            /// </summary>
            public string ThemeGithub;
        }

        /// <summary>
        /// Initiates the Theme Config Parser.
        /// </summary>
        public ThemeConfigParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
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
            themeConfig.ThemeName = iniData["Theme Configuration"]["Theme_Name"];
            themeConfig.ThemeDescription = iniData["Theme Configuration"]["Theme_Description"];
            themeConfig.ThemeVersion = iniData["Theme Configuration"]["Theme_Version"];
            themeConfig.ThemeAuthor = iniData["Theme Configuration"]["Theme_Author"];
            themeConfig.ThemeSite = iniData["Theme Configuration"]["Theme_Site"];
            themeConfig.ThemeGithub = iniData["Theme Configuration"]["Theme_Github"];

            // Return the config file.
            return themeConfig;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="themeDirectory">The relative directory of the individual mod to Mod-Loader-Mods. e.g. Sonic-Heroes/Vanilla-Tweakbox-II</param>
        /// <param name="modConfig">The mod configuration struct.</param>
        public void WriteConfig(ThemeConfig modConfig, string themeDirectory)
        {
            // Change the values of the current fields.
            iniData["Theme Configuration"]["Theme_Name"] = modConfig.ThemeName;
            iniData["Theme Configuration"]["Theme_Description"] = modConfig.ThemeDescription;
            iniData["Theme Configuration"]["Theme_Version"] = modConfig.ThemeVersion;
            iniData["Theme Configuration"]["Theme_Author"] = modConfig.ThemeAuthor;
            iniData["Theme Configuration"]["Theme_Site"] = modConfig.ThemeSite;
            iniData["Theme Configuration"]["Theme_Github"] = modConfig.ThemeGithub;

            // Write the file out to disk.
            iniParser.WriteFile(LoaderPaths.GetModLoaderConfigDirectory() + "/Themes/" + themeDirectory + "/Config.ini", iniData);
        }
    }
}
