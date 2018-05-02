using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Reloaded;
using Reloaded.Assembler;
using Reloaded.Native.Helpers.Arrays;
using Reloaded.Process;
using Reloaded.Process.Memory;

namespace Reloaded_Mod_Template
{
    public static class Program
    {
        #region Reloaded Mod Template Stuff
        /// <summary>
        /// Holds the game process for us to manipulate.
        /// Allows you to read/write memory, perform pattern scans, etc.
        /// See libReloaded/GameProcess (folder)
        /// </summary>
        public static ReloadedProcess GameProcess;

        /// <summary>
        /// Stores the absolute executable location of the currently executing game or process.
        /// </summary>
        public static string ExecutingGameLocation;

        /// <summary>
        /// Specifies the full directory location that the current mod 
        /// is contained in.
        /// </summary>
        public static string ModDirectory;
        #endregion Reloaded Mod Template Stuff

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: Reloaded-Assembler
                Architectures supported: X86, X64

                Demonstrates the simple usage of ReloadedAssembler, 
                a background service wrapped around FASM.NET.

                ReloadedAssembler provides you a means to effortlessly 
                generate and compile X86 and X64 assembly in a 
                real-time, just-in-time fashion.
            */

            // Want to see this in with a debugger? Uncomment this line.
            // Debugger.Launch();

            // Print demo details.
            Bindings.PrintInfo("Reloaded-Assembler Demo: 64bit Assembly");
            Bindings.PrintText("X64 Test:");

            // Sample assembler.
            string[] x64asm = new[]
            {
                "use64",                // Specifies this is a X64 piece of ASM.
                "mov rbx, rax",     
                "mov rax, 0x123456",
                "jmp qword [0x123456]"  // This is FASM, YOU MUST SPECIFY OPERAND SIZE EXPLICITLY (`qword` in this case)
            };

            Bindings.PrintInfo("Assembling\n" + ConvertStringArrayToString(x64asm));

            // Assemble and print result.
            byte[] resultx64 = Assembler.Assemble(x64asm);
            Bindings.PrintInfo("Result:" + ByteArrayToString(resultx64));

            // Sample assembler.
            Bindings.PrintText("X86 Test:");
            string[] x86asm = new[]
            {
                "use32",                // Specifies this is a X86 piece of ASM.
                "mov ebx, eax",
                "mov eax, 0x123456",
                "jmp dword [0x123456]"  // This is FASM, YOU MUST SPECIFY OPERAND SIZE EXPLICITLY (`dword` in this case)
            };

            Bindings.PrintInfo("Assembling\n" + ConvertStringArrayToString(x86asm));

            // Assemble and print result.
            byte[] resultx86 = Assembler.Assemble(x86asm);
            Bindings.PrintInfo("Result:" + ByteArrayToString(resultx86));

            // Credits
            Bindings.PrintText("Protip: When using Reloaded it is highly recommended to first test your assembler for compilation" +
                               "with standalone FASM assembler.");
            Bindings.PrintText("Credits to Zenlulz for FASM.NET, Tomasz Grysztar for FASM and FASMlib. Networked background service" +
                               "version is a port by myself.");
        }

        /// <summary>
        /// Converts a string array into a string delimited by newlines.
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        static string ConvertStringArrayToString(string[] stringArray)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in stringArray)
            {
                builder.Append(value);
                builder.Append('\n');
            }
            return builder.ToString();
        }

        /// <summary>
        /// Converts a byte array into a string.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);

            foreach (byte b in byteArray)
                hex.AppendFormat("{0:X2}", b);

            return hex.ToString();
        }
    }
}
