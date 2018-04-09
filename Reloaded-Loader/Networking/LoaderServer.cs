/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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

using System.Net;
using System.Net.Sockets;
using Reloaded.Networking;
using Reloaded.Networking.Sockets;
using Reloaded_Loader.Networking.LoaderServerFunctions;
using Reloaded_Loader.Terminal;
using static Reloaded.Networking.ModLoaderServer.MessageTypes;
using static Reloaded_Loader.Networking.LoaderServerFunctions.PrintToScreen;

namespace Reloaded_Loader.Networking
{
    /// <summary>
    /// Provides the implementation of the server functionality for Reloaded Mod Loader.
    /// Server functionality is used to provide communication between various modifications and
    /// the server itself.
    /// </summary>
    public static class LoaderServer
    {
        /// <summary>
        /// Wraps the WebSocket host for the Reloaded Mod Loader that is used to communicate
        /// to various different mod loader mods.
        /// </summary>
        public static Host ReloadedServer;

        /// <summary>
        /// Defines the server port to be used for hosting the Reloaded Server.
        /// If the server port is unavailable, Reloaded will attempt to use the
        /// port that is 1 above this one (to allow multiple game instances to run).
        /// </summary>
        public static int ServerPort = 13370;

        /// <summary>
        /// Sets up the Reloaded Mod Loader Server.
        /// </summary>
        public static void SetupServer()
        {
            try
            {
                // Create new server instance.
                ReloadedServer = new Host(IPAddress.Loopback, ServerPort);

                // Start Server Internally
                ReloadedServer.StartServer();

                // Redirect sent data towards certain method.
                ReloadedServer.ProcessBytesMethods += MessageHandler;

                // Print words of success
                ConsoleFunctions.PrintMessageWithTime("Local Server Successfully Started!", ConsoleFunctions.PrintInfoMessage);
            }
            // If the port is occupied, an exception will be thrown.
            // Try hosting the server on another port.
            catch (SocketException ex)
            {
                // Print Warning
                ConsoleFunctions.PrintMessageWithTime("Failed to create local host at port " + ServerPort + ". Attempting port " + (ServerPort + 1) + ".", ConsoleFunctions.PrintWarningMessage);
                ConsoleFunctions.PrintMessageWithTime("Original Message: " + ex.Message, ConsoleFunctions.PrintWarningMessage);

                // Increment the port number.
                ServerPort += 1;

                // Call self
                SetupServer();
            }
        }

        /// <summary>
        /// Handles the messages received from the individual client sockets consisting mainly of
        /// mods. Exposes various functionality features to mods.
        /// </summary>
        /// <param name="clientMessage">The message struct received from the individual mod loader server clients.</param>
        /// <param name="socket">The individual socket object to use for connection back with the mod loader clients.</param>
        private static void MessageHandler(Message.MessageStruct clientMessage, ReloadedSocket socket)
        {
            // Pass the relevant message.
            switch (clientMessage.MessageType)
            {
                // Regular Functions
                case (ushort)MessageType.Okay: ReplyOkay.ReplyOk(socket); break;

                // Text Functions
                case (ushort)MessageType.PrintText: Print(PrintMessageType.PrintText, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintInfo: Print(PrintMessageType.PrintInfo, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintWarning: Print(PrintMessageType.PrintWarning, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintError: Print(PrintMessageType.PrintError, clientMessage.Data, socket); break;

            }
        }
    }
}
