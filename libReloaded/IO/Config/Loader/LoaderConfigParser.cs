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

using System;
using System.IO;
using IniParser;
using IniParser.Model;
using Reloaded.Input.Modules;
using Reloaded.Utilities;

namespace Reloaded.IO.Config.Loader
{
    /// <summary>
    /// Simple parser for the main mod loader configuration file.
    /// </summary>
    public class LoaderConfigParser
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
        /// Stores loader config ini data section.
        /// </summary>
        private KeyDataCollection _loaderConfig;

        /// <summary>
        /// Instantiates the Loader Config Parser.
        /// </summary>
        public LoaderConfigParser()
        {
            _iniParser = new FileIniDataParser();
            _iniData = new IniData();
            _iniParser.Parser.Configuration.CommentString = Strings.Parsers.CommentCharacter;
        }

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <returns></returns>
        public Config ParseConfig()
        {
            // Instantiate a new configuration struct.
            Config config = new Config();

            // Read the mod loader configuration.
            _iniData = _iniParser.ReadFile(LoaderPaths.GetModLoaderConfig());
            _loaderConfig = _iniData["Mod Loader Configuration"];

            // Parse the mod loader configuration.
            config.CurrentTheme = _loaderConfig["Current_Theme"];
            config.DirectInputConfigType = (Remapper.DirectInputConfigType)Enum.Parse(typeof(Remapper.DirectInputConfigType), _loaderConfig["Controller_Config_Type"]);
            config.ExitAfterLaunch = TryOrDefault( () => ( bool.TryParse(_loaderConfig[nameof(Config.ExitAfterLaunch)], out var res ), res ), true );

            // Return the config file.
            return config;
        }

        /// <summary>
        /// Creates a new mod loader config from scratch.
        /// </summary>
        /// <param name="configLocation">The location of the config file to be written.</param>
        public void CreateConfig(string configLocation)
        {
            // Create category
            var loaderConfigSection = new SectionData("Mod Loader Configuration");
            _iniData.Sections.Add(loaderConfigSection);
            _loaderConfig = loaderConfigSection.Keys;

            // Create fields
            _loaderConfig.AddKey("Current_Theme");
            _loaderConfig.AddKey("Controller_Config_Type");
            _loaderConfig.AddKey(nameof(Config.ExitAfterLaunch));

            // Get default theme (first alphabetical theme)
            string[] directories = Directory.GetDirectories((LoaderPaths.GetModLoaderThemeDirectory()));

            // Set default theme if exists.
            if (directories.Length > 0) { _loaderConfig["Current_Theme"] = Path.GetFileNameWithoutExtension(directories[0]); }
            else { _loaderConfig["Current_Theme"] = "!Reloaded"; }

            // Set defaults
            _loaderConfig["Controller_Config_Type"] =
                Remapper.DirectInputConfigType.ProductGUID.ToString();

            _loaderConfig[nameof(Config.ExitAfterLaunch)] = true.ToString();

            // Write file
            _iniParser.WriteFile(configLocation, _iniData);
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="config"></param>
        public void WriteConfig(Config config)
        {
            // Change the values of the current fields.
            _loaderConfig["Current_Theme"] = config.CurrentTheme;
            _loaderConfig["Controller_Config_Type"] = Enum.GetName(typeof(Remapper.DirectInputConfigType), config.DirectInputConfigType);
            _loaderConfig[nameof(Config.ExitAfterLaunch)] = config.ExitAfterLaunch.ToString();

            // Write the file out to disk
            _iniParser.WriteFile(LoaderPaths.GetModLoaderConfig(), _iniData);
        }

        private static T TryOrDefault<T>( Func<(bool, T)> expression, T defaultValue )
        {
            var result = expression();
            return !result.Item1 ? defaultValue : result.Item2;
        }

        /// <summary>
        /// Defines a general struct for the Mod Loader Configuration tile.
        /// </summary>
        public class Config
        {
            /// <summary>
            /// Specifies the preferred configuration for DirectInput devices.
            /// Specifies the current configuration type used for DirectInput devices.
            /// Controllers can be differentiated by product (identical controllers will carry identical config) or...
            /// Controllers can be differentiated by instance (each controller is unique but also tied to USB port).
            /// </summary>
            public Remapper.DirectInputConfigType DirectInputConfigType { get; set; }
            
            /// <summary>
            /// Specifies the subdirectory containing the current mod loader theme.
            /// The subdirectory is relative to Reloaded-Config/Themes/
            /// </summary>
            public string CurrentTheme { get; set; }

            /// <summary>
            /// Specifies whether or not to automatically exit the launcher after launching a game.
            /// </summary>
            public bool ExitAfterLaunch { get; set; }
        }
    }
}
