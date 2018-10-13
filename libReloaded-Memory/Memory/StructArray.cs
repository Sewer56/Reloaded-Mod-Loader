using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Memory
{
    /// <summary>
    /// Utility class for working with struct arrays.
    /// </summary>
    public static unsafe class StructArray
    {
        /* FromPtr: Default Setting Shorthands */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        public static T[] FromPtr<T>(IntPtr memoryAddress) => FromPtr<T>(memoryAddress, 0);

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The number of items to read from memory.</param>
        public static T[] FromPtr<T>(IntPtr memoryAddress, int arrayLength) => FromPtr<T>(memoryAddress, arrayLength, false);

        /* ToPtr: Default Setting Shorthands */

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        public static void ToPtr<T>(IntPtr memoryAddress, T[] item) => ToPtr(memoryAddress, item, false);

        /* FromArray: Default Setting Shorthands */

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static T[] FromArray<T>(byte[] data, bool marshalElement) => FromArray<T>(data, 0, marshalElement);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        public static T[] FromArray<T>(byte[] data) => FromArray<T>(data, 0, false);

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        public static T[] FromArray<T>(byte[] data, int startIndex) => FromArray<T>(data, startIndex, false);

        /* GetBytes: Default Setting Shorthands */

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(T[] items) => GetBytes(items, false);

        /* Implementation */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The number of items to read from memory.</param>
        /// <param name="marshal">Set to true to marshal the element.</param>
        public static T[] FromPtr<T>(IntPtr memoryAddress, int arrayLength, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);
            T[] arrayEntries = new T[arrayLength];

            for (int x = 0; x < arrayLength; x++)
            {
                IntPtr address = memoryAddress + (structSize * x);
                arrayEntries[x] = Struct.FromPtr<T>(address, marshal);
            }

            return arrayEntries;
        }

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">Set this to true in order to marshal the value when writing to memory.</param>
        public static void ToPtr<T>(IntPtr memoryAddress, T[] item, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);

            for (int x = 0; x < item.Length; x++)
            {
                IntPtr address = memoryAddress + (structSize * x);
                Struct.ToPtr(address, ref item[x], marshal);
            }
        }

        /// <summary>
        /// Converts a pointer/memory address to a specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="data">A byte array containing data from which to extract a structure from.</param>
        /// <param name="startIndex">The index in the byte array to read the element(s) from.</param>
        /// <param name="marshalElement">Set to true to marshal the element.</param>
        public static T[] FromArray<T>(byte[] data, int startIndex, bool marshalElement)
        {
            int structSize = Struct.GetSize<T>(marshalElement);
            int structureCount = (data.Length - startIndex) / structSize;
            T[] structures = new T[structureCount];

            for (int x = 0; x < structures.Length; x++)
            {
                int offset = startIndex + (structSize * x);
                structures[x] = Struct.FromArray<T>(data, offset, marshalElement);
            }

            return structures;
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="marshalElement">If set to true; will return the size of an element after marshalling.</param>
        /// <param name="elementCount">The number of array elements present.</param>
        public static int GetSize<T>(bool marshalElement, int elementCount)
        {
            return Struct.GetSize<T>(marshalElement) * elementCount;
        }

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="elementCount">The number of array elements present.</param>
        public static int GetSize<T>(int elementCount)
        {
            return Struct.GetSize<T>(false) * elementCount;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        /// <param name="marshalElements">Set to true to marshal the item(s).</param>
        public static byte[] GetBytes<T>(T[] items, bool marshalElements)
        {
            int totalSize = GetSize<T>(items.Length);
            List<byte> array = new List<byte>(totalSize);

            for (int x = 0; x < items.Length; x++)
                array.AddRange(Struct.GetBytes(ref items[x]));

            return array.ToArray();
        }
    }
}
