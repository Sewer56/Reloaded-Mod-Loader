using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Reloaded.Networking.Message;

namespace Reloaded.Networking
{
    /// <summary>
    /// Provides various extensions to the socket class allowing us to receive and send.
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(MessageStruct messageStruct, Socket socket);

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="socket">The individual socket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        public static MessageStruct SendData(this ReloadedSocket socket, MessageStruct message, bool awaitResponse)
        {
            // Convert the message struct into bytes to send.
            byte[] data = message.BuildMessage();

            // Get the amount of bytes sent.
            int bytesSent = 0;
            
            // Ensure we send all bytes.
            while (bytesSent < data.Length)
            {
                // Offset: Bytes Sent
                // Length to Send: Length to send - Already Sent
                int bytesSuccessfullySent = socket.Send(data, bytesSent, data.Length - bytesSent, SocketFlags.None); // Send serialized Message!
                bytesSent += bytesSuccessfullySent;

                // If the socket is not connected, return empty message struct.
                if (bytesSuccessfullySent == 0) {
                    if (!IsSocketConnected(socket)) { return new MessageStruct(); }
                }
            }
            
            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (awaitResponse) return ReceiveData(socket);
            return new MessageStruct();
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="socket">The individual socket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        /// <param name="receiveDataDelegate">The method to call to process the received data from the server/client.</param>
        public static void SendData(this ReloadedSocket socket, MessageStruct message, bool awaitResponse, ProcessBytesDelegate receiveDataDelegate)
        {
            // Convert the message struct into bytes to send.
            byte[] data = message.BuildMessage();

            // Get the amount of bytes sent.
            int bytesSent = 0;

            // Ensure we send all bytes.
            while (bytesSent < data.Length)
            {
                // Offset: Bytes Sent
                // Length to Send: Length to send - Already Sent
                int bytesSuccessfullySent = socket.Send(data, bytesSent, data.Length - bytesSent, SocketFlags.None); // Send serialized Message!
                bytesSent += bytesSuccessfullySent;

                // If the socket is not connected, return empty message struct.
                if (bytesSuccessfullySent == 0) {
                    if (!IsSocketConnected(socket)) { return; }
                }
            }

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (awaitResponse) ReceiveData(socket, receiveDataDelegate);
        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// This version returns the result from the host as a byte array.
        /// </summary>
        /// <param name="socket">The individual socket object connected to either a host or client.</param>
        public static MessageStruct ReceiveData(this ReloadedSocket socket)
        {
            // ReceiveData the information from the host onto the buffer.
            // ClientSocket.Receive() returns the data length, stored here.
            int bytesToReceive = sizeof(UInt32);
            int bytesReceived = 0;
            bytesReceived += socket.Receive(socket.ReceiveBuffer, bytesToReceive, SocketFlags.None);

            // Receive packets until all information is acquired.
            while (bytesReceived < bytesToReceive)
            {
                // Receive extra bytes
                int newBytesReceived = socket.Receive(socket.ReceiveBuffer, bytesReceived, bytesToReceive - bytesReceived, SocketFlags.None);
                bytesReceived += newBytesReceived;

                // If the socket is not connected, return empty message struct.
                if (newBytesReceived == 0) {
                    if (!IsSocketConnected(socket)) { return new MessageStruct(); }
                }
            }

            // Get the true length of the message to be received.
            bytesToReceive = BitConverter.ToInt32(socket.ReceiveBuffer, 0);
            bytesReceived = 0;

            // Receive packets until all information is acquired.
            while (bytesReceived < bytesToReceive)
            {
                // Receive extra bytes
                int newBytesReceived = socket.Receive(socket.ReceiveBuffer, bytesReceived, bytesToReceive - bytesReceived, SocketFlags.None);
                bytesReceived += newBytesReceived;

                // If the socket is not connected, return empty message struct.
                if (newBytesReceived == 0) {
                    if (!IsSocketConnected(socket)) { return new MessageStruct(); }
                }
            }

            // Create a receive buffer with our own data length to be received.
            byte[] receiveBuffer = new byte[bytesToReceive];

            // Copy the received data into the buffer.
            Array.Copy(socket.ReceiveBuffer, receiveBuffer, bytesToReceive);

            // Convert Received Bytes into a Message Struct
            MessageStruct receivedData = ParseMessage(receiveBuffer);

            // Process the received bytes.
            return receivedData;
        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// </summary>
        /// <param name="socket">The individual socket object connected to either a host or client.</param>
        /// <param name="receiveDataDelegate">The method to call to process the received data from the server/client.</param>
        public static void ReceiveData(this ReloadedSocket socket, ProcessBytesDelegate receiveDataDelegate)
        {
            // ReceiveData using the other overload, but instead of passing
            // the result back, call the receive data delegate.
            receiveDataDelegate(ReceiveData(socket), socket);
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="socket">The individual socket object connected to either a host or client.</param>
        /// <param name="message">The message that is to be sent to the server.</param>
        public static void SendDataAsync(this ReloadedSocket socket, MessageStruct message)
        {
            // Convert the message struct into bytes to send.
            socket.DataToSend = message.BuildMessage();

            // Set sent bytes to 0.
            socket.AsyncBytesToSend = socket.DataToSend.Length;
            socket.AsyncSentBytes = 0;

            // Begin sending the message.
            socket.BeginSend(socket.DataToSend, 0, socket.DataToSend.Length, SocketFlags.None, SendAsyncCallback, socket);
        }

        /// <summary>
        /// Callback for the BeginSend asynchronous method.
        /// Asynchronously sends data in the buffer to the target client.
        /// </summary>
        /// <param name="asyncResult"></param>
        private static void SendAsyncCallback(IAsyncResult asyncResult)
        {
            // Get our socket back.
            ReloadedSocket clientSocket = (ReloadedSocket)asyncResult.AsyncState;

            // End the send operation.
            int bytesSent = clientSocket.EndSend(asyncResult);

            // Increment the amount of bytes sent.
            clientSocket.AsyncSentBytes += bytesSent;

            // Loop back if not all of the data was sent.
            if (clientSocket.AsyncSentBytes < clientSocket.AsyncBytesToSend)
            {
                // Begin sending the rest of the message.
                clientSocket.BeginSend(clientSocket.DataToSend, clientSocket.AsyncSentBytes, clientSocket.DataToSend.Length - clientSocket.AsyncSentBytes, SocketFlags.None, SendAsyncCallback, clientSocket);
                return;
            }

            // Else reset the bytes that are to be sent/received.
            clientSocket.AsyncBytesToSend = 0;
            clientSocket.AsyncSentBytes = 0;
        }

        /// <summary>
        /// Polls the socket, returns true if the socket is cononected, else false.
        /// </summary>
        /// <param name="socket">The individual socket object to check.</param>
        public static bool IsSocketConnected(this ReloadedSocket socket)
        {
            // First check if socket itself reports as connected.
            if (!socket.Connected) { return false; }

            // Poll the socket and check if there is anything readible.
            if (socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0))
            {
                return false;
            }

            return true;
        }
    }
}
