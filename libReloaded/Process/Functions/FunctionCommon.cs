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
using System.Linq;
using System.Reflection;
using Reloaded.Process.Functions.X64Functions;

namespace Reloaded.Process.Functions
{
    /// <summary>
    /// Conatins the common methods related to both X86 and X64 functions. 
    /// </summary>
    public static class FunctionCommon
    {
        /// <summary>
        /// Retrieves the number of parameters for a specific delegate Type, minus the floating point parameters.
        /// </summary>
        /// <param name="delegateType">The delegate type automatically containing the method "Invoke" with a set number of parameters.</param>
        /// <returns>Number of parameters for the supplied delegate type.</returns>
        public static int GetNumberofParameters(Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod("Invoke");
            return method != null ? method.GetParameters().Length : 0;
        }

        /// <summary>
        /// Retrieves the number of parameters for a specific delegate Type, minus the floating point parameters.
        /// </summary>
        /// <param name="delegateType">The delegate type automatically containing the method "Invoke" with a set number of parameters.</param>
        /// <returns>Number of parameters for the supplied delegate type.</returns>
        public static int GetNumberofParametersWithoutFloats(Type delegateType)
        {
            MethodInfo method = delegateType.GetMethod("Invoke");
            return method != null ? GetNonFloatParameters(method) : 0;
        }

        /// <summary>
        /// Retrieves the number of parameters for a specific delegate type minus the floating point parameters.
        /// </summary>
        /// <param name="methodInformation">Defines the individual information that describes a method to be called.</param>
        /// <returns>The number of non-float parameters.</returns>
        public static int GetNonFloatParameters(MethodInfo methodInformation)
        {
            // Retrieve all parameters.
            ParameterInfo[] parameters = methodInformation.GetParameters();

            // Check for non-float and return amount.
            return parameters.Count(parameter => parameter.ParameterType != typeof(Single) && parameter.ParameterType != typeof(Double));
        }


        /// <summary>
        /// Returns a register to be used for calling of the target function.
        /// Using this class may be necessary due to the fact that 
        /// </summary>
        /// <param name="reloadedFunction">
        ///     Structure containing the details of the actual function in question.
        ///     The source registers (parameters) are considered to be a blacklist of registers that cannot be used.
        ///     The return register is considered by default as the starting candidate.
        /// </param>
        public static X64ReloadedFunctionAttribute.Register GetCallRegister(X64ReloadedFunctionAttribute reloadedFunction)
        {
            // X86-64 doesn't support 64bit immediates in most instructions, meaning that we must, unfortunately
            // make use of a register to call a game function in question.
            // Here we find an unused register and delegate it to calling the function.

            // Default value | nonvolatile register
            X64ReloadedFunctionAttribute.Register callRegister = reloadedFunction.ReturnRegister;

            // Use return register if it's not a parameter (safe).
            if (!reloadedFunction.SourceRegisters.Contains(callRegister))
                return callRegister;

            // Use R11 (volatile) if it's not a parameter.
            if (!reloadedFunction.SourceRegisters.Contains(X64ReloadedFunctionAttribute.Register.r11))
                return X64ReloadedFunctionAttribute.Register.r11;

            // Use R10 (volatile) if it's not a parameter.
            if (!reloadedFunction.SourceRegisters.Contains(X64ReloadedFunctionAttribute.Register.r10))
                return X64ReloadedFunctionAttribute.Register.r10;

            // Otherwise brute force!
            foreach (X64ReloadedFunctionAttribute.Register foo in Enum.GetValues(
                typeof(X64ReloadedFunctionAttribute.Register)))
            {
                // Don't use a parameter register.
                if (reloadedFunction.SourceRegisters.Contains(foo))
                    continue;

                return foo;
            }

            return callRegister;
        }


    }
}
