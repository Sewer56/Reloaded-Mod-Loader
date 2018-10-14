using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Memory;
using Vanara.PInvoke;

namespace Reloaded.Process.Threads
{
    public static class ProcessThread
    {
        /// <summary>
        /// Parameter of the OpenThread method.
        /// Used to request full access of the desired thread.
        /// </summary>
        public const int ThreadAllAccess = 0x3FB;

        /// <summary>
        /// Returns true if the thread is suspended.
        /// </summary>
        public static bool IsRunning(this System.Diagnostics.ProcessThread processThread)
        {
            return processThread.ThreadState == ThreadState.Running;
        }

        /// <summary>
        /// Returns the ID of the current thread that is executing.
        /// This is not thread specific but is provided as an extension method for convenience.
        /// See <see cref="ProcessThread"/> for 1 line implementation.
        /// </summary>
        public static uint GetCurrentThreadId(this System.Diagnostics.ProcessThread processThread)
        {
            return GetCurrentThreadId();
        }

        /// <summary>
        /// Returns true if the thread is suspended.
        /// </summary>
        public static uint GetCurrentThreadId()
        {
            return Kernel32.GetCurrentThreadId();
        }

        /// <summary>
        /// Gets the individual context for the current thread.
        /// This lets you get information on things such as register contents.
        /// </summary>
        /// <param name="processThread"></param>
        public static unsafe Kernel32.CONTEXT GetThreadContext32(this System.Diagnostics.ProcessThread processThread)
        {
            // Use to resume thread after suspending if needed.
            bool originallyRunning = processThread.IsRunning();

            // Suspend if not suspended.
            if (processThread.IsRunning())
                processThread.Suspend();

            // Get the thread context.
            Kernel32.CONTEXT   threadContext32 = new Kernel32.CONTEXT();
            threadContext32.ContextFlags = Kernel32.CONTEXT_FLAGS.CONTEXT_ALL;

            byte[] threadContext32Bytes = Struct.GetBytes(threadContext32, true);

            fixed (byte* threadContext32Ptr = threadContext32Bytes)
            {
                Kernel32.GetThreadContext(processThread.GetHandle(), (IntPtr)threadContext32Ptr);

                // Resume thread if it originally ran.
                if (originallyRunning)
                    processThread.Resume();

                return Struct.FromPtr<Kernel32.CONTEXT>((IntPtr)threadContext32Ptr, true);
            }
        }

        /// <summary>
        /// Sets the individual context for the current thread.
        /// This lets you set information on things such as register contents.
        /// </summary>
        /// <param name="processThread"></param>
        /// <param name="newContext">The details of the new context; such as new register contents.</param>
        public static unsafe void SetThreadContext32(this System.Diagnostics.ProcessThread processThread, Kernel32.CONTEXT newContext)
        {
            // Use to resume thread after suspending if needed.
            bool originallyRunning = processThread.IsRunning();

            // Suspend if not suspended.
            if (processThread.IsRunning())
                processThread.Suspend();

            byte[] threadContext32Bytes = Struct.GetBytes(newContext, true);

            fixed (byte* threadContext32Ptr = threadContext32Bytes)
            {
                Kernel32.SetThreadContext(processThread.GetHandle(), (IntPtr)threadContext32Ptr);

                // Resume thread if it originally ran.
                if (originallyRunning)
                    processThread.Resume();
            }
        }

        /// <summary>
        /// Returns true if the thread is suspended.
        /// </summary>
        public static unsafe Kernel32.CONTEXT64 GetThreadContext64(this System.Diagnostics.ProcessThread processThread)
        {
            // Use to resume thread after suspending if needed.
            bool originallyRunning = processThread.IsRunning();

            // Suspend if not suspended.
            if (processThread.IsRunning())
                processThread.Suspend();

            // Get the thread context.
            Kernel32.CONTEXT64 threadContext64 = new Kernel32.CONTEXT64();
            threadContext64.ContextFlags = Kernel32.CONTEXT_FLAGS.CONTEXT_ALL;

            byte[] threadContext64Bytes = Struct.GetBytes(threadContext64, true);

            fixed (byte* threadContext64Ptr = threadContext64Bytes)
            {
                Kernel32.GetThreadContext(processThread.GetHandle(), (IntPtr)threadContext64Ptr);

                // Resume thread if it originally ran.
                if (originallyRunning)
                    processThread.Resume();

                return Struct.FromPtr<Kernel32.CONTEXT64>((IntPtr)threadContext64Ptr, true);
            }
        }

