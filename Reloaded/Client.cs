using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Networking;
using Reloaded.Networking.MessageTypes;
using static Reloaded.Networking.MessageTypes.LoaderServerMessages;

namespace Reloaded_Mod_Template.Reloaded_Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// The client of the Mod Loader Server. 
        /// Can be used for communication with the external local server (hosted in the CMD window).
        /// </summary>
        public static Reloaded.Networking.Client serverClient;

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
            Message.MessageStruct clientMessage = new Message.MessageStruct((ushort)MessageType.PrintText, bytesToSend);
            
            // Switch message type if necessary.
            switch (printMessageType)
            {
                case PrintMessageType.PrintInfo: clientMessage.MessageType = (ushort)MessageType.PrintInfo; break;
                case PrintMessageType.PrintError: clientMessage.MessageType = (ushort)MessageType.PrintError; break;
                case PrintMessageType.PrintWarning: clientMessage.MessageType = (ushort)MessageType.PrintWarning; break;
            }
             
            // Send the message.
            serverClient.ClientSocket.SendData(clientMessage, true);
        }
    }
}
