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
using System.Runtime.CompilerServices;
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
            
                Fast     - Does not change memory page permissions with VirtualProtect. Use only if you need performance.
                External - Can be used when using the library as Standalone (outside of a Reloaded Mod Loader Mod/Injected DLL).
                Unsafe   - Does not marshal managed data to unmanaged representations. Use with truly unmanaged types only!!
        */

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemoryFast<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(TType);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, size);

            // Return the read memory.
            return ConvertToPrimitive<TType>(buffer, type);
        }

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process.
        ///     Supports fully unmanaged structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static unsafe TType ReadMemoryFastUnsafe<TType>( this ReloadedProcess process, IntPtr address )
        {
            return Unsafe.Read<TType>( address.ToPointer() );
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
        public static unsafe byte[] ReadMemoryFast( this ReloadedProcess process, IntPtr address, int length)
        {
            // Read memory
            byte[] buffer = new byte[length];

            fixed (byte* pBuffer = &buffer[0])
                Unsafe.CopyBlock(pBuffer, address.ToPointer(), (uint)length);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process and converts into your chosen primitive type or struct.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemory<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(TType);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (IntPtr)size, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Read from the game memory.
            Marshal.Copy(address, buffer, 0, size);

            // Restore the old memory protection.
            VirtualProtect(address, (IntPtr)size, oldProtection, out oldProtection);

            // Return the read memory.
            return ConvertToPrimitive<TType>(buffer,type);
        }

        /// <summary>
        /// ReadMemory
        ///     Reads a specified specific amount of bytes from memory of the current process and converts into your chosen primitive type or struct.
        ///     Supports fully unmanaged structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemoryUnsafe<TType>(this ReloadedProcess process, IntPtr address)
        {
            var size = (uint)Unsafe.SizeOf<TType>();

            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (IntPtr)size, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Read value
            var value = ReadMemoryFastUnsafe<TType>( process, address );

            // Restore the old memory protection.
            VirtualProtect(address, (IntPtr)size, oldProtection, out oldProtection);

            return value;
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
        public static unsafe byte[] ReadMemory(this ReloadedProcess process, IntPtr address, int length)
        {
            // Initialize the buffer of required length.
            byte[] buffer = new byte[length];

            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (IntPtr)length, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Read from the game memory.
            fixed (byte* pBuffer = &buffer[0])
                Unsafe.CopyBlock(pBuffer, address.ToPointer(), (uint)length);

            // Restore the old memory protection.
            VirtualProtect(address, (IntPtr)length, oldProtection, out oldProtection);

            // Return the buffer.
            return buffer;
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
        public static byte[] ReadMemoryUnsafe(this ReloadedProcess process, IntPtr address, int length)
        {
            // Mark memory we are reading to as ExecuteReadWrite
            VirtualProtect(address, (IntPtr)length, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Read from the game memory.
            var buffer = ReadMemoryFast(process, address, length);

            // Restore the old memory protection.
            VirtualProtect(address, (IntPtr)length, oldProtection, out oldProtection);

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
            ReadProcessMemory(process.ProcessHandle, address, buffer, (IntPtr)length, out _);

            // Return the buffer.
            return buffer;
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemoryExternalFast<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(TType);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, (IntPtr)size, out _);

            // Return the read memory.
            return ConvertToPrimitive<TType>(buffer, type);
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        ///     Supports fully unmanaged structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static unsafe TType ReadMemoryExternalFastUnsafe<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Get type size
            int size = Unsafe.SizeOf<TType>();

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, (IntPtr)size, out _);

            fixed (byte* pBuffer = &buffer[0])
                return Unsafe.Read<TType>(pBuffer);
        }

        /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemoryExternal<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Retrieve the type of the passed in Generic.
            Type type = typeof(TType);

            // Retrieve the size of T.
            int size = Marshal.SizeOf(type);

            // Initializes the buffer of the length of the data to be read.
            byte[] buffer = new byte[size];
            
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)size, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, (IntPtr)size, out _);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)size, oldProtection, out oldProtection);

            // Return the read memory.
            return ConvertToPrimitive<TType>(buffer, type);
        }

                /// <summary>
        /// ReadMemoryExternal
        ///     Reads a specified specific amount of bytes from process memory using ReadProcessMemory.
        ///     Supports fully unmanaged structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <returns>The bytes which have been read from the memory at the specified offset and length.</returns>
        public static TType ReadMemoryExternalUnsafe<TType>(this ReloadedProcess process, IntPtr address)
        {
            // Get type size
            var size = (IntPtr)Unsafe.SizeOf<TType>();

            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtectEx(process.ProcessHandle, address, size, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            var value = ReadMemoryExternalFastUnsafe<TType>(process, address);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, size, oldProtection, out oldProtection);

            // Return the read memory.
            return value;
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
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)length, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);
            
            // Read from the game memory.
            ReadProcessMemory(process.ProcessHandle, address, buffer, (IntPtr)length, out _);

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
        public static unsafe void WriteMemoryFast(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Write the process memory.      
            fixed ( byte* pBuffer = &data[0] )
                Unsafe.CopyBlock( address.ToPointer(), pBuffer, ( uint )data.Length );
        }

        /// <summary>
        /// WriteMemory
        ///     Writes a specified specific amount of bytes to the process memory.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The structure or class marked [StructLayout(LayoutKind.Sequential)] to write to the target address.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static void WriteMemoryFast<TType>(this ReloadedProcess process, IntPtr address, TType data)
        {
            WriteStructureToAddress(data, address);
        }

        /// <summary>
        /// WriteMemory
        ///     Writes a specified specific amount of bytes to the process memory.
        ///     Supports fully unmanaged structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The structure or class marked [StructLayout(LayoutKind.Sequential)] to write to the target address.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static void WriteMemoryFastUnsafe<TType>(this ReloadedProcess process, IntPtr address, TType data)
        {
            WriteStructureToAddressUnsafe(data, address);
        }

        /// <summary>
        /// WriteMemory
        ///     Writes a specified specific amount of bytes to the process memory.
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The structure or class marked [StructLayout(LayoutKind.Sequential)] to write to the target address.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static void WriteMemory<TType>(this ReloadedProcess process, IntPtr address, TType data)
        {
            WriteMemory(process, address, ConvertStructureToByteArray(data));
        }

        /// <summary>
        /// WriteMemory
        ///     Writes a specified specific amount of bytes to the process memory.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The value you want to write at the address as a byte array.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static unsafe void WriteMemory(this ReloadedProcess process, IntPtr address, byte[] data)
        {
            // Mark memory we are writing to as ExecuteReadWrite
            VirtualProtect(address, (IntPtr)data.Length, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Write the process memory
            fixed (byte* pData = &data[0])            
                Unsafe.CopyBlock(address.ToPointer(), pData, (uint)data.Length);

            // Restore the old memory protection.
            VirtualProtect(address, (IntPtr)data.Length, oldProtection, out oldProtection);
        }

        /// <summary>
        /// WriteMemoryExternal
        ///     Writes a specified specific amount of bytes to the process using the native WriteProcessMemory call. 
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The structure or class marked [StructLayout(LayoutKind.Sequential)] to write to the target address.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemoryExternalFast<TType>(this ReloadedProcess process, IntPtr address, TType data)
        {
            // Return value
            return WriteMemoryExternalFast(process, address, ConvertStructureToByteArray(data));
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
            bool success = WriteProcessMemory(process.ProcessHandle, address, data, (IntPtr)data.Length, out _);

            // Return value
            return success;
        }

        /// <summary>
        /// WriteMemoryExternal
        ///     Writes a specified specific amount of bytes to the process using the native WriteProcessMemory call. 
        ///     Supports classes marked [StructLayout(LayoutKind.Sequential)] and regular structures.
        /// </summary>
        /// <param name="process">The process object of the game, Process.GetCurrentProcess() if injected into the game.</param>
        /// <param name="address">The address of the first byte you want to write memory to.</param>
        /// <param name="data">The structure or class marked [StructLayout(LayoutKind.Sequential)] to write to the target address.</param>
        /// <returns>Whether the write operation has been successful as true/false</returns>
        public static bool WriteMemoryExternal<TType>(this ReloadedProcess process, IntPtr address, TType data)
        {
            // Return value
            return WriteMemoryExternal(process, address, ConvertStructureToByteArray(data));
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
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)data.Length, MemoryProtections.ExecuteReadWrite, out MemoryProtections oldProtection);

            // Write the process memory.
            bool success = WriteProcessMemory(process.ProcessHandle, address, data, (IntPtr)data.Length, out _);

            // Restore the old memory protection.
            VirtualProtectEx(process.ProcessHandle, address, (IntPtr)data.Length, oldProtection, out oldProtection);

            // Return value
            return success;
        }

        /// <summary>
        /// Converts a passed in type into a primitive type.
        /// </summary>
        /// <typeparam name="TType">The type to return.</typeparam>
        /// <param name="buffer">The buffer containing the information about the specific type.</param>
        /// <param name="type">The type to convert to (same as type to return.</param>
        /// <returns>In the requested format</returns>
        public static TType ConvertToPrimitive<TType>(byte[] buffer, Type type)
        {
            // Convert to user specified structure if none of the types above apply.
            return ArrayToStructure<TType>(buffer);
        }

        /// <summary>
        /// Converts a supplied array of bytes into the user passed specified generic struct or class type.
        /// </summary>
        /// <typeparam name="TStructure">A user specified class or structure to convert an array of bytes to.</typeparam>
        /// <param name="bytes">The array of bytes to convert into a specified structure.</param>
        /// <returns>The array of bytes converted to the user's own specified class or structure.</returns>
        public static unsafe TStructure ArrayToStructure<TStructure>(byte[] bytes)
        {
            fixed (byte* ptr = &bytes[0])
            {
                try { return (TStructure)Marshal.PtrToStructure((IntPtr)ptr, typeof(TStructure)); }
                catch { return default(TStructure); }
            }
        }

        /// <summary>
        /// Converts a supplied array of bytes into the user passed specified generic struct.
        /// </summary>
        /// <typeparam name="TStructure">A user specified class or structure to convert an array of bytes to.</typeparam>
        /// <param name="bytes">The array of bytes to convert into a specified structure.</param>
        /// <returns>The array of bytes converted to the user's own specified class or structure.</returns>
        public static unsafe TStructure ArrayToStructureUnsafe<TStructure>(byte[] bytes)
        {
            fixed (byte* ptr = &bytes[0])
                return Unsafe.Read<TStructure>(ptr);
        }

        /// <summary>
        /// Converts a supplied user structure (or class marked [StructLayout(LayoutKind.Sequential)]) into an array of bytes for writing.
        /// </summary>
        /// <param name="structure">The structure to be converted to an array of bytes.</param>
        /// <returns>The user converted structure as an array of bytes.</returns>
        public static byte[] ConvertStructureToByteArray<TStructure>(TStructure structure)
        {
            // Retrieve size of structure and allocate buffer.
            int structSize = Marshal.SizeOf(structure);
            byte[] buffer = new byte[structSize];

            // Allocate memory and marshal structure into it.
            IntPtr structPointer = Marshal.AllocHGlobal(structSize);
            Marshal.StructureToPtr(structure, structPointer, true);

            // Copy the structure into our buffer.
            Marshal.Copy(structPointer, buffer, 0, structSize);

            // Free allocated memory and return structure.
            Marshal.FreeHGlobal(structPointer);
            return buffer;
        }

        /// <summary>
        /// Converts a supplied user structure into an array of bytes for writing.
        /// </summary>
        /// <param name="structure">The structure to be converted to an array of bytes.</param>
        /// <returns>The user converted structure as an array of bytes.</returns>
        public static unsafe byte[] ConvertStructureToByteArrayUnsafe<TStructure>(TStructure structure)
        {
            byte[] buffer = new byte[Unsafe.SizeOf<TStructure>()];
            fixed ( byte* pBuffer = &buffer[0] )
                Unsafe.Write( pBuffer, structure );

            return buffer;
        }

        /// <summary>
        /// Writes a supplied user structure (or class marked [StructLayout(LayoutKind.Sequential)])
        /// to a target location in the memory space of the same process.
        /// </summary>
        /// <param name="structure">The structure to be converted to an array of bytes.</param>
        /// <param name="targetAddress">The target address to write the structure contents to.</param>
        /// <returns>The user converted structure as an array of bytes.</returns>
        public static void WriteStructureToAddress<TStructure>(TStructure structure, IntPtr targetAddress)
        {
            // Allocate memory and marshal structure into it.
            Marshal.StructureToPtr(structure, targetAddress, true);
        }

        /// <summary>
        /// Writes a supplied user structure to a target location in the memory space of the same process.
        /// </summary>
        /// <param name="structure">The structure to be converted to an array of bytes.</param>
        /// <param name="targetAddress">The target address to write the structure contents to.</param>
        /// <returns>The user converted structure as an array of bytes.</returns>
        public static unsafe void WriteStructureToAddressUnsafe<TStructure>(TStructure structure, IntPtr targetAddress)
        {
            Unsafe.Write(targetAddress.ToPointer(), structure);
        }
    }
}
