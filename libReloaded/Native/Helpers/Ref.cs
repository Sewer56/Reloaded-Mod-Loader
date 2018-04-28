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
using System.Runtime.CompilerServices;

namespace Reloaded.Native.Helpers
{
    /// <summary>
    /// Represents a reference to a value of type <typeparamref name="T"/>.
    /// Wraps a native pointer around a managed type, improving the ease of use.
    /// </summary>
    /// <typeparam name="T">Value type to hold a reference to.</typeparam>
    public unsafe struct Ref<T> where T : struct
    {
        /// <summary>
        /// Gets the pointer to the value.
        /// </summary>
        public void* Pointer { get; }

        /// <summary>
        /// Sets the value of what the reference points to.
        /// </summary>
        public T Value
        {
            get => Unsafe.Read<T>(Pointer);
            set => Unsafe.Write(Pointer, value);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Ref{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="T"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        public Ref(ulong address)
        {
            Pointer = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Ref{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="T"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        public Ref(IntPtr address)
        {
            Pointer = (void*)address;
        }

        public static implicit operator T(Ref<T> value) => value.Value;
        // No assignment (=) operator overload is possible, unfortunately.

        /// <summary>
        /// Sets the value of what the reference points to.
        /// </summary>
        /// <param name="value"></param>
        public void Set(T value) => Value = value;
    }
}