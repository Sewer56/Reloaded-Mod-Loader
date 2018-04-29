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
using System.Diagnostics;
using System.Windows.Forms;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;
using Squirrel;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// This class provides a base form which should be copied for the generation of
    /// dialogs for the [Reloaded] Mod Loader UI.
    /// </summary>
    public partial class DownloadUpdatesDialog : Form
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
                cp.ExStyle = cp.ExStyle | (int)Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }

        #endregion

        /// <summary>
        /// This is set to true when the application is to be restarted after closing, else
        /// false.
        /// </summary>
        public bool RestartApplication { get; set; }

        /// <summary>
        /// True if the application is currently updating, else false.
        /// </summary>
        private bool IsCurrentlyUpdating { get; set; }

        /// <summary>
        /// Contains an instance of pre-initialized and awaited GithubUpdateManager from
        /// Squirrel.Windows.
        /// </summary>
        private UpdateManager _githubUpdateManager;

        /// <summary>
        /// Contains an instance of pre-acquired Github update information.
        /// </summary>
        private UpdateInfo _githubUpdateInfo;

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="githubUpdateManager">An instance of already initialized and awaited GithubUpdateManager from Squirrel.Windows</param>
        /// <param name="githubUpdateInfo">Contains an instance of pre-acquired Github update information.</param>
        public DownloadUpdatesDialog(UpdateManager githubUpdateManager, ref UpdateInfo githubUpdateInfo)
        {
            // Standard WinForms Init
            InitializeComponent();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);

            // Set the GithubManager
            _githubUpdateManager = githubUpdateManager;
            _githubUpdateInfo = githubUpdateInfo;
        }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme if not loaded.
            if (String.IsNullOrEmpty(Global.Theme.ThemeDirectory))
                Global.Theme.ThemeDirectory = Global.LoaderConfiguration.CurrentTheme;
            
            // Theme!
            ApplyTheme.ThemeWindowsForm(this);

            // Autostart update if enabled.
            if (Global.LoaderConfiguration.EnableAutomaticUpdates)
                item_Update_Click(item_Update, null);
        }

        /// <summary>
        /// Set the update details appropriately when the form is displayed/shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadUpdatesDialog_Shown(object sender, EventArgs e)
        {
            // Set new and old version in GUI.
            borderless_OldVersionNumber.Text = _githubUpdateInfo.CurrentlyInstalledVersion.Version.ToString();
            borderless_NewVersionNumber.Text = _githubUpdateInfo.FutureReleaseEntry.Version.ToString();
        }

        /// <summary>
        /// Present the changelog for the future update entry.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Changelog_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sewer56lol/Reloaded-Mod-Loader/releases");
        }

        /// <summary>
        /// Update the application when the user presses the update button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void item_Update_Click(object sender, EventArgs e)
        {
            // Do nothing if we are currently updating.
            if (IsCurrentlyUpdating)
                return;

            // Check if update complete by chacking the value of the progressbar.
            if (borderless_UpdateProgressBar.Value == borderless_UpdateProgressBar.MAX_VALUE)
            {
                RestartApplication = true;
                this.Close();
            }

            // Change the text.
            if (sender is Button testButton)
                testButton.Text = "Updating";

            // We're currently updating.
            IsCurrentlyUpdating = true;

            // Update the application.
            await _githubUpdateManager.UpdateApp(UpdateProgress);

            // Finished updating.
            IsCurrentlyUpdating = false;

            // Set update progress to max.
            borderless_UpdateProgressBar.Value = borderless_UpdateProgressBar.MAX_VALUE;

            // Change the text.
            if (sender is Button testButton2)
                testButton2.Text = "Restart";
        }

        /// <summary>
        /// Updates the current progress of downloading the Mod Loader
        /// update in questioon.
        /// </summary>
        /// <param name="progress"></param>
        private void UpdateProgress(int progress)
        {
            // 100 = Squirrel's maximum.
            borderless_UpdateProgressBar.Value = (int)((float)progress / 100 * borderless_UpdateProgressBar.MAX_VALUE);
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
