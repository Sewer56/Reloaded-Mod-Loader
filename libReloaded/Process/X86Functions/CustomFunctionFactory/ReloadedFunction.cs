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
using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Process.X86Functions.CustomFunctionFactory
{
    /// <summary>
    /// Class which stores the function information for custom functions
    /// to be called or hooked upon.
    /// See <see cref="CallingConventions" /> for information on settings
    /// for common calling conventions.
    /// </summary>
    public class ReloadedFunction : Attribute
    {
        /// <summary>
        /// Specifies the registers in left to right parameter order to pass to the custom function to be called.
        /// </summary>
        public Register[] SourceRegisters { get; set; }

        /// <summary>
        /// Specifies the register to return the value from the funtion in (mov eax, source).
        /// This is typically eax.
        /// </summary>
        public Register ReturnRegister { get; set; }

        /// <summary>
        /// Defines the stack cleanup rule for the function.
        /// Callee: Stack pointer restored inside the game function we are executing).
        /// Caller: Stack pointer restored in our own wrapper function.
        /// </summary>
        public StackCleanup Cleanup { get; set; }

        /// <summary>
        /// Specifies the target X86 ISA register for a specific parameter.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Register
        {
            eax,
            ebx,
            ecx,
            edx,
            esi,
            edi,
            ebp,
            esp
        }
        
        /// <summary>
        /// Declares who performs the stack cleanup duty.
        /// </summary>
        public enum StackCleanup
        {
            Caller,
            Callee
        }
    }
}
