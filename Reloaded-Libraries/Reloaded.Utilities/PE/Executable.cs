using System;
using System.IO;
using Vanara.PInvoke;

namespace Reloaded.Utilities.PE
{
    public class Executable
    {
        /// <summary>
        /// Attempts to remove the zone identifier of a file with a specific file name.
        /// This unblocks files downloaded from the internet for some of those PCs that
        /// have group policy settings which disallow for running of DLLs or EXEs downloaded from the
        /// internet.
        /// </summary>
        /// <param name="filePath">Specifies the full path of the file to delete.</param>
        public static bool Unblock(string filePath)
        {
            return Kernel32.DeleteFile(filePath + ":Zone.Identifier");
        }

        /// <summary>
        /// Retrieves the architecture/machine type of a specific executable.
        /// </summary>
        /// <param name="filePath">The file path of an executable file to be read and checked.</param>
        /// <returns></returns>
        public static PEMachineType GetMachineType(string filePath)
        {
            try
            {
                using (FileStream fileStream     = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    // Get address of new EXE header.
                    fileStream.Seek(0x3C, SeekOrigin.Begin);           // Navigate to `LONG AddressOfNewExeHeader`
                    uint exeHeaderAddress = binaryReader.ReadUInt32(); // Get our address of IMAGE_NT_HEADERS

                    // Navigate to IMAGE_FILE_HEADER struct, + offset of IMAGE_FILE_HEADER.
                    fileStream.Seek(exeHeaderAddress + 0x04, SeekOrigin.Begin);
                    return (PEMachineType) binaryReader.ReadUInt16(); // Read IMAGE_MACHINE.
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failure in parsing PE header of an executable: {filePath} | {e.Message}");
            }
        }

        /// <summary>
        /// Contains a listing of different machine types a PE header may contain.
        /// </summary>
        public enum PEMachineType
        {
            AMD64 = 34404,
            ARM  = 452,
            I386 = 332,
            IA64 = 512
        }
    }
}
