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

using Reloaded.Misc.Config;
using ReloadedLauncher.Utilities.Fonts;
using System.Drawing;
using System.IO;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Provides an implmentation that loads and stores specific style fonts for a theme.
    /// Allows for the storage of individual fonts which are currently used by the theme.
    /// </summary>
    public class ThemeFonts
    {
        /// <summary>
        /// Font displayed on the title bar of the application.
        /// </summary>
        public Font TitleFont { get; set; }

        /// <summary>
        /// Font displayed on the category bar under the title bar of the application.
        /// </summary>
        public Font CategoryFont { get; set; }

        /// <summary>
        /// The main font used for text within the application.
        /// </summary>
        public Font TextFont { get; set; }

        // Store the style of the current theme's fonts.
        private static System.Drawing.FontStyle textFontStyle;
        private static System.Drawing.FontStyle titleFontStyle;
        private static System.Drawing.FontStyle categoryFontStyle;

        // Store font locations.
        string titleFontLocation;
        string categoryFontLocation;
        string textFontLocation;

        // Private font collections. (Store to prevent Garbage Collection)
        FontLoader textFontLoader;
        FontLoader categoryFontLoader;
        FontLoader titleFontLoader;

        /// <summary>
        /// Loads all of the fonts used by this individual theme.
        /// </summary>
        /// <param name="fontDirectory">Path to the directory where the fonts for the current theme are stored.</param>
        public void LoadFonts(string fontDirectory)
        {
            // Instantiate font styles.
            textFontStyle = FontStyle.Regular;
            titleFontStyle = FontStyle.Regular;
            categoryFontStyle = FontStyle.Regular;

            // Create the font loaders.
            textFontLoader = new FontLoader();
            categoryFontLoader = new FontLoader();
            titleFontLoader = new FontLoader();

            // Calculate font styles.
            textFontStyle = GetFontStyle(textFontStyle, Theme.ThemeProperties.TextFontStyle);
            categoryFontStyle = GetFontStyle(categoryFontStyle, Theme.ThemeProperties.CategoryFontStyle);
            titleFontStyle = GetFontStyle(titleFontStyle, Theme.ThemeProperties.TitleFontStyle);

            // Initialize font locations.
            titleFontLocation = "";
            categoryFontLocation = "";
            textFontLocation = "";

            // Check if any file paths exist.
            if (File.Exists(fontDirectory + "\\TitleFont.ttf")) { titleFontLocation = fontDirectory + "\\TitleFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\TitleFont.otf")) { titleFontLocation = fontDirectory + "\\TitleFont.otf"; }

            if (File.Exists(fontDirectory + "\\CategoryFont.ttf")) { categoryFontLocation = fontDirectory + "\\CategoryFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\CategoryFont.otf")) { categoryFontLocation = fontDirectory + "\\CategoryFont.otf"; }

            if (File.Exists(fontDirectory + "\\TextFont.ttf")) { textFontLocation = fontDirectory + "\\TextFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\TextFont.otf")) { textFontLocation = fontDirectory + "\\TextFont.ttf"; }

            // Load appropriate fonts.
            if (titleFontLocation != "") { TitleFont = titleFontLoader.LoadExternalFont(titleFontLocation, 20.25F); }
            else { TitleFont = new Font("Times New Roman", 20.25F, FontStyle.Regular); }

            if (categoryFontLocation != "") { CategoryFont = categoryFontLoader.LoadExternalFont(categoryFontLocation, 20.25F); }
            else { CategoryFont = new Font("Times New Roman", 20.25F, FontStyle.Regular); }

            if (textFontLocation != "") { TextFont = textFontLoader.LoadExternalFont(textFontLocation, 18F); }
            else { TextFont = new Font("Times New Roman", 18F, FontStyle.Regular); }

            // Set font style.
            TitleFont = new Font(TitleFont, titleFontStyle);
            TextFont = new Font(TextFont, textFontStyle);
            CategoryFont = new Font(CategoryFont, categoryFontStyle);
        }

        /// <summary>
        /// Sets the font style for the fonts.
        /// </summary>
        /// <param name="fontStyle">The font style as set in the theme properties.</param>
        /// <param name="systemFontStyle">The System.Drawing.Fontstyle to change. (Should be initialized to regular)</param>
        private FontStyle GetFontStyle(FontStyle systemFontStyle, ThemePropertyParser.FontStyle fontStyle)
        {
            // Check font style flags and apply.
            if (fontStyle.Bold) { systemFontStyle = systemFontStyle | FontStyle.Bold; }
            if (fontStyle.Underlined) { systemFontStyle = systemFontStyle | FontStyle.Underline; }
            if (fontStyle.Striked) { systemFontStyle = systemFontStyle | FontStyle.Strikeout; }
            if (fontStyle.Italic) { systemFontStyle = systemFontStyle | FontStyle.Italic; }

            // Return new font.
            return systemFontStyle;
        }
    }
}
