using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded.Process.Native;
using Reloaded.Utilities.PE;

namespace Reloaded_Steam_Shim
{
    public static class Functions
    {
        /// <summary>
        /// Launches Reloaded's Launcher with the Steam flag set for a specific game.
        /// </summary>
        /// <param name="gameConfigDirectory"></param>
        public static void LaunchGame(string gameConfigDirectory)
        {
            // Get launcher location and arguments.
            string reloadedLauncherLocation = GetLauncherLocation();

            var arguments = new List<string>()
            {
               $"{Strings.Launcher.LaunchArgumentName}",
               $"{gameConfigDirectory}",
               $"{Strings.Common.LoaderSettingNoReattach}"
            };

            // Start Reloaded-Launcher.
            DateTime currentTime = DateTime.Now;
            ReloadedProcess process = new ReloadedProcess(reloadedLauncherLocation, arguments.ToArray());
            process.ResumeAllThreads();

            // Synchronize exit of this application with Reloaded-Loader.
            bool is64Bit = IsGameConfig64Bit(gameConfigDirectory);
            FindLoaderAndSynchronizeExit(is64Bit, currentTime);
        }

        /// <summary>
        /// Returns true if the game/process behind the game config is 64bit.
        /// </summary>
        private static bool IsGameConfig64Bit(string gameConfigDirectory)
        {
            // Get gameconfig and obtain executable of architecture of game.
            GameConfig gameConfig = GameConfig.ParseConfig(gameConfigDirectory);
            string gameExecutableLocation = gameConfig.GameDirectory + $"\\{gameConfig.ExecutableLocation}";

            // Get whether our executable is 64bit, then wait out for Reloaded-Loader with the same 
            // architecture.
            return Executable.GetMachineType(gameExecutableLocation) == Executable.PEMachineType.AMD64;
        }

        /// <summary>
        /// Finds a running instance of Reloaded-Loader with same 64/32bit architecture and synchronize exit/quit.
        /// </summary>
        private static void FindLoaderAndSynchronizeExit(bool is64Bit, DateTime processNewerThan)
        {
            int attempts            = 0;
            int attemptLimit        = 20;
            int attemptRetryTime    = 666;

            while (true)
            {
                var x86processes = Process.GetProcessesByName("Reloaded-Loader");
                var x64processes = Process.GetProcessesByName("Reloaded-Wrapper-x64");

                // Group all arch processes onto single list.
                List<Process> processes = new List<Process>();
                processes.AddRange(x86processes);
                processes.AddRange(x64processes);

                // Find Reloaded-Loader with the same architecture.
                foreach (var process in processes)
                {
                    try
                    {
                        // We probably found what we want if this is true.
                        if (IsProcess64Bit(process) == is64Bit && process.StartTime > processNewerThan)
                        {
                            process.EnableRaisingEvents = true;
                            process.Exited += (sender, args) => { Environment.Exit(0); };

                            // Our shim's exit time is synchronized, we're good here.
                            while (true)
                                Thread.Sleep(66666);
                        }
                    }
                    catch
                    {
                        /* Process died due to restart to X64 wrapper, ignore this. */
                    }

                }

                // Loop and check if attempt limit reached.
                attempts++;
                if (attempts > attemptLimit)
                {
                    MessageBox.Show("Reloaded's Loader Not Found Running. Did things crash? Exiting.");
                    Environment.Exit(0);
                }

                Thread.Sleep(attemptRetryTime);
            }

        }

        /// <summary>
        /// Retrieves the location of Reloaded's launcher.
        /// </summary>
        private static string GetLauncherLocation()
        {
            // Launcher is Squirrel Installed.
            if (File.Exists(LoaderPaths.ReloadedLauncherLocation))
            {
                return LoaderPaths.ReloadedLauncherLocation;
            }
            else
            {
                // Not squirrel installed, user must specify themselves.
                var shimSettings = ReloadedShimSettings.GetShim();

                // Check if file previously specified by user.
                if (! String.IsNullOrEmpty(shimSettings.LauncherLocation))
                    if (File.Exists(shimSettings.LauncherLocation))
                        return shimSettings.LauncherLocation;

                // Get file location from user.
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                MessageBox.Show("Reloaded-Launcher not found at default location; please specify the location manually.");
                openFileDialog.Title        = "Select Reloaded-Launcher.exe location";
                openFileDialog.Multiselect  = false;

                // Keep opening dialog until not OK
                while (openFileDialog.ShowDialog() != DialogResult.OK)
                { }

                string fileName = openFileDialog.FileName;
                shimSettings.LauncherLocation = openFileDialog.FileName;
                shimSettings.SaveShim();
                
                openFileDialog.Dispose();
                return fileName;
            }
        }
        
        private static bool IsProcess64Bit(Process reloadedProcess)
        {
            // Check if Process is x86.
            Native.IsWow64Process(reloadedProcess.Handle, out bool isActually32Bit);
            return !isActually32Bit;
        }
    }
}
