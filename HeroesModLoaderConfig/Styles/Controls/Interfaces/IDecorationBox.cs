namespace HeroesModLoaderConfig.Styles.Controls.Interfaces
{
    /// <summary>
    /// Declares the variables a control must posess if it is to be potentially used as a decoration box.
    /// Allows/Disallows the capturing of children controls if they are in the region of the 
    /// </summary>
    interface IDecorationBox
    {
        /// <summary>
        /// Declares whether the decoration box should capture the children controls 
        /// </summary>
        bool CaptureChildren { get; set; }
    }
}
