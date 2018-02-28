using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.IO;
using Reloaded.Misc;
using Reloaded_Loader.Terminal;

namespace Reloaded_Loader.Miscallenous
{
    /// <summary>
    /// Removes Zone Information from dynamic link libraries downloaded from the internet such
    /// that certain users of Microsoft Windows would not be denied loading of our own arbitrary code.
    /// </summary>
    public static class DllUnlocker
    {
        /// <summary>
        /// Removes Zone Information from dynamic link libraries downloaded from the internet such
        /// that certain users of Microsoft Windows would not be denied loading of our own arbitrary code.
        /// </summary>
        /// <remarks>
        /// Only affects files downloaded via very specific certain outdated programs such as
        /// Internet Explorer
        /// </remarks>
        public static void UnblockDlls()
        {
            // Print Info Message about Unlocking DLLs
            ConsoleFunctions.PrintMessageWithTime("Removing Zone Identifiers from Files (DLL Unlocking)", ConsoleFunctions.PrintInfoMessage);

            // Search all DLLs under loader directories.
            // Normally I'd restrict this to mod directories, but the loader's own libraries might also be worth checking.
            string[] dllFiles = Directory.GetFiles(LoaderPaths.GetModLoaderDirectory(), "*.dll", SearchOption.AllDirectories);

            // Unblock new file.
            for (int x = 0; x < dllFiles.Length; x++) {
                FileUnblocker.Unblock(dllFiles[x]);
            }
        }
    }
}
