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

using System.Net.Sockets;

namespace Reloaded.Networking.Sockets
{
    /// <summary>
    /// Extends the ReloadedSocket Class by providing a buffer for the sending and receiving of data for the mod loader's
    /// sockets.
    /// </summary>
    public class ReloadedSocket
    {
        /// <summary>
        /// Specifies the default size of the receive buffer.
        /// </summary>
        public int DefaultBufferSize { get; } = 65536;

        /// <summary>
        /// Holds the send and receive buffer for the current socket.
        /// </summary>
        public byte[] ReceiveBuffer;

        /// <summary>
        /// Stores the information in bytes that is to be sent to the next socket in asynchronous operations.
        /// </summary>
        public byte[] DataToSend;

        /// <summary>
        /// Specifies the amount of received bytes for asynchronous operations.
        /// </summary>
        public int AsyncReceivedBytes = 0;

        /// <summary>
        /// Specifies the amount of bytes to be received in an asynchronous send operation.
        /// </summary>
        public int AsyncBytesToReceive = 0;

        /// <summary>
        /// Specifies the amount of sent bytes for asynchronous operations.
        /// </summary>
        public int AsyncSentBytes = 0;

        /// <summary>
        /// Specifies the amount of bytes to be sent in an asynchronous send operation.
        /// </summary>
        public int AsyncBytesToSend = 0;

        /// <summary>
        /// Defines the socket stored by the ReloadedSocket class, since inheritance poses problems with the likes of 
        /// EndAccept, etc.
        /// </summary>
        public Socket Socket;

        /// <summary>
        /// Sets up an instance of a reloaded socket from a passed in socket.
        /// </summary>
        /// <param name="socket">ReloadedSocket to use as ReloadedSocket</param>
        public ReloadedSocket(Socket socket)
        {
            // Child ReloadedSocket
            Socket = socket;

            // Create default receive buffer
            ReceiveBuffer = new byte[DefaultBufferSize];

            // Set size of the buffer.
            socket.ReceiveBufferSize = ReceiveBuffer.Length;
            socket.SendBufferSize = ReceiveBuffer.Length;
        }
    }
}
