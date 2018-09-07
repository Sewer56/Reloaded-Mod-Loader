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
            X64ReloadedFunctionAttribute reloadedFunction = FunctionCommon.GetX64ReloadedFunctionAttribute<TFunction>();

            /*
                [Hook Part I] Create Custom => Microsoft Wrapper and Assemble 
            */

            // Assemble the wrapper function.
            // Assemble a jump to our wrapper function.
            IntPtr wrapperFunctionAddress = X64ReverseFunctionWrapper<TFunction>.CreateX64WrapperFunctionInternal<TFunction>(cSharpFunctionAddress, reloadedFunction);
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
        /// Private constructor, constructors do not support delegates therefore use Factory Design Pattern.
        /// </summary>
        private X64FunctionHook() { }
    }
}
