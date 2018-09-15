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
            #if DEBUG
            Debugger.Launch();
            #endif

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

            if (GameConfigurations.Count < 1)
            {
                MessageBox.Show("No game profiles found pointing to either the current directory or any of the subfolders.\n" +
                                "Please ensure you have your game profiles correctly set up.\n" +
                                "Refer to the readme pages on Github for more information.");
                Environment.Exit(0);
            }

            // Single game, launch it.
            Functions.LaunchGame(Path.GetDirectoryName(GameConfigurations[0].ConfigLocation));
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

            // Grab all valid configs.
            gameConfigurations          = gameConfigurations.Where(x => x.ModDirectory != "!Global" && x.ExecutableLocation != "Undefined" && x.GameDirectory != "Undefined").ToList();
            
            foreach (var gameConfiguration in gameConfigurations)
            {
                // Get all game profiles with executables in subdirectory or parent directory of current executable.
                string gameFullDirectory = "!NotARealDirectory.xxx";
                try  { gameFullDirectory = Path.GetFullPath(gameConfiguration.GameDirectory); }
                catch(Exception ex)
                {
                    MessageBox.Show($"You screwed up somewhere with one of your game profiles. Fix your profiles, dummy: {ex.Message}");
                    continue;
                }
                
                if (gameFullDirectory == currentDirectory || gameFullDirectory.Contains(currentDirectory) || currentDirectory.Contains(gameFullDirectory))
                {
                    GameConfigurations.Add(gameConfiguration);
                }
            }
        }
    }
}
