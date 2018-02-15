/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
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

using Reloaded.Native;
using ReloadedLauncher.Styles.Themes;
using ReloadedLauncher.Utilities.Windows;
using System;
using System.Windows.Forms;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// This class provides a base form which should be copied for the generation of
    /// dialogs for the [Reloaded] Mod Loader UI.
    /// </summary>
    public partial class DialogSample : Form, IDialog
    {
        #region Compositing
        /// <summary>
        /// Gets the creation parameters.
        /// The parameters are overridden to set the window as composited.
        /// Normally this would go into a child window class and other forms would
        /// derive from this, however this has shown to make the VS WinForm designer
        /// to be buggy.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | (int)WinAPI.WindowStyles.Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }
        #endregion

        /// <summary>
        /// Initializes the form.
        /// </summary>
        public DialogSample()
        {
            // Standard WinForms Init
            InitializeComponent();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);
        }

        // /////////////////////////////
        // Null Interface Implementation
        // /////////////////////////////
        public object GetValue() { return null; }
        public void SetTitle(string title) { }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Set title bar colour.
            Global.BaseForm.panel_CategoryBar.BackColor = Theme.ThemeProperties.CategoryColours.BGColour;
            Global.BaseForm.panel_TitleBar.BackColor = Theme.ThemeProperties.TitleColours.BGColour;

            // Load the global theme.
            ApplyTheme.ThemeWindowsForm(this);
        }

        /// <summary>
        /// Called when the mouse is moved within the client area of the button while the
        /// left (or right depending on user setting) mouse is down.
        /// As the title is a button, which covers the entire top panel it
        /// effectively serves as the top panel in itself in receiving mouse events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBarMouseDown(object sender, MouseEventArgs e) { MoveWindow.MoveTheWindow(this.Handle); }
    }
}
