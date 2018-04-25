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

using System;
using Reloaded.Input.DirectInput;
using Reloaded.Input.Modules;

namespace Reloaded.Input.Common
{
    /// <summary>
    /// Defines a class which defines shared structs used for mapping DInput and XInput devices.
    /// Internally provides an interface for a controller which defines the common methods found within 
    /// both of the Mod Loader's XInput and DirectInput controller/input device implementations.
    /// </summary>
    public class ControllerCommon
    {
        /// <summary>
        /// Reference for the controller buttons as in the ControllerButtons struct. 
        /// The IDs assigned follow the standard schema for the XBOX controller. 
        /// </summary>
        public enum ControllerButtonsGeneric
        {
            /// <summary>
            /// Playstation: Cross, Nintendo: B 
            /// </summary>
            ButtonA,

            /// <summary>
            /// Playstation: Circle, Nintendo: A
            /// </summary>
            ButtonB,

            /// <summary>
            /// Playstation: Square, Nintendo: Y 
            /// </summary>
            ButtonX,

            /// <summary>
            /// Playstation: Triangle, Nintendo: X
            /// </summary>
            ButtonY,

            /// <summary>
            /// Playstation: L1, Nintendo: L
            /// </summary>
            ButtonLb,

            /// <summary>
            /// Playstation: R1, Nintendo: R
            /// </summary>
            ButtonRb,

            /// <summary>
            /// Playstation: Select, Nintendo: Select
            /// </summary>
            ButtonBack,

            /// <summary>
            /// Playstation: Select, Nintendo: Start
            /// </summary>
            ButtonStart,

            /// <summary>
            /// Playstation: L3, Nintendo: L Click
            /// </summary>
            ButtonLs,

            /// <summary>
            /// Playstation: R3, Nintendo: R Click 
            /// </summary>
            ButtonRs,

            /// <summary>
            /// Playstation: PS Button, Nintendo: Home
            /// </summary>
            ButtonGuide
        }

        /// <summary>
        /// Defines the individual accepted axis for a generic Playstation/XBOX style controller.
        /// </summary>
        public enum ControllerAxis
        {
            Null,
            LeftStickX,
            LeftStickY,
            RightStickX,
            RightStickY,
            LeftTrigger,
            RightTrigger
        }


        /// <summary>
        /// Values for the individual directions of the directional pad. 
        /// These are the common values for a 8 directional DPAD.
        /// In reality, this value is analog, with a range of 0-36000, albeit this is
        /// practically never used.
        /// </summary>
        [Flags]
        public enum DpadDirection
        {
            Up = 0,
            UpRight = 4500,
            UpLeft = 31500,

            Right = 9000,
            Left = 27000,

            Down = 18000,
            DownRight = 13500,
            DownLeft = 22500,

            Null = 65535
        }

        /// <summary>
        /// Defines a struct specifying a custom mapping of buttons to the DPAD and axis for devices which do not
        /// support analog or POV-hat input such as keyboards and other potential peripherals.
        /// The emulated keys override the real keys if they are set.
        /// </summary>
        public enum EmulatedButtonsGeneric
        {
            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD UP if pressed.
            /// </summary>
            DpadUp,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD RIGHT if pressed.
            /// </summary>
            DpadRight,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD DOWN if pressed.
            /// </summary>
            DpadDown,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD LEFT if pressed.
            /// </summary>
            DpadLeft,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates right trigger if pressed.
            /// </summary>
            RightTrigger,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left trigger if pressed.
            /// </summary>
            LeftTrigger,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            LeftStickUp,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            LeftStickDown,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            LeftStickLeft,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            LeftStickRight,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            RightStickUp,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            RightStickDown,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            RightStickLeft,

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            RightStickRight
        }

        /// <summary>
        /// Placeholder when no button is set anywhere, whether it'd be remapping or not.
        /// </summary>
        public const int ButtonNull = 255;

        /// <summary>
        /// Represents the maximum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AxisMaxValueF = 100;

        /// <summary>
        /// Represents the minimum value of the axis as returned to the modder/user.
        /// </summary>
        public static float AxisMinValueF = -100;

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
            if (mappingEntry.IsReversed) rawValue = -1 * rawValue;

            // Scale Axis if Necessary.
            float newRawValue = rawValue;
            if (!isScaled) newRawValue = rawValue / (float)DInputManager.AxisMaxValue * AxisMaxValueF;

            // If triggers. scale to between 0 - 100 (from -100 - 100)
            switch (mappingEntry.DestinationAxis)
            {
                case ControllerAxis.LeftTrigger:
                case ControllerAxis.RightTrigger:
                    newRawValue += AxisMaxValueF;
                    newRawValue *= DInputManager.TriggerScaleFactor;
                    break;
            }

