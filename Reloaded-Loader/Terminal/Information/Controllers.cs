/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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

using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Input.Modules;

namespace Reloaded_Loader.Terminal.Information
{
    /// <summary>
    /// Provides various utility methods pertaining to retrieval of controller details via the use of the mod loader
    /// </summary>
    internal static class Controllers
    {
        /// <summary>
        /// Retrieves the controller list from the mod loader library libReloaded and prints it to the screen.
        /// </summary>
        public static void PrintControllerOrder()
        {
            // Header
            ConsoleFunctions.PrintMessageWithTime("Displaying Connected Controller List", ConsoleFunctions.PrintInfoMessage);
            ConsoleFunctions.PrintMessageWithTime("Key: [Controller Port] [Controller Type] <Controller Name> (Disconnected?)", ConsoleFunctions.PrintInfoMessage);

            // Retrieve Controllers
            ControllerManager controllerManager = new ControllerManager();

            // Print list of controllers.
            foreach (ControllerCommon.IController controller in controllerManager.Controllers)
            {
                // Is controller XInput or DInput
                string controllerName = "[" + controller.InputMappings.ControllerId.ToString("00") + "]";

                // Get Controller Type
                if (controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput)
                    controllerName += " [XInput] ";
                else if (controller.Remapper.DeviceType == Remapper.InputDeviceType.DirectInput)
                    controllerName += " [DInput] ";

                // Add controller name from remapper.
                controllerName += controller.Remapper.GetControllerName;

                // Check if disconnected.
                if (! controller.IsConnected()) { controllerName += " (Disconnected)"; }

                // Print controller to screen.
                ConsoleFunctions.PrintMessageWithTime(controllerName, ConsoleFunctions.PrintMessage);
            }


        }
    }
}
