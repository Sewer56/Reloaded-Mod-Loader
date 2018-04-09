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
using static Reloaded.Process.Native.Native;

namespace Reloaded.Process.Memory
{
    /// <summary>
    /// Class which allows the manipulation of in-game memory.
    /// This file provides the implementation for reading/writing of memory.
    /// See the class for the key to the different method names.
    /// </summary>
    public static class MemoryReadWrite
    {
        /*
            Key for the method names:
            
                Fast - Does not change memory page permissions with VirtualProtect. Use only if you need performance.
                External - Can be used when using the library as Standalone (outside of a Reloaded Mod Loader Mod/Injected DLL).
        */

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

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemoryFast<T>(this ReloadedProcess process, IntPtr address)
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
            return (T)ConvertToPrimitive<T>(buffer, type);
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
        public static byte[] ReadMemoryFast(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, length);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process and converts into your chosen primitive type.
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
            
            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (uint)size, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, size);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)size, oldProtection, out oldProtection);

            // Return the read memory.
            return (T) ConvertToPrimitive<T>(buffer,type);
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

            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (uint)length, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, length);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)length, oldProtection, out oldProtection);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes using ReadProcessMemory(), does no conversion and returns as untouched byte array. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="length">The value you want to write at the address as a byte array.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static byte[] ReadMemoryExternalFast(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, length, out IntPtr bytesRead);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static T ReadMemoryExternalFast<T>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(T);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, size, out IntPtr bytesRead);

            // Return the read memory.
            return (T)ConvertToPrimitive<T>(buffer, type);
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
            
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)size, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);

            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, size, out IntPtr bytesRead);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)size, oldProtection, out oldProtection);

            // Return the read memory.
            return (T)ConvertToPrimitive<T>(buffer, type);
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
            
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)length, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);
            
            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, length, out IntPtr bytesRead);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)length, oldProtection, out oldProtection);

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
        public static void WriteMemoryFast(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Write the process memory.            
            Marshal.Copy(data, 0, address, data.Length);
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
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtect(address, (uint)data.Length, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);

            // Write the process memory.            
            Marshal.Copy(data, 0, address, data.Length);

            // Restore the old memory protection.
            VirtualProtect(address, (uint)data.Length, oldProtection, out oldProtection);
        }

        /// <summary>
        /// WriteMemoryExternal
        ///     Writes a specified specific amount of bytes to the process using the native WriteProcessMemory call. 
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemoryExternalFast(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Write the process memory.
            bool success = WriteProcessMemory(process.ProcessHandle, address, data, data.Length, out IntPtr bytesWrite);

            // Return value
            return success;
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
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)data.Length, MemoryProtection.ExecuteReadWrite, out MemoryProtection oldProtection);

            // Write the process memory.
            bool success = WriteProcessMemory(process.ProcessHandle, address, data, data.Length, out IntPtr bytesWrite);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)data.Length, oldProtection, out oldProtection);

            // Return value
            return success;
        }

        /// <summary>
        /// Converts a passed in type into a primitive type.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="buffer">The buffer containing the information about the specific type.</param>
        /// <param name="type">The type to convert to (same as type to return.</param>
        /// <returns>In the requested format</returns>
        public static object ConvertToPrimitive<T>(byte[] buffer, Type type)
        {
            switch (type.Name)
            {
                case nameof(String): return BitConverter.ToString(buffer, 0);
                case nameof(Boolean): return BitConverter.ToBoolean(buffer, 0);
                case nameof(Char): return BitConverter.ToChar(buffer, 0);
                case nameof(Byte): return (T)Convert.ChangeType(buffer[0], typeof(T));
                case nameof(Single): return BitConverter.ToSingle(buffer, 0);
                case nameof(Double): return BitConverter.ToDouble(buffer, 0); 
                case nameof(Int32): return BitConverter.ToInt32(buffer, 0);
                case nameof(UInt32): return BitConverter.ToUInt32(buffer, 0);
                case nameof(UInt16): return BitConverter.ToUInt16(buffer, 0);
                case nameof(Int16): return BitConverter.ToInt16(buffer, 0);
            }
            return null;
        }
    }
}
