using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Reloaded.Misc.Config;
using Reloaded.Native;
using ReloadedLauncher.Styles.Themes;
using ReloadedLauncher.Utilities.Windows;
using ReloadedLauncher.Windows.Children;

namespace ReloadedLauncher
{
    public partial class Base : Form
    {
        /// <summary>
        /// Allows for the enabling/disabling of tab switching
        /// with the mouse forward and back button. Use it to temporarily
        /// suspend tab switching for when, e.g. a mouse button is being actively 
        /// binded.
        /// </summary>
        private bool enableTabSwitching;

        /// <summary>
        /// Thread which checks forward and back mouse buttons for
        /// switching tabs.
        /// </summary>
        private Thread mouseCheckThread;

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

            // Enable Tab Switching
            enableTabSwitching = true;
        }

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
        /// Waits for the user to release the tab mouse buttons before allowing
        /// tab switching to be disabled or enabled.
        /// </summary>
        public bool EnableTabSwitching
        {
            get => enableTabSwitching;
            set
            {
                while (MouseButtons.HasFlag(MouseButtons.XButton1) || MouseButtons.HasFlag(MouseButtons.XButton2)) Thread.Sleep(8);
                enableTabSwitching = value;
            }
        }

        /// <summary>
        /// Performs various tasks such as loading the global theme once the base form 
        /// has finished loading. Starts the mouse checking thread for forward/back
        /// button polling support.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Load the global theme.
            Global.Theme.LoadTheme();

