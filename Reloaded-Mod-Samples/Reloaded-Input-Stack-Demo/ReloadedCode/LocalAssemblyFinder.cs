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
using System.IO;
using System.Reflection;

namespace Reloaded_Mod_Template.ReloadedCode
{
    /// <summary>
    /// You know how when you inject a DLL into a process it will fail to find any of the libraries that 
    /// go along with it? I do too, worry not, this will help you.
    /// </summary>
    public static class LocalAssemblyFinder
    {
        /// <summary>
        /// Finds and retrieves an Assembly/Module/DLL from the libraries folder in the case it is not
        /// yet loaded or the mod fails to find the assembly.
        /// </summary>
        public static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            // Get mod folder.
            string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Get the path to the mod's libraries folder.
            string localLibrary = modFolder + "\\Libraries\\";

            // Append the assembly name.
            localLibrary += new AssemblyName(args.Name).Name + ".dll";

            // Check if the library is present in a static compile.
            if (File.Exists(localLibrary))
                return Assembly.LoadFrom(localLibrary);

            // Else try to find it in the mod directory.
            else
            {
                // Obtain potential libraries.
                string[] libraries = Directory.GetFiles(modFolder, args.Name, SearchOption.AllDirectories);
                
                // If one is found, select first.
                if (libraries.Length > 0) { return Assembly.LoadFrom(libraries[0]); }
            }

            // Return Null
            return null;
        }
    }
}
