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
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls
{
    /// <summary>
    /// Modifies the default label class to provide customized rendering hint and smoothing mode customization and
    /// features such as ignoring of the mouse, etc.
    /// </summary>
    class EnhancedLabel : Label, IControlIgnorable
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
        [Category("| Custom Options"), Description("Defines the hinting style used for text rendering using GDI.")]
        public TextRenderingHint TextRenderingHint { get; set; }

        /// <summary>
        /// Defines the smoothing mode used for text rendering.
        /// </summary>
        [Category("| Custom Options"), Description("Defines the smoothing mode used for text rendering.")]
        public SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// If set to true, the control ignores the mouse.
        /// </summary>
        [Category("| Custom Options"), Description("If set to true, the control ignores the mouse. This is useful if you don't want to lose focus in underlying controls/items. It also seems to make the designer ignore the mouse for the control, bear that in mind.")]
        public bool IgnoreMouse { get; set; }

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
            // Modify Hinting & Smoothing Style
            paintEventArguments.Graphics.TextRenderingHint = TextRenderingHint;
            paintEventArguments.Graphics.SmoothingMode = SmoothingMode;

            // Draw the label as originally intended.
            base.OnPaint(paintEventArguments);
        }
    }
}
