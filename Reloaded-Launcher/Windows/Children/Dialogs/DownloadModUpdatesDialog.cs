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
using System.IO;
using System.Windows.Forms;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes.ApplyTheme;
using Reloaded_GUI.Utilities.Windows;
using System.Net;
using ReloadedLauncher.Misc;
using Reloaded_Plugin_System.Interfaces.Updaters;
using Reloaded_Plugin_System.Utilities.Downloader;
using SevenZipExtractor;
// ReSharper disable All

namespace ReloadedLauncher.Windows.Children.Dialogs
{
    /// <summary>
    /// Contains the dialog invoked when the user launches the Reloaded Launcher with the 
    /// --download parameter and a valid URL.
    /// </summary>
    public partial class DownloadModUpdatesDialog : Form
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
        /// Contains a list of updates.
        /// </summary>
        private List<IUpdate> _updates;
        private bool _isUpdating;
        
        /// <summary>
        /// The size of the file is added here every time a file download completes.
        /// This is used to keep track of total file download bar.
        /// </summary>
        private float _currentDownloadedCumulativeFileSize = 0;
        private float _totalDownloadSize         = 0;
        private bool  _downloadComplete          = false;

        private Stopwatch updateTimer = new Stopwatch();
        private Stopwatch totalTimer  = new Stopwatch();

