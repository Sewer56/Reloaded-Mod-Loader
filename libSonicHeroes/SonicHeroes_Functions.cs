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
            /// The address to the instruction responsible for adding one to the ring count once a ring once a ring has been collected. The length represents the length of the instruction, nop this address for this length to disable instruction entirely.
            /// </summary>
            Add_Ring_Call = 0x00483366,
            Add_Ring_Call_Injection = 0x0048335C,
            Add_Ring_Call_Injection_Length = 16,
            Add_Ring_Call_Length = 50,

            /// <summary>
            /// The address to the instruction responsible for playing the sound for collecting a ring once a ring has been collected. The length represents the length of the instruction, nop this address for this length to disable instruction entirely.
            /// </summary>
            Add_Ring_PlaySound_Function = 0x00440A00,
            Add_Ring_PlaySound_Function_Length = 0x2A,
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

    }
}
