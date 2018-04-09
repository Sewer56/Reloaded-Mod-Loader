/*
    [Reloaded] Mod Loader Application Loader
    The main loader, which starts up an application loader and using DLL Injection methods
    provided in the main library initializes modifications for target games and applications.
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

using Reloaded.Networking;
using Reloaded.Networking.ModLoaderServer;
using Reloaded.Networking.Sockets;

namespace Reloaded_Loader.Networking.LoaderServerFunctions
{
    /// <summary>
    /// Replies an "Okay" message back to the mod loader client which needlessly sends an "Okay" message.
    /// The message can be used for testing if required.
    /// </summary>
    public static class ReplyOkay
    {
        /// <summary>
        /// Automatically Replies "Okay" to a client.
        /// </summary>
        /// <param name="clientSocket">The socket to which the "ok" message should be sent.</param>
        public static void ReplyOk(ReloadedSocket clientSocket)
        {
            // Send back empty message struct
            Message.MessageStruct messageStruct = new Message.MessageStruct();
            messageStruct.MessageType = (ushort)MessageTypes.MessageType.Okay;
            messageStruct.Data = new byte[1];

            clientSocket.SendData(messageStruct, false);
        }
    }
}
