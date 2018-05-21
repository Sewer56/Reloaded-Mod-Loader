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

namespace Reloaded.Process.Functions.X64Functions
{
    /// <summary>
    /// This class provides information on various commonly seen calling conventions and information on how
    /// to call functions utilising these calling conventions from Reloaded code.
    /// </summary>
    public enum X64CallingConventions
    {
        /// <summary>
        /// Attribute [UnmanagedFunctionPointer(CallingConvention.Cdecl)] on Delegate
        /// Usage: FunctionWrapper class.
        ///
        /// Parameters are passed in the order of RCX, RDX, R8, R9 registers, left to right.
        /// Remaining parameters are passed right to left onto the function pushing onto the stack.
        ///
        /// Caller's responsibility to create allocate 32 bytes of "shadow space" on the stack, 
        /// but worry not, that's handled for you already.
        /// 
        /// Calling function pops its own arguments from the stack, if necessary and uses the "shadow space"
        /// as storage for the individual parameters to free registers if necessary.
        ///
        /// (The calling function must manually restore the stack to previous state)
        /// 
        /// ReloadedFunction Attribute:
        ///     TargetRegisters:    RCX, RDX, R8, R9
        ///     ReturnRegister:     RAX    
        ///     Cleanup:            Caller
        /// </summary>
        Microsoft,

        /// <summary>
        /// Attribute [UnmanagedFunctionPointer(CallingConvention.Cdecl)] on Delegate
        /// Usage: FunctionWrapper class.
        ///
        /// Parameters are passed in the order of RDI, RSI, RDX, RCX, R8, R9 registers, left to right.
        /// Remaining parameters are passed right to left onto the function pushing onto the stack.
        /// 
        /// No necessity of "shadow space" is provided, though Reloaded will provide it anyway for
        /// compatibility with custom conventions.
        /// 
        /// ReloadedFunction Attribute:
        ///     TargetRegisters:    RDI, RSI, RDX, RCX, R8, R9 
        ///     ReturnRegister:     EAX    
        ///     Cleanup:            Callee
        /// </summary>
        SystemV,

        /// <summary>
        /// Placeholder for custom, compiler optimized calling conventions which don't 
        /// follow any particular standard.
        /// 
        /// Attribute [UnmanagedFunctionPointer(CallingConvention.Cdecl)] on Delegate
        /// Usage: FunctionWrapper class.
        /// 
        /// ReloadedFunction Attribute:
        ///     TargetRegisters:    Depends on Function
        ///     ReturnRegister:     Depends on Function
        ///     Cleanup:            Depends on Function
        /// </summary>
        Custom,
    }
}
