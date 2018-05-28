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

using System.IO;
using Newtonsoft.Json;
using Reloaded.Paths;

namespace Reloaded.IO.Config
{
    /// <summary>
    /// Defines a general struct for the loader theme configuration file.
    /// </summary>
    public class ThemeConfig
    {
        /// <summary>
        /// The name of the theme as it appears in the mod loader configuration tool.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeName { get; set; } = "Undefined Theme Name";

        /// <summary>
        /// The description of the mod.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeDescription { get; set; } = "Undefined Theme Description";

        /// <summary>
        /// The version of the theme. (Recommended Format: 1.XX)
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeVersion { get; set; } = "Undefined";

        /// <summary>
        /// The author of the specific theme.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeAuthor { get; set; } = "Undefined";

        /// <summary>
        /// The site shown in the hyperlink on the loader for your theme.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeSite { get; set; } = "N/A";

        /// <summary>
        /// Use if you want to provide self-updates from source code..
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public string ThemeGithub { get; set; } = "N/A";

        /// <summary>
        /// [DO NOT MODIFY] Stores the physical directory location of the theme configuration for re-save purposes.
        /// </summary>
        [JsonIgnore]
        public string ThemeLocation { get; set; }

        /// <summary>
        /// Retrieves the Mod Loader theme configuration file struct.
        /// </summary>
        /// <param name="themeLocations">The absolute path to the theme file.</param>
        public static ThemeConfig ParseConfig(string themeLocations)
        {
            // Read the mod loader theme configuration.
            string themeConfigurationLocation = themeLocations + $"/{Strings.Parsers.ConfigFile}";

            // Try parsing the config file, else return default one.
            ThemeConfig themeConfiguration;

            try
            {
                themeConfiguration =
                    File.Exists(themeConfigurationLocation)
                        ? JsonConvert.DeserializeObject<ThemeConfig>(File.ReadAllText(themeConfigurationLocation))
                        : new ThemeConfig();
            }
            catch
            {
                themeConfiguration = new ThemeConfig();
            }

            themeConfiguration.ThemeLocation = themeConfigurationLocation;
            return themeConfiguration;
        }

        /// <summary>
        /// Writes out the theme config file to an .ini.
        /// </summary>
        /// <param name="themeConfig">The theme configuration struct.</param>
        public static void WriteConfig(ThemeConfig themeConfig)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(themeConfig, Formatting.Indented);

            // Write to disk
            File.WriteAllText(themeConfig.ThemeLocation, json);
        }
    }
}