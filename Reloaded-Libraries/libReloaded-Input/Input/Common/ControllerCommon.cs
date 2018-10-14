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
using Reloaded.Input.Config.Substructures;
using Reloaded.Input.DirectInput;

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
    }
}
