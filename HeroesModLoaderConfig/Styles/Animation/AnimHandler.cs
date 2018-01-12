using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using static HeroesModLoaderConfig.Styles.Animation.AnimOverrides;
using System.Drawing;

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
        public static void AnimateMouseEnter(EventArgs e, IAnimatedControl Control, AnimProperties AnimProperties)
        {
            // If the override is none.
            if (AnimProperties.MouseEnterOverride == MouseEnterOverride.None) { Control.OnMouseEnterWrapper(e); return; }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage((Control)Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseEnterDuration, AnimProperties.MouseEnterFramerate), AnimProperties.MouseEnterBackColor));
            }

            if (AnimProperties.MouseEnterOverride.HasFlag(MouseEnterOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage((Control)Control);

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
        public static void AnimateMouseLeave(EventArgs e, IAnimatedControl Control, AnimProperties AnimProperties)
        {
            // If the override is none.
            if (AnimProperties.MouseLeaveOverride == MouseLeaveOverride.None) { Control.OnMouseEnterWrapper(e); return; }

            // Else if the BackColor or ForeColor are to be interpolated.
            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.BackColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.BackColorMessage.PlayAnimation = false;

                // Create new message for new control.
                AnimProperties.BackColorMessage = new AnimMessage((Control)Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateBackcolor(AnimProperties.BackColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveBackColor));
            }

            if (AnimProperties.MouseLeaveOverride.HasFlag(MouseLeaveOverride.ForeColorInterpolate))
            {
                // Cancel old message.
                AnimProperties.ForeColorMessage.PlayAnimation = false;

                // Replace old message with new.
                AnimProperties.ForeColorMessage = new AnimMessage((Control)Control);

                // Interpolate.
                Task.Run(() => AnimAnimations.InterpolateForecolor(AnimProperties.ForeColorMessage, new AnimInterpolator(AnimProperties.MouseLeaveDuration, AnimProperties.MouseLeaveFramerate), AnimProperties.MouseLeaveForeColor));
            }
        }
    }
}
