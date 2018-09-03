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
using Microsoft.WindowsAPICodePack.Dialogs;
using Reloaded;
using Reloaded.IO.Config;
using Reloaded.Paths;
using Reloaded.Utilities;
using Reloaded_GUI.Styles.Themes;
using Reloaded_GUI.Utilities.Controls;
using Bindings = Reloaded_GUI.Bindings;

namespace ReloadedLauncher.Windows.Children
{
    public partial class ManageScreen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="mdiParent">The MDI Parent form, an instance of Base.cs</param>
        public ManageScreen(Form mdiParent)
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
                Global.CurrentMenuName = Strings.Launcher.Menus.ManageMenuName;
                Global.BaseForm.UpdateTitle("");

                // Load the mod list for the game. 
                LoadGames();

                // Reselect the current game.
                ReselectCurrentGame();
            }
            else
            {
                // Save all configs.
                ConfigManager.WriteAllGameConfigs(Global.GameConfigurations);
                Global.BaseForm.ChildrenForms.MainMenu.SaveLastGame();
            }
        }

        /// <summary>
        /// Re-selects the last selected game upon entering the Main Menu.
        /// </summary>
        private void ReselectCurrentGame()
        {
            int currentRowIndex = 0;

            // Find row of last selected game.
            for (; currentRowIndex < Global.GameConfigurations.Count; currentRowIndex++)
            {
                // Select the row if there is a directory match.
                if (Global.GameConfigurations[currentRowIndex].ConfigLocation == Global.LoaderConfiguration.LastGameFolder)
                {
                    borderless_CurrentGame.SelectedIndex = currentRowIndex;
                    break;
                }
            }
        }

        /// <summary>
        /// Retrieves the list of games and their properties.
        /// </summary>
        private void LoadGames()
        {
            // Clear the current listview.
            borderless_CurrentGame.Items.Clear();

            // Retrieve current game list the into Global.
            Global.GameConfigurations = ConfigManager.GetAllGameConfigs();

            // For each config, append the name of the game.
            foreach (GameConfig gameConfig in Global.GameConfigurations)
            {
                borderless_CurrentGame.Items.Add
                (
                    gameConfig.GameName + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.ExecutableLocation + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                    gameConfig.GameVersion
                );
            }

            // Change selected index to 0.
            try { borderless_CurrentGame.SelectedIndex = 0; } catch { }
        }

        /// <summary>
        /// Saves the current game configuration.
        /// </summary>
        private void SaveCurrentGame()
        {
            // Get current selected game.
            GameComboBoxDetails comboBoxDetails = GetSelectedGame();

            // Find and remove first by details.
            foreach (GameConfig gameConfig in Global.GameConfigurations)
            {
                // Find the first match to game name, executable and version.
                if (gameConfig.GameName == comboBoxDetails.GameName &&
                    gameConfig.ExecutableLocation == comboBoxDetails.ExecutableRelativeLocation &&
                    gameConfig.GameVersion == comboBoxDetails.GameVersion)
                {
                    // Check if global config.
                    if (gameConfig.ConfigLocation == LoaderPaths.GetGlobalGameConfigDirectory())
                    {
                        MessageBox.Show($"I'm sorry {Environment.UserName}, I'm afraid I can't let you do that.");
                        return;
                    }

                    // Set the new game details.
                    gameConfig.GameName = borderless_GameName.Text;
                    gameConfig.GameVersion = borderless_GameVersion.Text;
                    gameConfig.GameDirectory = borderless_GameDirectory.Text;
                    gameConfig.ExecutableLocation = borderless_GameExecutableDirectory.Text;
                    gameConfig.ModDirectory = borderless_GameModDirectory.Text;
                    gameConfig.CommandLineArgs = borderless_CommandLineArguments.Text;

                    // Change the current item name to reflect new changes.
                    borderless_CurrentGame.Items[borderless_CurrentGame.SelectedIndex] =
                        borderless_GameName.Text + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                        borderless_GameExecutableDirectory.Text + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                        borderless_GameVersion.Text;

                    break;
                }
            }
        }

        /// <summary>
        /// Deletes a game configuration from the known configurations.
        /// </summary>
        private void DeleteGame(object sender, EventArgs e)
        {
            // Get current selected game.
            GameComboBoxDetails comboBoxDetails = GetSelectedGame();

            // If there is no game (null), return.
            if (comboBoxDetails == null) { return; }

            // Find and remove first by details.
            for (int x = 0; x < Global.GameConfigurations.Count; x++)
            {
                // Find the first match to game name, executable and version.
                if (Global.GameConfigurations[x].GameName == comboBoxDetails.GameName &&
                    Global.GameConfigurations[x].ExecutableLocation == comboBoxDetails.ExecutableRelativeLocation &&
                    Global.GameConfigurations[x].GameVersion == comboBoxDetails.GameVersion)
                {
                    // Check if global config.
                    if (Global.GameConfigurations[x].ConfigLocation == LoaderPaths.GetGlobalGameConfigDirectory())
                    {
                        MessageBox.Show($"It's no use {Environment.UserName}, give up.");
                        return;
                    }

                    // Maintain currently open banners.
                    try { Global.BaseForm.ChildrenForms.MainMenu.item_GameBanner.BackgroundImage.Dispose(); } catch { }
                    try { box_GameBanner.BackgroundImage.Dispose(); } catch { }

                    // Remove game from list & Switch game.
                    borderless_CurrentGame.Items.RemoveAt(x);
                    try { borderless_CurrentGame.SelectedIndex = borderless_CurrentGame.Items.Count - 1; } catch { }

                    // Garbage collect old possible image references.
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // Remove game config & physical location.
                    try { Directory.Delete(Path.GetDirectoryName(Global.GameConfigurations[x].ConfigLocation), true); } catch { }
                    Global.GameConfigurations.RemoveAt(x);

                    break;
                }
            }
        }

        /// <summary>
        /// Adds a new game onto the current game list.
        /// </summary>
        private void AddNewGame(object sender, EventArgs e)
        {
            // Select new location for game configuration.
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog
            {
                Title = "Select the folder for the new game configuration.",
                Multiselect = false,
                IsFolderPicker = true,
                InitialDirectory = LoaderPaths.GetModLoaderGamesDirectory()
            };

            // Open dialog.
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (folderDialog.FileName.Contains(LoaderPaths.GetModLoaderGamesDirectory()) && 
                    folderDialog.FileName.Length > LoaderPaths.GetModLoaderGamesDirectory().Length)
                    CreateNewGameConfig(folderDialog.FileName);
                else
                    MessageBox.Show($"Your chosen directory should be a subdirectory of {LoaderPaths.GetModLoaderGamesDirectory()} ");
            }

            // Dispose dialog.
            folderDialog.Dispose();
        }

        /// <summary>
        /// Creates the new game configuration for the combobox
        /// dropdown and selects it.
        /// </summary>
        /// <param name="configDirectory">The absolute directory where the game configuration will be stored. (Subdirectory of Reloaded-Config/Games</param>
        private void CreateNewGameConfig(string configDirectory)
        {
            // Get current index.
            int nextGameIndex = borderless_CurrentGame.Items.Count;

            // Add a new game onto the configurations.
            Global.GameConfigurations.Add
            (
                new GameConfig
                {
                    GameName = "New Game " + nextGameIndex,
                    GameDirectory = "",
                    GameVersion = "",
                    EnabledMods = new List<string>(),
                    ModDirectory = "",
                    ConfigLocation = configDirectory + $"\\{Strings.Parsers.ConfigFile}",
                    ExecutableLocation = ""
                }
            );

            // Get latest gameconfig
            GameConfig gameConfig = Global.GameConfigurations.Last();

            // Write latest gameconfig
            GameConfig.WriteConfig(gameConfig);

            // Add a new configuration.
            borderless_CurrentGame.Items.Add
            (
                gameConfig.GameName + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                gameConfig.ExecutableLocation + " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " " +
                gameConfig.GameVersion
            );

            // Select last item.
            borderless_CurrentGame.SelectedIndex = nextGameIndex;
        }

        /// <summary>
        /// Loads the details of the currently loaded game.
        /// </summary>
        private void SelectedGameChanged(object sender, EventArgs e)
        {
            // Get current selected game.
            GameComboBoxDetails comboBoxDetails = GetSelectedGame();

            // Find by details.
            GameConfig gameConfig = Global.GameConfigurations.First
            (
                x => x.GameName == comboBoxDetails.GameName && 
                x.ExecutableLocation == comboBoxDetails.ExecutableRelativeLocation && 
                x.GameVersion == comboBoxDetails.GameVersion
            );

            // Set global configuration.
            Global.CurrentGameConfig = gameConfig;

            // Populate fields.
            borderless_GameName.Text = gameConfig.GameName;
            borderless_GameModDirectory.Text = gameConfig.ModDirectory;
            borderless_GameVersion.Text = gameConfig.GameVersion;
            borderless_GameExecutableDirectory.Text = gameConfig.ExecutableLocation;
            borderless_GameDirectory.Text = gameConfig.GameDirectory;
            borderless_CommandLineArguments.Text = gameConfig.CommandLineArgs;

            // Load the game image.
            try
            {
                // We do not want to keep open file handles.
                Image image;
                using (var bmpTemp = new Bitmap(GameConfig.GetBannerPath(gameConfig)))
                { image = new Bitmap(bmpTemp); }

                box_GameBanner.BackgroundImage = image;
            }
            catch { box_GameBanner.BackgroundImage = null; }
        }

        /// <summary>
        /// Presents a dialog for the user to select a new location
        /// whereby the current game's executable resides.
        /// </summary>
        private void SelectExecutableLocation(object sender, EventArgs e)
        {
            // Dialog for launching executables.
            CommonOpenFileDialog executableDialog = new CommonOpenFileDialog();
            executableDialog.Title = "Select the Game Executable to run.";
            executableDialog.Multiselect = false;
            executableDialog.DefaultDirectory = borderless_GameDirectory.Text;
            executableDialog.Filters.Add(new CommonFileDialogFilter("Executable", "*.exe"));

            // Open dialog.
            if (executableDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // Executable Directory
                string executableDirectory = Path.GetDirectoryName(executableDialog.FileName);

                // If the game directory is not set, default it. 
                if (borderless_GameDirectory.Text.Length == 0) borderless_GameDirectory.Text = executableDirectory;

                // If the executable path is not a subdirectory, default it.
                if (! executableDialog.FileName.Contains(borderless_GameDirectory.Text))
                {
                    MessageBox.Show("The executable location should be a subdirectory of the game folder, dummy. For convenience, it's been reset to game executable directory.");
                    borderless_GameDirectory.Text = executableDirectory;
                }

                // Set executable location.
                borderless_GameExecutableDirectory.Text = executableDialog.FileName.Substring(borderless_GameDirectory.Text.Length + 1);
            }

            // Dispose dialog.
            executableDialog.Dispose();
        }

        /// <summary>
        /// Presents a dialog for the user to select a new location for the game directory.
        /// This should only require to be changed if the game executable 
        /// is in a subfolder such as /bin/ or /build/ and game files are in another directory.
        /// </summary>
        private void SelectGameDirectory(object sender, EventArgs e)
        {
            // Dialog for game directory.
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.Title = "Select the Game Folder";
            folderDialog.Multiselect = false;
            folderDialog.IsFolderPicker = true;

            // Open dialog.
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // Executable Path
                string gameDirectory = folderDialog.FileName;

                // Set game directory.
                borderless_GameDirectory.Text = gameDirectory;

                // Check if executable path contains game path.
                if (borderless_GameExecutableDirectory.Text.Contains(gameDirectory)) borderless_GameExecutableDirectory.Text = borderless_GameExecutableDirectory.Text.Substring(gameDirectory.Length + 1);
            }

            // Dispose dialog.
            folderDialog.Dispose();
        }

        /// <summary>
        /// Allows for the selection of a mod folder for storing mods for the current game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectModFolder(object sender, EventArgs e)
        {
            // Dialog for launching executables.
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.Title = "Select the folder for storing current game's mods.";
            folderDialog.Multiselect = false;
            folderDialog.IsFolderPicker = true;
            folderDialog.InitialDirectory = LoaderPaths.GetModLoaderModDirectory();

            // Open dialog.
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            { 
                if (folderDialog.FileName.Contains(LoaderPaths.GetModLoaderModDirectory()))
                {
                    // Check if a folder was selected.
                    if (folderDialog.FileName.Length < LoaderPaths.GetModLoaderModDirectory().Length + 1)
                    { MessageBox.Show("You should probably select a folder."); folderDialog.Dispose(); return; }

                    // Get relative path.
                    borderless_GameModDirectory.Text = folderDialog.FileName.Substring(LoaderPaths.GetModLoaderModDirectory().Length + 1);
                }
                else
                {
                    MessageBox.Show("Your chosen directory should be a subdirectory of Reloaded-Mods (" + LoaderPaths.GetModLoaderModDirectory() + ")");
                }
            }

            // Dispose dialog.
            folderDialog.Dispose();
        }

        /// <summary>
        /// Allow user to select a new banner image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectNewGameBanner(object sender, EventArgs e)
        {
            // Dialog for launching executables.
            CommonOpenFileDialog imageDialog = new CommonOpenFileDialog();
            imageDialog.Title = "Select the new banner image for the game.";
            imageDialog.Multiselect = false;

            // Open dialog.
            if (imageDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // Get current selected game.
                GameComboBoxDetails comboBoxDetails = GetSelectedGame();

                // Find current config by details.
                GameConfig gameConfig = Global.GameConfigurations.First
                (
                    x => x.GameName == comboBoxDetails.GameName &&
                    x.ExecutableLocation == comboBoxDetails.ExecutableRelativeLocation &&
                    x.GameVersion == comboBoxDetails.GameVersion
                );

                // Set new image.
                box_GameBanner.BackgroundImage?.Dispose();
                Global.BaseForm.ChildrenForms.MainMenu.item_GameBanner.BackgroundImage?.Dispose();

                // Copy the banner to new location.
                try { File.Copy(imageDialog.FileName, GameConfig.GetBannerPath(gameConfig), true); }

                // Same image or no permissions to access directory/file.
                catch { }

                // Set new image.
                box_GameBanner.BackgroundImage = Image.FromFile(GameConfig.GetBannerPath(gameConfig));
            }

            // Dispose dialog.
            imageDialog.Dispose();
        }

        /// <summary>
        /// Obtains the name details of the currently selected game in the combobox.
        /// </summary>
        private GameComboBoxDetails GetSelectedGame()
        {
            // Return empty if the index is -1
            if (borderless_CurrentGame.SelectedIndex == -1) { return null; }

            // Split name of currently selected game.
            string currentGame = (string)borderless_CurrentGame.Items[borderless_CurrentGame.SelectedIndex];

            // Splitstring
            string splitString = " " + Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter + " ";

            // Split the game details
            string[] gameDetails = currentGame.Split(new[] { splitString }, StringSplitOptions.None);

            // gameDetails[0] = Game Name
            // gameDetails[1] = Game Executable Relative Location
            // gameDetails[2] = Game Version
            GameComboBoxDetails comboBoxDetails = new GameComboBoxDetails();
            comboBoxDetails.GameName = gameDetails[0];
            comboBoxDetails.ExecutableRelativeLocation = gameDetails[1];
            comboBoxDetails.GameVersion = gameDetails[2];

            return comboBoxDetails;
        }

        /// <summary>
        /// Handles the Save Game Button, merely calls save game function.
        /// </summary>
        private void SaveGameButton(object sender, EventArgs e) { SaveCurrentGame(); }

        /// <summary>
        /// Stores the details of the current ComboBox selection.
        /// </summary>
        private class GameComboBoxDetails
        {
            public string GameName;
            public string GameVersion;
            public string ExecutableRelativeLocation;
        }

        /// <summary>
        /// Opent the current game configuration's game directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_OpenGameDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Global.CurrentGameConfig.GameDirectory))
                { Process.Start(Global.CurrentGameConfig.GameDirectory); }
                else
                { MessageBox.Show("It would appear that the given game directory does not exist, did you move it? Did you move me? I feel oddly suspicious..."); }
            }
            catch { MessageBox.Show("Failed to launch your default file manager inside the game directory. In some cases this may simply be a permissions problem" +
                                    "because you are trying to open a protected folder (e.g. in Program Files), in which case you may need to run Reloaded as an Administrator."); }
            
        }

        /// <summary>
        /// Opens the current game's modification directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_OpenModDir_Click(object sender, EventArgs e)
        {
            Process.Start(LoaderPaths.GetModLoaderModDirectory() + $"\\{Global.CurrentGameConfig.ModDirectory}");
        }

        /// <summary>
        /// Opens the directory containing the current game config.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_OpenConfigDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetDirectoryName(Global.CurrentGameConfig.ConfigLocation));
        }
    }
}
