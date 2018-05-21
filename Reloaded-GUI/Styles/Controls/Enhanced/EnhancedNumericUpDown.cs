using System.Drawing;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls.Enhanced
{
    class EnhancedNumericUpDown : NumericUpDown
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
        public int LeftWidth { get; set; }
        public int RightWidth { get; set; }
        public int TopWidth { get; set; }
        public int BottomWidth { get; set; }

        // Border Brushes.
        private Brush DropButtonBrush { get; set; }
        private Color _ButtonColor { get; set; }

        // Paint window message.
        const int WM_PAINT = 0x000F;

        /// <summary>
        /// Allow the setting of button colour.
        /// </summary>
        public Color ButtonColor
        {
            get { return _ButtonColor; }
            set
            {
                _ButtonColor = value;
                DropButtonBrush = new SolidBrush(this._ButtonColor);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public EnhancedNumericUpDown() : base()
        {
            // The brush for the button.
            DropButtonBrush = new SolidBrush(SystemColors.Control);

            // Set default button colour.
            ButtonColor = SystemColors.Control;

            // Enable double buffering.
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Override the window message handler to draw ourselves.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // Call original window message handler.
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                // Graphics object.
                Graphics graphics = Graphics.FromHwnd(Handle);

                // Do the button first!
                //Rectangle ButtonControl = new Rectangle(this.Width - 16, 0, 16, this.Height);
                //g.FillRectangle(DropButtonBrush, ButtonControl);

                // Rectangles!
                Rectangle ControlBoundaries = new Rectangle(0, 0, Width, Height - 2);

                // Draw the border!
                ControlPaint.DrawBorder
                (
                    graphics, 
                    ControlBoundaries, 
                    LeftBorderColour, LeftWidth, LeftBorderStyle, 

                    TopBorderColour, TopWidth, TopBorderStyle, 

                    RightBorderColour, RightWidth, RightBorderStyle, 

                    BottomBorderColour, BottomWidth, BottomBorderStyle
                );

                // Bye bye graphics!
                graphics.Dispose();
            }
        }
    }
}
