using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;

namespace Reloaded.Process.DllInjector
{

    /// <summary>
    /// Provides access to various functions that perform DLL operations on other processes.
    /// </summary>
    public static class RemoteDllFunctions
    {
        /// <summary>
        /// Calls a function at a specified address inside another process with a
        /// pointer to parameters for that function.
        /// </summary>
        /// <param name="handle">Process handle for the process to run the function in.</param>
        /// <param name="address">Address of method in library in another process to be executed.</param>
        /// <param name="parameterAddress">Pointer to parameter inside the other process to pass to the function.</param>
        /// <returns>An "exit code". This is basically the value returned by the function.</returns>
        public static int CallLibrary(IntPtr handle, IntPtr address, IntPtr parameterAddress)
        {
            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            var delegateInstance = Marshal.GetDelegateForFunctionPointer<Kernel32.PTHREAD_START_ROUTINE>(address);
            IntPtr hThread = Kernel32.CreateRemoteThread(handle, null, 0, delegateInstance, parameterAddress, 0, out uint threadId);

            // Wait for the thread to finish.
            Kernel32.WaitForSingleObject(hThread, unchecked((uint)-1));

            // Store and retrieve the exit code for the thread.
            Kernel32.GetExitCodeThread(hThread, out uint exitCode);

            // Return the exit code.
            return (int)exitCode;
        }

        /// <summary>
        /// This is the asynchronous version of <see cref="CallLibrary"/> that does not wait for a thread to finish.
        /// </summary>
        /// <param name="handle">Process handle for the process to run the function in.</param>
        /// <param name="address">Address of method in library in another process to be executed.</param>
        /// <param name="parameterAddress">Pointer to parameter inside the other process to pass to the function.</param>
        public static void CallLibraryAsync(IntPtr handle, IntPtr address, IntPtr parameterAddress)
        {
            // Create and initialize a thread at our address (which may be a C/C++ function) and parameter address.
            // hThread is a handle to the new thread.
            var delegateInstance = Marshal.GetDelegateForFunctionPointer<Kernel32.PTHREAD_START_ROUTINE>(address);
            IntPtr hThread = Kernel32.CreateRemoteThread(handle, null, 0, delegateInstance, parameterAddress, 0, out uint threadId);
        }
    }
}
