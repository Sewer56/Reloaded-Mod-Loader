using ReloadedLauncher.Styles.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls.Custom
{
    /// <summary>
    /// Completely custom class providing a analog stick position indicator implementation.
    /// The implementation is intended to show controller analog stick inputs, thus no smoothing
    /// or animation functions are implemented.
    /// The stick check bar is a thin implementation, not providing much input checking, etc.
    /// </summary>
    public class CustomAnalogStickIndicator : Control, IBorderedControl
    {
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

        // Stores progress and details of position indicator.
        // Note: Width of indicator equals the border width.
        public int IndicatorWidth { get; set; } // Defines the width of the dot.
        public Color IndicatorColour { get; set; } // Defines the colour of the dot.
        public int MAX_VALUE = 1000;
        public int MIN_VALUE = 0;
        public int valueX;
        public int valueY;


        /// <summary>
        /// Gets or sets the X value of the position indicator, from MIN_VALUE (0) to MAX_VALUE (1000)
        /// </summary>
        [Description("Specifies the X value of the analog stick indicator, from MIN_VALUE (0) to MAX_VALUE (1000)")]
        public int ValueX
        {
            get { return valueX; }
            set { valueX = value; Invalidate(); }
        }

        /// <summary>
        /// Gets or sets the Y value of the position indicator, from MIN_VALUE (0) to MAX_VALUE (1000)
        /// </summary>
        [Description("Specifies the Y value of the analog stick indicator, from MIN_VALUE (0) to MAX_VALUE (1000)")]
        public int ValueY
        {
            get { return valueY; }
            set { valueY = value; Invalidate(); }
        }

        /// <summary>
        /// Sets the new X and Y value simultaneously, reducing drawing overhead per each change.
        /// </summary>
        /// <param name="positionX">The new X position of the indicator (from MIN_VALUE to MAX_VALUE).</param>
        /// <param name="positionY">The new Y position of the indicator (from MIN_VALUE to MAX_VALUE).</param>
        public void SetXYValue(int positionX, int positionY)
        {
            valueX = positionX;
            valueY = positionY;
            Invalidate();
        }

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public CustomAnalogStickIndicator()
        {
            // Redirect all painting to us.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Setup double buffering.
            graphicManager = BufferedGraphicsManager.Current;
            ReallocateBuffer();
        }

        /////////////////////////////////
        // Implement own Double Buffering
        /////////////////////////////////
        BufferedGraphicsContext graphicManager;
        BufferedGraphics backBuffer;

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
            graphicManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            backBuffer = graphicManager.Allocate(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height));
        }

        ////////////////////////////////////////////////////////////////////////
        // Override the paint event sent to the control, draw our own control :V
        ////////////////////////////////////////////////////////////////////////
        private static int WM_PAINT = 0x000F;

        /// <summary>
        /// Override the window message handler.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Call base
            base.WndProc(ref m);

            // If it's a paint call, draw our own control.
            if (m.Msg == WM_PAINT) { DrawControl(); }
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
            PaintBackground(backBuffer.Graphics);

            // Paint the border.
            PaintBorders(backBuffer.Graphics);

            // Paints the indicator position showing current analog stick inputs.
            PaintIndicator(backBuffer.Graphics);

            // Render the backbuffer to the control's GFX object.
            backBuffer.Render(Graphics.FromHwnd(Handle));
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintBackground(Graphics graphics)
        {
            // Define and paint the background area.
            Brush brush = new SolidBrush(this.BackColor);
            Rectangle controlBounds = new Rectangle(0, 0, this.Width, this.Height);

            // Draw
            graphics.FillRectangle(brush, controlBounds);

            // Cleanup
            brush.Dispose();
        }
        
        /// <summary>
        /// Paints our own border around the current textbox control.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintBorders(Graphics graphics)
        {
            // Obtain the control borders.
            Rectangle controlBounds = new Rectangle(0, 0, this.Width, this.Height);

            // Draw the border!
            ControlPaint.DrawBorder(graphics, controlBounds, LeftBorderColour,
                LeftBorderWidth, LeftBorderStyle, TopBorderColour, TopBorderWidth, TopBorderStyle, RightBorderColour,
                RightBorderWidth, RightBorderStyle, BottomBorderColour, BottomBorderWidth, BottomBorderStyle);
        }

        /// <summary>
        /// Paints the indicator for the analog stick indicator.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintIndicator(Graphics graphics)
        {
            // Convert current value into height percentage.
            float positionHorizontal = valueX / (float)MAX_VALUE;
            float positionVertical = valueY / (float)MAX_VALUE;

            // Height, Width minus borders
            int borderlessHeight = this.Height - BottomBorderWidth - TopBorderWidth;
            int borderlessWidth = this.Width - LeftBorderWidth - RightBorderWidth;

            // Get left border of rectangle.
            int leftRectangle = (int)(borderlessWidth * positionHorizontal);
            int topRectangle = (int)(borderlessHeight * positionVertical);

            // The left and top measurements currently disregard border width.
            // We should add the size of the top and left borders which the calculations do not consider.
            leftRectangle += LeftBorderWidth;
            topRectangle += TopBorderWidth;

            // Now just offset by 1/2 the width and height of rectangle.
            leftRectangle -= IndicatorWidth / 2;
            topRectangle -= IndicatorWidth / 2;

            // Define and paint the rectangle.
            Brush brush = new SolidBrush(this.IndicatorColour);
            Rectangle rectangle = new Rectangle(leftRectangle, topRectangle, IndicatorWidth, IndicatorWidth);

            // Draw
            graphics.FillRectangle(brush, rectangle);

            // Cleanup
            brush.Dispose();
        }
    }
}
