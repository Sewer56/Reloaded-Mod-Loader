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
using Reloaded.Overlay.External.WPF;
using Reloaded.Overlay.External.WPF.Controls;
using Reloaded.Overlay.External.WPF.Structures;

namespace Reloaded.Overlay.External.D2D
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WPFOverlayWindow : Window
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
        /// Set this to true in order to ignore the window, else false.
        /// </summary>
        public bool IgnoreWindow
        {
            get => _ignoringWindow;
            set
            {
                if (value) { _overlayHelper.IgnoreWindow(); }
                else { _overlayHelper.UnIgnoreWindow(); }

                _ignoringWindow = value;
            }
        }

        /// <summary>
        /// Delegate which will be triggered upon resizing of the game window.
        /// </summary>
        public Action GameWindowResizeDelegate { get; set; }

        /// <summary>
        /// True if we are currently ignoring the window from user clicks, else false.
        /// </summary>
        private bool _ignoringWindow;

        /// <summary>
        /// The handle of the window which to follow, track and/or overlay.
        /// </summary>
        private IntPtr _targetWindowHandle;

        /// <summary>
        /// [WPF-D2D Mixed Overlay]
        /// Creates an instance of the WPF overlay used for providing a Direct2D Render Surface for the user.
        /// Protip: Copy this class to your own project if you want to mix WPF and Direct2D.
        /// 
        /// Note: This is for complete freaks who want the most choice in drawing, this class IS NOT GOOD for performance,
        /// if you want performance consider an internal DX9 overlay instead or use a pure WPF overlay (see Reloaded Samples).
        /// If you want Direct2D only at a good performance level, consider the option in the Overlay.External.Legacy namespace.
        /// </summary>
        public WPFOverlayWindow(IntPtr targetWindowHandle, D2DRenderControl.DelegateRenderDirect2D renderDelegate)
        {
            InitializeComponent();
            _targetWindowHandle = targetWindowHandle;
            _ignoringWindow = true;
            d2dRenderControl.RenderDelegate = renderDelegate;
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
            // The magic behind following game window.
            _overlayHelper = new WpfOverlayHelper(this, _targetWindowHandle);

            // Brings our window to front.
            this.Activate();

            // Ignore our window.
            _overlayHelper.IgnoreWindow();
            _overlayHelper.WindowFollowMode = WindowFollowMode.Overlay;
        }

        /// <summary>
        /// Called automatically because of WindowFollowMode.Overlay;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Call delegates for when the window size changes.
            GameWindowResizeDelegate?.Invoke();
        }
    }
}
