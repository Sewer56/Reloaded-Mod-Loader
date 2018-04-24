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
using System.Collections.Generic;
using Reloaded.Process.Memory;

namespace Reloaded.Process.X86Functions.Utilities
{
    /// <summary>
    /// Provides various utilities and fields related to Virtual Function
    /// tables for your use within the target process.
    /// </summary>
    public class VirtualFunctionTable
    {
        /// <summary>
        /// Stores a list of the individual table addresses for this Virtual Function Table.
        /// </summary>
        public List<TableEntry> TableEntries { get; set; }

        /// <summary>
        /// An indexer override allowing for individual Virtual Function Table
        /// entries to be easier accessed.
        /// </summary>
        /// <param name="i">The individual entry in the virtual function table.</param>
        /// <returns>The individual corresponding virtual function table entry.</returns>
        public TableEntry this[int i]
        {
            get => TableEntries[i];
            set => TableEntries[i] = value;
        }

        /// <summary>
        /// A structure type which describes the individual function pointer by its
        /// memory location and its target address.
        /// </summary>
        public struct TableEntry
        {
            /// <summary>
            /// The address in process memory where the VTable entry has been found.
            /// </summary>
            public IntPtr EntryAddress;

            /// <summary>
            /// The value of the individual entry in process memory for the VTable entry pointing to a function.
            /// </summary>
            public IntPtr FunctionPointer;
        }

        /// <summary>
        /// Initiates a virtual function table from an object address in memory.
        /// An assumption is made that the virtual function table pointer is the first parameter.
        /// </summary>
        /// <param name="objectAddress">
        ///     The memory address at which the object is stored.
        ///     The function will assume that the first entry is a pointer to the virtual function
        ///     table, as standard with C++ code.
        /// </param>
        /// <param name="numberOfMethods">
        ///     The number of methods contained in the virtual function table.
        ///     For enumerables, you may obtain this value as such: Enum.GetNames(typeof(MyEnum)).Length; where
        ///     MyEnum is the name of your enumerable.
        /// </param>
        public VirtualFunctionTable(IntPtr objectAddress, int numberOfMethods)
        {
            TableEntries = GetObjectVTableAddresses(objectAddress, numberOfMethods);
        }

        /// <summary>
        /// Retrieves an array of addresses for a given object's virtual function table, with
        /// the assumption that the virtual function table pointer is the first parameter.
        /// </summary>
        /// <param name="objectAddress">
        ///     The memory address at which the object is stored.
        ///     The function will assume that the first entry is a pointer to the virtual function
        ///     table, as standard with C++ code.
        /// </param>
        /// <param name="numberOfMethods">
        ///     The number of methods contained in the virtual function table.
        ///     For enumerables, you may obtain this value as such: Enum.GetNames(typeof(MyEnum)).Length; where
        ///     MyEnum is the name of your enumerable.
        /// </param>
        /// <returns>The addresses and values of each of the virtual function table pointers.</returns>
        public static List<TableEntry> GetObjectVTableAddresses(IntPtr objectAddress, int numberOfMethods)
        {
            // Stores the addresses of the virtual function table.
            IntPtr virtualFunctionTableAddress = Bindings.TargetProcess.ReadMemoryExternal<IntPtr>(objectAddress);

            // Return addresses for vTable
            return GetAddresses(virtualFunctionTableAddress, numberOfMethods);
        }


        /// <summary>
        /// Retrieves an array of addresses for a given supplied virtual function table.
        /// </summary>
        /// <param name="tablePointer">The memory address of the first item/pointer to the virtual function table.</param>
        /// <param name="numberOfMethods">The number of methods contained in the virtual function table.</param>
        /// <returns>The addresses of all of the individual methods of the virtual function table.</returns>
        public static List<TableEntry> GetAddresses(IntPtr tablePointer, int numberOfMethods)
        {
            // Stores the addresses of the virtual function table.
            List<TableEntry> tablePointers = new List<TableEntry>();

            // Append the table pointers onto the tablePointers list.
            // Using the size of the IntPtr allows for both x64 and x86 support.
            for (int i = 0; i < numberOfMethods; i++)
            {
                // The address of the Virtual Function Table entry.
                IntPtr targetAddress = tablePointer + (IntPtr.Size * i);

                // Instantiate the table address and add to the pointers.
                tablePointers.Add(new TableEntry {
                    EntryAddress = targetAddress,
                    FunctionPointer = Bindings.TargetProcess.ReadMemoryExternal<IntPtr>(targetAddress)
                });
            }

            return tablePointers;
        }
    }
}
