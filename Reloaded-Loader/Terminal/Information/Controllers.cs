using System;
using Reloaded.Input;

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
                string controllerName = "[" + controller.ControllerID.ToString("00") + "]";

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
