/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
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
using System.Threading;
using System.Windows.Forms;
using Reloaded.Native;

namespace Reloaded.Overlay.External
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of the game, if the game is in windowed mode.
    /// </summary>
    public class OverlayGlassDirect2D : Direct2DWindowRenderTargetBase
    {
        /// <summary>
        /// Class constructor. Instantiates both the overlay and DirectX Stuff.
        /// </summary>
        public OverlayGlassDirect2D(string gameWindowName)
        {
            // Set Window Name
            GameWindowName = gameWindowName;

            // Wait for and find the game window.
            Find_Game_Window();

            // Instantiate glass form
            OverlayForm = new GlassForm(GameWindowHandle);

            // Initialize base (directX Drawing Stuff)
            ConstructorAlias(OverlayForm.Handle);
        }

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
                if (GameWindowHandle != null) break;

                // Sleep to reduce CPU load.
                Thread.Sleep(16);
            }

            // Wait for the Window to show itself to screen before configuring.
            while (WinAPI.Windows.IsWindowVisible(GameWindowHandle) == false) Thread.Sleep(16);
        }
    }
}
