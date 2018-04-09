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
using System.Windows.Forms;
using Reloaded_GUI.Styles.Controls.Interfaces;

namespace Reloaded_GUI.Styles.Controls.Custom
{
    /// <summary>
    /// Completely custom class providing a button which swaps its 
    /// back and forecolor depending on whether a flag is set.
    /// The intended use is showing whether the user has pressed a controller button.
    /// </summary>
    public class ButtonPressIndicator : Control, IBorderedControl
    {
        ////////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own control :V
        ////////////////////////////////////////////////////////////////////////
        private static readonly int WM_PAINT = 0x000F;
        private BufferedGraphics _backBuffer;

        // Painting brushes
        private Brush _backgroundBrush;

        // Is button enabled?
        private bool _buttonEnabled;

        /////////////////////////////////
        // Implement own Double Buffering
        /////////////////////////////////
        private readonly BufferedGraphicsContext _graphicManager;
        private Brush _textBrush;

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public ButtonPressIndicator()
        {
            // Redirect all painting to us.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Setup default text format.
            StringFormat = new StringFormat();
            StringFormat.LineAlignment = StringAlignment.Center;
            StringFormat.Alignment = StringAlignment.Center;

            // Setup double buffering.
            _graphicManager = BufferedGraphicsManager.Current;
            ReallocateBuffer();
        }

        /// <summary>
        /// Gets or sets the text to be drawn inside the button.
        /// </summary>
        public override string Text
        {
            get => base.Text;
            set { base.Text = value; Invoke((Action)Refresh); }
        }

        /// <summary>
        /// Set to true to swap the background and foreground colour.
        /// </summary>
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set
            {
                if (_buttonEnabled != value)
                {
                    _buttonEnabled = value;
                    Invoke((Action)Refresh);
                }
            }
        }

        /// <summary>
        /// Defines the format of the button text to be displayed.
        /// </summary>
        public StringFormat StringFormat { get; set; }

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
            PaintButton(_backBuffer.Graphics);

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
        protected void PaintButton(Graphics graphics)
        {
            // Set brush to draw the background.
            if (_buttonEnabled)
                _backgroundBrush = new SolidBrush(ForeColor);
            else
                _backgroundBrush = new SolidBrush(BackColor);

            // Region of the background.
            Rectangle backgroundRegion = new Rectangle
            (
                LeftBorderWidth,
                TopBorderWidth,
                Width - LeftBorderWidth - RightBorderWidth,
                Height - TopBorderWidth - BottomBorderWidth
            );

            // Paint the background.
            graphics.FillRectangle(_backgroundBrush, backgroundRegion);

            // Obtain the control borders.
            Rectangle controlBounds = new Rectangle(0, 0, Width, Height);

            // Set brush to draw the text.
            if (_buttonEnabled)
                _textBrush = new SolidBrush(BackColor);
            else
                _textBrush = new SolidBrush(ForeColor);

            // Draw the text.
            graphics.DrawString(Text, Font, _textBrush, controlBounds, StringFormat);

            // Cleanup
            _textBrush.Dispose();
            _backgroundBrush.Dispose();
        }
    }
}
