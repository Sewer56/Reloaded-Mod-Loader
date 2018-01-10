using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls
{
    class EnhancedLabel : Label
    {
        /// <summary>
        /// Defines the hinting style used for text rendering using GDI.
        /// </summary>
        public TextRenderingHint TextRenderingHint { get; set; }

        /// <summary>
        /// Defines the smoothing mode used for text rendering.
        /// </summary>
        public SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// Define the rendering hint upon instantiation.
        /// </summary>
        public EnhancedLabel() : base()
        {
            TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            SmoothingMode = SmoothingMode.HighQuality;
        }

        /// <summary>
        /// Overrides the default paint event arguments for the paint event of the label.
        /// Changes the paint event arguments to our own.
        /// </summary>
        /// <param name="paintEventArguments"></param>
        protected override void OnPaint(PaintEventArgs paintEventArguments)
        {
            // Modify Hinting Style
            paintEventArguments.Graphics.TextRenderingHint = TextRenderingHint;

            // Draw the label as originally intended.
            base.OnPaint(paintEventArguments);
        }
        
    }
}
