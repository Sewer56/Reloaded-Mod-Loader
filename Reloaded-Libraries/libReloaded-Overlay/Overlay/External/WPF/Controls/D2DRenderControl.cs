/*
    [Reloaded] Mod Loader Common Library (libReloaded)
    The main library acting as common, shared code between the Reloaded Mod 
    Loader Launcher, Mods as well as plugins.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/


using SharpDX.Direct2D1;

namespace Reloaded.Overlay.External.WPF.Controls
{
    /// <summary>
    /// Simple control overridden from a NuGet library used to render user content to the screen.
    /// Exposes a simple RenderTarget to the user.
    /// </summary>
    public class D2DRenderControl : D2dControl.D2dControl
    {
        /// <summary>
        /// Defines a delegate signature used for rendering visual elements using direct2D.
        /// </summary>
        /// <param name="direct2DWindowTarget">Window Render Target used for drawing with Direct2D by the end user.</param>
        public delegate void DelegateRenderDirect2D(RenderTarget direct2DWindowTarget);

        /// <summary>
        /// Contains a method to be executed upon finishing the loading of the window.
        /// </summary>
        public DelegateRenderDirect2D RenderDelegate;

        /// <summary>
        /// Direct2D rendering is performed here.
        /// Wow, this small NuGet package is cool!
        /// </summary>
        /// <param name="target">The RenderTarget used to perform the rendering.</param>
        public override void Render(RenderTarget target)
        {
            RenderDelegate?.Invoke(target);
        }
    }
}
