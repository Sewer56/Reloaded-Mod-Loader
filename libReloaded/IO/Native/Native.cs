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

using System;
using System.Runtime.InteropServices;

namespace Reloaded.IO.Native
{
    /// <summary>
    /// Provides native functions used for input and output operations.
    /// </summary>
    public static class Native
    {
        /// <summary>
        /// Creates a hardlink for an already existing specific file elsewhere at another path.
        /// </summary>
        /// <param name="lpFileName">The name of the new file. This parameter may include the path but cannot specify the name of a directory.</param>
        /// <param name="lpExistingFileName">The name of the existing file. This parameter may include the path cannot specify the name of a directory.</param>
        /// <param name="lpSecurityAttributes">Reserved, should be set to null (IntPtr.Zero).</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
    }
}
