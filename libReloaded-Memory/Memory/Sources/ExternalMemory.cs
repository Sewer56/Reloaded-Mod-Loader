using System;
using System.Diagnostics;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    public unsafe class ExternalMemory : IMemory
    {
        /// <summary>
        /// Contains the current process' memory.
        /// </summary>
        private static Memory _localMemory = new Memory();

        /// <summary>
        /// Contains the handle of the process used to read memory
        /// from and write memory to external process.
        /// </summary>
        private IntPtr _processHandle;
        
        /*
             --------------
             Constructor(s)
             --------------
        */

        /// <summary>
        /// Creates an instance of the <see cref="ExternalMemory"/> class used to read from an
        /// external process with a specified handle.
        /// </summary>
        /// <param name="processHandle">Handle of the process to read/write memory from.</param>
        public ExternalMemory(IntPtr processHandle)
        {
            _processHandle  = processHandle;
        }

        /// <summary>
        /// Creates an instance of the <see cref="ExternalMemory"/> class used to read from an
        /// external process with a specified handle.
        /// </summary>
        /// <param name="process">The individual process to read/write memory from.</param>
        public ExternalMemory(Process process)
        {
            _processHandle  = process.Handle;
        }


        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        public T Read<T>(IntPtr memoryAddress, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);
            byte[] buffer  = new byte[structSize];

            fixed (byte* bufferPtr = buffer)
            {
                Kernel32.ReadProcessMemory(_processHandle, memoryAddress, (IntPtr)bufferPtr, (uint)structSize, out var _);
                return Struct.FromPtr<T>((IntPtr)bufferPtr, marshal, _localMemory.Read<T>);
            }
        }

        public void Write<T>(IntPtr memoryAddress, ref T item, bool marshal)
        {
            byte[] bytes = Struct.GetBytes(ref item, marshal);

            fixed (byte* bytePtr = bytes)
                Kernel32.WriteProcessMemory(_processHandle, memoryAddress, (IntPtr)bytePtr, (uint)bytes.Length, out var _);
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        /* Implementation */
        public Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions)
        {
            Kernel32.VirtualProtectEx(_processHandle, memoryAddress, (uint)size, newPermissions, out Kernel32.MEM_PROTECTION oldPermissions);
            return oldPermissions;
        }
    }
}
