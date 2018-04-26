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
    /// Abstracts a native 'C' type array of a fixed size in memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public unsafe class FixedArrayPtr<T> : ICloneable, IList<T>, IStructuralComparable, IStructuralEquatable where T : struct
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
        /// Gets a value indicating whether the <see cref="FixedArrayPtr{T}"/> is read only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize { get; }

        /// <summary>
        /// Size of the entire array, in bytes.
        /// </summary>
        public int ArraySize { get; }

        public T this[int index]
        {
            get => Unsafe.Read<T>( GetPointerToElement( index ) );
            set => Unsafe.Write( GetPointerToElement( index ), value );
        }

        private FixedArrayPtr( int count )
        {
            Count = count;
            ElementSize = Unsafe.SizeOf<T>();
            ArraySize = Count * ElementSize;
        }

        /// <summary>
        /// Construcs a new instance of
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        public FixedArrayPtr( ref T value, int count ) : this( count )
        {
            Pointer = Unsafe.AsPointer( ref value );
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        public FixedArrayPtr( ulong address, int count ) : this( count )
        {
            Pointer = ( void* ) address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="FixedArrayPtr{T}"/> given the address of the first element, 
        /// and the number of elements that follow it.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="count"></param>
        public FixedArrayPtr( IntPtr address, int count ) : this( count )
        {
            Pointer = address.ToPointer();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public object Clone() => new FixedArrayPtr<T>( ( ulong )Pointer, Count );

        public IEnumerator<T> GetEnumerator() => new Enumerator( this );

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
        /// Determines whether an element is in the <see cref="FixedArrayPtr{T}"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains( T item ) => IndexOf( item ) != -1;

        /// <summary>
        /// Copies all the elements of the current one-dimensional array to the specified one-dimensional array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo( T[] array, int arrayIndex )
        {
            // Get pointer to array
            var pArray = Unsafe.AsPointer( ref array[0] );

            // Calculate start offset within array
            var arrayOffset = ( arrayIndex * ElementSize );

            // Calculate destination ptr within array
            var pArrayDest = ( void* )( ( IntPtr )pArray + arrayOffset );

            // Copy the memory from this array to the other array
            Unsafe.CopyBlock( pArrayDest, Pointer, ( uint )ArraySize );
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove( T item ) => throw new NotSupportedException();

        public int IndexOf( T item )
        {
            for ( int i = 0; i < Count; i++ )
            {
                if ( this[i].Equals( item ) )
                    return i;
            }

            return -1;
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

            return ( void* ) ( ( long ) Pointer + ( index * ElementSize ) );
        }

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
        private class Enumerator : IEnumerator<T>
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
        {
            private readonly FixedArrayPtr<T> mParent;
            private void* mCursor;
            private int mIndex;

            public T Current => Unsafe.Read<T>( mCursor );

            object IEnumerator.Current => Current;

            public Enumerator( FixedArrayPtr<T> parent )
            {
                mParent = parent;
                mIndex = 0;
                mCursor = mParent.Pointer;
            }

            public void Dispose()
            {
                // Nothing to do
            }

            public bool MoveNext()
            {
                if ( mIndex >= mParent.Count )
                    return false;

                mCursor = ( void* ) ( ( long ) mCursor + mParent.ElementSize );
                ++mIndex;
                return true;
            }

            public void Reset()
            {
                mIndex = 0;
                mCursor = mParent.Pointer;
            }
        }
    }
}