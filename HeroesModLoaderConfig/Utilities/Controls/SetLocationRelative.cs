using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Utilities.Controls
{
    /// <summary>
    /// Sets the location of control X be relative to the location of control Y.
    /// </summary>
    public static class SetLocationRelative
    {
        /// <summary>
        /// Sets the location of the child to be relative to that of the parent.
        /// </summary>
        /// <param name="parentControl">The control to which the child's position will be relative to.</param>
        /// <param name="childControl">The child control, e.g. label within a textbox.</param>
        public static void SetRelativeLocation(Control parentControl, Control childControl)
        {
            // Set Location of Control Relative to Box
            Point childLocation = childControl.Location;
            childLocation.X -= parentControl.Location.X;
            childLocation.Y -= parentControl.Location.Y;

            // Set the child location.
            childControl.Location = childLocation;
        }
    }
}
