using System.Runtime.InteropServices;

namespace Reloaded.Misc
{
    /// <summary>
    /// Allows for the unblocking of files that are by default blocked by windows
    /// with the removal of Zone Identifiers for files such that Zone Information 
    /// is not stored.
    /// </summary>
    static class FileUnblocker
    {
        /// <summary>
        /// Deletes an existing file.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <returns>True (nonzero) if the function succeeds.</returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFile(string name);

        /// <summary>
        /// Atempts to remove the zone identifier of a file with a specific file name.
        /// </summary>
        public static bool Unblock(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }

    }
}
