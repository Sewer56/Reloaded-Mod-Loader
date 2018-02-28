using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.GameProcess;
using Reloaded_Mod_Template.Reloaded_Code;

namespace Reloaded_Mod_Template
{
    public class Initializer
    {
        /// <summary>
        /// This file contains the DLL Template for Reloaded Mod Loader mods.
        /// If you are looking for user code, please see Program.cs
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        [DllExport]
        static void Main(IntPtr portAddress)
        {
            // Retrieve Assemblies from the "Libraries" folder.
            AppDomain.CurrentDomain.AssemblyResolve += LocalAssemblyFinder.ResolveAssembly;

            // Initialize
            Init(portAddress);

            // Call Init
            Program.Init();
        }

        /// <summary>
        /// This is here because of the logic of the CLR.
        /// If the JIT tries to compile the method to execute, it will fail to find libreloaded,
        /// before we even set the assembly resolution path with AppDomain.CurrentDomain.AssemblyResolve.
        /// </summary>
        /// <param name="portAddress">Stores the memory location of the port.</param>
        static void Init(IntPtr portAddress)
        {
            // Set the game process.
            Program.GameProcess = ReloadedProcess.GetCurrentProcess();

            // Setup Local Server Client
            Client.serverClient = new Reloaded.Networking.Client(IPAddress.Loopback, Program.GameProcess.ReadMemorySafe<int>((IntPtr)portAddress));
            Client.serverClient.StartClient();
        }
    }
}
