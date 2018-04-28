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

using System;
using System.Windows.Forms;
using Reloaded.Input.Common;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;

namespace ReloadedLauncher.Windows.Children.Dialogs.Input_Screen
{
    /// <summary>
    /// This class provides a base form which should be copied for the generation of
    /// dialogs for the [Reloaded] Mod Loader UI.
    /// </summary>
    public partial class GetDestinationAxisDialog : Form, IGenericDialog
    {
        /// <summary>
        /// Initializes the form.
        /// </summary>
        public GetDestinationAxisDialog()
        {
            // Standard WinForms Init
            InitializeComponent();

            // Select textbox
            borderless_ValueBox.Select();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);
        }

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="initialValue">The initial value for the user to see.</param>
        public GetDestinationAxisDialog(string initialValue)
        {
            // Standard WinForms Init
            InitializeComponent();

            // Populate items in box with available axes from code.
            foreach (ControllerCommon.ControllerAxis controllerAxes in Enum.GetValues(typeof(ControllerCommon.ControllerAxis)))
            {
                borderless_ValueBox.Items.Add(controllerAxes.ToString());
            }

            // Find initial value by matching strings.
            for (int x = 0; x < borderless_ValueBox.Items.Count; x++)
            {
                // Grab ComboBox value at index x
                string value = borderless_ValueBox.GetItemText(borderless_ValueBox.Items[x]);

                // If matches, select and exit.
                if (value == initialValue) { borderless_ValueBox.SelectedIndex = x; break; }
            }

            // Select ComboBox
            borderless_ValueBox.Select();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);
        }

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
                cp.ExStyle = cp.ExStyle | (int)Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }

        #endregion

        /// <summary>
        /// Spawns the dialog window prompts the user to enter the specified
        /// floating value to enter. Returns the results of the dialog back.
        /// </summary>
        /// <returns>Requested floating point number.</returns>
        public object GetValue()
        {
            // Show this dialog.
            ShowDialog();

            // Check if an object is selected, else retry recursively.
            if (borderless_ValueBox.SelectedIndex != -1) return borderless_ValueBox.Text;

            MessageBox.Show("You should select something, dummy.");
            return GetValue();
        }

        /// <summary>
        /// Defines the title to be shown on the top of the dialog window.
        /// </summary>
        /// <param name="title">The title to be shown.</param>
        public void SetTitle(string title)
        {
            titleBar_Title.Text = title;
        }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme.
            ApplyTheme.ThemeWindowsForm(this);
        }

        /// <summary>
        /// Close this window if the user hits ok.
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Close this window if the user presses the "Enter" key.
        /// </summary>
        private void ValueBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter) Close();
        }
    }
}
