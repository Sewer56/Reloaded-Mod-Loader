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
using System.Collections.Generic;
using System.IO;
using Reloaded.IO.Config.Games;
using Reloaded.Process;
using Reloaded.Process.Memory;
using Reloaded.Utilities;
using Reloaded_Loader.Miscallenous;
using Reloaded_Loader.Networking;

namespace Reloaded_Loader.Core
{
    /// <summary>
    /// Finds the mods that are currently enabled for the game and injects into the target process.
    /// </summary>
    public static class ModLoader
    {
        /// <summary>
        /// Finds the mods that are currently enabled for the game and injects into the target process.
        /// </summary>
        /// <param name="gameConfiguration">The game configuration which contains the current directory and list of mods to load.</param>
        /// <param name="reloadedProcess">The reloaded process to inject the modifications into.</param>
        public static void LoadMods(GameConfigParser.GameConfig gameConfiguration, ReloadedProcess reloadedProcess)
        {
            // Get directory containing the game's mod list
            string gameModDirectory = Path.Combine(LoaderPaths.GetModLoaderModDirectory(), gameConfiguration.ModDirectory);

            // Get directories containing enabled mods.
            List<string> modLibraries = new List<string>(gameConfiguration.EnabledMods.Count);

            // Get the dll locations.
            foreach (string modDirectory in gameConfiguration.EnabledMods)
            {
                // Add native or not native.
                if (ReloadedArchitecture.IsGame32Bit) { modLibraries.Add(Path.Combine(gameModDirectory, modDirectory, "main32.dll")); }
                else { modLibraries.Add(Path.Combine(gameModDirectory, modDirectory, "main64.dll")); }  
            }

            // Initialize DLL Injector
            DllInjector reloadedDllInjector = new DllInjector(reloadedProcess);

            // If the main.dll exists, load it.
            foreach (string modLibrary in modLibraries)
            {
                // If the DLL Exists, Try to Load It
                if (File.Exists(modLibrary))
                {
                    // Allocate Memory for Server Port In Game Memory
                    IntPtr parameterAddress = reloadedProcess.AllocateMemory(IntPtr.Size);

                    // Write Server Port to Game Memory
                    reloadedProcess.WriteMemoryExternal(parameterAddress, BitConverter.GetBytes(LoaderServer.ServerPort));

                    // Inject the individual DLL.
                    reloadedDllInjector.InjectDll(modLibrary, parameterAddress);
                }
            }

            // Resume game after injection.
            reloadedProcess.ResumeFirstThread();
        }
    }
}
