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

using ReloadedLauncher.Styles.Controls.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls
{
    /// <summary>
    /// Modifies the default button class such that we may use our own rendering options for the text as well
    /// as reducing the internal text margins, preventing multi-line textboxes.
    /// </summary>
    public class EnhancedButton : Button, IControlIgnorable, IDecorationBox
    {
        /// <summary>
        /// Overrides the information needed when the control is created or accessed to
        /// either ignore input on the label or not ignore input.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (IgnoreMouse) { cp.Style |= 0x08000000; }  // Enable WS_DISABLED
                return cp;
            }
        }

        /// <summary>
        /// Defines the hinting style used for text rendering using GDI.
        /// </summary>
        public TextRenderingHint TextRenderingHint { get; set; }

        /// <summary>
        /// Defines the smoothing mode used for text rendering.
        /// </summary>
        public SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// If set to true, the control ignores the mouse.
        /// </summary>
        public bool IgnoreMouse { get; set; }

        /// <summary>
        /// Defines whether the button ignores mouse clicks.
        /// </summary>
        public bool IgnoreMouseClicks { get; set; }

        /// <summary>
        /// Declares whether the decoration box should capture the children controls 
        /// </summary>
        public bool CaptureChildren { get; set; }

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
            // Call the base method to draw us everything except text of the button.
            base.OnPaint(paintArguments);

            // Draw using GDI if there is anything to draw
            if (String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(customText))
            {
                // Set the Smoothing Mode for the Button
                paintArguments.Graphics.SmoothingMode = SmoothingMode;

                // Set the Text Rendering Mode for the Button 
                paintArguments.Graphics.TextRenderingHint = TextRenderingHint;

                // Create a new String Formatter and set the alignment of the string to our desired alignment.
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                // Paint our text
                paintArguments.Graphics.DrawString(customText, Font, new SolidBrush(ForeColor), ClientRectangle, stringFormat);
            }
        }

        // Redirects
        protected override void OnMouseDown(MouseEventArgs mevent) { if (!IgnoreMouseClicks) { base.OnMouseDown(mevent); } }
    }
}
