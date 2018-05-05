using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Reloaded;
using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Overlay.External.WPF;
using Reloaded.Overlay.External.WPF.Structures;
using Reloaded.Overlay.External.WPF.Utilities;

namespace WPF_Demo_Overlay
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
        /// Instance of the overlay helper which grants us the ability to overlay as
        /// well as follow the target window.
        /// </summary>
        private WpfOverlayHelper _overlayHelper;

        /// <summary>
        /// True if we are currently ignoring the window, else false.
        /// </summary>
        private bool _ignoringWindow;

        /// <summary>
        /// Runs in background to obtain controller status.
        /// </summary>
        private ControllerManager _reloadedControllerManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public OverlayWindow()
        {
            InitializeComponent();
            _ignoringWindow = false;
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
            // Instance members
            _reloadedControllerManager = new ControllerManager();

            // The magic behind following game window.
            _overlayHelper = new WpfOverlayHelper(this, Bindings.TargetProcess.Process.MainWindowHandle);

            // Brings our window to front.
            this.Activate();

            // Always on top (optional)
            // this.Topmost = true;

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
                    while (_reloadedControllerManager.GetInput(0).ControllerButtons.ButtonLs)
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
            if (controllerInputs.ControllerButtons.ButtonLs)
            {
                if (!_ignoringWindow) { _overlayHelper.IgnoreWindow(); }
                else { _overlayHelper.UnIgnoreWindow(); }
                _ignoringWindow = !_ignoringWindow;
            }
        }

        /// <summary>
        /// Registered event for the window when the mouse is held.
        /// Allows us to move the window around by dragging the background.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_overlayHelper.WindowFollowMode != WindowFollowMode.Overlay)
            {
                // Reusing Reloaded-GUI's (Launcher Theming/Utility Library)
                MoveWindow.MoveTheWindow(new WindowInteropHelper(this).Handle);
            }
        }

        /// <summary>
        /// Changes the window follow mode to overlay.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SitOnWindowClick(object sender, RoutedEventArgs e)
        {
            // Change follow mode.
            _overlayHelper.WindowFollowMode = WindowFollowMode.Overlay;
        }

        /// <summary>
        /// Changes the window follow mode to move with window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWithWindowClick(object sender, RoutedEventArgs e)
        {
            _overlayHelper.WindowFollowMode = WindowFollowMode.FollowWindow;
        }

        /// <summary>
        /// Resets the window follow property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableWindowSync(object sender, RoutedEventArgs e)
        {
            _overlayHelper.WindowFollowMode = WindowFollowMode.None;
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
            int newXPosition = (int)Window.Width + constantXOffset;

            // The first offset from the bottom of the canvas to the button.
            int firstButtonYPosition = (int)Window.Height + firstYButtonOffsetFromBottom;

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
