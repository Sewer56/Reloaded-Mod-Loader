using System;

namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Defines an interface which contains the necessary components for
    /// performing simple animation with the mod loader configuration tool.
    /// </summary>
    public interface IAnimatedControl
    {
        /// <summary>
        /// Calls the object's normally internal protected
        /// OnMouseEnter if no override is set.
        /// </summary>
        void OnMouseEnterWrapper(EventArgs e);

        /// <summary>
        /// Calls the object's normally internal protected
        /// OnMouseLeave if no override is set.
        /// </summary>
        void OnMouseLeaveWrapper(EventArgs e);

        /// <summary>
        /// Should contain an AnimProperties struct containing all of
        /// the animation properties.
        /// </summary>
        AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Stops all currently ongoing animations.
        /// </summary>
        void KillAnimations();
    }
}
