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
using System.Linq;
using System.Reflection;
using LiteNetLib;
using Reloaded;
using Reloaded.Assembler;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded_Loader.Core;
using Reloaded_Loader.Miscallenous;
using Reloaded_Loader.Networking;
using Reloaded_Loader.Terminal;
using Reloaded_Loader.Terminal.Information;
using Reloaded_Plugin_System;
using Squirrel;
using static Reloaded.Utilities.CheckArchitecture;
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
            #if DEBUG
            Debugger.Launch();
            #endif

            // Find Assemblies Manually if necessary (Deprecate app.config)
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyFinder.CurrentDomain_AssemblyResolve;

            /* - Initialization - */
            IgnoreSquirrel();
            ParseArguments(args);

            LoaderConsole.Initialize();
            SetuplibReloadedBindings();
            Banner.Execute();
            Controllers.PrintControllerOrder();
            DllUnlocker.UnblockDlls();          // Removes Zone Information which may prevent DLL injection if DLL was downloaded from e.g. Internet Explorer
            LoaderServer.SetupServer();

            /* - Boot up Reloaded Assembler, Get the Game Process running/attached and with mods running. - */
            Assembler.Assemble(new string[] {"use32", "nop eax"}); // Startup Assembler (So a running instance/open handle does not bother devs working on mods)
            GetGameProcess(args);
            InjectMods(args);

            // Resume game after injection if we are NOT in attach mode.
            if (_attachTargetName == null)
            { _gameProcess.ResumeAllThreads(); }
            
            // Stay alive in the background
            AppDomain.CurrentDomain.ProcessExit += Shutdown;
            Console.CancelKeyPress              += Shutdown;

            // Wait infinitely.
            while (true)
            { Console.ReadLine(); }
        }

        /// <summary>
        /// Checks whether to reattach Reloaded Mod Loader if the game process
        /// unexpectedly kills itself and then restarts (a standard for some games' launch procedures).
        /// </summary>
        private static void ProcessSelfReattach(string[] args)
        {
            // Use stopwatch for soft timing.
            Stopwatch timeoutStopWatch = new Stopwatch();
            timeoutStopWatch.Start();

            // Set attach name for reattaching. 
            _attachTargetName = Path.GetFileNameWithoutExtension(_gameConfig.ExecutableLocation);

            // Spin trying to get game process until timeout.
            while (timeoutStopWatch.ElapsedMilliseconds < 6000)
            {
                // Grab current already running game, did we find it? Not? Try again.
                ReloadedProcess localGameProcess = ReloadedProcess.GetProcessByName(_attachTargetName);

                if (localGameProcess == null)
                    continue;

                // Ensure we didn't find some background process that was running all along.
                if (localGameProcess.GetProcessFromReloadedProcess().StartTime > _processStartTime)
                {                   
                    Bindings.PrintInfo($"// Game Restarted, Probably from Self-Kill e.g. Denuvo, VMProtect Reattaching! | {_attachTargetName}");
                    Bindings.PrintInfo($"// In other words, super early attach not supported! Maybe with manual map in the future.");
                    _gameProcess = localGameProcess;

                    // Disconnect all clients
                    foreach (NetPeer peer in LoaderServer.ReloadedServer.GetPeers())
                    { LoaderServer.ReloadedServer.DisconnectPeer(peer); }

                    InjectMods(args);

                    // Wait infinitely.
                    while (true)
                    { Console.ReadLine(); }
                }
            }

            // Kill the process itself.
            Shutdown(null, null);
        }

        /// <summary>
        /// Kicks off the individual mods running inside the target process
        /// specified by our <see cref="_gameProcess"/> variable.
        /// </summary>
        private static void InjectMods(string[] commandLineArguments)
        {
            // Do not reattach on exit for Steam Shims - they should not reboot if shimmed.
            if (commandLineArguments.Contains(Strings.Common.LoaderSettingSteamShim))
            {
                _gameProcess.GetProcessFromReloadedProcess().EnableRaisingEvents = true;
                _gameProcess.GetProcessFromReloadedProcess().Exited += (sender, eventArgs) => Shutdown(null, null);
            }
            else
            {
                _gameProcess.GetProcessFromReloadedProcess().EnableRaisingEvents = true;
                _gameProcess.GetProcessFromReloadedProcess().Exited += (sender, eventArgs) => ProcessSelfReattach(commandLineArguments);
            }
            

            ModLoader.LoadMods(_gameConfig, _gameProcess);
        }

        /// <summary>
        /// Parses the arguments passed into the application.
        /// </summary>
        /// <param name="arguments"></param>
        private static void ParseArguments(string[] arguments)
        {
            // Save the original arguments from plugins in the case the process is X64.
            string[] originalArguments = arguments;

            // Set/Get arguments for all plugins.
            foreach (var eventPlugin in PluginLoader.LoaderEventPlugins)
            { arguments = eventPlugin.GetSetArguments(arguments); }

            // Go over known arguments.
            for (int x = 0; x < arguments.Length; x++)
            {
                if (arguments[x] == $"{Strings.Common.LoaderSettingConfig}") { _gameConfig = CheckConfigJson(arguments[x + 1]); }
                if (arguments[x] == $"{Strings.Common.LoaderSettingAttach}") { _attachTargetName = arguments[x+1]; }
                if (arguments[x] == $"{Strings.Common.LoaderSettingLog}") { Logger.Setup(arguments[x + 1]); }
            }

            // Check game config
            if (_gameConfig == null) { Information.DisplayWarning(); }

            // Are we running in 32bit mode.
            if (IntPtr.Size != 8)
            {
                // If the executable is 64bit, restart.
                if (GetMachineTypeFromPeHeader(_gameConfig.GameDirectory + $"\\{_gameConfig.ExecutableLocation}") == PEMachineType.AMD64)
                    RebootX64(originalArguments);
            } 
        }

        /// <summary>
        /// Retrieves the game instance we are going to be hacking as a ReloadedProcess 
        /// </summary>
        private static void GetGameProcess(string[] originalArguments)
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
                    LoaderConsole.PrintFormattedMessage("Error: An active running game instance was not found.", LoaderConsole.PrintErrorMessage);
                    Console.ReadLine();
                    Shutdown(null, null);
                }
            }

            // Otherwise start process suspended in Reloaded, hook it, exploit it and resume the intended way.
            else
            {
                _gameProcess = new ReloadedProcess($"{Path.Combine(_gameConfig.GameDirectory, _gameConfig.ExecutableLocation)}", _gameConfig.CommandLineArgs.Split(' '));

                // The process handle is 0 if the process failed to initialize.
                if ((int)_gameProcess.ProcessHandle == 0)
                {
                    // libReloaded will already print the error message.
                    Console.ReadLine();
                    Shutdown(null, null);
                }

                // Check if the game should run normally to toggle the shim.
                CheckSteamHack(originalArguments);
            }

            // Set binding for target process for memory IO
            Bindings.TargetProcess = _gameProcess;

            // Obtain the process start time.
            if (_gameProcess != null)
                _processStartTime = _gameProcess.GetProcessFromReloadedProcess().StartTime;
        }


        /*
            ---------------------
            Miscellaneous Actions
            ---------------------
        */

        /// <summary>
        /// Looks for the existence of ReloadedShim.json and restarts the game through the
        /// Steam shim using Steam if necessary. The game will relaunch itself through Steam.
        /// </summary>
        /// <param name="commandlineArguments">Original Commandline Parameters Passed to Application.</param>
        private static void CheckSteamHack(string[] commandlineArguments)
        {
            // If Steam shimming is not specified, check for existence of ReloadedShim.json.
            if (! commandlineArguments.Contains(Strings.Common.LoaderSettingSteamShim))
            {
                // Here, check for the shim existence. 
                // If the shim config exists and commandline parameter not specified, launch game normally and shutdown.
                string gameExeFile          = Path.Combine(_gameConfig.GameDirectory, _gameConfig.ExecutableLocation);
                string gameExeDirectory     = Path.GetDirectoryName(gameExeFile);

                string potentialShimFile    = $"{gameExeDirectory}\\{Strings.Common.SteamShimFileName}";
                if (File.Exists(potentialShimFile))
                {
                    _gameProcess.ResumeAllThreads();
                    Environment.Exit(0);
                }
            }
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
        /// Reboots the Reloaded Loader in x64 mode by running the x64 wrapper.
        /// </summary>
        /// <param name="arguments">A copy of the arguments originally passed to the starting application.</param>
        private static void RebootX64(string[] arguments)
        {
            // Add quotes to each argument in quotes.
            for (int x = 0; x < arguments.Length; x++)
            { arguments[x] = $"\"{arguments[x]}\""; }

            ReloadedProcess reloadedProcess = new ReloadedProcess($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Reloaded-Wrapper-x64.exe", arguments);
            reloadedProcess.ResumeAllThreads();

            Shutdown(null, null);
        }

        /// <summary>
        /// Shuts itself down upon being started by Squirrel.Windows over an update event.
        /// Squirrel.Windows has no business to do here.
        /// </summary>
        private static void IgnoreSquirrel()
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

        /// <summary>
        /// Sets up the print function shorthands.
        /// </summary>
        private static void SetuplibReloadedBindings()
        {
            Bindings.PrintError     += delegate (string message) { LoaderConsole.PrintFormattedMessage(message, LoaderConsole.PrintErrorMessage); };
            Bindings.PrintWarning   += delegate (string message) { LoaderConsole.PrintFormattedMessage(message, LoaderConsole.PrintWarningMessage); };
            Bindings.PrintInfo      += delegate (string message) { LoaderConsole.PrintFormattedMessage(message, LoaderConsole.PrintInfoMessage); };
            Bindings.PrintText      += delegate (string message) { LoaderConsole.PrintFormattedMessage(message, LoaderConsole.PrintMessage); };
        }

        /// <summary>
        /// Shuts down the application. Intended to be triggered when user presses CTRL+C.
        /// </summary>
        private static void Shutdown(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
