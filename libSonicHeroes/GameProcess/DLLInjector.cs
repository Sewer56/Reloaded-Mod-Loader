using System;
using System.Diagnostics;
using System.Text;

namespace SonicHeroes.GameProcess
{
    /// <summary>
    /// Provides a means by which a target DLLs may be injected into an individual process.
    /// First this class should be instantiated, and DLLs with path X may be injected by calling Inject().
    /// </summary>
    class DLLInjector
    {
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
        /// The handle to the process that is to be injected. The handle can be obtained by Process.Handle for an existing process or found via various other means.
        /// </summary>
        private Process Process { get; set; }

        /// <summary>
        /// Initialtes the DLL Injectior.
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
            Kernel32Handle = Native.LoadLibrary("kernel32");

            // Retrieves the address to the LoadLibraryA function.
            LoadLibraryAddress = Native.GetProcAddress(Kernel32Handle, "LoadLibraryA"); 
        }

        /// <summary>
        /// Injects a DLL onto the set target process with the specified library path.
        /// </summary>
        /// <param name="libraryPath">The absolute path to your DLL to be injected.</param>
        public void InjectDLL(string libraryPath)
        {
            // Write the module name in process memory to be used by Kernel32's LoadLibraryA function.
            IntPtr libraryNameMemoryAddress = WriteModuleNameBytes(libraryPath);

            // Call Kernel32's LoadLibrary function and execute it within the target process.
            // Execution within the target process works by creating a thread within the virtual address space of the target process.
            // Our library will now be loaded within the address space of the game/game module to use and ready for us to execute.
            Libraries.CallLibrary(Process, LoadLibraryAddress, libraryNameMemoryAddress);

            // Get the address of the "Main" function.
            IntPtr mainFunctionAddress = Libraries.GetLibraryFunctionAddress(libraryPath, "Main");

            // Call our main function within the target executable.
            Libraries.CallLibrary(Process, mainFunctionAddress, IntPtr.Zero);
        }

        /// <summary>
        /// Writes the module name and target path as a series of bytes into the executable memory space.
        /// This is such that we may call the internal LoadLibaryA of Kernel32 to load our DLL module.
        /// </summary>
        /// <param name="libraryPath">The path to the DLL library that is to have their name written to memory.</param>
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
