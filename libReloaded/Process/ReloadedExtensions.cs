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

using Reloaded.Process.Threads;
using static Reloaded.Native.WinAPI.Threads;

namespace Reloaded.Process
{
    /// <summary>
    /// Provides various uncategorized extension classes for <see cref="ReloadedProcess"/> class.
    /// </summary>
    public static class ReloadedExtensions
    {
        /// <summary>
        /// Resumes the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void ResumeFirstThread(this ReloadedProcess reloadedProcess)
        {
            ResumeThread(reloadedProcess.ThreadHandle);
        }

        /// <summary>
        /// Suspends the first/main/primary thread assigned to the Reloaded Process object.
        /// Note: It's not recommended to run this from a hook, first thread might be your program's current thread.
        /// </summary>
        public static void SuspendFirstThread(this ReloadedProcess reloadedProcess)
        {
            SuspendThread(reloadedProcess.ThreadHandle);
        }

        /// <summary>
        /// Resumes all of the threads of the current game except for the thread
        /// that is currently running (i.e. of the calling mod).
        /// </summary>
        public static void ResumeAllThreads(this ReloadedProcess reloadedProcess)
        {
            if (reloadedProcess.Is64Bit)
            {
                foreach (RemoteThread<ThreadContext64> processThread in reloadedProcess.GetRemoteThreads<ThreadContext64>())
                    processThread.Resume();
            }
            else
            {
                foreach (RemoteThread<ThreadContext32> processThread in reloadedProcess.GetRemoteThreads<ThreadContext32>())
                    processThread.Resume();
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
            // Get current thread (do not affect self)
            int currentThreadId = (int)GetCurrentThreadId();

            // Ignore self and suspend all other threads.
            if (reloadedProcess.Is64Bit)
            {
                // For each thread, Ignore self and suspend all other threads.
                foreach (RemoteThread<ThreadContext64> processThread in reloadedProcess.GetRemoteThreads<ThreadContext64>())
                {
                    if (processThread.Id != currentThreadId)
                        processThread.Suspend();
                }
            }
            else
            {
                // For each thread, Ignore self and suspend all other threads.
                foreach (RemoteThread<ThreadContext32> processThread in reloadedProcess.GetRemoteThreads<ThreadContext32>())
                {
                    if (processThread.Id != currentThreadId)
                        processThread.Suspend();
                }
            }
        }

        /// <summary>
        /// Kills the process behind the individual Reloaded Process instance.
        /// </summary>
        public static void KillProcess(this ReloadedProcess reloadedProcess)
        {
            System.Diagnostics.Process localReloadedProcess = reloadedProcess.GetProcessFromReloadedProcess();
            localReloadedProcess.Kill();
        }
    }
}
