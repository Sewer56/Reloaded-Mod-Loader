using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Reloaded.IO;
using Reloaded.IO.Config;
using Reloaded.Process;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Hooking;
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

        private static X64FunctionHook<CreateFile> _createFileHook64;
        private static X64FunctionHook<CreateFileW> _createFileWHook64;
        private static X64FunctionHook<CreateFileA> _createFileAHook64;
        private static FunctionHook<CreateFile> _createFileHook;
        private static FunctionHook<CreateFileW> _createFileWHook;
        private static FunctionHook<CreateFileA> _createFileAHook;

        /*
            Contains a dictionary which maps a file path (game's file path) to another file path
            belonging to a modification.
        */

        /// <summary>
        /// Maps file paths to be accessed by the game to another file path which
        /// belongs to a modification.
        /// </summary>
        private static Dictionary<string, string> _remapperDictionary;

        /// <summary>
        /// Part of Reloaded-IO package.
        /// Stores a list of all currently enabled modifications, we cache them in order
        /// to detect new files to be redirected.
        /// </summary>
        private static List<ModConfig> _enabledMods;

        /// <summary>
        /// Part of Reloaded-IO package.
        /// Stores the current game configuration, which includes the enabled mods.
        /// We cache this to because building the file redirection dictionary requires us to know the
        /// game path.
        /// </summary>
        private static GameConfig _currentGameConfig;

        /// <summary>
        /// With this, we setup an event for when each of the individual mods' Plugins/Redirector
        /// folders have a change in files, such that we may pick up, live, new changes to existing folders.
        /// We map the individual mod configuration to a FileSystemWatcher, allowing us to ensure that 
        /// </summary>
        private static Dictionary<ModConfig, FileSystemWatcher> _fileSystemWatcherDictionary;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: File Redirector
                Architectures supported: X86, X64

                This sample mod builds upon the File Monitor sample, providing universal file redirection for modifications
                which implement a `Plugins/Redirector/` folder.

                This is the same set of createFile, createFileA, createFileW hooks, except that, we instead build
                a list of files present within enabled mods' `Plugins/Redirector/` folder and build a dictionary mapping
                a path to path.

                In our hooks, we just check if the file path exists in the dictionary, and override it if so.
            */

            // Debugger.Launch();

            // This should automatically resolve to kernel32.dll as it is already registered by Windows.
            // The handle should return from already loaded library in memory, following the standard search strategy.
            IntPtr kernel32Handle = Reloaded.Process.Native.Native.LoadLibrary("kernel32");

            // Get the addresses of the CreateFileA, CreateFileW, CreateFile functions.
            IntPtr createFileAPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileA");
            IntPtr createFileWPointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFileW");
            IntPtr createFilePointer = Reloaded.Process.Native.Native.GetProcAddress(kernel32Handle, "CreateFile");

            // Retreieve all enabled mods using utility functions in libReloaded.
            _currentGameConfig = GameConfig.GetGameConfigFromExecutablePath(ExecutingGameLocation);
            _enabledMods = GameConfig.GetAllEnabledMods(_currentGameConfig);
            _enabledMods = GameConfig.TopologicallySortConfigurations(_enabledMods);

            // Generate a list of new filesystemwatchers which will let us monitor redirector folders in real time.
            _fileSystemWatcherDictionary = new Dictionary<ModConfig, FileSystemWatcher>();

            // Build a dictionary of enabled mods.
            BuildFileRedirectionDictionary(_enabledMods, _currentGameConfig);

            // Hook the obtained function pointers.

            // X86
            if (IntPtr.Size == 4)
            {
                if (createFileWPointer != IntPtr.Zero) { _createFileWHook = FunctionHook<CreateFileW>.Create((long)createFileWPointer, CreateFileWImpl).Activate(); }
                if (createFileAPointer != IntPtr.Zero) { _createFileAHook = FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate(); }
                if (createFilePointer != IntPtr.Zero) { _createFileHook = FunctionHook<CreateFile>.Create((long)createFilePointer, CreateFileImpl).Activate(); }
            }
            // X64
            else if (IntPtr.Size == 8)
            {
                if (createFileWPointer != IntPtr.Zero) { _createFileWHook64 = X64FunctionHook<CreateFileW>.Create((long)createFileWPointer, CreateFileWImpl).Activate(); }
                if (createFileAPointer != IntPtr.Zero) { _createFileAHook64 = X64FunctionHook<CreateFileA>.Create((long)createFileAPointer, CreateFileAImpl).Activate(); }
                if (createFilePointer != IntPtr.Zero) { _createFileHook64 = X64FunctionHook<CreateFile>.Create((long)createFilePointer, CreateFileImpl).Activate(); }
            }
        }

        /// <summary>
        /// Builds a dictionary mapping game file paths to our own mods' file paths, if available.
        /// </summary>
        /// <param name="modConfigurations">Stores the individual modification details and configuration.</param>
        /// <param name="currentGameConfig">The current game configuration, used to let us know the base directory of the game.</param>
        private static void BuildFileRedirectionDictionary(List<ModConfig> modConfigurations, GameConfig currentGameConfig)
        {
            // Instance dictionary.
            // OrdinalIgnoreCase = Ignore capitals/smalls in paths, we later want no case sensitivity when mapping A to B.
            Dictionary<string, string> localRemapperDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Enumerate over all mods, iterating over their directories.
            foreach (var modConfiguration in modConfigurations)
            {
                // The location of the folder for game files to be redirected to.
                string redirectonFolder = Path.GetDirectoryName(modConfiguration.ModLocation) + "\\Plugins\\Redirector";

                // Ignore mods not utilising this.
                if (Directory.Exists(redirectonFolder))
                {
                    // Retrieve a listing of all files relative to this directory.
                    List<string> allModFiles = RelativePaths.GetRelativeFilePaths(redirectonFolder);

                    // Process all files.
                    foreach (string modFile in allModFiles)
                    {
                        // Get the absolute location of the file as if it were in the game directory (file we are replacing)
                        // Get the absolute location of the file we will be replacing that file with.
                        string gameLocation = currentGameConfig.GameDirectory + modFile;
                        string modFileLocation = redirectonFolder + modFile;

                        // Removes inconsistencies such as backslash direction, other formatting symantics to ensure matches.
                        gameLocation = Path.GetFullPath(gameLocation);
                        modFileLocation = Path.GetFullPath(modFileLocation);

                        // Appends to the file path replacement dictionary.
                        localRemapperDictionary[gameLocation] = modFileLocation;
                    }

                    // Setup event based, realtime file addition/removal as new files are removed or added if not setup.
                    if (!_fileSystemWatcherDictionary.ContainsKey(modConfiguration)) // Ensure this is only done once.
                    {
                        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(redirectonFolder);
                        fileSystemWatcher.EnableRaisingEvents = true;
                        fileSystemWatcher.IncludeSubdirectories = true;
                        fileSystemWatcher.Created += (sender, args) => { BuildFileRedirectionDictionary(modConfigurations, currentGameConfig); };
                        fileSystemWatcher.Deleted += (sender, args) => { BuildFileRedirectionDictionary(modConfigurations, currentGameConfig); };
                        fileSystemWatcher.Renamed += (sender, args) => { BuildFileRedirectionDictionary(modConfigurations, currentGameConfig); };
                        _fileSystemWatcherDictionary.Add(modConfiguration, fileSystemWatcher);
                    }
                }
            }

            // Assign dictionary.
            _remapperDictionary = localRemapperDictionary;
        }

        /// <summary>
        /// Contains the implementation of the CreateFileA hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        private static IntPtr CreateFileAImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
        {
            // Here we simply check whether the file path is in the dictionary,
            // if it is, we replace it.
            if (!filename.StartsWith(@"\\?\"))
                if (_remapperDictionary.TryGetValue(Path.GetFullPath(filename), out string newFileName))
                {
                    return IntPtr.Size == 4 ? 
                        _createFileAHook.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                        _createFileAHook64.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
                }

            // Execute either X86 or X64 original function.
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
            // Here we simply check whether the file path is in the dictionary,
            // if it is, we replace it.
            if (!filename.StartsWith(@"\\?\"))
                if (_remapperDictionary.TryGetValue(Path.GetFullPath(filename), out string newFileName))
                {
                    return IntPtr.Size == 4 ?
                        _createFileWHook.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                        _createFileWHook64.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
                }

            return IntPtr.Size == 4 ?
                _createFileWHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                _createFileWHook64.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }

        /// <summary>
        /// Contains the implementation of the CreateFile hook.
        /// Simply prints the file name to the console and calls + returns the original function.
        /// </summary>
        private static IntPtr CreateFileImpl(string filename, FileAccess access, FileShare share, IntPtr securityAttributes, FileMode creationDisposition, FileAttributes flagsAndAttributes, IntPtr templateFile)
        {
            // Here we simply check whether the file path is in the dictionary,
            // if it is, we replace it.
            if (!filename.StartsWith(@"\\?\"))
                if (_remapperDictionary.TryGetValue(Path.GetFullPath(filename), out string newFileName))
                {
                    return IntPtr.Size == 4 ?
                        _createFileHook.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                        _createFileHook64.OriginalFunction(newFileName, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
                }

            return IntPtr.Size == 4 ?
                _createFileHook.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile) :
                _createFileHook64.OriginalFunction(filename, access, share, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        }
    }
}
