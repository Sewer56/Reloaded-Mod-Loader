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
using System.Diagnostics;
using System.IO;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded.Process.Memory;
using Reloaded.Utilities;
using Reloaded_Loader.Miscellaneous;
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
        public static void LoadMods(GameConfig gameConfiguration, ReloadedProcess reloadedProcess)
        {
            // Retrieve list of DLLs to be injected.
            string[] modLibraries = GetModulesToInject(gameConfiguration);

            // Initialize DLL Injector
            DllInjector reloadedClassicDllInjector = new DllInjector(reloadedProcess);

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
                    reloadedClassicDllInjector.InjectDll(modLibrary, parameterAddress, "Main");
                }
            }
        }

        /// <summary>
        /// Retrieves an array of DLLs locations to be injected and loaded into the target process.
        /// </summary>
        /// <param name="gameConfiguration">The game configuration which contains the current directory and list of mods to load.</param>
        /// <returns>A list of DLL locations to be injected and loaded into the target process.</returns>
        private static string[] GetModulesToInject(GameConfig gameConfiguration)
        {
            // Build a complete list of mods for the game and for the global configuration.
            List<ModConfig> enabledMods = GameConfig.GetAllEnabledMods(gameConfiguration);

            // Topologically sort mods.
            enabledMods = GameConfig.TopologicallySortConfigurations(enabledMods);

            // Convert back to folder structure and append to library list.
            List<string> dllFiles = new List<string>(enabledMods.Count);

            // Set path for all DLLs in enabled mods list.
            foreach (var enabledMod in enabledMods)
            {
                if (ReloadedArchitecture.IsGame32Bit)
                {
                    string x86DllLocation = Path.Combine(enabledMod.GetModDirectory(), Strings.Loader.Mod32BitDllFile);
                    if (File.Exists(x86DllLocation))
                        dllFiles.Add(x86DllLocation);
                }
                else
                {
                    string x64DllLocation = Path.Combine(enabledMod.GetModDirectory(), Strings.Loader.Mod64BitDllFile);
                    if (File.Exists(x64DllLocation))
                        dllFiles.Add(x64DllLocation);
                }
            }

            // Return the list.
            return dllFiles.ToArray();
        }
    }
}
