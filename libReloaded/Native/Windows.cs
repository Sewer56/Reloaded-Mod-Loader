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
using System.Diagnostics;
using System.Drawing;
using static Reloaded.Native.WinAPI;

namespace Reloaded.Native
{
    /// <summary>
    /// Provides various classes for obtaining information about various windows that are currently present.
    /// </summary>
    public static class Windows
    {
        /// <summary>
        /// Returns the coordinates of the edges of a specific window relative to the desktop the window is presented on.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the window rectangle should be obtained.</param>
        /// <returns></returns>
        public static WinAPIRectangle GetWindowRectangle(IntPtr windowHandle)
        {
            // Stores the rectangle which defines Sonic Heroes' game window.
            WinAPIRectangle gameWindowRectangle = new WinAPIRectangle();

            // Obtains the coordinates of the edges of the window.
            WinAPI.Windows.GetWindowRect(windowHandle, out gameWindowRectangle);

            // Return
            return gameWindowRectangle;
        }

        /// <summary>
        /// Returns the coordinates of the edges of a specific window in terms of 
        /// X from the left and Y from the top of the window. 
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns></returns>
        public static WinAPIRectangle GetClientAreaRectangle(IntPtr windowHandle)
        {
            // Stores the rectangle which defines Sonic Heroes' game window.
            WinAPIRectangle clientAreaRectangle = new WinAPIRectangle();

            // Obtains the coordinates of the edges of the window.
            WinAPI.Windows.GetClientRect(windowHandle, out clientAreaRectangle);

            // Return
            return clientAreaRectangle;
        }

        /// <summary>
        /// Returns the border width in terms of X and Y for a window.
        /// </summary>
        /// <returns></returns>
        public static Point GetBorderWidth(WinAPIRectangle gameWindowRectangle, WinAPIRectangle gameClientRectangle)
        {
            // Stores the size of the border vertically and horizontally.
            Point totalBorderSize = new Point();

            // Calculate the width and height of the window.
            int windowWidth = gameWindowRectangle.rightBorder - gameWindowRectangle.leftBorder;
            int windowHeight = gameWindowRectangle.bottomBorder - gameWindowRectangle.topBorder;

            // Remove the client area width/height to leave only the borders.
            totalBorderSize.X = windowWidth - gameClientRectangle.rightBorder;
            totalBorderSize.Y = windowHeight - gameClientRectangle.bottomBorder;

            // Return the borders.
            return totalBorderSize;
        }

        /// <summary>
        /// Retrieves the client area size of a window. This is a slight alias for Get_Game_ClientArea_Rectangle.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetWindowClientSize(IntPtr windowHandle)
        {
            // Get Window Client-Area
            WinAPIRectangle windowClientArea = GetClientAreaRectangle(windowHandle);

            // Return window internal size.
            return new Point(windowClientArea.rightBorder, windowClientArea.bottomBorder);
        }


        /// <summary>
        /// Retrieves the client area size of a window. This is a slight alias for Get_Game_ClientArea_Rectangle.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetWindowSize(IntPtr windowHandle)
        {
            // Get Window Client-Area
            WinAPIRectangle windowSizeRectangle = GetWindowRectangle(windowHandle);

            // Define height and width
            int windowWidth = windowSizeRectangle.rightBorder - windowSizeRectangle.leftBorder;
            int windowHeight = windowSizeRectangle.bottomBorder - windowSizeRectangle.topBorder;

            // Return window internal size.
            return new Point(windowSizeRectangle.rightBorder, windowSizeRectangle.bottomBorder);
        }

        /// <summary>
        /// Checks whether the current application is activated.
        /// The method compares the current active foreground window to the
        /// window thread of the current caller. 
        /// 
        /// The function is not specific
        /// to any technology and works for both child windows and the current window,
        /// also independently of the thread which owns a specific window.
        /// </summary>
        /// <returns>Returns true if the current application has focus/is foreground/is activated. Else false.</returns>
        public static bool IsWindowActivated()
        {
            // Obtain the active window handle.
            IntPtr activatedHandle = WinAPI.Windows.GetForegroundWindow();

            // Check if any window is active.
            if (activatedHandle == IntPtr.Zero) { return false; }

            // Retrieve unique identifier for this process.
            int currentProcessIdentifier = Process.GetCurrentProcess().Id;

            // Retrieve the process identifier for the active window.
            int activeProcessIdentifier;
            WinAPI.Windows.GetWindowThreadProcessId(activatedHandle, out activeProcessIdentifier);

            // Compare the process identifiers of active window and our process.
            return activeProcessIdentifier == currentProcessIdentifier;
        }

        /// <summary>
        /// This variant of IsWindowActivated simply checks whether a specified window handle
        /// is currently focused rather than whether any window belonging to the current process
        /// is focused.
        /// </summary>
        /// <returns>Returns true if the specified handle has focus, else false.</returns>
        public static bool IsWindowActivated(IntPtr windowHandle)
        {
            // Obtain the active window handle.
            IntPtr activatedHandle = WinAPI.Windows.GetForegroundWindow();

            // Check if any window is active.
            if (activatedHandle == IntPtr.Zero) { return false; }

            // Compare the process identifiers of active window and our process.
            return activatedHandle == windowHandle;
        }
    }
}
