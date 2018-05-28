
<div align="center">
	<h1>Reloaded Features</h1>
	<img src="https://i.imgur.com/mdohYO2.png" align="center" />
	<br/> <br/>
</div>

## Characteristics
 - 100% C# Source Code.
 - Launchable from GUI and CLI.
 - 0% Filesystem/Executable Modifications.
 - Can attach to already running games at any time, not only at startup.
 - Mods are forward and backward compatible (Statically Compiled Modifications).
 - Mods can list dependencies on other mods.
 - Allows for running of multiple instances at once, feel free to write Netplay/Cross-game mods.
 - Compatible with both X86 & X64 games/processes and/or Address Space Layout Randomization (ASLR).
 - Compatible out of the box with other mod loaders in 99.99% of cases (one of Reloaded's main design goals).
 - Supports a list of global mods to always load, regardless of game (for common utilities).
 - Supports third party one-click mod install support (custom URI handler).
 - Self updating, no need to manually check for Reloaded updates.

Reloaded was built from the ground up, from zero.

## Features (For Developers)
This list is non-exhaustive and does not contain all features, just the more notable ones.

 - [x] C# based mod development with rich development library.
 - [x] Compile to Any CPU, no need to switch build architecture per game.
 - [x] Mods can patch any executable code before anything is executed (patching games in suspended state on launch).
 - [x] Attach to any actively running process, test mod changes faster, bypass launcher restrictions.
 - [x] Debugging available: `Debugger.Launch()` or `Attach` to running game executable and breakpoint.
 - [x] Can also technically support native C/C++, D, other mod DLLs (see "Reloaded For Developers").

### Process/Memory Control:
 - [x] Read/Write to memory of current process or other processes (including structs).
 - [x] Launch executables in suspended state,
 - [x] Allocate memory inside target processes.
 - [x] Resume/Suspend threads, kill arbitrary processes.
 - [x] Call native functions in other executables.
 - [x] Pattern scan for signature including string with mask.

### Hooking/Code Manipulation:

 - [x] X86, X64, 16-bit live runtime assembly & disassembly.
 - [x] Automatic logging to console window during game execution with colour coding.
 - [x] Call X86 native game/process functions from C#, including custom calling conventions *"usercall", "userpurge"*.
 - [x] Hook X86 native game/process functions from C#, including custom calling conventions.
 - [x] Call X64 custom calling conventions *"usercall", "userpurge"*
 - [x] Hook X64 native game/process functions from C#, including custom calling conventions
 - [x] Auto-patching of other loaders'/programs' function hooks when performing hooks, stack Reloaded hooks on Reloaded hooks and non-Reloaded hooks.

### Other
- [x] Multiple external and internal overlays: WPF, D2D-WPF, D2D-WinForms, DX9, DX11.
- [x] Shared Codebase: Mods can check & load other mods, inject native code, read enabled mods/settings...
- [x] Reloaded Input Stack: Extremely customizable input support for mods (dual XInput + DInput), featuring hotplugging, on the fly remapping, button to axis, axis to axis, deadzone, radius customization etc.

### Native:
Reloaded also contains multiple, very well documented sets of functions allowing you to call native Windows API functions via P/Invoke, here are some examples of what you can do:

 - [x] Change window styles, automatic borderless windowed.
 - [x] Check if a window, including current game window is focused.
 - [x] Query and interact with window locations.
 - [x] Find a window by window name or class .
 - [x] Set window event hooks, capture when a window moves or is resized, etc.

### In the future
After a good rest at least. At time of writing, this has been my sole project for about 6+ months now give-take.
