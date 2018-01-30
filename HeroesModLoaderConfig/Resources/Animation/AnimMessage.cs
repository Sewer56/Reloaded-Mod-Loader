namespace HeroesModLoaderConfig.Styles.Animation
{
    /// <summary>
    /// Provides a struct that couples a control alongside a flag which signals whether the current animation
    /// playback should be terminated for next animation.
    /// </summary>
    public class AnimMessage
    {
        /// <summary>
        /// The Windows Forms Control (or any object Implementing BackColor, ForeColor) that is to be animated.
        /// </summary>
        public object Control { get; set; }

        /// <summary>
        /// The flag which defines whether the animation should be played.
        /// If set to false, the animation will permanently cease.
        /// </summary>
        public bool PlayAnimation { get; set; }

        /// <summary>
        /// Instantiates an animation message which defines a flag declaring whether
        /// the animation should be immediately halted and the Windows Forms control that
        /// should be animated.
        /// </summary>
        /// <param name="control">Defines the control that is intended to be animated.</param>
        public AnimMessage(object control)
        {
            Control = control;
            PlayAnimation = true;
        }

        /// <summary>
        /// Default constructor, not worth using.
        /// </summary>
        public AnimMessage() { PlayAnimation = true; }
    }
}
