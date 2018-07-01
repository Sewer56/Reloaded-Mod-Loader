/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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
using LiteNetLib;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded_Loader.Core;
using Reloaded_Loader.Miscellaneous;
using Reloaded_Loader.Networking;
using Reloaded_Loader.Terminal;
using Reloaded_Loader.Terminal.Information;
using Squirrel;
using Console = Colorful.Console;

namespace Reloaded_Loader
{
    /// <summary>
    /// The main program class provides code only for the initialization of the Reloaded Mod Loader
    /// loader, the main brawl and logic code is provided elsewhere in the mod loader.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Stores the individual process of the game.
        /// Allows for DLL Injection, Memory Manipulation, etc.
        /// </summary>
        private static ReloadedProcess _gameProcess;

        /// <summary>
        /// Stores the game configuration to be used.
        /// </summary>
        private static GameConfig _gameConfig;

        /// <summary>
        /// Specifies the name of an already running executable to attach to.
        /// </summary>
        private static string _attachTargetName;

        /// <summary>
        /// Obtains the start time of the game process.
        /// Used to determine if a process has shut itself down and then since restarted.
        /// </summary>
        private static DateTime _processStartTime;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Find Assemblies Manually if necessary (Deprecate app.config)
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyFinder.CurrentDomain_AssemblyResolve;

            /* - Initialization - */
            DoSquirrelStuff();

            // Initialize the console.
            ConsoleFunctions.Initialize();

            // Setup libReloaded Debug Bindings
            SetuplibReloadedBindings();

            // Print startup information.
            Banner.PrintBanner();

            // Get Controller Order
            Controllers.PrintControllerOrder();

            // Unlock DLLs, Removes Zone Information which may prevent DLL injection if DLL was downloaded from e.g. Internet Explorer
            DllUnlocker.UnblockDlls();

            // Setup Server
            LoaderServer.SetupServer();

            /* - Option Parsing, Linking - */

            // Parse Arguments
            ParseArguments(args);

            // Start game
            InjectByMethod(args);

            /* - Load and enter main close polling loop - */

            // Add to exited event 
            _gameProcess.GetProcessFromReloadedProcess().EnableRaisingEvents = true;
            _gameProcess.GetProcessFromReloadedProcess().Exited += (sender, eventArgs) => ProcessSelfReattach();

            // Load modifications for the current game.
            ModLoader.LoadMods(_gameConfig, _gameProcess);

            // Resume game after injection if we are NOT in attach mode.
            if (_attachTargetName == null)
            { _gameProcess.ResumeAllThreads(); }
            
            // Stay alive in the background
            AppDomain.CurrentDomain.ProcessExit += Shutdown;
            Console.CancelKeyPress += Shutdown;

