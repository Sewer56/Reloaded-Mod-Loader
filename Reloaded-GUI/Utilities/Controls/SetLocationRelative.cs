/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
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

using System.Drawing;
using System.Windows.Forms;

namespace Reloaded_GUI.Utilities.Controls
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
