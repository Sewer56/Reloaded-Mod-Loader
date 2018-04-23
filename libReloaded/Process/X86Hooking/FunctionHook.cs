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
using Reloaded.Process.Memory;
using Reloaded.Process.X86Functions;
using Reloaded.Process.X86Functions.CustomFunctionFactory;
using CallingConventions = Reloaded.Process.X86Functions.CallingConventions;

namespace Reloaded.Process.X86Hooking
{
    /// <summary>
    /// The FunctionHook class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall
    /// functions as well as custom ReloadedFunction Attribute declared functions allowing for the redirection
    /// of executing program code to our own code.
    /// 
    /// To use this class, simply pass the delegate type alongside to the create function just like with calling game functions.
    /// For calling of the original function (game/program function), this class provides the <see cref="OriginalFunction"/> member.
    /// The Generic parameter is your delegate type for the function.
    /// If you are unfamilliar with the hooking concept, please refer to the Reloaded Wiki.
    /// </summary>
    public class FunctionHook <TFunction>
    {
        /// <summary>
        /// Exposes the original function to the user, allowing for it to be effectively called.
        /// </summary>
        public TFunction OriginalFunction;

        /// <summary>
        /// Stores a copy of the original delegate passed into the Function Hook generator
        /// such that the delegate which provided our function pointer to our C# code is not swept up
        /// by the Garbage Collector.
        /// </summary>
        private TFunction _originalDelegate;

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="FunctionHook{TDelegate}"/> class.
        /// </summary>
        /// <param name="gameFunctionAddress">The address of the game function to create the wrapper for.</param>
        /// <param name="functionDelegate">
        ///     A delegate instance of the supplied generic delegate type which calls/invokes
        ///     the C# method that will be used to handle the hook.
        /// </param>
        /// <param name="hookLength">
        ///     Optional explicit length of the hook to perform used in the impossibly rare
        ///     cases whereby auto-length checking overflows the default into a jmp/call.
        /// </param>
        /// <returns>
        ///     An instance of <see cref="FunctionHook{TFunction}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public FunctionHook(long gameFunctionAddress, TFunction functionDelegate, int hookLength)
        {
            FunctionHook <TFunction> localFunctionHook = CreateFunctionHook(gameFunctionAddress, functionDelegate, hookLength);
            this.OriginalFunction = localFunctionHook.OriginalFunction;
            this._originalDelegate = localFunctionHook._originalDelegate;
        }

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="FunctionHook{TDelegate}"/> class.
        /// </summary>
        /// <param name="gameFunctionAddress">The address of the game function to create the wrapper for.</param>
        /// <param name="functionDelegate">
        ///     A delegate instance of the supplied generic delegate type which calls/invokes
        ///     the C# method that will be used to handle the hook.
        /// </param>
        /// <returns>
        ///     An instance of <see cref="FunctionHook{TFunction}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public FunctionHook(long gameFunctionAddress, TFunction functionDelegate)
        {
            FunctionHook<TFunction> localFunctionHook = CreateFunctionHook(gameFunctionAddress, functionDelegate);
            this.OriginalFunction = localFunctionHook.OriginalFunction;
            this._originalDelegate = localFunctionHook._originalDelegate;
        }

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="FunctionHook{TDelegate}"/> class.
        /// </summary>
        /// <param name="gameFunctionAddress">The address of the game function to create the wrapper for.</param>
        /// <param name="functionDelegate">
        ///     A delegate instance of the supplied generic delegate type which calls/invokes
        ///     the C# method that will be used to handle the hook.
        /// </param>
        /// <returns>
        ///     An instance of <see cref="FunctionHook{TDelegate}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public static FunctionHook<TFunction> CreateFunctionHook(long gameFunctionAddress, TFunction functionDelegate)
        {
            return CreateFunctionHook(gameFunctionAddress, functionDelegate, -1);
        }

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="FunctionHook{TDelegate}"/> class.
        /// </summary>
        /// <param name="gameFunctionAddress">The address of the game function to create the wrapper for.</param>
        /// <param name="functionDelegate">
        ///     A delegate instance of the supplied generic delegate type which calls/invokes
        ///     the C# method that will be used to handle the hook.
        /// </param>
        /// <param name="hookLength">
        ///     Optional explicit length of the hook to perform used in the impossibly rare
        ///     cases whereby auto-length checking overflows the default into a jmp/call.
        /// </param>
        /// <returns>
        ///     An instance of <see cref="FunctionHook{TFunction}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public static FunctionHook<TFunction> CreateFunctionHook(long gameFunctionAddress, TFunction functionDelegate, int hookLength)
        {
            /*
                Retrieve C# function details.
            */

            // Retrieve the function address from the supplied user delegate.
            // Our ReloadedFunction attribute.
            IntPtr cSharpFunctionAddress = Marshal.GetFunctionPointerForDelegate(functionDelegate);
            ReloadedFunction reloadedFunction = GetReloadedFunctionAttribute<TFunction>();

            /*
                [Hook Part I] Create Custom => CDECL Wrapper and Assemble 
            */

            // Assemble the wrapper function.
            // Assemble a jump to our wrapper function.
            IntPtr wrapperFunctionAddress = CreateWrapperFunction<TFunction>(cSharpFunctionAddress, reloadedFunction);
            List<byte> jumpBytes = HookCommon.AssembleJump(wrapperFunctionAddress).ToList();

            /*
                [Hook Part II] Calculate Hook Length (Unless Explicit)
            */

            // Retrieve hook length explicitly 
            if (hookLength == -1) { hookLength = HookCommon.GetHookLength((IntPtr)gameFunctionAddress, jumpBytes.Count); }

            // Assemble JMP + NOPs for stolen/stray bytes.
            if (hookLength > jumpBytes.Count)
            {
                // Append NOPs after JMP to fill remaining bytes.
                int nopBytes = hookLength - jumpBytes.Count;

                for (int x = 0; x < nopBytes; x++)
                { jumpBytes.Add(0x90); }
            }

            /*
                [Call Original Function Part I] Read stolen bytes and assemble function wrapper to call original function.
            */

            // Backup game's hook bytes.
            // Calculate jump back address for original function.
            // Append absolute JMP instruction to return to original function for calling the original function in hook.
            List<byte> stolenBytes = Bindings.TargetProcess.ReadMemoryExternal((IntPtr)gameFunctionAddress, hookLength).ToList();
            IntPtr jumpBackAddress = (IntPtr)(gameFunctionAddress + hookLength);
            stolenBytes.AddRange(HookCommon.AssembleJump(jumpBackAddress));

            /*
                [Call Original Function part II] Instantiate and return functionHook with the original game function address.
            */

            // Assign original function.
            FunctionHook<TFunction> functionHook = new FunctionHook<TFunction>();

            // Write original bytes and jump to memory, and return address.
            IntPtr gameFunctionWrapperAddress = MemoryBuffer.Add(stolenBytes.ToArray());

            // Create wrapper for calling the original function.
            functionHook.OriginalFunction = FunctionWrapper.CreateWrapperFunction<TFunction>((long)gameFunctionWrapperAddress);

            // Store a copy of the original function.
            functionHook._originalDelegate = functionDelegate;

            /*
                [Apply Hook] Write hook bytes. 
            */
            Bindings.TargetProcess.WriteMemoryExternal((IntPtr)gameFunctionAddress, jumpBytes.ToArray());

            return functionHook;
        }

