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
using System.Runtime.InteropServices;
using Reloaded.GameProcess;
using Reloaded.Networking;
using static Reloaded.GameProcess.Native;
using static Reloaded.Networking.MessageTypes.ModLoaderServerMessages;

namespace Reloaded.Hooking
{
    /// <summary>
    /// Provides a base storing the common features and aspects of each of the [Reloaded] Mod Loader Hooks.
    /// </summary>
    public abstract class HookBase
    {
        /// <summary>
        /// Protection
        ///     Specifies the memory protection constants for the region of pages 
        ///     to be allocated, referenced or used for a similar purpose.
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/aa366786(v=vs.85).aspx
        /// </summary>
        [Flags]
        public enum Protection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        /// <summary>
        /// Present for better code readibility, this is the length of the push and return sets of instructions themselves.
        /// </summary>
        protected const int PUSH_RETURN_INSTRUCTION_LENGTH = 6;

        /// <summary>
        /// Present for better code readibility, this is the length of a PUSH dword ASM instruction.
        /// </summary>
        protected const int PUSH_INSTRUCTION_LENGTH = 5;

        /// <summary>
        /// Present for better code readibility, this is the length of the jump (IntPtr) instruction in x86.
        /// </summary>
        protected const int JUMP_INSTRUCTION_LENGTH = 5;

        /// <summary>
        /// The amount of registers we will back up before calling own code, we will restore them right after the call!
        /// </summary>
        protected const int REGISTERS_TO_BACKUP_LENGTH = 8;

        /// <summary>
        /// Stores the representations of the POP instructions used to retrieve the registers from the stack post own method execution for compatible hooks.
        /// </summary>
        protected byte[] ASM_POP_REGISTERS_BYTES = new byte[REGISTERS_TO_BACKUP_LENGTH]
        {
            // These are in reverse order due to last in first out stack order, like a set of stacked books.
            0x5F, // POP EDI
            0x5E, // POP ESI
            0x5D, // POP EBP
            0x5C, // POP ESP
            0x5B, // POP EBX
            0x5A, // POP EDX
            0x59, // POP ECX
            0x58 // POP EAX
        };

        /// <summary>
        /// Stores the representations of the PUSH instructions used to place the registers on the stack before our own method execution for compatible hooks.
        /// </summary>
        protected byte[] ASM_PUSH_REGISTERS_BYTES = new byte[REGISTERS_TO_BACKUP_LENGTH]
        {
            0x50, // PUSH EAX
            0x51, // PUSH ECX
            0x52, // PUSH EDX
            0x53, // PUSH EBX
            0x54, // PUSH ESP
            0x55, // PUSH EBP
            0x56, // PUSH ESI
            0x57 // PUSH EDI
        };

        /// <summary>
        /// Hold a copy of the delegate to the method we want to execute. Otherwise the .NET Garbage Collector will nuke it and spectacularly crash Sonic Heroes since it probably thinks the game is garbage.
        /// </summary>
        protected Delegate customMethodDelegate;

        /// <summary>
        /// Might aswell also store the function pointer to this delegate, .NET Garbage Collector is a scary monster! Choo Choo!
        /// </summary>
        protected IntPtr funcionPointerToOwnMethodCall;

        /// <summary>
        /// This is the address which we will be hooking, the address where a call jmp is placed to redirect our program flow to our own function.
        /// </summary>
        public IntPtr hookAddress;

        /// <summary>
        /// The user defined number of bytes to replace while performing a hook, there must be no stray bytes left from another instruction or set of instructions.
        /// </summary>
        public int hookLength;

        /// <summary>
        /// The new bytes which we will place to make a call jump to our own code.
        /// </summary>
        protected byte[] newBytes;

        /// <summary>
        /// This will point to where the backing up of the registers will occur, the method call for the dll and the restoration of the registers, running of the original code and jumping back will occur (or similar).
        /// </summary>
        protected IntPtr newInstructionAddress;

        /// <summary>
        /// These are the bytes which will be stored at the new instruction address which correspond to assembly instructions. Here in this memory region, ASM to backup the registers will be written, a call to our own method, will be performed, registers will be restored and a jump will be made back.
        /// </summary>
        protected byte[] newInstructionBytes;

