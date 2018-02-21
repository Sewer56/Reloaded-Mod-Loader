/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using Reloaded.Input.DirectInput;
using System;

namespace Reloaded.Input
{
    /// <summary>
    /// Defines a class which defines shared structs used for mapping DInput and XInput devices.
    /// Internally provides an interface for a controller which defines the common methods found within 
    /// both of the Mod Loader's XInput and DirectInput controller/input device implementations.
    /// </summary>
    public class ControllerCommon
    {
        /// <summary>
        /// Placeholder when no button is set anywhere, whether it'd be remapping or not.
        /// </summary>
        public const int BUTTON_NULL = 255;

        /// <summary>
        /// Represents the maximum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AXIS_MAX_VALUE_F = 100;

        /// <summary>
        /// Represents the minimum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AXIS_MIN_VALUE_F = -100;

        /// <summary>
        /// Defines an interface for DirectInput + XInput Controller implementations which defines the function names
        /// and signatures to be shared between both the DirectInput and XInput controller implementations.
        /// </summary>
        public interface IController
        {
            /// <summary>
            /// Stores the individual button mappings structure for this controller.
            /// </summary>
            ButtonMapping ButtonMapping { get; set; }

            /// <summary>
            /// Stores the individual axis mappings structure for this controller.
            /// </summary>
            AxisMapping AxisMapping { get; set; }

            /// <summary>
            /// Defines the individual port used for this specific controller.
            /// </summary>
            int ControllerID { get; set; }

            /// <summary>
            /// Defines the custom botton mapping which simulates the individual axis and analog inputs.
            /// </summary>
            EmulationButtonMapping EmulationMapping { get; set; }

            /// <summary>
            /// Retrieves whether a specific button is pressed or not. 
            /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
            /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
            /// Note: The current controller state must first be manually updated.
            /// </summary>
            /// <returns>True if said button is pressed, else false.</returns>
            bool GetButtonState(Controller_Buttons_Generic button);

            /// <summary>
            /// Retrieves the specific intensity in terms of how far/deep an axis is pressed in.
            /// The return value should be a floating point number between -100 and 100 float.
            /// Note: The current controller state must first be manually updated prior.
            /// </summary>
            /// <returns>The value of the axis between -100 and 100.</returns>
            /// <remarks>
            /// This does not take into account the destination axis and reads the value
            /// of the equivalent source axis. If the user has Left Stick mapped to e.g. Right Stick
            /// and you request the right stick axis, the value will return 0 (assuming right stick is centered).
            /// </remarks>
            float GetAxisState(ControllerAxis axis);

            /// <summary>
            /// Retrieves all of the individual button states as an array of boolean values.
            /// True if a button is pressed, false if a button is not pressed.
            /// Note: The current controller state must first be manually updated prior.
            /// </summary>
            /// <returns>Array of currently pressed/held in buttons.</returns>
            bool[] GetButtons();

            /// <summary>
            /// Updates the current state of the controller in question, retrieving the current button presses
            /// and axis measurements.
            /// </summary>
            void UpdateControllerState();

            /// <summary>
            /// Waits for the user to move an axis and retrieves the last pressed axis. 
            /// Accepts any axis as input. Returns the read-in axis.
            /// </summary>
            /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
            /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
            /// <param name="mappingEntry">Specififies the mapping entry containing the axis to be remapped.</param>
            /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
            /// <returns>True if the axis has been successfully remapped, else false.</returns>
            bool RemapAxis(int timeoutSeconds, out float currentTimeout, AxisMappingEntry mappingEntry, ref bool cancellationToken);

            /// <summary>
            /// Waits for the user to press a button and retrieves the last pressed button. 
            /// Accepts any button as input, changes value of passed in button.
            /// </summary>
            /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
            /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
            /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
            /// <param name="cancellationToken">The method polls on this boolean such that if it is set to true, the method will exit.</param>
            /// <returns>True if the button has been successfully remapped, else false.</returns>
            bool RemapButtons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap, ref bool cancellationToken);

