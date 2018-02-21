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

using System.ComponentModel;
using System.Drawing;
using static ReloadedLauncher.Styles.Animation.AnimOverrides;

namespace ReloadedLauncher.Styles.Animation
{
    /// <summary>
    /// Provides various animation properties for common objects implementing the animation subsystem.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AnimProperties
    {
        #region Mouse Enter Animation Properties

        /// <summary>
        /// Defines what kind of animation should the button perform when the mouse
        /// enters over the button.
        /// </summary>
        [Category("Mouse Enter Animation")][Description("[Flags] Specifies the animations to be played when the mouse enters the control.")]
        public MouseEnterOverride MouseEnterOverride { get; set; }

        /// <summary>
        /// Specifies the duration dictating how long the animation is supposed to last.
        /// </summary>
        [Category("Mouse Enter Animation")][Description("Specifies the duration dictating how long the animation is supposed to last.")]
        public float MouseEnterDuration { get; set; }

        /// <summary>
        /// Specifies the amount of frames per second the animation is intended to be played at.
        /// </summary>
        [Category("Mouse Enter Animation")][Description("Specifies the amount of frames per second the animation is intended to be played at.")]
        public float MouseEnterFramerate { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Enter Animation")][Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.")]
        public Color MouseEnterBackColor { get; set; }

        /// <summary>
        /// Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Enter Animation")][Description("Target colour for mouse enter animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.")]
        public Color MouseEnterForeColor { get; set; }

        #endregion

        #region Mouse Leave Animation Properties

        /// <summary>
        /// Defines what kind of animation should the button perform when the mouse
        /// leaves the button.
        /// </summary>
        [Category("Mouse Leave Animation")][Description("[Flags] Specifies the animations to be played when the mouse leaves the control.")]
        public MouseLeaveOverride MouseLeaveOverride { get; set; }

        /// <summary>
        /// Specifies the duration dictating how long the animation is supposed to last.
        /// </summary>
        [Category("Mouse Leave Animation")][Description("Specifies the duration dictating how long the animation is supposed to last.")]
        public float MouseLeaveDuration { get; set; }

        /// <summary>
        /// Specifies the amount of frames per second the animation is intended to be played at.
        /// </summary>
        [Category("Mouse Leave Animation")][Description("Specifies the amount of frames per second the animation is intended to be played at.")]
        public float MouseLeaveFramerate { get; set; }

        /// <summary>
        /// Target colour for mouse leave animation. Specifies the colour that should be blended in if the backcolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Leave Animation")][Description("Target colour for mouse leave animation. Specifies the colour that should be blended in if the backcolor mouse leave override is set.")]
        public Color MouseLeaveBackColor { get; set; }

        /// <summary>
        /// Target colour for mouse leave animation. Specifies the colour that should be blended in if the forecolor mouse enter override is set.
        /// </summary>
        [Category("Mouse Leave Animation")][Description("Target colour for mouse leave animation. Specifies the colour that should be blended in if the forecolor mouse leave override is set.")]
        public Color MouseLeaveForeColor { get; set; }

        #endregion

        #region Property Animation Flags

        /// <summary>
        /// Set property PlayAnimation to false to terminate all current animations that affect the
        /// BackColor of the control.
        /// </summary>
        public AnimMessage BackColorMessage { get; set; }

        /// <summary>
        /// Set property PlayAnimation to false to terminate all current animations that affect the
        /// ForeColor of the control.
        /// </summary>
        public AnimMessage ForeColorMessage { get; set; }

        #endregion
    }
}
