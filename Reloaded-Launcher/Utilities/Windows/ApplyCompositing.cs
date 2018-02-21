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
    /// <summary>
    /// Applies compositing to a Windows Forms window by enabling the WS_EX_COMPOSITED
    /// extended window style.
    /// </summary>
    public static class WindowCompositing
    {
        /// <summary>
        /// Applies compositing to a Windows Forms window by enabling the WS_EX_COMPOSITED
        /// extended window style.
        /// </summary>
        /// <param name="windowsForm">The Windows form whose compositing is to be applied.</param>
        public static void ApplyCompositing(Form windowsForm)
        {
            // Retrieves the extended window style.
            long extendedWindowStyle = (long)WinAPI.WindowStyles.GetWindowLongPtr(windowsForm.Handle, WinAPI.WindowStyles.Constants.GWL_EXSTYLE);

            // Append WS_EX_COMPOSITED window style.
            extendedWindowStyle |= WinAPI.WindowStyles.Constants.WS_EX_COMPOSITED;

            // Set window style.
            WinAPI.WindowStyles.SetWindowLongPtr(new HandleRef(windowsForm, windowsForm.Handle), WinAPI.WindowStyles.Constants.GWL_EXSTYLE, (IntPtr)extendedWindowStyle);
        }
    }
}
