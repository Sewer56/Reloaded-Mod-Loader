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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Reloaded.Assembler.Definitions;
using Reloaded.Memory.Buffers;
using Reloaded.Memory.Buffers.Utilities;
using Reloaded.Memory.Sources;

namespace Reloaded.Assembler
{
    /// <summary>
    /// Assembler class allows you to assemble X86 and X64 mnemonics using FASM.
    /// </summary>
    public unsafe class Assembler : IDisposable
    {
        /// <summary>
        /// Contains a static instance of the <see cref="Assembler"/> class.
        /// </summary>
        public static Assembler Current = new Assembler();

        // Defines the functions used for assembly and getting function.
        private GetVersionFunction  _getVersionFunction;
        private AssembleFunction    _assembleFunction;

        private delegate int GetVersionFunction();
        private delegate FasmResult AssembleFunction(char* szSource, byte* lpMemory, int nSize, int nPassesLimit, void* hDisplayPipe);

        // For disposal of class.
        private int    _allocationSize;
        private IntPtr _allocationAddress;

        /// <summary>
        /// Creates a new instance of the FASM assembler.
        /// </summary>
        /// <param name="bufferSize">The minimum size of the buffer to be used for both passing in the string mnemonics and returning the assembled X86/X64 bytes.
        /// </param>
        public Assembler(int bufferSize = 0x10000)
        {
            // Find fitting buffer.
            var thisProcess = Process.GetCurrentProcess();
            var bufferHelper = new MemoryBufferHelper(thisProcess);
            var bufferLocation = bufferHelper.FindBufferLocation(bufferSize, (IntPtr)UInt32.MinValue, (IntPtr)UInt32.MaxValue);

            // Allocate given buffer.
            var virtualAllocFunction = VirtualAllocUtility.GetVirtualAllocFunction(thisProcess);
            IntPtr address = virtualAllocFunction(thisProcess.Handle, bufferLocation.MemoryAddress, (ulong)bufferLocation.Size);

            if (address == IntPtr.Zero)
                throw new Exception("Failed to allocate memory for Assembler.");

            // Set functions
            if (IntPtr.Size == 4)
            {
                _assembleFunction = Fasm32.Assemble;
                _getVersionFunction = Fasm32.GetVersion;
            }
            else if (IntPtr.Size == 8)
            {
                _assembleFunction = Fasm64.Assemble;
                _getVersionFunction = Fasm64.GetVersion;
            }
            else
            {
                throw new SystemException("Only 32bit and 64bit desktop architectures are supported (X86 and X86_64).");
            }
            // Assign for cleanup.
            _allocationSize = bufferLocation.Size;
            _allocationAddress = address;
        }

        /// <summary>
        /// Retrieves the version of the internally used FASM assembler DLL.
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            // Call the native function to get the version
            int nativeVersion = _getVersionFunction();

            // Create and return a managed version object
            return new Version(nativeVersion & 0xff, (nativeVersion >> 16) & 0xff);
        }

        /// <summary>
        /// Assembles the given mnemonics.
        /// </summary>
        /// <param name="mnemonics">The mnemonics to assemble.</param>
        /// <param name="passLimit">The maximum number of passes to perform when assembling data.</param>
        public byte[] Assemble(string[] mnemonics, ushort passLimit = 100)
        {
            string mnemonicsString = String.Join(Environment.NewLine, mnemonics);
            return Assemble(mnemonicsString);
        }

        /// <summary>
        /// Assembles the given mnemonics.
        /// </summary>
        /// <param name="mnemonics">The mnemonics to assemble; delimited by new line \n for each new instruction.</param>
        /// <param name="passLimit">The maximum number of passes to perform when assembling data.</param>
        public byte[] Assemble(string mnemonics, ushort passLimit = 100)
        {
            // Convert Text & Append
            byte[] mnemonicBytes = Encoding.ASCII.GetBytes(mnemonics + "\0");
            Memory.Sources.Memory.Current.WriteRaw(_allocationAddress, mnemonicBytes);

            // Details of the remainder of the buffer.
            IntPtr remainingBufferPtr   = _allocationAddress + mnemonicBytes.Length;
            int remainingBufferSize     = _allocationSize - mnemonicBytes.Length;

            // Assemble and check result.
            FasmResult result = _assembleFunction((char*)_allocationAddress, (byte*)remainingBufferPtr, remainingBufferSize, passLimit, (void*)IntPtr.Zero);
            FasmState state = Memory.Sources.Memory.Current.Read<FasmState>(remainingBufferPtr);

            if (result == FasmResult.Ok)
            {
                byte[] assembledBytes = new byte[state.OutputLength];
                Marshal.Copy((IntPtr)state.OutputData, assembledBytes, 0, state.OutputLength);
                return assembledBytes;
            }
            else
            {
                // TODO: Make this exception more detailed when X64 FASM version/hack uses 64bit addressing.
                throw new FasmSimpleException(state.ErrorCode);
            }
        }

        /// <summary>
        /// Releases the allocated memory for the assembler and
        /// </summary>
        public void Dispose()
        {
            var thisProcess = Process.GetCurrentProcess();
            var virtualFreeFunction = VirtualFreeUtility.GetVirtualFreeFunction(thisProcess);
            virtualFreeFunction(thisProcess.Handle, _allocationAddress, (ulong)_allocationSize);
        }
    }
}
