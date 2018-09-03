using System;
using System.Collections.Generic;

namespace Reloaded_Plugin_System.Config.Loader
{
    public class BannerOptions
    {
        /// <summary>
        /// Defines a delegate that should choose how an individual random message should be printed to the console.
        /// </summary>
        public delegate void PrintRandomMessageDelegate(string message);

        /// <summary>
        /// Selects an individual random message from a list of messages.
        /// </summary>
        public delegate string SelectRandomMessageDelegate(List<string> messages);

        /// <summary>
        /// Defines the individual function that prints the randomly picked out message.
        /// </summary>
        public PrintRandomMessageDelegate PrintRandomMessage;

        /// <summary>
        /// Defines the individual function that selects the random message to be printed.
        /// </summary>
        public SelectRandomMessageDelegate SelectRandomMessage;

        /// <summary>
        /// This function is used to print Reloaded's Mod Loader banner at the start of the loader.
        /// </summary>
        public Action PrintBannerFunction;

        /// <summary>
        /// Defines the list of Random Messages.
        /// You can either replace the message set with your own or extend it. Your pick.
        /// </summary>
        public List<string> RandomMessages;
    }
}
