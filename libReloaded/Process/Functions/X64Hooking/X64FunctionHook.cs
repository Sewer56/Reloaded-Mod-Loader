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
using Reloaded.Process.Buffers;
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Memory;
using SharpDisasm;

namespace Reloaded.Process.Functions.X64Hooking
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
    public class X64FunctionHook<TFunction>
    {
        /// <summary>
        /// Exposes the original function to the user, allowing for it to be effectively called.
        /// </summary>
        public TFunction OriginalFunction;

        /// <summary>
        /// Intended for debugging purposes. Exposes the address that the C# code calls for running the <see cref="OriginalFunction"/>.
        /// </summary>
        public IntPtr OriginalFunctionAddress { get; private set; }

        /// <summary>
        /// Stores a copy of the original delegate passed into the Function Hook generator
        /// such that the delegate which provided our function pointer to our C# code is not swept up
        /// by the Garbage Collector.
        /// </summary>
        private TFunction _originalDelegate;

        /// <summary>
        /// Stores the address of where to write our jump for the hook and the bytes for said jump,
        /// this is to activate our own hooks.
        /// </summary>
        private (IntPtr, byte[]) _hookToWrite;

        /// <summary>
        /// Contains a list of addresses belonging to other programs, hooks to be patched and the bytes to patch them with.
        /// This is to patch other programs' hooks with our own while maintaining them.
        /// </summary>
        private List<(IntPtr, byte[])> _addressesToPatch;

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="X64FunctionHook{TDelegate}"/> class.
        /// </summary>
        /// <param name="gameFunctionAddress">The address of the game function to create the wrapper for.</param>
        /// <param name="functionDelegate">
        ///     A delegate instance of the supplied generic delegate type which calls/invokes
        ///     the C# method that will be used to handle the hook.
        /// </param>
        /// <returns>
        ///     An instance of <see cref="X64FunctionHook{TDelegate}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public static X64FunctionHook<TFunction> Create(long gameFunctionAddress, TFunction functionDelegate)
        {
            return Create(gameFunctionAddress, functionDelegate, -1);
        }

        /// <summary>
        /// Creates a function hook for a function at a user specified address.
        /// This class provides Windows API (and general process) hooking functionality for standard cdecl, stdcall, as well as custom
        /// ReloadedFunction Attribute declared functions. For more details, see the description of the <see cref="X64FunctionHook{TDelegate}"/> class.
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
        ///     An instance of <see cref="X64FunctionHook{TFunction}"/>, which may be used
        ///     to call the original function.
        /// </returns>
        /// <remarks>
        ///     Due to safety and depth concerns regarding the use of multiple hooks on a singular address,
        ///     the class does not provide an implementation allowing you to unhook. Please instead implement
        ///     a flag and just call + return value from the original method without changing any of the input
        ///     parameters if you wish to achieve the same effect.
        /// </remarks>
        public static X64FunctionHook<TFunction> Create(long gameFunctionAddress, TFunction functionDelegate, int hookLength)
        {
            /*
                Retrieve C# function details.
            */

            // Retrieve the function address from the supplied user delegate.
            // Our ReloadedFunction attribute.
            IntPtr cSharpFunctionAddress = Marshal.GetFunctionPointerForDelegate(functionDelegate);
            X64ReloadedFunctionAttribute reloadedFunction = GetReloadedFunctionAttribute<TFunction>();

            /*
                [Hook Part I] Create Custom => Microsoft Wrapper and Assemble 
            */

            // Assemble the wrapper function.
            // Assemble a jump to our wrapper function.
            IntPtr wrapperFunctionAddress = CreateWrapperFunction<TFunction>(cSharpFunctionAddress, reloadedFunction);
            List<byte> jumpBytes = HookCommon.X64AssembleAbsoluteJump(wrapperFunctionAddress, reloadedFunction, true).ToList();

            /*
                [Hook Part II] Calculate Hook Length (Unless Explicit)
            */

            // Retrieve hook length explicitly 
            if (hookLength == -1) { hookLength = HookCommon.GetHookLength((IntPtr)gameFunctionAddress, jumpBytes.Count, ArchitectureMode.x86_64); }

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
            List<byte> stolenBytes = Bindings.TargetProcess.ReadMemoryExternal((IntPtr)gameFunctionAddress, hookLength).ToList();

            // Check other functions that may need to be patched.
            (List<byte>, List<(IntPtr, byte[])>) stolenBytesAndAddressesToPatch = HookCommon.ProcessStolenBytes(stolenBytes, (IntPtr)gameFunctionAddress, ArchitectureMode.x86_64, reloadedFunction);
            stolenBytes = stolenBytesAndAddressesToPatch.Item1;

            // Calculate jump back address for original function.
            // Append absolute JMP instruction to return to original function for calling the original function in hook.
            IntPtr jumpBackAddress = (IntPtr)(gameFunctionAddress + hookLength);
            stolenBytes.AddRange(HookCommon.X64AssembleAbsoluteJump(jumpBackAddress, reloadedFunction));

            /*
                [Call Original Function part II] Instantiate and return functionHook with the original game function address.
            */

            // Assign original function.
            X64FunctionHook<TFunction> functionHook = new X64FunctionHook<TFunction>();

            // Write original bytes and jump to memory, and return address.
            functionHook.OriginalFunctionAddress = MemoryBufferManager.Add(stolenBytes.ToArray());

            // Create wrapper for calling the original function.
            functionHook.OriginalFunction = X64FunctionWrapper.CreateWrapperFunction<TFunction>((long)functionHook.OriginalFunctionAddress);

            // Store a copy of the original function.
            functionHook._originalDelegate = functionDelegate;

            /*
                [Apply Hook] Write hook bytes. 
                It is very important that this class instance is returned back to the caller
            */
            functionHook._addressesToPatch = stolenBytesAndAddressesToPatch.Item2;
            functionHook._hookToWrite = ((IntPtr)gameFunctionAddress, jumpBytes.ToArray());

            return functionHook;
        }

        /// <summary>
        /// Activates our hook.
        /// This function should be called after instantiation as soon as possible, preferably in the same line as instantiation.
        /// This class exists such that we don't run into concurrency issues on attaching to other processes, whereby
        /// a game calls a function while this class has not yet returned to the called from the factory method.
        /// </summary>
        public X64FunctionHook<TFunction> Activate()
        {
            // Patch all addresses
            Bindings.TargetProcess.WriteMemory(_hookToWrite.Item1, ref _hookToWrite.Item2);

            // Apply all patches.
            foreach (var addressToPatch in _addressesToPatch)
            { Bindings.TargetProcess.WriteMemory(addressToPatch.Item1, addressToPatch.Item2); }

            return this;
        }

        /// <summary>
        /// Retrieves a ReloadedFunction from a supplied delegate type.
        /// </summary>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute that defines the individual function properties.</typeparam>
        /// <returns>ReloadedFunction class instance that the delegate has been tagged with.</returns>
        public static X64ReloadedFunctionAttribute GetReloadedFunctionAttribute<TFunction>()
        {
            // Retrieve the ReloadedFunction attribute
            foreach (Attribute attribute in typeof(TFunction).GetCustomAttributes())
            {
                if (attribute is X64ReloadedFunctionAttribute reloadedFunction)
                    return reloadedFunction;
            }

            // Return cdecl if false.
            Bindings.PrintWarning?.Invoke
            (
                $"Instance of {typeof(TFunction).Name} in a developer declared hook is missing its ReloadedFunction attribute.\n" +
                 "The specified calling convention will be assumed as CDECL by default.\n" +
                 "To developers: Please don't do this! Refer to the wiki or CallingConventions.cs common convention settings."
            );

            return new X64ReloadedFunctionAttribute(X64CallingConventions.Microsoft);
        }

        /// <summary>
        /// Creates the wrapper function for redirecting program flow to our C# function.
        /// </summary>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="functionAddress">The address of the function to create a wrapper for.</param>
        /// <returns></returns>
        private static IntPtr CreateWrapperFunction<TFunction>(IntPtr functionAddress, X64ReloadedFunctionAttribute reloadedFunction)
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
                assemblyCode.AddRange(HookCommon.X64AssembleAbsoluteJumpMnemonics(functionAddress, reloadedFunction));       
            }
            else
            {
                // Backup Stack Frame
                assemblyCode.Add("push rbp");       // Backup old call frame
                assemblyCode.Add("mov rbp, rsp");   // Setup new call frame
                
                // TODO: Something Better to ensure alignment if not Aligned
                // We assume that we are stack aligned in usercall/custom calling conventions, if we are not, game over for now.
                // Our stack frame alignment is off by 8 here, we must also consider non-register parameters.

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
                assemblyCode.Add($"add rbp, {stackMisalignment}");   // Setup new call frame

                // Restore Stack Frame and Return
                assemblyCode.Add("pop rbp");
                assemblyCode.Add("ret");
            }

            // Assemble and return pointer to code
            byte[] assembledMnemonics = Assembler.Assembler.Assemble(assemblyCode.ToArray());
            return MemoryBufferManager.Add(assembledMnemonics);
        }

        /// <summary>
        /// Generates the assembly code to assemble for the passing of the 
        /// function parameters for to our own C# CDECL compliant function.
        /// </summary>
        /// <param name="parameterCount">The total amount of parameters that the target function accepts.</param>
        /// <param name="reloadedFunction">Structure containing the details of the actual function in question.</param>
        /// <param name="stackMisalignment">The amount of extra bytes that the stack pointer is decremented by/grown in order to 16-byte align the stack.</param>
        /// <returns>A string array of compatible x64 mnemonics to be assembled.</returns>
        private static string[] AssembleFunctionParameters(int parameterCount, X64ReloadedFunctionAttribute reloadedFunction, int stackMisalignment)
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

        /// <summary>
        /// Private constructor, constructors do not support delegates therefore use Factory Design Pattern.
        /// </summary>
        private X64FunctionHook() { }
    }
}
