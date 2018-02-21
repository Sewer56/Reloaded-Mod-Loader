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

namespace Reloaded.Native
{
    /// <summary>
    /// Provides various Windows API P/Invoke function imports.
    /// </summary>
    public static class WinAPI
    {
        /// <summary>
        /// Defines a rectangle in the format used within the Windows API.
        /// </summary>
        public struct WinAPIRectangle
        {
            /// <summary>
            /// The X coordinate of the left border of the rectangle.
            /// </summary>
            public int leftBorder;

            /// <summary>
            /// The Y coordinate of the top border of the rectangle.
            /// </summary>
            public int topBorder;

            /// <summary>
            /// The X coordinate of the right border of the rectangle.
            /// </summary>
            public int rightBorder;

            /// <summary>
            /// The Y coordinate of the bottom border of the rectangle.
            /// </summary>
            public int bottomBorder;
        }

        /// <summary>
        /// Provides various Windows API Functions related to windows, 
        /// the ones on your desktop, not the operating system.
        /// </summary>
        public static class Windows
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
            public static extern bool GetWindowRect(IntPtr hwnd, out WinAPIRectangle lpRect);

            /// <summary>
            /// Obtains the screen coordinates of each of the edges of THE CLIENT AREA (WHAT YOU SEE INSIDE THE BORDERS) of a window holding a specified handle.
            /// Client coordinates are relative to the upper-left corner of a window's client area, thus likely the right will be the width and bottom the height.
            /// </summary>
            /// <param name="hWnd">Handle to the window.</param>
            /// <param name="lpRect">Rectangle structure containing the screen coordinates of all of the CLIENT AREA edges.</param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern bool GetClientRect(IntPtr hWnd, out WinAPIRectangle lpRect);

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
            public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref WinAPIRectangle pMargins);
        }

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

            /// <summary>
            /// Defines the individual constants for window styles.
            /// </summary>
            public static class Constants
            {
                /// <summary>
                /// Defines the nIndex for Get/SetWindowLong which returns or sets the extended window styles.
                /// </summary>
                public const int GWL_EXSTYLE = -20;

                /// <summary>
                /// Defines the nIndex for Get/SetWindowLong which returns or sets the regular window styles.
                /// </summary>
                public const int GWL_STYLE = -16;

                /// <summary>
                /// [Extended Window Style] The extended window style which declares the window as a layered window.
                /// See MSDN on Layered Windows: https://msdn.microsoft.com/en-us/library/windows/desktop/ms632599(v=vs.85).aspx#layered 
                /// </summary>
                public const long WS_EX_LAYERED = 0x80000;

                /// <summary>
                /// [Extended Window Style] The extended window style which declares the window as a transparent window.
                /// The window should not be painted until siblings beneath the window (created by the same thread) have been painted.
                /// Used alongside WS_EX_LAYERED, it disables hit testing such that windows under wouldn't receive clicks from the windows above.
                /// </summary>
                public const long WS_EX_TRANSPARENT = 0x20;

                /// <summary>
                /// [Extended Window Style] The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
                /// </summary>
                public const long WS_EX_TOPMOST = 0x00000008;

                /// <summary>
                /// [Extended Window Style] The window has a border with a sunken edge.
                /// </summary>
                public const long WS_EX_CLIENTEDGE = 0x00000200;

                /// <summary>
                /// [Extended Window Style] Paints all descendants of a window in bottom-to-top painting order using double-buffering.
                /// </summary>
                public const long WS_EX_COMPOSITED = 0x02000000;

                /// <summary>
                /// [Window Style] The Window has a horizontal scrollbar.
                /// </summary>
                public const long WS_HSCROLL = 0x00100000;

                /// <summary>
                /// [Window Style] The Window has a vertical scrollbar.
                /// </summary>
                public const long WS_VSCROLL = 0x00200000;

                /// <summary>
                /// [Layered Window] Uses bAlpha to determine the opacity of the layered window, allows for the changing of the opacity of the layered window.
                /// </summary>
                public const int LWA_ALPHA = 0x2;
            }
        }

        /// <summary>
        /// Defines the individual components involved with hooking window events via the use of Windows API.
        /// </summary>
        public static class WindowEvents
        {
            /// <summary>
            /// Defines a delegate to use in conjunction with SetWinEventHook.
            /// </summary>
            public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

            /// <summary>
            /// When the location or size of a window changes.
            /// </summary>
            public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;

            /// <summary>
            /// Allow cross-process hooking of window events.
            /// </summary>
            public const uint WINEVENT_OUTOFCONTEXT = 0;

            /// <summary>
            /// Allows to set an event hook function for a range of events.
            /// </summary>
            /// <param name="eventMin">Minimum event code of events to capture, can equal maximum.</param>
            /// <param name="eventMax">Maximum event code of events to capture, can equal maximum.</param>
            /// <param name="hmodWinEventProc">Handle to the DLL that contains the hook function at lpfnWinEventProc, the function we will be executing. Set to null in conjunction of WINEVENT_OUTOFCONTEXT dwFlags</param>
            /// <param name="lpfnWinEventProc">Pointer to the event hook function, either to the specified DLL or to your own code. Use a WinEventDelegate for this one.</param>
            /// <param name="idProcess">Specifies the ID of the process from which the hook function receives events. Specify zero (0) to receive events from all processes on the current desktop.</param>
            /// <param name="idThread">Specifies the ID of the thread from which the hook function receives events. If this parameter is zero, the hook function is associated with all existing threads on the current desktop.</param>
            /// <param name="dwFlags">Flag values that specify the location of the hook function and of the events to be skipped. WINEVENT_OUTOFCONTEXT is that you'll probably want.</param>
            /// <returns></returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        }
    }
}
