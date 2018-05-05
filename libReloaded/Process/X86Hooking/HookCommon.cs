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
using System.Linq;
using Reloaded.Assembler;
using Reloaded.Process.Memory;
using SharpDisasm;
using SharpDisasm.Udis86;
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
        /// Disassembles the provided array of stolen bytes and replaces instances of relative jumps to
        /// absolute jumps towards a specific location.
        /// </summary>
        /// <param name="stolenBytes">Bytes which are going to be overwritten by our own jump bytes with jmp calls to replace.</param>
        /// <param name="baseAddress">The original address of the start of the individual bytes.</param>
        /// <returns></returns>
        public static (List<byte>, List<(IntPtr addressToPatch, byte[] newBytes)>) ProcessStolenBytes(List<byte> stolenBytes, IntPtr baseAddress)
        {
            // List of addressses to patch.
            List <(IntPtr addressToPatch, byte[] newBytes)> addressesToPatch = new List<(IntPtr addressToPatch, byte[] newBytes)>();

            // Address of end of Reloaded's hook
            long reloadedHookEndAddress = stolenBytes.Count + (long)baseAddress;
            
            // Define the disassembler.
            Disassembler disassembler = new Disassembler(stolenBytes.ToArray(), ArchitectureMode.x86_32, (ulong)baseAddress, true);

            // Disassemble function header and find shortest amount of bytes.
            Instruction[] x86Instructions = disassembler.Disassemble().ToArray();

            // Declare new bytes
            List<byte> newBytes = new List<byte>();

            // Iterate over all instructions.
            foreach (Instruction x86Instruction in x86Instructions)
            {
                // Check if the opcode is a JMP.
                if (x86Instruction.Mnemonic == ud_mnemonic_code.UD_Ijmp && x86Instruction.Length <= 5)
                {
                    // Relative offset from the program counter (i.e. instruction offset + jmp length)
                    int relativeOffset = x86Instruction.Operands[0].LvalSDWord;

                    // Calculate final destination.
                    long finalJumpDestination = (long)x86Instruction.PC + relativeOffset;

                    // Backup other hooks' stray bytes
                    byte[] otherStrayBytes = Bindings.TargetProcess.ReadMemoryExternal((IntPtr)x86Instruction.PC, (int)(reloadedHookEndAddress - (long)x86Instruction.PC));

                    // Assemble mini-wrapper to preserve stray bytes and jump back to our final destination.
                    IntPtr strayByteWrapper = AssembleMiniWrapper(otherStrayBytes, reloadedHookEndAddress);

                    // Patch any relative calls at the final jump.
                    addressesToPatch.AddRange(GetPatchAddresses(finalJumpDestination, (long)x86Instruction.PC, (long)strayByteWrapper));

                    // Assemble absolute JMP
                    newBytes.AddRange(AssembleAbsoluteJump((IntPtr)finalJumpDestination));
                }
                // If not jump, add next instruction.
                else { newBytes.AddRange(x86Instruction.Bytes); }
            }

            return (newBytes, addressesToPatch);
        }

        /// <summary>
        /// Finds the module to which a specified address belongs and disassembles the entire module.
        /// Passes through each regular relative jump and corrects the target of the jump if the jump points
        /// to the specied (originalAddress) target.
        /// If no module is matched, the search region will be constrained to the current page in memory.
        /// </summary>
        /// <param name="searchStart">The initial address of another module's jump instruction.</param>
        /// <param name="originalAddress">Relative jumps which point to this original address will be patched.</param>
        /// <param name="newAddress">The new address to replace the relative jumps pointing to originalAddress to.</param>
        public static List<(IntPtr addressToPatch, byte[] newBytes)> GetPatchAddresses(long searchStart, long originalAddress, long newAddress)
        {
            // Declare search range
            long searchRange = 0;

            // Get the current modules in the process.
            foreach (ProcessModule module in Bindings.TargetProcess.GetProcessFromReloadedProcess().Modules)
            {
                // Retrieve address range.
                long minimumAddress = (long)module.BaseAddress;
                long maximumAddress = (long)module.BaseAddress + module.ModuleMemorySize;

                // Check if in range
                if (searchStart >= minimumAddress && searchStart <= maximumAddress)
                {
                    searchStart = minimumAddress;
                    searchRange = module.ModuleMemorySize;
                }
            }

            // If the search range is 0 (our address is not in a module, scan the entire page for jumps.
            if (searchRange == 0)
            {
                // Get page size.
                searchRange = Environment.SystemPageSize;

                // Go down to nearest multiple of the page size.
                searchStart -= searchStart % searchRange;
            }

            // List of addresses to patch.
            List<(IntPtr addressToPatch, byte[] newBytes)> addressesToPatch = new List<(IntPtr, byte[])>();

            // Read memory of search range.
            byte[] memoryBuffer = Bindings.TargetProcess.ReadMemoryExternal((IntPtr)searchStart, (int)searchRange);

            // Disassemble search range.
            Disassembler disassembler = new Disassembler(memoryBuffer, ArchitectureMode.x86_32, (ulong)searchStart, true);

            // Disassemble function header and find shortest amount of bytes.
            Instruction[] x86Instructions = disassembler.Disassemble().ToArray();

            // Iterate over all instructions.
            foreach (Instruction x86Instruction in x86Instructions)
            {
                // Check if the opcode is a JMP.
                if (x86Instruction.Mnemonic == ud_mnemonic_code.UD_Ijmp && x86Instruction.Length <= 5)
                {
                    // Relative offset from the program counter (i.e. instruction offset + jmp length)
                    int relativeOffset = x86Instruction.Operands[0].LvalSDWord;

                    // Calculate final destination.
                    long finalJumpDestination = (long)x86Instruction.PC + relativeOffset;

                    // Check if final destination is our old jump address.
                    if (finalJumpDestination == originalAddress)
                    {
                        // Patch the final destination.
                        IntPtr newRelativeOffset = (IntPtr)(newAddress - (long)x86Instruction.Offset);
                        byte[] relativeJumpBytes = AssembleRelativeJump(newRelativeOffset);

                        // Add onto list of things to patch.
                        addressesToPatch.Add(((IntPtr)x86Instruction.Offset, relativeJumpBytes));
                    }
                }
            }

            // Return all the addresses to patch!.
            return addressesToPatch;
        }

        /// <summary>
        /// Assembles a wrapper for other function hooks' stray bytes that will be overwritten by
        /// Reloaded's own hooking mechanism. Gives back the old functions their stray bytes and jumps back
        /// immediately to the end of Reloaded's hook.
        /// </summary>
        /// <param name="strayBytes">Bytes from another hook that will be replaced by Reloaded's hooking mechanism.</param>
        /// <param name="reloadedHookEndAddress">The target address for the mini wrapper to jump back to.</param>
        /// <returns>The address of a new "mini-wrapper" class for </returns>
        public static IntPtr AssembleMiniWrapper(byte[] strayBytes, long reloadedHookEndAddress)
        {
            // Get our stray bytes.
            List<byte> newBytes = strayBytes.ToList();

            // Append our absolute jump to the mix.
            newBytes.AddRange(AssembleAbsoluteJump((IntPtr) reloadedHookEndAddress));

            // Write to memory buffer and return
            return MemoryBuffer.Add(newBytes.ToArray());
        }

        /// <summary>
        /// Assembles a relative jump to by a user specified offset and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="relativeJumpOffset">The offset by which the relative jump is to be performed.</param>
        /// <returns>A set of X86 assembler bytes to perform a relative jump by a specified amount of bytes.</returns>
        public static byte[] AssembleRelativeJump(IntPtr relativeJumpOffset)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Jump by a relative amount of bytes.
            assemblyCode.Add("jmp dword " + relativeJumpOffset);

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }

        /// <summary>
        /// Assembles an absolute jump to a user specified address and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute jump to.</param>
        /// <returns>A set of X86 assembler bytes to absolute jump to a specified address.</returns>
        public static byte[] AssembleAbsoluteJump(IntPtr functionAddress)
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
        public static byte[] AssembleAbsoluteCall(IntPtr functionAddress)
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
