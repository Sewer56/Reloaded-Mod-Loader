using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Utilities
{
    public static class CheckArchitecture
    {
        /// <summary>
        /// Retrieves the architecture/machine type of a specific executable.
        /// </summary>
        /// <param name="filePath">The file path of an execuatble file to be read and checked.</param>
        /// <returns></returns>
        public static PEMachineType GetMachineTypeFromPeHeader(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                // Get address of new EXE header.
                fileStream.Seek(0x3C, SeekOrigin.Begin);            // Navigate to `LONG AddressOfNewExeHeader`
                uint exeHeaderAddress = binaryReader.ReadUInt32();  // Get our address of IMAGE_NT_HEADERS

                // Navigate to IMAGE_FILE_HEADER struct, + offset of IMAGE_FILE_HEADER.
                fileStream.Seek(exeHeaderAddress + 0x04, SeekOrigin.Begin);
                return (PEMachineType) binaryReader.ReadUInt16();    // Read IMAGE_MACHINE.
            }
        }

        /// <summary>
        /// Contains a listing of different machine types a PE header may contain.
        /// </summary>
        public enum PEMachineType
        {
            AMD64   = 34404,
            ARM     = 452,
            I386    = 332,
            IA64    = 512
        }
    }
}
