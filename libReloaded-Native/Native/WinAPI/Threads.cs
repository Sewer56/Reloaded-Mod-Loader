/*
    [Reloaded] Mod Loader Launcher
    A universal, powerful multi-game, multi-process mod loader based on DLL Injection. 
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
using System.Runtime.InteropServices;

namespace Reloaded.Native.WinAPI
{
    /// <summary>
    /// Provides various thread related functions and definitions to be used in Reloaded.
    /// </summary>
    public class Threads
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
        /// Suspends a windows on windows64 thread supplied by the handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern uint Wow64SuspendThread(IntPtr hThread);

        /// <summary>
        /// Retrieves the ID of the currently running thread.
        /// </summary>
        /// <returns>The ID of the currently runing thread.</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        /// <summary>
        /// WaitForSingleObject
        ///     Waits until the specified object is in the signaled state or 
        ///     the specific time-out interval elapses.
        /// </summary>
        /// <param name="hHandle">Handle to the object in question.</param>
        /// <param name="dwMilliseconds">The time interval in milliseconds.</param>
        /// <returns>
        ///     If the function succeeds, the return value indicates the event that caused the function to return. 
        ///     It can be one of the following values. See https://msdn.microsoft.com/en-us/library/windows/desktop/ms687032(v=vs.85).aspx.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        /// <summary>
        /// GetExitCodeThread
        ///     Retrieves the termination status of the specified thread.
        /// </summary>
        /// <param name="hThread">A handle to the thread.</param>
        /// <param name="lpExitCode">
        ///     A pointer to a variable to receive the thread termination status. 
        ///     For more information, see Remarks.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        /// <summary>
        ///     Retrieves the context of the specified thread.
        /// </summary>
        /// <param name="hThread">
        ///         A handle to the thread whose context is to be retrieved.
        ///         The handle must have ThreadAccessFlags.GetContext access to the thread. 
        ///         For more information, see Thread Security and Access Rights.
        ///         WOW64: The handle must also have ThreadAccessFlags.QueryInformation access.
        /// </param>
        /// <param name="lpContext">
        ///         A pointer to a <see cref="ThreadContext32"/> structure that receives the appropriate context of the specified thread. 
        ///         The value of the ContextFlags member of this structure specifies which portions of a thread's context are retrieved. 
        ///         The <see cref="ThreadContext32"/> structure is highly processor specific.
        ///         Refer to the WinNT.h header file for processor-specific definitions of this structures and any alignment requirements.
        /// </param>
        /// <returns>
        ///         If the function succeeds, the return value is nonzero, else it is zero.
        /// </returns>
        [DllImport("kernel32.dll", EntryPoint = "GetThreadContext", SetLastError = true)]
        public static extern bool GetThreadContext(IntPtr hThread, [MarshalAs(UnmanagedType.Struct)] ref ThreadContext32 lpContext);

        /// <summary>
        /// Sets the context for the specified thread. A 64-bit application can set the context of a WOW64 thread using the Wow64SetThreadContext function.
        /// </summary>
        /// <param name="hThread">
        /// A handle to the thread whose context is to be set. The handle must have the ThreadAccessFlags.SetContext access right to the thread. 
        /// For more information, see Thread Security and Access Rights.</param>
        /// <param name="lpContext">
        /// A pointer to a ThreadContext structure that contains the context to be set in the specified thread. 
        /// The value of the ContextFlags member of this structure specifies which portions of a thread's context to set. 
        /// Some values in the ThreadContext structure that cannot be specified are silently set to the correct value. 
        /// This includes bits in the CPU status register that specify the privileged processor mode, global enabling bits in the debugging register, 
        /// and other states that must be controlled by the operating system.
        /// </param>
        /// <returns>
        /// If the context was set, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", EntryPoint = "SetThreadContext", SetLastError = true)]
        public static extern bool SetThreadContext(IntPtr hThread, [MarshalAs(UnmanagedType.Struct)] ref ThreadContext32 lpContext);

        /// <summary>
        ///     Retrieves the context of the specified thread.
        /// </summary>
        /// <param name="hThread">
        ///         A handle to the thread whose context is to be retrieved.
        ///         The handle must have ThreadAccessFlags.GetContext access to the thread. 
        ///         For more information, see Thread Security and Access Rights.
        ///         WOW64: The handle must also have ThreadAccessFlags.QueryInformation access.
        /// </param>
        /// <param name="lpContext">
        ///         A pointer to a <see cref="ThreadContext64"/> structure that receives the appropriate context of the specified thread. 
        ///         The value of the ContextFlags member of this structure specifies which portions of a thread's context are retrieved. 
        ///         The <see cref="ThreadContext64"/> structure is highly processor specific.
        ///         Refer to the WinNT.h header file for processor-specific definitions of this structures and any alignment requirements.
        /// </param>
        /// <returns>
        ///         If the function succeeds, the return value is nonzero, else it is zero.
        /// </returns>
        [DllImport("kernel32.dll", EntryPoint = "GetThreadContext", SetLastError = true)]
        public static extern bool GetThreadContext(IntPtr hThread, [MarshalAs(UnmanagedType.Struct)] ref ThreadContext64 lpContext);

        /// <summary>
        /// Sets the context for the specified thread. A 64-bit application can set the context of a WOW64 thread using the Wow64SetThreadContext function.
        /// </summary>
        /// <param name="hThread">
        /// A handle to the thread whose context is to be set. The handle must have the ThreadAccessFlags.SetContext access right to the thread. 
        /// For more information, see Thread Security and Access Rights.</param>
        /// <param name="lpContext">
        /// A pointer to a <see cref="ThreadContext64"/> structure that contains the context to be set in the specified thread. 
        /// The value of the ContextFlags member of this structure specifies which portions of a thread's context to set. 
        /// Some values in the <see cref="ThreadContext64"/> structure that cannot be specified are silently set to the correct value. 
        /// This includes bits in the CPU status register that specify the privileged processor mode, global enabling bits in the debugging register, 
        /// and other states that must be controlled by the operating system.
        /// </param>
        /// <returns>
        /// If the context was set, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", EntryPoint = "SetThreadContext", SetLastError = true)]
        public static extern bool SetThreadContext(IntPtr hThread, [MarshalAs(UnmanagedType.Struct)] ref ThreadContext64 lpContext);

        /// <summary>
        /// Opens an existing thread object.
        /// </summary>
        /// <param name="dwDesiredAccess">
        /// The access to the thread object. 
        /// This access right is checked against the security descriptor for the thread. 
        /// This parameter can be one or more of the thread access rights.
        /// </param>
        /// <param name="bInheritHandle">
        /// If this value is TRUE, processes created by this process will inherit the handle. 
        /// Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="dwProcessId">
        /// The identifier of the thread to be opened.
        /// </param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified thread.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Parameter of the OpenThread method.
        /// Used to request full access of the desired thread.
        /// </summary>
        public const int THREAD_ALL_ACCESS = 0x3FB;

        /// <summary>
        /// Parameter of the OpenThread method.
        /// Used to request suspend/resume access of the desired thread.
        /// </summary>
        public const int THREAD_SUSPEND_RESUME = 0x0002;

        /// <summary>
        /// Represents a thread context.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ThreadContext32
        {
            /// <summary>
            /// Determines which registers are returned or set when using <see cref="NativeMethods.GetThreadContext"/> or <see cref="NativeMethods.SetThreadContext"/>.
            /// 
            /// If the context record is used as an INPUT parameter, then for each portion of the context record controlled by a flag whose value is set, it is assumed that portion of the 
            /// context record contains valid context. If the context record is being used to modify a threads context, then only that portion of the threads context will be modified.
            /// 
            /// If the context record is used as an INPUT/OUTPUT parameter to capture the context of a thread, then only those portions of the thread's context corresponding to set flags will be returned.
            /// 
            /// The context record is never used as an OUTPUT only parameter.
            /// </summary>
            public ThreadContextFlags ContextFlags;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr0;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr1;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr2;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr3;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr6;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.DebugRegisters"/>.
            /// </summary>
            public uint Dr7;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.FloatingPoint"/>.
            /// </summary>
            [MarshalAs(UnmanagedType.Struct)]
            public FloatingSaveArea FloatingSave;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Segments"/>.
            /// </summary>
            public uint SegGs;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Segments"/>.
            /// </summary>
            public uint SegFs;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Segments"/>.
            /// </summary>
            public uint SegEs;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Segments"/>.
            /// </summary>
            public uint SegDs;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint EDI;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint ESI;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint EBX;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint EDX;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint ECX;

            /// <summary>
            /// This register is specified/returned if the ContextFlags word contains the flag <see cref="ThreadContextFlags.Integer"/>.
            /// </summary>
            public uint EAX;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint EBP;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint EIP;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint SegCs;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint EFlags;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint Esp;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.Control"/>.
            /// </summary>
            public uint SegSs;

            /// <summary>
            /// This is specified/returned if <see cref="ContextFlags"/> contains the flag <see cref="ThreadContextFlags.ExtendedRegisters"/>.
            /// The format and contexts are processor specific.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }

        /// <summary>
        /// Determines which registers are returned or set when using <see cref="NativeMethods.GetThreadContext"/> or <see cref="NativeMethods.SetThreadContext"/>.
        /// </summary>
        [Flags]
        public enum ThreadContextFlags
        {
            /// <summary>
            /// The Intel 80386 microprocessor, also known as the i386.
            /// </summary>
            Intel386 = 0x10000,

            /// <summary>
            /// The Intel 80486 microprocessor, also known as the i486.
            /// </summary>
            Intel486 = 0x10000,

            /// <summary>
            /// SS:SP, CS:IP, FLAGS, BP
            /// </summary>
            Control = Intel386 | 0x01,

            /// <summary>
            /// AX, BX, CX, DX, SI, DI
            /// </summary>
            Integer = Intel386 | 0x02,

            /// <summary>
            /// DS, ES, FS, GS
            /// </summary>
            Segments = Intel386 | 0x04,

            /// <summary>
            /// 387 state
            /// </summary>
            FloatingPoint = Intel386 | 0x08,

            /// <summary>
            /// DB 0-3,6,7
            /// </summary>
            DebugRegisters = Intel386 | 0x10,

            /// <summary>
            /// CPU specific extensions
            /// </summary>
            ExtendedRegisters = Intel386 | 0x20,

            /// <summary>
            /// All flags excepted FloatingPoint, DebugRegisters and ExtendedRegisters. 
            /// </summary>
            Full = Control | Integer | Segments,

            /// <summary>
            /// All flags.
            /// </summary>
            All = Control | Integer | Segments | FloatingPoint | DebugRegisters | ExtendedRegisters
        }

        /// <summary>
        /// Returned if <see cref="ThreadContextFlags.FloatingPoint"/> flag is set.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct FloatingSaveArea
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;
            public uint Cr0NpxState;
        }

        /// <summary>
        /// x64
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 1232)]
        public struct ThreadContext64
        {
            public ulong P1Home;
            public ulong P2Home;
            public ulong P3Home;
            public ulong P4Home;
            public ulong P5Home;
            public ulong P6Home;

            public ThreadContextFlags ContextFlags;
            public uint MxCsr;

            public ushort SegCs;
            public ushort SegDs;
            public ushort SegEs;
            public ushort SegFs;
            public ushort SegGs;
            public ushort SegSs;
            public uint EFlags;

            public ulong Dr0;
            public ulong Dr1;
            public ulong Dr2;
            public ulong Dr3;
            public ulong Dr6;
            public ulong Dr7;

            public ulong Rax;
            public ulong Rcx;
            public ulong Rdx;
            public ulong Rbx;
            public ulong Rsp;
            public ulong Rbp;
            public ulong Rsi;
            public ulong Rdi;
            public ulong R8;
            public ulong R9;
            public ulong R10;
            public ulong R11;
            public ulong R12;
            public ulong R13;
            public ulong R14;
            public ulong R15;
            public ulong Rip;

            public XSAVE_FORMAT64 DUMMYUNIONNAME;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            public M128A[] VectorRegister;
            public ulong VectorControl;

            public ulong DebugControl;
            public ulong LastBranchToRip;
            public ulong LastBranchFromRip;
            public ulong LastExceptionToRip;
            public ulong LastExceptionFromRip;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct M128A
        {
            public ulong High;
            public long Low;

            public override string ToString()
            {
                return string.Format("High:{0}, Low:{1}", this.High, this.Low);
            }
        }

        /// <summary>
        /// x64
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct XSAVE_FORMAT64
        {
            public ushort ControlWord;
            public ushort StatusWord;
            public byte TagWord;
            public byte Reserved1;
            public ushort ErrorOpcode;
            public uint ErrorOffset;
            public ushort ErrorSelector;
            public ushort Reserved2;
            public uint DataOffset;
            public ushort DataSelector;
            public ushort Reserved3;
            public uint MxCsr;
            public uint MxCsr_Mask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public M128A[] FloatRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public M128A[] XmmRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] Reserved4;
        }
    }
}
