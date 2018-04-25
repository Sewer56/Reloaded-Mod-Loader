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
using System.Drawing;
using Reloaded.Native.WinAPI;

namespace Reloaded.Native.Functions
{
    /// <summary>
    /// Provides various classes for obtaining information about various windows that are currently present.
    /// </summary>
    public static class WindowProperties
    {
        /// <summary>
        /// Returns the coordinates of the edges of a specific window relative to the desktop the window is presented on.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the window rectangle should be obtained.</param>
        /// <returns></returns>
        public static Structures.WinapiRectangle GetWindowRectangle(IntPtr windowHandle)
        {
            // Obtains the coordinates of the edges of the window.
            WindowFunctions.GetWindowRect(windowHandle, out Structures.WinapiRectangle gameWindowRectangle);

            // Return
            return gameWindowRectangle;
        }

        /// <summary>
        /// Returns the coordinates of the edges of the client area of a specific window 
        /// relative to the desktop the window is presented on.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns></returns>
        public static Structures.WinapiRectangle GetClientRectangle(IntPtr windowHandle)
        {
            // Obtains the coordinates of the edges of the window.
            WindowFunctions.GetClientRect(windowHandle, out Structures.WinapiRectangle clientAreaRectangle);

            // Get the coordinates of the top left point on the screen in client's area.
            Structures.WinapiPoint topLeftClientCoordinate = new Structures.WinapiPoint();
            WindowFunctions.ClientToScreen(windowHandle, ref topLeftClientCoordinate);

            // Calculate each edge.
            Structures.WinapiRectangle clientArea = new Structures.WinapiRectangle();
            clientArea.LeftBorder = topLeftClientCoordinate.x;
            clientArea.TopBorder = topLeftClientCoordinate.y;

            clientArea.RightBorder = topLeftClientCoordinate.x + clientAreaRectangle.RightBorder;
            clientArea.BottomBorder = topLeftClientCoordinate.y + clientAreaRectangle.BottomBorder;

            // Return
            return clientArea;
        }

        /// <summary>
        /// Returns the coordinates of the edges of a specific window in terms of 
        /// X from the left and Y from the top of the window. 
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns></returns>
        public static Structures.WinapiRectangle GetClientAreaSize(IntPtr windowHandle)
        {
            // Obtains the size of the client area.
            WindowFunctions.GetClientRect(windowHandle, out Structures.WinapiRectangle clientAreaRectangle);

            // Return
            return clientAreaRectangle;
        }

        /// <summary>
        /// Returns the border width in terms of X and Y for a window.
        /// </summary>
        /// <returns>The border width and height as X and Y coordinates.</returns>
        public static Point GetBorderWidth(Structures.WinapiRectangle gameWindowRectangle, Structures.WinapiRectangle gameClientRectangle)
        {
            // Stores the size of the border vertically and horizontally.
            Point totalBorderSize = new Point();

            // Calculate the width and height of the window.
            int windowWidth = gameWindowRectangle.RightBorder - gameWindowRectangle.LeftBorder;
            int windowHeight = gameWindowRectangle.BottomBorder - gameWindowRectangle.TopBorder;

            // Remove the client area width/height to leave only the borders.
            totalBorderSize.X = windowWidth - gameClientRectangle.RightBorder;
            totalBorderSize.Y = windowHeight - gameClientRectangle.BottomBorder;

            // Return the borders.
            return totalBorderSize;
        }

        /// <summary>
        /// Retrieves the window client area size.
        /// i.e. The X and Y sizes, in pixels excluding the window borders.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetClientAreaSize2(IntPtr windowHandle)
        {
            // Get Window Client-Area
            Structures.WinapiRectangle windowClientArea = GetClientAreaSize(windowHandle);

            // Return window internal size.
            return new Point(windowClientArea.RightBorder, windowClientArea.BottomBorder);
        }


        /// <summary>
        /// Retrieves the window size.
        /// i.e. The X and Y sizes, in pixels including the window borders.
        /// </summary>
        /// <param name="windowHandle">Handle to the window of which the client area rectangle should be obtained.</param>
        /// <returns>Width as X and Height as Y of the window client area requested.</returns>
        public static Point GetWindowSize(IntPtr windowHandle)
        {
            // Get Window Client-Area
            Structures.WinapiRectangle windowSizeRectangle = GetWindowRectangle(windowHandle);

            // Define height and width
            int windowWidth = windowSizeRectangle.RightBorder - windowSizeRectangle.LeftBorder;
            int windowHeight = windowSizeRectangle.BottomBorder - windowSizeRectangle.TopBorder;

            // Return window internal size.
            return new Point(windowWidth, windowHeight);
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
            IntPtr activatedHandle = WindowFunctions.GetForegroundWindow();

            // Check if any window is active.
            if (activatedHandle == IntPtr.Zero) return false;

            // Retrieve unique identifier for this process.
            int currentProcessIdentifier = System.Diagnostics.Process.GetCurrentProcess().Id;

            // Retrieve the process identifier for the active window.
            WindowFunctions.GetWindowThreadProcessId(activatedHandle, out int activeProcessIdentifier);

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
            IntPtr activatedHandle = WindowFunctions.GetForegroundWindow();

            // Check if any window is active.
            if (activatedHandle == IntPtr.Zero) return false;

            // Compare the process identifiers of active window and our process.
            return activatedHandle == windowHandle;
        }
    }
}
