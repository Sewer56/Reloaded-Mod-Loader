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

namespace Reloaded.DirectX
{
    /// <summary>
    /// The Direct3DVersion enumerable is used often as a parameter
    /// to state which version of DirectX is to be used for hooking,
    /// rendering among other various operations within Reloaded Mod Loader.
    /// </summary>
    public enum Direct3DVersion
    {
        /// <summary>
        /// Unknown or not a DirectX process
        /// </summary>
        Null,

        /// <summary>
        /// DirectX 9
        /// </summary>
        Direct3D9,

        /// <summary>
        /// [Currently Unsupported]
        /// DirectX 10
        /// </summary>
        Direct3D10,

        /// <summary>
        /// [Currently Unsupported]
        /// DirectX 10.1
        /// </summary>
        Direct3D10_1,

        /// <summary>
        /// DirectX 11
        /// </summary>
        Direct3D11,

        /// <summary>
        /// DirectX 11.1
        /// </summary>
        Direct3D11_1,

        /// <summary>
        /// DirectX 11.2
        /// </summary>
        Direct3D11_2,
        
        /// <summary>
        /// DirectX 11.3
        /// </summary>
        Direct3D11_3,
        
        /// <summary>
        /// DirectX 11.4
        /// </summary>
        Direct3D11_4
    }
}
