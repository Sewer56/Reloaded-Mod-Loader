using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Permissions;

namespace Reloaded.Mod.Bootstrapper
{
    /// <summary>
    /// This class is simply a proxy for calling Init again from another AppDomain.
    /// </summary>
    public class InitProxy : MarshalByRefObject
    {
        public void Run(IntPtr portLocation)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                Init.ChildDomain_UnhandledException; // Pass exceptions to default AppDomain on crashes.
            Init.Initialize(portLocation);
        }
    }

    public class Init
    {
        /// <summary>
        /// Contains our child AppDomain used for init-ing mods in their own separate worlds.
        /// </summary>
        static AppDomain _childDomain;

        /// <summary>
        /// This file and/or Initializer.cs contains the DLL Template for Reloaded Mod Loader mods.
        /// If you are looking for user code, please see Program.cs
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        public static void Main(IntPtr portAddress)
        {
            // Retrieve Assemblies from the "Libraries" folder.
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyFinder.ResolveAppDomainAssembly;

            // Try restarting in another AppDomain if possible.
            try
            {
                // Give the new AppDomain full permissions.
                PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
                permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));

                // The ApplicationBase of the new domain should be the directory containing the current DLL.
                AppDomainSetup appDomainSetup = new AppDomainSetup()
                    {ApplicationBase = Path.GetDirectoryName(typeof(InitProxy).Assembly.Location)};
                _childDomain = AppDomain.CreateDomain("Reloaded", null, appDomainSetup, permissionSet);

                // Now make the new AppDomain load our code using our proxy.
                Type proxyType = typeof(InitProxy);
                dynamic initProxy = _childDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName)
                    .Unwrap(); // Our AssemblyResolve will pick the missing DLL out.
                initProxy.Run(portAddress);
            }
            catch (Exception ex)
            {
                Initialize(portAddress);
            }
        }

        /// <summary>
        /// Throws exceptions in the default AppDomain when/if the application crashes.
        /// VS may otherwise fail to get the stack trace.
        /// </summary>
        public static void ChildDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw (Exception) e.ExceptionObject;
        }

        /// <summary>
        /// This file and/or Initializer.cs contains the DLL Template for Reloaded Mod Loader mods.
        /// If you are looking for user code, please see Program.cs
        /// </summary>
        /// <param name="parameterAddress">Stores the memory location of the port used to connect back to Reloaded-Assembler.</param>
        public static void Initialize(IntPtr parameterAddress)
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyFinder.ResolveAssembly;
            InitializeInternal(parameterAddress);
        }

        /// <summary>
        /// This file contains the main entry code executed as part of the DLL template for Reloaded Mod Loader
        /// mods. It is very important that the entry method contains only AppDomain.CurrentDomain.AssemblyResolve
        /// due to otherwise possible problems with static initialization of Program.
        /// </summary>
        /// <param name="parameterAddress">Stores the memory location of the port.</param>
        public static void InitializeInternal(IntPtr parameterAddress)
        {
            // Initialize Client
            InitBindings();
            InitClient(parameterAddress);

            // Call Init
            try
            {
                Program.Init();
            }
            catch (Exception Ex)
            {
                Bindings.PrintError($"Failure in initializing Reloaded Mod | {Ex.Message} | {Ex.StackTrace}");
            }
        }
    }
}