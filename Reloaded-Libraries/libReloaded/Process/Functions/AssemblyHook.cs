using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reloaded.Assembler;
using System.Threading.Tasks;
using Reloaded.Memory;
using Reloaded.Memory.Sources;
using Reloaded.Process.Buffers;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Memory;
using SharpDisasm;

namespace Reloaded.Process.Functions
{
    /// <summary>
    /// Provides a hooking class which allows for Cheat Engine style, mid-function assembly hooks either in X86 or X64 mode.
    /// </summary>
    public class AssemblyHook
    {
        /// <summary>
        /// The original bytes that were overwritten in order to place our absolute jump to our hook mini-function.
        /// </summary>
        public byte[] OriginalBytes { get; private set; }

        /// <summary>
        /// Contains the new bytes to be written over the original instructions when the hook is applied.
        /// </summary>
        public byte[] NewBytes { get; private set; }

        /// <summary>
        /// Defines the address the absolute jump to our hook function will be written to.
        /// </summary>
        public IntPtr HookAddress { get; private set; }

        /// <summary>
        /// Exposes the address where your own custom assembly is written into memory; available to you for debugging, or if you are simply interested.
        /// </summary>
        public IntPtr AsmHookAddress { get; private set; }

        /// <summary>
        /// Defines a switch for the assembly hook builder telling it what should be done with the original set of bytes that are going to be
        /// replaced with the jump opcode.
        /// </summary>
        public enum OriginalInstructionOptions
        {
            /// <summary>
            /// The original set of bytes are not included in the assembly hook.
            /// </summary>
            DoNotInclude,

            /// <summary>
            /// The original set of bytes overwritten are executed first in the assembly hook.
            /// </summary>
            ExecuteFirst,

            /// <summary>
            /// The original set of bytes overwritten are executed last in the assembly hook.
            /// </summary>
            ExecuteLast
        }

        /// <summary>
        /// Allows you to insert a set of your own assembly instructions in the middle of a game function and have the game conditionally redirect
        /// to said function.
        /// 
        /// The hook requires a minimum of 6 (X86) or 7 (X64) bytes and will therefore overwrite as many instructions as it needs until it can get enough space
        /// to place the hook. If you have ever used Cheat ENgine's assembly injection, you're already familliar on how to work with this class.
        /// </summary>
        /// <param name="hookAddress">
        ///     The memory address for which the hook is to be generated.
        /// </param>
        /// <param name="mnemonics">
        ///     X86-32 or X64 FASM compatible assembly code that starts with the line use16, use32 or use64 for identifying the architecture.
        /// </param>
        /// <param name="originalInstructionOptions">
        ///     Defines a switch for the assembly hook builder telling it what should be done with the original set of bytes that are going to be
        ///     replaced with the jump opcode.
        /// </param>
        /// <param name="hookLength">[Optional] The explicit amount of bytes to overwrite when inserting the actual hook.</param>
        public AssemblyHook(IntPtr hookAddress, string[] mnemonics, OriginalInstructionOptions originalInstructionOptions, int hookLength = -1)
        {
            // Set hook address.
            HookAddress = hookAddress;

            // Our 32bit and 64bit code paths will differ once we get the disassembler rolling.
            bool is64bit = mnemonics[0] == "use64";

            // Get the default for architecture, then true hook length.
            if (hookLength == -1)
            {
                hookLength = GetDefaultHookLength(is64bit);

                ArchitectureMode disassemblerMode = is64bit ? ArchitectureMode.x86_64 : ArchitectureMode.x86_32;
                hookLength = HookCommon.GetHookLength(HookAddress, hookLength, disassemblerMode);
            }

            // Assemble our custom ASM hook function to be jumped to and executed by the game, then write it to memory.
            IntPtr returnAddress = HookAddress + hookLength;
            byte[] assembledBytes = Assembler.Assembler.Assemble(mnemonics);
            assembledBytes = ProcessCustomInstructions(assembledBytes, OriginalBytes, originalInstructionOptions);
            assembledBytes = AppendJumpBack(assembledBytes, is64bit, returnAddress);
            AsmHookAddress = MemoryBufferManager.Add(assembledBytes);

            // Backup our bytes to hook.
            OriginalBytes = Reloaded.Memory.Sources.Memory.Current.SafeReadRaw(HookAddress, hookLength);

            // Setup our new bytes to overwrite the old ones with (absolute jump).
            NewBytes = new byte[hookLength];
            Populate(NewBytes, (byte) 0x90); // NOP
            byte[] hookJump = AssembleJump(is64bit, AsmHookAddress); // Assemble absolute jump and copy to array of NOPs.
            Array.Copy(hookJump, NewBytes, hookJump.Length);
        }

