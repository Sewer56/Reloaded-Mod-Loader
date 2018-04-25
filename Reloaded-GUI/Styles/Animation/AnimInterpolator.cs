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

using System.Collections.Generic;
using ColorMine.ColorSpaces;

namespace Reloaded_GUI.Styles.Animation
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
        /// <param name="destinationColor">The target colour from which the to which the source colour gets interpolated to.</param>
        public List<Lch> CalculateIntermediateColours(Lch sourceColor, Lch destinationColor)
        {
            // Calculate the differences of LCH from source to destination.
            double hDelta = destinationColor.H - sourceColor.H;
            double cDelta = destinationColor.C - sourceColor.C;
            double lDelta = destinationColor.L - sourceColor.L;

            // Store list of colours.
            List<Lch> colours = new List<Lch>(Iterations);

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
                    new Lch
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
