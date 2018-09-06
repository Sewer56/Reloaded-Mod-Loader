using Reloaded.Input.Common.Controller_Inputs_Substructures;

namespace Reloaded.Input.Common.ControllerInputs
{
    /// <summary>
    /// To be used by the mod creator.
    /// Defines all of the individual controller inputs for a specific controller.
    /// </summary>
    public struct ControllerInputs
    {
        /// <summary>
        /// Range -100,100 float. Defines the left analogue stick. Range: -100 to 100
        /// </summary>
        public AnalogStick LeftStick;

        /// <summary>
        /// Range -100,100 float. Defines the right analogue stick. Range: -100 to 100
        /// </summary>
        public AnalogStick RightStick;

        /// <summary>
        /// Range 0,100 float. Defines the pressure on the left trigger. Range: 0-100
        /// </summary>
        private float _leftTriggerPressure;

        /// <summary>
        /// Range 0,100 float. Defines the pressure on the right trigger. Range: 0-100
        /// </summary>
        private float _rightTriggerPressure;

        /// <summary>
        /// Defines which of the buttons are currently pressed at the current moment in time.
        /// </summary>
        public JoystickButtons ControllerButtons;

        /// <summary>
        /// Sets the left trigger pressure such that it falls within the allowable bounds of 
        /// maximum and minimum acceptable value.
        /// </summary>
        public void SetLeftTriggerPressure(float value)
        {
            _leftTriggerPressure = value;
            if (_leftTriggerPressure > ControllerCommon.AxisMaxValueF) _leftTriggerPressure = ControllerCommon.AxisMaxValueF;
            if (_leftTriggerPressure < ControllerCommon.AxisMinValueF) _leftTriggerPressure = ControllerCommon.AxisMinValueF;
        }

        /// <summary>
        /// Sets the right trigger pressure such that it falls within the allowable bounds of 
        /// maximum and minimum acceptable value.
        /// </summary>
        public void SetRightTriggerPressure(float value)
        {
            _rightTriggerPressure = value;
            if (_rightTriggerPressure > ControllerCommon.AxisMaxValueF) _rightTriggerPressure = ControllerCommon.AxisMaxValueF;
            if (_rightTriggerPressure < ControllerCommon.AxisMinValueF) _rightTriggerPressure = ControllerCommon.AxisMinValueF;
        }

        /// <summary>
        /// Retrieves the value stored in the left trigger.
        /// </summary>
        public float GetLeftTriggerPressure() { return _leftTriggerPressure; }

        /// <summary>
        /// Retrieves the value stored in the right trigger.
        /// </summary>
        public float GetRightTriggerPressure() { return _rightTriggerPressure; }

        /// <summary>
        /// Returns a struct instance with default, zero set values.
        /// </summary>
        public static ControllerInputs GetDefaultInputs()
        {
            // Return default.
            ControllerInputs inputs = new ControllerInputs()
            {
                ControllerButtons = new JoystickButtons()
                {
                    ButtonA = false,
                    ButtonB = false,
                    ButtonBack = false,
                    ButtonGuide = false,
                    ButtonLb = false,
                    ButtonLs = false,
                    ButtonRb = false,
                    ButtonRs = false,
                    ButtonStart = false,
                    ButtonX = false,
                    ButtonY = false,
                    DpadDown = false,
                    DpadLeft = false,
                    DpadRight = false,
                    DpadUp = false
                },
                LeftStick = new AnalogStick(),
                RightStick = new AnalogStick()
            };
            inputs.LeftStick.SetX(0);
            inputs.LeftStick.SetY(0);
            inputs.RightStick.SetX(0);
            inputs.RightStick.SetY(0);
            return inputs;
        }
    }
}
