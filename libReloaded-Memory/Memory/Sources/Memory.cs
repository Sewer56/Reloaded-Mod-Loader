using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    public unsafe class Memory : IMemory
    {
        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        public T       Read<T>(IntPtr memoryAddress, bool marshal)
        {
            return marshal ? Marshal.PtrToStructure<T>(memoryAddress) : Unsafe.Read<T>((void*)memoryAddress);
        }

        public void    Write<T>(IntPtr memoryAddress, ref T item, bool marshal)
        {
            if (marshal)
                Marshal.StructureToPtr(item, memoryAddress, false);
            else
                Unsafe.Write((void*)memoryAddress, item);
        }

        /*
            --------------------------------
            Change Permission Implementation
            --------------------------------
        */

        public Kernel32.MEM_PROTECTION ChangePermission(IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions)
        {
            Kernel32.VirtualProtect(memoryAddress, (uint)size, newPermissions, out var oldPermissions);
            return oldPermissions;
        }
    }
}