            // If the input lays within the acceptable deadzone range, return null.
            if (VerifyDeadzones(newRawValue, mappingEntry)) return 0F;

            // Scale Radius Scale Value
            newRawValue *= mappingEntry.RadiusScale;

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
            switch (mappingEntry.DestinationAxis)
            {
                // For all analog sticks.
                case ControllerAxis.LeftStickX:
                case ControllerAxis.LeftStickY:
                case ControllerAxis.RightStickX:
                case ControllerAxis.RightStickY:

                    // Get boundaries of deadzone.
                    float deadzoneMax = AxisMaxValueF / 100.0F * mappingEntry.DeadZone;
                    float deadzoneMin = -(AxisMaxValueF / 100.0F) * mappingEntry.DeadZone;

                    // If within boundaries, axis value is 0.
                    if (axisValue < deadzoneMax && axisValue > deadzoneMin) return true;

                    // Else break.
                    break;

                // For all triggers
                case ControllerAxis.LeftTrigger:
                case ControllerAxis.RightTrigger:

                    // Get max deadzone
                    float deadzoneTriggerMax = AxisMaxValueF / 100.0F * mappingEntry.DeadZone;

                    // If within bounds, axis value is 0.
                    if (axisValue < deadzoneTriggerMax) return true;

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
            if (IsCorrectAxisMappingEntry(axisMapping.LeftStickX, axis)) return axisMapping.LeftStickX;
            if (IsCorrectAxisMappingEntry(axisMapping.LeftStickY, axis)) return axisMapping.LeftStickY;

            if (IsCorrectAxisMappingEntry(axisMapping.RightStickX, axis)) return axisMapping.RightStickX;
            if (IsCorrectAxisMappingEntry(axisMapping.RightStickY, axis)) return axisMapping.RightStickY;

            if (IsCorrectAxisMappingEntry(axisMapping.LeftTrigger, axis)) return axisMapping.LeftTrigger;
            if (IsCorrectAxisMappingEntry(axisMapping.RightTrigger, axis)) return axisMapping.RightTrigger;

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
            return axisMappingEntry.DestinationAxis == axis;
        }

        /// <summary>
        /// Returns the value of a property of an object which shares the same name as the passed in string.
        /// e.g. joystickState with a string named AccelX would return a theoretical joystickState.AccelX
        /// </summary>
        public static object Reflection_GetValue(object sourceObject, string propertyName)
        {
            try { return sourceObject.GetType().GetProperty(propertyName)?.GetValue(sourceObject, null); }
            catch { return 0; }
        }

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
            int ControllerId { get; set; }

            /// <summary>
            /// Defines the custom botton mapping which simulates the individual axis and analog inputs.
            /// </summary>
            EmulationButtonMapping EmulationMapping { get; set; }

            /// <summary>
            /// Provides an implementation to be used for remapping of controller inputs.
            /// </summary>
            Remapper Remapper { get; set; }

            /// <summary>
            /// Retrieves whether a specific button is pressed or not. 
            /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
            /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
            /// Note: The current controller state must first be manually updated.
            /// </summary>
            /// <returns>True if said button is pressed, else false.</returns>
            bool GetButtonState(ControllerButtonsGeneric button);

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
            /// Retrieves true if the input device is connected, else false.
            /// </summary>
            /// <returns>Retrieves true if the input device is connected, else false.</returns>
            bool IsConnected();

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
            public ControllerButtonStruct ControllerButtons;

            /// <summary>
            /// Sets the left trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetLeftTriggerPressure(float value)
            {
                _leftTriggerPressure = value;
                if (_leftTriggerPressure > AxisMaxValueF) _leftTriggerPressure = AxisMaxValueF;
                if (_leftTriggerPressure < AxisMinValueF) _leftTriggerPressure = AxisMinValueF;
            }

            /// <summary>
            /// Sets the right trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetRightTriggerPressure(float value)
            {
                _rightTriggerPressure = value;
                if (_rightTriggerPressure > AxisMaxValueF) _rightTriggerPressure = AxisMaxValueF;
                if (_rightTriggerPressure < AxisMinValueF) _rightTriggerPressure = AxisMinValueF;
            }

            /// <summary>
            /// Retrieves the value stored in the left trigger.
            /// </summary>
            public float GetLeftTriggerPressure() { return _leftTriggerPressure; }


            /// <summary>
            /// Retrieves the value stored in the right trigger.
            /// </summary>
            public float GetRightTriggerPressure() { return _rightTriggerPressure; }
        }

