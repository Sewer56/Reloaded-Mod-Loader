using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.DirectWrite;
using System.Threading;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

namespace SonicHeroes.Overlay
{
    /// <summary>
    /// This class is responsible for allowing you to instantiate a Windows Forms overlay which will be drawn ontop of the game, if the game is in windowed mode.
    /// </summary>
    public class SonicHeroes_Overlay
    {
        /// <summary>
        /// Fake glass form which we will be overlaying Sonic Heroes with.
        /// </summary>
        public SonicHeroes_Form_FakeTransparentOverlay OverlayForm = new SonicHeroes_Form_FakeTransparentOverlay();

        /// <summary>
        /// Create the fake GDI overlay.
        /// </summary>
        public SonicHeroes_Overlay() { }

        /// <summary>
        /// Makes your form a child form of the fake glass game overlay.
        /// </summary>
        /// <param name="Your_Form"></param>
        public void Attach_Windows_Form(Form Your_Form)
        {
            IntPtr User_Form_Handle_ID = Your_Form.Handle;
            IntPtr This_Form_Handle_ID = OverlayForm.Handle;
            WINAPI_Components.SetParent(User_Form_Handle_ID, This_Form_Handle_ID);
            Your_Form.BringToFront(); // Bring to front
        }

        ////////////////// For DirectX Hooking!
        public SharpDX.Direct2D1.WindowRenderTarget Direct2D_Graphics_Target;
        public delegate void Delegate_RenderDirect2D(WindowRenderTarget Direct2D_Graphics_Target);
        public Delegate_RenderDirect2D Direct2D_Render_Method;
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
                    // Wait for Sonic Heroes Window to spawn.
                    while (OverlayForm.Window_Setup_Complete == false)
                    {
                        // Get the handle for the Sonic_Heroes Window
                        Thread.Sleep(1000);
                    }
                    // Create the D2D1 Factory
                    SharpDX.Direct2D1.Factory Direct2D_Factory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

                    // Set the render properties!
                    SharpDX.Direct2D1.HwndRenderTargetProperties Direct2D_Render_Target_Properties = new HwndRenderTargetProperties();
                    Direct2D_Render_Target_Properties.Hwnd = OverlayForm.Handle;
                    Direct2D_Render_Target_Properties.PixelSize = new SharpDX.Size2(OverlayForm.Width, OverlayForm.Height);
                    Direct2D_Render_Target_Properties.PresentOptions = PresentOptions.None;

                    Direct2D_Graphics_Target = new SharpDX.Direct2D1.WindowRenderTarget
                    (
                        Direct2D_Factory,
                        new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)),
                        Direct2D_Render_Target_Properties
                    );

                    DirectX_Clear_Screen(); // Clear the residue graphics left from the Windows form which was made transparent and clickthrough.
                    Ready_To_Render = true;
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
            if (!Rectangle_Render)
            {
                Rectangle_Render = true; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
                try
                {
                    Direct2D_Graphics_Target.BeginDraw(); // Begin Drawing!
                    Clear_Screen(0, 0, 0, 0); // Clears everything drawn previously.

                    Direct2D_Render_Method(Direct2D_Graphics_Target); // Calls our own set rendering method
                    
                    Direct2D_Graphics_Target.EndDraw(); // End Drawing!
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
            if (!Rectangle_Render)
            {
                Rectangle_Render = true; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
                try
                {
                    Direct2D_Graphics_Target.BeginDraw(); // Begin Drawing!
                    Clear_Screen(0, 0, 0, 0); // Clears everything drawn previously.
                    Direct2D_Graphics_Target.EndDraw(); // End Drawing!
                }
                catch { Rectangle_Render = false; }
                Rectangle_Render = false; // Ensures that two instances never run simultaneously at the same time, this will prevent crashing.
            }

        }

        // Clears the drawing area.
        private void Clear_Screen(int r, int g, int b, int a) { Direct2D_Graphics_Target.Clear(new SharpDX.Mathematics.Interop.RawColor4(r, g, b, a)); }
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
        public static extern bool GetWindowRect(IntPtr hwnd, out SonicHeroes_Form_FakeTransparentOverlay.RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out SonicHeroes_Form_FakeTransparentOverlay.RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        /// Windows Event Flags
        internal static readonly uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
        internal static readonly uint WINEVENT_OUTOFCONTEXT = 0;
    }

}
