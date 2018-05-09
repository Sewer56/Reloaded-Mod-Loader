using System;
using System.Diagnostics;
using System.IO;
using Reloaded;
using Reloaded.Process;
using Reloaded.Process.Native;
using Reloaded.Process.X86Hooking;
using static Reloaded_Mod_Template.Native;

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

        private static FunctionHook<CreateFile> createFileHook;
        private static FunctionHook<CreateFileW> createFileWHook;
        private static FunctionHook<CreateFileA> createFileAHook;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: File Monitor
                Architectures supported: X86, no X64 (hooking currently unavailable)

                Gets our Windows API function pointers by first grabbing a handle to Kernel32 where
                the individual functions are located and then calling the GetProcAddress() Windows API
                function in order to obtain the address of the exported functions.

                We then hook the functions using the Reloaded Hooking classes, print to console the fileName
                parameter and redirect to the original functions.
            */

            // This should automatically resolve to kernel32.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            IntPtr kernel32Handle = Reloaded.Process.Native.Native.LoadLibrary("kernel32");

            // Get the addresses of the CreateFileA, CreateFileW, CreateFile functions.
            IntPtr createFileAPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileA");
            IntPtr createFileWPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileW");
            IntPtr createFilePointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFile");

            // Hook the obtained function pointers.
            if (createFilePointer  != IntPtr.Zero)  { createFileWHook = FunctionHook<CreateFileW>.Create((long)createFileWPointer, CreateFileWImpl).Activate(); }
            if (createFileAPointer != IntPtr.Zero)  { createFileAHook = FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate(); }
            if (createFilePointer  != IntPtr.Zero)  { createFileHook  = FunctionHook<CreateFile >.Create((long)createFilePointer , CreateFileImpl).Activate();  }
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
                Bindings.PrintInfo($"Loading File {filename}");

            return createFileAHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
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
                Bindings.PrintInfo($"Loading File {filename}");

            return createFileWHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }

        /// <summary>
        /// Contains the implementation of the CreateFile hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        private static IntPtr CreateFileImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
        {
            // This function delegate is automatically assigned by the Reloaded DLL Template Initializer
            // It simply prints to the console of the Mod Loader's Loader (which is a local server).
            // The if statement filters out non-files such as HID devices.
            if (!filename.StartsWith(@"\\?\"))
                Bindings.PrintInfo($"Loading File {filename}");

            return createFileHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
