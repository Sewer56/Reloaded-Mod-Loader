using ReloadedLauncher.Styles.Controls.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls.Enhanced
{
    /// <summary>
    /// Enhanced TextBox class providing support for painting custom borders.
    /// </summary>
    public class EnhancedRichTextbox : RichTextBox, IBorderedControl
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

        /// <summary>
        /// Constructor for the enhanced textbox.
        /// </summary>
        public EnhancedRichTextbox()
        {
            // Set control style.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
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
