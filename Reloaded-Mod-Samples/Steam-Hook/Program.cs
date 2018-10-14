using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Reloaded.Process;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Functions;
using Reloaded.Process.Functions.X86Hooking;
using Reloaded.Process.Native;

// ReSharper disable InconsistentNaming

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

        /* Constant Strings */
        public const string SteamAPI32 = "steam_api.dll";
        public const string SteamAPI64 = "steam_api64.dll";
        public const string FunctionName = "SteamAPI_RestartAppIfNecessary";
        public const string SteamAppId = "steam_appid.txt";

        /* Hooks */
        public static FunctionHook<SteamAPI_RestartAppIfNecessary> restartIfNecessaryHook32;
        public static X64FunctionHook<SteamAPI_RestartAppIfNecessary> restartIfNecessaryHook64;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Utility: Steam Hook
                Architectures supported: X86, X64

                Hooks the Steam function responsible for checking if the game should be
                restarted and just says "no, it shouldn't".
            */

            #if DEBUG
            Debugger.Launch();
            #endif

            // PS. I know the code below could be written better, but I wrote it FOR CLARITY.
            // Get directory of executing executable.
            string steamAPI32 = Path.GetFullPath(SteamAPI32);
            string steamAPI64 = Path.GetFullPath(SteamAPI64);

            // X86
            if (IntPtr.Size == 4)
            {
                if (File.Exists(steamAPI32))
                {
                    // Load file and get address of export.
                    IntPtr libraryAddress = Native.LoadLibraryW(steamAPI32);
                    IntPtr restartAppIfNecessaryPtr = Native.GetProcAddress(libraryAddress, FunctionName);

                    // Hook!
                    if (restartAppIfNecessaryPtr != IntPtr.Zero)
                        restartIfNecessaryHook32 = FunctionHook<SteamAPI_RestartAppIfNecessary>
                            .Create((long)restartAppIfNecessaryPtr, FunctionDelegate).Activate();
                }
            }
            
            // X64
            if (IntPtr.Size == 8)
            {
                if (File.Exists(steamAPI64))
                {
                    // Load file and get address of export.
                    IntPtr libraryAddress           = Native.LoadLibraryW(steamAPI64);
                    IntPtr restartAppIfNecessaryPtr = Native.GetProcAddress(libraryAddress, FunctionName);

                    // If the debugger tells you that those two numbers are 0, it's bullshit.
                    // It's a debugger bug, you'll see it right after the next if statement.

                    // Hook!
                    if (restartAppIfNecessaryPtr != IntPtr.Zero)
                        restartIfNecessaryHook64 = X64FunctionHook<SteamAPI_RestartAppIfNecessary>
                            .Create((long)restartAppIfNecessaryPtr, FunctionDelegate).Activate();
                }
            }
        }

        /// <summary>
        /// No, we don't need to restart app.
        /// </summary>
        private static bool FunctionDelegate(uint appid)
        {
            // Write the Steam AppID to a local file.
            File.WriteAllText(SteamAppId, $"{appid}");

            if (IntPtr.Size == 4)
                restartIfNecessaryHook32.OriginalFunction(appid);
            else
                restartIfNecessaryHook64.OriginalFunction(appid);

            return false;
        }

        /*
            See: https://partner.steamgames.com/doc/api/steam_api#SteamAPI_RestartAppIfNecessary
        */

        [X64ReloadedFunction(X64CallingConventions.Microsoft)]
        [ReloadedFunction(CallingConventions.Cdecl)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool SteamAPI_RestartAppIfNecessary(uint appId);
    }
}
