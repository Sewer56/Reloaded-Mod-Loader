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

namespace Reloaded.Mod.Bootstrapper
{
    /// <summary>
    /// You know how when you inject a DLL into a process it will fail to find any of the libraries that 
    /// go along with it? I do too, worry not, this will help you.
    /// </summary>
    public static class AssemblyFinder
    {
        /// <summary>
        /// Redirects requests to find main.dll to our own renamed versions (main32.dll and main64.dll) appropriately
        /// for the purpose of reloading this DLL in another appdomain.
        /// </summary>
        public static Assembly ResolveAppDomainAssembly(object sender, ResolveEventArgs args)
        {
            // Get the folder name of this directory containing our DLL.
            // And strip the requested DLL name as well as our own for comparison.
            string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            string thisDllName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            string targetDllName = new AssemblyName(args.Name).Name + ".dll";

            // If the DLL the program wants is main.dll, we replace it with our own main32/main64.dll we are executing from.
            // This is so that the new AppDomain loads the exact same DLL as our own and does not throw on casting a type.
            if (targetDllName == "main.dll")
                targetDllName = thisDllName;

            // Now we try to load the DLL :)
            if (File.Exists(modFolder + targetDllName))
                return Assembly.LoadFrom(modFolder + targetDllName);
            else
                return ResolveAssembly(sender, args);
        }

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
            string thisFolderLibrary = modFolder + "\\";
            string dllName = new AssemblyName(args.Name).Name + ".dll";

            // Append the assembly name.
            localLibrary += dllName;
            thisFolderLibrary += dllName;

            // Try loading from the current folder.
            if (File.Exists(thisFolderLibrary))
                return Assembly.LoadFrom(thisFolderLibrary);

            // Try loading from local library folder.
            if (File.Exists(localLibrary))
                return Assembly.LoadFrom(localLibrary);

            // Panic mode! Search all subdirectories!
            else
            {
                // Obtain potential libraries.
                string[] libraries = Directory.GetFiles(modFolder, args.Name, SearchOption.AllDirectories);

                // If one is found, select first.
                if (libraries.Length > 0)
                    return Assembly.LoadFrom(libraries[0]);
            }

            // Return Null
            return null;
        }
    }
}
