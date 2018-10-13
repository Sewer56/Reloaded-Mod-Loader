using System;
using System.Runtime.CompilerServices;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    /// <summary>
    /// A generic extension class that extends <see cref="IMemory"/>.
    /// Provides various functions such as reading arrays.
    /// </summary>
    public static unsafe class MemoryExtensions
    {
        /* All functions are documented in the IMemory interface. */

        /*
            ----------------------
            Read Implementation(s)
            ----------------------
        */

        /* Delegates */
        public delegate T    ReadFunction<T> (IntPtr memoryAddress, bool marshal);
        public delegate void WriteFunction<T>(IntPtr memoryAddress, ref T item, bool marshal);

        /* Read Base Implementation */

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static T[] Read<T>(this IMemory memory, IntPtr memoryAddress, int arrayLength, bool marshal)
        {
            IMemory oldSource = Struct.Source;
            Struct.Source = memory;

            var returnValue = StructArray.FromPtr<T>(memoryAddress, arrayLength, marshal);

            Struct.Source = oldSource;
            return returnValue;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static T SafeRead<T>(this IMemory memory, IntPtr memoryAddress, bool marshal)
        {
            int structSize = Struct.GetSize<T>(marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, structSize, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            var returnValue = memory.Read<T>(memoryAddress, marshal);
            memory.ChangePermission(memoryAddress, structSize, oldProtection);

            return returnValue;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be read and reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The amount of array items to read.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static T[] SafeRead<T>(this IMemory memory, IntPtr memoryAddress, int arrayLength, bool marshal)
        {
            int regionSize = StructArray.GetSize<T>(marshal, arrayLength);

            var oldProtection = memory.ChangePermission(memoryAddress, regionSize, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            var returnValue = memory.Read<T>(memoryAddress, arrayLength, marshal);
            memory.ChangePermission(memoryAddress, regionSize, oldProtection);

            return returnValue;
        }

        /* Write Base Implementation */

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void Write<T>(this IMemory memory, IntPtr memoryAddress, T[] items, bool marshal)
        {
            IMemory oldSource = Struct.Source;
            Struct.Source = memory;

            StructArray.ToPtr(memoryAddress, items, marshal);

            Struct.Source = oldSource;
        }

        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, ref T item, bool marshal)
        {
            int memorySize = Struct.GetSize<T>(marshal);

            var oldProtection = memory.ChangePermission(memoryAddress, memorySize, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, ref item, marshal);
            memory.ChangePermission(memoryAddress, memorySize, oldProtection);
        }


        /// <summary>
        /// Changes memory permissions to ensure memory can be written and writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="items">The array of items to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, T[] items, bool marshal)
        {
            int regionSize = StructArray.GetSize<T>(items.Length);

            var oldProtection = memory.ChangePermission(memoryAddress, regionSize, Kernel32.MEM_PROTECTION.PAGE_EXECUTE_READWRITE);
            memory.Write(memoryAddress, items, marshal);
            memory.ChangePermission(memoryAddress, regionSize, oldProtection);
        }

        /*
            ------------
            Redirections
            ------------
        */

        /*
            Redirections simply set the default settings for the various overload shorthands.
            While it is not necessary; deriving classes may override the defaults as they wish.
        */

        /* Read: Shorthand Redirections */

        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(this IMemory memory, IntPtr memoryAddress) => memory.Read<T>(memoryAddress, false);

        /// <summary>
        /// Reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The number of items to read from memory.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Read<T>(this IMemory memory, IntPtr memoryAddress, int arrayLength) => Read<T>(memory, memoryAddress, arrayLength, false);

        /// <summary>
        /// Overrides the page permissions for a memory region and reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SafeRead<T>(this IMemory memory, IntPtr memoryAddress) => SafeRead<T>(memory, memoryAddress, false);

        /// <summary>
        /// Overrides the page permissions for a memory region and reads a generic type array from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="arrayLength">The number of items to read from memory.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] SafeRead<T>(this IMemory memory, IntPtr memoryAddress, int arrayLength) => SafeRead<T>(memory, memoryAddress, arrayLength, false);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IMemory memory, IntPtr memoryAddress, ref T item) => memory.Write<T>(memoryAddress, ref item, false);

        /// <summary>
        /// Writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IMemory memory, IntPtr memoryAddress, T[] item) => Write(memory, memoryAddress, item, false);

        /// <summary>
        /// Overrides the page permissions for a memory region and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, ref T item) => SafeWrite<T>(memory, memoryAddress, ref item, false);

        /// <summary>
        /// Overrides the page permissions for a memory region and writes a generic type array to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, T[] item) => SafeWrite(memory, memoryAddress, item, false);

        /* Read: By Reference Redirections */

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IMemory memory, IntPtr memoryAddress, T item) => Write<T>(memory, memoryAddress, ref item);

        /// <summary>
        /// Overrides the page permissions for a memory region and writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The items to write to the address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeWrite<T>(this IMemory memory, IntPtr memoryAddress, T item) => SafeWrite<T>(memory, memoryAddress, ref item);

        /* ChangePermission: By Reference Redirections */

        /// <summary>
        /// Changes the page permissions for a specified combination of address and length.
        /// </summary>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="baseElement">The struct element from which the region size to change permissions for will be calculated.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <returns>The old page permissions.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Kernel32.MEM_PROTECTION ChangePermission<T>(this IMemory memory, IntPtr memoryAddress, T baseElement, Kernel32.MEM_PROTECTION newPermissions)
                           => ChangePermission<T>(memory, memoryAddress, ref baseElement, newPermissions);

        /* ChangePermission: Size Redirections */

        /// <summary>
        /// Changes the page permissions for a specified combination of address and length.
        /// </summary>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="baseElement">The struct element from which the region size to change permissions for will be calculated.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <returns>The old page permissions.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Kernel32.MEM_PROTECTION ChangePermission<T>(this IMemory memory, IntPtr memoryAddress, ref T baseElement, Kernel32.MEM_PROTECTION newPermissions)
                           => memory.ChangePermission(memoryAddress, Struct.GetSize<T>(false), newPermissions);
    }
}
