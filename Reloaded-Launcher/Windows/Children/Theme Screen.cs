using ReloadedLauncher.Styles.Misc;
using ReloadedLauncher.Styles.Themes;
using ReloadedLauncher.Utilities.Controls;
using Reloaded.Misc.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReloadedLauncher.Windows.Children
{
    public partial class Theme_Screen : Form
    {
        /// <summary>
        /// Constructor for this class.
        /// Requires the specification of the MDI Parent
        /// form that hosts this window in question.
        /// </summary>
        /// <param name="MDIParent">The MDI Parent form, an instance of Base.cs</param>
        public Theme_Screen(Form MDIParent)
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
        /// Loads the relevant menu contents when the visibility changes (user enters menu). 
        /// Saves and backs up when the user leaves for another menu (selects another tab). 
        /// </summary> 
        private void MenuVisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Set the titlebar.  
                Global.CurrentMenuName = "Theme Menu";
                Global.BaseForm.UpdateTitle("");

                // Set the button text for mod buttons to N/A.
                InitializeButtons();

                // Load the mod list for the game. 
                LoadThemes();
            }
            else
            {
                // Save theme when user exits screen.
                SaveCurrentTheme();
            }
        }

        /// <summary>
        /// Retrieves the list of mods from their configurations.
        /// </summary>
        private void LoadThemes()
        {
            // Unhook the theme change event.
            box_ThemeList.SelectionChanged -= LoadCurrentTheme;

            // Clear the current listview.
            box_ThemeList.Rows.Clear();

            try
            {
                // Retrieve current theme list into Global.
                Global.ThemeConfigurations = Global.ConfigurationManager.GetAllThemeConfigs();

                // Sort themes by name.
                Global.ThemeConfigurations.Sort((x, y) => x.ThemeName.CompareTo(y.ThemeName));

                // Iterate over each theme, populate the list.
                foreach (ThemeConfigParser.ThemeConfig themeConfig in Global.ThemeConfigurations)
                {
                    // Cells[0] = Theme Name
                    // Cells[1] = Theme Description
                    // Cells[2] = Author
                    // Cells[3] = Separator
                    // Cells[4] = Version

                    box_ThemeList.Rows.Add
                    (
                        themeConfig.ThemeName,
                        themeConfig.ThemeDescription,
                        themeConfig.ThemeAuthor,
                        Theme.ThemeProperties.TitleProperties.LoaderTitleDelimiter,
                        themeConfig.ThemeVersion
                    );
                }

                // Select the currently enabled theme.
                string currentThemeFolder = Global.LoaderConfiguration.CurrentTheme;

                // Select currently loaded theme.
                SelectLoadedTheme();
            }
            catch { }

            // Hook the theme change event.
            box_ThemeList.SelectionChanged += LoadCurrentTheme;
        }

        /// <summary>
        /// Finds the row which is equivalent to the current loaded
        /// theme out of the theme selector loads and selects the row
        /// upon the user's entry to the menu.
        /// </summary>
        private void SelectLoadedTheme()
        {
            // Find the row with current theme and select it.
            foreach (DataGridViewRow row in box_ThemeList.Rows)
            {
                // Get the theme title and version.
                string themeName = (string)row.Cells[0].Value;
                string themeVersion = (string)row.Cells[4].Value;

                // Get the theme configuration.
                ThemeConfigParser.ThemeConfig themeConfiguration = FindThemeConfiguration(themeName, themeVersion);

                // Obtain theme directory.
                string themeDirectory = Path.GetFileName(Path.GetDirectoryName(themeConfiguration.ThemeLocation));

                // Check if the theme configuration folder matches current theme folder.
                if (themeDirectory == Global.LoaderConfiguration.CurrentTheme)
                {
                    // Set theme config, select row and exit loop.
                    row.Selected = true;
                    Global.CurrentThemeConfig = themeConfiguration;
                    Global.LoaderConfiguration.CurrentTheme = themeDirectory;
                    break;
                }
            }
        }

        /// <summary>
        /// Saves the mod loader configuration which stores the currently used theme.
        /// </summary>
        private void SaveCurrentTheme()
        {
            // Save the mod loader configuration.
            Global.ConfigurationManager.LoaderConfigParser.WriteConfig(Global.LoaderConfiguration);
        }

        /// <summary>
        /// Load the relevant game details when the selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadCurrentTheme(object sender, EventArgs e)
        {
            try
            {
                // Cast the sender to the datagridview
                var senderGrid = (DataGridView)sender;

                // Obtain current row index. (Note: CurrentRow is invalid)
                int rowIndex = senderGrid.SelectedCells[0].RowIndex;

                // Get the theme title and version.
                string themeName = (string)senderGrid.Rows[rowIndex].Cells[0].Value;
                string themeVersion = (string)senderGrid.Rows[rowIndex].Cells[4].Value;

                // Cells[0] = Theme Name
                // Cells[1] = Theme Description
                // Cells[2] = Author
                // Cells[3] = Separator
                // Cells[4] = Version

                // Get the theme configuration.
                ThemeConfigParser.ThemeConfig themeConfiguration = FindThemeConfiguration(themeName, themeVersion);
                Global.CurrentThemeConfig = themeConfiguration;

                // Set the button text for website, source.
                if (themeConfiguration.ThemeSite.Length == 0) { borderless_WebBox.Text = "N/A"; } else { borderless_WebBox.Text = "Webpage"; }
                if (themeConfiguration.ThemeGithub.Length == 0) { borderless_SourceBox.Text = "N/A"; } else { borderless_SourceBox.Text = "Github"; }

                // Obtain theme directory.
                string themeDirectory = Path.GetFileName(Path.GetDirectoryName(themeConfiguration.ThemeLocation));

                // Load theme.
                Global.Theme.ThemeDirectory = themeDirectory;
                Global.LoaderConfiguration.CurrentTheme = themeDirectory;
            }
            catch { }
        }

        /// <summary>
        /// Retrieves a theme configuration from a supplied version string and theme name.
        /// </summary>
        /// <param name="themeName">Name of the mod as shown in the launcher.</param>
        /// <param name="themeVersion">Version of the mod as shown in the launcher.</param>
        private ThemeConfigParser.ThemeConfig FindThemeConfiguration(string themeName, string themeVersion)
        {
            // Search for first mod with title equivalent to mod title and version.
            return Global.ThemeConfigurations.Where
            (
                x =>
                {
                    return (x.ThemeName == themeName && x.ThemeVersion == (string)themeVersion);
                }
            ).First();
        }

        /// <summary>
        /// Opens the link to the source code website.
        /// </summary>
        private void SourceBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
            {
                OpenFile(Global.CurrentThemeConfig.ThemeGithub);
            }
        }

        /// <summary>
        /// Opens the link to the code website.
        /// </summary>
        private void WebBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
            {
                OpenFile(Global.CurrentThemeConfig.ThemeSite);
            }
        }

        /// <summary>
        /// Opens the configuration file or program.
        /// </summary>
        private void ConfigBox_Click(object sender, EventArgs e)
        {
            if (CheckIfEnabled((Control)sender))
            {
                OpenFile(Theme.ThemeProperties.ThemePropertyLocation);
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
            borderless_SourceBox.Text = "N/A";
            borderless_WebBox.Text = "N/A";
        }
    }
}
