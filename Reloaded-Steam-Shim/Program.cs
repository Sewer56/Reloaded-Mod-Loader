using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.IO.Config;

namespace Reloaded_Steam_Shim
{
    static class Program
    {
        /* Stores a list of games under the same folder as the current shim folder. */
        public static List<GameConfig> GameConfigurations = new List<GameConfig>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RetrieveConfigs();

            // Read shim settings.
            var shimSettings = ReloadedShimSettings.GetShim();

            // If there are multiple games, let the user select one.
            if (GameConfigurations.Count > 1)
            {
                // Auto-load game if default set, else ask user.
                if (!String.IsNullOrEmpty(shimSettings.LoadByDefault))
                {
                    Functions.LaunchGame(shimSettings.LoadByDefault);
                }
                else
                {
                    Application.Run(new GameSelector(GameConfigurations, shimSettings));
                }
            }
            else
            {
                // Single game, launch it.
                Functions.LaunchGame(Path.GetDirectoryName(GameConfigurations[0].ConfigLocation));
            }
        }

        /// <summary>
        /// Retrieves the game configurations which have a folder the current executable path contains.
        /// </summary>
        static void RetrieveConfigs()
        {
            // Filter all game configurations down to the configurations which contain a part of the current
            // folder path.

            var gameConfigurations      = ConfigManager.GetAllGameConfigs();
            string currentDirectory     = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            currentDirectory            = Path.GetFullPath(currentDirectory);
            foreach (var gameConfiguration in gameConfigurations)
            {
                string gameFullDirectory = Path.GetFullPath(gameConfiguration.GameDirectory);
                if (currentDirectory.Contains(gameFullDirectory))
                {
                    GameConfigurations.Add(gameConfiguration);
                }
            }
        }
    }
}
