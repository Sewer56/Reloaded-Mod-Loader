using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// The ReloadedProcess class provides various ways by which a game process may be manipulated,
    /// inclusive of suspending, resuming threads, writing and reading memory and calling functions.
    /// Note: Most of the implementation for ReloadedProcess is included in the other classes within this source directory.
    /// This class is only for creating the object and storing the properties.
    /// </summary>
    public class ReloadedProcess
    {
        /// <summary>
        /// A handle to the program's first thread itself. 
        /// The handle is used to specify the process in all functions that perform operations on the Windows' Internal thread object.
        /// </summary>
        public IntPtr threadHandle;

        /// <summary>
        /// An individual ID value that can be used to identify the program's first thread. 
        /// </summary>
        public IntPtr threadId;

        /// <summary>
        /// An individual ID value that can be used to identify a process (as seen in Task Manager). 
        /// </summary>
        public IntPtr processId;

        /// <summary>
        /// A handle to the process itself. 
        /// The handle is used to specify the process in all functions that perform operations on the Windows' Internal process object.
        /// </summary>
        public IntPtr processHandle;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public ReloadedProcess() { }

        /// <summary>
        /// Creates a process in a suspended state for us to use.
        /// In order to start the application's execution, consider running SuspendThread(threadHandle).
        /// </summary>
        /// <param name="filePath">The file path to the executable to launch.<</param>
        public ReloadedProcess(string filePath)
        {
            // Start up the process
            Native.STARTUPINFO startupInfo = new Native.STARTUPINFO();
            Native.PROCESS_INFORMATION processInformation = new Native.PROCESS_INFORMATION();
            Native.CreateProcess(filePath, null, IntPtr.Zero, IntPtr.Zero, false,
                Native.ProcessCreationFlags.CREATE_SUSPENDED,
                IntPtr.Zero, Path.GetDirectoryName(filePath), ref startupInfo, out processInformation);

            // Move Process Properties.
            processHandle = processInformation.hProcess;
            threadHandle = processInformation.hThread;
            processId = (IntPtr) processInformation.dwProcessId;
            threadId = (IntPtr)  processInformation.dwThreadId;
        }

        /// <summary>
        /// Creates an instance of ReloadedProcess from a supplied process ID.
        /// </summary>
        /// <param name="processId">The process ID (PID) to create the Reloaded Process from.</param>
        public ReloadedProcess(uint processId)
        {
            // Set Process ID
            this.processId = (IntPtr)processId;

            // Get Process Handle
            this.processHandle = Native.OpenProcess(Native.PROCESS_ALL_ACCESS, false, (int)this.processId);

            // Get C# Process by ID
            Process process = Process.GetProcessById((int)processId);

            // Set thread id and handle to be that of first thread.
            this.threadId = (IntPtr)process.Threads[0].Id;

            // Set thread handle to be that of the first thread.
            this.threadHandle = Native.OpenThread(Native.THREAD_ALL_ACCESS, false, (int)this.threadId);
        }


        /// <summary>
        /// Creates an instance of ReloadedProcess from a supplied process name.
        /// </summary>
        /// <param name="processName">The process name to find obtain Reloaded process from.</param>
        public static ReloadedProcess GetProcessByName(string processName)
        {
            try
            {
                // Create new ReloadedProcess
                ReloadedProcess reloadedProcess = new ReloadedProcess();

                // Get Process by Name
                Process process = Process.GetProcessesByName(processName)[0];

                // Set Process ID
                reloadedProcess.processId = (IntPtr)process.Id;

                // Get Process Handle
                reloadedProcess.processHandle = Native.OpenProcess(Native.PROCESS_ALL_ACCESS, false, (int)reloadedProcess.processId);

                // Set thread id and handle to be that of first thread.
                reloadedProcess.threadId = (IntPtr)process.Threads[0].Id;

                // Set thread handle to be that of the first thread.
                reloadedProcess.threadHandle = Native.OpenThread(Native.THREAD_ALL_ACCESS, false, (int)reloadedProcess.threadId);

                // Retrun Reloaded Process
                return reloadedProcess;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an instance of ReloadedProcess from the current Process.
        /// </summary>
        /// <returns></returns>
        public static ReloadedProcess GetCurrentProcess()
        {
            // Get Current Process
            Process currentProcess = Process.GetCurrentProcess();

            // Return Reloaded Process by ID
            return new ReloadedProcess((uint)currentProcess.Id);
        }
    }
}
