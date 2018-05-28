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
using System.IO;
using System.Windows.Forms;
using Reloaded.IO.Config;
using Reloaded_GUI.Utilities.Fonts;

namespace Reloaded_GUI.Styles.Themes.ApplyTheme
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This file handles the font aspect of the form.
    /// </summary>
    public static partial class ApplyTheme
    {
        /// <summary>
        /// Font displayed on the title bar of the application.
        /// </summary>
        public static Font TitleFont { get; set; }

        /// <summary>
        /// Font displayed on the category bar under the title bar of the application.
        /// </summary>
        public static Font CategoryFont { get; set; }

        /// <summary>
        /// The main font used for text within the application.
        /// </summary>
        public static Font TextFont { get; set; }

        /* Wrapped font collections (Store to prevent Garbage Collection of used fonts) */
        private static FontLoader _textFontLoader;
        private static FontLoader _categoryFontLoader;
        private static FontLoader _titleFontLoader;

        /// <summary>
        /// Loads all of the fonts used by this individual theme.
        /// </summary>
        /// <param name="fontDirectory">Path to the directory where the fonts for the current theme are stored.</param>
        public static void LoadFonts(string fontDirectory)
        {
            // Instantiate font styles.
            FontStyle textFontStyle = FontStyle.Regular;
            FontStyle titleFontStyle = FontStyle.Regular;
            FontStyle categoryFontStyle = FontStyle.Regular;

            // Create the font loaders.
            _textFontLoader = new FontLoader();
            _categoryFontLoader = new FontLoader();
            _titleFontLoader = new FontLoader();

            // Initialize font locations.
            string titleFontLocation = "";
            string categoryFontLocation = "";
            string textFontLocation = "";

            // Calculate font styles.
            textFontStyle = GetFontStyle(textFontStyle, Theme.ThemeProperties.TextFontStyle);
            categoryFontStyle = GetFontStyle(categoryFontStyle, Theme.ThemeProperties.CategoryFontStyle);
            titleFontStyle = GetFontStyle(titleFontStyle, Theme.ThemeProperties.TitleFontStyle);

            // Check if any file paths exist.
            if (File.Exists(fontDirectory + "\\TitleFont.ttf"))
                titleFontLocation = fontDirectory + "\\TitleFont.ttf";
            else if (File.Exists(fontDirectory + "\\TitleFont.otf"))
                titleFontLocation = fontDirectory + "\\TitleFont.otf";

            if (File.Exists(fontDirectory + "\\CategoryFont.ttf"))
                categoryFontLocation = fontDirectory + "\\CategoryFont.ttf";
            else if (File.Exists(fontDirectory + "\\CategoryFont.otf"))
                categoryFontLocation = fontDirectory + "\\CategoryFont.otf";

            if (File.Exists(fontDirectory + "\\TextFont.ttf"))
                textFontLocation = fontDirectory + "\\TextFont.ttf";
            else if (File.Exists(fontDirectory + "\\TextFont.otf"))
                textFontLocation = fontDirectory + "\\TextFont.ttf";

            // Load appropriate fonts.
            TitleFont = titleFontLocation != "" ? _titleFontLoader.LoadExternalFont(titleFontLocation, 20.25F) : new Font("Times New Roman", 20.25F, FontStyle.Regular);
            CategoryFont = categoryFontLocation != "" ? _categoryFontLoader.LoadExternalFont(categoryFontLocation, 20.25F) : new Font("Times New Roman", 20.25F, FontStyle.Regular);
            TextFont = textFontLocation != "" ? _textFontLoader.LoadExternalFont(textFontLocation, 18F) : new Font("Times New Roman", 18F, FontStyle.Regular);

            // Set font style.
            TitleFont = new Font(TitleFont, titleFontStyle);
            TextFont = new Font(TextFont, textFontStyle);
            CategoryFont = new Font(CategoryFont, categoryFontStyle);
        }

        /// <summary>
        /// Applies the common font style for an individual control.
        /// Changes the font used for the control to the one derived from the theme.
        /// </summary>
        private static void ApplyFonts(Control control)
        {
            // Filter the three text categories.
            if (IsTitleItem(control))
                control.Font = new Font(TitleFont.FontFamily, control.Font.Size, TitleFont.Style, control.Font.Unit);

            else if (IsCategoryItem(control))
                control.Font = new Font(CategoryFont.FontFamily, control.Font.Size, CategoryFont.Style, control.Font.Unit);

            else if (IsMainItem(control))
                control.Font = new Font(TextFont.FontFamily, control.Font.Size, TextFont.Style, control.Font.Unit);

            else if (IsBorderless(control))
                control.Font = new Font(TextFont.FontFamily, control.Font.Size, TextFont.Style, control.Font.Unit);

            else if (IsBox(control))
                control.Font = new Font(TextFont.FontFamily, control.Font.Size, TextFont.Style, control.Font.Unit);
        }

        /// <summary>
        /// Sets the font style for the fonts.
        /// </summary>
        /// <param name="fontStyle">The font style as set in the theme properties.</param>
        /// <param name="systemFontStyle">The System.Drawing.Fontstyle to change. (Should be initialized to regular)</param>
        private static FontStyle GetFontStyle(FontStyle systemFontStyle, ThemeProperties.FontStyle fontStyle)
        {
            // Check font style flags and apply.
            if (fontStyle.Bold) systemFontStyle = systemFontStyle | FontStyle.Bold;
            if (fontStyle.Underlined) systemFontStyle = systemFontStyle | FontStyle.Underline;
            if (fontStyle.Striked) systemFontStyle = systemFontStyle | FontStyle.Strikeout;
            if (fontStyle.Italic) systemFontStyle = systemFontStyle | FontStyle.Italic;

            // Return new font.
            return systemFontStyle;
        }
    }
}
