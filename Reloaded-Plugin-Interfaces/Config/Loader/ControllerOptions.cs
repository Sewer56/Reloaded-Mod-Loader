using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded_Plugin_System.Config.Loader
{
    /// <summary>
    /// Provides you with control of the controller acquiring and printing functionalitity.
    /// </summary>
    [Obsolete("Reloaded's Loader no longer prints controllers on startup.")]
    public class ControllerOptions
    {
        public delegate void PrintController(string controllerId, string controllerAPI, string controllerName, bool isConnected);

        /// <summary>
        /// Used to print the information regarding controllers before the actual acquisition of controllers and
        /// </summary>
        public Action PrintHeader;

        /// <summary>
        /// Used to print the details of an individual controller onto a new line of the screen.
        /// </summary>
        public PrintController PrintControllerFunction;

        /// <summary>
        /// Executed after the printing of all controllers ends.
        /// </summary>
        public Action PostCleanController;
    }
}
