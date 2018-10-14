using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded_Mod_Template.Structs
{
    /// <summary>
    /// Defines the file name of an individual file inside a .ONE archive.
    /// </summary>
    public unsafe struct CustomFileHeader
    {
        /// <summary>
        /// Contains the actual filename of the file as ASCII encoded bytes.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;

        public uint Offset;
        public uint Length;
    }
}