            // Sleep infinitely as much as it is necessary.
            while (true)
            { Console.ReadLine(); }
        }

        /// <summary>
        /// Checks whether to reattach Reloaded Mod Loader if the game process
        /// unexpectedly kills itself and then restarts (a standard for some games' launch procedures).
        /// </summary>
        private static void ProcessSelfReattach()
        {
            // Use stopwatch for soft timing.
            Stopwatch timeoutStopWatch = new Stopwatch();
            timeoutStopWatch.Start();

            // Set attach name for reattaching. 
            _attachTargetName = Path.GetFileNameWithoutExtension(_gameConfig.ExecutableLocation);

            // Spin trying to get game process until timeout.
            while (timeoutStopWatch.ElapsedMilliseconds < 6000)
            {
                // Grab current already running game.
                ReloadedProcess localGameProcess = ReloadedProcess.GetProcessByName(_attachTargetName);

                // Did we find the process? If not, try again.
                if (localGameProcess == null) continue;

                // Check if process is newer than our original game process, if it is, restart self.
                if (localGameProcess.GetProcessFromReloadedProcess().StartTime > _processStartTime)
                {                   
                    // Game restart
                    Bindings.PrintInfo($"// Game Restarted, Probably from Self-Kill e.g. Steam DRM, Reattaching! | {_attachTargetName}");
                    Bindings.PrintInfo($"// In other words, super early attach not supported! Maybe with manual map in the future.");
                    _gameProcess = localGameProcess;

                    // Disconnect all clients
                    foreach (NetPeer peer in LoaderServer.ReloadedServer.GetPeers())
                    { LoaderServer.ReloadedServer.DisconnectPeer(peer); }

                    // Re-subscribe to our event.
                    _gameProcess.GetProcessFromReloadedProcess().EnableRaisingEvents = true;
                    _gameProcess.GetProcessFromReloadedProcess().Exited += (sender, eventArgs) => ProcessSelfReattach();

                    // Reload our mods.
                    ModLoader.LoadMods(_gameConfig, _gameProcess);

                    // Sleep infinitely as much as it is necessary.
                    while (true)
                    { Console.ReadLine(); }
                }
            }

            // Kill the process itself.
            Shutdown(null, null);
        }

        /// <summary>
        /// Parses the arguments passed into the application.
        /// </summary>
        /// <param name="arguments"></param>
        private static void ParseArguments(string[] arguments)
        {
            // Go over known arguments.
            for (int x = 0; x < arguments.Length; x++)
            {
                if (arguments[x] == $"{Strings.Common.LoaderSettingConfig}") { _gameConfig = CheckConfigJson(arguments[x + 1]); }
                if (arguments[x] == $"{Strings.Common.LoaderSettingAttach}") { _attachTargetName = arguments[x+1]; }
                if (arguments[x] == $"{Strings.Common.LoaderSettingLog}") { Logger.Setup(arguments[x + 1]); }
            }

            // Check game config
            if (_gameConfig == null) { Banner.DisplayWarning(); }
        }

        /// <summary>
        /// Checks whether the supplied argument for the config path ends with Config.json and strips
        /// the argument if it does to obtain the game configuration from the parser.
        /// </summary>
        private static GameConfig CheckConfigJson(string argument)
        {
            // Accept the supplied config path if it ends on Config.json.
            if (argument.EndsWith(Strings.Parsers.ConfigFile))
            { return GameConfig.ParseConfig(Path.GetDirectoryName(argument)); }

            // Otherwise get the game config from the vanilla argument.
            return GameConfig.ParseConfig(argument);
        }

        /// <summary>
        /// Sets up bindings for libReloaded Print functions for printing any errors which happen within
        /// libReloaded.
        /// </summary>
        private static void SetuplibReloadedBindings()
        {
            Bindings.PrintError    += delegate (string message) { ConsoleFunctions.PrintMessageWithTime(message, ConsoleFunctions.PrintErrorMessage); };
            Bindings.PrintWarning  += delegate (string message) { ConsoleFunctions.PrintMessageWithTime(message, ConsoleFunctions.PrintWarningMessage); };
            Bindings.PrintInfo     += delegate (string message) { ConsoleFunctions.PrintMessageWithTime(message, ConsoleFunctions.PrintInfoMessage); };
            Bindings.PrintText     += delegate (string message) { ConsoleFunctions.PrintMessageWithTime(message, ConsoleFunctions.PrintMessage); };
        }

        /// <summary>
        /// Injects itself to the game depending on the individual method chosen either from the commandline or in the launcher.
        /// By default, Reloaded starts the game and injects immediately into suspended, otherwise if --attach is specified, Reloaded
        /// tries to hook itself into an already running game/application.
        /// </summary>
        /// <param name="arguments">A copy of the arguments passed into the application used for optionally rebooting in x64 mode.</param>
        private static void InjectByMethod(string[] arguments)
        {
            // Fast return if soft reboot (game process killed and restarted itself)
            if (_gameProcess != null)
                return;

            // Attach if specified by the user.
            if (_attachTargetName != null)
            {
                // Grab current already running game.
                _gameProcess = ReloadedProcess.GetProcessByName(_attachTargetName);

                // Check if gameProcess successfully returned.
                if (_gameProcess == null)
                {
                    ConsoleFunctions.PrintMessageWithTime("Error: An active running game instance was not found.", ConsoleFunctions.PrintErrorMessage);
                    Console.ReadLine();
                    Shutdown(null, null);
                }

                // Check if the current running architecture matched ~(32 bit), if not, restart as x64.
                if (!_gameProcess.CheckArchitectureMatch())
                {
                    RebootX64(arguments);
                }
            }

            // Otherwise start process suspended in Reloaded, hook it, exploit it and resume the intended way.
            else
            {
                _gameProcess = new ReloadedProcess($"{Path.Combine(_gameConfig.GameDirectory, _gameConfig.ExecutableLocation)}");

                // The process handle is 0 if the process failed to initialize.
                if ((int)_gameProcess.ProcessHandle == 0)
                {
                    // libReloaded will already print the error message.
                    Console.ReadLine();
                    Shutdown(null, null);
                }

                // Check if the current running architecture matched ~(32 bit), if not, restart as x64.
                if (!_gameProcess.CheckArchitectureMatch())
                {
                    _gameProcess.KillProcess();
                    RebootX64(arguments);
                }
            }

            // Set binding for target process for memory IO
            Bindings.TargetProcess = _gameProcess;

            // Obtain the process start time.
            if (_gameProcess != null)
                _processStartTime = _gameProcess.GetProcessFromReloadedProcess().StartTime;
        }

        /// <summary>
        /// Shuts down the application when the user presses CTRL+C.
        /// </summary>
        private static void Shutdown(object sender, EventArgs e)
        {
            // Kill self.
            Environment.Exit(0);
        }

        /// <summary>
        /// Reboots the Reloaded Loader in x64 mode by running
        /// the x64 wrapper.
        /// </summary>
        /// <param name="arguments">A copy of the arguments originally passed to the starting application.</param>
        private static void RebootX64(string[] arguments)
        {
            // Add quotes to each argument in quotes.
            for (int x = 0; x < arguments.Length; x++)
            { arguments[x] = $"\"{arguments[x]}\""; }

            ReloadedProcess reloadedProcess = new ReloadedProcess($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Reloaded-Wrapper-x64.exe", arguments);
            reloadedProcess.ResumeAllThreads();

            // Bye Bye Current Process
            Shutdown(null, null);
        }

        /// <summary>
        /// Shuts itself down upon being started by Squirrel.Windows over an update event.
        /// </summary>
        private static void DoSquirrelStuff()
        {
            try
            {
                // Note, in most of these scenarios, the app exits after this method
                // completes!
                SquirrelAwareApp.HandleEvents(
                    onInitialInstall: v => Environment.Exit(0),
                    onAppUpdate: v => Environment.Exit(0),
                    onFirstRun: () => Environment.Exit(0));
            }
            catch (Exception e)
            { }

        }
    }
}
