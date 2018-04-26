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

namespace Reloaded.IO.Config.Mods
{
    /// <summary>
    /// Simple parser for the loader mod configuration file.
    /// </summary>
    public static class ModConfigParser
    {
        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <param name="modDirectory">The absolute directory of the individual mod in question.</param>
        public static ModConfig ParseConfig(string modDirectory)
        {
            // Read the mod loader configuration.
            string modConfigurationLocation = modDirectory + $"/{Strings.Parsers.ConfigFile}";

            // Try parsing the config file, else return default one.
            ModConfig modConfiguration;

            try
            {
                modConfiguration =
                    File.Exists(modConfigurationLocation)
                        ? JsonConvert.DeserializeObject<ModConfig>(File.ReadAllText(modConfigurationLocation))
                        : new ModConfig();
            }
            catch { modConfiguration = new ModConfig(); }
            
            modConfiguration.ModLocation = modConfigurationLocation;
            return modConfiguration;
        }

        /// <summary>
        /// Writes out the config file to disk.
        /// </summary>
        /// <param name="modConfig">The mod configuration structure defining the details of the individual mod.</param>
        public static void WriteConfig(ModConfig modConfig)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(modConfig, Strings.Parsers.SerializerSettings);

            // Write to disk
            File.WriteAllText(modConfig.ModLocation, json);
        }

        /// <summary>
        /// Defines a general struct for the loader mod configuration file.
        /// </summary>
        public class ModConfig
        {
            /// <summary>
            /// The name of the mod as it appears in the mod loader configuration tool.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModName { get; set; } = "Modification Name Undefined";

            /// <summary>
            /// The description of the mod.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModDescription { get; set; } = "Modification Description Undefined";

            /// <summary>
            /// The version of the mod. (Recommended Format: 1.XX)
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModVersion { get; set; } = "Undefined";

            /// <summary>
            /// The author of the specific mod.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModAuthor { get; set; } = "Undefined";

            /// <summary>
            /// The site shown in the hyperlink on the loader for the mod.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ThemeSite { get; set; } = "N/A";

            /// <summary>
            /// Used for self-updates from source code.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ThemeGithub { get; set; } = "N/A";

            /// <summary>
            /// Specifies an executable or file in the same directory to be ran for configuration purposes.
            /// </summary>
            [JsonProperty(Required = Required.Default)]
            public string ModConfigExe { get; set; } = "N/A";

            /// <summary>
            /// [DO NOT MODIFY] Stores the physical directory location of the mod configuration for re-save purposes.
            /// </summary>
            [JsonIgnore]
            public string ModLocation { get; set; }
        }
    }
}
