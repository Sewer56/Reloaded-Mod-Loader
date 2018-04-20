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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Process;

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
        /// <param name="localArguments">Additional command line arguments/options to pass to Reloaded-Loader.</param>
        public static void LaunchLoader(string localArguments)
        {
            // Build filepath and arguments
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Reloaded-Loader.exe";
            string arguments = $"--config {Global.CurrentGameConfig.ConfigDirectory} {localArguments}";

            // Start process
            ReloadedProcess process = new ReloadedProcess(filePath, arguments);
            process.ResumeAllThreads();

            // Self-shutdown.
            Shutdown();
        }
    }
}