        /// <summary>
        /// Defines the struct which declares if each of the buttons is pressed.
        /// </summary>
        public struct ControllerButtonStruct
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
            public byte ButtonA;

            /// <summary>
            /// Playstation: Circle, Nintendo: A
            /// </summary>
            public byte ButtonB;

            /// <summary>
            /// Playstation: Select, Nintendo: Select
            /// </summary>
            public byte ButtonBack;

            /// <summary>
            /// Playstation: PS Button, Nintendo: Home
            /// </summary>
            public byte ButtonGuide;

            /// <summary>
            /// Playstation: L1, Nintendo: L
            /// </summary>
            public byte ButtonLb;

            /// <summary>
            /// Playstation: L3, Nintendo: L Click
            /// </summary>
            public byte ButtonLs;

            /// <summary>
            /// Playstation: R1, Nintendo: R
            /// </summary>
            public byte ButtonRb;

            /// <summary>
            /// Playstation: R3, Nintendo: R Click 
            /// </summary>
            public byte ButtonRs;

            /// <summary>
            /// Playstation: Select, Nintendo: Start
            /// </summary>
            public byte ButtonStart;

            /// <summary>
            /// Playstation: Square, Nintendo: Y 
            /// </summary>
            public byte ButtonX;

            /// <summary>
            /// Playstation: Triangle, Nintendo: X
            /// </summary>
            public byte ButtonY;
        }

        /// <summary>
        /// Defines a struct specifying a custom mapping of buttons to the DPAD and axis for devices which do not
        /// support analog or POV-hat input such as keyboards and other potential peripherals.
        /// The emulated keys override the real keys if they are set.
        /// </summary>
        public class EmulationButtonMapping
        {
            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD DOWN if pressed.
            /// </summary>
            public byte DpadDown;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD LEFT if pressed.
            /// </summary>
            public byte DpadLeft;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD RIGHT if pressed.
            /// </summary>
            public byte DpadRight;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates DPAD UP if pressed.
            /// </summary>
            public byte DpadUp;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            public byte LeftStickDown;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            public byte LeftStickLeft;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            public byte LeftStickRight;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            public byte LeftStickUp;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left trigger if pressed.
            /// </summary>
            public byte LeftTrigger;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick down if pressed.
            /// </summary>
            public byte RightStickDown;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick left if pressed.
            /// </summary>
            public byte RightStickLeft;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick right if pressed.
            /// </summary>
            public byte RightStickRight;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates left analog stick up if pressed.
            /// </summary>
            public byte RightStickUp;

            /// <summary>
            /// For keyboards and other misc input devices. Simulates right trigger if pressed.
            /// </summary>
            public byte RightTrigger;
        }

        /// <summary>
        /// Defines all of the axis mappings for the individual custom controller.
        /// Used for mapping internal XInput and DirectInput axis to own custom defined axis.
        /// </summary>
        public class AxisMapping
        {
            public AxisMappingEntry LeftStickX = new AxisMappingEntry();
            public AxisMappingEntry LeftStickY = new AxisMappingEntry();
            public AxisMappingEntry LeftTrigger = new AxisMappingEntry();
            public AxisMappingEntry RightStickX = new AxisMappingEntry();
            public AxisMappingEntry RightStickY = new AxisMappingEntry();
            public AxisMappingEntry RightTrigger = new AxisMappingEntry();
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
            public ControllerAxis DestinationAxis;

            /// <summary>
            /// Defines a deadzone between 0 and 100%. Range: 0-100
            /// </summary>
            public float DeadZone;

            /// <summary>
            /// True if the axis is to be reversed when being read.
            /// </summary>
            public bool IsReversed;

            /// <summary>
            /// Stores the name of the property (DirectInput, XInput) that is mapped to the axis type.
            /// Sometimes known in the source code as PropertyName.
            /// </summary>
            public string SourceAxis;

            /// <summary>
            /// Scales the raw input values by this value.
            /// </summary>
            public float RadiusScale;
        }

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
                if (_x > AxisMaxValueF)
                    _x = AxisMaxValueF;
                else if (_x < AxisMinValueF) _x = AxisMinValueF;
            }


            /// <summary>
            /// Sets the value of X. Ensures that the value is within permissible range.
            /// </summary>
            /// <param name="value"></param>
            public void SetY(float value)
            {
                _y = value;

                // Do not allow X to be lesser or greater than man/mix.
                if (_y > AxisMaxValueF)
                    _y = AxisMaxValueF;
                else if (_y < AxisMinValueF) _y = AxisMinValueF;
            }
        }
    }
}
