using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using System.Threading;
using SharpDX.DXGI;

namespace SonicHeroes.Overlay
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of the game, if the game is in windowed mode.
    /// </summary>
    public class Overlay_External_Direct2D
    {
        /// <summary>
        /// Fake glass form which we will be overlaying Sonic Heroes with.
        /// </summary>
        public Form_FakeTransparentOverlay overlayWinForm = new Form_FakeTransparentOverlay();

        /// <summary>
        /// Create the fake GDI overlay.
        /// </summary>
        public Overlay_External_Direct2D() { }

        ////////////////// For DirectX Hooking!
        public SharpDX.Direct2D1.WindowRenderTarget direct2DWindowTarget;
        public delegate void Delegate_RenderDirect2D(WindowRenderTarget direct2DWindowTarget);
        public delegate void Delegate_OnFrameDelegate(); // Allows for code to be ran on every rendered frame.
        public Delegate_RenderDirect2D direct2DRenderMethod;
        public Delegate_OnFrameDelegate direct2DOnframeDelegate; // Allows for code to be ran on every rendered frame.
        public bool Rectangle_Render = false;
        public bool Ready_To_Render = false;

        /// <summary>
        /// Initializes a Direct2D device used to draw to the screen. You MUST set the Direct2D Drawing Delegate to your own method, see the wiki for sample.
        /// </summary>
        public void Initialize_DirectX()
        {
            // Init DirectX
            // This initializes the DirectX device. It needs to be done once.

            // Wait for Heroes Window to spawn, when spawned, set the DirectX properties.
            Thread Get_Heroes_Window_Thread = new Thread
            (
                () =>
                {
                    try
                    {
                        // Wait for Sonic Heroes Window to spawn.
                        while (overlayWinForm.Window_Setup_Complete == false)
                        {
                            // Get the handle for the Sonic_Heroes Window
                            Thread.Sleep(1000);
                        }
                        // Create the D2D1 Factory
                        SharpDX.Direct2D1.Factory Direct2D_Factory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

                        // Set the render properties!
                        SharpDX.Direct2D1.HwndRenderTargetProperties Direct2D_Render_Target_Properties = new HwndRenderTargetProperties();
                        Direct2D_Render_Target_Properties.Hwnd = overlayWinForm.Handle;
                        Direct2D_Render_Target_Properties.PixelSize = new SharpDX.Size2(overlayWinForm.Width, overlayWinForm.Height);
                        Direct2D_Render_Target_Properties.PresentOptions = PresentOptions.None;

                        direct2DWindowTarget = new SharpDX.Direct2D1.WindowRenderTarget
                        (
                            Direct2D_Factory,
                            new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)),
                            Direct2D_Render_Target_Properties
                        );

                        DirectX_Clear_Screen(); // Clear the residue graphics left from the Windows form which was made transparent and clickthrough.
                        Ready_To_Render = true;
                    }
                    catch
                    {

                    }
                }
            );
            Get_Heroes_Window_Thread.Start();
        }

        /// <summary>
        /// Rendering is handled here, you must first pass a method to Direct2D_Render_Method which will be used for handling the rendering.
        /// </summary>
        /// <param name="sender"></param>
        public void DirectX_Render()
        {
            if (!Rectangle_Render && Ready_To_Render)
            {
                Rectangle_Render = true; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
                try
                {
                    direct2DWindowTarget.BeginDraw(); // Begin Drawing!
                    Clear_Screen(0, 0, 0, 0); // Clears everything drawn previously.

                    direct2DRenderMethod(direct2DWindowTarget); // Calls our own set rendering method
                    try { direct2DOnframeDelegate?.Invoke(); } catch { } // Safely invoke all assigned delegates.
                    direct2DWindowTarget.EndDraw(); // End Drawing!
                }
                catch { Rectangle_Render = false; } // Set default render state.

                Rectangle_Render = false;// Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
            }
        }

        /// <summary>
        /// Cleans up the currently drawn assets on the screen
        /// </summary>
        public void DirectX_Clear_Screen()
        {
            if (!Rectangle_Render && Ready_To_Render)
            {
                Rectangle_Render = true; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
                try
                {
                    direct2DWindowTarget.BeginDraw(); // Begin Drawing!
                    Clear_Screen(0, 0, 0, 0); // Clears everything drawn previously.
                    direct2DWindowTarget.EndDraw(); // End Drawing!
                }
                catch { Rectangle_Render = false; }
                Rectangle_Render = false; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
            }

        }

        /// <summary>
        /// Clears the drawing area with the specified RGBA Colours. Use this to wipe the screen.
        /// </summary>
        private void Clear_Screen(float r, float g, float b, float a)
        {
            direct2DWindowTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a));
        }

        /// <summary>
        /// Makes your form a child form of the fake glass game overlay.
        /// Use only for testing, or other potentially weird purposes.
        /// (Please no GDI Overlays)
        /// </summary>
        /// <param name="yourForm">Your windows forms form.</param>
        public void Attach_Windows_Form(Form yourForm)
        {
            IntPtr userForm_Handle_ID = yourForm.Handle;
            IntPtr thisForm_Handle_ID = overlayWinForm.Handle;
            WINAPI_Components.SetParent(userForm_Handle_ID, thisForm_Handle_ID);
            yourForm.BringToFront(); // Bring to front
        }
    }

    /// <summary>
    /// Windows API P/Invoke imports used within the scope of this class.
    /// </summary>
    static class WINAPI_Components
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Form_FakeTransparentOverlay.RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Form_FakeTransparentOverlay.RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        /// Windows Event Flags
        internal static readonly uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
        internal static readonly uint WINEVENT_OUTOFCONTEXT = 0;
    }

}
