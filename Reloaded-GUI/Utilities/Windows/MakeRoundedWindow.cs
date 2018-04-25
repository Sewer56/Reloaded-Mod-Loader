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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reloaded_GUI.Utilities.Windows
{
    /// <summary>
    /// Allows for setting the region of a windows form such that the form gains rounded edges.
    /// </summary>
    public class MakeRoundedWindow
    {
        /// <summary>
        /// The CreateRoundRectRgn function creates a rectangular region with rounded corners.
        /// </summary>
        /// <param name="nLeftRect">Specifies the x-coordinate of the upper-left corner of the region in device units.</param>
        /// <param name="nTopRect">Specifies the y-coordinate of the upper-left corner of the region in device units.</param>
        /// <param name="nRightRect">Specifies the x-coordinate of the lower-right corner of the region in device units.</param>
        /// <param name="nBottomRect">Specifies the y-coordinate of the lower-right corner of the region in device units.</param>
        /// <param name="nWidthEllipse">Specifies the width of the ellipse used to create the rounded corners in device units.</param>
        /// <param name="nHeightEllipse">Specifies the height of the ellipse used to create the rounded corners in device units.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// Rounds a Windows Form window.
        /// </summary>
        /// <param name="winForm">Windows form window</param>
        /// <param name="heightOfEllipse">Specifies the width of the ellipse used to create the rounded corners in device units.</param>
        /// <param name="widthOfEllipse">Specifies the height of the ellipse used to create the rounded corners in device units.</param>
        public static void RoundWindow(Form winForm, int widthOfEllipse, int heightOfEllipse)
        {
            winForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, winForm.Width, winForm.Height, widthOfEllipse, heightOfEllipse));
        }
    }
}
