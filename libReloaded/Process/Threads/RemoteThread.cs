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
using System.Diagnostics;
using System.Runtime.InteropServices;
using ThreadState = System.Diagnostics.ThreadState;
using static Reloaded.Native.WinAPI.Threads;

namespace Reloaded.Process.Threads
{
    /// <summary>
    /// Class repesenting a thread in the remote process.
    /// </summary>
    public class RemoteThread<TThreadStructure> where TThreadStructure : struct 
    {
        /// <summary>
        /// The remote thread handle opened with all rights.
        /// Pretty powerful.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Retrieves the unique identifier of the thread.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The native <see cref="ProcessThread"/> object corresponding to this thread,
        /// this is the object that created this <see cref="RemoteThread"/> instance.
        /// </summary>
        public ProcessThread Native { get; private set; }

        /// <summary>
        /// Returns true if the thread is suspended.
        /// </summary>
        public bool IsRunning => Native.ThreadState == ThreadState.Running;

        /// <summary>
        /// Gets if the thread is the first thread in the currently assigned process in libReloaded.
        /// </summary>
        public bool IsFirstThread => this.Handle == Bindings.TargetProcess.ThreadId;

        /// <summary>
        /// Use this only if you REALLY know what you're doing!
        /// Gets or sets the full context of the thread.
        /// If the thread is not already suspended, performs a <see cref="Suspend"/> and <see cref="Resume"/> call on the thread.
        /// </summary>
        public TThreadStructure Context
        {
            get => GetThreadContextLocal();
            set => SetThreadContextLocal(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteThread"/> class from a specified
        /// given <see cref="ProcessThread"/> native object.
        /// </summary>
        /// <param name="thread">The native <see cref="ProcessThread"/> object.</param>
        public RemoteThread(ProcessThread thread)
        {
            // Save the parameters.
            Native = thread;

            // Save the thread id.
            Id = thread.Id;

            // Open the thread.
            Handle = OpenThread(THREAD_ALL_ACCESS, true, Id);
        }

        /// <summary>
        /// Obtains the status of termination of the thread, i.e. the error
        /// code returned by the thread.
        /// </summary>
        public uint GetExitCode()
        {
            // Get the exit code of the thread (can be nullable)
            GetExitCodeThread(Handle, out uint lpExit);
            return lpExit;
        }

        /// <summary>
        /// Blocks the calling thread until the thread terminates.
        /// </summary>
        public void Join()
        {
            WaitForSingleObject(Handle, unchecked((uint)-1));
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="time">The timeout.</param>
        /// <returns>The return value is a flag that indicates if the thread terminated or if the time elapsed.</returns>
        public void Join(TimeSpan time)
        {
            WaitForSingleObject(Handle, (uint)time.Milliseconds);
        }

        /// <summary>
        /// Resumes a thread that has been suspended.
        /// </summary>
        public void Resume()
        {
            // Check if the thread is still alive
            if (IsRunning)
                return;

            // Start the thread
            ResumeThread(Handle);
        }

        /// <summary>
        /// Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        /// <returns>A new instance of the <see cref="FrozenThread"/> class. If this object is disposed, the thread is resumed.</returns>
        public void Suspend()
        {
            // Note: Suspending the debugger's thread can lead to hangups.
            SuspendThread(Handle);
        }

        /// <summary>
        /// Retrieves the individual thread context for the current thread.
        /// </summary>
        /// <returns>The individual context of the current thread.</returns>
        private TThreadStructure GetThreadContextLocal()
        {
            // Use to resume thread after suspending if needed.
            bool originallyRunning = IsRunning;

            // Suspend if not suspended.
            if (IsRunning)
                Suspend();

            // Get the thread context.
            ThreadContext32 threadContext32 = new ThreadContext32();
            ThreadContext64 threadContext64 = new ThreadContext64();
            threadContext32.ContextFlags = ThreadContextFlags.All;
            threadContext64.ContextFlags = ThreadContextFlags.All;

            // Get thread context.
            if (IntPtr.Size == 4)
                GetThreadContext(Handle, ref threadContext32);
            else if (IntPtr.Size == 8)
                GetThreadContext(Handle, ref threadContext64);

            // Warning
            if (Marshal.GetLastWin32Error() == 998 && IntPtr.Size == 8)
            {
                Bindings.PrintText($"Retrieval of the thread context is broken in X64 Debug mode: " +
                                   $"Good luck figuring out why, not even the internet knows :/. " +
                                   $"Release mode works fine though!");
            }

            // Resume Thread.
            if (originallyRunning)
                Resume();

            // Return thread context.
            if (IntPtr.Size == 4)
                return (TThreadStructure)(object)threadContext32;
            if (IntPtr.Size == 8)
                return (TThreadStructure)(object)threadContext64;

            return default(TThreadStructure);
        }

        /// <summary>
        /// Sets the individual context for the current thread.
        /// </summary>
        /// <param name="value">The individual thread context to be set.</param>
        private void SetThreadContextLocal(TThreadStructure value)
        {
            // Do not resume if originally suspended.
            bool originallyRunning = IsRunning;

            // Suspend if not suspended.
            if (IsRunning)
                Suspend();

            // Create 32/64 thread contexts.
            ThreadContext32 threadContext32;
            ThreadContext64 threadContext64;

            // Set the thread context.
            if (IntPtr.Size == 4)
            {
                threadContext32 = (ThreadContext32)(object)value;
                SetThreadContext(Handle, ref threadContext32);
            }
            else if (IntPtr.Size == 8)
            {
                threadContext64 = (ThreadContext64)(object)value;
                SetThreadContext(Handle, ref threadContext64);
            }

            // Resume Thread.
            if (originallyRunning)
                Resume();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Thread ID: {Id} Running: {IsRunning} FirstThread: {IsFirstThread}";
        }
    }
}