        /*
            -------- Function Hooking Plan --------

            Game call for our function:    
                BACKUP STACK FRAME                                      ; cdecl setup for own function

                RE-PUSH PARAMETERS TO STACK (RIGHT TO LEFT)             ; our function is cdecl, parameters on stack
                PUSH REGISTERS TO STACK (RIGHT TO LEFT)                 ; our function is cdecl, parameters on stack

                CALL OWN FUNCTION (CDECL)                               ; our own function can call original back
                MOV RETURN REGISTER (EAX => TARGET)                     ; Arbitrary Function Translation

                RESTORE STACK POINTER (add esp, 4 * parameters)         ; cdecl end for own function
                RESTORE STACK FRAME                                     ; cdecl end for own function

                IF CALLER CLEANUP (RET)                                 ; reloadedfunction is caller cleanup
                IF CALLEE CLEANUP (RET 0xPARAMETERS)                    ; reloadedfunction is callee cleanup

            Hook length finding:
                ITERATE OVER ASM INSTUCTIONS UNTIL HOOK LENGTH MET

            Assemble original: 
                Assemble indirect jump to end of original function's stolen bytes.
                Append indirect jump bytes to original bytes.
                Write all bytes to memory and create original function Delegate via FunctionWrapper
         */

        /// <summary>
        /// Retrieves a ReloadedFunction from a supplied delegate type.
        /// </summary>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute that defines the individual function properties.</typeparam>
        /// <returns>ReloadedFunction class instance that the delegate has been tagged with.</returns>
        public static ReloadedFunction GetReloadedFunctionAttribute<TFunction>()
        {
            // Retrieve the ReloadedFunction attribute
            foreach (Attribute attribute in typeof(TFunction).GetCustomAttributes())
            {
                if (attribute is ReloadedFunction reloadedFunction)
                    return reloadedFunction;
            }

            // Return cdecl if false.
            Bindings.PrintWarning?.Invoke
            (
                $"Instance of {typeof(TFunction).Name} in a developer declared hook is missing its ReloadedFunction attribute.\n" +
                 "The specified calling convention will be assumed as CDECL by default.\n" +
                 "To developers: Please don't do this! Refer to the wiki or CallingConventions.cs common convention settings."
            );

            return new ReloadedFunction(CallingConventions.Cdecl);
        }

