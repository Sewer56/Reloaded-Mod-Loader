using System;
using System.Drawing;

namespace Reloaded.Misc
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
