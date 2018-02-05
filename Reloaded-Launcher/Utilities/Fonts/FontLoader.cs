using System.Drawing;
using System.Drawing.Text;

namespace ReloadedLauncher.Utilities.Fonts
{
    /// <summary>
    /// The FontLoader class allows for the dynamic loading of fonts, provided the path to a TTF or supported 
    /// font file format without having the user manually have to install the fonts (normally requiring administrative)
    /// priviledges.
    /// </summary>
    public class FontLoader
    {
        /// <summary>
        /// Stores the fonts to be loaded from files for this class (prevent font Garbage Collection).
        /// </summary>
        PrivateFontCollection privateFontCollection;

        /// <summary>
        /// Loads a font from a specified path/location and returns the instance of the requested specified font.
        /// </summary>
        /// <param name="fontPath">The full path to the specific font file that is to be loaded.</param>
        public Font LoadExternalFont(string fontPath, float fontSize)
        {
            // Instantiate a Private Font Collection
            privateFontCollection = new PrivateFontCollection();

            // Add the font by its path.
            privateFontCollection.AddFontFile(fontPath);

            // Instantiate the font and return the font.
            return new Font(privateFontCollection.Families[0], fontSize);
        }
    }
}
