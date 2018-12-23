using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Memory.Buffers.Structs
{
    /// <summary>
    /// Contains the individual details of the memory buffer in question.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBufferHeader
    {
        /// <summary>
        /// Stores the current offset in the buffer, ranging from 0 to <see cref="BufferSize"/>.
        /// </summary>
        public int BufferOffset;

        /// <summary>
        /// Stores the size of the individual buffer in question.
        /// </summary>
        public int BufferSize;

        public MemoryBufferHeader(int bufferOffset, int bufferSize)
        {
            BufferOffset = bufferOffset;
            BufferSize = bufferSize;
        }
    }
}
