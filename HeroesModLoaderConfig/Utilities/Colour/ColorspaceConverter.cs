using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Utilities.Colour
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
        public static ColorMine.ColorSpaces.Lch ColorToLCH(Color color)
        {
            // First convert to ColorMine.RGB 
            ColorMine.ColorSpaces.Rgb colorRGB = new ColorMine.ColorSpaces.Rgb(color.R / 255.0, color.G / 255.0, color.B / 255.0);

            // Return as LCH.
            return new ColorMine.ColorSpaces.Lch(colorRGB);
        }

        /// <summary>
        /// Converts ColorMine.ColorSpaces.Lch to System.Drawing.Color 
        /// </summary>
        /// <param name="LCHColor">The LCH colour that is to be converted to System.Drawing.Color.</param>
        /// <returns>The source RGB colour in LCH.</returns>
        public static Color LCHToColor(ColorMine.ColorSpaces.Lch LCHColor)
        {
            // First convert to ColorSpace.colorRGB
            ColorMine.ColorSpaces.Rgb colorRGB = new ColorMine.ColorSpaces.Rgb(LCHColor);

            // Retrieve the RGB Components
            int R = (int)Math.Round(colorRGB.R * 255.0, MidpointRounding.AwayFromZero);
            int G = (int)Math.Round(colorRGB.G * 255.0, MidpointRounding.AwayFromZero);
            int B = (int)Math.Round(colorRGB.B * 255.0, MidpointRounding.AwayFromZero);

            // Ensure they are in permissible ranges. (In case of multithreading, concurrency issues)
            if (R > 255) { R = 255; }
            if (G > 255) { G = 255; }
            if (B > 255) { B = 255; }

            // Return as System.Drawing.Color
            return Color.FromArgb(R, G, B);
        }

        /// <summary>
        /// Converts a list of ColorMine.ColorSpaces.Lch to System.Drawing.Color 
        /// </summary>
        /// <param name="LCHColors">The LCH colours to be converted to System.Drawing.Color.</param>
        public static List<Color> LCHListToColor(List<ColorMine.ColorSpaces.Lch> LCHColors)
        {
            // Instantiate a new list of colours.
            List<Color> colorList = new List<Color>(LCHColors.Count);

            // For each colour in the LCH list convert to color and add to list.
            foreach(ColorMine.ColorSpaces.Lch colorLCH in LCHColors) { colorList.Add(LCHToColor(colorLCH)); }

            // Return list of colors.
            return colorList;
        }

        /// <summary>
        /// Converts a list of System.Drawing.Color to ColorMine.ColorSpaces.Lch.  
        /// </summary>
        /// <param name="colorList">The System.Drawing.Color(s) to be converted to ColorMine.ColorSpaces.Lch</param>
        public static List<ColorMine.ColorSpaces.Lch> ColorListToLCH(List<Color> colorList)
        {
            // Instantiate a new list of colours.
            List<ColorMine.ColorSpaces.Lch> LCHList = new List<ColorMine.ColorSpaces.Lch>(colorList.Count);

            // For each colour in the LCH list convert to color and add to list.
            foreach (Color color in colorList) { LCHList.Add(ColorToLCH(color)); }

            // Return list of colors.
            return LCHList;
        }
    }
}
