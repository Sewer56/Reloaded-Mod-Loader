/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
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

using System.Runtime.InteropServices;

namespace Reloaded.Utilities
{
    /// <summary>
    /// Allows for the unblocking of files that are by default blocked by windows
    /// with the removal of Zone Identifiers for files such that Zone Information 
    /// is not stored.
    /// </summary>
    public static class FileUnblocker
    {
        /// <summary>
        /// Deletes an existing file.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <returns>True (nonzero) if the function succeeds.</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFile(string name);

        /// <summary>
        /// Atempts to remove the zone identifier of a file with a specific file name.
        /// </summary>
        public static bool Unblock(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }
    }
}
