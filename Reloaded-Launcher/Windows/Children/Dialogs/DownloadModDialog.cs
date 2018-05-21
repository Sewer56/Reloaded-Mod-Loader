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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;
using System.Net;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.IO.Config.Games;
using Reloaded.Utilities;
using Reloaded_GUI.Styles.Themes;
using SevenZipExtractor;

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// Contains the dialog invoked when the user launches the Reloaded Launcher with the 
    /// --download parameter and a valid URL.
    /// </summary>
    public partial class DownloadModDialog : Form
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
        /// Stores a list of all mod loader game configurations.
        /// </summary>
        private List<GameConfigParser.GameConfig> gameConfigs;

        /// <summary>
        /// Set to true if the download button has been hit and the mod is being downloaded.
        /// </summary>
        private bool IsCurrentlyDownloading;

        /// <summary>
        /// Stores the HTTP URL to the modification, allowing downloads 
        /// </summary>
        private string ModificationURL;

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="modDownloadURL">Specifies the URL of the mod to be downloaded.</param>
        public DownloadModDialog(string modDownloadURL)
        {
            // Standard WinForms Init
            InitializeComponent();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);

            // Set the URL to download.
            ModificationURL = modDownloadURL;

            // Strip the Reloaded Link Specifier (if necessary)
            if (ModificationURL.StartsWith(Strings.Launcher.ReloadedProtocolName, true, CultureInfo.InvariantCulture))
            {
                ModificationURL = Regex.Replace(ModificationURL, "reloaded:", "", RegexOptions.IgnoreCase);
            }
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

            // Get all available games and populate them.
            gameConfigs = ConfigManager.GetAllGameConfigs();

            // Clear available items.
            borderless_SelectGame.Items.Clear();

            // Populate the dropdown listbox.
            // For each config, append the name of the game.
            foreach (GameConfigParser.GameConfig gameConfig in gameConfigs)
            {
                borderless_SelectGame.Items.Add
                (
                    gameConfig.GameName + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.ExecutableLocation + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.GameVersion
                );
            }

            // Select first item.
            borderless_SelectGame.SelectedIndex = 0;

            // Reset value of progressbar.
            borderless_UpdateProgressBar.Value = 0;
        }

        /// <summary>
        /// Update the application when the user presses the update button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void item_Update_Click(object sender, EventArgs e)
        {
            try
            {
                // Cast the button.
                Button testButton = (Button)sender;

                // Do nothing if we are currently downloading.
                if (IsCurrentlyDownloading)
                    return;

                // Check if download complete by chacking the value of the progressbar.
                if (borderless_UpdateProgressBar.Value == borderless_UpdateProgressBar.MAX_VALUE)
                    this.Close();

                // Start download process.
                testButton.Text = "Downloading";

                // We're currently donwloading.
                IsCurrentlyDownloading = true;

                // Get the current game configuration.
                GameConfigParser.GameConfig gameConfig = gameConfigs[borderless_SelectGame.SelectedIndex];

                // Try to get the file name of the file
                string fileName = "temp.archive";
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        // Obtain the name of the file.
                        client.OpenRead(ModificationURL);
                        fileName = new ContentDisposition(client.ResponseHeaders["content-disposition"]).FileName;
                    }
                }
                catch { }

                // Set the other paths.
                string gameModDirectory = $"{LoaderPaths.GetModLoaderModDirectory()}\\{gameConfig.ModDirectory}";
                string fileLocationOutput = $"{gameModDirectory}\\{fileName}";

                // Setup the modification download.
                using (WebClient client = new WebClient())
                {
                    // Obtain the name of the file.
                    Uri fileDownloadUri = new Uri(ModificationURL);
                    client.DownloadProgressChanged += ClientOnUploadProgressChanged;

                    // Download
                    await client.DownloadFileTaskAsync(fileDownloadUri, fileLocationOutput);
                }

                // Start download process.
                testButton.Text = "Downloading";

                // Change the text.
                testButton.Text = "Unpacking";

                // Unpacking
                using (ArchiveFile archiveFile = new ArchiveFile(fileLocationOutput))
                {
                    archiveFile.Extract($"{gameModDirectory}\\");
                }        

                // Delete original.
                File.Delete(fileLocationOutput);

                // Finished downloading.
                IsCurrentlyDownloading = false;

                // Set update progress to max.
                borderless_UpdateProgressBar.Value = borderless_UpdateProgressBar.MAX_VALUE;

                // Change the text to close.
                testButton.Text = "Close";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

                // Not downloading.
                IsCurrentlyDownloading = false;
            }

        }

        /// <summary>
        /// Updates the current progress of downloading the Mod Loader
        /// update in questioon.
        /// </summary>
        private void ClientOnUploadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            // 100 = Default maximum
            borderless_UpdateProgressBar.Value = (int)((float)downloadProgressChangedEventArgs.ProgressPercentage / 100 * borderless_UpdateProgressBar.MAX_VALUE);

            // Get the current downloaded progress as megabytes and set the control text.
            string downloadedMegabytes = ((float)downloadProgressChangedEventArgs.BytesReceived / 1000.0F / 1000.0F).ToString("000.#");
            string remainingMegabytes = ((float)downloadProgressChangedEventArgs.TotalBytesToReceive / 1000.0F / 1000.0F).ToString("000.#");
            borderless_DownloadProgress.Text = $"{downloadedMegabytes}/{remainingMegabytes}MB";
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
