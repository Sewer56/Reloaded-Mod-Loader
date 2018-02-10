using ReloadedLauncher.Utilities.Colour;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReloadedLauncher.Styles.Animation
{
    /// <summary>
    /// Provides the individual animation implementations for generic objects implementing
    /// certain properties, such as Windows Forms Controls.
    /// </summary>
    public static class AnimAnimations
    {
        /// <summary>
        /// Delegate used for invocation that will change the color of a control element.
        /// Used for the purpose of invokation of colour change on an object that may require
        /// invokation, such as Windows Forms Controls.
        /// </summary>
        delegate void ChangeColorDelegate(PropertyInfo propertyInfo, object control, Color newColor);

        /// <summary>
        /// Interpolates the value of a System.Drawing.Color property such as BackColor or ForeColor of a generic object.
        /// The property to interpolate is provided via the propertyInfo object.
        /// </summary>
        /// <param name="propertyInfo">The property of an object such as BackColor of whose color is to be interpolated. </param>
        /// <param name="animationMessage">A message structure that can be modified by the calling structure to prematurely end the animation. </param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        /// <param name="sourceColor">The colour to which start interpolating from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async void InterpolateColor(PropertyInfo propertyInfo, AnimMessage animationMessage, AnimInterpolator interpolator, Color sourceColor, Color destinationColor)
        {
            // Safety procedure
            try
            {
                // Calculate all interpolated colours in between.
                ColorMine.ColorSpaces.Lch originalColorLCH = ColorspaceConverter.ColorToLCH(sourceColor);
                ColorMine.ColorSpaces.Lch newColorLCH = ColorspaceConverter.ColorToLCH(destinationColor);
                List<ColorMine.ColorSpaces.Lch> lchColours = interpolator.CalculateIntermediateColours(originalColorLCH, newColorLCH);

                // Converted interpolated colours to RGB.
                List<Color> interpolatedColours = ColorspaceConverter.LCHListToColor(lchColours);

                // Check if object is a winform control.
                Control winFormControl = animationMessage.Control as Control;
                bool isWinFormControl = winFormControl != null ? true : false;

                // Interpolate over the colours.
                foreach (Color newBackgroundColour in interpolatedColours)
                {
                    // Check exit condition.
                    if (animationMessage.PlayAnimation == false)
                    { return; }

                    // Set the BackColor
                    if (isWinFormControl) { winFormControl.Invoke((ChangeColorDelegate)ChangeColor, propertyInfo, winFormControl, newBackgroundColour); }
                    else { propertyInfo.SetValue(animationMessage.Control, newBackgroundColour, null); }

                    // Wait
                    await Task.Delay(interpolator.SleepTime);
                }
            } catch { }
        }

        /// <summary>
        /// Interpolates the forecolor of a windows forms control (or any object with a ForeColor property).
        /// </summary>
        /// <param name="animationMessage">The control whose forecolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        /// <param name="sourceColor">The colour to which start interpolating from.</param>
        public static async void InterpolateForecolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color sourceColor, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("ForeColor");

            // Call internal overload.
            InterpolateColor(property, animationMessage, interpolator, sourceColor, destinationColor);
        }

        /// <summary>
        /// Interpolates the backcolor of a windows forms control (or any object with a BackColor property).
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        /// <param name="sourceColor">The colour to which start interpolating from.</param>
        public static async void InterpolateBackcolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color sourceColor, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("BackColor");

            // Call internal overload.
            InterpolateColor(property, animationMessage, interpolator, sourceColor, destinationColor);
        }

        /// <summary>
        /// Interpolates the forecolor of a windows forms control (or any object with a ForeColor property).
        /// </summary>
        /// <param name="animationMessage">The control whose forecolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        public static async void InterpolateForecolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("ForeColor");

            // Calculate the original colour.
            Color originalColor = (Color)property.GetValue(animationMessage.Control);

            // Call internal overload.
            InterpolateColor(property, animationMessage, interpolator, originalColor, destinationColor);
        }

        /// <summary>
        /// Interpolates the backcolor of a windows forms control (or any object with a BackColor property).
        /// </summary>
        /// <param name="animationMessage">The control whose backcolor is meant to be interpolated.</param>
        /// <param name="destinationColor">The target colour to interpolate the backcolor to.</param>
        /// <param name="interpolator">The interpolator object used for calculating interpolations of colours as well as animation durations.</param>
        public static async void InterpolateBackcolor(AnimMessage animationMessage, AnimInterpolator interpolator, Color destinationColor)
        {
            // Retrieve the BackColor Property via Reflection.
            var property = animationMessage.Control.GetType().GetProperty("BackColor");

            // Calculate the original colour.
            Color originalColor = (Color)property.GetValue(animationMessage.Control);

            // Call internal overload.
            InterpolateColor(property, animationMessage, interpolator, originalColor, destinationColor);
        }

        /// <summary>
        /// Simple delegate for changing the color of an object.
        /// </summary>
        /// <param name="propertyInfo">The property to be changed, either ForeColor or BackColor, obtainable via e.g. object.GetType().GetProperty("ForeColor");</param>
        /// <param name="control">The object whose property is to be changed, e.g. WinForm control.</param>
        /// <param name="newColor">The new colour to be applied.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ChangeColor(PropertyInfo propertyInfo, object control, Color newColor)
        {
            propertyInfo.SetValue(control, newColor);
        }
    }
}