            /// <summary>
            /// Retrieves the state of the whole controller in question.
            /// Using a combination of GetAxisState and GetButton state
            /// </summary>
            ControllerInputs GetControllerState();

            /// <summary>
            /// Provides an implementation to be used for remapping of controller inputs.
            /// </summary>
            Remapper Remapper { get; set; }
        }

        /// <summary>
        /// To be used by the mod creator.
        /// Defines all of the individual controller inputs for a specific controller.
        /// </summary>
        public struct ControllerInputs
        {
            /// <summary>
            /// Range -100,100 float. Defines the left analogue stick. Range: -100 to 100
            /// </summary>
            public Analog_Stick leftStick;

            /// <summary>
            /// Range -100,100 float. Defines the right analogue stick. Range: -100 to 100
            /// </summary>
            public Analog_Stick rightStick;

            /// <summary>
            /// Range 0,100 float. Defines the pressure on the left trigger. Range: 0-100
            /// </summary>
            private float leftTriggerPressure;

            /// <summary>
            /// Range 0,100 float. Defines the pressure on the right trigger. Range: 0-100
            /// </summary>
            private float rightTriggerPressure;

            /// <summary>
            /// Defines which of the buttons are currently pressed at the current moment in time.
            /// </summary>
            public ControllerButtonStruct controllerButtons;

            /// <summary>
            /// Sets the left trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetLeftTriggerPressure(float value)
            {
                leftTriggerPressure = value;
                if (leftTriggerPressure > ControllerCommon.AXIS_MAX_VALUE_F) { leftTriggerPressure = ControllerCommon.AXIS_MAX_VALUE_F; }
                if (leftTriggerPressure < ControllerCommon.AXIS_MIN_VALUE_F) { leftTriggerPressure = ControllerCommon.AXIS_MIN_VALUE_F; }
            }

            /// <summary>
            /// Sets the right trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetRightTriggerPressure(float value)
            {
                rightTriggerPressure = value;
                if (rightTriggerPressure > ControllerCommon.AXIS_MAX_VALUE_F) { rightTriggerPressure = ControllerCommon.AXIS_MAX_VALUE_F; }
                if (rightTriggerPressure < ControllerCommon.AXIS_MIN_VALUE_F) { rightTriggerPressure = ControllerCommon.AXIS_MIN_VALUE_F; }
            }

            /// <summary>
            /// Retrieves the value stored in the left trigger.
            /// </summary>
            public float GetLeftTriggerPressure() { return leftTriggerPressure; }


            /// <summary>
            /// Retrieves the value stored in the right trigger.
            /// </summary>
            public float GetRightTriggerPressure() { return rightTriggerPressure; }
        }

        /// <summary>
        /// Defines the struct which declares if each of the buttons is pressed.
        /// </summary>
        public struct ControllerButtonStruct
        {
            /// <summary>
            /// Playstation: Cross, Nintendo: B 
            /// </summary>
            public bool Button_A;

            /// <summary>
            /// Playstation: Circle, Nintendo: A
            /// </summary>
            public bool Button_B;
            
            /// <summary>
            /// Playstation: Square, Nintendo: Y 
            /// </summary>
            public bool Button_X;

            /// <summary>
            /// Playstation: Triangle, Nintendo: X
            /// </summary>
            public bool Button_Y;

            /// <summary>
            /// Playstation: L1, Nintendo: L
            /// </summary>
            public bool Button_LB;

            /// <summary>
            /// Playstation: R1, Nintendo: R
            /// </summary>
            public bool Button_RB;

            /// <summary>
            /// Playstation: Select, Nintendo: Select
            /// </summary>
            public bool Button_Back;

            /// <summary>
            /// Playstation: Select, Nintendo: Start
            /// </summary>
            public bool Button_Start;

            /// <summary>
            /// Playstation: L3, Nintendo: L Click
            /// </summary>
            public bool Button_LS;

            /// <summary>
            /// Playstation: R3, Nintendo: R Click 
            /// </summary>
            public bool Button_RS;

