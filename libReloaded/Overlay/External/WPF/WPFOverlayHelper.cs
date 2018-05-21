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
using System.Windows;
using System.Windows.Interop;
using Reloaded.Native.Functions;
using Reloaded.Native.WinAPI;
using Reloaded.Overlay.External.WPF.Structures;

namespace Reloaded.Overlay.External.WPF
{
    /// <summary>
    /// The <see cref="WpfOverlayHelper"/> provides code to help with the creation of transparent WPF
    /// overlays over the target games, by providing code which allows WPF forms to follow game window movement
    /// as well as overlay the entire game window in question.
    /// </summary>
    public class WpfOverlayHelper
    {
        /// <summary>
        /// Defines the behaviour of the window with respect to the game window.
        /// </summary>
        private WindowFollowMode _windowFollowMode;

        /// <summary>
        /// Defines the behaviour of the window with respect to the game window.
        /// </summary>
        public WindowFollowMode WindowFollowMode
        {
            get => _windowFollowMode;
            set
            {
                if (value == WindowFollowMode.Overlay)
                    _originalWindowSize = new Point(_ourWindow.Width, _ourWindow.Height);
                else if (_windowFollowMode == WindowFollowMode.Overlay)
                    ResetGameWindow();  

                _windowFollowMode = value;
            }
        }

        /// <summary>
        /// Contains the properties of the individual game window in question.
        /// </summary>
        private GameWindowProperties _gameWindowProperties;

        /// <summary>
        /// Contains the handle of the game's window. Used for interacting with the window
        /// using Native API methods if required.
        /// </summary>
        private IntPtr _gameWindowHandle;

        /// <summary>
        /// A copy of our own window made when the class was created.
        /// </summary>
        private Window _ourWindow;

        /// <summary>
        /// Delegate which will be triggered upon the movement of the game window or shape/size changes.
        /// </summary>
        private WindowEventHooks.WinEventDelegate _windowEventDelegate;

        /// <summary>
        /// Stores the original window size before making use of the window overlay mode.
        /// </summary>
        private Point _originalWindowSize;

        /// <summary>
        /// Constructor for the WPF Overlay Helper which provides code to help with the creation of transparent WPF overlays
        /// to use over target games: allowing them to for instance follow game window movement or overlay the whole window in question.
        /// </summary>
        /// <param name="wpfWindow">An instance of a WPF window to provide the move with window and overlay window services to.</param>
        /// <param name="targetWindowHandle">Specifies the window handle (of game, or other window) to which track movements of.</param>
        public WpfOverlayHelper(Window wpfWindow, IntPtr targetWindowHandle)
        {
            // Set follow mode.
            this._windowFollowMode = WindowFollowMode.None;
            _ourWindow = wpfWindow;

            // Get handle of game window.
            _gameWindowHandle = targetWindowHandle;
            _gameWindowProperties = new GameWindowProperties();

            // Set owner window.
            SetOwnerWindow(targetWindowHandle);

            // Setup game window hook, firing when the game window is moved, resized, etc.
            _windowEventDelegate += GameWindowMoveDelegate;

            // Register the hook using A Windows API Function.
            WindowEventHooks.SetWinEventHook
            (
                WindowEventHooks.EVENT_OBJECT_LOCATIONCHANGE,       // Minimum event code to capture
                WindowEventHooks.EVENT_OBJECT_LOCATIONCHANGE,       // Maximum event code to capture
                IntPtr.Zero,                                        // DLL Handle (none required) 
                _windowEventDelegate,                               // Pointer to the hook function. (Delegate in our case)
                0,                                                  // Process ID (0 = all)
                0,                                                  // Thread ID (0 = all)
                WindowEventHooks.WINEVENT_OUTOFCONTEXT              // Flags: Allow cross-process event hooking
            );

            // Initialize game window properties.
            Native.WinAPI.Structures.WinapiRectangle gameClientSize = WindowProperties.GetClientRectangle(_gameWindowHandle);
            _gameWindowProperties.xPosition = gameClientSize.LeftBorder;
            _gameWindowProperties.yPosition = gameClientSize.TopBorder;
            _gameWindowProperties.width = gameClientSize.RightBorder - gameClientSize.LeftBorder;
            _gameWindowProperties.height = gameClientSize.BottomBorder - gameClientSize.TopBorder;
        }

        /// <summary>
        /// Sets the owner of the current window by the Window Handle.
        /// </summary>
        /// <param name="targetWindowHandle">Specifies the window handle (of game, or other window).</param>
        public void SetOwnerWindow(IntPtr targetWindowHandle)
        {
            WindowInteropHelper helper = new WindowInteropHelper(_ourWindow);
            helper.Owner = _gameWindowHandle;
        }

