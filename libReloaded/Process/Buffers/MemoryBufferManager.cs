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
using System.Collections.Generic;
using Reloaded.Process.Memory;
using System.Runtime.InteropServices;
using static Reloaded.Process.Native.Native;

namespace Reloaded.Process.Buffers
{
    /// <summary>
    /// Provides a a way to detect individual Reloaded buffers inside a process used for general small size memory storage,
    /// adding buffer information within certain proximity of an address as well as other various utilities partaining to
    /// buffers.
    /// </summary>
    public static class MemoryBufferManager
    {
        /// <summary>
        /// Scans the currently targeted process in <see cref="Bindings.TargetProcess"/> for any buffers
        /// managed by Reloaded and returns a list of available buffers.
        /// </summary>
        /// <param name="size">Specifies an optional size in bytes that must be at least available in the buffer for it to be included in the list.</param>
        /// <returns>A list of available Reloaded Buffers to be used.</returns>
        public static List<MemoryBuffer> GetBuffers(int size = 0)
        {
            // Get a list of all pages.
            var memoryBasicInformation = Bindings.TargetProcess.GetPages();

            // Get System Allocation Granularity.
            GetSystemInfo(out var systemInfo);

            // Check if each page is the start of a buffer, and add it conditionally.
            List<MemoryBuffer> memoryBuffers = new List<MemoryBuffer>();
            for (int x = 0; x < memoryBasicInformation.Count; x++)
            {
                if (memoryBasicInformation[x].State == (PageState.Commit | PageState.Reserve) && memoryBasicInformation[x].lType == PageType.Private && IsBuffer(memoryBasicInformation[x].BaseAddress))
                {
                    MemoryBuffer buffer = new MemoryBuffer(memoryBasicInformation[x].BaseAddress, systemInfo.allocationGranularity);
                    if (buffer.CheckItemSize(size))
                        memoryBuffers.Add(buffer);
                }      
            }

            // Return all buffers.
            return memoryBuffers;
        }