        /// <summary>
        /// The original source array of bytes which we will be hooking/placing a call jump to our own code from.
        /// </summary>
        protected byte[] originalBytes;

        /// <summary>
        /// This will store the old original memory protection which we will restore along with the original bytes should we wish to fully dispose of the hook.
        /// </summary>
        protected MemoryProtection originalMemoryProtection;

        /// <summary>
        /// Sets up the common hook properties and fields such as length and address.
        /// </summary>
        protected void SetupHookCommon(IntPtr hookAddress, int hookLength)
        {
            // Set Hook Length, Address
            this.hookLength = hookLength;
            this.hookAddress = hookAddress;

            // The Setup
            // Getting Ready for Hooking!
            originalBytes = new byte[hookLength]; // Initialize storage of original bytes.
            newBytes = new byte[hookLength]; // Initialize storage of new bytes.

            // Backup Original Bytes &
            // Remove protection from the old set of bytes such that we may alter the bytes.
            VirtualProtect(hookAddress, (uint)hookLength, (MemoryProtection)Protection.ExecuteReadWrite, out originalMemoryProtection);
            Marshal.Copy(hookAddress, originalBytes, 0, hookLength);
        }

        /// <summary>
        /// Assembles a return instruction to a specified address.
        /// </summary>
        protected byte[] AssembleReturn(int address, Client modLoaderServerSonic)
        {
            // Assemble code for push from new time string format address.
            string[] x86Mnemonics = {
                "use32",
                "push 0x" + address.ToString("X"),
                "ret"
            };

            // Assemble The Message to be Sent to the server.
            Message.MessageStruct messageStruct = new Message.MessageStruct((ushort)ModLoaderServerMessageType.AssembleX86, SerializeX86Mnemonics(x86Mnemonics));

            // Sent the message across.
            return modLoaderServerSonic.SendDataRaw(messageStruct);
        }

        /// <summary>
        /// Assembles a push instruction to a specified address.
        /// </summary>
        protected byte[] AssemblePush(int address, Client modLoaderServerSonic)
        {
            // Assemble code for push from new time string format address.
            string[] x86Mnemonics = {
                "use32",
                "push 0x" + address.ToString("X")
            };

            // Assemble The Message to be Sent to the server.
            Message.MessageStruct messageStruct = new Message.MessageStruct((ushort)ModLoaderServerMessageType.AssembleX86, SerializeX86Mnemonics(x86Mnemonics));

            // Sent the message across.
            return modLoaderServerSonic.SendDataRaw(messageStruct);
        }

        /// <summary>
        /// Sets the new instruction address for the hooked assembly or custom code.
        /// </summary>
        protected void SetNewInstructionAddress(int length)
        {
            // Retrieve Memory Address where to write to our own.
            newInstructionAddress = Process.GetCurrentProcess().AllocateMemory(length);
        }

        /// <summary>
        /// Fill a byte array with NOPs until the specified hook length.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        protected byte[] FillNOPs(byte[] byteArray)
        {
            // Assign list of bytes for the new byte array.
            List<byte> newByteArray = byteArray.ToList();

            // If necessary, replace any stray bytes with NOP.
            for (int x = PUSH_RETURN_INSTRUCTION_LENGTH - 1; x < hookLength; x++) newByteArray.Add(0x90);

            // Return Byte Array
            return newByteArray.ToArray();
        }

        /// <summary>
        /// Produces a NOP Array of the specified passed in length.
        /// </summary>
        protected byte[] ProduceNOPArray(int length)
        {
            // Allocate Array.
            byte[] nopArray = new byte[length];

            // No Operation Array.
            for (int x = 0; x < nopArray.Length; x++) nopArray[x] = 0x90;

            // Return NOP Array
            return nopArray;
        }

        /// <summary>
        /// Activates the hook such that it may be used.
        /// </summary>
        public void Activate() { Marshal.Copy(newBytes, 0, hookAddress, hookLength); }

        /// <summary>
        /// Deactivates the hook such that it may be used.
        /// </summary>
        public void Deactivate() { Marshal.Copy(originalBytes, 0, hookAddress, hookLength); }
    }
}
