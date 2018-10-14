using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    public unsafe class Memory : IMemory
    {
        /// <summary>
        /// Allows you to access the memory for the currently running process.
        /// </summary>
        public static Memory Current = new Memory();

        /*
            -------------------------
            Read/Write Implementation
            -------------------------
        */

        public T       Read<T>(IntPtr memoryAddress, bool marshal)
        {
            return marshal ? Marshal.PtrToStructure<T>(memoryAddress) : Unsafe.Read<T>((void*)memoryAddress);
        }

        public byte[]  ReadRaw(IntPtr memoryAddress, int length)
        {
            byte[] rawData = new byte[length];
            Marshal.Copy(memoryAddress, rawData, 0, rawData.Length);
            return rawData;
        }

        public void    Write<T>(IntPtr memoryAddress, ref T item, bool marshal)
        {
            if (marshal)
                Marshal.StructureToPtr(item, memoryAddress, false);
            else
                Unsafe.Write((void*)memoryAddress, item);
        }

        public void    WriteRaw(IntPtr memoryAddress, byte[] data)
        {
            Marshal.Copy(data, 0, memoryAddress, data.Length);
        }

        public IntPtr  Allocate(int length)
        {
            return Marshal.AllocHGlobal(length);
        }

        public bool    Free(IntPtr address)
        {
            Marshal.FreeHGlobal(address);
            return true;
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
