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

using System.Collections.Generic;
using Reloaded;
using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Input.Modules;
using Reloaded_Plugin_System;
using Reloaded_Plugin_System.Config.Loader;

namespace Reloaded_Loader.Terminal.Information
{
    /// <summary>
    /// Provides various utility methods pertaining to retrieval of controller details via the use of the mod loader
    /// </summary>
    internal static class Controllers
    {
        public static ControllerOptions ControllerOptions;

        static Controllers()
        {
            ControllerOptions = new ControllerOptions();
            ControllerOptions.PrintHeader = PrintHeaderDefault;
            ControllerOptions.PrintControllerFunction = PrintControllerDefault;

            foreach (var configPlugin in PluginLoader.LoaderConfigPlugins)
                ControllerOptions = configPlugin.SetControllerOptions(ControllerOptions);
        }

        /// <summary>
        /// Retrieves the controller list from the mod loader library libReloaded and prints it to the screen.
        /// </summary>
        public static void PrintControllerOrder()
        {
            ControllerOptions.PrintHeader();
            List<IController> controllers = GetControllersDefault();

            // Print list of controllers.
            foreach (IController controller in controllers)
            {
                string controllerId = controller.InputMappings.ControllerId.ToString("00");
                string controllerAPI = controller.Remapper.DeviceType == Remapper.InputDeviceType.XInput ? "XInput" : "DInput";
                string controllerName = controller.Remapper.GetControllerName;
                bool isConnected = controller.IsConnected();

                ControllerOptions.PrintControllerFunction(controllerId, controllerAPI, controllerName, isConnected);
            }

            ControllerOptions.PostCleanController?.Invoke();
        }

        /*
            -----------------------------------
            No Plugins: Default Method Handlers
            -----------------------------------
        */

        private static void PrintHeaderDefault()
        {
            Bindings.PrintInfo("Displaying Connected Controller List");
            Bindings.PrintInfo("Key: [Controller Port] [Controller Type] <Controller Name> (Disconnected?)");
            Bindings.PrintInfo("If this freezes, you're probably running Win 10 & are experiencing a DirectInput bug.");
            Bindings.PrintInfo("The only way to get around that is to restart your PC; sorry :/");
        }

        private static List<IController> GetControllersDefault()
        {
            ControllerManager controllerManager = new ControllerManager();
            return controllerManager.Controllers;
        }

        private static void PrintControllerDefault(string controllerId, string controllerAPI, string controllerName, bool isConnected)
        {
            string connectedText = isConnected ? "" : "(Disconnected)";
            Bindings.PrintText($"[{controllerId}] [{controllerAPI}] {controllerName} {connectedText}");
        }
    }
}
