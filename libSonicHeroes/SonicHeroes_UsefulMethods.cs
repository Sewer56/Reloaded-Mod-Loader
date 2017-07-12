using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonicHeroes.Memory;

namespace SonicHeroes.Programming.UtilityMethods
{
    /// <summary>
    /// This class defines methods which the user/modder may find useful when creating hacks.
    /// </summary>
    class SonicHeroes_UsefulMethods
    {

        /// <summary>
        /// This utility method will allow you to set a specified amount of bytes to 0x90/NOP in Assembly, disabling parts of code as wanted.
        /// </summary>
        /// <param name="NumberOfBytes">Number of bytes which you want to set to 0x90 (NOP/No Operation)</param>
        /// <param name="Address">The address at which the NOP Operation Starts with.</param>
        /// <param name="SonicHeroesProcess">The Process which holds the Sonic Heroes Game, generally Process.GetCurrentProcess()</param>
        public static void NukeBytes(int NumberOfBytes, uint Address, Process SonicHeroesProcess)
        {
            byte[] Null = new byte[1] { 0x90 };
            for (int x = 0; x < NumberOfBytes; x++)
            {
                SonicHeroesProcess.WriteMemory((IntPtr)Address + x, Null);
            }
        }
        

    }
}
