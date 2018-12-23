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
using System.Reflection;
using System.Runtime.InteropServices;

namespace Reloaded.Process.Functions.X64Functions
{
    public static class X64FunctionWrapper
    {
        /// <summary>
        /// Creates the wrapper function that wraps a native X64 Microsoft calling convention function or
        /// other custom function into our own function delegate allowing us to call the individual game function.
        /// 
        /// The return value is a delegate which you can use to call the game function as if it were your own.
        /// </summary>
        /// <param name="functionAddress">The address of the game function to create the wrapper for.</param>
        /// <typeparam name="TFunction">Delegate type marked with complete <see cref="X64ReloadedFunctionAttribute"/> that defines the individual function properties.</typeparam>
        /// <returns>Delegate which you can use to call the game function as if it were your own.</returns>
        public static TFunction CreateWrapperFunction<TFunction>(long functionAddress)
        {
            // Find our ReloadedFunction attribute and create the wrapper.
            foreach (Attribute attribute in typeof(TFunction).GetCustomAttributes())
            {
                if (attribute is X64ReloadedFunctionAttribute reloadedFunction)
                {
                    // Stores the pointer to the function.
                    IntPtr wrapperFunctionPointer;

                    // [Performance] CDECL functions are natively supported without our system, we do not need to wrap those functions.
                    if (reloadedFunction.Equals(new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft)))
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
        /// into a regular X64 Microsoft Calling Convention function that is natively supported by the C# programming language.
        /// 
        /// This allows us to call non-standard "usercall" game functions, such as for example functions that take values
        /// in registers as parameters instead of using the stack, functions which take paremeters in a mixture of both stack
        /// and registers as well as functions which varying return parameters, either caller or callee cleaned up.
        /// </summary>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <returns>Pointer to the new X64 Microsoft Calling Convention function address to call from C# code to invoke our game function.</returns>
        private static IntPtr CreateWrapperFunctionInternal<TFunction>(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters = FunctionCommon.GetNumberofParametersWithoutFloats(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> {"use64"};

            // Backup Stack Frame
            assemblyCode.Add("push rbp");       // Backup old call frame
            assemblyCode.Add("mov rbp, rsp");   // Setup new call frame

            // Setup Function Parameters
            if (numberOfParameters > 0)
                assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction.SourceRegisters));

            // Make Shadow Space if necessary.
            if (reloadedFunction.ShadowSpace)
                assemblyCode.Add("sub rsp, 32");   // Setup new call frame

            // Assemble the call to the game function in question.
            assemblyCode.AddRange(HookCommon.X64AssembleAbsoluteCallMnemonics(functionAddress, reloadedFunction));

            // Move return register back if necessary.
            if (reloadedFunction.ReturnRegister != X64ReloadedFunctionAttribute.Register.rax)
                assemblyCode.Add("mov rax, " + reloadedFunction.ReturnRegister);

            // Restore the stack pointer from the Shadow Space
            // 8 = IntPtr.Size
            // 32 = Shadow Space
            if (reloadedFunction.ShadowSpace)
                assemblyCode.Add("add rsp, " + ((nonRegisterParameters * 8) + 32));
            else
                assemblyCode.Add("add rsp, " + (nonRegisterParameters * 8));

            // Restore Stack Frame and Return
            assemblyCode.Add("pop rbp");
            assemblyCode.Add("ret");

            // Write function to buffer and return pointer.
            byte[] assembledMnemonics = Assembler.Assembler.Current.Assemble(assemblyCode.ToArray());
            return HookCommon.BufferHelper.GetBuffers(assembledMnemonics.Length, true)[0].Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our custom function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="registers">The registers in left to right order to be passed onto the method.</param>
        /// <returns>A string array of compatible x86 mnemonics to be assembled.</returns>
        private static string[] AssembleFunctionParameters(int parameterCount, X64ReloadedFunctionAttribute.Register[] registers)
        {
            // Store our JIT Assembly Code
            List<string> assemblyCode = new List<string>();

            // At the current moment in time, our register contents and parameters are as follows: RCX, RDX, R8, R9.
            // The base address of old call stack (EBP) is at [ebp + 0]
            // The return address of the calling function is at [ebp + 8], last parameter is therefore at [ebp + 16].
            // Reminder: The stack grows by DECREMENTING THE STACK POINTER.

            // Push our parameters.
            assemblyCode.Add($"push {X64ReloadedFunctionAttribute.Register.r9}");
            assemblyCode.Add($"push {X64ReloadedFunctionAttribute.Register.r8}");
            assemblyCode.Add($"push {X64ReloadedFunctionAttribute.Register.rdx}");
            assemblyCode.Add($"push {X64ReloadedFunctionAttribute.Register.rcx}");

            // Pop them into appropriate registers.
            // Our "4" here are our invdividual register pushes.
            // If a register at [x] does not exist, 
            for (int x = 0; x < 4; x++)
            {
                // Checks if there is an assigned register for the parameter.
                if (x < registers.Length)
                {
                    assemblyCode.Add($"pop {registers[x]}");
                }
                else
                {
                    switch (x)
                    {
                        case 0: assemblyCode.Add($"pop {X64ReloadedFunctionAttribute.Register.rcx}"); break;
                        case 1: assemblyCode.Add($"pop {X64ReloadedFunctionAttribute.Register.rdx}"); break;
                        case 2: assemblyCode.Add($"pop {X64ReloadedFunctionAttribute.Register.r8}"); break;
                        case 3: assemblyCode.Add($"pop {X64ReloadedFunctionAttribute.Register.r9}");break;
                    }
                }
            }

            // The initial offset from EBP (Stack Base Pointer) for the rightmost parameter (right to left passing):
            // + 1 accounts for the return address being at rsp + 8, so we start from rsp + 16.
            // WE DON'T NEED 32 BYTES TO ACCOUNT FOR THE "SHADOW SPACE", AS THOSE 32 BYTES ARE NATURALLY FOR THE FIRST
            // 4 PARAMETERS.
            int currentBaseStackOffset = ((parameterCount + 1) * 8);
            int nonRegisterParameters = parameterCount - registers.Length;
            
            // Now re-push our non-register parameters passed onto the method onto the stack.
            for (int x = 0; x < nonRegisterParameters; x++)
            {
                // Push parameter onto stack.
                assemblyCode.Add($"push qword [rbp + {currentBaseStackOffset}]");

                // Go to next parameter.
                currentBaseStackOffset -= 8;
            }

            return assemblyCode.ToArray();
        }
    }
}
