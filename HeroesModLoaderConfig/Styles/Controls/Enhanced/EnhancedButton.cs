using HeroesModLoaderConfig.Styles.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HeroesModLoaderConfig.Styles.Animation.AnimOverrides;

namespace HeroesModLoaderConfig.Styles.Controls
{
    /// <summary>
    /// Modifies the default button class such that we may use our own rendering options for the text as well
    /// as reducing the internal text margins, preventing multi-line textboxes.
    /// </summary>
    public class EnhancedButton : Button
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
        /// Redirects the text property to use our own, that is such that the
        /// base method used for painting the control.
        /// </summary>
        public override string Text
        {
            get { return customText; }
            set { customText = value; Invalidate(); }
        }

        /// <summary>
        /// Defines our own custom text object that is to be used for drawing.
        /// </summary>
        private string customText;

        /// <summary>
        /// Define the rendering hint upon instantiation.
        /// </summary>
        public EnhancedButton() : base()
        {
            TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            SmoothingMode = SmoothingMode.HighQuality;
        }

        /// <summary>
        /// The default painting event for the button.
        /// </summary>
        /// <param name="paintArguments"></param>
        protected override void OnPaint(PaintEventArgs paintArguments)
        {
            // Set the Smoothing Mode for the Button
            paintArguments.Graphics.SmoothingMode = SmoothingMode;

            // Set the Text Rendering Mode for the Button 
            paintArguments.Graphics.TextRenderingHint = TextRenderingHint;

            // Call the base method to draw us everything except text of the button.
            base.OnPaint(paintArguments);

            // Create a new String Formatter and set the alignment of the string to our desired alignment.
            StringFormat stringFormat = new StringFormat(); 
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            // Draw using GDI if there is anything to draw
            if (String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(customText))
            { paintArguments.Graphics.DrawString(customText, Font, new SolidBrush(ForeColor), ClientRectangle, stringFormat); }
        }
    }
}
