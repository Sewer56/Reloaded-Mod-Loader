using HeroesModLoaderConfig.Styles.Animation;
using HeroesModLoaderConfig.Styles.Controls.Interfaces;
using System;

namespace HeroesModLoaderConfig.Styles.Controls.Animated
{
    /// <summary>
    /// Provides the animation implementation for EnhancedButton.
    /// </summary>
    public class AnimatedButton : EnhancedButton, IAnimatedControl, IDecorationBox
    {
        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedButton()
        {
            // Instantiate all of the animation messages.
            this.AnimProperties = new AnimProperties();
            this.AnimProperties.ForeColorMessage = new AnimMessage(this);
            this.AnimProperties.BackColorMessage = new AnimMessage(this);
        }

        // //////////////////////////
        // Common Animation Redirects
        // //////////////////////////
        protected override void OnMouseEnter(EventArgs e) { AnimHandler.AnimateMouseEnter(e, this, AnimProperties); }
        protected override void OnMouseLeave(EventArgs e) { AnimHandler.AnimateMouseLeave(e, this, AnimProperties); }
        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }
    }
}
