using System;
using System.Collections.Generic;
using System.Text;
using Vanara.PInvoke;

namespace Reloaded.Process.DllInjector
{
    /// <summary>
    /// Provides access to various functions that perform DLL operations on the current process.
    /// </summary>
    public static class LocalDllFunctions
    {
        /// <summary>
        /// Returns the address of an exported function (generally marked __declspec(dllexport))
        /// in the library supplied by the parameter (which may already be loaded).
        /// </summary>
        /// <param name="libraryPath">The path/name of/to the specific DLL to obtain function from.</param>
        /// <param name="functionName">The function name of an exported function in a library.</param>
        public static IntPtr GetFunctionAddress(string libraryPath, string functionName)
        {
            // Obtain the handle to the library.
            Kernel32.SafeLibraryHandle libraryHandle = Kernel32.LoadLibrary(libraryPath);

            // Return the address of the function with the specified function name.
            return Kernel32.GetProcAddress(libraryHandle, functionName);
        }

        /// <summary>
        /// Returns the offset of the address of an exported function relative to the library module base.
        /// I.e. Offset from start of the library in memory to an exported function.
        /// </summary>
        /// <param name="libraryPath">The path/name of/to the specific DLL to obtain function from.</param>
        /// <param name="functionName">The function name of an exported function in a library.</param>
        public static IntPtr GetFunctionOffset(string libraryPath, string functionName)
        {
            // This method loads the DLL and gets the base using LoadLibrary,
            // using GetProcAddress() it then obtains the address of a specified function
            // and returns the difference between the two.

            // Obtain the handle to the library.
            Kernel32.SafeLibraryHandle libraryHandle = Kernel32.LoadLibrary(libraryPath);

            // The address of the function with the specified function name.
            IntPtr functionHandle = Kernel32.GetProcAddress(libraryHandle, functionName);

            return (IntPtr)((long)functionHandle - (long)libraryHandle.DangerousGetHandle());
        }
    }
}
