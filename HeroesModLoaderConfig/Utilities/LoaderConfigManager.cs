using SonicHeroes.Misc.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Utilities
{
    /// <summary>
    /// Stores all of the currently loaded configurator parsers for the mod loader configuration manager.
    /// </summary>
    class LoaderConfigManager
    {
        /// <summary>
        /// Stores the Mod Loader Configuration Parser.
        /// </summary>
        public LoaderConfigParser LoaderConfigParser { get; set; }

        /// <summary>
        /// Starts up all of the individual parsers.
        /// </summary>
        public LoaderConfigManager()
        {
            // Instantiate the Parsers
            LoaderConfigParser = new LoaderConfigParser();
        }
    }
}
