using HeroesModLoaderConfig.Styles.Misc;
using HeroesModLoaderConfig.Styles.Themes;
using HeroesModLoaderConfig.Utilities.Controls;
using HeroesModLoaderConfig.Utilities.Windows;
using SonicHeroes.Misc;
using SonicHeroes.Misc.Config;
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
        /// Triggers when the user leaves for another menu.
        /// Saves all of the mods' states.
        /// </summary>
        private void Mods_Screen_Leave(object sender, EventArgs e)
        {
            SaveMods();
        }

        /// <summary>
        /// Loads the relevant menu contents when the visibility changes.
        /// </summary>
        private void Mods_Screen_VisibleChanged(object sender, EventArgs e)
        {
            // If set to visible
            if (this.Visible)
            {
                // Set the titlebar. 
                Global.CurrentMenuName = "Mods Menu";
                Global.BaseForm.UpdateTitle(Global.CurrentGameConfig.GameName);

                // Load the mod list for the game.
                LoadMods();
            }
        }

        /// <summary>
        /// Retrieves the list of mods from their configurations.
        /// </summary>
        private void LoadMods()
        {
            // Clear the current listview.
            box_ModList.Rows.Clear();

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

                // Set the description.
                item_ModDescription.Text = modConfiguration.ModDescription;

                /*
                // Retrieve the current game details.
                GameConfigParser.GameConfig gameConfig = Global.GameConfigurations[box_ModList.SelectedCells[0].RowIndex];

                // Update note box.
                item_NoteBoxEXEPath.Text = gameConfig.ExecutableDirectory.Substring(gameConfig.ExecutableDirectory.IndexOf("/") + 1);
                item_NoteBoxVerPath.Text = gameConfig.GameVersion;
                item_NoteBoxGameName.Text = gameConfig.GameName;

                // Update location box.
                item_LocationBoxDirectoryPath.Text = gameConfig.GameDirectory;
                item_LocationBoxEXEPath.Text = "$DIRECTORY + " + gameConfig.ExecutableDirectory;

                // Update injection details.
                item_InjectionBoxInjection.Text = "INJECTION: " + gameConfig.HookMethod.ToString();

                // Load the game image.
                try { item_GameBanner.BackgroundImage = Image.FromFile(gameConfig.ConfigDirectory + "\\Banner.png"); }
                catch { item_GameBanner.BackgroundImage = null; }
                */
            }
            catch { }
        }
    }
}
