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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Reloaded.Memory.Sources;
using Reloaded.Process.Threads;
using static Reloaded.Native.WinAPI.Threads;
using ProcessThread = System.Diagnostics.ProcessThread;
using SystemProcess = System.Diagnostics.Process;

namespace Reloaded.Process
{
    /// <summary>
    /// The ReloadedProcess class provides various ways by which a game process may be manipulated,
    /// inclusive of suspending, resuming threads, and writing/reading memory.
    /// </summary>
    public class ReloadedProcess
    {
        /// <summary>
        /// An individual ID value that can be used to identify a process (as seen in Task Manager). 
        /// </summary>
        public IntPtr ProcessId { get; private set; }

        /// <summary>
        /// A handle to the process itself. 
        /// The handle is used to specify the process in all functions that perform operations on the Windows' Internal process object.
        /// </summary>
        public IntPtr ProcessHandle { get; private set; }

        /// <summary>
        /// Allows you to access the process' memory.
        /// </summary>
        public IMemory Memory { get; private set; }

        /// <summary>
        /// Stores an instance of System.Diagnostics.Process inside ReloadedProcess.
        /// </summary>
        public SystemProcess Process { get; private set; }

        /// <summary>
        /// Returns true if the process is a 64bit process.
        /// </summary>
        public bool Is64Bit
        {
            get
            {
                Native.Native.IsWow64Process(ProcessHandle, out bool isGame32Bit);
                return !isGame32Bit;
            }
        }

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Empty constructor.
        /// </summary>
        private ReloadedProcess() { }

        /// <summary>
        /// Creates a process in a suspended state for us to use.
        /// In order to start the application's execution, consider running SuspendThread(threadHandle).
        /// Note: The process starts suspended by default, you should call ResumeFirstThread() via 
        /// <see cref="ReloadedExtensions"/> or ResumeAllThreads().
        /// </summary>
        /// <param name="filePath">
        ///     The file path to the executable to launch.
        ///     The file path should NOT be wrapped in quotes.
        /// </param>
        /// <param name="arguments">
        ///     The command line arguments to be passed.
        ///     Each individual command line argument should be wrapped in quotes in case of spaces.
        /// </param>
        public ReloadedProcess(string filePath, string[] arguments)
        {
            // Build up the arguments string.
            string lpCommandLine = $"\"{filePath}\"";

            // Build arguments if necessary.
            if (arguments != null)
            {
                lpCommandLine += " ";

                // Append all arguments.
                foreach (string argument in arguments)
                { lpCommandLine += argument + " "; }

                // Trim last whitespace
                lpCommandLine = lpCommandLine.Substring(0, lpCommandLine.Length - 1);
            }

            // Start up the process
            Native.Native.STARTUPINFO startupInfo = new Native.Native.STARTUPINFO();
            Native.Native.SECURITY_ATTRIBUTES lpProcessAttributes = new Native.Native.SECURITY_ATTRIBUTES();
            Native.Native.SECURITY_ATTRIBUTES lpThreadAttributes = new Native.Native.SECURITY_ATTRIBUTES();
            Native.Native.PROCESS_INFORMATION processInformation = new Native.Native.PROCESS_INFORMATION();

            bool success =  Native.Native.CreateProcessW(null, lpCommandLine, ref lpProcessAttributes,
                            ref lpThreadAttributes, false, Native.Native.ProcessCreationFlags.CREATE_SUSPENDED,
                            IntPtr.Zero, Path.GetDirectoryName(filePath), ref startupInfo,
                            ref processInformation);

            ProcessId = (IntPtr)processInformation.dwProcessId;
            ProcessHandle = processInformation.hProcess;
            Memory = new ExternalMemory(processInformation.hProcess);
            Process = GetSystemProcess();

            // Print Error is Failed
            if (!success) { Bindings.PrintError?.Invoke($"Failed to start ReloadedProcess {filePath}. Is your path correct? Also try running as Administrator."); }
        }

