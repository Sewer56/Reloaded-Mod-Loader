using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ColorMine.ColorSpaces;
using Reloaded.DirectX.Definitions;
using Reloaded.Overlay.Internal;
using Reloaded.Process;
using Reloaded.Process.X86Functions;
using Reloaded.Process.X86Functions.CustomFunctionFactory;
using Reloaded.Process.X86Functions.Utilities;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

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
        public static async void Init()
        {
            /*
                Reloaded Mod Loader Sample: DX9 Internal Overlay Example
                Architectures supported: X86

                Demonstrates an example of drawing with Reloaded's DirectX 9 hooking mechanism,
                the API that's used by a large majority of the games out there at the time of writing.

                Note: Debugging this might prove troublesome for this entry function, with async methods,
                local variable values are not exactly known to show in all configurations.

                The following sample has been tested with Sonic Adventure 2 and Sonic Heroes (under Crosire's D3D8To9).
            */
            
            /*
                AnimInterpolator: A tool for generating intermediate LCH colours between two targets.
                ColorspaceConverter: Converts between System.Drawing.Color and LCH in batch.

                Both of these have been shamelessly and lazily stolen from Reloaded's
                Reloaded-GUI project for the Launcher WinForms GUI, please do not heed them any mind.
            */

            // Calculate triangle render colours (for animation).
            // This isn't pretty, just a reuse of code from Reloaded-GUI.
            AnimInterpolator interpolator = new AnimInterpolator(1000,60);

            Lch redColor = ColorspaceConverter.ColorToLch(System.Drawing.Color.Red);
            Lch blueColor = ColorspaceConverter.ColorToLch(System.Drawing.Color.Blue);
            Lch greenColor = ColorspaceConverter.ColorToLch(System.Drawing.Color.Green);

            List<Lch> redToGreenColour = interpolator.CalculateIntermediateColours(redColor, greenColor);
            List<Lch> greenToBlueColour = interpolator.CalculateIntermediateColours(greenColor, blueColor);
            List<Lch> blueToRedColour = interpolator.CalculateIntermediateColours(blueColor, redColor);

            _redToGreenColours = new List<Color>();        
            _greenToBlueColours = new List<Color>();
            _blueToRedColours = new List<Color>();

            // Convert our Lch colours to SharpDX ones.
            foreach (var colour in redToGreenColour)
            { System.Drawing.Color colorRGB = ColorspaceConverter.LchToColor(colour);
              _redToGreenColours.Add(new SharpDX.Color(colorRGB.R, colorRGB.G, colorRGB.B)); }

            foreach (var colour in greenToBlueColour)
            { System.Drawing.Color colorRGB = ColorspaceConverter.LchToColor(colour);
              _greenToBlueColours.Add(new SharpDX.Color(colorRGB.R, colorRGB.G, colorRGB.B)); }

            foreach (var colour in blueToRedColour)
            { System.Drawing.Color colorRGB = ColorspaceConverter.LchToColor(colour);
              _blueToRedColours.Add(new SharpDX.Color(colorRGB.R, colorRGB.G, colorRGB.B)); }

            /*
                This one's going to be embarrasingly short, sit tight.
                Make sure to add using SharpDX.Direct3D9; to the top of your file.

                RenderDelegate: EndScene hook, this is where you will render stuff to the screen.
                ResetDelegate: Reset hook, this is called when the resolution of the window changes, or a switch from fullscreen to window is performed.
                HookDelay: Some games require this due to bad programming *cough* Sonic Adventure 2 *cough*.
            */
            _directX9Overlay = await DX9Overlay.CreateDirectXOverlay(RenderDelegate, ResetDelegate, 4000);
            _directX9Overlay.EndSceneHook.Activate();
            _directX9Overlay.ResetHook.Activate();

            /*
                Warning: Amateur DirectX 9 Rendering Code.
                I've only provided the hooks, I'm not familliar with DirectX nor SharpDX myself, below is a basic
                example of rendering some text, a line and a basic arbitrary shape.

                The triangle example is stolen and adapted shamelessly from: http://www.directxtutorial.com/Lesson.aspx?lessonid=9-4-4
                The animation is my own.
                Protip: If looking for a SharpDX function, search the SharpDX repository with the C++ function.
                I would also highly advise looking at SharpDX examples https://github.com/sharpdx/SharpDX-Samples/tree/master/Desktop/Direct3D9
            */
        }

        /// <summary>
        /// Reloaded's internal DirectX 9 overlay class/
        /// </summary>
        private static DX9Overlay _directX9Overlay;

        /// <summary>
        /// This is where we adjust our overlay when the resolution of the game or the process changes.
        /// </summary>
        /// <param name="device">Pointer to the DirectX9 Device</param>
        /// <param name="presentParameters">Defines a few very nice parameters such as whether we are windowed.</param>
        /// <returns>The original function's result.</returns>
        private static int ResetDelegate(IntPtr device, ref PresentParameters presentParameters)
        {
            // Here we add code to adjust our overlay as necessary to e.g. resolution changes.
            // In this very specific case however, no code is provided in the example.

            // At the end of this, we should call the original Reset
            return _directX9Overlay.ResetHook.OriginalFunction(device, ref presentParameters);
        }

        // Initialization flag
        private static bool _initialized;

        // Line Rendering
        private static Line _sampleLine;
        private static RawVector2[] _lineVertices;

        // Text Rendering
        private static Font _sampleFont;
        private static RawColorBGRA _semiTransparentGray;
        private static RawRectangle _textRectangle;

        // Triangle rendering.
        private static Vertex[] _triangleVertices;
        private static VertexBuffer _localVertexBuffer;
        private static VertexDeclaration _vertexDeclaration;
        private static SetPixelShaderDelegate _setPixelShaderFunction;

        // Colours for triangle render animation.
        private static VertexRenderingMode[] _vertexRenderingModes;
        private static List<Color> _redToGreenColours;
        private static List<Color> _greenToBlueColours;
        private static List<Color> _blueToRedColours;
        private static int _frameCounter = 0;

        /// <summary>
        /// Sets up the individual Direct3D Contstants and objects used for rendering.
        /// </summary>
        /// <param name="device"></param>
        private static unsafe void SetupRenderingConstants(Device device)
        {
            // Create our individual devices and fonts if necessary.
            // (Normally you would do this on initialization, not in the render function but this is just for clarity).
            if (!_initialized)
            {
                // Let's create a line for us to draw later.
                _sampleLine = new Line(device);
                _lineVertices = new[]
                {
                    new RawVector2(100, 300),
                    new RawVector2(150, 200),
                    new RawVector2(250, 200)
                };

                // Create a font for us to draw later (SharpDX D3D9 Font)
                _sampleFont = new Font(device, 20, 0, FontWeight.Normal, 1, false, FontCharacterSet.Ansi,
                    FontPrecision.Default, FontQuality.ClearType, FontPitchAndFamily.Modern, "Times New Roman");

                // Set position of text and colour of line + text.
                _semiTransparentGray = new RawColorBGRA(255, 255, 255, 128);
                _textRectangle = new RawRectangle(100, 100, 9999, 9999);

                // Create triangle vertices.
                // Fun fact: W acts as the blend factor here.
                _triangleVertices = new[] {
                    new Vertex() { Color = Color.Red, Position = new Vector4(400.0f, 100.0f, 0.5f, 1.0f) },
                    new Vertex() { Color = Color.Blue, Position = new Vector4(650.0f, 500.0f, 0.5f, 1.0f) },
                    new Vertex() { Color = Color.Green, Position = new Vector4(150.0f, 500.0f, 0.5f, 1.0f) }};

                // Set vertex render mode for our vertices.
                _vertexRenderingModes = new[]
                    {VertexRenderingMode.RedToGreen, VertexRenderingMode.BlueToRed, VertexRenderingMode.GreenToBlue};

                // Create vertex buffer.
                _localVertexBuffer = new VertexBuffer(device, sizeof(Vertex) * 3, 0, VertexFormat.None, Pool.Default);
                _localVertexBuffer.Lock(0, 0, LockFlags.None).WriteRange(_triangleVertices);
                _localVertexBuffer.Unlock();

                // Specifies the Vertex Format
                var vertexElems = new[] { new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.PositionTransformed, 0),
                                          new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                                          VertexElement.VertexDeclarationEnd };
                _vertexDeclaration = new VertexDeclaration(device, vertexElems);

                // Wrap SetPixelShader for us to call later (otherwise our shape may not show)
                // This uses Reloaded's Virtual Function Table Utility Class
                if (_directX9Overlay != null)
                {
                    VirtualFunctionTable.TableEntry vTableEntry = _directX9Overlay.DirectX9Hook.DirectXFunctions[(int)Direct3DDevice9.SetPixelShader];
                    _setPixelShaderFunction = vTableEntry.CreateX86WrapperFunction<SetPixelShaderDelegate>();
                }

                // Never run this again.
                _initialized = true;
            }
        }

        /// <summary>
        /// This is where we draw using SharpDX functions and our device pointer.
        /// </summary>
        /// <param name="device">Pointer to the DirectX9 Device</param>
        /// <returns>The original function's result.</returns>
        private static unsafe int RenderDelegate(IntPtr device)
        {
            // Obtain SharpDX device instance.
            Device localDevice = new Device(device);

            // Setup Rendering Constants
            SetupRenderingConstants(localDevice);

            // Let's draw!
            _sampleLine.Draw(_lineVertices, _semiTransparentGray);
            _sampleFont.DrawText(null, "Reloaded D3D9 Overlay Sample", _textRectangle, FontDrawFlags.Left | FontDrawFlags.Top, _semiTransparentGray);

            // Render Triangle
            RenderTriangle(localDevice);

            // At the end of this, we should call the original EndScene
            return _directX9Overlay.EndSceneHook.OriginalFunction(device);
        }

        /// <summary>
        /// Renders the individual triangle as seen in the demonstration.
        /// </summary>
        private static unsafe void RenderTriangle(Device localDevice)
        {
            // Backup old stream and render properties.
            VertexDeclaration oldVertexDeclaration = localDevice.VertexDeclaration;
            localDevice.GetStreamSource(0, out var currentVertexBuffer, out var currentBufferOffset, out var currentStrideRef);
            localDevice.SetTexture(0, null);                                        // mDevice->SetTexture(0, NULL); Necessary or might not show ingame.
            _setPixelShaderFunction?.Invoke(localDevice.NativePointer, IntPtr.Zero);// mDevice->SetPixelShader(0);

            // Change triangle properties and animate the local vertex buffer.
            CalculateVertexColours();      // Does our colour animation.
            _localVertexBuffer.Lock(0, 0, LockFlags.None).WriteRange(_triangleVertices);
            _localVertexBuffer.Unlock();

            // Setup Render Properties
            localDevice.VertexDeclaration = _vertexDeclaration;                     // mDevice->SetFVF(VertexDeclaration);
            localDevice.SetRenderState(RenderState.AlphaBlendEnable, true);
            localDevice.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
            localDevice.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);

            localDevice.SetRenderState(RenderState.Lighting, false);                // Lighting might affect our triangle colour.
            localDevice.SetStreamSource(0, _localVertexBuffer, 0, sizeof(Vertex));
            localDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

            // Restore old stream and render properties.
            localDevice.SetRenderState(RenderState.Lighting, true);
            localDevice.VertexDeclaration = oldVertexDeclaration;                   // mDevice->SetFVF(&dwTmpFVF);
            localDevice.SetStreamSource(0, currentVertexBuffer, currentBufferOffset, currentStrideRef);
        }

        /// <summary>
        /// Performs our little animation with the vertex colours.
        /// </summary>
        private static void CalculateVertexColours()
        {
            // Set vertex colours.
            for (int x = 0; x < _triangleVertices.Length; x++)
            {
                Vertex triangleVertex = _triangleVertices[x];
                VertexRenderingMode vertexRenderMode = _vertexRenderingModes[x];

                // Load appropriate color and change render mode if necessary.
                switch (vertexRenderMode)
                {
                    case VertexRenderingMode.RedToGreen:
                        triangleVertex.Color = _redToGreenColours[_frameCounter];

                        if (_frameCounter >= 60)
                            vertexRenderMode = VertexRenderingMode.GreenToBlue;

                        break;

                    case VertexRenderingMode.GreenToBlue:
                        triangleVertex.Color = _greenToBlueColours[_frameCounter];

                        if (_frameCounter >= 60)
                            vertexRenderMode = VertexRenderingMode.BlueToRed;

                        break;

                    case VertexRenderingMode.BlueToRed:
                        triangleVertex.Color = _blueToRedColours[_frameCounter];

                        if (_frameCounter >= 60)
                            vertexRenderMode = VertexRenderingMode.RedToGreen;

                        break;
                }

                // Replace vertex.
                _triangleVertices[x] = triangleVertex;
                _vertexRenderingModes[x] = vertexRenderMode;
            }

            // Increment frame counter.
            if (_frameCounter >= 60)
                _frameCounter = 0;
            else
                _frameCounter++;
        }

        /// <summary>
        /// Defines our own custom FVF format used for rendering
        /// Vertices to the screen.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public Vector4 Position;
            public ColorBGRA Color;        

            public Vertex(Vector4 position, ColorBGRA color)
            {
                this.Position = position;
                this.Color = color;
            }
        }

        /// <summary>
        /// Specifies the individual rendering animation for the current vertex.
        /// </summary>
        public enum VertexRenderingMode
        {
            RedToGreen,
            GreenToBlue,
            BlueToRed,
        }

        /// <summary>
        /// Defines the IDirect3DDevice9.SetPixelShader function.
        /// changes.
        /// </summary>
        /// <param name="pixelShaderPointer">Pointer to the pixel shader.</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        [ReloadedFunction(CallingConventions.Stdcall)]
        public delegate int SetPixelShaderDelegate(IntPtr devicePtr, IntPtr pixelShaderPointer);
    }
}
