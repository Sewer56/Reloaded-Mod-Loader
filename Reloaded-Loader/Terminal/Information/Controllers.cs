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
            // Retrieve Controllers
            ControllerManager controllerManager = new ControllerManager();

            // Get list of controllers.
            //controllerManager.Controllers[0].Remapper.
        }
    }
}
