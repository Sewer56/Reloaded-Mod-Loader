/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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

namespace Reloaded_Loader.Miscellaneous
{
    public static class AssemblyFinder
    {
        /// <summary>
        /// Finds and retrieves an Assembly/Module/DLL from the libraries folder in the case it is not
        /// yet loaded or the mod fails to find the assembly.
        /// </summary>
        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Get the path to the mod loader libraries folder.
            string localLibraryFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Reloaded-Libraries\\";

            // Append the assembly name.
            string assemblyName = new AssemblyName(args.Name).Name;
            localLibraryFolder += assemblyName + ".dll";

            // Store Assembly Object
            Assembly assembly;

            // Check if the library is present in a static compile.
            if (File.Exists(localLibraryFolder))
                assembly = Assembly.LoadFrom(localLibraryFolder);

            // Brute-force Search
            else
            {
                // Find the first matched file.
                string file = Directory.GetFiles(Assembly.GetEntryAssembly().Location, assemblyName + ".dll", SearchOption.AllDirectories)[0];

                // Load our loaded assembly
                assembly = Assembly.LoadFrom(file);
            }

            // Return Assembly
            return assembly;
        }
    }
}