        /// <summary>
        /// Creates the wrapper function for redirecting program flow to our C# function.
        /// </summary>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <returns></returns>
        private static IntPtr CreateWrapperFunction<TFunction>(IntPtr functionAddress, ReloadedFunction reloadedFunction)
        {
            // Retrieve number of parameters.
            int numberOfParameters = FunctionCommon.GetNumberofParameters(typeof(TFunction));
            int nonRegisterParameters = numberOfParameters - reloadedFunction.SourceRegisters.Length;

            // List of ASM Instructions to be Compiled
            List<string> assemblyCode = new List<string> { "use32" };

            // Backup Stack Frame
            assemblyCode.Add("push ebp");       // Backup old call frame
            assemblyCode.Add("mov ebp, esp");   // Setup new call frame

            // Push registers for our C# method as necessary.
            assemblyCode.AddRange(AssembleFunctionParameters(numberOfParameters, reloadedFunction.SourceRegisters));

            // Call C# Function Pointer (cSharpFunctionPointer is address at which our C# function address is written)
            IntPtr cSharpFunctionPointer = MemoryBuffer.Add(functionAddress);
            assemblyCode.Add("call dword [0x" + cSharpFunctionPointer.ToString("X") + "]");

            // Restore stack pointer + stack frame
            assemblyCode.Add($"add esp, {numberOfParameters * 4}");

            // MOV our own return register, EAX into the register expected by the calling convention
            assemblyCode.Add($"mov {reloadedFunction.ReturnRegister}, eax");

            // Restore Stack Frame and Return
            assemblyCode.Add("pop ebp");

            // Caller/Callee Cleanup
            if (reloadedFunction.Cleanup == ReloadedFunction.StackCleanup.Callee)
                assemblyCode.Add($"ret {nonRegisterParameters * 4}");
            else
                assemblyCode.Add("ret");

            // Assemble and return pointer to code
            byte[] assembledMnemonics = Assembler.Assembler.Assemble(assemblyCode.ToArray());
            return MemoryBuffer.Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our own C# CDECL compliant function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="registers">The registers in left to right order used in the calling convention we are hooking.</param>
        /// <returns>A string array of compatible x86 mnemonics to be assembled.</returns>
        private static string[] AssembleFunctionParameters(int parameterCount, ReloadedFunction.Register[] registers)
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

            // Now push the remaining parameters from the custom calling convention's registers onto the stack.
            // We process the registers in right to left order, .
            ReloadedFunction.Register[] newRegisters = registers.Reverse().ToArray();
            foreach (ReloadedFunction.Register registerParameter in newRegisters)
            {
                // Push the register variable onto the stack for our C# CDECL function./
                assemblyCode.Add($"push {registerParameter.ToString()}");

                // Go to next parameter.
                currentBaseStackOffset -= 4;
            }

            return assemblyCode.ToArray();
        }

        /// <summary>
        /// Private constructor, constructors do not support delegates therefore use Factory Design Pattern.
        /// </summary>
        private FunctionHook() { }
    }
}
