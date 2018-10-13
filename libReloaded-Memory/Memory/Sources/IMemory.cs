using System;
using Vanara.PInvoke;

namespace Reloaded.Memory.Sources
{
    public interface IMemory
    {
        /// <summary>
        /// Reads a generic type from a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to read from.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        T Read<T>       (IntPtr memoryAddress, bool marshal);

        /// <summary>
        /// Writes a generic type to a specified memory address.
        /// </summary>
        /// <typeparam name="T">An individual struct type of a class with an explicit StructLayout.LayoutKind attribute.</typeparam>
        /// <param name="memoryAddress">The memory address to write to.</param>
        /// <param name="item">The item to write to the address.</param>
        /// <param name="marshal">Set this to true to enable struct marshalling.</param>
        void Write<T>       (IntPtr memoryAddress, ref T item, bool marshal);

        /// <summary>
        /// Changes the page permissions for a specified combination of address and length.
        /// </summary>
        /// <param name="memoryAddress">The memory address for which to change page permissions for.</param>
        /// <param name="size">The region size for which to change permissions for.</param>
        /// <param name="newPermissions">The new permissions to set.</param>
        /// <returns>The old page permissions.</returns>
        Kernel32.MEM_PROTECTION ChangePermission     (IntPtr memoryAddress, int size, Kernel32.MEM_PROTECTION newPermissions);
    }
}
