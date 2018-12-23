using System;
using System.Collections.Generic;
using System.Diagnostics;
using Reloaded.Memory.Buffers.Utilities;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers
{
    /// <summary>
    /// Provides various utility methods which allow for the query and retrieval of information regarding individual pages of
    /// RAM memory.
    /// </summary>
    public static unsafe class MemoryPages
    {
        /// <summary>
        /// Returns a list of pages that exist within a set process' memory.
        /// </summary>
        /// <returns></returns>
        public static List<Kernel32.MEMORY_BASIC_INFORMATION> GetPages(Process process)
        {
            // Get the minimum and maximum memory address boundaries.
            var memoryLimits = GetMemoryLimits();

            // Get the VirtualQuery function implementation to use.
            // Local is faster and works for current process; Remote is for another process.
            VirtualQueryUtility.VirtualQueryFunction virtualQueryFunction = VirtualQueryUtility.GetVirtualQueryFunction(process);
            
            // Shorthand for convenience.
            IntPtr processHandle = process.Handle;
            List<Kernel32.MEMORY_BASIC_INFORMATION> memoryPages = new List<Kernel32.MEMORY_BASIC_INFORMATION>();

            // Until we get all of the pages.
            while (memoryLimits.CurrentAddress < memoryLimits.MaxAddress)
            {
                // Get our info from VirtualQueryEx.
                var memoryInformation = virtualQueryFunction(processHandle, (IntPtr)memoryLimits.CurrentAddress);

                // Add the page and increment address iterator to go to next page.
                memoryPages.Add(memoryInformation);
                memoryLimits.CurrentAddress += memoryInformation.RegionSize.Value;
            }

            return memoryPages;
        }

        /// <summary>
        /// Returns the memory limitations that state the maximum and minimum
        /// addressable addresses by executables and DLLs.
        /// </summary>
        /// <returns></returns>
        private static MemoryLimits GetMemoryLimits()
        {
            // Retrieve the system information which gives us the range of all pages available for DLL + EXE
            Kernel32.GetSystemInfo(out var systemInfo);

            // Used to iterate our memory pages, get our address range of pages.
            return new MemoryLimits((ulong)systemInfo.lpMinimumApplicationAddress, (ulong)systemInfo.lpMaximumApplicationAddress);
        }

        /* Supplementary Structures */

        private struct MemoryLimits
        {
            /// <summary>
            /// Contains the current addressable address; initializes as the minimum.
            /// </summary>
            public ulong CurrentAddress;

            /// <summary>
            /// Contains the maximum addressable for applications and DLLs.
            /// </summary>
            public ulong MaxAddress;

            public MemoryLimits(ulong currentAddress, ulong maxAddress)
            {
                CurrentAddress = currentAddress;
                MaxAddress = maxAddress;
            }
        }
    }
}
