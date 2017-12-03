using SonicHeroes.Networking;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SonicHeroes.Hooking
{
    /// <summary>
    /// This hook class generates a call to your code, keeps registers intact and executes the original code after your code has finished executing. 
    /// To use this hook, you require at least a hook length of 6 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
    /// </summary>
    public class Injection : Hook_Base
    {
        /// <summary>
        /// This hook class generates a call to your code, keeps registers intact and executes the original code after your code has finished executing. 
        /// To use this hook, you require at least a hook length of 5 bytes + any stray bytes from any instruction. For more information, do refer to the Wiki on Github.
        /// </summary>
        /// <param name="hookAddress">The address at which we will start our hook process.</param>
        /// <param name="destinationDelegate">Delegate to the method we will want to run. (DelegateName)Method</param>
        /// <param name="hookLength">The amount of bytes the hook lasts, all stray bytes will be replaced with NOP/No Operation.</param>
        public Injection(IntPtr hookAddress, Delegate destinationDelegate, int hookLength, WebSocket_Client modLoaderServerSocket)
        {
            // Assign class members.
            customMethodDelegate = destinationDelegate;

            // Obtain pointer to our own C# method.
            funcionPointerToOwnMethodCall = Marshal.GetFunctionPointerForDelegate(customMethodDelegate);

            // Setup Common Hook Properties
            SetupHookCommon(hookAddress, hookLength);

            // Run the hook builder.
            Injection_Internal(modLoaderServerSocket);
        }

        /// <summary>
        /// The inner workings of the Injection Hook Type.
        /// </summary>
        private void Injection_Internal(WebSocket_Client modLoaderServerSocket)
        {
            // Allocate memory to write old bytes in Sonic Heroes.
            SetNewInstructionAddress(REGISTERS_TO_BACKUP_LENGTH + PUSH_RETURN_INSTRUCTION_LENGTH + hookLength + REGISTERS_TO_BACKUP_LENGTH + PUSH_RETURN_INSTRUCTION_LENGTH);

            ///
            /// The Bytes of code we will overwrite the original to toggle the injection.
            ///

            // Assemble a return address to our own injection code.
            newBytes = AssembleReturn((int)newInstructionAddress, modLoaderServerSocket);

            // Fill with NOPs until the hook length.
            newBytes = FillNOPs(newBytes);

            ///
            /// The Bytes of our Injected Code
            ///

            // The bytes to be written to the newly allocated memory.
            List<byte> injectionBytes = new List<byte>();

            // Append Register Backup
            injectionBytes.AddRange(ASM_PUSH_REGISTERS_BYTES);

            // Calculate PUSH Which will allow our own code to return beyond the return statement for the call to our own code.
            // PUSH, PUSH, RET, <JUMP BACK HERE>
            /* 
             * e.g. 
             * [1000] PUSH 1007
             * [1005] PUSH <Own Code Address>
             * [1006] RET
             * [1007] pop edi
            */
            injectionBytes.AddRange(AssemblePush((int)newInstructionAddress + REGISTERS_TO_BACKUP_LENGTH + PUSH_INSTRUCTION_LENGTH + PUSH_RETURN_INSTRUCTION_LENGTH, modLoaderServerSocket));

            // Append Push + Return Call to Own Code
            injectionBytes.AddRange(AssembleReturn((int)funcionPointerToOwnMethodCall, modLoaderServerSocket));

            // Append Restoration of Registers.
            injectionBytes.AddRange(ASM_POP_REGISTERS_BYTES);

            // Insert the original bytes to be executed.
            injectionBytes.AddRange(originalBytes);

            // Insert bytes to return back.
            injectionBytes.AddRange(AssembleReturn((int)hookAddress + PUSH_RETURN_INSTRUCTION_LENGTH, modLoaderServerSocket));

            // List to Array!
            newInstructionBytes = injectionBytes.ToArray();

            /// Write our payload which will be redirected to using activate and deactivate hook!
            Marshal.Copy(newInstructionBytes, 0, newInstructionAddress, newInstructionBytes.Length);
        }

    }
}
