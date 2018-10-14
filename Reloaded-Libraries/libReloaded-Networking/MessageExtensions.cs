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

namespace libReloaded_Networking
{
    /// <summary>
    /// Extension methods for the <see cref="Message"/> class.
    /// Provides the code used for building and parsing messages.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Builds a message to be sent to the machine A to machine B.
        /// A message consists of a message type (two bytes), followed by the raw data of the message, 
        /// which forms the remaining part of the message (this is stored in a struct).
        /// </summary>
        public static byte[] GetBytes(this Message message)
        {
            // Allocate enough data to form the message.
            List<byte> messageData = new List<byte>(message.MessageLength);

            // Append the message length.
            messageData.AddRange(BitConverter.GetBytes(message.MessageLength));

            // Append the message type.
            messageData.AddRange(BitConverter.GetBytes(message.MessageType));

            // Append the raw message.
            messageData.AddRange(message.Data);

            // Return the message raw data.
            return messageData.ToArray();
        }

        /// <summary>
        /// Constructs a MessageStruct from a series of received bytes from another machine or source.
        /// Note that the received bytes here, does not include the MessageLength and is the received rest of the information.
        /// A MessageStruct consists of a message type (two bytes), followed by the raw data of the message, 
        /// which forms the remaining part of the message.
        /// </summary>
        /// <returns></returns>
        public static Message GetMessage(this Message message, byte[] data)
        {
            // Instantiate the MessageStruct
            message = new Message
            {
                MessageType = BitConverter.ToUInt16(data, 0),
                Data = data.Skip(sizeof(ushort)).ToArray()
            };

            // Return the message struct.
            return message;
        }
    }
}
