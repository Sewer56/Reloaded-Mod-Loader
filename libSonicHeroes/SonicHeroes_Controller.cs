using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SonicHeroes.Controller
{
    /// <summary>
    /// This class contains code to deal with DirectInput and dealing with individual controllers, acquiring input from the controllers outside of the realm of the game. I only have a DS4 to test, so I've made the class to support that at the current time. To use this class, you will need to install the SlimDX NuGet package.
    /// </summary>
    public class DirectInput_Joystick_Manager
    {
        // Variables
        private DirectInput DirectInputAdapter = new DirectInput(); // Declare the direct input adapter instance for storing the player gamepad

        /// <summary>
        /// This list holds all of the currently connected controllers. To refresh this, run GetConnectedControllers();
        /// </summary>
        public List<Sonic_Heroes_Joystick> PlayerControllers = new List<Sonic_Heroes_Joystick>();

        // Game Controller or Keyboard
        static JoystickState State; // The current state of the gamepad.

        /// <summary>
        /// Creates a DirectInput class for handling joysticks, requires SlimDX. Tested with a Dualshock 4. To get the buttons pressed, run Get_Joystickstate(), want it constantly? Set up a timer, best on another thread. To get joysticks manually if needed at any time, use GetConnectedControllers().
        /// </summary>
        public DirectInput_Joystick_Manager()
        {
            // Gets the available gamepads.
            GetConnectedControllers();
        }

        /// <summary>
        /// This will evaluate the currently available joysticks, if you have a controller with a possibility of disconnection, you might want implementing running of this from time to time.
        /// </summary>
        /// <returns>The joysticks</returns>
        public void GetConnectedControllers()
        {
            Sonic_Heroes_Joystick PlayerController; // Setup new array of all controllers.                          
            List<DeviceInstance> Devices = new List<DeviceInstance>(DirectInputAdapter.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly)); // Get all controllers and keyboards.
            // TODO: Keyboard support
            // Devices.AddRange(new List<DeviceInstance>(DirectInputAdapter.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AttachedOnly))); 
            int ControllerID = 1;

            // For each device/controller.
            foreach (DeviceInstance DirectInputDevice in Devices)
            {
                try
                {
                    // Acquire Device!
                    PlayerController = new Sonic_Heroes_Joystick(ControllerID, DirectInputAdapter, DirectInputDevice.InstanceGuid); // Assign the new PlayerController
                    PlayerController.Acquire(); // Acquire the current device.

                    // For each Device Object/Controller/Input type in the Direct Input Devices, if it contains an axis, set the range of the axis to -1000 - 1000 
                    foreach (DeviceObjectInstance DeviceObject in PlayerController.GetObjects())
                    {
                        if ((DeviceObject.ObjectId.Flags & DeviceObjectTypeFlags.Axis) != 0)
                        {
                            // DO NOT CHANGE -1000, 1000 without verifying that other pieces of code using this value have changed.
                            PlayerController.GetObjectPropertiesById(DeviceObject.ObjectId).Range = new SharpDX.DirectInput.InputRange(-1000, 1000);
                        }
                    }

                    ControllerID = ControllerID + 1;
                    try { PlayerController.Load_Controller_Configuration(); } catch { }
                    PlayerControllers.Add(PlayerController); // Add the PlayerController to recognized PlayerControllers.
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Returns the current state of a singular controller. The joystick object which you will want to pass through is in List<Joystick> PlayerControllers. This returns information for a generic DInput controller, use it how you want :)
        /// </summary>
        /// <param name="PlayerControllerX"></param>
        /// <param name="ControllerID"></param>
        public JoystickState Get_JoystickState_Generic(Sonic_Heroes_Joystick PlayerControllerX)
        {
            State = new JoystickState(); /// Assign a new JoystickState Object, such that the values of the last JoystickState are erased.
            State = PlayerControllerX.GetCurrentState(); /// Get the current state of the joysticks including all values etc.

            return State;
        }

        /// <summary>
        /// Intended for remapping to the game's input stack. Waits for the user to press a button and retrieves the index of the last pressed button, accepts any controller as input.
        /// </summary>
        /// <returns></returns>
        public byte Button_Mapping_GUI_Set_Button_With_Timeout_Any_Controller(int Timeout_Seconds)
        {
            try
            {
                /// Area to store all of the inputs for each individual controller in question.
                List<JoystickState> Controllers_Inputs = new List<JoystickState>(PlayerControllers.Count);
                /// List where the initial state of each button will be stored for each controller.
                List<bool[]> Joystick_Pressed_Button_List = new List<bool[]>(PlayerControllers.Count);

                /// Gets the joystick/input state for each joystick individually.
                /// Get the buttons pressed for each controller as is.
                for (int x = 0; x < PlayerControllers.Count; x++) { Controllers_Inputs.Add(Get_JoystickState_Generic(PlayerControllers[x])); }
                for (int x = 0; x < Controllers_Inputs.Count; x++) { Joystick_Pressed_Button_List.Add(Controllers_Inputs[x].Buttons); }

                // Joystick Initial Buttons: Stores all pressed buttons as bool[]

                /// Ayy lmao
                int Attempts = 0;
                int TimeLimit = Timeout_Seconds * 32;
                while (Attempts < TimeLimit) // Approximately 10 seconds.
                {
                    /// We will get our inputs, again!
                    /// Area to store all of the inputs for each individual controller in question.
                    List<JoystickState> Controllers_Inputs_New = new List<JoystickState>(PlayerControllers.Count);
                    List<bool[]> Joystick_Pressed_Button_List_New = new List<bool[]>(PlayerControllers.Count);

                    /// Gets the joystick/input state for each joystick individually.
                    /// Get the buttons pressed for each controller as is.
                    for (int x = 0; x < PlayerControllers.Count; x++) { Controllers_Inputs_New.Add(Get_JoystickState_Generic(PlayerControllers[x])); }
                    for (int x = 0; x < Controllers_Inputs.Count; x++) { Joystick_Pressed_Button_List_New.Add(Controllers_Inputs_New[x].Buttons); }

                    // Joystick Initial Buttons New: Stores all pressed buttons right now as bool[]

                    // Iterate over each entry containing all buttons
                    for (int x = 0; x < Joystick_Pressed_Button_List.Count; x++)
                    {
                        // Get the old pressed buttons and the new pressed buttons for each individual controller..
                        bool[] OldButtons = Joystick_Pressed_Button_List[x];
                        bool[] NewButtons = Joystick_Pressed_Button_List_New[x];

                        // Iterate over each button, return if there is no match.
                        for (int z = 0; z < OldButtons.Length; z++) { if (OldButtons[z] != NewButtons[z]) { return (byte)z; } }
                    }
                    Thread.Sleep(32);
                    Attempts++;
                }
                return 255;
            }
            catch (Exception Ex) { return 255; }
        }

        /// <summary>
        /// Intended for remapping to the game's input stack. Waits for the user to press a button and retrieves the index of the last pressed button, accepts any controller as input.
        /// </summary>
        /// <returns></returns>
        public byte Button_Mapping_GUI_Set_Button_With_Timeout(int Timeout_Seconds, int ControllerID, Sonic_Heroes_Joystick.Controller_Buttons_Generic Button_To_Assign)
        {
            try
            {
                // Get our controller with matching ID.
                // Controller ID will always match array Index. It is not implemented to serve purpose here, but plugins may want such variable, like swapping P1 and P2 or maybe adding P3 for control mods etc.
                Sonic_Heroes_Joystick GameController = PlayerControllers[ControllerID];
                // Get currently pressed buttons.
                JoystickState State_Original = GameController.GetCurrentState();

                /// Ayy lmao
                int Attempts = 0;
                int TimeLimit = Timeout_Seconds * 32;
                while (Attempts < TimeLimit) // Approximately 10 seconds.
                {
                    // Get current press state.
                    JoystickState State_New = GameController.GetCurrentState();

                    // Iterate over each entry containing all buttons
                    for (int x = 0; x < State_New.Buttons.Length; x++)
                    {
                        // If the state of any of the buttons changed, assign the button!
                        if (State_New.Buttons[x] != State_Original.Buttons[x])
                        {
                            /// Asisgn the appropriate button on the virtual 360 controller to our input button!
                            switch (Button_To_Assign)
                            {
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_A: PlayerControllers[ControllerID].Button_Mappings.Button_A = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_B: PlayerControllers[ControllerID].Button_Mappings.Button_B = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_X: PlayerControllers[ControllerID].Button_Mappings.Button_X = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_Y: PlayerControllers[ControllerID].Button_Mappings.Button_Y = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_L1: PlayerControllers[ControllerID].Button_Mappings.Button_L1 = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_R1: PlayerControllers[ControllerID].Button_Mappings.Button_R1 = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_L3: PlayerControllers[ControllerID].Button_Mappings.Button_L3 = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_R3: PlayerControllers[ControllerID].Button_Mappings.Button_R3 = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_Back: PlayerControllers[ControllerID].Button_Mappings.Button_Back = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Optional_Button_Guide: PlayerControllers[ControllerID].Button_Mappings.Optional_Button_Guide = (byte)x; break;
                                case Sonic_Heroes_Joystick.Controller_Buttons_Generic.Button_Start: PlayerControllers[ControllerID].Button_Mappings.Button_Start = (byte)x; break;
                            }

                            return (byte)x;
                        }
                    }
                    Thread.Sleep(32);
                    Attempts++;
                }
                return 255;
            }
            catch (Exception Ex) { return 255; }
        }

        /// <summary>
        /// Waits for the user to press a button and retrieves the index of the last pressed button, accepts any controller as input. Returns the axis and whether the image was negative or positive. True => Positive.
        /// </summary>
        /// <returns>The returned axis and whether the change was negative or positive. (True = Positive)</returns>
        public (Sonic_Heroes_Joystick.Controller_Axis_Generic, bool) Button_Mapping_GUI_Set_Axis_With_Timeout(int Timeout_Seconds, int ControllerID, Sonic_Heroes_Joystick.Controller_Axis_Generic Axis_To_Assign)
        {
            try
            {
                // Get our controller with matching ID.
                // Controller ID will always match array Index. It is not implemented to serve purpose here, but plugins may want such variable, like swapping P1 and P2 or maybe adding P3 for control mods etc.
                Sonic_Heroes_Joystick GameController = PlayerControllers[ControllerID];
                JoystickState State_Original = GameController.GetCurrentState();

                /// Ayy lmao
                int Attempts = 0;
                int TimeLimit = Timeout_Seconds * 32;
                while (Attempts < TimeLimit) // Approximately 10 seconds.
                {
                    // Retrieve new state for comparison/
                    JoystickState State_New = GameController.GetCurrentState();

                    // Check each axis individually now for the amount of change since the initial value! 250 = 25% | If axis moved more than 25%, this is what we want.
                    if ( State_Original.X - State_New.X < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.X); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.X, true); }
                    else if (State_Original.X - State_New.X > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.X); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.X, false); }

                    else if (State_Original.Y - State_New.Y < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.Y); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Y, true); }
                    else if (State_Original.Y - State_New.Y > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.Y); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Y, false); }

                    else if (State_Original.Z - State_New.Z < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.Z); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Z, true); }
                    else if (State_Original.Z - State_New.Z > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.Z); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Z, false); }

                    else if (State_Original.RotationX - State_New.RotationX < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_X); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_X, true); }
                    else if (State_Original.RotationX - State_New.RotationX > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_X); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_X, false); }

                    else if (State_Original.RotationY - State_New.RotationY < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Y); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Y, true); }
                    else if (State_Original.RotationY - State_New.RotationY > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Y); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Y, false); }

                    else if (State_Original.RotationZ - State_New.RotationZ < -250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, true, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Z); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Z, true); }
                    else if (State_Original.RotationZ - State_New.RotationZ > 250) { Set_Axis_Controller(ControllerID, Axis_To_Assign, false, Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Z); return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Z, false); }

                    Thread.Sleep(32);
                    Attempts++;
                }
                return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Null, true);
            }
            catch (Exception Ex) { return (Sonic_Heroes_Joystick.Controller_Axis_Generic.Null, true); }
        }

        /// <summary>
        /// Sets an axis to be inverse or not inverse, this simple.
        /// </summary>
        /// <param name="Axis_To_Be_Assigned"></param>
        /// <param name="Is_Inverse"></param>
        private void Set_Axis_Controller(int ControllerID, Sonic_Heroes_Joystick.Controller_Axis_Generic Axis_To_Be_Assigned, bool Is_Inverse, Sonic_Heroes_Joystick.Controller_Axis_Generic Axis_To_Assign)
        {
            switch (Axis_To_Be_Assigned)
            {
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.X: PlayerControllers[ControllerID].Axis_Mappings.LeftStick_X = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.LeftStick_X_IsReversed = Is_Inverse; break;
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.Y: PlayerControllers[ControllerID].Axis_Mappings.LeftStick_Y = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.LeftStick_Y_IsReversed = Is_Inverse; break;
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.Z: PlayerControllers[ControllerID].Axis_Mappings.LeftTrigger_Pressure = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.LeftTrigger_Pressure_IsReversed = Is_Inverse; break;
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Z: PlayerControllers[ControllerID].Axis_Mappings.RightTrigger_Pressure = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.RightTrigger_Pressure_IsReversed = Is_Inverse; break;
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_X: PlayerControllers[ControllerID].Axis_Mappings.RightStick_X = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.RightStick_X_IsReversed = Is_Inverse; break;
                case SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Axis_Generic.Rotation_Y: PlayerControllers[ControllerID].Axis_Mappings.RightStick_Y = Axis_To_Assign; PlayerControllers[ControllerID].Axis_Mappings.RightStick_Y_IsReversed = Is_Inverse; break;
            }
        }

    }

    /// <summary>
    /// An extension of the SharpDX 'Joystick' class, delivering specific properties for this project.
    /// </summary>
    public class Sonic_Heroes_Joystick : Joystick
    {
        public Sonic_Heroes_Joystick(int ControllerID, DirectInput directInput, Guid deviceGuid) : base(directInput, deviceGuid)
        {
            this.ControllerID = ControllerID;
        }

        /// <summary>
        /// Custom mapping for the generic buttons. Maps each regular button of an XBOX to a button ID custom defined by the user.
        /// </summary>
        public struct Controller_Button_Generic_Custom_Mapping
        {
            public byte Button_A;
            public byte Button_B;
            public byte Button_X;
            public byte Button_Y;
            public byte Button_L1;
            public byte Button_R1;
            public byte Button_Back;
            public byte Button_Start;
            public byte Button_L3;
            public byte Button_R3;

            public byte Optional_Button_Guide;
        }

        /// <summary>
        /// Custom mapping for the generic buttons. Maps each regular button of an XBOX to a button ID custom defined by the user.
        /// </summary>
        public struct Controller_Buttons_Struct
        {
            public bool Button_A;
            public bool Button_B;
            public bool Button_X;
            public bool Button_Y;
            public bool Button_L1;
            public bool Button_R1;
            public bool Button_Back;
            public bool Button_Start;
            public bool Button_L3;
            public bool Button_R3;
            public bool Optional_Button_Guide;
        }


        /// <summary>
        /// Reference for the controller buttons as in the ControllerButtons[] array for the PS4 controller. Use this for debugging.
        /// </summary>
        public enum Controller_Buttons_Generic
        {
            Button_A = 0x1,
            Button_B = 0x2,
            Button_X = 0x3,
            Button_Y = 0x4,
            Button_L1 = 0x5,
            Button_R1 = 0x6,
            Button_Back = 0x7,
            Button_Start = 0x8,
            Button_L3 = 0x9,
            Button_R3 = 0xA,

            Optional_Button_Guide = 0xB,
        }

        /// <summary>
        /// The struct to hold onto the current information about the joystick for which we are acquiring information for. It is designed with use on current commonplace controllers e.g. DS2/3/4, GCpad, X360 Controller etc. If you have something very specialized, consider modifying Get_JoystickState().
        /// </summary>
        public struct Controller_Inputs_Generic
        {
            public Analogue_Stick LeftStick; // Contains information on the left analogue stick.
            public Analogue_Stick RightStick; // Contains information on the right analogue stick.
            public short LeftTriggerPressure; // Pressure set on the left trigger.
            public short RightTriggerPressure; // Pressure set on the right trigger.
            public Controller_Buttons_Struct ControllerButtons; // The buttons that are currently pressed on the gamepad.
            public ushort ControllerDPad; // The DPAD buttons of the gamepad.
        }

        /// <summary>
        /// Reference for what these are: https://msdn.microsoft.com/en-us/library/windows/desktop/bb151904(v=vs.85).aspx | This is used for assigning controller axis to virtual 360 controller axis.
        /// </summary>
        public enum Controller_Axis_Generic
        {
            // DO NOT CHANGE THE NAME OF THIS AXIS WITHOUT CHECKING THE CONFIGURATOR FIRST!
            Null = 0x0, // Best axis ever
            X = 0x1, // Left Stick X
            Y = 0x2, // Left Stick Y

            Z = 0x3, // Left Trigger Pressure

            Rotation_X = 0x4, // Right Stick X
            Rotation_Y = 0x5, // Right Stick Y

            Rotation_Z = 0x6, // Right Trigger Pressure
        }

        /// <summary>
        /// Custom mapping for the generic axis. Maps each regular axis of an XBOX to a button ID custom defined by the user. IsReversed determines whether the axis expects a decreased value and the value is initially at 0 e.g. the value is in reality increased. *cough* 360 Controllers under DInput *cough*
        /// </summary>
        public struct Controller_Axis_Generic_Custom_Mapping
        {
            public Controller_Axis_Generic LeftStick_X; public bool LeftStick_X_IsReversed;
            public Controller_Axis_Generic LeftStick_Y; public bool LeftStick_Y_IsReversed;
            public Controller_Axis_Generic RightStick_X; public bool RightStick_X_IsReversed;
            public Controller_Axis_Generic RightStick_Y; public bool RightStick_Y_IsReversed;
            public Controller_Axis_Generic LeftTrigger_Pressure; public bool LeftTrigger_Pressure_IsReversed;
            public Controller_Axis_Generic RightTrigger_Pressure; public bool RightTrigger_Pressure_IsReversed;
        }

        /// <summary>
        /// Used for storing the analogue stick information, XYZ
        /// </summary>
        public struct Analogue_Stick
        {
            public short X;
            public short Y;
        }

        /// <summary>
        /// Values for the directional pad. These are the common values for a 8 directional DPAD.
        /// </summary>
        public enum DPAD_Direction
        {
            UP = 0,
            UP_RIGHT = 4500,
            UP_LEFT = 27000,

            RIGHT = 9000,
            LEFT = 27000,

            DOWN = 18000,
            DOWN_RIGHT = 13500,
            DOWN_LEFT = 22500,
        };

        /// <summary>
        /// ID of the current controller, which player?
        /// </summary>
        public int ControllerID;

        /// <summary>
        /// Store the individual button mappings for this controller.
        /// </summary>
        public Controller_Button_Generic_Custom_Mapping Button_Mappings;

        /// <summary>
        /// Store the individual axis mappings for this controller.
        /// </summary>
        public Controller_Axis_Generic_Custom_Mapping Axis_Mappings;

        /// <summary>
        /// Is this button pressed, is it not pressed? We will find out! This returns true if a button is pressed and false if it is not pressed.
        /// </summary>
        /// <param name="Controller_ID">Which connected controller ID are we talking about?</param>
        public bool Get_Button_State(Controller_Buttons_Generic Button)
        {
            int Button_To_Test = 255; // Button to check for true/false.

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (Button)
            {
                case Controller_Buttons_Generic.Button_A: Button_To_Test = Button_Mappings.Button_A; break;
                case Controller_Buttons_Generic.Button_B: Button_To_Test = Button_Mappings.Button_B; break;
                case Controller_Buttons_Generic.Button_X: Button_To_Test = Button_Mappings.Button_X; break;
                case Controller_Buttons_Generic.Button_Y: Button_To_Test = Button_Mappings.Button_Y; break;
                case Controller_Buttons_Generic.Button_L1: Button_To_Test = Button_Mappings.Button_L1; break;
                case Controller_Buttons_Generic.Button_R1: Button_To_Test = Button_Mappings.Button_R1; break;
                case Controller_Buttons_Generic.Button_R3: Button_To_Test = Button_Mappings.Button_R3; break;
                case Controller_Buttons_Generic.Button_L3: Button_To_Test = Button_Mappings.Button_L3; break;
                case Controller_Buttons_Generic.Button_Back: Button_To_Test = Button_Mappings.Button_Back; break;
                case Controller_Buttons_Generic.Button_Start: Button_To_Test = Button_Mappings.Button_Start; break;
                case Controller_Buttons_Generic.Optional_Button_Guide: Button_To_Test = Button_Mappings.Optional_Button_Guide; break;
            }

            // Get state of controller.
            JoystickState State = this.GetCurrentState();
            return State.Buttons[Button_To_Test];
        }

        /// <summary>
        /// Gets the state of an individual axis.
        /// </summary>
        /// <returns>The individual pressure/range/distance applied to the axis.</returns>
        public int Get_Axis_State(Controller_Axis_Generic Axis)
        {
            Controller_Axis_Generic Axis_To_Test = 0; // X Axis as temporary.
            bool IsAxisReversed = false; // Is the axis reversed?

            // Switch statement checks the mapping of each axis to verify where it points to. Then retrieves that axis!
            switch (Axis)
            {
                case Controller_Axis_Generic.X: Axis_To_Test = Axis_Mappings.LeftStick_X; if (Axis_Mappings.LeftStick_X_IsReversed) { IsAxisReversed = true; } break;
                case Controller_Axis_Generic.Y: Axis_To_Test = Axis_Mappings.LeftStick_Y; if (Axis_Mappings.LeftStick_Y_IsReversed) { IsAxisReversed = true; } break;

                case Controller_Axis_Generic.Z: Axis_To_Test = Axis_Mappings.LeftTrigger_Pressure; if (Axis_Mappings.LeftTrigger_Pressure_IsReversed) { IsAxisReversed = true; } break;
                case Controller_Axis_Generic.Rotation_Z: Axis_To_Test = Axis_Mappings.RightTrigger_Pressure; if (Axis_Mappings.RightTrigger_Pressure_IsReversed) { IsAxisReversed = true; } break;

                case Controller_Axis_Generic.Rotation_X: Axis_To_Test = Axis_Mappings.RightStick_X; if (Axis_Mappings.RightStick_X_IsReversed) { IsAxisReversed = true; } break;
                case Controller_Axis_Generic.Rotation_Y: Axis_To_Test = Axis_Mappings.RightStick_Y; if (Axis_Mappings.RightStick_Y_IsReversed) { IsAxisReversed = true; } break;
            }

            // If no pairing, return null.
            if (Axis_To_Test == 0) { return 0; }

            // Get state of controller.
            JoystickState State = this.GetCurrentState();

            // Now with the axis we want retrieved and the state, we will switch the axis we want and return it.
            int Value_To_Return = 0;

            // Return the appropriately mapped axis!
            switch (Axis_To_Test)
            {
                case Controller_Axis_Generic.X: Value_To_Return = State.X; break;
                case Controller_Axis_Generic.Y: Value_To_Return = State.Y; break;

                case Controller_Axis_Generic.Z: Value_To_Return = State.Z; break;
                case Controller_Axis_Generic.Rotation_Z: Value_To_Return = State.RotationZ; break;

                case Controller_Axis_Generic.Rotation_X: Value_To_Return = State.RotationX; break;
                case Controller_Axis_Generic.Rotation_Y: Value_To_Return = State.RotationY; break;
            }

            // Adjust the value if the axis is reversed (This is pretty much a modulus operation lol).
            if (IsAxisReversed) { Value_To_Return = 1000 - (Value_To_Return + 1000); }

            /// 360 Controller, Left Trigger: State.Z;
            /// 360 Controller, Right Trigger: 1000 - (State.Z + 1000);

            // Return
            return Value_To_Return;
        }

        /// <summary>
        /// Retrieves the entire controller and all of the current inputs. Unless you want to interact with plenty of buttons and axis at once, it makes no sense to use this method due to 'technical' performance implications (although negligible). When running the controller input server on the mod loader itself, this is used and values are grabbed from the results of this.
        /// </summary>
        /// <returns></returns>
        public Controller_Inputs_Generic Get_Whole_Controller_State()
        {
            Controller_Inputs_Generic Current_Controller_All = new Controller_Inputs_Generic(); // Store total controller state here.           
            JoystickState State = this.GetCurrentState(); // Get state of controller.

            // Axis
            Current_Controller_All.LeftStick.X = Get_Axis_Value(Axis_Mappings.LeftStick_X, State, Axis_Mappings.LeftStick_X_IsReversed);
            Current_Controller_All.LeftStick.Y = Get_Axis_Value(Axis_Mappings.LeftStick_Y, State, Axis_Mappings.LeftStick_Y_IsReversed);

            Current_Controller_All.RightStick.X = Get_Axis_Value(Axis_Mappings.RightStick_X, State, Axis_Mappings.RightStick_X_IsReversed);
            Current_Controller_All.RightStick.Y = Get_Axis_Value(Axis_Mappings.RightStick_Y, State, Axis_Mappings.RightStick_Y_IsReversed);

            Current_Controller_All.LeftTriggerPressure = Get_Axis_Value(Axis_Mappings.LeftTrigger_Pressure, State, Axis_Mappings.LeftTrigger_Pressure_IsReversed);
            Current_Controller_All.RightTriggerPressure = Get_Axis_Value(Axis_Mappings.RightTrigger_Pressure, State, Axis_Mappings.RightTrigger_Pressure_IsReversed);

            // Buttons!
            Current_Controller_All.ControllerButtons.Button_A = Get_Button_State_Internal(Controller_Buttons_Generic.Button_A, State);
            Current_Controller_All.ControllerButtons.Button_B = Get_Button_State_Internal(Controller_Buttons_Generic.Button_B, State);
            Current_Controller_All.ControllerButtons.Button_X = Get_Button_State_Internal(Controller_Buttons_Generic.Button_X, State);
            Current_Controller_All.ControllerButtons.Button_Y = Get_Button_State_Internal(Controller_Buttons_Generic.Button_Y, State);

            Current_Controller_All.ControllerButtons.Button_L1 = Get_Button_State_Internal(Controller_Buttons_Generic.Button_L1, State);
            Current_Controller_All.ControllerButtons.Button_R1 = Get_Button_State_Internal(Controller_Buttons_Generic.Button_R1, State);
            Current_Controller_All.ControllerButtons.Button_R3 = Get_Button_State_Internal(Controller_Buttons_Generic.Button_R3, State);
            Current_Controller_All.ControllerButtons.Button_L3 = Get_Button_State_Internal(Controller_Buttons_Generic.Button_L3, State);

            Current_Controller_All.ControllerButtons.Button_Back = Get_Button_State_Internal(Controller_Buttons_Generic.Button_Back, State);
            Current_Controller_All.ControllerButtons.Optional_Button_Guide = Get_Button_State_Internal(Controller_Buttons_Generic.Optional_Button_Guide, State);
            Current_Controller_All.ControllerButtons.Button_Start = Get_Button_State_Internal(Controller_Buttons_Generic.Button_Start, State);

            // DPAD
            Current_Controller_All.ControllerDPad = (ushort)State.PointOfViewControllers[0];

            return Current_Controller_All; // Return final Controller state.
        }

        /// <summary>
        /// Is this button pressed, is it not pressed? We will find out! This returns true if a button is pressed and false if it is not pressed.
        /// </summary>
        /// <param name="Controller_ID">Which connected controller ID are we talking about?</param>
        public bool Get_Button_State_Internal(Controller_Buttons_Generic Button, JoystickState State)
        {
            int Button_To_Test = 255; // Button to check for true/false.

            // Switch statement checks the mapping of each button to the virtual xbox button.
            switch (Button)
            {
                case Controller_Buttons_Generic.Button_A: Button_To_Test = Button_Mappings.Button_A; break;
                case Controller_Buttons_Generic.Button_B: Button_To_Test = Button_Mappings.Button_B; break;
                case Controller_Buttons_Generic.Button_X: Button_To_Test = Button_Mappings.Button_X; break;
                case Controller_Buttons_Generic.Button_Y: Button_To_Test = Button_Mappings.Button_Y; break;
                case Controller_Buttons_Generic.Button_L1: Button_To_Test = Button_Mappings.Button_L1; break;
                case Controller_Buttons_Generic.Button_R1: Button_To_Test = Button_Mappings.Button_R1; break;
                case Controller_Buttons_Generic.Button_R3: Button_To_Test = Button_Mappings.Button_R3; break;
                case Controller_Buttons_Generic.Button_L3: Button_To_Test = Button_Mappings.Button_L3; break;
                case Controller_Buttons_Generic.Button_Back: Button_To_Test = Button_Mappings.Button_Back; break;
                case Controller_Buttons_Generic.Button_Start: Button_To_Test = Button_Mappings.Button_Start; break;
                case Controller_Buttons_Generic.Optional_Button_Guide: Button_To_Test = Button_Mappings.Optional_Button_Guide; break;
            }
            return State.Buttons[Button_To_Test];
        }

        /// <summary>
        /// Returns the value of the requested axis. Axis_To_Test is the axis from the Axis_Mappings table, IsAxisReversed is whether the Axis is reversed, obtained from the same table.
        /// </summary>
        /// <param name="Axis_To_Test"></param>
        /// <param name="State"></param>
        /// <param name="IsAxisReversed"></param>
        /// <returns></returns>
        private short Get_Axis_Value(Controller_Axis_Generic Axis_To_Test, JoystickState State, bool IsAxisReversed)
        {
            short Value_To_Return = 0;
            switch (Axis_To_Test)
            {
                case Controller_Axis_Generic.X: Value_To_Return = (short)State.X; break;
                case Controller_Axis_Generic.Y: Value_To_Return = (short)State.Y; break;

                case Controller_Axis_Generic.Z: Value_To_Return = (short)State.Z; break;
                case Controller_Axis_Generic.Rotation_Z: Value_To_Return = (short)State.RotationZ; break;

                case Controller_Axis_Generic.Rotation_X: Value_To_Return = (short)State.RotationX; break;
                case Controller_Axis_Generic.Rotation_Y: Value_To_Return = (short)State.RotationY; break;
            }

            // Adjust the value if the axis is reversed (This is pretty much a modulus operation lol).
            if (IsAxisReversed) { Value_To_Return = (short)(1000 - (Value_To_Return + 1000)); }

            return Value_To_Return;
        }

        /// <summary>
        /// Verifies whether the DPAD is pressed pointing towards a specific direction. If it is, returns true.
        /// </summary>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool Get_Verify_Current_DPAD_Direction(DPAD_Direction Direction)
        {
            // Get state of controller.
            JoystickState State = this.GetCurrentState();
            
            // Get direction of DPAD
            int DPAD_Direction = State.PointOfViewControllers[0];

            if (DPAD_Direction == (int)Direction) { return true; } else { return false; }
            
            return false;
        }

        /// <summary>
        /// Saves the controller configuration to a file. Expects the current directory to be the directory of the game's executable, which should always be the case as long as the tool is located in the same folder or is injected into the game.
        /// </summary>
        public void Save_Controller_Configuration()
        {
            try
            {
                // Sonic Heroes Directory + Mod-Loader-Config + GUID
                string Save_Setting_Path = Environment.CurrentDirectory + @"\Mod-Loader-Config\\" + this.Information.ProductName + "-" + this.Information.ProductGuid + ".cc";
                List<string> Configuration_Text_File = new List<string>(30);

                /// Dump Axis Mappings!
                Configuration_Text_File.Add("# Sonic Heroes Mod Loader Input Stack Configuration for " + this.Information.ProductName + " | GUID: " + this.Information.ProductGuid);
                // Write Buttons
                Configuration_Text_File.Add("\n# Buttons");
                Configuration_Text_File.Add("Button_A=" + Button_Mappings.Button_A);
                Configuration_Text_File.Add("Button_B=" + Button_Mappings.Button_B);
                Configuration_Text_File.Add("Button_X=" + Button_Mappings.Button_X);
                Configuration_Text_File.Add("Button_Y=" + Button_Mappings.Button_Y);
                Configuration_Text_File.Add("Button_LB=" + Button_Mappings.Button_L1);
                Configuration_Text_File.Add("Button_RB=" + Button_Mappings.Button_R1);
                Configuration_Text_File.Add("Button_LS=" + Button_Mappings.Button_L3);
                Configuration_Text_File.Add("Button_RS=" + Button_Mappings.Button_R3);
                Configuration_Text_File.Add("Button_Back=" + Button_Mappings.Button_Back);
                Configuration_Text_File.Add("Button_Guide=" + Button_Mappings.Optional_Button_Guide);
                Configuration_Text_File.Add("Button_Start=" + Button_Mappings.Button_Start);
                // Write Axis
                Configuration_Text_File.Add("\n# Axis");
                Configuration_Text_File.Add("Axis_X=" + Axis_Mappings.LeftStick_X);
                Configuration_Text_File.Add("Axis_Y=" + Axis_Mappings.LeftStick_Y);
                Configuration_Text_File.Add("Axis_Z=" + Axis_Mappings.LeftTrigger_Pressure);
                Configuration_Text_File.Add("Axis_Rotation_X=" + Axis_Mappings.RightStick_X);
                Configuration_Text_File.Add("Axis_Rotation_Y=" + Axis_Mappings.RightStick_Y);
                Configuration_Text_File.Add("Axis_Rotation_Z=" + Axis_Mappings.RightTrigger_Pressure);
                // Axis Orientation
                Configuration_Text_File.Add("\n# Axis_Orientation");
                Configuration_Text_File.Add("Axis_X_IsReversed=" + Axis_Mappings.LeftStick_X_IsReversed);
                Configuration_Text_File.Add("Axis_Y_IsReversed=" + Axis_Mappings.LeftStick_Y_IsReversed);
                Configuration_Text_File.Add("Axis_Z_IsReversed=" + Axis_Mappings.LeftTrigger_Pressure_IsReversed);
                Configuration_Text_File.Add("Axis_Rotation_X_IsReversed=" + Axis_Mappings.RightStick_X_IsReversed);
                Configuration_Text_File.Add("Axis_Rotation_Y_IsReversed=" + Axis_Mappings.RightStick_Y_IsReversed);
                Configuration_Text_File.Add("Axis_Rotation_Z_IsReversed=" + Axis_Mappings.RightTrigger_Pressure_IsReversed);
                File.WriteAllLines(Save_Setting_Path, Configuration_Text_File);
            }
            catch (Exception Ex) { }
        }

        /// <summary>
        /// Loads the controller configuration from a file. Expects the current directory to be the directory of the game's executable, which should always be the case as long as the tool is located in the same folder or is injected into the game.
        /// </summary>
        public void Load_Controller_Configuration()
        {
            try
            {
                string Save_Seting_Path = Environment.CurrentDirectory + @"\Mod-Loader-Config\\" + this.Information.ProductName + "-" + this.Information.ProductGuid + ".cc";
                string Local_Save_Path = Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location) + "\\" + this.Information.ProductName + "-" + this.Information.ProductGuid + ".cc";
                
                if (!File.Exists(Save_Seting_Path))
                {
                    try { Save_Seting_Path = File.ReadAllText(Environment.CurrentDirectory + "\\Mod_Loader_Config.txt") + @"\Mod-Loader-Config\\" + this.Information.ProductName + "-" + this.Information.ProductGuid + ".cc"; }
                    catch { }
                }

                if (File.Exists(Local_Save_Path)) { Save_Seting_Path = Local_Save_Path; }

                string[] Save_File = File.ReadAllLines(Save_Seting_Path);

                // I never use foreach but I'll give in once, it looks cleaner.
                foreach (string Save_File_String in Save_File)
                {
                    string Value = Save_File_String.Substring(Save_File_String.IndexOf("=") + 1);
                    // Ignore comments
                    if (Save_File_String.StartsWith("#")) { continue; }
                    else if (Save_File_String.StartsWith("Button_A=")) { Button_Mappings.Button_A = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_B=")) { Button_Mappings.Button_B = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_X=")) { Button_Mappings.Button_X = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_Y=")) { Button_Mappings.Button_Y = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_LB=")) { Button_Mappings.Button_L1 = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_RB=")) { Button_Mappings.Button_R1 = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_LS=")) { Button_Mappings.Button_L3 = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_RS=")) { Button_Mappings.Button_R3 = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_Back=")) { Button_Mappings.Button_Back = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_Guide=")) { Button_Mappings.Optional_Button_Guide = Byte.Parse(Value); }
                    else if (Save_File_String.StartsWith("Button_Start=")) { Button_Mappings.Button_Start = Byte.Parse(Value); }

                    else if (Save_File_String.StartsWith("Axis_X=")) { Axis_Mappings.LeftStick_X = Get_Enumerable_Axis(Value); }
                    else if (Save_File_String.StartsWith("Axis_Y=")) { Axis_Mappings.LeftStick_Y = Get_Enumerable_Axis(Value); }
                    else if (Save_File_String.StartsWith("Axis_Z=")) { Axis_Mappings.LeftTrigger_Pressure = Get_Enumerable_Axis(Value); }
                    else if (Save_File_String.StartsWith("Axis_Rotation_X=")) { Axis_Mappings.RightStick_X = Get_Enumerable_Axis(Value); }
                    else if (Save_File_String.StartsWith("Axis_Rotation_Y=")) { Axis_Mappings.RightStick_Y = Get_Enumerable_Axis(Value); }
                    else if (Save_File_String.StartsWith("Axis_Rotation_Z=")) { Axis_Mappings.RightTrigger_Pressure = Get_Enumerable_Axis(Value); }

                    else if (Save_File_String.StartsWith("Axis_X_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.LeftStick_X_IsReversed = Result; }
                    else if (Save_File_String.StartsWith("Axis_Y_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.LeftStick_Y_IsReversed = Result; }
                    else if (Save_File_String.StartsWith("Axis_Z_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.LeftTrigger_Pressure_IsReversed = Result; }
                    else if (Save_File_String.StartsWith("Axis_Rotation_X_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.RightStick_X_IsReversed = Result; }
                    else if (Save_File_String.StartsWith("Axis_Rotation_Y_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.RightStick_Y_IsReversed = Result; }
                    else if (Save_File_String.StartsWith("Axis_Rotation_Z_IsReversed")) { bool Result = false; Boolean.TryParse(Value, out Result); Axis_Mappings.RightTrigger_Pressure_IsReversed = Result; }
                }
            }
            catch (Exception Ex) { }
        }

        /// <summary>
        /// Returns an axis from the axis generic enumerables with the name equal to the submitted string.
        /// </summary>
        public Controller_Axis_Generic Get_Enumerable_Axis(string Axis_Name)
        {
            foreach (Controller_Axis_Generic Gamepad_Axis in Enum.GetValues(typeof(Controller_Axis_Generic))) 
            {
                string Enumerable_Name = Enum.GetName(typeof(Controller_Axis_Generic), Gamepad_Axis);
                if (Enumerable_Name == Axis_Name) { return Gamepad_Axis; }
            }
            return Controller_Axis_Generic.Null;
        }
    }

    /// <summary>
    /// Some of the stuff I wanted to remove but didn't, but then other parts I did, IDK.
    /// </summary>
    class Debug
    {
        /// <summary>
        /// Reference for the controller buttons as in the ControllerButtons[] array for the PS4 controller. Use this for debugging.
        /// </summary>
        public enum Controller_Buttons_DS4
        {
            Button_Square = 0x1,
            Button_Cross = 0x2,
            Button_Circle = 0x3,
            Button_Triangle = 0x4,
            Button_L1 = 0x5,
            Button_R1 = 0x6,
            Trigger_L2 = 0x7,
            Trigger_R2 = 0x8,
            Button_Share = 0x9,
            Button_Start = 0xA,
            Button_L3 = 0xB,
            Button_R3 = 0xC,
            Button_Playstation = 0xD,
            Button_Touchpad = 0xE
        }
    }

 }
