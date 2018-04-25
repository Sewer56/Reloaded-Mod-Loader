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
    /// Provides various Windows API Functions related to windows, 
    /// the ones on your desktop, not the operating system.
    /// </summary>
    public static class WindowFunctions
    {
        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). 
        /// The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads. 
        /// </summary>
        /// <returns>
        /// The return value is a handle to the foreground window. 
        /// The foreground window can be NULL in certain circumstances, such as when a window is losing activation. 
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Activates a window. The window must be attached to the calling thread's message queue.
        /// </summary>
        /// <param name="hWnd">A handle to the top-level window to be activated.</param>
        /// <returns>If the function succeeds, the return value is the handle to the window that was previously active.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window and, 
        /// optionally, the identifier of the process that created the window. 
        /// </summary>
        /// <param name="handle">A handle to the window. </param>
        /// <param name="processId">
        /// A pointer to a variable that receives the process identifier. 
        /// If this parameter is not NULL, GetWindowThreadProcessId copies the 
        /// identifier of the process to the variable; otherwise, it does not.
        /// </param>
        /// <returns>The return value is the identifier of the thread that created the window. </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        /// <summary>
        /// Allows us to set a window to become a child of another window (thus exist within the space of parent's window).
        /// </summary>
        /// <param name="hWndChild">The handle of the child window we are attaching.</param>
        /// <param name="hWndNewParent">The handle to the parent window we are attaching a window to.</param>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// Obtains the screen coordinates of each of the edges of a window holding a specified handle.
        /// </summary>
        /// <param name="hwnd">The handle to the individual window.</param>
        /// <param name="lpRect">Rectangle structure containing the screen coordinates of all of the edges.</param>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Structures.WinapiRectangle lpRect);

        /// <summary>
        /// Obtains the screen coordinates of each of the edges of THE CLIENT AREA (WHAT YOU SEE INSIDE THE BORDERS) of a window holding a specified handle.
        /// Client coordinates are relative to the upper-left corner of a window's client area, thus likely the right will be the width and bottom the height.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="lpRect">Rectangle structure containing the screen coordinates of all of the CLIENT AREA edges.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Structures.WinapiRectangle lpRect);

        /// <summary>
        /// The MoveWindow function changes the position and dimensions of the specified window. 
        /// For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. 
        /// For a child window, they are relative to the upper-left corner of the parent window's client area.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="x">Specifies the new position of the left side of the window.</param>
        /// <param name="y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">
        /// Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. 
        /// If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, 
        /// the nonclient area (including the title bar and scroll bars), 
        /// and any part of the parent window uncovered as a result of moving a child window.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// Tries to find a window by matching the name of the window with all of the current top-level windows (does not search child windows).
        /// Returns the handle to the window if found.
        /// </summary>
        /// <param name="lpClassName">Name of the class the window belongs in. Can be null.</param>
        /// <param name="lpWindowName">Name of the window we are looking for. Can be null.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Returns a boolean true/false declaring whether a specific window is visible given the handle to the specific window.
        /// </summary>
        /// <param name="hWnd">The handle of the window to test for visibility.</param>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// Extends the window frame into the client area.
        /// </summary>
        /// <param name="hWnd">The handle of the window of whose aero glass frame is to be extended.</param>
        /// <param name="pMargins"></param>
        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Structures.WinapiRectangle pMargins);

        /// <summary>
        /// The ClientToScreen function converts the client-area coordinates of a specified point to screen coordinates.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose client area is used for the conversion.</param>
        /// <param name="lpPoint">A pointer to a POINT structure that contains the client coordinates to be converted. The new screen coordinates are copied into this structure if the function succeeds.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Structures.WinapiPoint lpPoint);
    }
}
