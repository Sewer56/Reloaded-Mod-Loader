using HeroesModLoaderConfig.Styles.Themes;
using HeroesModLoaderConfig.Utilities.Windows;
using HeroesModLoaderConfig.Windows.Children;
using SonicHeroes.Native;
using System;
using System.Windows.Forms;
using static SonicHeroes.Misc.Config.ThemePropertyParser;

namespace HeroesModLoaderConfig
{
    public partial class Base : Form
    {
        #region Compositing
        /// <summary>
        /// Gets the creation parameters.
        /// The parameters are overridden to set the window as composited.
        /// Normally this would go into a child window class and other forms would
        /// derive from this, however this has shown to make the VS WinForm designer
        /// to be buggy.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | (int)WinAPI.WindowStyles.Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }
        #endregion

        /// <summary>
        /// Stores all of the child forms to this Windows form which
        /// effectively are represented each of the tabs.
        /// </summary>
        public ChildForms MDIChildren { get; set; }

        /// <summary>
        /// A structure which defines all of the child forms
        /// that this form in question hosts.
        /// </summary>
        public class ChildForms
        {
            public Main_Screen MainMenu { get; set; }
            public Mods_Screen ModsMenu { get; set; }

            /// <summary>
            /// Stores the currently opened menu.
            /// </summary>
            public Form CurrentMenu { get; set; }
        }

        /// <summary>
        /// Initializes the form.
        /// </summary>
        public Base()
        {
            // Standard WinForms Init
            InitializeComponent();

            // Make the form rounded.
            MakeRoundedWindow.RoundWindow(this, 30, 30);

            // Add to the window list.
            Global.WindowsForms.Add(this);

            // Set this form as an MDI Container
            IsMdiContainer = true;

            // Open all child forms
            InitializeMDIChildren();
        }

        // Click Events for Category Buttons
        #region Category Buttons
        private void CategoryBar_Games_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.MainMenu); }
        private void CategoryBar_Mods_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.ModsMenu); }
        private void CategoryBar_Input_Click(object sender, EventArgs e) { }
        private void CategoryBar_Theme_Click(object sender, EventArgs e) { }
        private void CategoryBar_Manager_Click(object sender, EventArgs e) { }
        private void CategoryBar_About_Click(object sender, EventArgs e) { }
        #endregion

        /// <summary>
        /// Updates the current title for the mod loader menu, taking in mind theme configuration
        /// and the delimiter + delimiter settings.
        /// </summary>
        /// <param name="extraText">This text is appended to the end of the menu title.</param>
        public void UpdateTitle(string extraText)
        {
            // Retrieve the theme properties.
            ThemeConfig themeProperties = Theme.ThemeProperties;

            // Stores the title of the mod loader, before it is set and rendered.
            string loaderTitle = "";
            string delimiter = "";

            // Set the delimiter.
            delimiter += themeProperties.TitleProperties.DelimiterHasSpaces ? " " : "";
            delimiter += themeProperties.TitleProperties.LoaderTitleDelimiter;
            delimiter += themeProperties.TitleProperties.DelimiterHasSpaces ? " " : "";

            // Set the non-title contents.
            if (themeProperties.TitleProperties.LoaderTitlePrefix)
            {
                // Menu Name, Delimiter and Title
                loaderTitle += Global.CurrentMenuName;
                loaderTitle += delimiter;
                loaderTitle += themeProperties.TitleProperties.LoaderTitle;
            }
            else
            {
                // Title, Delimiter and Menu name
                loaderTitle += themeProperties.TitleProperties.LoaderTitle;
                loaderTitle += delimiter;
                loaderTitle += Global.CurrentMenuName;
            }

            // Append extra text
            if (extraText.Length != 0) { loaderTitle += delimiter; }
            loaderTitle += extraText;

            // Set the title of the loader.
            titleBar_Title.Text = loaderTitle;
        }

        /// <summary>
        /// Initializes the remaining tabs/windows of this
        /// Windows Forms application, which are children of the
        /// main window using Multiple Document Interface
        /// </summary>
        private void InitializeMDIChildren()
        {
            // Instantiate the class of children
            MDIChildren = new ChildForms();

            // Create the children
            MDIChildren.MainMenu = new Main_Screen(this);
            MDIChildren.ModsMenu = new Mods_Screen(this);
            MDIChildren.CurrentMenu = MDIChildren.MainMenu;

            // Remove the borders from the children forms
            RemoveMDIChildBorder.SetBevel(this, false);

            // Show the main menu
            MDIChildren.MainMenu.Show();
        }

        /// <summary>
        /// Load the global theme once the base form has finished loading (all MDI children should also have finished loading)
        /// by then, as they are loaded in the constructor, pretty convenient.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme.
            Global.Theme.LoadTheme();
        }

        /// <summary>
        /// Hides the currently shown MDI Child form and presents a new one.
        /// </summary>
        private void SwapMenu(Form targetMenu)
        {
            // Hide the current menu.
            MDIChildren.CurrentMenu.Hide();

            // Show the new menu.
            targetMenu.Show();

            // Set new menu location.
            targetMenu.Location = new System.Drawing.Point(0,0);

            // Set new menu.
            MDIChildren.CurrentMenu = targetMenu;
        }

        /// <summary>
        /// Called when the mouse is moved within the client area of the button while the
        /// left (or right depending on user setting) mouse is down.
        /// As the title is a button, which covers the entire top panel it
        /// effectively serves as the top panel in itself in receiving mouse events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBarMouseDown(object sender, MouseEventArgs e) { MoveWindow.MoveTheWindow(this.Handle); }
    }
}
