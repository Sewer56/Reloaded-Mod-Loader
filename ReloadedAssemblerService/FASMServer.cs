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
using System.Text;
using Binarysharp.Assemblers.Fasm;
using Reloaded.Networking;
using Reloaded.Networking.Sockets;

namespace ReloadedAssembler
{
    /// <summary>
    /// Provides an implementation of a networked flat assembler server.
    /// </summary>
    public class FasmServer
    {
        /// <summary>
        /// Server instance used for accepting incoming assembly requests.
        /// </summary>
        Host _server;

        /// <summary>
        /// Reloaded Mod Loader instances reserve ports in the 1337X space.
        /// 13370 is taken by Reloaded Mod Loader Server
        /// 13380 is the port for this program
        /// </summary>
        int _serverPort = 13380;

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
        /// Initializes the FASM Assembler Server.
        /// </summary>
        public FasmServer()
        {
            StartServer();

            // Write to temp directory.
            File.WriteAllText(ModLoaderAssemblerPort, Convert.ToString(_serverPort));

            // Register ReceiveMessage to handle data sent by client.
            _server.ProcessBytesMethods += ReceiveMessage;
        }

        /// <summary>
        /// Starts a FASM Assembler Server, incrementing Port by 1 if starting fails.
        /// </summary>
        private void StartServer()
        {
            try
            {
                // Start local server at 127.0.0.1 (accessible only locally)
                _server = new Host(IPAddress.Loopback, _serverPort);
                _server.StartServer();
            }
            catch (SocketException)
            {
                // Try on next port.
                _serverPort += 1;
                StartServer();
            }
        }

        /// <summary>
        /// Handles the individual assembly requests sent by the client.
        /// </summary>
        /// <param name="messageStruct">Message received from the client containing the assembly details</param>
        /// <param name="socket">The websocket socket used to send back the assembled bytes to the client.</param>
        private void ReceiveMessage(Message.MessageStruct messageStruct, ReloadedSocket socket)
        {
            switch ((MessageTypes)messageStruct.MessageType)
            {
                case MessageTypes.Assemble:             Assemble(DeserializeX86Mnemonics(messageStruct.Data), socket);  break;
                case MessageTypes.ReportAssembler:      Report(socket);                                                 break;
            }
        }

        /// <summary>
        /// Replies the name of the assembler back to the client.
        /// </summary>
        /// <param name="clientSocket">The socket to which the "ok" message should be sent.</param>
        private static void Report(ReloadedSocket clientSocket)
        {
            // Send back empty message struct.
            Message.MessageStruct messageStruct = new Message.MessageStruct
            {
                MessageType = (ushort)MessageTypes.ReportAssembler,

                // ABSOLUTELY DO NOT CHANGE THIS STRING
                // libReloaded EXPECTS THIS STRING AND WILL IGNORE SERVER UNTIL
                // THIS STRING IS RETURNED. THIS IDENTIFIES THE ASSEMBLER.
                Data = Encoding.ASCII.GetBytes(ReloadedCheckMessage)
            };

            clientSocket.SendData(messageStruct, false);
        }

        /// <summary>
        /// Assembles the received request and sends back information to the client.
        /// </summary>
        /// <param name="mnemonics">The assembly code to be assembled.</param>
        /// <param name="clientSocket">The socket used for communication with the client. which sent the request.</param>
        private static void Assemble(string[] mnemonics, ReloadedSocket clientSocket)
        {
            // Send back empty message struct
            Message.MessageStruct messageStruct = new Message.MessageStruct
            {
                // Client will likely ignore this anyway (but shouldn't).
                MessageType = ( ushort ) MessageTypes.Assemble
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
            clientSocket.SendData(messageStruct, false);
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
