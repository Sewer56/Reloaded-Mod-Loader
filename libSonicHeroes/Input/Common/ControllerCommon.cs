using SonicHeroes.Input.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Input
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
        /// Defines an interface for DirectInput & XInput Controller implementations which defines the function names
        /// and signatures to be shared between both the DirectInput and XInput controller implementations.
        /// </summary>
        public interface IController
        {
            /// <summary>
            /// Store the individual button mappings structure for this controller.
            /// </summary>
            Controller_Button_Mapping ButtonMapping { get; set; }

            /// <summary>
            /// Store the individual axis mappings structure for this controller.
            /// </summary>
            Controller_Axis_Mapping AxisMapping { get; set; }

            /// <summary>
            /// Defines the individual port used for this specific controller.
            /// </summary>
            int ControllerID { get; set; }

            /// <summary>
            /// Defines the custom botton mapping which simulates the individual axis and analog inputs.
            /// </summary>
            Emulation_Button_Mapping EmulationMapping { get; set; }

            /// <summary>
            /// Retrieves whether a specific button is pressed or not. 
            /// Accepts an enum of Controller_Buttons_Generic as parameter and returns the button ID mapped to
            /// the requested Controller_Buttons_Generic member of the "emulated" 360 pad.
            /// True if said button is pressed, else false.
            /// Note: The current controller state must first be manually updated.
            /// </summary>
            bool GetButtonState(Controller_Buttons_Generic button);

            /// <summary>
            /// Retrieves the specific intensity in terms of how far/deep an axis is pressed in.
            /// The return value should be a floating point number between -100 and 100 float.
            /// Note: The current controller state must first be manually updated.
            /// </summary>
            float GetAxisState(Controller_Axis_Generic axis);

            /// <summary>
            /// Retrieves all of the individual button states as an array of boolean values.
            /// True if a button is pressed, false if a button is not pressed.
            /// Note: The current controller state must first be manually updated.
            /// </summary>
            /// <returns></returns>
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
            void Remap_Axis(int timeoutSeconds, out float currentTimeout, Controller_Axis_Mapping_Entry mappingEntry);

            /// <summary>
            /// Waits for the user to press a button and retrieves the last pressed button. 
            /// Accepts any axis as input. Returns the read-in axis.
            /// </summary>
            /// <param name="timeoutSeconds">The timeout in seconds for the controller assignment.</param>
            /// <param name="currentTimeout">The current amount of time left in seconds, use this to update the GUI.</param>
            /// <param name="buttonToMap">Specififies the button variable where the index of the pressed button will be written to. Either a member of Controller_Button_Mapping or Emulation_Button_Mapping</param>
            void Remap_Buttons(int timeoutSeconds, out float currentTimeout, ref byte buttonToMap);

            /// <summary>
            /// Retrieves the state of the whole controller in question.
            /// Using a combination of GetAxisState and GetButton state
            /// </summary>
            Controller_Inputs GetControllerState();
        }

        /// <summary>
        /// To be used by the mod creator.
        /// Defines all of the individual controller inputs for a specific controller.
        /// </summary>
        public struct Controller_Inputs
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
            public Controller_Button_Struct controllerButtons;

            /// <summary>
            /// Sets the left trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetLeftTriggerPressure(float value)
            {
                leftTriggerPressure = value;
                if (leftTriggerPressure > DInputManager.AXIS_MAX_VALUE_F) { leftTriggerPressure = DInputManager.AXIS_MAX_VALUE_F; }
                if (leftTriggerPressure < DInputManager.AXIS_MIN_VALUE_F) { leftTriggerPressure = DInputManager.AXIS_MIN_VALUE_F; }
            }

            /// <summary>
            /// Sets the right trigger pressure such that it falls within the allowable bounds of 
            /// maximum and minimum acceptable value.
            /// </summary>
            public void SetRightTriggerPressure(float value)
            {
                rightTriggerPressure = value;
                if (rightTriggerPressure > DInputManager.AXIS_MAX_VALUE_F) { rightTriggerPressure = DInputManager.AXIS_MAX_VALUE_F; }
                if (rightTriggerPressure < DInputManager.AXIS_MIN_VALUE_F) { rightTriggerPressure = DInputManager.AXIS_MIN_VALUE_F; }
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
        public struct Controller_Button_Struct
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
        public struct Controller_Button_Mapping
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
        public struct Emulation_Button_Mapping
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
        public struct Controller_Axis_Mapping
        {
            public Controller_Axis_Mapping_Entry leftStickX;
            public Controller_Axis_Mapping_Entry leftStickY;
            public Controller_Axis_Mapping_Entry rightStickX;
            public Controller_Axis_Mapping_Entry rightStickY;
            public Controller_Axis_Mapping_Entry leftTrigger;
            public Controller_Axis_Mapping_Entry rightTrigger;
        }

        /// <summary>
        /// Defines an individual mapping entry for a controller axis as defined in Controller_Axis_Struct.
        /// Serves as a bridge to provide each axis with an individual 
        /// </summary>
        public struct Controller_Axis_Mapping_Entry
        {
            /// <summary>
            /// Defines the individual axis entry.
            /// </summary>
            public Controller_Axis_Generic axis;

            /// <summary>
            /// Stores the name of the property (DirectInput, XInput) that is mapped to the axis type.
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
        public enum Controller_Axis_Generic
        {
            Null,
            Left_Stick_X,
            Left_Stick_Y,
            Right_Stick_X,
            Right_Stick_Y,
            Left_Trigger_Pressure,
            Right_Trigger_Pressure
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
        /// Defines an analog stick in terms of X & Y, nothing more.
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
                if (X > DInputManager.AXIS_MAX_VALUE_F) { X = DInputManager.AXIS_MAX_VALUE_F; }
                else if (X < DInputManager.AXIS_MIN_VALUE_F) { X = DInputManager.AXIS_MIN_VALUE_F; }
            }


            /// <summary>
            /// Sets the value of X. Ensures that the value is within permissible range.
            /// </summary>
            /// <param name="value"></param>
            public void SetY(float value)
            {
                Y = value;

                // Do not allow X to be lesser or greater than man/mix.
                if (Y > DInputManager.AXIS_MAX_VALUE_F) { Y = DInputManager.AXIS_MAX_VALUE_F; }
                else if (Y < DInputManager.AXIS_MIN_VALUE_F) { Y = DInputManager.AXIS_MIN_VALUE_F; }
            }
        }

        /// <summary>
        /// Returns the value of a property of an object which shares the same name as the passed in string.
        /// e.g. joystickState with a string named AccelX would return a theoretical joystickState.AccelX
        /// </summary>
        public static object Reflection_GetValue(object sourceObject, string propertyName)
        {
            return sourceObject.GetType().GetProperty(propertyName).GetValue(sourceObject, null);
        }
    }
}
