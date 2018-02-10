using ReloadedLauncher.Utilities.Controls;
using Reloaded.Misc;
using Reloaded.Misc.Config;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ReloadedLauncher.Windows.Children
{
    public partial class Main_Screen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public Main_Screen(Form MDIParent)
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
        /// Changes the titlebar & other properties when the form visibility of the form changes.
        /// </summary>
        private void Main_Screen_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Update title
                Global.CurrentMenuName = "Main Menu";
                Global.BaseForm.UpdateTitle("");

                // Set version
                this.item_VersionBoxVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

                // Load games.
                LoadGames();
            }
        }

        /// <summary>
        /// Retrieves the list of games from their configurations.
        /// </summary>
        private void LoadGames()
        {
            // Clear the current listview.
            box_GameList.Rows.Clear();

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
                box_GameList.Rows.Add(gameConfig.GameName, relativeModPath);
            }
        }

        /// <summary>
        /// Load the relevant game details when the selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the current game details.
                Global.CurrentGameConfig = Global.GameConfigurations[box_GameList.SelectedCells[0].RowIndex];

                // Update note box.
                item_NoteBoxEXEPath.Text = Global.CurrentGameConfig.ExecutableLocation.Substring(Global.CurrentGameConfig.ExecutableLocation.IndexOf("/") + 1);
                item_NoteBoxVerPath.Text = Global.CurrentGameConfig.GameVersion;
                item_NoteBoxGameName.Text = Global.CurrentGameConfig.GameName;

                // Update location box.
                item_LocationBoxDirectoryPath.Text = Global.CurrentGameConfig.GameDirectory;
                item_LocationBoxEXEPath.Text = "$DIRECTORY + " + Global.CurrentGameConfig.ExecutableLocation;

                // Update injection details.
                item_InjectionBoxInjection.Text = "INJECTION: " + Global.CurrentGameConfig.HookMethod.ToString();

                // Load the game image.
                try { item_GameBanner.BackgroundImage = Image.FromFile(Global.CurrentGameConfig.ConfigDirectory + "\\Banner.png"); }
                catch { item_GameBanner.BackgroundImage = null; }
            }
            catch { }
        }

        /// <summary>
        /// Quits the mod loader.
        /// </summary>
        private void QuitBox_Click(object sender, EventArgs e)
        {
            // Shutdown program.
            Global.BaseForm.Shutdown();
        }
    }
}
