using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Defines an interface which contains the necessary components for
    /// performing simple animation with the mod loader configuration tool.
    /// </summary>
    interface IAnimation
    {
        /// <summary>
        /// Defines whether the mouse is currently focusing on the object.
        /// If so, define an appropriate effect e.g. color tint.
        /// </summary>
        bool MouseInFocus { get; set; }

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the button/thing.
        /// </summary>
        void AnimateInFocus();
    }
}
