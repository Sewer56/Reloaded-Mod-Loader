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

namespace ReloadedLauncher.Utilities.Windows
{
    /// <summary>
    /// Class which allows for the suspending and resuming of drawing of windows forms and controls.
    /// </summary>
    public static class DrawingControl
    {
        /// <summary>
        /// Sends the specified message to a window or windows. 
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
        /// <param name="msg">The message to be sent. For lists of the system-provided messages, see System-Defined Messages on MSDN.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window to allow changes 
        /// in that window to be redrawn or to prevent changes in that window from being redrawn.
        /// </summary>
        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Suspends all draw operations for the specific control.
        /// </summary>
        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, 0, 0);
        }

        /// <summary>
        /// Resumes all draw operations for the specific control.
        /// </summary>
        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, 1, 0);
            parent.Refresh();
        }

        /// <summary>
        /// Suspends all draw operations for the specific control.
        /// </summary>
        public static void SuspendDrawing(Form parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, 0, 0);
        }

        /// <summary>
        /// Resumes all draw operations for the specific control.
        /// </summary>
        public static void ResumeDrawing(Form parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, 1, 0);
            parent.Refresh();
        }
    }
}
