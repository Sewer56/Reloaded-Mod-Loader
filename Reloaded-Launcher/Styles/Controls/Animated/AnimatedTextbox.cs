using ReloadedLauncher.Styles.Animation;
using ReloadedLauncher.Styles.Controls.Enhanced;
using ReloadedLauncher.Styles.Controls.Interfaces;
using System;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Controls.Animated
{
    /// <summary>
    /// Provides the animation implementation for EnhancedButton.
    /// </summary>
    public class AnimatedTextbox : EnhancedTextbox, IAnimatedControl
    {
        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedTextbox()
        {
            // Instantiate all of the animation messages.
            this.AnimProperties = new AnimProperties();
            this.AnimProperties.ForeColorMessage = new AnimMessage(this);
            this.AnimProperties.BackColorMessage = new AnimMessage(this);
        }

        /// <summary>
        /// Stops ongoing animations.
        /// </summary>
        public void KillAnimations()
        {
            this.AnimProperties.BackColorMessage.PlayAnimation = false;
            this.AnimProperties.ForeColorMessage.PlayAnimation = false;
        }

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
            this.BottomBorderColour = this.ForeColor;
            this.LeftBorderColour = this.ForeColor;
            this.RightBorderColour = this.ForeColor;
            this.TopBorderColour = this.ForeColor;

            // Change the BackColor.
            base.OnBackColorChanged(e);
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
            AnimHandler.AnimateMouseLeave(e, this, AnimProperties);
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
        protected override void OnMouseEnter(EventArgs e) { AnimHandler.AnimateMouseEnter(e, this, AnimProperties); }
        protected override void OnMouseLeave(EventArgs e)
        {
            // "Mouse" doesn't leave if textbox is currently selected/user is typing.
            if (! Focused) { AnimHandler.AnimateMouseLeave(e, this, AnimProperties); }
        }

        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }
    }
}
