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
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using libReloaded_Networking;
using LiteNetLib;
using LiteNetLib.Utils;
using Reloaded.Utilities;

namespace Reloaded.Assembler
{
    /// <summary>
    /// The Assembler class is responsible for using the Reloaded Mod Loader Assembler Server (ReloadedAssembler)
    /// which wraps around FASM.NET.
    /// </summary>
    public static class Assembler
    {
        /// <summary>
        /// ABSOLUTELY DO NOT CHANGE THIS STRING
        /// libReloaded EXPECTS THIS STRING AND WILL KEEP IGNORE SERVER UNTIL
        /// THIS STRING IS RETURNED. THIS IDENTIFIES THE ASSEMBLER.
        /// </summary>
        private const string ReloadedCheckMessage = "Reloaded Assembler";

        /// <summary>
        /// The client of the Mod Loader Server. 
        /// Can be used for communication with the external local server (hosted in the CMD window).
        /// </summary>
        public static NetManager ReloadedClient;

        /// <summary>
        /// Listens to network events triggered as other clients interact with the
        /// Reloaded Mod Loader Server.
        /// </summary>
        private static EventBasedNetListener _reloadedClientListener;

        /// <summary>
        /// Reloaded mod loader stuff reserve ports in the 1337X space.
        /// Port number of the assembler will be stored in %temp% in a path specified in the variable below.
        /// Defaults:
        /// 13370 is taken by Reloaded Mod Loader Server
        /// 13380 is the port for the networked assembler.
        /// </summary>
        private static int _serverPort = 13380;

        /// <summary>
        /// Stores the port # used by the Mod Loader Assembler Server, in the rare case
        /// that creating a server on the default port fails.
        /// </summary>
        private static readonly string ModLoaderAssemblerPort = Path.GetTempPath() + "\\Reloaded-Assembler-Port.txt";

        /// <summary>
        /// Defines the assembler server message types.
        /// One for assembly, other for reporting version.
        /// </summary>
        public enum MessageTypes
        {
            /// <summary>
            /// Returns a string to confirm that the end is the assembler server.
            /// </summary>
            ReportAssembler = 0x0,

            /// <summary>
            /// Assembles a set of ASM instructions.
            /// Returns the ASM instructions as a list of bytes.
            /// </summary>
            Assemble = 0x1
        }

        /// <summary>
        /// Automatically Initialize the Reloaded Assembler
        /// </summary>
        static Assembler()
        {
            ConnectToServer();
        }

        /// <summary>
        /// Assembles a supplied set of FASM assembler compatible mnemonics
        /// (X86, X64) and returns the result back to the user.
        /// 
        /// NOTE: THIS METHOD IS NOT THREAD SAFE, DO NOT CALL ASYNCHRONOUSLY.
        /// </summary>
        /// <param name="mnemonics">The successfully assembled X86, X64 or other compatible mnemonics.</param>
        /// <returns>0x90 (nop) if the assembly operation fails, else the successfully assembled bytes.</returns>
        public static byte[] Assemble(string[] mnemonics)
        {
            // Connect if not connected (we may be disconnected after application DRM self-kill, timeout using debugger, etc.)
            while (ReloadedClient.PeersCount < 1)
            {
                ConnectToServer();
            }   

            // Build Message to Assemble Mnemonics.
            Message assemblyRequest = new Message
            {
                MessageType = (ushort)MessageTypes.Assemble,
                Data = SerializeObject(mnemonics)
            };

            // Request & Retrieve MessageStruct Data
            byte[] messageBytes = null;

            // Local function to assign the message Bytes.
            void GetMessageBytes(NetPeer peer, NetDataReader reader)
            {
                messageBytes = Message.GetMessage(reader.GetBytesWithLength()).Data;
            }

            // Add event for receiving on network.
            _reloadedClientListener.NetworkReceiveEvent += GetMessageBytes;
            ReloadedClient.GetFirstPeer().Send(assemblyRequest.GetBytes(), SendOptions.ReliableOrdered);

            // Wait for response and return data.
            while (messageBytes == null)
            { ReloadedClient.PollEvents(); }

            // Unsubscribe delegate.
            _reloadedClientListener.NetworkReceiveEvent -= GetMessageBytes;

            return messageBytes;
        }

