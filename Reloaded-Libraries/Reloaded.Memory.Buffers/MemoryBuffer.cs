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
using System.Collections.Generic;
using System.Diagnostics;
using Reloaded.Memory.Buffers.Structs;
using Reloaded.Memory.Buffers.Utilities;
using Reloaded.Memory.Sources;
using Reloaded.Utilities.Mathematics;
using Vanara.PInvoke;
using static Reloaded.Memory.Buffers.Utilities.VirtualQueryUtility;

namespace Reloaded.Memory.Buffers
{
    /// <summary>
    /// Provides a buffer for general small size memory storage, storage of custom assembly code as well as 
    /// address addresses for assembly instructions taking "m32" parameters, e.g. jmp dword [0x123456]. 
    /// See the MemoryBuffer source code for a more detailed explanation and purpose.
    /// </summary>
    public class MemoryBuffer
    {
        /*
            Notes:
                If you plan to modify your written data in the future, save the address it was written at, this is a thin implementation.

                If the data you are writing is larger than the remaining bytes in the buffer, the written memory will be entirely reserved by your function.
                You must set the target process manually if you are using this from an external program.
                The memory written to these buffers is never moved or freed.

                In most cases you'll also probably want to be using MemoryBufferManager either way.
         */

        /*
            Original Purpose:

            The minimum granularity (size in bytes) of the AllocateMemory function (wrapping VirtualAllocEx) is generally 65535 bytes,
            equivalent to a few memory pages, meaning that even if you request 4 bytes, 65535 bytes are automatically allocated.

            In the cases that you would wish to use indirect (m32) parameters jumps, calls or parameters in general for assembly purposes, this
            class exists such that memory is not wasted on each allocation, as under normal circumstances, most of the time the other 60000+
            bytes would be left zeroed or unused.

            The most common use of this class is for multiple assembly operations such as storing "trampoline" code (stub to call original code from our own code),
            your custom assembly code as well as long term variable storage that needs to be accessed by an external source. 

            tl;dr Use this for writing any kind of ASM stuff into memory, saves heaps of memory.
         */

        /// <summary>
        /// Specifies the byte alignment of each item that will be added onto the buffer.
        /// </summary>
        private const int Alignment = 4;

        /// <summary>
        /// Contains an instance of Magic to compare new potential already existing Memory Buffers against.
        /// </summary>
        public static MemoryBufferMagic BufferMagic = new MemoryBufferMagic(true);

        /// <summary>
        /// Returns true if the entire contents of the buffer lie within the range of the first 2GB of memory.
        /// </summary>
        public bool Is32BitBuffer => (ulong)BufferAddress + (uint)BufferHeader.BufferSize <= Int32.MaxValue;

        /// <summary>
        /// Stores the base address after the magic header.
        /// The base address starts with the <see cref="MemoryBufferHeader"/> structure and then raw data.
        /// </summary>
        public IntPtr BufferAddress { get; private set; }

        /// <summary>
        /// Contains the "Header" of the buffer which defines various properties such
        /// as the current offset and size.
        /// </summary>
        private MemoryBufferHeader BufferHeader
        {
            get => _memorySource.Read<MemoryBufferHeader>(BufferAddress);
            set => _memorySource.Write(BufferAddress, ref value, false);
        }

        /// <summary>
        /// Defines where Memory will be read in or written to.
        /// </summary>
        private IMemory _memorySource;

        /*
            --------------
            Constructor(s)
            --------------
        */

        /// <summary>
        /// Creates a new buffer in a specified location in memory with a specified size.
        /// This constructor will override any existing buffer! Please use <see cref="FromAddress"/> instead
        /// if you wish to get a hold of an already existing buffer..
        /// </summary>
        /// <param name="process">The process inside which the <see cref="MemoryBuffer"/> will be allocated.</param>
        /// <param name="bufferAddress">Specifies the base address of the new buffer to be created. The "magic" will be written there and buffer right after.</param>
        /// <param name="bufferSize">The size of the buffer to be created. Note that the buffer size includes the buffer header!</param>
        /// <param name="isBufferPreallocated">Set this to true if the position where the buffer is being created has already been allocated.</param>
        public unsafe MemoryBuffer(Process process, IntPtr bufferAddress, int bufferSize, bool isBufferPreallocated = false)
        {
            if (!isBufferPreallocated)
            {
                // Get the function, commit the pages and check.
                var virtualAllocFunction = VirtualAllocUtility.GetVirtualAllocFunction(process);
                var address = virtualAllocFunction(process.Handle, bufferAddress, (uint)bufferSize);

                if (address == IntPtr.Zero)
                    throw new Exception("Failed to allocate MemoryBuffer.");
            }

            // Setup R/W and write a new Buffer Magic
            _memorySource = GetMemorySource(process);
            _memorySource.Write(bufferAddress, ref BufferMagic);

            // Setup buffer after Magic.
            BufferAddress = bufferAddress + sizeof(MemoryBufferMagic);
            BufferHeader = new MemoryBufferHeader(sizeof(MemoryBufferHeader), bufferSize - sizeof(MemoryBufferMagic));
        }

