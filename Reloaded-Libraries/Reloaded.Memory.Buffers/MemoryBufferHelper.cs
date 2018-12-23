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

using Reloaded.Memory.Buffers.Structs;
using Reloaded.Memory.Buffers.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Reloaded.Utilities.Mathematics;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers
{
    /// <summary>
    /// Provides a a way to detect individual Reloaded buffers inside a process used for general small size memory storage,
    /// adding buffer information within certain proximity of an address as well as other various utilities partaining to
    /// buffers.
    /// </summary>
    public class MemoryBufferHelper
    {
        /// <summary>
        /// Contains the default size of memory pages to be allocated.
        /// </summary>
        public const int DefaultPageSize = 0x1000;

        /// <summary>
        /// Implementation of the Searcher that scans and finds existing MemoryBuffers within the current process.
        /// </summary>
        private MemoryBufferSearcher _bufferSearcher;

        /// <summary>
        /// The process on which the MemoryBuffer acts upon.
        /// </summary>
        private Process _process;

        /// <summary>
        /// Creates a new Buffer Helper for the specified process.
        /// </summary>
        /// <param name="process">The process for which to create the Buffer Helper for.</param>
        public MemoryBufferHelper(Process process)
        {
            _process = process;
            _bufferSearcher = new MemoryBufferSearcher(process);
        }

        /*
            -----------------------
            Memory Buffer Factories
            -----------------------
        */

        /// <summary>
        /// Finds an appropriate location where a <see cref="MemoryBuffer"/>;
        /// or other memory allocation could be performed inside the target process.
        /// </summary>
        /// <param name="size">The minimum size in bytes of the buffer to be allocated.</param>
        public BufferAllocationProperties FindBufferLocation(int size)
        {
            var memoryPages = MemoryPages.GetPages(_process);
            int bufferSize = GetBufferSize(size);

            Kernel32.GetSystemInfo(out var systemInfo);

            // Find a free page that satisfies the necessary properties to create a new buffer there.
            foreach (var page in memoryPages)
            {
                // Align the page base address with minimum allocation granularity.
                IntPtr baseAddress = (IntPtr)Mathematics.RoundUp((long)page.BaseAddress, (long)systemInfo.dwAllocationGranularity);
                ulong adjustedBufferSize = (ulong)page.RegionSize.Value - ((ulong)baseAddress - (ulong)page.BaseAddress);

                if (adjustedBufferSize >= (uint)bufferSize &&
                    page.State == (uint)Kernel32.MEM_ALLOCATION_TYPE.MEM_FREE &&
                    (ulong)baseAddress >= (ulong)systemInfo.lpMinimumApplicationAddress &&
                    (ulong)baseAddress <= (ulong)systemInfo.lpMaximumApplicationAddress)
                {
                    return new BufferAllocationProperties(baseAddress, bufferSize);
                }

            }

            throw new Exception($"Unable to find memory location to fit MemoryBuffer of size {size} ({bufferSize}).");
        }

        /// <summary>
        /// Finds an appropriate location where a <see cref="MemoryBuffer"/>;
        /// or other memory allocation could be performed inside the target process.
        /// </summary>
        /// <param name = "size" > The minimum size in bytes of the buffer to be allocated.</param>
        /// <param name="minimumAddress">The minimum absolute address to find a buffer in.</param>
        /// <param name="maximumAddress">The maximum absolute address to find a buffer in.</param>
        public BufferAllocationProperties FindBufferLocation(int size, IntPtr minimumAddress, IntPtr maximumAddress)
        {
            var memoryPages = MemoryPages.GetPages(_process);
            int bufferSize  = GetBufferSize(size);

            // Find a free page that satisfies the necessary properties to create a new buffer there.
            foreach (var page in memoryPages)
            {
                var pointer = GetBufferPointerInProximity(page, bufferSize, minimumAddress, maximumAddress);
                if (pointer != IntPtr.Zero)
                    return new BufferAllocationProperties(pointer, bufferSize);
            }

            throw new Exception($"Unable to find memory location to fit MemoryBuffer of size {size} ({bufferSize}) between {minimumAddress.ToString("X")} and {maximumAddress.ToString("X")}.");
        }

        /// <summary>
        /// Finds an appropriate location where a <see cref="MemoryBuffer"/>;
        /// or other memory allocation could be performed inside the target process.
        /// </summary>
        /// <param name = "size" > The minimum size in bytes of the buffer to be allocated.</param>
        /// <param name="targetAddress">The target address near which the buffer address is to be found.</param>
        /// <param name="proximity">The proximity in bytes for the target address.</param>
        public BufferAllocationProperties FindBufferLocation(int size, IntPtr targetAddress, long proximity)
        {
            IntPtr minimumAddress = (IntPtr)((long)targetAddress - proximity);
            IntPtr maximumAddress = (IntPtr)((long)targetAddress + proximity);
            return FindBufferLocation(size, minimumAddress, maximumAddress);
        }

        /// <summary>
        /// Creates a <see cref="MemoryBuffer"/> that satisfies at least; the
        /// specified minimum amount of memory specified by the parameter.
        /// </summary>
        public MemoryBuffer CreateMemoryBuffer(int size)
        {
            var bufferProperties = FindBufferLocation(size);
            return new MemoryBuffer(_process, bufferProperties.MemoryAddress, bufferProperties.Size);
        }

        /// <summary>
        /// Creates a <see cref="MemoryBuffer"/> that satisfies a set size constraint
        /// and proximity to a set address.
        /// </summary>
        /// <param name="size">The size of the buffer.</param>
        /// <param name="targetAddress">The target address within which the new buffer should be located with the specified proximity.</param>
        /// <param name="proximity">The proximity in bytes for the target address.</param>
        public MemoryBuffer CreateMemoryBuffer(int size, IntPtr targetAddress, long proximity)
        {
            var bufferProperties = FindBufferLocation(size, targetAddress, proximity);
            return new MemoryBuffer(_process, bufferProperties.MemoryAddress, bufferProperties.Size);
        }


        /// <summary>
        /// Creates a <see cref="MemoryBuffer"/> that satisfies a set size constraint
        /// and proximity to a set address.
        /// </summary>
        /// <param name="size">The size of the buffer.</param>
        /// <param name="minimumAddress">The minimum absolute address to find a buffer in.</param>
        /// <param name="maximumAddress">The maximum absolute address to find a buffer in.</param>
        public MemoryBuffer CreateMemoryBuffer(int size, IntPtr minimumAddress, IntPtr maximumAddress)
        {
            var bufferProperties = FindBufferLocation(size, minimumAddress, maximumAddress);
            return new MemoryBuffer(_process, bufferProperties.MemoryAddress, bufferProperties.Size);
        }


        /*
            -------------------
            Core Helper Methods
            -------------------
        */

        /// <summary>
        /// Returns a list of already known buffers that satisfy size requirements.
        /// </summary>
        /// <param name="size">The amount of bytes a buffer must have minimum.</param>
        /// <param name="createBuffer">If this is set to true; if no buffer satisfies the conditions; a new one will be created.</param>
        /// <returns></returns>
        public MemoryBuffer[] GetBuffers(int size, bool createBuffer)
        {
            // Get buffers already existing in process.
            var buffers = _bufferSearcher.GetBuffers(size);

            if (buffers.Length < 1 && createBuffer)
                return new[] { CreateMemoryBuffer(size) };
            else
                return buffers;
        }

        /// <summary>
        /// Returns a list of already known buffers that satisfy size requirements within a set proximity
        /// of a target address.
        /// </summary>
        /// <param name="size">The amount of bytes a buffer must have minimum.</param>
        /// <param name="createBuffer">If this is set to true; if no buffer satisfies the conditions; a new one will be created.</param>
        /// <param name="targetAddress">The target address within which the new buffer should be located with the specified proximity.</param>
        /// <param name="proximity">The proximity in bytes for the target address.</param>
        /// <returns></returns>
        public MemoryBuffer[] GetBuffers(int size, bool createBuffer, IntPtr targetAddress, int proximity)
        {
            // Get buffers already existing in process.
            var buffers = _bufferSearcher.GetBuffers(size);

            if (buffers.Length < 1 && createBuffer)
                return new[] { CreateMemoryBuffer(size, targetAddress, proximity) };
            else
                return buffers;
        }

        /*
            -----------------------
            Internal Helper Methods
            -----------------------
        */

        /// <summary>
        /// Calculates the size of a <see cref="MemoryBuffer"/> to be created that is a rounded
        /// multiple of the minimum allocation granularity.
        /// </summary>
        /// <param name="size">The size of the buffer to be allocated.</param>
        /// <returns>A calculated buffer size based off of the requested capacity in bytes.</returns>
        private int GetBufferSize(int size)
        {
            // Get size of buffer; allocation granularity or larger if greater than the granularity.
            Kernel32.GetSystemInfo(out var systemInfo);

            // Guard to ensure hat page size is at least the minimum supported by the processor
            // While Reloaded is only intended for X86/64; this may be useful in the future.
            // The second guard ensured the default page size is aligned with the system info.
            int pageSize = DefaultPageSize;
            if (systemInfo.dwPageSize > pageSize || (pageSize % systemInfo.dwPageSize != 0))
                pageSize = (int)systemInfo.dwPageSize;

            return Mathematics.RoundUp(size, pageSize);
        }

        /// <summary>
        /// This function returns true if a buffer can be created inside the page specified by memoryInformation
        /// with a specified buffer size and within the minimum and maximum ptr.
        /// </summary>
        /// <param name="pageInfo">Contains the information about a singular memory page.</param>
        /// <param name="bufferSize">The size that a MemoryBuffer would occupy.</param>
        /// <param name="minimumPtr">The maximum pointer a MemoryBuffer can occupy.</param>
        /// <param name="maximumPtr">The minimum pointer a MemoryBuffer can occupy.</param>
        /// <returns>Zero if the operation fails; otherwise any other value.</returns>
        private IntPtr GetBufferPointerInProximity(Kernel32.MEMORY_BASIC_INFORMATION pageInfo, int bufferSize, IntPtr minimumPtr, IntPtr maximumPtr)
        {
            // TODO: Wait for own patch to be approved to Vanara.
            // The structure returned is incorrectly defined and fails on x64 architectures.
            // https://github.com/dahall/Vanara/pull/21
            Kernel32.GetSystemInfo(out var systemInfo);

            // End of page.
            IntPtr pagePtrEnd               = (IntPtr)((long)pageInfo.BaseAddress + (long)pageInfo.RegionSize.Value);

            // Create potential buffer ranges aligned to allocation granularity.
            // Note: bufferSize should be pre-aligned.
            IntPtr startRegionStartAddress  = (IntPtr)Mathematics.RoundUp((long)pageInfo.BaseAddress, (long)systemInfo.dwAllocationGranularity);
            IntPtr startRegionEndAddress    = startRegionStartAddress + bufferSize;

            IntPtr endRegionStartAddress    = (IntPtr)Mathematics.RoundDown((long)pagePtrEnd - (long)bufferSize, systemInfo.dwAllocationGranularity);
            IntPtr endRegionEndAddress      = endRegionStartAddress + bufferSize;

            if (pageInfo.State == (uint)Kernel32.MEM_ALLOCATION_TYPE.MEM_FREE)
            {
                // Define possible buffer creation locations at start and end of buffer.
                var bufferStartRange        = new Mathematics.AddressRange(startRegionStartAddress, startRegionEndAddress);
                var bufferEndRange          = new Mathematics.AddressRange(endRegionStartAddress, endRegionEndAddress);
                var minimumMaximumRange     = new Mathematics.AddressRange(minimumPtr, maximumPtr);
                var freeMemoryRange         = new Mathematics.AddressRange(pageInfo.BaseAddress, pageInfo.BaseAddress + (int)pageInfo.RegionSize.Value);

                // The address where the new buffer will be created.
                IntPtr address = IntPtr.Zero;

                if (minimumMaximumRange.Contains(bufferStartRange) && freeMemoryRange.Contains(bufferStartRange))
                    address = bufferStartRange.StartPointer;
                else if (minimumMaximumRange.Contains(bufferEndRange) && freeMemoryRange.Contains(bufferEndRange))
                    address = bufferEndRange.StartPointer;
                
                // Check if it actually can be allocated.
                if ((ulong)address >= (ulong)systemInfo.lpMinimumApplicationAddress &&
                    (ulong)address <= (ulong)systemInfo.lpMaximumApplicationAddress)
                {
                    return address;
                }
            }

            return IntPtr.Zero;
        }


    }
}
