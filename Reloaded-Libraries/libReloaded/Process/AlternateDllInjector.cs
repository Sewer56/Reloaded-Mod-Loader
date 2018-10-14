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

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using PeNet;
using Reloaded.Process.Memory;

/*
 * A remainment of a small experiment with custom style DLL injection.
 * This experiement was incomplete, it was just experimentation ground.
 * :)
 * Maybe I'll come back to it later.
 * Requires PeNet library.
*/

namespace Reloaded.Process
{
    /// <summary>
    /// Provides an alternative to the known and reliable <see cref="DllInjector"/>.
    /// This alternative is intended to be used if for instance the process has been manually suspended
    /// and you require to inject a DLL into said process, in which case trying to use LoadLibraryA
    /// can be hopeless.
    /// </summary>
    public class AlternateDllInjector
    {
        /// <summary>
        /// Contains the details of the individual PE file for the DLL to inject.
        /// </summary>
        private PeFile PortableExecutable { get; set; }

        /// <summary>
        /// The handle to the process that is to be injected. The handle can be obtained by 
        /// Process.Handle for an existing process or found via various other means.
        /// </summary>
        private ReloadedProcess Process { get; }

        /// <summary>
        /// Initializes the alternate DLL Injector of Reloaded Mod Loader.
        /// Once the DLL Injector is initialted, DLL injection may be performed by calling
        /// InjectDLL();
        /// </summary>
        /// <param name="process">
        /// The process object that is to be used. 
        /// The handle can be obtained by Process.Handle or found via various other means.
        /// </param>
        public AlternateDllInjector(ReloadedProcess process)
        {
            // Set the Location and Handle to the Process to be Injected.
            Process = process;
        }

        /// <summary>
        /// Injects a DLL onto the set target process.
        /// If the target process is running the administrator, the injector should also be
        /// ran as administrator.
        /// </summary>
        /// <param name="libraryPath">The absolute path to your DLL to be injected.</param>
        /// <param name="parameterPointer">
        /// Singular parameter to pass onto the library main method to be called.
        /// This should be a pointer to a structure in memory or single integer value
        /// already in the target process (i.e. you need to write your parameterPointer structure into memory manually).
        /// If this parameterPointer is null, a value of 0/null is passed.
        /// </param>
        /// <param name="functionToExecute">The name of the function to be executed inside of the target process. </param>
        /// <returns>True if the function exists, else false.</returns>
        public bool InjectDll(string libraryPath, IntPtr parameterPointer, string functionToExecute)
        {
            // Read the DLL into memory such that we can write it to game process.
            byte[] dllBytes = File.ReadAllBytes(libraryPath);

            // Allocate the memory for the DLL, we will need to slightly patch the DLL before we parse it and do stuff.
            IntPtr moduleBaseAddress = Process.AllocateMemory(dllBytes.Length);

            // Parse the contents of the DLL file.
            PortableExecutable = new PeFile(dllBytes);

            // Search for our function and execute.
            foreach (ExportFunction exportedFunction in PortableExecutable.ExportedFunctions)
            {
                if (exportedFunction.Name == functionToExecute)
                {
                    // Write the contents of our own DLL into the target space of the process manually through
                    // reading the contents of it into a byte array, allocating adequate memory and writing our
                    // own DLL to the memory space of said process.
                    Debugger.Launch();
                    Process.WriteMemoryExternal(moduleBaseAddress, PortableExecutable.Buff);

                    // Obtains the address pointed to from our export table, this address points to a jmp stub which contains
                    // the real location of our function to execute, albeit with unfortunately the old module base.
                    IntPtr exportAddress = (IntPtr)((long)moduleBaseAddress + RelativeVirtualAddressToOffset(exportedFunction.Address));

                    // From here we patch the absolute jump address source, patch a few exports and execute.
                    // Code for this is left out, I'll come back to this one in the future.
                    // It's practically a manual map.

                    // Call our main function within the target executable.
                    Process.CallLibrary(exportAddress, parameterPointer);
                    return true;
                }
            }

            // Our function name not found.
            return false;
        }

        /// <summary>
        /// Converts a specific Relative Virtual Address such as an exported function address
        /// to an offset from the base of the DLL module.
        /// Assumes <see cref="PortableExecutable "/> is valid and set.
        /// </summary>
        /// <param name="relativeVirtualAddress">
        ///     The relative virtual address: Address of an item after it is loaded into memory,
        ///     with the base address of the image file subtracted from it.
        /// </param>
        public long RelativeVirtualAddressToOffset(long relativeVirtualAddress)
        {
            // Probably not a proper relative virtual address.
            if (relativeVirtualAddress < PortableExecutable.ImageSectionHeaders[0].PointerToRawData)
                return relativeVirtualAddress;

            // Iterate over the image section headers
            foreach (var imageSectionHeader in PortableExecutable.ImageSectionHeaders)
            {
                // Check if the relative virtual address falls within the specified range of an image section
                // header entry, that being between the virtual address and the end of the virtual address
                // raw data.
                if (relativeVirtualAddress >= imageSectionHeader.VirtualAddress &&
                    relativeVirtualAddress < imageSectionHeader.VirtualAddress + imageSectionHeader.SizeOfRawData)
                {
                    // Subtract the relative address from the pointer to the raw data of this section header.
                    return (relativeVirtualAddress - imageSectionHeader.VirtualAddress + imageSectionHeader.PointerToRawData);
                }
            }

            return 0;
        }
    }
}
