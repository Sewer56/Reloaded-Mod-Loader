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
using IniParser;
using IniParser.Model;
using static Reloaded.Input.Remapper;

namespace Reloaded.Misc.Config
{
    /// <summary>
    /// Simple parser for the main mod loader configuration file.
    /// </summary>
    public class LoaderConfigParser
    {
        /// <summary>
        /// Specifies the preferred overlay mode.
        /// External overlay draws a window above the current game/process.
        /// Internal overlay tries to hook onto DirectX process of the game.
        /// If Internal overlay fails (unimplemented API e.g. OpenGL, DX12), mods
        /// will attempt to load an external overlay.
        /// </summary>
        public enum PreferredOverlay
        {
            External,
            Internal
        }

        /// <summary>
        /// Holds an instance of ini-parser used for parsing INI files.
        /// </summary>
        private readonly FileIniDataParser iniParser;

        /// <summary>
        /// Stores the ini data read by the ini-parser.
        /// </summary>
        private IniData iniData;

        /// <summary>
        /// Instantiates the Loader Config Parser.
        /// </summary>
        public LoaderConfigParser()
        {
            iniParser = new FileIniDataParser();
            iniParser.Parser.Configuration.CommentString = "#";
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
            iniData = iniParser.ReadFile(LoaderPaths.GetModLoaderConfig());

            // Parse the mod loader configuration.
            config.CurrentTheme = iniData["Mod Loader Configuration"]["Current_Theme"];
            config.DirectInputConfigType = (DirectInputConfigType)Enum.Parse(typeof(DirectInputConfigType), iniData["Mod Loader Configuration"]["Controller_Config_Type"]);
            config.PreferredOverlay = (PreferredOverlay)Enum.Parse(typeof(PreferredOverlay), iniData["Mod Loader Configuration"]["Preferred_Overlay"]);

            // Return the config file.
            return config;
        }

        /// <summary>
        /// Writes out the config file to an .ini file.
        /// </summary>
        /// <param name="config"></param>
        public void WriteConfig(Config config)
        {
            // Change the values of the current fields.
            iniData["Mod Loader Configuration"]["Current_Theme"] = config.CurrentTheme;
            iniData["Mod Loader Configuration"]["Controller_Config_Type"] = Enum.GetName(typeof(DirectInputConfigType), config.DirectInputConfigType);
            iniData["Mod Loader Configuration"]["Preferred_Overlay"] = Enum.GetName(typeof(PreferredOverlay), config.PreferredOverlay);

            // Write the file out to disk
            iniParser.WriteFile(LoaderPaths.GetModLoaderConfig(), iniData);
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
            public DirectInputConfigType DirectInputConfigType { get; set; }

            /// <summary>
            /// Specifies the preferred overlay mode.
            /// External overlay draws a window above the current game/process.
            /// Internal overlay tries to hook onto DirectX process of the game.
            /// If Internal overlay fails (unimplemented API e.g. OpenGL, DX12), mods
            /// will attempt to load an external overlay.
            /// </summary>
            public PreferredOverlay PreferredOverlay { get; set; }

            /// <summary>
            /// Specifies the subdirectory containing the current mod loader theme.
            /// The subdirectory is relative to Reloaded-Config/Themes/
            /// </summary>
            public string CurrentTheme { get; set; }
        }
    }
}
