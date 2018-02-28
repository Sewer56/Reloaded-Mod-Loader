using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Networking;
using Reloaded.Networking.MessageTypes;

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
            messageStruct.MessageType = (ushort)LoaderServerMessages.MessageType.Okay;
            messageStruct.Data = new byte[1];
            clientSocket.SendData(messageStruct, false);
        }
    }
}