            /// <summary>
            /// Playstation: PS Button, Nintendo: Home
            /// </summary>
            public bool Button_Guide;

            public bool DPAD_UP;
            public bool DPAD_LEFT;
            public bool DPAD_RIGHT;
            public bool DPAD_DOWN;
        }


        /// <summary>
        /// Custom mapping for the generic buttons. 
        /// Maps each regular button of an XBOX to a button ID custom defined by the user.
        /// Sony Playstation and Nintendo equivalents are provided in comments for each button, 
        /// for the convenience of the programmer.
        /// </summary>
        public class ButtonMapping
        {
            /// <summary>
            /// Playstation: Cross, Nintendo: B 
            /// </summary>
            public byte Button_A;

            /// <summary>
            /// Playstation: Circle, Nintendo: A
            /// </summary>
            public byte Button_B;

            /// <summary>
            /// Playstation: Square, Nintendo: Y 
            /// </summary>
            public byte Button_X;

            /// <summary>
            /// Playstation: Triangle, Nintendo: X
            /// </summary>
            public byte Button_Y;

            /// <summary>
            /// Playstation: L1, Nintendo: L
            /// </summary>
            public byte Button_LB;

            /// <summary>
            /// Playstation: R1, Nintendo: R
            /// </summary>
            public byte Button_RB;

            /// <summary>
            /// Playstation: Select, Nintendo: Select
            /// </summary>
            public byte Button_Back;

            /// <summary>
            /// Playstation: Select, Nintendo: Start
            /// </summary>
            public byte Button_Start;

            /// <summary>
            /// Playstation: L3, Nintendo: L Click
            /// </summary>
            public byte Button_LS;

            /// <summary>
            /// Playstation: R3, Nintendo: R Click 
            /// </summary>
            public byte Button_RS;

            /// <summary>
            /// Playstation: PS Button, Nintendo: Home
            /// </summary>
            public byte Button_Guide;
        }

        /// <summary>
        /// Defines a struct specifying a custom mapping of buttons to the DPAD and axis for devices which do not
        /// support analog or POV-hat input such as keyboards and other potential peripherals.
        /// The emulated keys override the real keys if they are set.
        /// </summary>
        public class EmulationButtonMapping
        {
            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD UP if pressed.
            /// </summary>
            public byte DPAD_UP;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD RIGHT if pressed.
            /// </summary>
            public byte DPAD_RIGHT;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD DOWN if pressed.
            /// </summary>
            public byte DPAD_DOWN;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD LEFT if pressed.
            /// </summary>
            public byte DPAD_LEFT;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates right trigger if pressed.
            /// </summary>
            public byte Right_Trigger;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left trigger if pressed.
            /// </summary>
            public byte Left_Trigger;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            public byte Left_Stick_Up;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            public byte Left_Stick_Down;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            public byte Left_Stick_Left;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            public byte Left_Stick_Right;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            public byte Right_Stick_Up;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            public byte Right_Stick_Down;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            public byte Right_Stick_Left;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            public byte Right_Stick_Right;
        }

        /// <summary>
        /// Defines a struct specifying a custom mapping of buttons to the DPAD and axis for devices which do not
        /// support analog or POV-hat input such as keyboards and other potential peripherals.
        /// The emulated keys override the real keys if they are set.
        /// </summary>
        public enum Emulated_Buttons_Generic
        {
            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD UP if pressed.
            /// </summary>
            DPAD_UP,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD RIGHT if pressed.
            /// </summary>
            DPAD_RIGHT,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD DOWN if pressed.
            /// </summary>
            DPAD_DOWN,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD LEFT if pressed.
            /// </summary>
            DPAD_LEFT,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates right trigger if pressed.
            /// </summary>
            Right_Trigger,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left trigger if pressed.
            /// </summary>
            Left_Trigger,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            Left_Stick_Up,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            Left_Stick_Down,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            Left_Stick_Left,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            Left_Stick_Right,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            Right_Stick_Up,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            Right_Stick_Down,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            Right_Stick_Left,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            Right_Stick_Right
        }

