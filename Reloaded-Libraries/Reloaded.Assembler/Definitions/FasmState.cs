using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Assembler.Definitions
{
    /// <summary>
    /// Defines the state of the FASM assembler after an assembly operation.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct FasmState
    {
        // TODO: Update this with proper pointer types when FASMX64 ever gets proper 64bit addressing.

        [FieldOffset(0)]
        public FasmResult Condition;

        [FieldOffset(4)]
        public int OutputLength;

        [FieldOffset(4)]
        public FasmErrors ErrorCode;

        /// <summary>
        /// 32bit pointer to the output bytes.
        /// </summary>
        [FieldOffset(8)]
        public int OutputData;

        /// <summary>
        /// 32bit pointer to the <see cref="FasmLineHeader"/> struct.
        /// </summary>
        [FieldOffset(8)]
        public int ErrorLine;
    }
}
