using System;
using System.Diagnostics;
using System.Reflection;
using Reloaded;
using Reloaded.Assembler;
using Reloaded.Process;

namespace Reloaded_Mod_Template
{
    public static class Program
    {
        /*
         *  Reloaded Mod Loader DLL Modification Template
         *  Sewer56, 2018 ©
         *
         *  -------------------------------------------------------------------------------
         *
         *  Here starts your own mod loader DLL code.
         *
         *  The Init function below is ran at the initialization stage of the game.
         *
         *  The game at this point suspended and frozen in memory. There is no execution
         *  of game code currently ongoing.
         *
         *  This is where you do your hot-patches such as graphics stuff, patching the
         *  window style of the game to borderless, setting up your initial variables, etc.
         *
         *  -------------------------------------------------------------------------------
         *
         *  Important Note:
         *
         *  This function is executed once during startup and SHOULD return as the
         *  mod loader awaits successful completion of the main function.
         *
         *  If you want your mod/code to sit running in the background, please initialize
         *  another thread and run your code in the background on that thread, otherwise
         *  please remember to return from the function.
         *
         *  There is also some extra code, including DLL stubs for Reloaded, classes
         *  to interact with the Mod Loader Server as well as other various loader related
         *  utilities available.
         *
         *  -------------------------------------------------------------------------------
         *
         *  Brief Walkthrough:
         *
         *  > ReloadedCode/Initializer.cs
         *      Stores Reloaded Mod Loader DLL Template/Initialization Code.
         *      You are not required/should not (need) to modify any of the code.
         *
         *  > ReloadedCode/Client.cs
         *      Contains various pieces of code to interact with the mod loader server.
         *
         *      For convenience it's recommended you import Client static(ally) into your
         *      classes by doing it as such `Reloaded_Mod_Template.Reloaded_Code.Client`.
         *
         *      This will avoid you typing the full class name and let you simply type
         *      e.g. Print("SomeTextToConsole").
         *
         *  -------------------------------------------------------------------------------
         *
         *  If you like Reloaded, please consider giving a helping hand. This has been
         *  my sole project taking up most of my free time for months. Being the programmer,
         *  artist, tester, quality assurance, alongside various other roles is pretty hard
         *  and time consuming, not to mention that I am doing all of this for free.
         *
         *  Well, alas, see you when Reloaded releases.
         *
         *  Please keep this notice here for future contributors or interested parties.
         *  If it bothers you, consider wrapping it in a #region.
        */

        /*
            Default Variables:
            These variables are automatically assigned by the mod template, you do not
            need to assign those manually.
        */

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
        
        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static unsafe void Init()
        {
            Debugger.Launch();
            Bindings.PrintInfo("Hello World!");
            Assembler.Assemble(new[] {"use32", "mov eax, ebx", "mov eax, 0x123456"});
        }
    }
}
