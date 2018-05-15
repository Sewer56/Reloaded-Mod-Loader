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

namespace Reloaded.Process.X86Functions
{
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
            return parameters.Count(parameter => parameter.ParameterType != typeof(float) || parameter.ParameterType != typeof(double));
        }
    }
}
