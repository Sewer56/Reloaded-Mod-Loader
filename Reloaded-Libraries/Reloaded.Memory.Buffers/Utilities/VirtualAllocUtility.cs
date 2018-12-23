using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers.Utilities
{
    public static unsafe class VirtualAllocUtility
    {
        public delegate IntPtr VirtualAllocFunction(IntPtr processHandle, IntPtr address, ulong size);

        /// <summary>
        /// Retrieves the function to use in place of VirtualAlloc.
        /// Returns VirtualAlloc if target is same process; else VirtualAllocEx
        /// </summary>
        /// <param name="targetProcess">The process which the VirtualAlloc call intends to target.</param>
        /// <returns>A delegate implementation of <see cref="VirtualAllocFunction"/></returns>
        public static VirtualAllocFunction GetVirtualAllocFunction(Process targetProcess)
        {
            // Get the VirtualQuery function implementation to use.
            // Local is faster and works for current process; Remote is for another process.
            VirtualAllocFunction virtualAllocFunction = VirtualAllocRemote;

            if (Process.GetCurrentProcess().Id == targetProcess.Id)
                virtualAllocFunction = VirtualAllocLocal;

            return virtualAllocFunction;
        }

        /*
         * Two implementations of VirtualAlloc for the VirtualAlloc Delegate
         * The Local one; runs it for the current process; being the faster of the two.
         * The Remote one; runs it for another process; being the slower of the two.
         */

        private static IntPtr VirtualAllocLocal(IntPtr processHandle, IntPtr address, ulong size)
        {
            return Kernel32.VirtualAlloc
            (
                address,
                size,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE | Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );
        }

        private static IntPtr VirtualAllocRemote(IntPtr processHandle, IntPtr address, ulong size)
        {
            return Kernel32.VirtualAllocEx
            (
                processHandle,
                address,
                size,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE | Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT,
                Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE
            );
        }
    }
}