        /// <summary>
        /// Activates our hook by replacing the original bytes with our now absolute jump to hook function.
        /// </summary>
        public void Activate()
        {
            byte[] newBytesLocal = NewBytes;
            Reloaded.Memory.Sources.Memory.Current.SafeWriteRaw(HookAddress, newBytesLocal);
        }

        /// <summary>
        /// Deactivates our hook by replacing our own absolute jump bytes with the original bytes.
        /// </summary>
        public void Deactivate()
        {
            byte[] originalBytesLocal = OriginalBytes;
            Reloaded.Memory.Sources.Memory.Current.SafeWriteRaw(HookAddress, originalBytesLocal);
        }

        /// <summary>
        /// Prepends, appends or leaves a set of original instruction bytes before or after our own custom assembled bytes.
        /// </summary>
        /// <param name="assembledBytes">Our own custom assembled bytes to which to append a jump back address.</param>
        /// <param name="originalInstructionBytes">The original instructions to which append or prepend.</param>
        /// <param name="originalInstructionOptions">Lets us know what to do with the said original instruction bytes.</param>
        /// <returns></returns>
        private byte[] ProcessCustomInstructions(byte[] assembledBytes, byte[] originalInstructionBytes, OriginalInstructionOptions originalInstructionOptions)
        {
            // Create a list of bytes and depending on the options, populate it.
            List<byte> newBytes = new List<byte>(assembledBytes.Length + originalInstructionBytes.Length);

            // Append bytes in the required order to the list.
            switch (originalInstructionOptions)
            {
                case OriginalInstructionOptions.DoNotInclude:
                    newBytes.AddRange(assembledBytes);
                    break;

                case OriginalInstructionOptions.ExecuteFirst:
                    newBytes.AddRange(originalInstructionBytes);
                    newBytes.AddRange(assembledBytes);
                    break;

                case OriginalInstructionOptions.ExecuteLast:
                    newBytes.AddRange(assembledBytes);
                    newBytes.AddRange(originalInstructionBytes);
                    break;
            }

            return newBytes.ToArray();
        }

        /// <summary>
        /// Assembles a dummy jump to an absolute address in order to get the length of an absolute jump under
        /// X86/X64.
        /// </summary>
        /// <param name="is64Bit">Set to true to calculate a 64bit absolute jump length.</param>
        /// <returns></returns>
        private int GetDefaultHookLength(bool is64Bit)
        {
            return is64Bit ? 
                HookCommon.X64AssembleAbsoluteJump((IntPtr)0x11223344, new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft)).Length : 
                HookCommon.X86AssembleAbsoluteJump((IntPtr)0x11223344).Length;
        }

        /// <summary>
        /// Assembles an absolute jump either from the end of the hook back, or to the hook.
        /// </summary>
        /// <param name="is64Bit">Set to true to assemble a 64bit jump, else 32bit.</param>
        /// <param name="jumpAddress">The address to jump to.</param>
        /// <returns>An bytes for an absolute jump back to the end of the hook.</returns>
        private byte[] AssembleJump(bool is64Bit, IntPtr jumpAddress)
        {
            return is64Bit ?
                HookCommon.X64AssembleAbsoluteJump(jumpAddress, new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft)):
                HookCommon.X86AssembleAbsoluteJump(jumpAddress);
        }

        /// <summary>
        /// Appends a absolute jump X86/X64 instruction to the end of the source array.
        /// </summary>
        /// <param name="sourceArray">The array to append the absolute jump to.</param>
        /// <param name="is64Bit">Decides if to do a X64 jump or a X86 jump.</param>
        /// <param name="targetAddress">The target address to jump to.</param>
        /// <returns>The source array with the appended jump instruction.</returns>
        private byte[] AppendJumpBack(byte[] sourceArray, bool is64Bit, IntPtr targetAddress)
        {
            List<byte> byteList = sourceArray.ToList();
            byteList.AddRange(AssembleJump(is64Bit, targetAddress));
            return byteList.ToArray();
        }

        /// <summary>
        /// Populates an array of <T> with a preset default value. 
        /// </summary>
        /// <param name="array">The array to populate.</param>
        /// <param name="value">The value to populate the array with.</param>
        private static void Populate<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}
