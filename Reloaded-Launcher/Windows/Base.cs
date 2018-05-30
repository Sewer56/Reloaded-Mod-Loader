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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Reloaded.IO.Config;
using Reloaded.Native.WinAPI;
using Reloaded_GUI.Styles.Themes;
using ReloadedLauncher.Windows.Children;
using Reloaded_GUI;
using Reloaded_GUI.Utilities.Windows;

namespace ReloadedLauncher.Windows
{
    public partial class Base : Form
    {
        /// <summary>
        /// Stores all of the child forms to this Windows form which
        /// effectively are represented each of the tabs.
        /// </summary>
        public ChildForms ChildrenForms { get; set; }

        /// <summary>
        /// Waits for the user to release the tab mouse buttons before allowing
        /// tab switching to be disabled or enabled.
        /// </summary>
        public bool EnableTabSwitching
        {
            get => _enableTabSwitching;
            set
            {
                while (MouseButtons.HasFlag(MouseButtons.XButton1) || MouseButtons.HasFlag(MouseButtons.XButton2)) Thread.Sleep(8);
                _enableTabSwitching = value;
            }
        }


        /// <summary>
        /// A structure which defines all of the child forms
        /// that this form in question hosts.
        /// </summary>
        public class ChildForms
        {
            public MainScreen MainMenu { get; set; }
            public ModsScreen ModsMenu { get; set; }
            public ThemeScreen ThemeMenu { get; set; }
            public AboutScreen AboutMenu { get; set; }
            public ManageScreen ManageMenu { get; set; }
            public InputScreen InputMenu { get; set; }

            /// <summary>
            /// Stores the currently opened menu.
            /// </summary>
            public Form CurrentMenu { get; set; }
        }

        /// <summary>
        /// Allows for the enabling/disabling of tab switching
        /// with the mouse forward and back button. Use it to temporarily
        /// suspend tab switching for when, e.g. a mouse button is being actively 
        /// binded.
        /// </summary>
        private bool _enableTabSwitching;

        /// <summary>
        /// Thread which checks forward and back mouse buttons for
        /// switching tabs.
        /// </summary>
        private Thread _mouseCheckThread;

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
            Bindings.WindowsForms.Add(this);

            // Set this form as an MDI Container
            IsMdiContainer = true;

            // Open all child forms
            InitializeMdiChildren();

            // Enable Tab Switching
            _enableTabSwitching = true;
        }

