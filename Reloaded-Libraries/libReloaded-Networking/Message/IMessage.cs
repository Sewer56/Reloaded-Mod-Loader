namespace Reloaded.Networking.Message
{
    /// <summary>
    /// Defines an individual Reloaded Networking library message
    /// that defines a structure that can be sent or received across the
    /// network. 
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Returns the message as a sequence of raw bytes that could be sent
        /// to another receiver and parsed with <see cref="FromBytes"/>.
        /// </summary>
        /// <returns></returns>
        byte[] GetBytes();

        /// <summary>
        /// Converts a series of bytes into an instance of the message.
        /// This method should successfully convert a byte array generated
        /// by <see cref="GetBytes"/>.
        /// </summary>
        /// <returns></returns>
        IMessage FromBytes(byte[] data);
    }
}
