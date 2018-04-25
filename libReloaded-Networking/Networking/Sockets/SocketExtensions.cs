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
using System.Net.Sockets;

namespace Reloaded.Networking.Sockets
{
    /// <summary>
    /// Provides various extensions to the reloadedSocket class allowing us to receive and send.
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(Message.MessageStruct messageStruct, ReloadedSocket socket);

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        public static Message.MessageStruct SendData(this ReloadedSocket reloadedSocket, Message.MessageStruct message, bool awaitResponse)
        {
            try
            {
                // Return empty message if socket not connected.
                if (!reloadedSocket.Socket.Connected) { return null; }

                // Convert the message struct into bytes to send.
                byte[] data = message.BuildMessage();

                // Get the amount of bytes sent.
                int bytesSent = 0;

                // Ensure we send all bytes.
                while (bytesSent < data.Length)
                {
                    // Offset: Bytes Sent
                    // Length to Send: Length to send - Already Sent
                    int bytesSuccessfullySent = reloadedSocket.Socket.Send(data, bytesSent, data.Length - bytesSent, SocketFlags.None); // Send serialized Message!
                    bytesSent += bytesSuccessfullySent;

                    // If the reloadedSocket is not connected, return empty message struct.
                    if (bytesSuccessfullySent == 0)
                    {
                        if (!IsSocketConnected(reloadedSocket)) { return null; }
                    }
                }

                // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
                if (awaitResponse) return ReceiveData(reloadedSocket);
                return null;
            }
            catch
            // Probably lost connection.
            {
                reloadedSocket.CloseIfDisconnected();
                return null;
            }
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        /// <param name="receiveDataDelegate">The method to call to process the received data from the server/client.</param>
        public static void SendData(this ReloadedSocket reloadedSocket, Message.MessageStruct message, bool awaitResponse, ProcessBytesDelegate receiveDataDelegate)
        {
            // Call the other overload and get our data back.
            Message.MessageStruct messageLocal = SendData(reloadedSocket, message, awaitResponse);
            
            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (awaitResponse) receiveDataDelegate(messageLocal, reloadedSocket);
        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// This version returns the result from the host as a byte array.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object connected to either a host or client.</param>
        public static Message.MessageStruct ReceiveData(this ReloadedSocket reloadedSocket)
        {
            try
            {
                // Return empty message if socket not connected.
                if (!reloadedSocket.Socket.Connected) { return null; }

                // ReceiveData the information from the host onto the buffer.
                // ClientSocket.Receive() returns the data length, stored here.
                int bytesToReceive = sizeof(uint);
                int bytesReceived = 0;
                bytesReceived += reloadedSocket.Socket.Receive(reloadedSocket.ReceiveBuffer, bytesToReceive, SocketFlags.None);

                // Receive packets until all information is acquired.
                while (bytesReceived < bytesToReceive)
                {
                    // Receive extra bytes
                    int newBytesReceived = reloadedSocket.Socket.Receive(reloadedSocket.ReceiveBuffer, bytesReceived, bytesToReceive - bytesReceived, SocketFlags.None);
                    bytesReceived += newBytesReceived;

                    // If the reloadedSocket is not connected, return empty message struct.
                    if (newBytesReceived == 0)
                    {
                        if (!IsSocketConnected(reloadedSocket)) { return null; }
                    }
                }

                // Get the true length of the message to be received.
                bytesToReceive = BitConverter.ToInt32(reloadedSocket.ReceiveBuffer, 0);
                bytesReceived = 0;

                // Increase buffer size if necessary.
                bool messageTooLarge = false;
                if (bytesToReceive > reloadedSocket.ReceiveBuffer.Length)
                {
                    reloadedSocket.ReceiveBuffer = new byte[bytesToReceive];
                    messageTooLarge = true;
                }

                // Receive packets until all information is acquired.
                while (bytesReceived < bytesToReceive)
                {
                    // Receive extra bytes
                    int newBytesReceived = reloadedSocket.Socket.Receive(reloadedSocket.ReceiveBuffer, bytesReceived, bytesToReceive - bytesReceived, SocketFlags.None);
                    bytesReceived += newBytesReceived;

                    // If the reloadedSocket is not connected, return empty message struct.
                    if (newBytesReceived == 0)
                    {
                        if (!IsSocketConnected(reloadedSocket)) { return null; }
                    }
                }

                // Create a receive buffer with our own data length to be received.
                byte[] receiveBuffer = new byte[bytesToReceive];

                // Copy the received data into the buffer.
                Array.Copy(reloadedSocket.ReceiveBuffer, receiveBuffer, bytesToReceive);

                // Convert Received Bytes into a Message Struct
                Message.MessageStruct receivedData = Message.ParseMessage(receiveBuffer);

                // Reset buffer size to smaller (for Garbage Collector to pick up old buffer)
                if (messageTooLarge)
                    reloadedSocket.ReceiveBuffer = new byte[reloadedSocket.DefaultBufferSize];

                // Process the received bytes.
                return receivedData;
            }
            catch
            // Probably lost connection.
            {
                reloadedSocket.CloseIfDisconnected();
                return null;
            }

        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object connected to either a host or client.</param>
        /// <param name="receiveDataDelegate">The method to call to process the received data from the server/client.</param>
        public static void ReceiveData(this ReloadedSocket reloadedSocket, ProcessBytesDelegate receiveDataDelegate)
        {
            // ReceiveData using the other overload, but instead of passing
            // the result back, call the receive data delegate.
            receiveDataDelegate(ReceiveData(reloadedSocket), reloadedSocket);
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        public static void SendDataAsync(this ReloadedSocket reloadedSocket, Message.MessageStruct message)
        {
            try
            {
                // Return empty message if socket not connected.
                if (!reloadedSocket.Socket.Connected) { return; }

                // Convert the message struct into bytes to send.
                reloadedSocket.DataToSend = message.BuildMessage();

                // Set sent bytes to 0.
                reloadedSocket.AsyncBytesToSend = reloadedSocket.DataToSend.Length;
                reloadedSocket.AsyncSentBytes = 0;

                // Begin sending the message.
                reloadedSocket.Socket.BeginSend(reloadedSocket.DataToSend, 0, reloadedSocket.DataToSend.Length, SocketFlags.None, SendAsyncCallback, reloadedSocket);
            }
            catch { reloadedSocket.CloseIfDisconnected(); }
        }

        /// <summary>
        /// Callback for the BeginSend asynchronous method.
        /// Asynchronously sends data in the buffer to the target client.
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void SendAsyncCallback(IAsyncResult asyncResult)
        {
            try
            {
                // Get our reloadedSocket back.
                ReloadedSocket clientSocket = (ReloadedSocket) asyncResult.AsyncState;

                // End the send operation.
                int bytesSent = clientSocket.Socket.EndSend(asyncResult);

                // Increment the amount of bytes sent.
                clientSocket.AsyncSentBytes += bytesSent;

                // Loop back if not all of the data was sent.
                if (clientSocket.AsyncSentBytes < clientSocket.AsyncBytesToSend)
                {
                    // Begin sending the rest of the message.
                    clientSocket.Socket.BeginSend(clientSocket.DataToSend, clientSocket.AsyncSentBytes,
                        clientSocket.DataToSend.Length - clientSocket.AsyncSentBytes, SocketFlags.None,
                        SendAsyncCallback, clientSocket);
                    return;
                }

                // Else reset the bytes that are to be sent/received.
                clientSocket.AsyncBytesToSend = 0;
                clientSocket.AsyncSentBytes = 0;
            }
            catch
            {
                // Get our reloadedSocket back.
                ReloadedSocket clientSocket = (ReloadedSocket)asyncResult.AsyncState;
                clientSocket.AsyncBytesToSend = 0;
                clientSocket.AsyncSentBytes = 0;
            }
        }

        /// <summary>
        /// Polls the reloadedSocket, returns true if the reloadedSocket is cononected, else false.
        /// </summary>
        /// <param name="reloadedSocket">The individual reloadedSocket object to check.</param>
        public static bool IsSocketConnected(this ReloadedSocket reloadedSocket)
        {
            // First check if reloadedSocket itself reports as connected.
            if (!reloadedSocket.Socket.Connected) { return false; }

            // Poll the reloadedSocket and check if there is anything readible.
            if (reloadedSocket.Socket.Poll(1000, SelectMode.SelectRead) && (reloadedSocket.Socket.Available == 0))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the other Client/Host has disconnected and 
        /// automatically closes the Host/Client Socket connection if disconnected.
        /// </summary>
        /// <param name="reloadedSocket"></param>
        /// <returns></returns>
        public static void CloseIfDisconnected(this ReloadedSocket reloadedSocket)
        {
            if (reloadedSocket.IsSocketConnected() == false)
            {
                reloadedSocket.Socket.Close();
            }
        }
    }
}
