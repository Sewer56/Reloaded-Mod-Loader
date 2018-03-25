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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using static Reloaded.Networking.Message;

namespace Reloaded.Networking
{
    /// <summary>
    /// This class allows the user to host a server allowing them to exchange data with other clients via the use of WebSockets. 
    /// By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// This basically sets up an asynchronous websocket host, that's about it.
    /// </summary>
    public class Host
    {
        /// <summary>
        /// The socket we will be using to communicate with the clients.
        /// Should you wish to configure its options, feel free to grab it and replace it.
        /// </summary>
        public ReloadedSocket ReloadedSocket { get; set; }

        /// <summary>
        /// Defines a list of the clients that we will be serving!
        /// </summary>
        public List<ReloadedSocket> Clients { get; set; }

        /// <summary>
        /// Specifies the methods to be executed when data is received from clients.
        /// </summary>
        public SocketExtensions.ProcessBytesDelegate ProcessBytesMethods { get; set; }

        /// <summary>
        /// Constructor for the class allowing the user to establish a host.
        /// Initializes all members of the class and configures them to their defaults.
        /// To start the server, call StartServer().
        /// </summary>
        /// <param name="ipAddress">The type of IP addresses to listen to. e.g. loopback, any, ipv4</param>
        /// <param name="port">The port over which the server will be locally hosted.</param>
        public Host(IPAddress ipAddress, int port)
        {
            // Initialize the ReloadedSocket
            Socket localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            // Set up Reloaded ReloadedSocket
            ReloadedSocket = new ReloadedSocket(localSocket);

            // Instantiate the list of client sockets.
            Clients = new List<ReloadedSocket>();

            // Set the socket by default to disable Nagle's Algorhithm.
            localSocket.NoDelay = true;

            // Bind the socket to an endpoint.
            localSocket.Bind(new IPEndPoint(ipAddress, port));
        }

        /// <summary>
        /// Starts up the websocket server over which communication will occur on.
        /// Note: Please first assign ProcessBytesMethods before executing this.
        /// </summary>
        public void StartServer()
        {
            // Maximum 100 clients can be left as pending.
            // We are going to be sending messages async anyway, so worry needs not placed at the performance on the game end.
            ReloadedSocket.Socket.Listen(100);

            // Start accepting connections!
            ReloadedSocket.Socket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Shuts down the server by closing all connections and stopping accepting 
        /// new connections.
        /// For starting the server over again, you should create a new instance of
        /// the host class.
        /// </summary>
        public void ShutdownServer()
        {
            // Close all client sockets.
            foreach (ReloadedSocket client in Clients) {
                client.Socket.Disconnect(false);
                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Close();    
            }

            // Close own socket
            ReloadedSocket.Socket.Disconnect(false);
            ReloadedSocket.Socket.Shutdown(SocketShutdown.Both);
            ReloadedSocket.Socket.Close();
        }

        /// <summary>
        /// This will be ran when a new connection from a client is established.
        /// </summary>
        /// <param name="asyncResult"></param>
        public void AcceptCallback(IAsyncResult asyncResult)
        {
            // Temporarily stop accepting connections.
            // Set up a socket responsible for communication with the client.
            Socket clientSocket = (Socket)ReloadedSocket.Socket.EndAccept(asyncResult);
           
            // Set up Reloaded Socket from Socket
            ReloadedSocket actualClientSocket = new ReloadedSocket(clientSocket);

            // Add this socket to the list of sockets.
            Clients.Add(actualClientSocket);

            // Start receiving data from the client asynchronously!
            // The size of data to receive is the size of a message header.
            actualClientSocket.Socket.BeginReceive(ReloadedSocket.ReceiveBuffer, 0, sizeof(UInt32), SocketFlags.None, ReceiveSizeCallback, actualClientSocket);
            
            // Start accepting new connections again!
            ReloadedSocket.Socket.BeginAccept(AcceptCallback, null);
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
                int bytesReceived = clientSocket.Socket.EndReceive(asyncResult);
                clientSocket.AsyncReceivedBytes += bytesReceived;

                // If we have not received all of the bytes yet.
                if (clientSocket.AsyncReceivedBytes < clientSocket.AsyncBytesToReceive)
                {
                    // The size of data to receive is the size of a message header.
                    clientSocket.Socket.BeginReceive(ReloadedSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                        clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                        ReceiveSizeCallback, clientSocket);
                    return;
                }

                // Get the true length of the message to be received.
                clientSocket.AsyncBytesToReceive = BitConverter.ToInt32(ReloadedSocket.ReceiveBuffer, 0);
                clientSocket.AsyncReceivedBytes = 0;

                // The size of data to receive is the size of a message header.
                clientSocket.Socket.BeginReceive(ReloadedSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                    clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                    ReceiveDataCallback, clientSocket);
            }
            // Server was closed.
            catch (Exception) { }
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
                int bytesReceived = clientSocket.Socket.EndReceive(asyncResult);
                clientSocket.AsyncReceivedBytes += bytesReceived;

                // If we have not received all of the bytes yet.
                if (clientSocket.AsyncReceivedBytes < clientSocket.AsyncBytesToReceive)
                {
                    // The size of data to receive is the size of a message header.
                    clientSocket.Socket.BeginReceive(ReloadedSocket.ReceiveBuffer, clientSocket.AsyncReceivedBytes,
                        clientSocket.AsyncBytesToReceive - clientSocket.AsyncReceivedBytes, SocketFlags.None,
                        ReceiveDataCallback, clientSocket);
                    return;
                }

                // Create the buffer for the received data.
                byte[] dataBuffer = new byte[clientSocket.AsyncBytesToReceive];

                // Copy the received data.
                Array.Copy(ReloadedSocket.ReceiveBuffer, dataBuffer, clientSocket.AsyncReceivedBytes);

                // Send the buffer to the subscribing method!
                ProcessBytesMethods(ParseMessage(dataBuffer), clientSocket);

                // Re-set bytes received.
                clientSocket.AsyncReceivedBytes = 0;

                // Accept connections again!
                // Header Length!
                clientSocket.Socket.BeginReceive(ReloadedSocket.ReceiveBuffer, 0, sizeof(UInt32), SocketFlags.None,
                    ReceiveSizeCallback, clientSocket);
            }
            // Server was closed.
            catch (Exception) { }
        }
    }
}
