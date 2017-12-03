using SonicHeroes.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Hooking
{
    /// <summary>
    /// This hook class executes your own ASM Code from a supplied list of bytes.
    /// To use this hook, you require at least a hook length of 6 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
    /// </summary>
    class ASM_Hook : Hook_Base
    {
        /// <summary>
        /// This hook class executes your own ASM Code from a supplied list of bytes.
        /// To use this hook, you require at least a hook length of 6 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
        /// </summary>
        /// <param name="hookAddress">The address at which we will start our hook process.</param>
        /// <param name="asmBytes">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="hookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        /// <param name="modLoaderServerSocket">Current Socket that is Connected to the Mod Loader Server</param>
        /// <param name="cleanHook">Set true to not execute original bytes after the </param>
        public ASM_Hook(IntPtr hookAddress, byte[] asmBytes, int hookLength, WebSocket_Client modLoaderServerSocket, bool cleanHook)
        {
            // Setup Common Hook Properties
            SetupHookCommon(hookAddress, hookLength);

            // Check for compatible Mod Loader Hook Method Signature, If Signature Found, Do not Clean Hook!
            if (cleanHook) { cleanHook = CheckCleanHook(); }

            // Run the hook builder.
            Hook_ASM_Internal(modLoaderServerSocket, asmBytes, cleanHook);
        }

        /// <summary>
        /// The inner workings of the Injection Hook Type.
        /// </summary>
        private void Hook_ASM_Internal(WebSocket_Client modLoaderServerSocket, byte[] asmBytes, bool cleanHook)
        {
            // Allocate memory to write old bytes in Sonic Heroes.
            SetNewInstructionAddress(PUSH_RETURN_INSTRUCTION_LENGTH + asmBytes.Length + hookLength + PUSH_RETURN_INSTRUCTION_LENGTH);

            ///
            /// The Bytes of code we will overwrite the original to toggle the injection.
            ///

            // Assemble a return address to our own injection code.
            newBytes = AssembleReturn((int)newInstructionAddress, modLoaderServerSocket);

            // Fill with NOPs until the hook length.
            newBytes = FillNOPs(newInstructionBytes);

            ///
            /// The Bytes of our Injected Code
            ///

            // The bytes to be written to the newly allocated memory.
            List<byte> injectionBytes = new List<byte>();

            // Append Jump call to Own Code
            injectionBytes.AddRange(asmBytes);

            // Insert the original bytes to be executed.
            if (cleanHook) { injectionBytes.AddRange(ProduceNOPArray(originalBytes.Length)); }
            else { injectionBytes.AddRange(originalBytes); }

            // Insert bytes to return back.
            injectionBytes.AddRange(AssembleReturn((int)hookAddress + PUSH_RETURN_INSTRUCTION_LENGTH, modLoaderServerSocket));

            // List to Array!
            newInstructionBytes = injectionBytes.ToArray();

            /// Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(newInstructionBytes, 0, newInstructionAddress, newInstructionBytes.Length);
        }

        /// <summary>
        /// Checks whether the hook is clean
        /// </summary>
        private bool CheckCleanHook()
        {
            // If the address we are hooking is a PUSH opcode with a return.
            // Mod Loader Hook Method Signature
            if ((originalBytes[0] == 0x68) && (originalBytes[0] == 0xC3)) { return false; }
            else { return false; }
        }
    }
}
