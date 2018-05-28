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
using Reloaded.Input.Common;

namespace Reloaded.Input.Config
{
    /// <summary>
    /// Parses configuration files for XInput, DirectInput controllers.
    /// </summary>
    public static class ControllerConfigParser
    {
        /// <summary>
        /// Parses a Mod Loader Controller Configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public static ControllerCommon.IController ParseConfig(string configLocation, ControllerCommon.IController controller)
        {
            // Store input mappings before parsing
            ControllerCommon.InputMappings inputMappings;

            // Try parsing input mappings.
            try
            {
                inputMappings =
                    File.Exists(configLocation)
                        ? JsonConvert.DeserializeObject<ControllerCommon.InputMappings>(File.ReadAllText(configLocation))
                        : new ControllerCommon.InputMappings();
            }
            catch { inputMappings = new ControllerCommon.InputMappings(); }

            // Reassign input mappings.
            controller.InputMappings = inputMappings;

            // Return controller.
            return controller;
        }

        /// <summary>
        /// Writes a mod loader controller configuration.
        /// </summary>
        /// <param name="configLocation">States the location of the controller configuration.</param>
        /// <param name="controller">A DirectInput or XInput controller instance.</param>
        public static void WriteConfig(string configLocation, ControllerCommon.IController controller)
        {
            // Create path for config if not exists.
            if (!Directory.Exists(Path.GetDirectoryName(configLocation)))
            { Directory.CreateDirectory(Path.GetDirectoryName(configLocation)); }

            // Convert structure to JSON
            string json = JsonConvert.SerializeObject(controller.InputMappings, Formatting.Indented);

            // Write to disk
            File.WriteAllText(configLocation, json);

        }
    }
}
