using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Reloaded.Assembler;
using Reloaded.GameProcess.Functions.CustomFunctionFactory;

namespace Reloaded.GameProcess.Functions.CustomFunctionFactory
{
    public static class FunctionWrapper
    {
        /// <summary>
        /// Creates the wrapper function that wraps a native function with our own 
        /// defined or custom calling convention, as specified in ReloadedFunction Attribute 
        /// into a regular CDECL function that is natively supported by the C# programming language.
        /// 
        /// The return value is a delegate to be assigned to an [UnmanagedFunctionPointer(CallingConvention.Cdecl)] 
        /// Attribute marked delegate.
        /// </summary>
        /// <param name="functionAddress">The address of the game function to create the wrapper for</param>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute that defines the individual function properties.</typeparam>
        /// <returns>Delegate to assign back to ReloadedFunction marked game function</returns>
        public static TFunction CreateWrapperFunction<TFunction>(long functionAddress)
        {
            // Find our ReloadedFunction attribute and create the wrapper.
            foreach (Attribute attribute in typeof(TFunction).GetCustomAttributes())
            {
                ReloadedFunction reloadedFunction = attribute as ReloadedFunction;

                if (reloadedFunction != null)
                {
                    // Return delegate type for our function.
                    return Marshal.GetDelegateForFunctionPointer<TFunction>(CreateWrapperFunctionInternal<TFunction>((IntPtr)functionAddress, reloadedFunction));
                }
            }

            // Return null
            return default(TFunction);
        }

        /// <summary>
        /// Creates the wrapper function that wraps a native function with our own defined or custom calling convention,
        /// into a regular CDECL function that is natively supported by the C# programming language.
        /// 
        /// The return value is a pointer to a C# compatible CDECL fcuntion that calls our game function.
        ///
        /// This allows us to call non-standard "usercall" game functions, such as for example functions that take values
        /// in registers as parameters instead of using the stack, functions which take paremeters in a mixture of both stack
        /// and registers as well as functions which varying return parameters, either caller or callee cleaned up.
        /// </summary>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="delegateType">The delegate type which defines the parameters of the your function to call. i.e. typeof(yourGameFunction)</param>
        /// <returns>Pointer to the new CDECL function address to call from C# code to invoke our game function.</returns>
        private static IntPtr CreateWrapperFunctionInternal<TFunction>(IntPtr functionAddress, ReloadedFunction reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters = GetNumberofParameters(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> {"use32"};

            // Backup Stack Frame
            assemblyCode.Add("push ebp");       // Backup old call frame
            assemblyCode.Add("mov ebp, esp");   // Setup new call frame

            // Setup Function Parameters
            assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction.SourceRegisters));

            // Call Game Function Pointer (gameFunctionPointer is address at which our function address is written)
            IntPtr gameFunctionPointer = MemoryBuffer.Add(functionAddress); 
            assemblyCode.Add("call dword [0x" + gameFunctionPointer.ToString("X") + "]"); 

            // Stack cleanup if necessary 
            // Move back the stack pointer to before our pushed parameters
            if (reloadedFunction.Cleanup == ReloadedFunction.StackCleanup.Caller)
            {
                int stackCleanupBytes = 4 * nonRegisterParameters;
                assemblyCode.Add($"add esp, {stackCleanupBytes}");
            }

            // MOV Game's custom calling convention return register into our return register, EAX.
            assemblyCode.Add("mov eax, " + reloadedFunction.ReturnRegister);

            // Restore Stack Frame and Return
            assemblyCode.Add("pop ebp");
            assemblyCode.Add("ret");

            // Write function to buffer and return pointer.
            byte[] assembledMnemonics = Assembler.Assembler.Assemble(assemblyCode.ToArray());
            return MemoryBuffer.Add(assembledMnemonics);
        }

        /// <summary>
        /// Retrieves the number of parameters for a specific delegate Type.
        /// </summary>
        /// <param name="parameters">The delegate type to get number of parameters from.</param>
        /// <returns>Number of parameters for the supplied delegate type.</returns>
        public static int GetNumberofParameters(Type methodType)
        {
            MethodInfo method = methodType.GetMethod("Invoke");
            return method.GetParameters().Length;
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our custom function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="registers">The registers in left to right order to be passed onto the method.</param>
        /// <returns></returns>
        public static string[] AssembleFunctionParameters(int parameterCount, ReloadedFunction.Register[] registers)
        {
            // Store our JIT Assembly Code
            List<string> assemblyCode = new List<string>();

            // At the current moment in time, the base address of old call stack (EBP) is at [ebp + 0]
            // the return address of the calling function is at [ebp + 4], last parameter is therefore at [ebp + 8].
            // Reminder: The stack grows by DECREMENTING THE STACK POINTER.

            // The initial offset from EBP (Stack Base Pointer) for the rightmost parameter (right to left passing):
            int currentBaseStackOffset = ((parameterCount + 1) * 4);
            int nonRegisterParameters = parameterCount - registers.Length;
            
            // Re-push our non-register parameters passed onto the method onto the stack.
            for (int x = 0; x < nonRegisterParameters; x++)
            {
                // Push parameter onto stack.
                assemblyCode.Add($"push dword [ebp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 4;
            }

            // Now move the remaining parameters into the target registers.
            // We reverse the left to right register order to right to left however.
            registers.Reverse();
            foreach (ReloadedFunction.Register registerParameter in registers)
            {
                // MOV into target register.
                assemblyCode.Add($"mov {registerParameter.ToString()}, [ebp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 4;
            }

            return assemblyCode.ToArray();
        }
    }
}
