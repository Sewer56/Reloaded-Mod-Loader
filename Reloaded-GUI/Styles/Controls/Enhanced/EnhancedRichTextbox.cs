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
using Reloaded_GUI.Styles.Controls.Interfaces;

namespace Reloaded_GUI.Styles.Controls.Enhanced
{
    /// <summary>
    /// Enhanced TextBox class providing support for painting custom borders.
    /// </summary>
    public class EnhancedRichTextbox : RichTextBox, IBorderedControl
    {
        //////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own stuff :V
        //////////////////////////////////////////////////////////////////////
        private static readonly int WM_PAINT = 0x000F;

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public EnhancedRichTextbox()
        {
            // Set control style.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        // Border Colours
        public Color LeftBorderColour { get; set; }
        public Color TopBorderColour { get; set; }
        public Color RightBorderColour { get; set; }
        public Color BottomBorderColour { get; set; }

        // Border Styles
        public ButtonBorderStyle LeftBorderStyle { get; set; }
        public ButtonBorderStyle RightBorderStyle { get; set; }
        public ButtonBorderStyle TopBorderStyle { get; set; }
        public ButtonBorderStyle BottomBorderStyle { get; set; }

        // Border Widths
        public int LeftBorderWidth { get; set; }
        public int RightBorderWidth { get; set; }
        public int TopBorderWidth { get; set; }
        public int BottomBorderWidth { get; set; }

        /// <summary>
        /// Paints our own border around the current textbox control.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintBorders(Graphics graphics)
        {
            // Obtain the control borders.
            Rectangle controlBounds = new Rectangle(0, 0, Width, Height);

            // Draw the border!
            ControlPaint.DrawBorder(graphics, controlBounds, LeftBorderColour,
                LeftBorderWidth, LeftBorderStyle, TopBorderColour, TopBorderWidth, TopBorderStyle, RightBorderColour,
                RightBorderWidth, RightBorderStyle, BottomBorderColour, BottomBorderWidth, BottomBorderStyle);
        }

        /// <summary>
        /// Override the window message handler.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Call base
            base.WndProc(ref m);

            // If it's a paint call, draw our own button ontop of the original.
            if (m.Msg == WM_PAINT)
            {
                // Obtain GFX Object for this Control.
                Graphics graphics = Graphics.FromHwnd(Handle);

                // Paint the border.
                PaintBorders(graphics);

                // Dispose GFX.
                graphics.Dispose();
            }
        }
    }
}
