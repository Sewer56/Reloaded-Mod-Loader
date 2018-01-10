using HeroesModLoaderConfig.Styles.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HeroesModLoaderConfig.Styles.Animation.AnimOverrides;

namespace HeroesModLoaderConfig.Styles.Controls.Animated
{
    /// <summary>
    /// Provides the animation implementation for EnhancedButton.
    /// </summary>
    class AnimatedButton : EnhancedButton, IAnimatedControl
    {
        #region Mouse Enter Animation Properties
        /// <summary>
        /// Defines what kind of animation should the button perform when the mouse
        /// enters over the button.
        /// </summary>
        [Category("Mouse Enter Animation"), Description("[Flags] Specifies the animations to be played when the mouse enters the control.")]
        public MouseEnterOverride MouseEnterOverride { get; set; }

        /// <summary>
        /// Specifies the duration dictating how long the animation is supposed to last.
        /// </summary>
        [Category("Mouse Enter Animation"), Description("Specifies the duration dictating how long the animation is supposed to last.")]
        public float MouseEnterDuration { get; set; }

        /// <summary>
        /// Specifies the amount of frames per second the animation is intended to be played at.
        /// </summary>
        [Category("Mouse Enter Animation"), Description("Specifies the amount of frames per second the animation is intended to be played at.")]
        public float MouseEnterFramerate { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Enter Animation"), Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.")]
        public Color MouseEnterBackColor { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Enter Animation"), Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.")]
        public Color MouseEnterForeColor { get; set; }
        #endregion

        #region Mouse Leave Animation Properties
        /// <summary>
        /// Defines what kind of animation should the button perform when the mouse
        /// leaves the button.
        /// </summary>
        [Category("Mouse Leave Animation"), Description("[Flags] Specifies the animations to be played when the mouse leaves the control.")]
        public MouseLeaveOverride MouseLeaveOverride { get; set; }

        /// <summary>
        /// Specifies the duration dictating how long the animation is supposed to last.
        /// </summary>
        [Category("Mouse Leave Animation"), Description("Specifies the duration dictating how long the animation is supposed to last.")]
        public float MouseLeaveDuration { get; set; }

        /// <summary>
        /// Specifies the amount of frames per second the animation is intended to be played at.
        /// </summary>
        [Category("Mouse Leave Animation"), Description("Specifies the amount of frames per second the animation is intended to be played at.")]
        public float MouseLeaveFramerate { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Leave Animation"), Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse leave override is set.")]
        public Color MouseLeaveBackColor { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Leave Animation"), Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse leave override is set.")]
        public Color MouseLeaveForeColor { get; set; }
        #endregion

        #region Property Animation Flags
        /// <summary>
        /// Set property PlayAnimation to false to terminate all current animations that affect the
        /// BackColor of the control.
        /// </summary>
        private AnimMessage BackColorMessage { get; set; }

        /// <summary>
        /// Set property PlayAnimation to false to terminate all current animations that affect the
        /// ForeColor of the control.
        /// </summary>
        private AnimMessage ForeColorMessage { get; set; }
        #endregion 

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedButton()
        {
            // Instantiate all of the animation messages.
            this.BackColorMessage = new AnimMessage(this);
            this.ForeColorMessage = new AnimMessage(this);
        }

        /// <summary>
        /// Animates the button when it is in focus of the mouse cursor.
        /// </summary>
        public void AnimateMouseEnter(EventArgs e)
        {
            // If the override is none.
            if (MouseEnterOverride == MouseEnterOverride.None) { base.OnMouseEnter(e); return; }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                BackColorMessage = new AnimMessage(this);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(BackColorMessage, new AnimInterpolator(MouseEnterDuration, MouseEnterFramerate), MouseEnterBackColor));
            }

            if (MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                ForeColorMessage = new AnimMessage(this);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(ForeColorMessage, new AnimInterpolator(MouseEnterDuration, MouseEnterFramerate), MouseEnterForeColor));
            }
        }

        /// <summary>
        /// Animates the button when it is out of focus of the mouse cursor.
        /// </summary>
        public void AnimateMouseLeave(EventArgs e)
        {
            // If the override is none.
            if (MouseLeaveOverride == MouseLeaveOverride.None) { base.OnMouseLeave(e); return; }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                BackColorMessage = new AnimMessage(this);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(BackColorMessage, new AnimInterpolator(MouseLeaveDuration, MouseLeaveFramerate), MouseLeaveBackColor));
            }

            if (MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                ForeColorMessage = new AnimMessage(this);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(ForeColorMessage, new AnimInterpolator(MouseLeaveDuration, MouseLeaveFramerate), MouseLeaveForeColor));
            }
        }

        // //////////////////////////
        // Common Animation Redirects
        // //////////////////////////
        protected override void OnMouseEnter(EventArgs e) { AnimateMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { AnimateMouseLeave(e); }
    }
}
