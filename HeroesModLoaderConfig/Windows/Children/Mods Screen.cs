using HeroesModLoaderConfig.Styles.Themes;
using HeroesModLoaderConfig.Utilities.Controls;
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
        /// Retrieves the list of games from their configurations.
        /// </summary>
        private void LoadMods()
        {
            // Clear the current listview.
            box_ModList.Rows.Clear();

            // Retrieve current game list the into Global.
            Global.GameConfigurations = Global.ConfigurationManager.GetAllGameConfigs();

            // For each config, append it.
            foreach (GameConfigParser.GameConfig gameConfig in Global.GameConfigurations)
            {
                // Stores the path of the mod for display.
                string modPath = LoaderPaths.GetModLoaderModDirectory() + "\\" + gameConfig.ModDirectory;

                // Retrieves the relative path for presentation.
                string relativeModPath = LoaderPaths.GetModLoaderRelativePath(modPath);

                // Add the relative path.
                box_ModList.Rows.Add(gameConfig.GameName, relativeModPath);
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
