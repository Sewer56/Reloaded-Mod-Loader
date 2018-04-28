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
using Reloaded.Process.X86Functions.CustomFunctionFactory;

namespace Reloaded.Native.Helpers.Functions
{
    /*
       Commented code.
       Requires at least C# 7.3 and/or Visual Studio 15.7 to compile.
       Delegate constraint required.
    */

    /// <summary>
    /// Wraps a native function pointer into a more familliar, managed type.
    /// Represents a native function pointer.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate type marked with complete ReloadedFunction Attribute that defines the individual function properties.</typeparam>
    public abstract class FunctionPtr<TDelegate> // where TDelegate : Delegate /* For when C# 7.3 arrives to normal VS */
    {
        /// <summary>
        /// Contains a cache of already created function wrappers for functions at a specified address.
        /// </summary>
        private readonly Dictionary<IntPtr, TDelegate> _methodCache;

        /// <summary>
        /// Contains the delegate specifying the native function to be called.
        /// </summary>
        protected TDelegate Delegate;

        /// <summary>
        /// [Performance Optimization] Contains the address of the last called function.
        /// </summary>
        private IntPtr _lastFunctionPointer;

        /// <summary>
        /// Gets the address of the assigned function pointer for the method to be called.
        /// </summary>
        public ulong FunctionPointer { get; }

        /// <summary>
        /// Gets the address of the function from the assigned function pointer for the method to be executed.
        /// This may be null/IntPtr.Zero.
        /// </summary>
        public unsafe IntPtr Pointer => *(IntPtr*)FunctionPointer;

        /// <summary>
        /// Construct a new callback given the address of the function pointer assigned to the callback.
        /// </summary>
        /// <param name="functionPointer"></param>
        protected FunctionPtr(ulong functionPointer)
        {
            FunctionPointer = functionPointer;
            _methodCache = new Dictionary<IntPtr, TDelegate>();
        }

        /// <summary>
        /// Calls the function behind the pointer.
        /// </summary>
        /// <param name="args">The arguments to be passsed to the delegate.</param>
        /// <returns></returns>
        public object CallFunction(params object[] args)
        {
            // Prepare the calling of the function.
            PrepareInvoke();

            // TODO: This is untested, not the non C# 7.3 version at least.
            // Cast to delegate.
            Delegate methodDelegate = Delegate as Delegate;

            // Return delegate if exists.
            return methodDelegate?.DynamicInvoke(args);
        }

        /// <summary>
        /// Overrides the boolean operator, returning false if the pointer is invalid (zero).
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator bool(FunctionPtr<TDelegate> value) => value.Pointer != IntPtr.Zero;

        /// <summary>
        /// Prepare for the invocation of the callback delegate.
        /// </summary>
        /// <returns>False if the pointer to call is invalid, else true.</returns>
        protected bool PrepareInvoke()
        {
            // Return false if pointer points to invalid address.
            if (Pointer == IntPtr.Zero)
                return false;

            // Get the pointer to the function.
            IntPtr functionPointer = Pointer;

            // [Performance] Fast return if the pointer is the same as the last value.
            if (functionPointer == _lastFunctionPointer)
                return true;

            // Try to get the cached function wrapper.
            if (_methodCache.TryGetValue(functionPointer, out var cachedDelegate))
            {
                Delegate = cachedDelegate;
            }
            // Cached delegate not found.
            else
            {
                // Create wrapper if nonexisting.
                Delegate = FunctionWrapper.CreateWrapperFunction<TDelegate>((long) functionPointer);

                // Cache the function wrapper.
                _methodCache[functionPointer] = Delegate;
            }
            
            _lastFunctionPointer = functionPointer;

            return true;
        }
    }
}