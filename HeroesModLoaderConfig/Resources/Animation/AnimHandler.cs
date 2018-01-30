using System;
using System.Drawing;
using System.Threading.Tasks;
using static HeroesModLoaderConfig.Styles.Animation.AnimOverrides;

namespace HeroesModLoaderConfig.Styles.Animation
{
    public static class AnimHandler
    {
        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which to start the interpolation.</param>
        public static void AnimateMouseEnter(EventArgs e, object Control, AnimProperties AnimProperties)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return;
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), AnimProperties.MouseEnterBackColor));
            }

            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(AnimProperties.ForeColorMessage, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), AnimProperties.MouseEnterForeColor));
            }
        }

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse loses focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static void AnimateMouseLeave(EventArgs e, object Control, AnimProperties AnimProperties)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return;
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveBackColor));
            }

            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(AnimProperties.ForeColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveForeColor));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which to start the interpolation.</param>
        public static void AnimateMouseEnter(EventArgs e, object Control, AnimProperties AnimProperties, Color sourceColor)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return;
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), sourceColor, AnimProperties.MouseEnterBackColor));
            }

            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(AnimProperties.ForeColorMessage, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), sourceColor, AnimProperties.MouseEnterForeColor));
            }
        }

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse loses focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static void AnimateMouseLeave(EventArgs e, object Control, AnimProperties AnimProperties, Color sourceColor)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return;
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), sourceColor, AnimProperties.MouseLeaveBackColor));
            }

            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(AnimProperties.ForeColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), sourceColor, AnimProperties.MouseLeaveForeColor));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which to start the interpolation.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseEnter(EventArgs e, object Control, AnimProperties AnimProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground, Color sourceColor)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                animMessageBackground.PlayAnimation = false;

                // Create new message for new control.
                animMessageBackground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), sourceColor, AnimProperties.MouseEnterBackColor));
            }

            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                animMessageForeground.PlayAnimation = false;

                // Replace old message with new.
                animMessageForeground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), sourceColor, AnimProperties.MouseEnterForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse loses focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseLeave(EventArgs e, object Control, AnimProperties AnimProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground, Color sourceColor)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                animMessageBackground.PlayAnimation = false;

                // Create new message for new control.
                animMessageBackground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), sourceColor, AnimProperties.MouseLeaveBackColor));
            }

            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                animMessageForeground.PlayAnimation = false;

                // Replace old message with new.
                animMessageForeground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), sourceColor, AnimProperties.MouseLeaveForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }

        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which to start the interpolation.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseEnter(EventArgs e, object Control, AnimProperties AnimProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                animMessageBackground.PlayAnimation = false;

                // Create new message for new control.
                animMessageBackground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), AnimProperties.MouseEnterBackColor));
            }

            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                animMessageForeground.PlayAnimation = false;

                // Replace old message with new.
                animMessageForeground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), AnimProperties.MouseEnterForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse loses focus of the control.
        /// </summary>
        /// <param name="AnimProperties">The animation properties for this object.</param>
        /// <param name="Control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseLeave(EventArgs e, object Control, AnimProperties AnimProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None)
            {
                if (Control is IAnimatedControl)
                {
                    IAnimatedControl animatedControl = (IAnimatedControl)Control;
                    animatedControl.OnMouseEnterWrapper(e);
                }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                animMessageBackground.PlayAnimation = false;

                // Create new message for new control.
                animMessageBackground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveBackColor));
            }

            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                animMessageForeground.PlayAnimation = false;

                // Replace old message with new.
                animMessageForeground = new AnimMessage(Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }
    }
}
