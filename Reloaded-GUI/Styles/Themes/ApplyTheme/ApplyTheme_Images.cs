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
using System.Windows.Forms;

namespace Reloaded_GUI.Styles.Themes.ApplyTheme
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
            // Load the images from HDD.
            try
            {
                Bindings.Images.AboutIconImage = Image.FromFile(imagesFolder + "\\About-Icon.png");
                Bindings.Images.ManagerImage = Image.FromFile(imagesFolder + "\\Entry-Icon.png");
                Bindings.Images.PaintImage = Image.FromFile(imagesFolder + "\\Paint-Icon.png");
                Bindings.Images.InputImage = Image.FromFile(imagesFolder + "\\Controller-Icon.png");
                Bindings.Images.TweaksImage = Image.FromFile(imagesFolder + "\\Tweaks-Icon.png");
                Bindings.Images.GamesImage = Image.FromFile(imagesFolder + "\\Main-Icon.png");
                Bindings.Images.TweaksImage2 = Image.FromFile(imagesFolder + "\\Tweaks2-Icon.png");
                Bindings.Images.GithubImage = Image.FromFile(imagesFolder + "\\Github-Icon.png");
                Bindings.Images.WorldImage = Image.FromFile(imagesFolder + "\\World-Icon.png");
            }
            catch { MessageBox.Show("Could not load theme images, either the current theme is not installed or no theme is installed."); }
            
            // Call the relevant delegates to set the images loaded from HDD
            Bindings.ApplyImagesDelegate?.Invoke(Bindings.Images);
        }
    }
}
