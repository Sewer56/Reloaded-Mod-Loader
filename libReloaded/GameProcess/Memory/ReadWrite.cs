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
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Reloaded.GameProcess.Native;

namespace Reloaded.GameProcess
{
    /// <summary>
    /// Class which allows the manipulation of in-game memory.
    /// This file provides the implementation for reading/writing of memory.
    /// </summary>
    public static partial class Memory
    {
        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemory<T>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(T);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, size);

            // Return the read memory.
            return (T)Convert.ChangeType(buffer, typeof(T));
        }

        /// <summary>
        /// See <see cref="ReadMemory"/>. Use if you are denied normally reading memory. Reads game memory but first removes protection on a page of memory before reading the memory followed by restoring it.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemorySafe<T>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(T);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];

            // Store the old memory protection flags.
            MemoryProtection OldProtection;

            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (uint)size, MemoryProtection.ExecuteReadWrite, out OldProtection);

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, size);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)size, OldProtection, out OldProtection);

            // Return the read memory.
            return (T)Convert.ChangeType(buffer, typeof(T));
        }

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process.
        ///     Returns the raw bytes back, without conversion.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="length">The value you want to write at the address as a byte array.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static byte[] ReadMemory(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, length);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// <see cref="ReadMemory"/> Use if you are denied normally reading memory. Reads game memory but first removes protection on a page of memory before reading the memory followed by restoring it. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="length">The value you want to write at the address as a byte array.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static byte[] ReadMemorySafe(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Store the old memory protection flags.
            MemoryProtection OldProtection;

            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (uint)length, MemoryProtection.ExecuteReadWrite, out OldProtection);

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, length);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)length, OldProtection, out OldProtection);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// WriteMemory
        ///     Writes a specified specific amount of bytes to the process memory. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static void WriteMemory(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Write the process memory.            
            Marshal.Copy(data, 0, address, data.Length);
        }

        /// <summary>
        /// See <see cref="WriteMemory"/> Use if you are denied normally writing memory. Writes game memory but first removes protection on a page of memory before writing the memory followed by restoring it. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static void WriteMemorySafe(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Store the old memory protection flags.
            MemoryProtection OldProtection;

            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtect(address, (uint)data.Length, MemoryProtection.ExecuteReadWrite, out OldProtection);

            // Write the process memory.            
            Marshal.Copy(data, 0, address, data.Length);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)data.Length, OldProtection, out OldProtection);
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemoryExternal<T>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(T);
            
            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];

            // Store the amount of bytes read.
            IntPtr bytesRead;

            // Read from the game memory.
            ReadProcessMemory(process.processHandle, address, buffer, size, out bytesRead);
            
            // Return the read memory.
            return (T)Convert.ChangeType(buffer, typeof(T));
        }

        /// <summary>
        /// See <see cref="ReadMemoryExternal"/>. Use if you are denied normally reading memory. Reads game memory but first removes protection on a page of memory before reading the memory followed by restoring it.  
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemoryExternalSafe<T>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(T);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];

            // Store the amount of bytes read.
            IntPtr bytesRead;

            // Store the old memory protection flags.
            MemoryProtection OldProtection;

            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.processHandle, address, (IntPtr)size, MemoryProtection.ExecuteReadWrite, out OldProtection);

            // Read from the game memory.
            ReadProcessMemory(process.processHandle, address, buffer, size, out bytesRead);

            // Restore the old memory protection.
            VirtualProtectEx(process.processHandle, address, (IntPtr)size, OldProtection, out OldProtection);

            // Return the read memory.
            return (T)Convert.ChangeType(buffer, typeof(T));
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes using ReadProcessMemory(), does no conversion and returns as untouched byte array. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="length">The value you want to write at the address as a byte array.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static byte[] ReadMemoryExternal(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Store the amount of bytes read.
            IntPtr bytesRead;

            // Read from the game memory.
            ReadProcessMemory(process.processHandle, address, buffer, length, out bytesRead);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// WriteMemoryExternal
        ///     Writes a specified specific amount of bytes to the process using the native WriteProcessMemory call. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemoryExternal(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Store the amount of bytes written.
            IntPtr bytesWrite;

            // Write the process memory.
            bool success = WriteProcessMemory(process.processHandle, address, data, data.Length, out bytesWrite);

            // Return value
            return success;
        }

        /// <summary>
        /// See <see cref="WriteMemoryExternal"/>  Use only if you are denied normally writing memory. Writes game memory but first removes protection on a page of memory before writing the memory followed by restoring it.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemoryExternalSafe(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Store the amount of bytes written.
            IntPtr bytesWrite;

            // Store the old memory protection flags.
            MemoryProtection OldProtection;

            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.processHandle, address, (IntPtr)data.Length, MemoryProtection.ExecuteReadWrite, out OldProtection);

            // Write the process memory.
            bool success = WriteProcessMemory(process.processHandle, address, data, data.Length, out bytesWrite);

            // Restore the old memory protection.
            VirtualProtectEx(process.processHandle, address, (IntPtr)data.Length, OldProtection, out OldProtection);

            // Return value
            return success;
        }

        /// <summary>
        /// GetBaseAddress
        ///     Retrieves the base address of the module, i.e. 0x400000 for executables
        ///     not using Address Space Layout Randomization.
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetBaseAddress()
        {
            return GetModuleHandle(null);
        }
    }
}
