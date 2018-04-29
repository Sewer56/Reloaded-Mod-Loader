using System.Text;
using Reloaded.Networking;
using Reloaded.Networking.ModLoaderServer;
using Reloaded.Networking.Sockets;

namespace Reloaded_Mod_Template.Reloaded
{
    /// <summary>
    /// Class used for providing services via communication with the Mod Loader server.
    /// Currently provides methods used for printing text.
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// The client of the Mod Loader Server. 
        /// Can be used for communication with the external local server (hosted in the CMD window).
        /// </summary>
        public static global::Reloaded.Networking.Client ServerClient;

        /// <summary>
        /// Defines the message types that could be printed to the screen.
        /// </summary>
        public enum PrintMessageType
        {
            /// <summary>
            /// Prints the message in the default white/gray colour.
            /// </summary>
            PrintText,

            /// <summary>
            /// Prints in the blue/information colour.
            /// </summary>
            PrintInfo,

            /// <summary>
            /// Prints in the yellow/orange warning colour.
            /// </summary>
            PrintWarning,

            /// <summary>
            /// Prints in the red/failure error colour.
            /// </summary>
            PrintError
        }

        /// <summary>
        /// Sends a message to the mod loader server to print
        /// a message to standard output.
        /// </summary>
        /// <param name="message">The text to print to the console.</param>
        public static void Print(string message)
        {
            // Print in default style
            Print(message, PrintMessageType.PrintText);
        }

        /// <summary>
        /// Sends a informational message to the mod loader server to print
        /// to the standard output.
        /// </summary>
        /// <param name="message">The message to be printed to standard output.</param>
        public static void PrintInfo(string message) { Print(message, PrintMessageType.PrintInfo); }

        /// <summary>
        /// Sends a warning message to the mod loader server to 
        /// print to the standard output.
        /// </summary>
        /// <param name="message">The message to be printed to standard output.</param>
        public static void PrintWarning(string message) { Print(message, PrintMessageType.PrintWarning); }

        /// <summary>
        /// Sends a error message to the mod loader server to print to the standard output.
        /// </summary>
        /// <param name="message">The message to be printed to standard output.</param>
        public static void PrintError(string message) { Print(message, PrintMessageType.PrintError); }

        /// <summary>
        /// Sends a message to the mod loader server to print
        /// a message to standard output with a user defined style.
        /// </summary>
        /// <param name="message">The text to print to the console.</param>
        /// <param name="printMessageType">The style of text to be printed to the screen.</param>
        public static void Print(string message, PrintMessageType printMessageType)
        {
            // Get string to print as bytes.
            byte[] bytesToSend = Encoding.ASCII.GetBytes(message);

            // Build a Server Message with text to print.
            // The style of the message is user-set.
            Message.MessageStruct clientMessage = new Message.MessageStruct((ushort)MessageTypes.MessageType.PrintText, bytesToSend);
            
            // Switch message type if necessary.
            switch (printMessageType)
            {
                case PrintMessageType.PrintInfo: clientMessage.MessageType = (ushort)MessageTypes.MessageType.PrintInfo; break;
                case PrintMessageType.PrintError: clientMessage.MessageType = (ushort)MessageTypes.MessageType.PrintError; break;
                case PrintMessageType.PrintWarning: clientMessage.MessageType = (ushort)MessageTypes.MessageType.PrintWarning; break;
            }
             
            // Send the message.
            ServerClient.ClientSocket.SendData(clientMessage, false);
        }
    }
}
