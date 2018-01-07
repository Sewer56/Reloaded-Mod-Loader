using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Utilities.Fonts
{
    /// <summary>
    /// The FontLoader class allows for the dynamic loading of fonts, provided the path to a TTF or supported 
    /// font file format without having the user manually have to install the fonts (normally requiring administrative)
    /// priviledges.
    /// </summary>
    public static class FontLoader
    {
        /// <summary>
        /// Loads a font from a specified path/location and returns the instance of the requested specified font.
        /// </summary>
        /// <param name="fontPath">The full path to the specific font file that is to be loaded.</param>
        public static Font LoadExternalFont(string fontPath, float fontSize)
        {
            // Instantiate a Private Font Colelction
            PrivateFontCollection modernFont = new PrivateFontCollection();

            // Add the font by its path.
            modernFont.AddFontFile(fontPath);

            // Instantiate the font and return the font.
            return new Font(modernFont.Families[0], fontSize);
        }
    }
}