        /// <summary>
        /// Reference for the controller buttons as in the ControllerButtons struct. 
        /// The IDs assigned follow the standard schema for the XBOX controller. 
        /// </summary>
        public enum Controller_Buttons_Generic
        {
            /// <summary>
            /// Playstation: Cross, Nintendo: B 
            /// </summary>
            Button_A,

            /// <summary>
            /// Playstation: Circle, Nintendo: A
            /// </summary>
            Button_B,

            /// <summary>
            /// Playstation: Square, Nintendo: Y 
            /// </summary>
            Button_X,

            /// <summary>
            /// Playstation: Triangle, Nintendo: X
            /// </summary>
            Button_Y,

            /// <summary>
            /// Playstation: L1, Nintendo: L
            /// </summary>
            Button_LB,

            /// <summary>
            /// Playstation: R1, Nintendo: R
            /// </summary>
            Button_RB,

            /// <summary>
            /// Playstation: Select, Nintendo: Select
            /// </summary>
            Button_Back,

            /// <summary>
            /// Playstation: Select, Nintendo: Start
            /// </summary>
            Button_Start,

            /// <summary>
            /// Playstation: L3, Nintendo: L Click
            /// </summary>
            Button_LS,

            /// <summary>
            /// Playstation: R3, Nintendo: R Click 
            /// </summary>
            Button_RS,

            /// <summary>
            /// Playstation: PS Button, Nintendo: Home
            /// </summary>
            Button_Guide
        }

        /// <summary>
        /// Defines all of the axis mappings for the individual custom controller.
        /// Used for mapping internal XInput and DirectInput axis to own custom defined axis.
        /// </summary>
        public class AxisMapping
        {
            public AxisMappingEntry leftStickX = new AxisMappingEntry();
            public AxisMappingEntry leftStickY = new AxisMappingEntry();
            public AxisMappingEntry rightStickX = new AxisMappingEntry();
            public AxisMappingEntry rightStickY = new AxisMappingEntry();
            public AxisMappingEntry leftTrigger = new AxisMappingEntry();
            public AxisMappingEntry rightTrigger = new AxisMappingEntry();
        }

        /// <summary>
        /// Defines an individual mapping entry for a controller axis as defined in Controller_Axis_Struct.
        /// Serves as a bridge to provide each axis with an individual name and misc properties.
        /// </summary>
        public class AxisMappingEntry
        {
            /// <summary>
            /// Defines the mapping for the individual axis entry.
            /// Also sometimes known in the source code as the Destination Axis.
            /// </summary>
            public ControllerAxis axis;

            /// <summary>
            /// Stores the name of the property (DirectInput, XInput) that is mapped to the axis type.
            /// Sometimes known in the source code as the Source Axis.
            /// </summary>
            public string propertyName;

            /// <summary>
            /// True if the axis is to be reversed when being read.
            /// </summary>
            public bool isReversed;

            /// <summary>
            /// Defines a deadzone between 0 and 100%. Range: 0-100
            /// </summary>
            public float deadZone;

            /// <summary>
            /// Scales the raw input values by this value.
            /// </summary>
            public float radiusScale;
        }

        /// <summary>
        /// Defines the individual accepted axis for a generic Playstation/XBOX style controller.
        /// </summary>
        public enum ControllerAxis
        {
            Null,
            Left_Stick_X,
            Left_Stick_Y,
            Right_Stick_X,
            Right_Stick_Y,
            Left_Trigger,
            Right_Trigger
        }


        /// <summary>
        /// Values for the individual directions of the directional pad. 
        /// These are the common values for a 8 directional DPAD.
        /// In reality, this value is analog, with a range of 0-36000, albeit this is
        /// practically never used.
        /// </summary>
        [Flags]
        public enum DPAD_Direction
        {
            UP = 0,
            UP_RIGHT = 4500,
            UP_LEFT = 31500,

            RIGHT = 9000,
            LEFT = 27000,

