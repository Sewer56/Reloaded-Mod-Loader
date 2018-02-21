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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ReloadedLauncher.Styles.Controls.Interfaces;

namespace ReloadedLauncher.Styles.Controls.Enhanced
{
    public class EnhancedComboBox : ComboBox, IBorderedControl
    {
        /////////////////////
        // Painting Constants
        /////////////////////

        /// <summary>
        /// Specifies the width of the drop down button containing the arrow.
        /// </summary>
        private const int WIDTH_OF_DROPBUTTON = 20;

        //////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own stuff :V
        //////////////////////////////////////////////////////////////////////
        private static readonly int WM_PAINT = 0x000F;
        private Brush arrowBrush = new SolidBrush(SystemColors.ControlText);

        //////////////////////////////
        //////////////////////////////
        //////////////////////////////
        private Brush backgroundBrush;
        private Color bottomBorderColour;
        public ButtonBorderStyle bottomBorderStyle;
        public int bottomWidth;

        private Color buttonColor;

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Colours
        private Color dropDownArrowColor;

        private Brush dropDownButtonBrush = new SolidBrush(SystemColors.Control);

        // Border Colours
        private Color leftBorderColour;

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Border Styles
        public ButtonBorderStyle leftBorderStyle;

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Border Widths
        public int leftWidth;
        private Color rightBorderColour;
        public ButtonBorderStyle rightBorderStyle;
        public int rightWidth;
        private Color topBorderColour;
        public ButtonBorderStyle topBorderStyle;
        public int topWidth;

