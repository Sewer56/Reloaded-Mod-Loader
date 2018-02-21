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
using System.Collections.Generic;
using System.Linq;

namespace Reloaded.Networking
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
