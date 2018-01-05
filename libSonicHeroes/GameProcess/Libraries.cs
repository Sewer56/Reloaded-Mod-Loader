using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonicHeroes.GameProcess;

namespace SonicHeroes.GameProcess
{
    /// <summary>
    /// Class providing management and functions related to the use of libraries injected or containes into/inside a particular process. 
    /// </summary>
    public static class Libraries
    {
        /// <summary>
        /// Calls a function at a specified address with a singular parameter at the specified pointer within the virtual address space of a target process.
        /// Generally used to call singular Windows API functions such as LoadLibraryA inside a target process.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">Address to the starting point of the library, module or library method to be executed.</param>
        /// <param name="parameteraddress">Address of parameters for the method to be called.</param>
        /// <returns>An exit code.</returns>
        public static int CallLibrary(this Process process, IntPtr address, IntPtr parameteraddress)
        {
            // Store the thread identifier.
            IntPtr threadID;

            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            var hThread = Native.CreateRemoteThread(process.Handle, IntPtr.Zero, 0, address, parameteraddress, 0, out threadID);

            // Wait for the thread to finish.
            Native.WaitForSingleObject(hThread, unchecked((uint)-1));

            // Store and retrieve the exit code for the thread.
            uint exitCode;
            Native.GetExitCodeThread(hThread, out exitCode);

            // Return the exit code.
            return (int)exitCode;
        }

        /// <summary>
        /// See <see cref="CallLibrary"/>, it is the asynchronous version of CallLibrary that does not wait for the thread to finish.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">Address to the starting point of the library, module or library method to be executed.</param>
        /// <param name="parameteraddress">Address of parameters for the method to be called.</param>
        /// <returns>An exit code</returns>
        public static void CallLibraryAsync(this Process process, IntPtr address, IntPtr parameteraddress)
        {
            // Store the thread identifier.
            IntPtr threadID;

            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            var hThread = Native.CreateRemoteThread(process.Handle, IntPtr.Zero, 0, address, parameteraddress, 0, out threadID);
        }

        /// <summary>
        /// Returns the address of an exported function (generally marked __declspec(dllexport)) in a loaded library.
        /// The handle of the library is first acquired then GetProcAddress() is called with a function name and 
        /// then GetProcAddress() is used to acquire an address of a specific function.
        /// For injected DLLs, this will work because the library which we are calling is mapped in the address space of the game process to its real location,
        /// which all programs, utilities, etc. share
        /// </summary>
        /// <param name="libraryPath">The name of the specific DLL/Library whose function address you would want to grab.</param>
        /// <param name="functionName">The function name of a C/C++/Native exported function in a library.</param>
        /// <returns></returns>
        public static IntPtr GetLibraryFunctionAddress(string libraryPath, string functionName)
        {
            // Obtain the handle to the library.
            IntPtr libraryHandle = Native.LoadLibrary(libraryPath);

            // Return the address of the function with the specified function name.
            return Native.GetProcAddress(libraryHandle, functionName);
        }
    }
}
