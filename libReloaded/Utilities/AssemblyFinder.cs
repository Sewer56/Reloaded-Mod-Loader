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

namespace Reloaded.Utilities
{
    /// <summary>
    /// You know how when you inject a DLL into a process it will fail to find any of the libraries that 
    /// go along with it? I do too, worry not, this will help you.
    /// </summary>
    public static class AssemblyFinder
    {
        /// <summary>
        /// Finds and retrieves an Assembly/Module/DLL from the libraries folder in the case it is not
        /// yet loaded or the mod fails to find the assembly.
        /// </summary>
        public static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            // Get the path to Reloaded-Libraries
            string modLoaderLibraryPath = LoaderPaths.GetModLoaderLibraryDirectory();

            // Get the path to the mod loader libraries folder.
            string localLibraryFolder = LoaderPaths.GetModDirectory() + "\\Libraries\\";

            // Append the assembly name.
            string assemblyName = new AssemblyName(args.Name).Name;
            modLoaderLibraryPath += assemblyName + ".dll";
            localLibraryFolder += assemblyName + ".dll";

            // Store Assembly Object
            Assembly assembly;

            // Check if the library is present in a static compile.
            if (File.Exists(localLibraryFolder))
                assembly = Assembly.LoadFrom(localLibraryFolder);

            // Else load it from Reloaded-Libraries. (or not, doesn't matter, program will crash anyway if it doesn't exist)
            else if (File.Exists(localLibraryFolder))
                assembly = Assembly.LoadFrom(modLoaderLibraryPath);

            else
            {
                // Find the first matched file.
                string file = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), assemblyName + ".dll", SearchOption.AllDirectories)[0];

                // Load our loaded assembly
                assembly = Assembly.LoadFrom(file);
            }


            // Return Assembly
            return assembly;
        }
    }
}
