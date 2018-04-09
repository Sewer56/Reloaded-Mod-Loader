/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
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

namespace Reloaded.Native.WinAPI
{
    /// <summary>
    /// Defines the individual components involved with getting and setting window styles and visuals via the use of Windows API.
    /// </summary>
    public static class WindowStyles
    {
        /// 
        /// Set Window Properties 32/64
        /// 

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLong32(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// Sets window properties to the user specified window properties.
        /// Selects appropriate method to execute on both x86 and x64.
        /// Likely to be used in order to change extended window properties.
        /// </summary>
        /// <param name="hWnd">
        /// Handle Reference to the window we are modifying.
        /// The handle reference is defined as object,handle.
        /// e.g. For Windows form: new HandleRef(this,this.Handle)
        /// </param>
        /// <param name="nIndex">See MSDN: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591(v=vs.85).aspx</param>
        /// <param name="dwNewLong">The value to be set.</param>
        /// <returns>The old value prior to being overwritten.</returns>
        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            // Check if x64
            if (IntPtr.Size == 8) return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

            // Else go x86
            return SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        /// 
        /// Get Window Properties 32/64
        /// 

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        /// <summary>
        /// Retrieves window properties at user's specified nIndex.
        /// Selects appropriate method to execute on both x86 and x64.
        /// Likely to be used in order to retrieve extended window properties.
        /// </summary>
        /// <param name="hWnd">Handle to the window we are modifying.</param>
        /// <param name="nIndex">See MSDN: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591(v=vs.85).aspx </param>
        /// <returns>The requested information stored at nIndex.</returns>
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            // Check if x64
            if (IntPtr.Size == 8) return GetWindowLongPtr64(hWnd, nIndex);

            // Else go x86
            return GetWindowLongPtr32(hWnd, nIndex);
        }

        /// <summary>
        /// Allows for setting window attributes of a window with the extended window style of WS_EX_LAYERED.
        /// Used for setting an alpha colour and/or enabling an alpha channel.
        /// </summary>
        /// <param name="hwnd">Handle to the window of which attributes are to be set.</param>
        /// <param name="crKey">A COLORREF struct, 0x00BBGGRR.</param>
        /// <param name="bAlpha">Defines the opacity of the window in question.</param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
    }
}
