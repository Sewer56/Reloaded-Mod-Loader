using System;
using System.Runtime.InteropServices;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Defines the native Windows API function imports used within the current namespace.
    /// </summary>
    public class Native
    {
        ///
        /// DLL IMPORTS
        ///

        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">
        /// The name of the module.This can be either a library module (a.dll file) or an executable module (an.exe file).
        /// If the string specifies a full path, the function searches only that path for the module.
        /// If the string specifies a relative path or a module name without a path, the function uses the standard search strategy.
        /// </param>
        /// <returns>Handle to the module.</returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">The handle to the module from which the function or variable is to be called.</param>
        /// <param name="procName">The name of the function for which the address is to be obtained for.</param>
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified process. 
        /// The function initializes the memory it allocates to zero.
        /// </summary>
        /// <param name="hProcess">The handle to a process. The function allocates memory within the virtual address space of this process.</param>
        /// <param name="lpAddress">The address that specifies a desired starting address for the region of pages that you want to allocate.</param>
        /// <param name="dwSize">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="flAllocationType">The type of memory allocation. See MSDN.</param>
        /// <param name="flProtect">The memory protection for the region of pages to be allocated.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">A handle to a process. The function frees memory within the virtual address space of the process.</param>
        /// <param name="lpAddress">Starting address of the region of memory to be freed. </param>
        /// <param name="dwSize">The size of the region of memory to free, in bytes. </param>
        /// <param name="dwFreeType">The type of free operation.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="hProcess">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="lpBaseAddress">
        /// The base address in the specified process to which data is written. 
        /// Before data transfer occurs, the system verifies that all data in the base address and memory 
        /// of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="nSize">The number of bytes to be written to the specified process.</param>
        /// <param name="lpNumberOfBytesWritten">
        /// Variable that receives the number of bytes transferred into the specified process. 
        /// This parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.
        /// </param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="hProcess">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="lpBaseAddress">The base address in the specified process from which to read.</param>
        /// <param name="lpBuffer">Buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="dwSize">Number of bytes to be read from the specified process.</param>
        /// <param name="lpNumberOfBytesRead">Pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is NULL, the parameter is ignored.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of the calling process.
        /// </summary>
        /// <param name="lpAddress">Address of which the protection should be changed.</param>
        /// <param name="dwSize">The size of memory of which the permissions should be changed.</param>
        /// <param name="flNewProtect">The new protection flags.</param>
        /// <param name="lpflOldProtect">The old protection flags.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="hProcess">A handle to the process in which the thread is to be created. </param>
        /// <param name="lpThreadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be inherited.</param>
        /// <param name="dwStackSize">The initial size of the stack, in bytes.</param>
        /// <param name="lpStartAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process. </param>
        /// <param name="lpParameter">A pointer to a variable to be passed to the thread function. (Parameter of lpStartAddress)</param>
        /// <param name="dwCreationFlags">The flags that control the creation of the thread.</param>
        /// <param name="lpThreadId">A pointer to a variable that receives the thread identifier. </param>
        /// <returns>A handle to the newly created thread.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);


        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// </summary>
        /// <param name="hHandle">Handle to the object in question.</param>
        /// <param name="dwMilliseconds">The time internal in milliseconds.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        /// <summary>
        /// Retrieves the termination status of the specified thread.
        /// </summary>
        /// <param name="hThread">A handle to the thread.</param>
        /// <param name="lpExitCode">A pointer to a variable to receive the thread termination status. For more information, see Remarks.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        ///
        /// FLAGS
        ///

        /// <summary>
        /// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa366786(v=vs.85).aspx"/>
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
        /// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa366894(v=vs.85).aspx"/> 
        /// </summary>
        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        /// <summary>
        /// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa366890(v=vs.85).aspx"/>
        /// </summary>
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }
    }
}
