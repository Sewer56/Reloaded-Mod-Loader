using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SonicHeroes.Memory
{
    /// <summary>
    /// This is a class that extends the Process class of InteropServices, allowing various memory related actions to be performed to the game, such as writing to memory, reading to memory, allocating memory etc. These additional features should be adequate to allow for the injection and manipulation of C# based assemblies inside the game.
    /// </summary>
    public static class HeroesProcess
    {
        /// <summary>
        /// An Import of Windows' Kernel32.
        /// </summary>
        private static IntPtr Kernel32Library;
        /// <summary>
        /// Pointer to the address of the exported LoadLibraryA function of Kernel32 to load a library into another process. Allows for loading a specified module into the address space of the calling process. 
        /// </summary>
        private static IntPtr LoadLibraryX; 

        static HeroesProcess()
        {
            Kernel32Library = LoadLibrary("kernel32"); // This should automatically resolve to kernel32.dll as it is registered by Windows.
            LoadLibraryX = GetProcAddress(Kernel32Library, "LoadLibraryA"); // Retrieves the address of LoadLibraryA function.
        }

        /// <summary>
        /// Allows for allocation of space inside the target process, in our case, Sonic Heroes. The return value for this method is the address at which the new memory has been reserved. You may use this extra space to e.g. insert assembly code to which you may jump to.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Length">Length of free bytes you want to allocate.</param>
        /// <returns>Base pointer address to the newly allocated memory.</returns>
        public static IntPtr AllocateMemory(this Process Process, int Length)
        {
            // Call VirtualAllocEx to allocate memory of fixed chosen size.
            return VirtualAllocEx(Process.Handle, IntPtr.Zero, (IntPtr)Length,
                AllocationType.Commit | AllocationType.Reserve,
                MemoryProtection.ExecuteReadWrite);
        }

        /// <summary>
        /// Allows for the freeing of memory space inside the target process. Releases memory such that it may be cleaned and re-used by the Windows Operating System.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Pointer">The address of the first byte you want to free memory from.</param>
        /// <returns>A value that is not 0 if the operation is successful.</returns>
        public static bool FreeMemory(this Process Process, IntPtr Pointer)
        {
            return VirtualFreeEx(Process.Handle, Pointer, 0,
                FreeType.Decommit | FreeType.Release);
        }

        /// <summary>
        /// Writes a specified specific amount of bytes to the process using the native WriteProcessMemory call. 
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Pointer">The address of the first byte you want to write memory to.</param>
        /// <param name="Data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemory(this Process Process, IntPtr Pointer, byte[] Data)
        {
            IntPtr nBytes;
            MemoryProtection OldProtection;
            VirtualProtect(Pointer, (uint)Data.Length, MemoryProtection.ExecuteReadWrite, out OldProtection);
            return WriteProcessMemory(Process.Handle, Pointer, Data,
                Data.Length, out nBytes);
        }

        /// <summary>
        /// Reads a specified specific amount of bytes to the process using the native ReadProcessMemory call. 
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Pointer">The address of the first byte you want to write memory to.</param>
        /// <param name="Length">The value you want to write at the address as a byte array.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static byte[] ReadMemory(this Process Process, IntPtr Pointer, int Length)
        {
            var Data = new byte[Length];
            IntPtr nBytes;
            ReadProcessMemory(Process.Handle, Pointer, Data, Length, out nBytes);
            return Data;
        }

        /// <summary>
        /// Makes a call to the injected library to verify validity. Returns the thread executing the library in parallel.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Pointer">Pointer to the starting point of the library, module or library method to be executed.</param>
        /// <param name="ParameterPointer">Pointer to parameters for the method to be called.</param>
        /// <returns>An exit code</returns>
        public static IntPtr CallLibraryAsync(this Process Process, IntPtr Pointer, IntPtr ParameterPointer)
        {
            IntPtr ThreadID;
            var hThread = CreateRemoteThread(Process.Handle, IntPtr.Zero, 0,
                Pointer, ParameterPointer, 0, out ThreadID);
            return hThread;
        }

        /// <summary>
        /// Makes a call to the injected library to verify validity. Waits for an exit code. Returns a nonzero value declaring successful execution, or zero value for failed.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="Pointer">Pointer to the starting point of the library, module or library method to be executed.</param>
        /// <param name="ParameterPointer">Pointer to parameters for the method to be called.</param>
        /// <returns>An exit code</returns>
        public static int CallLibrary(this Process Process, IntPtr Pointer, IntPtr ParameterPointer)
        {
            IntPtr ThreadID;
            var hThread = CreateRemoteThread(Process.Handle, IntPtr.Zero, 0,
                Pointer, ParameterPointer, 0, out ThreadID);
            WaitForSingleObject(hThread, unchecked((uint)-1));
            uint exitCode;
            GetExitCodeThread(hThread, out exitCode);
            return (int)exitCode;
        }

        /// <summary>
        /// Loads a library such as your mod .dll into the specified process in question. Memory is allocated into the process equivalent of the library length, the library is written into the process and the address of the called method is returned such that the method may be re-used again.
        /// </summary>
        /// <param name="Process">The process object of Sonic Heroes, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="LibraryPath">Pointer to the starting point of the library, module or library method to be executed.</param>
        /// <returns></returns>
        public static IntPtr LoadLibrary(this Process Process, string LibraryPath)
        {
            var LibraryData = Encoding.UTF8.GetBytes(LibraryPath);
            var ProcessPointer = Process.AllocateMemory(LibraryData.Length);
            Process.WriteMemory(ProcessPointer, LibraryData);
            // Loads the specified module into the address space of the calling process using LoadLibraryA such that the loaded library may be executed, returns a pointer/handle to the library/module.
            var LibraryAddress = Process.CallLibrary(LoadLibraryX, ProcessPointer);
            return (IntPtr)LibraryAddress;
        }

        // Retrieves the address of an exported function or variable from a specified DLL.s
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        IntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, FreeType dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll")]
        static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize,
        MemoryProtection flNewProtect, out MemoryProtection lpflOldProtect);
    }
}

[Flags]
public enum FreeType
{
    Decommit = 0x4000,
    Release = 0x8000,
}

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