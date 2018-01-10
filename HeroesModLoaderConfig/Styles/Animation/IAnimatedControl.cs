using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Defines an interface which contains the necessary components for
    /// performing simple animation with the mod loader configuration tool.
    /// </summary>
    interface IAnimatedControl
    {
        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse is in focus of the control.
        /// </summary>
        void AnimateMouseEnter(EventArgs e);

        /// <summary>
        /// Defines the animation sequence that is to be played asynchronously while
        /// the mouse loses focus of the control.
        /// </summary>
        void AnimateMouseLeave(EventArgs e);
    }
}
