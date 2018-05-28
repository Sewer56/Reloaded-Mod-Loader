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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded_GUI.Utilities.Controls;
using Bindings = Reloaded_GUI.Styles.Themes.Bindings;

namespace ReloadedLauncher.Windows.Children
{
    public partial class AboutScreen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public AboutScreen(Form mdiParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = mdiParent;

            // Add to the window list.
            Bindings.WindowsForms.Add(this);

            // Add Box Controls
            SetupDecorationBoxes.FindDecorationControls(this);

            // Set version
            borderless_Author.Text = $"Written by Sewer56 ~ (C) 2018 | Compiled on {File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location)}";

            // Automatically toggle checkboxes when form shown.
            // This only needs to be done once and doing it in VisibleChanged can induce location change "flicker".
            Shown += (sender, args) =>
            {
                
                borderless_AutoUpdatesBox.ButtonEnabled = Global.LoaderConfiguration.EnableAutomaticUpdates;
                borderless_SilentUpdatesBox.ButtonEnabled = Global.LoaderConfiguration.SilentUpdates;
                borderless_AllowPreReleasesBox.ButtonEnabled = Global.LoaderConfiguration.AllowBetaBuilds;
                borderless_CloseOnLaunchBox.ButtonEnabled = Global.LoaderConfiguration.ExitAfterLaunch;
            };
        }

        /// <summary> 
        /// Loads the relevant menu contents when the visibility changes (user enters menu). 
        /// Saves and backs up when the user leaves for another menu (selects another tab). 
        /// </summary> 
        private void MenuVisibleChanged(object sender, EventArgs e)
        {
            // If set to visible 
            if (Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = Strings.Launcher.Menus.AboutMenuName;
                Global.BaseForm.UpdateTitle("");
            }
            else
            {
                // Update config.
                Global.LoaderConfiguration.EnableAutomaticUpdates = borderless_AutoUpdatesBox.ButtonEnabled;
                Global.LoaderConfiguration.SilentUpdates = borderless_SilentUpdatesBox.ButtonEnabled;
                Global.LoaderConfiguration.AllowBetaBuilds = borderless_AllowPreReleasesBox.ButtonEnabled;
                Global.LoaderConfiguration.ExitAfterLaunch = borderless_CloseOnLaunchBox.ButtonEnabled;
                LoaderConfig.WriteConfig(Global.LoaderConfiguration);
            }
        }
    }
}
