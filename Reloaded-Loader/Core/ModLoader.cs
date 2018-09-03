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
using Newtonsoft.Json;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Process;
using Reloaded.Process.Memory;
using Reloaded.Utilities;
using Reloaded_Loader.Miscallenous;
using Reloaded_Loader.Networking;
using Reloaded_Plugin_System;

namespace Reloaded_Loader.Core
{
    public static class ModLoader
    {
        /*
            ------------------------
            Performing DLL Injection
            ------------------------
        */

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

            // For each DLL file, if the DLL exists, load the DLL.
            foreach (string modLibrary in modLibraries)
            {
                if (File.Exists(modLibrary))
                {
                    // To handle plugin support for this one, we pass the parameters onto each plugin and
                    // each plugin on their own can decide whether to manually inject themselves or not.
                    // If they return "true", we go to next file; else we inject normally.
                    bool dllInjected = false;
                    foreach (var plugin in PluginLoader.LoaderEventPlugins)
                    {
                        if (plugin.ManualDllInject((int)reloadedProcess.ProcessId, modLibrary, "Main"))
                        {
                            dllInjected = true;
                            break;
                        }
                    }

                    if (!dllInjected)
                        InjectDLLDefault(reloadedProcess, reloadedClassicDllInjector, modLibrary, "Main");
                }
            }
        }

        private static void InjectDLLDefault(ReloadedProcess reloadedProcess, DllInjector dllInjector, string dllPath, string dllMethodName)
        {
            // Allocate Memory for Server Port In Game Memory
            IntPtr parameterAddress = reloadedProcess.AllocateMemory(IntPtr.Size);

            // Write Server Port to Game Memory
            byte[] serverPort = BitConverter.GetBytes(LoaderServer.ServerPort);
            reloadedProcess.WriteMemoryExternal(parameterAddress, ref serverPort);

            // Inject the individual DLL.
            dllInjector.InjectDll(dllPath, parameterAddress, dllMethodName);
        }

        /*
            -----------------------
            Preparing DLL Injection
            -----------------------
        */

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

            // Set path for all DLLs in enabled mods list; including plugin supported mods.
            string gameConfig = JsonConvert.SerializeObject(gameConfiguration);

            List<string> dllFiles = new List<string>(enabledMods.Count);
            foreach (var enabledMod in enabledMods)
            {
                // Get dll path to inject.
                string dllPath = GetDLLPathToInject(enabledMod);

                string modConfig = JsonConvert.SerializeObject(enabledMod);
                foreach (var plugin in PluginLoader.LoaderEventPlugins)
                    dllPath = plugin.SetDllInjectionPath(dllPath, modConfig, gameConfig);

                if (dllPath != null)
                    dllFiles.Add(dllPath);
            }

            return dllFiles.ToArray();
        }

        /// <summary>
        /// Gets the full path of the mod dll to be injected for this individual game.
        /// </summary>
        private static string GetDLLPathToInject(ModConfig modConfig)
        {
            if (ReloadedArchitecture.IsGame32Bit)
            {
                // Explicitly Set? Use that path.
                if (! String.IsNullOrEmpty(modConfig.DllFile32))
                    return Path.Combine(modConfig.GetModDirectory(), modConfig.DllFile32);

                // Not Explicitly Set.
                string x86DllLocation = Path.Combine(modConfig.GetModDirectory(), Strings.Loader.Mod32BitDllFile);
                if (File.Exists(x86DllLocation))
                    return x86DllLocation;
            }
            else
            {
                // Explicitly Set? Use that path.
                if (!String.IsNullOrEmpty(modConfig.DllFile64))
                    return Path.Combine(modConfig.GetModDirectory(), modConfig.DllFile64);

                // Not Explicitly Set.
                string x64DllLocation = Path.Combine(modConfig.GetModDirectory(), Strings.Loader.Mod64BitDllFile);
                if (File.Exists(x64DllLocation))
                    return x64DllLocation;
            }

            return null;
        }
    }
}
