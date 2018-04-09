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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Reloaded.Native.WinAPI
{
    /// <summary>
    /// Provides code used for hooking of Window events in Microsoft Windows.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class WindowEventHooks
    {
        /// <summary>
        /// When the location or size of a window changes.
        /// </summary>
        public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;

        /// <summary>
        /// Allow cross-process hooking of window events.
        /// </summary>
        public const uint WINEVENT_OUTOFCONTEXT = 0;

        /// <summary>
        /// Defines a delegate to use in conjunction with SetWinEventHook.
        /// </summary>
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        /// <summary>
        /// Allows to set an event hook function for a range of events.
        /// </summary>
        /// <param name="eventMin">Minimum event code of events to capture, can equal maximum.</param>
        /// <param name="eventMax">Maximum event code of events to capture, can equal maximum.</param>
        /// <param name="hmodWinEventProc">Handle to the DLL that contains the hook function at lpfnWinEventProc, the function we will be executing. Set to null in conjunction of WINEVENT_OUTOFCONTEXT dwFlags</param>
        /// <param name="lpfnWinEventProc">Pointer to the event hook function, either to the specified DLL or to your own code. Use a WinEventDelegate for this one.</param>
        /// <param name="idProcess">Specifies the ID of the process from which the hook function receives events. Specify zero (0) to receive events from all processes on the current desktop.</param>
        /// <param name="idThread">Specifies the ID of the thread from which the hook function receives events. If this parameter is zero, the hook function is associated with all existing threads on the current desktop.</param>
        /// <param name="dwFlags">Flag values that specify the location of the hook function and of the events to be skipped. WINEVENT_OUTOFCONTEXT is that you'll probably want.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
    }
}