            DOWN = 18000,
            DOWN_RIGHT = 13500,
            DOWN_LEFT = 22500,

            NULL = 65535
        };

        /// <summary>
        /// Defines an analog stick in terms of X + Y, nothing more.
        /// </summary>
        public struct Analog_Stick
        {
            private float X;
            private float Y;

            /// <summary>
            /// Retrieves the X Component.
            /// </summary>
            public float GetX() { return X; }

            /// <summary>
            /// Retrieves the Y Component.
            /// </summary>
            public float GetY() { return Y; }

            /// <summary>
            /// Sets the value of X. Ensures that the value is within permissible range.
            /// </summary>
            /// <param name="value"></param>
            public void SetX(float value)
            {
                X = value;

                // Do not allow X to be lesser or greater than man/mix.
                if (X > ControllerCommon.AXIS_MAX_VALUE_F) { X = ControllerCommon.AXIS_MAX_VALUE_F; }
                else if (X < ControllerCommon.AXIS_MIN_VALUE_F) { X = ControllerCommon.AXIS_MIN_VALUE_F; }
            }


            /// <summary>
            /// Sets the value of X. Ensures that the value is within permissible range.
            /// </summary>
            /// <param name="value"></param>
            public void SetY(float value)
            {
                Y = value;

                // Do not allow X to be lesser or greater than man/mix.
                if (Y > ControllerCommon.AXIS_MAX_VALUE_F) { Y = ControllerCommon.AXIS_MAX_VALUE_F; }
                else if (Y < ControllerCommon.AXIS_MIN_VALUE_F) { Y = ControllerCommon.AXIS_MIN_VALUE_F; }
            }
        }

        /// <summary>
        /// Processes the obtained raw value with DInput/XInput ranges and performs modifications on it such
        /// as changing the radius of the axis reading or checking for deadzones.
        /// </summary>
        /// <param name="mappingEntry">The mapping entry for the axis defining which axis should be used.</param>
        /// <param name="rawValue">The raw value obtained from the axis query..</param>
        /// <param name="isScaled">Declares whether the value is already scaled appropriately before reaching the function or not. Currently false for DInput and true for XInput.</param>
        public static float InputProcessAxisRawValue(int rawValue, AxisMappingEntry mappingEntry, bool isScaled)
        {
            // Reverse Axis if Necessary
            if (mappingEntry.isReversed) { rawValue = -1 * rawValue; }

            // Scale Axis if Necessary.
            float newRawValue = rawValue;
            if (!isScaled) { newRawValue = ((float)rawValue / (float)DInputManager.AXIS_MAX_VALUE) * ControllerCommon.AXIS_MAX_VALUE_F; }

            // If triggers. scale to between 0 - 100 (from -100 - 100)
            switch (mappingEntry.axis)
            {
                case ControllerAxis.Left_Trigger:
                case ControllerAxis.Right_Trigger:
                    newRawValue += ControllerCommon.AXIS_MAX_VALUE_F;
                    newRawValue *= DInputManager.TRIGGER_SCALE_FACTOR;
                    break;
            }

            // If the input lays within the acceptable deadzone range, return null.
            if (VerifyDeadzones(newRawValue, mappingEntry)) { return 0F; }

            // Scale Radius Scale Value
            newRawValue *= mappingEntry.radiusScale;

            // Return axis value.
            return newRawValue;
        }

