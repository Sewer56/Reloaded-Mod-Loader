using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Reloaded.Process;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Hooking;
using static Reloaded_Mod_Template.Native;
using Bindings = Reloaded.Bindings;

namespace Reloaded_Mod_Template
{
    public static class Program
    {
        #region Reloaded Mod Template Stuff
        /// <summary>
        /// Holds the game process for us to manipulate.
        /// Allows you to read/write memory, perform pattern scans, etc.
        /// See libReloaded/GameProcess (folder)
        /// </summary>
        public static ReloadedProcess GameProcess;

        /// <summary>
        /// Stores the absolute executable location of the currently executing game or process.
        /// </summary>
        public static string ExecutingGameLocation;

        /// <summary>
        /// Specifies the full directory location that the current mod 
        /// is contained in.
        /// </summary>
        public static string ModDirectory;
        #endregion Reloaded Mod Template Stuff

        /*
            Contains the hook objects for the individual function hooks in Reloaded Mod Loader.
            These function hooks can be instantiated via either the available factory method, or
            with constructor, which just runs the factory method and is an alias for it.
        */


        private static X64FunctionHook<NtCreateFile> _ntCreateFileHook64;
        private static FunctionHook<NtCreateFile> _ntCreateFileHook;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: File Monitor (New)
                Architectures supported: X86, X64

                Retrieves our Windows API function pointer by first grabbing a handle to Ntdll where
                the individual function is located then calls the GetProcAddress() Windows API function
                in order to obtain the address of the exported function.

                We then hook the function using the Reloaded hooking classes, print to console the filename
                and redirect to the original function.
            */

            #if DEBUG
            Debugger.Launch();
            #endif

            // This should automatically resolve to ntdll.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            IntPtr ntdllHandle = Reloaded.Process.Native.Native.LoadLibraryW("ntdll");

            // Get the addresses of our desired NtCreateFile
            IntPtr ntCreateFilePointer = Reloaded.Process.Native.Native.GetProcAddress(ntdllHandle, "NtCreateFile");

            // Hook the obtained function pointers.

            // X86
            if (IntPtr.Size == 4)
            {
                if (ntCreateFilePointer != IntPtr.Zero) { _ntCreateFileHook = FunctionHook<NtCreateFile>.Create((long)ntCreateFilePointer, NtCreateFileImpl).Activate(); }
            }
            // X64
            else if (IntPtr.Size == 8)
            {
                if (ntCreateFilePointer != IntPtr.Zero) { _ntCreateFileHook64 = X64FunctionHook<NtCreateFile>.Create((long)ntCreateFilePointer, NtCreateFileImpl).Activate(); }
            }
        }

        /// <summary>
        /// Contains the implementation of our NtCreateFile hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        /// <returns></returns>
        private static int NtCreateFileImpl(out IntPtr filehandle, FileAccess access, ref OBJECT_ATTRIBUTES objectAttributes, ref IO_STATUS_BLOCK ioStatus, ref long allocSize, uint fileattributes, FileShare share, uint createDisposition, uint createOptions, IntPtr eaBuffer, uint eaLength)
        {
            Bindings.PrintInfo($"[NTCF] Loading File {objectAttributes.ObjectName.ToString()}");

            return IntPtr.Size == 4 ?
                _ntCreateFileHook.OriginalFunction(out filehandle, access, ref objectAttributes, ref ioStatus, ref allocSize, fileattributes, share, createDisposition, createOptions, eaBuffer, eaLength) :
                _ntCreateFileHook64.OriginalFunction(out filehandle, access, ref objectAttributes, ref ioStatus, ref allocSize, fileattributes, share, createDisposition, createOptions, eaBuffer, eaLength);
        }
    }
}
