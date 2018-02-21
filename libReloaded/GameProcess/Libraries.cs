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

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Class providing management and functions related to the use of libraries injected or containes into/inside a particular process. 
    /// </summary>
    public static class Libraries
    {
        /// <summary>
        /// CallLibrary
        ///     Calls a function at a specified address with a singular parameter at the 
        ///     specified pointer within the virtual address space of a target process.
        ///     Generally used to call singular Windows API functions such as 
        ///     LoadLibraryA inside a target process while not executing in the same process.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">Address to the starting point of the library, module or library method to be executed.</param>
        /// <param name="parameteraddress">Address of a singular parameter for the method to be called.</param>
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
        /// GetLibraryFunctionAddress
        ///     Returns the address of an exported function (generally marked __declspec(dllexport))
        ///     in the library supplied by the parameter (which may already be loaded).
        ///     The method loads the DLL using LoadLibrary and using GetProcAddress(),
        ///     obtains the address of a specified function with the matching name.
        /// </summary>
        /// <param name="libraryPath">The path to the specific DLL/Library to obtain function from.</param>
        /// <param name="functionName">The function name of a C/C++/Native exported function in a library.</param>
        /// <remarks>
        ///     For injected DLLs into target processes, this will work because the library in reality is only
        ///     allocated once to a location and not reloaded per-process. It is loaded from the same real physical
        ///     address and always mapped onto equivalent address in the virtual address space for each process.
        ///     i.e. Our pointer for Function1 would also be valid for another process, provided the other process
        ///     has at first loaded the library with LoadLibraryA.
        /// </remarks>
        public static IntPtr GetLibraryFunctionAddress(string libraryPath, string functionName)
        {
            // Obtain the handle to the library.
            IntPtr libraryHandle = Native.LoadLibrary(libraryPath);

            // Return the address of the function with the specified function name.
            return Native.GetProcAddress(libraryHandle, functionName);
        }
    }
}
