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
using System.Text;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Provides a means by which a target DLLs may be injected into an individual process.
    /// If the target process is running the administrator, the injector should also be
    /// ran as administrator.
    /// </summary>
    class DLLInjector
    {
        /// <summary>
        /// The name of the function to be executed inside of the target
        /// process. 
        /// </summary>
        public string FunctionToExecute { get; set; }

        /// <summary>
        /// A handle to the Kernel32 Module, whereby LoadLibraryA is stored.
        /// </summary>
        private static IntPtr Kernel32Handle { get; set; }

        /// <summary>
        /// Pointer to the address of the exported LoadLibraryA function of Kernel32. 
        /// Allows for loading a specified module into the address space of the calling process. 
        /// </summary>
        private static IntPtr LoadLibraryAddress { get; set; }

        /// <summary>
        /// The handle to the process that is to be injected. The handle can be obtained by 
        /// Process.Handle for an existing process or found via various other means.
        /// </summary>
        private Process Process { get; set; }

        /// <summary>
        /// Initializes the DLL Injector of Reloaded Mod Loader.
        /// Once the DLL Injector is initialted, DLL injection may be performed by calling
        /// InjectDLL();
        /// </summary>
        /// <param name="process">
        /// The process object that is to be used. 
        /// The handle can be obtained by Process.Handle or found via various other means.
        /// </param>
        public DLLInjector(Process process)
        {
            // Set the Location and Handle to the Process to be Injected.
            Process = process;

            // This should automatically resolve to kernel32.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            Kernel32Handle = Native.LoadLibrary("kernel32");

            // Retrieves the address to the LoadLibraryA function.
            // We will later call LoadLibraryA inside the target process using CreateRemoteThread,
            // which will load our own wanted DLL (game modification) inside of the target process.
            LoadLibraryAddress = Native.GetProcAddress(Kernel32Handle, "LoadLibraryA");

            // Set the name of the function we will execute in the target process.
            FunctionToExecute = "Main";
        }

        /// <summary>
        /// Injects a DLL onto the set target process.
        /// If the target process is running the administrator, the injector should also be
        /// ran as administrator.
        /// </summary>
        /// <param name="libraryPath">The absolute path to your DLL to be injected.</param>
        public void InjectDLL(string libraryPath)
        {
            // Write the module name in process memory to be used by Kernel32's LoadLibraryA function.
            IntPtr libraryNameMemoryAddress = WriteModuleNameBytes(libraryPath);

            // Call Kernel32's LoadLibrary function and execute it within the target process.
            // Execution within the target process works by creating a thread within the virtual address space of the target process.
            // Our library will now be loaded within the address space of the game/game module to use and ready for us to execute.
            // This means that the game will now be able to access it ;)
            Libraries.CallLibrary(Process, LoadLibraryAddress, libraryNameMemoryAddress);

            // Get the address of the "Main" function by loading the library into
            // ourselves. The address we will receive should be identical to the address
            // that has been allocated within the target process.
            IntPtr mainFunctionAddress = Libraries.GetLibraryFunctionAddress(libraryPath, FunctionToExecute);

            // Call our main function within the target executable.
            Libraries.CallLibrary(Process, mainFunctionAddress, IntPtr.Zero);
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
            byte[] libraryNameBytes = Encoding.UTF8.GetBytes(libraryPath);

            // Get the pointer to the freed up memory for the library name.
            IntPtr processPointer = Process.AllocateMemory(libraryNameBytes.Length);

            // Write the name of the library to the memory space of the process.
            Process.WriteMemory(processPointer, libraryNameBytes);

            // Return the address of written path.
            return processPointer;
        }
    }
}
