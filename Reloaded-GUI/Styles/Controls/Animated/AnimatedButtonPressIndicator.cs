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

using System;
using System.Drawing;
using System.Windows.Forms;
using Reloaded_GUI.Styles.Animation;
using Reloaded_GUI.Styles.Controls.Custom;

namespace Reloaded_GUI.Styles.Controls.Animated
{
    /// <summary>
    /// Completely custom class providing a button which swaps its 
    /// back and forecolor depending on whether a flag is set.
    /// The intended use is showing whether the user has pressed a controller button.
    /// </summary>
    public class AnimatedButtonPressIndicator : ButtonPressIndicator, IAnimatedControl
    {
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedButtonPressIndicator()
        {
            // Instantiate all of the animation messages.
            AnimProperties = new AnimProperties();
            AnimProperties.ForeColorMessage = new AnimMessage(this);
            AnimProperties.BackColorMessage = new AnimMessage(this);

            // Setup click event.
            this.MouseClick += OnMouseClick;
        }

        /// <summary>
        /// Overrides the text used on the checkbuttons as the button is toggled or untoggled.
        /// </summary>
        public new bool ButtonEnabled
        {
            get => base.ButtonEnabled;
		    set
		    {
                base.ButtonEnabled = value;

		        // Enable/Disable Animation
		        if (ButtonEnabled) { AnimDispatcher.AnimateMouseEnter(null, this, AnimProperties); }
		        else { AnimDispatcher.AnimateMouseLeave(null, this, AnimProperties); }

				// Set new enabled/disabled text
                Text = ButtonEnabled ? "+" : "-";
            }
        }

		/// <summary>
        /// Manually toggles the button press indicator on mouse click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouseEventArgs"></param>
        private void OnMouseClick(object sender, MouseEventArgs mouseEventArgs)
		{
		    // Invert current status.
            ButtonEnabled = !ButtonEnabled;
        }

        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Stops ongoing animations.
        /// </summary>
        public void KillAnimations()
        {
            AnimProperties.BackColorMessage.PlayAnimation = false;
            AnimProperties.ForeColorMessage.PlayAnimation = false;
        }

        public void OnMouseEnterWrapper(EventArgs e) { OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { OnMouseEnter(e); }

        //////////////////////////////////////////////////////////////
        // Our own graphics job, for when to draw the control contents
        //////////////////////////////////////////////////////////////

        /// <summary>
        /// Fills the vertical progress bar with the current level of progression.
        /// </summary>
        /// <param name="graphics">The GDI+ graphics object to use for painting.</param>
        protected override void PaintButton(Graphics graphics)
        {
            // Set brush to draw the background.
            BackgroundBrush = new SolidBrush(BackColor);

            // Region of the background.
            Rectangle backgroundRegion = new Rectangle
            (
                LeftBorderWidth,
                TopBorderWidth,
                Width - LeftBorderWidth - RightBorderWidth,
                Height - TopBorderWidth - BottomBorderWidth
            );

            // Paint the background.
            graphics.FillRectangle(BackgroundBrush, backgroundRegion);

            // Obtain the control borders.
            Rectangle controlBounds = new Rectangle(0, 0, Width, Height);

            // Set brush to draw the text.
            TextBrush = new SolidBrush(ForeColor);

            // Draw the text.
            graphics.DrawString(Text, Font, TextBrush, controlBounds, StringFormat);

            // Cleanup
            TextBrush.Dispose();
            BackgroundBrush.Dispose();
        }

        // //////////////////////////////////
        // Sync Border with Foreground Colour
        // //////////////////////////////////

        /// <summary>
        /// Synchronizes the border colour with the colour of the text.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            // Change the border colour.
            BottomBorderColour = ForeColor;
            LeftBorderColour = ForeColor;
            RightBorderColour = ForeColor;
            TopBorderColour = ForeColor;

            // Change the BackColor.
            OnBackColorChanged(e);
        }
    }
}
