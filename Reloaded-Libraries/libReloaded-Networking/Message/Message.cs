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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Reloaded.Memory;
using Reloaded.Utilities.Arrays;

namespace Reloaded.Networking.Message
{
    /// <summary>
    /// Helper class that helps with the sending of different messages between client and the host.
    /// </summary>
    public struct Message
    {
        /*
            ---------
            Structure
            ---------
        */

        /// <summary>
        /// Contains the header of the current message.
        /// </summary>
        public MessageHeader Header;

        /// <summary>
        /// The raw data of the message in question.
        /// </summary>
        public byte[] RawData;

        /*
            -----------
            Constructor
            -----------
        */

        /// <summary>
        /// Constructs an instance of <see cref="Message"/> from the raw <see cref="IMessage"/> wrapper.
        /// </summary>
        /// <param name="message">The implementation of <see cref="IMessage"/> to send over to the client/server.</param>
        /// <param name="messageChannel">The channel used for sending/receiving the message.</param>
        public Message(IMessage message, ushort messageChannel)
        {
            RawData                 = message.GetBytes();
            Header.MessageChannel   = messageChannel;
        }

        /*
            -------
            Methods
            -------
        */

        /// <summary>
        /// Constructs a <see cref="Message"/> from a supplied byte array.
        /// </summary>
        public static Message FromBytes(byte[] data)
        {
            // Unsafe grab the header.
            MessageHeader messageHeader = Struct.FromArray<MessageHeader>(data);

            // Create a new message with header and data.
            Message message = new Message();
            message.Header  = messageHeader;
            message.RawData = data.Slice(Unsafe.SizeOf<MessageHeader>(), data.Length);

            return message;
        }

        /// <summary>
        /// Converts the current instance of <see cref="Message"/> into a byte array for sending.
        /// </summary>
        public byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>(RawData.Length + Unsafe.SizeOf<MessageHeader>());
            bytes.AddRange(Struct.GetBytes(ref Header));
            bytes.AddRange(RawData);
            return bytes.ToArray();
        }
    }
}
