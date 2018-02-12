using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// Defines a dialog, small openable window that allows the user to input some arbitrary 
    /// information to be passed onto the main form. Implements methods to show itself, 
    /// return relevant values back to the main form and self destruct.
    /// </summary>
    interface IDialog
    {
        /// <summary>
        /// Spawns the dialog window and allows the user to enter the specified
        /// requested arbitrary set of fields for the dialog. Returns the results
        /// of the dialog in question.
        /// </summary>
        /// <returns>Custom per-window specified struct or return type.</returns>
        object GetValue();

        /// <summary>
        /// Defines the title to be shown on the top of the dialog window.
        /// </summary>
        /// <param name="title">The title to be shown.</param>
        void SetTitle(string title);
    }
}
