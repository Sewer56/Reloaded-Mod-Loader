using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SonicHeroes.Networking.MessageTypes
{
    /// <summary>
    /// Defines the different individual message types accepted by the Mod Loader Server.
    /// </summary>
    public static class ModLoader
    {
        /// <summary>
        /// Defines the individual message types that can be sent towards the mod loader server.
        /// It's recommended that you actually go to the definition of this in your IDE, everything is nicely formatted there.
        /// </summary>
        public enum Message_Type
        {
            /// <summary>
            /// Expects Response:   False
            /// Definition:         Confirmation of operation being performed successfully.
            /// </summary>
            Okay = 0x0,
            
            /// <summary>
            /// Expects Response:   True/False
            /// Definition:         Prints message to mod loader's command line. 
            /// Data:               ASCII Encoded String, e.g. Encoding.ASCII.GetBytes("Ayylmao");
            /// </summary>
            Client_Call_Send_Message = 0x1,

            /// <summary>
            /// Expects Response:   True 
            /// Definition:         Assembles your x86 mnemonics sent to the mod loader server. Powered by FASM.NET.
            /// Data:               Returns the bytes representing the x86 mnemonics given. 
            /// </summary>
            Client_Call_Assemble_x86_Mnemonics = 0x2,
        }

        /// <summary>
        /// Serializes a string array of X86 Mnemonics such that they may be transmitted to the loader server.
        /// </summary>
        /// <param name="mnemonics">
        ///     Your x86 assembler instructions to be assembled. 
        ///     Rule of thumb: Test your ASM in FASM outside of mod loader mods first for successful compilation.
        ///     Don't forget use32!
        /// </param>
        public static byte[] SerializeX86Mnemonics(string[] mnemonics)
        {
            // Initialize MemStream & BinaryFormatter
            MemoryStream MnemonicStream = new MemoryStream();
            BinaryFormatter BinaryFormatter_X = new BinaryFormatter();

            // Serialize array at once
            BinaryFormatter_X.Serialize(MnemonicStream, mnemonics);

            // Return Serialized
            return MnemonicStream.ToArray();
        }

        /// <summary>
        /// Deserializes a string array of X86 Mnemonics that was received from another machine.
        /// </summary>
        /// <param name="mnemonics">
        ///     Your x86 assembler instructions to be assembled. 
        ///     Rule of thumb: Test your ASM in FASM outside of mod loader mods first for successful compilation.
        ///     Don't forget use32!
        /// </param>
        public static string[] DeserializeX86Mnemonics(byte[] Mnemonics)
        {
            // Initialize MemStream & BinaryFormatter
            BinaryFormatter BinaryFormatter_X = new BinaryFormatter();
            MemoryStream MnemonicStream = new MemoryStream(Mnemonics);

            // Return deserialized.
            return (string[])BinaryFormatter_X.Deserialize(MnemonicStream);
        }
    }
}
