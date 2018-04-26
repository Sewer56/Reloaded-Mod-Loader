/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Reloaded.Utilities
{
    /// <summary>
    /// Extracts an embedded file to disk which has been originally embedded and compressed by Fody.Costura.
    /// </summary>
    internal static class ExtractFodyCosturaFile
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

                var embeddedAssemblerStream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "costura." + lowerCase + ".compressed" );
                if ( embeddedAssemblerStream == null )
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
