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
using Reloaded.Process.Buffers;
using Reloaded.Process.Memory;
using SharpDisasm;
using SharpDisasm.Udis86;
using Reloaded.Process.Functions.X64Functions;
using static Reloaded.Assembler.Assembler;

namespace Reloaded.Process.Functions
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
        /// <param name="architectureMode">Defines the architecture as X86 or X64 to use for disassembly.</param>
        /// <returns>The necessary length of bytes to hook the individual game function.</returns>
        public static int GetHookLength(IntPtr hookAddress, int hookLength, ArchitectureMode architectureMode)
        {
            // Retrieve the function header, arbitrary length of 32 bytes is used for this operation.
            // While you can technically build infinite length X86 instructions, anything greater than 16 to compare seems reasonable.
            byte[] functionHeader = Bindings.TargetProcess.ReadMemoryExternal(hookAddress, 64);

            // Define the disassembler.
            Disassembler disassembler = new Disassembler(functionHeader, architectureMode);

            // Disassemble function header and find shortest amount of bytes.
            Instruction[] instructions = disassembler.Disassemble().ToArray();

            int completeHookLength = 0;
            foreach (Instruction instruction in instructions)
            {
                completeHookLength += instruction.Length;
                if (completeHookLength >= hookLength) { break; }
            }

            return completeHookLength;
        }

        /// <summary>
        /// Disassembles the provided array of stolen bytes and replaces instances of relative jumps to
        /// absolute jumps towards a specific location.
        /// </summary>
        /// <param name="stolenBytes">Bytes which are going to be overwritten by our own jump bytes with jmp calls to replace.</param>
        /// <param name="baseAddress">The original address of the start of the individual bytes.</param>
        /// <param name="architectureMode">Defines the architecture as X86 or X64 to use for disassembly.</param>
        /// <param name="reloadedFunctionX64">[Only for X64] Contains the register blacklist (source registers) and a default register for assembling absolute jumps in ASLR mode.</param>
        /// <returns></returns>
        public static (List<byte>, List<(IntPtr addressToPatch, byte[] newBytes)>) ProcessStolenBytes(List<byte> stolenBytes, IntPtr baseAddress, ArchitectureMode architectureMode, X64ReloadedFunctionAttribute reloadedFunctionX64 = null)
        {
            // List of addressses to patch.
            List <(IntPtr addressToPatch, byte[] newBytes)> addressesToPatch = new List<(IntPtr addressToPatch, byte[] newBytes)>();

            // Address of end of Reloaded's hook
            long reloadedHookEndAddress = stolenBytes.Count + (long)baseAddress;

            // Define the disassembler and isassemble function header and find shortest amount of bytes.
            Disassembler disassembler = new Disassembler(stolenBytes.ToArray(), architectureMode, (ulong)baseAddress, true);
            Instruction[] instructions = disassembler.Disassemble().ToArray();

            // New bytes to replace the stolen bytes.
            List<byte> newStolenBytes = new List<byte>();

            // Iterate over all instructions.
            foreach (Instruction instruction in instructions)
            {
                // Check if the opcode is a JMP, this is what we want to patch.
                if (instruction.Mnemonic == ud_mnemonic_code.UD_Ijmp && instruction.Length <= 5)
                {
                    // Relative offset from the program counter (i.e. instruction offset + jmp length)
                    int relativeOffset = instruction.Operands[0].LvalSDWord;

                    // Calculate final destination, this is where the existing hook's jmp leads to.
                    long finalJumpDestination = (long)instruction.PC + relativeOffset;

                    // Backup other hooks' stray bytes
                    byte[] otherStrayBytes = Bindings.TargetProcess.ReadMemoryExternal((IntPtr)instruction.PC, (int)(reloadedHookEndAddress - (long)instruction.PC));

                    // Assemble mini-wrapper which preserves stray bytes and jumps back to our final destination.
                    IntPtr strayByteWrapper;

                    if (architectureMode == ArchitectureMode.x86_32)
                        strayByteWrapper = X86AssembleMiniWrapper(otherStrayBytes, reloadedHookEndAddress);
                    else
                        strayByteWrapper = X64AssembleMiniWrapper(otherStrayBytes, reloadedHookEndAddress, reloadedFunctionX64, finalJumpDestination);

                    // Patch any relative calls at the final jump.
                    addressesToPatch.AddRange(GetPatchAddresses(finalJumpDestination, (long)instruction.PC, (long)strayByteWrapper, architectureMode));

                    // Assemble absolute JMP
                    if (architectureMode == ArchitectureMode.x86_32)
                        newStolenBytes.AddRange(X86AssembleAbsoluteJump((IntPtr)finalJumpDestination));
                    else
                        newStolenBytes.AddRange(X64AssembleAbsoluteJump((IntPtr)finalJumpDestination, reloadedFunctionX64));
                }
                // X64: Convert Relative Address Pointers to Absolute Jumps.
                else if (instruction.Mnemonic == ud_mnemonic_code.UD_Ijmp &&
                         architectureMode == ArchitectureMode.x86_64 &&
                         instruction.Operands.Length >= 1 &&
                         instruction.Operands[0].Base == ud_type.UD_R_RIP)
                {
                    // Resolve the target address of the rip relative addressing absolute jump.
                    IntPtr targetAddress = Bindings.TargetProcess.ReadMemoryExternal<IntPtr>((IntPtr)(instruction.PC + (ulong)instruction.Operands[0].LvalSDWord));

                    // Replace with our own copy of an assembled absolute jump.
                    newStolenBytes.AddRange(X64AssembleAbsoluteJump((IntPtr)targetAddress, reloadedFunctionX64));
                }

                // If not jump, add next instruction.
                else { newStolenBytes.AddRange(instruction.Bytes); }
            }

            return (newStolenBytes, addressesToPatch);
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
        /// <param name="architectureMode">Defines the architecture as X86 or X64 to use for disassembly.</param>
        private static List<(IntPtr addressToPatch, byte[] newBytes)> GetPatchAddresses(long searchStart, long originalAddress, long newAddress, ArchitectureMode architectureMode)
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
            Disassembler disassembler = new Disassembler(memoryBuffer, architectureMode, (ulong)searchStart, true);

            // Disassemble function header and find shortest amount of bytes.
            Instruction[] instructions = disassembler.Disassemble().ToArray();

            // Iterate over all instructions.
            foreach (Instruction x86Instruction in instructions)
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
                        byte[] relativeJumpBytes;

                        if (architectureMode == ArchitectureMode.x86_32)
                            relativeJumpBytes = X86AssembleRelativeJump(newRelativeOffset);
                        else
                            relativeJumpBytes = X64AssembleRelativeJump(newRelativeOffset);

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
        public static IntPtr X86AssembleMiniWrapper(byte[] strayBytes, long reloadedHookEndAddress)
        {
            // Get our stray bytes.
            List<byte> newBytes = strayBytes.ToList();

            // Append our absolute jump to the mix.
            newBytes.AddRange(X86AssembleAbsoluteJump((IntPtr) reloadedHookEndAddress));

            // Write to memory buffer and return
            return MemoryBufferManager.Add(newBytes.ToArray());
        }

        /// <summary>
        /// Assembles a wrapper for other function hooks' stray bytes that will be overwritten by
        /// Reloaded's own hooking mechanism. Gives back the old functions their stray bytes and jumps back
        /// immediately to the end of Reloaded's hook.
        /// </summary>
        /// <param name="strayBytes">Bytes from another hook that will be replaced by Reloaded's hooking mechanism.</param>
        /// <param name="reloadedHookEndAddress">The target address for the mini wrapper to jump back to.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="wrapperAddress">[Optional] Target address within of which the wrapper should be placed in 2GB range.</param>
        /// <returns>The address of a new "mini-wrapper" class for </returns>
        public static IntPtr X64AssembleMiniWrapper(byte[] strayBytes, long reloadedHookEndAddress, X64ReloadedFunctionAttribute reloadedFunction, long wrapperAddress = 0)
        {
            // Get our stray bytes.
            List<byte> newBytes = strayBytes.ToList();
            
            // Append our absolute jump to the mix.
            newBytes.AddRange(X64AssembleAbsoluteJump((IntPtr)reloadedHookEndAddress, reloadedFunction));

            // Write to memory buffer and return
            return MemoryBufferManager.Add(newBytes.ToArray(), (IntPtr)wrapperAddress);
        }

        /// <summary>
        /// Assembles a relative jump to by a user specified offset and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="relativeJumpOffset">The offset by which the relative jump is to be performed.</param>
        /// <returns>A set of X86 assembler bytes to perform a relative jump by a specified amount of bytes.</returns>
        public static byte[] X86AssembleRelativeJump(IntPtr relativeJumpOffset)
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
        public static byte[] X86AssembleAbsoluteJump(IntPtr functionAddress)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Jump to Game Function Pointer (gameFunctionPointer is address at which our function address is written)
            IntPtr gameFunctionPointer = MemoryBufferManager.Add(functionAddress, IntPtr.Zero);
            assemblyCode.Add("jmp dword [0x" + gameFunctionPointer.ToString("X") + "]");

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }

        /// <summary>
        /// Assembles an absolute jump to a user specified address and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute jump to.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="shortJump">Set to true to shorten the length of the JMP at the expense of a free register's content in ASLR Mode.</param>
        /// <param name="targetAbsoluteJumpAddress">[Optional] Target address within which 2GB absolute jump to be assembled.</param>
        /// <returns>A set of X64 assembler bytes to absolute jump to a specified address.</returns>
        public static byte[] X64AssembleAbsoluteJump(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction, bool shortJump = false, long targetAbsoluteJumpAddress = 0)
        {
            // Get mnemonics to assemble.
            List<string> assemblyCode = new List<string> { "use64" };
            assemblyCode.AddRange(X64AssembleAbsoluteJumpMnemonics(functionAddress, reloadedFunction, shortJump, targetAbsoluteJumpAddress));

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }

        /// <summary>
        /// Generates the mnemonics to form an absolute jump to a user
        /// specified process and returns the mnemonics as a list of strings.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute jump to.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="shortJump">Set to true to shorten the length of the JMP at the expense of a free register's content in ASLR Mode.</param>
        /// <param name="targetAbsoluteJumpAddress">[Optional] Target address within which 2GB absolute jump to be assembled.</param>
        /// <returns>A set of X64 assembler mnemonic strings to absolute jump to a specified address.</returns>
        public static List<string> X64AssembleAbsoluteJumpMnemonics(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction, bool shortJump = false, long targetAbsoluteJumpAddress = 0)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string>();

            // Assemble the jump to the game function in question.
            // With the rewrite of MemoryBuffer, this piece of code has been greatly simplified.
            IntPtr gameFunctionPointer;
            if ((gameFunctionPointer = MemoryBufferManager.Add(functionAddress, (IntPtr)targetAbsoluteJumpAddress)) != IntPtr.Zero)
            {
                // Jump to Game Function Pointer (gameFunctionPointer is address at which our function address is written)
                assemblyCode.Add("jmp qword [qword 0x" + gameFunctionPointer.ToString("X") + "]");
            }
            // Cannot get buffer in 2GB range.
            else if (shortJump)
            {
                // Get register to delegate function calling to.
                X64ReloadedFunctionAttribute.Register jmpRegister = FunctionCommon.GetCallRegister(reloadedFunction);

                // Call Game Function Pointer (gameFunctionPointer is address at which our function address is written)
                assemblyCode.Add($"mov {jmpRegister}, 0x{functionAddress.ToString("X")}");
                assemblyCode.Add($"jmp {jmpRegister}");
            }
            else
            {
                // Get register to delegate function calling to.
                X64ReloadedFunctionAttribute.Register jmpRegister = FunctionCommon.GetCallRegister(reloadedFunction);

                // Call Game Function Pointer (gameFunctionPointer is address at which our function address is written)
                assemblyCode.Add($"push {jmpRegister}");
                assemblyCode.Add($"mov {jmpRegister}, 0x{functionAddress.ToString("X")}");
                assemblyCode.Add($"xchg {jmpRegister}, [rsp]");
                assemblyCode.Add($"ret");
            }

            // Assemble the individual bytes.
            return assemblyCode;
        }

        /// <summary>
        /// Assembles an absolute jump to a user specified address and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="functionAddress">The address to assemble the absolute call to.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <returns>A set of X64 assembler bytes to absolute jump to a specified address.</returns>
        public static List<string> X64AssembleAbsoluteCallMnemonics(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction)
        {
            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string>();

            // Assemble the call to the game function in question.
            // With the rewrite of MemoryBuffer, this piece of code has been greatly simplified.
            IntPtr gameFunctionPointer;
            if ((gameFunctionPointer = MemoryBufferManager.Add(functionAddress, IntPtr.Zero)) != IntPtr.Zero)
            {
                // Jump to Game Function Pointer (gameFunctionPointer is address at which our function address is written)
                assemblyCode.Add("call qword [qword 0x" + gameFunctionPointer.ToString("X") + "]");
            }
            // Cannot get buffer in 2GB range.
            else
            {
                // Get register to delegate function calling to.
                X64ReloadedFunctionAttribute.Register jmpRegister = FunctionCommon.GetCallRegister(reloadedFunction);

                // Call Game Function Pointer (gameFunctionPointer is address at which our function address is written)
                assemblyCode.Add($"mov {jmpRegister}, 0x{functionAddress.ToString("X")}");
                assemblyCode.Add($"call {jmpRegister}");
            }

            // Assemble the individual bytes.
            return assemblyCode;
        }

        /// <summary>
        /// Assembles a relative jump to by a user specified offset and returns
        /// the resultant bytes of the assembly process.
        /// </summary>
        /// <param name="relativeJumpOffset">The offset by which the relative jump is to be performed.</param>
        /// <returns>A set of X64 assembler bytes to perform a relative jump by a specified amount of bytes.</returns>
        public static byte[] X64AssembleRelativeJump(IntPtr relativeJumpOffset)
        {
            // List of ASM Instructions to be Compiled
            // TODO: [Optional] If outside of range, use a targeted memorybuffer created in range, and absolute jmp from that buffer.

            /*
                The probability of this to-do being necessary is pretty much 0 in a realistic scenario as Reloaded would preallocate the necessary
                memory before the application would allocate any extra memory on its heap. (Remember: Reloaded mods execute on a suspended application).

                Even popular commercial 3rd party software such as overlays (e.g. Steam, RTSS) do not seem to make a consideration of this.
            */
            List<string> assemblyCode = new List<string> { "use64" };

            // Jump by a relative amount of bytes.
            assemblyCode.Add("jmp qword " + relativeJumpOffset);

            // Assemble the individual bytes.
            return Assemble(assemblyCode.ToArray());
        }
    }
}
