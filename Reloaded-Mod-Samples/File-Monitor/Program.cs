using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using libReloaded_Networking;
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

        private static X64FunctionHook<CreateFileW> _createFileWHook64;
        private static X64FunctionHook<CreateFileA> _createFileAHook64;
        private static FunctionHook<CreateFileW> _createFileWHook;
        private static FunctionHook<CreateFileA> _createFileAHook;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: File Monitor
                Architectures supported: X86, X64

                Gets our Windows API function pointers by first grabbing a handle to Kernel32 where
                the individual functions are located and then calling the GetProcAddress() Windows API
                function in order to obtain the address of the exported functions.

                We then hook the functions using the Reloaded Hooking classes, print to console the fileName
                parameter and redirect to the original functions.
            */

            // Debugger.Launch();

            // This should automatically resolve to kernel32.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            IntPtr kernel32Handle = Reloaded.Process.Native.Native.LoadLibraryW("kernel32");

            // Get the addresses of the CreateFileA, CreateFileW, CreateFile functions.
            IntPtr createFileAPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileA");
            IntPtr createFileWPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileW");

            // Hook the obtained function pointers.

            // X86
            if (IntPtr.Size == 4)
            {
                if (createFileWPointer != IntPtr.Zero) { _createFileWHook = FunctionHook<CreateFileW>.Create((long)createFileWPointer, CreateFileWImpl).Activate(); }
                if (createFileAPointer != IntPtr.Zero) { _createFileAHook = FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate(); }
            }
            // X64
            else if (IntPtr.Size == 8)
            {
                if (createFileWPointer != IntPtr.Zero) { _createFileWHook64 = X64FunctionHook<CreateFileW>.Create((long)createFileWPointer, CreateFileWImpl).Activate(); }
                if (createFileAPointer != IntPtr.Zero) { _createFileAHook64 = X64FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate(); }
            }
        }

        /// <summary>
        /// Contains the implementation of the CreateFileA hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        private static IntPtr CreateFileAImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
        {
            // This function delegate is automatically assigned by the Reloaded DLL Template Initializer
            // It simply prints to the console of the Mod Loader's Loader (which is a local server).
            // The if statement filters out non-files such as HID devices.
            if (!filename.StartsWith(@"\\?\"))
                Bindings.PrintInfo($"[CFA] Loading File {filename}");

            return IntPtr.Size == 4 ?
                _createFileAHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                _createFileAHook64.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }

        /// <summary>
        /// Contains the implementation of the CreateFileW hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        private static IntPtr CreateFileWImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
        {
            // This function delegate is automatically assigned by the Reloaded DLL Template Initializer
            // It simply prints to the console of the Mod Loader's Loader (which is a local server).
            // The if statement filters out non-files such as HID devices.
            if (!filename.StartsWith(@"\\?\"))
                Bindings.PrintInfo($"[CFW] Loading File {filename}");

            return IntPtr.Size == 4 ?
                _createFileWHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                _createFileWHook64.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
