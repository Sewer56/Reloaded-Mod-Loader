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
using System.Drawing;
using System.Text.RegularExpressions;
using Reloaded_Plugin_System;
using Console = Colorful.Console;
using Reloaded_Plugin_System.Config;
using Reloaded_Plugin_System.Config.Loader;
using static Reloaded_Plugin_System.Config.Loader.ConsoleOptions;

namespace Reloaded_Loader.Terminal
{
    public static class LoaderConsole
    {
        public static ConsoleOptions ConsoleOptions   = new ConsoleOptions();
        public static ConsoleColours Colours          = new ConsoleColours();

        /// <summary>
        /// Initializes the console settings to new defaults.
        /// i.e. Sets the default background and foreground colour.
        /// </summary>
        static LoaderConsole()
        {
            ConsoleOptions.PrintErrorMessage   = PrintErrorMessageDefault;
            ConsoleOptions.PrintInfoMessage    = PrintInfoMessageDefault;
            ConsoleOptions.PrintTextMessage    = PrintMessageDefault;
            ConsoleOptions.PrintWarningMessage = PrintWarningMessageDefault;
            ConsoleOptions.InitConsole         = InitializeConsoleDefault;
            ConsoleOptions.PrintFormattedMessage = PrintFormattedMessageDefault;

            foreach (var configPlugin in PluginLoader.LoaderConfigPlugins)
            {
                ConsoleOptions  = configPlugin.SetConsoleOptions(ConsoleOptions);
                Colours         = configPlugin.SetConsoleColours(Colours);
            }
        }

        /// <summary>
        /// Initializes the console settings to new defaults.
        /// </summary>
        public static void Initialize()
        {
            ConsoleOptions.InitConsole();
        }

        /*
            --------------------------
            Standard Message Printouts
            --------------------------
        */

        /// <summary>
        /// Prints a message with any arbitrary additional miscellaneous formatting.
        /// In our default case, it just appends the current time.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printText">The function that will actually print the message to the screen</param>
        public static void PrintFormattedMessage(string message, PrintMessage printText)
        {
            ConsoleOptions.PrintFormattedMessage(message, printText);
        }

        public static void PrintErrorMessage(string message)
        {
            ConsoleOptions.PrintErrorMessage(message);
            Logger.Append(message);
        }

        public static void PrintInfoMessage(string message)
        {
            ConsoleOptions.PrintInfoMessage(message);
            Logger.Append(message);
        }

        public static void PrintWarningMessage(string message)
        {
            ConsoleOptions.PrintWarningMessage(message);
            Logger.Append(message);
        }

        public static void PrintMessage(string message)
        {
            ConsoleOptions.PrintTextMessage(message);
            Logger.Append(message);
        }


        /*
            -----------------------------------
            No Plugins: Default Method Handlers
            -----------------------------------
        */

        private static void PrintErrorMessageDefault(string message)     => Console.WriteLine(message, Colours.ColorRed);
        private static void PrintInfoMessageDefault(string message)      => Console.WriteLine(message, Colours.ColorBlueLight);
        private static void PrintWarningMessageDefault(string message)   => Console.WriteLine(message, Colours.ColorYellowLight);
        private static void PrintMessageDefault(string message)          => Console.WriteLine(message);

        private static void InitializeConsoleDefault()
        {
            // Set the background console colour.
            Console.BackgroundColor = Colours.BackgroundColor;
            Console.ForegroundColor = Colours.TextColor;
            Console.WindowWidth     = 122;
            Console.WindowHeight    = 32;

            // Clear the console buffer.
            Console.Clear();
        }

        /// <summary>
        /// Prints a message with any arbitrary additional miscellaneous formatting.
        /// In our default case, it just appends the current time.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printText">The function that will actually print the message to the screen</param>
        public static void PrintFormattedMessageDefault(string message, PrintMessage printText)
        {
            printText(GetCurrentTime() + message);
        }

        /*
            -------------
            Miscellaneous
            -------------
        */

        /// <summary>
        /// Obtains the current time for both logging purposes and
        /// extra information for the end user.
        /// </summary>
        public static string GetCurrentTime()
        {
            return "[" + DateTime.Now.ToString("hh:mm:ss") + "] ";
        }

        /// <summary>
        /// Writes a message to the center of the screen, accepts a delegate object,
        /// redirecting execution to the appropriate method for printing the text
        /// in an appropriate colour.
        /// </summary>
        /// <param name="message">The message to be printed to the screen.</param>
        /// <param name="printText">The function that will actually print the message to the screen</param>
        public static void PrintMessageCenter(string message, PrintMessage printText)
        {
            // If the message is multi-line, split it into a string array and call self to print
            // message to the center to the screen.
            if (message.Contains("\n"))
            {
                // Lines to print
                string[] lines = Regex.Split(message, "\n");

                // Call self to print lines.
                foreach (string line in lines)
                    PrintMessageCenter(line, printText);

                // Early exit
                return;
            }

            // Obtain the left side/left edge of the text we are going to write.
            int consolePointer = (Console.WindowWidth - message.Length) / 2;

            // Check/resolve text overflow possibilities.
            if (consolePointer < 0) consolePointer = 0;

            // Set the cursor position to our new left edge, print and restore.
            Console.SetCursorPosition(consolePointer, Console.CursorTop);
            printText(message);
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
