using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Misc
{
    /// <summary>
    /// Extracts an embedded file to disk which has been originally embedded and compressed by Fody.Costura.
    /// </summary>
    class ExtractFodyCosturaFile
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
            catch  { return false; }
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

                using (Stream outputFileStream = new FileStream(folderLocation + "//" + resourceName, FileMode.Create, FileAccess.Write))
                using (Stream embeddedAssemblerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("costura." + lowerCase + ".compressed"))
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
