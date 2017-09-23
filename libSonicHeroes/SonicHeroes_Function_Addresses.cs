using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Functions
{
    public class SonicHeroes_Functions
    {
        /// <summary>
        /// Set of addresses where the necessary checks to load Team Super members are performed.
        /// </summary>
        public enum Check_TeamSuper
        {
            /// <summary>
            /// Checks if the apppropriate flag for Super Sonic is set. Replace two bytes with 90 90 (nop, nop) to always load SS.
            /// </summary>
            Check_If_SuperSonic = 0x5CBFE6,

            /// <summary>
            /// Checks if the apppropriate flag for Super Tails is set. Replace two bytes with 90 90 (nop, nop) to always load SS.
            /// </summary>
            Check_If_SuperTails = 0x5B7ED2,

            /// <summary>
            /// Checks if the apppropriate flag for Super Knuckles is set. Replace two bytes with 90 90 (nop, nop) to always load SS.
            /// </summary>
            Check_If_SuperKnuckles = 0x5C1D72,
        }

        /// <summary>
        /// Set of addresses related to ring collection ingame.
        /// </summary>
        public enum Collect_Ring
        {
            /// <summary>
            /// This address' function adds a single ring to the counter of rings.
            /// </summary>
            Add_Ring_Function = 0x00423B26,

            /// <summary>
            /// The start of the function executed when the ring is collected.
            /// </summary>
            Collect_Ring_Function = 0x004832F0,

            /// <summary>
            /// [DO NOT USE WITH INJECTION HOOK] The address to the instruction responsible for adding one to the ring count once a ring once a ring has been collected. The length represents the length of the instruction, nop this address for this length to disable instruction entirely.
            /// </summary>
            Add_Ring_Call = 0x00483366,
            /// <summary>
            /// Use this one for injection hook! The address to the instruction responsible for adding one to the ring count once a ring once a ring has been collected.
            /// </summary>
            Add_Ring_Call_Injection = 0x0048335C,
            Add_Ring_Call_Injection_Length = 16,
            Add_Ring_Call_Length = 50,

            /// <summary>
            /// The address to the instruction responsible for playing the sound for collecting a ring once a ring has been collected. The length represents the length of the instruction, nop this address for this length to disable instruction entirely.
            /// </summary>
            Add_Ring_PlaySound_Function = 0x00440A00,

            /// <summary>
            /// The specific JMP/Call responsible for calling the routine which will play a sound for a ring.
            /// </summary>
            Add_Ring_PlaySound_Function_Call = 0x004833F6,

            /// <summary>
            /// [ Byte [2] ] : Address: allows you to change the sound played when collecting a ring. 1st byte = BankID, 2nd byte = Sound ID
            /// </summary>
            Add_Ring_SoundEffect_BankID_TwoBytes_Sound = 0x440a10, // 2nd byte | BankID, 1st byte | Sound ID

            /// <summary>
            /// This instruction removes the ring once it has been collected.
            /// </summary>
            Remove_Ring_After_Collect = 0x00483320,
            Remove_Ring_After_Collect_Length = 0x17,
        }

        /// <summary>
        /// This function plays any specified sound effect from a .pac archive of sound effects.
        /// </summary>
        public enum Sound_PlaySoundBank
        {
            /// <summary>
            /// [ Arguments: BankID (Byte), SoundID (Dword) ] 
            /// </summary>
            Play_Sound_Bank_Subroutine = 0x004405F0, // 2 bytes | parameter
        }

        /// <summary>
        /// This is the subroutine responsible for rendering a frame to the screen.
        /// </summary>
        public enum Render_A_Frame
        {
            Render_Frame_Subroutine_Start = 0x00445E90,
        }

        /// <summary>
        /// Functions which control the game's internal controller handler.
        /// </summary>
        public enum Controller_Polling_Functions
        {
            /// <summary>
            /// Nop or remove all calls to this function to completely disable everything controller related in Sonic Heroes.
            /// </summary>
            Master_Function_Control_Handler = 0x0044315E,

            /// <summary>
            /// Runs Master_Function_Control_Handler, Nop the instruction here for safe controller disabling.
            /// </summary>
            Master_Function_Control_OnFrame_Call = 0x0044315E,

            /// <summary>
            /// Nopping this function call will call the game to recognize no pressed buttons or axis, alternative safe way for control disabling.
            /// </summary>
            Function_Call_Controller_Get_Pressed_Buttons_Axis = 0x00435420,

            /// <summary>
            /// Disabling Mouse Controls = Mwah!
            /// </summary>
            Function_Get_Mouse_Controls = 0x00445A3E,
            Function_Get_Mouse_Controls_Length = 0xFC,

            /// <summary>
            /// [Float] Left Stick X Value, from -1 to 1.
            /// </summary>
            Variable_Controller_Left_Stick_X = 0x00A2F958,

            /// <summary>
            /// [Float] Left Stick Y Value, from -1 to 1.
            /// </summary>
            Variable_Controller_Left_Stick_Y = 0x00A2F95C,

            /// <summary>
            /// [Float] Right Stick X Value, from -1 to 1.
            /// </summary>
            Variable_Controller_Right_Stick_X = 0x00A2F960,

            /// <summary>
            /// [Float] Right Stick Y Value, from -1 to 1.
            /// </summary>
            Variable_Controller_Right_Stick_Y = 0x00A2F964,

            /// <summary>
            /// [Byte] The pressure applied to the right trigger. Controls camera rotation. From 0 to 255
            /// </summary>
            Variable_Controller_Right_Trigger_Pressure = 0x00A23A7A,

            /// <summary>
            /// [Byte] The pressure applied to the right trigger. Controls camera rotation. From 0 to 255
            /// </summary>
            Variable_Controller_Right_Trigger_Pressure_II = 0x00A236D2,

            /// <summary>
            /// [Byte] The pressure applied to the left trigger. Controls camera rotation. From 0 to 255
            /// </summary>
            Variable_Controller_Left_Trigger_Pressure = 0x00A23A78,

            /// <summary>
            /// [Byte] The pressure applied to the left trigger. Controls camera rotation. From 0 to 255
            /// </summary>
            Variable_Controller_Left_Trigger_Pressure_II = 0x00A236D0,

            /// <summary>
            /// [Byte] Various flags representing buttons flags in enum Buttons_Flags_I
            /// </summary>
            Variable_Controller_Buttons_Flags_I = 0x00A2F948,

            /// <summary>
            /// [Byte] Various flags representing buttons, flags in enum Buttons_Flags_II
            /// </summary>
            Variable_Controller_Buttons_Flags_II = 0x00A2F949,

            /// <summary>
            /// [Byte] Various flags representing buttons flags in enum Buttons_Flags_III
            /// </summary>
            Variable_Controller_Buttons_Flags_III = 0x00A2F94A,

            /// <summary>
            /// [Byte] Various flags representing buttons flags. No known buttons.
            /// </summary>
            Variable_Controller_Buttons_Flags_IV = 0x00A2F94B,
        }

        /// <summary>
        /// Flags for Variable_Controller_Buttons_Flags_I
        /// </summary>
        [Flags]
        public enum Buttons_Flags_I
        {
            Jump = 0x01,
            Formation_R = 0x02,
            Action = 0x04,
            Formation_L = 0x08,
            DPAD_UP = 0x10,
            DPAD_DOWN = 0x20,
            DPAD_LEFT = 0x40,
            DPAD_RIGHT = 0x80,
        }

        /// <summary>
        /// Flags for Variable_Controller_Buttons_Flags_II
        /// </summary>
        [Flags]
        public enum Buttons_Flags_II
        {
            Right_Trigger = 0x01,
            Left_Trigger = 0x02,
            Start_Button = 0x40,
        }

        /// <summary>
        /// Flags for Variable_Controller_Buttons_Flags_II
        /// </summary>
        [Flags]
        public enum Buttons_Flags_III
        {
            Select = 0x01,
        }

    }
}
