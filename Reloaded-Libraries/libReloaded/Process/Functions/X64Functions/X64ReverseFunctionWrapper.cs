using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Reloaded.Process.Functions.X64Functions
{
    /// <summary>
    /// The reverse function wrapper function allows you to obtain a pointer to a C# function.
    /// </summary>
    public class X64ReverseFunctionWrapper<TFunction>
    {
        /// <summary>
        /// A copy of our C# function delegate used that the game/pointer will call. 
        /// </summary>
        public TFunction CSharpFunctionCopy { get; private set; }

        /// <summary>
        /// A pointer to our C# function, wrapped to a custom calling convention.
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /* Factory */
        private X64ReverseFunctionWrapper() { }

        /// <summary>
        /// Creates a wrapper function that transforms our own C# function into an arbitrary calling convention; with pointer to transformed C# function.
        /// Basically, this creates a function pointer to a C# function.
        /// Note: You must keep an instance of this class as long as you're using the function pointer.
        /// </summary>
        /// <param name="function">The function to create a function pointer to.</param>
        public static X64ReverseFunctionWrapper<TFunction> CreateReverseWrapper(TFunction function)
        {
            // Set our C# function.
            var reverseFunctionWrapper = new X64ReverseFunctionWrapper<TFunction>();
            reverseFunctionWrapper.CSharpFunctionCopy = function;

            // Take fast path for CDECL, otherwise assemble wrapper.
            var reloadedFunctionAttribute = FunctionCommon.GetX64ReloadedFunctionAttribute<TFunction>();
            reverseFunctionWrapper.Pointer = CreateX64WrapperFunctionInternal<TFunction>(Marshal.GetFunctionPointerForDelegate(function), reloadedFunctionAttribute);

            return reverseFunctionWrapper;
        }

        /// <summary>
        /// Creates the wrapper function for redirecting program flow to our C# function.
        /// </summary>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <returns></returns>
        internal static IntPtr CreateX64WrapperFunctionInternal<TFunction>(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters = FunctionCommon.GetNumberofParametersWithoutFloats(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use64" };

            // If the attribute is Microsoft Call Convention, take fast path!
            if (reloadedFunction.Equals(new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft)))
            {
                // Backup old call frame
                assemblyCode.AddRange(HookCommon.X64AssembleAbsoluteJumpMnemonics(functionAddress));
            }
            else
            {
                // Backup Stack Frame
                assemblyCode.Add("push rbp");       // Backup old call frame
                assemblyCode.Add("mov rbp, rsp");   // Setup new call frame

                // We assume that we are stack aligned in usercall/custom calling conventions, if we are not, game over for now.
                // Our stack frame alignment is off by 8 here, we must also consider non-register parameters.
                // Note to self: In the case you forget, dummy. Your stack needs to be 16 byte aligned.

                // Calculate the bytes our wrapper parameters take on the stack in total.
                // The stack frame backup, push rbp and CALL negate themselves so it's down to this.
                // Then our misalignment to the stack.
                int stackBytesTotal = (nonRegisterParameters * 8);
                int stackMisalignment = (stackBytesTotal % 16);

                // Prealign stack
                assemblyCode.Add($"sub rbp, {stackMisalignment}");   // Setup new call frame

                // Setup the registers for our C# method.
                assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction, stackMisalignment));

                // And now we add the shadow space for C# method's Microsoft Call Convention.
                assemblyCode.Add("sub rsp, 32");   // Setup new call frame

                // Assemble the Call to C# Function Pointer.
                assemblyCode.AddRange(HookCommon.X64AssembleAbsoluteCallMnemonics(functionAddress, reloadedFunction));

                // MOV our own return register, EAX into the register expected by the calling convention
                assemblyCode.Add($"mov {reloadedFunction.ReturnRegister}, rax");

                // Restore stack pointer (this is always the same, our function is Microsoft Call Convention Compliant)
                assemblyCode.Add("add rsp, " + ((nonRegisterParameters * 8) + 32));

                // Restore stack alignment
                assemblyCode.Add($"add rbp, {stackMisalignment}");

                // Restore Stack Frame and Return
                assemblyCode.Add("pop rbp");
                assemblyCode.Add("ret");
            }

            // Assemble and return pointer to code
            byte[] assembledMnemonics = Assembler.Assembler.Current.Assemble(assemblyCode.ToArray());
            return HookCommon.BufferHelper.GetBuffers(assembledMnemonics.Length, true)[0].Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our own C# CDECL compliant function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="stackMisalignment">The amount of extra bytes that the stack pointer is decremented by/grown in order to 16-byte align the stack.</param>
        /// <returns>A string array of compatible x64 mnemonics to be assembled.</returns>
        internal static string[] AssembleFunctionParameters(int parameterCount, X64ReloadedFunctionAttribute reloadedFunction, int stackMisalignment)
        {
            // Store our JIT Assembly Code
            List<string> assemblyCode = new List<string>();

            // At the current moment in time, the base address of old call stack (EBP) is at [ebp + 0]
            // the return address of the calling function is at [ebp + 8], last parameter is therefore at [ebp + 16].
            // Reminder: The stack grows by DECREMENTING THE STACK POINTER.

            // There exists 32 bits of Shadow Space depending on the function, we must move it to a convention supporting shadow space (Microsoft).

            // The initial offset from EBP (Stack Base Pointer) for the rightmost parameter (right to left passing):
            int nonRegisterParameters = parameterCount - reloadedFunction.SourceRegisters.Length;
            int currentBaseStackOffset = 0;

            // Set the base stack offset depending on whether the method has "Shadow Space" or not.
            if (reloadedFunction.ShadowSpace)
                // Including parameter count will compensate for the "Shadow Space"
                currentBaseStackOffset = ((parameterCount + 1) * 8) + stackMisalignment;
            else
                // If there is no "Shadow Space", it's directly on the stack below.
                currentBaseStackOffset = ((nonRegisterParameters + 1) * 8) + stackMisalignment;

            /*
                 Re-push register parameters to be used for calling the method.
            */

            // Re-push our non-register parameters passed onto the method onto the stack.
            for (int x = 0; x < nonRegisterParameters; x++)
            {
                // Push parameter onto stack.
                assemblyCode.Add($"push qword [rbp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 8;
            }

            // Push our register parameters onto the stack.
            // We reverse the order of the register parameters such that they are ultimately pushed in right to left order, matching
            // our individual parameter order as if they were pushed onto the stack in left to right order.
            X64ReloadedFunctionAttribute.Register[] newRegisters = reloadedFunction.SourceRegisters.Reverse().ToArray();
            foreach (X64ReloadedFunctionAttribute.Register registerParameter in newRegisters)
            {
                assemblyCode.Add($"push {registerParameter.ToString()}");
            }

            // Now we pop our individual register parameters from the stack back into registers expected by the Microsoft
            // X64 Calling Convention.

            // This looks stupid, I know.
            X64ReloadedFunctionAttribute microsoftX64CallingConvention = new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft);

            // Loop for all registers in Microsoft Convention
            for (int x = 0; x < microsoftX64CallingConvention.SourceRegisters.Length; x++)
            {
                // Reduce this until the end of our parameter list, in the case we have e.g. only 3 parameters
                // and the convention can take 4.
                if (x < parameterCount)
                    assemblyCode.Add($"pop {microsoftX64CallingConvention.SourceRegisters[x].ToString()}");
                else
                    break;
            }

            return assemblyCode.ToArray();
        }
    }
}
