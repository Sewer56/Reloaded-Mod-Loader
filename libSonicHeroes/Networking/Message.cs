using System;
using System.Collections.Generic;
using System.Linq;

namespace SonicHeroes.Networking
{
    /// <summary>
    /// Helper class that helps with the identification of different messages sent over TCP between client and the host.
    /// Provides a struct for holding a message and methods 
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// A struct which defines a message to be sent over TCP or UDP.
        /// </summary>
        public struct MessageStruct
        {
            /// <summary>
            /// The type of the message sent. Types are supposed to be your
            /// own custom defined enumerables. The mod loader server uses Client_Functions.Message_Type.
            /// </summary>
            public ushort MessageType { get; set; }

            /// <summary>
            /// The raw data of the message in question.
            /// </summary>
            public byte[] Data { get; set; }

            /// <summary>
            /// Constructor allowing immediate struct assignment.
            /// </summary>
            public MessageStruct(ushort messageType, byte[] data)
            {
                MessageType = messageType;
                Data = data;
            }
        }

        /// <summary>
        /// Builds a message to be sent to the machine A to machine B.
        /// A message consists of a message type (two bytes), followed by the raw data of the message, 
        /// which forms the remaining part of the message (this is stored in a struct).
        /// </summary>
        public static byte[] BuildMessage(MessageStruct message)
        {
            // Allocate enough data to form the message.
            List<byte> messageData = new List<byte>(sizeof(ushort) + message.Data.Length);

            // Append the message type.
            messageData.AddRange(BitConverter.GetBytes(message.MessageType));

            // Append the raw message.
            messageData.AddRange(message.Data);

            // Return the message raw data.
            return messageData.ToArray();
        }

        /// <summary>
        /// Constructs a MessageStruct from a series of received bytes from another machine or source.
        /// A MessageStruct consists of a message type (two bytes), followed by the raw data of the message, 
        /// which forms the remaining part of the message.
        /// </summary>
        /// <returns></returns>
        public static MessageStruct ReceiveMessage(byte[] data)
        {
            // Instantiate the MessageStruct
            MessageStruct messageStruct = new MessageStruct();

            // Assign the Message Type from the received data.
            messageStruct.MessageType = BitConverter.ToUInt16(data, 0);

            // Copy the received data to the struct.
            messageStruct.Data = data.Skip(sizeof(ushort)).ToArray();

            // Return the message struct.
            return messageStruct;
        }

    }
}
