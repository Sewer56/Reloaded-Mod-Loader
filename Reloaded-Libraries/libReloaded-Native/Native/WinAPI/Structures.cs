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

namespace Reloaded.Native.WinAPI
{
    /// <summary>
    /// Provides the individual structures used within Windows API functions.
    /// </summary>
    public static class Structures
    {
        /// <summary>
        /// Defines a rectangle in the format used within the Windows API.
        /// </summary>
        public struct WinapiRectangle
        {
            /// <summary>
            /// The X coordinate of the left border of the rectangle.
            /// </summary>
            public int LeftBorder;

            /// <summary>
            /// The Y coordinate of the top border of the rectangle.
            /// </summary>
            public int TopBorder;

            /// <summary>
            /// The X coordinate of the right border of the rectangle.
            /// </summary>
            public int RightBorder;

            /// <summary>
            /// The Y coordinate of the bottom border of the rectangle.
            /// </summary>
            public int BottomBorder;
        }

        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
        public struct WinapiPoint
        {
            /// <summary>
            /// The x-coordinate of the point.
            /// </summary>
            public int x;

            /// <summary>
            /// The y-coordinate of the point.
            /// </summary>
            public int y;
        }
    }
}
