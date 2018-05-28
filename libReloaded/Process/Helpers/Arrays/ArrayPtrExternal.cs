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
using Reloaded.Process.Memory;

namespace Reloaded.Process.Helpers.Arrays
{
    /// <summary>
    /// An alternative variation of <see cref="ArrayPtr{TStruct}"/> that can be used for
    /// reading arrays from external processes, i.e. when not an injected DLL/module inside the same project.
    /// </summary>
    /// <typeparam name="TStruct">
    ///     The struct type to wrap in an array.
    ///     It should (probably) be marked [StructLayout(LayoutKind.Sequential)]
    /// </typeparam>
    public unsafe class ArrayPtrExternal<TStruct> : ICloneable where TStruct : struct
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="ArrayPtrExternal{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize { get; }

        /// <summary>
        /// Indexer for this class, allowing for retrieval of an item at a specific index.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>Your item to retrieve from the array.</returns>
        public TStruct this[int index]
        {
            get => Bindings.TargetProcess.ReadMemoryExternal<TStruct>((IntPtr)GetPointerToElement(index));
            set => Bindings.TargetProcess.WriteMemoryExternal((IntPtr)GetPointerToElement(index), value);
        }

        /// <summary>
        /// Construcs a new instance of <see cref="ArrayPtr{T}"/> from a pointer to a structure
        /// in memory.
        /// </summary>
        /// <param name="value">The pointer to the structure in memory.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtrExternal(ref TStruct value) : this()
        {
            Pointer = Unsafe.AsPointer(ref value);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtrExternal(ulong address) : this()
        {
            Pointer = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtrExternal(IntPtr address) : this()
        {
            Pointer = address.ToPointer();
        }

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>Pointer to the requested element at index.</returns>
        public void* GetPointerToElement(int index)
        {
            return (void*)((long)Pointer + (index * ElementSize));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public object Clone() => new ArrayPtr<TStruct>((ulong)Pointer);

        /// <summary>
        /// Common private constructor initializing the size of the individual structures.
        /// </summary>
        private ArrayPtrExternal()
        {
            ElementSize = Unsafe.SizeOf<TStruct>();
        }
    }
}