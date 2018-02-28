using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.GameProcess;
using Reloaded.Misc;
using Reloaded.Misc.Config;
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
                modLibraries.Add(Path.Combine(gameModDirectory, modDirectory, "main.dll"));
            }

            // Initialize DLL Injector
            DLLInjector reloadedDllInjector = new DLLInjector(reloadedProcess);

            // If the main.dll exists, load it.
            foreach (string modLibrary in modLibraries)
            {
                // If the DLL Exists, Try to Load It
                if (File.Exists(modLibrary))
                {
                    // Allocate Memory for Server Port In Game Memory
                    IntPtr parameterAddress = reloadedProcess.AllocateMemory(IntPtr.Size);

                    // Write Server Port to Game Memory
                    reloadedProcess.WriteMemoryExternalSafe(parameterAddress, BitConverter.GetBytes(LoaderServer.SERVER_PORT));

                    // Inject the individual DLL.
                    reloadedDllInjector.InjectDll(modLibrary, parameterAddress);
                }
            }

            // Resume game after injection.
            reloadedProcess.ResumeFirstThread();
        }
    }
}
