using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Vanara.PInvoke;

namespace Reloaded.Memory.Buffers.Utilities
{
    public static unsafe class VirtualFreeUtility
    {
        public delegate bool VirtualFreeFunction(IntPtr processHandle, IntPtr address, ulong size);

        /// <summary>
        /// Retrieves the function to use in place of VirtualFree.
        /// Returns VirtualFree if target is same process; else VirtualFreeEx
        /// </summary>
        /// <param name="targetProcess">The process which the VirtualFree call intends to target.</param>
        /// <returns>A delegate implementation of <see cref="VirtualFreeFunction"/></returns>
        public static VirtualFreeFunction GetVirtualFreeFunction(Process targetProcess)
        {
            // Get the VirtualQuery function implementation to use.
            // Local is faster and works for current process; Remote is for another process.
            VirtualFreeFunction virtualFreeFunction = VirtualFreeRemote;

            if (Process.GetCurrentProcess().Id == targetProcess.Id)
                virtualFreeFunction = VirtualFreeLocal;

            return virtualFreeFunction;
        }

        /*
         * Two implementations of VirtualFree for the VirtualFree Delegate
         * The Local one; runs it for the current process; being the faster of the two.
         * The Remote one; runs it for another process; being the slower of the two.
         */

        private static bool VirtualFreeLocal(IntPtr processHandle, IntPtr address, ulong size)
        {
            return Kernel32.VirtualFree(address, size, Kernel32.MEM_ALLOCATION_TYPE.MEM_FREE);
        }

        private static bool VirtualFreeRemote(IntPtr processHandle, IntPtr address, ulong size)
        {
            return Kernel32.VirtualFreeEx(processHandle, address, size, Kernel32.MEM_ALLOCATION_TYPE.MEM_FREE);
        }
    }
}
