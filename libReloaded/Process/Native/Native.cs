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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Reloaded.Process.Native
{
    /// <summary>
    /// Defines native Windows API function imports used within the current namespace to aid in
    /// the services of DLL Injection, Memory Manipulation and other similar arbitrary functions.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Native
    {
        /// <summary>
        /// AllocationTypes
        ///     Specifies the type of memory allocation to be used alongside functions such as VirtualCommitEx
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366890(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum AllocationTypes
        {
            /// <summary>
            /// Allocates memory charges (from the overall size of memory and the paging files on disk) 
            /// for the specified reserved memory pages. The function also guarantees that when the 
            /// caller later initially accesses the memory, the contents will be zero. 
            /// </summary>
            Commit = 0x1000,

            /// <summary>
            /// Reserves a range of the process's virtual address space without allocating any actual 
            /// physical storage in memory or in the paging file on disk.
            /// You commit reserved pages by calling VirtualAllocEx again with MEM_COMMIT.
            /// </summary>
            Reserve = 0x2000,

            /// <summary>
            /// Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. 
            /// The pages should not be read from or written to the paging file.
            /// However, the memory block will be used again later, so it should not be decommitted. 
            /// </summary>
            Reset = 0x80000,

            /// <summary>
            /// MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully 
            /// applied earlier. It indicates that the data in the specified memory range specified by lpAddress 
            /// and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET.
            /// </summary>
            ResetUndo = 0x1000000,

            /// <summary>
            /// Allocates memory at the highest possible address. This can be slower than regular allocations, 
            /// especially when there are many allocations.
            /// </summary>
            TopDown = 0x100000
        }

        /// <summary>
        /// FreeTypes
        ///     The type of free operation. This parameter can be either Decommit or Release.
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366894(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum FreeTypes
        {
            /// <summary>
            /// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state. 
            /// </summary>
            Decommit = 0x4000,
            /// <summary>
            /// Releases the specified region of pages. After the operation, the pages are in the free state. 
            /// </summary>
            Release = 0x8000
        }

        /// <summary>
        /// MemoryProtections
        ///     Specifies the memory protection constants for the region of pages 
        ///     to be allocated, referenced or used for a similar purpose.
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366786(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum MemoryProtections
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        /// <summary>
        /// LoadLibrary
        ///     Loads the specified module into the address space of the calling process.
        ///     The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">
        ///     The name of the module. This can be either a library module (a.dll file) 
        ///     or an executable module (.exe file). If the string specifies a full path, 
        ///     the function searches only that path for the module. If the string specifies 
        ///     a relative path or a module name without a path, the function uses the 
        ///     standard search strategy.
        /// </param>
        /// <returns>Handle to the module.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        /// <summary>
        /// GetProcAddress
        ///     Retrieves the address of an exported function or variable from the specified 
        ///     dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">The handle to the module to get function/variable.</param>
        /// <param name="procName">The name of the function or variable for which the address is to be obtained for.</param>
        /// <returns>Address of exported function or variable.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// VirtualAllocEx
        ///     Reserves, commits, or changes the state of a region of memory within the 
        ///     virtual address space of a specified process. The function initializes the 
        ///     memory it allocates to zero.
        /// </summary>
        /// <param name="hProcess">
        ///     The handle to a process. The function allocates memory within the virtual 
        ///     address space of this process.
        /// </param>
        /// <param name="lpAddress">
        ///     The address that specifies a desired starting address for the region of pages
        ///     that you want to allocate. If lpAddress is NULL, the function determines where 
        ///     to allocate the region itself (i.e. allocates new virtual memory).
        /// </param>
        /// <param name="dwSize">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="flAllocationTypes">The type of memory allocation. See MSDN.</param>
        /// <param name="flProtect">The memory protection for the region of pages to be allocated.</param>
        /// <returns>If succeeds, the base address of the allocated region of pages, else NULL.</returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationTypes flAllocationTypes, MemoryProtections flProtect);

        /// <summary>
        /// VirtualFreeEx
        ///     Releases, decommits, or releases and decommits a region of memory within the 
        ///     virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to a process. The function frees memory within the virtual 
        ///     address space of the process.
        /// </param>
        /// <param name="lpAddress">Starting address of the region of memory to be freed. </param>
        /// <param name="dwSize">The size of the region of memory to free, in bytes. </param>
        /// <param name="dwFreeTypes">The type of free operation.</param>
        /// <returns>If the function succeeds, the return value is a nonzero value, else zero.</returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, FreeTypes dwFreeTypes);

        /// <summary>
        /// WriteProcessMemory
        ///     Writes data to an area of memory in a specified process. 
        ///     The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process memory to be modified. 
        ///     The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access 
        ///     to the process.
        /// </param>
        /// <param name="lpBaseAddress">
        ///     The base address in the specified process to which data is written. 
        ///     Before data transfer occurs, the system verifies that all data in the 
        ///     base address and memory of the specified size is accessible for write 
        ///     access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="lpNumberOfBytesWritten">
        ///     Variable that receives the number of bytes transferred into the specified 
        ///     process. This parameter is optional. If lpNumberOfBytesWritten is NULL, 
        ///     the parameter is ignored.
        /// </param>
        /// <param name="lpBuffer">
        ///     A pointer to the buffer that contains data to be written 
        ///     in the address space of the specified process.
        /// </param>
        /// <param name="nSize">The number of bytes to be written to the specified process.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// ReadProcessMemory
        ///     Reads data from an area of memory in a specified process. The entire area 
        ///     to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process with memory that is being read. 
        ///     The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="lpBaseAddress">The base address in the specified process from which to read.</param>
        /// <param name="lpBuffer">Buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="dwSize">Number of bytes to be read from the specified process.</param>
        /// <param name="lpNumberOfBytesRead">
        ///     Pointer to a variable that receives the number of bytes transferred into the  
        ///     specified buffer. If lpNumberOfBytesRead is NULL, the parameter is ignored.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, IntPtr dwSize, out IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// VirtualProtect
        ///     Changes the protection on a region of committed pages in the 
        ///     virtual address space of the calling process.
        /// </summary>
        /// <param name="lpAddress">Address of which the protection should be changed.</param>
        /// <param name="dwSize">The size of memory of which the permissions should be changed.</param>
        /// <param name="flNewProtect">The new protection flags.</param>
        /// <param name="lpflOldProtect">The old protection flags.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, MemoryProtections flNewProtect, out MemoryProtections lpflOldProtect);

        /// <summary>
        /// VirtualProtectEx
        ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">
        ///     A handle to the process whose memory protection is to be changed. 
        ///     The handle must have the PROCESS_VM_OPERATION access right.
        /// </param>
        /// <param name="lpAddress">
        ///     A pointer to the base address of the region of pages whose access protection attributes are to be changed.
        ///     All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc or VirtualAllocEx function using MEM_RESERVE.
        ///     The pages cannot span adjacent reserved regions that were allocated by separate calls to VirtualAlloc or VirtualAllocEx using MEM_RESERVE.
        /// </param>
        /// <param name="dwSize">
        ///     The size of the region whose access protection attributes are changed, in bytes. 
        ///     The region of affected pages includes all pages containing one or more bytes in the range 
        ///     from the lpAddress parameter to (lpAddress+dwSize).
        ///     This means that a 2-byte range straddling a page boundary causes the protection attributes of both pages to be changed.
        /// </param>
        /// <param name="flNewProtect">
        ///     The memory protection option. This parameter can be one of the memory protection constants.
        ///     See <see cref="MemoryProtections"/>
        /// </param>
        /// <param name="lpflOldProtect">
        ///     A pointer to a variable that receives the previous access protection of the first page in the specified region of pages. 
        ///     If this parameter is NULL or does not point to a valid variable, the function fails.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
            IntPtr dwSize, MemoryProtections flNewProtect, out MemoryProtections lpflOldProtect);

        /// <summary>
        /// CreateRemoteThread
        ///     Creates a thread that runs in the virtual address space of a target process.
        /// </summary>
        /// <param name="hProcess">A handle to the process in which the thread is to be created.</param>
        /// <param name="lpThreadAttributes">
        ///     A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor 
        ///     for the new thread and determines whether child processes can inherit the returned 
        ///     handle. If lpThreadAttributes is NULL, the thread gets a default security 
        ///     descriptor and the handle cannot be inherited.
        /// </param>
        /// <param name="dwStackSize">The initial size of the stack, in bytes.</param>
        /// <param name="lpStartAddress">
        ///     A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be 
        ///     executed by the thread and represents the starting address of the thread in the 
        ///     remote process. The function must exist in the remote process. 
        /// </param>
        /// <param name="lpParameter">A pointer to a variable to be passed to the thread function. (Parameter of lpStartAddress)</param>
        /// <param name="dwCreationFlags">The flags that control the creation of the thread.</param>
        /// <param name="lpThreadId">A pointer to a variable that receives the thread identifier. </param>
        /// <returns>A handle to the newly created thread.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize, 
            IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        /// <summary>
        /// WaitForSingleObject
        ///     Waits until the specified object is in the signaled state or 
        ///     the specific time-out interval elapses.
        /// </summary>
        /// <param name="hHandle">Handle to the object in question.</param>
        /// <param name="dwMilliseconds">The time internal in milliseconds.</param>
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
        /// GetModuleHandle
        ///     Retrieves a module handle for the specified module. 
        ///     The module must have been loaded by the calling process.
        /// </summary>
        /// <param name="lpModuleName">
        ///     The name of the loaded module (either a .dll or .exe file). 
        ///     If the file name extension is omitted, the default library extension .dll is appended.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is a handle to the specified module.
        ///     If the function fails, the return value is 0. 
        /// </returns>
        /// <remarks>
        ///     Specify the parameter as null to obtain the base address of the module.
        /// </remarks>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Creates a new process and its primary thread. The new process runs in the security context of the calling process.
        /// </summary>
        /// <param name="lpApplicationName">
        /// The name of the module to be executed. This module can be a Windows-based application.
        /// The string can specify the full path and file name of the module to execute or it can specify a partial name.
        /// Partial Name: Relative to the current directory.
        /// </param>
        /// <param name="lpCommandLine">
        /// Specifies the command line parameters passed to the newly spawned process.
        /// This should be set to null.
        /// </param>
        /// <param name="lpProcessAttributes">
        /// Note: Just use NULL.
        /// A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new process object can be inherited by child processes. 
        /// If lpProcessAttributes is NULL, the handle cannot be inherited
        /// </param>
        /// <param name="lpThreadAttributes">
        /// Note: Just use NULL.
        /// A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle to the new thread object can be inherited by child processes. 
        /// If lpThreadAttributes is NULL, the handle cannot be inherited. 
        /// </param>
        /// <param name="bInheritHandles">
        /// If this parameter is TRUE, each inheritable handle in the calling process is inherited by the new process.
        /// If the parameter is FALSE, the handles are not inherited. 
        /// Note that inherited handles have the same value and access rights as the original handles.
        /// </param>
        /// <param name="dwCreationFlags">
        /// The flags that control the priority class and the creation of the process. 
        /// For a list of values, see <see cref="ProcessCreationFlags" /> 
        /// </param>
        /// <param name="lpEnvironment">
        /// A pointer to the environment block for the new process. If this parameter is NULL, the new process uses the environment of the calling process.
        /// </param>
        /// <param name="lpCurrentDirectory">
        /// The full path to the current directory for the process. The string can also specify a UNC path.
        /// If this parameter is NULL, the new process will have the same current drive and directory as the calling process. 
        /// </param>
        /// <param name="lpStartupInfo">
        /// A pointer to a STARTUPINFO or STARTUPINFOEX structure.
        /// </param>
        /// <param name="lpProcessInformation">
        /// A pointer to a PROCESS_INFORMATION structure that receives identification information about the new process. 
        /// </param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes, bool bInheritHandles, ProcessCreationFlags dwCreationFlags,
            IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        /// <summary>
        /// Determines whether the specified process is running under Windows on Windows 64.
        /// i.e. if the Process is 32 bit.
        /// </summary>
        /// <param name="hProcess">The handle to the process to check whether it is 64 bit.</param>
        /// <param name="lpSystemInfo">System info structure, which we likely want not.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="dwDesiredAccess">
        /// The access to the process object. 
        /// This access right is checked against the security descriptor for the process. This parameter can be one or more of the process access rights.
        /// </param>
        /// <param name="bInheritHandle">
        /// If this value is TRUE, processes created by this process will inherit the handle. 
        /// Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="dwProcessId">
        /// The identifier of the local process to be opened.
        /// If the specified process is the System Process (0x00000000), the function fails and the last error code is ERROR_INVALID_PARAMETER. 
        /// </param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

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
        /// Parameter of the OpenProcess method.
        /// Used to request full access of the desired process.
        /// </summary>
        public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        /// <summary>
        /// Parameter of the OpenThread method.
        /// Used to request full access of the desired thread.
        /// </summary>
        public const int THREAD_ALL_ACCESS = 0x3FB;

        /// <summary>
        /// Specifies the window station, desktop, standard handles, and appearance of the main window for a process at creation time.
        /// I'm not even going to comment this one... for now... it's a pain.
        /// </summary>
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        /// <summary>
        /// Contains information about a newly created process and its primary thread. 
        /// It is used with the CreateProcess, CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function.
        /// </summary>
        public struct PROCESS_INFORMATION
        {
            /// <summary>
            /// A handle to the newly created process. 
            /// The handle is used to specify the process in all functions that perform operations on the process object.
            /// </summary>
            public IntPtr hProcess;

            /// <summary>
            /// A handle to the primary thread of the newly created process. 
            /// The handle is used to specify the thread in all functions that perform operations on the thread object.
            /// </summary>
            public IntPtr hThread;

            /// <summary>
            /// A value that can be used to identify a process. 
            /// The value is valid from the time the process is created until all handles to the process are closed and the process object is freed; at this point, the identifier may be reused.
            /// </summary>
            public uint dwProcessId;

            /// <summary>
            /// A value that can be used to identify a thread. 
            /// The value is valid from the time the thread is created until all handles to the thread are closed and the thread object is freed; at this point, the identifier may be reused.
            /// </summary>
            public uint dwThreadId;
        }

        /// <summary>
        /// The following process creation flags are used by the CreateProcess, 
        /// CreateProcessAsUser, CreateProcessWithLogonW, and CreateProcessWithTokenW functions.
        /// See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms684863(v=vs.85).aspx 
        /// </summary>
        [Flags]
        public enum ProcessCreationFlags : uint
        {
            ZERO_FLAG = 0x00000000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00001000,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }
    }
}