        /// <summary>
        /// See <see cref="Add(byte[])"/> for details. Intended to use for X64 assembly purely.
        /// Adds the specified byte contents into a buffer in 32bit signed (2GB) proximity to a specified target address.
        /// Intended to be used in conjunction with 64bit Assembly, where 64bit immediates are not available.
        /// </summary>
        /// <param name="bytesToWrite">Individual bytes to be written onto a buffer.</param>
        /// <param name="targetAddress">
        ///     The target address within which the buffer to be written to must lie in 2GB memory proximity of.
        ///     Specify 0 for 32bit address, else near whatever assembly code you are targeting.
        /// </param>
        /// <returns>Pointer to the passed in bytes written to memory.</returns>
        public static unsafe IntPtr Add(byte[] bytesToWrite, IntPtr targetAddress)
        {
            // Get available buffers.
            List<MemoryBuffer> buffers = GetBuffers(bytesToWrite.Length);

            // Iterate over all buffers to see if any fits the desired target.
            ulong minimumAddress = (ulong)(targetAddress  + -2000000000); // 2GB
            ulong maximumAddress = (ulong) targetAddress  + 2000000000;   // 2GB

            // Check in case minimum address < 0
            if (minimumAddress > (ulong)targetAddress)
                minimumAddress = 0;

            // Find appropriate buffer, add and return.
            // If buffer not found, fallback is provided below to create a new buffer.
            foreach (var buffer in buffers)
            {
                ulong startAddress = (ulong)buffer.BaseBufferAddress;
                ulong endAddress =   (ulong)buffer.BaseBufferAddress + buffer.BufferHeader.BufferSize;

                if (startAddress >= minimumAddress && endAddress < maximumAddress)
                {
                    // Ensure our bytes fit to the buffer.
                    if (buffer.CheckItemSize(bytesToWrite.Length))
                    {
                        return buffer.Add(bytesToWrite);
                    }
                }
            }

            // If we have not returned, no buffer was found in the specified range.
            // Get a list of all pages.
            var memoryBasicInformation = Bindings.TargetProcess.GetPages();

            // Get System Allocation Granularity.
            GetSystemInfo(out var systemInfo);

            // Get the desired page size, that being 1 allocation granularity greater than the bytes to be written.
            int totalBytesToWrite = bytesToWrite.Length + sizeof(MemoryBufferHeader);
            int desiredGranularity = (totalBytesToWrite / (int)systemInfo.allocationGranularity) + 1;
            uint desiredBufferSize = (uint)(desiredGranularity * systemInfo.allocationGranularity);

            // Iterate each page.
            foreach (var memoryPages in memoryBasicInformation)
            {
                // For clarity, will be optimized out by the JIT compiler.
                // We have to align the starting address with the allocation granularity, it must be a multiple of it.
                ulong pageEnd = (ulong)memoryPages.BaseAddress + (ulong)memoryPages.RegionSize;

                // Place on next 64K boundary (or allocation granularity)
                ulong firstPageBase = (((ulong)memoryPages.BaseAddress / systemInfo.allocationGranularity) + 1) * systemInfo.allocationGranularity;
                ulong firstPageEnd = firstPageBase + desiredBufferSize;

                ulong lastPageBase = ((pageEnd - desiredBufferSize) / systemInfo.allocationGranularity) * systemInfo.allocationGranularity;
                ulong lastPageEnd = lastPageBase + desiredBufferSize;

                // Check if desired buffer would fit in the free page region.
                if ((ulong) memoryPages.RegionSize > desiredBufferSize && memoryPages.State == PageState.Free)
                {
                    // 1. Check if inbounds. 2. Check if new page start aligned to 64K/Granularity still fits.
                    if (lastPageBase >= minimumAddress && lastPageEnd < maximumAddress
                                                       && lastPageBase > (ulong)memoryPages.BaseAddress)
                    {
                        MemoryBuffer buffer = new MemoryBuffer((IntPtr)lastPageBase, desiredBufferSize);
                        return buffer.Add(bytesToWrite);
                    }
                    // 1. Check if inbounds. 2. Check if new page start aligned to 64K/Granularity still fits.
                    else if (firstPageBase >= minimumAddress && firstPageEnd < maximumAddress
                                                             && firstPageEnd < (ulong)memoryPages.BaseAddress + (ulong) memoryPages.RegionSize)
                    {
                        MemoryBuffer buffer = new MemoryBuffer((IntPtr)firstPageBase, desiredBufferSize);
                        return buffer.Add(bytesToWrite);
                    }
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Finds an available already present buffer and writes writes your own memory bytes into
        /// the specified buffer, giving you back the address of said buffer. If there is no available
        /// buffer, a new buffer will be automatically created.
        /// </summary>
        /// <param name="bytesToWrite">Individual bytes to be written onto a buffer.</param>
        /// <returns>Pointer to the passed in bytes written to memory.</returns>
        public static IntPtr Add(byte[] bytesToWrite)
        {
            // Get available buffers.
            List<MemoryBuffer> buffers = GetBuffers(bytesToWrite.Length);

            // If there are no available buffers, create one.
            if (buffers.Count < 1)
            {
                // Get System Allocation Granularity.
                GetSystemInfo(out var systemInfo);

                // Get the desired page size, that being 1 allocation granularity greater than the bytes to be written.
                // 40 is an (over) estimate on the buffer header size, which cannot be properly read due string.
                // Retrieves the total size of the object to be added to a buffer, and the buffer header.
                int totalObjectSize = bytesToWrite.Length + Marshal.SizeOf(typeof(MemoryBufferHeader));
                int desiredGranularity = (totalObjectSize / (int) systemInfo.allocationGranularity) + 1;
                uint desiredBufferSize = (uint) (desiredGranularity * systemInfo.allocationGranularity);

                // Create new buffer.
                var memoryPageSet = Bindings.TargetProcess.GetPages();
                foreach (var memoryPages in memoryPageSet)
                {
                    // For clarity, will be optimized out by the JIT compiler.
                    // We have to align the starting address with the allocation granularity, it must be a multiple of it.
                    long pageEnd = (long)memoryPages.BaseAddress + (long)memoryPages.RegionSize;

                    // Place on next 64K boundary (or allocation granularity)
                    long firstPageBase = (((long)memoryPages.BaseAddress / systemInfo.allocationGranularity) + 1) * systemInfo.allocationGranularity;
                    long firstPageEnd = firstPageBase + desiredBufferSize;

                    long lastPageBase = ((pageEnd - desiredBufferSize) / systemInfo.allocationGranularity) * systemInfo.allocationGranularity;
                    long lastPageEnd = lastPageBase + desiredBufferSize;

                    // Check if desired buffer would fit in the free page region.
                    if ((uint)memoryPages.RegionSize > desiredBufferSize && memoryPages.State == PageState.Free)
                    {
                        // 1. Check if inbounds. 2. Check if new page start aligned to 64K/Granularity still fits.
                        if (lastPageBase > (long)memoryPages.BaseAddress)
                        {
                            MemoryBuffer buffer = new MemoryBuffer((IntPtr)lastPageBase, desiredBufferSize);
                            return buffer.Add(bytesToWrite);
                        }
                        // 1. Check if inbounds. 2. Check if new page start aligned to 64K/Granularity still fits.
                        else if (firstPageEnd < (long)memoryPages.BaseAddress + (long)memoryPages.RegionSize)
                        {
                            MemoryBuffer buffer = new MemoryBuffer((IntPtr)firstPageBase, desiredBufferSize);
                            return buffer.Add(bytesToWrite);
                        }
                    }
                }

                // Probably out of memory.
                return IntPtr.Zero;
            }
            else
            {
                return buffers[0].Add(bytesToWrite);
            }
        }

        /// <summary>
        /// Finds an available buffer, or creates one if it doesn't exist and writes your own structure
        /// address into process' memory and gives you the address to which the structure has been directly written to.
        /// </summary>
        /// <param name="bytesToWrite">A structure to be converted into individual bytes to be written onto the buffer.</param>
        /// <returns>Pointer to the newly written structure in memory.</returns>
        public static IntPtr Add<TStructure>(TStructure bytesToWrite)
        {
            // Know what to do
            return Add(MemoryReadWrite.ConvertStructureToByteArray(ref bytesToWrite));
        }

        /// <summary>
        /// See <see cref="Add(byte[],System.IntPtr)"/>
        /// </summary>
        public static IntPtr Add<TStructure>(TStructure bytesToWrite, IntPtr targetAddress)
        {
            // Know what to do
            return Add(MemoryReadWrite.ConvertStructureToByteArray(ref bytesToWrite), targetAddress);
        }

        /// <summary>
        /// Returns true if the supplied memory address is the start of a Reloaded Buffer.
        /// Else false.
        /// </summary>
        /// <param name="address">Address to check if it contains the start of a buffer, a start of a region of pages obtained from GetPages().</param>
        private static bool IsBuffer(IntPtr address)
        {
            try
            {
                // Check if buffer already exists by reading an existing or nonexisting header.
                MemoryBufferHeader bufferHeader = Bindings.TargetProcess.ReadMemoryExternal<MemoryBufferHeader>(address);

                if (bufferHeader.ReloadedMagic == MemoryBufferHeader.RELOADED_MAGIC)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
