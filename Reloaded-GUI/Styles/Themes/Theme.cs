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


using Reloaded.IO.Config;
using Reloaded.Paths;

namespace Reloaded_GUI.Styles.Themes
{
    /// <summary>
    /// Defines a theme entry for each of the individual themes.
    /// Stores all of the a theme's fonts, colours as well as other constants.
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Retrieves the general theme configuration for the current theme.
        /// </summary>
        public static ThemeProperties.Theme ThemeProperties { get; set; }

        /// <summary>
        /// Changes the directory for the theme to be used.
        /// After setting, the current theme is automatically changed/loaded.
        /// The directory is relative to the mod loader's Config/Themes directory in AppData.
        /// </summary>
        public string ThemeDirectory
        {
            get => _themeDirectory;
            set
            {
                _themeDirectory = value;
                LoadTheme();
            }
        }

        /// <summary>
        /// Defines the directory where a theme is to be stored.
        /// This location is relative to Reloaded's theme directory.
        /// After setting or changing the directory, the new theme should manually be loaded
        /// with LoadTheme();
        /// </summary>
        private string _themeDirectory;

        /// <summary>
        /// Automatically obtains and loads the currently enabled theme in the user's
        /// Reloaded Mod Loader settings.
        /// </summary>
        public void LoadCurrentTheme()
        {
            ThemeDirectory = Reloaded.IO.Config.LoaderConfig.ParseConfig().CurrentTheme;
        }

        /// <summary>
        /// Loads the theme set at the current directory.
        /// </summary>
        private void LoadTheme()
        {
            // Kill current animations.
            ApplyTheme.ApplyTheme.KillAnimations();

            // Retrieve the theme properties
            ApplyTheme.ApplyTheme.LoadProperties(_themeDirectory);

            // Load the fonts that are to be used in this session.
            ApplyTheme.ApplyTheme.LoadFonts(LoaderPaths.GetModLoaderThemeDirectory() + "\\" + _themeDirectory + "\\Fonts");

            // Load the images for the theme.
            ApplyTheme.ApplyTheme.LoadImages(LoaderPaths.GetModLoaderThemeDirectory() + "\\" + _themeDirectory + "\\Images");

            // Apply the theme.
            ApplyTheme.ApplyTheme.ApplyCurrentTheme();
        }
    }
}
