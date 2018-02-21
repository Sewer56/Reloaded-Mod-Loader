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


using Reloaded.Misc;
using Reloaded.Misc.Config;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Defines a theme entry for each of the individual themes.
    /// Stores all of the a theme's fonts, colours as well as other constants.
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Defines the directory where a theme is to be stored.
        /// After setting or changing the directory, the new theme should manually be loaded
        /// with LoadTheme();
        /// </summary>
        private string themeDirectory;

        /// <summary>
        /// Constructor, initializes the class.
        /// </summary>
        public Theme()
        {
            // Initialize the fonts class.
            Fonts = new ThemeFonts();
        }

        /// <summary>
        /// Stores all of the currently loaded in fonts for this theme in particular.
        /// </summary>
        public static ThemeFonts Fonts { get; set; }

        /// <summary>
        /// Retrieves the general theme configuration for the current theme.
        /// </summary>
        public static ThemePropertyParser.ThemeConfig ThemeProperties { get; set; }

        /// <summary>
        /// Changes the directory for the theme to be used.
        /// After setting, the current theme is automatically changed/loaded.
        /// </summary>
        public string ThemeDirectory
        {
            get => themeDirectory;
            set
            {
                themeDirectory = value;
                LoadTheme();
            }
        }

        /// <summary>
        /// Loads the theme set at the current directory.
        /// </summary>
        public void LoadTheme()
        {
            // Kill current animations.
            ApplyTheme.KillAnimations();

            // Retrieve the theme properties
            ApplyTheme.LoadProperties(themeDirectory);

            // Load the fonts that are to be used in this session.
            Fonts.LoadFonts(LoaderPaths.GetModLoaderThemeDirectory() + "\\" + themeDirectory + "\\Fonts");

            // Load the images for the theme.
            ApplyTheme.LoadImages(LoaderPaths.GetModLoaderThemeDirectory() + "\\" + themeDirectory + "\\Images");

            // Apply the theme.
            ApplyTheme.ApplyCurrentTheme();
        }
    }
}
