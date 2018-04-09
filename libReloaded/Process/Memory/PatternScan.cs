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
using System.Runtime.CompilerServices;

namespace Reloaded.Process.Memory
{
    /// <summary>
    /// Class which allows the manipulation of in-game memory status.
    /// </summary>
    public static class PatternScan
    {
        /// <summary>
        /// FindPattern  
        ///     Attempts to locate the given pattern inside a supplied memory region,
        ///     with support for checking against a given mask. 
        ///     If the pattern is found, the offset within the supplied memory region
        ///     where the pattern matches is returned.
        /// </summary>
        /// <param name="memoryRegion">The memory region in which to look for the specified pattern.</param>
        /// <param name="bytePattern">Byte pattern to look for in the dumped region. e.g. new byte[] { 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88 }</param>
        /// <param name="stringMask">The mask string to compare against. `x` represents check while `?` ignores. Each `x` and `?` represent 1 byte.</param>
        /// <returns>0 if not found, offset in memory region if found.</returns>
        /// <remarks>
        ///     Generally useful when looking for game code for games which receive periodic updates.
        ///     As the generated compiler code is not likely to differ, you could automatically re-obtain
        ///     certain memory addresses for ASM code that writes to, for example player location between
        ///     different executables and versions of the same game or target process.
        /// </remarks>
        public static IntPtr FindPattern(byte[] memoryRegion, byte[] bytePattern, string stringMask)
        {
            try
            {
                // Ensure mask and pattern length match.
                if (stringMask.Length != bytePattern.Length) return IntPtr.Zero;

                // Loop the region and look for the pattern.
                for (int x = 0; x < memoryRegion.Length; x++)

                // Check for the mask, incrementing start offset each time.
                if (MaskCheck(memoryRegion, bytePattern, stringMask, x)) return new IntPtr(x);

                // Pattern was not found.
                return IntPtr.Zero;
            }
            catch (Exception) { return IntPtr.Zero; }
        }


        /// <summary>
        /// MaskCheck
        ///     Compares the current pattern byte to the supplied memory dump
        ///     byte to check for a match. Uses wildcards to skip bytes that
        ///     are deemed unneeded in the compares.
        /// </summary>
        /// <param name="memoryRegion">The memory region in which to look for the specified pattern.</param>
        /// <param name="bytePattern">Byte pattern to look for in the dumped region.</param>
        /// <param name="stringMask">The mask string to compare against. `XX` represents check while `??` ignores.</param>
        /// <param name="startOffset">Offset in the dump to start comparing bytes against at.</param>
        /// <returns>Boolean depending on if the pattern was found.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool MaskCheck(byte[] memoryRegion, byte[] bytePattern, string stringMask, int startOffset)
        {
            // Loop the pattern and compare to the mask and our memory region.
            for (int x = 0; x < bytePattern.Length; x++)
            {
                // If the mask char is a wildcard, ignore.
                if (stringMask[x] == '?') continue;

                // If the mask char is not a wildcard, check if a match is not made in the pattern.
                if
                (
                    stringMask[x] == 'x' &&                    
                    bytePattern[x] != memoryRegion[startOffset + x]
                )
                    return false;
            }

            // The loop was successful, as everything matched up to this point.
            // We found the pattern.
            return true;
        }
    }
}
