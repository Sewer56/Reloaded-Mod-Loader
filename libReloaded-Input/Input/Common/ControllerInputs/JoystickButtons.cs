namespace Reloaded.Input.Common.Controller_Inputs_Substructures
{

    /// <summary>
    /// Defines the struct which declares if each of the buttons is pressed.
    /// </summary>
    public struct JoystickButtons
    {
        /// <summary>
        /// Playstation: Cross, Nintendo: B 
        /// </summary>
        public bool ButtonA;

        /// <summary>
        /// Playstation: Circle, Nintendo: A
        /// </summary>
        public bool ButtonB;

        /// <summary>
        /// Playstation: Square, Nintendo: Y 
        /// </summary>
        public bool ButtonX;

        /// <summary>
        /// Playstation: Triangle, Nintendo: X
        /// </summary>
        public bool ButtonY;

        /// <summary>
        /// Playstation: L1, Nintendo: L
        /// </summary>
        public bool ButtonLb;

        /// <summary>
        /// Playstation: R1, Nintendo: R
        /// </summary>
        public bool ButtonRb;

        /// <summary>
        /// Playstation: Select, Nintendo: Select
        /// </summary>
        public bool ButtonBack;

        /// <summary>
        /// Playstation: Select, Nintendo: Start
        /// </summary>
        public bool ButtonStart;

        /// <summary>
        /// Playstation: L3, Nintendo: L Click
        /// </summary>
        public bool ButtonLs;

        /// <summary>
        /// Playstation: R3, Nintendo: R Click 
        /// </summary>
        public bool ButtonRs;

        /// <summary>
        /// Playstation: PS Button, Nintendo: Home
        /// </summary>
        public bool ButtonGuide;

        public bool DpadUp;
        public bool DpadLeft;
        public bool DpadRight;
        public bool DpadDown;
    }
}
