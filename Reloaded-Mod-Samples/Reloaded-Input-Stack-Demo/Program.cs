using System.Diagnostics;
using System.Threading;
using Reloaded;
using Reloaded.Input;
using Reloaded.Input.Common;
using Reloaded.Input.Common.ControllerInputs;
using Reloaded.Process;

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
                Reloaded Mod Loader Sample: Reloaded Input Stack Demo
                Architectures supported: X86, X64

                Demonstrates how to use the Reloaded Input Stack to read user inputs.
            */

            // Want to see this in with a debugger? Uncomment this line.
            // Debugger.Launch();

            // Print demo details.
            Thread controllerReadThread = new Thread(() =>
            {
                // The main hub/interface for directing with controllers.
                ControllerManager controllerManager = new ControllerManager();
                int controllerPort = 0;             // Arrays start at X
                
                // Get inputs and print them.
                while (true)
                {
                    // That's it, did you expect anything more? Easy peasy!
                    ControllerInputs inputs = controllerManager.GetInput(controllerPort);

                    // Let's print it to the console.
                    Bindings.PrintText
                    (
                        "///////////////////////CONTROLLER INPUT BEGIN\n" +
                        $"Buttons (Some of them):\n" +
                        $"A: {inputs.ControllerButtons.ButtonA}\n" +
                        $"B: {inputs.ControllerButtons.ButtonB}\n" +
                        $"X: {inputs.ControllerButtons.ButtonX}\n" +
                        $"Y: {inputs.ControllerButtons.ButtonY}\n" +
                        $"LB: {inputs.ControllerButtons.ButtonLb}\n" +
                        $"RB: {inputs.ControllerButtons.ButtonRb}\n" +
                        $"Axis (Some of them):\n" +
                        $"Left Stick X: {inputs.LeftStick.GetX()}\n" +
                        $"Left Stick Y: {inputs.LeftStick.GetY()}\n" +
                        $"Left Trigger: {inputs.GetLeftTriggerPressure()}\n" +
                        $"Right Trigger: {inputs.GetRightTriggerPressure()}\n" +
                        $"Game Window Has Focus: {Reloaded.Native.Functions.WindowProperties.IsWindowActivated(GameProcess.Process.MainWindowHandle)}\n" +
                        "///////////////////////CONTROLLER INPUT END\n"
                    );

                    // Refresh approximately every 1/3 second.
                    Thread.Sleep(333);
                }

                /*
                    The controller code is all custom and written from the ground up over SharpDX.
                    Automatic Hotplugging, Remapping, Deadzones, Button to Axis, Axis to Axis etc. are supported.
                    Feel free to poke around the libReloaded library.
                */

            });
            controllerReadThread.Start();
        }
    }
}
