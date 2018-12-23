using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace Reloaded.Utilities.Fody
{
    public static class Fody
    {
        /// <summary>
        /// Extracts an embedded resource which has originally been compressed by Fody.Costura.
        /// Output location is the location of the executing DLL, unless specified.
        /// </summary>
        /// <param name="resourceName">The name of the resource to extract to disk. e.g. ReloadedAssembler.exe</param>
        /// <returns>true if the operation succeeded, else false</returns>
        public static bool ExtractResource(string resourceName)
        {
            try
            {
                // Get executing assembly folder.
                string currentAssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return ExtractResource(resourceName, currentAssemblyFolder);
            }
            catch { return false; }
        }

        /// <summary>
        /// Extracts an embedded resource which has originally been compressed by Fody.Costura.
        /// Output folder is user specified.
        /// </summary>
        /// <param name="resourceName">The name of the resource to extract to disk. e.g. ReloadedAssembler.exe</param>
        /// <param name="folderLocation">The folder to extract the file to</param>
        /// <returns>true if the operation succeeded, else false</returns>
        public static bool ExtractResource(string resourceName, string folderLocation)
        {
            try
            {
                // Costura embeds names with lowercase.
                string lowerCase = resourceName.ToLower();

                var embeddedAssemblerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("costura." + lowerCase + ".compressed");
                if (embeddedAssemblerStream == null)
                    return false;

                using (Stream outputFileStream = new FileStream(folderLocation + "//" + resourceName, FileMode.Create, FileAccess.Write))
                using (Stream decompressStream = new DeflateStream(embeddedAssemblerStream, CompressionMode.Decompress))
                {
                    decompressStream.CopyTo(outputFileStream);
                }

                return true;
            }
            catch { return false; }
        }
    }
}
