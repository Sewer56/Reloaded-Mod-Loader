using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reloaded_Plugin_System.Config.Loader;
using Reloaded_Plugin_System.Interfaces.Loader;
using Console = Colorful.Console;

namespace Console_Colourizer
{
    public class ColourizerImplementation : ILoaderBehaviourV1
    {
        /*
            Important note for those who do not notice:
                Our Console class used here originates from Colorful.Console.
        */

        /// <summary>
        /// Stores a copy of all of the colours that can be used for printing to the screen.
        /// </summary>
        public List<Color> AllColours = new List<Color>();
        public Random Random = new Random();
        public Settings Settings;

        /// <summary>
        /// Used in the mode that loops colours instead of selecting random.
        /// </summary>
        public int IndexCounter = 0;

        /*
            --------------
            Initialization
            --------------
        */

        public ColourizerImplementation()
        {
            Settings = Settings.ParseSettings();
        }

        /*
            ------------------------
            Interface Implementation
            ------------------------
        */

        /// <summary>
        /// We override the methods called for printing an individual message with our own printmessage.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public ConsoleOptions SetConsoleOptions(ConsoleOptions options)
        {
            options.PrintTextMessage = PrintTextMessage;
            options.PrintErrorMessage = PrintTextMessage;
            options.PrintInfoMessage = PrintTextMessage;
            options.PrintWarningMessage = PrintTextMessage;
            return options;
        }

        /// <summary>
        /// Builds a copy of our colours we will use for printing.
        /// </summary>
        public ConsoleColours SetConsoleColours(ConsoleColours colours)
        {
            AllColours.Add(colours.TextColor);

            // Red
            AllColours.Add(colours.ColorRed);
            AllColours.Add(colours.ColorRedLight);

            // Pink
            AllColours.Add(colours.ColorPink);
            AllColours.Add(colours.ColorPinkLight);

            // Blue
            AllColours.Add(colours.ColorBlue);
            AllColours.Add(colours.ColorBlueLight);

            // LBlue
            AllColours.Add(colours.ColorLightBlue);
            AllColours.Add(colours.ColorLightBlueLight);

            // Green
            AllColours.Add(colours.ColorGreen);
            AllColours.Add(colours.ColorGreenLight);

            // Yellow
            AllColours.Add(colours.ColorYellow);
            AllColours.Add(colours.ColorYellowLight);

            return colours;
        }

        /// <summary>
        /// Add an extra message that may be printed at launch.
        /// </summary>
        public BannerOptions SetBannerOptions(BannerOptions options)
        {
            options.RandomMessages.Add("Not a fan of colour coding? Taste the rainbow!");
            return options;
        }

        /*
            ----------
            Logic Code
            ----------
        */

        /// <summary>
        /// Selects a random colour from the list of console colours.
        /// </summary>
        private Color SelectNextColour()
        {
            IndexCounter += 1;
            if (IndexCounter >= AllColours.Count)
                IndexCounter = 0;

            return AllColours[IndexCounter];
        }


        /// <summary>
        /// Selects a random colour from the list of console colours.
        /// </summary>
        private Color SelectRandomColour()
        {
            int randomIndex = Random.Next(AllColours.Count);
            return AllColours[randomIndex];
        }

        /// <summary>
        /// Prints a message to the console with a pseudo-random colour.
        /// </summary>
        /// <param name="message"></param>
        private void PrintTextMessage(string message)
        {
            Color printingColor = Color.White;

            if (Settings.PickRandom)
                printingColor = SelectRandomColour();
            else
                printingColor = SelectNextColour();

            Console.WriteLine(message, printingColor);
        }

        /* Ignore these aspects of the interface. We only want to edit the print functions. */
        public ControllerOptions SetControllerOptions(ControllerOptions controllerOptions) => controllerOptions;
    }
}
