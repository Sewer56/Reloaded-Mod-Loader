using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Reloaded.Networking
{
    /// <summary>
    /// Extends the Socket Class by providing a buffer for the sending and receiving of data for the mod loader's
    /// sockets.
    /// </summary>
    public class ReloadedSocket : Socket
    {
        /// <summary>
        /// Holds the send and receive buffer for the current socket.
        /// </summary>
        public byte[] ReceiveBuffer = new byte[65535];

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

        // Constructores
        public ReloadedSocket(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType) { }
        public ReloadedSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) { }
        public ReloadedSocket(SocketInformation socketInformation) : base(socketInformation) { }
    }
}
