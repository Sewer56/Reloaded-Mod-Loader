using HeroesModLoaderConfig.Styles.Misc;
using HeroesModLoaderConfig.Styles.Themes;
using HeroesModLoaderConfig.Utilities.Controls;
using HeroesModLoaderConfig.Utilities.Windows;
using SonicHeroes.Misc;
using SonicHeroes.Misc.Config;
using SonicHeroes.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Windows.Children
{
    public partial class Mods_Screen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public Mods_Screen(Form MDIParent)
        {
            // Standard Winforms Initialization
            InitializeComponent();

            // Set the MDI parent
            MdiParent = MDIParent;

            // Add to the window list.
            Global.WindowsForms.Add(this);

            // Add Box Controls
            SetupDecorationBoxes.FindDecorationControls(this);
        }

        /// <summary>
        /// Is executed once the windows form has finshed loading.
        /// </summary>
        private void Mods_Screen_Load(object sender, EventArgs e)
        {

        }

        /// <summary> 
        /// Loads the relevant menu contents when the visibility changes (user enters menu). 
        /// Saves and backs up when the user leaves for another menu (selects another tab). 
        /// </summary> 
        private void MenuVisibleChanged(object sender, EventArgs e)
        {
            // If set to visible 
            if (this.Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = "Mods Menu";
                Global.BaseForm.UpdateTitle(Global.CurrentGameConfig.GameName);

                // Set the button text for mod buttons to N/A.
                InitializeButtons();

                // Load the mod list for the game. 
                LoadMods();
            }
            else
            {
                SaveMods();
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
                // Retrieve current game list the into Global.
                Global.ModConfigurations = Global.ConfigurationManager.GetAllMods(Global.CurrentGameConfig);

                // For each config, append it.
                foreach (ModConfigParser.ModConfig modConfig in Global.ModConfigurations)
                {
                    // Get the folder name of the durrent mod.
                    string directoryName = Path.GetFileName(Path.GetDirectoryName(modConfig.ModLocation));

                    // If the current mod is enabled.
                    string enabled;

                    if (Global.CurrentGameConfig.EnabledMods.Contains(directoryName)) { enabled = TextButtons.BUTTON_ENABLED; }
                    else { enabled = TextButtons.BUTTON_DISABLED; }

                    // Add the relative path.
                    box_ModList.Rows.Add
                    (
                        enabled,                                                     // Enabled/Disabled
                        modConfig.ModName,                                           // The name of the mod in question
                        modConfig.ModAuthor,                                         // Author of the theme in question
                        Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter,  // Separator character as set by theme
                        modConfig.ModVersion                                         // Version of the mod
                   );
                }
            }
            catch { }
        }

        /// <summary>
        /// Saves the mods from their listview onto the mod configuration for the game.
        /// </summary>
        private void SaveMods()
        {
            // Stores the currently enabled mods.
            List<string> enabledMods = new List<string>();

            // Cells[0] = Enabled/Disabled Tickbox
            // Cells[1] = Mod Title
            // Cells[2] = Author
            // Cells[3] = Separator
            // Cells[4] = Version

            // Cycle each row of the DataGridView
            foreach (DataGridViewRow row in box_ModList.Rows)
            {
                // Check if the mod in the row is enabled.
                bool modEnabled = (string)row.Cells[0].Value == TextButtons.BUTTON_ENABLED ? true : false;

                // If the mod is enabled.
                if (modEnabled)
                {
                    // Find the mod configuration from the set row and column.
                    ModConfigParser.ModConfig modConfiguration = FindModConfiguration((string)row.Cells[1].Value, (string)row.Cells[4].Value);

                    // Append the folder name only to the list of mods.
                    // Mod-Loader-Mods/SA2/Testmod => Testmod
                    enabledMods.Add(Path.GetFileName(Path.GetDirectoryName(modConfiguration.ModLocation)));
                }
            }

            // Swap the currently enabled mods for the game.
            Global.CurrentGameConfig.EnabledMods = enabledMods;

            // Save the game configuration.
            Global.ConfigurationManager.GameConfigParser.WriteConfig(Global.CurrentGameConfig);
        }

        /// <summary>
        /// Retrieves a mod configuration from a supplied version string and mod name.
        /// </summary>
        /// <param name="modName">Name of the mod as shown in the launcher.</param>
        /// <param name="modVersion">Version of the mod as shown in the launcher.</param>
        private ModConfigParser.ModConfig FindModConfiguration(string modName, string modVersion)
        {
            // Search for first mod with title equivalent to mod title and version.
            return Global.ModConfigurations.Where
            (
                x =>
                {
                    return (x.ModName == modName && x.ModVersion == (string)modVersion);
                }
            ).First();
        }

        /// <summary>
        /// Handle clicking of the enable and disable buttons for themes.
        /// </summary>
        private void Box_ModList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cast the sender to the datagridview
            var senderGrid = (DataGridView)sender;

            // Check if the column index is the first column
            if (e.ColumnIndex == 0)
            {
                // Switch state from disabled to enabled and vice versa.
                if ((string)senderGrid.Rows[e.RowIndex].Cells[0].Value == TextButtons.BUTTON_DISABLED)
                { senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.BUTTON_ENABLED; }
                else { senderGrid.Rows[e.RowIndex].Cells[0].Value = TextButtons.BUTTON_DISABLED; }
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

                // Get the mod title and version.
                string modTitle = (string)senderGrid.CurrentRow.Cells[1].Value;
                string modVersion = (string)senderGrid.CurrentRow.Cells[4].Value;

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
                if (modConfiguration.ModConfigEXE.Length == 0) { borderless_ConfigBox.Text = "N/A"; } else { borderless_ConfigBox.Text = "Configuration"; }
                if (modConfiguration.ModSite.Length == 0) { borderless_WebBox.Text = "N/A"; } else { borderless_WebBox.Text = "Webpage"; }
                if (modConfiguration.ModGithub.Length == 0) { borderless_SourceBox.Text = "N/A"; } else { borderless_SourceBox.Text = "Source Code"; }

                // Obtain mod directory.
                string modDirectory = Path.GetDirectoryName(modConfiguration.ModLocation);

                // Attempt to load image.
                try { box_ModPreview.BackgroundImage = Image.FromFile(modDirectory + "\\Banner.png"); }
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
            {
                OpenFile(Global.CurrentModConfig.ModGithub);
            }
        }

        /// <summary>
        /// Opens the link to the code website.
        /// </summary>
        private void WebBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
            {
                OpenFile(Global.CurrentModConfig.ModSite);
            }
        }

        /// <summary>
        /// Opens the configuration file or program.
        /// </summary>
        private void ConfigBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
            {
                OpenFile(Global.CurrentModConfig.ModConfigEXE);
            }
        }

        /// <summary>
        /// Opens a file with a specified path.
        /// </summary>
        /// <param name="filePath">The path to the file to be opened.</param>
        private void OpenFile(string filePath)
        {
            try { System.Diagnostics.Process.Start(filePath); }
            catch { }
        }

        /// <summary>
        /// String checks control text to check if the button is enabled.
        /// </summary>
        /// <param name="control">The control (typically button) to check.</param>
        private bool CheckIfEnabled(Control control)
        {
            if (control.Text != "N/A") { return true; }
            else { return false; }
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
