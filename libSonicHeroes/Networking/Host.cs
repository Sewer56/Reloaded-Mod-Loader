using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static SonicHeroes.Networking.Message;

namespace SonicHeroes.Networking
{
    /// <summary>
    /// This class allows the user to host a server allowing them to exchange data with other clients via the use of WebSockets. 
    /// By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// </summary>
    public class Host
    {
        /// <summary>
        /// The socket we will be using to communicate with the clients.
        /// Should you wish to configure its options, feel free to grab it and replace it.
        /// </summary>
        public Socket ServerSocket { get; set; }

        /// <summary>
        /// Defines a list of the clients that we will be serving!
        /// </summary>
        public List<Socket> ClientSockets { get; set; }

        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public byte[] Buffer { get; set; }  
        
        /// <summary>
        /// What kind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        public IPAddress IPAddressType { get; set; }

        /// <summary>
        /// The port at which the server itself will be hosted.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod { get; set; }

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] data, Socket socket);

        /// <summary>
        /// Constructor for the class allowing the user to establish a host.
        /// Initializes all members of the class and configures them to their defaults.
        /// To start the server, call StartServer().
        /// </summary>
        /// <param name="ipAddress">The type of IP addresses to listen to. e.g. loopback, any, ipv4</param>
        /// <param name="port">The port over which the server will be locally hosted.</param>
        public Host(IPAddress ipAddress, int port)
        {
            // Initialize the Socket
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set size of the buffer.
            Buffer = new byte[ServerSocket.ReceiveBufferSize];

            // Instantiate the list of client sockets.
            ClientSockets = new List<Socket>();

            // Set IP address type.
            IPAddressType = ipAddress;

            // Set port.
            Port = port;

            // Set the socket by default to disable Nagle's Algorhithm.
            ServerSocket.NoDelay = true;

            // Bind the socket to an endpoint.
            ServerSocket.Bind(new IPEndPoint(IPAddressType, Port));
        }

        /// <summary>
        /// Starts up the websocket server over which communication will occur on.
        /// </summary>
        public void StartServer()
        {
            // Maximum 100 clients can be left as pending.
            // We are going to be sending messages async anyway, so worry needs not placed at the performance on the game end.
            ServerSocket.Listen(100);

            // Start accepting connections!
            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        /// <summary>
        /// This will be ran when a new connection from a client is established.
        /// </summary>
        /// <param name="asyncResult"></param>
        public void AcceptCallback(IAsyncResult asyncResult)
        {
            // Set up a socket responsible for communication with the client.
            Socket clientSocket = ServerSocket.EndAccept(asyncResult);
           
            // Add this socket to the list of sockets.
            ClientSockets.Add(clientSocket);
            
            // Start receiving data from the client!
            clientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            
            // Start accepting new connections again!
            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        /// <summary>
        /// Ran when the server receives some data from a client.
        /// </summary>
        /// <param name="asyncResult"></param>
        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            // This socket is the same socket as in AcceptCallback
            Socket SocketX = (Socket)asyncResult.AsyncState;

            try
            {
                // Gets the length of data that has been received.
                int Received = SocketX.EndReceive(asyncResult);
                
                // Create a new buffer with the data which has been received.
                byte[] DataBuffer = new byte[Received];

                // Copy the received data.
                Array.Copy(Buffer, DataBuffer, Received);

                // Send the buffer to the subscribing method!
                ProcessBytesMethod(DataBuffer, SocketX);

                // Accept connections again!
                SocketX.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), SocketX);
            }
            catch { }
        }

        /// <summary>
        /// Send the data in a byte array to the server. 
        /// Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="awaitResponse">Set true to wait for a response from the server, else false.</param>
        /// <param name="socket">The socket to send the message to, usually a member of ClientSockets.</param>
        public void SendData(MessageStruct message, bool awaitResponse, Socket socket)
        {
            // Convert the message struct into bytes to send.
            byte[] data = Message.BuildMessage(message);

            // Send the serialized message.
            socket.Send(data); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (awaitResponse) { Receive(socket); }
        }

        /// <summary>
        /// Waits for a response from the client by simply calling on the
        /// Receive() method of the websocket server.
        /// <param name="socket">The socket to send the message to, usually a member of ClientSockets.</param>
        /// </summary>
        public void Receive(Socket socket)
        {
            // Receive the information from the host onto the buffer.
            // ClientSocket.Receive() returns the data length, stored here.
            int dataLength = socket.Receive(Buffer);

            // Create a byte array with a buffer of equal size to the received data.
            byte[] receiveBuffer = new byte[dataLength];

            // Copy the received data into the buffer.
            Array.Copy(Buffer, receiveBuffer, dataLength);

            // Process the received bytes.
            ProcessBytesMethod(receiveBuffer, ServerSocket);
        }

        /// <summary>
        /// An alternate way to retrieve requested data.
        /// Send the data in a byte array to the server. 
        /// Returns the received data as a byte array.
        /// </summary>
        /// <param name="message">The message that is to be sent to the server.</param>
        /// <param name="socket">The socket to send the message to, usually a member of ClientSockets.</param>
        public void SendDataRaw(MessageStruct message, Socket socket)
        {
            // Convert the message struct into bytes to send.
            byte[] data = Message.BuildMessage(message);

            // Send the serialized message.
            socket.Send(data); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            Receive(socket);
        }

        /// <summary>
        /// Waits for data to be received from the websocket client.
        /// Can be used to wait for a response from the server in question.
        /// This version returns the result from the host as a byte array.
        /// <param name="socket">The socket to send the message to, usually a member of ClientSockets.</param>
        /// </summary>
        public byte[] ReceiveRaw(Socket socket)
        {
            // Receive the information from the host onto the buffer.
            // ClientSocket.Receive() returns the data length, stored here.
            int dataLength = socket.Receive(Buffer);

            // Create a byte array with a buffer of equal size to the received data.
            byte[] receiveBuffer = new byte[dataLength];

            // Copy the received data into the buffer.
            Array.Copy(Buffer, receiveBuffer, dataLength);

            // Process the received bytes.
            return receiveBuffer;
        }

    }
}
