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

using System.Text;
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
        public static void Print(PrintMessageType printMessageType, byte[] asciiMessage)
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
        }
    }
}
