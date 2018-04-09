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
using System.Threading.Tasks;

namespace Reloaded_GUI.Styles.Animation
{
    /// <summary>
    /// The <see cref="AnimDispatcher"/> class is responsible for sending out various Mouse Leave/Enter colour interpolation
    /// requests to be handled in separate background threads or tasks that happen in parallel.
    /// 
    /// Animations consist of <see cref="AnimProperties"/> classes which contain the information on the
    /// target colours for the mouse fade-in and fade out effects (both foreground and background) as well as 
    /// other details for the <see cref="AnimInterpolator"/> such as framerate and duration.
    /// 
    /// Each <see cref="AnimProperties"/> object also has instances of <see cref="AnimMessage"/> for backcolor
    /// and forecolor interpolation. The purpose of those are simply to be able to interactively immediately
    /// terminate ongoing background animations for new animations to be played (i.e. mouse enters button during 
    /// another animation, other animation is instantly halted).
    /// 
    /// For controls which have sub-controls such as datagridview rows, overloads are provided for explicitly
    /// passing indivual stored AnimMessages for each sub control. How they are managed and stored is up to the user.
    /// </summary>
    public static class AnimDispatcher
    {
        #region Back/Forecolor Fade Effect: Animation Properties Overloads Only
        /// <summary>
        /// Plays the mouse enter animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static void AnimateMouseEnter(EventArgs e, object control, AnimProperties animProperties)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return;
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively (set to false if true).
            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.BackColorInterpolate))
            {
                animProperties.BackColorMessage.PlayAnimation = false;
                animProperties.BackColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animProperties.BackColorMessage, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), animProperties.MouseEnterBackColor));
            }

            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.ForeColorInterpolate))
            {
                animProperties.ForeColorMessage.PlayAnimation = false;
                animProperties.ForeColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animProperties.ForeColorMessage, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), animProperties.MouseEnterForeColor));
            }
        }

        /// <summary>
        /// Plays the mouse leave animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        public static void AnimateMouseLeave(EventArgs e, object control, AnimProperties animProperties)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return;
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.BackColorInterpolate))
            {
                animProperties.BackColorMessage.PlayAnimation = false;
                animProperties.BackColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animProperties.BackColorMessage, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), animProperties.MouseLeaveBackColor));
            }

            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.ForeColorInterpolate))
            {
                animProperties.ForeColorMessage.PlayAnimation = false;
                animProperties.ForeColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animProperties.ForeColorMessage, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), animProperties.MouseLeaveForeColor));
            }
        }
        #endregion Back/Forecolor Fade Effect: Animation Properties Overloads Only

        #region Back/Forecolor Fade Effect: Animation Properties + Source Colour Overloads
        /// <summary>
        /// Plays the mouse enter animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which the mouse leave animation begins from (source colour).</param>
        public static void AnimateMouseEnter(EventArgs e, object control, AnimProperties animProperties, Color sourceColor)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return;
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.BackColorInterpolate))
            {
                animProperties.BackColorMessage.PlayAnimation = false;
                animProperties.BackColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animProperties.BackColorMessage, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), sourceColor, animProperties.MouseEnterBackColor));
            }

            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.ForeColorInterpolate))
            {
                animProperties.ForeColorMessage.PlayAnimation = false;
                animProperties.ForeColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animProperties.ForeColorMessage, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), sourceColor, animProperties.MouseEnterForeColor));
            }
        }

        /// <summary>
        /// Plays the mouse leave animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="sourceColor">The colour from which the mouse leave animation begins from (source colour).</param>
        public static void AnimateMouseLeave(EventArgs e, object control, AnimProperties animProperties, Color sourceColor)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return;
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.BackColorInterpolate))
            {
                animProperties.BackColorMessage.PlayAnimation = false;
                animProperties.BackColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animProperties.BackColorMessage, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), sourceColor, animProperties.MouseLeaveBackColor));
            }

            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.ForeColorInterpolate))
            {
                animProperties.ForeColorMessage.PlayAnimation = false;
                animProperties.ForeColorMessage = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animProperties.ForeColorMessage, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), sourceColor, animProperties.MouseLeaveForeColor));
            }
        }
        #endregion Back/Forecolor Fade Effect: Animation Properties + Source Colour Overloads

        #region Back/Forecolor Fade Effect: Animation Properties + Source Colour + Explicit AnimMessage Overloads
        /// <summary>
        /// Plays the mouse enter animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="animMessageBackground">The individual message for the background interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="animMessageForeground">The individual message for the foreground interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="sourceColor">The colour from which the mouse leave animation begins from (source colour).</param>
        public static (AnimMessage, AnimMessage) AnimateMouseEnter(EventArgs e, object control, AnimProperties animProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground, Color sourceColor)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.BackColorInterpolate))
            {
                animMessageBackground.PlayAnimation = false;
                animMessageBackground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), sourceColor, animProperties.MouseEnterBackColor));
            }

            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.ForeColorInterpolate))
            {
                animMessageForeground.PlayAnimation = false;
                animMessageForeground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), sourceColor, animProperties.MouseEnterForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }

        /// <summary>
        /// Plays the mouse leave animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="animMessageBackground">The individual message for the background interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="animMessageForeground">The individual message for the foreground interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="sourceColor">The colour from which the mouse leave animation begins from (source colour).</param>
        public static (AnimMessage, AnimMessage) AnimateMouseLeave(EventArgs e, object control, AnimProperties animProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground, Color sourceColor)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.BackColorInterpolate))
            {
                animMessageBackground.PlayAnimation = false;
                animMessageBackground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), sourceColor, animProperties.MouseLeaveBackColor));
            }

            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.ForeColorInterpolate))
            {
                animMessageForeground.PlayAnimation = false;
                animMessageForeground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), sourceColor, animProperties.MouseLeaveForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }
        #endregion Back/Forecolor Fade Effect: Animation Properties + Source Colour + Explicit AnimMessage Overloads

        #region Back/Forecolor Fade Effect: Animation Properties + Explicit AnimMessage Overloads
        /// <summary>
        /// Plays the mouse enter animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="animMessageBackground">The individual message for the background interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="animMessageForeground">The individual message for the foreground interpolation action used to cancel ongoing animations if necessary.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseEnter(EventArgs e, object control, AnimProperties animProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.BackColorInterpolate))
            {
                animMessageBackground.PlayAnimation = false;
                animMessageBackground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), animProperties.MouseEnterBackColor));
            }

            if (animProperties.MouseEnterOverride.HasFlag(AnimOverrides.MouseEnterOverride.ForeColorInterpolate))
            {
                animMessageForeground.PlayAnimation = false;
                animMessageForeground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(animProperties.MouseEnterDuration, animProperties.MouseEnterFramerate), animProperties.MouseEnterForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }

        /// <summary>
        /// Plays the mouse leave animation to change the foreground and background colour
        /// of a windows forms control (or any object with fields BackColor and ForeColor).
        /// </summary>
        /// <param name="animProperties">The animation properties for this object.</param>
        /// <param name="control">The object itself (this)</param>
        /// <param name="e">From the regular event.</param>
        /// <param name="animMessageBackground">The individual message for the background interpolation action used to cancel ongoing animations if necessary.</param>
        /// <param name="animMessageForeground">The individual message for the foreground interpolation action used to cancel ongoing animations if necessary.</param>
        public static (AnimMessage, AnimMessage) AnimateMouseLeave(EventArgs e, object control, AnimProperties animProperties, AnimMessage animMessageBackground, AnimMessage animMessageForeground)
        {
            // If the override is none.
            if (animProperties.MouseEnterOverride == AnimOverrides.MouseEnterOverride.None)
            {
                if (control is IAnimatedControl animatedControl)
                { animatedControl.OnMouseEnterWrapper(e); }
                return (new AnimMessage(), new AnimMessage());
            }

            // Else if the BackColor and/or ForeColor are to be interpolated.
            // Cancel old task & interpolate Animate colours respectively.
            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.BackColorInterpolate))
            {
                animMessageBackground.PlayAnimation = false;
                animMessageBackground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateBackcolor(animMessageBackground, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), animProperties.MouseLeaveBackColor));
            }

            if (animProperties.MouseLeaveOverride.HasFlag(AnimOverrides.MouseLeaveOverride.ForeColorInterpolate))
            {
                animMessageForeground.PlayAnimation = false;
                animMessageForeground = new AnimMessage(control);
                Task.Run(() => AnimAnimations.InterpolateForecolor(animMessageForeground, new AnimInterpolator(animProperties.MouseLeaveDuration, animProperties.MouseLeaveFramerate), animProperties.MouseLeaveForeColor));
            }

            return (animMessageBackground, animMessageForeground);
        }
        #endregion Back/Forecolor Fade Effect: Animation Properties + Explicit AnimMessage Overloads
    }
}
