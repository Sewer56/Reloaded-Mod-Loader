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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Reloaded.IO;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows;
using ReloadedLauncher.Windows.Children.Dialogs;
using ReloadedLauncher.Windows.Children.Dialogs.Tutorial;
using Reloaded_Plugin_System;
using Reloaded_Plugin_System.Config;
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
#if DEBUG
            Debugger.Launch();  
#endif

            // Unpack default files if not available.
            CopyDefaultFiles();

            // Run OnLaunch of plugins.
            foreach (var launcherEventPlugin in PluginLoader.LauncherEventPlugins)
                launcherEventPlugin.OnLaunch();

            #if DEBUG
            GenerateConfigTemplates();
            #endif

            // Initialize the Configs.
            InitializeGlobalProperties();

            // Check for updates.
            DoSquirrelStuff();
            CheckForModUpdates();

            // Checks if this is a Reloaded Protocol download.
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
            string sourceDirectoryDefaultPlugins    = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Default-Plugins";
            string targetDirectoryDefaulPlugins     = LoaderPaths.GetPluginsDirectory();

            string sourceDirectoryDefaultMods = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Default-Mods";
            string targetDirectoryDefaultMods = LoaderPaths.GetGlobalModDirectory();
            
            // Files
            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectory, targetDirectory, RelativePaths.FileCopyMethod.Copy, false, true);
                Directory.Delete(sourceDirectory, true);
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }

            // Mods
            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectoryDefaultMods, targetDirectoryDefaultMods, RelativePaths.FileCopyMethod.Copy, true, true);

                // We want to avoid deleting symbols.
                #if DEBUG
                    // Do nothing.
                #else
                // Delete default mods directory.
                Directory.Delete(sourceDirectoryDefaultMods, true);
                #endif
            }
            catch (Exception)
            { /* ¯\_(ツ)_/¯ */ }

            // Plugins
            try
            {
                RelativePaths.CopyByRelativePath(sourceDirectoryDefaultPlugins, targetDirectoryDefaulPlugins, RelativePaths.FileCopyMethod.Copy, true, true);

                #if DEBUG
                    // Do nothing.
                #else
                // Delete default plugins directory.
                Directory.Delete(sourceDirectoryDefaultPlugins, true);
                #endif
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
            Global.LoaderConfiguration = LoaderConfig.ParseConfig();

            // Set the initial menu name.
            Global.CurrentMenuName = "Main Menu";
        }

        /// <summary>
        /// Initializes all of the windows forms to be used by the application.
        /// </summary>
        private void InitializeForms()
        {
            // Display the Welcome Message.
            if (Global.LoaderConfiguration.FirstLaunch)
            {
                // Display the welcome dialog.
                new WelcomeScreen().ShowDialog();

                // Save without prompt.
                Global.LoaderConfiguration.FirstLaunch = false;
                LoaderConfig.WriteConfig(Global.LoaderConfiguration);
            }

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
                        onInitialInstall: v =>
                        {
                            tempUpdateManager.CreateShortcutForThisExe();
                            GenerateConfigTemplates();
                        },
                        onAppUpdate: v =>
                        {
                            tempUpdateManager.CreateShortcutForThisExe();
                            GenerateConfigTemplates();
                        },
                        onAppUninstall: v =>
                        {
                            tempUpdateManager.RemoveShortcutForThisExe();
                            // TODO: Ask user should all data for Reloaded be deleted.
                        });
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
            var classesSubKey = Registry.CurrentUser.OpenSubKey("Software", true)?.OpenSubKey("Classes", true);

            // Add a Reloaded Key.
            RegistryKey reloadedProtocolKey = classesSubKey?.CreateSubKey($"{Strings.Launcher.ReloadedProtocolName}");
            reloadedProtocolKey?.SetValue("", $"URL:{Strings.Launcher.ReloadedProtocolName}");
            reloadedProtocolKey?.SetValue("URL Protocol", "");
            reloadedProtocolKey?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{Assembly.GetExecutingAssembly().Location}\" --download %1");
        }

        /// <summary>
        /// Parses the commandline arguments and checks whether the second argument is a download link.
        /// </summary>
        /// <param name="arguments">The individual commandline arguments.</param>
        private static void HandleDownloads(string[] arguments)
        {
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
                else if (arguments[0] == Strings.Launcher.LaunchArgumentName)
                {
                    // Gets the game config specified and launches the launcher.
                    Global.CurrentGameConfig = GameConfig.ParseConfig(arguments[1]);

                    // Get arguments and launch, then say goodbye.
                    string[] additionalArguments = new string[0];
                    if (arguments.Length > 2)
                        additionalArguments = Slice(arguments, 2, arguments.Length);

                    Functions.LaunchLoader(additionalArguments);
                    Functions.Shutdown();
                }
            }
        }

        private static void GenerateConfigTemplates()
        {
            // Get path to our folder containing the template set of configs.
            string templateDirectory = LoaderPaths.GetModLoaderConfigDirectory() + "\\Config Templates";
            Directory.CreateDirectory(templateDirectory);

            // Write all configs.
            ModConfig.WriteConfig(new ModConfig() { ModLocation = $"{templateDirectory}\\Mod-Config.json" });
            GameConfig.WriteConfig(new GameConfig() { ConfigLocation = $"{templateDirectory}\\Game-Config.json" });
            ThemeConfig.WriteConfig(new ThemeConfig() { ThemeLocation = $"{templateDirectory}\\Theme-Config.json" });
            PluginConfig.WriteConfig(new PluginConfig() { PluginConfigLocation = $"{templateDirectory}\\Plugin-Config.json" } );
        }

        private static async void CheckForModUpdates()
        {
            var updates = await UpdateChecker.GetAllUpdatesFromSources();

            if (updates.Count > 0)
            {
                DownloadModUpdatesDialog dialog = new DownloadModUpdatesDialog(updates);
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// Inclusive for start index, exclusive for end index.
        /// </summary>
        private static T[] Slice<T>(T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }
    }
}
