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
using Reloaded.Assembler;
using Reloaded.Process.Memory;
using SharpDisasm;
using static Reloaded.Assembler.Assembler;

namespace Reloaded.Process.X86Hooking
{
    /// <summary>
    /// Contains all of the common code shared by the various individual hook classes.
    /// </summary>
    public static class HookCommon
    {
        /// <summary>
        /// Retrieves the length of the hook for trampoline, mid-function hooks etc.
        /// This works by reading a short fixed array of bytes from memory then disassembling the bytes
        /// and iterating over each individual instruction up to the point where the total length of the
        /// disassembled exceeds the user set length of instructions to be assembled.
        /// </summary>
        /// <param name="hookAddress">The address that is to be hooked.</param>
        /// <param name="hookLength">The minimum length of the hook, the length of our assembled bytes for the hook.</param>
        /// <returns>The necessary length of bytes to hook the individual game function.</returns>
        public static int GetHookLength(IntPtr hookAddress, int hookLength)
        {
            // Retrieve the function header, arbitrary length of 32 bytes is used for this operation.
            // While you can technically build infinite length X86 instructions, anything greater than 16 to compare seems reasonable.
            byte[] functionHeader = Bindings.TargetProcess.ReadMemoryExternal(hookAddress, 64);

            // Define the disassembler.
            Disassembler disassembler = new Disassembler(functionHeader, ArchitectureMode.x86_32);

            // Disassemble function header and find shortest amount of bytes.
            Instruction[] x86Instructions = disassembler.Disassemble().ToArray();

            int completeHookLength = 0;
            foreach (Instruction x86Instruction in x86Instructions)
            {
                completeHookLength += x86Instruction.Length;
                if (completeHookLength > hookLength) { break; }
            }

            return completeHookLength;
        }

        /// <summary>
        /// Assembles an absolute jump to a user specified address and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute jump to.</param>
        /// <returns>A set of X86 assembler bytes to absolute jump to a specified address.</returns>
        public static byte[] AssembleJump(IntPtr functionAddress)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Jump to Game Function Pointer (gameFunctionPointer is address at which our function address is written)
            IntPtr gameFunctionPointer = MemoryBuffer.Add(functionAddress);
            assemblyCode.Add("jmp dword [0x" + gameFunctionPointer.ToString("X") + "]");

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }

        /// <summary>
        /// Assembles an absolute call to a user specified address and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute call to.</param>
        /// <returns>A set of X86 assembler bytes to absolute call to a specified address.</returns>
        public static byte[] AssembleCall(IntPtr functionAddress)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Jump to Game Function Pointer (gameFunctionPointer is address at which our function address is written)
            IntPtr gameFunctionPointer = MemoryBuffer.Add(functionAddress);
            assemblyCode.Add("call dword [0x" + gameFunctionPointer.ToString("X") + "]");

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }
    }
}