        /// <summary>
        /// Updates the current title for the mod loader menu, taking in mind theme configuration
        /// and the delimiter + delimiter settings.
        /// </summary>
        /// <param name="extraText">This text is appended to the end of the menu title.</param>
        public void UpdateTitle(string extraText)
        {
            // Retrieve the theme properties.
            ThemeProperties.Theme themeProperties = Theme.ThemeProperties;

            // Stores the title of the mod loader, before it is set and rendered.
            string loaderTitle = "";
            string delimiter = "";

            // Set the delimiter.
            delimiter += themeProperties.TitleProperties.LoaderTitleDelimiter;

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
            if (!string.IsNullOrEmpty(extraText))
            {
                loaderTitle += delimiter;
                loaderTitle += extraText;
            }

            // Set the title of the loader.
            titleBar_Title.Text = loaderTitle;
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
                cp.ExStyle = cp.ExStyle | (int)Constants.WS_EX_COMPOSITED;
                return cp;
            }
        }

        #endregion

        /// <summary>
        /// Performs various tasks such as loading the global theme once the base form 
        /// has finished loading. Starts the mouse checking thread for forward/back
        /// button polling support.
        /// </summary>
        private void Base_Load(object sender, EventArgs e)
        {
            // Bind the load images delegate.
            Bindings.ApplyImagesDelegate += ApplyImagesDelegate; 

            // Load the global theme.
            Global.Theme.ThemeDirectory = Global.LoaderConfiguration.CurrentTheme;

            // Run mouse check thread.
            _mouseCheckThread = new Thread(CheckMouseInput);
            _mouseCheckThread.Start();
        }

        /// <summary>
        /// Automatically applies images to all of the currently loaded windows forms on change of theme.
        /// </summary>
        /// <param name="images">The images that are to be applied to the individual pages.</param>
        private void ApplyImagesDelegate(Bindings.ReloadedImages images)
        {
            // Assign individual images.
            Global.BaseForm.categoryBar_About.Image = images.AboutIconImage;
            Global.BaseForm.categoryBar_Manager.Image = images.ManagerImage;
            Global.BaseForm.categoryBar_Theme.Image = images.PaintImage;
            Global.BaseForm.categoryBar_Input.Image = images.InputImage;
            Global.BaseForm.categoryBar_Mods.Image = images.TweaksImage;
            Global.BaseForm.categoryBar_Games.Image = images.GamesImage;

            Global.BaseForm.ChildrenForms.ModsMenu.borderless_ConfigBox.Image = images.TweaksImage2;
            Global.BaseForm.ChildrenForms.ModsMenu.borderless_SourceBox.Image = images.GithubImage;
            Global.BaseForm.ChildrenForms.ModsMenu.borderless_WebBox.Image = images.WorldImage;

            Global.BaseForm.ChildrenForms.ThemeMenu.borderless_ConfigBox.Image = images.TweaksImage2;
            Global.BaseForm.ChildrenForms.ThemeMenu.borderless_SourceBox.Image = images.GithubImage;
            Global.BaseForm.ChildrenForms.ThemeMenu.borderless_WebBox.Image = images.WorldImage;
            
            Global.BaseForm.ChildrenForms.ManageMenu.box_GameDirectorySelect.BackgroundImage = images.TweaksImage;
            Global.BaseForm.ChildrenForms.ManageMenu.box_GameEXESelect.BackgroundImage = images.TweaksImage;
            Global.BaseForm.ChildrenForms.ManageMenu.box_GameFolderSelect.BackgroundImage = images.TweaksImage;
        }

        /// <summary>
        /// Initializes the remaining tabs/windows of this
        /// Windows Forms application, which are children of the
        /// main window using Multiple Document Interface
        /// </summary>
        private void InitializeMdiChildren()
        {
            // Instantiate the class of children
            ChildrenForms = new ChildForms
            {
                MainMenu = new MainScreen(this),
                ModsMenu = new ModsScreen(this),
                ThemeMenu = new ThemeScreen(this),
                AboutMenu = new AboutScreen(this),
                ManageMenu = new ManageScreen(this),
                InputMenu = new InputScreen(this)
            };

            // Create the children
            ChildrenForms.CurrentMenu = ChildrenForms.MainMenu;

            // Remove the borders from the children forms
            this.SetBevel(false);

            // Show the main menu
            ChildrenForms.MainMenu.Show();
        }

        /// <summary>
        /// Hides the currently shown MDI Child form and presents a new one.
        /// </summary>
        private void SwapMenu(Form targetMenu)
        {
            // Check if we are not in the target menu already.
            if (targetMenu != ChildrenForms.CurrentMenu)
            {
                // Hide the current menu.
                ChildrenForms.CurrentMenu.Hide();

                // Show the new menu.
                targetMenu.Show();

                // Set new menu location.
                targetMenu.Location = new Point(0, 0);

                // Set new menu.
                ChildrenForms.CurrentMenu = targetMenu;
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
                case MenuScreens.MainMenu: SwapMenu(ChildrenForms.MainMenu); break;
                case MenuScreens.ModsMenu: SwapMenu(ChildrenForms.ModsMenu); break;
                case MenuScreens.ThemeMenu: SwapMenu(ChildrenForms.ThemeMenu); break;
                case MenuScreens.AboutMenu: SwapMenu(ChildrenForms.AboutMenu); break;
                case MenuScreens.ManageMenu: SwapMenu(ChildrenForms.ManageMenu); break;
                case MenuScreens.InputMenu: SwapMenu(ChildrenForms.InputMenu); break;
            }
        }

        /// <summary>
        /// Iterates over the MDI Children to return enumerable storing the current menu.
        /// </summary>
        private MenuScreens GetCurrentMenuEnum()
        {
            // Grab a copy of the curernt menu reference.
            Form currentMenu = ChildrenForms.CurrentMenu;

            // Iterate over children and find matching menu.
            if (currentMenu == ChildrenForms.MainMenu)
                return MenuScreens.MainMenu;
            if (currentMenu == ChildrenForms.ModsMenu)
                return MenuScreens.ModsMenu;
            if (currentMenu == ChildrenForms.ThemeMenu)
                return MenuScreens.ThemeMenu;
            if (currentMenu == ChildrenForms.AboutMenu)
                return MenuScreens.AboutMenu;
            if (currentMenu == ChildrenForms.ManageMenu)
                return MenuScreens.ManageMenu;
            if (currentMenu == ChildrenForms.InputMenu) return MenuScreens.InputMenu;

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
            IntPtr baseHandle = (IntPtr)Invoke( (GetBaseHandleDelegate)(() => Handle) );

            while (true)
            {
                // Check if the base form has focus first.
                if (Reloaded.Native.Functions.WindowProperties.IsWindowActivated(baseHandle) && _enableTabSwitching)
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
            Functions.Shutdown();
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

        private void CategoryBar_Games_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.MainMenu); }
        private void CategoryBar_Mods_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.ModsMenu); }
        private void CategoryBar_Input_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.InputMenu); }
        private void CategoryBar_Theme_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.ThemeMenu); }
        private void CategoryBar_Manager_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.ManageMenu); }
        private void CategoryBar_About_Click(object sender, EventArgs e) { SwapMenu(ChildrenForms.AboutMenu); }

        #endregion

        private void categoryBar_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
