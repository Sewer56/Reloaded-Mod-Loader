using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Process.Buffers
{
    /// <summary>
    /// Defines a structure which defines the individual header of a memory buffer managed by libReloaded.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MemoryBufferHeader
    {
        /// <summary>
        /// [Expected Value] Sits at the top of the buffer and identifies the buffer as libReloaded managed.
        /// </summary>
        public const long RELOADED_MAGIC = 0x1337200306032017;

        /// <summary>
        /// Sits at the top of the buffer and identifies the buffer as libReloaded managed.
        /// </summary>
        public long ReloadedMagic;

        /// <summary>
        /// Stores the current offset in the buffer, ranging from 0 to <see cref="BufferSize"/>.
        /// </summary>
        public uint BufferOffset;

        /// <summary>
        /// Stores the size of the individual buffer in question.
        /// </summary>
        public uint BufferSize;
    }
}
