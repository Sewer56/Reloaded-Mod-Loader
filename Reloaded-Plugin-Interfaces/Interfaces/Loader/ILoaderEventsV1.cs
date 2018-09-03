
namespace Reloaded_Plugin_System.Interfaces.Loader
{
    /// <summary>
    /// The current interface allows you to execute your own code as Reloaded's Loader performs certain miscellaneous
    /// tasks.
    /// </summary>
    public interface ILoaderEventsV1
    {
        /// <summary>
        /// Allows you to get and/or change the individual arguments passed into Reloaded's
        /// loader at startup.
        /// </summary>
        string[] SetArguments(string[] args);

        /*
            As the various layouts of individual classes in libReloaded can change 
            over time; the individual parameters below; and in fact this event library
            are stripped of any dependencies.
         
            Any parameters which may require elements such as passing game or mod details
            are serialized.

            Consider using `JsonConvert.DeserializeObject<T>();` in order to get your instances
            of ModConfig or GameConfig back should you need them.
        */

        /// <summary>
        /// Allows you to set the DLL injection path for an individual modification.
        /// Return the currentPath parameter if you wish to make no changes.
        /// </summary>
        /// <param name="currentPath">Contains the current path which will be used for DLL Injection.</param>
        /// <param name="modConfig">Contains a JSON serialized copy of the ModConfig used.</param>
        /// <param name="gameConfig">Contains a JSON serialized copy of the GameConfig used.</param>
        /// <returns>The new DLL Injection path.</returns>
        string SetDllInjectionPath(string currentPath, string modConfig, string gameConfig);

        /// <summary>
        /// Allows an individual plugin to manually perform DLL Injection for a specific DLL.
        /// The passed in parameters are the defaults with which the DLL Injection would be otherwise performed.
        /// </summary>
        /// <param name="processId">You can use this Process ID to recreate a ReloadedProcess and DllInjector using libReloaded.</param>
        /// <param name="dllPath">The path to the DLL about to be injected.</param>
        /// <param name="dllMethodName">The name of the method to be by default executed.</param>
        /// <returns>Return TRUE if you have manually performed DLL injection. ELSE return FALSE.</returns>
        bool ManualDllInject(int processId, string dllPath, string dllMethodName);
    }
}
