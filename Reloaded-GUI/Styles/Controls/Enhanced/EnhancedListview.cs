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
using Reloaded.Native.WinAPI;

namespace Reloaded_GUI.Styles.Controls.Enhanced
{
    /// <summary>
    /// Provides a customized implementation of a listview that removes the vertical and horizontal scrollbars from the 
    /// listview control itself. 
    /// </summary>
    public class EnhancedListview : ListView
    {
        public EnhancedListview()
        {
            Scrollable = false;
        }

        /// <summary>
        /// Overrides the WndProc function that handles messages sent to the ListView
        /// control in question. It removes the scrollbars from the listview when the listview
        /// in the case that the control may want to place them.
        /// We accomodate scrolling ourselves by using the arrow keys and scroll wheel.
        /// </summary>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                // WM_NCCALCSIZE | Message that calculates the size of the window.
                case 0x83:
                    // Obtain Initial Style
                    long windowStyle = (long)WindowStyles.GetWindowLongPtr(Handle, Constants.GWL_STYLE); 

                    // If the initial style for the Window contains the vertical scrollbar, remove it from the window style.
                    if ((windowStyle & Constants.WS_HSCROLL) == Constants.WS_HSCROLL) windowStyle = windowStyle & ~Constants.WS_HSCROLL;

                    // Repeat for horizontal scrollbar if it is contained in the window style.
                    if ((windowStyle & Constants.WS_VSCROLL) == Constants.WS_VSCROLL) windowStyle = windowStyle & ~Constants.WS_VSCROLL;

                    // Write the initial window style.
                    WindowStyles.SetWindowLongPtr(new HandleRef(this, Handle), Constants.GWL_STYLE, (IntPtr)windowStyle);

                    // Send the message to the base function, for potential painting purposes.
                    base.WndProc(ref message);
                    break;

                // Ignore WM_STYLECHANGED (No Permission to write, this is a Win32 wrapped control)
                case 0x7D:
                    break;

                // Ignore WM_REFLECT | WM_NOTIFY
                case 0x204e:
                    break;

                // No modification.
                default:
                    base.WndProc(ref message); 
                    break;
            }
        }
    }
}
