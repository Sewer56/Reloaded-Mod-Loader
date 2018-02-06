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

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Defines native Windows API function imports used within the current namespace to aid in
    /// the services of DLL Injection, Memory Manipulation and other similar arbitrary functions.
    /// </summary>
    public class Native
    {
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
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        /// <summary>
        /// GetProcAddress
        ///     Retrieves the address of an exported function or variable from the specified 
        ///     dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">The handle to the module to get function/variable.</param>
        /// <param name="procName">The name of the function or variable for which the address is to be obtained for.</param>
        /// <returns>Address of exported function or variable.</returns>
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
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
        /// <param name="flAllocationType">The type of memory allocation. See MSDN.</param>
        /// <param name="flProtect">The memory protection for the region of pages to be allocated.</param>
        /// <returns>If succeeds, the base address of the allocated region of pages, else NULL.</returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

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
        /// <param name="dwFreeType">The type of free operation.</param>
        /// <returns>If the function succeeds, the return value is a nonzero value, else zero.</returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

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
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

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
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

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
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);

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
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, 
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
        /// </returns>
        /// <remarks>
        ///     Specify the parameter as null to obtain the base address of the module.
        /// </remarks>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// MemoryProtection
        ///     Specifies the memory protection constants for the region of pages 
        ///     to be allocated, referenced or used for a similar purpose.
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366786(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum MemoryProtection
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
        /// FreeType
        ///     The type of free operation. This parameter can be either Decommit or Release.
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366894(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum FreeType
        {
            /// <summary>
            /// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state. 
            /// </summary>
            Decommit = 0x4000,
            /// <summary>
            /// Releases the specified region of pages. After the operation, the pages are in the free state. 
            /// </summary>
            Release = 0x8000,
        }

        /// <summary>
        /// AllocationType
        ///     Specifies the type of memory allocation to be used alongside functions such as VirtualCommitEx
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366890(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum AllocationType
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
    }
}
