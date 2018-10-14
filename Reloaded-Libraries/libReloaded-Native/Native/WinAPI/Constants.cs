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

using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Native.WinAPI
{
    /// <summary>
    /// Provides a list of constants for various Windows API functions/
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
        public const uint WS_EX_TRANSPARENT = 0x20;

        /// <summary>
        /// [Extended Window Style] The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        /// </summary>
        public const uint WS_EX_TOPMOST = 0x00000008;

        /// <summary>
        /// [Extended Window Style] The window has a border with a sunken edge.
        /// </summary>
        public const uint WS_EX_CLIENTEDGE = 0x00000200;

        /// <summary>
        /// [Extended Window Style] Paints all descendants of a window in bottom-to-top painting order using double-buffering.
        /// </summary>
        public const uint WS_EX_COMPOSITED = 0x02000000;

        /// <summary>
        /// [Window Style] The Window has a horizontal scrollbar.
        /// </summary>
        public const uint WS_HSCROLL = 0x00100000;

        /// <summary>
        /// [Window Style] The Window has a vertical scrollbar.
        /// </summary>
        public const uint WS_VSCROLL = 0x00200000;

        /// <summary>
        /// [Window Style] The window has a thin-line border.
        /// </summary>
        public const uint WS_BORDER = 0x00800000;

        /// <summary>
        /// [Window Style] The window has a title bar (includes the WS_BORDER style).
        /// </summary>
        public const uint WS_CAPTION = 0x00C00000;

        /// <summary>
        /// [Window Style] The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        /// </summary>
        public const uint WS_CHILD = 0x40000000;

        /// <summary>
        /// [Window Style] The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
        /// </summary>
        public const uint WS_DISABLED = 0x08000000;

        /// <summary>
        /// [Window Style] The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        /// </summary>
        public const uint WS_DLGFRAME = 0x00400000;

        /// <summary>
        /// [Window Style] The window is initially maximized.
        /// </summary>
        public const uint WS_MAXIMIZE = 0x01000000;

        /// <summary>
        /// [Window Style] The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        public const uint WS_MAXIMIZEBOX = 0x00010000;

        /// <summary>
        /// [Window Style] The window is initially minimized. Same as the WS_ICONIC style.
        /// </summary>
        public const uint WS_MINIMIZE = 0x20000000;

        /// <summary>
        /// [Window Style] The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        public const uint WS_MINIMIZEBOX = 0x00020000;

        /// <summary>
        /// [Window Style] The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
        /// </summary>
        public const uint WS_OVERLAPPED = 0x00000000;

        /// <summary>
        /// [Window Style] The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
        /// </summary>
        public const uint WS_POPUP = 0x80000000;

        /// <summary>
        /// [Window Style] The window has a sizing border. Same as the WS_THICKFRAME style.
        /// </summary>
        public const uint WS_SIZEBOX = 0x00040000;

        /// <summary>
        /// [Window Style] The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        /// </summary>
        public const uint WS_SYSMENU = 0x00080000;

        /// <summary>
        /// [Window Style] The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        /// </summary>
        public const uint WS_TABSTOP = 0x00010000;

        /// <summary>
        /// [Window Style] The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        /// </summary>
        public const uint WS_VISIBLE = 0x10000000;

        /// <summary>
        /// [Layered Window] Uses bAlpha to determine the opacity of the layered window, allows for the changing of the opacity of the layered window.
        /// </summary>
        public const int LWA_ALPHA = 0x2;
    }
}