            // Run mouse check thread.
            mouseCheckThread = new Thread(CheckMouseInput);
            mouseCheckThread.Start();
        }

        /// <summary>
        /// Updates the current title for the mod loader menu, taking in mind theme configuration
        /// and the delimiter + delimiter settings.
        /// </summary>
        /// <param name="extraText">This text is appended to the end of the menu title.</param>
        public void UpdateTitle(string extraText)
        {
            // Retrieve the theme properties.
            ThemePropertyParser.ThemeConfig themeProperties = Theme.ThemeProperties;

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
            if (extraText.Length != 0) loaderTitle += delimiter;
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
            MDIChildren.ThemeMenu = new Theme_Screen(this);
            MDIChildren.AboutMenu = new About_Screen(this);
            MDIChildren.ManageMenu = new Manage_Screen(this);
            MDIChildren.InputMenu = new Input_Screen(this);
            MDIChildren.CurrentMenu = MDIChildren.MainMenu;

            // Remove the borders from the children forms
            this.SetBevel(false);

            // Show the main menu
            MDIChildren.MainMenu.Show();
        }

        /// <summary>
        /// Hides the currently shown MDI Child form and presents a new one.
        /// </summary>
        private void SwapMenu(Form targetMenu)
        {
            // Check if we are not in the target menu already.
            if (targetMenu != MDIChildren.CurrentMenu)
            {
                // Hide the current menu.
                MDIChildren.CurrentMenu.Hide();

                // Show the new menu.
                targetMenu.Show();

                // Set new menu location.
                targetMenu.Location = new Point(0, 0);

                // Set new menu.
                MDIChildren.CurrentMenu = targetMenu;
            }
        }

        /// <summary>
        /// Enters the menu either to the right or to the left of the current menu.
        /// </summary>
        /// <param name="forward">Set to true to cycle right, else cycle left.</param>
        private void CycleMenu(bool forward)
        {
            // Get the current menu.
            int currentMenu = (int)GetCurrentMenuEnum();

            // Obtain the last number for the menus
            // This is the modulus for forward looping.
            int maxMenu = Enum.GetValues(typeof(MenuScreens)).Cast<int>().Max() + 1;

            // Add or remove the value.
            if (forward)
            {
                // Add to the current menu.
                currentMenu += 1;

                // Add 1 to modulus and perform mod operation to forward cycle.
                currentMenu = currentMenu % maxMenu;  
            }
            else
            {
                // Take away from current menu.
                currentMenu -= 1;

                // Check for cycling, remove from max menu if last.
                // e.g. 2 + (-1) = 1
                // (2 = maxMenu)
                if (currentMenu < 0) currentMenu = maxMenu + currentMenu;
            }

            // Open new menu.
            OpenNewMenu((MenuScreens)currentMenu);
        }

        /// <summary>
        /// Opens a new menu using the SwapMenus method.
        /// </summary>
        /// <param name="menuScreen">The menu screen to be opeened.</param>
        private void OpenNewMenu(MenuScreens menuScreen)
        {
            switch (menuScreen)
            {
                case MenuScreens.MainMenu: SwapMenu(MDIChildren.MainMenu); break;
                case MenuScreens.ModsMenu: SwapMenu(MDIChildren.ModsMenu); break;
                case MenuScreens.ThemeMenu: SwapMenu(MDIChildren.ThemeMenu); break;
                case MenuScreens.AboutMenu: SwapMenu(MDIChildren.AboutMenu); break;
                case MenuScreens.ManageMenu: SwapMenu(MDIChildren.ManageMenu); break;
                case MenuScreens.InputMenu: SwapMenu(MDIChildren.InputMenu); break;
            }
        }

        /// <summary>
        /// Iterates over the MDI Children to return enumerable storing the current menu.
        /// </summary>
        private MenuScreens GetCurrentMenuEnum()
        {
            // Grab a copy of the curernt menu reference.
            Form currentMenu = MDIChildren.CurrentMenu;

            // Iterate over children and find matching menu.
            if (currentMenu == MDIChildren.MainMenu)
                return MenuScreens.MainMenu;
            if (currentMenu == MDIChildren.ModsMenu)
                return MenuScreens.ModsMenu;
            if (currentMenu == MDIChildren.ThemeMenu)
                return MenuScreens.ThemeMenu;
            if (currentMenu == MDIChildren.AboutMenu)
                return MenuScreens.AboutMenu;
            if (currentMenu == MDIChildren.ManageMenu)
                return MenuScreens.ManageMenu;
            if (currentMenu == MDIChildren.InputMenu) return MenuScreens.InputMenu;

            // Return main menu as default.
            return MenuScreens.MainMenu;
        }

        /// <summary>
        /// Runs an infinite loop to check current mouse input.
        /// </summary>
        private void CheckMouseInput()
        {
            // Create delegate for thread-safe tab cycling.
            CycleMouseDelegate cycleMenuDelegate = CycleMenu;

            // Obtain handle of Base window.
            IntPtr baseHandle = (IntPtr)Invoke( (GetBaseHandleDelegate)delegate { return Handle; } );

            while (true)
            {
                // Check if the base form has focus first.
                if (Reloaded.Native.Windows.IsWindowActivated(baseHandle) && enableTabSwitching)
                {
                    // Check input.
                    // Back button.
                    if (MouseButtons.HasFlag(MouseButtons.XButton1))
                    {
                        Invoke(cycleMenuDelegate, false);
                        while (MouseButtons.HasFlag(MouseButtons.XButton1)) Thread.Sleep(32);
                    }

                    // Forward button.
                    if (MouseButtons.HasFlag(MouseButtons.XButton2))
                    {
                        Invoke(cycleMenuDelegate, true);
                        while (MouseButtons.HasFlag(MouseButtons.XButton2)) Thread.Sleep(32);
                    }
                }

                // Sleep
                Thread.Sleep(32);
            }
        }

        /// <summary>
        /// Called when the mouse is moved within the client area of the button while the
        /// left (or right depending on user setting) mouse is down.
        /// As the title is a button, which covers the entire top panel it
        /// effectively serves as the top panel in itself in receiving mouse events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBarMouseDown(object sender, MouseEventArgs e) { MoveWindow.MoveTheWindow(Handle); }

        /// <summary>
        /// If the user htis ALT + F4, close the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Base_FormClosed(object sender, FormClosedEventArgs e)
        {
            Shutdown();
        }

        /// <summary>
        /// Kills the mod loader safely.
        /// </summary>
        public void Shutdown()
        {
            // Shut the program.
            Environment.Exit(0);
        }


        /// <summary>
        /// A structure which defines all of the child forms
        /// that this form in question hosts.
        /// </summary>
        public class ChildForms
        {
            public Main_Screen MainMenu { get; set; }
            public Mods_Screen ModsMenu { get; set; }
            public Theme_Screen ThemeMenu { get; set; }
            public About_Screen AboutMenu { get; set; }
            public Manage_Screen ManageMenu { get; set; }
            public Input_Screen InputMenu { get; set; }

            /// <summary>
            /// Stores the currently opened menu.
            /// </summary>
            public Form CurrentMenu { get; set; }
        }

        /// <summary>
        /// Stores the menu screens.
        /// </summary>
        private enum MenuScreens
        {
            MainMenu,
            ModsMenu,
            InputMenu,
            ThemeMenu,
            ManageMenu,
            AboutMenu
        }

        //
        // Delegates for Cross-Thread Functions
        //

        private delegate IntPtr GetBaseHandleDelegate();

        private delegate void CycleMouseDelegate(bool forward);

        // Click Events for Category Buttons

        #region Category Buttons

        private void CategoryBar_Games_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.MainMenu); }
        private void CategoryBar_Mods_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.ModsMenu); }
        private void CategoryBar_Input_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.InputMenu); }
        private void CategoryBar_Theme_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.ThemeMenu); }
        private void CategoryBar_Manager_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.ManageMenu); }
        private void CategoryBar_About_Click(object sender, EventArgs e) { SwapMenu(MDIChildren.AboutMenu); }

        #endregion
    }
}
