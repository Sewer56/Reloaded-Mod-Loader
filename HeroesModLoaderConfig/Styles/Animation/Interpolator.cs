using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Styles.Animation
{
    class Interpolator
    {
        /// <summary>
        /// Specifies the amount of sleep iterations for the current colour interpolation event.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SleepTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="durationMilliseconds">Specifies the duration dictating how long the animation is supposed to last.</param>
        /// <param name="framerateMilliseconds">Specifies the amount of frames per second the animation is intended to be played at.</param>
        public Interpolator(float durationMilliseconds, float framerateMilliseconds)
        {
            // Calculate the amount of sleep iterations for the animation.
            Iterations = (int)(durationMilliseconds / framerateMilliseconds);

            // Calculate for the 
            SleepTime = (int)(1000 / framerateMilliseconds);

        }

    }
}
