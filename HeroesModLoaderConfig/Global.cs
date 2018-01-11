using HeroesModLoaderConfig.Styles.Themes;
using SonicHeroes.Misc.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig
{
    /// <summary>
    /// Provides access to global application objects, settings and other common and/or shared variables.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Stores the current theme of the application.
        /// </summary>
        public static Theme Theme { get; set; }

        /// <summary>
        /// Defines the name of the menu that the user is currently in.
        /// Used for correctly setting the titlebar title.
        /// </summary>
        public static string CurrentMenuName { get; set; }

        /// <summary>
        /// Stores a list of all instantiated windows forms. 
        /// Used for setting themes on each form alongside other misc purposes.
        /// </summary>
        public static List<Form> WindowsForms { get; set; }

        /// <summary>
        /// Stores the base form, which contains all of the child forms.
        /// </summary>
        public static Base BaseForm { get; set; }

        /// <summary>
        /// Stores the current configuration for the mod loader.
        /// </summary>
        public static LoaderConfigParser.Config LoaderConfiguration { get; set; }
    }
}
