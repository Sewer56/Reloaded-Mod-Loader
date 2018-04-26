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


using Reloaded.IO.Config.Themes;
using Reloaded.Utilities;

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
        public static ThemePropertyParser.ThemeConfig ThemeProperties { get; set; }

        /// <summary>
        /// Changes the directory for the theme to be used.
        /// After setting, the current theme is automatically changed/loaded.
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
<<<<<<< refs/remotes/origin/master
=======

        /// <summary>
        /// Verifies whether the theme about to be loaded exists.
        /// If the theme to be loaded does not exist, it tries another.
        /// If no themes exist, Reloaded-GUI unpacks the defaults from its own resources and runs it.
        /// </summary>
        private void PerformSafetyChecks()
        {
            // Directory storing mod loader themes.
            string themesDirectory = LoaderPaths.GetModLoaderThemeDirectory();

            // Check if requested theme exists (if so, exit)
            if (Directory.Exists(themesDirectory + Path.DirectorySeparatorChar + _themeDirectory)) { return; }

            // Extract default theme if current theme does not exist
            ExtractDefaultThemes();

            // Check if missing theme was default (if so, exit)
            // Just in case default theme ever changes.
            if (Directory.Exists(themesDirectory + Path.DirectorySeparatorChar + _themeDirectory)) { return; }

            // Else override and try load default theme if exists.
            string[] directories = Directory.GetDirectories(themesDirectory);
            _themeDirectory = Path.GetFileNameWithoutExtension(directories[0]);
        }

        /// <summary>
        /// Performs a check on the presence of default themes and extracts the 
        /// default application themes to Reloaded's Themes directory. 
        /// If you are using Reloaded-GUI, you should try to call this on the first time that the user
        /// opens your application (first-run). 
        /// DO NOT ASSUME THAT THEY ALREADY USE RELOADED MOD LOADER.
        /// </summary>
        public static void ExtractDefaultThemes()
        {
            // Directory storing mod loader themes.
            string themesDirectory = LoaderPaths.GetModLoaderThemeDirectory();

            // Check if any theme is available.
            // If there are no themes unavailable, unpack default and select first.
            if ( Directory.GetDirectories( themesDirectory ).Length <= 0 )
            {
                // Get embedded resource names.
                string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                string resourceName = resources.First(x => x.Contains("DefaultThemes.7z"));

                // Unpack Default Themes
                Stream defaultThemeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream( resourceName );
                using (ArchiveFile archiveFile = new ArchiveFile(defaultThemeStream))
                {
                    archiveFile.Extract(themesDirectory);
                }
            }
        }
>>>>>>> Fix a bunch of warnings
    }
}
