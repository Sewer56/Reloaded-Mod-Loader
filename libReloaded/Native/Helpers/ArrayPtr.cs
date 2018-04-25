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

namespace Reloaded.Native.Helpers
{
    /// <summary>
    /// Abstracts a native 'C' type array of unknown size in memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe class ArrayPtr<T> : ICloneable, IList<T>, IStructuralComparable, IStructuralEquatable where T : struct
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="ArrayPtr{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ArrayPtr{T}"/>.
        /// </summary>
        public int Count => throw new NotSupportedException();

        /// <summary>
        /// Gets a value indicating whether the <see cref="ArrayPtr{T}"/> is read only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize { get; }

        public T this[int index]
        {
            get => Unsafe.Read<T>( GetPointerToElement( index ) );
            set => Unsafe.Write( GetPointerToElement( index ), value );
        }

        private ArrayPtr()
        {
            ElementSize = Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Construcs a new instance of
        /// </summary>
        /// <param name="value"></param>
        public ArrayPtr( ref T value ) : this()
        {
            Pointer = Unsafe.AsPointer( ref value );
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        public ArrayPtr( ulong address ) : this()
        {
            Pointer = ( void* )address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        public ArrayPtr( IntPtr address ) : this()
        {
            Pointer = address.ToPointer();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public object Clone() => new ArrayPtr<T>( ( ulong )Pointer );

        public IEnumerator<T> GetEnumerator() => new Enumerator<T>( this );

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item"></param>
        public void Add( T item ) => throw new NotSupportedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item"></param>
        public void Clear() => throw new NotSupportedException();

        /// <summary>
        /// Determines whether an element is in the <see cref="ArrayPtr{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains( T item ) => IndexOf( item ) != -1;

        /// <summary>
        /// Copies all the elements of the current one-dimensional array to the specified one-dimensional array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo( T[] array, int arrayIndex ) => throw new NotSupportedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove( T item ) => throw new NotSupportedException();

        public int IndexOf( T item )
        {
            // Risky
            var i = 0;
            while ( true )
            {
                if ( this[i++].Equals( item ) )
                    return i;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert( int index, T item ) => throw new NotSupportedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt( int index ) => throw new NotSupportedException();

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public int CompareTo( object other, IComparer comparer ) => throw new NotImplementedException();

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public bool Equals( object other, IEqualityComparer comparer ) => throw new NotImplementedException();

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public int GetHashCode( IEqualityComparer comparer ) => throw new NotImplementedException();

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void* GetPointerToElement( int index )
        {
            if ( index >= Count )
#pragma warning disable S112 // General exceptions should never be thrown
                throw new IndexOutOfRangeException( nameof( index ) );
#pragma warning restore S112 // General exceptions should never be thrown

            return ( void* )( ( long )Pointer + ( index * ElementSize ) );
        }

        private class Enumerator<T> : IEnumerator<T> where T : struct
        {
            private readonly ArrayPtr<T> mParent;
            private void* mCursor;

            public T Current => Unsafe.Read<T>( mCursor );

            object IEnumerator.Current => Current;

            public Enumerator( ArrayPtr<T> parent )
            {
                mParent = parent;
                mCursor = mParent.Pointer;
            }

            public void Dispose()
            {
                // Nothing to do
            }

            public bool MoveNext()
            {
                mCursor = ( void* )( ( long )mCursor + mParent.ElementSize );
                return true;
            }

            public void Reset()
            {
                mCursor = mParent.Pointer;
            }
        }
    }
}