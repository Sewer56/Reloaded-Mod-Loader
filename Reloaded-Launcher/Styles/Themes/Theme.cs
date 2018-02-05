using Reloaded.Misc;
using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;
using static Reloaded.Misc.Config.ThemePropertyParser;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Defines a theme entry for each of the individual themes.
    /// Stores all of the a theme's fonts, colours as well as other constants.
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// Stores all of the currently loaded in fonts for this theme in particular.
        /// </summary>
        public static ThemeFonts Fonts { get; set; }

        /// <summary>
        /// Retrieves the general theme configuration for the current theme.
        /// </summary>
        public static ThemeConfig ThemeProperties { get; set; }

        /// <summary>
        /// Changes the directory for the theme to be used.
        /// After setting, the current theme is automatically changed/loaded.
        /// </summary>
        public string ThemeDirectory
        {
            get { return themeDirectory; }
            set
            {
                themeDirectory = value;
                LoadTheme();
            }
        }

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
