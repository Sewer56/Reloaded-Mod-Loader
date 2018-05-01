using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using Reloaded.Native.Functions;
using Reloaded.Native.WinAPI;
using Reloaded.Overlay.External;
using Reloaded.Overlay.External.Forms;
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

        private static OverlayWindow WPFWindow;
        private static TransparentWinform OverlayForm;
        private static System.Windows.Application WPFApp;

        /// <summary>
        /// Your own user code starts here.
        /// If this is your first time, do consider reading the notice above.
        /// It contains some very useful information.
        /// </summary>
        public static void Init()
        {
            /*
                Reloaded Mod Loader Sample: Windows Presentation Foundation
                Architectures supported: X86, X64

                Creates an interactive Windows Presentation Foundation semi-transparent window inside
                the running process, demonstrating or giving an idea on how perhaps utility tools 
                could be developed with libReloaded or how WPF may be used as an alternative to Direct2D
                HUDs/Overlays.

                Note: In order to allow the use of WPF over the Reloaded Template, you need to add
                <ProjectTypeGuids>{60dc8134-eba5-43b8-bcc9-bb4bc16c2548};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>
                to the first PropertyGroup in the Project's .csproj and reference PresentationFramework + System.Xaml.
            */

            /*
                We create our own thread and run it in the background because Reloaded-Loader explicitly waits
                for the mod's thread to return before continuing to load other mods and ultimately the game.

                Inside the thread, we initiate a regular WPF window to use for drawing.
            */
            Thread setBorderlessThread = new Thread
            (
                () =>
                {
                    // Loop infinitely until a window handle is found.
                    while (GameProcess.Process.MainWindowHandle == IntPtr.Zero)
                    {
                        // Sleep the thread for a sensible amount of time.
                        Thread.Sleep(1000);
                    }

                    // Initiate WPF
                    WPFApp = new System.Windows.Application();
                    WPFWindow = new OverlayWindow();
                    WPFApp.Run(WPFWindow);
                }
            );
            setBorderlessThread.SetApartmentState(ApartmentState.STA);
            setBorderlessThread.Start();
        }
    }
}