        /// <summary>
        /// Sets the individual context for the current thread.
        /// This lets you set information on things such as register contents.
        /// </summary>
        /// <param name="processThread"></param>
        /// <param name="newContext">The details of the new context; such as new register contents.</param>
        public static unsafe void SetThreadContext64(this System.Diagnostics.ProcessThread processThread, Kernel32.CONTEXT64 newContext)
        {
            // Use to resume thread after suspending if needed.
            bool originallyRunning = processThread.IsRunning();

            // Suspend if not suspended.
            if (processThread.IsRunning())
                processThread.Suspend();

            byte[] threadContext64Bytes = Struct.GetBytes(newContext, true);

            fixed (byte* threadContext64Ptr = threadContext64Bytes)
            {
                Kernel32.SetThreadContext(processThread.GetHandle(), (IntPtr)threadContext64Ptr);

                // Resume thread if it originally ran.
                if (originallyRunning)
                    processThread.Resume();
            }
        }

        /// <summary>
        /// Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        public static void Suspend(this System.Diagnostics.ProcessThread processThread)
        {
            // Note: Suspending the debugger's thread can lead to hangups.
            Kernel32.SuspendThread(processThread.GetHandle());
        }

        /// <summary>
        /// Resumes a thread that has been suspended.
        /// </summary>
        public static void Resume(this System.Diagnostics.ProcessThread processThread)
        {
            // Check if the thread is still alive
            if (processThread.IsRunning())
                return;

            // Start the thread
            Kernel32.ResumeThread(processThread.GetHandle());
        }

        /// <summary>
        /// Opens a handle to the thread with all access rights.
        /// </summary>
        /// <param name="processThread">The individual thread for which the handle is to be opened.</param>
        /// <returns></returns>
        public static IntPtr GetHandle(this System.Diagnostics.ProcessThread processThread)
        {
            // 3F8 = THREAD_ALL_ACCESS
            return GetHandle(processThread, ThreadAllAccess);
        }

        /// <summary>
        /// Opens a handle to the thread with a specified set of access rights.
        /// </summary>
        /// <param name="processThread">The individual thread for which the handle is to be opened.</param>
        /// <param name="accessRights">The access rights to open the thread with.</param>
        /// <returns></returns>
        public static IntPtr GetHandle(this System.Diagnostics.ProcessThread processThread, uint accessRights)
        {
            return Kernel32.OpenThread(accessRights, false, (uint) processThread.Id);
        }

        /// <summary>
        /// Obtains the status of termination of the thread, i.e. the error
        /// code returned by the thread.
        /// </summary>
        public static uint GetExitCode(this System.Diagnostics.ProcessThread processThread)
        {
            return GetExitCode(processThread.GetHandle());
        }

        /// <summary>
        /// Obtains the status of termination of the thread, i.e. the error
        /// code returned by the thread.
        /// </summary>
        /// <param name="handle">A handle to the thread.</param>
        public static uint GetExitCode(IntPtr handle)
        {
            // Get the exit code of the thread (can be nullable)
            Kernel32.GetExitCodeThread(handle, out uint lpExit);

            return lpExit;
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates.
        /// </summary>
        public static void Join(this System.Diagnostics.ProcessThread processThread)
        {
            Join(processThread.GetHandle());
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="handle">A handle to the thread.</param>
        public static void Join(IntPtr handle)
        {
            Kernel32.WaitForSingleObject(handle, unchecked((uint)-1));
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates.
        /// </summary>
        /// <param name="processThread"></param>
        /// <param name="length">The timeout for waiting.</param>
        public static void Join(this System.Diagnostics.ProcessThread processThread, TimeSpan length)
        {
            Join(processThread.GetHandle(), length);
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="time">The timeout.</param>
        /// <param name="handle">A handle to the thread.</param>
        /// <returns>The return value is a flag that indicates if the thread terminated or if the time elapsed.</returns>
        public static void Join(IntPtr handle, TimeSpan time)
        {
            Kernel32.WaitForSingleObject(handle, (uint)time.Milliseconds);
        }
    }
}
