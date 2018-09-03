using System;

namespace Reloaded_Plugin_System.Config.Loader
{
    public class ConsoleOptions
    {
        public delegate void PrintFormattedMessageDel(string message, PrintMessage printMessage);
        public delegate void PrintMessage            (string message);

        /*
            The individual print methods for each message type.
            These allow you to print to console with your own
            colour out of the standard colours palette.
        */

        /// <summary>
        /// Used to adjust the text of the individual message
        /// before it is printed to the screen (by default append time).
        /// After changing the text, simply call the delegate passed in the parameter.
        /// </summary>
        public PrintFormattedMessageDel PrintFormattedMessage   ;
        public PrintMessage PrintErrorMessage                   ;
        public PrintMessage PrintInfoMessage                    ;
        public PrintMessage PrintWarningMessage                 ;
        public PrintMessage PrintTextMessage                    ;

        public Action       InitConsole                         ;
    }
}
