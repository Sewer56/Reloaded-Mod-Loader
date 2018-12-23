using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Reloaded.Memory.Buffers.Utilities;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers
{
    /// <summary>
    /// Utility class which searches for existing Reloaded memory buffers within an executable
    /// and maintains a dictionary of address to buffer mappings.
    /// </summary>
    public class MemoryBufferSearcher
    {
        /// <summary>
        /// Maintains address to buffer mappings.
        /// </summary>
        private Dictionary<IntPtr, MemoryBuffer> _knownBuffers = new Dictionary<IntPtr, MemoryBuffer>(100);

        /// <summary>
        /// The process in which the buffers are being searched for.
        /// </summary>
        private Process _process;

        /// <summary>
        /// Creates a new instance of <see cref="MemoryBufferSearcher"/> that can search for existing Reloaded Memory buffers
        /// within an executable.
        /// </summary>
        /// <param name="targetProcess">The process in which to search for buffers for.</param>
        public MemoryBufferSearcher(Process targetProcess)
        {
            _process = targetProcess;
        }

        /// <summary>
        /// Scans the set targeted process for any buffers
        /// managed by Reloaded and returns a list of available buffers.
        /// </summary>
        /// <returns>A list of available Reloaded Buffers to be used.</returns>
        public MemoryBuffer[] FindBuffers()
        {
            // Get a list of all pages.
            var memoryBasicInformation = MemoryPages.GetPages(_process);

            // Check if each page is the start of a buffer, and add it conditionally.
            for (int x = 0; x < memoryBasicInformation.Count; x++)
            {
                if (memoryBasicInformation[x].State == (uint)(Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE | Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT) &&
                    memoryBasicInformation[x].Type == (uint)Kernel32.MEM_ALLOCATION_TYPE.MEM_PRIVATE &&
                    MemoryBuffer.IsBuffer(_process, memoryBasicInformation[x].BaseAddress))
                {
                    var address = memoryBasicInformation[x].BaseAddress;
                    MemoryBuffer buffer = MemoryBuffer.FromAddress(_process, address);

                    // Add the buffer to list if the item fits.
                    if (! _knownBuffers.ContainsKey(address))
                        _knownBuffers.Add(address, buffer);
                }
            }

            return _knownBuffers.Values.ToArray();
        }


        /// <summary>
        /// Returns a list of already known buffers that satisfy the passed in size requirements
        /// in bytes. If no known buffers are found; a scan will be performed for any existing not yet
        /// known buffers.
        /// </summary>
        /// <param name="size">The amount of bytes a buffer must have minimum.</param>
        /// <returns></returns>
        public MemoryBuffer[] GetBuffers(int size)
        {
            // Return the already known buffers if there is a buffer that can fit the size.
            var memoryBuffers = _knownBuffers.Values.Where(x => x.CheckItemSize(size)).ToArray();
            if (memoryBuffers.Length > 0)
                return memoryBuffers;

            // No cached buffers meet the criteria; find a set of new buffers.
            memoryBuffers = FindBuffers();
            return memoryBuffers;
        }
    }
}