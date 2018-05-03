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

namespace Reloaded_Mod_Template
{
    /// <summary>
    /// The MoveWindow class allows for the window in which a control is held to be moved.
    /// To use this class, call <see cref="MoveTheWindow"/> in an MouseDown event inside a Windows Forms application.
    /// </summary>
    public static class MoveWindow
    {
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) 
        /// or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        private const int WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// wParam that moves the window.
        /// </summary>
        /// <remarks>
        /// Why does this even work? This is undocumented on MSDN when it comes to 
        /// SYSCOMMAND messages, it also appears that all 0xF01X where X is a value from 1-F work.
        /// Closest documented SYSCOMMAND param is SC_MOVE, 0xF010.
        /// </remarks>
        private const int MOUSE_MOVE = 0xF012;

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
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

        /// <summary>
        /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing. 
        /// A window that has captured the mouse receives all mouse input, regardless of the position of the cursor, 
        /// except when a mouse button is clicked while the cursor hot spot is in the window of another thread. 
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        /// <summary>
        /// Moves the window if executed, accepts the windows form that is
        /// to be moved.
        /// </summary>
        /// <param name="handle">The handle to the form, event or control that is to be moved.</param>
        public static void MoveTheWindow(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WM_SYSCOMMAND, (IntPtr)MOUSE_MOVE, IntPtr.Zero);
        }
    }
}
