using SonicHeroes.Native;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SonicHeroes.Overlay.External
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of the game, if the game is in windowed mode.
    /// </summary>
    public class OverlayGlassDirect2D : Direct2DWindowRenderTargetBase
    {
        /// <summary>
        /// Fake glass form which we will be overlaying the game with.
        /// </summary>
        public GlassForm OverlayForm { get; set; }

        /// <summary>
        /// A handle to the game window which is found via searching for a window with a title defined in gameWindowName.
        /// </summary>
        public IntPtr GameWindowHandle { get; set; }

        /// <summary>
        /// Defines the name of the game window, this name is used and matched against existing windows to detect window over which to overlay.
        /// </summary>
        public string GameWindowName { get; set; }

        /// <summary>
        /// A thread which hosts the glass overlay windows form, ensuring that it keeps running.
        /// </summary>
        public Thread WindowsFormThread { get; set; }

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// </summary>
        public OverlayGlassDirect2D(string gameWindowName)
        {
            // Set Window Name
            this.GameWindowName = gameWindowName;

            // Wait for and find the game window.
            Find_Game_Window();

            // Instantiate glass form
            OverlayForm = new GlassForm(GameWindowHandle);

            // Initialize base (directX Drawing Stuff)
            base.ConstructorAlias(OverlayForm.Handle);
        }

        /// <summary>
        /// Calls Application.Run to host the overlay glass window such that it may be displayed.
        /// Want multiple windows (for some reason?). Create a thread and call this method.
        /// </summary>
        public void Enable_Overlay()
        {
            // Enable the Overlay Window.
            Application.Run(OverlayForm);
        }

        /// <summary>
        /// Waits for an instance of the game window to spawn (searches window by name).
        /// Proceeds to wait until said window is visible to the end user.
        /// </summary>
        private void Find_Game_Window()
        {
            // Wait for Game Window to spawn.
            while (true)
            {
                // Get the handle for the Sonic_Heroes Window
                GameWindowHandle = WinAPI.Windows.FindWindow(null, GameWindowName);

                // If handle successfully acquired.
                if (GameWindowHandle != null) { break; }

                // Sleep to reduce CPU load.
                Thread.Sleep(16);
            }

            // Wait for the Window to show itself to screen before configuring.
            while (WinAPI.Windows.IsWindowVisible(GameWindowHandle) == false) { Thread.Sleep(16); }
        }
    }
}
