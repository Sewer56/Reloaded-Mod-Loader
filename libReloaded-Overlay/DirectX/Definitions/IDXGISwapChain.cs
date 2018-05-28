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

namespace Reloaded.DirectX.Definitions
{
    /// <summary>
    /// Contains a full list of IDXGISwapChain functions to be used alongside
    /// <see cref="DX11Hook"/> as an indexer into the SwapChain Virtual Function Table entries.
    /// </summary>
    public enum IDXGISwapChain
    {
        // IUnknown
        QueryInterface = 0,
        AddRef = 1,
        Release = 2,

        // IDXGIObject
        SetPrivateData = 3,
        SetPrivateDataInterface = 4,
        GetPrivateData = 5,
        GetParent = 6,

        // IDXGIDeviceSubObject
        GetDevice = 7,

        // IDXGISwapChain
        Present = 8,
        GetBuffer = 9,
        SetFullscreenState = 10,
        GetFullscreenState = 11,
        GetDesc = 12,
        ResizeBuffers = 13,
        ResizeTarget = 14,
        GetContainingOutput = 15,
        GetFrameStatistics = 16,
        GetLastPresentCount = 17,
    }
}
