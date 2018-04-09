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
using System.Net;
using System.Net.Sockets;
using Reloaded.Networking.Sockets;
using static Reloaded.Networking.Message;

namespace Reloaded.Networking
{
    /// <summary>
    /// This class provides an easy to use implementation of a WebSocket Client which would allow communication with the websocket host.
    /// This basically sets up the websocket client, that's about it.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Defines the maximum amount of connection attempts before the client gives
        /// up trying to connect to the host. Poor client...
        /// </summary>
        public int MaxConnectionAttempts = 10;

        /// <summary>
        /// The socket we will be using to communicate with the server.
        /// </summary>
        public ReloadedSocket ClientSocket { get; set; }

        /// <summary>
        /// What kind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        public IPAddress IpAddressType { get; set; }

        /// <summary>
        /// The port at which the client will try to connect to.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Specifies the methods to be executed when data is received from clients.
        /// </summary>
        public SocketExtensions.ProcessBytesDelegate ProcessBytesMethods { get; set; }

        /// <summary>
        /// Provides an easy to use implementation of a WebSocket Client, allowing 
        /// communication with a websocket host such as the mod loader server or a generic host.
        /// To start the client, call SetupClient().
        /// </summary>
        /// <param name="ipAddress">The IP Address to connect to. The IP Address can be specified by e.g. IPAddress.Parse("127.0.0.1");</param>
        /// <param name="port">The port over which the server will be locally hosted.</param>
        public Client(IPAddress ipAddress, int port)
        {
            // Initialize the ReloadedSocket
            Socket localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set up Reloaded ReloadedSocket
            ClientSocket = new ReloadedSocket(localSocket);

            // Set the IP Address Type
            IpAddressType = ipAddress;

            // Set the port.
            Port = port;
        }

        /// <summary>
        /// Starts up the websocket client over which communication will occur on. 
        /// Returns true if the client has successfully connected to the host.
        /// </summary>
        public bool StartClient()
        {
            // Counter defining the current count of connection attempts.
            int connectionAttempts = 0;

            // While not connected
            while (!ClientSocket.Socket.Connected)
            {
                // Try X Times.
                if (connectionAttempts >= MaxConnectionAttempts) return false;

                // Attempt to establish a connection.
                try
                {
                    ClientSocket.Socket.Connect(IpAddressType, Port);
                    return true;
                }
                catch { connectionAttempts += 1; } 
            }
            return false;
        }

        /// <summary>
        /// Stops an asynchronous receive operation.
        /// </summary>
        public void ShutdownClient()
        {
            // Close own socket
            ClientSocket.Socket.Disconnect(false);
            ClientSocket.Socket.Shutdown(SocketShutdown.Both);
            ClientSocket.Socket.Close();
        }

        /// <summary>
        /// Starts an asynchronous receive operation on the client socket.
        /// This allows us to automatically receive and process data from the host
        /// with the methods defined in ProcessBytesMethods delegate instance.
        /// </summary>
        public void StartAsyncReceive()
        {
            // Start receiving data from the client asynchronously!
            // The size of data to receive is the size of a message header.
            ClientSocket.Socket.BeginReceive(ClientSocket.ReceiveBuffer, 0, sizeof(UInt32), SocketFlags.None, ReceiveSizeCallback, ClientSocket);
        }

        /// <summary>
        /// Ran when the server receives some data from a client.
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReceiveSizeCallback(IAsyncResult asyncResult)
        {
            try
            {
                // This socket is the same socket as in AcceptCallback
                ReloadedSocket clientSocket = (ReloadedSocket) asyncResult.AsyncState;

                // Set expected data to be received to 4. (Message length)
                clientSocket.AsyncBytesToReceive = sizeof(UInt32);

                // Gets the length of data that has been received.
                // Increment Received Bytes
                int bytesReceived = ClientSocket.Socket.EndReceive(asyncResult);
                clientSocket.AsyncReceivedBytes += bytesReceived;

                // Close if we are disconnected if 0 bytes received
                if (bytesReceived == 0)
                {
                    clientSocket.CloseIfDisconnected();
                    return;
                }

                // If we have not received all of the bytes yet.
                if (clientSocket.AsyncReceivedBytes < clientSocket.AsyncBytesToReceive)
                {
                    // The size of data to receive is the size of a message header.
                    ClientSocket.Socket.BeginReceive(ClientSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                        clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                        ReceiveSizeCallback, clientSocket);
                    return;
                }

                // Get the true length of the message to be received.
                clientSocket.AsyncBytesToReceive = BitConverter.ToInt32(ClientSocket.ReceiveBuffer, 0);
                clientSocket.AsyncReceivedBytes = 0;

                // Increase buffer size if necessary.
                if (clientSocket.AsyncBytesToReceive > clientSocket.ReceiveBuffer.Length)
                { clientSocket.ReceiveBuffer = new byte[clientSocket.AsyncBytesToReceive]; }

                // The size of data to receive is the size of a message header.
                ClientSocket.Socket.BeginReceive(ClientSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                    clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                    ReceiveDataCallback, clientSocket);
            }
            // Server was closed.
            catch (Exception ex)
            {
                Bindings.PrintWarning("[libReloaded] Exception thrown while receiving packet size header, the host probably closed the connection | " + ex.Message);
            }
        }

        /// <summary>
        /// Ran when the server receives some data from a client.
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReceiveDataCallback(IAsyncResult asyncResult)
        {
            try
            {
                // This socket is the same socket as in AcceptCallback
                ReloadedSocket clientSocket = (ReloadedSocket) asyncResult.AsyncState;

                // Gets the length of data that has been received.
                int bytesReceived = ClientSocket.Socket.EndReceive(asyncResult);
                clientSocket.AsyncReceivedBytes += bytesReceived;

                // Close if we are disconnected if 0 bytes received
                if (bytesReceived == 0)
                {
                    clientSocket.CloseIfDisconnected();
                    return;
                }

                // If we have not received all of the bytes yet.
                if (clientSocket.AsyncReceivedBytes < clientSocket.AsyncBytesToReceive)
                {
                    // The size of data to receive is the size of a message header.
                    ClientSocket.Socket.BeginReceive(ClientSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                        clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                        ReceiveDataCallback, clientSocket);
                    return;
                }

                // Create the buffer for the received data.
                byte[] dataBuffer = new byte[clientSocket.AsyncBytesToReceive];

                // Copy the received data.
                Array.Copy(ClientSocket.ReceiveBuffer, dataBuffer, clientSocket.AsyncReceivedBytes);

                // Send the buffer to the subscribing method!
                ProcessBytesMethods(ParseMessage(dataBuffer), clientSocket);

                // Re-set bytes received.
                clientSocket.AsyncReceivedBytes = 0;

                // Reset buffer size (old buffer will be Garbage Collected in due time).
                if (clientSocket.ReceiveBuffer.Length > clientSocket.DefaultBufferSize)
                { clientSocket.ReceiveBuffer = new byte[clientSocket.DefaultBufferSize]; }

                // Accept connections again!
                // Header Length!
                ClientSocket.Socket.BeginReceive(ClientSocket.ReceiveBuffer, 0, sizeof(UInt32), SocketFlags.None,
                    ReceiveSizeCallback, clientSocket);
            }
            // Server was closed.
            catch (Exception ex)
            {
                Bindings.PrintWarning("[libReloaded] Exception thrown while receiving packet data, the host probably closed the connection | " + ex.Message);
            }
        }
    }
}
