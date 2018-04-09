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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Reloaded.IO.Config.Games;
using Reloaded.Process;
using Reloaded.Utilities;
using Reloaded_GUI.Styles.Themes;
using Reloaded_GUI.Utilities.Controls;

namespace ReloadedLauncher.Windows.Children
{
    public partial class MainScreen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public MainScreen(Form mdiParent)
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
        /// Changes the titlebar & other properties when the form visibility of the form changes.
        /// </summary>
        private void Main_Screen_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                // Update title
                Global.CurrentMenuName = "Main Menu";
                Global.BaseForm.UpdateTitle("");

                // Set version
                item_VersionBoxVersion.Text = Application.ProductVersion;
                item_InjectionBoxInjection.Text = Convert.ToString(File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location), CultureInfo.InvariantCulture);

                // Load games.
                LoadGames();
            }
        }

        /// <summary>
        /// Retrieves the list of games from their configurations.
        /// </summary>
        private void LoadGames()
        {
            // Save current game config
            GameConfigParser.GameConfig currentConfig = Global.CurrentGameConfig;

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

            // Re-select the currently selected game.
            ReselectCurrentGame(currentConfig);
        }

        /// <summary>
        /// Re-selects the currently selected game upon entering the Main Menu.
        /// </summary>
        /// <param name="gameConfiguration">The game configuration to try and re-select.</param>
        private void ReselectCurrentGame(GameConfigParser.GameConfig gameConfiguration)
        {
            // Find and select the last highlighted game.
            string currentGameName = gameConfiguration?.GameName;
            string currentRelativeModDirectory = LoaderPaths.GetModLoaderRelativePath(LoaderPaths.GetModLoaderModDirectory() + "\\" + gameConfiguration?.ModDirectory);

            foreach (DataGridViewRow row in box_GameList.Rows)
            {
                // Cells[0] = Game Name
                // Cells[1] = Mod Location
                if ((string)row.Cells[0].Value == currentGameName &&
                    (string)row.Cells[1].Value == currentRelativeModDirectory)
                {
                    row.Selected = true;
                }
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
                item_NoteBoxEXEPath.Text = Global.CurrentGameConfig.ExecutableLocation.Substring(Global.CurrentGameConfig.ExecutableLocation.IndexOf("/", StringComparison.Ordinal) + 1);
                item_NoteBoxVerPath.Text = Global.CurrentGameConfig.GameVersion;
                item_NoteBoxGameName.Text = Global.CurrentGameConfig.GameName;

                // Update location box.
                item_LocationBoxDirectoryPath.Text = Global.CurrentGameConfig.GameDirectory;
                item_LocationBoxEXEPath.Text = "$DIRECTORY + " + Global.CurrentGameConfig.ExecutableLocation;

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

        /// <summary>
        /// Launches Reloaded-Loader.exe in the same directory as the launcher.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_LaunchBox_Click(object sender, EventArgs e)
        {
            // Build filepath and arguments
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Reloaded-Loader.exe";
            string arguments = $"--config {Global.CurrentGameConfig.ConfigDirectory}";

            // Start process
            ReloadedProcess process = new ReloadedProcess(filePath, arguments);
            process.ResumeAllThreads();
            
            // Self-shutdown.
            QuitBox_Click(null, null);
        }
    }
}