        /// <summary>
        /// Returns true the input raw value is within a deadzone specified
        /// by the specific axis mapping entry. 
        /// </summary>
        /// <param name="axisValue">The scaled value of the prior obtained raw axis reading: Range -100 to 100 (Sticks) or 0-100 (Triggers)</param>
        /// <param name="mappingEntry">The mapping entry for the axis defining the deadzone for the specific axis.</param>
        public static bool VerifyDeadzones(float axisValue, AxisMappingEntry mappingEntry)
        {
            // Verify Deadzones
            switch (mappingEntry.axis)
            {
                // For all analog sticks.
                case ControllerAxis.Left_Stick_X:
                case ControllerAxis.Left_Stick_Y:
                case ControllerAxis.Right_Stick_X:
                case ControllerAxis.Right_Stick_Y:

                    // Get boundaries of deadzone.
                    float deadzoneMax = (ControllerCommon.AXIS_MAX_VALUE_F / 100.0F) * mappingEntry.deadZone;
                    float deadzoneMin = -(ControllerCommon.AXIS_MAX_VALUE_F / 100.0F) * mappingEntry.deadZone;

                    // If within boundaries, axis value is 0.
                    if ((axisValue < deadzoneMax) && (axisValue > deadzoneMin))
                    {
                        return true;
                    }

                    // Else break.
                    break;

                // For all triggers
                case ControllerAxis.Left_Trigger:
                case ControllerAxis.Right_Trigger:

                    // Get max deadzone
                    float deadzoneTriggerMax = ((ControllerCommon.AXIS_MAX_VALUE_F) / 100.0F) * mappingEntry.deadZone;

                    // If within bounds, axis value is 0.
                    if (axisValue < deadzoneTriggerMax)
                    {
                        return true;
                    }

                    // Else break.
                    break;
            }

            // If not in deadzones return false.
            return false;
        }


        /// <summary>
        /// Returns the appropriate axis mapping entry for each individual specific axis.
        /// The axis have a 1-1 relationship (oldaxis -> new axis) and a set of properties as defined in
        /// Controller_Axis_Mapping.
        /// </summary>
        /// <param name="axis">The axis whose details we want to obtain.</param>
        /// <param name="axisMapping">The axis mapping which stores the axis details for the individual controller.</param>
        /// <returns></returns>
        public static AxisMappingEntry InputGetMappedAxis(ControllerAxis axis, AxisMapping axisMapping)
        {
            // Stores the axis configuration that is to be returned.
            AxisMappingEntry controllerAxisMapping = new AxisMappingEntry();

            // Find axis mapped to the requested controller axis.
            // Check every axis manually, until one of the axes contains the desired destination axis.
            if (IsCorrectAxisMappingEntry(axisMapping.leftStickX, axis)) { return axisMapping.leftStickX; }
            if (IsCorrectAxisMappingEntry(axisMapping.leftStickY, axis)) { return axisMapping.leftStickY; }

            if (IsCorrectAxisMappingEntry(axisMapping.rightStickX, axis)) { return axisMapping.rightStickX; }
            if (IsCorrectAxisMappingEntry(axisMapping.rightStickY, axis)) { return axisMapping.rightStickY; }

            if (IsCorrectAxisMappingEntry(axisMapping.leftTrigger, axis)) { return axisMapping.leftTrigger; }
            if (IsCorrectAxisMappingEntry(axisMapping.rightTrigger, axis)) { return axisMapping.rightTrigger; }

            // Retrieve empty struct if null, else the correct axis mapping.
            return controllerAxisMapping;
        }

        /// <summary>
        /// Verifies whether a passed in axis mapping entry's axis is the one requested
        /// by the user/programmer. i.e. axisMappingEntry.axis == axis
        /// </summary>
        /// <param name="axis">The requested axis to verify if there is a match with the other parameter axis of axis mapping entry.</param>
        /// <param name="axisMappingEntry">The mapping entry to check if matches the currently requested axis.</param>
        /// <returns>True if it is the correct entry, else false.</returns>
        private static bool IsCorrectAxisMappingEntry(AxisMappingEntry axisMappingEntry, ControllerAxis axis)
        {
            return axisMappingEntry.axis == axis ? true : false;
        }

        /// <summary>
        /// Returns the value of a property of an object which shares the same name as the passed in string.
        /// e.g. joystickState with a string named AccelX would return a theoretical joystickState.AccelX
        /// </summary>
        public static object Reflection_GetValue(object sourceObject, string propertyName)
        {
            try { return sourceObject.GetType().GetProperty(propertyName).GetValue(sourceObject, null); }
            catch { return 0; }
        }
    }
}
