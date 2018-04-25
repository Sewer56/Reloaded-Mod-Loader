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
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Reloaded;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;
using Bindings = Reloaded_GUI.Styles.Themes.Bindings;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// This class provides the dialog containing extended game launch options such as logging support.
    /// </summary>
    public partial class LaunchGameDialog : Form, IDialog
    {
        /// <summary>
        /// Initializes the form.
        /// </summary>
        public LaunchGameDialog()
        {
            // Standard WinForms Init
            InitializeComponent();

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

            // Set default logging location.
            borderless_LogLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Strings.Launcher.DefaultLogFileName}";
            borderless_AttachExecutableName.Text = Path.GetFileNameWithoutExtension(Global.CurrentGameConfig.ExecutableLocation);

            // Set images.
            box_LogLocationSelect.BackgroundImage = Bindings.Images.TweaksImage;
            box_AttachExecutableNameSelect.BackgroundImage = Bindings.Images.TweaksImage;
        }

        /// <summary>
        /// Presents a dialog for the user to select the file to which Reloaded Mod Loader logs will be saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void box_LogLocationSelect_Click(object sender, EventArgs e)
        {
            // Dialog for launching executables.
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.Title = "Specify the file to use for logging. Create a new text file if necessary.";
            folderDialog.Multiselect = false;
            folderDialog.Filters.Add(new CommonFileDialogFilter("Text File", "*.txt"));
            folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Open dialog.
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                borderless_LogLocation.Text = folderDialog.FileName;
            }

            // Dispose dialog.
            folderDialog.Dispose();
        }

        /// <summary>
        /// Presents a dialog for the user to select the executable name for Reloaded Mod Loader to use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void box_AttachExecutableNameSelect_Click(object sender, EventArgs e)
        {
            // Dialog for launching executables.
            CommonOpenFileDialog exectuableDialog = new CommonOpenFileDialog();
            exectuableDialog.Title = "Select the already running executable you want to attach to.";
            exectuableDialog.Multiselect = false;
            exectuableDialog.IsFolderPicker = false;
            exectuableDialog.Filters.Add(new CommonFileDialogFilter("Executable", "*.exe"));
            exectuableDialog.InitialDirectory = Global.CurrentGameConfig.GameDirectory;
            exectuableDialog.DefaultFileName = Global.CurrentGameConfig.ExecutableLocation;

            // Open dialog.
            if (exectuableDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                borderless_AttachExecutableName.Text = Path.GetFileNameWithoutExtension(exectuableDialog.FileName);
            }

            // Dispose dialog.
            exectuableDialog.Dispose();
        }

        /// <summary>
        /// Closes the current form when the player hits "Close".
        /// </summary>
        private void item_CloseBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Launches the Reloaded Loader when the launch button is hit from the advanced menu.
        /// Allows for the game to be launched with a log file explicitly specified by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_LaunchBox_Click(object sender, EventArgs e)
        {
            // The addtional arguments to be passed onto Reloaded-Loader.
            List<string> localArguments = new List<string>();

            // Append the log file option as necessary.
            if (borderless_EnableLogs.ButtonEnabled)
            {
                localArguments.Add($"\"{Strings.Common.LoaderSettingLog}\"");
                localArguments.Add($"\"{borderless_LogLocation.Text}\"");
            }

            // Start process
            Functions.LaunchLoader(localArguments.ToArray());
        }

        /// <summary>
        /// Launches the Reloaded Loader when the attach button is hit from the advanced menu.
        /// Allows for Reloaded to attach to an already running game instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_AttachBox_Click(object sender, EventArgs e)
        {
            // The addtional arguments to be passed onto Reloaded-Loader.
            List<string> localArguments = new List<string>();

            // Provide the attach executable name.
            localArguments.Add($"\"{Strings.Common.LoaderSettingAttach}\"");
            localArguments.Add($"\"{borderless_AttachExecutableName.Text}\"");

            // Append the log file option as necessary.
            if (borderless_EnableLogs.ButtonEnabled)
            {
                localArguments.Add($"\"{Strings.Common.LoaderSettingLog}\"");
                localArguments.Add($"\"{borderless_LogLocation.Text}\"");
            }

            // Start process
            Functions.LaunchLoader(localArguments.ToArray());
        }
    }
}
