using HeroesModLoaderConfig.Styles.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls.Enhanced
{
    class EnhancedComboBox : ComboBox
    {
        // Border Colours
        private Color leftBorderColour; 
        private Color topBorderColour;
        private Color rightBorderColour;
        private Color bottomBorderColour;

        // Border Properties
        public Color LeftBorderColour { get { return leftBorderColour; } set { leftBorderColour = value; Invalidate(); } }
        public Color TopBorderColour { get { return topBorderColour; } set { topBorderColour = value; Invalidate(); } }
        public Color RightBorderColour { get { return rightBorderColour; } set { rightBorderColour = value; Invalidate(); } }
        public Color BottomBorderColour { get { return bottomBorderColour; } set { bottomBorderColour = value; Invalidate(); } }

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Border Styles
        public ButtonBorderStyle leftBorderStyle;
        public ButtonBorderStyle rightBorderStyle;
        public ButtonBorderStyle topBorderStyle;
        public ButtonBorderStyle bottomBorderStyle;

        // Border Properties
        public ButtonBorderStyle LeftBorderStyle { get { return leftBorderStyle; } set { leftBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle RightBorderStyle { get { return rightBorderStyle; } set { rightBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle TopBorderStyle { get { return topBorderStyle; } set { topBorderStyle = value; Invalidate(); } }
        public ButtonBorderStyle BottomBorderStyle { get { return bottomBorderStyle; } set { bottomBorderStyle = value; Invalidate(); } }

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Border Widths
        public int leftWidth;
        public int rightWidth;
        public int topWidth;
        public int bottomWidth;

        // Border Properties
        public int LeftBorderWidth { get { return leftWidth; } set { leftWidth = value; Invalidate(); } }
        public int RightBorderWidth { get { return rightWidth; } set { rightWidth = value; Invalidate(); } }
        public int TopBorderWidth { get { return topWidth; } set { topWidth = value; Invalidate(); } }
        public int BottomBorderWidth { get { return bottomWidth; } set { bottomWidth = value; Invalidate(); } }

        ////////////////////////////////
        ////////////////////////////////
        ////////////////////////////////

        // Colours
        private Color dropDownArrowColor;
        private Brush arrowBrush = new SolidBrush(SystemColors.ControlText);

        private Color buttonColor;
        private Brush dropDownButtonBrush = new SolidBrush(SystemColors.Control); 

        // Arrow Colour
        public Color DropDownArrowColour
        {
            get { return dropDownArrowColor; }
            set { dropDownArrowColor = value; arrowBrush = new SolidBrush(this.DropDownArrowColour); this.Invalidate(); }
        }

        // Drop Down Button Colour
        public Color DropDownButtonColour
        {
            get { return buttonColor; }
            set { buttonColor = value; dropDownButtonBrush = new SolidBrush(this.buttonColor); this.Invalidate(); }
        }

        //////////////////////////////
        //////////////////////////////
        //////////////////////////////
        private Brush backgroundBrush;

        // Sets the colour of highlighed items.
        public Color HighlightColor { get; set; }

        /// <summary>
        /// Constructor :V
        /// </summary>
        public EnhancedComboBox() : base()
        {
            // Double buffer this control.
            this.DoubleBuffered = true;
            this.HighlightColor = Color.Red;
            this.DrawItem += new DrawItemEventHandler(EnhancedComboBox_DrawItem);
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        /////////////////////
        // Painting Constants
        /////////////////////

        /// <summary>
        /// Specifies the width of the drop down button containing the arrow.
        /// </summary>
        const int WIDTH_OF_DROPBUTTON = 20;

        //////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own stuff :V
        //////////////////////////////////////////////////////////////////////
        private static int WM_PAINT = 0x000F;

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
                Rectangle backgroundRectangle = new Rectangle(0, 0, this.Width, this.Height);
                Rectangle dropDownRectangle = new Rectangle(this.Width - WIDTH_OF_DROPBUTTON, 0, WIDTH_OF_DROPBUTTON, this.Height);

                //graphics.FillRectangle(dropDownButtonBrush, backgroundRectangle);
                graphics.FillRectangle(dropDownButtonBrush, dropDownRectangle);

                // Draw arrows.
                DrawArrow(graphics);

                // Rectangles!
                Rectangle controlBounds = new Rectangle(0,0,this.Width,this.Height);

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
            PointF topRightCorner = new PointF(this.Width, 0);
            PointF bottomLeftCorner = new PointF(0, this.Height);
            PointF bottomRightCorner = new PointF(this.Width, this.Height);

            // Background Brush
            backgroundBrush = new SolidBrush(this.BackColor);
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
            PointF arrowTopLeft = new PointF(this.Width - 13, (this.Height - 5) / 2);
            PointF arrowTopRight = new PointF(this.Width - 6, (this.Height - 5) / 2);
            PointF arrowBottom = new PointF(this.Width - 9, (this.Height + 2) / 2);

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
            if (e.Index < 0) { return; }

            // Cast the ComboBox Sender
            ComboBox combo = sender as ComboBox;

            // If it's selected, draw the item with the highlight colour.
            if (e.State.HasFlag(DrawItemState.Selected))
            { e.Graphics.FillRectangle(new SolidBrush(HighlightColor), e.Bounds); }

            // Otherwise use the BG Colour.
            else
            { e.Graphics.FillRectangle(new SolidBrush(combo.BackColor), e.Bounds); }

            // Draw the original intended text.
            e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, new SolidBrush(combo.ForeColor), new Point(e.Bounds.X, e.Bounds.Y));
        }
    }
}
