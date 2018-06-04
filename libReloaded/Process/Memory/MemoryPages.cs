using System;
using System.Collections.Generic;
using static Reloaded.Process.Native.Native;

namespace Reloaded.Process.Memory
{
    /// <summary>
    /// Provides various utiltity methods which allow for the query and retrieval of information regarding individual pages of
    /// RAM memory.
    /// </summary>
    public static unsafe class MemoryPages
    {
        /// <summary>
        /// Returns an individual list of various pages inside the current process' memory,
        /// </summary>
        /// <returns></returns>
        public static List<MEMORY_BASIC_INFORMATION> GetPages(this ReloadedProcess reloadedProcess)
        {
            // Retrieve the system information which gives us the range of all pages available for DLL + EXE
            GetSystemInfo(out var systemInfo);

            // Used to iterate our memory pages, get our address range of pages.
            ulong currentAddress;
            ulong maxAddress;
            ulong maxIntPtr; // Contains the maximum addressable address.
            if (IntPtr.Size == 4)
            {
                currentAddress = (uint)systemInfo.minimumApplicationAddress;
                maxAddress = (uint)systemInfo.maximumApplicationAddress;
                maxIntPtr = Int32.MaxValue;
            }
            else
            {
                currentAddress = (ulong)systemInfo.minimumApplicationAddress;
                maxAddress = (ulong)systemInfo.maximumApplicationAddress;
                maxIntPtr = Int64.MaxValue;
            }

            // Shorthand for convenience.
            IntPtr processHandle = reloadedProcess.ProcessHandle;
            List<MEMORY_BASIC_INFORMATION> memoryPages = new List<MEMORY_BASIC_INFORMATION>();

            // Until we get all of the pages.
            while (currentAddress < maxAddress && currentAddress < maxIntPtr)
            {
                // Get our info from VirtualQueryEx.
                MEMORY_BASIC_INFORMATION memoryInformation = new MEMORY_BASIC_INFORMATION();
                VirtualQueryEx(processHandle, (IntPtr)currentAddress, out memoryInformation, (uint)sizeof(MEMORY_BASIC_INFORMATION));

                // Add the page and increment address iterator to go to next page.
                memoryPages.Add(memoryInformation);
                currentAddress += (ulong)memoryInformation.RegionSize;
            }

            return memoryPages;
        }
    }
}
