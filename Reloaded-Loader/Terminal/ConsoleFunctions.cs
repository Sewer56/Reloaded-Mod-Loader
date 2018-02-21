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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Reloaded_Loader.Terminal
{
    public static class ConsoleFunctions
    {

        /// <summary>
        /// Delegate used for the dynamic printing of text to the command line.
        /// Used for extension and arbitrary methods, such as print to center of console,
        /// while combining it with existing colour and format print messages.
        /// </summary>
        /// <param name="message">The message to print to the screen.</param>
        public delegate void PrintTextDelegate(string message);

        /// <summary>
        /// Delegate used for passing onward
        /// Used for extension and arbitrary methods, such as print to center of console,
        /// while combining it with existing colour and format print messages.
        /// </summary>
        /// <param name="message">The message to print to the screen.</param>
        /// <param name="printTextDelegate">Delegate to the internal/nested method used for the combining of text formatting specifiers.</param>
        public delegate void PrintFormattedTextDelegate(string message, PrintTextDelegate printTextDelegate);

        // ///////////////
        // Console Colours
        // ///////////////

        // Main Colours
        public static Color BackgroundColor = Color.FromArgb(20, 25, 31);
        public static Color TextColor = Color.FromArgb(239, 240, 235);

        // Highlighting Remaining 14 Colours

        // Reds
        public static Color ColorRed = Color.FromArgb(255, 92, 87);
        public static Color ColorRedLight = Color.FromArgb(220, 163, 163);

        // Greens
        public static Color ColorGreen = Color.FromArgb(90, 247, 142);
        public static Color ColorGreenLight = Color.FromArgb(195, 191, 159);

        // Yellows
        public static Color ColorYellow = Color.FromArgb(243, 249, 157);
        public static Color ColorYellowLight = Color.FromArgb(240, 223, 175);

        // Blues
        public static Color ColorBlue = Color.FromArgb(87, 199, 255);
        public static Color ColorBlueLight = Color.FromArgb(148, 191, 243);

        // Pink
        public static Color ColorPink = Color.FromArgb(255, 106, 193);
        public static Color ColorPinkLight = Color.FromArgb(236, 147, 211);

        // Light Blue
        public static Color ColorLightBlue = Color.FromArgb(154, 237, 254);
        public static Color ColorLightBlueLight = Color.FromArgb(147, 224, 227);

        /// <summary>
        /// Initializes the console settings to new defaults.
        /// i.e. Sets the default background and foreground colour.
        /// </summary>
        public static void Initialize()
        {
            // Set the background console colour.
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = TextColor;
            Console.WindowWidth = 122;
            Console.WindowHeight = 32;

            // Clear the console buffer.
            Console.Clear();
        }

        /// <summary>
        /// Obtains the current time for both logging purposes and
        /// extra information for the end user.
        /// </summary>
        public static string GetCurrentTime()
        {
            return "[" + DateTime.Now.ToString("hh:mm:ss") + "] ";
        }

        /// <summary>
        /// Prints an error message with the appropriate console formatting and 
        /// text colours.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        public static void PrintErrorMessage(string message)
        {
            Console.WriteLine(message, ColorRed);
        }

        /// <summary>
        /// Prints an informational message with the appropriate console formatting and 
        /// text colours.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        public static void PrintInfoMessage(string message)
        {
            Console.WriteLine(message, ColorBlueLight);
        }

        /// <summary>
        /// Prints a warning message with the appropriate console formatting and 
        /// text colours.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        public static void PrintWarningMessage(string message)
        {
            Console.WriteLine(message, ColorYellowLight);
        }

        /// <summary>
        /// Prints a regular message with the appropriate console formatting and 
        /// text colours.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        public static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes a message to the center of the screen, accepts a delegate object,
        /// redirecting execution to the appropriate method for printing the text
        /// in an appropriate colour.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printTextDelegate">The function that will actually print the message to the screen</param>
        public static void PrintMessageCenter(string message, PrintTextDelegate printTextDelegate)
        {
            // If the message is multi-line, split it into a string array and call self to print
            // message to the center to the screen.
            if (message.Contains("\n"))
            {
                // Lines to print
                string[] lines = Regex.Split(message, "\n");

                // Call self to print lines.
                foreach (string line in lines) {
                    PrintMessageCenter(line, printTextDelegate);
                }

                // Early exit
                return;
            }

            // Obtain the left side/left edge of the text we are going to write.
            int consolePointer = (Console.WindowWidth - message.Length) / 2;

            // Check/resolve text overflow possibilities.
            if (consolePointer < 0) { consolePointer = 0; }

            // Set the cursor position to our new left edge.
            Console.SetCursorPosition(consolePointer, Console.CursorTop);

            // Call our delegate to print the message.
            printTextDelegate(message);
            
            // Restore the left side/left edge pointer position.
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        /// <summary>
        /// Writes a message to the center of the screen, accepts a delegate object,
        /// redirecting execution to the appropriate method for printing the text
        /// in an appropriate colour.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printTextDelegate">The function that will actually print the message to the screen</param>
        /// <param name="printFormattedTextDelegate">Used for recursive declaration of text formatting/styles.</param>
        public static void PrintMessageCenter(string message, PrintTextDelegate printTextDelegate, PrintFormattedTextDelegate printFormattedTextDelegate)
        {
            // Obtain the left side/left edge of the text we are going to write.
            int consolePointer = (Console.WindowWidth - message.Length) / 2;

            // Check/resolve text overflow possibilities.
            if (consolePointer < 0) { consolePointer = 0; }

            // Set the cursor position to our new left edge.
            Console.SetCursorPosition(consolePointer, Console.CursorTop);

            // Call our delegate to print the message.
            printFormattedTextDelegate(message, printTextDelegate);

            // Restore the left side/left edge pointer position.
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        /// <summary>
        /// Writes a message to the with the current time to the screen, accepts a delegate object,
        /// redirecting execution to the appropriate method for printing the text
        /// in an appropriate colour.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printTextDelegate">The function that will actually print the message to the screen</param>
        public static void PrintMessageWithTime(string message, PrintTextDelegate printTextDelegate)
        {
            // Call our delegate to print the message.
            printTextDelegate(GetCurrentTime() + message);
        }

        /// <summary>
        /// Writes a message to the with the current time to the screen, accepts a delegate object,
        /// redirecting execution to the appropriate method for printing the text
        /// in an appropriate colour.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printTextDelegate">The function that will actually print the message to the screen</param>
        /// <param name="printFormattedTextDelegate">Used for recursive declaration of text formatting/styles.</param>
        public static void PrintMessageWithTime(string message, PrintTextDelegate printTextDelegate, PrintFormattedTextDelegate printFormattedTextDelegate)
        {
            // Call our passed in delegate with message.
            printFormattedTextDelegate(GetCurrentTime() + message, printTextDelegate);
        }
    }
}
