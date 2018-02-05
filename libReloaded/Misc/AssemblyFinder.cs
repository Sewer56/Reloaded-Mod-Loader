using System;
using System.IO;
using System.Reflection;

namespace Reloaded.Misc
{
    /// <summary>
    /// You know how when you inject a DLL into a process it will fail to find any of the libraries that 
    /// go along with it? I do too, worry not, this will help you.
    /// </summary>
    public class AssemblyFinder
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
            modLoaderLibraryPath += new AssemblyName(args.Name).Name + ".dll";
            localLibraryFolder += new AssemblyName(args.Name).Name + ".dll";

            // Store Assembly Object
            Assembly assembly;

            // Check if the library is present in a static compile.
            if (File.Exists(localLibraryFolder))  { assembly = Assembly.LoadFrom(localLibraryFolder); }
            
            // Else load it from Reloaded-Libraries. (or not, doesn't matter, program will crash anyway if it doesn't exist)
            else { assembly = Assembly.LoadFrom(modLoaderLibraryPath); }

            // Return Assembly
            return assembly;
        }
    }
}
