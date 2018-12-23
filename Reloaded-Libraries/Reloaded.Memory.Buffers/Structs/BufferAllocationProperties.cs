using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory.Buffers.Structs
{
    public struct BufferAllocationProperties
    {
        public IntPtr MemoryAddress;
        public int Size;

        public BufferAllocationProperties(IntPtr memoryAddress, int size)
        {
            MemoryAddress = memoryAddress;
            Size = size;
        }
    }
}
