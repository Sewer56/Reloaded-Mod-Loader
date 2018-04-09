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

// ReSharper disable InconsistentNaming

using System;
using System.Drawing;

namespace Reloaded.Utilities
{
    /// <summary>
    /// Parses colours in the ARGB hexadecimal #AARRGGBB format and returns them in the format
    /// supported by System.Drawing.Color.
    /// </summary>
    public static class ColourLoader
    {
        /// <summary>
        /// Accepts a string in the #AARRGGBB format and returns an equivalent System.Drawing.Color
        /// colour to be used by e.g. Windows Forms.
        /// </summary>
        /// <param name="colourAARRGGBB">A hex colour in the #AARRGGBB format.</param>
        public static Color AARRGGBBToColor(string colourAARRGGBB)
        {
            return Color.FromArgb(Convert.ToInt32(colourAARRGGBB.Substring(1), 16));
        }
    }
}
