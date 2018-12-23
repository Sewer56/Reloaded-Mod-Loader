using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers.Utilities
{
    public static unsafe class VirtualQueryUtility
    {
        public delegate Kernel32.MEMORY_BASIC_INFORMATION VirtualQueryFunction(IntPtr processHandle, IntPtr address);

        /// <summary>
        /// Retrieves the function to use in place of VirtualQuery.
        /// Returns VirtualQuery if target is same process; else VirtualQueryEx
        /// </summary>
        /// <param name="targetProcess">The process which the VirtualQuery call intends to target.</param>
        /// <returns>A delegate implementation of <see cref="VirtualQueryFunction"/></returns>
        public static VirtualQueryFunction GetVirtualQueryFunction(Process targetProcess)
        {
            // Get the VirtualQuery function implementation to use.
            // Local is faster and works for current process; Remote is for another process.
            VirtualQueryFunction virtualQueryFunction = VirtualQueryRemote;

            if (Process.GetCurrentProcess().Id == targetProcess.Id)
                virtualQueryFunction = VirtualQueryLocal;

            return virtualQueryFunction;
        }

        /*
         * Two implementations of VirtualQuery for the VirtualQuery Delegate
         * The Local one; runs it for the current process; being the faster of the two.
         * The Remote one; runs it for another process; being the slower of the two.
         */

        private static Kernel32.MEMORY_BASIC_INFORMATION VirtualQueryLocal(IntPtr processHandle, IntPtr address)
        {
            var memoryInformation = new Kernel32.MEMORY_BASIC_INFORMATION();
            Kernel32.VirtualQuery(address, (IntPtr)(&memoryInformation), (uint)sizeof(Kernel32.MEMORY_BASIC_INFORMATION));
            return memoryInformation;
        }

        private static Kernel32.MEMORY_BASIC_INFORMATION VirtualQueryRemote(IntPtr processHandle, IntPtr address)
        {
            var memoryInformation = new Kernel32.MEMORY_BASIC_INFORMATION();
            Kernel32.VirtualQueryEx(processHandle, address, (IntPtr)(&memoryInformation), (uint)sizeof(Kernel32.MEMORY_BASIC_INFORMATION));
            return memoryInformation;
        }
    }
}
