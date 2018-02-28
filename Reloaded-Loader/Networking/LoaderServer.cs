using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Networking;
using Reloaded_Loader.Networking.LoaderServerFunctions;
using Reloaded_Loader.Terminal;
using static Reloaded.Networking.MessageTypes.LoaderServerMessages;
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
        public static int SERVER_PORT = 13370;

        /// <summary>
        /// Sets up the Reloaded Mod Loader Server.
        /// </summary>
        public static void SetupServer()
        {
            try
            {
                // Create new server instance.
                ReloadedServer = new Host(IPAddress.Loopback, SERVER_PORT);

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
                ConsoleFunctions.PrintMessageWithTime("Failed to create local host at port " + SERVER_PORT + ". Attempting port " + (SERVER_PORT + 1) + ".", ConsoleFunctions.PrintWarningMessage);
                ConsoleFunctions.PrintMessageWithTime("Original Message: " + ex.Message, ConsoleFunctions.PrintWarningMessage);

                // Increment the port number.
                SERVER_PORT += 1;

                // Call self
                SetupServer();
            }
        }

        /// <summary>
        /// Handles the messages received from the individual client sockets consisting mainly of
        /// mods. Exposes various functionality features to mods.
        /// </summary>
        /// <param name="data">The data sent over by the individual mod loader server clients.</param>
        /// <param name="socket">The individual socket object to use for connection back with the mod loader clients.</param>
        private static void MessageHandler(Message.MessageStruct clientMessage, ReloadedSocket socket)
        {
            // Pass the relevant message.
            switch (clientMessage.MessageType)
            {
                // Regular Functions
                case (ushort)MessageType.Okay: ReplyOkay.ReplyOk(socket); break;

                // Text Functions
                case (ushort)MessageType.PrintText: PrintToScreen.Print(PrintMessageType.PrintText, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintInfo: PrintToScreen.Print(PrintMessageType.PrintInfo, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintWarning: PrintToScreen.Print(PrintMessageType.PrintWarning, clientMessage.Data, socket); break;
                case (ushort)MessageType.PrintError: PrintToScreen.Print(PrintMessageType.PrintError, clientMessage.Data, socket); break;

            }
        }
    }
}
