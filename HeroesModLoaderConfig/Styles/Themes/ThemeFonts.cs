using HeroesModLoaderConfig.Utilities.Fonts;
using System.Drawing;
using System.IO;

namespace HeroesModLoaderConfig.Styles.Themes
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

        /// <summary>
        /// Loads all of the fonts used by this individual theme.
        /// </summary>
        /// <param name="fontDirectory">Path to the directory where the fonts for the current theme are stored.</param>
        public void LoadFonts(string fontDirectory)
        {
            // Store font locations.
            string titleFontLocation = "";
            string categoryFontLocation = "";
            string textFontLocation = "";

            // Check if any file paths exist.
            if (File.Exists(fontDirectory + "\\TitleFont.ttf")) { titleFontLocation = fontDirectory + "\\TitleFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\TitleFont.otf")) { titleFontLocation = fontDirectory + "\\TitleFont.otf"; }

            if (File.Exists(fontDirectory + "\\CategoryFont.ttf")) { categoryFontLocation = fontDirectory + "\\CategoryFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\CategoryFont.otf")) { categoryFontLocation = fontDirectory + "\\CategoryFont.otf"; }

            if (File.Exists(fontDirectory + "\\TextFont.ttf")) { textFontLocation = fontDirectory + "\\TextFont.ttf"; }
            else if (File.Exists(fontDirectory + "\\TextFont.otf")) { textFontLocation = fontDirectory + "\\TextFont.ttf"; }

            // Load appropriate fonts.
            if (titleFontLocation != "") { TitleFont = FontLoader.LoadExternalFont(titleFontLocation, 20.25F); }
            else { TitleFont = new Font("Times New Roman", 20.25F, FontStyle.Regular); }

            if (categoryFontLocation != "") { CategoryFont = FontLoader.LoadExternalFont(categoryFontLocation, 20.25F); }
            else { CategoryFont = new Font("Times New Roman", 20.25F, FontStyle.Regular); }

            if (textFontLocation != "") { TextFont = FontLoader.LoadExternalFont(textFontLocation, 18F); }
            else { TextFont = new Font("Times New Roman", 18F, FontStyle.Regular); }
        }
    }
}
