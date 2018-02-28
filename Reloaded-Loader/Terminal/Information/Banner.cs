using System;
using Colorful;
using Reloaded.IO;
using Reloaded.Misc;
using Console = Colorful.Console;

namespace Reloaded_Loader.Terminal.Information
{
    internal static class Banner
    {
        /// <summary>
        /// Prints all of the text to be displayed on startup of the Reloaded Launcher.
        /// </summary>
        public static void PrintBanner()
        {
            // Print the banner
            PrintBannerInternal();

            // Print semi-random message
            PrintRandomMessage();
        }

        /// <summary>
        /// Displays a warning about how to use the mod loader.
        /// </summary>
        public static void DisplayWarning()
        {
            // Print
            ConsoleFunctions.PrintMessageWithTime("No game to launch has been specified.", ConsoleFunctions.PrintErrorMessage);
            ConsoleFunctions.PrintInfoMessage
            (
                "\nCommand Line Reloaded Mod Loader Usage Instructions:\n" +
                "Reloaded-Loader.exe <Arguments>\n\n" +

                "Arguments List:\n" +
                "--config <GAME_CONFIGURATION_PATH> | Specifies the game configuration to run.\n" +
                "--attach <EXECUTABLE_NAME> | Append after --config to load mods to an arbitrary executable name.\n\n" +
                
                "Examples:\n" +
                "Reloaded-Loader.exe --config D:/Reloaded/Reloaded-Config/Games/Sonic-Heroes\n" +
                "Reloaded-Loader.exe --config D:/Reloaded/Reloaded-Config/Games/Sonic-Heroes --attach Tsonic_win_custom.exe\n\n" +

                "Notes:\n" +
                "--attach is a suffix parameter and should be used last, if no executable name is specified\n" +
                "it tries to guess it from the executable path in the game configuration."
            );
            Console.ReadLine();
            Environment.Exit(0);
        }

        /// <summary>
        /// Prints the Reloaded Logo banner,
        /// </summary>
        private static void PrintBannerInternal()
        {
            // Print empty line (spacing)
            ConsoleFunctions.PrintMessage("");

            // Reloaded Banner Stylesheet
            Formatter[] reloadedFormatter = {
                new Formatter(@"MMMMMMMMMMMMNmyyyyyyyyyyyy-:yy`           /syyyyyyyyo.     `/     :yyyyyyyyyys: /yyyyyyyyyyyy`+yyyyyyyyyyo.", ConsoleFunctions.TextColor),
                new Formatter(@"MMMMM   MNMMhodmhmo:::::::.:hh.          /hho:::::/yhy`   `sh/    :hh+:::::/yhh./hh/:::::::::`ohy::::::/yhy", ConsoleFunctions.TextColor),
                new Formatter(@"MMMMM   :+MMo-hh/./.....   :hh.          +hh       /hh`  `ohhh/   :hh.      -hh-/hh-......`   ohy       +hh", ConsoleFunctions.TextColor),
                new Formatter(@"MMMMM  MMMMm--dddddddddh   /dd.          odd       +dd`  sdy-dd/  :dd.      :dd-+dddddddddo   sdh       odd", ConsoleFunctions.TextColor),
                new Formatter(@"MMoyMMMMMMd/ :NN:          +NN.          sNN`      oNN``dNd::+NNs /NN-      :NN:oNN`          yNm       sNN", ConsoleFunctions.TextColor),
                new Formatter(@"MMN.m   :dMMh+hhhhhhhhhhhh::hhhhhhhhhhhh./hhhhhhhhhhhy.yhhhhhhhhho:hhhhhhhhhhhh./hhhhhhhhhhhh`ohhhhhhhhhhhy", ConsoleFunctions.TextColor),
                new Formatter(@"MMM/h     :ssso+++++++++++.-++++++++++++` :+++++++++/.:+/      `++/++++++++++/- -++++++++++++`:++++++++++:.", ConsoleFunctions.TextColor)
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
            Console.WriteLineFormatted(reloadedBannerText, ConsoleFunctions.ColorRed, reloadedFormatter);

            // Print empty line (spacing)
            ConsoleFunctions.PrintMessage("");
        }

        /// <summary>
        /// Prints a random message from a hardcoded array of messages.
        /// </summary>
        private static void PrintRandomMessage()
        {
            string[] messages = {
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

            // Random number generator.
            Random randonNumberGenerator = new Random();
            int arrayIndex = randonNumberGenerator.Next(0, messages.Length);

            // Print the message.
            ConsoleFunctions.PrintMessageCenter(messages[arrayIndex], ConsoleFunctions.PrintMessage);

            // Print Space
            Console.WriteLine();
        }
    }
}
