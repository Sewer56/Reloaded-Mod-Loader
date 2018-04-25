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

namespace Reloaded.Native.Helpers
{
    /// <summary>
    /// Represents a native function pointer.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TDelegate"></typeparam>
    public class FunctionPtr<TDelegate> : FunctionPtrBase<TDelegate> where TDelegate : class
    {
        /// <summary>
        /// Construct a new callback given the address of the function pointer assigned to the callback.
        /// </summary>
        /// <param name="pointerAddress"></param>
        public FunctionPtr( ulong pointerAddress ) : base( pointerAddress )
        {
        }

        public static implicit operator bool( FunctionPtr<TDelegate> value ) => value.Pointer != IntPtr.Zero;

        /// <summary>
        /// Invoke the callback function. Throws if the callback has no function pointer assigned.
        /// </summary>
        /// <param name="args">Arguments to the callback function.</param>
        public void Invoke( params object[] args )
        {
            PrepareInvoke();
            InvokeMethod.Invoke( Delegate, args );
        }
    }

    /// <summary>
    /// Represents a native function pointer.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TDelegate"></typeparam>
    public class FunctionPtr<TResult, TDelegate> : FunctionPtrBase<TDelegate> where TDelegate : class
    {
        /// <summary>
        /// Construct a new callback given the address of the function pointer assigned to the callback.
        /// </summary>
        /// <param name="pointerAddress"></param>
        public FunctionPtr( ulong pointerAddress ) : base( pointerAddress )
        {
        }

        public static implicit operator bool( FunctionPtr<TResult, TDelegate> value ) => value.Pointer != IntPtr.Zero;

        /// <summary>
        /// Invoke the callback function. Throws if the callback has no function pointer assigned.
        /// </summary>
        /// <param name="args">Arguments to the callback function.</param>
        /// <returns>Return value of the function.</returns>
        public TResult Invoke( params object[] args )
        {
            PrepareInvoke();
            return ( TResult )InvokeMethod.Invoke( Delegate, args );
        }
    }
}