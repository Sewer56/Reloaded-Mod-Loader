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
using Newtonsoft.Json;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using ReloadedLauncher.Misc;
using ReloadedLauncher.Windows.Children.Dialogs;
using Reloaded_GUI.Styles.Themes;
using Reloaded_GUI.Utilities.Controls;
using Reloaded_Plugin_System;
using Bindings = Reloaded_GUI.Bindings;

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
                Global.ModConfigurations = ConfigManager.GetAllModsForGame(Global.CurrentGameConfig);

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
            foreach (ModConfig modConfig in Global.ModConfigurations)
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
                    dataGridViewRow.Cells[0].Value = TextButtons.ButtonEnabled;     // Enabled Mod
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
            foreach (ModConfig modConfig in Global.ModConfigurations)
            {
                // Get the folder name of the durrent mod.
                string directoryName = Path.GetFileName(Path.GetDirectoryName(modConfig.ModLocation));

                // Store the datagridviewrow
                DataGridViewRow dataGridViewRow = (DataGridViewRow)box_ModList.RowTemplate.Clone();
                dataGridViewRow.CreateCells(box_ModList);

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
                bool modEnabled = (string)row.Cells[(int)ModListCell.Enabled].Value == TextButtons.ButtonEnabled;

                // If the mod is enabled.
                if (modEnabled)
                {
                    // Find the mod configuration from the set row and column.
                    // Match by mod title and version.
                    ModConfig modConfiguration = FindModConfiguration((string)row.Cells[(int)ModListCell.ModTitle].Value, (string)row.Cells[(int)ModListCell.Version].Value);

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
            GameConfig.WriteConfig(Global.CurrentGameConfig);
        }

        /// <summary>
        /// Retrieves a mod configuration from a supplied version string and mod name.
        /// </summary>
        /// <param name="modName">Name of the mod as shown in the launcher.</param>
        /// <param name="modVersion">Version of the mod as shown in the launcher.</param>
        private ModConfig FindModConfiguration(string modName, string modVersion)
        {
            // Search for first mod with title equivalent to mod title and version.
            return Global.ModConfigurations.First(x => x.ModName == modName && x.ModVersion == modVersion);
        }

        /// <summary>
        /// Handle clicking of the enable and disable buttons for themes.
        /// </summary>
        private void Box_ModList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Our sender is a datagridview, always.
            var senderGrid = (DataGridView)sender;

            // Check if the column index is the first column (disable/enable checkbox).
            if (e.ColumnIndex == 0)
            {
                if ((string) senderGrid.Rows[e.RowIndex].Cells[(int)ModListCell.Enabled].Value == TextButtons.ButtonDisabled)
                {
                    CheckDependenciesModEnable();
                    senderGrid.Rows[e.RowIndex].Cells[(int)ModListCell.Enabled].Value = TextButtons.ButtonEnabled;
                }
                else
                {
                    CheckDependenciesModDisable();
                    senderGrid.Rows[e.RowIndex].Cells[(int)ModListCell.Enabled].Value = TextButtons.ButtonDisabled;
                } 
            }
        }

        /// <summary>
        /// Performs a dependency check for a mod that is about to be disabled, letting the user know of
        /// any mods that depend on the current mod.
        /// </summary>
        private void CheckDependenciesModDisable()
        {
            // Populate a list of every single mod.
            List<GameConfig> allGameConfigs = ConfigManager.GetAllGameConfigs();
            List<ModConfig> allMods = new List<ModConfig>(100);

            // Populate total mod list.
            foreach (var gameConfig in allGameConfigs)
                allMods.AddRange(ConfigManager.GetAllModsForGame(gameConfig));

            // Check for any dependencies on current entry.
            ModConfig localModConfig = Global.CurrentModConfig;
            List<ModConfig> allEnabledDependencies = allMods.Where(x => x.Dependencies.Contains(localModConfig.ModId) && x.IsEnabled()).ToList();

            // Display dialog optionally.
            if (allEnabledDependencies.Count > 0)
            {
                EnabledDependencyDialog dependencyDialog = new EnabledDependencyDialog(allEnabledDependencies);
                dependencyDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Performs a dependency check for a mod that is about to be enabled to inform the user of missing
        /// dependencies and ensure that all of the mod's necessary dependencies are enabled.
        /// </summary>
        private void CheckDependenciesModEnable()
        {
            // Check for missing dependencies.
            List<string> missingDependencies = Global.CurrentModConfig.GetMissingDependencies();
            if (missingDependencies.Count > 0)
            {
                MessageBox.Show($"Seems that you are missing some dependencies required by the current modification: {string.Join(",", missingDependencies)}.\n\n" +
                                $"While you will not be stopped from enabling the mod, do note your mod may not perform as expected (and possibly crash).");
            }

            // Check for disabled dependencies.
            List<ModConfig> disabledConfigs = Global.CurrentModConfig.GetDisabledDependencies();
            if (disabledConfigs.Count > 0)
            {
                DisabledDependencyDialog disabledDependencyDialog = new DisabledDependencyDialog(disabledConfigs);
                disabledDependencyDialog.ShowDialog();

                // If uses decides to not enable dependencies, exit handler.
                if (!disabledDependencyDialog.EnableDependencies)
                    return;

                // Else enable each dependency.
                foreach (var disabledConfig in disabledConfigs)
                {
                    disabledConfig.ParentGame.EnabledMods.Add(disabledConfig.GetModDirectoryName());
                    GameConfig.WriteConfig(disabledConfig.ParentGame); // Inefficient but will do for now.
                }
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
                string modTitle = (string)senderGrid.Rows[rowIndex].Cells[(int)ModListCell.ModTitle].Value;
                string modVersion = (string)senderGrid.Rows[rowIndex].Cells[(int)ModListCell.Version].Value;

                // Get the mod configuration.
                ModConfig modConfiguration = FindModConfiguration(modTitle, modVersion);
                Global.CurrentModConfig = modConfiguration;

                // Set the description, button text for website, config, source.
                item_ModDescription.Text = modConfiguration.ModDescription;

                borderless_ConfigBox.Text   = modConfiguration.ConfigurationFile == "" ? "N/A" : "Configuration";
                borderless_WebBox.Text      = modConfiguration.ModSite           == "" ? "N/A" : "Webpage";
                borderless_SourceBox.Text   = modConfiguration.ModSource         == "" ? "N/A" : "Source Code";

                // Obtain mod directory.
                string localModDirectory = Path.GetDirectoryName(modConfiguration.ModLocation);

                // Attempt to load image.
                try { box_ModPreview.BackgroundImage = Image.FromFile(localModDirectory + $"\\{Strings.Launcher.BannerName}"); }
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
                OpenFile(Global.CurrentModConfig.ModSource);
        }

        /// <summary>
        /// Opens the link to the code website.
        /// </summary>
        private void WebBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentModConfig.ModSite);
        }

        /// <summary>
        /// Opens the configuration file or program.
        /// </summary>
        private void ConfigBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control) sender))
            {
                string filePath = Global.CurrentModConfig.GetModDirectory() + "\\" +
                                  Global.CurrentModConfig.ConfigurationFile;

                if (File.Exists(filePath))
                    OpenFile(filePath);
                else
                    MessageBox.Show($@"This mod's configuration file {filePath} does not exist." +
                                    "\n\nPlease note that some mods may require to run them at least once to autogenerate a config file.");
            }
                
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
            if (control.Text != "" && control.Text != "N/A")
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

        private enum ModListCell : int
        {
            Enabled     = 0,
            ModTitle    = 1,
            Author      = 2,
            Separator   = 3,
            Version     = 4
        }


    }
}
