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

namespace Reloaded.Process
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
        public static int CallLibrary(this ReloadedProcess process, IntPtr address, IntPtr parameteraddress)
        {
            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            IntPtr hThread = Native.Native.CreateRemoteThread(process.ProcessHandle, IntPtr.Zero, IntPtr.Zero, address, parameteraddress, 0, out IntPtr threadId);

            // Wait for the thread to finish.
            Reloaded.Native.WinAPI.Threads.WaitForSingleObject(hThread, unchecked((uint)-1));

            // Store and retrieve the exit code for the thread.
            Reloaded.Native.WinAPI.Threads.GetExitCodeThread(hThread, out uint exitCode);

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
        public static void CallLibraryAsync(this ReloadedProcess process, IntPtr address, IntPtr parameteraddress)
        {
            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            Native.Native.CreateRemoteThread(process.ProcessHandle, IntPtr.Zero, IntPtr.Zero, address, parameteraddress, 0, out IntPtr threadId);
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
        public static IntPtr GetLibraryFunctionAddress(string libraryPath, string functionName)
        {
            // Obtain the handle to the library.
            IntPtr libraryHandle = Native.Native.LoadLibrary(libraryPath);

            // Return the address of the function with the specified function name.
            return Native.Native.GetProcAddress(libraryHandle, functionName);
        }

        /// <summary>
        /// GetLibraryFunctionAddress
        ///     Returns the offset of the address of an exported function relative to the handle/module base 
        ///     of the library with the passed in directory (generally marked __declspec(dllexport))
        ///     in the library supplied by the parameter (which may already be loaded).
        ///     The method loads the DLL and gets the base using LoadLibrary and using GetProcAddress(),
        ///     obtains the address of a specified function with the matching name and subtracts it from the base.
        /// </summary>
        /// <param name="libraryPath">The path to the specific DLL/Library to obtain function from.</param>
        /// <param name="functionName">The function name of a C/C++/Native exported function in a library.</param>
        public static IntPtr GetLibraryFunctionOffset(string libraryPath, string functionName)
        {
            // Obtain the handle to the library.
            IntPtr libraryHandle = Native.Native.LoadLibrary(libraryPath);

            // The address of the function with the specified function name.
            IntPtr functionHandle = Native.Native.GetProcAddress(libraryHandle, functionName);
            
            return (IntPtr)((long)functionHandle - (long)libraryHandle);
        }
    }
}
