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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Reloaded.IO.Config;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// This class provides a base form which should be copied for the generation of
    /// dialogs for the [Reloaded] Mod Loader UI.
    /// </summary>
    public partial class DisabledDependencyDialog : Form, IGenericDialog
    {
        /// <summary>
        /// Set to true if this mod's dependencies are to be enabled.
        /// </summary>
        public bool EnableDependencies { get; private set; } = false;

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="disabledConfigs">The list of individual disabled mod configurations.</param>
        public DisabledDependencyDialog(List<ModConfig> disabledConfigs)
        {
            // Standard WinForms Init
            InitializeComponent();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);

            // Set the disabled dependency text.
            this.borderless_DisabledDependencies.Text = string.Join("\n", disabledConfigs.Select(x => x.ModName + " | " + x.ModId));
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
            // Load the global theme.
            ApplyTheme.ThemeWindowsForm(this);
        }

        private void item_yes_Click(object sender, EventArgs e)
        {
            EnableDependencies = true;
            this.Close();
        }

        private void item_no_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
