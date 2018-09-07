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
using System.Runtime.InteropServices;
using Reloaded.Process.Memory;
using static Reloaded.Process.Native.Native;

namespace Reloaded.Process.Buffers
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
        /// The base address to the current buffer for our stored data.
        /// Now exposed.
        /// </summary>
        public IntPtr BaseBufferAddress { get; private set; }

        /// <summary>
        /// Returns true if the entire contents of the buffer lie within the range of the first 2GB of memory.
        /// </summary>
        public bool Is32BitBuffer => (ulong)BaseBufferAddress + BufferHeader.BufferSize < 2147483648;

        /// <summary>
        /// Contains the individual details of our buffer, such as the size of the buffer or the current offset
        /// within the buffer in question.
        /// </summary>
        public MemoryBufferHeader BufferHeader;

        /// <summary>
        /// Creates a new buffer in a specified location in memory with a specified size.
        /// This constructor is not intended to be used directly, consider using <see cref="MemoryBufferManager"/> instead.
        /// While you can use this constructor if you're sure the memory pages to be allocated are free, it is not recommended.
        /// MemoryBuffer creates a buffer if it doesn't exist, if the supplied address is already allocated and is not a buffer, sets the Base Address to 0.
        /// </summary>
        /// <param name="bufferAddress">Specifies the base address of the new buffer to be created.</param>
        /// <param name="bufferSize">The size of the buffer to be created. Note that the buffer size includes the buffer header!</param>
        public unsafe MemoryBuffer(IntPtr bufferAddress, uint bufferSize)
        {
            // Set the base address for the buffer.
            this.BaseBufferAddress = bufferAddress;

            // Get our info on desired page from VirtualQueryEx.
            MEMORY_BASIC_INFORMATION memoryInformation = new MEMORY_BASIC_INFORMATION();
            VirtualQueryEx(Bindings.TargetProcess.ProcessHandle, bufferAddress, out memoryInformation, (uint)sizeof(MEMORY_BASIC_INFORMATION));

            // If the page is already occupied, check and assign the buffer header.
            if (memoryInformation.State != PageState.Free)
            {
                // Check if buffer already exists by reading an existing or nonexisting header.
                MemoryBufferHeader bufferHeader = Bindings.TargetProcess.ReadMemoryExternal<MemoryBufferHeader>(BaseBufferAddress);

                // If buffer already exists, set buffer's header.
                if (bufferHeader.ReloadedMagic == MemoryBufferHeader.RELOADED_MAGIC)
                    BufferHeader = bufferHeader;
                else
                    BaseBufferAddress = IntPtr.Zero;
 
            }

            // Else if the pages are free.
            else
            {
                // Commit the pages.
                IntPtr virtualAllocAddress = VirtualAllocEx
                (
                    Bindings.TargetProcess.ProcessHandle,
                    BaseBufferAddress,
                    bufferSize,
                    AllocationTypes.Reserve | AllocationTypes.Commit,
                    MemoryProtections.ExecuteReadWrite
                );

                // MemoryBuffer.
                if (virtualAllocAddress == IntPtr.Zero)
                {
                    Bindings.PrintError("[FATAL] Failed to allocate MemoryBuffer.");
                    throw new Exception("Failed to allocate MemoryBuffer.");
                }

                // Write a new Buffer
                BufferHeader = new MemoryBufferHeader()
                {
                    BufferOffset = (uint)sizeof(MemoryBufferHeader),
                    BufferSize = bufferSize,
                    ReloadedMagic = MemoryBufferHeader.RELOADED_MAGIC
                };

                Bindings.TargetProcess.WriteMemoryExternal(BaseBufferAddress, ref BufferHeader);
            }
        }

        /// <summary>
        /// Writes your own memory bytes into process' memory and gives you the address
        /// for the memory location of the written bytes.
        /// </summary>
        /// <param name="bytesToWrite">Individual bytes to be written onto the buffer.</param>
        /// <returns>Pointer to the passed in bytes written to memory. Null pointer, if it cannot fit into the buffer.</returns>
        public IntPtr Add(byte[] bytesToWrite)
        {
            // Read the buffer contents back from memory.
            BufferHeader = Bindings.TargetProcess.ReadMemoryExternal<MemoryBufferHeader>(BaseBufferAddress);

            // Do not add beyond buffer size.
            if (!CheckItemSize(bytesToWrite.Length))
                return IntPtr.Zero;

            // If the buffer is invalid, do not add either.
            if (BaseBufferAddress == IntPtr.Zero)
                return IntPtr.Zero;

            // Do the append operation.
            IntPtr appendAddress = (IntPtr)((ulong)BaseBufferAddress + BufferHeader.BufferOffset);
            Bindings.TargetProcess.WriteMemoryExternal(appendAddress, ref bytesToWrite);

            // Set current offset.
            BufferHeader.BufferOffset += (uint)bytesToWrite.Length;

            // Write the new buffer contents back to memory.
            Bindings.TargetProcess.WriteMemoryExternal(BaseBufferAddress, ref BufferHeader);

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
            return Add(MemoryReadWrite.ConvertStructureToByteArrayUnsafe(ref bytesToWrite));
        }

        /// <summary>
        /// Returns true if the object can fit into the buffer, else false.
        /// </summary>
        /// <param name="objectSize">The size of the object to be appended to the buffer.</param>
        /// <returns>Returns true if the object can fit into the buffer, else false.</returns>
        public bool CheckItemSize(int objectSize)
        {
            // Check if base buffer uninitialized or if object size too big.
            if (BufferHeader.BufferOffset + objectSize > BufferHeader.BufferSize)
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
            return buffer != null && BaseBufferAddress == buffer.BaseBufferAddress;
        }

        public override int GetHashCode()
        {
            var hashCode = 2004231517;
            hashCode = hashCode * -1521134295 + EqualityComparer<IntPtr>.Default.GetHashCode(BaseBufferAddress);
            hashCode = hashCode * -1521134295 + Is32BitBuffer.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<MemoryBufferHeader>.Default.GetHashCode(BufferHeader);
            return hashCode;
        }
    }
}
