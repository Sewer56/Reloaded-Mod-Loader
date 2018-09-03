using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded_Plugin_System.Config;
using Reloaded_Plugin_System.Config.Loader;

namespace Reloaded_Plugin_System.Interfaces.Loader
{
    /// <summary>
    /// This interface allows you to modify the behaviour of the individual classes
    /// of the Loader configuration, such as the banner printer or DLL Injector.
    /// </summary>
    public interface ILoaderBehaviourV1
    {
        /// <summary>
        /// Controls the functions executed before Reloaded prints its banner and
        /// after the window size + theme colours are set.
        /// Simply return the parameter passed if you do not want to make any changes.
        /// </summary>
        BannerOptions SetBannerOptions(BannerOptions options);

        /// <summary>
        /// Allows you to hijack Reloaded-Loader's console visual style and printing options.
        /// Simply return the parameter passed if you do not want to make any changes.
        /// </summary>
        ConsoleOptions SetConsoleOptions(ConsoleOptions options);

        /// <summary>
        /// Allows you to modify the individual console colours that are used for various messages
        /// such as printing to the screen.
        /// </summary>
        ConsoleColours SetConsoleColours(ConsoleColours colours);

        /// <summary>
        /// Allows you to modify how the Reloaded Launcher prints controllers during the startup process.
        /// </summary>
        ControllerOptions SetControllerOptions(ControllerOptions controllerOptions);
    }
}
