/*
    [Reloaded] Mod Loader FASM Server
    A websocket server based off of libReloaded [Reloaded] main library,
    allowing x86/x64 FASM compatible mnemonics to be assembled.
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

/*
   Purpose:
   
   Allows x64, other architecture processes to assemble mnemonics via the
   FASM assembler as long as the machine supports running x86 code in any fashion.

   This is important for Reloaded mods that exist particularly in the target space
   of x64 processes, as the assembler cannot normally be used within that context
   and starting FASM.exe as a process for singular assembly jobs causes additional
   unsightly thread creation overhead.
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using Binarysharp.Assemblers.Fasm;
using libReloaded_Networking;
using LiteNetLib;
using LiteNetLib.Utils;

namespace ReloadedAssembler
{
    /// <summary>
    /// Provides an implementation of a networked flat assembler server.
    /// </summary>
    public class FasmServer : ServiceBase
    {
        /// <summary>
        /// Stores the actual Mod Loader Server host that is used to communicate alongside
        /// other peers in the network.
        /// </summary>
        public NetManager ReloadedServer;

        /// <summary>
        /// Listens to network events triggered as other clients interact with ther 
        /// Reloaded Mod Loader Server.
        /// </summary>
        public EventBasedNetListener ReloadedServerListener;

        /// <summary>
        /// Reloaded Mod Loader instances reserve ports in the 1337X space.
        /// 13370 is taken by Reloaded Mod Loader Server
        /// 13380 is the port for this program
        /// </summary>
        public int ServerPort { get; set; } = 13380;

        /// <summary>
        /// ABSOLUTELY DO NOT CHANGE THIS STRING
        /// libReloaded EXPECTS THIS STRING AND WILL KEEP IGNORE SERVER UNTIL
        /// THIS STRING IS RETURNED. THIS IDENTIFIES THE ASSEMBLER.
        /// </summary>
        const string ReloadedCheckMessage = "Reloaded Assembler";

        /// <summary>
        /// Stores the port # used by the Mod Loader Assembler Server, in the rare case
        /// that creating a server on the default port fails.
        /// </summary>
        private static readonly string ModLoaderAssemblerPort = Path.GetTempPath() + "\\Reloaded-Assembler-Port.txt";

        /// <summary>
        /// Defines the assembler server message types.
        /// One for assembly, other for reporting version.
        /// </summary>
        enum MessageTypes
        {
            /// <summary>
            /// Assembles a set of ASM instructions.
            /// Returns the ASM instructions as a list of bytes.
            /// </summary>
            Assemble = 0x1
        }

        /// <summary>
        /// Initializes the FASM Assembler Server.
        /// </summary>
        public FasmServer()
        {
            StartServer();

            // Write to temp directory.
            File.WriteAllText(ModLoaderAssemblerPort, Convert.ToString(ServerPort));
        }

        /// <summary>
        /// Starts a FASM Assembler Server, incrementing Port by 1 if starting fails.
        /// </summary>
        private void StartServer()
        {
            try
            {
                // Create new server instance.
                ReloadedServerListener = new EventBasedNetListener();
                ReloadedServer = new NetManager(ReloadedServerListener, 65535, ReloadedCheckMessage);

                ReloadedServer.DisconnectTimeout = 10000;
                #if DEBUG
                ReloadedServer.DisconnectTimeout = Int64.MaxValue;
                #endif

                ReloadedServer.ReconnectDelay = 100;
                ReloadedServer.UnsyncedEvents = true;
                ReloadedServer.MaxConnectAttempts = 10;
                ReloadedServer.UpdateTime = 1;

                // Send received data to the message handler
                ReloadedServerListener.NetworkReceiveEvent += ReceiveMessage;

                // Start Server Internally
                ReloadedServer.Start(IPAddress.Loopback, IPAddress.IPv6Loopback, ServerPort);
            }
            catch (SocketException)
            {
                // Try on next port.
                ServerPort += 1;
                StartServer();
            }
        }

        /// <summary>
        /// Handles the individual assembly requests sent by the client.
        /// </summary>
        /// <param name="netPeer">Contains the peer client that sent the message to the server. A wrapper around a WebSocket Client.</param>
        /// <param name="reader">Contains the information sent by the client to the server.</param>
        private void ReceiveMessage(NetPeer netPeer, NetDataReader reader)
        {
            // Parse received message.
            Message messageStruct = Message.GetMessage(reader.GetBytesWithLength());

            switch ((MessageTypes)messageStruct.MessageType)
            {
                case MessageTypes.Assemble:             Assemble(DeserializeX86Mnemonics(messageStruct.Data), netPeer);  break;
            }
        }

        /// <summary>
        /// Assembles the received request and sends back information to the client.
        /// </summary>
        /// <param name="mnemonics">The assembly code to be assembled.</param>
        /// <param name="netPeer">Contains the peer client that sent the message to the server. A wrapper around a WebSocket Client.</param>
        private static void Assemble(string[] mnemonics, NetPeer netPeer)
        {
            // Send back empty message struct
            Message messageStruct = new Message()
            {
                // Client will likely ignore this anyway (but shouldn't).
                MessageType = (ushort) MessageTypes.Assemble
            };

            // Try Assembly
            // Assemble the bytes
            try
            { messageStruct.Data = FasmNet.Assemble(mnemonics); }

            // Failed to Assemble
            // Return nop on failure.
            catch
            { messageStruct.Data = new byte[1] { 0x90 }; }

            // Return back.
            netPeer.Send(messageStruct.GetBytes(), SendOptions.ReliableOrdered);
            netPeer.Flush();
        }

        /// <summary>
        /// Deserializes a string array of X86 Mnemonics that was received from another machine
        /// for the assembly procedure to take place.
        /// </summary>
        /// <param name="mnemonics">
        ///     x86 assembler instructions received serialized using BinaryFormatter.
        /// </param>
        private static string[] DeserializeX86Mnemonics(byte[] mnemonics)
        {
            // Initialize MemStream & BinaryFormatter
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream mnemonicStream = new MemoryStream(mnemonics);

            // Return deserialized.
            return (string[])binaryFormatter.Deserialize(mnemonicStream);
        }
    }
}
