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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Reloaded;
using Reloaded.IO.Config.Games;
using Reloaded.IO.Config.Mods;
using ReloadedLauncher.Misc;
using Reloaded_GUI.Styles.Themes;
using Reloaded_GUI.Utilities.Controls;
using Bindings = Reloaded_GUI.Styles.Themes.Bindings;

namespace ReloadedLauncher.Windows.Children
{
    public partial class ModsScreen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public ModsScreen(Form mdiParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = mdiParent;

            // Add to the window list.
            Bindings.WindowsForms.Add(this);

            // Add Box Controls
            SetupDecorationBoxes.FindDecorationControls(this);
        }

        /// <summary> 
        /// Loads the relevant menu contents when the visibility changes (user enters menu). 
        /// Saves and backs up when the user leaves for another menu (selects another tab). 
        /// </summary> 
        private void MenuVisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = Strings.Launcher.Menus.ModsMenuName;
                Global.BaseForm.UpdateTitle(Global.CurrentGameConfig?.GameName);

                // Set the button text for mod buttons to N/A.
                InitializeButtons();

                // Load the mod list for the game. 
                LoadMods();
            }
            else
            {
                // Save mod list when user exits screen.
                try { SaveMods(); }
                catch { }
            }
        }

        /// <summary>
        /// Retrieves the list of mods from their configurations.
        /// </summary>
        private void LoadMods()
        {
            // Clear the current listview.
            box_ModList.Rows.Clear();

            try
            {
                // Retrieve current mod list the into Global.
                Global.ModConfigurations = Global.ConfigurationManager.GetAllMods(Global.CurrentGameConfig);

                // Hold enabled rows which we will later reverse order of and append to disabled rows.
                // Also store disabled rows.
                List<DataGridViewRow> enabledRows = new List<DataGridViewRow>();
                List<DataGridViewRow> disabledRows = new List<DataGridViewRow>();

                // Retrieve enabled mods.
                GetEnabledMods(enabledRows);

                // Retrieve disabled mods.
                GetDisabledMods(disabledRows);

                // Reverse the enabled rows (present highest priority as topmost)
                enabledRows.Reverse();

                // And merge with disabled rows, append to DataGridView rows.
                foreach (DataGridViewRow enabledRow in enabledRows) box_ModList.Rows.Add(enabledRow);
                foreach (DataGridViewRow disabledRow in disabledRows) box_ModList.Rows.Add(disabledRow);
            }
            catch { }
        }

        /// <summary>
        /// Finds the currently enabled mods for this game configuration and
        /// appends them onto the list of enabled mods.
        /// </summary>
        /// <param name="enabledRows">List of enabled mods for this game.</param>
        private void GetEnabledMods(List<DataGridViewRow> enabledRows)
        {
            // Appends all enabled mods to enabled mod list.
            // Iterate over each "enabled" mod folder list.
            foreach (string modFolder in Global.CurrentGameConfig.EnabledMods)
            // Iterate over mod configurations and find relevant mod config.
            foreach (ModConfigParser.ModConfig modConfig in Global.ModConfigurations)
            {
                // Reloaded-Mods/SA2/Testmod => Testmod
                string modConfigFolderName = Path.GetFileName(Path.GetDirectoryName(modConfig.ModLocation));

                // Check if the mod folder and configuration match.
                // If there is no match in all loop iterations for a folder, the mod does not exist
                if (modConfigFolderName == modFolder)
                {
                    // Clone row style.
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)box_ModList.RowTemplate.Clone();
                    dataGridViewRow.CreateCells(box_ModList);

                    // Assign row
                    dataGridViewRow.Cells[0].Value = TextButtons.ButtonEnabled;    // Enabled Mod
                    dataGridViewRow.Cells[1].Value = modConfig.ModName;             // The name of the mod
                    dataGridViewRow.Cells[2].Value = modConfig.ModAuthor;           // Author of the mod
                    dataGridViewRow.Cells[3].Value = Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter; // Separator character as set by theme
                    dataGridViewRow.Cells[4].Value = modConfig.ModVersion;          // The version of the mod

                    // Append the row.
                    enabledRows.Add(dataGridViewRow);
                }
            }
        }

        /// <summary>
        /// Finds the currently disabled mods for this game configuration and
        /// appends them onto the list of disabled mods.
        /// </summary>
        /// <param name="disabledRows">List of disabled mods for this game.</param>
        private void GetDisabledMods(List<DataGridViewRow> disabledRows)
        {
            // Append all disabled mods to disabled mod list.
            foreach (ModConfigParser.ModConfig modConfig in Global.ModConfigurations)
            {
                // Get the folder name of the durrent mod.
                string directoryName = Path.GetFileName(Path.GetDirectoryName(modConfig.ModLocation));

                // Store the datagridviewrow
                DataGridViewRow dataGridViewRow = (DataGridViewRow)box_ModList.RowTemplate.Clone();
                dataGridViewRow.CreateCells(box_ModList);

                // Cells[0] = Enabled/Disabled Tickbox
                // Cells[1] = Mod Title
                // Cells[2] = Author
                // Cells[3] = Separator
                // Cells[4] = Version

                // Check if the mod is disabled.
                if (!Global.CurrentGameConfig.EnabledMods.Contains(directoryName))
                {
                    // Disabled
                    dataGridViewRow.Cells[0].Value = TextButtons.ButtonDisabled;
                    dataGridViewRow.Cells[1].Value = modConfig.ModName;     // The name of the mod
                    dataGridViewRow.Cells[2].Value = modConfig.ModAuthor;   // Author of the mod
                    dataGridViewRow.Cells[3].Value = Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter; // Separator character as set by theme
                    dataGridViewRow.Cells[4].Value = modConfig.ModVersion;  // The version of the mod

                    // Append the row.
                    disabledRows.Add(dataGridViewRow);
                }
            }
        }

        /// <summary>
        /// Saves the mods from their listview onto the mod configuration for the game.
        /// </summary>
        private void SaveMods()
        {
            // Stores the currently enabled mods.
            List<string> enabledMods = new List<string>();

            // Cycle each row of the DataGridView
            for (int x = 0; x < box_ModList.Rows.Count; x++)
            {
                // Assign DataGridView Row
                DataGridViewRow row = box_ModList.Rows[x];

                // Check if the mod in the row is enabled.
                bool modEnabled = (string)row.Cells[0].Value == TextButtons.ButtonEnabled;

                // If the mod is enabled.
                if (modEnabled)
                {
                    // Find the mod configuration from the set row and column.
                    // Match by mod title and version.
                    ModConfigParser.ModConfig modConfiguration = FindModConfiguration((string)row.Cells[1].Value, (string)row.Cells[4].Value);

                    // Append the folder name only to the list of mods.
                    // Reloaded-Mods/SA2/Testmod => Testmod
                    enabledMods.Add(Path.GetFileName(Path.GetDirectoryName(modConfiguration.ModLocation)));
                }
            }

            // Reverse the mod order such that mods on top take priority.
            enabledMods.Reverse();

            // Assign the currently enabled mods for the game.
            Global.CurrentGameConfig.EnabledMods = enabledMods;

            // Save the game configuration.
            GameConfigParser.WriteConfig(Global.CurrentGameConfig);
        }

        /// <summary>
        /// Retrieves a mod configuration from a supplied version string and mod name.
        /// </summary>
        /// <param name="modName">Name of the mod as shown in the launcher.</param>
        /// <param name="modVersion">Version of the mod as shown in the launcher.</param>
        private ModConfigParser.ModConfig FindModConfiguration(string modName, string modVersion)
        {
            // Search for first mod with title equivalent to mod title and version.
            return Global.ModConfigurations.First(x => x.ModName == modName && x.ModVersion == modVersion);
        }

        /// <summary>
        /// Handle clicking of the enable and disable buttons for themes.
        /// </summary>
        private void Box_ModList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cast the sender to the datagridview
            var senderGrid = (DataGridView)sender;

            // Check if the column index is the first column
            if (e.ColumnIndex == 0)
            {
                if ((string)senderGrid.Rows[e.RowIndex].Cells[0].Value == TextButtons.ButtonDisabled)
                    senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonEnabled;
                else
                    senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonDisabled;
            }
        }

        /// <summary>
        /// Load the relevant game details when the selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Cast the sender to the datagridview
                var senderGrid = (DataGridView)sender;

                // Obtain current row index. (Note: CurrentRow is invalid)
                int rowIndex = senderGrid.SelectedCells[0].RowIndex;

                // Get the mod title and version.
                string modTitle = (string)senderGrid.Rows[rowIndex].Cells[1].Value;
                string modVersion = (string)senderGrid.Rows[rowIndex].Cells[4].Value;

                // Cells[0] = Enabled/Disabled Tickbox
                // Cells[1] = Mod Title
                // Cells[2] = Author
                // Cells[3] = Separator
                // Cells[4] = Version

                // Get the mod configuration.
                ModConfigParser.ModConfig modConfiguration = FindModConfiguration(modTitle, modVersion);
                Global.CurrentModConfig = modConfiguration;

                // Set the description.
                item_ModDescription.Text = modConfiguration.ModDescription;

                // Set the button text for website, config, source.
                borderless_ConfigBox.Text   = modConfiguration.ModConfigExe.Length == 0     ? "N/A" : "Configuration";
                borderless_WebBox.Text      = modConfiguration.ThemeSite.Length == 0        ? "N/A" : "Webpage";
                borderless_SourceBox.Text   = modConfiguration.ThemeGithub.Length == 0      ? "N/A" : "Source Code";

                // Obtain mod directory.
                string modDirectory = Path.GetDirectoryName(modConfiguration.ModLocation);

                // Attempt to load image.
                try { box_ModPreview.BackgroundImage = Image.FromFile(modDirectory + $"\\{Strings.Launcher.BannerName}"); }
                catch { box_ModPreview.BackgroundImage = null; }
            }
            catch { }
        }

        /// <summary>
        /// Opens the link to the source code website.
        /// </summary>
        private void SourceBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentModConfig.ThemeGithub);
        }

        /// <summary>
        /// Opens the link to the code website.
        /// </summary>
        private void WebBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentModConfig.ThemeSite);
        }

        /// <summary>
        /// Opens the configuration file or program.
        /// </summary>
        private void ConfigBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentModConfig.ModConfigExe);
        }

        /// <summary>
        /// Opens a file with a specified path.
        /// </summary>
        /// <param name="filePath">The path to the file to be opened.</param>
        private void OpenFile(string filePath)
        {
            try { Process.Start(filePath); }
            catch { }
        }

        /// <summary>
        /// String checks control text to check if the button is enabled.
        /// </summary>
        /// <param name="control">The control (typically button) to check.</param>
        private bool CheckIfEnabled(Control control)
        {
            if (control.Text != "N/A")
                return true;
            return false;
        }

        /// <summary>
        /// Sets the button text for mod buttons to N/A.
        /// </summary>
        private void InitializeButtons()
        {
            borderless_ConfigBox.Text = "N/A";
            borderless_SourceBox.Text = "N/A";
            borderless_WebBox.Text = "N/A";
            item_ModDescription.Text = "You have no mods, heheheheh.";
        }
    }
}
