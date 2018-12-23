using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Reloaded.Networking.Message
{
    /// <summary>
    /// Contains the message header seen at the start of every struct.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MessageHeader
    {
        /// <summary>
        /// The channel of the message.
        /// This value is by default inherited from <see cref="IMessage.GetDefaultMessageChannel()"/>.
        /// </summary>
        public ushort MessageChannel;
    }
}
