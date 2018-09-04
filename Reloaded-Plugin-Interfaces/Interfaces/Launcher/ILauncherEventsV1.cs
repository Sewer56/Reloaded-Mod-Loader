using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.IO.Config.Interfaces;

namespace Reloaded_Plugin_System.Interfaces.Launcher
{
    /// <summary>
    /// The current interface allows you to execute your own code as Reloaded's Launcher
    /// performs certain miscellaneous tasks.
    /// </summary>
    public interface ILauncherEventsV1
    {
        /// <summary>
        /// Allows the plugin implementation to receive a list of mod configurations serialized as JSON
        /// which it can parse and validate/change individual mod entries.
        ///
        /// Tip: Use ModConfig.IsValidConfig() as a hint on whether the individual is valid.
        /// Tip: Utilities.LauncherHelper has classes for deserializing the input parameter and re-serializing the output.
        /// </summary>
        /// <param name="modConfigurations">Contains an array (as in JSON array) of all of the mod configurations.</param>
        /// <returns>Same as parameter. A (JSON) array of serialized mod configurations which the old configurations will be replaced by.</returns>
        List<IModConfigV1> ResolveModConfigurations(List<IModConfigV1> modConfigurations);

        /// <summary>
        /// Allows the plugin implementation to receive a list of game configurations serialized as JSON
        /// which it can parse and validate/change individual game entries.
        /// </summary>
        /// <param name="gameConfigurations">Contains an array (as in JSON array) of all of the game configurations.</param>
        /// <returns>Same as parameter. A (JSON) array of serialized game configurations which the old game configurations will be replaced by.</returns>
        List<IGameConfigV1> ResolveGameConfigurations(List<IGameConfigV1> gameConfigurations);

        /// <summary>
        /// Allows you to modify the parameters sent into Reloaded-Loader.
        /// </summary>
        /// <param name="loaderParameters">List of parameters to be passed onto the loader.</param>
        /// <returns>New list of parameters to be passed onto the loader.</returns>
        List<string> SetLoaderParameters(List<string> loaderParameters);

        /// <summary>
        /// This is executed on launch before any launcher logic is performed.
        /// </summary>
        void OnLaunch();
    }
}