        /// <summary>
        /// Creates a new instance of the mod update dialog.
        /// </summary>
        public DownloadModUpdatesDialog(List<IUpdate> modUpdates)
        {
            InitializeComponent();
            _updates = modUpdates;
        }

        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme if not loaded.
            if (String.IsNullOrEmpty(Global.Theme.ThemeDirectory))
                Global.Theme.ThemeDirectory = Global.LoaderConfiguration.CurrentTheme;
            MakeRoundedWindow.RoundWindow(this, 30, 30);
            ApplyTheme.ThemeWindowsForm(this);
            PopulateList();
        }

        /*
            --------------
            Business Logic
            --------------
        */

        /// <summary>
        /// Update the application when the user presses the update button.
        /// </summary>
        private async void item_Update_Click(object sender, EventArgs e)
        {
            if (_isUpdating)
                return;

            if (_downloadComplete)
                this.Close();

            var senderControl = (Control)sender;
            senderControl.Text = "Downloading";

            // First obtain our mod list.
            _isUpdating = true;
            var enabledMods = GetEnabledMods();

            // Now we tally up the cumulative download size that is about to happen.
            enabledMods.ForEach(x => _totalDownloadSize += x.FileSize);

            // Update bottom left label.
            int filesComplete = 0;
            int totalFiles    = enabledMods.Count;
            borderless_FilesCompleteNumber.Text = $"{filesComplete:00}/{totalFiles:00}";
            foreach (var mod in enabledMods)
            {
                // Reset progress bar, start downloading with synced progress bar.
                borderless_FileUpdateProgressBar.Value = 0;
                byte[] file = await FileDownloader.DownloadFile(mod.DownloadLink, DownloadCompleted, UpdateFileProgress);

                // Extract the current file.
                using (Stream memoryStream = new MemoryStream(file))
                using (ArchiveFile archiveFile = new ArchiveFile(memoryStream))
                {
                    archiveFile.Extract(mod.GameModFolder, true);
                }

                filesComplete += 1;
                _currentDownloadedCumulativeFileSize += mod.FileSize;
                borderless_FilesCompleteNumber.Text = $"{filesComplete:00}/{totalFiles:00}";
            }

            AllDownloadsCompleted();
            _isUpdating       = false;
            _downloadComplete = true;
            senderControl.Text = "Close";
        }

        /// <summary>
        /// Toggles mod downloads on/off.
        /// </summary>
        private void UpdateList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Our sender is a datagridview, always.
            var senderGrid = (DataGridView)sender;

            // Check if the column index is the first column (disable/enable checkbox).
            try
            {
                if (e.ColumnIndex == 0)
                {
                    if ((string)senderGrid.Rows[e.RowIndex].Cells[0].Value == TextButtons.ButtonDisabled)
                    {
                        senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonEnabled;
                    }
                    else
                    {
                        senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonDisabled;
                    }
                }
            }
            catch { /* Ignored */ }
        }

        /// <summary>
        /// Fills the DataGridView with the current list of mods to be updated.
        /// </summary>
        private void PopulateList()
        {
            foreach (var update in _updates)
            {
                box_UpdateList.Rows.Add(TextButtons.ButtonEnabled, update.ModName, update.ModVersion, $"{update.FileSizeMB:000.0}MB");
            }
        }

        private List<IUpdate> GetEnabledMods()
        {
            // The DataGridView cannot be rearranged, thus the order of _updates matches our tick/cross order.
            List<IUpdate> enabledUpdates = new List<IUpdate>();

            for (int x = 0; x < box_UpdateList.Rows.Count; x++)
            {
                DataGridViewRow row = box_UpdateList.Rows[x];
                bool modEnabled = (string)row.Cells[(int)ModListRows.Enabled].Value == TextButtons.ButtonEnabled;

                // Add to the enabled mods queue.
                if (modEnabled)
                    enabledUpdates.Add(_updates[x]);
            }

            return enabledUpdates;
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void item_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
            -----------------
            Progressbar Logic
            -----------------
        */

        /// <summary>
        /// Updates the current GUI's total progress bar with a specific downloaded
        /// amount and maximum amount.
        /// </summary>
        private void SetTotalFileProgress(float currentSizeBytes, float maxSizeBytes)
        {
            float currentSizeMB = ToMegaBytes(currentSizeBytes);
            float maxSizeMB     = ToMegaBytes(maxSizeBytes);
            borderless_TotalDownloadProgress.Text = $"{currentSizeMB:000.0}/{maxSizeMB:000.0}";

            // Update total file horizontal progress bar.
            var bar = borderless_TotalUpdateProgressBar;
            bar.Value = (int)((currentSizeBytes / maxSizeBytes) * bar.MAX_VALUE);
        }

        private void SetCurrentFileProgress(float currentSizeBytes, float maxSizeBytes)
        {
            float currentSizeMB = ToMegaBytes(currentSizeBytes);
            float maxSizeMB     = ToMegaBytes(maxSizeBytes);
            borderless_FileDownloadProgress.Text = $"{currentSizeMB:000.0}/{maxSizeMB:000.0}";

            // Update current file horizontal progress bar.
            var bar   = borderless_FileUpdateProgressBar;
            bar.Value = (int)((currentSizeBytes / maxSizeBytes) * bar.MAX_VALUE);
        }

        private float ToKiloBytes(float bytes) => bytes / 1000F;
        private float ToMegaBytes(float bytes) => bytes / 1000F / 1000F;

        /*
           -------------------
           Download Operations
           -------------------
        */
        void DownloadCompleted(object _x, DownloadDataCompletedEventArgs dataCompleted)
        {
            borderless_FileUpdateProgressBar.Value = borderless_FileUpdateProgressBar.MAX_VALUE;
        }

        void UpdateFileProgress(object __x, DownloadProgressChangedEventArgs progressChanged)
        {
            if (CanUpdateGUI())
            {
                SetCurrentFileProgress(progressChanged.BytesReceived, progressChanged.TotalBytesToReceive);
                SetTotalFileProgress(progressChanged.BytesReceived + _currentDownloadedCumulativeFileSize, _totalDownloadSize);
                SetCurrentTime();
            }
        }

        void AllDownloadsCompleted()
        {
            borderless_FileUpdateProgressBar.Value = borderless_FileUpdateProgressBar.MAX_VALUE;
            borderless_TotalUpdateProgressBar.Value = borderless_TotalUpdateProgressBar.MAX_VALUE;
            borderless_FileDownloadProgress.Text = "Complete";
            borderless_TotalDownloadProgress.Text = "Complete";
        }

        /*
            ------------
            Miscellanous
            ------------
        */

        /// <summary>
        /// Super simple inaccurate pseudo framerate limiter used to simply limit the amount of
        /// redraws on the front end of the screen in order to prevent flicker.
        /// </summary>
        /// <returns></returns>
        bool CanUpdateGUI()
        {
            if (! updateTimer.IsRunning)
                updateTimer.Start();

            if (updateTimer.ElapsedMilliseconds > 16)
            {
                updateTimer.Restart();
                return true;
            }

            return false;
        }

        void SetCurrentTime()
        {
            if (! totalTimer.IsRunning)
                totalTimer.Start();

            borderless_DownloadTimer.Text = $"Time: {totalTimer.Elapsed.Minutes:00}:{totalTimer.Elapsed.Seconds:00}:{totalTimer.Elapsed.Milliseconds:00}";
        }

        // Misc
        enum ModListRows
        {
            Enabled,
            ModId,
            ModName,
            ModVersion,
            FileSizeMB
        }
    }
}
