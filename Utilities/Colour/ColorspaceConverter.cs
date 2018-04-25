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
using System.Collections.Generic;
using System.Drawing;
using ColorMine.ColorSpaces;

namespace Reloaded_GUI.Utilities.Colour
{
    /// <summary>
    /// Provides various conversions between System.Drawing.Color and the Colorspace library.
    /// </summary>
    public static class ColorspaceConverter
    {
        /// <summary>
        /// Converts System.Drawing.Color to ColorMine.ColorSpaces.Lch.
        /// </summary>
        /// <param name="color">The colour that is to be converted to LCH.</param>
        /// <returns>The source RGB colour in LCH.</returns>
        public static Lch ColorToLch(Color color)
        {
            // First convert to ColorMine.RGB 
            Rgb colorRgb = new Rgb(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            // Return as LCH.
            return new Lch(colorRgb);
        }

        /// <summary>
        /// Converts ColorMine.ColorSpaces.Lch to System.Drawing.Color 
        /// </summary>
        /// <param name="lchColor">The LCH colour that is to be converted to System.Drawing.Color.</param>
        /// <returns>The source RGB colour in LCH.</returns>
        public static Color LchToColor(Lch lchColor)
        {
            // First convert to ColorSpace.colorRGB
            Rgb colorRGB = new Rgb(lchColor);

            // Retrieve the RGB Components
            int R = (int)Math.Round(colorRGB.R * 255.0, MidpointRounding.AwayFromZero);
            int G = (int)Math.Round(colorRGB.G * 255.0, MidpointRounding.AwayFromZero);
            int B = (int)Math.Round(colorRGB.B * 255.0, MidpointRounding.AwayFromZero);

            // Ensure they are in permissible ranges. (In case of multithreading, concurrency issues)
            if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;

            // Return as System.Drawing.Color
            return Color.FromArgb(R, G, B);
        }

        /// <summary>
        /// Converts a list of ColorMine.ColorSpaces.Lch to System.Drawing.Color 
        /// </summary>
        /// <param name="lchColors">The LCH colours to be converted to System.Drawing.Color.</param>
        public static List<Color> LchListToColor(List<Lch> lchColors)
        {
            // Instantiate a new list of colours.
            List<Color> colorList = new List<Color>(lchColors.Count);

            // For each colour in the LCH list convert to color and add to list.
            foreach(Lch colorLch in lchColors) colorList.Add(LchToColor(colorLch));

            // Return list of colors.
            return colorList;
        }

        /// <summary>
        /// Converts a list of System.Drawing.Color to ColorMine.ColorSpaces.Lch.  
        /// </summary>
        /// <param name="colorList">The System.Drawing.Color(s) to be converted to ColorMine.ColorSpaces.Lch</param>
        public static List<Lch> ColorListToLch(List<Color> colorList)
        {
            // Instantiate a new list of colours.
            List<Lch> lchList = new List<Lch>(colorList.Count);

            // For each colour in the LCH list convert to color and add to list.
            foreach (Color color in colorList) lchList.Add(ColorToLch(color));

            // Return list of colors.
            return lchList;
        }
    }
}
