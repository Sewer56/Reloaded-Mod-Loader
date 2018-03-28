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

using Reloaded.Networking;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using MessageStruct = Reloaded.Networking.Message.MessageStruct;

namespace Reloaded.Assembler
{
    /// <summary>
    /// The Assembler class is responsible for using the Reloaded Mod Loader Assembler Server (ReloadedAssembler)
    /// which wraps around FASM.NET.
    /// </summary>
    public static class Assembler
    {
        /// <summary>
        /// Used for communication with the external process.
        /// </summary>
        private static Client assemblerClient;

        /// <summary>
        /// Reloaded mod loader stuff reserve ports in the 1337X space.
        /// Port number of the assembler will be stored in %temp% in a path specified in the variable below.
        /// 
        /// Defaults:
        /// 13370 is taken by Reloaded Mod Loader Server
        /// 13380 is the port for the networked assembler.
        /// </summary>
        private static int SERVER_PORT = 13380;

        /// <summary>
        /// Stores the port # used by the Mod Loader Assembler Server, in the rare case
        /// that creating a server on the default port fails.
        /// </summary>
        private static readonly string MOD_LOADER_ASSEMBLER_PORT = Path.GetTempPath() + "\\Reloaded-Assembler-Port.txt";

        /// <summary>
        /// ABSOLUTELY DO NOT CHANGE THIS STRING
        /// libReloaded EXPECTS THIS STRING AND WILL KEEP IGNORE SERVER UNTIL
        /// THIS STRING IS RETURNED. THIS IDENTIFIES THE ASSEMBLER.
        /// </summary>
        const string RELOADED_CHECK_MESSAGE = "Reloaded Assembler";

        /// <summary>
        /// Defines the assembler server message types.
        /// One for assembly, other for reporting version.
        /// </summary>
        enum MessageTypes
        {
            /// <summary>
            /// Returns a string to confirm that the end is the assembler server.
            /// </summary>
            reportAssembler = 0x0,

            /// <summary>
            /// Assembles a set of ASM instructions.
            /// Returns the ASM instructions as a list of bytes.
            /// </summary>
            assemble = 0x1
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
        /// </summary>
        /// <param name="mnemonics">The successfully assembled X86, X64 or other compatible mnemonics.</param>
        /// <returns>0x90 (nop) if the assembly operation fails, else the successfully assembled bytes.</returns>
        public static byte[] Assemble(string[] mnemonics)
        {
            // Build Message to Assemble Mnemonics.
            MessageStruct assemblyRequest = new MessageStruct();
            assemblyRequest.MessageType = (ushort)MessageTypes.assemble;
            assemblyRequest.Data = SerializeObject(mnemonics);

            // Request & Retrieve | MessageStruct Data
            MessageStruct response = assemblerClient.ClientSocket.SendData(assemblyRequest, true);
            return response.Data;
        }

        /// <summary>
        /// Connects to the mod loader assembler server.
        /// </summary>
        static void ConnectToServer()
        {
            // Get port from file.
            if (File.Exists(MOD_LOADER_ASSEMBLER_PORT))
            {
                SERVER_PORT = Convert.ToInt32(File.ReadAllText(MOD_LOADER_ASSEMBLER_PORT));
            }

            // Is server not running?
            if (Process.GetProcessesByName("ReloadedAssembler").Length < 1)
            {
                // Start the assembler server.
                if (File.Exists("ReloadedAssembler.exe")) { Process.Start("ReloadedAssembler.exe"); }
                else
                {
                    // Search recursively in all subdirectories.
                    string[] files = Directory.GetFiles(Environment.CurrentDirectory, "ReloadedAssembler.exe", SearchOption.AllDirectories);

                    // Start first match.
                    if (files.Length > 0) { Process.Start(files[0]); }
                }
                
                // Connect to our server.
                ConnectToServer();
            }
            // Else try to find/connect to the server if already running.
            else
            {
                // Create the client instance.
                assemblerClient = new Client(IPAddress.Loopback, SERVER_PORT);

                // Try to connect.
                bool isConnected = assemblerClient.StartClient();

                // Check below if connected client is our assembler.
                if (isConnected)
                {
                    // If it is our assembler, return.
                    if (CheckAssembler()) { return; }

                    // Shutdown and try another port.
                    TrySecondPort();

                }
                else
                {
                    TrySecondPort();
                }
            }
        }

        /// <summary>
        /// Attempts to establish connection with the Assembler Server through 
        /// a port 1 digit higher than the current port.
        /// </summary>
        static void TrySecondPort()
        {
            // Shutdown.
            assemblerClient.ShutdownClient();

            // Try another port.
            assemblerClient = new Client(IPAddress.Loopback, SERVER_PORT + 1);
            bool isConnected2 = assemblerClient.StartClient();

            if (isConnected2)
            {
                // Check if it is our assembler.
                if (CheckAssembler()) { return; }
            }

            // Did not connect
            MessageBox.Show("Unable to connect/find the Mod Loader FASM Assembler Server\n" +
                            "If you see this message, contact the developer :P");
        }

        /// <summary>
        /// Checks if the connected socket on the other end is our Reloaded Assembler
        /// by sending a 0x00 message type and expecting a string.
        /// </summary>
        static bool CheckAssembler()
        {
            // Check Data
            MessageStruct checkAssembler = new MessageStruct();
            checkAssembler.MessageType = (ushort)MessageTypes.reportAssembler;
            checkAssembler.Data = new byte[1] { 0x00 };

            // Send check to server
            MessageStruct response = assemblerClient.ClientSocket.SendData(checkAssembler, true);

            // Check returned string.
            string checkString = Encoding.ASCII.GetString(response.Data);

            // Check the identification string for our assembler.
            if (checkString == RELOADED_CHECK_MESSAGE) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Serializes an object, or in our case a string array of X86, X64 
        /// Mnemonics such that they may be transmitted to the loader server.
        /// </summary>
        /// <param name="serializeObject">
        ///     Your x86 assembler instructions to be assembled. 
        ///     Rule of thumb: Test your ASM in FASM outside of mod loader mods first for successful compilation.
        /// </param>
        static byte[] SerializeObject(object serializeObject)
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
