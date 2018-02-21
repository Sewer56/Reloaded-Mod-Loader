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

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Reloaded.Native;

namespace ReloadedLauncher.Utilities.Windows
{
    public static class RemoveMDIChildBorder
    {
        /// <summary>
        /// Removes all of the sunken 3D borders from the client MDI Children
        /// of an MDI Parent form.
        /// </summary>
        /// <param name="winForm">The form whose MDI Children are to have their sunken borders removed.</param>
        /// <param name="showBorders">Set to true if the sunken 3D borders are to be shown, else false.</param>
        public static void SetBevel(this Form winForm, bool showBorders)
        {
            // For each control within the form (MDI Children are also regarded as controls)
            foreach (Control control in winForm.Controls)
            {
                // Try to cast the control as an MDI Child Form
                MdiClient mdiCLientForm = control as MdiClient;

                // If it succeeds, remove the bezel.
                if (mdiCLientForm != null)
                {
                    // Get the window properties.
                    long windowLong = (long)WinAPI.WindowStyles.GetWindowLongPtr(control.Handle, WinAPI.WindowStyles.Constants.GWL_EXSTYLE);

                    // Remove (or append) the border flags.
                    if (showBorders)
                        windowLong |= WinAPI.WindowStyles.Constants.WS_EX_CLIENTEDGE;
                    else
                        windowLong &= ~WinAPI.WindowStyles.Constants.WS_EX_CLIENTEDGE;

                    // Set the new extended window flags.
                    WinAPI.WindowStyles.SetWindowLongPtr(new HandleRef(control, control.Handle), WinAPI.WindowStyles.Constants.GWL_EXSTYLE, (IntPtr)windowLong);
                }
            }
        }
    }
}
