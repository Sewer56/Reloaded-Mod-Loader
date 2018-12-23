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
using System.Linq;

namespace Reloaded.Process.Functions.X64Functions
{
    /// <summary>
    /// Class which stores the function information for custom functions
    /// to be called or hooked upon.
    /// See <see cref="X64CallingConventions" /> for information on settings
    /// for common calling conventions.
    /// </summary>
    public class X64ReloadedFunctionAttribute : Attribute
    {
        /// <summary>
        /// Specifies the registers in left to right parameter order to pass to the custom function to be called.
        /// </summary>
        public Register[] SourceRegisters { get; }

        /// <summary>
        /// Specifies the register to return the value from the funtion in (mov rax, source).
        /// This is typically rax.
        /// </summary>
        public Register ReturnRegister { get; }

        /// <summary>
        /// Defines whether the function to be called or hooked expects "Shadow Space".
        /// Shadow space allocates 32 bytes of memory onto the stack before calling the function, such that they
        /// may be used locally within the target function as storage.
        /// [Default] True: Microsoft X64 based calling conventions.
        /// False: SystemV-based calling conventions.
        /// </summary>
        public bool ShadowSpace { get; } = true;

        /// <summary>
        /// Specifies the target X86 ISA register for a specific parameter.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Register
        {
            rax,
            rbx,
            rcx,
            rdx,
            rsi,
            rdi,
            rbp,
            rsp,
            r8,
            r9,
            r10,
            r11,
            r12,
            r13,
            r14,
            r15
        }

        /// <summary>
        /// Initializes a ReloadedFunction with its default parameters supplied in the constructor.
        /// </summary>
        /// <param name="sourceRegisters">Specifies the registers in left to right parameter order to pass to the custom function to be called.</param>
        /// <param name="returnRegister">Specifies the register to return the value from the funtion in (mov rax, source). This is typically rax.</param>
        /// <param name="shadowSpace">
        ///     [Default = true] Defines whether the function to be called or hooked expects "Shadow Space".
        ///     Shadow space allocates 32 bytes of memory onto the stack before calling the function. See class definition for more details.
        /// </param>
        public X64ReloadedFunctionAttribute(Register[] sourceRegisters, Register returnRegister, bool shadowSpace)
        {
            SourceRegisters = sourceRegisters;
            ReturnRegister = returnRegister;
            ShadowSpace = shadowSpace;
        }

        /// <summary>
        /// Initializes a ReloadedFunction with its default parameters supplied in the constructor.
        /// </summary>
        /// <param name="sourceRegister">Specifies the registers for the parameter.</param>
        /// <param name="returnRegister">Specifies the register to return the value from the funtion in (mov rax, source). This is typically rax.</param>
        /// <param name="shadowSpace">
        ///     [Default = true] Defines whether the function to be called or hooked expects "Shadow Space".
        ///     Shadow space allocates 32 bytes of memory onto the stack before calling the function. See class definition for more details.
        /// </param>
        public X64ReloadedFunctionAttribute(Register sourceRegister, Register returnRegister, bool shadowSpace)
        {
            SourceRegisters = new[] { sourceRegister };
            ReturnRegister = returnRegister;
            ShadowSpace = shadowSpace;
        }

        /// <summary>
        /// Initializes the ReloadedFunction using a preset calling convention.
        /// </summary>
        /// <param name="callingConvention">
        ///     The calling convention preset to use for instantiating the ReloadedFunction.
        ///     Please remember to mark your function delegate as [UnmanagedFunctionPointer(CallingConvention.Cdecl)],
        ///     mark only the ReloadedFunction Attribute with the true calling convention.
        /// </param>
        public X64ReloadedFunctionAttribute(X64CallingConventions callingConvention)
        {
            switch (callingConvention)
            {
                case X64CallingConventions.Microsoft:
                    SourceRegisters = new [] { Register.rcx, Register.rdx, Register.r8, Register.r9 };
                    ReturnRegister = Register.rax;
                    ShadowSpace = true;
                    break;

                case X64CallingConventions.SystemV:
                    SourceRegisters = new [] { Register.rdi, Register.rsi, Register.rdx, Register.rcx, Register.r8, Register.r9 };
                    ReturnRegister = Register.rax;
                    ShadowSpace = false;
                    break;

                default:
                    throw new ArgumentException($"There is no preset for the specified calling convention {callingConvention.GetType().Name}");
            }
        }

        /// <summary>
        /// Checks whether an instance of <see cref="X64ReloadedFunctionAttribute"/> has the same logical meaning
        /// as the current instance of <see cref="X64ReloadedFunctionAttribute"/>.
        /// </summary>
        /// <param name="obj">The <see cref="X64ReloadedFunctionAttribute"/> to compare to the current attribute.</param>
        /// <returns>True if both of the ReloadedFunctions are logically equivalent.</returns>
        public override bool Equals(Object obj)
        {
            // Check for type.
            X64ReloadedFunctionAttribute functionAttribute = obj as X64ReloadedFunctionAttribute;

            // Return false if null
            if (functionAttribute == null) return false;
            
            // Check by value.
            return functionAttribute.ShadowSpace == ShadowSpace &&
                   functionAttribute.ReturnRegister == ReturnRegister &&
                   functionAttribute.SourceRegisters.SequenceEqual(SourceRegisters);
        }

        /// <summary>
        /// Retrieves the HashCode for the current object.
        /// </summary>
        /// <returns>The hashcode for the current object.</returns>
        public override int GetHashCode()
        {
            // Stores the initial value.
            int initialHash = 13;

            // Calculate hash.
            foreach (Register register in SourceRegisters)
            { initialHash = (initialHash * 7) + (int)register; }
            
            initialHash = (initialHash * 7) + (int)ReturnRegister;
            initialHash = (initialHash * 7) + ShadowSpace.GetHashCode();

            // Return
            return initialHash;
        }
    }
}
