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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Reloaded;
using Reloaded.IO;
using Reloaded.IO.Config.Loader;
using Reloaded.Utilities;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows;
using ReloadedLauncher.Windows.Children.Dialogs;
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
        public Initializer(string[] arguments)
        {
            // Initialize the Configs.
            InitializeGlobalProperties();

            // Unpack default files if not available.
            CopyDefaultFiles();

            // Start self-update.
            DoSquirrelStuff();

            // Checks if the is a Reloaded Protocol download.
            HandleDownloads(arguments);

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

            // Files
            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectory, targetDirectory, RelativePaths.FileCopyMethod.Copy, false);
                Directory.Delete(sourceDirectory, true);
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }

            // Mods
            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectoryDefaultMods, targetDirectoryDefaultMods, RelativePaths.FileCopyMethod.Copy, true);

                // Don't delete debug symbols at their path if in debug mode.
                #if DEBUG
                return;
                #endif
                
                // Delete default mods directory.
                Directory.Delete(sourceDirectoryDefaultMods, true);
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }
        }

        /// <summary>
        /// Loads all configurations to be used by the program, alongside with their parsers.
        /// </summary>
        private void InitializeGlobalProperties()
        {
            // Initialize other Properties.
            Global.Theme = new Theme();

            // Grab relevant configs.
            // Note: Game list is grabbed upon entry to the main screen form.
            Global.LoaderConfiguration = LoaderConfigParser.ParseConfig();

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
                // Sets up the URL handler to point to reloaded, ran regardless of squirrel.
                SetupURLHandler();

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
                using (var updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/sewer56lol/Reloaded-Mod-Loader", 
                                           prerelease: Global.LoaderConfiguration.AllowBetaBuilds))
                {
                    // Check for release info.
                    UpdateInfo githubUpdateInfo = await updateManager.CheckForUpdate(false);

                    // Update if there are any releases.
                    if (githubUpdateInfo.ReleasesToApply.Count > 0)
                    {
                        // Insert handler here.
                        // Show dialog for updates if not silent.
                        // Take handler from here.
                        if (!Global.LoaderConfiguration.SilentUpdates)
                        {
                            // Open up the update dialog.
                            DownloadUpdatesDialog downloadUpdatesDialog = new DownloadUpdatesDialog(updateManager, ref githubUpdateInfo);
                            downloadUpdatesDialog.ShowDialog();

                            // Note: Restarting will THROW in the debugger, it appears that this behaviour is intended.
                            if (downloadUpdatesDialog.RestartApplication)
                                UpdateManager.RestartApp();
                            else
                                return;
                        }

                        // Else silent update
                        await updateManager.UpdateApp();
                    }
                        
                }
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }
        }

        /// <summary>
        /// Registers Reloaded Mod Loader as an URL handler for the reloaded:// protocol, allowing
        /// those links to be redirected to Reloaded for purposes such as GameBanana's 1 click mod installs.
        /// </summary>
        private static void SetupURLHandler()
        {
            // Get the user classes subkey.
            var classesSubKey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Classes", true);

            // Add a Reloaded Key.
            RegistryKey reloadedProtocolKey = classesSubKey.CreateSubKey($"{Strings.Launcher.ReloadedProtocolName}");
            reloadedProtocolKey.SetValue("", $"URL:{Strings.Launcher.ReloadedProtocolName}");
            reloadedProtocolKey.SetValue("URL Protocol", "");
            reloadedProtocolKey.CreateSubKey(@"shell\open\command").SetValue("", $"{Assembly.GetExecutingAssembly().Location} --download %1");
        }

        /// <summary>
        /// Parses the commandline arguments and checks whether the second argument is a download link.
        /// </summary>
        /// <param name="arguments">The individual commandline arguments.</param>
        private static void HandleDownloads(string[] arguments)
        {
            Debugger.Launch();

            // If args are available.
            if (arguments.Length > 0)
            {
                // Checks if we are downloading.
                if (arguments[0] == Strings.Launcher.DownloadArgumentName)
                {
                    // Gets the download path for the mod.
                    string httpPath = arguments[1];

                    // Summon dialog to download the individual game mod.
                    DownloadModDialog downloadModDialog = new DownloadModDialog(httpPath);
                    downloadModDialog.ShowDialog();
                    Environment.Exit(0);
                }
            }
        }
    }
}