        /// <summary>
        /// Connects to the mod loader assembler server.
        /// </summary>
        private static void ConnectToServer()
        {
            // Get port from file.
            if (File.Exists(ModLoaderAssemblerPort))
            {
                _serverPort = Convert.ToInt32(File.ReadAllText(ModLoaderAssemblerPort));
            }

            // Is server not running?
            if (System.Diagnostics.Process.GetProcessesByName("Reloaded-Assembler").Length < 1)
            {
                // Get path to assembler.
                string currentExecutingFolder = Assembly.GetExecutingAssembly().Location;

                // Check if library is packed, if it is (no location), get current directory.
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (currentExecutingFolder == "") { currentExecutingFolder = Path.GetTempPath(); }
                else { currentExecutingFolder = Path.GetDirectoryName(currentExecutingFolder); }

                // Create and define path in subdirectory
                string assemblerDirectory = Path.GetTempPath();
                string reloadedPath = assemblerDirectory + "//" + "Reloaded-Assembler.exe";
                Directory.CreateDirectory(assemblerDirectory);

                // Extract the executable and run it.
                ExtractFodyCosturaFile.ExtractResource("Reloaded-Assembler.exe", assemblerDirectory);

                // Start the assembler.
                System.Diagnostics.Process.Start(reloadedPath);

                // Connect to our server.
                ConnectToServer();
            }
            // Else try to find/connect to the server if already running.
            else
            {
                // Setup Local Server Client
                _reloadedClientListener = new EventBasedNetListener();
                ReloadedClient = new NetManager(_reloadedClientListener, 10, ReloadedCheckMessage);
                ReloadedClient.Start(IPAddress.Loopback, IPAddress.IPv6Loopback, 0);
                ReloadedClient.ReconnectDelay = 50;
                ReloadedClient.MaxConnectAttempts = 5;
                ReloadedClient.DisconnectTimeout = Int64.MaxValue;
                ReloadedClient.Connect(IPAddress.Loopback.ToString(), _serverPort);

                // Check below if connected client is our assembler.
                if (ReloadedClient.PeersCount < 1)
                {
                    // Shutdown and try another port.
                    TrySecondPort();
                }
                else
                {
                    // Wait for full connection with peer.
                    NetPeer[] peers = ReloadedClient.GetPeers();
                    foreach (var peer in peers)
                    {
                        while (peer.ConnectionState == ConnectionState.InProgress)
                        { Thread.Sleep(1); }
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to establish connection with the Assembler Server through 
        /// a port 1 digit higher than the current port.
        /// </summary>
        private static void TrySecondPort()
        {
            // Increment the server port.
            _serverPort++;

            // Call the startup method.
            // Tries 19 ports in succession.
            if (_serverPort < 13399)
            { ConnectToServer(); }

            // Did not connect
            Bindings.PrintError("Unable to connect/find the Mod Loader FASM Assembler Server\n" +
                            "If you see this message, contact the developer :P");
        }

        /// <summary>
        /// Serializes an object, or in our case a string array of X86, X64 
        /// Mnemonics such that they may be transmitted to the loader server.
        /// </summary>
        /// <param name="serializeObject">
        ///     Your x86 assembler instructions to be assembled. 
        ///     Rule of thumb: Test your ASM in FASM outside of mod loader mods first for successful compilation.
        /// </param>
        private static byte[] SerializeObject(object serializeObject)
        {
            // Initialize MemStream & BinaryFormatter
            MemoryStream mnemonicStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            // Serialize array at once
            binaryFormatter.Serialize(mnemonicStream, serializeObject);

            // Return Serialized
            return mnemonicStream.ToArray();
        }

    }
}
