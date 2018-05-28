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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Reloaded.Process.Helpers.Arrays
{
    /// <summary>
    /// Abstracts a native 'C' type array of a fixed size in memory to a more familliar interface.
    /// This class is performance optimized and does NOT respect page permissions, therefore will throw IRREPERABLY
    /// if you attempt to read memory without read/write permissions.
    /// </summary>
    /// <typeparam name="TStruct">
    ///     The struct type to wrap in an array.
    ///     It should (probably) be marked [StructLayout(LayoutKind.Sequential)]
    /// </typeparam>
    public unsafe class FixedArrayPtr<TStruct> : ICloneable, IEnumerable<TStruct> where TStruct : struct
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Returns the size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize { get; }

        /// <summary>
        /// Contains the size of the entire array, in bytes.
        /// </summary>
        public int ArraySize { get; }

        /// <summary>
        /// Indexer for this class, allowing for retrieval of an item at a specific index.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>Your item to retrieve from the array.</returns>
        public TStruct this[int index]
        {
            get => Unsafe.Read<TStruct>(GetPointerToElement(index));
            set => Unsafe.Write(GetPointerToElement(index), value);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public object Clone() => new FixedArrayPtr<TStruct>((ulong)Pointer, Count);

        /// <summary>
        /// Determines whether an element is in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TStruct item) => IndexOf(item) != -1;

        /// <summary>
        /// Construcs a new instance of <see cref="FixedArrayPtr{T}"/> given an initial starting
        /// pointer to a structure in memory and the amount of elements in the array.
        /// </summary>
        /// <param name="value">The pointer to a structure in memory.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <remarks>See <see cref="FixedArrayPtr{T}"/></remarks>
        public FixedArrayPtr(ref TStruct value, int count) : this(count)
        {
            Pointer = Unsafe.AsPointer(ref value);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <remarks>See <see cref="FixedArrayPtr{T}"/></remarks>
        public FixedArrayPtr(ulong address, int count) : this(count)
        {
            Pointer = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="count">The amount of elements in the array structure in memory.</param>
        /// <remarks>See <see cref="FixedArrayPtr{T}"/></remarks>
        public FixedArrayPtr(IntPtr address, int count) : this(count)
        {
            Pointer = address.ToPointer();
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional array to the specified one-dimensional array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TStruct[] array, int arrayIndex)
        {
            // Get pointer to array
            var pArray = Unsafe.AsPointer(ref array[0]);

            // Calculate start offset within array
            var arrayOffset = (arrayIndex * ElementSize);

            // Calculate destination ptr within array
            var pArrayDest = (void*)((IntPtr)pArray + arrayOffset);

            // Copy the memory from this array to the other array
            Unsafe.CopyBlock(pArrayDest, Pointer, (uint)ArraySize);
        }

        /// <summary>
        /// Searches for a specified item and returns the index of the item
        /// if present.
        /// </summary>
        /// <param name="item">The item to search for in the array.</param>
        /// <returns>The index of the item, if present in the array.</returns>
        public int IndexOf(TStruct item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Equals(item))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>
        ///     Pointer to the requested element at index.
        ///     -1 if the element is not part of the collection.
        /// </returns>
        public void* GetPointerToElement(int index)
        {
            // Do not throw, throwing exceptions makes for some very ugly code on other side.
            if (index >= Count)
                return (void*)-1;
            else
                return (void*)((long) Pointer + (index * ElementSize));
        }

        /// <summary>
        /// Common private constructor initializing the size of the individual structures and
        /// complete array size.
        /// </summary>
        private FixedArrayPtr(int count)
        {
            Count = count;
            ElementSize = Unsafe.SizeOf<TStruct>();
            ArraySize = Count * ElementSize;
        }

        // ///////////////////////////////////////////
        // Implement IEnumerable to allow LINQ Queries
        // ///////////////////////////////////////////
        public IEnumerator<TStruct> GetEnumerator() => new FixedArrayPtrEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Implements the IEnumerator Structure for the Fixed Array Pointer, allowing for
        /// LINQ queries to be used.
        /// </summary>
        private class FixedArrayPtrEnumerator : IEnumerator<TStruct>
        {
            /// <summary>
            /// Contains a copy of the parent object that is to be enumerated.
            /// </summary>
            private readonly FixedArrayPtr<TStruct> _arrayPtr;

            /// <summary>
            /// Contains the index of the current element being enumerated.
            /// </summary>
            private int _currentIndex;

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Stores the address of the current element being enumerated.
            /// </summary>
            private void* _cursor;

            /// <summary>
            /// Constructor for the custom enumerator.
            /// </summary>
            /// <param name="parentArrayPtr">Contains original FixedArrayPtr this enumerator was intended for.</param>
            public FixedArrayPtrEnumerator(FixedArrayPtr<TStruct> parentArrayPtr)
            {
                _arrayPtr = parentArrayPtr;
                _currentIndex = 0;
                _cursor = parentArrayPtr.Pointer;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            public TStruct Current => Unsafe.Read<TStruct>(_cursor);

            /// <summary>
            /// Advances the enumerator <see cref="_cursor"/> to the next element of the collection.
            /// </summary>
            /// <returns>
            ///     True if the enumerator was successfully advanced to the next element.
            ///     False if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                // Check if we passed the end of the collection.
                if (_currentIndex >= _arrayPtr.Count)
                    return false;

                // Otherwise append the array size tot he current collection.
                _cursor = (void*)((long)_cursor + _arrayPtr.ElementSize);

                // Increase our current index.
                ++_currentIndex;

                return true;
            }

            /// <summary>
            /// Resets the current index and pointer to the defaults.
            /// </summary>
            public void Reset()
            {
                _currentIndex = 0;
                _cursor = _arrayPtr.Pointer;
            }

            /// <summary>
            /// Nothing to do.
            /// </summary>
            public void Dispose()
            {
                // Nothing to do
            }
        }
    }
}