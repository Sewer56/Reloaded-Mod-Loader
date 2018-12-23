using System;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using LiteNetLib.Utils;
using Reloaded.Networking.Message;

namespace Reloaded.Networking.Host
{
    /// <summary>
    /// Provides a basic easy to use wrapper around LiteNetLib that can be used as a client or server implementation.
    /// </summary>
    public class Client
    {
        private Dictionary<ushort, MessageFunctionPair> _messageHandlingMapping = new Dictionary<ushort, MessageFunctionPair>(ushort.MaxValue);

        /// <summary>
        /// This is the actual <see cref="NetManager"/> host used to communicate with the clients.
        /// </summary>
        public NetManager Host { get; private set; }

        /// <summary>
        /// Allows for subscribing to network events such as peers joining and other arbitrary actions.
        /// </summary>
        public EventBasedNetListener EventListener { get; private set; }

        /// <summary>
        /// Contains the password used for joining this client/server.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Client"/> with a given password and parameters.
        /// After creation; use <see cref="SetHandler"/>.
        /// </summary>
        /// <param name="port">The port with which the server will be started.</param>
        /// <param name="localServer">If this is true; the server will be hosted over the local machine and will be inaccessible from the internet.</param>
        /// <param name="disconnectTimeout">If#<see cref="Host"/> doesn't receive any packet from remote peer during this time then connection will be closed</param>
        /// <param name="maxConnections">The maximum connections allowed to join the server.</param>
        /// <param name="password">The pass phrase necessary for joining the server.</param>
        public Client(bool localServer = true, int disconnectTimeout = 1500, ushort maxConnections = 65535, ushort port = 13370, string password = "El Psy Kongroo")
        {
            // Set server password.
            Password = password;

            // Create new server instance.
            EventListener   = new EventBasedNetListener();
            Host            = new NetManager(EventListener, maxConnections, Password);

            // Ping when message is received.
            EventListener.NetworkReceiveEvent += OnNetworkReceive;

            #if DEBUG
            Host.DisconnectTimeout = Int64.MaxValue;
            #else
            Host.DisconnectTimeout = disconnectTimeout;
            #endif

            Host.UnsyncedEvents = true;

            if (localServer)
                Host.Start(IPAddress.Loopback, IPAddress.IPv6Loopback, port);
            else
                Host.Start(port);
        }

        /// <summary>
        /// Registers a function to handle a message (<see cref="messageHandler"/>) to a specific "channel".
        /// This causes all messages with a specific <see cref="channel"/> to be sent to said specific <see cref="messageHandler"/>.
        /// </summary>
        /// <param name="fromBytes">Converts an array of bytes into an instance of <see cref="IMessage"/>. Simply pass your implementation of <see cref="IMessage.FromBytes"/> function into this field.</param>
        /// <param name="messageHandler">The function used to process messages received from the client. Inside this function; cast the <see cref="IMessage"/> instance to your proper type.</param>
        /// <param name="channel">This specifies the channel to map to your function..</param>
        public void SetHandler(MessageFunctionPair.FromBytes fromBytes, MessageFunctionPair.HandleMessage messageHandler, ushort channel = 0)
        {
            _messageHandlingMapping[channel] = new MessageFunctionPair(fromBytes, messageHandler);
        }

        /// <summary>
        /// Removes an existing message handler assigned to a specific channel.
        /// </summary>
        /// <param name="channel">The channel to remove the message handler for.</param>
        public void RemoveHandler(ushort channel)
        {
            _messageHandlingMapping.Remove(channel);
        }

        /// <summary>
        /// Returns true if a handler for a specific channel is assigned.
        /// </summary>
        /// <param name="channel">The channel check.</param>
        public bool IsHandlerAssigned(ushort channel)
        {
            if (_messageHandlingMapping.ContainsKey(channel))
                return true;

            return false;
        }

        /// <summary>
        /// Default handler for incoming messages.
        /// </summary>
        private void OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            // Parse Message struct; convert internal message from bytes and send to handler.
            Message.Message message = Message.Message.FromBytes(reader.GetRemainingBytes());
            if (_messageHandlingMapping.TryGetValue(message.Header.MessageChannel, out var handler))
            {
                var iMessage = handler.FromBytesFunction(message.RawData);
                handler.MessageHandleFunction(iMessage, peer);
            }
        }
    }
}