        /// <summary>
        /// Creates a new <see cref="MemoryBuffer"/> given the address of the buffer and the header details.
        /// </summary>
        /// <param name="memorySource">Contains the source used for read/write of memory.</param>
        /// <param name="bufferAddress">The address of the memory buffer; this follows the memory buffer "magic".</param>
        private MemoryBuffer(IMemory memorySource, IntPtr bufferAddress)
        {
            BufferAddress = bufferAddress;
            _memorySource = memorySource;
        }

        /*
            -----------------
            Factory Method(s)
            -----------------
        */

        /// <summary>
        /// Attempts to find an existing <see cref="MemoryBuffer"/> at the specified address and returns an instance of it.
        /// If the operation fails; the function returns null.
        /// </summary>
        public static unsafe MemoryBuffer FromAddress(Process process, IntPtr bufferMagicAddress)
        {
            // Query the region we are going to create a buffer in.
            var virtualQueryFunction = GetVirtualQueryFunction(process);
            var memoryInformation = virtualQueryFunction(process.Handle, bufferMagicAddress);

            if (memoryInformation.State != (uint)Kernel32.MEM_ALLOCATION_TYPE.MEM_FREE)
            {
                if (IsBuffer(process, bufferMagicAddress))
                    return new MemoryBuffer(GetMemorySource(process), bufferMagicAddress + sizeof(MemoryBufferMagic));
            }

            return null;
        }

        /*
            -----------------
            Factory Helper(s)
            -----------------
        */

        /// <summary>
        /// Checks if a <see cref="MemoryBuffer"/> exists at this location by comparing the bytes available here
        /// against the MemoryBuffer "Magic".
        /// </summary>
        public static bool IsBuffer(Process process, IntPtr bufferMagicAddress)
        {
            // Check if buffer already exists by reading an existing or nonexisting header.
            MemoryBufferMagic bufferMagic = GetMemorySource(process).SafeRead<MemoryBufferMagic>(bufferMagicAddress);

            // Compare magic and return if equal.
            if (BufferMagic.MagicEquals(ref bufferMagic))
                return true;

            return false;
        }

        /*
            --------------
            Core Functions
            --------------
        */

        /// <summary>
        /// Writes your own memory bytes into process' memory and gives you the address
        /// for the memory location of the written bytes.
        /// </summary>
        /// <param name="bytesToWrite">Individual bytes to be written onto the buffer.</param>
        /// <returns>Pointer to the passed in bytes written to memory. Null pointer, if it cannot fit into the buffer.</returns>
        public IntPtr Add(byte[] bytesToWrite)
        {
            // Read the buffer contents back from memory.
            var bufferHeader = BufferHeader;

            // Do not add beyond buffer size.
            if (!CheckItemSize(bytesToWrite.Length))
                return IntPtr.Zero;

            // If the buffer is invalid, do not add either.
            if (BufferAddress == IntPtr.Zero)
                return IntPtr.Zero;

            // Do the append operation.
            IntPtr appendAddress = BufferAddress + bufferHeader.BufferOffset;
            Sources.Memory.Current.SafeWriteRaw(appendAddress, bytesToWrite);

            // Set current offset.
            bufferHeader.BufferOffset += bytesToWrite.Length;
            bufferHeader.BufferOffset = Mathematics.RoundUp(bufferHeader.BufferOffset, Alignment);

            // Write the new buffer contents back to memory.
            BufferHeader = bufferHeader;

            return appendAddress;
        }

        /// <summary>
        /// Writes your own structure address into process' memory and gives you the address 
        /// to which the structure has been directly written to.
        /// </summary>
        /// <param name="bytesToWrite">A structure to be converted into individual bytes to be written onto the buffer.</param>
        /// <returns>Pointer to the newly written structure in memory. Null pointer, if it cannot fit into the buffer.</returns>
        public IntPtr Add<TStructure>(TStructure bytesToWrite)
        {
            // Know what to do
            return Add(Struct.GetBytes(ref bytesToWrite));
        }

        /// <summary>
        /// Returns true if the object can fit into the buffer, else false.
        /// </summary>
        /// <param name="objectSize">The size of the object to be appended to the buffer.</param>
        /// <returns>Returns true if the object can fit into the buffer, else false.</returns>
        public bool CheckItemSize(int objectSize)
        {
            var header = BufferHeader;

            // Check if base buffer uninitialized or if object size too big.
            if (header.BufferOffset + objectSize > header.BufferSize)
                return false;
            else
                return true;
        }

        /// <summary>
        /// The two <see cref="MemoryBuffer"/>s are equal if their base address is the same.
        /// </summary>
        public override bool Equals(object obj)
        {
            var buffer = obj as MemoryBuffer;
            return buffer != null && BufferAddress == buffer.BufferAddress;
        }

        public override int GetHashCode()
        {
            return (int)BufferAddress;
        }

        /// <summary>
        /// Assigns a Memory Source to a buffer.
        /// </summary>
        /// <param name="process">The process where memory will be read/written to.</param>
        private static IMemory GetMemorySource(Process process)
        {
            if (process.Id == Process.GetCurrentProcess().Id)
                return new Sources.Memory();
            else
                return new ExternalMemory(process);
        }
    }
}
