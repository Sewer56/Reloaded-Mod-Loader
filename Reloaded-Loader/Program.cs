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
using System.Text;
using System.Threading;
using Reloaded.GameProcess;
using Reloaded.Misc;
using Reloaded.Misc.Config;
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
        private static ReloadedProcess gameProcess;

        /// <summary>
        /// Stores the game configuration to be used.
        /// </summary>
        private static GameConfigParser.GameConfig gameConfig;

        /// <summary>
        /// Specifies the name of the target of an existing running process to attach to.
        /// </summary>
        private static string injectionTargetName;

        /// <summary>
        /// The amount of milliseconds to delay program execution before injecting mods.
        /// </summary>
        private const int DELAY_HOOK_DURATION = 5000;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // Initialize the console.
            ConsoleFunctions.Initialize();

            // Write Loader Location
            File.WriteAllText(LoaderPaths.GetModLoaderLinkLocation(), Environment.CurrentDirectory);

            // Print startup information.
            Banner.PrintBanner();

            // Get Controller Order
            Controllers.PrintControllerOrder();

            // Parse Arguments
            ParseArguments(args);

            // Unlock DLLs
            DllUnlocker.UnblockDlls();

            // Setup Server
            LoaderServer.SetupServer();

            // Start game
            InjectByMethod();

            // Load modifications for the current game.
            ModLoader.LoadMods(gameConfig, gameProcess);

            // Stay alive in the background
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Shutdown);
            Console.CancelKeyPress += (sender, eArgs) => { Shutdown(sender, eArgs); };

            // Poll every 3 seconds, kill self if child dies.
            while (true)
            {
                try
                {
                    // Get Process
                    Process localGameProcess = gameProcess.GetProcessFromReloadedProcess();

                    // Check if process has exited.
                    if (localGameProcess.HasExited) { Shutdown(null, null); }

                    // Sleep
                    Thread.Sleep(2000);
                }
                // Argument of Process.GetProcessById fails inside GetProcessFromReloadedProcess
                // The process died.
                catch (ArgumentException e) { Shutdown(null, null); }
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
                if (arguments[x] == "--config") { gameConfig = new GameConfigParser().ParseConfig(arguments[x + 1]); }
                if (arguments[x] == "--attach") {
                    if (x + 1 < arguments.Length) {
                        injectionTargetName = arguments[x+1];
                    }
                    gameConfig.HookMethod = GameConfigParser.HookMethod.Inject;
                }
            }

            // Check game config
            if (gameConfig == null) { Banner.DisplayWarning(); }
        }

        /// <summary>
        /// Injects itself to the game depending on the individual method chosen in the mod loader configuration.
        /// Instant hooking injects immediately, manual hooking injects on user command, delayed hooking hooks at a delay.
        /// </summary>
        private static void InjectByMethod()
        {
            switch (gameConfig.HookMethod)
            {
                // Instant Hook, Start the Game Manually and Hook it.
                case GameConfigParser.HookMethod.Instant:
                    gameProcess = new ReloadedProcess(Path.Combine(gameConfig.GameDirectory, gameConfig.ExecutableLocation));
                    break;

                // Inject Hook, Hook the Game by User/Mid-Runtime
                case GameConfigParser.HookMethod.Inject:

                    // If executable name was not manually set, get it from the game config.
                    if (injectionTargetName == null) {
                        injectionTargetName = Path.GetFileNameWithoutExtension(gameConfig.ExecutableLocation);
                    }

                    // Grab current already running game.
                    gameProcess = ReloadedProcess.GetProcessByName(injectionTargetName);

                    // Check if gameProcess successfully returned.
                    if (gameProcess == null) {
                        ConsoleFunctions.PrintMessageWithTime("Error: An active running game instance was not found.", ConsoleFunctions.PrintErrorMessage);
                        Console.ReadLine();
                        Shutdown(null, null);
                    }

                    break;

                // If the method is delayed, start process and wait 5 seconds
                case GameConfigParser.HookMethod.Delayed:
                    gameProcess = new ReloadedProcess(Path.Combine(gameConfig.GameDirectory, gameConfig.ExecutableLocation));
                    gameProcess.ResumeFirstThread();
                    Thread.Sleep(DELAY_HOOK_DURATION);
                    break;
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
    }
}
