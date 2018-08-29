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
using System.Diagnostics;
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
using Reloaded.Paths;
using Reloaded.Utilities;
using ReloadedUpdateChecker;
using ReloadedUpdateChecker.Updaters;
using ReloadedUpdateChecker.Utilities.Downloader;
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
        private List<GameConfig> _gameConfigs;

        /// <summary>
        /// Set to true if the download button has been hit and the mod is being downloaded.
        /// </summary>
        private bool _isCurrentlyDownloading;

        /// <summary>
        /// Stores the HTTP URL to the modification, allowing downloads 
        /// </summary>
        private string _modificationUrl;

        /// <summary>
        /// The update sources whose events will be fired during download.
        /// </summary>
        private List<IUpdateSource> _updateSources;

        /// <summary>
        /// Initializes the form.
        /// </summary>
        /// <param name="modDownloadUrl">Specifies the URL of the mod to be downloaded.</param>
        public DownloadModDialog(string modDownloadUrl)
        {
            // Standard WinForms Init
            InitializeComponent();
            _updateSources = UpdateChecker.UpdateSources;
            MakeRoundedWindow.RoundWindow(this, 30, 30);
            _modificationUrl = modDownloadUrl;

            // Strip the Reloaded Link Specifier (if necessary) | reloaded:
            if (_modificationUrl.StartsWith(Strings.Launcher.ReloadedProtocolName, true, CultureInfo.InvariantCulture))
            {
                _modificationUrl = Regex.Replace(_modificationUrl, "reloaded:", "", RegexOptions.IgnoreCase);
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
            _gameConfigs = ConfigManager.GetAllGameConfigs();

            // Clear available items.
            borderless_SelectGame.Items.Clear();

            // Populate the dropdown listbox.
            // For each config, append the name of the game.
            foreach (GameConfig gameConfig in _gameConfigs)
            {
                borderless_SelectGame.Items.Add
                (
                    gameConfig.GameName + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.ExecutableLocation + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.GameVersion
                );
            }

            borderless_SelectGame.SelectedIndex = 0;
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
                Debugger.Launch();
                // Cast the button & ignore if currently downloading.
                Button testButton = (Button)sender;
                if (_isCurrentlyDownloading)
                    return;

                // If the progressbar is max, we are done here.
                if (borderless_UpdateProgressBar.Value == borderless_UpdateProgressBar.MAX_VALUE)
                    this.Close();

                // Set flags & GUI
                testButton.Text             = "Downloading";
                _isCurrentlyDownloading     = true;

                // Get the current game configuration & file paths.
                GameConfig gameConfig       = _gameConfigs[borderless_SelectGame.SelectedIndex];
                string gameModDirectory     = $"{LoaderPaths.GetModLoaderModDirectory()}\\{gameConfig.ModDirectory}";
                CallUpdateSourcesOnDownloadLink(gameModDirectory);                                   // May change modification URL download link.
                string fileName             = "Temp.tmp";
                string downloadLocation     = $"{gameModDirectory}\\{fileName}";

                // Start the modification download.
                byte[] remoteFile = await FileDownloader.DownloadFile
                (
                    new Uri(_modificationUrl), 
                    downloadProgressChanged: ClientOnDownloadProgressChanged
                );

                // Start unpacking
                testButton.Text = "Unpacking";
                testButton.Refresh();
                using (Stream stream = new MemoryStream(remoteFile))
                using (ArchiveFile archiveFile = new ArchiveFile(stream))
                {
                    archiveFile.Extract($"{gameModDirectory}\\");
                    CallUpdateSourcesOnExtractLink(archiveFile);
                }    

                // Cleanup
                File.Delete(downloadLocation);
                _isCurrentlyDownloading = false;
                borderless_UpdateProgressBar.Value = borderless_UpdateProgressBar.MAX_VALUE;
                testButton.Text = "Close";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                _isCurrentlyDownloading = false;
            }

        }

        /// <summary>
        /// Updates the current progress of downloading the Mod Loader
        /// update in questioon.
        /// </summary>
        private void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            borderless_UpdateProgressBar.Value = (int)((float)downloadProgressChangedEventArgs.ProgressPercentage / 100 * borderless_UpdateProgressBar.MAX_VALUE);

            // Get the current downloaded progress as megabytes and set the control text.
            string downloadedMegabytes       = ((float)downloadProgressChangedEventArgs.BytesReceived / 1000.0F / 1000.0F).ToString("000.0");
            string remainingMegabytes        = ((float)downloadProgressChangedEventArgs.TotalBytesToReceive / 1000.0F / 1000.0F).ToString("000.0");
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

        /*
            -----------------
            Interface Callers
            -----------------
        */
        private void CallUpdateSourcesOnDownloadLink(string gameModDirectory)
        {
            foreach (var updateSource in _updateSources)
                _modificationUrl = updateSource.OnLinkDownload(_modificationUrl, gameModDirectory);
        }

        private void CallUpdateSourcesOnExtractLink(ArchiveFile archive)
        {
            // Finds every single top level folder of the archive and passes it to our update sources.
            List<string> folderNames = new List<string>();

            foreach (var entry in archive.Entries)
            {
                // Second condition tests for subfolders.
                if (entry.IsFolder && Path.GetDirectoryName(entry.FileName) == "")
                {
                    folderNames.Add(entry.FileName);
                }
            }

            foreach (var updateSource in _updateSources)
               updateSource.OnModExtract(folderNames.ToArray());
        }
    }
}
