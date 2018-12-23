using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded.Paths;
using Console = Colorful.Console;

namespace Reloaded_Loader.Terminal.Information
{
    public static class Information
    {
        /// <summary>
        /// Displays a warning about how to use the mod loader.
        /// </summary>
        public static void DisplayHelp()
        {
            // Print
            LoaderConsole.PrintFormattedMessage("No game to launch has been specified.", LoaderConsole.PrintError);
            LoaderConsole.PrintInfo
            (
                "\nCommand Line Reloaded Mod Loader Usage Instructions:\n" +
                "Reloaded-Loader.exe <Arguments>\n\n" +

                "Arguments List:\n" +
                $"{Strings.Common.LoaderSettingConfig} <GAME_CONFIGURATION_PATH> | Specifies the game configuration to run.\n" +
                $"{Strings.Common.LoaderSettingAttach} <EXECUTABLE_NAME> | Attaches to an already running game/executable.\n\n" +
                $"{Strings.Common.LoaderSettingNoReattach} | If this is set to true; the loader will not reattach.\n\n" +
                $"{Strings.Common.LoaderSettingAutoAttach} | Waits until the game executable spawns then attaches to it. This is a flag you specify WITH {Strings.Common.LoaderSettingAttach}\n\n" +

                "Examples:\n" +
                $"Reloaded-Loader.exe {Strings.Common.LoaderSettingConfig} D:/Reloaded/Reloaded-Config/Games/Sonic-Heroes\n" +
                $"Reloaded-Loader.exe {Strings.Common.LoaderSettingConfig} D:/Reloaded/Reloaded-Config/Games/Sonic-Heroes {Strings.Common.LoaderSettingAttach} Tsonic_win_custom.exe\n\n"
            );
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
