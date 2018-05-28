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
using Reloaded.Process.Functions.X86Functions;

namespace Reloaded.Process.Helpers.Functions
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
    public class FunctionPtr<TDelegate> where TDelegate : Delegate
    {
        /// <summary>
        /// Contains a cache of already created function wrappers for functions at a specified address.
        /// </summary>
        private readonly Dictionary<IntPtr, TDelegate> _methodCache;

        /// <summary>
        /// Contains the delegate specifying the native function to be called.
        /// </summary>
        private TDelegate _delegate;

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
        public FunctionPtr(ulong functionPointer)
        {
            FunctionPointer = functionPointer;
            _methodCache = new Dictionary<IntPtr, TDelegate>();
        }

        /// <summary>
        /// Overrides the boolean operator, returning false if the pointer is invalid (zero).
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator bool(FunctionPtr<TDelegate> value) => value.Pointer != IntPtr.Zero;

        /// <summary>
        /// Retrieves an instance of the delegate which can be used to call the function behind the function pointer.
        /// </summary>
        /// <returns>False if the pointer to call is invalid, else true.</returns>
        public TDelegate GetDelegate()
        {
            // Return false if pointer points to invalid address.
            if (Pointer == IntPtr.Zero)
                return null;

            // Get the pointer to the function.
            // Our pointer is dereferenced here, see "Pointer" Property.
            IntPtr functionPointer = Pointer;

            // [Performance] Fast return if the pointer is the same as the last value.
            if (functionPointer == _lastFunctionPointer)
                return _delegate;

            // Try to get the cached function wrapper.
            if (_methodCache.TryGetValue(functionPointer, out var cachedDelegate))
            {
                return cachedDelegate;
            }

            // Cached delegate not found.
            else
            {
                // Create wrapper if nonexisting.
                _delegate = FunctionWrapper.CreateWrapperFunction<TDelegate>((long) functionPointer);

                // Cache the function wrapper.
                _methodCache[functionPointer] = _delegate;

                // Cache last function pointer.
                _lastFunctionPointer = functionPointer;

                // Return new Delegate
                return _delegate;
            }
        }
    }
}