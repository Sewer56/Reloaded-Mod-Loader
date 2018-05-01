using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Native.Functions;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Utilities.Windows;

namespace Reloaded_Mod_Template
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        /*
            Transparency in this case is achieved by checking the AllowsTransparency
            flag available in the designer's Window Properties page for Window.
        */

        /// <summary>
        /// Contains the handle to our window, used for interacting with the window
        /// using native Windows API methods if required.
        /// </summary>
        private IntPtr _ourHandle;

        /// <summary>
        /// Contains the handle of the game's window. Used for interacting with the window
        /// using Native API methods if required.
        /// </summary>
        private IntPtr _gameWindowHandle;

        /// <summary>
        /// Contains the properties of the individual game window in question.
        /// </summary>
        private GameWindowProperties _gameWindowProperties;

        /// <summary>
        /// Stores the original window size before making use of the window overlay mode.
        /// </summary>
        private Point _originalWindowSize;

        /// <summary>
        /// Delegate which will be triggered upon the movement of the game window or shape/size changes.
        /// </summary>
        private WindowEventHooks.WinEventDelegate _windowEventDelegate;

        /// <summary>
        /// Defines the behaviour of the window with respect to the game window.
        /// </summary>
        private WindowFollowMode _windowFollowMode;

        /// <summary>
        /// True if we are currently ignoring the window, else false.
        /// </summary>
        private bool _ignoringWindow;

        /// <summary>
        /// Runs in background to obtain controller status.
        /// </summary>
        private ControllerManager _reloadedControllerManager;

        /// <summary>
        /// Stores the original style of our window, for later restoring in order to un-ingore our window.
        /// </summary>
        private IntPtr _originalWindowStyle;

        /// <summary>
        /// Defines the following modes of the current window.
        /// </summary>
        enum WindowFollowMode
        {
            /// <summary>
            /// Window will behave as normal.
            /// </summary>
            None,

            /// <summary>
            /// Window will follow movements of game window.
            /// </summary>
            FollowWindow,

            /// <summary>
            /// Window will overlay parent window's client area.
            /// </summary>
            Overlay
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public OverlayWindow()
        {
            InitializeComponent();
            _ignoringWindow = false;
            this._windowFollowMode = WindowFollowMode.None;
        }

        /// <summary>
        /// Executed upon the loading of the program,
        /// sets the current window as a child of the game window,
        /// allowing the window to be minimized along with the game window, closed
        /// along with the game window etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get handle of game window.
            _gameWindowHandle = Program.GameProcess.Process.MainWindowHandle;
            _gameWindowProperties = new GameWindowProperties();
            _reloadedControllerManager = new ControllerManager();

            // Assigns the Window owner to our game Window.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            helper.Owner = _gameWindowHandle;

            // Set the handle of our window for later easier use.
            _ourHandle = helper.Handle;

            // Brings our window to front.
            this.Activate();

            // Always on top (optional)
            this.Topmost = true;

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
            Structures.WinapiRectangle gameClientSize = WindowProperties.GetClientRectangle(_gameWindowHandle);
            _gameWindowProperties.xPosition = gameClientSize.LeftBorder;
            _gameWindowProperties.yPosition = gameClientSize.TopBorder;
            _gameWindowProperties.width = gameClientSize.RightBorder - gameClientSize.LeftBorder;
            _gameWindowProperties.height = gameClientSize.BottomBorder - gameClientSize.TopBorder;

            // Controller Thread
            Thread controllerThread = new Thread(() =>
            {
                _reloadedControllerManager.SetupControllerManager();

                while (true)
                {
                    // Get inputs.
                    ControllerCommon.ControllerInputs controllerInputs = _reloadedControllerManager.GetInput(0);

                    // Process inputs.
                    ProcessInputs(controllerInputs);

                    // Wait for button release.
                    while (_reloadedControllerManager.GetInput(0).ControllerButtons.ButtonBack)
                    { Thread.Sleep(16); }

                    // Sleep
                    Thread.Sleep(16);
                }
                
            });
            controllerThread.Start();
        }

        /// <summary>
        /// Processes the obtained controller inputs from a background thread.
        /// </summary>
        /// <param name="controllerInputs"></param>
        private void ProcessInputs(ControllerCommon.ControllerInputs controllerInputs)
        {
            if (controllerInputs.ControllerButtons.ButtonBack)
            {
                if (! _ignoringWindow) { IgnoreWindow(); }
                else { UnIgnoreWindow(); }
            }
        }

        /// <summary>
        /// Sets the current Window Styles of the current window such that the current window is
        /// ignored by all user input.
        /// </summary>
        private void IgnoreWindow()
        {
            // Retrieve the original window style.
            long initialStyle = (long)WindowStyles.GetWindowLongPtr(_ourHandle, Constants.GWL_EXSTYLE);

            // Set the new window style
            _originalWindowStyle = WindowStyles.SetWindowLongPtr
            (
                _ourHandle,            // Handle reference for the window.  
                Constants.GWL_EXSTYLE, // nIndex which writes to the currently set window style.

                // Set window as layered window, transparent (removes hit testing when layered) and keep it topmost.
                (IntPtr)(initialStyle | Constants.WS_EX_LAYERED | Constants.WS_EX_TRANSPARENT)
            );

            _ignoringWindow = !_ignoringWindow;
        }

        /// <summary>
        /// Restores the original Window Styles of the current window.
        /// </summary>
        private void UnIgnoreWindow()
        {
            WindowStyles.SetWindowLongPtr
            (
                _ourHandle,            // Handle reference for the window.  
                Constants.GWL_EXSTYLE, // nIndex which writes to the currently set window style.
                _originalWindowStyle   // Restore original style
            );

            _ignoringWindow = !_ignoringWindow;
        }

        /// <summary>
        /// Registered event for the window when the mouse is held.
        /// Allows us to move the window around by dragging the background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_windowFollowMode != WindowFollowMode.Overlay)
            {
                // Reusing Reloaded-GUI (Launcher Theming/Utility Library)
                MoveWindow.MoveTheWindow(_ourHandle);
            }
        }

        /// <summary>
        /// Changes the window follow mode to overlay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SitOnWindowClick(object sender, RoutedEventArgs e)
        {
            // Backs up the old window details and sits ontop of the game window.
            _originalWindowSize = new Point(this.Width, this.Height);

            // Change follow mode.
            this._windowFollowMode = WindowFollowMode.Overlay;
        }

        /// <summary>
        /// Changes the window follow mode to move with window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWithWindowClick(object sender, RoutedEventArgs e)
        {
            ResetGameWindow();
            this._windowFollowMode = WindowFollowMode.FollowWindow;
        }

        /// <summary>
        /// Resets the window follow property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableWindowSync(object sender, RoutedEventArgs e)
        {
            ResetGameWindow();
            this._windowFollowMode = WindowFollowMode.None;
        }

        /// <summary>
        /// Executed when the properties of the game window such as position or location change.
        /// </summary>
        private void GameWindowChanged()
        {
            // Get game client edges using Reloaded Function.
            Structures.WinapiRectangle gameClientSize = WindowProperties.GetClientRectangle(_gameWindowHandle);

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
        /// Sets our window properties to that of the game window.
        /// </summary>
        private void OverlayGameWindow(GameWindowProperties gameWindowProperties)
        {
            this.Height = gameWindowProperties.height;
            this.Width = gameWindowProperties.width;
            this.Left = gameWindowProperties.xPosition;
            this.Top = gameWindowProperties.yPosition;
        }

        /// <summary>
        /// Resets the game window if the previous layout mode has been overlay.
        /// </summary>
        private void ResetGameWindow()
        {
            if (_windowFollowMode == WindowFollowMode.Overlay)
            {
                this.Width = _originalWindowSize.X;
                this.Height = _originalWindowSize.Y;
            }
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
            this.Left += xDelta;
            this.Top += yDelta;
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

        /// <summary>
        /// Triggered when the size of the window changes.
        /// Moves the controls relative to the window size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
                This is only an example of how you may update your UI according to window size changes. 
                This example is rather "cheap" and is meant to give you a starting point as a "proof of concept".
            */
            int constantXOffset = -202;
            int firstYButtonOffsetFromBottom = -30;
            int subsequentYButtonOffsets = -25;

            // The offset of the left side of the buttons to the canvas is -202, we want to maintain that offset.
            int newXPosition = (int)window.Width + constantXOffset;

            // The first offset from the bottom of the canvas to the button.
            int firstButtonYPosition = (int)window.Height + firstYButtonOffsetFromBottom;

            // Set X Positions.
            Canvas.SetLeft(moveWithWindowButton, newXPosition);
            Canvas.SetLeft(disableBothButton, newXPosition);
            Canvas.SetLeft(overlayModeButton, newXPosition);

            // Set Y Positions in order.
            Canvas.SetTop(overlayModeButton, firstButtonYPosition);
            Canvas.SetTop(moveWithWindowButton, Canvas.GetTop(overlayModeButton) + subsequentYButtonOffsets);
            Canvas.SetTop(disableBothButton, Canvas.GetTop(moveWithWindowButton) + subsequentYButtonOffsets);
        }
    }
}