        /// <summary>
        /// Sets the current Window Styles of the current window such that the current window is
        /// ignored by all user input.
        /// </summary>
        public void IgnoreWindow()
        {
            _ourWindow.Dispatcher.Invoke(
                () =>
                {
                    // Handle to our WPF form.
                    IntPtr ourHandle = new WindowInteropHelper(_ourWindow).Handle;

                    // Retrieve the original window style.
                    long initialStyle = (long)WindowStyles.GetWindowLongPtr(ourHandle, Constants.GWL_EXSTYLE);

                    // Set the new window style
                    WindowStyles.SetWindowLongPtr
                    (
                        ourHandle,             // Handle reference for the window.  
                        Constants.GWL_EXSTYLE, // nIndex which writes to the currently set window style.

                        // Set window as layered window, transparent (removes hit testing when layered) and keep it topmost.
                        (IntPtr)(initialStyle | Constants.WS_EX_LAYERED | Constants.WS_EX_TRANSPARENT)
                    );
                }    
            );
        }

        /// <summary>
        /// Restores the original Window Styles of the current window.
        /// </summary>
        public void UnIgnoreWindow()
        {
            _ourWindow.Dispatcher.Invoke(
                () =>
                {
                    // Handle to our WPF form.
                    IntPtr ourHandle = new WindowInteropHelper(_ourWindow).Handle;

                    // Retrieve the original window style.
                    long initialStyle = (long)WindowStyles.GetWindowLongPtr(ourHandle, Constants.GWL_EXSTYLE);
                    initialStyle &= ~Constants.WS_EX_LAYERED;
                    initialStyle &= ~Constants.WS_EX_TRANSPARENT;

                    WindowStyles.SetWindowLongPtr
                    (
                        ourHandle,             // Handle reference for the window.  
                        Constants.GWL_EXSTYLE, // nIndex which writes to the currently set window style.

                        // Restore original style
                        (IntPtr)(initialStyle)
                    );
                }
            );
        }

        /// <summary>
        /// Executed when the properties of the game window such as position or location change.
        /// </summary>
        private void GameWindowChanged()
        {
            // Get game client edges using Reloaded Function.
            Native.WinAPI.Structures.WinapiRectangle gameClientSize = WindowProperties.GetClientRectangle(_gameWindowHandle);

            // Set the game window height, width and location.
            GameWindowProperties localProperties = new GameWindowProperties()
            {
                xPosition = gameClientSize.LeftBorder,
                yPosition = gameClientSize.TopBorder,
                width = gameClientSize.RightBorder - gameClientSize.LeftBorder,
                height = gameClientSize.BottomBorder - gameClientSize.TopBorder
            };

            // Process any of the follow modes if set.
            switch (_windowFollowMode)
            {
                case WindowFollowMode.None: break;
                case WindowFollowMode.FollowWindow: FollowGameWindow(localProperties); break;
                case WindowFollowMode.Overlay: OverlayGameWindow(localProperties); break;
            }

            // Assign Window Properties
            _gameWindowProperties = localProperties;
        }

        /// <summary>
        /// Calculates the diference in location from before to now for the game window and moves our window 
        /// appropriately alongside the game window.
        /// </summary>
        private void FollowGameWindow(GameWindowProperties gameWindowProperties)
        {
            // Calculate the X & Y difference.
            int xDelta = gameWindowProperties.xPosition - _gameWindowProperties.xPosition;
            int yDelta = gameWindowProperties.yPosition - _gameWindowProperties.yPosition;

            // Move our window by the same.
            _ourWindow.Left += xDelta;
            _ourWindow.Top += yDelta;
        }

        /// <summary>
        /// Sets our window properties to that of the game window.
        /// </summary>
        private void OverlayGameWindow(GameWindowProperties gameWindowProperties)
        {
            _ourWindow.Height = gameWindowProperties.height;
            _ourWindow.Width = gameWindowProperties.width;
            _ourWindow.Left = gameWindowProperties.xPosition;
            _ourWindow.Top = gameWindowProperties.yPosition;
        }

        /// <summary>
        /// Resets the game window if the previous layout mode has been overlay.
        /// </summary>
        private void ResetGameWindow()
        {
            if (_windowFollowMode == WindowFollowMode.Overlay)
            {
                _ourWindow.Width = _originalWindowSize.X;
                _ourWindow.Height = _originalWindowSize.Y;
            }
        }

        /// <summary>
        /// Defines the delegate method which is fired when the game window is moved or resized within this class.
        /// Simply resizes the overlay window back to match the target game's window.
        /// </summary>
        private void GameWindowMoveDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Filter out non-HWND changes, e.g. items within a listbox.
            // Technically speaking shouldn't be necessary, though just in case.
            if (idObject != 0 || idChild != 0) return;

            // Set the size and location of the external overlay to match the game/target window.
            // Only if an object has changed location, shape, or size.
            if (eventType == 0x800B)
                GameWindowChanged();
        }
    }
}
