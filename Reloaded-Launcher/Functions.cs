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
using Reloaded.IO;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded_Plugin_System;

namespace ReloadedLauncher
{
    /// <summary>
    /// The <see cref="Functions"/> class contains global, shared functions used between the 
    /// individual Windows Forms of the Reloaded Mod Loader Launcher.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Kills the mod loader safely.
        /// </summary>
        public static void Shutdown()
        {
            // Shut the program.
            Environment.Exit(0);
        }

        /// <summary>
        /// Shuts down the Reloaded Launcher and launches the currently selected game.
        /// The parameter of this method allows for the specification OPTIONAL ADDITIONAL arguments such as --log or --attach.
        /// </summary>
        /// <param name="extraArguments">Additional command line arguments/options to pass to Reloaded-Loader.</param>
        public static void LaunchLoader(string[] extraArguments)
        {
            // Gets a file path of the loader.
            string filePath = GetLoaderFilePath();
            
            // Start process
            ReloadedProcess process = new ReloadedProcess(filePath, GetCommandlineParameters(extraArguments).ToArray());
            process.ResumeAllThreads();

            // Self-shutdown.
            if (Global.LoaderConfiguration.ExitAfterLaunch)
                Shutdown();
        }

        /// <summary>
        /// Gets the file path of Reloaded's Loader executable.
        /// </summary>
        public static string GetLoaderFilePath()
        {
            return $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Reloaded-Loader.exe";
        }

        /// <summary>
        /// Retrieves the commandline parameters to launch the currently selected global process in <see cref="Global.CurrentGameConfig"/>
        /// </summary>
        /// <param name="extraArguments">Additional command line arguments/options to pass to Reloaded-Loader.</param>
        public static List<string> GetCommandlineParameters(string[] extraArguments)
        {
            // Build arguments.
            List<string> argumentsList = new List<string>()
            {
                $"\"{Strings.Common.LoaderSettingConfig}\"",
                $"\"{Global.CurrentGameConfig.ConfigLocation}\""
            };

            // We are done here.
            argumentsList.AddRange(extraArguments);

            foreach (var launcherEventPlugin in PluginLoader.LauncherEventPlugins)
                argumentsList = launcherEventPlugin.SetLoaderParameters(argumentsList);

            return argumentsList;
        }

        /// <summary>
        /// Generates a Reloaded Steam shim for the currently selected game.
        /// </summary>
        public static void GenerateShim()
        {
            // Get file names and paths.
            string shimExecutableName = Path.GetFileName(Global.CurrentGameConfig.ExecutableLocation);
            string shimOutputLocation = Path.GetTempPath() + "\\Reloaded-Temp-Steam-Shim";

            // Generate instructions.
            string instructions = $"Using the pseudo-launcher as a game's basic launcher replacement (for some games that can only be launched via launcher):\r\n" +
                                  $"1. Rename the pseudo-launcher executable name \"{shimExecutableName}\" to the name of the game's default launcher (e.g. SteamLauncher.exe)\r\n" +
                                  $"2. Replace the game's launcher executable with the shim executable.\r\n" +
                                  $"3. Create (if you haven't already), individual game profiles for individual executables the default launcher would launch e.g. Shenmue.exe, Shenmue2.exe\r\n" +
                                  "Result: If done correctly, when launching the game, you will get a prompt asking\r\n" +
                                  "which game to launch via Reloaded if there is more than 1 game. Else the only game will be launched directly.\r\n\r\n" +
                                  $"-----------------------------------------------------------------------------------------\r\n\r\n" +
                                  $"Using the pseudo-launcher to trick Steam API:\r\n" +
                                  $"1. Rename {Global.CurrentGameConfig.ExecutableLocation} to a different name e.g. {Global.CurrentGameConfig.ExecutableLocation.Replace(".exe", "-Reloaded.exe")}\r\n" +
                                  $"2. Set the new executable path for the game to the changed path, e.g. {Global.CurrentGameConfig.ExecutableLocation.Replace(".exe", "-Reloaded.exe")} in Reloaded-Launcher.\r\n" +
                                  $"3. Copy Reloaded's Pseudo-launcher \"{shimExecutableName}\" to the place of the old executable {Global.CurrentGameConfig.ExecutableLocation}\r\n" +
                                  $"Result: You can now launch games directly through Steam and Reloaded mods still are applied.\r\n\r\n" +

                                  "With Steam games, note that after Steam updates, or verifying game files you may have to redo this process.\r\n" +
                                  "For more information refer to Reloaded's Github readme pages/wiki.\r\n" +
                                  "You should delete this folder when you are done.";

            // Output everything to disc.
            Directory.CreateDirectory(shimOutputLocation);
            File.WriteAllText($"{shimOutputLocation}\\Instructions.txt", instructions);
            RelativePaths.CopyByRelativePath($"{LoaderPaths.GetTemplatesDirectory()}\\Steam-Shim", shimOutputLocation, RelativePaths.FileCopyMethod.Copy, true);

            if (File.Exists($"{shimOutputLocation}\\{shimExecutableName}")) { File.Delete($"{shimOutputLocation}\\{shimExecutableName}"); }
            File.Move($"{shimOutputLocation}\\Reloaded-Steam-Shim.exe", $"{shimOutputLocation}\\{shimExecutableName}");
            
            // Open directory in explorer.
            Process.Start($"{shimOutputLocation}");
        }
    }
}
