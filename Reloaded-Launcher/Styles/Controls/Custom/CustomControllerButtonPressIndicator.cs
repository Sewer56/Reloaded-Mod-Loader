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
    /// Completely custom class providing a button which swaps its 
    /// back and forecolor depending on whether a flag is set.
    /// The intended use is showing whether the user has pressed a controller button.
    /// </summary>
    public class CustomControllerButtonPressIndicator : Control, IBorderedControl
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

        // Is button enabled?
        private bool buttonEnabled;

        // Painting brushes
        Brush backgroundBrush;
        Brush textBrush;

        /// <summary>
        /// Gets or sets the text to be drawn inside the button.
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; this.Invoke((Action)delegate { Refresh(); }); }
        }

        /// <summary>
        /// Set to true to swap the background and foreground colour.
        /// </summary>
        public bool ButtonEnabled
        {
            get { return buttonEnabled; }
            set
            {
                if (buttonEnabled != value)
                {
                    buttonEnabled = value;
                    this.Invoke((Action)delegate { Refresh(); });
                }
            }
        }

        /// <summary>
        /// Defines the format of the button text to be displayed.
        /// </summary>
        public StringFormat StringFormat { get; set; }

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public CustomControllerButtonPressIndicator()
        {
            // Redirect all painting to us.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Setup default text format.
            StringFormat = new StringFormat();
            StringFormat.LineAlignment = StringAlignment.Center;
            StringFormat.Alignment = StringAlignment.Center;

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

            // Fills the vertical progress bar with the appropriate value.
            PaintButton(backBuffer.Graphics);

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
        /// Paints our own border around the current control.
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
        /// Fills the vertical progress bar with the current level of progression.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected void PaintButton(Graphics graphics)
        {
            // Set brush to draw the background.
            if (buttonEnabled) { backgroundBrush = new SolidBrush(this.ForeColor); }
            else { backgroundBrush = new SolidBrush(this.BackColor); }

            // Region of the background.
            Rectangle backgroundRegion = new Rectangle
            (
                this.LeftBorderWidth,
                this.TopBorderWidth,
                this.Width - LeftBorderWidth - RightBorderWidth,
                this.Height - TopBorderWidth - BottomBorderWidth
            );

            // Paint the background.
            graphics.FillRectangle(backgroundBrush, backgroundRegion);

            // Obtain the control borders.
            Rectangle controlBounds = new Rectangle(0, 0, this.Width, this.Height);

            // Set brush to draw the text.
            if (buttonEnabled) { textBrush = new SolidBrush(this.BackColor); }
            else { textBrush = new SolidBrush(this.ForeColor); }

            // Draw the text.
            graphics.DrawString(this.Text, this.Font, textBrush, controlBounds, StringFormat);

            // Cleanup
            textBrush.Dispose();
            backgroundBrush.Dispose();
        }
        
    }
}
