using System;
using System.Runtime.CompilerServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Pointers
{
    /// <summary>
    /// Abstracts a native 'C' type array of unknown size in memory to a more familiar interface.
    /// TStruct can be a primitive, a struct or a class with explicit StructLayout attribute.
    /// </summary>
    public unsafe class ArrayPtr<TStruct>
    {
        /// <summary>
        /// Gets the pointer to the start of the data contained in the <see cref="ArrayPtr{T}"/>.
        /// </summary>
        public void* Pointer { get; set; }

        /// <summary>
        /// If this is true; elements will be marshaled as they are read in and out from memory.
        /// </summary>
        public bool MarshalElements { get; set; }

        /// <summary>
        /// The source where memory will be read/written to/from.
        /// </summary>
        public IMemory MemorySource { get; set; } = new Sources.Memory();

        /// <summary>
        /// Size of a single element in the array, in bytes.
        /// </summary>
        public int ElementSize => Struct.GetSize<TStruct>(MarshalElements);

        /// <summary>
        /// Indexer for this class, allowing for retrieval of an item at a specific index.
        /// </summary>
        /// <param name="index">The index of the item to retrieve.</param>
        /// <returns>Your item to retrieve from the array.</returns>
        public TStruct this[int index]
        {
            get => MemorySource.Read<TStruct>((IntPtr)GetPointerToElement(index), MarshalElements);
            set => MemorySource.Write((IntPtr)GetPointerToElement(index), ref value, MarshalElements);
        }

        /*
            ------------
            Constructors
            ------------
        */

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address)
        {
            Pointer = (void*)address;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element
        /// and whether elements should be marshaled or not as they are read.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="marshalElements">Set to true in order to marshal elements as they are read in and out.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address, bool marshalElements)
        {
            Pointer = (void*)address;
            MarshalElements = marshalElements;
        }

        /// <summary>
        /// Constructs a new instance of <see cref="ArrayPtr{T}"/> given the address of the first element
        /// and whether elements should be marshaled or not as they are read.
        /// </summary>
        /// <param name="address">The address of the first element of the structure array.</param>
        /// <param name="marshalElements">Set to true in order to marshal elements as they are read in and out.</param>
        /// <param name="memorySource">Specifies the source from which the individual array elements should be read/written.</param>
        /// <remarks>See <see cref="ArrayPtr{T}"/></remarks>
        public ArrayPtr(ulong address, bool marshalElements, IMemory memorySource)
        {
            Pointer = (void*)address;
            MarshalElements = marshalElements;
            MemorySource = memorySource;
        }

        /*
            --------------
            Core Functions
            --------------
        */

        /// <summary>
        /// Gets the pointer to the element at the given index.
        /// </summary>
        /// <param name="index">The index to retrieve a pointer for.</param>
        /// <returns>Pointer to the requested element at index.</returns>
        public void* GetPointerToElement(int index)
        {
            return (void*)((long)Pointer + (index * ElementSize));
        }
    }
}