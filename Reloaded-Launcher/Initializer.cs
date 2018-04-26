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

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.IO;
using Reloaded.IO.Config;
using Reloaded.IO.Config.Loader;
using Reloaded.Utilities;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows;
using Squirrel;

namespace ReloadedLauncher
{
    /// <summary>
    /// Defines the class that sets up and starts off the mod loader config manager.
    /// The actual real entry point for the application is Program.cs, this is merely an abstraction.
    /// </summary>
    internal class Initializer
    {
        /// <summary>
        /// Initializes the Windows Forms Application.
        /// </summary>
        public Initializer()
        {
            // Start self-update.
            DoSquirrelStuff();

            // Unpack default files if not available.
            CopyDefaultFiles();

            // Initialize the Configs.
            InitializeGlobalProperties();
            
            // Initialize all WinForms.
            InitializeForms();
        }

        /// <summary>
        /// Copies the default Mod Loader configuration and theme files upon first launch.
        /// </summary>
        private void CopyDefaultFiles()
        {
            // Copy without replacement.
            // Source directory = App Directory
            string sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Files";
            string targetDirectory = LoaderPaths.GetModLoaderDirectory();

            // Copy without replacement.
            // Source directory = App Directory
            string sourceDirectoryDefaultMods = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Default-Mods";
            string targetDirectoryDefaultMods = LoaderPaths.GetGlobalModDirectory();

            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectory, targetDirectory, RelativePaths.FileCopyMethod.Copy, false); 
                RelativePaths.CopyByRelativePath(sourceDirectoryDefaultMods, targetDirectoryDefaultMods, RelativePaths.FileCopyMethod.Copy, true);
            
                // Nuke remaining files.
                Directory.Delete(sourceDirectory, true);
                Directory.Delete(sourceDirectoryDefaultMods, true);
            }
            catch ( Exception )
            {
                /* ¯\_(ツ)_/¯ */
            }
        }

        /// <summary>
        /// Loads all configurations to be used by the program, alongside with their parsers.
        /// </summary>
        private void InitializeGlobalProperties()
        {
            // Instantiate the Global Config Manager
            Global.ConfigurationManager = new ConfigManager();

            // Grab relevant configs.
            // Note: Game list is grabbed upon entry to the main screen form.
            Global.LoaderConfiguration = LoaderConfigParser.ParseConfig();

            // Initialize other Properties.
            Global.Theme = new Theme();

            // Set the initial menu name.
            Global.CurrentMenuName = "Main Menu";
        }

        /// <summary>
        /// Initializes all of the windows forms to be used by the application.
        /// </summary>
        private void InitializeForms()
        {
            // Store the base form in Global
            Global.BaseForm = new Base();

            // Launches the first window.
            Application.Run(Global.BaseForm);
        }

        /// <summary>
        /// Does stuff related to Squirrel.Windows, such as beginning a self-update in the background.
        /// Starts the self-update process, updating the application to the newest version in the background.
        /// </summary>
        private static async void DoSquirrelStuff()
        {
            try
            {
                // Use regular update manager in case there is no found commit on Github
                using (var tempUpdateManager = new UpdateManager(""))
                {
                    // Handle application installs, uninstalls.
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => tempUpdateManager.CreateShortcutForThisExe(),
                        onAppUpdate: v => tempUpdateManager.CreateShortcutForThisExe(),
                        onAppUninstall: v => tempUpdateManager.RemoveShortcutForThisExe()
                    );
                }

                // Update from Github
                using (var updateManager = UpdateManager.GitHubUpdateManager("https://github.com/sewer56lol/Reloaded-Mod-Loader"))
                {
                    // Update manager for Github
                    UpdateManager githubUpdateManager = updateManager.Result;

                    // Check for release info.
                    UpdateInfo update = await githubUpdateManager.CheckForUpdate(false);

                    // Update if there are any releases.
                    if (update.ReleasesToApply.Count > 0)
                    { await githubUpdateManager.UpdateApp(); }
                }
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }
        }
    }
}
