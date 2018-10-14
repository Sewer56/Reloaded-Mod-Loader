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
using System.Text;
using Reloaded.Process.Memory;

namespace Reloaded.Process
{
    /// <summary>
    /// Provides a means by which a target DLLs may be injected into an individual process.
    /// If the target process is running the administrator, the injector should also be
    /// ran as administrator.
    /// </summary>
    public class DllInjector
    {
        /// <summary>
        /// A handle to the Kernel32 Module, whereby LoadLibraryW is stored.
        /// </summary>
        private static IntPtr Kernel32Handle { get; set; }

        /// <summary>
        /// Pointer to the address of the exported LoadLibraryW function of Kernel32. 
        /// Allows for loading a specified module into the address space of the calling process. 
        /// </summary>
        private static IntPtr LoadLibraryAddress { get; set; }

        /// <summary>
        /// The handle to the process that is to be injected. The handle can be obtained by 
        /// Process.Handle for an existing process or found via various other means.
        /// </summary>
        private ReloadedProcess Process { get; }

        /// <summary>
        /// Initializes the DLL Injector of Reloaded Mod Loader.
        /// Once the DLL Injector is initialted, DLL injection may be performed by calling
        /// InjectDLL();
        /// </summary>
        /// <param name="process">
        /// The process object that is to be used. 
        /// The handle can be obtained by Process.Handle or found via various other means.
        /// </param>
        public DllInjector(ReloadedProcess process)
        {
            // Set the Location and Handle to the Process to be Injected.
            Process = process;

            // This should automatically resolve to kernel32.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            Kernel32Handle = Native.Native.LoadLibraryW("kernel32");

            // Retrieves the address to the LoadLibraryW function.
            // We will later call LoadLibraryW inside the target process using CreateRemoteThread,
            // which will load our own wanted DLL (game modification) inside of the target process.
            LoadLibraryAddress = Native.Native.GetProcAddress(Kernel32Handle, "LoadLibraryW");
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
        public void InjectDll(string libraryPath, IntPtr parameterPointer, string functionToExecute)
        {
            // Write the module name in process memory to be used by Kernel32's LoadLibraryW function.
            IntPtr libraryNameMemoryAddress = WriteModuleNameBytes(libraryPath);

            // Call Kernel32's LoadLibrary function and execute it within the target process.
            // Execution within the target process works by creating a thread within the virtual address space of the target process.
            // Our library will now be loaded within the address space of the game/game module to use and ready for us to execute.
            // This means that the game will now be able to access it ;)
            IntPtr moduleBaseAddress = (IntPtr)Process.CallLibrary(LoadLibraryAddress, libraryNameMemoryAddress);

            // Get the address of the "functionToExecute" function by loading the library into
            // ourselves. Then take away from the module base address to retrieve the offset of the requested
            // function. By sheer chance, the handle happens to be module base address :^)
            IntPtr mainFunctionOffset = Libraries.GetLibraryFunctionOffset(libraryPath, functionToExecute);
            IntPtr functionAddress = (IntPtr)((long) moduleBaseAddress + (long)mainFunctionOffset);

            // Call our main function within the target executable.
            Process.CallLibrary(functionAddress, parameterPointer);
        }

        /// <summary>
        /// Writes the module name and target path as a series of bytes into the executable memory space.
        /// This is such that we may call the LoadLibaryA of Kernel32 to load our DLL module inside
        /// the virtual memory space of the target process.
        /// </summary>
        /// <param name="libraryPath">The path to the DLL library to be written to memory.</param>
        /// <returns>The address at which the library path was written to.</returns>
        private IntPtr WriteModuleNameBytes(string libraryPath)
        {
            // Retrieve the library name as a series of bytes.
            byte[] libraryNameBytes = Encoding.Unicode.GetBytes(libraryPath);

            // Get the pointer to the freed up memory for the library name.
            IntPtr processPointer = Process.Memory.Allocate(libraryNameBytes.Length);

            // Write the name of the library to the memory space of the process.
            Process.Memory.WriteRaw(processPointer, libraryNameBytes);

            // Return the address of written path.
            return processPointer;
        }
    }
}
