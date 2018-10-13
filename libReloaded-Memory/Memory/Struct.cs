using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory
{
    /// <summary>
    /// Struct is a general utility class providing functions which provides various functions for working with structures; such
    /// as reading/writing to/from memory of structures.
    /// </summary>
    public static unsafe class Struct
    {
        /* Memory Sources */

        /// <summary>
        /// Defines the source for the default memory reading and writing <see cref="ToPtr"/> and <see cref="FromPtr"/> functions.
        /// </summary>
        public static IMemory Source { get; set; } = new Sources.Memory();

        /// <summary>
        /// Allows for access of memory of this individual process.
        /// </summary>
        private static IMemory _thisProcessMemory = new Sources.Memory(); 

        /* Redirections/Shorthands */

        /* ToPtr: Default Setting Shorthands */
        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">T</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static void ToPtr<T>(IntPtr pointer, T item, bool marshalElement) => ToPtr<T>(pointer, ref item, marshalElement);

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        public static void ToPtr<T>(IntPtr pointer, ref T item) => ToPtr(pointer, ref item, false);

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        public static void ToPtr<T>(IntPtr pointer, T item) => ToPtr(pointer, ref item, false);

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static void ToPtr<T>(IntPtr pointer, ref T item, bool marshalElement) => ToPtr(pointer, ref item, marshalElement, Source.Write);

        /* ToPtr: Master Function */

        /// <summary>
        /// Writes an item with a specified structure or class type with explicit StructLayout attribute to a pointer/memory address.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="item">The item to write to a specified pointer.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="writeFunction">The function to use that writes data to memory given a pointer, item, type and marshal option.</param>
        public static void ToPtr<T>(IntPtr pointer, ref T item, bool marshalElement, MemoryExtensions.WriteFunction<T> writeFunction) => writeFunction(pointer, ref item, marshalElement);

        /* FromPtr: Default Setting Shorthands */

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        public static T FromPtr<T>(IntPtr pointer) => FromPtr<T>(pointer, false);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="readFunction">A function that reads data from memory given a pointer, type and marshal option.</param>
        public static T FromPtr<T>(IntPtr pointer, MemoryExtensions.ReadFunction<T> readFunction) => FromPtr(pointer, false, readFunction);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static T FromPtr<T>(IntPtr pointer, bool marshalElement) => FromPtr(pointer, marshalElement, Source.Read<T>);

        /* FromPtr: Master Function */

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="pointer">The address where to read the struct from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        /// <param name="readFunction">A function that reads data from memory given a pointer, type and marshal option.</param>
        public static T FromPtr<T>(IntPtr pointer, bool marshalElement, MemoryExtensions.ReadFunction<T> readFunction) => readFunction(pointer, marshalElement);

        /* FromArray: Default Setting Shorthands */

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static T FromArray<T>(byte[] data, bool marshalElement) => FromArray<T>(data, 0, marshalElement);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        public static T FromArray<T>(byte[] data) => FromArray<T>(data, false);

        /* GetBytes: Default Setting Shorthands */

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(ref T item) => GetBytes(ref item, false);

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(T item) => GetBytes(ref item, false);

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static byte[] GetBytes<T>(T item, bool marshalElement) => GetBytes(ref item, false);

        /* GetSize: Default Setting Shorthands */

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        public static int GetSize<T>() => GetSize<T>(false);

        /* Implementation */

        /// <summary>
        /// Converts a byte array to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static T FromArray<T>(byte[] data, int startIndex, bool marshalElement)
        {
            fixed (byte* dataPtr = data)
            {
                return FromPtr((IntPtr)(&dataPtr[startIndex]), marshalElement, _thisProcessMemory.Read<T>);
            }
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="marshalElement">If set to true; will return the size of an element after marshalling.</param>
        public static int GetSize<T>(bool marshalElement)
        {
            return marshalElement ? Marshal.SizeOf<T>() : Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static byte[] GetBytes<T>(ref T item, bool marshalElement)
        {
            int size     = GetSize<T>(marshalElement);
            byte[] array = new byte[size];

            fixed (byte* arrayPtr = array)
            {
                ToPtr((IntPtr)arrayPtr, ref item, marshalElement, _thisProcessMemory.Write);
            }

            return array;
        }
    }
}
