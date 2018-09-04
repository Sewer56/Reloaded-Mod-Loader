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
using System.Linq;
using System.Windows.Forms;
using Reloaded.Paths;
using ReloadedLauncher.Misc;
using Reloaded_GUI.Utilities.Controls;
using Reloaded_Plugin_System.Config;
using Bindings = Reloaded_GUI.Bindings;

namespace ReloadedLauncher.Windows.Children
{
    public partial class PluginScreen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public PluginScreen(Form mdiParent)
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
                Global.CurrentMenuName = Strings.Launcher.Menus.PluginMenuName;
                Global.BaseForm.UpdateTitle("");

                // Set the button text for mod buttons to N/A.
                InitializeButtons();

                // Load the mod list for the game. 
                LoadPluginList();
            }
            else
            {
                SaveAllPluginEntries();
            }

            // Plugins' enabled/disabled state automatically is set on toggle.
        }

        /*
            -------------------
            Main Business Logic
            -------------------
        */

        /// <summary>
        /// Retrieves the list of plugins from their configurations.
        /// </summary>
        private void LoadPluginList()
        {
            // Clear the current listview.
            box_PluginList.Rows.Clear();

            try
            {
                // Retrieve current theme list into Global.
                Global.PluginConfigurations = PluginConfig.GetAllConfigs();
                Global.PluginConfigurations = Global.PluginConfigurations.OrderBy(x => x.Enabled ? 0 : 1).ToList();

                // Add all of the plugins.
                foreach (var pluginConfig in Global.PluginConfigurations)
                {
                    box_PluginList.Rows.Add
                    (
                        pluginConfig.Enabled ? TextButtons.ButtonEnabled : TextButtons.ButtonDisabled,
                        pluginConfig.Name,
                        pluginConfig.Description,
                        pluginConfig.Author,
                        pluginConfig.Version
                    );
                }
            }
            catch { }
        }

        /// <summary>
        /// Loads the currently highlighted plugin on change of highlighted item.
        /// </summary>
        private void PluginList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Cast the sender to the datagridview
                var senderGrid = (DataGridView)sender;

                // Obtain current row index. (Note: CurrentRow is invalid)
                int rowIndex = senderGrid.SelectedCells[0].RowIndex;

                // Set current plugin.
                Global.CurrentPlugin = Global.PluginConfigurations[rowIndex];

                // Conditionally Update Buttons
                borderless_ConfigBox.Text   = Global.CurrentPlugin.ConfigFile  == "" ? "N/A" : "Configuration";
                borderless_WebBox.Text      = Global.CurrentPlugin.Site        == "" ? "N/A" : "Webpage";
            }
            catch
            { }
        }

        /// <summary>
        /// Handle clicking of the enable and disable buttons for plugins.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PluginList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Our sender is a datagridview, always.
            var senderGrid = (DataGridView)sender;

            // Check if the column index is the first column (disable/enable checkbox).
            if (e.ColumnIndex == 0)
            {
                // If disabled, enable, and vice versa.
                if ((string)senderGrid.Rows[e.RowIndex].Cells[0].Value == TextButtons.ButtonDisabled)
                {
                    senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonEnabled;
                    Global.PluginConfigurations[e.RowIndex].Enabled = true;
                }
                else
                {
                    senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.ButtonDisabled;
                    Global.PluginConfigurations[e.RowIndex].Enabled = false;
                }
            }
        }

        /// <summary>
        /// Saves every single plugin entry to HDD on exit.
        /// </summary>
        private void SaveAllPluginEntries()
        {
            foreach (var pluginConfig in Global.PluginConfigurations)
                PluginConfig.WriteConfig(pluginConfig);
        }

        /*
            -------------------
            Miscellaneous Logic
            -------------------
        */

        /// <summary>
        /// Sets the button text for mod buttons to N/A.
        /// </summary>
        private void InitializeButtons()
        {
            borderless_ConfigBox.Text = "N/A";
            borderless_WebBox.Text = "N/A";
        }

        /// <summary>
        /// Opens the link to the source code website.
        /// </summary>
        private void InfoBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile("https://github.com/sewer56lol/Reloaded-Mod-Loader");
        }

        /// <summary>
        /// Opens the link to the code website.
        /// </summary>
        private void WebBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentPlugin.Site);
        }

        /// <summary>
        /// Opens the configuration file or program.
        /// </summary>
        private void ConfigBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
                OpenFile(Global.CurrentPlugin.ConfigFileLocation);
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
            return control.Text != "N/A";
        }
    }
}
