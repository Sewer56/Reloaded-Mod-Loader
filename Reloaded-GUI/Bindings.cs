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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Reloaded_GUI
{
    /// <summary>
    /// Method/delegate/variable declarations for Reloaded-GUI to use for theming the Mod Loader.
    /// These are for you to assign manually to.
    /// </summary>
    public static class Bindings
    {
        /// <summary>
        /// Stores a list of all instantiated windows forms. 
        /// Used for dynamically setting themes on each form as they are loaded.
        /// </summary>
        public static List<Form> WindowsForms { get; set; } = new List<Form>();

        /// <summary>
        /// Delegate that is fired when the Theme Engine loads the images for the specified theme.
        /// You should add your own function to this delegate and manually assign your images in your
        /// windows forms as required. Else grab the images manually from <see cref="Images"/>
        /// </summary>
        public static ApplyImages ApplyImagesDelegate { get; set; }

        /// <summary>
        /// Append your function to this delegate to assign the "About Icon" image
        /// </summary>
        public static ReloadedImages Images { get; set; } = new ReloadedImages();

        /// <summary>
        /// ImageStruct contains all of the the individual images that are loaded by the
        /// individual themes.
        /// </summary>
        public class ReloadedImages
        {
            /// <summary>
            /// Reloaded Launcher: About Icon Image
            /// </summary>
            public Image AboutIconImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Manager Tab Option Image
            /// </summary>
            public Image ManagerImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Theme Category Icon
            /// </summary>
            public Image PaintImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Controller Icon
            /// </summary>
            public Image InputImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Tweaks Image
            /// </summary>
            public Image TweaksImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Games Image/Main Icon
            /// </summary>
            public Image GamesImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: Tweaks Alternative Image
            /// </summary>
            public Image TweaksImage2 { get; set; }

            /// <summary>
            /// Reloaded Launcher: Github Icon
            /// </summary>
            public Image GithubImage { get; set; }

            /// <summary>
            /// Reloaded Launcher: World Icon
            /// </summary>
            public Image WorldImage { get; set; }
        }

        /// <summary>
        /// Delegate type used to apply an image to a windows forms control or other supporting self
        /// declared object.
        /// </summary>
        /// <param name="images">The images to be applied to the control/object.</param>
        public delegate void ApplyImages(ReloadedImages images);
    }
}
