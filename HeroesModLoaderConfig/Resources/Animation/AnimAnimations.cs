using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorMine;
using System.Windows.Forms;
using System.Drawing;
using HeroesModLoaderConfig.Utilities.Colour;

namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Provides the individual animation implementations for windows forms controls.
    /// </summary>
    public static class AnimAnimations
    {
        /// <summary>
        /// Interpolates the backcolor of a windows forms control.
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        /// <param name="sourceColor">The colour to which start interpolating from.</param>
        public static async void InterpolateBackcolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color sourceColor, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("BackColor");

            // Calculate all interpolated colours in between.
            ColorMine.ColorSpaces.Lch originalColorLCH = ColorspaceConverter.ColorToLCH(sourceColor);
            ColorMine.ColorSpaces.Lch newColorLCH = ColorspaceConverter.ColorToLCH(destinationColor);
            List<ColorMine.ColorSpaces.Lch> lchColours = interpolator.CalculateIntermediateColours(originalColorLCH, newColorLCH);

            // Converted interpolated colours to RGB.
            List<Color> interpolatedColours = ColorspaceConverter.LCHListToColor(lchColours);

            // Interpolate over the colours.
            foreach (Color newBackgroundColour in interpolatedColours)
            {
                // Set the BackColor
                property.SetValue(animationMessage.Control, newBackgroundColour, null);

                // Wait
                await Task.Delay(interpolator.SleepTime);

                // Check exit condition.
                if (animationMessage.PlayAnimation == false)
                { return; }
            }
        }

        /// <summary>
        /// Interpolates the forecolor of a windows forms control.
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        /// <param name="sourceColor">The colour to which start interpolating from.</param>
        public static async void InterpolateForecolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color sourceColor, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("ForeColor");

            // Calculate all interpolated colours in between.
            ColorMine.ColorSpaces.Lch originalColorLCH = ColorspaceConverter.ColorToLCH((Color)property.GetValue(animationMessage.Control));
            ColorMine.ColorSpaces.Lch newColorLCH = ColorspaceConverter.ColorToLCH(destinationColor);
            List<ColorMine.ColorSpaces.Lch> lchColours = interpolator.CalculateIntermediateColours(originalColorLCH, newColorLCH);

            // Converted interpolated colours to RGB.
            List<Color> interpolatedColours = ColorspaceConverter.LCHListToColor(lchColours);

            // Interpolate over the colours.
            foreach (Color newBackgroundColour in interpolatedColours)
            {
                // Set the BackColor
                property.SetValue(animationMessage.Control, newBackgroundColour, null);

                // Wait
                await Task.Delay(interpolator.SleepTime);

                // Check exit condition.
                if (animationMessage.PlayAnimation == false)
                { return; }
            }
        }

        /// <summary>
        /// Interpolates the forecolor of a windows forms control.
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        public static async void InterpolateForecolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("ForeColor");

            // Calculate all interpolated colours in between.
            Color originalColor = (Color)property.GetValue(animationMessage.Control);

            // Call internal overload.
            InterpolateForecolor(animationMessage, interpolator, originalColor, destinationColor);
        }

        /// <summary>
        /// Interpolates the backcolor of a windows forms control.
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        public static async void InterpolateBackcolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("BackColor");

            // Calculate all interpolated colours in between.
            Color originalColor = (Color)property.GetValue(animationMessage.Control);

            // Call internal overload.
            InterpolateBackcolor(animationMessage, interpolator, originalColor, destinationColor);
        }
    }
}
