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

using System;
using System.Diagnostics;
using libReloaded_Networking;
using LiteNetLib;
using LiteNetLib.Utils;
using Reloaded;
using Reloaded_Loader.Terminal;
using static Reloaded_Loader.Networking.LoaderServerFunctions.PrintToScreen;
using static libReloaded_Networking.MessageTypes;
using Bindings = Reloaded.Bindings;

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
        /// Stores the actual Mod Loader Server host that is used to communicate alongside
        /// other peers in the network.
        /// </summary>
        public static NetManager ReloadedServer;

        /// <summary>
        /// Listens to network events triggered as other clients interact with the
        /// Reloaded Mod Loader Server.
        /// </summary>
        public static EventBasedNetListener ReloadedServerListener;

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
            // Triggered on soft reboot, self killing and restarting games.
            if (ReloadedServer != null)
            {
                ConsoleFunctions.PrintMessageWithTime("Local Server Already Running!", ConsoleFunctions.PrintInfoMessage);
                return;
            }

            try
            {
                // Create new server instance.
                ReloadedServerListener = new EventBasedNetListener();
                ReloadedServer = new NetManager(ReloadedServerListener, Strings.Loader.ServerConnectKey);

                // Start Server Internally
                ReloadedServer.Start(ServerPort);

                // Ping when message is received.
                ReloadedServerListener.PeerConnectedEvent += peer =>
                { Bindings.PrintInfo($"Received connection from: {peer.EndPoint.Host}:{peer.EndPoint.Port}"); };

                // Send received data to the message handler
                ReloadedServerListener.NetworkReceiveEvent += MessageHandler;

                // Process received events immediately.
                ReloadedServer.UnsyncedEvents = true;

                // Print words of success
                Bindings.PrintInfo($"Local Server Started at: {ReloadedServer.LocalPort}");
            }
            // If the port is occupied, an exception will be thrown.
            // Try hosting the server on another port.
            catch (Exception ex)
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
        /// Handles the messages received from the individual client peers consisting mainly of
        /// mods. Exposes various functionality features to mods.
        /// </summary>
        /// <param name="netPeer">Contains the peer client that sent the message to the server. A wrapper around a WebSocket Client.</param>
        /// <param name="reader">Contains the information sent by the client to the server.</param>
        private static void MessageHandler(NetPeer netPeer, NetDataReader reader)
        {
            // Parse the message structure.
            Message messageStruct = Message.GetMessage(reader.GetBytesWithLength());

            // Pass the relevant message.
            switch (messageStruct.MessageType)
            {
                // Text Functions
                case (ushort)MessageType.PrintText: Print(PrintMessageType.PrintText, messageStruct.Data); break;
                case (ushort)MessageType.PrintInfo: Print(PrintMessageType.PrintInfo, messageStruct.Data); break;
                case (ushort)MessageType.PrintWarning: Print(PrintMessageType.PrintWarning, messageStruct.Data); break;
                case (ushort)MessageType.PrintError: Print(PrintMessageType.PrintError, messageStruct.Data); break;
            }
        }
    }
}
