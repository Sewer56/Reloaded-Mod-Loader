using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SonicHeroes.Networking
{
    /// <summary>
    /// This class defines the means by which one individual may host a server to which they and the other clients can easily exchange data. By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// </summary>
    class WebSocket_Host
    {
        /// <summary>
        /// The socket we will be using to communicate with the clients.
        /// </summary>
        private Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// Defines a list of the clients that we will be serving!
        /// </summary>
        private List<Socket> ClientSockets = new List<Socket>();
        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public static byte[] Buffer = new byte[512];
        /// <summary>
        /// What kuind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        IPAddress IPAddressType;

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] Data, Socket SocketX);

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod;

        /// <summary>
        /// Sets up the websocket server over which communication will occur on.
        /// </summary>
        public void SetupServer(IPAddress IPAddressTypeX)
        {
            // Set of IP address.
            IPAddressType = IPAddressTypeX;

            // Listen to any IP Address
            ServerSocket.Bind(new IPEndPoint(IPAddressType, 13370));

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
            Console.WriteLine(GetCurrentTime() + "Client Connected!");
            // Set up a socket responsible for communication with the client.
            Socket SocketX = ServerSocket.EndAccept(asyncResult);
            // Add this socket to the list of sockets.
            ClientSockets.Add(SocketX);
            // Start receiving data!
            SocketX.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), SocketX);
            // Start accepting new connections again!
            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

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
            catch (Exception Ex) { }
        }

        /// <summary>
        /// Resolves current time to be appended to a message.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime() { return "[" + DateTime.Now.ToString("hh:mm:ss") + "] "; }

        /// <summary>
        /// Writes a centered line to the console.
        /// </summary>
        /// <param name=""></param>
        /// <param name="Message"></param>
        public static void ConsoleX_WriteLine_Center(string Message)
        {
            Console.SetCursorPosition((Console.WindowWidth - Message.Length) / 2, Console.CursorTop);
            Console.WriteLine(Message);
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }

    /// <summary>
    /// This class defines the means by which one individual may connect to an existing server to easily exchange data. By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// </summary>
    class WebSocket_Client
    {
        /// <summary>
        /// The socket we will be using to communicate with the server.
        /// </summary>
        private Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public static byte[] Buffer = new byte[512];
        /// <summary>
        /// What kuind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        IPAddress IPAddressType;

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] Data, Socket SocketX);

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod;

        /// <summary>
        /// Sets up the websocket server over which communication will occur on. Returns true on successful attempt.
        /// </summary>
        public bool SetupClient(IPAddress IPAddressTypeX)
        {
            // Set of IP address.
            IPAddressType = IPAddressTypeX;

            // Amount of tries we tried to connect.
            int ConnectionAttempts = 0;
            while (!ClientSocket.Connected)
            {
                if (ConnectionAttempts > 10) { return false; }
                try { ClientSocket.Connect(IPAddress.Loopback, 13370); return true; }
                catch (SocketException Ex) { ConnectionAttempts += 1; } // If you can't connect, you have no hope!
            }
            return false;
        }

        /// <summary>
        /// Send the data in a byte array to the server.
        /// </summary>
        /// <param name="Data"></param>
        public void SendData(byte[] Data, bool ExpectResponse)
        {
            // Seeeeend
            ClientSocket.Send(Data);
            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (ExpectResponse)
            {
                int DataLength = ClientSocket.Receive(Buffer);
                byte[] ReceiveBuffer = new byte[DataLength];
                Array.Copy(Buffer, ReceiveBuffer, DataLength);
                // Process the received bytes.
                ProcessBytesMethod(ReceiveBuffer, ClientSocket);
            }

        }
    }

}
