using System;
using System.Drawing;
using System.Threading;
using Reloaded.Native.Functions;
using Reloaded.Native.WinAPI;
using Reloaded.Process;
using static Reloaded.Native.WinAPI.WindowStyles;
using static Reloaded.Native.WinAPI.Constants;

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
                Reloaded Mod Loader Sample: Universal Borderless Windowed
                Architectures supported: X86, X64

                Waits until the game or process spawns off its initial border and then changes the
                window border style of the application to borderless using the Windows API.
            */

            /*
                We create our own thread and run it in the background because Reloaded-Loader explicitly waits
                for the mod's thread to return before continuing to load other mods and ultimately the game.

                For anything we want to do in the background during initialization with Reloaded or you need to wait
                for the process/game for some reason, you are requires to start a background thread.
            */
            Thread setBorderlessThread = new Thread
            (
                () =>
                {
                    // Loop infinitely until a window handle is found.
                    while (GameProcess.Process.MainWindowHandle == IntPtr.Zero)
                    {
                        // Sleep the thread for a sensible amount of time.
                        Thread.Sleep(2000);
                    }

                    // Get the window size.
                    Point windowSize = WindowProperties.GetWindowSize(GameProcess.Process.MainWindowHandle);
                    Structures.WinapiRectangle windowLocation = WindowProperties.GetWindowRectangle(GameProcess.Process.MainWindowHandle);

                    // Get the game's Window Style.
                    uint windowStyle = (uint)GetWindowLongPtr(GameProcess.Process.MainWindowHandle, GWL_STYLE);

                    // Change the window style.
                    windowStyle &= ~WS_BORDER;
                    windowStyle &= ~WS_CAPTION;
                    windowStyle &= ~WS_MAXIMIZEBOX;
                    windowStyle &= ~WS_MINIMIZEBOX;

                    // Set the window style.
                    SetWindowLongPtr(GameProcess.Process.MainWindowHandle, GWL_STYLE, (IntPtr)windowStyle);

                    // Set the window size.
                    WindowFunctions.MoveWindow(GameProcess.Process.MainWindowHandle, windowLocation.LeftBorder,
                        windowLocation.TopBorder, windowSize.X, windowSize.Y, true);
                }    
            );
            setBorderlessThread.Start();

        }
    }
}
