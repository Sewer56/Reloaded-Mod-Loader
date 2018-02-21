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

namespace Reloaded.Misc.Config
{
    /// <summary>
    /// Provides a quick and easy parser for the .ini files used to define theme configurations.
    /// </summary>
    public class ThemeConfigParser
    {
        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private readonly FileIniDataParser iniParser;

        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData iniData;

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
        /// <param name="themeLocations">The absolute path to the theme file.</param>
        public ThemeConfig ParseConfig(string themeLocations)
        {
            // Instantiate a new configuration struct.
            ThemeConfig themeConfig = new ThemeConfig();

            // Set theme location.
            themeConfig.ThemeLocation = themeLocations + "\\Config.ini";

            // Read the mod loader configuration.
            iniData = iniParser.ReadFile(themeConfig.ThemeLocation);

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
        /// <param name="themeConfig">The theme configuration struct.</param>
        public void WriteConfig(ThemeConfig themeConfig)
        {
            // Change the values of the current fields.
            iniData["Theme Configuration"]["Theme_Name"] = themeConfig.ThemeName;
            iniData["Theme Configuration"]["Theme_Description"] = themeConfig.ThemeDescription;
            iniData["Theme Configuration"]["Theme_Version"] = themeConfig.ThemeVersion;
            iniData["Theme Configuration"]["Theme_Author"] = themeConfig.ThemeAuthor;
            iniData["Theme Configuration"]["Theme_Site"] = themeConfig.ThemeSite;
            iniData["Theme Configuration"]["Theme_Github"] = themeConfig.ThemeGithub;

            // Write the file out to disk.
            iniParser.WriteFile(themeConfig.ThemeLocation, iniData);
        }

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

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the theme configuration for re-save purposes.
            /// </summary>
            public string ThemeLocation;
        }
    }
}
