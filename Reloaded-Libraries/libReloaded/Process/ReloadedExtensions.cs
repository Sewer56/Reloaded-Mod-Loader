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

using System.Collections.Generic;
using Reloaded.Native.WinAPI;
using Reloaded.Process.Threads;
using Vanara.PInvoke;
using static Reloaded.Native.WinAPI.Threads;

namespace Reloaded.Process
{
    /// <summary>
    /// Provides various uncategorized extension classes for <see cref="ReloadedProcess"/> class.
    /// </summary>
    public static class ReloadedExtensions
    {
        /// <summary>
        /// Resumes all of the threads of the current process from a specified list of threads.
        /// </summary>
        public static void ResumeAllThreads(this ReloadedProcess reloadedProcess, List<ThreadSuspendItemPair> threadPairs) => ResumeAllThreads(threadPairs);

        /// <summary>
        /// Resumes the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void ResumeFirstThread(this ReloadedProcess reloadedProcess)
        {
            Kernel32.ResumeThread(reloadedProcess.Process.Threads[0].GetHandle());
        }

        /// <summary>
        /// Suspends the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void SuspendFirstThread(this ReloadedProcess reloadedProcess)
        {
            Kernel32.SuspendThread(reloadedProcess.Process.Threads[0].GetHandle());
        }

        /// <summary>
        /// Resumes all of the threads of the current process.
        /// Note: This can resume threads that have been explicitly put to sleep by process; this can be dangerous.
        /// If you want to resume after <see cref="SuspendAllThreads"/>; use the return value instead.
        /// </summary>
        public static void ResumeAllThreads(this ReloadedProcess reloadedProcess)
        {
            foreach (System.Diagnostics.ProcessThread processThread in reloadedProcess.Process.Threads)
                processThread.Resume();
        }
        
        /// <summary>
        /// Resumes all of the threads of the current process from a specified list of threads.
        /// </summary>
        public static void ResumeAllThreads(List<ThreadSuspendItemPair> threadPairs)
        {
            foreach (var threadPair in threadPairs)
                if (! threadPair.Suspended)
                    threadPair.Thread.Resume();
        }

        /// <summary>
        /// Suspends all of the threads of the current game except for the thread that is currently running (i.e. of the calling mod).
        /// Note: If you run this from a hook, the game thread you are executing this on
        /// will not be suspended.
        /// </summary>
        public static List<ThreadSuspendItemPair> SuspendAllThreads(this ReloadedProcess reloadedProcess)
        {
            // Get current thread (do not affect self)
            int currentThreadId = (int)GetCurrentThreadId();
            List<ThreadSuspendItemPair> threadSuspendPairs = new List<ThreadSuspendItemPair>(100); // Reasonable thread count.

            // Suspend every thread.
            foreach (System.Diagnostics.ProcessThread processThread in reloadedProcess.Process.Threads)
            {
                if (processThread.Id != currentThreadId)
                {
                    threadSuspendPairs.Add(new ThreadSuspendItemPair(processThread.IsRunning(), processThread));
                    processThread.Suspend();
                }
                else
                    threadSuspendPairs.Add(new ThreadSuspendItemPair(processThread.IsRunning(), processThread));
            }

            return threadSuspendPairs;
        }

        /// <summary>
        /// Kills the process behind the individual Reloaded Process instance.
        /// </summary>
        public static void Kill(this ReloadedProcess reloadedProcess)
        {
            System.Diagnostics.Process localReloadedProcess = reloadedProcess.Process;
            localReloadedProcess.Kill();
        }
    }
}
