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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Reloaded.Native.WinAPI;

namespace Reloaded.Overlay.External
{
    public partial class GlassForm : Form
    {
        /// <summary>
        /// Constructor for the overlay form, is executed upon creation of the overlay.
        /// </summary>
        public GlassForm(IntPtr gameWindowHandle)
        {
            // Initialize Windows Form
            InitializeComponent();

            // Set handle to game window.
            GameWindowHandle = gameWindowHandle;

            // Set up the glass window to overlay over target window.
            SetupWindow();
        }

        /// <summary>
        /// Sets/Gets a handle for the game window whose client area the window will expand and move to match.
        /// </summary>
        public IntPtr GameWindowHandle { get; set; }

        /// <summary>
        /// Delegate which will be triggered upon the movement of the game window or shape/size changes.
        /// </summary>
        public WindowEvents.WinEventDelegate GameWindowMoveDelegate { get; set; }

        /// <summary>
        /// Sets up the overlay window properties such as window style, topmost status, etc.
        /// </summary>
        private void SetupWindow()
        {
            // Set the appropriate window styles.
            SetWindowStyles();

            // Adjust window size and properties to overlap the game window.
            AdjustOverlayToGameWindow(); 

            // Setup hook for when the game window is moved, resized, changes shape...
            GameWindowMoveDelegate = WinEventProc;
            WindowEvents.SetWinEventHook
            (
                WindowEvents.EVENT_OBJECT_LOCATIONCHANGE, // Minimum event code to capture
                WindowEvents.EVENT_OBJECT_LOCATIONCHANGE, // Maximum event code to capture
                IntPtr.Zero,                                      // DLL Handle (none required) 
                GameWindowMoveDelegate,                           // Pointer to the hook function. (Delegate in our case)
                0,                                                // Process ID (0 = all)
                0,                                                // Thread ID (0 = all)
                WindowEvents.WINEVENT_OUTOFCONTEXT        // Flags: Allow cross-process event hooking
            );

            // Expand Aero Glass onto Client Area
            ExtendFrameToClientArea();
        }

        /// <summary>
        /// Sets the appropriate extended and non-extended window styles and attributes.
        /// The window is made topmost, transparent and layered and the layered window alpha
        /// is set to the maximum posible value.
        /// </summary>
        private void SetWindowStyles()
        {
            // Retrieve the original window style.
            long initialStyle = (long)WindowStyles.GetWindowLongPtr(Handle, WindowStyles.Constants.GWL_EXSTYLE);

            // Set window as visible
            Visible = true;

            // Set the new window style
            WindowStyles.SetWindowLongPtr
            (
                new HandleRef(this, Handle),    // Handle reference for the window.  
                WindowStyles.Constants.GWL_EXSTYLE, // nIndex which writes to the currently set window style.

                // Set window as layered window, transparent (removes hit testing when layered) and keep it topmost.
                (IntPtr)(initialStyle | WindowStyles.Constants.WS_EX_LAYERED | WindowStyles.Constants.WS_EX_TRANSPARENT)
            ); 

            // Set the Alpha on the Layered Window to 255 (solid)
            WindowStyles.SetLayeredWindowAttributes(Handle, 0, 255, WindowStyles.Constants.LWA_ALPHA);

            // Set window as topmost.
            TopMost = true;
        }

        /// <summary>
        /// Expands the Frame Border Effect to the whole form.
        /// Normally, without background rendering and a border, a window should technically
        /// be invisible. However, this unfortunately is not the case as compositing does not default
        /// apply to the client area of the window. Well, let's just make it apply and see the windows
        /// underneath, sounds good?
        /// </summary>
        private void ExtendFrameToClientArea()
        {
            // Instantiate a new instance of the margins class.
            WinAPIRectangle formMargins = new WinAPIRectangle();

            // Set the new form margins to conver whole window.
            formMargins.leftBorder = 0;
            formMargins.topBorder= 0;
            formMargins.rightBorder = Width;
            formMargins.bottomBorder = Height;

            // Extend the frame into client area.
            Windows.DwmExtendFrameIntoClientArea(Handle, ref formMargins);
        }

        /// <summary>
        /// Sets the overlay window location to overlap the window of the game instance.
        /// Both moves the window to the game location and sets appropriate height and width for the window.
        /// </summary>
        public void AdjustOverlayToGameWindow()
        {
            // Get game client area.
            WinAPIRectangle gameClientRectangle = Native.Windows.GetClientAreaRectangle(GameWindowHandle);

            // Set overlay edges to the edges of the client area.
            Left = gameClientRectangle.leftBorder;
            Top = gameClientRectangle.topBorder;

            // Set width and height.
            Width = gameClientRectangle.rightBorder - gameClientRectangle.leftBorder;
            Height = gameClientRectangle.bottomBorder - gameClientRectangle.topBorder;
        }

        /// <summary>
        /// Defines the delegate method which is fired when the game window is moved or resized within this class.
        /// Simply resizes the overlay window back to match Sonic Heroes' window.
        /// </summary>
        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Filter out non-HWND changes, e.g. items within a listbox.
            // Technically speaking shouldn't be necessary, though just in case.
            if (idObject != 0 || idChild != 0) return;

            // Set the size and location of the external overlay to match the 
            AdjustOverlayToGameWindow();
        }

        /// <summary>
        /// Do not paint the background, override the background painting method with an empty method.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e) { }
    }
}