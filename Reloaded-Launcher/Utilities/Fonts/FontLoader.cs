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
        private PrivateFontCollection privateFontCollection;

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
