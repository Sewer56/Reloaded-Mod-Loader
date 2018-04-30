using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using LiteNetLib;
using Reloaded;
using Reloaded.Process;
using Reloaded.Process.Memory;

namespace Reloaded_Mod_Template.ReloadedCode
{
    public class Initializer
    {
        /// <summary>
        /// This file contains the DLL Template for Reloaded Mod Loader mods.
        /// If you are looking for user code, please see Program.cs
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        [DllExport]
        public static void Main(IntPtr portAddress)
        {
            // Retrieve Assemblies from the "Libraries" folder.
            AppDomain.CurrentDomain.AssemblyResolve += LocalAssemblyFinder.ResolveAssembly;
            Initialize(portAddress);
        }

        /// <summary>
        /// This file contains the main entry code executed as part of the DLL template for Reloaded Mod Loader
        /// mods. It is very important that the entry method contains only AppDomain.CurrentDomain.AssemblyResolve
        /// due to otherwise possible problems with static initialization of Program.
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        public static void Initialize(IntPtr portAddress)
        {
            // Initialize Client
            InitClient(portAddress);
            InitBindings();

            // Call Init
            Program.Init();
        }

        /// <summary>
        /// This is here because of the logic of the CLR.
        /// If the JIT tries to compile the method to execute, it will fail to find libreloaded,
        /// before we even set the assembly resolution path with AppDomain.CurrentDomain.AssemblyResolve.
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        static void InitClient(IntPtr portAddress)
        {
            // Setup Local Server Client
            EventBasedNetListener reloadedClientListener = new EventBasedNetListener();
            Client.ReloadedClient = new NetManager(reloadedClientListener, Strings.Loader.ServerConnectKey);
            Client.ReloadedClient.Start(IPAddress.Loopback, IPAddress.IPv6Loopback, 0);
            Client.ReloadedClient.Connect(IPAddress.Loopback.ToString(), Program.GameProcess.ReadMemory<int>(portAddress));
        }

        /// <summary>
        /// Initializes the libReloaded bindings used for internal Reloaded Mod Loader functions
        /// such as printing to buffers, logging and other functions.
        /// </summary>
        static void InitBindings()
        {
            // Set local game process.
            Program.GameProcess = ReloadedProcess.GetCurrentProcess();
            Program.ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Program.ExecutingGameLocation = Environment.GetCommandLineArgs()[0];

            // Set up Reloaded Mod Loader bindings.
            Bindings.PrintText += Client.Print;
            Bindings.PrintError += Client.PrintError;
            Bindings.PrintInfo += Client.PrintInfo;
            Bindings.PrintWarning += Client.PrintWarning;
            Bindings.TargetProcess = Program.GameProcess;
        }
    }
}
