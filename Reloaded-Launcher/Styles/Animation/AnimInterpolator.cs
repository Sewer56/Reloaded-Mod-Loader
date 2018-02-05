using System.Collections.Generic;

namespace ReloadedLauncher.Styles.Animation
{
    public class AnimInterpolator
    {
        /// <summary>
        /// Specifies the amount of sleep iterations for the current colour interpolation event.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Specifies the time between each sleep call for the animation thread.
        /// </summary>
        public int SleepTime { get; set; }

        /// <summary>
        /// Creates an instance of the interpolator for the individual animation.
        /// Allows setting a duration for the animation to be performed and framerate at which the animation is performed at.
        /// </summary>
        /// <param name="durationMilliseconds">Specifies the duration dictating how long the animation is supposed to last.</param>
        /// <param name="framerate">Specifies the amount of frames per second the animation is intended to be played at.</param>
        public AnimInterpolator(float durationMilliseconds, float framerate)
        {
            // Calculate for the time between each sleep call for the animation thread.
            SleepTime = (int)(1000 / framerate);

            // Calculate the amount of sleep iterations for the animation.
            Iterations = (int)(durationMilliseconds / SleepTime);
        }

        /// <summary>
        /// Calculates all of the intermediate colours between colour X and colour Y
        /// to be used for interpolation purposes.
        /// </summary>
        /// <param name="sourceColor">Colour from which the list of interpolated colours begins from.</param>
        /// <param name="sourceColor">The target colour from which the to which the source colour gets interpolated to.</param>
        public List<ColorMine.ColorSpaces.Lch> CalculateIntermediateColours(ColorMine.ColorSpaces.Lch sourceColor, ColorMine.ColorSpaces.Lch destinationColor)
        {
            // Calculate the differences of LCH from source to destination.
            double hDelta = destinationColor.H - sourceColor.H;
            double cDelta = destinationColor.C - sourceColor.C;
            double lDelta = destinationColor.L - sourceColor.L;

            // Store list of colours.
            List<ColorMine.ColorSpaces.Lch> colours = new List<ColorMine.ColorSpaces.Lch>(Iterations);

            // Calculate all intermediate colours.
            // x = 1 ignores source colour.
            // Iterations + 1 ensures no traces of original colour left when e.g. fading to black.
            for (int x = 1; x < Iterations; x++)
            {
                // Defines the percentage in terms of completeness of all iterations.
                double percentageProgress = (float)x / Iterations;

                // Scale the delta values by the percentage.
                double hScaled = hDelta * percentageProgress;
                double cScaled = cDelta * percentageProgress;
                double lScaled = lDelta * percentageProgress;

                // Add a new colour which is a combination of the source value with added scaled delta
                colours.Add
                (
                    new ColorMine.ColorSpaces.Lch
                    (
                        sourceColor.L + lScaled,
                        sourceColor.C + cScaled,
                        sourceColor.H + hScaled
                    )
                );
            }

            // Add target colour
            colours.Add(destinationColor);

            // Return the new colours.
            return colours;
        }
    }
}
