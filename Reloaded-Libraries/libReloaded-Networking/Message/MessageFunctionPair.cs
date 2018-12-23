using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;

namespace Reloaded.Networking.Message
{
    public struct MessageFunctionPair
    {
        /// <summary>
        /// A clone of <see cref="IMessage.FromBytes"/>
        /// </summary>
        public delegate IMessage FromBytes(byte[] data);

        /// <summary>
        /// Defines a delegate used to define a function that handles an oncoming message.
        /// </summary>
        /// <param name="message">The message received from the server. Cast this to your specific message type.</param>
        /// <param name="peer">The peer that sent the message.</param>
        public delegate void HandleMessage(IMessage message, NetPeer peer);

        public FromBytes FromBytesFunction;
        public HandleMessage MessageHandleFunction;

        /// <summary>
        /// Creates a new Message and Function; which is mapped to a message channel and used to
        /// convert and send received messages to the handling function.
        /// </summary>
        /// <param name="fromBytesFunction">Function from <see cref="IMessage"/> that converts bytes to the message instance.</param>
        /// <param name="messageHandleFunction">The function used to process the received message.</param>
        public MessageFunctionPair(FromBytes fromBytesFunction, HandleMessage messageHandleFunction)
        {
            FromBytesFunction = fromBytesFunction;
            MessageHandleFunction = messageHandleFunction;
        }
    }
}
