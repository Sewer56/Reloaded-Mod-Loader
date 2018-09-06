namespace Reloaded.Input.Common.Controller_Inputs_Substructures
{
    /// <summary>
    /// Defines an analog stick in terms of X + Y, nothing more.
    /// </summary>
    public struct AnalogStick
    {
        private float _x;
        private float _y;

        /// <summary>
        /// Retrieves the X Component.
        /// </summary>
        public float GetX() { return _x; }

        /// <summary>
        /// Retrieves the Y Component.
        /// </summary>
        public float GetY() { return _y; }

        /// <summary>
        /// Sets the value of X. Ensures that the value is within permissible range.
        /// </summary>
        /// <param name="value"></param>
        public void SetX(float value)
        {
            _x = value;

            // Do not allow X to be lesser or greater than man/mix.
            if (_x > ControllerCommon.AxisMaxValueF)
                _x = ControllerCommon.AxisMaxValueF;
            else if (_x < ControllerCommon.AxisMinValueF) _x = ControllerCommon.AxisMinValueF;
        }


        /// <summary>
        /// Sets the value of X. Ensures that the value is within permissible range.
        /// </summary>
        /// <param name="value"></param>
        public void SetY(float value)
        {
            _y = value;

            // Do not allow X to be lesser or greater than man/mix.
            if (_y > ControllerCommon.AxisMaxValueF)
                _y = ControllerCommon.AxisMaxValueF;
            else if (_y < ControllerCommon.AxisMinValueF) _y = ControllerCommon.AxisMinValueF;
        }
    }
}
