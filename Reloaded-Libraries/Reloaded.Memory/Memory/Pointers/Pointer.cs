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
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Represents a reference to a value of type <typeparamref name="TStruct"/>.
    /// Wraps a native pointer around a managed type, improving the ease of use.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    /// <typeparam name="TStruct">Value type to hold a reference to.</typeparam>
    public unsafe class Pointer<TStruct>
    {
        /// <summary>
        /// Gets the pointer to the value.
        /// </summary>
        public void* Address { get; set; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        public bool MarshalElements { get; set; }

        /// <summary>
        /// The source where memory will be read/written to/from.
        /// </summary>
        public IMemory Source { get; set; } = new Sources.Memory();

        /// <summary>
        /// Sets the value of what the reference points to.
        /// </summary>
        public TStruct Value
        {
            get => Source.Read<TStruct>((IntPtr)Address, MarshalElements);
            set => Source.Write((IntPtr)Address, ref value, MarshalElements);
        }

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Constructs a new instance of <see cref="Pointer{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="TStruct"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        public Pointer(ulong address)
        {
            Address = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Pointer{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="TStruct"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        /// <param name="marshalElements">If this is true; elements will be marshaled as they are read in and out from memory.</param>
        public Pointer(ulong address, bool marshalElements)
        {
            Address = (void*)address;
            MarshalElements = marshalElements;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Pointer{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="TStruct"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        /// <param name="memorySource">Specifies the source from which the pointer should be read/written.</param>
        public Pointer(ulong address, IMemory memorySource)
        {
            Address = (void*)address;
            Source = memorySource;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="Pointer{T}"/> given the address (pointer)
        /// at which the value of type <typeparamref name="TStruct"/> is stored.
        /// </summary>
        /// <param name="address">The address of the pointer pointing to generic type {T}</param>
        /// <param name="marshalElements">If this is true; elements will be marshaled as they are read in and out from memory.</param>
        /// <param name="memorySource">Specifies the source from which the pointer should be read/written.</param>
        public Pointer(ulong address, bool marshalElements, IMemory memorySource)
        {
            Address = (void*)address;
            MarshalElements = marshalElements;
            Source = memorySource;
        }
    }
}