using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Networking;
using Reloaded.Networking.MessageTypes;
using Reloaded_Loader.Terminal;

namespace Reloaded_Loader.Networking.LoaderServerFunctions
{
    /// <summary>
    /// Decodes a user sent message and prints it to the standard output.
    /// </summary>
    public static class PrintToScreen
    {
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
        /// Prints a message received from a client to the console/standard output.
        /// </summary>
        /// <param name="printMessageType">The message colour/type to print.</param>
        /// <param name="asciiMessage">The character array of ASCII bytes to parse back to string. (MessageStruct.Data)</param>
        /// <param name="socket">Sends a message back to signify that the message has successfully printed.</param>
        public static void Print(PrintMessageType printMessageType, byte[] asciiMessage, ReloadedSocket socket)
        {
            // Retrieve the message to print.
            string messageToPrint = Encoding.ASCII.GetString(asciiMessage);

            // Print to screen.
            switch (printMessageType)
            {
                case PrintMessageType.PrintText:
                    ConsoleFunctions.PrintMessageWithTime(messageToPrint, ConsoleFunctions.PrintMessage);
                    break;


                case PrintMessageType.PrintInfo:
                    ConsoleFunctions.PrintMessageWithTime(messageToPrint, ConsoleFunctions.PrintInfoMessage);
                    break;


                case PrintMessageType.PrintWarning:
                    ConsoleFunctions.PrintMessageWithTime(messageToPrint, ConsoleFunctions.PrintWarningMessage);
                    break;


                case PrintMessageType.PrintError:
                    ConsoleFunctions.PrintMessageWithTime(messageToPrint, ConsoleFunctions.PrintErrorMessage);
                    break;
            }

            // Send pingback
            Message.MessageStruct pingOk = new Message.MessageStruct((ushort)LoaderServerMessages.MessageType.Okay, new byte[1]);
            socket.SendData(pingOk, false);
        }
    }
}
