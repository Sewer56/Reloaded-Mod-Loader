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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.GameProcess;

namespace Reloaded.Assembler
{
    /// <summary>
    /// Provides a buffer for general small size memory storage, storage of custom assembly code as well as 
    /// address addresses for assembly instructions taking "m32" parameters, e.g. jmp dword [0x123456]. 
    /// See the MemoryBuffer source code for a more detailed explanation and purpose.
    /// </summary>
    public static class MemoryBuffer
    {
        /*
            Notes:
                If you plan to modify your written data in the future, save the address it was written at, this is a thin implementation.
                If the data you are writing is larger than 4096 bytes, the written memory will be entirely reserved by your function.
                You must set the target process manually if you are using this from an external program.
         */

        /*
            Original Purpose:

            The minimum granularity (size in bytes) of the AllocateMemory function (wrapping VirtualAllocEx) is 4096 bytes,
            equivalent to one memory page, meaning that even if you request 4 bytes, 4096 bytes are automatically allocated.

            In the cases that you would wish to use indirect (m32) parameters jumps, calls or parameters in general for assembly purposes, this
            class exists such that memory is not wasted on each allocation, as under normal circumstances, most of the time the other 4092
            bytes would be left zeroed or unused.

            Now it's used for multiple assembly operations such as storing "trampoline" code (stub to call original code from our own code),
            your custom assembly code as well as 

            tl;dr Use this for writing any kind of ASM stuff into memory, saves heaps of memory.
         */

        /// <summary>
        /// The base address to the current buffer for our stored data.
        /// </summary>
        public static IntPtr baseBufferAddress;

        /// <summary>
        /// Target process in which the buffer is to be stored.
        /// (The same process you are injecting assembly to).
        /// </summary>
        public static ReloadedProcess targetProcess;

        /// <summary>
        /// The current offset in the current page.
        /// </summary>
        public static int currentOffsetAddress;

        /// <summary>
        /// The size in bytes that we will be allocating in memory (multiple of 4096).
        /// </summary>
        public const int ALLOCATION_SIZE = 4096;

        /// <summary>
        /// The individual actions to be performed based off of the size
        /// of the data to be added onto the buffer.
        /// </summary>
        private enum SizeCheckResult
        {
            AppendCurrentPage,
            MakeNewPage,
            MakeDedicatedPages
        }

        /// <summary>
        /// Allocate new memory page for buffer use on first use of class.
        /// </summary>
        static MemoryBuffer()
        {
            targetProcess = ReloadedProcess.GetCurrentProcess();
            AllocateNewPage();
        }

        /// <summary>
        /// Writes your own memory address into process' memory and gives you the address address
        /// for the memory location to use directly functions accepting indirect jumps.
        /// </summary>
        /// <param name="addressIntPtr">The address the return address address will point to.</param>
        /// <returns>Pointer to the passed in address/</returns>
        public static IntPtr Add(IntPtr addressIntPtr)
        {
            // Know what to do
            SizeCheckResult allocationAction = CheckItemSize(IntPtr.Size);

            // IntPtr can't be larger than a page (until we get 4096+ bit CPUs), do not check dedicated page.

            // Allocate new page first if necessary.
            if (allocationAction == SizeCheckResult.MakeNewPage) { AllocateNewPage(); }

            // Append to page.
            return AppendToPage(addressIntPtr);
        }

        /// <summary>
        /// Writes your own memory address into process' memory and gives you the address address
        /// for the memory location to use directly functions accepting indirect jumps.
        /// </summary>
        /// <param name="addressIntPtr">The address the return address address will point to.</param>
        /// <returns>Pointer to the passed in address/</returns>
        public static IntPtr Add(byte[] bytesToWrite)
        {
            // Know what to do
            SizeCheckResult allocationAction = CheckItemSize(bytesToWrite.Length);

            // UIntPtr can't be larger than a page (until we get 4096+ bit CPUs), do not check dedicated page.

            // If the result is that the byte array is too large, allocate the array its own
            if (allocationAction == SizeCheckResult.MakeDedicatedPages)
            {
                IntPtr newBufferAddress = targetProcess.AllocateMemory(bytesToWrite.Length);
                targetProcess.WriteMemoryExternalSafe(newBufferAddress, bytesToWrite);
                return newBufferAddress;
            }
            else
            {
                // Allocate new page first if necessary.
                if (allocationAction == SizeCheckResult.MakeNewPage) { AllocateNewPage(); }

                // Append to page.
                return AppendToPage(bytesToWrite);
            }
        }

        /// <summary>
        /// Returns the append/allocation action to be performed based off of the size of
        /// the item that is to be added to the buffer.
        /// </summary>
        /// <param name="objectSize"></param>
        /// <returns></returns>
        private static SizeCheckResult CheckItemSize(int objectSize)
        {
            // Check if larger than whole buffer.
            if (objectSize > ALLOCATION_SIZE) { return SizeCheckResult.MakeDedicatedPages; }

            // Check if too big.
            if (currentOffsetAddress + objectSize > ALLOCATION_SIZE) { return SizeCheckResult.MakeNewPage; }

            // Else Append
            return SizeCheckResult.AppendCurrentPage;
        }

        /// <summary>
        /// Appends our requested address onto the buffer and returns its pointer address.
        /// </summary>
        /// <param name="address">The address to append.</param>
        private static IntPtr AppendToPage(IntPtr address)
        {
            // Get our address to append memory at.
            IntPtr appendAddress = baseBufferAddress + currentOffsetAddress;

            // Append differently depending on x86/x64
            if (IntPtr.Size == 4)
            {
                targetProcess.WriteMemoryExternalSafe(appendAddress, BitConverter.GetBytes((int)address));
            }
            else if (IntPtr.Size == 8)
            {
                targetProcess.WriteMemoryExternalSafe(appendAddress, BitConverter.GetBytes((long)address));
            }

            // Set current offset 
            currentOffsetAddress += IntPtr.Size;

            return appendAddress;
        }

        /// <summary>
        /// Appends our requested data onto the buffer and returns its pointer address.
        /// </summary>
        /// <param name="bytesToAppend">The bytes for our ASM code, data or other component to append.</param>
        private static IntPtr AppendToPage(byte[] bytesToAppend)
        {
            // Do the append operation.
            IntPtr appendAddress = baseBufferAddress + currentOffsetAddress;
            targetProcess.WriteMemoryExternalSafe(appendAddress, bytesToAppend);

            // Set current offset.
            currentOffsetAddress += bytesToAppend.Length;

            return appendAddress;
        }

        /// <summary>
        /// Resets the current page offset and allocates a new page for the current buffer.
        /// </summary>
        private static void AllocateNewPage()
        {
            baseBufferAddress = targetProcess.AllocateMemory(ALLOCATION_SIZE);
            currentOffsetAddress = 0;
        }
    }
}
