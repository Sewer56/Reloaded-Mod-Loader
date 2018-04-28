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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Reloaded.Assembler;

namespace Reloaded.Process.X86Functions.CustomFunctionFactory
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
        /// <param name="functionAddress">The address of the game function to create the wrapper for.</param>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute that defines the individual function properties.</typeparam>
        /// <returns>Delegate to assign back to ReloadedFunction marked game function</returns>
        public static TFunction CreateWrapperFunction<TFunction>(long functionAddress)
        {
            // Find our ReloadedFunction attribute and create the wrapper.
            foreach (Attribute attribute in typeof(TFunction).GetCustomAttributes())
            {
                if (attribute is ReloadedFunctionAttribute reloadedFunction)
                {
                    // Stores the pointer to the function.
                    IntPtr wrapperFunctionPointer;

                    // [Performance] CDECL functions are natively supported without our system, we do not need to wrap those functions.
                    if (reloadedFunction.Equals(new ReloadedFunctionAttribute(CallingConventions.Cdecl)))
                        wrapperFunctionPointer = (IntPtr)functionAddress;
                    else
                        wrapperFunctionPointer = CreateWrapperFunctionInternal<TFunction>((IntPtr)functionAddress, reloadedFunction);

                    // Return delegate type for our function.
                    return Marshal.GetDelegateForFunctionPointer<TFunction>(wrapperFunctionPointer);
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
        /// <returns>Pointer to the new CDECL function address to call from C# code to invoke our game function.</returns>
        private static IntPtr CreateWrapperFunctionInternal<TFunction>(IntPtr functionAddress, ReloadedFunctionAttribute reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters = FunctionCommon.GetNumberofParameters(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> {"use32"};

            // Backup Stack Frame
            assemblyCode.Add("push ebp");       // Backup old call frame
            assemblyCode.Add("mov ebp, esp");   // Setup new call frame

            // Setup Function Parameters
            if (numberOfParameters > 0)
                assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction.SourceRegisters));

            // Call Game Function Pointer (gameFunctionPointer is address at which our function address is written)
            IntPtr gameFunctionPointer = MemoryBuffer.Add(functionAddress); 
            assemblyCode.Add("call dword [0x" + gameFunctionPointer.ToString("X") + "]"); 

            // Stack cleanup if necessary 
            // Move back the stack pointer to before our pushed parameters
            if (nonRegisterParameters > 0 && reloadedFunction.Cleanup == ReloadedFunctionAttribute.StackCleanup.Caller)
            {
                int stackCleanupBytes = 4 * nonRegisterParameters;
                assemblyCode.Add($"add esp, {stackCleanupBytes}");
            }

            if (reloadedFunction.ReturnRegister != ReloadedFunctionAttribute.Register.eax)
            {
                // MOV Game's custom calling convention return register into our return register, EAX.
                assemblyCode.Add("mov eax, " + reloadedFunction.ReturnRegister);
            }

            // Restore Stack Frame and Return
            assemblyCode.Add("pop ebp");
            assemblyCode.Add("ret");

            // Write function to buffer and return pointer.
            byte[] assembledMnemonics = Assembler.Assembler.Assemble(assemblyCode.ToArray());
            return MemoryBuffer.Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our custom function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="registers">The registers in left to right order to be passed onto the method.</param>
        /// <returns>A string array of compatible x86 mnemonics to be assembled.</returns>
        private static string[] AssembleFunctionParameters(int parameterCount, ReloadedFunctionAttribute.Register[] registers)
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
            ReloadedFunctionAttribute.Register[] newRegisters = registers.Reverse().ToArray();
            foreach (ReloadedFunctionAttribute.Register registerParameter in newRegisters)
            {
                // MOV into target register.
                assemblyCode.Add($"mov {registerParameter}, [ebp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 4;
            }

            return assemblyCode.ToArray();
        }
    }
}
