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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Reloaded_GUI.Styles.Controls.Interfaces;

namespace Reloaded_GUI.Styles.Controls.Custom
{
    /// <summary>
    /// Completely custom class providing a vertical progress bar implementation.
    /// The implementation is intended to show controller analog inputs, thus no smoothing
    /// or animation functions are implemented.
    /// The progress bar is a thin implementation, not providing much input checking, etc.
    /// </summary>
    public class VerticalProgressBar : Control, IBorderedControl
    {
        ////////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own control :V
        ////////////////////////////////////////////////////////////////////////
        private static readonly int WM_PAINT = 0x000F;
        private BufferedGraphics _backBuffer;

        /////////////////////////////////
        // Implement own Double Buffering
        /////////////////////////////////
        private readonly BufferedGraphicsContext _graphicManager;
        public int MAX_VALUE = 1000;
        public int MIN_VALUE = 0;
        private int _progressValue;

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public VerticalProgressBar()
        {
            // Redirect all painting to us.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Setup double buffering.
            _graphicManager = BufferedGraphicsManager.Current;
            ReallocateBuffer();
        }

        // Stores progress and details of progress bar.
        public Color ProgressColour { get; set; }

        /// <summary>
        /// Gets or sets the value of the progress bar, from MIN_VALUE (0) to MAX_VALUE (1000)
        /// </summary>
        [Description("Specifies the value of the progress bar, from MIN_VALUE (0) to MAX_VALUE (1000)")]
        public int Value
        {
            get => _progressValue;
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    Invoke((Action)Refresh); 
                }
            }
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
        /// Calls the method to reallocate buffer when the control size changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            // Call base.
            base.OnResize(e);

            // Reallocate buffer.
            ReallocateBuffer();
        }

        /// <summary>
        /// Reallocates the buffer of the control for drawing.
        /// </summary>
        private void ReallocateBuffer()
        {
            // Reallocate buffer.
            _graphicManager.MaximumBuffer = new Size(Width + 1, Height + 1);
            _backBuffer = _graphicManager.Allocate(CreateGraphics(), new Rectangle(0, 0, Width, Height));
        }

        /// <summary>
        /// Override the window message handler.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Call base
            base.WndProc(ref m);

            // If it's a paint call, draw our own control.
            if (m.Msg == WM_PAINT) DrawControl();
        }

        //////////////////////////////////////////////////////////////
        // Our own graphics job, for when to draw the control contents
        //////////////////////////////////////////////////////////////

        /// <summary>
        /// Draws the vertical progress bar control.
        /// </summary>
        protected void DrawControl()
        {
            // Paint the background.
            PaintBackground(_backBuffer.Graphics);

            // Paint the border.
            PaintBorders(_backBuffer.Graphics);

            // Fills the vertical progress bar with the appropriate value.
            FillProgressBar(_backBuffer.Graphics);

            // Render the backbuffer to the control's GFX object.
            _backBuffer.Render(Graphics.FromHwnd(Handle));
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintBackground(Graphics graphics)
        {
            // Define and paint the background area.
            Brush brush = new SolidBrush(BackColor);
            Rectangle controlBounds = new Rectangle(0, 0, Width, Height);

            // Draw
            graphics.FillRectangle(brush, controlBounds);

            // Cleanup
            brush.Dispose();
        }

        /// <summary>
        /// Paints our own border around the current control.
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
        /// Fills the vertical progress bar with the current level of progression.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void FillProgressBar(Graphics graphics)
        {
            // Convert current value into height percentage.
            float fillPercentage = _progressValue / (float)MAX_VALUE;

            // Height minus borders
            int borderlessHeight = Height - BottomBorderWidth - TopBorderWidth;

            // Obtain the height and width of the rectangle to fill with.
            int heightRectangle = (int)(borderlessHeight * fillPercentage) - 1; // -1: Do not draw over border.
            int widthRectangle = Width - RightBorderWidth - LeftBorderWidth;

            // Obtain top, left of the fill.
            int topRectangle = Height - heightRectangle - BottomBorderWidth;
            int leftRectangle = LeftBorderWidth;

            // Define and paint the rectangle.
            Brush brush = new SolidBrush(ProgressColour);
            Rectangle rectangle = new Rectangle(leftRectangle, topRectangle, widthRectangle, heightRectangle);

            // Draw
            graphics.FillRectangle(brush, rectangle);

            // Cleanup
            brush.Dispose();
        }
    }
}