        /// <summary>
        /// Creates a process in a suspended state for us to use.
        /// In order to start the application's execution, consider running SuspendThread(threadHandle).
        /// </summary>
        /// <param name="filePath">
        ///     The file path to the executable to launch.
        ///     The file path should NOT be wrapped in quotes.
        /// </param>
        public ReloadedProcess(string filePath) : this(filePath, null) { }

        /*
            -----------------
            Factory Functions
            -----------------
        */

        /// <summary>
        /// Returns an instance of ReloadedProcess from the current Process.
        /// </summary>
        /// <returns></returns>
        public static ReloadedProcess FromCurrentProcess()
        {
            // Get Current Process
            var currentProcess = SystemProcess.GetCurrentProcess();

            // Return Reloaded Process
            return FromProcess(currentProcess);
        }

        /// <summary>
        /// Returns an instance of <see cref="ReloadedProcess"/> from a specified <see cref="Process"/>.
        /// </summary>
        /// <returns></returns>
        public static ReloadedProcess FromProcessId(uint processId)
        {
            // Get C# Process by ID
            var process = SystemProcess.GetProcessById((int)processId);
  
            return FromProcess(process);
        }

        /// <summary>
        /// Returns an instance of <see cref="ReloadedProcess"/> from a specified <see cref="Process"/>.
        /// </summary>
        /// <returns></returns>
        public static ReloadedProcess FromProcess(SystemProcess process)
        {
            ReloadedProcess rldProcess = new ReloadedProcess();
            rldProcess.ProcessId = (IntPtr)process.Id;
            rldProcess.ProcessHandle = Native.Native.OpenProcess(Native.Native.PROCESS_ALL_ACCESS, false, (int)rldProcess.ProcessId);
            rldProcess.Process = rldProcess.GetSystemProcess();
            CreateMemory(rldProcess);

            return rldProcess;
        }

        /// <summary>
        /// Creates an instance of <see cref="ReloadedProcess"/> from a supplied process name.
        /// </summary>
        /// <param name="processName">The process name to create the <see cref="ReloadedProcess"/> from.</param>
        /// <returns>Null if the operation fails; otherwise a valid <see cref="ReloadedProcess"/></returns>
        public static ReloadedProcess FromProcessName(string processName)
        {
            try
            {
                // Create new ReloadedProcess and get Process by name using System.Diagnostics API.
                ReloadedProcess reloadedProcess = new ReloadedProcess();
                var process = SystemProcess.GetProcessesByName(processName)[0];

                // Extract Process Details
                reloadedProcess.ProcessId = (IntPtr)process.Id;
                reloadedProcess.ProcessHandle = Native.Native.OpenProcess(Native.Native.PROCESS_ALL_ACCESS, false, (int)reloadedProcess.ProcessId);
                reloadedProcess.Process = reloadedProcess.GetSystemProcess();
                CreateMemory(reloadedProcess);

                return reloadedProcess;
            }
            catch
            {
                return null;
            }
        }

        /*
            --------------
            Core Functions
            --------------
        */

        /// <summary>
        /// GetBaseAddress
        ///     Retrieves the base address of the module, i.e. 0x400000 for executables
        ///     not using Address Space Layout Randomization.
        /// </summary>
        /// <returns></returns>
        public IntPtr GetBaseAddress() => Process.Modules[0].BaseAddress;

        /// <summary>
        /// Retrieves Process from the current <see cref="ReloadedProcess"/>.
        /// </summary>
        /// <returns><see cref="System.Diagnostics.Process"/> class for the current Reloaded Process.</returns>
        private SystemProcess GetSystemProcess() => SystemProcess.GetProcessById((int)ProcessId);

        /// <summary>
        /// Creates a new instance of <see cref="IMemory"/> for the <see cref="ReloadedProcess"/>.
        /// </summary>
        /// <param name="process"></param>
        private static void CreateMemory(ReloadedProcess process)
        {
            // Create internal or external memory.
            if (process.Process.Id == SystemProcess.GetCurrentProcess().Id)
                process.Memory = new Reloaded.Memory.Sources.Memory();
            else
                process.Memory = new ExternalMemory(process.Process);
        }
    }
}
