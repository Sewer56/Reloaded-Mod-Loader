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
using Reloaded.Process;
using Reloaded.Process.Native;

namespace Reloaded_Loader.Miscellaneous
{
    /// <summary>
    /// Verifies whether the architecture of a ReloadedProcess matches the architecture
    /// of the current executable.
    /// </summary>
    public static class ReloadedArchitecture
    {
        /// <summary>
        /// Declares whether the game is 32 bit.
        /// This allows us to run the appropriate injector.
        /// </summary>
        public static bool IsGame32Bit;

        /// <summary>
        /// Verifies whether the architecture of a Reloaded Process matches the architecture of
        /// the current program.
        /// </summary>
        public static bool CheckArchitectureMatch(this ReloadedProcess reloadedProcess)
        {
            // Check if Process is x64.
            Native.IsWow64Process(reloadedProcess.ProcessHandle, out IsGame32Bit);

            // Compare against current process.
            return IsGame32Bit == (! Environment.Is64BitProcess);
        }
    }
}
