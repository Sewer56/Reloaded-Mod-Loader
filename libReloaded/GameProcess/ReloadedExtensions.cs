using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Provides various uncategorized extension classes which don't belong in any of the other classes in question.
    /// </summary>
    public static class ReloadedExtensions
    {
        /// <summary>
        /// Resumes a suspended thread supplied by the handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        /// <summary>
        /// Suspends a running thread supplied by the handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        /// <summary>
        /// Resumes the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void ResumeFirstThread(this ReloadedProcess reloadedProcess)
        {
            ResumeThread(reloadedProcess.threadHandle);
        }

        /// <summary>
        /// Suspends the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void SuspendFirstThread(this ReloadedProcess reloadedProcess)
        {
            SuspendThread(reloadedProcess.threadHandle);
        }

        /// <summary>
        /// Resumes all of the threads of the current game except for the thread
        /// that is currently running (i.e. of the calling mod).
        /// </summary>
        public static void ResumeAllThreads(this ReloadedProcess reloadedProcess)
        {
            // Get Process from Current Process
            Process gameProcess = reloadedProcess.GetProcessFromReloadedProcess();

            // Get current thread (do not affect self)
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            // For each thread.
            foreach (ProcessThread processThread in gameProcess.Threads)
            {
                // Ignore self
                if (processThread.Id != currentThreadId)
                {
                    // Get thread handle
                    IntPtr resumeThreadHandle = Native.OpenThread(Native.THREAD_ALL_ACCESS, false, processThread.Id);

                    // Suspend Thread
                    ResumeThread(resumeThreadHandle);
                }
            }
        }

        /// <summary>
        /// Suspends all of the threads of the current game except for the thread
        /// that is currently running (i.e. of the calling mod).
        /// Note: If you run this from a hook, the game thread you are executing this on
        /// will not be suspended, you should manually in your program.
        /// </summary>
        public static void SuspendAllThreads(this ReloadedProcess reloadedProcess)
        {
            // Get Process from Current Process
            Process gameProcess = reloadedProcess.GetProcessFromReloadedProcess();

            // Get current thread (do not affect self)
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            // For each thread.
            foreach (ProcessThread processThread in gameProcess.Threads)
            {
                // Ignore self
                if (processThread.Id != currentThreadId)
                {
                    // Get thread handle
                    IntPtr suspendThreadHandle = Native.OpenThread(Native.THREAD_ALL_ACCESS, false, processThread.Id);

                    // Suspend Thread
                    SuspendThread(suspendThreadHandle);
                }
            }
        }

        /// <summary>
        /// Kills the process behind the individual Reloaded Process instance.
        /// </summary>
        public static void KillProcess(this ReloadedProcess reloadedProcess)
        {
            Process localReloadedProcess = reloadedProcess.GetProcessFromReloadedProcess();
            localReloadedProcess.Kill();
        }
    }
}
