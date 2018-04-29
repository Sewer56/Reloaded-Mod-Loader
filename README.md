
<div align="center">
	<h1>Project Reloaded</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong>All your mods are belong to us.</strong>
	<p>Experimental, C# universal mod loader framework compatible with arbitrary processes.</p>
</div>

# Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Building](#building)
- [Project Structure](#project-structure)

# Introduction
**[Reloaded]** is a WIP DLL Injection based Mod Loader, Mod Management System, Optional Mod SDK *(libReloaded)* among various other utilities. 

<div align="center">
	<img src="https://i.imgur.com/aG6rXm9.png" width="550" align="center" />
	<br/>
</div>

It is a completely free and open source public rewrite of **Heroes Mod Loader**, my original first attempt at a mod management/loader system all the way from the ground up using the C# programming language.

At the time of writing of this readme, Reloaded has currently been in development for 5 months, plus the extra time spent on the predecessing mod loader, this included me learning many new aspects of hacking and reverse engineering from the ground up.

# Features
Unchecked elements are currently on the TODO list, currently there are plans to complete all of these features before approaching a first public release.

This readme itself is a work in progress and will be updated.
Consequently, there are currently no ETAs for this project, it will be done when it's done.

## Characteristics
 - [x] 100% C# source code
 - [x] 0% Filesystem/Executable modifications
 - [x] Compatible with both X86 and X64 processes
 - [x] Automatically runs in X64 mode for X64 processes, X86 for X86 processes
 - [x] Compatible with executables using Address Space Layout Randomization (ASLR)
 - [x] Allows for running of multiple instances
 - [x] Launchable from GUI and CLI
 - [x] Statically Compiled Modifications: Mods are forward and backward compatible to loader

## Features (Developers)
The following is a quick, non-exhaustive from the top rundown of the features and things that you can do with Reloaded.

Many of these can also be used in standalone applications, outside of the context of mod loader mods.

### General:
 - [x] C# based mod development with rich development library
 - [x] Debugging available: Attach to running game executable and breakpoint
 - [x] Compile to Any CPU, no need to switch build architecture per game
 - [x] Can also support native C/C++, D, other mod DLLs (see Wiki)
 - [x] Attach to any actively running process, test mod changes faster, bypass launcher restrictions
 - [x] File replacement, addition of new files via hardlinking to game directory prior to execution

### Process Control:
 - [x] Read/Write to memory of current process or other processes
 - [x] Launch executables in suspended state, patch them before they execute anything
 - [x] Allocate memory inside target processes
 - [x] Resume/Suspend threads, kill arbitrary processes
 - [x] Call native functions in other executables
 - [x] Pattern scan for signature including string with mask

### Native:
Reloaded contains multiple, very well documented sets of functions allowing you to call native Windows API functions via P/Invoke, here are some examples of what you can do:

 - [x] Change window styles, automatic borderless windowed
 - [x] Check if a window, including current game window is focused
 - [x] Query and interact with window locations
 - [x] Find a window by window name or class 
 - [x] Set window event hooks, capture when a window moves or is resized, etc

### Custom:
 - [x] Hotpatching executables in RAM in suspended state, your code runs before any game code
 - [x] X86, X64, 16-bit runtime assembly & disassembly
 - [x] External Direct2D based overlay for all applications
 - [x] Automatic logging to console window during game execution with colour coding
 - [x] Custom network code available: Step towards inter-mod networking and netplay mods
 - [x] Shared codebase: Mods can check & load other mods, DLL inject native code, read settings, use themes...
 - [X] Hook X86 native game/process functions from C#, including custom calling conventions
 - [x] Call X86 native game/process functions from C#, including custom calling conventions *"usercall", "userpurge"*
 - [x] Multi level hooking for Reloaded hooks, module + in-page patching of other loaders' hooks to interoperate in multi-level hooking
 - [x] DirectX 9, 11 hooking (overlays)
 - [x] Reloaded Input Stack: Extremely customizable input support for mods (dual XInput + DInput), featuring hotplugging, on the fly remapping, button to axis, axis to axis, deadzone, radius customization etc.
 - [x] List of mods to always load, regardless of game (for common utilities, e.g. borderless, file read/write monitor)
 - [x] Self updating

### Todo (Short term: Working at it now!)
To be completed until first public release:
 - [ ] Third party one-click mod install support (GameBanana, separate application to Launcher)
 - [ ] Reloaded-wiki
 - [ ] Other quality of life features and improvements, bug fixes that come to mind

Any contributions to speed the process up would kindly be accepted.

### Todo (Long term: After a good rest)
 Note: All other features are currently known to be supported in X64 mode at the time of writing.
 - [ ] Call X64 custom calling conventions *"usercall", "userpurge"*
 - [ ] Hook X64 native game/process functions from C#, including custom calling conventions

# Building
## Prerequisites

 - MS Visual Studio (2017+)
 - Git for Windows (with Git.exe available in PATH)

## Compiling Guidance

 - Clone this repository and the submodules.
 ```bash
git clone https://github.com/sewer56lol/Reloaded-Mod-Loader
cd Reloaded-Mod-Loader
git submodule update --init --recursive
```
- Open 'Reloaded-Launcher.sln' in Visual Studio.
- Right-click solution in Solution Explorer and restore NuGet packages.
- Now you're done (* ^ ω ^), go play around!

## Troubleshooting

### Git.exe is missing: 
Make sure to install Git for Windows or a compatible Git solution for windows, and ensure that Git is in your environment variables' PATH. 

![Git for Windows setup](https://i.imgur.com/RdcabVT.png)

### Cannot see local variables when debugging Reloaded Mods:

Try enabling `Native Code Debugging` under Project => Properties => Debug for your current project as well as `Use Managed Compatibility Mode` under Tools => Options => Debugging.

This applies regardless of whether the debugger has been manually attached at runtime by the user or via `Debugger.Launch()`.

# Project Structure
Reloaded composes of the various following sub-projects, which are all components of Project Reloaded:

## Reloaded-Wrapper
A small, miniature wrapper program to force the load of the Any CPU compiled Reloaded-Loader.exe in x64 mode, used when the target process is not x86 (Loader runs in x86 mode by default).

## Reloaded-Mod-Template
A template for the development of code based mods for Project Reloaded.
Provides useful out of the box functions such as logging and printing to Reloaded console. 

## Reloaded-Loader
The actual Mod Loader and just that. Basically a glorified commandline DLL injector that reads your configurations, parses them and loads the appropriate mods. Includes a tacked-on locally hosted server for providing print to console functionality.

## Reloaded-Launcher
A front-end to the Reloaded Mod loader allowing for enabling/disabling of user mods, adding new games to Reloaded, configuring the custom input stack in realtime and updating Reloaded (soon).

## Reloaded-GUI
Forked from the launcher, the basic built from the ground up theming engine used in the Reloaded Launcher.
Includes a few built from the ground custom Windows Forms controls and fade-in/out animations.

Reloaded-GUI allows for the creation of applications for mods which use the theme and match the style set by the user in the Reloaded Launcher. The theming process is automatic but initialized and initially set-up by the user.

Do note that the WinForm controls available are not fully exhaustive, they have been created just as the launcher needed them.

## Reloaded-Assembler
A thin wrapper around FASM.NET (Zenlulz) wrapper of flat assembler (FASM) which grants FASM.NET networking capabilities via the mod loader networking code.

Reloaded-Assembler is a process that runs in the background and accepts TCP based assembly requests from C# applications, returning the user supplied ASM mnemonics as X86/X64/16-bit assembly bytes. 

For mod loader mods, it is packed with all of its dependencies during the build process and embedded in libReloaded, automatically connected to (if already running) or launched as soon as the user makes an assembly request.

## libReloaded
The main bulk of the project. The library whose code is shared between all of the individual Reloaded projects and is directly used in mod development. Constitutes around 80% of the overall code base.
