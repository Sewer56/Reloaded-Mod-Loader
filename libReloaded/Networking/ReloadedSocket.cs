using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Networking
{
    /// <summary>
    /// Extends the ReloadedSocket Class by providing a buffer for the sending and receiving of data for the mod loader's
    /// sockets.
    /// </summary>
    public class ReloadedSocket
    {
        /// <summary>
        /// Holds the send and receive buffer for the current socket.
        /// </summary>
        public byte[] ReceiveBuffer = new byte[32767];

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

            // Set size of the buffer.
            socket.ReceiveBufferSize = ReceiveBuffer.Length;
            socket.SendBufferSize = ReceiveBuffer.Length;
        }
    }
}
