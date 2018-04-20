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
using System.Threading;
using Reloaded;
using Reloaded.IO.Config.Games;
using Reloaded.Process;
using Reloaded.Utilities;
using Reloaded_Loader.Core;
using Reloaded_Loader.Miscallenous;
using Reloaded_Loader.Networking;
using Reloaded_Loader.Terminal;
using Reloaded_Loader.Terminal.Information;
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
        private static GameConfigParser.GameConfig _gameConfig;

        /// <summary>
        /// Specifies the name of an already running executable to attach to.
        /// </summary>
        private static string _attachTargetName;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Find Assemblies Manually if necessary (Deprecate app.config)
            AppDomain.CurrentDomain.AssemblyResolve += Miscallenous.AssemblyFinder.CurrentDomain_AssemblyResolve;

            /* - Initialization - */

            // Initialize the console.
            ConsoleFunctions.Initialize();

            // Print startup information.
            Banner.PrintBanner();

            // Get Controller Order
            Controllers.PrintControllerOrder();

            // Unlock DLLs, Removes Zone Information which may prevent DLL injection if DLL was downloaded from e.g. Internet Explorer
            DllUnlocker.UnblockDlls();

            // Setup Server
            LoaderServer.SetupServer();

            // Setup libReloaded Debug Bindings
            SetuplibReloadedBindings();

            /* - Option Parsing, Linking - */

            // Parse Arguments
            ParseArguments(args);

            // Start game
            InjectByMethod(args);

            /* - Load and enter main close polling loop - */

            // Load modifications for the current game.
            ModLoader.LoadMods(_gameConfig, _gameProcess);

            // Stay alive in the background
            AppDomain.CurrentDomain.ProcessExit += Shutdown;
            Console.CancelKeyPress += Shutdown;

            // Poll every 3 seconds, kill self if child dies.
            while (true)
            {
                try
                {
                    // Get Process
                    Process localGameProcess = _gameProcess.GetProcessFromReloadedProcess();

                    // Check if process has exited.
                    if (localGameProcess.HasExited) { Shutdown(null, null); }

                    // Sleep
                    Thread.Sleep(2000);
                }
                // Argument of Process.GetProcessById fails inside GetProcessFromReloadedProcess
                // The process died.
                catch (ArgumentException) { Shutdown(null, null); }
            }
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
                if (arguments[x] == "--config") { _gameConfig = new GameConfigParser().ParseConfig(arguments[x + 1]); }
                if (arguments[x] == "--attach") { _attachTargetName = arguments[x+1]; }
                if (arguments[x] == "--log") { Logger.Setup(arguments[x + 1]); }
            }

            // Check game config
            if (_gameConfig == null) { Banner.DisplayWarning(); }
        }

        /// <summary>
        /// Sets up bindings for libReloaded Print functions for printing any errors which happen within
        /// libReloaded.
        /// </summary>
        private static void SetuplibReloadedBindings()
        {
            Bindings.TargetProcess = _gameProcess;
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
                _gameProcess = new ReloadedProcess(Path.Combine(_gameConfig.GameDirectory, _gameConfig.ExecutableLocation));

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
            // Build arguments
            string localArgs = "";
            foreach (string argument in arguments) { localArgs += argument + " "; }
            Process.Start(LoaderPaths.GetModLoaderDirectory() + "\\Reloaded-Wrapper-x64.exe", localArgs);

            // Bye Bye Current Process
            Shutdown(null, null);
        }
    }
}
