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
using Reloaded.Networking;

namespace Reloaded.Hooking
{
    /// <summary>
    /// This hook class executes your own ASM Code from a supplied list of bytes.
    /// To use this hook, you require at least a hook length of 6 bytes + any stray bytes from any instructions. For more information, do refer to the Wiki on Github.
    /// </summary>
    public class ASM_Hook : HookBase
    {
        /// <summary>
        /// This hook class executes your own ASM Code from a supplied list of bytes.
        /// To use this hook, you require at least a hook length of 6 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
        /// </summary>
        /// <param name="hookAddress">The address at which we will start our hook process.</param>
        /// <param name="asmBytes">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="hookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        /// <param name="modLoaderServerSocket">Current Socket that is Connected to the Mod Loader Server</param>
        /// <param name="cleanHook">Set true to not execute original bytes after your own ASM</param>
        public ASM_Hook(IntPtr hookAddress, byte[] asmBytes, int hookLength, Client modLoaderServerSocket, bool cleanHook)
        {
            // Setup Common Hook Properties
            SetupHookCommon(hookAddress, hookLength);

            // Check for compatible Mod Loader Hook Method Signature, If Signature Found, Do not Clean Hook!
            if (cleanHook) cleanHook = CheckCleanHook();

            // Run the hook builder.
            Hook_ASM_Internal(modLoaderServerSocket, asmBytes, cleanHook);
        }

        /// <summary>
        /// The inner workings of the Injection Hook Type.
        /// </summary>
        private void Hook_ASM_Internal(Client modLoaderServerSocket, byte[] asmBytes, bool cleanHook)
        {
            // Allocate memory to write old bytes in Sonic Heroes.
            SetNewInstructionAddress(PUSH_RETURN_INSTRUCTION_LENGTH + asmBytes.Length + hookLength + PUSH_RETURN_INSTRUCTION_LENGTH);

            //
            // The Bytes of code we will overwrite the original to toggle the injection.
            //

            // Assemble a return address to our own injection code.
            newBytes = AssembleReturn((int)newInstructionAddress, modLoaderServerSocket);

            // Fill with NOPs until the hook length.
            newBytes = FillNOPs(newBytes);

            //
            // The Bytes of our Injected Code
            //

            // The bytes to be written to the newly allocated memory.
            List<byte> injectionBytes = new List<byte>();

            // Append Jump call to Own Code
            injectionBytes.AddRange(asmBytes);

            // Insert the original bytes to be executed.
            if (cleanHook)
                injectionBytes.AddRange(ProduceNOPArray(originalBytes.Length));
            else
                injectionBytes.AddRange(originalBytes);

            // Insert bytes to return back.
            injectionBytes.AddRange(AssembleReturn((int)hookAddress + PUSH_RETURN_INSTRUCTION_LENGTH, modLoaderServerSocket));

            // List to Array!
            newInstructionBytes = injectionBytes.ToArray();

            // Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(newInstructionBytes, 0, newInstructionAddress, newInstructionBytes.Length);
        }

        /// <summary>
        /// Checks whether the hook is clean
        /// </summary>
        private bool CheckCleanHook()
        {
            // If the address we are hooking is a PUSH opcode with a return. (a Mod Loader Hook Signature)
            // Do not Clean Hook!
            if (originalBytes[0] == 0x68 && originalBytes[5] == 0xC3)
                return false;
            return true;
        }
    }
}
