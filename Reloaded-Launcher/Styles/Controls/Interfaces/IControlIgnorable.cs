namespace ReloadedLauncher.Styles.Controls.Interfaces
{
    /// <summary>
    /// Interface which allows controls to specify whether they may be ignored by mouse or other user inputs.
    /// Interface elements should also override CreateParams, of access modifier protected. 
    /// </summary>
    interface IControlIgnorable
    {
        /// <summary>
        /// If set to true, the control ignores the mouse.
        /// </summary>
        bool IgnoreMouse { get; set; }

        /* 
        Note: This should be appended to all members implementing IControlIgnorable
         
        /// <summary>
        /// Overrides the information needed when the control is created or accessed to
        /// either ignore input on the label or not ignore input.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (IgnoreMouse) { cp.Style |= 0x08000000; }  // Enable WS_DISABLED
                return cp;
            }
        }
        */
    }
}