        /// <summary>
        /// Constructor :V
        /// </summary>
        public EnhancedComboBox()
        {
            // Double buffer this control.
            DoubleBuffered = true;
            HighlightColor = Color.Red;
            DrawItem += EnhancedComboBox_DrawItem;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        // Arrow Colour
        public Color DropDownArrowColour
        {
            get => dropDownArrowColor;
            set { dropDownArrowColor = value; arrowBrush = new SolidBrush(DropDownArrowColour); Invalidate(); }
        }

        // Drop Down Button Colour
        public Color DropDownButtonColour
        {
            get => buttonColor;
            set { buttonColor = value; dropDownButtonBrush = new SolidBrush(buttonColor); Invalidate(); }
        }

        // Sets the colour of highlighed items.
        public Color HighlightColor { get; set; }

        // Border Properties
        public Color LeftBorderColour { get => leftBorderColour;
            set { leftBorderColour = value; Invalidate(); } }
        public Color TopBorderColour { get => topBorderColour;
            set { topBorderColour = value; Invalidate(); } }
        public Color RightBorderColour { get => rightBorderColour;
            set { rightBorderColour = value; Invalidate(); } }
        public Color BottomBorderColour { get => bottomBorderColour;
            set { bottomBorderColour = value; Invalidate(); } }

        // Border Properties
        public ButtonBorderStyle LeftBorderStyle { get => leftBorderStyle;
            set { leftBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle RightBorderStyle { get => rightBorderStyle;
            set { rightBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle TopBorderStyle { get => topBorderStyle;
            set { topBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle BottomBorderStyle { get => bottomBorderStyle;
            set { bottomBorderStyle = value; Invalidate(); } }

        // Border Properties
        public int LeftBorderWidth { get => leftWidth;
            set { leftWidth = value; Invalidate(); } }
        public int RightBorderWidth { get => rightWidth;
            set { rightWidth = value; Invalidate(); } }
        public int TopBorderWidth { get => topWidth;
            set { topWidth = value; Invalidate(); } }
        public int BottomBorderWidth { get => bottomWidth;
            set { bottomWidth = value; Invalidate(); } }

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
                // Obtain GFX Object
                Graphics graphics = Graphics.FromHwnd(Handle);

                // Remove Selection Border
                RemoveSelectionBorder(graphics);

                // Draw the drop-down button & regular background.
                Rectangle backgroundRectangle = new Rectangle(0, 0, Width, Height);
                Rectangle dropDownRectangle = new Rectangle(Width - WIDTH_OF_DROPBUTTON, 0, WIDTH_OF_DROPBUTTON, Height);

                //graphics.FillRectangle(dropDownButtonBrush, backgroundRectangle);
                graphics.FillRectangle(dropDownButtonBrush, dropDownRectangle);

                // Draw arrows.
                DrawArrow(graphics);

                // Rectangles!
                Rectangle controlBounds = new Rectangle(0,0,Width,Height);

                // Draw the border!
                ControlPaint.DrawBorder(graphics, controlBounds, LeftBorderColour,
                    LeftBorderWidth, LeftBorderStyle, TopBorderColour, TopBorderWidth, TopBorderStyle, RightBorderColour,
                    RightBorderWidth, RightBorderStyle, BottomBorderColour, BottomBorderWidth, BottomBorderStyle);

                // Dispose GFX
                graphics.Dispose();
            }
        }

        /// <summary>
        /// By default, depending on the windows theme, a two pixel border may be displayed over
        /// the control if the user is in focus or has selected the control.
        /// This method draws over the selection border such that it matches the background colour of the button.
        /// </summary>
        /// <param name="graphics">The graphics object for the current object.</param>
        private void RemoveSelectionBorder(Graphics graphics)
        {
            // Create the path for the border removal for builtin border.
            GraphicsPath borderRemovalPath = new GraphicsPath();
            PointF topLeftCorner = new PointF(0, 0);
            PointF topRightCorner = new PointF(Width, 0);
            PointF bottomLeftCorner = new PointF(0, Height);
            PointF bottomRightCorner = new PointF(Width, Height);

            // Background Brush
            backgroundBrush = new SolidBrush(BackColor);
            Pen backgroundPen = new Pen(backgroundBrush, 2);

            // Fill in the points!
            borderRemovalPath.AddLine(topLeftCorner, topRightCorner);
            borderRemovalPath.AddLine(topRightCorner, bottomRightCorner);
            borderRemovalPath.AddLine(bottomRightCorner, bottomLeftCorner);
            borderRemovalPath.AddLine(bottomLeftCorner, topLeftCorner);

            // Draw the BG
            graphics.DrawPath(backgroundPen, borderRemovalPath);
        }

        /// <summary>
        /// Draws the arrow for the drop down menu.
        /// </summary>
        /// <param name="graphics">The graphics object for the current object.</param>
        private void DrawArrow(Graphics graphics)
        {
            // Create the path for the arrow
            GraphicsPath arrowPath = new GraphicsPath();

            // Define arrow vertices
            PointF arrowTopLeft = new PointF(Width - 13, (Height - 5) / 2);
            PointF arrowTopRight = new PointF(Width - 6, (Height - 5) / 2);
            PointF arrowBottom = new PointF(Width - 9, (Height + 2) / 2);

            // State the arrow points!
            arrowPath.AddLine(arrowTopLeft, arrowTopRight);
            arrowPath.AddLine(arrowTopRight, arrowBottom);
            arrowPath.AddLine(arrowBottom, arrowTopLeft);

            // Fill the arrow
            graphics.FillPath(arrowBrush, arrowPath);
        }

        /// <summary>
        /// Draw the individual Combobox items manually over the place of where they would
        /// be drawn otherwise..
        /// </summary>
        private void EnhancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Check the index of the item, if below 0, it's not an item.
            if (e.Index < 0) return;

            // Cast the ComboBox Sender
            ComboBox combo = sender as ComboBox;

            // If it's selected, draw the item with the highlight colour.
            if (e.State.HasFlag(DrawItemState.Selected))
                e.Graphics.FillRectangle(new SolidBrush(HighlightColor), e.Bounds);

            // Otherwise use the BG Colour.
            else
                e.Graphics.FillRectangle(new SolidBrush(combo.BackColor), e.Bounds);

            // Draw the original intended text.
            e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
        }
    }
}
