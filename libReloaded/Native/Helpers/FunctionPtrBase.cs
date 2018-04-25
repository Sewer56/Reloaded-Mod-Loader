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

namespace Reloaded.Native.Helpers
{
    /// <summary>
    /// Represents a native function pointer.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TDelegate"></typeparam>
    public abstract class FunctionPtrBase<TDelegate> where TDelegate : class
    {
        private IntPtr mLastFunctionPointer;
        private readonly Dictionary<IntPtr, (MethodInfo, TDelegate)> mCache;
        protected MethodInfo InvokeMethod;
        protected TDelegate Delegate;

        /// <summary>
        /// Gets the address of the function pointer assigned to the callback.
        /// </summary>
        public ulong PointerAddress { get; }

        /// <summary>
        /// Gets the function pointer assigned to the callback (may be null/IntPtr.Zero).
        /// </summary>
        public unsafe IntPtr Pointer => *( IntPtr* )PointerAddress;

        /// <summary>
        /// Construct a new callback given the address of the function pointer assigned to the callback.
        /// </summary>
        /// <param name="pointerAddress"></param>
        protected FunctionPtrBase( ulong pointerAddress )
        {
            PointerAddress = pointerAddress;
            mCache = new Dictionary<IntPtr, (MethodInfo, TDelegate)>();
        }

        /// <summary>
        /// Prepare for the invocation of the callback delegate.
        /// </summary>
        protected void PrepareInvoke()
        {
            if ( Pointer == IntPtr.Zero )
                throw new InvalidOperationException( "Function pointer is null" );

            var functionPointer = Pointer;
            if ( functionPointer == mLastFunctionPointer )
                return;

            if ( !mCache.TryGetValue( functionPointer, out var cached ) )
            {
                InvokeMethod = typeof( TDelegate ).GetMethod( "Invoke" );
                if ( InvokeMethod == null )
                    throw new InvalidOperationException( "No 'Invoke' method found on TDelegate" );

                Delegate = Marshal.GetDelegateForFunctionPointer<TDelegate>( functionPointer );
                mCache[functionPointer] = (InvokeMethod, Delegate);
            }
            else
            {
                InvokeMethod = cached.Item1;
                Delegate = cached.Item2;
            }

            mLastFunctionPointer = functionPointer;
        }
    }
}