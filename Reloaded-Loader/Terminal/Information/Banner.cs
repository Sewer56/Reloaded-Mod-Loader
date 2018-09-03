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
using Colorful;
using Reloaded;
using Reloaded.Paths;
using Reloaded_Plugin_System;
using Reloaded_Plugin_System.Config;
using Reloaded_Plugin_System.Config.Loader;
using Console = Colorful.Console;

namespace Reloaded_Loader.Terminal.Information
{
    internal static class Banner
    {
        /// <summary>
        /// Controls the individual options for the banner/info operations.
        /// Powered by Reloaded's plugin system.
        /// </summary>
        public static BannerOptions BannerOptions;

        static Banner()
        {
            // Auto prepare Banner class before first call.
            BannerOptions = new BannerOptions();
            BannerOptions.PrintBannerFunction           = PrintBannerDefault;
            BannerOptions.PrintRandomMessage            = PrintRandomMessageDefault;
            BannerOptions.SelectRandomMessage           = SelectRandomMessageDefault;

            BannerOptions.RandomMessages = new List<string>()
            {
                "Think your Mod Loader has EDGE? Think again.",
                "A Mod Loader by Sewer56?",
                "Cuteness is Justice. It is the law!",
               @"It all started here: https://discord.gg/4DHujrb",
                "null",
                "All your base are belong to us.",
                "The answer is \"Sometime between now and never.\"",
               @"Don't forget to backup Salvato's character file.",
                "\"For A Really Disturbing Image... Flip Disc Over\"",
                "Hello! Trojan:Win32/ReloadedModLoader",

                "The program 'fortune' is currently not installed.\n" +
                "You can install it by typing: sudo apt install fortune-mod"
            };

            foreach (var configPlugin in PluginLoader.LoaderConfigPlugins)
                BannerOptions = configPlugin.SetBannerOptions(BannerOptions);
        }

        /*
            --------------------
            Class Implementation
            --------------------
        */

        /// <summary>
        /// Executes all of the banner related options.
        /// </summary>
        public static void Execute()
        {
            // Print the banner
            BannerOptions.PrintBannerFunction();

            // Select random function and print it.
            string message = BannerOptions.SelectRandomMessage(BannerOptions.RandomMessages);
            BannerOptions.PrintRandomMessage(message);
        }


        /*
            -----------------------------------
            No Plugins: Default Method Handlers
            -----------------------------------
        */

        /// <summary>
        /// Prints a random message to the console window.
        /// </summary>
        /// <param name="message"></param>
        private static void PrintRandomMessageDefault(string message)
        {
            // Print the message.
            LoaderConsole.PrintMessageCenter(message, LoaderConsole.PrintMessage);
            LoaderConsole.PrintMessage("");
        }

        /// <summary>
        /// Selects a random message from the list of messages.
        /// </summary>
        private static string SelectRandomMessageDefault(List<string> messages)
        {
            // Random number generator.
            Random randonNumberGenerator = new Random();
            int arrayIndex = randonNumberGenerator.Next(0, messages.Count);
            return messages[arrayIndex];
        }

        /// <summary>
        /// Prints the Reloaded Logo banner,
        /// </summary>
        private static void PrintBannerDefault()
        {
            // Print empty line (spacing)
            LoaderConsole.PrintMessage("");

            // Reloaded Banner Stylesheet
            Formatter[] reloadedFormatter = {
                new Formatter(@"MMMMMMMMMMMMNmyyyyyyyyyyyy-:yy`           /syyyyyyyyo.     `/     :yyyyyyyyyys: /yyyyyyyyyyyy`+yyyyyyyyyyo.", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMMMM   MNMMhodmhmo:::::::.:hh.          /hho:::::/yhy`   `sh/    :hh+:::::/yhh./hh/:::::::::`ohy::::::/yhy", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMMMM   :+MMo-hh/./.....   :hh.          +hh       /hh`  `ohhh/   :hh.      -hh-/hh-......`   ohy       +hh", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMMMM  MMMMm--dddddddddh   /dd.          odd       +dd`  sdy-dd/  :dd.      :dd-+dddddddddo   sdh       odd", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMoyMMMMMMd/ :NN:          +NN.          sNN`      oNN``dNd::+NNs /NN-      :NN:oNN`          yNm       sNN", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMN.m   :dMMh+hhhhhhhhhhhh::hhhhhhhhhhhh./hhhhhhhhhhhy.yhhhhhhhhho:hhhhhhhhhhhh./hhhhhhhhhhhh`ohhhhhhhhhhhy", LoaderConsole.Colours.TextColor),
                new Formatter(@"MMM/h     :ssso+++++++++++.-++++++++++++` :+++++++++/.:+/      `++/++++++++++/- -++++++++++++`:++++++++++:.", LoaderConsole.Colours.TextColor)
            };

            // Reloaded Text to Print
            string reloadedBannerText =
@"                   ./oyo:`
          -/   `/yNMd/`
       .sms` /hMMMh-
     -hMM/ /mMMMm:
    sMMM/.dMMMMs
  `dMMMs-NMMMMo
  yMMMMoNMMMM {0}
 -MMMMMMMMMMM {1}
 yMMMMMMMNmmM {2}
 dMMMMMy.   - {3}
/MMMMMy   /hm {4}
mMMMMMy     y {5}
MMMMMMMh/::+m {6}
mMNMMMMMMMMMMMm.Nh
+M`sMMMMMMMMN+`/:
 s` -hMMMMM+
      .omMMMm+.
         -odMMMms/-`
            `:+ydmNMNdy+:`";

            // Write formatted line.
            Console.WriteLineFormatted(reloadedBannerText, LoaderConsole.Colours.ColorRed, reloadedFormatter);

            // Print empty line (spacing)
            LoaderConsole.PrintMessage("");
        }
    }
}
