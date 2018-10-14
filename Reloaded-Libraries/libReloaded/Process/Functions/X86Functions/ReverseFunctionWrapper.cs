using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Process.Buffers;

namespace Reloaded.Process.Functions.X86Functions
{
    /// <summary>
    /// The reverse function wrapper function allows you to obtain a pointer to a C# function.
    /// </summary>
    public class ReverseFunctionWrapper<TFunction>
    {
        /// <summary>
        /// A copy of our C# function delegate used that the game/pointer will call. 
        /// </summary>
        public TFunction CSharpFunctionCopy { get; private set; }

        /// <summary>
        /// A pointer to our C# function, wrapped to a custom calling convention.
        /// </summary>
        public IntPtr Pointer               { get; private set; }

        /* Factory */
        private ReverseFunctionWrapper() { }

        /// <summary>
        /// Creates a wrapper function that transforms our own C# function into an arbitrary calling convention; with pointer to transformed C# function.
        /// Basically, this creates a function pointer to a C# function.
        /// Note: You must keep an instance of this class as long as you're using the function pointer.
        /// </summary>
        /// <param name="function">The function to create a function pointer to.</param>
        public static ReverseFunctionWrapper<TFunction> CreateReverseWrapper(TFunction function)
        {
            // Set our C# function.
            var reverseFunctionWrapper = new ReverseFunctionWrapper<TFunction>();
            reverseFunctionWrapper.CSharpFunctionCopy = function;

            // Take fast path for CDECL, otherwise assemble wrapper.
            var reloadedFunctionAttribute = FunctionCommon.GetReloadedFunctionAttribute<TFunction>();
            if (reloadedFunctionAttribute.Equals(new ReloadedFunctionAttribute(CallingConventions.Cdecl)))
                reverseFunctionWrapper.Pointer = (IntPtr) Marshal.GetFunctionPointerForDelegate(function);
            else
                reverseFunctionWrapper.Pointer = CreateReverseWrapperInternal<TFunction>(Marshal.GetFunctionPointerForDelegate(function), reloadedFunctionAttribute);

            return reverseFunctionWrapper;
        }

        /// <summary>
        /// Creates the wrapper function for redirecting program flow to our C# function.
        /// </summary>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <returns></returns>
        internal static IntPtr CreateReverseWrapperInternal<TFunction>(IntPtr functionAddress, ReloadedFunctionAttribute reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters    = FunctionCommon.GetNumberofParameters(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Backup Stack Frame
            assemblyCode.Add("push ebp");       // Backup old call frame
            assemblyCode.Add("mov ebp, esp");   // Setup new call frame

            // Push registers for our C# method as necessary.
            assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction.SourceRegisters));

            // Call C# Function Pointer (cSharpFunctionPointer is address at which our C# function address is written)
            IntPtr cSharpFunctionPointer = MemoryBufferManager.Add(functionAddress, IntPtr.Zero);
            assemblyCode.Add("call dword [0x" + cSharpFunctionPointer.ToString("X") + "]");

            // Restore stack pointer + stack frame
            assemblyCode.Add($"add esp, {numberOfParameters * 4}");

            // MOV our own return register, EAX into the register expected by the calling convention
            assemblyCode.Add($"mov {reloadedFunction.ReturnRegister}, eax");

            // Restore Stack Frame and Return
            assemblyCode.Add("pop ebp");

            // Caller/Callee Cleanup
            if (reloadedFunction.Cleanup == ReloadedFunctionAttribute.StackCleanup.Callee)
                assemblyCode.Add($"ret {nonRegisterParameters * 4}");
            else
                assemblyCode.Add("ret");

            // Assemble and return pointer to code
            byte[] assembledMnemonics = Assembler.Assembler.Assemble(assemblyCode.ToArray());
            return MemoryBufferManager.Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our own C# CDECL compliant function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="registers">The registers in left to right order used in the calling convention we are hooking.</param>
        /// <returns>A string array of compatible x86 mnemonics to be assembled.</returns>
        private static string[] AssembleFunctionParameters(int parameterCount, ReloadedFunctionAttribute.Register[] registers)
        {
            // Store our JIT Assembly Code
            List<string> assemblyCode = new List<string>();

            // At the current moment in time, the base address of old call stack (EBP) is at [ebp + 0]
            // the return address of the calling function is at [ebp + 4], last parameter is therefore at [ebp + 8].
            // Reminder: The stack grows by DECREMENTING THE STACK POINTER.

            // The initial offset from EBP (Stack Base Pointer) for the rightmost parameter (right to left passing):
            int nonRegisterParameters = parameterCount - registers.Length;
            int currentBaseStackOffset = ((nonRegisterParameters + 1) * 4);

            // Re-push our non-register parameters passed onto the method onto the stack.
            for (int x = 0; x < nonRegisterParameters; x++)
            {
                // Push parameter onto stack.
                assemblyCode.Add($"push dword [ebp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 4;
            }

            // Now push the remaining parameters from the custom calling convention's registers onto the stack.
            // We process the registers in right to left order.
            ReloadedFunctionAttribute.Register[] newRegisters = registers.Reverse().ToArray();
            foreach (ReloadedFunctionAttribute.Register registerParameter in newRegisters)
            {
                // Push the register variable onto the stack for our C# CDECL function./
                assemblyCode.Add($"push {registerParameter.ToString()}");
            }

            return assemblyCode.ToArray();
        }
    }
}
