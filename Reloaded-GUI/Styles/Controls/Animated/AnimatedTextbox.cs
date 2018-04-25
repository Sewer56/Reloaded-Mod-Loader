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
using Reloaded_GUI.Styles.Animation;
using Reloaded_GUI.Styles.Controls.Enhanced;

namespace Reloaded_GUI.Styles.Controls.Animated
{
    /// <summary>
    /// Provides the animation implementation for EnhancedButton.
    /// </summary>
    public class AnimatedTextbox : EnhancedTextbox, IAnimatedControl
    {
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedTextbox()
        {
            // Instantiate all of the animation messages.
            AnimProperties = new AnimProperties();
            AnimProperties.ForeColorMessage = new AnimMessage(this);
            AnimProperties.BackColorMessage = new AnimMessage(this);
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

        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }

        // ////////////////////////////
        // Sync Border with Text Colour
        // ////////////////////////////

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

        /// <summary>
        /// If the user loses focus of the textbox, the textbox
        /// colour should revert alongside the text colour to the original
        /// gray (or theme dependent) colour.
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            // Run base.
            base.OnLostFocus(e);

            // Cancel animation.
            AnimDispatcher.AnimateMouseLeave(e, this, AnimProperties);
        }

        /// <summary>
        /// If the user gains focus of the textbox, the textbox
        /// colour should play the mouse enter animation.
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            // Run base.
            base.OnGotFocus(e);

            // Start animation.
            OnMouseEnter(e);
        }

        // //////////////////////////
        // Common Animation Redirects
        // //////////////////////////
        protected override void OnMouseEnter(EventArgs e) { AnimDispatcher.AnimateMouseEnter(e, this, AnimProperties); }

        protected override void OnMouseLeave(EventArgs e)
        {
            // "Mouse" doesn't leave if textbox is currently selected/user is typing.
            if (! Focused) AnimDispatcher.AnimateMouseLeave(e, this, AnimProperties);
        }
    }
}
