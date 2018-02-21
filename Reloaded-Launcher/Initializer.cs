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

using ReloadedLauncher.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace ReloadedLauncher
{
    /// <summary>
    /// Defines the class that sets up and starts off the mod loader config manager.
    /// The actual real entry point for the application is Program.cs, this is merely an abstraction.
    /// </summary>
    class Initializer
    {
        /// <summary>
        /// Initializes the Windows Forms Application.
        /// </summary>
        public Initializer()
        {
            // Write Loader Location
            File.WriteAllText(LoaderPaths.GetModLoaderLinkLocation(), Environment.CurrentDirectory);

            // Initialize the Configs.
            InitializeGlobalProperties();
            
            // Initialize all WinForms.
            InitializeForms();
        }

        /// <summary>
        /// Loads all configurations to be used by the program, alongside with their parsers.
        /// </summary>
        private void InitializeGlobalProperties()
        {
            // Instantiate the Global Config Manager
            Global.ConfigurationManager = new LoaderConfigManager();

            // Grab relevant configs.
            // Note: Game list is grabbed upon entry to the main screen form.
            Global.LoaderConfiguration = Global.ConfigurationManager.LoaderConfigParser.ParseConfig();

            // Initialize other Properties.
            Global.Theme = new Styles.Themes.Theme();
            Global.WindowsForms = new List<Form>();

            // Set the initial menu name.
            Global.CurrentMenuName = "Main Menu";

            // Set the initial theme.
            Global.Theme.ThemeDirectory = Global.LoaderConfiguration.CurrentTheme;
        }

        /// <summary>
        /// Initializes all of the windows forms to be used by the application.
        /// </summary>
        private void InitializeForms()
        {
            // Store the base form in Global
            Global.BaseForm = new Base();

            // Launches the first window.
            Application.Run(Global.BaseForm);
        }
    }
}
