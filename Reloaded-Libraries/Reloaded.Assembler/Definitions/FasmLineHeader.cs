using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Assembler.Definitions
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe class FasmLineHeader
    {
        /// <summary>
        /// 32bit pointer to the file path of the source file.
        /// </summary>
        [FieldOffset(0)]
        public int FilePathPtr;

        [FieldOffset(4)]
        public int LineNumber;

        [FieldOffset(8)]
        public int FileOffset;

        /// <summary>
        /// 32bit pointer to the LINE_HEADER structure for the line which called the macroinstruction.
        /// </summary>
        [FieldOffset(8)]
        public int MacroCallingFilePtr;

        /// <summary>
        /// 32bit pointer to the LINE_HEADER structure for the line within the definition of macroinstruction, which generated this one.
        /// </summary>
        [FieldOffset(12)]
        public int MacroLinePtr;
    }
}
