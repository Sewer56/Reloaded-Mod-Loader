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

using System.IO;
using Reloaded.Paths;
using Reloaded.Utilities;
using Reloaded.Utilities.PE;
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
            LoaderConsole.PrintFormattedMessage("Removing Zone Identifiers from Files (DLL Unlocking)", LoaderConsole.PrintInfo);

            // Search all DLLs under loader directories.
            // Normally I'd restrict this to mod directories, but the loader's own libraries might also be worth checking.
            string[] dllFiles = Directory.GetFiles(LoaderPaths.GetModLoaderDirectory(), "*.dll", SearchOption.AllDirectories);

            // Unblock new file.
            foreach (string dllFile in dllFiles)
            {
                Executable.Unblock(dllFile);
            }
        }
    }
}
