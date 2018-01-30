using System.IO;
using System.Linq;

namespace SonicHeroes.Misc
{
    /// <summary>
    /// Removes invalid characters from a specified path.
    /// Used for sanitization of controller names and devices which contain symbols
    /// which would form invalid paths.
    /// </summary>
    static class PathSanitizer
    {
        // Set Invalid Path Characters
        static char[] invalid = Path.GetInvalidPathChars().Union(Path.GetInvalidFileNameChars()).ToArray();

        /// <summary>
        /// Removes invalid characters from a specified path.
        /// Used for sanitization of controller names and devices which contain symbols
        /// which would form invalid paths.
        /// </summary>
        /// <returns>Original string without characters deemed invalid in a file path.</returns>
        public static string ForceValidFilePath(string text)
        {
            // Valid path force
            foreach (char c in invalid)
            {
                // Ignore paths
                if (c != '\\' || c != '/') { text = text.Replace(c.ToString(), ""); }
            }

            return text;
        }
    }
}
