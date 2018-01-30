using System;
using System.Runtime.InteropServices;

namespace SonicHeroes.IO.Native
{
    /// <summary>
    /// Provides native functions used for input and output operations.
    /// </summary>
    public static class Native
    {
        /// <summary>
        /// Creates a hardlink for an already existing specific file elsewhere at another path.
        /// </summary>
        /// <param name="lpFileName">The name of the new file. This parameter may include the path but cannot specify the name of a directory.</param>
        /// <param name="lpExistingFileName">The name of the existing file. This parameter may include the path cannot specify the name of a directory.</param>
        /// <param name="lpSecurityAttributes">Reserved, should be set to null (IntPtr.Zero).</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
    }
}
