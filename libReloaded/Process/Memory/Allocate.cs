/*
    [Reloaded] Mod Loader Launcher
    A universal, powerful multi-game, multi-process mod loader based on DLL Injection. 
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
using static Reloaded.Process.Native.Native;

namespace Reloaded.Process.Memory
{
    /// <summary>
    /// Class which allows the manipulation of in-game memory.
    /// This file provides the implementation for allocating and freeing memory.
    /// </summary>
    public static class MemoryAllocator
    {
        /// <summary>
        /// AllocateMemory
        ///     Allows for allocation of memory space inside the target process. 
        ///     The return value for this method is the address at which the new memory has been allocated. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="length">Length of free bytes you want to allocate.</param>
        /// <returns>Base pointer address to the newly allocated memory.</returns>
        public static IntPtr AllocateMemory(this ReloadedProcess process, int length)
        {
            // Call VirtualAllocEx to allocate memory of fixed chosen size.
            return VirtualAllocEx
            (
                process.ProcessHandle, 
                IntPtr.Zero, 
                (IntPtr)length, 
                AllocationType.Commit | AllocationType.Reserve, 
                MemoryProtection.ExecuteReadWrite
            );
        }

        /// <summary>
        /// FreeMemory
        ///     Allows for the freeing of memory space inside the target process. 
        ///     Releases memory such that it may be cleaned and re-used by the Windows Operating System.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to free memory from.</param>
        /// <returns>A value that is not 0 if the operation is successful.</returns>
        public static bool FreeMemory(this ReloadedProcess process, IntPtr address)
        {
            return VirtualFreeEx(process.ProcessHandle, address, 0, FreeType.Decommit | FreeType.Release);
        }
    }
}
