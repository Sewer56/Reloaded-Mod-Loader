using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using System.Threading;
using SharpDX.DXGI;
using SonicHeroes.Native;
using SonicHeroes.Overlay.External;

namespace SonicHeroes.Overlay.External
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of the game, if the game is in windowed mode.
    /// </summary>
    public class Overlay_Glass_Direct2D : Direct2D_WindowRenderTarget_Base
    {
        /// <summary>
        /// Fake glass form which we will be overlaying the game with.
        /// </summary>
        public Glass_Form overlayWinForm;

        /// <summary>
        /// A handle to the game window which is found via searching for a window with a title defined in gameWindowName.
        /// </summary>
        public IntPtr gameWindowHandle;

        /// <summary>
        /// Defines the name of the game window, this name is used and matched against existing windows to detect window over which to overlay.
        /// </summary>
        public string gameWindowName;

        /// <summary>
        /// A thread which hosts the glass overlay windows form, ensuring that it keeps running.
        /// </summary>
        public Thread Windows_Form_Thread;

        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// </summary>
        public Overlay_Glass_Direct2D(string gameWindowName)
        {
            // Set Window Name
            this.gameWindowName = gameWindowName;

            // Wait for and find the game window.
            Find_Game_Window();

            // Instantiate glass form
            overlayWinForm = new Glass_Form(gameWindowHandle);

            // Initialize base (directX Drawing Stuff)
            base.ConstructorAlias(overlayWinForm.Handle);
        }

        /// <summary>
        /// Calls Application.Run to host the overlay glass window such that it may be displayed.
        /// Want multiple windows (for some reason?). Create a thread and call this method.
        /// </summary>
        public void Enable_Overlay()
        {
            // Enable the Overlay Window.
            Application.Run(overlayWinForm);
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
                gameWindowHandle = WinAPI.Windows.FindWindow(null, gameWindowName);

                // If handle successfully acquired.
                if (gameWindowHandle != null) { break; }

                // Sleep to reduce CPU load.
                Thread.Sleep(16);
            }

            // Wait for the Window to show itself to screen before configuring.
            while (WinAPI.Windows.IsWindowVisible(gameWindowHandle) == false) { Thread.Sleep(16); }
        }
    }
}
