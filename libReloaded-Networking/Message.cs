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
using System.Linq;

namespace libReloaded_Networking
{
    /// <summary>
    /// Helper class that helps with the sending of different messages sent over TCP between client and the host.
    /// Provides a struct for holding a message type and the contained data.
    /// </summary>
    [Serializable]
    public class Message
    {
        /// <summary>
        /// Defines the length of the individual message.
        /// The length of the message is calculated by 
        /// </summary>
        public int MessageLength { get; private set; }

        /// <summary>
        /// The type of the message sent. Types are supposed to be your
        /// own custom defined enumerables. The mod loader server uses Client_Functions.Message_Type.
        /// Cast it to the ushort type :V
        /// </summary>
        public ushort MessageType { get; set; }

        /// <summary>
        /// The raw data of the message in question.
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// The raw data of the message in question.
        /// </summary>
        public byte[] Data
        {
            get => _data;
            set
            {
                _data = value;
                MessageLength = BitConverter.GetBytes(MessageType).Length + _data.Length;
            }
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Message() { }

        /// <summary>
        /// Constructor allowing immediate struct assignment.
        /// </summary>
        public Message(ushort messageType, byte[] data)
        {
            MessageType = messageType;
            _data = data;
            MessageLength = BitConverter.GetBytes(MessageType).Length + _data.Length;
        }

        /// <summary>
        /// Constructs a MessageStruct from a series of received bytes from another machine or source.
        /// Note that the received bytes here, does not include the MessageLength and is the received rest of the information.
        /// A MessageStruct consists of a message type (two bytes), followed by the raw data of the message, 
        /// which forms the remaining part of the message.
        /// </summary>
        /// <returns></returns>
        public static Message GetMessage(byte[] data)
        {
            // Instantiate the MessageStruct
            Message messageStruct = new Message
            {
                MessageType = BitConverter.ToUInt16(data, 0),
                Data = data.Skip(sizeof(ushort)).ToArray()
            };

            // Return the message struct.
            return messageStruct;
        }
    }
}
