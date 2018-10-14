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
using Reloaded.Process.Functions.X64Functions;
using Reloaded.Process.Functions.X64Hooking;
using Reloaded.Process.Functions.X86Functions;
using Reloaded.Process.Functions.X86Hooking;
namespace Reloaded.Process.Functions.Utilities
{
    /// <summary>
    /// Provides individual extension methods for virtual function table functions.
    /// </summary>
    public static class VirtualFunctionTableExtensions
    {
        /// <summary>
        /// Generates a wrapper function for an individual virtual function table entry
        /// in a virtual function table.
        /// </summary>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute/X64 Reloaded Function Attribute that defines the individual function properties.</typeparam>
        /// <param name="tableEntry">The individual Virtual function table entry to create a wrapper function for.</param>
        /// <returns>Delegate to assign back to ReloadedFunction marked game function.</returns>
        public static TFunction CreateWrapperFunction<TFunction>(this VirtualFunctionTable.TableEntry tableEntry)
        {
            // Create X86/X64 wrapper conditionally.
            if (IntPtr.Size == 4)
                return FunctionWrapper.CreateWrapperFunction<TFunction>((long)tableEntry.FunctionPointer);
            if (IntPtr.Size == 8)
                return X64FunctionWrapper.CreateWrapperFunction<TFunction>((long) tableEntry.FunctionPointer);

            // Null case
            return default(TFunction);
        }

        /// <summary>
        /// Hooks an individual virtual function table entry in a virtual function table.
        /// </summary>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute/X64 Reloaded Function Attribute that defines the individual function properties.</typeparam>
        /// <param name="tableEntry">The individual Virtual function table entry to hook.</param>
        /// <param name="delegateType">The delegate type of your own individual function, such as an instance of the delegate.</param>
        /// <returns>Delegate to assign back to ReloadedFunction marked game function.</returns>
        public static FunctionHook<TFunction> CreateFunctionHook86<TFunction>(this VirtualFunctionTable.TableEntry tableEntry, TFunction delegateType)
        {
            return FunctionHook<TFunction>.Create((long)tableEntry.FunctionPointer, delegateType);
        }

        /// <summary>
        /// Hooks an individual virtual function table entry in a virtual function table.
        /// </summary>
        /// <typeparam name="TFunction">Delegate type marked with complete ReloadedFunction Attribute/X64 Reloaded Function Attribute that defines the individual function properties.</typeparam>
        /// <param name="tableEntry">The individual Virtual function table entry to hook.</param>
        /// <param name="delegateType">The delegate type of your own individual function, such as an instance of the delegate.</param>
        /// <returns>Delegate to assign back to ReloadedFunction marked game function.</returns>
        public static X64FunctionHook<TFunction> CreateFunctionHook64<TFunction>(this VirtualFunctionTable.TableEntry tableEntry, TFunction delegateType)
        {
            return X64FunctionHook<TFunction>.Create((long)tableEntry.FunctionPointer, delegateType);
        }
    }
}
