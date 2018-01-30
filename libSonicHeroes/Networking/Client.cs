using System;
using System.Net;
using System.Net.Sockets;
using static SonicHeroes.Networking.Message;

namespace SonicHeroes.Networking
{
    /// <summary>
    /// This class provides an easy to use implementation of a WebSocket Client which would allow communication with the websocket host.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Defines the maximum amount of connection attempts before the client gives
        /// up trying to connect to the host. Poor client...
        /// </summary>
        public int MAX_CONNECTION_ATTEMPTS = 10;

        /// <summary>
        /// The socket we will be using to communicate with the server.
        /// </summary>
        private Socket ClientSocket { get; set; }

        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public static byte[] Buffer { get; set; }

        /// <summary>
        /// What kuind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        public IPAddress IPAddressType { get; set; }

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod { get; set; }

        /// <summary>
        /// The port at which the client will try to connect to.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] data, Socket socket);

        /// <summary>
        /// Provides an easy to use implementation of a WebSocket Client, allowing 
        /// communication with a websocket host such as the mod loader server or a generic host.
        /// To start the client, call SetupClient().
        /// </summary>
        /// <param name="ipAddress">The IP Address to connect to. The IP Address can be specified by e.g. IPAddress.Parse("127.0.0.1");</param>
        /// <param name="port">The port over which the server will be locally hosted.</param>
        public Client(IPAddress ipAddress, int port)
        {
            // Initialize the Socket.
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set the size of the buffer.
            Buffer = new byte[ClientSocket.ReceiveBufferSize];

            // Set the IP Address Type
            IPAddressType = ipAddress;

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
            int ConnectionAttempts = 0;

            // While not connected
            while (!ClientSocket.Connected)
            {
                // Try X Times.
                if (ConnectionAttempts >= MAX_CONNECTION_ATTEMPTS) { return false; }

                // Attempt to establish a connection.
                try
                {
                    ClientSocket.Connect(IPAddressType, Port);
                    return true;
                }
                catch { ConnectionAttempts += 1; } 
            }
            return false;
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        public void SendData(MessageStruct message, bool awaitResponse)
        {
            // Convert the message struct into bytes to send.
            byte[] data = Message.BuildMessage(message);

            // Send the serialized message.
            ClientSocket.Send(data); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (awaitResponse) { Receive(); }
        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// </summary>
        public void Receive()
        {
            // Receive the information from the host onto the buffer.
            // ClientSocket.Receive() returns the data length, stored here.
            int dataLength = ClientSocket.Receive(Buffer);

            // Create a byte array with a buffer of equal size to the received data.
            byte[] receiveBuffer = new byte[dataLength];

            // Copy the received data into the buffer.
            Array.Copy(Buffer, receiveBuffer, dataLength);

            // Process the received bytes.
            ProcessBytesMethod(receiveBuffer, ClientSocket);
        }

        /// <summary>
        /// An alternate way to retrieve requested data.
        /// Send the data in a byte array to the server. 
        /// Returns the received data as a byte array.
        /// </summary>
        /// <param name="message">The message that is to be sent to the server.</param>
        public byte[] SendDataRaw(MessageStruct message)
        {
            // Convert the message struct into bytes to send.
            byte[] data = Message.BuildMessage(message);

            // Send the serialized message.
            ClientSocket.Send(data); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            return ReceiveRaw(); 
        }

        /// <summary>
        /// Waits for data to be received from the websocket host.
        /// Can be used to wait for a response from the server in question.
        /// This version returns the result from the host as a byte array.
        /// </summary>
        public byte[] ReceiveRaw()
        {
            // Receive the information from the host onto the buffer.
            // ClientSocket.Receive() returns the data length, stored here.
            int dataLength = ClientSocket.Receive(Buffer);

            // Create a byte array with a buffer of equal size to the received data.
            byte[] receiveBuffer = new byte[dataLength];

            // Copy the received data into the buffer.
            Array.Copy(Buffer, receiveBuffer, dataLength);

            // Process the received bytes.
            return receiveBuffer;
        }
    }
}
