using SonicHeroes.Misc;
using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;
using static SonicHeroes.Misc.Config.ThemePropertyParser;

namespace HeroesModLoaderConfig.Styles.Themes
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
        /// Prevents theming from being performed on different threads.
        /// </summary>
        private Object threadLock = new Object();

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
        /// This method specifically places intact the necessary safety procedures,
        /// the actual theming is done in LoadThemeInternal()
        /// </summary>
        public void LoadTheme()
        {
            // Lock to prevent multiple thread access.
            lock (threadLock)
            {
                // Collect Garbage
                GC.Collect();

                // Change the Garbage Collector Latency Mode to prevent Garbage Collection
                // during theme change.
                GCLatencyMode oldMode = GCSettings.LatencyMode;

                // Make sure we can always go to the catch block, 
                // so we can set the latency mode back to `oldMode`
                RuntimeHelpers.PrepareConstrainedRegions();

                try
                {
                    GCSettings.LatencyMode = GCLatencyMode.LowLatency;

                    // Generation 2 garbage collection is now
                    // deferred, except in extremely low-memory situations

                    // Change the theme.
                    LoadThemeInternal();
                }
                finally
                {
                    // ALWAYS set the latency mode back
                    GCSettings.LatencyMode = oldMode;
                }
            }
        }
        /// <summary>
        /// Loads the theme set at the current directory.
        /// </summary>
        private void LoadThemeInternal()
        {
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
