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
using Reloaded.Input.Modules;
using Reloaded.Paths;

namespace Reloaded.Input
{
    /// <summary>
    /// Defines a general struct for the Mod Loader Configuration tile.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class ControllerConfig
    {
        /// <summary>
        /// Specifies the preferred configuration for DirectInput devices.
        /// Specifies the current configuration type used for DirectInput devices.
        /// Controllers can be differentiated by product (identical controllers will carry identical config) or...
        /// Controllers can be differentiated by instance (each controller is unique but also tied to USB port).
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public Remapper.DirectInputConfigType DirectInputConfigType { get; set; } = Remapper.DirectInputConfigType.ProductGUID;

        /// <summary>
        /// Retrieves the Mod Loader configuration file struct.
        /// </summary>
        /// <returns>Parses a Mod Loader configuration file.</returns>
        public static ControllerConfig ParseConfig()
        {
            // Try parsing the config file, else return default one.
            try
            {
                return File.Exists(LoaderPaths.GetModLoaderConfig())
                    ? JsonConvert.DeserializeObject<ControllerConfig>(File.ReadAllText(LoaderPaths.GetModLoaderConfig()))
                    : new ControllerConfig();
            }
            catch { return new ControllerConfig(); }
        }

        /// <summary>
        /// Writes out the config file to the config file location/
        /// </summary>
        /// <param name="config">The config file to write to disk.</param>
        public static void WriteConfig(ControllerConfig config)
        {
            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);

            // Write to disk
            File.WriteAllText(LoaderPaths.GetModLoaderConfig(), json);
        }
    }
}
