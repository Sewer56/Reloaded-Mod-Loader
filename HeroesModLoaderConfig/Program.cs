using System;
using System.Windows.Forms;

namespace HeroesModLoaderConfig
{
    static class Program
    {
        /// <summary>
        /// Defines the initializer class which starts up the Windows Forms application.
        /// </summary>
        static Initializer initializer;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable Visual Styles
            Application.EnableVisualStyles();

            // Set Compatible Text Rendering Defs.
            Application.SetCompatibleTextRenderingDefault(false);

            // Call the Program Initializer.
            initializer = new Initializer();
        }
    }
}
