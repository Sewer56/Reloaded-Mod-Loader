
<div align="center">
	<h1>Project Structure</h1>
	<img src="https://i.imgur.com/TbCUc05.png" align="center" />
	<br/> <br/>
</div>

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

## libReloaded-Networking
Used to be my own from the ground up TCP based networking implementation, although has since been replaced with an external, dedicated library to the task. The last commit to have the networking code is 
https://github.com/sewer56lol/Reloaded-Mod-Loader/commit/181c782e23ced0b793138a00a2f710b32834a856 on 29/04/2018.