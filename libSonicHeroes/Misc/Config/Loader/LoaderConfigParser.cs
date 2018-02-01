using IniParser;
using IniParser.Model;
using System;
using static SonicHeroes.Input.Remapper;

namespace SonicHeroes.Misc.Config
{
    /// <summary>
    /// Simple parser for the main mod loader configuration file.
    /// </summary>
    public class LoaderConfigParser
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
            /// The subdirectory is relative to Mod-Loader-Config/Themes/
            /// </summary>
            public string CurrentTheme { get; set; }
        }

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
    }
}
