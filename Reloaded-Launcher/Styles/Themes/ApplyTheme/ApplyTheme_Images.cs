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

using System.Drawing;

namespace ReloadedLauncher.Styles.Themes
{
    /// <summary>
    /// Applies a theme to the current form.
    /// This file handles the image aspect of the form.
    /// </summary>
    public static partial class ApplyTheme
    {
        /// <summary>
        /// Loads all of the theme images from storage onto the relevant base form buttons.
        /// </summary>
        /// <param name="imagesFolder">Dictates the folder where the images are supposed to be loaded from.</param>
        public static void LoadImages(string imagesFolder)
        {
            // If the baseform is instantiated
            if (Global.BaseForm != null)
            {
                // Load the images from HDD.
                Global.BaseForm.categoryBar_About.Image = Image.FromFile(imagesFolder + "\\About-Icon.png");
                Global.BaseForm.categoryBar_Manager.Image = Image.FromFile(imagesFolder + "\\Entry-Icon.png");
                Global.BaseForm.categoryBar_Theme.Image = Image.FromFile(imagesFolder + "\\Paint-Icon.png");
                Global.BaseForm.categoryBar_Input.Image = Image.FromFile(imagesFolder + "\\Controller-Icon.png");
                Global.BaseForm.categoryBar_Mods.Image = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                Global.BaseForm.categoryBar_Games.Image = Image.FromFile(imagesFolder + "\\Main-Icon.png");

                // If the mods form is created.
                if (Global.BaseForm.MDIChildren.ModsMenu != null)
                {
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_ConfigBox.Image = Image.FromFile(imagesFolder + "\\Tweaks2-Icon.png");
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_SourceBox.Image = Image.FromFile(imagesFolder + "\\Github-Icon.png");
                    Global.BaseForm.MDIChildren.ModsMenu.borderless_WebBox.Image = Image.FromFile(imagesFolder + "\\World-Icon.png");
                }

                // If the theme form is created.
                if (Global.BaseForm.MDIChildren.ThemeMenu != null)
                {
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_ConfigBox.Image = Image.FromFile(imagesFolder + "\\Tweaks2-Icon.png");
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_SourceBox.Image = Image.FromFile(imagesFolder + "\\Github-Icon.png");
                    Global.BaseForm.MDIChildren.ThemeMenu.borderless_WebBox.Image = Image.FromFile(imagesFolder + "\\World-Icon.png");
                }

                // If the manage form is created.
                if (Global.BaseForm.MDIChildren.ManageMenu != null)
                {
                    Global.BaseForm.MDIChildren.ManageMenu.box_GameDirectorySelect.BackgroundImage = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                    Global.BaseForm.MDIChildren.ManageMenu.box_GameEXESelect.BackgroundImage = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                    Global.BaseForm.MDIChildren.ManageMenu.box_GameFolderSelect.BackgroundImage = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                }
            }
        }
    }
}
